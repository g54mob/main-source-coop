using System;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.PlayersPingModule.Scripts
{
	public class PlayersPingsSystem : IInitializable, IDisposable
	{
		private const float PINGS_UPDATE_DELAY = 5f;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly IGameUpdater _gameUpdater;

		private readonly PlayersPingModel _playersPingModel;

		private readonly NetworkRunnerEventBus _networkRunnerEventBus;

		private float _pingTimerValue;

		public PlayersPingsSystem(MultiplayerModel multiplayerModel, IGameUpdater gameUpdater, PlayersPingModel playersPingModel, NetworkRunnerEventBus networkRunnerEventBus)
		{
			_multiplayerModel = multiplayerModel;
			_gameUpdater = gameUpdater;
			_playersPingModel = playersPingModel;
			_networkRunnerEventBus = networkRunnerEventBus;
		}

		public void Initialize()
		{
			_pingTimerValue = 5f;
			_gameUpdater.OnUpdate += ProcessPings;
			_networkRunnerEventBus.Subscribe<OnPlayerLeftEvent>(OnPlayerLeftHandler);
		}

		public void Dispose()
		{
			_gameUpdater.OnUpdate -= ProcessPings;
			_networkRunnerEventBus.Unsubscribe<OnPlayerLeftEvent>(OnPlayerLeftHandler);
		}

		private void ProcessPings()
		{
			if (!(_multiplayerModel.NetworkRunner == null) && !(_multiplayerModel.NetworkRunner.LocalPlayer == PlayerRef.None))
			{
				_pingTimerValue += Time.deltaTime;
				if (!(_pingTimerValue < 5f))
				{
					_pingTimerValue = 0f;
					double playerRtt = _multiplayerModel.NetworkRunner.GetPlayerRtt(_multiplayerModel.NetworkRunner.LocalPlayer);
					_playersPingModel.SetNewValue(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, playerRtt * 1000.0);
					_playersPingModel.Synchronize();
				}
			}
		}

		private void OnPlayerLeftHandler(OnPlayerLeftEvent eventData)
		{
			if (!(eventData.Player == eventData.Runner.LocalPlayer))
			{
				_playersPingModel.RemovePlayer(eventData.Player.PlayerId);
			}
		}
	}
}
