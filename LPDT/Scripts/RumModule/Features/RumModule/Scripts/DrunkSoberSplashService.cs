using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.PostProcessingModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.SynchronizedModelsModule.Scripts.JsonModelSynchronizer;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;

namespace Features.RumModule.Scripts
{
	public class DrunkSoberSplashService : IDrunkSoberSplashService
	{
		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly CameraWaterLensModel _cameraWaterLensModel;

		private readonly DrunkennessConfiguration _configuration;

		public DrunkSoberSplashService(SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, CameraWaterLensModel cameraWaterLensModel, DrunkennessConfiguration configuration)
		{
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_cameraWaterLensModel = cameraWaterLensModel;
			_configuration = configuration;
		}

		public void ApplySoberSplash(float wetnessPulse = 1f)
		{
			if (!(_multiplayerModel?.NetworkRunner == null))
			{
				int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
				ClearDrunkennessForPlayers(new int[1] { playerId });
				ApplyLocalWetnessPulse(wetnessPulse);
			}
		}

		public void ClearDrunkennessForPlayers(IReadOnlyList<int> playerIds)
		{
			if (playerIds != null && playerIds.Count != 0)
			{
				for (int i = 0; i < playerIds.Count; i++)
				{
					ClearDrunkennessForPlayer(playerIds[i]);
				}
			}
		}

		public void ApplyLocalWetnessPulse(float wetnessPulse = 1f)
		{
			_cameraWaterLensModel?.AddWetness(Mathf.Clamp01(wetnessPulse));
		}

		private void ClearDrunkennessForPlayer(int playerId)
		{
			if (!_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				return;
			}
			IStat stat = value.GetStat(EntityStatType.Drunkenness);
			if (stat == null)
			{
				return;
			}
			float fullValue = stat.FullValue;
			if (!(fullValue <= 0f))
			{
				stat.MinValue = 0f;
				if (_configuration != null)
				{
					stat.MaxValue = Mathf.Max(stat.MaxValue, _configuration.MaxDrunkenness);
				}
				value.SynchronizeStatValues(EntityStatType.Drunkenness, RPCType.InAllWays);
				value.AddStatValueSynchronized(EntityStatType.Drunkenness, 0f - fullValue, RPCType.InAllWays);
			}
		}
	}
}
