using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppVersionCreateSessionConfig
	{
		[DataMember(Name = "kind", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "kind")]
		public string Kind { get; set; }

		[DataMember(Name = "sockets", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "sockets")]
		public int? Sockets { get; set; }

		[DataMember(Name = "autodeploy", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "autodeploy")]
		public bool? Autodeploy { get; set; }

		[DataMember(Name = "empty_ttl", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "empty_ttl")]
		public int? EmptyTtl { get; set; }

		[DataMember(Name = "session_max_duration", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "session_max_duration")]
		public int? SessionMaxDuration { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppVersionCreateSessionConfig {\n");
			stringBuilder.Append("  Kind: ").Append(Kind).Append("\n");
			stringBuilder.Append("  Sockets: ").Append(Sockets).Append("\n");
			stringBuilder.Append("  Autodeploy: ").Append(Autodeploy).Append("\n");
			stringBuilder.Append("  EmptyTtl: ").Append(EmptyTtl).Append("\n");
			stringBuilder.Append("  SessionMaxDuration: ").Append(SessionMaxDuration).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
