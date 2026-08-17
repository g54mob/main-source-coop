using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class SelectorModel
	{
		[DataMember(Name = "tag", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "tag")]
		public string Tag { get; set; }

		[DataMember(Name = "tag_only", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "tag_only")]
		public bool? TagOnly { get; set; }

		[DataMember(Name = "env", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "env")]
		public object Env { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class SelectorModel {\n");
			stringBuilder.Append("  Tag: ").Append(Tag).Append("\n");
			stringBuilder.Append("  TagOnly: ").Append(TagOnly).Append("\n");
			stringBuilder.Append("  Env: ").Append(Env).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
