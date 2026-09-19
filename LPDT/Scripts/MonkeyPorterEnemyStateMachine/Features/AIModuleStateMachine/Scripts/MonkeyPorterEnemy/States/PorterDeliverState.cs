using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.States
{
	public class PorterDeliverState : StateBase<MonkeyPorterStateId>
	{
		private readonly MonkeyPorterEnemy _monkeyPorterEnemy;

		private readonly MonkeyPorterContext _monkeyPorterContext;

		private readonly MonkeyPorterSettings _monkeyPorterSettings;

		private float _elapsed;

		private float _throwElapsed;

		private bool _isThrowStarted;

		private bool _delivered;

		public PorterDeliverState(MonkeyPorterEnemy monkeyPorterEnemy, MonkeyPorterContext monkeyPorterContext, MonkeyPorterSettings monkeyPorterSettings)
			: base(false, false)
		{
			_monkeyPorterEnemy = monkeyPorterEnemy;
			_monkeyPorterContext = monkeyPorterContext;
			_monkeyPorterSettings = monkeyPorterSettings;
		}

		public override void OnEnter()
		{
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Carrying);
			_monkeyPorterContext.StopAgent();
			_elapsed = 0f;
			_throwElapsed = 0f;
			_isThrowStarted = false;
			_delivered = false;
			_monkeyPorterEnemy.SetAimTarget(_monkeyPorterContext.GetBoatWorldPosition());
		}

		public override void OnExit()
		{
			_monkeyPorterEnemy.ClearAimTarget();
		}

		public override void OnLogic()
		{
			float tickDelta = _monkeyPorterEnemy.GetTickDelta();
			_elapsed += tickDelta;
			if (!_isThrowStarted)
			{
				if (!IsReadyToThrow())
				{
					return;
				}
				_isThrowStarted = true;
				_monkeyPorterEnemy.PlayThrowAnimation();
			}
			_throwElapsed += tickDelta;
			if (!_delivered)
			{
				if (_throwElapsed < _monkeyPorterSettings.DeliverThrowDuration)
				{
					return;
				}
				_monkeyPorterEnemy.DeliverCarriedItem();
				_delivered = true;
			}
			if (!(_throwElapsed < _monkeyPorterSettings.DeliverThrowDuration + _monkeyPorterSettings.DeliverHoldAfterThrow))
			{
				_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnItemDelivered);
			}
		}

		private bool IsReadyToThrow()
		{
			if (!_monkeyPorterEnemy.IsAimedAtTarget)
			{
				return _elapsed >= _monkeyPorterSettings.DeliverAimTimeout;
			}
			return true;
		}
	}
}
