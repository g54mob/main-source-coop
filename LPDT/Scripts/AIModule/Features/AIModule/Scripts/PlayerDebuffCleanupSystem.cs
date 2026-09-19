using System;
using Features.CameraModelModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using Zenject;

namespace Features.AIModule.Scripts
{
	public class PlayerDebuffCleanupSystem : IInitializable, IDisposable
	{
		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly CameraModel _cameraModel;

		private readonly IPlayerStateService _playerStateService;

		public PlayerDebuffCleanupSystem(PlayersStatesSynchronizer playersStatesSynchronizer, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, CameraModel cameraModel, IPlayerStateService playerStateService)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_cameraModel = cameraModel;
			_playerStateService = playerStateService;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnPlayerStateChanged;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnPlayerStateChanged;
		}

		private void OnPlayerStateChanged(PlayerStateData playerStateData)
		{
			if (!_playerStateService.IsPlayerAlive(playerStateData.PlayerId))
			{
				CleanupPlayerDebuffs(playerStateData.PlayerId);
			}
		}

		private void CleanupPlayerDebuffs(int playerId)
		{
			if (_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				IStat stat = value.GetStat(EntityStatType.WalkSpeed);
				foreach (StatModifier statModifier in stat.StatModifiers)
				{
					if (statModifier.ModifierType == ModifierType.PercentMulti && statModifier.Value < 0f)
					{
						stat.RemoveStatModifierThatEqual(statModifier);
					}
				}
			}
			if (_multiplayerModel.NetworkRunner != null && _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId)
			{
				_cameraModel.ClearSensitivityModifiers();
			}
		}
	}
}
