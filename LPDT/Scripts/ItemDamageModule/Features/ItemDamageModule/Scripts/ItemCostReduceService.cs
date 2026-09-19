using Features.ItemCollisionModule.Scripts;
using Features.ItemsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;

namespace Features.ItemDamageModule.Scripts
{
	public class ItemCostReduceService : IItemCostReduceService
	{
		private const float FORCE_NORMALIZE_VALUE = 3f;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly ItemCostLossNetworkEvent _itemCostLossNetworkEvent;

		private readonly IItemCostReduceSpawnCoinsService _itemCostReduceSpawnCoinsService;

		public ItemCostReduceService(MultiplayerModel multiplayerModel, ItemCostLossNetworkEvent itemCostLossNetworkEvent, IItemCostReduceSpawnCoinsService itemCostReduceSpawnCoinsService)
		{
			_multiplayerModel = multiplayerModel;
			_itemCostLossNetworkEvent = itemCostLossNetworkEvent;
			_itemCostReduceSpawnCoinsService = itemCostReduceSpawnCoinsService;
		}

		public bool ProcessItemCollisionData(ItemCollisionData collisionData, PlayerRef? playerThatDamage = null, bool isIgnoreLimits = false)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return false;
			}
			IItem item = collisionData.Item;
			ItemCollisionConfig itemCollisionConfig = null;
			MonoItem monoItem = null;
			if (item is MonoItem monoItem2)
			{
				monoItem = monoItem2;
				itemCollisionConfig = monoItem.GetCollisionConfig();
			}
			if (!CanProcessItemDamage(item, monoItem))
			{
				return false;
			}
			if (itemCollisionConfig == null)
			{
				return false;
			}
			if (collisionData.Force < itemCollisionConfig.MinCollisionForce && !isIgnoreLimits)
			{
				return false;
			}
			int currencyValue = item.CurrencyValue;
			float num = CalculateCostLossPercent(collisionData.Force, itemCollisionConfig, isIgnoreLimits);
			int a;
			if (isIgnoreLimits)
			{
				a = Mathf.Max(1, Mathf.RoundToInt((float)(int)item.MaxCurrencyValue * num));
				a = Mathf.Min(a, currencyValue);
			}
			else
			{
				num = Mathf.Min(num, itemCollisionConfig.MaxCostLossPercentPerHit);
				a = Mathf.Max(1, Mathf.RoundToInt((float)(int)item.MaxCurrencyValue * num));
				a = Mathf.Min(a, currencyValue);
			}
			int num2 = currencyValue - a;
			bool flag = IsResponsiblePeer(item, playerThatDamage);
			if (a > 0 && flag)
			{
				item.SetCurrencyValue(num2);
				if (ShouldApplyCostLossFeedback(monoItem))
				{
					Vector3 vector = collisionData.CollisionCollider.ClosestPoint(collisionData.Item.NetworkObject.transform.position);
					_itemCostLossNetworkEvent.SendEvent(a, item.GetPricePosition(), vector, item.NetworkObject.Id);
					_itemCostReduceSpawnCoinsService.SpawnCoins(vector, a).Forget();
				}
			}
			if (num2 <= 0 && monoItem != null && monoItem.Object != null && monoItem.Object.IsValid)
			{
				monoItem.BreakItem();
				return true;
			}
			return false;
		}

		public bool BreakItemCompletely(ItemCollisionData collisionData, PlayerRef? playerThatDamage = null)
		{
			if (_multiplayerModel.NetworkRunner == null)
			{
				return false;
			}
			IItem item = collisionData.Item;
			if (!(item is MonoItem monoItem))
			{
				return false;
			}
			if (!CanProcessItemDamage(item, monoItem))
			{
				return false;
			}
			int currencyValue = item.CurrencyValue;
			if (IsResponsiblePeer(item, playerThatDamage))
			{
				item.SetCurrencyValue(0);
				if (ShouldApplyCostLossFeedback(monoItem) && currencyValue > 0)
				{
					Vector3 vector = collisionData.CollisionCollider.ClosestPoint(collisionData.Item.NetworkObject.transform.position);
					_itemCostLossNetworkEvent.SendEvent(currencyValue, item.GetPricePosition(), vector, item.NetworkObject.Id);
					_itemCostReduceSpawnCoinsService.SpawnCoins(vector, currencyValue).Forget();
				}
			}
			if (monoItem.Object == null || !monoItem.Object.IsValid)
			{
				return false;
			}
			monoItem.BreakItem();
			return true;
		}

		private bool IsResponsiblePeer(IItem item, PlayerRef? playerThatDamage)
		{
			if (playerThatDamage.HasValue || !item.NetworkObject.HasStateAuthority)
			{
				if (playerThatDamage.HasValue)
				{
					PlayerRef localPlayer = _multiplayerModel.NetworkRunner.LocalPlayer;
					PlayerRef? playerRef = playerThatDamage;
					return localPlayer == playerRef;
				}
				return false;
			}
			return true;
		}

		private static bool CanProcessItemDamage(IItem item, MonoItem monoItem)
		{
			if (item == null || !item.IsReducible || item.IsDespawned)
			{
				return false;
			}
			if (item.AreDespawnEffectsSuppressed)
			{
				return false;
			}
			if (monoItem != null && monoItem.TryGetComponent<IItemCollisionBreakGate>(out var component) && !component.CanBreakFromCollision)
			{
				return false;
			}
			if (item.CurrencyValue <= 0)
			{
				return CanDestroyWithoutMoney(monoItem);
			}
			return true;
		}

		private static bool CanDestroyWithoutMoney(MonoItem monoItem)
		{
			if (monoItem != null && monoItem.DefaultConfig != null)
			{
				return monoItem.DefaultConfig.CanDestroyWithoutMoney;
			}
			return false;
		}

		private static bool ShouldApplyCostLossFeedback(MonoItem monoItem)
		{
			return !CanDestroyWithoutMoney(monoItem);
		}

		private float CalculateCostLossPercent(float force, ItemCollisionConfig collisionConfig, bool isIgnoreLimits)
		{
			float num = force / 3f;
			float result = collisionConfig.Fragility * num;
			if (!isIgnoreLimits)
			{
				result = Mathf.Clamp(collisionConfig.Fragility * num, 0f, collisionConfig.MaxCostLossPercentPerHit);
			}
			return result;
		}
	}
}
