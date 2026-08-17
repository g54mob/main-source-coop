using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class ApiModelContainerlogs
	{
		[DataMember(Name = "logs", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "logs")]
		public string Logs { get; set; }

		[DataMember(Name = "encoding", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "encoding")]
		public string Encoding { get; set; }

		[DataMember(Name = "crash_logs", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "crash_logs")]
		public string CrashLogs { get; set; }

		[DataMember(Name = "crash_data", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "crash_data")]
		public ApiModelContainercrashdata CrashData { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ApiModelContainerlogs {\n");
			stringBuilder.Append("  Logs: ").Append(Logs).Append("\n");
			stringBuilder.Append("  Encoding: ").Append(Encoding).Append("\n");
			stringBuilder.Append("  CrashLogs: ").Append(CrashLogs).Append("\n");
			stringBuilder.Append("  CrashData: ").Append(CrashData).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
