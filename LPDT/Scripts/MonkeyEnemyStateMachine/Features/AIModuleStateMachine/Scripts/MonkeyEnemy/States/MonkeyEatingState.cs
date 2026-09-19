using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Fusion;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyEatingState : StateBase<MonkeyItemInteractionStateId>
	{
		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyItemInteractionSettings _itemSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		private bool _grabRequested;

		public MonkeyEatingState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyItemInteractionSettings itemSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_itemSettings = itemSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_enemy.SetVisualState(MonkeyVisualState.Eating);
			_context.ResetEatingInteractionState();
			_enemy.ResetStateTimer(_itemSettings.EatingDuration);
			_context.StopAgent();
			_grabRequested = false;
			if (_context.IsTargetItemHeld())
			{
				_context.TryCaptureCoinSourcePlayerFromTargetItem();
			}
		}

		public override void OnLogic()
		{
			if (_context.EatingCoinConsumed)
			{
				UpdatePocketPhase();
				return;
			}
			if (!_context.HasTargetItem)
			{
				_enemy.TriggerEvent(MonkeyEvent.OnItemLost);
				return;
			}
			_context.FacePosition(_context.GetItemPosition(_context.TargetItem), _movementSettings.RotationSpeed, _enemy.GetTickDelta());
			if (!_grabRequested)
			{
				_enemy.RaiseGrabAnimation();
				_enemy.RaiseTakeCoinSound();
				_grabRequested = true;
			}
			if (_enemy.AdvanceStateTimer())
			{
				_enemy.TryConsumeCoinDuringEating();
				if (!_context.EatingCoinConsumed)
				{
					_enemy.TriggerEvent(MonkeyEvent.OnItemLost);
				}
			}
		}

		private void UpdatePocketPhase()
		{
			if (_context.CoinSourcePlayer != PlayerRef.None && _context.TryGetPlayerWorldPosition(_context.CoinSourcePlayer, out var position))
			{
				_context.FacePosition(position, _movementSettings.RotationSpeed, _enemy.GetTickDelta());
			}
			if (_enemy.AdvanceStateTimer())
			{
				_enemy.CompleteEatingInteraction();
			}
		}
	}
}
