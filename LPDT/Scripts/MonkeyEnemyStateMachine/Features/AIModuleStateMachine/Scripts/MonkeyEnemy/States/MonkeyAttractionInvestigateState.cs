using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyAttractionInvestigateState : StateBase<MonkeyStateId>
	{
		private const int SamplingAttempts = 12;

		private const float RepathInterval = 1.5f;

		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyMovementSettings _movementSettings;

		private float _repathTimer;

		private bool _hasApproachPoint;

		public MonkeyAttractionInvestigateState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.Wandering);
			_enemy.SetCurrentTargetPlayerId(-1);
			_enemy.RaiseMoveSlowAnimation();
			_context.ClearTargetItem();
			_context.EnableMovement();
			_context.Agent.speed = _movementSettings.WanderingSpeed;
			_repathTimer = 0f;
			_hasApproachPoint = false;
			MoveToApproachPoint();
		}

		public override void OnLogic()
		{
			if (!(_context.Agent == null) && _context.Agent.isOnNavMesh)
			{
				if (!_hasApproachPoint)
				{
					RetryApproachPoint();
				}
				else if (!_context.Agent.pathPending && (_context.Agent.pathStatus != NavMeshPathStatus.PathComplete || _context.Agent.remainingDistance <= _context.Agent.stoppingDistance))
				{
					_enemy.TriggerEvent(MonkeyEvent.OnAttractionZoneExited);
				}
			}
		}

		private void RetryApproachPoint()
		{
			_repathTimer += _enemy.GetTickDelta();
			if (!(_repathTimer < 1.5f))
			{
				_repathTimer = 0f;
				MoveToApproachPoint();
			}
		}

		private void MoveToApproachPoint()
		{
			_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
			_context.Agent.speed = _movementSettings.WanderingSpeed;
			if (_context.TryGetAttractionApproachPoint(12, out var point))
			{
				_context.MoveToPosition(point);
				_hasApproachPoint = true;
			}
		}
	}
}
