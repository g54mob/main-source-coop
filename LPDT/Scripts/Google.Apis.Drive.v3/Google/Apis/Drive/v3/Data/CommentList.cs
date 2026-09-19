using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class CommentList : IDirectResponseSchema
	{
		[JsonProperty("comments")]
		public virtual IList<Comment> Comments { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("nextPageToken")]
		public virtual string NextPageToken { get; set; }

		public virtual string ETag { get; set; }
	}
}
