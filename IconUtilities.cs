using System;
using System.IO;

namespace windows_desktop_grabber
{
	public enum IconTypes
	{
		File,
		Directory,
		Shortcut,
		VirtualFolder
	}

	internal static class IconUtilities
	{
		public static string GetFileDesktopPath(string iconName)
		{
			return Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
				iconName
			);
		}

		// Check both local and public desktop folders for checking out an icon
		public static string GetValidIconPath(string iconName, bool checkCommonDesktop = true)
		{
			string fileDesktopPath = GetFileDesktopPath(iconName);

			if (File.Exists(fileDesktopPath) || Directory.Exists(fileDesktopPath))
			{
				return fileDesktopPath;
			}
			else if (File.Exists(fileDesktopPath + ".lnk"))
			{
				return fileDesktopPath + ".lnk";
			}
			if (checkCommonDesktop)
			{
				return GetValidIconPath(
					Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory),
						iconName
					),
					false
				);
			}

			// Fallback
			return fileDesktopPath;
		}

		public static IconTypes GetIconType(string path)
		{
			if (File.Exists(path))
			{
				if (path.Split(".")[^1] == "lnk")
				{
					return IconTypes.Shortcut;
				}

				return IconTypes.File;
			}
			else if (Directory.Exists(path))
			{
				return IconTypes.Directory;
			}

			return IconTypes.VirtualFolder; // "This PC", "Recycle Bin" etc.
		}
	}
}
