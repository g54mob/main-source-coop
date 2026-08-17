using System;

namespace VisualDesignCafe.Nature
{
	internal class TimerScope : IDisposable
	{
		private readonly ILog _log;

		public long ElapsedMiliseconds => _log?.ElapsedMilliseconds ?? 0;

		public TimerScope(ILog log)
		{
			_log = log;
			_log?.StartTimer();
		}

		public void Dispose()
		{
			_log?.StopTimer();
		}
	}
}
