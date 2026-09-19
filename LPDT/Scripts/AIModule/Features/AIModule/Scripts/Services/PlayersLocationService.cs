using Features.PlayerSpawner.Scripts;
using UnityEngine;

namespace Features.AIModule.Scripts.Services
{
	public class PlayersLocationService : IPlayersLocationService
	{
		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		public PlayersLocationService(SpawnedPlayersModel spawnedPlayersModel)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
		}

		public PlayerDataHolder GetLeastCrowdedPlayer(float detectionRadius)
		{
			if (_spawnedPlayersModel.Players.Count == 0)
			{
				return null;
			}
			PlayerDataHolder result = null;
			int num = int.MaxValue;
			foreach (PlayerDataHolder value in _spawnedPlayersModel.Players.Values)
			{
				if (value?.NetworkObject == null)
				{
					continue;
				}
				Vector3 position = value.NetworkObject.transform.position;
				int num2 = 0;
				foreach (PlayerDataHolder value2 in _spawnedPlayersModel.Players.Values)
				{
					if (!(value2?.NetworkObject == null) && value2 != value)
					{
						Vector3 position2 = value2.NetworkObject.transform.position;
						if (Vector3.Distance(position, position2) <= detectionRadius)
						{
							num2++;
						}
					}
				}
				if (num2 < num)
				{
					num = num2;
					result = value;
				}
			}
			return result;
		}
	}
}
