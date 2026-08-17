using UnityEngine;

namespace QFSW.QC
{
	public static class LogTypeExtensions
	{
		public static LoggingThreshold ToLoggingThreshold(this LogType logType)
		{
			LoggingThreshold result = LoggingThreshold.Always;
			switch (logType)
			{
			case LogType.Exception:
				result = LoggingThreshold.Exception;
				break;
			case LogType.Error:
				result = LoggingThreshold.Error;
				break;
			case LogType.Assert:
				result = LoggingThreshold.Error;
				break;
			case LogType.Warning:
				result = LoggingThreshold.Warning;
				break;
			case LogType.Log:
				result = LoggingThreshold.Always;
				break;
			}
			return result;
		}
	}
}
