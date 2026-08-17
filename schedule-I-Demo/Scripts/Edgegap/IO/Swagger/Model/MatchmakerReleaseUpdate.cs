using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class MatchmakerReleaseUpdate : MatchmakerReleaseUpdateBase
	{
		[DataMember(Name = "frontend_component_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "frontend_component_name")]
		public string FrontendComponentName { get; set; }

		[DataMember(Name = "director_component_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "director_component_name")]
		public string DirectorComponentName { get; set; }

		[DataMember(Name = "match_function_component_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "match_function_component_name")]
		public string MatchFunctionComponentName { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class MatchmakerReleaseUpdate {\n");
			stringBuilder.Append("  FrontendComponentName: ").Append(FrontendComponentName).Append("\n");
			stringBuilder.Append("  DirectorComponentName: ").Append(DirectorComponentName).Append("\n");
			stringBuilder.Append("  MatchFunctionComponentName: ").Append(MatchFunctionComponentName).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public new string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
