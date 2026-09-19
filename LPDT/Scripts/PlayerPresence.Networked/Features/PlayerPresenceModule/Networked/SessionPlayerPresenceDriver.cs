using System.Collections.Generic;
using Features.PlayerIdentityModule;
using Fusion;

namespace Features.PlayerPresenceModule.Networked
{
	public class SessionPlayerPresenceDriver
	{
		public const int DEFAULT_SPAWN_SETTLE_FRAMES = 16;

		private readonly NetworkRunner _runner;

		private readonly PersistentPlayerId _localId;

		private readonly NetworkObject _sessionPlayerPrefab;

		private readonly SessionPlayerBridge _sessionPlayerBridge;

		private readonly int _spawnSettleFrames;

		private readonly List<SessionPlayerNetworkObject> _observed = new List<SessionPlayerNetworkObject>();

		private readonly List<SessionPlayerObservation> _observations = new List<SessionPlayerObservation>();

		private SessionPlayerNetworkObject _bound;

		private bool _isInScope;

		private int _spawnDeferredFrames;

		public SessionPlayerNetworkObject BoundObject
		{
			get
			{
				if (!HasLiveBinding())
				{
					return null;
				}
				return _bound;
			}
		}

		public bool IsInScope => _isInScope;

		public SessionPlayerPresenceDriver(NetworkRunner runner, PersistentPlayerId localId, NetworkObject sessionPlayerPrefab, SessionPlayerBridge sessionPlayerBridge, int spawnSettleFrames = 16)
		{
			_runner = runner;
			_localId = localId;
			_sessionPlayerPrefab = sessionPlayerPrefab;
			_sessionPlayerBridge = sessionPlayerBridge;
			_spawnSettleFrames = spawnSettleFrames;
		}

		public void SetScope(bool isInScope)
		{
			_isInScope = isInScope;
		}

		public void Reconcile()
		{
			if (_bound != null && !HasLiveBinding())
			{
				DropBinding();
			}
			if (HasLiveBinding() && _bound.OwnerIdValue.Equals(_localId))
			{
				_spawnDeferredFrames = 0;
				if (!_isInScope)
				{
					if (_bound.HasStateAuthority)
					{
						Release(_bound);
					}
					else
					{
						DropBinding();
					}
				}
				return;
			}
			BuildObservations();
			SessionPlayerReconcileResult sessionPlayerReconcileResult = SessionPlayerReconciler.Reconcile(_localId, _isInScope, _observations);
			switch (sessionPlayerReconcileResult.Action)
			{
			case SessionPlayerReconcileAction.Spawn:
				if (_spawnDeferredFrames < _spawnSettleFrames)
				{
					_spawnDeferredFrames++;
					break;
				}
				_spawnDeferredFrames = 0;
				Spawn();
				break;
			case SessionPlayerReconcileAction.Reclaim:
				_spawnDeferredFrames = 0;
				Reclaim(_observed[sessionPlayerReconcileResult.TargetIndex]);
				break;
			case SessionPlayerReconcileAction.Hold:
				_spawnDeferredFrames = 0;
				BindTo(_observed[sessionPlayerReconcileResult.TargetIndex]);
				break;
			case SessionPlayerReconcileAction.Release:
				_spawnDeferredFrames = 0;
				Release(_observed[sessionPlayerReconcileResult.TargetIndex]);
				break;
			default:
				_spawnDeferredFrames = 0;
				break;
			}
		}

		public void Dispose()
		{
			DropBinding();
		}

		private void BuildObservations()
		{
			_observed.Clear();
			_observations.Clear();
			foreach (NetworkObject allNetworkObject in _runner.GetAllNetworkObjects())
			{
				if (allNetworkObject.TryGetComponent<SessionPlayerNetworkObject>(out var component))
				{
					_observed.Add(component);
					_observations.Add(new SessionPlayerObservation(component.OwnerIdValue, component.HasStateAuthority));
				}
			}
		}

		private void Spawn()
		{
			NetworkObject networkObject = _runner.Spawn(_sessionPlayerPrefab);
			if (!networkObject.TryGetComponent<SessionPlayerNetworkObject>(out var component) || !component.TryClaimOwnership(_localId))
			{
				_runner.Despawn(networkObject);
			}
			else
			{
				BindTo(component);
			}
		}

		private void Reclaim(SessionPlayerNetworkObject target)
		{
			if (target.TryReclaim(_localId))
			{
				BindTo(target);
			}
		}

		private void Release(SessionPlayerNetworkObject target)
		{
			DropBinding();
			if (target.Object != null && target.Object.IsValid)
			{
				_runner.Despawn(target.Object);
			}
		}

		private void BindTo(SessionPlayerNetworkObject target)
		{
			if (!(_bound == target))
			{
				_sessionPlayerBridge.Bind(target);
				_bound = target;
			}
		}

		private void DropBinding()
		{
			if (!(_bound == null))
			{
				_sessionPlayerBridge.Unbind();
				_bound = null;
			}
		}

		private bool HasLiveBinding()
		{
			if (_bound != null && _bound.Object != null)
			{
				return _bound.Object.IsValid;
			}
			return false;
		}
	}
}
