using System;
using System.IO;
using UnityEngine;

namespace Features.UserReport.CustomUserReporting.Scripts.AutoReportModule.API.Configurations
{
	public class PlayerLogsReportAttachment : FileReportAttachment
	{
		private const string CONTENT_TYPE = "text/plain";

		private const string LOGS_FILE_NAME = "Player.log";

		private const string EDITOR_LOGS_FILE_NAME = "Editor.log";

		private const string NAME = "PlayerLogs";

		public PlayerLogsReportAttachment()
			: base("PlayerLogs", "text/plain", GetFileName(), GetFilePath())
		{
		}

		private static string GetFilePath()
		{
			if (Application.isEditor)
			{
				return Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Unity", "Editor"), "Editor.log");
			}
			return Path.Combine(Application.persistentDataPath, "Player.log");
		}

		private static string GetFileName()
		{
			if (!Application.isEditor)
			{
				return "Player.log";
			}
			return "Editor.log";
		}
	}
}
