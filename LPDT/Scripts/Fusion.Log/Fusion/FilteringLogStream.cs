using System;

namespace Fusion
{
	public class FilteringLogStream : LogStream
	{
		private readonly string _filter;

		private readonly LogStream _actualLogStream;

		public FilteringLogStream(string filter, LogStream actualLogStream)
		{
			_filter = filter;
			_actualLogStream = actualLogStream;
		}

		public override void Log(string message)
		{
			if (message == null || !message.Contains(_filter, StringComparison.Ordinal))
			{
				_actualLogStream.Log(message);
			}
		}

		public override void Log(string message, Exception error)
		{
			if (message == null || !message.Contains(_filter, StringComparison.Ordinal))
			{
				_actualLogStream.Log(message, error);
			}
		}

		public override void Log(Exception error)
		{
			_actualLogStream.Log(error);
		}

		public override void Log(ILogSource source, string message)
		{
			if (message == null || !message.Contains(_filter, StringComparison.Ordinal))
			{
				_actualLogStream.Log(source, message);
			}
		}

		public override void Log(ILogSource source, string message, Exception error)
		{
			if (message == null || !message.Contains(_filter, StringComparison.Ordinal))
			{
				_actualLogStream.Log(source, message, error);
			}
		}
	}
}
