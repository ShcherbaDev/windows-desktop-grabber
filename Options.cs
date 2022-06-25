using CommandLine;

namespace windows_desktop_grabber
{
	class Options
	{
		[Option("ignore-virtual-folders", Default = false, HelpText = "Ignore virtual folders like \"My PC\", \"Recycle Bin\" etc.")]
		public bool IgnoreVirtualFolders { get; set; }

		[Option("exclude-icon-images", Default = false, HelpText = "Should not add actual image of the icon")]
		public bool ExcludeIconImages { get; set; }

		[Option("exclude-wallpaper", Default = false, HelpText = "Should not add wallpaper data to the output")]
		public bool ExcludeWallpaper { get; set; }

		[Option("icon-images-path", Default = "./icons", HelpText = "Relative path to the directory with icon's images")]
		public string IconImagesPath { get; set; }
	}
}
