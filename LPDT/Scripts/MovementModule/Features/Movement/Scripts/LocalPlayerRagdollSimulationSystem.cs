using System;
using System.Collections.Generic;
using Features.CameraModelModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.RagdollModule.Scripts;
using GameplayEvents;
using UnityEngine;
using Zenject;

namespace Features.Movement.Scripts
{
	public class LocalPlayerRagdollSimulationSystem : IInitializable, ITickable, IDisposable
	{
		private const float STUCK_THRESHOLD_SECONDS = 12f;

		private const float STUCK_REDUMP_INTERVAL_SECONDS = 5f;

		private readonly CameraModel _cameraModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly PlayersRagdollModel _playersRagdollModel;

		private readonly PlayerRagdollSideEffectsConfiguration _configuration;

		private readonly GameplayEventBus _gameplayEventBus;

		private PlayerRagdollEntity _trackedRagdoll;

		private bool _areSideEffectsApplied;

		private float _sideEffectsStartTime;

		private float _lastStuckDumpTime;

		public LocalPlayerRagdollSimulationSystem(CameraModel cameraModel, MultiplayerModel multiplayerModel, PlayerMovableModel playerMovableModel, PlayersRagdollModel playersRagdollModel, PlayerRagdollSideEffectsConfiguration configuration, GameplayEventBus gameplayEventBus)
		{
			_cameraModel = cameraModel;
			_multiplayerModel = multiplayerModel;
			_playerMovableModel = playerMovableModel;
			_playersRagdollModel = playersRagdollModel;
			_configuration = configuration;
			_gameplayEventBus = gameplayEventBus;
		}

		public void Initialize()
		{
			_playersRagdollModel.OnPlayerRagdollAdded += OnPlayerRagdollAdded;
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (_playersRagdollModel.TryGetPlayerRagdoll(playerId, out var ragdoll))
			{
				Track(ragdoll);
			}
		}

		public void Dispose()
		{
			_playersRagdollModel.OnPlayerRagdollAdded -= OnPlayerRagdollAdded;
			Untrack();
		}

		private void OnPlayerRagdollAdded(int playerId, PlayerRagdollEntity ragdoll)
		{
			if (playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				Track(ragdoll);
			}
		}

		private void Track(PlayerRagdollEntity ragdoll)
		{
			if (!(_trackedRagdoll == ragdoll))
			{
				Untrack();
				_trackedRagdoll = ragdoll;
				_trackedRagdoll.OnSimulationStarted += OnSimulationStarted;
				_trackedRagdoll.OnSimulationStopped += OnSimulationStopped;
				_trackedRagdoll.OnRagdollSimulationReasonAdded += OnRagdollSimulationReasonChanged;
				_trackedRagdoll.OnRagdollSimulationReasonRemoved += OnRagdollSimulationReasonChanged;
				ReconcileSideEffects();
			}
		}

		private void Untrack()
		{
			if (!(_trackedRagdoll == null))
			{
				_trackedRagdoll.OnSimulationStarted -= OnSimulationStarted;
				_trackedRagdoll.OnSimulationStopped -= OnSimulationStopped;
				_trackedRagdoll.OnRagdollSimulationReasonAdded -= OnRagdollSimulationReasonChanged;
				_trackedRagdoll.OnRagdollSimulationReasonRemoved -= OnRagdollSimulationReasonChanged;
				_trackedRagdoll = null;
				DiscardSimulationSideEffects();
			}
		}

		private void OnSimulationStarted(IRagdollEntity ragdoll)
		{
			ReconcileSideEffects();
		}

		private void OnSimulationStopped(IRagdollEntity ragdoll)
		{
			ReconcileSideEffects();
		}

		private void OnRagdollSimulationReasonChanged(RagdollSimulationReasonEnum reason)
		{
			ReconcileSideEffects();
		}

		private void ReconcileSideEffects()
		{
			if (ShouldApplySideEffects())
			{
				ApplySimulationSideEffects();
			}
			else
			{
				DiscardSimulationSideEffects();
			}
		}

		private bool ShouldApplySideEffects()
		{
			if (!_trackedRagdoll.IsSimulated || _trackedRagdoll.SimulationReasons.Count == 0)
			{
				return false;
			}
			foreach (RagdollSimulationReasonEnum simulationReason in _trackedRagdoll.SimulationReasons)
			{
				if (IsIgnoredSideEffectReason(simulationReason))
				{
					return false;
				}
			}
			return true;
		}

		private bool IsIgnoredSideEffectReason(RagdollSimulationReasonEnum reason)
		{
			return _configuration.ReasonsToIgnoreSideEffects.Contains(reason);
		}

		private void ApplySimulationSideEffects()
		{
			if (!_areSideEffectsApplied)
			{
				_areSideEffectsApplied = true;
				_sideEffectsStartTime = Time.time;
				_lastStuckDumpTime = Time.time;
				_cameraModel.AddPerlinDisableReason(PerlinDisableReasonEnum.Ragdoll);
				_cameraModel.AddCameraReason(Features.CameraModelModule.CameraType.TPCamera, CameraReasonEnum.Ragdoll);
				_playerMovableModel.AddLockMovementReason(LockMovementReasonEnum.Ragdoll);
				_gameplayEventBus.Publish(new OnPlayerTemporaryRagdollStartedGameplayEvent(LocalPlayerId(), isLocal: true, BuildReasons()));
			}
		}

		private void DiscardSimulationSideEffects()
		{
			if (_areSideEffectsApplied)
			{
				_areSideEffectsApplied = false;
				_playerMovableModel.RemoveLockMovementReason(LockMovementReasonEnum.Ragdoll);
				_cameraModel.RemoveCameraReason(Features.CameraModelModule.CameraType.TPCamera, CameraReasonEnum.Ragdoll);
				_cameraModel.RemovePerlinDisableReason(PerlinDisableReasonEnum.Ragdoll);
				_gameplayEventBus.Publish(new OnPlayerTemporaryRagdollRecoveredGameplayEvent(LocalPlayerId(), isLocal: true, Time.time - _sideEffectsStartTime));
			}
		}

		public void Tick()
		{
			if (_areSideEffectsApplied && !(_trackedRagdoll == null))
			{
				float num = Time.time - _sideEffectsStartTime;
				if (!(num < 12f) && !(Time.time - _lastStuckDumpTime < 5f))
				{
					_lastStuckDumpTime = Time.time;
					_gameplayEventBus.Publish(new OnPlayerTemporaryRagdollStuckGameplayEvent(LocalPlayerId(), isLocal: true, BuildReasons(), num));
				}
			}
		}

		private int LocalPlayerId()
		{
			return _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
		}

		private string BuildReasons()
		{
			if (_trackedRagdoll == null)
			{
				return string.Empty;
			}
			List<string> list = new List<string>(_trackedRagdoll.SimulationReasons.Count);
			foreach (RagdollSimulationReasonEnum simulationReason in _trackedRagdoll.SimulationReasons)
			{
				list.Add(simulationReason.ToString());
			}
			return string.Join(",", list);
		}
	}
}
