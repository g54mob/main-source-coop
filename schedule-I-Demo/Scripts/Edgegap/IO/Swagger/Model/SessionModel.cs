using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class SessionModel
	{
		[DataMember(Name = "app_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "app_name")]
		public string AppName { get; set; }

		[DataMember(Name = "version_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "version_name")]
		public string VersionName { get; set; }

		[DataMember(Name = "ip_list", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "ip_list")]
		public List<string> IpList { get; set; }

		[DataMember(Name = "geo_ip_list", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "geo_ip_list")]
		public List<GeoIpListModel> GeoIpList { get; set; }

		[DataMember(Name = "deployment_request_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "deployment_request_id")]
		public string DeploymentRequestId { get; set; }

		[DataMember(Name = "location", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "location")]
		public LocationModel Location { get; set; }

		[DataMember(Name = "city", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "city")]
		public string City { get; set; }

		[DataMember(Name = "country", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "country")]
		public string Country { get; set; }

		[DataMember(Name = "continent", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "continent")]
		public string Continent { get; set; }

		[DataMember(Name = "administrative_division", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "administrative_division")]
		public string AdministrativeDivision { get; set; }

		[DataMember(Name = "region", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "region")]
		public string Region { get; set; }

		[DataMember(Name = "selectors", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "selectors")]
		public List<SelectorModel> Selectors { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class SessionModel {\n");
			stringBuilder.Append("  AppName: ").Append(AppName).Append("\n");
			stringBuilder.Append("  VersionName: ").Append(VersionName).Append("\n");
			stringBuilder.Append("  IpList: ").Append(IpList).Append("\n");
			stringBuilder.Append("  GeoIpList: ").Append(GeoIpList).Append("\n");
			stringBuilder.Append("  DeploymentRequestId: ").Append(DeploymentRequestId).Append("\n");
			stringBuilder.Append("  Location: ").Append(Location).Append("\n");
			stringBuilder.Append("  City: ").Append(City).Append("\n");
			stringBuilder.Append("  Country: ").Append(Country).Append("\n");
			stringBuilder.Append("  Continent: ").Append(Continent).Append("\n");
			stringBuilder.Append("  AdministrativeDivision: ").Append(AdministrativeDivision).Append("\n");
			stringBuilder.Append("  Region: ").Append(Region).Append("\n");
			stringBuilder.Append("  Selectors: ").Append(Selectors).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
