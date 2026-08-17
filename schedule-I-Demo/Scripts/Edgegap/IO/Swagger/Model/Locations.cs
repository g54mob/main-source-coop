using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Locations
	{
		[DataMember(Name = "locations", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "locations")]
		public List<Location> _Locations { get; set; }

		[DataMember(Name = "messages", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "messages")]
		public List<string> Messages { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Locations {\n");
			stringBuilder.Append("  _Locations: ").Append(_Locations).Append("\n");
			stringBuilder.Append("  Messages: ").Append(Messages).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
