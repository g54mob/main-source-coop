using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.QuotaModule.Scripts.Networked
{
	public class QuotaCompletionModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly QuotaCompletionModel _quotaCompletionModel;

		private QuotaCompletionNetworkObject _quotaCompletionNetworkObject;

		private bool _hasPendingIsQuotaCompleted;

		private bool _pendingIsQuotaCompleted;

		private bool _hasPendingIsBellActivated;

		private bool _pendingIsBellActivated;

		public QuotaCompletionModelBridge(QuotaCompletionModel quotaCompletionModel)
		{
			_quotaCompletionModel = quotaCompletionModel;
		}

		public void Bind(QuotaCompletionNetworkObject quotaCompletionNetworkObject)
		{
			Unbind();
			_quotaCompletionNetworkObject = quotaCompletionNetworkObject;
			_quotaCompletionNetworkObject.OnNetworkedIsQuotaCompletedChanged += HandleNetworkedIsQuotaCompletedChanged;
			_quotaCompletionNetworkObject.OnNetworkedIsBellActivatedChanged += HandleNetworkedIsBellActivatedChanged;
			_quotaCompletionNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_quotaCompletionNetworkObject.OnDespawned += HandleDespawned;
			_quotaCompletionModel.IsQuotaCompleted.BindWriter(WriteIsQuotaCompleted);
			_quotaCompletionModel.IsBellActivated.BindWriter(WriteIsBellActivated);
			ApplyIsQuotaCompletedFromNetwork(_quotaCompletionNetworkObject.IsQuotaCompleted);
			ApplyIsBellActivatedFromNetwork(_quotaCompletionNetworkObject.IsBellActivated);
			_quotaCompletionModel.SetAuthorityProvider(() => _quotaCompletionNetworkObject.HasStateAuthority);
			_quotaCompletionModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_quotaCompletionNetworkObject == null))
			{
				_quotaCompletionNetworkObject.OnNetworkedIsQuotaCompletedChanged -= HandleNetworkedIsQuotaCompletedChanged;
				_quotaCompletionNetworkObject.OnNetworkedIsBellActivatedChanged -= HandleNetworkedIsBellActivatedChanged;
				_quotaCompletionNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_quotaCompletionNetworkObject.OnDespawned -= HandleDespawned;
				_quotaCompletionModel.IsQuotaCompleted.BindWriter(null);
				_quotaCompletionModel.IsBellActivated.BindWriter(null);
				_quotaCompletionNetworkObject = null;
				_hasPendingIsQuotaCompleted = false;
				_hasPendingIsBellActivated = false;
				_quotaCompletionModel.SetAuthorityProvider(null);
				_quotaCompletionModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<QuotaCompletionNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedIsQuotaCompletedChanged(bool isQuotaCompleted)
		{
			ApplyIsQuotaCompletedFromNetwork(isQuotaCompleted);
		}

		private void HandleNetworkedIsBellActivatedChanged(bool isBellActivated)
		{
			ApplyIsBellActivatedFromNetwork(isBellActivated);
		}

		private void ApplyIsQuotaCompletedFromNetwork(bool isQuotaCompleted)
		{
			_quotaCompletionModel.IsQuotaCompleted.ApplyFromNetwork(isQuotaCompleted);
		}

		private void ApplyIsBellActivatedFromNetwork(bool isBellActivated)
		{
			_quotaCompletionModel.IsBellActivated.ApplyFromNetwork(isBellActivated);
		}

		private bool WriteIsQuotaCompleted(bool value)
		{
			if (!_quotaCompletionModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] QuotaCompletionModel.IsQuotaCompleted was written without state authority; the write was ignored.");
				return false;
			}
			_pendingIsQuotaCompleted = value;
			_hasPendingIsQuotaCompleted = true;
			return true;
		}

		private bool WriteIsBellActivated(bool value)
		{
			if (!_quotaCompletionModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] QuotaCompletionModel.IsBellActivated was written without state authority; the write was ignored.");
				return false;
			}
			_pendingIsBellActivated = value;
			_hasPendingIsBellActivated = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_quotaCompletionNetworkObject == null || _quotaCompletionNetworkObject.Object == null || _quotaCompletionNetworkObject.Runner == null)
			{
				return false;
			}
			if (_quotaCompletionNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_quotaCompletionNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingIsQuotaCompleted)
			{
				_hasPendingIsQuotaCompleted = false;
				_quotaCompletionNetworkObject.TryWriteIsQuotaCompleted(_pendingIsQuotaCompleted);
			}
			if (_hasPendingIsBellActivated)
			{
				_hasPendingIsBellActivated = false;
				_quotaCompletionNetworkObject.TryWriteIsBellActivated(_pendingIsBellActivated);
			}
		}
	}
}
