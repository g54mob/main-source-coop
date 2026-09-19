using System;
using Features.GamePhasesModule.Scripts.Data;
using Features.NetworkedModelRuntime;
using Fusion;
using UnityEngine;

namespace Features.GamePhasesModule.Scripts.Networked
{
	public class GamePhasesModelBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly GamePhasesModel _gamePhasesModel;

		private GamePhasesNetworkObject _gamePhasesNetworkObject;

		private bool _hasPendingNetworkedCurrentPhaseCount;

		private int _pendingNetworkedCurrentPhaseCount;

		private bool _hasPendingNetworkedIsActive;

		private bool _pendingNetworkedIsActive;

		public GamePhasesModelBridge(GamePhasesModel gamePhasesModel)
		{
			_gamePhasesModel = gamePhasesModel;
		}

		public void Bind(GamePhasesNetworkObject gamePhasesNetworkObject)
		{
			Unbind();
			_gamePhasesNetworkObject = gamePhasesNetworkObject;
			_gamePhasesNetworkObject.OnNetworkedNetworkedCurrentPhaseCountChanged += HandleNetworkedNetworkedCurrentPhaseCountChanged;
			_gamePhasesNetworkObject.OnNetworkedNetworkedIsActiveChanged += HandleNetworkedNetworkedIsActiveChanged;
			_gamePhasesNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_gamePhasesNetworkObject.OnDespawned += HandleDespawned;
			_gamePhasesModel.NetworkedCurrentPhaseCount.BindWriter(WriteNetworkedCurrentPhaseCount);
			_gamePhasesModel.NetworkedIsActive.BindWriter(WriteNetworkedIsActive);
			ApplyNetworkedCurrentPhaseCountFromNetwork(_gamePhasesNetworkObject.NetworkedCurrentPhaseCount);
			ApplyNetworkedIsActiveFromNetwork(_gamePhasesNetworkObject.NetworkedIsActive);
			_gamePhasesModel.SetAuthorityProvider(() => _gamePhasesNetworkObject.HasStateAuthority);
			_gamePhasesModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_gamePhasesNetworkObject == null))
			{
				_gamePhasesNetworkObject.OnNetworkedNetworkedCurrentPhaseCountChanged -= HandleNetworkedNetworkedCurrentPhaseCountChanged;
				_gamePhasesNetworkObject.OnNetworkedNetworkedIsActiveChanged -= HandleNetworkedNetworkedIsActiveChanged;
				_gamePhasesNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_gamePhasesNetworkObject.OnDespawned -= HandleDespawned;
				_gamePhasesModel.NetworkedCurrentPhaseCount.BindWriter(null);
				_gamePhasesModel.NetworkedIsActive.BindWriter(null);
				_gamePhasesNetworkObject = null;
				_hasPendingNetworkedCurrentPhaseCount = false;
				_hasPendingNetworkedIsActive = false;
				_gamePhasesModel.SetAuthorityProvider(null);
				_gamePhasesModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<GamePhasesNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedNetworkedCurrentPhaseCountChanged(int networkedCurrentPhaseCount)
		{
			ApplyNetworkedCurrentPhaseCountFromNetwork(networkedCurrentPhaseCount);
		}

		private void HandleNetworkedNetworkedIsActiveChanged(bool networkedIsActive)
		{
			ApplyNetworkedIsActiveFromNetwork(networkedIsActive);
		}

		private void ApplyNetworkedCurrentPhaseCountFromNetwork(int networkedCurrentPhaseCount)
		{
			_gamePhasesModel.NetworkedCurrentPhaseCount.ApplyFromNetwork(networkedCurrentPhaseCount);
		}

		private void ApplyNetworkedIsActiveFromNetwork(bool networkedIsActive)
		{
			_gamePhasesModel.NetworkedIsActive.ApplyFromNetwork(networkedIsActive);
		}

		private bool WriteNetworkedCurrentPhaseCount(int value)
		{
			if (!_gamePhasesModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] GamePhasesModel.NetworkedCurrentPhaseCount was written without state authority; the write was ignored.");
				return false;
			}
			_pendingNetworkedCurrentPhaseCount = value;
			_hasPendingNetworkedCurrentPhaseCount = true;
			return true;
		}

		private bool WriteNetworkedIsActive(bool value)
		{
			if (!_gamePhasesModel.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] GamePhasesModel.NetworkedIsActive was written without state authority; the write was ignored.");
				return false;
			}
			_pendingNetworkedIsActive = value;
			_hasPendingNetworkedIsActive = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_gamePhasesNetworkObject == null || _gamePhasesNetworkObject.Object == null || _gamePhasesNetworkObject.Runner == null)
			{
				return false;
			}
			if (_gamePhasesNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_gamePhasesNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingNetworkedCurrentPhaseCount)
			{
				_hasPendingNetworkedCurrentPhaseCount = false;
				_gamePhasesNetworkObject.TryWriteNetworkedCurrentPhaseCount(_pendingNetworkedCurrentPhaseCount);
			}
			if (_hasPendingNetworkedIsActive)
			{
				_hasPendingNetworkedIsActive = false;
				_gamePhasesNetworkObject.TryWriteNetworkedIsActive(_pendingNetworkedIsActive);
			}
		}
	}
}
