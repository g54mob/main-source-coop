using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyFearState : StateBase<MonkeyStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyFearSettings _fearSettings;

		public MonkeyFearState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyFearSettings fearSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_fearSettings = fearSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.Fear);
			_enemy.SetCurrentTargetPlayerId(-1);
			_enemy.RaiseRunAnimation();
			_enemy.RaiseMoveAnimation();
			_context.TargetPlayer = PlayerRef.None;
			_context.ClearTargetItem();
			_context.EnableMovement();
			_context.Agent.speed = _fearSettings.FearSpeed;
			_context.FearDestinationElapsed = _fearSettings.DestinationUpdateInterval;
		}

		public override void OnLogic()
		{
			if (!_context.IsEnemyVisibleByPlayers(_fearSettings.DetectionDistance))
			{
				if (_context.IsDespawnAfterFear)
				{
					_enemy.RequestDespawnAfterFear();
					return;
				}
				_enemy.MarkFearCompleted();
				_enemy.TriggerEvent(MonkeyEvent.OnTargetLost);
				return;
			}
			_context.FearDestinationElapsed += _enemy.GetTickDelta();
			if (!(_context.FearDestinationElapsed < _fearSettings.DestinationUpdateInterval) && (!_context.Agent.hasPath || !(Vector3.Distance(_context.Agent.pathEndPosition, _context.transform.position) > _fearSettings.DestinationReachedDistance)))
			{
				_context.FearDestinationElapsed = 0f;
				_context.MoveToPosition(_context.GetRandomSafeNavmeshPosition(_fearSettings.RunDistance, _fearSettings.SafePositionAttempts, _fearSettings.SafePositionMinDistanceFromCenter, _fearSettings.SafePositionCenterDistanceWeight, _fearSettings.SafePositionAverageAvoidDistanceWeight, _fearSettings.SafePositionTooClosePenaltyRadius));
			}
		}
	}
}
