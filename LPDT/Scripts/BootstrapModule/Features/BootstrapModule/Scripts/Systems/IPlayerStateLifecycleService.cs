using Fusion;

namespace Features.BootstrapModule.Scripts.Systems
{
	public interface IPlayerStateLifecycleService
	{
		void PurgePlayerState(PlayerRef playerRef);

		void PurgePlayerState(int playerId);
	}
}
