using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppVersionWhitelistResponse
	{
		[DataMember(Name = "whitelist_entries", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "whitelist_entries")]
		public List<AppVersionWhitelistEntry> WhitelistEntries { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppVersionWhitelistResponse {\n");
			stringBuilder.Append("  WhitelistEntries: ").Append(WhitelistEntries).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
