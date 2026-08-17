using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class PortMapping
	{
		[DataMember(Name = "external", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "external")]
		public int? External { get; set; }

		[DataMember(Name = "internal", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "internal")]
		public int? Internal { get; set; }

		[DataMember(Name = "protocol", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "protocol")]
		public string Protocol { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[DataMember(Name = "tls_upgrade", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "tls_upgrade")]
		public bool? TlsUpgrade { get; set; }

		[DataMember(Name = "link", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "link")]
		public string Link { get; set; }

		[DataMember(Name = "proxy", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "proxy")]
		public int? Proxy { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class PortMapping {\n");
			stringBuilder.Append("  External: ").Append(External).Append("\n");
			stringBuilder.Append("  Internal: ").Append(Internal).Append("\n");
			stringBuilder.Append("  Protocol: ").Append(Protocol).Append("\n");
			stringBuilder.Append("  Name: ").Append(Name).Append("\n");
			stringBuilder.Append("  TlsUpgrade: ").Append(TlsUpgrade).Append("\n");
			stringBuilder.Append("  Link: ").Append(Link).Append("\n");
			stringBuilder.Append("  Proxy: ").Append(Proxy).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
