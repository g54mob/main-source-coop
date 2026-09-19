using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States
{
	public class EarStunState : StateBase<EarStateId>
	{
		private readonly EarEnemy _enemy;

		private readonly EarEnemyContext _context;

		private readonly EarEnemySettings _settings;

		public EarStunState(EarEnemy enemy, EarEnemyContext context, EarEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(EarStateId.Stun);
			_context.SetVisualState(EarVisualState.Stun);
			_context.StateDurationTimeSystem.Enable();
			_context.StopAgentAndResetVelocity();
		}

		public override void OnExit()
		{
			_context.StateDurationTimeSystem.Disable();
			_context.NavMeshAgent.isStopped = false;
		}

		public override void OnLogic()
		{
			if (_context.CurrentStateTime >= _settings.StunDuration)
			{
				_enemy.TriggerEvent(EarEvent.OnStunFinished);
			}
		}
	}
}
