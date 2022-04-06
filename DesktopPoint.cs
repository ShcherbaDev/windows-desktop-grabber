using System.Xml.Serialization;

namespace windows_desktop_grabber
{
	public struct FullDesktopIcon
	{
		[XmlText]
		public string Name;

		[XmlAttribute("x")]
		public int X;

		[XmlAttribute("y")]
		public int Y;

		// Width and height in pixels separated by comma
		[XmlAttribute("size")]
		public string Size;

		public FullDesktopIcon(string name, int x, int y, string size)
		{
			this.Name = name;
			this.X = x;
			this.Y = y;
			this.Size = size;
		}
	}
}
