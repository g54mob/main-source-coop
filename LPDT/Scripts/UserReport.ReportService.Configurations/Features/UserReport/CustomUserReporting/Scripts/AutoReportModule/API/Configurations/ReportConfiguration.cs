using System.Collections.Generic;
using Features.UserReport.CustomUserReporting.Scripts.Client;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class ReportConfiguration
	{
		public UserReportingClientConfiguration UserReportingClientConfiguration { get; set; }

		public string ReportName { get; set; }

		public List<ReportAttachment> Attachments { get; set; }

		public List<ReportDimension> Dimensions { get; set; }

		public List<ReportField> Fields { get; set; }

		internal ReportConfiguration()
		{
		}

		public static ReportConfigurationBuilder Builder()
		{
			return new ReportConfigurationBuilder();
		}
	}
}
