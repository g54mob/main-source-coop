using System;
using Features.NetworkedCodegenProbeModule.Data;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.NetworkedCodegenProbeModule.Networked
{
	public class ProbeFlagModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly ProbeFlagModel _probeFlagModel;

		private ProbeFlagNetworkObject _probeFlagNetworkObject;

		private bool _hasPendingLevel;

		private float _pendingLevel;

		public ProbeFlagModelBridge(ProbeFlagModel probeFlagModel)
		{
			_probeFlagModel = probeFlagModel;
		}

		public void Bind(ProbeFlagNetworkObject probeFlagNetworkObject)
		{
			Unbind();
			_probeFlagNetworkObject = probeFlagNetworkObject;
			_probeFlagNetworkObject.OnNetworkedLevelChanged += HandleNetworkedLevelChanged;
			_probeFlagNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_probeFlagNetworkObject.OnDespawned += HandleDespawned;
			_probeFlagModel.Level.BindWriter(WriteLevel);
			ApplyLevelFromNetwork(_probeFlagNetworkObject.Level);
			_probeFlagModel.SetAuthorityProvider(() => _probeFlagNetworkObject.HasStateAuthority);
			_probeFlagModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_probeFlagNetworkObject == null))
			{
				_probeFlagNetworkObject.OnNetworkedLevelChanged -= HandleNetworkedLevelChanged;
				_probeFlagNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_probeFlagNetworkObject.OnDespawned -= HandleDespawned;
				_probeFlagModel.Level.BindWriter(null);
				_probeFlagNetworkObject = null;
				_hasPendingLevel = false;
				_probeFlagModel.SetAuthorityProvider(null);
				_probeFlagModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<ProbeFlagNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedLevelChanged(float level)
		{
			ApplyLevelFromNetwork(level);
		}

		private void ApplyLevelFromNetwork(float level)
		{
			_probeFlagModel.Level.ApplyFromNetwork(level);
		}

		private bool WriteLevel(float value)
		{
			if (!_probeFlagModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] ProbeFlagModel.Level was written without state authority; the write was ignored.");
				return false;
			}
			_pendingLevel = value;
			_hasPendingLevel = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_probeFlagNetworkObject == null || _probeFlagNetworkObject.Object == null || _probeFlagNetworkObject.Runner == null)
			{
				return false;
			}
			if (_probeFlagNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_probeFlagNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingLevel)
			{
				_hasPendingLevel = false;
				_probeFlagNetworkObject.TryWriteLevel(_pendingLevel);
			}
		}
	}
}
