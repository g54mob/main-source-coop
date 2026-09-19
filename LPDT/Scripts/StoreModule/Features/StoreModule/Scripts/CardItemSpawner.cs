using System;
using System.Threading.Tasks;
using Features.CartUpgradesModule.Scripts.Core;
using Features.DeadPartsModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using PlayerCustomization;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	public class CardItemSpawner : ICardItemSpawner
	{
		private MultiplayerModel _multiplayerModel;

		private IPlayerDeadPartSpawnService _playerDeadPartSpawnService;

		private ICartUpgradeService _cartUpgradeService;

		public CardItemSpawner(MultiplayerModel multiplayerModel, IPlayerDeadPartSpawnService playerDeadPartSpawnService, ICartUpgradeService cartUpgradeService)
		{
			_playerDeadPartSpawnService = playerDeadPartSpawnService;
			_multiplayerModel = multiplayerModel;
			_cartUpgradeService = cartUpgradeService;
		}

		public async Task<NetworkObject> Spawn(StoreCardData storeCardData, Vector3 position, Quaternion rotation, Color color, Transform parent = null)
		{
			NetworkBehaviour rewardItemPrefab = storeCardData.GetRewardItemPrefab();
			if (rewardItemPrefab == null)
			{
				return null;
			}
			if (_cartUpgradeService.TryResolveCartPrefab(rewardItemPrefab.gameObject, out var cartPrefab))
			{
				return await SpawnDefaultObject(cartPrefab, position, rotation, parent);
			}
			switch (storeCardData.GetRewardCardItemType())
			{
			case CardItemType.None:
			case CardItemType.Tool:
			case CardItemType.Weapon:
			case CardItemType.Consumable:
			case CardItemType.Upgrade:
				return await SpawnDefaultObject(rewardItemPrefab, position, rotation, parent);
			case CardItemType.PlayerDeadPart:
				return await SpawnPlayerDeadPart(rewardItemPrefab, position, color);
			case CardItemType.Skin:
				return null;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private async Task<NetworkObject> SpawnDefaultObject(NetworkBehaviour networkBehaviour, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			return ParentSpawnedObject(await _multiplayerModel.NetworkRunner.SpawnAsync(networkBehaviour, position, rotation), parent);
		}

		private async Task<NetworkObject> SpawnDefaultObject(NetworkObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			return ParentSpawnedObject(await _multiplayerModel.NetworkRunner.SpawnAsync(prefab, position, rotation), parent);
		}

		private NetworkObject ParentSpawnedObject(NetworkObject spawnedObject, Transform parent)
		{
			if (spawnedObject == null)
			{
				return null;
			}
			if (parent != null)
			{
				spawnedObject.transform.parent = parent;
			}
			return spawnedObject;
		}

		private async Task<NetworkObject> SpawnPlayerDeadPart(NetworkBehaviour networkBehaviour, Vector3 position, Color color)
		{
			if (!networkBehaviour.TryGetComponent<PlayerDeadPart>(out var component))
			{
				throw new Exception("There is no PlayerDeadPart component attached to " + networkBehaviour.name + " - check if card data is configured properly");
			}
			return (await _playerDeadPartSpawnService.SpawnPlayerDeadPart(component.DeadPartType, position, 0, new PlayerCustomizationSlotData
			{
				VariableColor = color
			})).Object;
		}
	}
}
