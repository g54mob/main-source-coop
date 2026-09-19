using System;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class TeamDrive : IDirectResponseSchema
	{
		public class BackgroundImageFileData
		{
			[JsonProperty("id")]
			public virtual string Id { get; set; }

			[JsonProperty("width")]
			public virtual float? Width { get; set; }

			[JsonProperty("xCoordinate")]
			public virtual float? XCoordinate { get; set; }

			[JsonProperty("yCoordinate")]
			public virtual float? YCoordinate { get; set; }
		}

		public class CapabilitiesData
		{
			[JsonProperty("canAddChildren")]
			public virtual bool? CanAddChildren { get; set; }

			[JsonProperty("canChangeCopyRequiresWriterPermissionRestriction")]
			public virtual bool? CanChangeCopyRequiresWriterPermissionRestriction { get; set; }

			[JsonProperty("canChangeDomainUsersOnlyRestriction")]
			public virtual bool? CanChangeDomainUsersOnlyRestriction { get; set; }

			[JsonProperty("canChangeTeamDriveBackground")]
			public virtual bool? CanChangeTeamDriveBackground { get; set; }

			[JsonProperty("canChangeTeamMembersOnlyRestriction")]
			public virtual bool? CanChangeTeamMembersOnlyRestriction { get; set; }

			[JsonProperty("canComment")]
			public virtual bool? CanComment { get; set; }

			[JsonProperty("canCopy")]
			public virtual bool? CanCopy { get; set; }

			[JsonProperty("canDeleteChildren")]
			public virtual bool? CanDeleteChildren { get; set; }

			[JsonProperty("canDeleteTeamDrive")]
			public virtual bool? CanDeleteTeamDrive { get; set; }

			[JsonProperty("canDownload")]
			public virtual bool? CanDownload { get; set; }

			[JsonProperty("canEdit")]
			public virtual bool? CanEdit { get; set; }

			[JsonProperty("canListChildren")]
			public virtual bool? CanListChildren { get; set; }

			[JsonProperty("canManageMembers")]
			public virtual bool? CanManageMembers { get; set; }

			[JsonProperty("canReadRevisions")]
			public virtual bool? CanReadRevisions { get; set; }

			[JsonProperty("canRemoveChildren")]
			public virtual bool? CanRemoveChildren { get; set; }

			[JsonProperty("canRename")]
			public virtual bool? CanRename { get; set; }

			[JsonProperty("canRenameTeamDrive")]
			public virtual bool? CanRenameTeamDrive { get; set; }

			[JsonProperty("canResetTeamDriveRestrictions")]
			public virtual bool? CanResetTeamDriveRestrictions { get; set; }

			[JsonProperty("canShare")]
			public virtual bool? CanShare { get; set; }

			[JsonProperty("canTrashChildren")]
			public virtual bool? CanTrashChildren { get; set; }
		}

		public class RestrictionsData
		{
			[JsonProperty("adminManagedRestrictions")]
			public virtual bool? AdminManagedRestrictions { get; set; }

			[JsonProperty("copyRequiresWriterPermission")]
			public virtual bool? CopyRequiresWriterPermission { get; set; }

			[JsonProperty("domainUsersOnly")]
			public virtual bool? DomainUsersOnly { get; set; }

			[JsonProperty("teamMembersOnly")]
			public virtual bool? TeamMembersOnly { get; set; }
		}

		[JsonProperty("backgroundImageFile")]
		public virtual BackgroundImageFileData BackgroundImageFile { get; set; }

		[JsonProperty("backgroundImageLink")]
		public virtual string BackgroundImageLink { get; set; }

		[JsonProperty("capabilities")]
		public virtual CapabilitiesData Capabilities { get; set; }

		[JsonProperty("colorRgb")]
		public virtual string ColorRgb { get; set; }

		[JsonProperty("createdTime")]
		public virtual string CreatedTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? CreatedTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(CreatedTimeRaw);
			}
			set
			{
				CreatedTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("id")]
		public virtual string Id { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("name")]
		public virtual string Name { get; set; }

		[JsonProperty("orgUnitId")]
		public virtual string OrgUnitId { get; set; }

		[JsonProperty("restrictions")]
		public virtual RestrictionsData Restrictions { get; set; }

		[JsonProperty("themeId")]
		public virtual string ThemeId { get; set; }

		public virtual string ETag { get; set; }
	}
}
