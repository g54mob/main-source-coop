using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class Location
	{
		[DataMember(Name = "city", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "city")]
		public string City { get; set; }

		[DataMember(Name = "continent", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "continent")]
		public string Continent { get; set; }

		[DataMember(Name = "country", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "country")]
		public string Country { get; set; }

		[DataMember(Name = "timezone", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "timezone")]
		public string Timezone { get; set; }

		[DataMember(Name = "administrative_division", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "administrative_division")]
		public string AdministrativeDivision { get; set; }

		[DataMember(Name = "latitude", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "latitude")]
		public decimal? Latitude { get; set; }

		[DataMember(Name = "longitude", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "longitude")]
		public decimal? Longitude { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "type")]
		public string Type { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class Location {\n");
			stringBuilder.Append("  City: ").Append(City).Append("\n");
			stringBuilder.Append("  Continent: ").Append(Continent).Append("\n");
			stringBuilder.Append("  Country: ").Append(Country).Append("\n");
			stringBuilder.Append("  Timezone: ").Append(Timezone).Append("\n");
			stringBuilder.Append("  AdministrativeDivision: ").Append(AdministrativeDivision).Append("\n");
			stringBuilder.Append("  Latitude: ").Append(Latitude).Append("\n");
			stringBuilder.Append("  Longitude: ").Append(Longitude).Append("\n");
			stringBuilder.Append("  Type: ").Append(Type).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
