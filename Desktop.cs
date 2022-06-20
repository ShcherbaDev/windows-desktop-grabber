using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using Microsoft.Win32;
using WindowsAPI;

namespace windows_desktop_grabber
{
	internal class Desktop
	{
		private readonly IntPtr _desktopHandle;

		public Desktop()
		{
			_desktopHandle = User32.GetDesktopWindow(User32.DesktopWindow.SysListView32);
		}

		private int GetIconsNumber()
		{
			return (int)User32.SendMessage(_desktopHandle, ComCtl32.LVM_GETITEMCOUNT, 0, 0);
		}

		private bool AreIconsHidden()
		{
			try
			{
				return Convert.ToBoolean(
					(int)Registry.CurrentUser
						.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced")
						.GetValue("HideIcons")
				);
			}
			catch (Exception)
			{
				return false;
			}
		}

		// Wallpapers
		private object GetDesktopValue(string propertyName)
		{
			return Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop").GetValue(propertyName);
		}

		public string GetWallpaperPath()
		{
			return GetDesktopValue("WallPaper") as string;
		}

		public string GetWallpaperTile()
		{
			return GetDesktopValue("TileWallpaper") as string;
		}

		public string GetWallpaperStyle()
		{
			return GetDesktopValue("WallpaperStyle") as string;
		}

		// Runs when wallpaper image is empty and returns an RGB color
		public string GetWallpaperBackgroundColor()
		{
			return Registry.CurrentUser.OpenSubKey(@"Control Panel\Colors").GetValue("Background") as string;
		}

		// Icons
		public DesktopIcon[] GetIconsPositions()
		{
			var icons = new LinkedList<DesktopIcon>();

			// Return nothing if desktop icons are hidden
			if (AreIconsHidden())
			{
				#if DEBUG
				Console.WriteLine("Desktop icons are hidden");
				#endif

				return icons.ToArray();
			}

			string desktopLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			uint desktopProcessId;
			User32.GetWindowThreadProcessId(_desktopHandle, out desktopProcessId);

			IntPtr vProcess = IntPtr.Zero;
			IntPtr vPointer = IntPtr.Zero;

			try
			{
				vProcess = Kernel32.OpenProcess(
					Kernel32.ProcessAccess.VmOperation | Kernel32.ProcessAccess.VmRead | Kernel32.ProcessAccess.VmWrite,
					false, desktopProcessId
				);

				vPointer = Kernel32.VirtualAllocEx(
					vProcess,
					IntPtr.Zero,
					sizeof(uint),
					Kernel32.AllocationType.Reserve | Kernel32.AllocationType.Commit,
					Kernel32.MemoryProtection.ReadWrite
				);

				int iconCount = GetIconsNumber();

				for (int i = 0; i < iconCount; i++)
				{
					byte[] vBuffer = new byte[1024];
					ComCtl32.LVITEM[] vItem = new ComCtl32.LVITEM[1];
					vItem[0].mask = ComCtl32.LVIF_TEXT;
					vItem[0].iItem = i;
					vItem[0].iSubItem = 0; // 2 for file type
					vItem[0].cchTextMax = vBuffer.Length;
					vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(ComCtl32.LVITEM)));

					uint vNumberOfBytesRead = 0;

					Kernel32.WriteProcessMemory(
						vProcess, vPointer,
						Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
						Marshal.SizeOf(typeof(ComCtl32.LVITEM)),
						ref vNumberOfBytesRead
					);

					User32.SendMessage(_desktopHandle, ComCtl32.LVM_GETITEMW, i, vPointer.ToInt32());
					
					// Get icon text
					Kernel32.ReadProcessMemory(
						vProcess,
						(IntPtr)((int)vPointer + Marshal.SizeOf(typeof(ComCtl32.LVITEM))),
						Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
						vBuffer.Length,
						ref vNumberOfBytesRead
					);

					string vText = Encoding.Unicode.GetString(vBuffer, 0, (int)vNumberOfBytesRead);
					string name = vText.Substring(0, vText.IndexOf('\0'));

					// Get icon width and height
					// TODO: try to use this: https://docs.microsoft.com/en-us/windows/win32/api/shobjidl_core/nn-shobjidl_core-ifolderview2?redirectedfrom=MSDN
					User32.SendMessage(_desktopHandle, ComCtl32.LVM_GETITEMRECT, i, vPointer.ToInt32());
					ComCtl32.RECT[] vRect = new ComCtl32.RECT[1];
					Kernel32.ReadProcessMemory(
						vProcess, vPointer,
						Marshal.UnsafeAddrOfPinnedArrayElement(vRect, 0),
						Marshal.SizeOf(typeof(ComCtl32.RECT)), ref vNumberOfBytesRead
					);

					int width = vRect[0].Right - vRect[0].Left;
					int height = vRect[0].Bottom - vRect[0].Top;
					string size = width + "," + height;

					// Get icon position
					User32.SendMessage(_desktopHandle, ComCtl32.LVM_GETITEMPOSITION, i, vPointer.ToInt32());
					Point[] vPoint = new Point[1];
					Kernel32.ReadProcessMemory(
						vProcess, vPointer,
						Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0),
						Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead
					);

					string fullPath = IconUtilities.GetValidIconPath(name);
					IconTypes iconType = IconUtilities.GetIconType(fullPath);

					icons.AddLast(new DesktopIcon(
						name,
						fullPath,
						vPoint[0].X, vPoint[0].Y,
						(int)iconType, 
						size
					));
				}

				return icons.ToArray();
			}
			finally
			{
				Kernel32.VirtualFreeEx(vProcess, vPointer, 0, Kernel32.AllocationType.Release);
				Kernel32.CloseHandle(vProcess);
			}
		}
	}
}
