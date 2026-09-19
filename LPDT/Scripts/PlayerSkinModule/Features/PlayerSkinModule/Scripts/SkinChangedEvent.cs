using System;

namespace Features.PlayerSkinModule.Scripts
{
	public class SkinChangedEvent
	{
		public event Action<int> OnSkinChanged;

		public void Invoke(int playerId)
		{
			this.OnSkinChanged?.Invoke(playerId);
		}
	}
}
