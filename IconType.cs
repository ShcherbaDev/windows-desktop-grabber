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

	internal static class IconType
	{
		public static IconTypes GetIconType(string path)
		{
			if (File.Exists(path))
			{
				return IconTypes.File;
			}
			else if (File.Exists(path + ".lnk"))
			{
				return IconTypes.Shortcut;
			}
			else if (Directory.Exists(path))
			{
				return IconTypes.Directory;
			}
			return IconTypes.System;
		}
	}
}
