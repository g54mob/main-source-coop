using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppVersions
	{
		[DataMember(Name = "versions", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "versions")]
		public List<AppVersion> Versions { get; set; }

		[DataMember(Name = "total_count", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "total_count")]
		public int? TotalCount { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppVersions {\n");
			stringBuilder.Append("  Versions: ").Append(Versions).Append("\n");
			stringBuilder.Append("  TotalCount: ").Append(TotalCount).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
