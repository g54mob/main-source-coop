namespace Features.PlayerIdentityModule
{
	public interface IPersistentPlayerIdProvider
	{
		PersistentPlayerId LocalId { get; }
	}
}
