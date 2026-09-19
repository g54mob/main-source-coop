using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.CrabEnemy.States
{
	public class CrabAttractionInvestigateState : StateBase<CrabStateId>
	{
		private const int SamplingAttempts = 12;

		private const float RepathInterval = 1.5f;

		private readonly CrabEnemy _enemy;

		private readonly CrabEnemyContext _context;

		private float _repathTimer;

		private bool _hasApproachPoint;

		public CrabAttractionInvestigateState(CrabEnemy enemy, CrabEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(CrabStateId.AttractionInvestigate);
			_context.SetVisualState(CrabVisualState.Locomotion);
			_context.CrabDamageAggrSystem.Enable();
			_context.CrabMoveSpeedSetupSystem.Enable();
			_context.CrabBurstMovementSystem.Enable();
			_context.CrabTurnSpeedSystem.Enable();
			_context.CrabSearchRangeSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.CrabVisionDetectingSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.AttackCooldownSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_repathTimer = 0f;
			_hasApproachPoint = false;
			MoveToApproachPoint();
		}

		public override void OnExit()
		{
			_context.CrabMoveSpeedSetupSystem.Disable();
			_context.CrabBurstMovementSystem.Disable();
			_context.CrabTurnSpeedSystem.Disable();
			_context.CrabSearchRangeSetupSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.CrabVisionDetectingSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.AttackCooldownSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.AttackCooldown <= 0f && _context.CanEnemyTargetPriorityPlayer())
			{
				_enemy.TriggerEvent(CrabEvent.OnTargetAcquired);
			}
			else if (!(_context.NavMeshAgent == null) && _context.NavMeshAgent.isOnNavMesh)
			{
				if (!_hasApproachPoint)
				{
					RetryApproachPoint();
				}
				else if (!_context.NavMeshAgent.pathPending && (_context.NavMeshAgent.pathStatus != NavMeshPathStatus.PathComplete || _context.NavMeshAgent.remainingDistance <= _context.NavMeshAgent.stoppingDistance))
				{
					_enemy.TriggerEvent(CrabEvent.OnAttractionZoneExited);
				}
			}
		}

		private void RetryApproachPoint()
		{
			_repathTimer += GetDeltaTime();
			if (!(_repathTimer < 1.5f))
			{
				_repathTimer = 0f;
				MoveToApproachPoint();
			}
		}

		private void MoveToApproachPoint()
		{
			if (_context.TryGetAttractionApproachPoint(12, out var point))
			{
				_context.SetTargetPosition(point);
				_context.SetTargetPositionCompleted(isCompleted: false);
				_hasApproachPoint = true;
			}
		}

		private float GetDeltaTime()
		{
			if (!(_enemy.Runner != null))
			{
				return Time.deltaTime;
			}
			return _enemy.Runner.DeltaTime;
		}
	}
}
