using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class StartPageToken : IDirectResponseSchema
	{
		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("startPageToken")]
		public virtual string StartPageTokenValue { get; set; }

		public virtual string ETag { get; set; }
	}
}
