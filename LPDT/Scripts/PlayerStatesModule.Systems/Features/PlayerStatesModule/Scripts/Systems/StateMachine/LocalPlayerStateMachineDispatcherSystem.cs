using System;
using System.Collections.Generic;
using System.Linq;
using Features.CameraModelModule;
using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.PlayerStatesModule.Scripts.Systems.StateMachine
{
	public class LocalPlayerStateMachineDispatcherSystem : IInitializable, IDisposable
	{
		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LocalPlayerStateMachine _localPlayerStateMachine;

		private readonly SpectatorModel _spectatorModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly CameraModel _cameraModel;

		private readonly ICameraSpectatorFollowService _cameraSpectatorFollowService;

		private readonly IReadOnlyList<LocalPlayerStateBase> _states;

		public LocalPlayerStateMachineDispatcherSystem(PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, LocalPlayerStateMachine localPlayerStateMachine, SpectatorModel spectatorModel, PlayerMovableModel playerMovableModel, CameraModel cameraModel, ICameraSpectatorFollowService cameraSpectatorFollowService, List<LocalPlayerStateBase> localPlayerStates)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_localPlayerStateMachine = localPlayerStateMachine;
			_spectatorModel = spectatorModel;
			_playerMovableModel = playerMovableModel;
			_cameraModel = cameraModel;
			_cameraSpectatorFollowService = cameraSpectatorFollowService;
			_states = localPlayerStates;
		}

		public void Initialize()
		{
			_localPlayerStateMachine.Initialize(_states);
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerStateChanged;
			_spectatorModel.OnSpectatableChanged += OnSpectatableChanged;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerStateChanged;
			_spectatorModel.OnSpectatableChanged -= OnSpectatableChanged;
			_localPlayerStateMachine.Dispose();
		}

		private void OnSomePlayerStateChanged(PlayerStateData data)
		{
			if (data.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && data.PlayerState != PlayerState.None)
			{
				_localPlayerStateMachine.EnterByEnum(data.PlayerState);
			}
		}

		private void OnSpectatableChanged()
		{
			PlayerRef playerRef = _multiplayerModel.NetworkRunner.ActivePlayers.ElementAt(_spectatorModel.CurrentSpectatablePlayer);
			if (_playerMovableModel.AllCharacterMovables.TryGetValue(playerRef, out var value))
			{
				Transform transform = value.CameraPositionTransform;
				if (playerRef == _multiplayerModel.NetworkRunner.LocalPlayer)
				{
					_cameraSpectatorFollowService.End();
				}
				else
				{
					transform = _cameraSpectatorFollowService.Begin(transform);
				}
				_cameraModel.Cameras[Features.CameraModelModule.CameraType.FPCamera].SetTrackingTarget(transform, forceUpdate: true);
				_cameraModel.Cameras[Features.CameraModelModule.CameraType.TPCamera].SetTrackingTarget(transform, forceUpdate: true);
			}
		}
	}
}
