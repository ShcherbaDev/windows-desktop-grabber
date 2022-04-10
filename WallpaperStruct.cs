using System.Xml.Serialization;

namespace windows_desktop_grabber
{
	public struct WallpaperStruct
	{
		[XmlAttribute("path")]
		public string Path;

		[XmlAttribute("tile")]
		public string Tile;

		[XmlAttribute("style")]
		public string Style;

		[XmlAttribute("rgb")]
		public string RGB;

		/*
			Reference: https://docs.microsoft.com/en-us/windows/win32/controls/themesfileformat-overview?redirectedfrom=MSDN#control-paneldesktop-section

			- path
			- rgb
			- tile:
				- 0: not tiled
				- 1: tiled
			- style:
				- 0: centered if tile=0 or tiled if tile=1
				- 2: stretched to fill the screen
				- 6: resisted to fit the screen while maintaining the aspect ratio
				- 10: resized and cropped to fill the screen while maintaining the aspect ratio
		*/
		public WallpaperStruct(string path, string tile, string style) : this()
		{
			this.Path = path;
			this.Tile = tile;
			this.Style = style;
		}

		public WallpaperStruct(string path, string rgb, string tile, string style) : this()
		{
			this.Path = path;
			this.RGB = rgb;
			this.Tile = tile;
			this.Style = style;
		}

		public WallpaperStruct(string rgb) : this()
		{
			this.RGB = rgb;
		}
	}
}
