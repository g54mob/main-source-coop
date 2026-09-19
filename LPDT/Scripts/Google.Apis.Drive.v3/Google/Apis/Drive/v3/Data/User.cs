using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class User : IDirectResponseSchema
	{
		[JsonProperty("displayName")]
		public virtual string DisplayName { get; set; }

		[JsonProperty("emailAddress")]
		public virtual string EmailAddress { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("me")]
		public virtual bool? Me { get; set; }

		[JsonProperty("permissionId")]
		public virtual string PermissionId { get; set; }

		[JsonProperty("photoLink")]
		public virtual string PhotoLink { get; set; }

		public virtual string ETag { get; set; }
	}
}
