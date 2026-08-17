using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppVersionWhitelistEntry
	{
		[DataMember(Name = "id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[DataMember(Name = "cidr", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "cidr")]
		public string Cidr { get; set; }

		[DataMember(Name = "label", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "label")]
		public string Label { get; set; }

		[DataMember(Name = "is_active", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "is_active")]
		public bool? IsActive { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppVersionWhitelistEntry {\n");
			stringBuilder.Append("  Id: ").Append(Id).Append("\n");
			stringBuilder.Append("  Cidr: ").Append(Cidr).Append("\n");
			stringBuilder.Append("  Label: ").Append(Label).Append("\n");
			stringBuilder.Append("  IsActive: ").Append(IsActive).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
