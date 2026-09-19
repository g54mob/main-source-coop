using System;

namespace Features.GameCycle.Scripts.SessionCleanup
{
	public class SessionCleanUpStartedEvent
	{
		public event Action OnSessionCleanUpStarted;

		public void Publish()
		{
			this.OnSessionCleanUpStarted?.Invoke();
		}
	}
}
