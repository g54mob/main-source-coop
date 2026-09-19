namespace Features.PlayerIdentityModule
{
	public class FixedPersistentPlayerIdProvider : IPersistentPlayerIdProvider
	{
		public PersistentPlayerId LocalId { get; }

		public FixedPersistentPlayerIdProvider(PersistentPlayerId localId)
		{
			LocalId = localId;
		}
	}
}
