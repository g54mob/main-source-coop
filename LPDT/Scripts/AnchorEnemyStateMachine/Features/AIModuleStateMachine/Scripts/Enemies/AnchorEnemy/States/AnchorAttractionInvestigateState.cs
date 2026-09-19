using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.AnchorEnemy.States
{
	public class AnchorAttractionInvestigateState : StateBase<AnchorStateId>
	{
		private const int SamplingAttempts = 12;

		private const float RepathInterval = 1.5f;

		private readonly AnchorEnemy _enemy;

		private readonly AnchorEnemyContext _context;

		private float _repathTimer;

		private bool _hasApproachPoint;

		public AnchorAttractionInvestigateState(AnchorEnemy enemy, AnchorEnemyContext context)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(AnchorStateId.AttractionInvestigate);
			_context.SetVisualState(AnchorVisualState.AttractionInvestigate);
			_context.AnchorDamageReactionSystem.Enable();
			_context.AnchorMoveSpeedSetupSystem.Enable();
			_context.MoveSystem.Enable();
			_context.TargetPositionCompletedSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.AreaTypeTrackSystem.Enable();
			_context.AnchorVisionDetectingSystem.Enable();
			_context.DetectedPlayersTimeSystem.Enable();
			_context.EnemyDetectionAnalyticsSystem.Enable();
			_context.TargetPlayerPrioritizeSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_repathTimer = 0f;
			_hasApproachPoint = false;
			MoveToApproachPoint();
		}

		public override void OnExit()
		{
			_context.AnchorDamageReactionSystem.Disable();
			_context.AnchorMoveSpeedSetupSystem.Disable();
			_context.MoveSystem.Disable();
			_context.TargetPositionCompletedSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.AreaTypeTrackSystem.Disable();
			_context.AnchorVisionDetectingSystem.Disable();
			_context.DetectedPlayersTimeSystem.Disable();
			_context.EnemyDetectionAnalyticsSystem.Disable();
			_context.TargetPlayerPrioritizeSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.HasLivePriorityPlayer())
			{
				_enemy.TriggerEvent(AnchorEvent.OnTargetAcquired);
			}
			else if (!(_context.NavMeshAgent == null) && _context.NavMeshAgent.isOnNavMesh)
			{
				if (!_hasApproachPoint)
				{
					RetryApproachPoint();
				}
				else if (!_context.NavMeshAgent.pathPending && (_context.NavMeshAgent.pathStatus != NavMeshPathStatus.PathComplete || _context.NavMeshAgent.remainingDistance <= _context.NavMeshAgent.stoppingDistance))
				{
					_enemy.TriggerEvent(AnchorEvent.OnAttractionZoneExited);
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
