using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class BaseModel
	{
		[DataMember(Name = "created_at", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "created_at")]
		public string CreatedAt { get; set; }

		[DataMember(Name = "updated_at", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "updated_at")]
		public string UpdatedAt { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class BaseModel {\n");
			stringBuilder.Append("  CreatedAt: ").Append(CreatedAt).Append("\n");
			stringBuilder.Append("  UpdatedAt: ").Append(UpdatedAt).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
