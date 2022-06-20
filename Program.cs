using System;
using System.IO;
using System.Drawing;

using static WindowsAPI.Shell32;

namespace windows_desktop_grabber
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			const int ICON_SIZE = 256;
			const string ICON_IMAGES_PATH = "./icons";

			// Create folder for icon images
			string iconImagesDirectoryPath = Path.GetFullPath(
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ICON_IMAGES_PATH)
			);
			if (!Directory.Exists(iconImagesDirectoryPath))
			{
				Directory.CreateDirectory(iconImagesDirectoryPath);
			}
			else
			{
				foreach (FileInfo file in new DirectoryInfo(iconImagesDirectoryPath).GetFiles())
				{
					file.IsReadOnly = false;
					file.Delete();
				}
			}

			// Get desktop data
			Desktop desktop = new Desktop();
			DesktopIcon[] icons = desktop.GetIconsPositions();

			// Save icon actual images
			foreach (var icon in icons)
			{
				// TODO: add support for system icons
				if ((IconTypes)icon.Type == IconTypes.System)
				{
					#if DEBUG
					Console.WriteLine("File {0} is not supported yet", icon.Name);
					#endif

					continue;
				}

				string iconName = icon.FullPath.Split("\\")[^1];
				string iconImagePath = Path.Combine(iconImagesDirectoryPath, iconName);

				try
				{
					Bitmap iconBitmap = ThumbnailProvider.GetThumbnail(
						icon.FullPath,
						ICON_SIZE, ICON_SIZE,
						ThumbnailOptions.None
					);
					ThumbnailProvider.SaveBitmap(iconBitmap, iconImagePath);
					ThumbnailProvider.RemoveBitmap(iconBitmap);
				}
				catch (FileNotFoundException) {
					#if DEBUG
					Console.WriteLine("File {0} was not found", icon.Name);
					#endif
				}
			}

			// Get wallpaper data
			string wallpaperPath = desktop.GetWallpaperPath();
			string backgroundColor = desktop.GetWallpaperBackgroundColor();
			WallpaperStruct wallpaper;

			// If wallpaper is the image
			if (wallpaperPath != "")
			{
				string wallpaperIsTiled = desktop.GetWallpaperTile();
				string wallpaperStyle = desktop.GetWallpaperStyle();
				wallpaper = new WallpaperStruct(wallpaperPath, backgroundColor, wallpaperIsTiled, wallpaperStyle);
			}
			else
			{
				wallpaper = new WallpaperStruct(backgroundColor);
			}

			// Create XML output
			XmlContent desktopData = new XmlContent()
			{
				Platform = "windows",
				Wallpaper = wallpaper,
				IconImagesPath = ICON_IMAGES_PATH,
				Icons = icons
			};

			string xml = XmlManager.GenerateXml(desktopData);
			Console.WriteLine(xml);
        }
    }
}
