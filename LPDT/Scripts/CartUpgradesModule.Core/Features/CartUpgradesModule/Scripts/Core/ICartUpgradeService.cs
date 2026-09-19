using System;
using Fusion;
using UnityEngine;

namespace Features.CartUpgradesModule.Scripts.Core
{
	public interface ICartUpgradeService
	{
		int ActiveModulesMask { get; }

		bool IsStateReady { get; }

		int RequiredCartVariantIndex { get; }

		event Action<int> OnModulesChanged;

		bool HasModule(CartUpgradeModule module);

		bool IsModulePurchasable(CartUpgradeModule module);

		void ApplyModule(CartUpgradeModule module);

		void ResetModules();

		NetworkObject ResolveCartPrefab();

		bool TryResolveCartPrefab(GameObject authoredPrefab, out NetworkObject cartPrefab);
	}
}
