using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyMoveToItemState : StateBase<MonkeyItemInteractionStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyItemInteractionSettings _itemSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		public MonkeyMoveToItemState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyItemInteractionSettings itemSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_itemSettings = itemSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.MoveToItem);
			_enemy.RaiseMoveMiddleAnimation();
			_enemy.RaiseMoveAnimation();
			_context.EnableMovement();
			_context.MoveToItemElapsed = 0f;
			_context.Agent.speed = _itemSettings.MoveToItemSpeed;
			_context.Agent.stoppingDistance = _movementSettings.StoppingDistance;
		}

		public override void OnLogic()
		{
			if (!_context.HasTargetItem)
			{
				_enemy.TriggerEvent(MonkeyEvent.OnItemLost);
				return;
			}
			Vector3 itemPosition = _context.GetItemPosition(_context.TargetItem);
			if (itemPosition == Vector3.zero)
			{
				_context.ClearTargetItem();
				_enemy.TriggerEvent(MonkeyEvent.OnItemLost);
				return;
			}
			if (Vector3.Distance(_context.transform.position, itemPosition) <= _movementSettings.StoppingDistance)
			{
				_enemy.TriggerEvent(MonkeyEvent.OnItemReached);
				return;
			}
			_context.MoveToItemElapsed += _enemy.GetTickDelta();
			if (_context.MoveToItemElapsed >= _itemSettings.MoveToItemTimeoutDuration)
			{
				_context.ClearTargetItem();
				_enemy.TriggerEvent(MonkeyEvent.OnItemLost);
			}
			else
			{
				_context.Agent.speed = _itemSettings.MoveToItemSpeed;
				_context.MoveToPosition(itemPosition);
			}
		}
	}
}
