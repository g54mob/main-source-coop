using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.RatsHoleEnemy.States
{
	public class RatsHoleEnemyChaseState : StateBase<RatsHoleEnemyStateId>
	{
		private const float PriorityLostGraceSeconds = 0.4f;

		private const float DestinationRetargetXZ = 0.35f;

		private const float DestinationSampleRadius = 1f;

		private readonly RatsHoleEnemy _enemy;

		private readonly RatsHoleEnemyContext _context;

		private readonly INavigationService _navigationService;

		private float _priorityLostTimer;

		private float _unreachableTimer;

		private bool _hasDestination;

		private Vector3 _lastDestination;

		public RatsHoleEnemyChaseState(RatsHoleEnemy enemy, RatsHoleEnemyContext context, INavigationService navigationService)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_navigationService = navigationService;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(RatsHoleEnemyStateId.Chase);
			_context.SetVisualState(RatsHoleEnemyVisualState.Chase);
			_context.MoveSpeedSetupSystem.Enable();
			_context.ResumeMovement();
			_context.TargetSearchRange = _context.AggroRange;
			_priorityLostTimer = 0f;
			_unreachableTimer = 0f;
			_hasDestination = false;
			_lastDestination = default(Vector3);
			_context.FindTargetPlayerPositionResetSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.PlayerDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
		}

		public override void OnExit()
		{
			_context.MoveSpeedSetupSystem.Disable();
			_context.FindTargetPlayerPositionResetSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.PlayerDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			float num = ((_enemy.Runner != null) ? _enemy.Runner.DeltaTime : Time.deltaTime);
			PlayerDataHolder priorityPlayer = _context.PriorityPlayer;
			if (priorityPlayer == null || priorityPlayer.NetworkObject == null)
			{
				_priorityLostTimer += num;
				if (_priorityLostTimer >= 0.4f)
				{
					_enemy.LosePriorityTarget();
				}
				return;
			}
			_priorityLostTimer = 0f;
			if (_enemy.IsPriorityPlayerOutsideGate())
			{
				_enemy.LosePriorityTarget();
				return;
			}
			if (!_enemy.CanInteractWithPriorityPlayer())
			{
				_enemy.LosePriorityTarget();
				return;
			}
			if (_enemy.IsPriorityPlayerInSafeZone())
			{
				_enemy.LoseSafeZonePriorityTargetToPatrolFallback();
				return;
			}
			if (_enemy.IsPriorityPlayerOutsideHomeAggro())
			{
				_enemy.LosePriorityTarget();
				return;
			}
			if (!_enemy.IsPriorityPlayerReachable())
			{
				_unreachableTimer += num;
				if (_unreachableTimer >= 0.4f)
				{
					LoseUnreachablePriorityTarget();
				}
				return;
			}
			if (!TryUpdateStableChaseDestination())
			{
				_unreachableTimer += num;
				if (_unreachableTimer >= 0.4f)
				{
					LoseUnreachablePriorityTarget();
				}
				return;
			}
			_unreachableTimer = 0f;
			Vector3 position = priorityPlayer.NetworkObject.transform.position;
			if (GetHorizontalDistance(_enemy.transform.position, position) <= _context.DistanceToAttack)
			{
				_enemy.TriggerEvent(RatsHoleEnemyEvent.OnInAttackRange);
			}
		}

		private static float GetHorizontalDistance(Vector3 from, Vector3 to)
		{
			Vector3 vector = to - from;
			return new Vector2(vector.x, vector.z).magnitude;
		}

		private void LoseUnreachablePriorityTarget()
		{
			if (_enemy.IsPriorityPlayerInSafeZone())
			{
				_enemy.LoseSafeZonePriorityTargetToPatrolFallback();
			}
			else
			{
				_enemy.LosePriorityTarget();
			}
		}

		private bool TryUpdateStableChaseDestination()
		{
			if (_context.NavMeshAgent == null || !_context.NavMeshAgent.isActiveAndEnabled || !_context.NavMeshAgent.isOnNavMesh)
			{
				return false;
			}
			if (!_enemy.TryGetPriorityPlayerTrackingPosition(out var position))
			{
				return false;
			}
			if (!_navigationService.TryGetCompletePath(_context.NavMeshAgent, position, 1f, out var path) || path.corners == null || path.corners.Length == 0)
			{
				return false;
			}
			Vector3 vector = path.corners[path.corners.Length - 1];
			if (_hasDestination && GetHorizontalDistance(_lastDestination, vector) < 0.35f)
			{
				return true;
			}
			_lastDestination = vector;
			_hasDestination = true;
			_context.SetTargetPosition(vector);
			_context.SetTargetPositionCompleted(isCompleted: false);
			return true;
		}
	}
}
