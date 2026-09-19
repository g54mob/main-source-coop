using System;
using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;

namespace Features.StatsUsageModule.Scripts.StatsData
{
	public class SpawnedEntityStatsModel : ISessionCleanup
	{
		private readonly Dictionary<int, EntityStatEntityNetworkedBase> _playerStats = new Dictionary<int, EntityStatEntityNetworkedBase>();

		public IReadOnlyDictionary<int, EntityStatEntityNetworkedBase> PlayerStats => _playerStats;

		public event Action<int> OnPlayerStatRegistered;

		public void RegisterPlayerStats(int playerRef, EntityStatEntityNetworkedBase statEntity)
		{
			_playerStats[playerRef] = statEntity;
			this.OnPlayerStatRegistered?.Invoke(playerRef);
		}

		public void UnregisterPlayerStats(int statRef)
		{
			_playerStats.Remove(statRef);
		}

		public void UnregisterPlayerStats(int statRef, EntityStatEntityNetworkedBase statEntity)
		{
			if (_playerStats.TryGetValue(statRef, out var value) && value == statEntity)
			{
				_playerStats.Remove(statRef);
			}
		}

		public void Cleanup()
		{
			_playerStats.Clear();
			this.OnPlayerStatRegistered = null;
		}
	}
}
