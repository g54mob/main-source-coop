using Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.EarEnemy.States
{
	public class EarAttackState : StateBase<EarStateId>
	{
		private readonly EarEnemy _enemy;

		private readonly EarEnemyContext _context;

		private readonly EarEnemySettings _settings;

		public EarAttackState(EarEnemy enemy, EarEnemyContext context, EarEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(EarStateId.Attack);
			_context.SetVisualState(EarVisualState.Attack);
			_context.StateDurationTimeSystem.Enable();
			_context.EarAttackSystem.Enable();
			_context.StopAgentAndResetVelocity();
		}

		public override void OnExit()
		{
			_context.StateDurationTimeSystem.Disable();
			_context.EarAttackSystem.Disable();
			_context.NavMeshAgent.isStopped = false;
		}

		public override void OnLogic()
		{
			if (_context.CurrentStateTime >= _settings.AttackDuration)
			{
				_enemy.TriggerEvent(EarEvent.OnAttackFinished);
			}
		}
	}
}
