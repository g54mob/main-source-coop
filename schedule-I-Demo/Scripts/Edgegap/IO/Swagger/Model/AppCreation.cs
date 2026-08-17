using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppCreation
	{
		[DataMember(Name = "success", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "success")]
		public bool? Success { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "version")]
		public AppVersion Version { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppCreation {\n");
			stringBuilder.Append("  Success: ").Append(Success).Append("\n");
			stringBuilder.Append("  Version: ").Append(Version).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
