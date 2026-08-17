using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Deployment
	{
		[DataMember(Name = "request_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "request_id")]
		public string RequestId { get; set; }

		[DataMember(Name = "public_ip", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "public_ip")]
		public string PublicIp { get; set; }

		[DataMember(Name = "status", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; }

		[DataMember(Name = "ready", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "ready")]
		public bool? Ready { get; set; }

		[DataMember(Name = "whitelisting_active", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "whitelisting_active")]
		public bool? WhitelistingActive { get; set; }

		[DataMember(Name = "fqdn", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "fqdn")]
		public string Fqdn { get; set; }

		[DataMember(Name = "ports", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "ports")]
		public Dictionary<string, PortMapping> Ports { get; set; }

		[DataMember(Name = "location", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "location")]
		public DeploymentLocation Location { get; set; }

		[DataMember(Name = "tags", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "tags")]
		public List<string> Tags { get; set; }

		[DataMember(Name = "sockets", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "sockets")]
		public int? Sockets { get; set; }

		[DataMember(Name = "sockets_usage", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "sockets_usage")]
		public int? SocketsUsage { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Deployment {\n");
			stringBuilder.Append("  RequestId: ").Append(RequestId).Append("\n");
			stringBuilder.Append("  PublicIp: ").Append(PublicIp).Append("\n");
			stringBuilder.Append("  Status: ").Append(Status).Append("\n");
			stringBuilder.Append("  Ready: ").Append(Ready).Append("\n");
			stringBuilder.Append("  WhitelistingActive: ").Append(WhitelistingActive).Append("\n");
			stringBuilder.Append("  Fqdn: ").Append(Fqdn).Append("\n");
			stringBuilder.Append("  Ports: ").Append(Ports).Append("\n");
			stringBuilder.Append("  Location: ").Append(Location).Append("\n");
			stringBuilder.Append("  Tags: ").Append(Tags).Append("\n");
			stringBuilder.Append("  Sockets: ").Append(Sockets).Append("\n");
			stringBuilder.Append("  SocketsUsage: ").Append(SocketsUsage).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
