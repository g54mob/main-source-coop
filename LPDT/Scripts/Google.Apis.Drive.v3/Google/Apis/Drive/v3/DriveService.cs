using System.Collections.Generic;
using Google.Apis.Discovery;
using Google.Apis.Services;

namespace Google.Apis.Drive.v3
{
	public class DriveService : BaseClientService
	{
		public class Scope
		{
			public static string Drive = "https://www.googleapis.com/auth/drive";

			public static string DriveAppdata = "https://www.googleapis.com/auth/drive.appdata";

			public static string DriveFile = "https://www.googleapis.com/auth/drive.file";

			public static string DriveMetadata = "https://www.googleapis.com/auth/drive.metadata";

			public static string DriveMetadataReadonly = "https://www.googleapis.com/auth/drive.metadata.readonly";

			public static string DrivePhotosReadonly = "https://www.googleapis.com/auth/drive.photos.readonly";

			public static string DriveReadonly = "https://www.googleapis.com/auth/drive.readonly";

			public static string DriveScripts = "https://www.googleapis.com/auth/drive.scripts";
		}

		public static class ScopeConstants
		{
			public const string Drive = "https://www.googleapis.com/auth/drive";

			public const string DriveAppdata = "https://www.googleapis.com/auth/drive.appdata";

			public const string DriveFile = "https://www.googleapis.com/auth/drive.file";

			public const string DriveMetadata = "https://www.googleapis.com/auth/drive.metadata";

			public const string DriveMetadataReadonly = "https://www.googleapis.com/auth/drive.metadata.readonly";

			public const string DrivePhotosReadonly = "https://www.googleapis.com/auth/drive.photos.readonly";

			public const string DriveReadonly = "https://www.googleapis.com/auth/drive.readonly";

			public const string DriveScripts = "https://www.googleapis.com/auth/drive.scripts";
		}

		public const string Version = "v3";

		public static DiscoveryVersion DiscoveryVersionUsed;

		public override IList<string> Features => new string[0];

		public override string Name => "drive";

		public override string BaseUri => base.BaseUriOverride ?? "https://www.googleapis.com/drive/v3/";

		public override string BasePath => "drive/v3/";

		public override string BatchUri => "https://www.googleapis.com/batch/drive/v3";

		public override string BatchPath => "batch/drive/v3";

		public virtual AboutResource About { get; }

		public virtual ChangesResource Changes { get; }

		public virtual ChannelsResource Channels { get; }

		public virtual CommentsResource Comments { get; }

		public virtual DrivesResource Drives { get; }

		public virtual FilesResource Files { get; }

		public virtual PermissionsResource Permissions { get; }

		public virtual RepliesResource Replies { get; }

		public virtual RevisionsResource Revisions { get; }

		public virtual TeamdrivesResource Teamdrives { get; }

		public DriveService()
			: this(new Initializer())
		{
		}

		public DriveService(Initializer initializer)
			: base(initializer)
		{
			About = new AboutResource(this);
			Changes = new ChangesResource(this);
			Channels = new ChannelsResource(this);
			Comments = new CommentsResource(this);
			Drives = new DrivesResource(this);
			Files = new FilesResource(this);
			Permissions = new PermissionsResource(this);
			Replies = new RepliesResource(this);
			Revisions = new RevisionsResource(this);
			Teamdrives = new TeamdrivesResource(this);
		}
	}
}
