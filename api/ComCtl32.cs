using System;
using System.Runtime.InteropServices;

namespace WindowsAPI
{
	internal static class ComCtl32
	{
		public const uint LVM_FIRST = 0x1000;
		public const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
		public const uint LVM_GETITEMW = LVM_FIRST + 75; // Same as LVM_GETITEM
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
	}
}
