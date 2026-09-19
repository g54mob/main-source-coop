using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Features.CartUpgradesModule.Scripts.Core;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;
using UnityEngine;
using Zenject;

namespace Features.CartUpgradesModule.Scripts.Systems
{
	public class CartUpgradeSwapSystem : IInitializable, IDisposable
	{
		private const int AUTHORITY_WAIT_TICKS = 60;

		private readonly ICartUpgradeService _cartUpgradeService;

		private readonly ICartTierRegistry _cartTierRegistry;

		private readonly MultiplayerModel _multiplayerModel;

		public CartUpgradeSwapSystem(ICartUpgradeService cartUpgradeService, ICartTierRegistry cartTierRegistry, MultiplayerModel multiplayerModel)
		{
			_cartUpgradeService = cartUpgradeService;
			_cartTierRegistry = cartTierRegistry;
			_multiplayerModel = multiplayerModel;
		}

		public void Initialize()
		{
			_cartUpgradeService.OnModulesChanged += HandleModulesChanged;
		}

		public void Dispose()
		{
			_cartUpgradeService.OnModulesChanged -= HandleModulesChanged;
		}

		private void HandleModulesChanged(int modulesMask)
		{
			if (modulesMask != 0)
			{
				SwapOutdatedCarts(_cartUpgradeService.RequiredCartVariantIndex).Forget();
			}
		}

		private async UniTaskVoid SwapOutdatedCarts(int cartVariantIndex)
		{
			NetworkRunner runner = _multiplayerModel.NetworkRunner;
			if (runner == null || !runner.IsRunning || !runner.IsSharedModeMasterClient)
			{
				return;
			}
			NetworkObject prefab = _cartUpgradeService.ResolveCartPrefab();
			if (prefab == null)
			{
				return;
			}
			List<ICartTierCarrier> list = new List<ICartTierCarrier>();
			foreach (ICartTierCarrier cart in _cartTierRegistry.Carts)
			{
				if (cart != null && cart.Tier != cartVariantIndex && cart.NetworkObject != null && cart.NetworkObject.IsValid)
				{
					list.Add(cart);
				}
			}
			foreach (ICartTierCarrier item in list)
			{
				await SwapCart(runner, item, prefab);
			}
		}

		private async UniTask SwapCart(NetworkRunner runner, ICartTierCarrier cart, NetworkObject prefab)
		{
			NetworkObject outdatedObject = cart.NetworkObject;
			if (outdatedObject == null || !outdatedObject.IsValid)
			{
				return;
			}
			Vector3 position = cart.Transform.position;
			Quaternion rotation = cart.Transform.rotation;
			if (!outdatedObject.HasStateAuthority)
			{
				outdatedObject.RequestStateAuthority();
				for (int tick = 0; tick < 60; tick++)
				{
					if (outdatedObject.HasStateAuthority)
					{
						break;
					}
					await UniTask.Yield(PlayerLoopTiming.Update);
					if (outdatedObject == null || !outdatedObject.IsValid)
					{
						return;
					}
				}
				if (!outdatedObject.HasStateAuthority)
				{
					return;
				}
			}
			runner.Despawn(outdatedObject);
			await runner.SpawnAsync(prefab, position, rotation, runner.LocalPlayer);
		}
	}
}
