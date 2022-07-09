using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsAPI
{
	internal static class User32
	{
		public enum DesktopWindow
		{
			ProgMan,
			SHELLDLL_DefViewParent,
			SHELLDLL_DefView,
			SysListView32
		}

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool DestroyIcon(IntPtr hIcon);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		[DllImport("user32.dll")]
		public static extern IntPtr GetShellWindow();

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		public static IntPtr GetDesktopWindow(DesktopWindow desktopWindow)
		{
			IntPtr _ProgMan = GetShellWindow();
			IntPtr _SHELLDLL_DefViewParent = _ProgMan;
			IntPtr _SHELLDLL_DefView = FindWindowEx(_ProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
			IntPtr _SysListView32 = FindWindowEx(_SHELLDLL_DefView, IntPtr.Zero, "SysListView32", "FolderView");

			if (_SHELLDLL_DefView == IntPtr.Zero)
			{
				EnumWindows((hwnd, lParam) =>
				{
					var sb = new StringBuilder(256);
					var className = GetClassName(hwnd, sb, sb.Capacity);

					if (sb.ToString() == "WorkerW")
					{
						IntPtr child = FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
						if (child != IntPtr.Zero)
						{
							_SHELLDLL_DefViewParent = hwnd;
							_SHELLDLL_DefView = child;
							_SysListView32 = FindWindowEx(child, IntPtr.Zero, "SysListView32", "FolderView");
							return false;
						}
					}
					return true;
				}, IntPtr.Zero);
			}

			switch (desktopWindow)
            {
				case DesktopWindow.ProgMan:
					return _ProgMan;
				case DesktopWindow.SHELLDLL_DefViewParent:
					return _SHELLDLL_DefViewParent;
				case DesktopWindow.SHELLDLL_DefView:
					return _SHELLDLL_DefView;
				case DesktopWindow.SysListView32:
					return _SysListView32;
				default:
					return IntPtr.Zero;
            }
		}
	}
}
