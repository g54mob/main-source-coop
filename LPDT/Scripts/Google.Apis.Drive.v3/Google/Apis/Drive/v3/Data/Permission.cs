using System;
using System.Collections.Generic;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class Permission : IDirectResponseSchema
	{
		public class PermissionDetailsData
		{
			[JsonProperty("inherited")]
			public virtual bool? Inherited { get; set; }

			[JsonProperty("inheritedFrom")]
			public virtual string InheritedFrom { get; set; }

			[JsonProperty("permissionType")]
			public virtual string PermissionType { get; set; }

			[JsonProperty("role")]
			public virtual string Role { get; set; }
		}

		public class TeamDrivePermissionDetailsData
		{
			[JsonProperty("inherited")]
			public virtual bool? Inherited { get; set; }

			[JsonProperty("inheritedFrom")]
			public virtual string InheritedFrom { get; set; }

			[JsonProperty("role")]
			public virtual string Role { get; set; }

			[JsonProperty("teamDrivePermissionType")]
			public virtual string TeamDrivePermissionType { get; set; }
		}

		[JsonProperty("allowFileDiscovery")]
		public virtual bool? AllowFileDiscovery { get; set; }

		[JsonProperty("deleted")]
		public virtual bool? Deleted { get; set; }

		[JsonProperty("displayName")]
		public virtual string DisplayName { get; set; }

		[JsonProperty("domain")]
		public virtual string Domain { get; set; }

		[JsonProperty("emailAddress")]
		public virtual string EmailAddress { get; set; }

		[JsonProperty("expirationTime")]
		public virtual string ExpirationTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? ExpirationTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(ExpirationTimeRaw);
			}
			set
			{
				ExpirationTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("id")]
		public virtual string Id { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("pendingOwner")]
		public virtual bool? PendingOwner { get; set; }

		[JsonProperty("permissionDetails")]
		public virtual IList<PermissionDetailsData> PermissionDetails { get; set; }

		[JsonProperty("photoLink")]
		public virtual string PhotoLink { get; set; }

		[JsonProperty("role")]
		public virtual string Role { get; set; }

		[JsonProperty("teamDrivePermissionDetails")]
		public virtual IList<TeamDrivePermissionDetailsData> TeamDrivePermissionDetails { get; set; }

		[JsonProperty("type")]
		public virtual string Type { get; set; }

		[JsonProperty("view")]
		public virtual string View { get; set; }

		public virtual string ETag { get; set; }
	}
}
