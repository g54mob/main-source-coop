using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Delete
	{
		[DataMember(Name = "message", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

		[DataMember(Name = "deployment_summary", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "deployment_summary")]
		public Status DeploymentSummary { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Delete {\n");
			stringBuilder.Append("  Message: ").Append(Message).Append("\n");
			stringBuilder.Append("  DeploymentSummary: ").Append(DeploymentSummary).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
