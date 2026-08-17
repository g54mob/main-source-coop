using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class SessionContext
	{
		[DataMember(Name = "session_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "session_id")]
		public string SessionId { get; set; }

		[DataMember(Name = "custom_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "custom_id")]
		public string CustomId { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; }

		[DataMember(Name = "ready", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "ready")]
		public bool? Ready { get; set; }

		[DataMember(Name = "linked", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "linked")]
		public bool? Linked { get; set; }

		[DataMember(Name = "kind", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "kind")]
		public string Kind { get; set; }

		[DataMember(Name = "user_count", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "user_count")]
		public int? UserCount { get; set; }

		[DataMember(Name = "deployment_request_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "deployment_request_id")]
		public string DeploymentRequestId { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class SessionContext {\n");
			stringBuilder.Append("  SessionId: ").Append(SessionId).Append("\n");
			stringBuilder.Append("  CustomId: ").Append(CustomId).Append("\n");
			stringBuilder.Append("  Status: ").Append(Status).Append("\n");
			stringBuilder.Append("  Ready: ").Append(Ready).Append("\n");
			stringBuilder.Append("  Linked: ").Append(Linked).Append("\n");
			stringBuilder.Append("  Kind: ").Append(Kind).Append("\n");
			stringBuilder.Append("  UserCount: ").Append(UserCount).Append("\n");
			stringBuilder.Append("  DeploymentRequestId: ").Append(DeploymentRequestId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
