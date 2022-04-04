using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace windows_desktop_grabber
{
	[XmlRoot("root")]
	public class XmlContent
	{
		[XmlElement("platform")]
		public string Platform;

		[XmlArray("icons")]
		[XmlArrayItem("icon")]
		public FullDesktopIcon[] Icons;
	}

	internal static class XmlManager
	{
		public static string GenerateXml(XmlContent data)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlContent));
			MemoryStream memoryStream = new MemoryStream();

			xmlSerializer.Serialize(memoryStream, data);
			memoryStream.Position = 0;

			StreamReader streamReader = new StreamReader(memoryStream);
			return streamReader.ReadToEnd();
		}
	}
}
