using System;

namespace Fusion
{
	public static class LogLevelExtensions
	{
		public static string GetDefine(this LogLevel logLevel)
		{
			return logLevel switch
			{
				LogLevel.Debug => "FUSION_LOGLEVEL_DEBUG", 
				LogLevel.Info => "FUSION_LOGLEVEL_INFO", 
				LogLevel.Warn => "FUSION_LOGLEVEL_WARN", 
				LogLevel.Error => "FUSION_LOGLEVEL_ERROR", 
				LogLevel.None => "FUSION_LOGLEVEL_NONE", 
				_ => throw new ArgumentOutOfRangeException("logLevel"), 
			};
		}
	}
}
