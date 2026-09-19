using System.Collections.Generic;

namespace Features.UserReport.CustomUserReporting.Scripts.Client
{
	public class UserReportList
	{
		public string ContinuationToken { get; set; }

		public string Error { get; set; }

		public bool HasMore { get; set; }

		public List<UserReportPreview> UserReportPreviews { get; set; }

		public UserReportList()
		{
			UserReportPreviews = new List<UserReportPreview>();
		}

		public void Complete(int originalLimit, string continuationToken)
		{
			if (UserReportPreviews.Count > 0 && UserReportPreviews.Count > originalLimit)
			{
				while (UserReportPreviews.Count > originalLimit)
				{
					UserReportPreviews.RemoveAt(UserReportPreviews.Count - 1);
				}
				ContinuationToken = continuationToken;
				HasMore = true;
			}
		}
	}
}
