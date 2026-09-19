using System.IO;
using UnityEngine;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class PersistentDataPathFileReportAttachment : FileReportAttachment
	{
		public PersistentDataPathFileReportAttachment(string name, string contentType, string fileName, string relativePath)
			: base(name, contentType, fileName, Path.Combine(Application.persistentDataPath, relativePath))
		{
		}
	}
}
