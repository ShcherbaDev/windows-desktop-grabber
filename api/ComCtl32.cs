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
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LVITEM
		{
			public int mask;
			public int iItem;
			public int iSubItem;
			public int state;
			public int stateMask;
			public IntPtr pszText; //string
			public int cchTextMax;
			public int iImage;
			public IntPtr lParam;
			public int iIndent;
			public int iGroupId;
			public int cColumns;
			public IntPtr puColumns;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct IMAGELISTDRAWPARAMS
		{
			public int cbSize;
			public IntPtr himl;
			public int i;
			public IntPtr hdcDst;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public int xBitmap; // x offset from the upperleft of bitmap
			public int yBitmap; // y offset from the upperleft of bitmap
			public int rgbBk;
			public int rgbFg;
			public int fStyle;
			public int dwRop;
			public int fState;
			public int Frame;
			public int crEffect;
		}

		[Flags]
		public enum ILD_FLAGS : int
		{
			ILD_NORMAL = 0x00000000,
			ILD_TRANSPARENT = 0x00000001,
			ILD_BLEND25 = 0x00000002,
			ILD_FOCUS = 0x00000002,
			ILD_BLEND50 = 0x00000004,
			ILD_SELECTED = 0x00000004,
			ILD_BLEND = 0x00000004,
			ILD_MASK = 0x00000010,
			ILD_IMAGE = 0x00000020,
			ILD_ROP = 0x00000040,
			ILD_OVERLAYMASK = 0x00000F00,
			ILD_PRESERVEALPHA = 0x00001000,
			ILD_SCALE = 0x00002000,
			ILD_DPISCALE = 0x00004000,
			ILD_ASYNC = 0x00008000
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct IMAGEINFO
		{
			public IntPtr hbmImage;
			public IntPtr hbmMask;
			public int Unused1;
			public int Unused2;
			public RECT rcImage;
		}

		[ComImportAttribute()]
		[GuidAttribute("46EB5926-582E-4017-9FDF-E8998DAA0950")]
		[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
		public interface IImageList
		{
			[PreserveSig]
			int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);

			[PreserveSig]
			int ReplaceIcon(int i, IntPtr hicon, ref int pi);

			[PreserveSig]
			int SetOverlayImage(int iImage, int iOverlay);

			[PreserveSig]
			int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);

			[PreserveSig]
			int AddMasked(IntPtr hbmImage, int crMask, ref int pi);

			[PreserveSig]
			int Draw(ref IMAGELISTDRAWPARAMS pimldp);

			[PreserveSig]
			int Remove(int i);

			[PreserveSig]
			int GetIcon(int i, int flags, ref IntPtr picon);

			[PreserveSig]
			int GetImageInfo(int i, ref IMAGEINFO pImageInfo);

			[PreserveSig]
			int Copy(int iDst, IImageList punkSrc, int iSrc, int uFlags);

			[PreserveSig]
			int Merge(int i1, IImageList punk2, int i2, int dx, int dy, ref Guid riid, ref IntPtr ppv);

			[PreserveSig]
			int Clone(ref Guid riid, ref IntPtr ppv);

			[PreserveSig]
			int GetImageRect(int i, ref RECT prc);

			[PreserveSig]
			int GetIconSize(ref int cx, ref int cy);

			[PreserveSig]
			int SetIconSize(int cx, int cy);

			[PreserveSig]
			int GetImageCount(ref int pi);

			[PreserveSig]
			int SetImageCount(int uNewCount);

			[PreserveSig]
			int SetBkColor(int clrBk, ref int pclr);

			[PreserveSig]
			int GetBkColor(ref int pclr);

			[PreserveSig]
			int BeginDrag(int iTrack, int dxHotspot, int dyHotspot);

			[PreserveSig]
			int EndDrag();

			[PreserveSig]
			int DragEnter(IntPtr hwndLock, int x, int y);

			[PreserveSig]
			int DragLeave(IntPtr hwndLock);

			[PreserveSig]
			int DragMove(int x, int y);

			[PreserveSig]
			int SetDragCursorImage(ref IImageList punk, int iDrag, int dxHotspot, int dyHotspot);

			[PreserveSig]
			int DragShowNolock(int fShow);

			[PreserveSig]
			int GetDragImage(ref POINT ppt, ref POINT pptHotspot, ref Guid riid, ref IntPtr ppv);

			[PreserveSig]
			int GetItemFlags(int i, ref int dwFlags);

			[PreserveSig]
			int GetOverlayImage(int iOverlay, ref int piIndex);
		}
	}
}
