using UnityEngine;

namespace GameAnalyticsSDK.Net.Logging
{
	internal class GALogger
	{
		private static readonly GALogger _instance = new GALogger();

		private bool infoLogEnabled;

		private bool infoLogVerboseEnabled;

		private static bool debugEnabled;

		private const string Tag = "GameAnalytics";

		private static GALogger Instance => _instance;

		public static bool InfoLog
		{
			set
			{
				Instance.infoLogEnabled = value;
			}
		}

		public static bool VerboseLog
		{
			set
			{
				Instance.infoLogVerboseEnabled = value;
			}
		}

		private GALogger()
		{
		}

		public static void I(string format)
		{
			if (Instance.infoLogEnabled)
			{
				string message = "Info/GameAnalytics: " + format;
				Instance.SendNotificationMessage(message, EGALoggerMessageType.Info);
			}
		}

		public static void W(string format)
		{
			string message = "Warning/GameAnalytics: " + format;
			Instance.SendNotificationMessage(message, EGALoggerMessageType.Warning);
		}

		public static void E(string format)
		{
			string message = "Error/GameAnalytics: " + format;
			Instance.SendNotificationMessage(message, EGALoggerMessageType.Error);
		}

		public static void II(string format)
		{
			if (Instance.infoLogVerboseEnabled)
			{
				string message = "Verbose/GameAnalytics: " + format;
				Instance.SendNotificationMessage(message, EGALoggerMessageType.Info);
			}
		}

		public static void D(string format)
		{
			if (debugEnabled)
			{
				string message = "Debug/GameAnalytics: " + format;
				Instance.SendNotificationMessage(message, EGALoggerMessageType.Debug);
			}
		}

		private void SendNotificationMessage(string message, EGALoggerMessageType type)
		{
			switch (type)
			{
			case EGALoggerMessageType.Error:
				Debug.LogError(message);
				break;
			case EGALoggerMessageType.Warning:
				Debug.LogWarning(message);
				break;
			case EGALoggerMessageType.Debug:
				Debug.Log(message);
				break;
			case EGALoggerMessageType.Info:
				Debug.Log(message);
				break;
			}
		}
	}
}
