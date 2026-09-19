using System.Collections.Generic;
using System.Linq;
using Features.AIModuleStateMachine.Scripts.Core.Contexts;
using Features.AIModuleStateMachine.Scripts.Core.SystemsBehavior;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Fusion;
using NetworkServices.NetworkEvents;
using UnityEngine;
using Zenject;

namespace Features.AIModuleStateMachine.Scripts.Core.Systems.Detection
{
	[NetworkBehaviourWeaved(0)]
	public class TargetPlayerPrioritizeSystem : MonoSystem
	{
		private IMovementContext _movementContext;

		private IDetectionContext _detectionContext;

		private IAttackTimingContext _attackTimingContext;

		[SerializeField]
		private bool _selectByDistance;

		[SerializeField]
		[Range(0f, 1f)]
		private float _attackDistanceRangeFactor;

		private INavigationService _navigationService;

		private PlayerRaycastPointsModel _raycastPointsModel;

		private NetworkRunnerEventBus _eventBus;

		private bool _isEnabled;

		public override bool IsEnabled => _isEnabled;

		[Inject]
		public void InjectDependencies(IMovementContext movementContext, IDetectionContext detectionContext, IAttackTimingContext attackTimingContext, INavigationService navigationService, PlayerRaycastPointsModel raycastPointsModel, NetworkRunnerEventBus eventBus)
		{
			_movementContext = movementContext;
			_detectionContext = detectionContext;
			_attackTimingContext = attackTimingContext;
			_navigationService = navigationService;
			_raycastPointsModel = raycastPointsModel;
			_eventBus = eventBus;
		}

		public override void Spawned()
		{
			base.Spawned();
			_eventBus?.Subscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			_eventBus?.Unsubscribe<OnPlayerLeftEvent>(HandlePlayerLeft);
			base.Despawned(runner, hasState);
		}

		public override void Enable()
		{
			_isEnabled = true;
			_detectionContext.OnDetectedPlayersChanged += FindPriorityTarget;
			FindPriorityTarget();
		}

		public override void Disable()
		{
			_isEnabled = false;
			_detectionContext.OnDetectedPlayersChanged -= FindPriorityTarget;
			Clear();
		}

		public override void Clear()
		{
		}

		private void HandlePlayerLeft(OnPlayerLeftEvent evt)
		{
			if (base.Initialized && base.HasStateAuthority && !IsPriorityStickyValid(_detectionContext.PriorityPlayer))
			{
				_detectionContext.SetPriorityPlayer(null);
				FindPriorityTarget();
			}
		}

		private void FindPriorityTarget()
		{
			if (!base.Initialized || !_isEnabled)
			{
				return;
			}
			if (_detectionContext.DetectedPlayers.Count <= 0)
			{
				_detectionContext.SetPriorityPlayer(null);
			}
			else
			{
				if (IsPriorityStickyValid(_detectionContext.PriorityPlayer))
				{
					return;
				}
				List<PlayerDataHolder> list = new List<PlayerDataHolder>();
				foreach (PlayerDataHolder detectedPlayer in _detectionContext.DetectedPlayers)
				{
					if (!(detectedPlayer?.NetworkObject == null) && detectedPlayer.NetworkObject.IsValid)
					{
						PlayerRef inputAuthority = detectedPlayer.NetworkObject.InputAuthority;
						if (IsPlayerSessionActive(inputAuthority) && IsPlayerReachableForPriority(detectedPlayer, inputAuthority))
						{
							list.Add(detectedPlayer);
						}
					}
				}
				if (list.Count == 0)
				{
					_detectionContext.SetPriorityPlayer(null);
					return;
				}
				PlayerDataHolder priorityPlayer = (_selectByDistance ? list.OrderBy(GetDistanceToPlayerForPrioritization).First() : list[Random.Range(0, list.Count)]);
				_detectionContext.SetPriorityPlayer(priorityPlayer);
			}
		}

		private bool IsPriorityStickyValid(PlayerDataHolder priorityPlayer)
		{
			if (priorityPlayer?.NetworkObject == null || !priorityPlayer.NetworkObject.IsValid)
			{
				return false;
			}
			if (!_detectionContext.DetectedPlayers.Contains(priorityPlayer))
			{
				return false;
			}
			return IsPlayerSessionActive(priorityPlayer.NetworkObject.InputAuthority);
		}

		private bool IsPlayerSessionActive(PlayerRef playerRef)
		{
			if (base.Runner == null || playerRef == PlayerRef.None)
			{
				return false;
			}
			foreach (PlayerRef activePlayer in base.Runner.ActivePlayers)
			{
				if (activePlayer == playerRef)
				{
					return true;
				}
			}
			return false;
		}

		private bool IsPlayerReachableForPriority(PlayerDataHolder player, PlayerRef inputAuthority)
		{
			if (_attackDistanceRangeFactor > 0f)
			{
				return IsPlayerReachableByAttackRange(player, inputAuthority);
			}
			PlayerReachableData reachableData;
			return _navigationService.IsPlayerOnReachablePoint(inputAuthority, _movementContext.NavMeshAgent, out reachableData);
		}

		private bool IsPlayerReachableByAttackRange(PlayerDataHolder player, PlayerRef inputAuthority)
		{
			float distanceToAttack = _attackTimingContext.DistanceToAttack;
			float num = distanceToAttack * _attackDistanceRangeFactor;
			if (distanceToAttack <= 0f || num <= 0f)
			{
				return false;
			}
			if (!_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out var position))
			{
				if (player.NetworkObject == null)
				{
					return false;
				}
				position = player.NetworkObject.transform.position;
			}
			Vector3 availablePosition;
			return _navigationService.HasAvailablePointInRange(_movementContext.NavMeshAgent, position, num, out availablePosition);
		}

		private float GetDistanceToPlayerForPrioritization(PlayerDataHolder player)
		{
			PlayerRef inputAuthority = player.NetworkObject.InputAuthority;
			if (_navigationService.TryGetPlayerTrackingPosition(inputAuthority, out var position))
			{
				return Vector3.Distance(base.transform.position, position);
			}
			return Vector3.Distance(base.transform.position, player.NetworkObject.transform.position);
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
