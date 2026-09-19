using System;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;

namespace Features.SessionManagementModule.Networked
{
	public class ShopVoteModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly ShopVoteModel _shopVoteModel;

		private ShopVoteNetworkObject _shopVoteNetworkObject;

		private bool _hasPendingHasVotedToLeave;

		private bool _pendingHasVotedToLeave;

		public ShopVoteModelBridge(ShopVoteModel shopVoteModel)
		{
			_shopVoteModel = shopVoteModel;
		}

		public void Bind(ShopVoteNetworkObject shopVoteNetworkObject)
		{
			Unbind();
			_shopVoteNetworkObject = shopVoteNetworkObject;
			_shopVoteNetworkObject.OnNetworkedHasVotedToLeaveChanged += HandleNetworkedHasVotedToLeaveChanged;
			_shopVoteNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_shopVoteNetworkObject.OnDespawned += HandleDespawned;
			_shopVoteModel.HasVotedToLeave.BindWriter(WriteHasVotedToLeave);
			ApplyHasVotedToLeaveFromNetwork(_shopVoteNetworkObject.HasVotedToLeave);
			_shopVoteModel.SetAuthorityProvider(() => _shopVoteNetworkObject.HasStateAuthority);
			_shopVoteModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_shopVoteNetworkObject == null))
			{
				_shopVoteNetworkObject.OnNetworkedHasVotedToLeaveChanged -= HandleNetworkedHasVotedToLeaveChanged;
				_shopVoteNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_shopVoteNetworkObject.OnDespawned -= HandleDespawned;
				_shopVoteModel.HasVotedToLeave.BindWriter(null);
				_shopVoteNetworkObject = null;
				_hasPendingHasVotedToLeave = false;
				_shopVoteModel.SetAuthorityProvider(null);
				_shopVoteModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<ShopVoteNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedHasVotedToLeaveChanged(bool hasVotedToLeave)
		{
			ApplyHasVotedToLeaveFromNetwork(hasVotedToLeave);
		}

		private void ApplyHasVotedToLeaveFromNetwork(bool hasVotedToLeave)
		{
			_shopVoteModel.HasVotedToLeave.ApplyFromNetwork(hasVotedToLeave);
		}

		private bool WriteHasVotedToLeave(bool value)
		{
			if (!_shopVoteModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] ShopVoteModel.HasVotedToLeave was written without state authority; the write was ignored.");
				return false;
			}
			_pendingHasVotedToLeave = value;
			_hasPendingHasVotedToLeave = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_shopVoteNetworkObject == null || _shopVoteNetworkObject.Object == null || _shopVoteNetworkObject.Runner == null)
			{
				return false;
			}
			if (_shopVoteNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_shopVoteNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingHasVotedToLeave)
			{
				_hasPendingHasVotedToLeave = false;
				_shopVoteNetworkObject.TryWriteHasVotedToLeave(_pendingHasVotedToLeave);
			}
		}
	}
}
