using Features.UserReport.CustomUserReporting.Scripts.Client;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class PlatformVersionReportDimension : ReportDimension
	{
		private const string UNKNOWN = "Unknown";

		private const string VERSION_0_0 = "0.0";

		private const string VERSION = "Version";

		private const string PLATFORM_VERSION = "Platform.Version";

		private const string PLATFORM = "Platform";

		public override string Value => GetPlatformDimensionData(base.Context);

		public override string Name => "Platform.Version";

		private string GetPlatformDimensionData(Features.UserReport.CustomUserReporting.Scripts.Client.UserReport userReport)
		{
			string text = "Unknown";
			string text2 = "0.0";
			foreach (UserReportNamedValue deviceMetadatum in userReport.DeviceMetadata)
			{
				if (deviceMetadatum.Name == "Platform")
				{
					text = deviceMetadatum.Value;
				}
				if (deviceMetadatum.Name == "Version")
				{
					text2 = deviceMetadatum.Value;
				}
			}
			return text + "." + text2;
		}
	}
}
