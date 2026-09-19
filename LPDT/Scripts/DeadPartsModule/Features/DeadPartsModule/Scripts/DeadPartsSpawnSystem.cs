using System;
using Features.GameCycle.Scripts.SessionCleanup;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Zenject;

namespace Features.DeadPartsModule.Scripts
{
	public class DeadPartsSpawnSystem : IInitializable, IDisposable, ISessionCleanup
	{
		private readonly IPlayerDeadPartSpawnService _playerDeadPartSpawnService;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayerDeadPartModel _playerDeadPartModel;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private bool _isDeadInSession;

		private bool _hasBeenAliveInSession;

		public DeadPartsSpawnSystem(IPlayerDeadPartSpawnService playerDeadPartSpawnService, MultiplayerModel multiplayerModel, PlayerDeadPartModel playerDeadPartModel, PlayersStatesSynchronizer playersStatesSynchronizer)
		{
			_playerDeadPartSpawnService = playerDeadPartSpawnService;
			_multiplayerModel = multiplayerModel;
			_playerDeadPartModel = playerDeadPartModel;
			_playersStatesSynchronizer = playersStatesSynchronizer;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerStateChanged;
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerStateChanged;
		}

		private void OnSomePlayerStateChanged(PlayerStateData data)
		{
			if (data.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				if (data.PlayerState == PlayerState.PreDeadCrouch)
				{
					HandleLocalPlayerLethalDown();
				}
				else if (data.PlayerState == PlayerState.Dead)
				{
					HandleLocalPlayerDeadRestore();
				}
				else if (data.PlayerState == PlayerState.Alive)
				{
					HandleLocalPlayerAlive();
				}
			}
		}

		private void HandleLocalPlayerLethalDown()
		{
			_isDeadInSession = true;
			if (_hasBeenAliveInSession)
			{
				_playerDeadPartSpawnService.SpawnPlayerDeadPartForCurrentPlayer();
				_playerDeadPartModel.PlayerAlivePart.DeadPartJoinBoosterBehaviour.DisableDeadPartEffect();
			}
		}

		private void HandleLocalPlayerDeadRestore()
		{
			_isDeadInSession = true;
		}

		private void HandleLocalPlayerAlive()
		{
			_isDeadInSession = false;
			_hasBeenAliveInSession = true;
			PlayerSessionPrefs.SetDeadPartSpawned(isSpawned: false);
		}

		public void Cleanup()
		{
			_isDeadInSession = false;
			_hasBeenAliveInSession = false;
		}
	}
}
