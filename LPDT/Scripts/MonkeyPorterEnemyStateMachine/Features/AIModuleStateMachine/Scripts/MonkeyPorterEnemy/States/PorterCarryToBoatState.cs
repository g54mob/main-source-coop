using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.States
{
	public class PorterCarryToBoatState : StateBase<MonkeyPorterStateId>
	{
		private readonly MonkeyPorterEnemy _monkeyPorterEnemy;

		private readonly MonkeyPorterContext _monkeyPorterContext;

		private readonly MonkeyPorterSettings _monkeyPorterSettings;

		public PorterCarryToBoatState(MonkeyPorterEnemy monkeyPorterEnemy, MonkeyPorterContext monkeyPorterContext, MonkeyPorterSettings monkeyPorterSettings)
			: base(false, false)
		{
			_monkeyPorterEnemy = monkeyPorterEnemy;
			_monkeyPorterContext = monkeyPorterContext;
			_monkeyPorterSettings = monkeyPorterSettings;
		}

		public override void OnEnter()
		{
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Carrying);
			_monkeyPorterContext.SetSpeed(_monkeyPorterSettings.CarrySpeed);
			_monkeyPorterContext.SetStoppingDistance(GetStopDistance());
			_monkeyPorterEnemy.BlockDetector.Reset();
			if (_monkeyPorterContext.TryGetBoatDropPosition(out var position))
			{
				_monkeyPorterContext.MoveToPosition(position);
			}
		}

		public override void OnLogic()
		{
			Vector3 position;
			if (!_monkeyPorterEnemy.HasCartItem())
			{
				_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnItemLostFromCart);
			}
			else if (_monkeyPorterContext.TryGetBoatDropPosition(out position))
			{
				float num = GetStopDistance() + _monkeyPorterSettings.BoatArriveThreshold;
				float num2 = Vector3.Distance(_monkeyPorterContext.transform.position, position);
				bool flag = !_monkeyPorterContext.HasBoatApproach && _monkeyPorterContext.IsWithinBoatThrowRange(_monkeyPorterSettings.BoatStopDistance);
				if (_monkeyPorterEnemy.BlockDetector.IsStalled(_monkeyPorterEnemy.GetTickDelta()) && !flag && num2 > num)
				{
					_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnPathBlocked);
				}
				else if (flag || _monkeyPorterContext.AgentReachedDestination(num))
				{
					_monkeyPorterContext.StopAgent();
					_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnArrivedToBoat);
				}
				else
				{
					_monkeyPorterContext.EnsureHeadingTo(position);
				}
			}
		}

		private float GetStopDistance()
		{
			if (!_monkeyPorterContext.HasBoatApproach)
			{
				return _monkeyPorterSettings.BoatStopDistance;
			}
			return _monkeyPorterSettings.BoatApproachStopDistance;
		}
	}
}
