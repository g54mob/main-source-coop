using Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API
{
	public interface IReportService
	{
		void SendReport(ReportConfiguration reportConfiguration);
	}
}
