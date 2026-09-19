using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperAttackState : StateBase<SleeperStateId>
	{
		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		private readonly SleeperEnemySettings _settings;

		public SleeperAttackState(SleeperEnemy enemy, SleeperEnemyContext context, SleeperEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.Attack);
			_context.SetVisualState(SleeperVisualState.Attack);
			_context.RotateTowardsDirectionSystem.Disable();
			_context.SleeperAttackRotationSystem.Enable();
			_context.StateDurationTimeSystem.Enable();
			_context.SleeperAttackSystem.Enable();
			_context.NavMeshAgent.isStopped = true;
		}

		public override void OnExit()
		{
			_context.SleeperAttackRotationSystem.Disable();
			_context.StateDurationTimeSystem.Disable();
			_context.SleeperAttackSystem.Disable();
			_context.NavMeshAgent.isStopped = false;
			_context.AttackCooldown = _context.TimeToAttack;
			_context.IsAttackOnCooldown = _context.AttackCooldown > 0f;
		}

		public override void OnLogic()
		{
			if (_context.CurrentStateTime >= _settings.AttackDuration)
			{
				_enemy.TriggerEvent(SleeperEvent.OnAttackFinished);
			}
		}
	}
}
