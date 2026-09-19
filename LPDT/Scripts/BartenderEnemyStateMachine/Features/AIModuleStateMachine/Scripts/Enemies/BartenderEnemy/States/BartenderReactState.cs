using Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.Enemies.BartenderEnemy.States
{
	public class BartenderReactState : StateBase<BartenderStateId>
	{
		private readonly BartenderEnemy _enemy;

		private readonly BartenderEnemyContext _context;

		private readonly BartenderReactSettings _reactSettings;

		public BartenderReactState(BartenderEnemy enemy, BartenderEnemyContext context, BartenderReactSettings reactSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_reactSettings = reactSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetCurrentStateId(BartenderStateId.React);
			_context.SetIsDancing(value: false);
			_context.SetActiveDanceIndex(-1);
			_context.SetIsReacting(value: true);
			_context.CurrentStateTime = 0f;
			_context.AnimationSystem.Enable();
			_context.ReplicateTargetSystem.Enable();
		}

		public override void OnLogic()
		{
			_context.CurrentStateTime += _enemy.GetTickDelta();
			float num = ((_reactSettings != null) ? _reactSettings.ReactDurationSeconds : 1.25f);
			if (_context.CurrentStateTime >= num)
			{
				_enemy.TriggerEvent(BartenderEvent.OnReactFinished);
			}
		}

		public override void OnExit()
		{
			_context.SetIsReacting(value: false);
		}
	}
}
