using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using static WindowsAPI.Gdi32;
using static WindowsAPI.Shell32;

namespace windows_desktop_grabber
{
	/*
		Reference:
		https://stackoverflow.com/a/21752100
	*/
	internal static class ThumbnailProvider
	{
		private const string IShellItem2Guid = "7E9FB0D3-919F-4307-AB2E-9B1860310C93";

		public static Bitmap GetThumbnail(string fileName, int width, int height, ThumbnailOptions options)
		{
			IntPtr hBitmap = GetHBitmap(Path.GetFullPath(fileName), width, height, options);

			try
			{
				// Return a System.Drawing.Bitmap from the hBitmap
				return GetBitmapFromHBitmap(hBitmap);
			}
			finally
			{
				// Delete HBitmap to avoid memory leaks
				DeleteObject(hBitmap);
			}
		}

		public static void SaveBitmap(Bitmap bitmap, string destinationPath)
		{
			bitmap.Save(destinationPath + ".png", ImageFormat.Png);
		}

		public static void RemoveBitmap(Bitmap bitmap)
		{
			bitmap.Dispose();
		}

		private static Bitmap GetBitmapFromHBitmap(IntPtr nativeHBitmap)
		{
			Bitmap bmp = Bitmap.FromHbitmap(nativeHBitmap);

			if (Bitmap.GetPixelFormatSize(bmp.PixelFormat) < 32)
			{
				return bmp;
			}

			return CreateAlphaBitmap(bmp, PixelFormat.Format32bppArgb);
		}

		private static Bitmap CreateAlphaBitmap(Bitmap srcBitmap, PixelFormat targetPixelFormat)
		{
			Bitmap result = new Bitmap(srcBitmap.Width, srcBitmap.Height, targetPixelFormat);
			Rectangle bmpBounds = new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height);
			BitmapData srcData = srcBitmap.LockBits(bmpBounds, ImageLockMode.ReadOnly, srcBitmap.PixelFormat);

			bool isAlplaBitmap = false;

			try
			{
				for (int y = 0; y <= srcData.Height - 1; y++)
				{
					for (int x = 0; x <= srcData.Width - 1; x++)
					{
						Color pixelColor = Color.FromArgb(
							Marshal.ReadInt32(srcData.Scan0, (srcData.Stride * y) + (4 * x)));

						if (pixelColor.A > 0 & pixelColor.A < 255)
						{
							isAlplaBitmap = true;
						}

						result.SetPixel(x, y, pixelColor);
					}
				}
			}
			finally
			{
				srcBitmap.UnlockBits(srcData);
			}

			if (isAlplaBitmap)
			{
				return result;
			}
			return srcBitmap;
		}

		private static IntPtr GetHBitmap(string fileName, int width, int height, ThumbnailOptions options)
		{
			IShellItem nativeShellItem;
			Guid shellItem2Guid = new Guid(IShellItem2Guid);
			int retCode = SHCreateItemFromParsingName(
				fileName,
				IntPtr.Zero,
				ref shellItem2Guid,
				out nativeShellItem
			);

			if (retCode != 0)
			{
				throw Marshal.GetExceptionForHR(retCode);
			}

			NativeSize nativeSize = new NativeSize();
			nativeSize.Width = width;
			nativeSize.Height = height;

			IntPtr hBitmap;
			HResult hr = ((IShellItemImageFactory)nativeShellItem).GetImage(
				nativeSize, options, out hBitmap
			);

			Marshal.ReleaseComObject(nativeShellItem);

			if (hr == HResult.Ok)
			{
				return hBitmap;
			}

			throw Marshal.GetExceptionForHR((int)hr);
		}
	}
}
