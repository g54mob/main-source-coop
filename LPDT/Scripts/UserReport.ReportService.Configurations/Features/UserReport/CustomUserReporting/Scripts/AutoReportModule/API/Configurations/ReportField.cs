namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class ReportField
	{
		public string Name { get; }

		public string Value { get; }

		public ReportField(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}
