using System;
using System.Globalization;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Photon.Voice.Unity
{
	public static class UnityLogger
	{
		public static void Log(DebugLevel level, UnityEngine.Object obj, string tag, string objName, string fmt, params object[] args)
		{
			fmt = GetFormatString(level, tag, objName, fmt);
			if (obj == null)
			{
				switch (level)
				{
				case DebugLevel.ERROR:
					Debug.LogErrorFormat(fmt, args);
					break;
				case DebugLevel.WARNING:
					Debug.LogWarningFormat(fmt, args);
					break;
				case DebugLevel.INFO:
					Debug.LogFormat(fmt, args);
					break;
				case DebugLevel.ALL:
					Debug.LogFormat(fmt, args);
					break;
				case (DebugLevel)4:
					break;
				}
			}
			else
			{
				switch (level)
				{
				case DebugLevel.ERROR:
					Debug.LogErrorFormat(obj, fmt, args);
					break;
				case DebugLevel.WARNING:
					Debug.LogWarningFormat(obj, fmt, args);
					break;
				case DebugLevel.INFO:
					Debug.LogFormat(obj, fmt, args);
					break;
				case DebugLevel.ALL:
					Debug.LogFormat(obj, fmt, args);
					break;
				case (DebugLevel)4:
					break;
				}
			}
		}

		private static string GetFormatString(DebugLevel level, string tag, string objName, string fmt)
		{
			return $"[{GetTimestamp()}] [{level}] [{tag}] [{objName}] {fmt}";
		}

		private static string GetTimestamp()
		{
			return DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss", new CultureInfo("en-US"));
		}
	}
}
