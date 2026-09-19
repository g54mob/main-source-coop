using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Sheets.v4.Data
{
	public class Link : IDirectResponseSchema
	{
		[JsonProperty("uri")]
		public virtual string Uri { get; set; }

		public virtual string ETag { get; set; }
	}
}
