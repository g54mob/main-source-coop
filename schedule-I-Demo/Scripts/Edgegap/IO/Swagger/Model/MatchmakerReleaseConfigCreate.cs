using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class MatchmakerReleaseConfigCreate
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[DataMember(Name = "configuration", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "configuration")]
		public string Configuration { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class MatchmakerReleaseConfigCreate {\n");
			stringBuilder.Append("  Name: ").Append(Name).Append("\n");
			stringBuilder.Append("  Configuration: ").Append(Configuration).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
