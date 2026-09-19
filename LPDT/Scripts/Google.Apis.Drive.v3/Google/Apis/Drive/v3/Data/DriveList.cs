using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class DriveList : IDirectResponseSchema
	{
		[JsonProperty("drives")]
		public virtual IList<Drive> Drives { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("nextPageToken")]
		public virtual string NextPageToken { get; set; }

		public virtual string ETag { get; set; }
	}
}
