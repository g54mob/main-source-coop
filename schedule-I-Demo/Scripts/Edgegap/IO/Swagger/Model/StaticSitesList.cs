using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class StaticSitesList
	{
		[DataMember(Name = "sites", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "sites")]
		public List<StaticSites> Sites { get; set; }

		[DataMember(Name = "total_count", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "total_count")]
		public int? TotalCount { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class StaticSitesList {\n");
			stringBuilder.Append("  Sites: ").Append(Sites).Append("\n");
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
