using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanWanderingState : StateBase<HeadmanStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanWanderingSettings _settings;

		private readonly HeadmanChasingSettings _chasingSettings;

		public HeadmanWanderingState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanWanderingSettings settings, HeadmanChasingSettings chasingSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
			_chasingSettings = chasingSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.Wandering);
			_context.ClearAuxTarget();
			_context.RestoreDefaultAreaMask();
			_context.WanderingTimer = _context.TargetPositionUpdateFrequency;
			_context.ChangeLocationTimer = 0f;
			_context.RageInteractedTimer = 0f;
			_context.FearDestinationTimer = _settings.ChangeLocationUpdateFrequency;
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			if (_context.TryAcquireAuxTarget(tickDelta))
			{
				_enemy.TriggerEvent(HeadmanEvent.OnTargetAcquired);
				return;
			}
			_context.WanderingTimer += tickDelta;
			_context.ChangeLocationTimer += tickDelta;
			_context.AttackTimer += tickDelta;
			if (_context.WanderingTimer >= _context.TargetPositionUpdateFrequency)
			{
				if (_context.Agent != null)
				{
					_context.Agent.speed = _settings.WanderingSpeed;
					_context.MoveToPosition(_context.GetRandomNavmeshPosition(_settings.WanderRadius));
				}
				_context.WanderingTimer = 0f;
				_context.UpdatePositionUpdateFrequency();
			}
			if (_context.ChangeLocationTimer >= _settings.ChangeLocationUpdateFrequency)
			{
				_enemy.RaiseChangeAreaTriggered(_context.transform.position);
				_context.ChangeLocationTimer = 0f;
			}
		}
	}
}
