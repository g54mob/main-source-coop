using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class ComponentCredentials
	{
		[DataMember(Name = "username", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "username")]
		public string Username { get; set; }

		[DataMember(Name = "token", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "token")]
		public string Token { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class ComponentCredentials {\n");
			stringBuilder.Append("  Username: ").Append(Username).Append("\n");
			stringBuilder.Append("  Token: ").Append(Token).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
