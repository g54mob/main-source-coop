using System.IO;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class FileReportAttachment : ReportAttachment
	{
		public string FullPath { get; set; }

		public override byte[] GetData()
		{
			return File.ReadAllBytes(FullPath);
		}

		public FileReportAttachment(string name, string contentType, string fileName, string fullPath)
			: base(name, contentType, fileName)
		{
			FullPath = fullPath;
		}
	}
}
