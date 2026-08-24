using System;
using System.Xml.Serialization;

namespace HeathenEngineering
{
	[Serializable]
	public class StringFieldRecord
	{
		[XmlAttribute("xml:space")]
		public string SpacePreserve = "preserve";

		[XmlAttribute]
		public uint Id { get; set; }

		[XmlText]
		public string Value { get; set; }
	}
}
