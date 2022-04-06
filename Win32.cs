using System;
using System.Runtime.InteropServices;
using System.Text;

namespace windows_desktop_grabber
{
	internal static class Win32
	{
		public const uint LVM_FIRST = 0x1000;
		public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
		public const uint LVM_GETITEMW = LVM_FIRST + 75;
		public const uint LVM_GETITEMRECT = LVM_FIRST + 14;
		public const uint LVM_GETITEMPOSITION = LVM_FIRST + 16;
		public const int LVIF_TEXT = 0x0001;

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left, Top, Right, Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LVITEM
		{
			public int mask;
			public int iItem;
			public int iSubItem;
			public int state;
			public int stateMask;
			public IntPtr pszText;//string
			public int cchTextMax;
			public int iImage;
			public IntPtr lParam;
			public int iIndent;
			public int iGroupId;
			public int cColumns;
			public IntPtr puColumns;
		}

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

		public enum DesktopWindow
		{
			ProgMan,
			SHELLDLL_DefViewParent,
			SHELLDLL_DefView,
			SysListView32
		}

		[DllImport("user32.dll")]
		public static extern IntPtr GetShellWindow();

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

		public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

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

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(ProcessAccess dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwProcessId);

		[Flags]
		public enum ProcessAccess
		{
			/// <summary>
			/// Required to create a thread.
			/// </summary>
			CreateThread = 0x0002,

			/// <summary>
			///
			/// </summary>
			SetSessionId = 0x0004,

			/// <summary>
			/// Required to perform an operation on the address space of a process
			/// </summary>
			VmOperation = 0x0008,

			/// <summary>
			/// Required to read memory in a process using ReadProcessMemory.
			/// </summary>
			VmRead = 0x0010,

			/// <summary>
			/// Required to write to memory in a process using WriteProcessMemory.
			/// </summary>
			VmWrite = 0x0020,

			/// <summary>
			/// Required to duplicate a handle using DuplicateHandle.
			/// </summary>
			DupHandle = 0x0040,

			/// <summary>
			/// Required to create a process.
			/// </summary>
			CreateProcess = 0x0080,

			/// <summary>
			/// Required to set memory limits using SetProcessWorkingSetSize.
			/// </summary>
			SetQuota = 0x0100,

			/// <summary>
			/// Required to set certain information about a process, such as its priority class (see SetPriorityClass).
			/// </summary>
			SetInformation = 0x0200,

			/// <summary>
			/// Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken).
			/// </summary>
			QueryInformation = 0x0400,

			/// <summary>
			/// Required to suspend or resume a process.
			/// </summary>
			SuspendResume = 0x0800,

			/// <summary>
			/// Required to retrieve certain information about a process (see GetExitCodeProcess, GetPriorityClass, IsProcessInJob, QueryFullProcessImageName).
			/// A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted PROCESS_QUERY_LIMITED_INFORMATION.
			/// </summary>
			QueryLimitedInformation = 0x1000,

			/// <summary>
			/// Required to wait for the process to terminate using the wait functions.
			/// </summary>
			Synchronize = 0x100000,

			/// <summary>
			/// Required to delete the object.
			/// </summary>
			Delete = 0x00010000,

			/// <summary>
			/// Required to read information in the security descriptor for the object, not including the information in the SACL.
			/// To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
			/// </summary>
			ReadControl = 0x00020000,

			/// <summary>
			/// Required to modify the DACL in the security descriptor for the object.
			/// </summary>
			WriteDac = 0x00040000,

			/// <summary>
			/// Required to change the owner in the security descriptor for the object.
			/// </summary>
			WriteOwner = 0x00080000,

			StandardRightsRequired = 0x000F0000,

			/// <summary>
			/// All possible access rights for a process object.
			/// </summary>
			AllAccess = StandardRightsRequired | Synchronize | 0xFFFF
		}

		[DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
		public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

		[Flags]
		public enum AllocationType
		{
			Commit = 0x1000,
			Reserve = 0x2000,
			Decommit = 0x4000,
			Release = 0x8000,
			Reset = 0x80000,
			Physical = 0x400000,
			TopDown = 0x100000,
			WriteWatch = 0x200000,
			LargePages = 0x20000000
		}

		[Flags]
		public enum MemoryProtection
		{
			Execute = 0x10,
			ExecuteRead = 0x20,
			ExecuteReadWrite = 0x40,
			ExecuteWriteCopy = 0x80,
			NoAccess = 0x01,
			ReadOnly = 0x02,
			ReadWrite = 0x04,
			WriteCopy = 0x08,
			GuardModifierflag = 0x100,
			NoCacheModifierflag = 0x200,
			WriteCombineModifierflag = 0x400
		}

		[Flags]
		public enum FreeType
		{
			Decommit = 0x4000,
			Release = 0x8000,
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint lpNumberOfBytesWritten);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] IntPtr buffer, int size, ref uint lpNumberOfBytesRead);
	}
}

