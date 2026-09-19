using System;
using System.Collections.Generic;
using Fusion;

namespace Features.LevelModule.Scripts.LevelTransition
{
	public class BeachOccupancyModel
	{
		public readonly Dictionary<PlayerRef, LevelTransitTrigger> Players = new Dictionary<PlayerRef, LevelTransitTrigger>();

		public event Action OnPlayerAdded;

		public void AddPlayer(PlayerRef player, LevelTransitTrigger levelTransitTrigger)
		{
			if (Players.TryAdd(player, levelTransitTrigger))
			{
				this.OnPlayerAdded?.Invoke();
			}
		}
	}
}
