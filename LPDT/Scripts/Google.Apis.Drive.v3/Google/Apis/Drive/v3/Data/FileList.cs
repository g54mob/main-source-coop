using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class FileList : IDirectResponseSchema
	{
		[JsonProperty("files")]
		public virtual IList<File> Files { get; set; }

		[JsonProperty("incompleteSearch")]
		public virtual bool? IncompleteSearch { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("nextPageToken")]
		public virtual string NextPageToken { get; set; }

		public virtual string ETag { get; set; }
	}
}
