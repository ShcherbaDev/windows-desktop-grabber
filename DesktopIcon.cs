using System.Xml.Serialization;

namespace windows_desktop_grabber
{
	public struct DesktopIcon
	{
		[XmlText]
		public string Name;

		[XmlIgnore]
		public string FullPath;

		[XmlAttribute("x")]
		public int X;

		[XmlAttribute("y")]
		public int Y;

		/// <summary>
		/// Value from <see cref="IconTypes"/>
		/// </summary>
		[XmlAttribute("type")]
		public int Type;

		/// <summary>
		/// Width and height in pixels separated by comma
		/// </summary>
		[XmlAttribute("size")]
		public string Size;

		/// <summary>
		/// Icon's actual image filename without extension
		/// </summary>
		[XmlAttribute("image-filename")]
		#nullable enable
		public string? ImageName;

		public DesktopIcon(string name, string? fullPath, string imageName, int x, int y, int type, string size)
		{
			this.Name = name;
			this.FullPath = fullPath;
			this.ImageName = imageName;
			this.X = x;
			this.Y = y;
			this.Type = type;
			this.Size = size;
		}
		#nullable disable
	}
}
