using System;
using System.Collections.Generic;

namespace Features.StoreModule.Scripts
{
	public class SpawnedRewardsModel
	{
		private readonly List<SpawnedReward> _spawnedRewards = new List<SpawnedReward>();

		public IReadOnlyList<SpawnedReward> SpawnedRewards => _spawnedRewards;

		public event Action<SpawnedReward> OnRewardSpawned;

		public event Action OnAllRewardsSpawned;

		public void AddSpawnedReward(SpawnedReward spawnedReward)
		{
			_spawnedRewards.Add(spawnedReward);
			this.OnRewardSpawned?.Invoke(spawnedReward);
		}

		public void ClearSpawnedRewards()
		{
			_spawnedRewards.Clear();
		}

		public void InvokeOnAllRewardsSpawned()
		{
			this.OnAllRewardsSpawned?.Invoke();
		}
	}
}
