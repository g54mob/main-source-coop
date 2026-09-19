using System;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.QuotaModule.Scripts;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.LevelModule.Scripts.LevelTransition
{
	public class TimerSystem : IInitializable, IDisposable
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly LevelModel _levelModel;

		private readonly LevelTransitionTimerSynchronizedModel _levelTransitionTimerSynchronizedModel;

		private readonly LevelTransitionConfiguration _levelTransitionConfiguration;

		private readonly IGameUpdater _gameUpdater;

		private readonly QuotaCompletionModel _quotaCompletionModel;

		private readonly BeachOccupancyModel _beachOccupancyModel;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private readonly ILevelTransitionBeachReadiness _levelTransitionBeachReadiness;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private float _lastTimerTick;

		public TimerSystem(MultiplayerModel multiplayerModel, LevelModel levelModel, LevelTransitionTimerSynchronizedModel levelTransitionTimerSynchronizedModel, LevelTransitionConfiguration levelTransitionConfiguration, IGameUpdater gameUpdater, QuotaCompletionModel quotaCompletionModel, BeachOccupancyModel beachOccupancyModel, NetworkRunnerEventBus networkRunnerEventBus, ILevelTransitionBeachReadiness levelTransitionBeachReadiness, PlayersStatesSynchronizer playersStatesSynchronizer)
		{
			_multiplayerModel = multiplayerModel;
			_levelModel = levelModel;
			_levelTransitionTimerSynchronizedModel = levelTransitionTimerSynchronizedModel;
			_levelTransitionConfiguration = levelTransitionConfiguration;
			_gameUpdater = gameUpdater;
			_quotaCompletionModel = quotaCompletionModel;
			_beachOccupancyModel = beachOccupancyModel;
			_networkRunnerEventBus = networkRunnerEventBus;
			_levelTransitionBeachReadiness = levelTransitionBeachReadiness;
			_playersStatesSynchronizer = playersStatesSynchronizer;
		}

		public void Initialize()
		{
			_levelModel.OnLevelLoaded += InitializeQuota;
			_gameUpdater.OnUpdate += IncreaseTimer;
			_quotaCompletionModel.OnBellActivated += StartTimer;
			_beachOccupancyModel.OnPlayerAdded += OnBeachPlayerAdded;
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnPlayerStateChanged;
			_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeft);
		}

		public void Dispose()
		{
			_levelModel.OnLevelLoaded -= InitializeQuota;
			_gameUpdater.OnUpdate -= IncreaseTimer;
			_quotaCompletionModel.OnBellActivated -= StartTimer;
			_beachOccupancyModel.OnPlayerAdded -= OnBeachPlayerAdded;
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnPlayerStateChanged;
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeft);
		}

		private void InitializeQuota(LevelType levelType)
		{
			_beachOccupancyModel.Players.Clear();
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_levelTransitionTimerSynchronizedModel.Initialize(0f, _levelTransitionConfiguration.MaxTimer);
			}
		}

		private void IncreaseTimer()
		{
			if (!_levelTransitionTimerSynchronizedModel.IsTimerRunning || !_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				return;
			}
			_lastTimerTick += Time.deltaTime;
			if (_lastTimerTick >= 1f)
			{
				_levelTransitionTimerSynchronizedModel.CurrentTimer += 1f;
				_levelTransitionTimerSynchronizedModel.Synchronize();
				_lastTimerTick -= 1f;
				if (_levelTransitionTimerSynchronizedModel.CurrentTimer >= _levelTransitionConfiguration.MaxTimer)
				{
					_levelTransitionTimerSynchronizedModel.IsTimerRunning = false;
					_levelTransitionTimerSynchronizedModel.Synchronize();
				}
			}
		}

		private void StartTimer(bool isBellActivated)
		{
			if (isBellActivated && _multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_lastTimerTick = 0f;
				_levelTransitionTimerSynchronizedModel.Initialize(0f, _levelTransitionConfiguration.MaxTimer);
				if (_levelTransitionBeachReadiness.AreAllActivePlayersInBeach())
				{
					CompleteTransitionInstantly();
					return;
				}
				_levelTransitionTimerSynchronizedModel.IsTimerRunning = true;
				_levelTransitionTimerSynchronizedModel.Synchronize();
			}
		}

		private void OnBeachPlayerAdded()
		{
			TryCompleteTransitionForBeachState();
		}

		private void OnPlayerStateChanged(PlayerStateData _)
		{
			TryCompleteTransitionForBeachState();
		}

		private void OnPlayerLeft(OnPlayerLeftEvent eventData)
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient)
			{
				_beachOccupancyModel.Players.Remove(eventData.Player);
				TryCompleteTransitionForBeachState();
			}
		}

		private void TryCompleteTransitionForBeachState()
		{
			if (_multiplayerModel.NetworkRunner.IsSharedModeMasterClient && _levelTransitionTimerSynchronizedModel.IsTimerRunning && _quotaCompletionModel.IsBellActivated.Value && _levelTransitionBeachReadiness.AreAllActivePlayersInBeach())
			{
				CompleteTransitionInstantly();
			}
		}

		private void CompleteTransitionInstantly()
		{
			_levelTransitionTimerSynchronizedModel.IsTimerRunning = false;
			_levelTransitionTimerSynchronizedModel.CurrentTimer = _levelTransitionConfiguration.MaxTimer;
			_levelTransitionTimerSynchronizedModel.Synchronize();
		}
	}
}
