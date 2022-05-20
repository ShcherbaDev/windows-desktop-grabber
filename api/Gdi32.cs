using System;
using System.Runtime.InteropServices;

namespace WindowsAPI
{
	internal static class Gdi32
	{
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DeleteObject(IntPtr hObject);
	}
}
