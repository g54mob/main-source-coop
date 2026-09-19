using System;
using Features.CartUpgradesModule.Scripts.Data;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.CartUpgradesModule.Scripts.Networked
{
	public class CartUpgradesModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly CartUpgradesModel _cartUpgradesModel;

		private CartUpgradesNetworkObject _cartUpgradesNetworkObject;

		private bool _hasPendingUpgradeModules;

		private int _pendingUpgradeModules;

		public CartUpgradesModelBridge(CartUpgradesModel cartUpgradesModel)
		{
			_cartUpgradesModel = cartUpgradesModel;
		}

		public void Bind(CartUpgradesNetworkObject cartUpgradesNetworkObject)
		{
			Unbind();
			_cartUpgradesNetworkObject = cartUpgradesNetworkObject;
			_cartUpgradesNetworkObject.OnNetworkedUpgradeModulesChanged += HandleNetworkedUpgradeModulesChanged;
			_cartUpgradesNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_cartUpgradesNetworkObject.OnDespawned += HandleDespawned;
			_cartUpgradesModel.UpgradeModules.BindWriter(WriteUpgradeModules);
			ApplyUpgradeModulesFromNetwork(_cartUpgradesNetworkObject.UpgradeModules);
			_cartUpgradesModel.SetAuthorityProvider(() => _cartUpgradesNetworkObject.HasStateAuthority);
			_cartUpgradesModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_cartUpgradesNetworkObject == null))
			{
				_cartUpgradesNetworkObject.OnNetworkedUpgradeModulesChanged -= HandleNetworkedUpgradeModulesChanged;
				_cartUpgradesNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_cartUpgradesNetworkObject.OnDespawned -= HandleDespawned;
				_cartUpgradesModel.UpgradeModules.BindWriter(null);
				_cartUpgradesNetworkObject = null;
				_hasPendingUpgradeModules = false;
				_cartUpgradesModel.SetAuthorityProvider(null);
				_cartUpgradesModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<CartUpgradesNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedUpgradeModulesChanged(int upgradeModules)
		{
			ApplyUpgradeModulesFromNetwork(upgradeModules);
		}

		private void ApplyUpgradeModulesFromNetwork(int upgradeModules)
		{
			_cartUpgradesModel.UpgradeModules.ApplyFromNetwork(upgradeModules);
		}

		private bool WriteUpgradeModules(int value)
		{
			if (!_cartUpgradesModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] CartUpgradesModel.UpgradeModules was written without state authority; the write was ignored.");
				return false;
			}
			_pendingUpgradeModules = value;
			_hasPendingUpgradeModules = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_cartUpgradesNetworkObject == null || _cartUpgradesNetworkObject.Object == null || _cartUpgradesNetworkObject.Runner == null)
			{
				return false;
			}
			if (_cartUpgradesNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_cartUpgradesNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingUpgradeModules)
			{
				_hasPendingUpgradeModules = false;
				_cartUpgradesNetworkObject.TryWriteUpgradeModules(_pendingUpgradeModules);
			}
		}
	}
}
