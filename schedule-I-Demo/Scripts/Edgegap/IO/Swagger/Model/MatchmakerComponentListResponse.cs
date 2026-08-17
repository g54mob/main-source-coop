using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class MatchmakerComponentListResponse
	{
		[DataMember(Name = "count", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "count")]
		public int? Count { get; set; }

		[DataMember(Name = "data", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "data")]
		public MatchmakerComponentResponse Data { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class MatchmakerComponentListResponse {\n");
			stringBuilder.Append("  Count: ").Append(Count).Append("\n");
			stringBuilder.Append("  Data: ").Append(Data).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
