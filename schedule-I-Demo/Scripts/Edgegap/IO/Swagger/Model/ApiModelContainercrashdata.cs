using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class ApiModelContainercrashdata
	{
		[DataMember(Name = "exit_code", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "exit_code")]
		public int? ExitCode { get; set; }

		[DataMember(Name = "message", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

		[DataMember(Name = "restart_count", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "restart_count")]
		public int? RestartCount { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ApiModelContainercrashdata {\n");
			stringBuilder.Append("  ExitCode: ").Append(ExitCode).Append("\n");
			stringBuilder.Append("  Message: ").Append(Message).Append("\n");
			stringBuilder.Append("  RestartCount: ").Append(RestartCount).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
