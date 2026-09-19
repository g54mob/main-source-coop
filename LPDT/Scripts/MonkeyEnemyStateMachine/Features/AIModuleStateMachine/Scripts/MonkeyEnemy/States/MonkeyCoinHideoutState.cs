using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyCoinHideoutState : StateBase<MonkeyItemInteractionStateId>
	{
		private enum HideoutPhase
		{
			Fleeing = 0,
			Wandering = 1
		}

		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyItemInteractionSettings _itemSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		private HideoutPhase _phase;

		private Vector3 _fleeDestination;

		private bool _hasFleeDestination;

		public MonkeyCoinHideoutState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyItemInteractionSettings itemSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_itemSettings = itemSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.CoinHideout);
			_enemy.SetCurrentTargetPlayerId(-1);
			_context.TargetPlayer = PlayerRef.None;
			_context.ClearTargetItem();
			_context.EnableMovement();
			_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
			_context.HideoutFleeElapsed = 0f;
			_hasFleeDestination = false;
			_phase = HideoutPhase.Fleeing;
			if (_context.CoinSourcePlayer == PlayerRef.None)
			{
				CompleteHideout();
				return;
			}
			_enemy.RaiseRunAnimation();
			_enemy.RaiseMoveAnimation();
			_context.Agent.speed = _itemSettings.HideoutFleeSpeed;
			TryPickFleeDestination();
		}

		public override void OnLogic()
		{
			if (_context.CoinSourcePlayer == PlayerRef.None)
			{
				CompleteHideout();
			}
			else if (_phase == HideoutPhase.Fleeing)
			{
				UpdateFleeing();
			}
			else
			{
				UpdateHideoutWandering();
			}
		}

		private void UpdateFleeing()
		{
			_context.HideoutFleeElapsed += _enemy.GetTickDelta();
			float hideoutMinDistanceFromCoinSourcePlayer = _itemSettings.HideoutMinDistanceFromCoinSourcePlayer;
			bool flag = _context.GetDistanceToPlayer(_context.CoinSourcePlayer) >= hideoutMinDistanceFromCoinSourcePlayer;
			bool flag2 = _hasFleeDestination && Vector3.Distance(_context.transform.position, _fleeDestination) <= _itemSettings.HideoutFleeDestinationReachedDistance;
			if (_context.HideoutFleeElapsed >= _itemSettings.HideoutFleeTimeoutDuration || (flag && (flag2 || !_hasFleeDestination)))
			{
				BeginHideoutWandering();
				return;
			}
			if (!_hasFleeDestination || flag2)
			{
				TryPickFleeDestination();
			}
			if (_hasFleeDestination)
			{
				_context.MoveToPosition(_fleeDestination);
			}
		}

		private void BeginHideoutWandering()
		{
			_phase = HideoutPhase.Wandering;
			_context.HideoutCenterPosition = _context.transform.position;
			_enemy.ResetStateTimer(_itemSettings.HideoutWanderDuration);
			_enemy.RaiseMoveSlowAnimation();
			_context.Agent.speed = _movementSettings.WanderingSpeed;
			ResetWanderMovementTimer();
		}

		private void UpdateHideoutWandering()
		{
			_context.MovementUpdateElapsed += _enemy.GetTickDelta();
			if (_context.MovementUpdateElapsed >= _context.MovementUpdateInterval)
			{
				_context.MoveToPosition(_context.GetRandomNavmeshPosition(_context.HideoutCenterPosition, _itemSettings.HideoutWanderRadius));
				ResetWanderMovementTimer();
			}
			if (_enemy.AdvanceStateTimer())
			{
				CompleteHideout();
			}
		}

		private void TryPickFleeDestination()
		{
			_hasFleeDestination = _context.TryGetHideoutFleePosition(_itemSettings, out _fleeDestination);
		}

		private void ResetWanderMovementTimer()
		{
			_context.MovementUpdateElapsed = 0f;
			_context.MovementUpdateInterval = Random.Range(_movementSettings.MinWanderPositionUpdateTime, _movementSettings.MaxWanderPositionUpdateTime);
		}

		private void CompleteHideout()
		{
			_context.ClearCoinSourcePlayer();
			_enemy.TriggerEvent(MonkeyEvent.OnItemConsumed);
		}
	}
}
