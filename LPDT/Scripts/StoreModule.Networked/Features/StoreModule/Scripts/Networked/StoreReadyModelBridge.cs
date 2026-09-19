using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.StoreModule.Scripts.Networked
{
	public class StoreReadyModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly StoreReadyModel _storeReadyModel;

		private StoreReadyNetworkObject _storeReadyNetworkObject;

		private bool _hasPendingIsReady;

		private bool _pendingIsReady;

		public StoreReadyModelBridge(StoreReadyModel storeReadyModel)
		{
			_storeReadyModel = storeReadyModel;
		}

		public void Bind(StoreReadyNetworkObject storeReadyNetworkObject)
		{
			Unbind();
			_storeReadyNetworkObject = storeReadyNetworkObject;
			_storeReadyNetworkObject.OnNetworkedIsReadyChanged += HandleNetworkedIsReadyChanged;
			_storeReadyNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_storeReadyNetworkObject.OnDespawned += HandleDespawned;
			_storeReadyModel.IsReady.BindWriter(WriteIsReady);
			ApplyIsReadyFromNetwork(_storeReadyNetworkObject.IsReady);
			_storeReadyModel.SetAuthorityProvider(() => _storeReadyNetworkObject.HasStateAuthority);
			_storeReadyModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_storeReadyNetworkObject == null))
			{
				_storeReadyNetworkObject.OnNetworkedIsReadyChanged -= HandleNetworkedIsReadyChanged;
				_storeReadyNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_storeReadyNetworkObject.OnDespawned -= HandleDespawned;
				_storeReadyModel.IsReady.BindWriter(null);
				_storeReadyNetworkObject = null;
				_hasPendingIsReady = false;
				_storeReadyModel.SetAuthorityProvider(null);
				_storeReadyModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<StoreReadyNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedIsReadyChanged(bool isReady)
		{
			ApplyIsReadyFromNetwork(isReady);
		}

		private void ApplyIsReadyFromNetwork(bool isReady)
		{
			_storeReadyModel.IsReady.ApplyFromNetwork(isReady);
		}

		private bool WriteIsReady(bool value)
		{
			if (!_storeReadyModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] StoreReadyModel.IsReady was written without state authority; the write was ignored.");
				return false;
			}
			_pendingIsReady = value;
			_hasPendingIsReady = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_storeReadyNetworkObject == null || _storeReadyNetworkObject.Object == null || _storeReadyNetworkObject.Runner == null)
			{
				return false;
			}
			if (_storeReadyNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_storeReadyNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingIsReady)
			{
				_hasPendingIsReady = false;
				_storeReadyNetworkObject.TryWriteIsReady(_pendingIsReady);
			}
		}
	}
}
