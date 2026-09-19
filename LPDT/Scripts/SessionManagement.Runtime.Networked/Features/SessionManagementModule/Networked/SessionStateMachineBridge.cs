using System;
using Features.NetworkedModelRuntime;
using Features.SessionManagementModule.Models;
using Fusion;
using UnityEngine;

namespace Features.SessionManagementModule.Networked
{
	public class SessionStateMachineBridge : IDisposable, INetworkedModelShadowBridge
	{
		private readonly SessionStateMachine _sessionStateMachine;

		private SessionStateMachineNetworkObject _sessionStateMachineNetworkObject;

		private bool _hasPendingTarget;

		private SessionState _pendingTarget;

		private bool _hasPendingIsTargetActive;

		private bool _pendingIsTargetActive;

		private bool _hasPendingEpoch;

		private int _pendingEpoch;

		public SessionStateMachineBridge(SessionStateMachine sessionStateMachine)
		{
			_sessionStateMachine = sessionStateMachine;
		}

		public void Bind(SessionStateMachineNetworkObject sessionStateMachineNetworkObject)
		{
			Unbind();
			_sessionStateMachineNetworkObject = sessionStateMachineNetworkObject;
			_sessionStateMachineNetworkObject.OnNetworkedTargetChanged += HandleNetworkedTargetChanged;
			_sessionStateMachineNetworkObject.OnNetworkedIsTargetActiveChanged += HandleNetworkedIsTargetActiveChanged;
			_sessionStateMachineNetworkObject.OnNetworkedEpochChanged += HandleNetworkedEpochChanged;
			_sessionStateMachineNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_sessionStateMachineNetworkObject.OnDespawned += HandleDespawned;
			_sessionStateMachineNetworkObject.OnRender += HandleRender;
			_sessionStateMachine.Target.BindWriter(WriteTarget);
			_sessionStateMachine.IsTargetActive.BindWriter(WriteIsTargetActive);
			_sessionStateMachine.Epoch.BindWriter(WriteEpoch);
			ApplyTargetFromNetwork(_sessionStateMachineNetworkObject.Target);
			ApplyIsTargetActiveFromNetwork(_sessionStateMachineNetworkObject.IsTargetActive);
			ApplyEpochFromNetwork(_sessionStateMachineNetworkObject.Epoch);
			_sessionStateMachine.SetAuthorityProvider(() => _sessionStateMachineNetworkObject.HasStateAuthority);
			_sessionStateMachine.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_sessionStateMachineNetworkObject == null))
			{
				_sessionStateMachineNetworkObject.OnNetworkedTargetChanged -= HandleNetworkedTargetChanged;
				_sessionStateMachineNetworkObject.OnNetworkedIsTargetActiveChanged -= HandleNetworkedIsTargetActiveChanged;
				_sessionStateMachineNetworkObject.OnNetworkedEpochChanged -= HandleNetworkedEpochChanged;
				_sessionStateMachineNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_sessionStateMachineNetworkObject.OnDespawned -= HandleDespawned;
				_sessionStateMachineNetworkObject.OnRender -= HandleRender;
				_sessionStateMachine.Target.BindWriter(null);
				_sessionStateMachine.IsTargetActive.BindWriter(null);
				_sessionStateMachine.Epoch.BindWriter(null);
				_sessionStateMachineNetworkObject = null;
				_hasPendingTarget = false;
				_hasPendingIsTargetActive = false;
				_hasPendingEpoch = false;
				_sessionStateMachine.SetAuthorityProvider(null);
				_sessionStateMachine.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		public void BindObject(NetworkObject networkObject)
		{
			Bind(networkObject.GetComponent<SessionStateMachineNetworkObject>());
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleRender()
		{
			_sessionStateMachine.RaiseFusionUpdate();
		}

		private void HandleNetworkedTargetChanged(SessionState target)
		{
			ApplyTargetFromNetwork(target);
		}

		private void HandleNetworkedIsTargetActiveChanged(bool isTargetActive)
		{
			ApplyIsTargetActiveFromNetwork(isTargetActive);
		}

		private void HandleNetworkedEpochChanged(int epoch)
		{
			ApplyEpochFromNetwork(epoch);
		}

		private void ApplyTargetFromNetwork(SessionState target)
		{
			_sessionStateMachine.Target.ApplyFromNetwork(target);
		}

		private void ApplyIsTargetActiveFromNetwork(bool isTargetActive)
		{
			_sessionStateMachine.IsTargetActive.ApplyFromNetwork(isTargetActive);
		}

		private void ApplyEpochFromNetwork(int epoch)
		{
			_sessionStateMachine.Epoch.ApplyFromNetwork(epoch);
		}

		private bool WriteTarget(SessionState value)
		{
			if (!_sessionStateMachine.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] SessionStateMachine.Target was written without state authority; the write was ignored.");
				return false;
			}
			_pendingTarget = value;
			_hasPendingTarget = true;
			return true;
		}

		private bool WriteIsTargetActive(bool value)
		{
			if (!_sessionStateMachine.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] SessionStateMachine.IsTargetActive was written without state authority; the write was ignored.");
				return false;
			}
			_pendingIsTargetActive = value;
			_hasPendingIsTargetActive = true;
			return true;
		}

		private bool WriteEpoch(int value)
		{
			if (!_sessionStateMachine.IsAuthority && !IsAuthorityMigratingToMaster())
			{
				Debug.LogError("[NetworkedModel] SessionStateMachine.Epoch was written without state authority; the write was ignored.");
				return false;
			}
			_pendingEpoch = value;
			_hasPendingEpoch = true;
			return true;
		}

		private bool IsAuthorityMigratingToMaster()
		{
			if (_sessionStateMachineNetworkObject == null || _sessionStateMachineNetworkObject.Object == null || _sessionStateMachineNetworkObject.Runner == null)
			{
				return false;
			}
			if (_sessionStateMachineNetworkObject.Runner.IsSharedModeMasterClient)
			{
				return (_sessionStateMachineNetworkObject.Object.Flags & NetworkObjectFlags.MasterClientObject) != 0;
			}
			return false;
		}

		private void HandleAuthoritativeTick()
		{
			if (_hasPendingTarget)
			{
				_hasPendingTarget = false;
				_sessionStateMachineNetworkObject.TryWriteTarget(_pendingTarget);
			}
			if (_hasPendingIsTargetActive)
			{
				_hasPendingIsTargetActive = false;
				_sessionStateMachineNetworkObject.TryWriteIsTargetActive(_pendingIsTargetActive);
			}
			if (_hasPendingEpoch)
			{
				_hasPendingEpoch = false;
				_sessionStateMachineNetworkObject.TryWriteEpoch(_pendingEpoch);
			}
		}
	}
}
