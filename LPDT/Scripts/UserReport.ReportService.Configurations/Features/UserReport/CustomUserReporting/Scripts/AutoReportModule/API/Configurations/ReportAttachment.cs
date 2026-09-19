using Features.UserReport.CustomUserReporting.Scripts.Client;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public abstract class ReportAttachment
	{
		public string Name { get; set; }

		public string ContentType { get; set; }

		public string FileName { get; set; }

		public Features.UserReport.CustomUserReporting.Scripts.Client.UserReport Context { get; set; }

		protected ReportAttachment(string name, string contentType, string fileName)
		{
			Name = name;
			ContentType = contentType;
			FileName = fileName;
		}

		public abstract byte[] GetData();
	}
}
