using System;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.RagdollModule.Scripts;
using Zenject;

namespace Features.LineArmModule.Scripts
{
	public class UnjoinItemsOnPlayerFallSystem : IInitializable, IDisposable
	{
		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly LineArmsModel _lineArmsModel;

		private readonly PlayersRagdollModel _playersRagdollModel;

		private IRagdollEntity _localRagdoll;

		public UnjoinItemsOnPlayerFallSystem(PlayersStatesSynchronizer playersStatesSynchronizer, MultiplayerModel multiplayerModel, LineArmsModel lineArmsModel, PlayersRagdollModel playersRagdollModel)
		{
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_multiplayerModel = multiplayerModel;
			_lineArmsModel = lineArmsModel;
			_playersRagdollModel = playersRagdollModel;
		}

		public void Initialize()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged += OnSomePlayerStateChanged;
			_playersRagdollModel.OnPlayerRagdollAdded += OnPlayerRagdollAdded;
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (_playersRagdollModel.TryGetPlayerRagdoll(playerId, out var ragdoll))
			{
				SubscribeToRagdoll(ragdoll);
			}
		}

		public void Dispose()
		{
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= OnSomePlayerStateChanged;
			_playersRagdollModel.OnPlayerRagdollAdded -= OnPlayerRagdollAdded;
			if (_localRagdoll != null)
			{
				_localRagdoll.OnSimulationStarted -= OnLocalRagdollSimulationStarted;
			}
		}

		private void OnPlayerRagdollAdded(int playerId, PlayerRagdollEntity ragdoll)
		{
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				SubscribeToRagdoll(ragdoll);
			}
		}

		private void SubscribeToRagdoll(IRagdollEntity ragdoll)
		{
			if (_localRagdoll != null)
			{
				_localRagdoll.OnSimulationStarted -= OnLocalRagdollSimulationStarted;
			}
			_localRagdoll = ragdoll;
			_localRagdoll.OnSimulationStarted += OnLocalRagdollSimulationStarted;
		}

		private void OnLocalRagdollSimulationStarted(IRagdollEntity _)
		{
			UnjoinAll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
		}

		private void OnSomePlayerStateChanged(PlayerStateData playerStateData)
		{
			if (playerStateData.PlayerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				PlayerState playerState = playerStateData.PlayerState;
				if (playerState == PlayerState.Dead || playerState == PlayerState.PreDeadCrouch)
				{
					UnjoinAll(playerStateData.PlayerId);
				}
			}
		}

		private void UnjoinAll(int playerId)
		{
			if (_lineArmsModel.TryGetLineArmForPlayer(playerId, out var lineArm))
			{
				lineArm.UnJoinAll(throwItem: false);
			}
		}
	}
}
