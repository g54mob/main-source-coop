using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class BulkSessionPost
	{
		[DataMember(Name = "sessions", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "sessions")]
		public List<SessionRequest> Sessions { get; set; }

		[DataMember(Name = "errors", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "errors")]
		public List<string> Errors { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class BulkSessionPost {\n");
			stringBuilder.Append("  Sessions: ").Append(Sessions).Append("\n");
			stringBuilder.Append("  Errors: ").Append(Errors).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
