using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Status
	{
		[DataMember(Name = "request_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "request_id")]
		public string RequestId { get; set; }

		[DataMember(Name = "fqdn", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "fqdn")]
		public string Fqdn { get; set; }

		[DataMember(Name = "app_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "app_name")]
		public string AppName { get; set; }

		[DataMember(Name = "app_version", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "app_version")]
		public string AppVersion { get; set; }

		[DataMember(Name = "current_status", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "current_status")]
		public string CurrentStatus { get; set; }

		[DataMember(Name = "running", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "running")]
		public bool? Running { get; set; }

		[DataMember(Name = "whitelisting_active", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "whitelisting_active")]
		public bool? WhitelistingActive { get; set; }

		[DataMember(Name = "start_time", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "start_time")]
		public string StartTime { get; set; }

		[DataMember(Name = "removal_time", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "removal_time")]
		public string RemovalTime { get; set; }

		[DataMember(Name = "elapsed_time", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "elapsed_time")]
		public int? ElapsedTime { get; set; }

		[DataMember(Name = "last_status", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "last_status")]
		public string LastStatus { get; set; }

		[DataMember(Name = "error", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "error")]
		public bool? Error { get; set; }

		[DataMember(Name = "error_detail", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "error_detail")]
		public string ErrorDetail { get; set; }

		[DataMember(Name = "ports", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "ports")]
		public Dictionary<string, PortMapping> Ports { get; set; }

		[DataMember(Name = "public_ip", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "public_ip")]
		public string PublicIp { get; set; }

		[DataMember(Name = "sessions", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "sessions")]
		public List<DeploymentSessionContext> Sessions { get; set; }

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
			stringBuilder.Append("class Status {\n");
			stringBuilder.Append("  RequestId: ").Append(RequestId).Append("\n");
			stringBuilder.Append("  Fqdn: ").Append(Fqdn).Append("\n");
			stringBuilder.Append("  AppName: ").Append(AppName).Append("\n");
			stringBuilder.Append("  AppVersion: ").Append(AppVersion).Append("\n");
			stringBuilder.Append("  CurrentStatus: ").Append(CurrentStatus).Append("\n");
			stringBuilder.Append("  Running: ").Append(Running).Append("\n");
			stringBuilder.Append("  WhitelistingActive: ").Append(WhitelistingActive).Append("\n");
			stringBuilder.Append("  StartTime: ").Append(StartTime).Append("\n");
			stringBuilder.Append("  RemovalTime: ").Append(RemovalTime).Append("\n");
			stringBuilder.Append("  ElapsedTime: ").Append(ElapsedTime).Append("\n");
			stringBuilder.Append("  LastStatus: ").Append(LastStatus).Append("\n");
			stringBuilder.Append("  Error: ").Append(Error).Append("\n");
			stringBuilder.Append("  ErrorDetail: ").Append(ErrorDetail).Append("\n");
			stringBuilder.Append("  Ports: ").Append(Ports).Append("\n");
			stringBuilder.Append("  PublicIp: ").Append(PublicIp).Append("\n");
			stringBuilder.Append("  Sessions: ").Append(Sessions).Append("\n");
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
