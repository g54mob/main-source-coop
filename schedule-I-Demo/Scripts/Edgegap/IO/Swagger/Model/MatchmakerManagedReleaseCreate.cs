using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class MatchmakerManagedReleaseCreate : MatchmakerReleaseCreateBase
	{
		[DataMember(Name = "release_config_name", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "release_config_name")]
		public string ReleaseConfigName { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class MatchmakerManagedReleaseCreate {\n");
			stringBuilder.Append("  ReleaseConfigName: ").Append(ReleaseConfigName).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public new string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
