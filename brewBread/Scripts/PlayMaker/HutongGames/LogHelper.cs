using System.Diagnostics;
using UnityEngine;

namespace HutongGames
{
	public static class LogHelper
	{
		[Conditional("DEBUG_LOG")]
		public static void LogWarning(object prefix, object message)
		{
			UnityEngine.Debug.LogWarning(string.Concat(prefix, ": ", message));
		}

		[Conditional("DEBUG_LOG")]
		public static void Log(object message, LogColor logColor = LogColor.None)
		{
			UnityEngine.Debug.Log(FormatLog(message, logColor));
		}

		[Conditional("DEBUG_LOG")]
		public static void Log(object prefix, object message, LogColor logColor = LogColor.None)
		{
			UnityEngine.Debug.Log(string.Concat(prefix, ": ", FormatLog(message, logColor)));
		}

		[Conditional("DEBUG_LOG")]
		public static void Log(object prefix, object message, object postfix, LogColor logColor = LogColor.None)
		{
			UnityEngine.Debug.Log(string.Concat(prefix, ": ", FormatLog(message, logColor), " \t", postfix));
		}

		private static object FormatLog(object message, LogColor logColor)
		{
			return logColor switch
			{
				LogColor.Green => string.Concat("<color=green>", message, "</color>"), 
				LogColor.Yellow => string.Concat("<color=yellow>", message, "</color>"), 
				LogColor.Red => string.Concat("<color=red>", message, "</color>"), 
				_ => message, 
			};
		}
	}
}
