using System;
using System.Globalization;
using Google.Apis.Util;

namespace Google.Apis.Logging
{
	public abstract class BaseLogger : ILogger
	{
		private const string DateTimeFormatString = "yyyy-MM-dd HH:mm:ss.ffffff";

		private readonly string _loggerForTypeString;

		public IClock Clock { get; }

		public Type LoggerForType { get; }

		public LogLevel MinimumLogLevel { get; }

		public bool IsDebugEnabled { get; }

		public bool IsInfoEnabled { get; }

		public bool IsWarningEnabled { get; }

		public bool IsErrorEnabled { get; }

		protected BaseLogger(LogLevel minimumLogLevel, IClock clock, Type forType)
		{
			MinimumLogLevel = minimumLogLevel;
			IsDebugEnabled = minimumLogLevel <= LogLevel.Debug;
			IsInfoEnabled = minimumLogLevel <= LogLevel.Info;
			IsWarningEnabled = minimumLogLevel <= LogLevel.Warning;
			IsErrorEnabled = minimumLogLevel <= LogLevel.Error;
			Clock = clock ?? SystemClock.Default;
			LoggerForType = forType;
			if (forType != null)
			{
				string text = forType.Namespace ?? "";
				if (text.Length > 0)
				{
					text += ".";
				}
				_loggerForTypeString = text + forType.Name + " ";
			}
			else
			{
				_loggerForTypeString = "";
			}
		}

		protected abstract ILogger BuildNewLogger(Type type);

		public ILogger ForType<T>()
		{
			return ForType(typeof(T));
		}

		public ILogger ForType(Type type)
		{
			if (!(type == LoggerForType))
			{
				return BuildNewLogger(type);
			}
			return this;
		}

		protected abstract void Log(LogLevel logLevel, string formattedMessage);

		private string FormatLogEntry(string severityString, string message, params object[] formatArgs)
		{
			string text = string.Format(message, formatArgs);
			string text2 = Clock.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture);
			return severityString + text2 + " " + _loggerForTypeString + text;
		}

		public void Debug(string message, params object[] formatArgs)
		{
			if (IsDebugEnabled)
			{
				Log(LogLevel.Debug, FormatLogEntry("D", message, formatArgs));
			}
		}

		public void Info(string message, params object[] formatArgs)
		{
			if (IsInfoEnabled)
			{
				Log(LogLevel.Info, FormatLogEntry("I", message, formatArgs));
			}
		}

		public void Warning(string message, params object[] formatArgs)
		{
			if (IsWarningEnabled)
			{
				Log(LogLevel.Warning, FormatLogEntry("W", message, formatArgs));
			}
		}

		public void Error(Exception exception, string message, params object[] formatArgs)
		{
			if (IsErrorEnabled)
			{
				Log(LogLevel.Error, string.Format("{0} {1}", FormatLogEntry("E", message, formatArgs), exception));
			}
		}

		public void Error(string message, params object[] formatArgs)
		{
			if (IsErrorEnabled)
			{
				Log(LogLevel.Error, FormatLogEntry("E", message, formatArgs));
			}
		}
	}
}
