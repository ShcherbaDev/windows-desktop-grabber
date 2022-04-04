﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;

namespace windows_desktop_grabber
{
	internal class Desktop
	{
		private readonly IntPtr _desktopHandle;

		public Desktop()
		{
			_desktopHandle = Win32.GetDesktopWindow(Win32.DesktopWindow.SysListView32);
		}

		private int GetIconsNumber()
		{
			return (int)Win32.SendMessage(_desktopHandle, Win32.LVM_GETITEMCOUNT, 0, 0);
		}

		public FullDesktopIcon[] GetIconsPositions()
		{
			uint desktopProcessId;
			Win32.GetWindowThreadProcessId(_desktopHandle, out desktopProcessId);

			IntPtr vProcess = IntPtr.Zero;
			IntPtr vPointer = IntPtr.Zero;

			var icons = new LinkedList<FullDesktopIcon>();

			try
			{
				vProcess = Win32.OpenProcess(
					Win32.ProcessAccess.VmOperation | Win32.ProcessAccess.VmRead | Win32.ProcessAccess.VmWrite,
					false, desktopProcessId
				);

				vPointer = Win32.VirtualAllocEx(
					vProcess,
					IntPtr.Zero,
					sizeof(uint),
					Win32.AllocationType.Reserve | Win32.AllocationType.Commit,
					Win32.MemoryProtection.ReadWrite
				);

				int iconCount = GetIconsNumber();

				for (int i = 0; i < iconCount; i++)
				{
					byte[] vBuffer = new byte[256];
					Win32.LVITEM[] vItem = new Win32.LVITEM[1];
					vItem[0].mask = Win32.LVIF_TEXT;
					vItem[0].iItem = i;
					vItem[0].iSubItem = 0;
					vItem[0].cchTextMax = vBuffer.Length;
					vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(Win32.LVITEM)));

					uint vNumberOfBytesRead = 0;

					Win32.WriteProcessMemory(
						vProcess, vPointer,
						Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
						Marshal.SizeOf(typeof(Win32.LVITEM)), ref vNumberOfBytesRead
					);
					Win32.SendMessage(_desktopHandle, Win32.LVM_GETITEMW, i, vPointer.ToInt32());
					Win32.ReadProcessMemory(
						vProcess,
						(IntPtr)((int)vPointer + Marshal.SizeOf(typeof(Win32.LVITEM))),
						Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
						vBuffer.Length, ref vNumberOfBytesRead
					);

					// Get icon text
					string vText = Encoding.Unicode.GetString(vBuffer, 0, (int)vNumberOfBytesRead);
					string name = vText.Substring(0, vText.IndexOf('\0'));

					// Get icon position
					Win32.SendMessage(_desktopHandle, Win32.LVM_GETITEMPOSITION, i, vPointer.ToInt32());
					Point[] vPoint = new Point[1];
					Win32.ReadProcessMemory(
						vProcess, vPointer,
						Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0),
						Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead
					);

					icons.AddLast(new FullDesktopIcon(name, vPoint[0].X, vPoint[0].Y));
				}

				return icons.ToArray();
			}
			finally
			{
				Win32.CloseHandle(vProcess);
			}
		}
	}
}
