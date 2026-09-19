using Features.UserReport.CustomUserReporting.Scripts.Client;

namespace Features.UserReport.CustomUserReporting.Scripts.Plugin
{
	public static class UnityUserReportParser
	{
		public static Features.UserReport.CustomUserReporting.Scripts.Client.UserReport ParseUserReport(string json)
		{
			return SimpleJson.DeserializeObject<Features.UserReport.CustomUserReporting.Scripts.Client.UserReport>(json);
		}

		public static UserReportList ParseUserReportList(string json)
		{
			return SimpleJson.DeserializeObject<UserReportList>(json);
		}
	}
}
