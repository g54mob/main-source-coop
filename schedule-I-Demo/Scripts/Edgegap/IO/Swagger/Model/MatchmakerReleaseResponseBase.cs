using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class MatchmakerReleaseResponseBase : BaseModel
	{
		[DataMember(Name = "app_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "app_name")]
		public string AppName { get; set; }

		[DataMember(Name = "version_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "version_name")]
		public string VersionName { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "version")]
		public string Version { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class MatchmakerReleaseResponseBase {\n");
			stringBuilder.Append("  AppName: ").Append(AppName).Append("\n");
			stringBuilder.Append("  VersionName: ").Append(VersionName).Append("\n");
			stringBuilder.Append("  Version: ").Append(Version).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public new string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
