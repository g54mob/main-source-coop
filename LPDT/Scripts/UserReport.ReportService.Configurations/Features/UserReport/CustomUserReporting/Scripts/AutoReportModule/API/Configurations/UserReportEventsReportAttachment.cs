using System.Text;
using Features.UserReport.CustomUserReporting.Scripts.Client;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class UserReportEventsReportAttachment : ReportAttachment
	{
		private const string CONTENT_TYPE = "text/plain";

		private const string LOGS_FILE_NAME = "Player.log";

		private const string NAME = "PlayerLogs";

		public UserReportEventsReportAttachment()
			: base("PlayerLogs", "text/plain", "Player.log")
		{
		}

		public override byte[] GetData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (UserReportEvent @event in base.Context.Events)
			{
				stringBuilder.Append(@event.FullMessage);
			}
			return Encoding.UTF8.GetBytes(stringBuilder.ToString());
		}
	}
}
