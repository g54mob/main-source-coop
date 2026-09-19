using System;

namespace Google.Apis.Logging
{
	public interface ILogger
	{
		bool IsDebugEnabled { get; }

		ILogger ForType(Type type);

		ILogger ForType<T>();

		void Debug(string message, params object[] formatArgs);

		void Info(string message, params object[] formatArgs);

		void Warning(string message, params object[] formatArgs);

		void Error(Exception exception, string message, params object[] formatArgs);

		void Error(string message, params object[] formatArgs);
	}
}
