namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class ByteArrayReportAttachment : ReportAttachment
	{
		public byte[] Data { get; set; }

		public override byte[] GetData()
		{
			return Data;
		}

		public ByteArrayReportAttachment(string name, string contentType, string fileName, byte[] data)
			: base(name, contentType, fileName)
		{
			Data = data;
		}
	}
}
