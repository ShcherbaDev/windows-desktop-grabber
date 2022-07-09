using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using CommandLine;

using static WindowsAPI.Shell32;

namespace windows_desktop_grabber
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(Run)
				.WithNotParsed(HandleParseErrors);
        }

		static void Run(Options options)
		{
			const int ICON_SIZE = 256;

			// Get desktop data
			Desktop desktop = new Desktop();
			List<DesktopIcon> icons = desktop.GetIcons(!options.ExcludeIconImages);

			if (options.IgnoreVirtualFolders)
			{
				icons = icons.Where((icon) => icon.Type != (int)IconTypes.VirtualFolder).ToList();
			}

			if (!options.ExcludeIconImages)
			{
				// Create folder for icon images
				string iconImagesDirectoryPath = Path.GetFullPath(
					Path.Combine(AppDomain.CurrentDomain.BaseDirectory, options.IconImagesPath)
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

				if (icons.Any(icon => icon.Type == (int)IconTypes.Shortcut))
				{
					Bitmap shortcutArrowBitmap = ThumbnailProvider.GetShortcutArrowOverlayBitmap(SHIL.SHIL_JUMBO);
					ThumbnailProvider.SaveBitmap(shortcutArrowBitmap, Path.Combine(iconImagesDirectoryPath, "shortcut_overlay"));
					ThumbnailProvider.RemoveBitmap(shortcutArrowBitmap);
				}

				// Save icon actual images
				foreach (var icon in icons)
				{
					// TODO: add support for virtual folders
					if ((IconTypes)icon.Type == IconTypes.VirtualFolder)
					{
						#if DEBUG
						Console.WriteLine("Virtual folder {0} is not supported yet", icon.Name);
						#endif

						continue;
					}

					string iconImagePath = Path.Combine(iconImagesDirectoryPath, icon.ImageName);

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
			}

			// Get wallpaper data
			WallpaperStruct? wallpaper = null;
			if (!options.ExcludeWallpaper)
			{
				string wallpaperPath = desktop.GetWallpaperPath();
				string backgroundColor = desktop.GetWallpaperBackgroundColor();

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
			}

			// Create XML output
			XmlContent desktopData = new XmlContent()
			{
				Platform = "windows",
				Wallpaper = wallpaper,
				IconImagesPath = !options.ExcludeIconImages ? options.IconImagesPath : null,
				Icons = icons
			};

			string xml = XmlManager.GenerateXml(desktopData);
			Console.WriteLine(xml);
		}

		static void HandleParseErrors(IEnumerable<Error> errors)
		{
			// Don't count --help and --version as error
			if (errors.IsHelp() || errors.IsVersion())
			{
				return;
			}

			foreach (var error in errors)
			{
				Console.WriteLine(error);
			}
		}
    }
}
