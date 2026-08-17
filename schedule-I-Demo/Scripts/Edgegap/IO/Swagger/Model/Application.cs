using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Application
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[DataMember(Name = "is_active", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "is_active")]
		public bool? IsActive { get; set; }

		[DataMember(Name = "image", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "image")]
		public string Image { get; set; }

		[DataMember(Name = "create_time", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "create_time")]
		public string CreateTime { get; set; }

		[DataMember(Name = "last_updated", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "last_updated")]
		public string LastUpdated { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Application {\n");
			stringBuilder.Append("  Name: ").Append(Name).Append("\n");
			stringBuilder.Append("  IsActive: ").Append(IsActive).Append("\n");
			stringBuilder.Append("  Image: ").Append(Image).Append("\n");
			stringBuilder.Append("  CreateTime: ").Append(CreateTime).Append("\n");
			stringBuilder.Append("  LastUpdated: ").Append(LastUpdated).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
