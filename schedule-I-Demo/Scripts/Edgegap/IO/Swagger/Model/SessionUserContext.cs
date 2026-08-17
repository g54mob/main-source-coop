using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class SessionUserContext
	{
		[DataMember(Name = "session_users", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "session_users")]
		public List<SessionUser> SessionUsers { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class SessionUserContext {\n");
			stringBuilder.Append("  SessionUsers: ").Append(SessionUsers).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
