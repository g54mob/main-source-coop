using System;
using Features.PlayerIdentityModule;

namespace Features.PlayerPresenceModule
{
	public class SessionPlayerProfileModel
	{
		private bool _hasPendingProfile;

		private SessionPlayerProfileData _pendingProfile;

		public PersistentPlayerId OwnerId { get; private set; }

		public SessionPlayerProfileData Profile { get; private set; }

		public bool IsAttached { get; private set; }

		public event Action<SessionPlayerProfileData> OnProfileChanged;

		public event Action<PersistentPlayerId> OnOwnerChanged;

		public event Action<bool> OnAttachmentChanged;

		public void RequestProfile(SessionPlayerProfileData profile)
		{
			if (IsAttached)
			{
				_pendingProfile = profile;
				_hasPendingProfile = true;
			}
		}

		public bool TryConsumePendingProfile(out SessionPlayerProfileData profile)
		{
			profile = _pendingProfile;
			if (!_hasPendingProfile)
			{
				return false;
			}
			_hasPendingProfile = false;
			return true;
		}

		public void ApplyOwnerFromNetwork(PersistentPlayerId ownerId)
		{
			if (!OwnerId.Equals(ownerId))
			{
				OwnerId = ownerId;
				this.OnOwnerChanged?.Invoke(ownerId);
			}
		}

		public void ApplyProfileFromNetwork(SessionPlayerProfileData profile)
		{
			if (!Profile.Equals(profile))
			{
				Profile = profile;
				this.OnProfileChanged?.Invoke(profile);
			}
		}

		public void SetAttached(bool isAttached)
		{
			if (IsAttached != isAttached)
			{
				IsAttached = isAttached;
				if (!isAttached)
				{
					_hasPendingProfile = false;
				}
				this.OnAttachmentChanged?.Invoke(isAttached);
			}
		}
	}
}
