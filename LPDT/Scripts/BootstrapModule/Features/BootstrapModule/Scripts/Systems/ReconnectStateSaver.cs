using Features.Movement.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.SessionManagementModule.Models;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using UnityEngine;
using Zenject;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class ReconnectStateSaver : ITickable
	{
		private const float SAVE_INTERVAL_SECONDS = 0.2f;

		private readonly ISessionPlayerPresence _sessionPlayerPresence;

		private readonly IReconnectPlacement _reconnectPlacement;

		private readonly SessionStateMachine _sessionStateMachine;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private float _nextSaveTime;

		public ReconnectStateSaver(ISessionPlayerPresence sessionPlayerPresence, IReconnectPlacement reconnectPlacement, SessionStateMachine sessionStateMachine, PlayerMovableModel playerMovableModel, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel)
		{
			_sessionPlayerPresence = sessionPlayerPresence;
			_reconnectPlacement = reconnectPlacement;
			_sessionStateMachine = sessionStateMachine;
			_playerMovableModel = playerMovableModel;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
		}

		public void Tick()
		{
			if (_sessionStateMachine.Current == SessionState.Level && _sessionStateMachine.IsActive && !(Time.time < _nextSaveTime))
			{
				_nextSaveTime = Time.time + 0.2f;
				PlayerCharacterMovableBase localMovable = _playerMovableModel.LocalMovable;
				if (!(localMovable == null))
				{
					Vector3 reconnectPlacementPosition = localMovable.GetReconnectPlacementPosition();
					float yaw = ((localMovable.RotatoblePart != null) ? localMovable.RotatoblePart.eulerAngles.y : 0f);
					bool isCrouching = localMovable.IsHardCrouch();
					float health = ResolveLocalHealth();
					_sessionPlayerPresence.SaveReconnectState(_reconnectPlacement.CurrentLevelId, reconnectPlacementPosition, yaw, isCrouching, health);
				}
			}
		}

		private float ResolveLocalHealth()
		{
			int playerId = _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId;
			if (!_spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				return 0f;
			}
			return value.GetStat(EntityStatType.Health).FullValue;
		}
	}
}
