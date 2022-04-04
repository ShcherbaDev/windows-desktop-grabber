using System;

namespace windows_desktop_grabber
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			// Get desktop data
			Desktop desktop = new Desktop();
			FullDesktopIcon[] icons = desktop.GetIconsPositions();

			// Create XML output
			XmlContent desktopData = new XmlContent()
			{
				Platform = "windows",
				Icons = icons
			};

			string xml = XmlManager.GenerateXml(desktopData);
			Console.WriteLine(xml);
        }
    }
}
