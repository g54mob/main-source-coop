using System;

namespace Features.GameCycle.Scripts.SessionCleanup
{
	public class SessionCleanupEvent
	{
		public event Action OnSessionCleanup;

		public void Publish()
		{
			this.OnSessionCleanup?.Invoke();
		}
	}
}
