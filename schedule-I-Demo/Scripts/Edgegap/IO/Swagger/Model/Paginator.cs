using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Paginator
	{
		[DataMember(Name = "num_pages", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "num_pages")]
		public int? NumPages { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Paginator {\n");
			stringBuilder.Append("  NumPages: ").Append(NumPages).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
