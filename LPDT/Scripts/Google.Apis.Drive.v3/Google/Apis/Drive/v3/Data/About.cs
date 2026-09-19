using System.Collections.Generic;
using Google.Apis.Requests;
using Newtonsoft.Json;

namespace Google.Apis.Drive.v3.Data
{
	public class About : IDirectResponseSchema
	{
		public class DriveThemesData
		{
			[JsonProperty("backgroundImageLink")]
			public virtual string BackgroundImageLink { get; set; }

			[JsonProperty("colorRgb")]
			public virtual string ColorRgb { get; set; }

			[JsonProperty("id")]
			public virtual string Id { get; set; }
		}

		public class StorageQuotaData
		{
			[JsonProperty("limit")]
			public virtual long? Limit { get; set; }

			[JsonProperty("usage")]
			public virtual long? Usage { get; set; }

			[JsonProperty("usageInDrive")]
			public virtual long? UsageInDrive { get; set; }

			[JsonProperty("usageInDriveTrash")]
			public virtual long? UsageInDriveTrash { get; set; }
		}

		public class TeamDriveThemesData
		{
			[JsonProperty("backgroundImageLink")]
			public virtual string BackgroundImageLink { get; set; }

			[JsonProperty("colorRgb")]
			public virtual string ColorRgb { get; set; }

			[JsonProperty("id")]
			public virtual string Id { get; set; }
		}

		[JsonProperty("appInstalled")]
		public virtual bool? AppInstalled { get; set; }

		[JsonProperty("canCreateDrives")]
		public virtual bool? CanCreateDrives { get; set; }

		[JsonProperty("canCreateTeamDrives")]
		public virtual bool? CanCreateTeamDrives { get; set; }

		[JsonProperty("driveThemes")]
		public virtual IList<DriveThemesData> DriveThemes { get; set; }

		[JsonProperty("exportFormats")]
		public virtual IDictionary<string, IList<string>> ExportFormats { get; set; }

		[JsonProperty("folderColorPalette")]
		public virtual IList<string> FolderColorPalette { get; set; }

		[JsonProperty("importFormats")]
		public virtual IDictionary<string, IList<string>> ImportFormats { get; set; }

		[JsonProperty("kind")]
		public virtual string Kind { get; set; }

		[JsonProperty("maxImportSizes")]
		public virtual IDictionary<string, long?> MaxImportSizes { get; set; }

		[JsonProperty("maxUploadSize")]
		public virtual long? MaxUploadSize { get; set; }

		[JsonProperty("storageQuota")]
		public virtual StorageQuotaData StorageQuota { get; set; }

		[JsonProperty("teamDriveThemes")]
		public virtual IList<TeamDriveThemesData> TeamDriveThemes { get; set; }

		[JsonProperty("user")]
		public virtual User User { get; set; }

		public virtual string ETag { get; set; }
	}
}
