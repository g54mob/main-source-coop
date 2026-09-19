using System;
using System.Collections.Generic;
using Google.Apis.Requests;
using Google.Apis.Util;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class File : IDirectResponseSchema
	{
		public class CapabilitiesData
		{
			[JsonProperty("canAcceptOwnership")]
			public virtual bool? CanAcceptOwnership { get; set; }

			[JsonProperty("canAddChildren")]
			public virtual bool? CanAddChildren { get; set; }

			[JsonProperty("canAddFolderFromAnotherDrive")]
			public virtual bool? CanAddFolderFromAnotherDrive { get; set; }

			[JsonProperty("canAddMyDriveParent")]
			public virtual bool? CanAddMyDriveParent { get; set; }

			[JsonProperty("canChangeCopyRequiresWriterPermission")]
			public virtual bool? CanChangeCopyRequiresWriterPermission { get; set; }

			[JsonProperty("canChangeSecurityUpdateEnabled")]
			public virtual bool? CanChangeSecurityUpdateEnabled { get; set; }

			[JsonProperty("canChangeViewersCanCopyContent")]
			public virtual bool? CanChangeViewersCanCopyContent { get; set; }

			[JsonProperty("canComment")]
			public virtual bool? CanComment { get; set; }

			[JsonProperty("canCopy")]
			public virtual bool? CanCopy { get; set; }

			[JsonProperty("canDelete")]
			public virtual bool? CanDelete { get; set; }

			[JsonProperty("canDeleteChildren")]
			public virtual bool? CanDeleteChildren { get; set; }

			[JsonProperty("canDownload")]
			public virtual bool? CanDownload { get; set; }

			[JsonProperty("canEdit")]
			public virtual bool? CanEdit { get; set; }

			[JsonProperty("canListChildren")]
			public virtual bool? CanListChildren { get; set; }

			[JsonProperty("canModifyContent")]
			public virtual bool? CanModifyContent { get; set; }

			[JsonProperty("canModifyContentRestriction")]
			public virtual bool? CanModifyContentRestriction { get; set; }

			[JsonProperty("canModifyLabels")]
			public virtual bool? CanModifyLabels { get; set; }

			[JsonProperty("canMoveChildrenOutOfDrive")]
			public virtual bool? CanMoveChildrenOutOfDrive { get; set; }

			[JsonProperty("canMoveChildrenOutOfTeamDrive")]
			public virtual bool? CanMoveChildrenOutOfTeamDrive { get; set; }

			[JsonProperty("canMoveChildrenWithinDrive")]
			public virtual bool? CanMoveChildrenWithinDrive { get; set; }

			[JsonProperty("canMoveChildrenWithinTeamDrive")]
			public virtual bool? CanMoveChildrenWithinTeamDrive { get; set; }

			[JsonProperty("canMoveItemIntoTeamDrive")]
			public virtual bool? CanMoveItemIntoTeamDrive { get; set; }

			[JsonProperty("canMoveItemOutOfDrive")]
			public virtual bool? CanMoveItemOutOfDrive { get; set; }

			[JsonProperty("canMoveItemOutOfTeamDrive")]
			public virtual bool? CanMoveItemOutOfTeamDrive { get; set; }

			[JsonProperty("canMoveItemWithinDrive")]
			public virtual bool? CanMoveItemWithinDrive { get; set; }

			[JsonProperty("canMoveItemWithinTeamDrive")]
			public virtual bool? CanMoveItemWithinTeamDrive { get; set; }

			[JsonProperty("canMoveTeamDriveItem")]
			public virtual bool? CanMoveTeamDriveItem { get; set; }

			[JsonProperty("canReadDrive")]
			public virtual bool? CanReadDrive { get; set; }

			[JsonProperty("canReadLabels")]
			public virtual bool? CanReadLabels { get; set; }

			[JsonProperty("canReadRevisions")]
			public virtual bool? CanReadRevisions { get; set; }

			[JsonProperty("canReadTeamDrive")]
			public virtual bool? CanReadTeamDrive { get; set; }

			[JsonProperty("canRemoveChildren")]
			public virtual bool? CanRemoveChildren { get; set; }

			[JsonProperty("canRemoveMyDriveParent")]
			public virtual bool? CanRemoveMyDriveParent { get; set; }

			[JsonProperty("canRename")]
			public virtual bool? CanRename { get; set; }

			[JsonProperty("canShare")]
			public virtual bool? CanShare { get; set; }

			[JsonProperty("canTrash")]
			public virtual bool? CanTrash { get; set; }

			[JsonProperty("canTrashChildren")]
			public virtual bool? CanTrashChildren { get; set; }

			[JsonProperty("canUntrash")]
			public virtual bool? CanUntrash { get; set; }
		}

		public class ContentHintsData
		{
			public class ThumbnailData
			{
				[JsonProperty("image")]
				public virtual string Image { get; set; }

				[JsonProperty("mimeType")]
				public virtual string MimeType { get; set; }
			}

			[JsonProperty("indexableText")]
			public virtual string IndexableText { get; set; }

			[JsonProperty("thumbnail")]
			public virtual ThumbnailData Thumbnail { get; set; }
		}

		public class ImageMediaMetadataData
		{
			public class LocationData
			{
				[JsonProperty("altitude")]
				public virtual double? Altitude { get; set; }

				[JsonProperty("latitude")]
				public virtual double? Latitude { get; set; }

				[JsonProperty("longitude")]
				public virtual double? Longitude { get; set; }
			}

			[JsonProperty("aperture")]
			public virtual float? Aperture { get; set; }

			[JsonProperty("cameraMake")]
			public virtual string CameraMake { get; set; }

			[JsonProperty("cameraModel")]
			public virtual string CameraModel { get; set; }

			[JsonProperty("colorSpace")]
			public virtual string ColorSpace { get; set; }

			[JsonProperty("exposureBias")]
			public virtual float? ExposureBias { get; set; }

			[JsonProperty("exposureMode")]
			public virtual string ExposureMode { get; set; }

			[JsonProperty("exposureTime")]
			public virtual float? ExposureTime { get; set; }

			[JsonProperty("flashUsed")]
			public virtual bool? FlashUsed { get; set; }

			[JsonProperty("focalLength")]
			public virtual float? FocalLength { get; set; }

			[JsonProperty("height")]
			public virtual int? Height { get; set; }

			[JsonProperty("isoSpeed")]
			public virtual int? IsoSpeed { get; set; }

			[JsonProperty("lens")]
			public virtual string Lens { get; set; }

			[JsonProperty("location")]
			public virtual LocationData Location { get; set; }

			[JsonProperty("maxApertureValue")]
			public virtual float? MaxApertureValue { get; set; }

			[JsonProperty("meteringMode")]
			public virtual string MeteringMode { get; set; }

			[JsonProperty("rotation")]
			public virtual int? Rotation { get; set; }

			[JsonProperty("sensor")]
			public virtual string Sensor { get; set; }

			[JsonProperty("subjectDistance")]
			public virtual int? SubjectDistance { get; set; }

			[JsonProperty("time")]
			public virtual string Time { get; set; }

			[JsonProperty("whiteBalance")]
			public virtual string WhiteBalance { get; set; }

			[JsonProperty("width")]
			public virtual int? Width { get; set; }
		}

		public class LabelInfoData
		{
			[JsonProperty("labels")]
			public virtual IList<Label> Labels { get; set; }
		}

		public class LinkShareMetadataData
		{
			[JsonProperty("securityUpdateEligible")]
			public virtual bool? SecurityUpdateEligible { get; set; }

			[JsonProperty("securityUpdateEnabled")]
			public virtual bool? SecurityUpdateEnabled { get; set; }
		}

		public class ShortcutDetailsData
		{
			[JsonProperty("targetId")]
			public virtual string TargetId { get; set; }

			[JsonProperty("targetMimeType")]
			public virtual string TargetMimeType { get; set; }

			[JsonProperty("targetResourceKey")]
			public virtual string TargetResourceKey { get; set; }
		}

		public class VideoMediaMetadataData
		{
			[JsonProperty("durationMillis")]
			public virtual long? DurationMillis { get; set; }

			[JsonProperty("height")]
			public virtual int? Height { get; set; }

			[JsonProperty("width")]
			public virtual int? Width { get; set; }
		}

		[JsonProperty("appProperties")]
		public virtual IDictionary<string, string> AppProperties { get; set; }

		[JsonProperty("capabilities")]
		public virtual CapabilitiesData Capabilities { get; set; }

		[JsonProperty("contentHints")]
		public virtual ContentHintsData ContentHints { get; set; }

		[JsonProperty("contentRestrictions")]
		public virtual IList<ContentRestriction> ContentRestrictions { get; set; }

		[JsonProperty("copyRequiresWriterPermission")]
		public virtual bool? CopyRequiresWriterPermission { get; set; }

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

		[JsonProperty("description")]
		public virtual string Description { get; set; }

		[JsonProperty("driveId")]
		public virtual string DriveId { get; set; }

		[JsonProperty("explicitlyTrashed")]
		public virtual bool? ExplicitlyTrashed { get; set; }

		[JsonProperty("exportLinks")]
		public virtual IDictionary<string, string> ExportLinks { get; set; }

		[JsonProperty("fileExtension")]
		public virtual string FileExtension { get; set; }

		[JsonProperty("folderColorRgb")]
		public virtual string FolderColorRgb { get; set; }

		[JsonProperty("fullFileExtension")]
		public virtual string FullFileExtension { get; set; }

		[JsonProperty("hasAugmentedPermissions")]
		public virtual bool? HasAugmentedPermissions { get; set; }

		[JsonProperty("hasThumbnail")]
		public virtual bool? HasThumbnail { get; set; }

		[JsonProperty("headRevisionId")]
		public virtual string HeadRevisionId { get; set; }

		[JsonProperty("iconLink")]
		public virtual string IconLink { get; set; }

		[JsonProperty("id")]
		public virtual string Id { get; set; }

		[JsonProperty("imageMediaMetadata")]
		public virtual ImageMediaMetadataData ImageMediaMetadata { get; set; }

		[JsonProperty("isAppAuthorized")]
		public virtual bool? IsAppAuthorized { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("labelInfo")]
		public virtual LabelInfoData LabelInfo { get; set; }

		[JsonProperty("lastModifyingUser")]
		public virtual User LastModifyingUser { get; set; }

		[JsonProperty("linkShareMetadata")]
		public virtual LinkShareMetadataData LinkShareMetadata { get; set; }

		[JsonProperty("md5Checksum")]
		public virtual string Md5Checksum { get; set; }

		[JsonProperty("mimeType")]
		public virtual string MimeType { get; set; }

		[JsonProperty("modifiedByMe")]
		public virtual bool? ModifiedByMe { get; set; }

		[JsonProperty("modifiedByMeTime")]
		public virtual string ModifiedByMeTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? ModifiedByMeTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(ModifiedByMeTimeRaw);
			}
			set
			{
				ModifiedByMeTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("modifiedTime")]
		public virtual string ModifiedTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? ModifiedTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(ModifiedTimeRaw);
			}
			set
			{
				ModifiedTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("name")]
		public virtual string Name { get; set; }

		[JsonProperty("originalFilename")]
		public virtual string OriginalFilename { get; set; }

		[JsonProperty("ownedByMe")]
		public virtual bool? OwnedByMe { get; set; }

		[JsonProperty("owners")]
		public virtual IList<User> Owners { get; set; }

		[JsonProperty("parents")]
		public virtual IList<string> Parents { get; set; }

		[JsonProperty("permissionIds")]
		public virtual IList<string> PermissionIds { get; set; }

		[JsonProperty("permissions")]
		public virtual IList<Permission> Permissions { get; set; }

		[JsonProperty("properties")]
		public virtual IDictionary<string, string> Properties { get; set; }

		[JsonProperty("quotaBytesUsed")]
		public virtual long? QuotaBytesUsed { get; set; }

		[JsonProperty("resourceKey")]
		public virtual string ResourceKey { get; set; }

		[JsonProperty("sha1Checksum")]
		public virtual string Sha1Checksum { get; set; }

		[JsonProperty("sha256Checksum")]
		public virtual string Sha256Checksum { get; set; }

		[JsonProperty("shared")]
		public virtual bool? Shared { get; set; }

		[JsonProperty("sharedWithMeTime")]
		public virtual string SharedWithMeTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? SharedWithMeTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(SharedWithMeTimeRaw);
			}
			set
			{
				SharedWithMeTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("sharingUser")]
		public virtual User SharingUser { get; set; }

		[JsonProperty("shortcutDetails")]
		public virtual ShortcutDetailsData ShortcutDetails { get; set; }

		[JsonProperty("size")]
		public virtual long? Size { get; set; }

		[JsonProperty("spaces")]
		public virtual IList<string> Spaces { get; set; }

		[JsonProperty("starred")]
		public virtual bool? Starred { get; set; }

		[JsonProperty("teamDriveId")]
		public virtual string TeamDriveId { get; set; }

		[JsonProperty("thumbnailLink")]
		public virtual string ThumbnailLink { get; set; }

		[JsonProperty("thumbnailVersion")]
		public virtual long? ThumbnailVersion { get; set; }

		[JsonProperty("trashed")]
		public virtual bool? Trashed { get; set; }

		[JsonProperty("trashedTime")]
		public virtual string TrashedTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? TrashedTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(TrashedTimeRaw);
			}
			set
			{
				TrashedTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("trashingUser")]
		public virtual User TrashingUser { get; set; }

		[JsonProperty("version")]
		public virtual long? Version { get; set; }

		[JsonProperty("videoMediaMetadata")]
		public virtual VideoMediaMetadataData VideoMediaMetadata { get; set; }

		[JsonProperty("viewedByMe")]
		public virtual bool? ViewedByMe { get; set; }

		[JsonProperty("viewedByMeTime")]
		public virtual string ViewedByMeTimeRaw { get; set; }

		[JsonIgnore]
		public virtual DateTime? ViewedByMeTime
		{
			get
			{
				return Utilities.GetDateTimeFromString(ViewedByMeTimeRaw);
			}
			set
			{
				ViewedByMeTimeRaw = Utilities.GetStringFromDateTime(value);
			}
		}

		[JsonProperty("viewersCanCopyContent")]
		public virtual bool? ViewersCanCopyContent { get; set; }

		[JsonProperty("webContentLink")]
		public virtual string WebContentLink { get; set; }

		[JsonProperty("webViewLink")]
		public virtual string WebViewLink { get; set; }

		[JsonProperty("writersCanShare")]
		public virtual bool? WritersCanShare { get; set; }

		public virtual string ETag { get; set; }
	}
}
