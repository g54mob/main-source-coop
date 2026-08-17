using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class ContainerLogStorageModel
	{
		[DataMember(Name = "enabled", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "enabled")]
		public bool? Enabled { get; set; }

		[DataMember(Name = "endpoint_storage", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "endpoint_storage")]
		public string EndpointStorage { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ContainerLogStorageModel {\n");
			stringBuilder.Append("  Enabled: ").Append(Enabled).Append("\n");
			stringBuilder.Append("  EndpointStorage: ").Append(EndpointStorage).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
