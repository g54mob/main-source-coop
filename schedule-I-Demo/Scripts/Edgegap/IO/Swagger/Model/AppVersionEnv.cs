using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class AppVersionEnv
	{
		[DataMember(Name = "key", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "key")]
		public string Key { get; set; }

		[DataMember(Name = "value", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "value")]
		public string Value { get; set; }

		[DataMember(Name = "is_secret", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "is_secret")]
		public bool? IsSecret { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class AppVersionEnv {\n");
			stringBuilder.Append("  Key: ").Append(Key).Append("\n");
			stringBuilder.Append("  Value: ").Append(Value).Append("\n");
			stringBuilder.Append("  IsSecret: ").Append(IsSecret).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
