using System;

namespace windows_desktop_grabber
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			// Get desktop data
			Desktop desktop = new Desktop();
			FullDesktopIcon[] icons = desktop.GetIconsPositions();

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
				Icons = icons
			};

			string xml = XmlManager.GenerateXml(desktopData);
			Console.WriteLine(xml);
        }
    }
}
