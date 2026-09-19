namespace Features.PlayerSpawner.Scripts
{
	public interface IPlayerStatsInitializeService
	{
		void InitializeStats(int playerId, float initialHealth = -1f);

		void ApplyReconnectHealthClampedToMax(int playerId);
	}
}
