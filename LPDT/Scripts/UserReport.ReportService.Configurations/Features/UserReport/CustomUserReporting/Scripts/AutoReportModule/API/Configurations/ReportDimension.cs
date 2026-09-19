using Features.UserReport.CustomUserReporting.Scripts.Client;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public abstract class ReportDimension
	{
		public abstract string Name { get; }

		public abstract string Value { get; }

		public Features.UserReport.CustomUserReporting.Scripts.Client.UserReport Context { get; set; }
	}
}
