using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Model
{
	[DataContract]
	public class SessionDelete
	{
		[DataMember(Name = "message", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "message")]
		public string Message { get; set; }

		[DataMember(Name = "session_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "session_id")]
		public string SessionId { get; set; }

		[DataMember(Name = "custom_id", EmitDefaultValue = false)]
		[JsonProperty(PropertyName = "custom_id")]
		public string CustomId { get; set; }

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("class SessionDelete {\n");
			stringBuilder.Append("  Message: ").Append(Message).Append("\n");
			stringBuilder.Append("  SessionId: ").Append(SessionId).Append("\n");
			stringBuilder.Append("  CustomId: ").Append(CustomId).Append("\n");
			stringBuilder.Append("}\n");
			return stringBuilder.ToString();
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}
