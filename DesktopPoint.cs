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

		public FullDesktopIcon(string name, int x, int y)
		{
			this.Name = name;
			this.X = x;
			this.Y = y;
		}
	}
}
