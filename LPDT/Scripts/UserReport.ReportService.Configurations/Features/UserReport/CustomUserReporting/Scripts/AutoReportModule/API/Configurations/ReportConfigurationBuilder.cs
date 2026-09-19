using System.Collections.Generic;
using Features.UserReport.CustomUserReporting.Scripts.Client;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class ReportConfigurationBuilder
	{
		private const string REPORT_DEFAULT_NAME = "Report";

		private string _reportName = "Report";

		private readonly List<ReportDimension> _dimensions = new List<ReportDimension>();

		private readonly List<ReportAttachment> _attachments = new List<ReportAttachment>();

		private readonly List<ReportField> _fields = new List<ReportField>();

		private UserReportingClientConfiguration _userReportingClientConfiguration = new UserReportingClientConfiguration(100, MetricsGatheringMode.Disabled, 300, 1, 10);

		public ReportConfigurationBuilder WithReportName(string reportName)
		{
			_reportName = reportName;
			return this;
		}

		public ReportConfigurationBuilder WithAttachment(ReportAttachment reportAttachment)
		{
			_attachments.Add(reportAttachment);
			return this;
		}

		public ReportConfigurationBuilder WithDimension(ReportDimension reportDimension)
		{
			_dimensions.Add(reportDimension);
			return this;
		}

		public ReportConfigurationBuilder WithField(ReportField reportField)
		{
			_fields.Add(reportField);
			return this;
		}

		public ReportConfigurationBuilder WithUserReportingClientConfiguration(UserReportingClientConfiguration userReportingClientConfiguration)
		{
			_userReportingClientConfiguration = userReportingClientConfiguration;
			return this;
		}

		public ReportConfiguration Build()
		{
			return new ReportConfiguration
			{
				UserReportingClientConfiguration = _userReportingClientConfiguration,
				ReportName = _reportName,
				Attachments = _attachments,
				Dimensions = _dimensions,
				Fields = _fields
			};
		}
	}
}
