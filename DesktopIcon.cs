using System.Xml.Serialization;

namespace windows_desktop_grabber
{
	public struct DesktopIcon
	{
		[XmlText]
		public string Name;

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

		public DesktopIcon(string name, int x, int y, int type, string size)
		{
			this.Name = name;
			this.X = x;
			this.Y = y;
			this.Type = type;
			this.Size = size;
		}
	}
}
