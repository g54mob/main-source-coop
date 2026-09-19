using Features.PlayerSpawner.Scripts;

namespace Features.AIModule.Scripts.Services
{
	public interface IPlayersLocationService
	{
		PlayerDataHolder GetLeastCrowdedPlayer(float detectionRadius);
	}
}
