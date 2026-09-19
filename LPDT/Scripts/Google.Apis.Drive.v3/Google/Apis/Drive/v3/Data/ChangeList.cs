using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class ChangeList : IDirectResponseSchema
	{
		[JsonProperty("changes")]
		public virtual IList<Change> Changes { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("newStartPageToken")]
		public virtual string NewStartPageToken { get; set; }

		[JsonProperty("nextPageToken")]
		public virtual string NextPageToken { get; set; }

		public virtual string ETag { get; set; }
	}
}
