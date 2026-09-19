using System;
using Features.PlayerIdentityModule;

namespace Features.PlayerPresenceModule.Networked
{
	public class SessionPlayerBridge : IDisposable
	{
		private readonly SessionPlayerProfileModel _sessionPlayerProfileModel;

		private SessionPlayerNetworkObject _sessionPlayerNetworkObject;

		public SessionPlayerBridge(SessionPlayerProfileModel sessionPlayerProfileModel)
		{
			_sessionPlayerProfileModel = sessionPlayerProfileModel;
		}

		public void Bind(SessionPlayerNetworkObject sessionPlayerNetworkObject)
		{
			Unbind();
			_sessionPlayerNetworkObject = sessionPlayerNetworkObject;
			_sessionPlayerNetworkObject.OnNetworkedOwnerChanged += HandleNetworkedOwnerChanged;
			_sessionPlayerNetworkObject.OnNetworkedProfileChanged += HandleNetworkedProfileChanged;
			_sessionPlayerNetworkObject.OnAuthoritativeTick += HandleAuthoritativeTick;
			_sessionPlayerNetworkObject.OnDespawned += HandleDespawned;
			_sessionPlayerProfileModel.ApplyOwnerFromNetwork(_sessionPlayerNetworkObject.OwnerIdValue);
			_sessionPlayerProfileModel.ApplyProfileFromNetwork(_sessionPlayerNetworkObject.CurrentProfile);
			_sessionPlayerProfileModel.SetAttached(isAttached: true);
		}

		public void Unbind()
		{
			if (!(_sessionPlayerNetworkObject == null))
			{
				_sessionPlayerNetworkObject.OnNetworkedOwnerChanged -= HandleNetworkedOwnerChanged;
				_sessionPlayerNetworkObject.OnNetworkedProfileChanged -= HandleNetworkedProfileChanged;
				_sessionPlayerNetworkObject.OnAuthoritativeTick -= HandleAuthoritativeTick;
				_sessionPlayerNetworkObject.OnDespawned -= HandleDespawned;
				_sessionPlayerNetworkObject = null;
				_sessionPlayerProfileModel.SetAttached(isAttached: false);
			}
		}

		public void Dispose()
		{
			Unbind();
		}

		private void HandleDespawned()
		{
			Unbind();
		}

		private void HandleNetworkedOwnerChanged(PersistentPlayerId ownerId)
		{
			_sessionPlayerProfileModel.ApplyOwnerFromNetwork(ownerId);
		}

		private void HandleNetworkedProfileChanged(SessionPlayerProfileData profile)
		{
			_sessionPlayerProfileModel.ApplyProfileFromNetwork(profile);
		}

		private void HandleAuthoritativeTick()
		{
			if (_sessionPlayerProfileModel.TryConsumePendingProfile(out var profile))
			{
				_sessionPlayerNetworkObject.TryWriteProfile(profile);
			}
		}
	}
}
