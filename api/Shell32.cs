using System;
using System.Runtime.InteropServices;

namespace WindowsAPI
{
	internal static class Shell32
	{
		[Flags]
		public enum ThumbnailOptions
		{
			None = 0x00,
			BiggerSizeOk = 0x01,
			InMemoryOnly = 0x02,
			IconOnly = 0x04,
			ThumbnailOnly = 0x08,
			InCacheOnly = 0x10
		}

		public enum SIGDN : uint
		{
			SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
			SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
			SIGDN_FILESYSPATH = 0x80058000,
			SIGDN_NORMALDISPLAY = 0,
			SIGDN_PARENTRELATIVE = 0x80080001,
			SIGDN_PARENTRELATIVEEDITING = 0x80031001,
			SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
			SIGDN_PARENTRELATIVEFORUI = 0x80094001,
			SIGDN_PARENTRELATIVEPARSING = 0x80018001,
			SIGDN_URL = 0x80068000
		}

		public enum HResult
		{
			Ok = 0x0000,
			False = 0x0001,
			InvalidArguments = unchecked((int)0x80070057),
			OutOfMemory = unchecked((int)0x8007000E),
			NoInterface = unchecked((int)0x80004002),
			Fail = unchecked((int)0x80004005),
			ElementNotFound = unchecked((int)0x80070490),
			TypeElementNotFound = unchecked((int)0x8002802B),
			NoObject = unchecked((int)0x800401E5),
			Win32ErrorCanceled = 1223,
			Canceled = unchecked((int)0x800704C7),
			ResourceInUse = unchecked((int)0x800700AA),
			AccessDenied = unchecked((int)0x80030005)
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NativeSize
		{
			private int width;
			private int height;

			public int Width { set { width = value; } }
			public int Height { set { height = value; } }
		};

		[StructLayout(LayoutKind.Sequential), Serializable]
		public struct SIZE : IEquatable<SIZE>
		{
			public int cx;
			public int cy;

			public SIZE(int width, int height)
			{
				cx = width;
				cy = height;
			}

			public int Width { get => cx; set => cx = value; }
			public int Height { get => cy; set => cy = value; }
			public bool IsEmpty => cx == 0 && cy == 0;
			public bool Equals(SIZE other) => cx == other.cx || cy == other.cy;
		}

		[ComImport()]
		[Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		internal interface IShellItemImageFactory
		{
			[PreserveSig]
			HResult GetImage(
				[In, MarshalAs(UnmanagedType.Struct)] Shell32.NativeSize size,
				[In] ThumbnailOptions flags,
				[Out] out IntPtr phbm
			);
		}

		[ComImport()]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
		public interface IShellItem {
			void BindToHandler(IntPtr pbc,
				[MarshalAs(UnmanagedType.LPStruct)]Guid bhid,
				[MarshalAs(UnmanagedType.LPStruct)]Guid riid,
				out IntPtr ppv
			);
			void GetParent(out IShellItem ppsi);
			void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
			void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
			void Compare(IShellItem psi, uint hint, out int piOrder);
		};

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHCreateItemFromParsingName(
			[MarshalAs(UnmanagedType.LPWStr)] string path,
			IntPtr pbc,
			ref Guid riid,
			[MarshalAs(UnmanagedType.Interface)] out IShellItem shellItem
		);
	}
}
