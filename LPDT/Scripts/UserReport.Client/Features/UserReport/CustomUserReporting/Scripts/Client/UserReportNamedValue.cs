namespace Features.UserReport.CustomUserReporting.Scripts.Client
{
	public struct UserReportNamedValue
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public UserReportNamedValue(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}
