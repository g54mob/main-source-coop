using System;
using System.Collections.Generic;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.MultiplayerSessionServices.Scripts.Networked
{
	public class PlayerJoinSourcesModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly PlayerJoinSourcesModel _playerJoinSourcesModel;

		private PlayerJoinSourcesNetworkObject _playerJoinSourcesNetworkObject;

		private bool _hasPendingSources;

		private Dictionary<int, JoinSource> _pendingSources;

		public PlayerJoinSourcesModelBridge(PlayerJoinSourcesModel playerJoinSourcesModel)
		{
			_playerJoinSourcesModel = playerJoinSourcesModel;
		}

		public void Bind(PlayerJoinSourcesNetworkObject playerJoinSourcesNetworkObject)
		{
			Unbind();
			_playerJoinSourcesNetworkObject = playerJoinSourcesNetworkObject;
			_playerJoinSourcesNetworkObject.OnNetworkedSourcesChanged += HandleNetworkedSourcesChanged;
			_playerJoinSourcesNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_playerJoinSourcesNetworkObject.OnDespawned += HandleDespawned;
			_playerJoinSourcesNetworkObject.OnReportSignalReceived += HandleReportSignalReceived;
			_playerJoinSourcesModel.Sources.BindWriter(WriteSources);
			_playerJoinSourcesModel.ReportSignal.BindSender(_playerJoinSourcesNetworkObject.RpcRaiseReportSignal, "PlayerJoinSourcesModel.ReportSignal");
			ApplySourcesFromNetwork(_playerJoinSourcesNetworkObject.ReadSources());
			_playerJoinSourcesModel.SetAuthorityProvider(() => _playerJoinSourcesNetworkObject.HasStateAuthority);
			_playerJoinSourcesModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_playerJoinSourcesNetworkObject == null))
			{
				_playerJoinSourcesNetworkObject.OnNetworkedSourcesChanged -= HandleNetworkedSourcesChanged;
				_playerJoinSourcesNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_playerJoinSourcesNetworkObject.OnDespawned -= HandleDespawned;
				_playerJoinSourcesNetworkObject.OnReportSignalReceived -= HandleReportSignalReceived;
				_playerJoinSourcesModel.Sources.BindWriter(null);
				_playerJoinSourcesModel.ReportSignal.BindSender(null, null);
				_playerJoinSourcesNetworkObject = null;
				_hasPendingSources = false;
				_playerJoinSourcesModel.SetAuthorityProvider(null);
				_playerJoinSourcesModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<PlayerJoinSourcesNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleReportSignalReceived(int value)
		{
			_playerJoinSourcesModel.ReportSignal.RaiseReceived(value);
		}

		private void HandleNetworkedSourcesChanged(IReadOnlyDictionary<int, JoinSource> sources)
		{
			ApplySourcesFromNetwork(sources);
		}

		private void ApplySourcesFromNetwork(IReadOnlyDictionary<int, JoinSource> sources)
		{
			_playerJoinSourcesModel.Sources.ApplyFromNetwork(sources);
		}

		private bool WriteSources(IReadOnlyDictionary<int, JoinSource> value)
		{
			if (!_playerJoinSourcesModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] PlayerJoinSourcesModel.Sources was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSources = new Dictionary<int, JoinSource>();
			foreach (KeyValuePair<int, JoinSource> item in value)
			{
				_pendingSources[item.Key] = item.Value;
			}
			_hasPendingSources = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_playerJoinSourcesNetworkObject == null || _playerJoinSourcesNetworkObject.Object == null || _playerJoinSourcesNetworkObject.Runner == null)
			{
				return false;
			}
			if (_playerJoinSourcesNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_playerJoinSourcesNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingSources)
			{
				_hasPendingSources = false;
				_playerJoinSourcesNetworkObject.TryWriteSources(_pendingSources);
			}
		}
	}
}
