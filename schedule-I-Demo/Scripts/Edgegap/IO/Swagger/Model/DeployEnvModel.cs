using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class DeployEnvModel
	{
		[DataMember(Name = "key", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "key")]
		public string Key { get; set; }

		[DataMember(Name = "value", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; }

		[DataMember(Name = "is_hidden", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "is_hidden")]
		public bool? IsHidden { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class DeployEnvModel {\n");
			stringBuilder.Append("  Key: ").Append(Key).Append("\n");
			stringBuilder.Append("  Value: ").Append(Value).Append("\n");
			stringBuilder.Append("  IsHidden: ").Append(IsHidden).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
