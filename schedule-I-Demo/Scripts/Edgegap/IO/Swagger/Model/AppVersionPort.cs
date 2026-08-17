using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppVersionPort
	{
		[DataMember(Name = "port", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "port")]
		public int? Port { get; set; }

		[DataMember(Name = "protocol", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "protocol")]
		public string Protocol { get; set; }

		[DataMember(Name = "to_check", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "to_check")]
		public bool? ToCheck { get; set; }

		[DataMember(Name = "tls_upgrade", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "tls_upgrade")]
		public bool? TlsUpgrade { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppVersionPort {\n");
			stringBuilder.Append("  Port: ").Append(Port).Append("\n");
			stringBuilder.Append("  Protocol: ").Append(Protocol).Append("\n");
			stringBuilder.Append("  ToCheck: ").Append(ToCheck).Append("\n");
			stringBuilder.Append("  TlsUpgrade: ").Append(TlsUpgrade).Append("\n");
			stringBuilder.Append("  Name: ").Append(Name).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
