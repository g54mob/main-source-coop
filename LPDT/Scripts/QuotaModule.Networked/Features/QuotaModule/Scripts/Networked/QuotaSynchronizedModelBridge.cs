using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.QuotaModule.Scripts.Networked
{
	public class QuotaSynchronizedModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly QuotaSynchronizedModel _quotaSynchronizedModel;

		private QuotaSynchronizedNetworkObject _quotaSynchronizedNetworkObject;

		private bool _hasPendingCurrentQuota;

		private float _pendingCurrentQuota;

		private bool _hasPendingMaxQuota;

		private float _pendingMaxQuota;

		public QuotaSynchronizedModelBridge(QuotaSynchronizedModel quotaSynchronizedModel)
		{
			_quotaSynchronizedModel = quotaSynchronizedModel;
		}

		public void Bind(QuotaSynchronizedNetworkObject quotaSynchronizedNetworkObject)
		{
			Unbind();
			_quotaSynchronizedNetworkObject = quotaSynchronizedNetworkObject;
			_quotaSynchronizedNetworkObject.OnNetworkedCurrentQuotaChanged += HandleNetworkedCurrentQuotaChanged;
			_quotaSynchronizedNetworkObject.OnNetworkedMaxQuotaChanged += HandleNetworkedMaxQuotaChanged;
			_quotaSynchronizedNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_quotaSynchronizedNetworkObject.OnDespawned += HandleDespawned;
			_quotaSynchronizedModel.CurrentQuota.BindWriter(WriteCurrentQuota);
			_quotaSynchronizedModel.MaxQuota.BindWriter(WriteMaxQuota);
			ApplyCurrentQuotaFromNetwork(_quotaSynchronizedNetworkObject.CurrentQuota);
			ApplyMaxQuotaFromNetwork(_quotaSynchronizedNetworkObject.MaxQuota);
			_quotaSynchronizedModel.SetAuthorityProvider(() => _quotaSynchronizedNetworkObject.HasStateAuthority);
			_quotaSynchronizedModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_quotaSynchronizedNetworkObject == null))
			{
				_quotaSynchronizedNetworkObject.OnNetworkedCurrentQuotaChanged -= HandleNetworkedCurrentQuotaChanged;
				_quotaSynchronizedNetworkObject.OnNetworkedMaxQuotaChanged -= HandleNetworkedMaxQuotaChanged;
				_quotaSynchronizedNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_quotaSynchronizedNetworkObject.OnDespawned -= HandleDespawned;
				_quotaSynchronizedModel.CurrentQuota.BindWriter(null);
				_quotaSynchronizedModel.MaxQuota.BindWriter(null);
				_quotaSynchronizedNetworkObject = null;
				_hasPendingCurrentQuota = false;
				_hasPendingMaxQuota = false;
				_quotaSynchronizedModel.SetAuthorityProvider(null);
				_quotaSynchronizedModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<QuotaSynchronizedNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedCurrentQuotaChanged(float currentQuota)
		{
			ApplyCurrentQuotaFromNetwork(currentQuota);
		}

		private void HandleNetworkedMaxQuotaChanged(float maxQuota)
		{
			ApplyMaxQuotaFromNetwork(maxQuota);
		}

		private void ApplyCurrentQuotaFromNetwork(float currentQuota)
		{
			_quotaSynchronizedModel.CurrentQuota.ApplyFromNetwork(currentQuota);
		}

		private void ApplyMaxQuotaFromNetwork(float maxQuota)
		{
			_quotaSynchronizedModel.MaxQuota.ApplyFromNetwork(maxQuota);
		}

		private bool WriteCurrentQuota(float value)
		{
			if (!_quotaSynchronizedModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] QuotaSynchronizedModel.CurrentQuota was written without state authority; the write was ignored.");
				return false;
			}
			_pendingCurrentQuota = value;
			_hasPendingCurrentQuota = true;
			return true;
		}

		private bool WriteMaxQuota(float value)
		{
			if (!_quotaSynchronizedModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] QuotaSynchronizedModel.MaxQuota was written without state authority; the write was ignored.");
				return false;
			}
			_pendingMaxQuota = value;
			_hasPendingMaxQuota = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_quotaSynchronizedNetworkObject == null || _quotaSynchronizedNetworkObject.Object == null || _quotaSynchronizedNetworkObject.Runner == null)
			{
				return false;
			}
			if (_quotaSynchronizedNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_quotaSynchronizedNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingCurrentQuota)
			{
				_hasPendingCurrentQuota = false;
				_quotaSynchronizedNetworkObject.TryWriteCurrentQuota(_pendingCurrentQuota);
			}
			if (_hasPendingMaxQuota)
			{
				_hasPendingMaxQuota = false;
				_quotaSynchronizedNetworkObject.TryWriteMaxQuota(_pendingMaxQuota);
			}
		}
	}
}
