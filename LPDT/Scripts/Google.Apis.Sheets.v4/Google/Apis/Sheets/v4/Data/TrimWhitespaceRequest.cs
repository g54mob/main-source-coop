using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class TrimWhitespaceRequest : IDirectResponseSchema
	{
		[JsonProperty("range")]
		public virtual GridRange Range { get; set; }

		public virtual string ETag { get; set; }
	}
}
