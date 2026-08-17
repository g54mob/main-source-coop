using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class SessionRequest
	{
		[DataMember(Name = "session_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "session_id")]
		public string SessionId { get; set; }

		[DataMember(Name = "custom_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "custom_id")]
		public string CustomId { get; set; }

		[DataMember(Name = "app", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "app")]
		public string App { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "version")]
		public string Version { get; set; }

		[DataMember(Name = "deployment_request_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "deployment_request_id")]
		public string DeploymentRequestId { get; set; }

		[DataMember(Name = "selectors", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "selectors")]
		public List<SelectorModel> Selectors { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class SessionRequest {\n");
			stringBuilder.Append("  SessionId: ").Append(SessionId).Append("\n");
			stringBuilder.Append("  CustomId: ").Append(CustomId).Append("\n");
			stringBuilder.Append("  App: ").Append(App).Append("\n");
			stringBuilder.Append("  Version: ").Append(Version).Append("\n");
			stringBuilder.Append("  DeploymentRequestId: ").Append(DeploymentRequestId).Append("\n");
			stringBuilder.Append("  Selectors: ").Append(Selectors).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
