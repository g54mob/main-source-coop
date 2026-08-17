using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Applications
	{
		[DataMember(Name = "applications", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "applications")]
		public List<Application> _Applications { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Applications {\n");
			stringBuilder.Append("  _Applications: ").Append(_Applications).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
