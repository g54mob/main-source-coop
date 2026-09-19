using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Google.Apis.Util;

namespace Google.Apis.Logging
{
	public sealed class MemoryLogger : BaseLogger, ILogger
	{
		private readonly int _maximumEntryCount;

		private readonly List<string> _logEntries;

		public IList<string> LogEntries { get; }

		public MemoryLogger(LogLevel minimumLogLevel, int maximumEntryCount = 1000, IClock clock = null)
			: this(minimumLogLevel, maximumEntryCount, clock, new List<string>(), null)
		{
		}

		private MemoryLogger(LogLevel minimumLogLevel, int maximumEntryCount, IClock clock, List<string> logEntries, Type forType)
			: base(minimumLogLevel, clock, forType)
		{
			_logEntries = logEntries;
			LogEntries = new ReadOnlyCollection<string>(_logEntries);
			_maximumEntryCount = maximumEntryCount;
		}

		protected override ILogger BuildNewLogger(Type type)
		{
			return new MemoryLogger(base.MinimumLogLevel, _maximumEntryCount, base.Clock, _logEntries, type);
		}

		protected override void Log(LogLevel logLevel, string formattedMessage)
		{
			lock (_logEntries)
			{
				if (_logEntries.Count < _maximumEntryCount)
				{
					_logEntries.Add(formattedMessage);
				}
			}
		}
	}
}
