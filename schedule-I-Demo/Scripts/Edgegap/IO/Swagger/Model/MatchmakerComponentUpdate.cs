using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class MatchmakerComponentUpdate
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[DataMember(Name = "repository", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "repository")]
		public string Repository { get; set; }

		[DataMember(Name = "image", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "image")]
		public string Image { get; set; }

		[DataMember(Name = "tag", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "tag")]
		public string Tag { get; set; }

		[DataMember(Name = "credentials", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "credentials")]
		public object Credentials { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class MatchmakerComponentUpdate {\n");
			stringBuilder.Append("  Name: ").Append(Name).Append("\n");
			stringBuilder.Append("  Repository: ").Append(Repository).Append("\n");
			stringBuilder.Append("  Image: ").Append(Image).Append("\n");
			stringBuilder.Append("  Tag: ").Append(Tag).Append("\n");
			stringBuilder.Append("  Credentials: ").Append(Credentials).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
