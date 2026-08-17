using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class TotalMetricsModel
	{
		[DataMember(Name = "receive_total", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "receive_total")]
		public MetricsModel ReceiveTotal { get; set; }

		[DataMember(Name = "transmit_total", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "transmit_total")]
		public MetricsModel TransmitTotal { get; set; }

		[DataMember(Name = "disk_read_total", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "disk_read_total")]
		public MetricsModel DiskReadTotal { get; set; }

		[DataMember(Name = "disk_write_total", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "disk_write_total")]
		public MetricsModel DiskWriteTotal { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class TotalMetricsModel {\n");
			stringBuilder.Append("  ReceiveTotal: ").Append(ReceiveTotal).Append("\n");
			stringBuilder.Append("  TransmitTotal: ").Append(TransmitTotal).Append("\n");
			stringBuilder.Append("  DiskReadTotal: ").Append(DiskReadTotal).Append("\n");
			stringBuilder.Append("  DiskWriteTotal: ").Append(DiskWriteTotal).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
