using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.States
{
	public class PorterGoToPlayersState : StateBase<MonkeyPorterStateId>
	{
		private readonly MonkeyPorterEnemy _monkeyPorterEnemy;

		private readonly MonkeyPorterContext _monkeyPorterContext;

		private readonly MonkeyPorterSettings _monkeyPorterSettings;

		private float _repathElapsed;

		public PorterGoToPlayersState(MonkeyPorterEnemy monkeyPorterEnemy, MonkeyPorterContext monkeyPorterContext, MonkeyPorterSettings monkeyPorterSettings)
			: base(false, false)
		{
			_monkeyPorterEnemy = monkeyPorterEnemy;
			_monkeyPorterContext = monkeyPorterContext;
			_monkeyPorterSettings = monkeyPorterSettings;
		}

		public override void OnEnter()
		{
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Moving);
			_monkeyPorterContext.SetSpeed(_monkeyPorterSettings.MoveSpeed);
			_monkeyPorterContext.SetStoppingDistance(_monkeyPorterSettings.StoppingDistance);
			_repathElapsed = _monkeyPorterSettings.RepathInterval;
			_monkeyPorterEnemy.BlockDetector.Reset();
		}

		public override void OnLogic()
		{
			if (!_monkeyPorterContext.TryGetNearestPlayerPosition(out var position, out var distance))
			{
				return;
			}
			if (distance <= _monkeyPorterSettings.WaitRadius)
			{
				_monkeyPorterContext.StopAgent();
				_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnArrivedToPlayers);
				return;
			}
			_monkeyPorterEnemy.TryInitializeFloorAnchor();
			if (_monkeyPorterContext.TryClampGoalToAllowedArea(position, _monkeyPorterEnemy.HomePosition, _monkeyPorterSettings.RoamRadius, _monkeyPorterSettings.RoamMaxHeightDelta, _monkeyPorterEnemy.IsFloorAnchorInitialized, _monkeyPorterEnemy.FloorAnchor, out var allowedGoal) && IsAtBoundary(allowedGoal))
			{
				HoldBoundary();
				return;
			}
			float tickDelta = _monkeyPorterEnemy.GetTickDelta();
			if (_monkeyPorterEnemy.BlockDetector.IsStalled(tickDelta) || _monkeyPorterEnemy.BlockDetector.IsGoalUnreachable(allowedGoal, tickDelta))
			{
				_monkeyPorterEnemy.TriggerEvent(MonkeyPorterEvent.OnPathBlocked);
				return;
			}
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Moving);
			_repathElapsed += tickDelta;
			if (!(_repathElapsed < _monkeyPorterSettings.RepathInterval))
			{
				_repathElapsed = 0f;
				_monkeyPorterContext.MoveToPosition(allowedGoal);
			}
		}

		private bool IsAtBoundary(Vector3 goal)
		{
			return Vector3.Distance(_monkeyPorterContext.transform.position, goal) <= _monkeyPorterSettings.RoamBoundaryStopDistance;
		}

		private void HoldBoundary()
		{
			_monkeyPorterContext.StopAgent();
			_monkeyPorterEnemy.SetVisualState(MonkeyPorterVisualState.Idle);
			_monkeyPorterEnemy.BlockDetector.Reset();
			_repathElapsed = _monkeyPorterSettings.RepathInterval;
		}
	}
}
