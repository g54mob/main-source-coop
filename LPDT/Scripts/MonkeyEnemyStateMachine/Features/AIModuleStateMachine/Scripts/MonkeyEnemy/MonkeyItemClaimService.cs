using Features.GrabModule.Scripts;
using Features.ItemsModule.Scripts;

namespace Features.AIModuleStateMachine.Scripts.MonkeyEnemy
{
	public class MonkeyItemClaimService
	{
		public bool TryClaim(MonkeyItemClaimModel model, IItem currentTargetItem, IItem requestedItem, MonkeyEnemy monkeyEnemy, int fallbackHolderId)
		{
			if (!CanClaim(model, requestedItem, monkeyEnemy, fallbackHolderId, out var grabable))
			{
				return false;
			}
			if (currentTargetItem == requestedItem && IsClaimOwner(model, monkeyEnemy, fallbackHolderId))
			{
				return true;
			}
			Release(model, monkeyEnemy, fallbackHolderId);
			int holderId = GetHolderId(monkeyEnemy, fallbackHolderId);
			grabable.GrabbedByExternal(holderId, monkeyEnemy.NetworkObject.StateAuthority.PlayerId);
			if (!grabable.GrabbedByExternals.Contains(holderId))
			{
				return false;
			}
			model.ClaimedGrabable = grabable;
			model.HasClaim = true;
			return true;
		}

		public void Release(MonkeyItemClaimModel model, MonkeyEnemy monkeyEnemy, int fallbackHolderId)
		{
			if (model == null)
			{
				return;
			}
			if (!model.HasClaim || model.ClaimedGrabable == null)
			{
				model.ClearClaimState();
				return;
			}
			int holderId = GetHolderId(monkeyEnemy, fallbackHolderId);
			if (model.ClaimedGrabable.GrabbedByExternals.Contains(holderId))
			{
				model.ClaimedGrabable.UnGrabbedByExternal(holderId);
			}
			model.ClearClaimState();
		}

		public void ForgetConsumed(MonkeyItemClaimModel model, IItem targetItem)
		{
			if (model != null)
			{
				if (targetItem?.NetworkObject != null)
				{
					model.ConsumedTargetItemIds.Add(targetItem.NetworkObject.Id.Raw);
				}
				model.ClearClaimState();
			}
		}

		public bool IsClaimOwner(MonkeyItemClaimModel model, MonkeyEnemy monkeyEnemy, int fallbackHolderId)
		{
			if (model == null || !model.HasClaim || model.ClaimedGrabable == null)
			{
				return false;
			}
			int holderId = GetHolderId(monkeyEnemy, fallbackHolderId);
			return model.ClaimedGrabable.GrabbedByExternals.Contains(holderId);
		}

		private bool CanClaim(MonkeyItemClaimModel model, IItem item, MonkeyEnemy monkeyEnemy, int fallbackHolderId, out SimplePointGrabable grabable)
		{
			grabable = null;
			if (model == null || monkeyEnemy == null || monkeyEnemy.NetworkObject == null || item == null || item.NetworkObject == null || item.IsDespawned || item.IsConsumed || !item.AvailableForEnemy)
			{
				return false;
			}
			if (model.ConsumedTargetItemIds.Contains(item.NetworkObject.Id.Raw))
			{
				return false;
			}
			if (!item.NetworkObject.TryGetComponent<SimplePointGrabable>(out grabable) || grabable == null)
			{
				return false;
			}
			if (grabable.InCart || grabable.GrabBlocked)
			{
				return false;
			}
			int holderId = GetHolderId(monkeyEnemy, fallbackHolderId);
			if (grabable.GrabbedByExternalsCount != 0)
			{
				return grabable.GrabbedByExternals.Contains(holderId);
			}
			return true;
		}

		private int GetHolderId(MonkeyEnemy monkeyEnemy, int fallbackHolderId)
		{
			if (!(monkeyEnemy != null) || !(monkeyEnemy.NetworkObject != null) || !monkeyEnemy.NetworkObject.Id.IsValid)
			{
				return fallbackHolderId;
			}
			return monkeyEnemy.NetworkObject.Id.GetHashCode();
		}
	}
}
