using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.States
{
	public class PorterWaitForItemState : StateBase<MonkeyPorterStateId>
	{
		private readonly MonkeyPorterEnemy _monkeyPorterEnemy;

		private readonly MonkeyPorterContext _monkeyPorterContext;

		public PorterWaitForItemState(MonkeyPorterEnemy monkeyPorterEnemy, MonkeyPorterContext monkeyPorterContext)
			: base(false, false)
		{
			_monkeyPorterEnemy = monkeyPorterEnemy;
			_monkeyPorterContext = monkeyPorterContext;
		}

		public override void OnEnter()
		{
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Idle);
			_monkeyPorterContext.StopAgent();
		}

		public override void OnLogic()
		{
			if (_monkeyPorterEnemy.HasCartItem())
			{
				_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnItemLoaded);
			}
		}
	}
}
