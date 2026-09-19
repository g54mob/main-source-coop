using System;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;

namespace Features.SessionManagementModule.Networked
{
	public class LevelPlayerModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly LevelPlayerModel _levelPlayerModel;

		private LevelPlayerNetworkObject _levelPlayerNetworkObject;

		private bool _hasPendingOwnerSlot;

		private int _pendingOwnerSlot;

		public LevelPlayerModelBridge(LevelPlayerModel levelPlayerModel)
		{
			_levelPlayerModel = levelPlayerModel;
		}

		public void Bind(LevelPlayerNetworkObject levelPlayerNetworkObject)
		{
			Unbind();
			_levelPlayerNetworkObject = levelPlayerNetworkObject;
			_levelPlayerNetworkObject.OnNetworkedOwnerSlotChanged += HandleNetworkedOwnerSlotChanged;
			_levelPlayerNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_levelPlayerNetworkObject.OnDespawned += HandleDespawned;
			_levelPlayerModel.OwnerSlot.BindWriter(WriteOwnerSlot);
			ApplyOwnerSlotFromNetwork(_levelPlayerNetworkObject.OwnerSlot);
			_levelPlayerModel.SetAuthorityProvider(() => _levelPlayerNetworkObject.HasStateAuthority);
			_levelPlayerModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_levelPlayerNetworkObject == null))
			{
				_levelPlayerNetworkObject.OnNetworkedOwnerSlotChanged -= HandleNetworkedOwnerSlotChanged;
				_levelPlayerNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_levelPlayerNetworkObject.OnDespawned -= HandleDespawned;
				_levelPlayerModel.OwnerSlot.BindWriter(null);
				_levelPlayerNetworkObject = null;
				_hasPendingOwnerSlot = false;
				_levelPlayerModel.SetAuthorityProvider(null);
				_levelPlayerModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<LevelPlayerNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedOwnerSlotChanged(int ownerSlot)
		{
			ApplyOwnerSlotFromNetwork(ownerSlot);
		}

		private void ApplyOwnerSlotFromNetwork(int ownerSlot)
		{
			_levelPlayerModel.OwnerSlot.ApplyFromNetwork(ownerSlot);
		}

		private bool WriteOwnerSlot(int value)
		{
			if (!_levelPlayerModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] LevelPlayerModel.OwnerSlot was written without state authority; the write was ignored.");
				return false;
			}
			_pendingOwnerSlot = value;
			_hasPendingOwnerSlot = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_levelPlayerNetworkObject == null || _levelPlayerNetworkObject.Object == null || _levelPlayerNetworkObject.Runner == null)
			{
				return false;
			}
			if (_levelPlayerNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_levelPlayerNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingOwnerSlot)
			{
				_hasPendingOwnerSlot = false;
				_levelPlayerNetworkObject.TryWriteOwnerSlot(_pendingOwnerSlot);
			}
		}
	}
}
