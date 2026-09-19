using System;
using System.Collections.Generic;

namespace Features.DeadPartsModule.Scripts
{
	public class PlayerResurrectionModel
	{
		private readonly List<int> _playersAvailableToResurrection = new List<int>();

		public List<int> PlayersAvailableToResurrection => _playersAvailableToResurrection;

		public event Action OnResurrectionAvailablePlayerChanged;

		public void AddResurrectionAvailablePlayer(int player)
		{
			if (!_playersAvailableToResurrection.Contains(player))
			{
				_playersAvailableToResurrection.Add(player);
				this.OnResurrectionAvailablePlayerChanged?.Invoke();
			}
		}

		public void RemoveResurrectionAvailablePlayer(int player)
		{
			if (_playersAvailableToResurrection.Contains(player))
			{
				_playersAvailableToResurrection.Remove(player);
				this.OnResurrectionAvailablePlayerChanged?.Invoke();
			}
		}
	}
}
