using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Monitor
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "version")]
		public string Version { get; set; }

		[DataMember(Name = "host", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "host")]
		public string Host { get; set; }

		[DataMember(Name = "host_url", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "host_url")]
		public string HostUrl { get; set; }

		[DataMember(Name = "spec_url", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "spec_url")]
		public string SpecUrl { get; set; }

		[DataMember(Name = "messages", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "messages")]
		public List<string> Messages { get; set; }

		[DataMember(Name = "errors", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "errors")]
		public List<string> Errors { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Monitor {\n");
			stringBuilder.Append("  Name: ").Append(Name).Append("\n");
			stringBuilder.Append("  Version: ").Append(Version).Append("\n");
			stringBuilder.Append("  Host: ").Append(Host).Append("\n");
			stringBuilder.Append("  HostUrl: ").Append(HostUrl).Append("\n");
			stringBuilder.Append("  SpecUrl: ").Append(SpecUrl).Append("\n");
			stringBuilder.Append("  Messages: ").Append(Messages).Append("\n");
			stringBuilder.Append("  Errors: ").Append(Errors).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
