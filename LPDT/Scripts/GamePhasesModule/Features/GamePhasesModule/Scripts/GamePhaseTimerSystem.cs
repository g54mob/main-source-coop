using System;
using Features.ExtendedLogger.Scripts;
using Features.GamePhasesModule.Scripts.Data;
using Features.GameUpdaterModule;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.NavigationModule.Scripts;
using Features.QuotaModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.GamePhasesModule.Scripts
{
	public class GamePhaseTimerSystem : IInitializable, IDisposable
	{
		private readonly GamePhasesModel _gamePhasesModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly IGamePhaseService _gamePhaseService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly INavigationService _navigationService;

		private readonly PlayerMovableModel _playerMovableModel;

		public GamePhaseTimerSystem(GamePhasesModel gamePhasesModel, IGameUpdater gameUpdater, IGamePhaseService gamePhaseService, MultiplayerModel multiplayerModel, QuotaSynchronizedModel quotaSynchronizedModel, SpawnedEntityStatsModel spawnedEntityStatsModel, INavigationService navigationService, PlayerMovableModel playerMovableModel)
		{
			_gamePhasesModel = gamePhasesModel;
			_gameUpdater = gameUpdater;
			_gamePhaseService = gamePhaseService;
			_multiplayerModel = multiplayerModel;
			_quotaSynchronizedModel = quotaSynchronizedModel;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_navigationService = navigationService;
			_playerMovableModel = playerMovableModel;
		}

		public void Initialize()
		{
			_gameUpdater.OnUpdate += UpdateTime;
			_quotaSynchronizedModel.OnQuotaChanged += CheckQuotaToSubtractTimer;
			_spawnedEntityStatsModel.OnPlayerStatRegistered += SubscribeOnDeath;
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= UpdateTime;
			_quotaSynchronizedModel.OnQuotaChanged -= CheckQuotaToSubtractTimer;
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= SubscribeOnDeath;
			foreach (EntityStatEntityNetworkedBase value in _spawnedEntityStatsModel.PlayerStats.Values)
			{
				value.OnReachedMinValue -= SubtractQuotaByDeath;
			}
		}

		private void UpdateTime()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient && _gamePhasesModel.IsActive && _gamePhasesModel.IsSomePlayerMovedFromSpawn)
			{
				_gamePhaseService.SubtractTime(Time.deltaTime * _gamePhasesModel.TimeMultiplier);
				CheckHiddenPlayers();
			}
		}

		private void CheckHiddenPlayers()
		{
			if (_gamePhasesModel.GamePhasesData == null)
			{
				return;
			}
			int num = 0;
			foreach (PlayerCharacterMovableBase value in _playerMovableModel.AllCharacterMovables.Values)
			{
				if (IsPlayerHidden(value.FootPoint.position))
				{
					num++;
				}
			}
			_gamePhaseService.ChangeSubtractByTimeMultiplier(1f + (float)num * _gamePhasesModel.GamePhasesData.TimerMultiplierByHiddenPlayer);
		}

		private bool IsPlayerHidden(Vector3 playerPosition)
		{
			if (!_navigationService.IsPointOnNavMeshProjected(playerPosition, out var hit))
			{
				return true;
			}
			int num = 1 << NavMesh.GetAreaFromName("LowCrouching");
			int num2 = 1 << NavMesh.GetAreaFromName("MiddleCrouching");
			int num3 = 1 << NavMesh.GetAreaFromName("HighCrouching");
			if ((hit.mask & num) == 0 && (hit.mask & num2) == 0)
			{
				return (hit.mask & num3) != 0;
			}
			return true;
		}

		private void CheckQuotaToSubtractTimer(float current, float max)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient && _gamePhasesModel.IsActive && _gamePhasesModel.GamePhasesData != null && (current - _gamePhasesModel.QuotaOnStartPhase) / max * 100f >= _gamePhasesModel.GamePhasesData.QuotaPercentForSubtract.QuotaPercentForSubtract)
			{
				_gamePhaseService.SubtractTime(_gamePhasesModel.GamePhasesData.QuotaPercentForSubtract.SubtractTime);
				_gamePhasesModel.QuotaOnStartPhase = current;
				ExtendedDebug.LogFiltered(DebugFilterType.EnemiesSpawn, $"\ud83e\ude99 Subtract phase timer by quota percent: <b>{_gamePhasesModel.GamePhasesData.QuotaPercentForSubtract.QuotaPercentForSubtract}</b>. Subtracted value: <b>{_gamePhasesModel.GamePhasesData.QuotaPercentForSubtract.SubtractTime}</b>, new timer value: <b>{_gamePhasesModel.CurrentPhaseTime}</b>. New start quota: <b>{_gamePhasesModel.QuotaOnStartPhase}</b>");
			}
		}

		private void SubscribeOnDeath(int playerId)
		{
			_spawnedEntityStatsModel.PlayerStats[playerId].OnReachedMinValue += SubtractQuotaByDeath;
		}

		private void SubtractQuotaByDeath(EntityStatType entityStatType)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient && _gamePhasesModel.IsActive && _gamePhasesModel.GamePhasesData != null && entityStatType == EntityStatType.Health)
			{
				_gamePhaseService.SubtractTime(_gamePhasesModel.GamePhasesData.TimerSubtractByDeath);
				ExtendedDebug.LogFiltered(DebugFilterType.EnemiesSpawn, $"❤\ufe0f Subtract phase timer by player death. Subtracted value: <b>{_gamePhasesModel.GamePhasesData.TimerSubtractByDeath}</b>, new timer value: <b>{_gamePhasesModel.CurrentPhaseTime}</b>");
			}
		}
	}
}
