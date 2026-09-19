using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class PermissionList : IDirectResponseSchema
	{
		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("nextPageToken")]
		public virtual string NextPageToken { get; set; }

		[JsonProperty("permissions")]
		public virtual IList<Permission> Permissions { get; set; }

		public virtual string ETag { get; set; }
	}
}
