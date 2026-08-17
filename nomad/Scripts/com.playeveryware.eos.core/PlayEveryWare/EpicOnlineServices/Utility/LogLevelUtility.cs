using System;
using System.Collections.Generic;
using System.IO;
using Epic.OnlineServices.Logging;
using UnityEngine;

namespace PlayEveryWare.EpicOnlineServices.Utility
{
	public static class LogLevelUtility
	{
		public static string[] LogCategoryStringArray => Enum.GetNames(typeof(LogCategory));

		public static string[] LogLevelStringArray => Enum.GetNames(typeof(LogLevel));

		public static List<LogLevel> LogLevelList
		{
			get
			{
				LogLevelConfig logLevelConfig = null;
				try
				{
					logLevelConfig = Config.Get<LogLevelConfig>();
				}
				catch (FileNotFoundException)
				{
					Debug.Log("Log level config does not exist, using default");
					return null;
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
					Debug.Log("Exception when reading log level config, using default");
					return null;
				}
				if (logLevelConfig.LogCategoryLevelPairs == null)
				{
					return null;
				}
				List<LogLevel> list = new List<LogLevel>();
				for (int i = 0; i < LogCategoryStringArray.Length - 1; i++)
				{
					if (Enum.TryParse<LogLevel>(logLevelConfig.LogCategoryLevelPairs[i].Level, out var result))
					{
						list.Add(result);
						continue;
					}
					list.Add(LogLevel.Info);
					Debug.Log("Failed to Parse Log Level");
				}
				return list;
			}
		}
	}
}
