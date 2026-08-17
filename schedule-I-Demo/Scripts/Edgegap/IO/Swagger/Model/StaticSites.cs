using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class StaticSites
	{
		[DataMember(Name = "url", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "url")]
		public string Url { get; set; }

		[DataMember(Name = "public_ip", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "public_ip")]
		public string PublicIp { get; set; }

		[DataMember(Name = "port", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "port")]
		public decimal? Port { get; set; }

		[DataMember(Name = "latitude", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "latitude")]
		public decimal? Latitude { get; set; }

		[DataMember(Name = "longitude", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "longitude")]
		public decimal? Longitude { get; set; }

		[DataMember(Name = "city", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "city")]
		public string City { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class StaticSites {\n");
			stringBuilder.Append("  Url: ").Append(Url).Append("\n");
			stringBuilder.Append("  PublicIp: ").Append(PublicIp).Append("\n");
			stringBuilder.Append("  Port: ").Append(Port).Append("\n");
			stringBuilder.Append("  Latitude: ").Append(Latitude).Append("\n");
			stringBuilder.Append("  Longitude: ").Append(Longitude).Append("\n");
			stringBuilder.Append("  City: ").Append(City).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
