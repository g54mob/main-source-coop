using System;
using System.Globalization;
using UnityEngine;

namespace Photon.Voice.Unity
{
	public static class UnityLogger
	{
		public static void Log(LogLevel level, UnityEngine.Object obj, string tag, string objName, string fmt, params object[] args)
		{
			fmt = GetFormatString(level, tag, objName, fmt);
			if (obj == null)
			{
				switch (level)
				{
				case LogLevel.Error:
					Debug.LogErrorFormat(fmt, args);
					break;
				case LogLevel.Warning:
					Debug.LogWarningFormat(fmt, args);
					break;
				default:
					Debug.LogFormat(fmt, args);
					break;
				}
			}
			else
			{
				switch (level)
				{
				case LogLevel.Error:
					Debug.LogErrorFormat(obj, fmt, args);
					break;
				case LogLevel.Warning:
					Debug.LogWarningFormat(obj, fmt, args);
					break;
				default:
					Debug.LogFormat(obj, fmt, args);
					break;
				}
			}
		}

		private static string GetFormatString(LogLevel level, string tag, string objName, string fmt)
		{
			return $"[{GetTimestamp()}] [{level}] [{tag}] [{objName}] {fmt}";
		}

		private static string GetTimestamp()
		{
			return DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss", new CultureInfo("en-US"));
		}
	}
}
