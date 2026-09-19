using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityEngine.AI;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanAttractionInvestigateState : StateBase<HeadmanStateId>
	{
		private const int SamplingAttempts = 12;

		private const float RepathInterval = 1.5f;

		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanWanderingSettings _settings;

		private float _repathTimer;

		private bool _hasApproachPoint;

		public HeadmanAttractionInvestigateState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanWanderingSettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.Wandering);
			_context.RestoreDefaultAreaMask();
			if (_context.Agent != null)
			{
				_context.Agent.speed = _settings.WanderingSpeed;
			}
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
					_enemy.TriggerEvent(HeadmanEvent.OnAttractionZoneExited);
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
			if (_context.TryGetAttractionApproachPoint(12, out var point))
			{
				_context.MoveToPosition(point);
				_hasApproachPoint = true;
			}
		}
	}
}
