using System;
using System.IO;

namespace windows_desktop_grabber
{
	public enum IconTypes
	{
		File,
		Directory,
		Shortcut,
		System
	}

	internal static class IconUtilities
	{
		// Check both local and public desktop folders for checking out an icon
		public static string GetValidIconPath(string iconName, bool checkCommonDesktop = true)
		{
			string foo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), iconName);

			if (File.Exists(foo) || Directory.Exists(foo))
			{
				return foo;
			}
			else if (File.Exists(foo + ".lnk"))
			{
				return foo + ".lnk";
			}
			if (checkCommonDesktop)
			{
				return GetValidIconPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), iconName), false);
			}

			// Fallback
			return foo;
		}

		public static IconTypes GetIconType(string path, bool checkCommonDesktop = true)
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

			return IconTypes.System;
		}
	}
}
