using Features.AIModuleStateMachine.Scripts.MonkeyEnemy.Settings;
using Fusion;
using UnityEngine;
using UnityHFSM;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy.States
{
	public class MonkeyGivingItemState : StateBase<MonkeyItemInteractionStateId>
	{
		private enum GivingPhase
		{
			Offering = 0,
			Retracting = 1
		}

		private readonly MonkeyEnemy _enemy;

		private readonly MonkeyEnemyContext _context;

		private readonly MonkeyItemGiftSettings _itemGiftSettings;

		private readonly MonkeyItemInteractionSettings _itemInteractionSettings;

		private readonly MonkeyMovementSettings _movementSettings;

		private GivingPhase _phase;

		public MonkeyGivingItemState(MonkeyEnemy enemy, MonkeyEnemyContext context, MonkeyItemGiftSettings itemGiftSettings, MonkeyItemInteractionSettings itemInteractionSettings, MonkeyMovementSettings movementSettings)
			: base(false, false)
		{
			_enemy = enemy;
			_context = context;
			_itemGiftSettings = itemGiftSettings;
			_itemInteractionSettings = itemInteractionSettings;
			_movementSettings = movementSettings;
		}

		public override void OnEnter()
		{
			_phase = GivingPhase.Offering;
			_enemy.SetVisualState(MonkeyVisualState.GivingItem);
			_enemy.RaiseCoinRequestAnimation();
			_enemy.ResetStateTimer(_itemGiftSettings.GivingItemDuration);
			_enemy.ResetGiftItemSpawnState();
			_context.StopAgent();
			_context.ClearGiftedItem();
		}

		public override void OnExit()
		{
			if (_context.GiftedItem != null && !_context.GiftedItem.IsDespawned)
			{
				if (_context.IsGiftedItemHeldByCoinSourcePlayer())
				{
					_context.ClearGiftedItem();
				}
				else
				{
					_context.DestroyGiftedItemOffering();
				}
			}
		}

		public override void OnLogic()
		{
			if (_context.CoinSourcePlayer == PlayerRef.None)
			{
				CompleteGiving();
				return;
			}
			if (_context.TryGetPlayerWorldPosition(_context.CoinSourcePlayer, out var position))
			{
				_context.FacePosition(position, _movementSettings.RotationSpeed, _enemy.GetTickDelta());
			}
			if (_context.IsGiftedItemHeldByCoinSourcePlayer())
			{
				CompleteGiving();
			}
			else if (_phase == GivingPhase.Retracting)
			{
				UpdateRetractingPhase();
			}
			else if (!TryProcessGiftedItemActivation() && _enemy.AdvanceStateTimer())
			{
				if (_context.GiftedItem != null)
				{
					BeginRetractingPhase();
				}
				else
				{
					CompleteGiving();
				}
			}
		}

		private bool TryProcessGiftedItemActivation()
		{
			if (!_context.GiftCanActivateItem || _context.GiftItemActivationAttempted || _context.GiftedItem == null || _context.GiftedItem.IsDespawned)
			{
				return false;
			}
			_context.AdvanceGiftItemSpawnElapsed(_enemy.GetTickDelta());
			if (_context.GiftItemSpawnElapsed < _context.GiftActivateItemDelay)
			{
				return false;
			}
			_context.MarkGiftItemActivationAttempted();
			if (!_enemy.TryActivateGiftedItemIfEligible())
			{
				return false;
			}
			CompleteGiving();
			return true;
		}

		private void BeginRetractingPhase()
		{
			_phase = GivingPhase.Retracting;
			_enemy.RaiseGrabAnimation();
			_context.RetractGiftedItem(_itemGiftSettings.ItemScaleDuration, _itemGiftSettings.ItemScaleEase);
			float duration = Mathf.Max(_itemGiftSettings.ItemScaleDuration, _itemInteractionSettings.CoinPocketAnimationDuration);
			_enemy.ResetStateTimer(duration);
		}

		private void UpdateRetractingPhase()
		{
			if (_context.IsGiftedItemHeldByCoinSourcePlayer())
			{
				CompleteGiving();
			}
			else if (_enemy.AdvanceStateTimer())
			{
				CompleteGiving(retractUnclaimedItem: true);
			}
		}

		private void CompleteGiving(bool retractUnclaimedItem = false)
		{
			if (retractUnclaimedItem)
			{
				_context.DestroyGiftedItemOffering();
			}
			else
			{
				_context.ClearGiftedItem();
			}
			_enemy.TriggerEvent(MonkeyEvent.OnItemGiveCompleted);
		}
	}
}
