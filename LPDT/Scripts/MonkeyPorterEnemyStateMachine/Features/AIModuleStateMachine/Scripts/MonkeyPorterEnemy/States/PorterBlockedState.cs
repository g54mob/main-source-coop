using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.States
{
	public class PorterBlockedState : StateBase<MonkeyPorterStateId>
	{
		private readonly MonkeyPorterEnemy _monkeyPorterEnemy;

		private readonly MonkeyPorterContext _monkeyPorterContext;

		private readonly MonkeyPorterSettings _monkeyPorterSettings;

		private float _retryElapsed;

		public PorterBlockedState(MonkeyPorterEnemy monkeyPorterEnemy, MonkeyPorterContext monkeyPorterContext, MonkeyPorterSettings monkeyPorterSettings)
			: base(false, false)
		{
			_monkeyPorterEnemy = monkeyPorterEnemy;
			_monkeyPorterContext = monkeyPorterContext;
			_monkeyPorterSettings = monkeyPorterSettings;
		}

		public override void OnEnter()
		{
			_monkeyPorterContext.StopAgent();
			_retryElapsed = 0f;
			_monkeyPorterEnemy.SetVisualState(ResolveVisualState());
		}

		public override void OnLogic()
		{
			_retryElapsed += _monkeyPorterEnemy.GetTickDelta();
			if (!(_retryElapsed < _monkeyPorterSettings.BlockedRecheckInterval))
			{
				_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnPathCleared);
			}
		}

		private MonkeyPorterVisualState ResolveVisualState()
		{
			if (!TryGetGoalDirection(out var direction))
			{
				return MonkeyPorterVisualState.Blocked;
			}
			if (!_monkeyPorterContext.IsPlayerInTheWay(direction, _monkeyPorterSettings.PlayerBlockCheckDistance))
			{
				return MonkeyPorterVisualState.Blocked;
			}
			return MonkeyPorterVisualState.Idle;
		}

		private bool TryGetGoalDirection(out Vector3 direction)
		{
			direction = default(Vector3);
			if (!(_monkeyPorterEnemy.HasCartItem() ? _monkeyPorterContext.TryGetBoatDropPosition(out var position) : _monkeyPorterContext.TryGetNearestPlayerPosition(out position, out var _)))
			{
				return false;
			}
			direction = position - _monkeyPorterContext.transform.position;
			return true;
		}
	}
}
