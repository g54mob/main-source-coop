using System;

namespace Features.DeadPartsModule.Scripts
{
	public class PlayerResurrectionByPlayerEventClass
	{
		public event Action<int, int> OnPlayerResurrectedByPlayer;

		public void InvokePlayerResurrectedByPlayer(int resurrectedPlayerId, int resurrectedByPlayerId)
		{
			this.OnPlayerResurrectedByPlayer?.Invoke(resurrectedPlayerId, resurrectedByPlayerId);
		}
	}
}
