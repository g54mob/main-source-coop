using System;
using Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Data;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.AIModuleStateMachine.Scripts.MonkeyPorterEnemy.Networked
{
	public class MonkeyPorterRunModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly MonkeyPorterRunModel _monkeyPorterRunModel;

		private MonkeyPorterRunNetworkObject _monkeyPorterRunNetworkObject;

		private bool _hasPendingIsOwned;

		private bool _pendingIsOwned;

		public MonkeyPorterRunModelBridge(MonkeyPorterRunModel monkeyPorterRunModel)
		{
			_monkeyPorterRunModel = monkeyPorterRunModel;
		}

		public void Bind(MonkeyPorterRunNetworkObject monkeyPorterRunNetworkObject)
		{
			Unbind();
			_monkeyPorterRunNetworkObject = monkeyPorterRunNetworkObject;
			_monkeyPorterRunNetworkObject.OnNetworkedIsOwnedChanged += HandleNetworkedIsOwnedChanged;
			_monkeyPorterRunNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_monkeyPorterRunNetworkObject.OnDespawned += HandleDespawned;
			_monkeyPorterRunModel.IsOwned.BindWriter(WriteIsOwned);
			ApplyIsOwnedFromNetwork(_monkeyPorterRunNetworkObject.IsOwned);
			_monkeyPorterRunModel.SetAuthorityProvider(() => _monkeyPorterRunNetworkObject.HasStateAuthority);
			_monkeyPorterRunModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_monkeyPorterRunNetworkObject == null))
			{
				_monkeyPorterRunNetworkObject.OnNetworkedIsOwnedChanged -= HandleNetworkedIsOwnedChanged;
				_monkeyPorterRunNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_monkeyPorterRunNetworkObject.OnDespawned -= HandleDespawned;
				_monkeyPorterRunModel.IsOwned.BindWriter(null);
				_monkeyPorterRunNetworkObject = null;
				_hasPendingIsOwned = false;
				_monkeyPorterRunModel.SetAuthorityProvider(null);
				_monkeyPorterRunModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<MonkeyPorterRunNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedIsOwnedChanged(bool isOwned)
		{
			ApplyIsOwnedFromNetwork(isOwned);
		}

		private void ApplyIsOwnedFromNetwork(bool isOwned)
		{
			_monkeyPorterRunModel.IsOwned.ApplyFromNetwork(isOwned);
		}

		private bool WriteIsOwned(bool value)
		{
			if (!_monkeyPorterRunModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] MonkeyPorterRunModel.IsOwned was written without state authority; the write was ignored.");
				return false;
			}
			_pendingIsOwned = value;
			_hasPendingIsOwned = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_monkeyPorterRunNetworkObject == null || _monkeyPorterRunNetworkObject.Object == null || _monkeyPorterRunNetworkObject.Runner == null)
			{
				return false;
			}
			if (_monkeyPorterRunNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_monkeyPorterRunNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingIsOwned)
			{
				_hasPendingIsOwned = false;
				_monkeyPorterRunNetworkObject.TryWriteIsOwned(_pendingIsOwned);
			}
		}
	}
}
