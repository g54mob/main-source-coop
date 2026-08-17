using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class SessionGet
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

		[DataMember(Name = "app_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "app_id")]
		public int? AppId { get; set; }

		[DataMember(Name = "create_time", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "create_time")]
		public string CreateTime { get; set; }

		[DataMember(Name = "elapsed", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "elapsed")]
		public int? Elapsed { get; set; }

		[DataMember(Name = "error", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "error")]
		public string Error { get; set; }

		[DataMember(Name = "session_users", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "session_users")]
		public List<SessionUser> SessionUsers { get; set; }

		[DataMember(Name = "session_ips", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "session_ips")]
		public List<SessionUser> SessionIps { get; set; }

		[DataMember(Name = "deployment", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "deployment")]
		public Deployment Deployment { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class SessionGet {\n");
			stringBuilder.Append("  SessionId: ").Append(SessionId).Append("\n");
			stringBuilder.Append("  CustomId: ").Append(CustomId).Append("\n");
			stringBuilder.Append("  Status: ").Append(Status).Append("\n");
			stringBuilder.Append("  Ready: ").Append(Ready).Append("\n");
			stringBuilder.Append("  Linked: ").Append(Linked).Append("\n");
			stringBuilder.Append("  Kind: ").Append(Kind).Append("\n");
			stringBuilder.Append("  UserCount: ").Append(UserCount).Append("\n");
			stringBuilder.Append("  AppId: ").Append(AppId).Append("\n");
			stringBuilder.Append("  CreateTime: ").Append(CreateTime).Append("\n");
			stringBuilder.Append("  Elapsed: ").Append(Elapsed).Append("\n");
			stringBuilder.Append("  Error: ").Append(Error).Append("\n");
			stringBuilder.Append("  SessionUsers: ").Append(SessionUsers).Append("\n");
			stringBuilder.Append("  SessionIps: ").Append(SessionIps).Append("\n");
			stringBuilder.Append("  Deployment: ").Append(Deployment).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
