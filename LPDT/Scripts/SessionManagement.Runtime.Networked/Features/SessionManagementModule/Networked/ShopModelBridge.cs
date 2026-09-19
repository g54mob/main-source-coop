using System;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;

namespace Features.SessionManagementModule.Networked
{
	public class ShopModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly ShopModel _shopModel;

		private ShopNetworkObject _shopNetworkObject;

		private bool _hasPendingRevision;

		private int _pendingRevision;

		public ShopModelBridge(ShopModel shopModel)
		{
			_shopModel = shopModel;
		}

		public void Bind(ShopNetworkObject shopNetworkObject)
		{
			Unbind();
			_shopNetworkObject = shopNetworkObject;
			_shopNetworkObject.OnNetworkedRevisionChanged += HandleNetworkedRevisionChanged;
			_shopNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_shopNetworkObject.OnDespawned += HandleDespawned;
			_shopModel.Revision.BindWriter(WriteRevision);
			ApplyRevisionFromNetwork(_shopNetworkObject.Revision);
			_shopModel.SetAuthorityProvider(() => _shopNetworkObject.HasStateAuthority);
			_shopModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_shopNetworkObject == null))
			{
				_shopNetworkObject.OnNetworkedRevisionChanged -= HandleNetworkedRevisionChanged;
				_shopNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_shopNetworkObject.OnDespawned -= HandleDespawned;
				_shopModel.Revision.BindWriter(null);
				_shopNetworkObject = null;
				_hasPendingRevision = false;
				_shopModel.SetAuthorityProvider(null);
				_shopModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<ShopNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedRevisionChanged(int revision)
		{
			ApplyRevisionFromNetwork(revision);
		}

		private void ApplyRevisionFromNetwork(int revision)
		{
			_shopModel.Revision.ApplyFromNetwork(revision);
		}

		private bool WriteRevision(int value)
		{
			if (!_shopModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] ShopModel.Revision was written without state authority; the write was ignored.");
				return false;
			}
			_pendingRevision = value;
			_hasPendingRevision = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_shopNetworkObject == null || _shopNetworkObject.Object == null || _shopNetworkObject.Runner == null)
			{
				return false;
			}
			if (_shopNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_shopNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingRevision)
			{
				_hasPendingRevision = false;
				_shopNetworkObject.TryWriteRevision(_pendingRevision);
			}
		}
	}
}
