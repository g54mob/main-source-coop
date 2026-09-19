using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.AIModuleStateMachine.Scripts.Services;
using Features.LevelGatesModule.Data;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Detection
{
	[NetworkBehaviourWeaved(0)]
	public class PlayerDetectingSystem : MonoSystem
	{
		private IMovementContext _movementContext;

		private IDetectionContext _detectionContext;

		private PlayersGatesModelSynchronizedModel _playersGatesModelSynchronizedModel;

		private SpawnedPlayersModel _spawnedPlayersModel;

		private IPlayerStateService _playerStateService;

		private INavigationService _navigationService;

		private IEnemyPlayerAttackabilityService _enemyPlayerAttackabilityService;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(IMovementContext movementContext, IDetectionContext detectionContext, SpawnedPlayersModel spawnedPlayersModel, IPlayerStateService playerStateService, INavigationService navigationService, PlayersGatesModelSynchronizedModel playersGatesModelSynchronizedModel, IEnemyPlayerAttackabilityService enemyPlayerAttackabilityService)
		{
			_movementContext = movementContext;
			_detectionContext = detectionContext;
			_spawnedPlayersModel = spawnedPlayersModel;
			_playerStateService = playerStateService;
			_navigationService = navigationService;
			_playersGatesModelSynchronizedModel = playersGatesModelSynchronizedModel;
			_enemyPlayerAttackabilityService = enemyPlayerAttackabilityService;
		}

		public override void Enable()
		{
			_isEnabled = true;
		}

		public override void Disable()
		{
			_isEnabled = false;
			Clear();
		}

		private void Update()
		{
			if (base.Initialized && _isEnabled)
			{
				DetectPlayersInArea();
			}
		}

		private void DetectPlayersInArea()
		{
			if (_spawnedPlayersModel.Players.Count == 0)
			{
				return;
			}
			List<(PlayerDataHolder, float)> list = new List<(PlayerDataHolder, float)>();
			NavMeshAgent navMeshAgent = _movementContext.NavMeshAgent;
			float targetSearchRange = _detectionContext.TargetSearchRange;
			foreach (KeyValuePair<PlayerRef, PlayerDataHolder> player2 in _spawnedPlayersModel.Players)
			{
				if (_playersGatesModelSynchronizedModel.TryGetPlayerState(player2.Key.PlayerId, out var state) && !state.PlayerInsideGate)
				{
					continue;
				}
				PlayerDataHolder value = player2.Value;
				if (!(value?.NetworkObject == null) && _playerStateService.IsPlayerAlive(player2.Key.PlayerId) && _enemyPlayerAttackabilityService.CanEnemyTargetPlayer(player2.Key.PlayerId))
				{
					PlayerRef inputAuthority = value.NetworkObject.InputAuthority;
					if (!_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out var position))
					{
						position = value.NetworkObject.transform.position;
					}
					float num = Vector3.Distance(navMeshAgent.transform.position, position);
					if (!(num > targetSearchRange) && _navigationService.HasAvailablePointInRange(navMeshAgent, position, _detectionContext.AvailablePointRange, out var _))
					{
						list.Add((value, num));
					}
				}
			}
			foreach (PlayerDataHolder key in _detectionContext.DetectedPlayersDistance.Keys.ToList())
			{
				if (!list.Exists(((PlayerDataHolder holder, float distance) tuple2) => tuple2.holder == key))
				{
					_detectionContext.RemoveDetectedPlayerDistance(key);
				}
			}
			foreach (var (player, distance) in list)
			{
				_detectionContext.SetDetectedPlayerDistance(player, distance);
			}
			_detectionContext.SetDetectedPlayers(list.Select<(PlayerDataHolder, float), PlayerDataHolder>(((PlayerDataHolder holder, float distance) tuple2) => tuple2.holder).ToList());
		}

		public override void Clear()
		{
			_detectionContext.ClearDetectedPlayersDistance();
		}

		[WeaverGenerated]
		public override void CopyBackingFieldsToState(bool P_0)
		{
			base.CopyBackingFieldsToState(P_0);
		}

		[WeaverGenerated]
		public override void CopyStateToBackingFields()
		{
			base.CopyStateToBackingFields();
		}
	}
}
