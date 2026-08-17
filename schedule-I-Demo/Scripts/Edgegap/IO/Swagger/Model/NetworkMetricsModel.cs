using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class NetworkMetricsModel
	{
		[DataMember(Name = "receive", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "receive")]
		public MetricsModel Receive { get; set; }

		[DataMember(Name = "transmit", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "transmit")]
		public MetricsModel Transmit { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class NetworkMetricsModel {\n");
			stringBuilder.Append("  Receive: ").Append(Receive).Append("\n");
			stringBuilder.Append("  Transmit: ").Append(Transmit).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
