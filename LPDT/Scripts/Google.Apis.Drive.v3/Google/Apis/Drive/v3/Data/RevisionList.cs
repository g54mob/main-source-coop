using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class RevisionList : IDirectResponseSchema
	{
		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("nextPageToken")]
		public virtual string NextPageToken { get; set; }

		[JsonProperty("revisions")]
		public virtual IList<Revision> Revisions { get; set; }

		public virtual string ETag { get; set; }
	}
}
