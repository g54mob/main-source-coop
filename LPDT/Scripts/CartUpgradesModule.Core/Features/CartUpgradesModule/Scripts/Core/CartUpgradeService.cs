using System;
using Features.CartUpgradesModule.Scripts.Data;
using Fusion;
using UnityEngine;

namespace Features.CartUpgradesModule.Scripts.Core
{
	public class CartUpgradeService : ICartUpgradeService, IDisposable
	{
		private readonly CartUpgradesModel _cartUpgradesModel;

		private readonly CartUpgradeConfiguration _cartUpgradeConfiguration;

		public int ActiveModulesMask => _cartUpgradesModel.UpgradeModules.Value;

		public bool IsStateReady => _cartUpgradesModel.IsAttached;

		public int RequiredCartVariantIndex => _cartUpgradeConfiguration.GetCartVariantIndex(ActiveModulesMask);

		public event Action<int> OnModulesChanged;

		public CartUpgradeService(CartUpgradesModel cartUpgradesModel, CartUpgradeConfiguration cartUpgradeConfiguration)
		{
			_cartUpgradesModel = cartUpgradesModel;
			_cartUpgradeConfiguration = cartUpgradeConfiguration;
			_cartUpgradesModel.OnUpgradeModulesChanged += RaiseModulesChanged;
		}

		public bool HasModule(CartUpgradeModule module)
		{
			return module.IsInMask(ActiveModulesMask);
		}

		public bool IsModulePurchasable(CartUpgradeModule module)
		{
			if (module != CartUpgradeModule.None && !HasModule(module))
			{
				return _cartUpgradeConfiguration.IsModuleSupported(module);
			}
			return false;
		}

		public void ApplyModule(CartUpgradeModule module)
		{
			if (module != CartUpgradeModule.None && !HasModule(module))
			{
				_cartUpgradesModel.AddUpgradeModules(module.ToMask());
			}
		}

		public void ResetModules()
		{
			_cartUpgradesModel.ResetUpgradeModules();
		}

		public NetworkObject ResolveCartPrefab()
		{
			return _cartUpgradeConfiguration.GetCartPrefab(ActiveModulesMask);
		}

		public bool TryResolveCartPrefab(GameObject authoredPrefab, out NetworkObject cartPrefab)
		{
			cartPrefab = null;
			if (!_cartUpgradeConfiguration.IsCartPrefab(authoredPrefab))
			{
				return false;
			}
			cartPrefab = ResolveCartPrefab();
			return cartPrefab != null;
		}

		public void Dispose()
		{
			_cartUpgradesModel.OnUpgradeModulesChanged -= RaiseModulesChanged;
		}

		private void RaiseModulesChanged(int modulesMask)
		{
			this.OnModulesChanged?.Invoke(modulesMask);
		}
	}
}
