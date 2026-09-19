using System;
using System.Diagnostics;
using System.Text;
using Photon.Client;
using UnityEngine;

namespace Photon.Realtime
{
	public static class Log
	{
		public enum PrefixOptions
		{
			None = 0,
			Time = 1,
			Level = 2,
			TimeAndLevel = 3
		}

		public enum LogOutputOption
		{
			Auto = 0,
			Console = 1,
			Debug = 2,
			UnityDebug = 3
		}

		public static PrefixOptions LogPrefix;

		private static Action<string> onError;

		private static Action<string> onWarn;

		private static Action<string> onInfo;

		private static Action<string> onDebug;

		private static Action<Exception, string> onException;

		private static Stopwatch sw;

		private static readonly StringBuilder prefixesBuilder;

		static Log()
		{
			LogPrefix = PrefixOptions.None;
			prefixesBuilder = new StringBuilder();
			Init(LogOutputOption.Auto);
		}

		public static void Init(LogOutputOption logOutput)
		{
			onError = null;
			onWarn = null;
			onInfo = null;
			onDebug = null;
			sw = new Stopwatch();
			sw.Restart();
			switch (logOutput)
			{
			case LogOutputOption.Auto:
			case LogOutputOption.UnityDebug:
				onError = UnityEngine.Debug.LogError;
				onWarn = UnityEngine.Debug.LogWarning;
				onInfo = UnityEngine.Debug.Log;
				onDebug = UnityEngine.Debug.Log;
				break;
			case LogOutputOption.Console:
				onError = delegate(string msg)
				{
					Console.WriteLine(msg);
				};
				onWarn = delegate(string msg)
				{
					Console.WriteLine(msg);
				};
				onInfo = delegate(string msg)
				{
					Console.WriteLine(msg);
				};
				onDebug = delegate(string msg)
				{
					Console.WriteLine(msg);
				};
				break;
			case LogOutputOption.Debug:
				onError = delegate
				{
				};
				onWarn = delegate
				{
				};
				onInfo = delegate
				{
				};
				onDebug = delegate
				{
				};
				break;
			}
		}

		public static void Init(Action<string> error, Action<string> warn, Action<string> info, Action<string> debug, Action<Exception, string> exception)
		{
			sw = new Stopwatch();
			sw.Restart();
			onError = error;
			onWarn = warn;
			onInfo = info;
			onDebug = debug;
			onException = exception;
		}

		private static string ApplyPrefixes(string msg, LogLevel lvl = LogLevel.Error, string prefix = null)
		{
			lock (prefixesBuilder)
			{
				prefixesBuilder.Clear();
				if (LogPrefix == PrefixOptions.Time || LogPrefix == PrefixOptions.TimeAndLevel)
				{
					TimeSpan elapsed = sw.Elapsed;
					if (elapsed.Minutes > 0)
					{
						prefixesBuilder.Append($"[{elapsed.Minutes}:{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}]");
					}
					else
					{
						prefixesBuilder.Append($"[{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}]");
					}
				}
				if (LogPrefix == PrefixOptions.Level || LogPrefix == PrefixOptions.TimeAndLevel)
				{
					prefixesBuilder.Append($"[{lvl}]");
				}
				if (!string.IsNullOrEmpty(prefix))
				{
					prefixesBuilder.Append(prefix + ": ");
				}
				else if (prefixesBuilder.Length > 0)
				{
					prefixesBuilder.Append(" ");
				}
				prefixesBuilder.Append(msg);
				return prefixesBuilder.ToString();
			}
		}

		public static void Exception(Exception ex, LogLevel lvl = LogLevel.Error, string prefix = null)
		{
			if ((int)lvl >= 1 && onException != null)
			{
				string arg = ApplyPrefixes(ex.Message, lvl, prefix);
				onException(ex, arg);
			}
		}

		public static void Error(string msg, LogLevel lvl = LogLevel.Error, string prefix = null)
		{
			if ((int)lvl >= 1 && onError != null)
			{
				string obj = ApplyPrefixes(msg, lvl, prefix);
				onError(obj);
			}
		}

		[Conditional("DEBUG")]
		[Conditional("PHOTON_LOG_WARNING")]
		public static void Warn(string msg, LogLevel lvl = LogLevel.Warning, string prefix = null)
		{
			if ((int)lvl >= 2 && onWarn != null)
			{
				string obj = ApplyPrefixes(msg, lvl, prefix);
				onWarn(obj);
			}
		}

		[Conditional("DEBUG")]
		[Conditional("PHOTON_LOG_INFO")]
		public static void Info(string msg, LogLevel lvl = LogLevel.Info, string prefix = null)
		{
			if ((int)lvl >= 3 && onInfo != null)
			{
				string obj = ApplyPrefixes(msg, lvl, prefix);
				onInfo(obj);
			}
		}

		[Conditional("DEBUG")]
		[Conditional("PHOTON_LOG_DEBUG")]
		public static void Debug(string msg, LogLevel lvl = LogLevel.Debug, string prefix = null)
		{
			if ((int)lvl >= 4 && onDebug != null)
			{
				string obj = ApplyPrefixes(msg, lvl, prefix);
				onDebug(obj);
			}
		}
	}
}
