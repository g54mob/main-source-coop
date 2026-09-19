using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;

namespace Features.PlayerSpawner.Scripts
{
	public class PlayerStatsInitializeService : IPlayerStatsInitializeService
	{
		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly PlayerStatsConfiguration _playerStatsConfiguration;

		private readonly MultiplayerModel _multiplayerModel;

		public PlayerStatsInitializeService(SpawnedEntityStatsModel spawnedEntityStatsModel, PlayerStatsConfiguration playerStatsConfiguration, MultiplayerModel multiplayerModel)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_playerStatsConfiguration = playerStatsConfiguration;
			_multiplayerModel = multiplayerModel;
		}

		public void InitializeStats(int playerId, float initialHealth = -1f)
		{
			if (!_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				return;
			}
			InitializeStat(value, EntityStatType.Health);
			InitializeStat(value, EntityStatType.Stamina);
			InitializeStat(value, EntityStatType.Invisibility);
			InitializeStat(value, EntityStatType.HiddenStamina);
			InitializeStat(value, EntityStatType.HandsDistance);
			InitializeStat(value, EntityStatType.MouseHandsDistance);
			InitializeStat(value, EntityStatType.GrabStrength);
			InitializeStat(value, EntityStatType.MaxWeightCapacity);
			value.SynchronizeAllStatsBaseValues(RPCType.InAllWays);
			if (initialHealth >= 0f)
			{
				if (initialHealth <= 0f)
				{
					_multiplayerModel.ReconnectDeathAction?.Invoke(playerId);
				}
				value.GetStat(EntityStatType.Health).OverrideValue(initialHealth);
				value.SynchronizeStatValues(EntityStatType.Health, RPCType.InAllWays);
			}
		}

		public void ApplyReconnectHealthClampedToMax(int playerId)
		{
			if (PlayerSessionPrefs.TryGetSavedHealth(out var health) && _spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				IStat stat = value.GetStat(EntityStatType.Health);
				float value2 = Mathf.Min(health, stat.MaxValue);
				stat.OverrideValue(value2);
				value.SynchronizeStatValues(EntityStatType.Health, RPCType.InAllWays);
			}
		}

		private void InitializeStat(EntityStatEntityNetworkedBase playerStats, EntityStatType entityStatType)
		{
			IStat stat = playerStats.GetStat(entityStatType);
			if (_playerStatsConfiguration.PlayerStats.TryGetValue(entityStatType, out var value))
			{
				stat.MaxValue = value;
				stat.OverrideValue(value);
			}
		}
	}
}
