using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Sessions
	{
		[DataMember(Name = "data", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "data")]
		public List<SessionContext> Data { get; set; }

		[DataMember(Name = "total_count", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "total_count")]
		public int? TotalCount { get; set; }

		[DataMember(Name = "pagination", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "pagination")]
		public Pagination Pagination { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Sessions {\n");
			stringBuilder.Append("  Data: ").Append(Data).Append("\n");
			stringBuilder.Append("  TotalCount: ").Append(TotalCount).Append("\n");
			stringBuilder.Append("  Pagination: ").Append(Pagination).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
