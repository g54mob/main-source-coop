using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppVersionWhitelistEntrySuccess
	{
		[DataMember(Name = "success", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "success")]
		public bool? Success { get; set; }

		[DataMember(Name = "whitelist_entry", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "whitelist_entry")]
		public AppVersionWhitelistEntry WhitelistEntry { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppVersionWhitelistEntrySuccess {\n");
			stringBuilder.Append("  Success: ").Append(Success).Append("\n");
			stringBuilder.Append("  WhitelistEntry: ").Append(WhitelistEntry).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
