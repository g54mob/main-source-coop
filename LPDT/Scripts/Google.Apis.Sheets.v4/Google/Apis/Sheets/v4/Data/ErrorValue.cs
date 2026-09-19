using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class ErrorValue : IDirectResponseSchema
	{
		[JsonProperty("message")]
		public virtual string Message { get; set; }

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		public virtual string ETag { get; set; }
	}
}
