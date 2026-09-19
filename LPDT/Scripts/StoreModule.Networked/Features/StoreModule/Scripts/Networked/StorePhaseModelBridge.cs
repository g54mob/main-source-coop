using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.StoreModule.Scripts.Networked
{
	public class StorePhaseModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly StorePhaseModel _storePhaseModel;

		private StorePhaseNetworkObject _storePhaseNetworkObject;

		private bool _hasPendingIsStoreActive;

		private bool _pendingIsStoreActive;

		public StorePhaseModelBridge(StorePhaseModel storePhaseModel)
		{
			_storePhaseModel = storePhaseModel;
		}

		public void Bind(StorePhaseNetworkObject storePhaseNetworkObject)
		{
			Unbind();
			_storePhaseNetworkObject = storePhaseNetworkObject;
			_storePhaseNetworkObject.OnNetworkedIsStoreActiveChanged += HandleNetworkedIsStoreActiveChanged;
			_storePhaseNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_storePhaseNetworkObject.OnDespawned += HandleDespawned;
			_storePhaseModel.IsStoreActive.BindWriter(WriteIsStoreActive);
			ApplyIsStoreActiveFromNetwork(_storePhaseNetworkObject.IsStoreActive);
			_storePhaseModel.SetAuthorityProvider(() => _storePhaseNetworkObject.HasStateAuthority);
			_storePhaseModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_storePhaseNetworkObject == null))
			{
				_storePhaseNetworkObject.OnNetworkedIsStoreActiveChanged -= HandleNetworkedIsStoreActiveChanged;
				_storePhaseNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_storePhaseNetworkObject.OnDespawned -= HandleDespawned;
				_storePhaseModel.IsStoreActive.BindWriter(null);
				_storePhaseNetworkObject = null;
				_hasPendingIsStoreActive = false;
				_storePhaseModel.SetAuthorityProvider(null);
				_storePhaseModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<StorePhaseNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedIsStoreActiveChanged(bool isStoreActive)
		{
			ApplyIsStoreActiveFromNetwork(isStoreActive);
		}

		private void ApplyIsStoreActiveFromNetwork(bool isStoreActive)
		{
			_storePhaseModel.IsStoreActive.ApplyFromNetwork(isStoreActive);
		}

		private bool WriteIsStoreActive(bool value)
		{
			if (!_storePhaseModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] StorePhaseModel.IsStoreActive was written without state authority; the write was ignored.");
				return false;
			}
			_pendingIsStoreActive = value;
			_hasPendingIsStoreActive = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_storePhaseNetworkObject == null || _storePhaseNetworkObject.Object == null || _storePhaseNetworkObject.Runner == null)
			{
				return false;
			}
			if (_storePhaseNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_storePhaseNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingIsStoreActive)
			{
				_hasPendingIsStoreActive = false;
				_storePhaseNetworkObject.TryWriteIsStoreActive(_pendingIsStoreActive);
			}
		}
	}
}
