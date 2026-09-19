using Features.AIModuleStateMachine.Scripts.HeadmanEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.HeadmanEnemy.States
{
	public class HeadmanRageEndState : StateBase<HeadmanRageStateId>
	{
		private readonly HeadmanEnemy _enemy;

		private readonly HeadmanEnemyContext _context;

		private readonly HeadmanRageSettings _settings;

		public HeadmanRageEndState(HeadmanEnemy enemy, HeadmanEnemyContext context, HeadmanRageSettings settings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_settings = settings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(HeadmanVisualState.RageEnd);
			_context.SetMovementEnabled(enabled: false);
			_context.RoarsPerformed = 0;
			_context.ActiveRageSubstate = HeadmanRageStateId.End;
			_enemy.TriggerRoarPresentation();
		}

		public override void OnLogic()
		{
			if (_context.RoarsPerformed >= _settings.RoarCount)
			{
				_enemy.TriggerEvent(HeadmanEvent.OnRageFinished);
			}
		}
	}
}
