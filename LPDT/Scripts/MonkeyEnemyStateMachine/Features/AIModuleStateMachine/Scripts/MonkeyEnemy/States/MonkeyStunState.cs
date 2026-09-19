using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyStunState : StateBase<MonkeyStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyStunSettings _stunSettings;

		public MonkeyStunState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyStunSettings stunSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_stunSettings = stunSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.Stun);
			_enemy.RaiseHitAnimation();
			_enemy.RaiseHitSound();
			_enemy.ResetStateTimer(_stunSettings.StunDuration);
			_context.ClearTargetItem();
			_context.StopAgent();
		}

		public override void OnLogic()
		{
			if (_enemy.AdvanceStateTimer())
			{
				_enemy.TriggerEvent(MonkeyEvent.OnRecovered);
			}
		}
	}
}
