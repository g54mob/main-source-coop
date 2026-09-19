using System.Collections.Generic;
using Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Settings;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.CoinRobSwarmEnemy.Sensors
{
	public class CoinRobPlayerProximityService
	{
		private readonly CoinRobSwarmEnemySettings _swarmSettings;

		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly IPlayerStateService _playerStateService;

		public CoinRobPlayerProximityService(CoinRobSwarmEnemySettings swarmSettings, SpawnedPlayersModel spawnedPlayersModel, IPlayerStateService playerStateService)
		{
			_swarmSettings = swarmSettings;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerStateService = playerStateService;
		}

		public bool IsWithinPlayerRadius(Vector3 worldPosition)
		{
			float nearPlayerCoinSearchRadius = _swarmSettings.NearPlayerCoinSearchRadius;
			if (nearPlayerCoinSearchRadius <= 0f)
			{
				return true;
			}
			float num = nearPlayerCoinSearchRadius * nearPlayerCoinSearchRadius;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player in _spawnedPlayersModel.Players)
			{
				player.Deconstruct(out var key, out var value);
				PlayerDataHolder playerDataHolder = value;
				if (!(playerDataHolder.NetworkObject == null))
				{
					IPlayerStateService playerStateService = _playerStateService;
					key = playerDataHolder.NetworkObject.StateAuthority;
					if (playerStateService.IsPlayerAlive(key.PlayerId) && (playerDataHolder.NetworkObject.transform.position - worldPosition).sqrMagnitude <= num)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
