using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanSlowedState : StateBase<HeadmanStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanSlowedSettings _settings;

		public HeadmanSlowedState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanSlowedSettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.Slowed);
			_context.SlowedTimer = _settings.SlowedDuration;
			_context.RestoreDefaultAreaMask();
		}

		public override void OnLogic()
		{
			float tickDelta = _enemy.GetTickDelta();
			_context.SlowedTimer -= tickDelta;
			_context.AttackTimer += tickDelta;
			if (_context.Agent != null)
			{
				_context.Agent.speed = _settings.SlowedSpeed;
			}
			if (_context.HasChaseTarget)
			{
				_context.MoveToTargetPlayer();
			}
		}
	}
}
