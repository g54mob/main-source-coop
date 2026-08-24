using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace HeathenEngineering
{
	[Serializable]
	public class StringFieldDataModel
	{
		[XmlAttribute]
		public string Name { get; set; }

		[XmlAttribute]
		public string Code { get; set; }

		public List<StringFieldRecord> Records { get; set; }
	}
}
