using System;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.WeatherModule.Scripts.Networked
{
	public class WeatherSelectionModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly WeatherSelectionModel _weatherSelectionModel;

		private WeatherSelectionNetworkObject _weatherSelectionNetworkObject;

		private bool _hasPendingSelection;

		private long _pendingSelection;

		public WeatherSelectionModelBridge(WeatherSelectionModel weatherSelectionModel)
		{
			_weatherSelectionModel = weatherSelectionModel;
		}

		public void Bind(WeatherSelectionNetworkObject weatherSelectionNetworkObject)
		{
			Unbind();
			_weatherSelectionNetworkObject = weatherSelectionNetworkObject;
			_weatherSelectionNetworkObject.OnNetworkedSelectionChanged += HandleNetworkedSelectionChanged;
			_weatherSelectionNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_weatherSelectionNetworkObject.OnDespawned += HandleDespawned;
			_weatherSelectionModel.Selection.BindWriter(WriteSelection);
			ApplySelectionFromNetwork(_weatherSelectionNetworkObject.Selection);
			_weatherSelectionModel.SetAuthorityProvider(() => _weatherSelectionNetworkObject.HasStateAuthority);
			_weatherSelectionModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_weatherSelectionNetworkObject == null))
			{
				_weatherSelectionNetworkObject.OnNetworkedSelectionChanged -= HandleNetworkedSelectionChanged;
				_weatherSelectionNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_weatherSelectionNetworkObject.OnDespawned -= HandleDespawned;
				_weatherSelectionModel.Selection.BindWriter(null);
				_weatherSelectionNetworkObject = null;
				_hasPendingSelection = false;
				_weatherSelectionModel.SetAuthorityProvider(null);
				_weatherSelectionModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<WeatherSelectionNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedSelectionChanged(long selection)
		{
			ApplySelectionFromNetwork(selection);
		}

		private void ApplySelectionFromNetwork(long selection)
		{
			_weatherSelectionModel.Selection.ApplyFromNetwork(selection);
		}

		private bool WriteSelection(long value)
		{
			if (!_weatherSelectionModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] WeatherSelectionModel.Selection was written without state authority; the write was ignored.");
				return false;
			}
			_pendingSelection = value;
			_hasPendingSelection = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_weatherSelectionNetworkObject == null || _weatherSelectionNetworkObject.Object == null || _weatherSelectionNetworkObject.Runner == null)
			{
				return false;
			}
			if (_weatherSelectionNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_weatherSelectionNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingSelection)
			{
				_hasPendingSelection = false;
				_weatherSelectionNetworkObject.TryWriteSelection(_pendingSelection);
			}
		}
	}
}
