using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppVersionProbe
	{
		[DataMember(Name = "optimal_ping", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "optimal_ping")]
		public int? OptimalPing { get; set; }

		[DataMember(Name = "rejected_ping", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "rejected_ping")]
		public int? RejectedPing { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppVersionProbe {\n");
			stringBuilder.Append("  OptimalPing: ").Append(OptimalPing).Append("\n");
			stringBuilder.Append("  RejectedPing: ").Append(RejectedPing).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
