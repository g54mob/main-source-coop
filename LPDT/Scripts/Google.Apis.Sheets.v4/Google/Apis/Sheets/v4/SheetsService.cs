using System.Collections.Generic;
using Google.Apis.Discovery;
using Google.Apis.Services;

namespace Google.Apis.Sheets.v4
{
	public class SheetsService : BaseClientService
	{
		public class Scope
		{
			public static string Drive = "https://www.googleapis.com/auth/drive";

			public static string DriveFile = "https://www.googleapis.com/auth/drive.file";

			public static string DriveReadonly = "https://www.googleapis.com/auth/drive.readonly";

			public static string Spreadsheets = "https://www.googleapis.com/auth/spreadsheets";

			public static string SpreadsheetsReadonly = "https://www.googleapis.com/auth/spreadsheets.readonly";
		}

		public static class ScopeConstants
		{
			public const string Drive = "https://www.googleapis.com/auth/drive";

			public const string DriveFile = "https://www.googleapis.com/auth/drive.file";

			public const string DriveReadonly = "https://www.googleapis.com/auth/drive.readonly";

			public const string Spreadsheets = "https://www.googleapis.com/auth/spreadsheets";

			public const string SpreadsheetsReadonly = "https://www.googleapis.com/auth/spreadsheets.readonly";
		}

		public const string Version = "v4";

		public static DiscoveryVersion DiscoveryVersionUsed;

		public override IList<string> Features => new string[0];

		public override string Name => "sheets";

		public override string BaseUri => base.BaseUriOverride ?? "https://sheets.googleapis.com/";

		public override string BasePath => "";

		public override string BatchUri => "https://sheets.googleapis.com/batch";

		public override string BatchPath => "batch";

		public virtual SpreadsheetsResource Spreadsheets { get; }

		public SheetsService()
			: this(new Initializer())
		{
		}

		public SheetsService(Initializer initializer)
			: base(initializer)
		{
			Spreadsheets = new SpreadsheetsResource(this);
		}
	}
}
