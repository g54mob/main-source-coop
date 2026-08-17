using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class MetricsResponse
	{
		[DataMember(Name = "total", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "total")]
		public TotalMetricsModel Total { get; set; }

		[DataMember(Name = "cpu", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "cpu")]
		public MetricsModel Cpu { get; set; }

		[DataMember(Name = "mem", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "mem")]
		public MetricsModel Mem { get; set; }

		[DataMember(Name = "network", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "network")]
		public NetworkMetricsModel Network { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class MetricsResponse {\n");
			stringBuilder.Append("  Total: ").Append(Total).Append("\n");
			stringBuilder.Append("  Cpu: ").Append(Cpu).Append("\n");
			stringBuilder.Append("  Mem: ").Append(Mem).Append("\n");
			stringBuilder.Append("  Network: ").Append(Network).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
