using UnityEngine;

namespace Photon.Voice.Unity
{
	public class Logger : ILogger
	{
		public LogLevel Level { get; set; }

		public Logger(LogLevel level = LogLevel.Debug)
		{
			Level = level;
		}

		public void Log(LogLevel level, string fmt, params object[] args)
		{
			if (Level >= level)
			{
				if (level >= LogLevel.Info)
				{
					Debug.LogFormat(fmt, args);
				}
				else if (level == LogLevel.Warning)
				{
					Debug.LogWarningFormat(fmt, args);
				}
				else
				{
					Debug.LogErrorFormat(fmt, args);
				}
			}
		}
	}
}
