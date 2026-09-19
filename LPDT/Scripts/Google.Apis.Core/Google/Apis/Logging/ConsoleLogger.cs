using System;
using Google.Apis.Util;

namespace Google.Apis.Logging
{
	public sealed class ConsoleLogger : BaseLogger, ILogger
	{
		public bool LogToStdOut { get; }

		public ConsoleLogger(LogLevel minimumLogLevel, bool logToStdOut = false, IClock clock = null)
			: this(minimumLogLevel, logToStdOut, clock, null)
		{
		}

		private ConsoleLogger(LogLevel minimumLogLevel, bool logToStdOut, IClock clock, Type forType)
			: base(minimumLogLevel, clock, forType)
		{
			LogToStdOut = logToStdOut;
		}

		protected override ILogger BuildNewLogger(Type type)
		{
			return new ConsoleLogger(base.MinimumLogLevel, LogToStdOut, base.Clock, type);
		}

		protected override void Log(LogLevel logLevel, string formattedMessage)
		{
			(LogToStdOut ? Console.Out : Console.Error).WriteLine(formattedMessage);
		}
	}
}
