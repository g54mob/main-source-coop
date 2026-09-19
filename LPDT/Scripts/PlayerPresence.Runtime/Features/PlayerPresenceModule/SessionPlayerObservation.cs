using Features.PlayerIdentityModule;

namespace Features.PlayerPresenceModule
{
	public readonly struct SessionPlayerObservation
	{
		public readonly PersistentPlayerId OwnerId;

		public readonly bool HasStateAuthority;

		public SessionPlayerObservation(PersistentPlayerId ownerId, bool hasStateAuthority)
		{
			OwnerId = ownerId;
			HasStateAuthority = hasStateAuthority;
		}
	}
}
