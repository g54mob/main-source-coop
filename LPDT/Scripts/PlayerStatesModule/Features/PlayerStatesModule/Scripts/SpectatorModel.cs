using System;

namespace Features.PlayerStatesModule.Scripts
{
	public class SpectatorModel
	{
		public int CurrentSpectatablePlayer;

		public event Action OnSpectatableChanged;

		public void InvokeSpectatableChanged()
		{
			this.OnSpectatableChanged?.Invoke();
		}
	}
}
