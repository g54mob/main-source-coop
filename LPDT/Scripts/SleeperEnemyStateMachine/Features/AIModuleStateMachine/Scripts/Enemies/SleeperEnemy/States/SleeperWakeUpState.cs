using Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.SleeperEnemy.States
{
	public class SleeperWakeUpState : StateBase<SleeperStateId>
	{
		private readonly SleeperEnemy _enemy;

		private readonly SleeperEnemyContext _context;

		private readonly SleeperEnemySettings _settings;

		public SleeperWakeUpState(SleeperEnemy enemy, SleeperEnemyContext context, SleeperEnemySettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(SleeperStateId.WakeUp);
			_context.SetVisualState(SleeperVisualState.WakeUp);
			_context.StateDurationTimeSystem.Enable();
			_context.RotateTowardsDirectionSystem.Enable();
			_context.SleeperDamageAggrSystem.Enable();
		}

		public override void OnExit()
		{
			_context.StateDurationTimeSystem.Disable();
			_context.RotateTowardsDirectionSystem.Disable();
			_context.SleeperDamageAggrSystem.Disable();
		}

		public override void OnLogic()
		{
			if (_context.CurrentStateTime >= _settings.WakeUpDuration)
			{
				_enemy.TriggerEvent(SleeperEvent.OnWakeUpFinished);
			}
		}
	}
}
