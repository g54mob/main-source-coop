using System;

namespace Features.LevelModule.Scripts
{
	public class PlayerTransitedToLevelEvent
	{
		public event Action<int> OnPlayerTransitedToLevel;

		public void Invoke(int playerId)
		{
			this.OnPlayerTransitedToLevel?.Invoke(playerId);
		}
	}
}
