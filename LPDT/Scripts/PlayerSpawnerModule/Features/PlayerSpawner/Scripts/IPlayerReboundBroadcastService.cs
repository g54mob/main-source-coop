using Fusion;

namespace Features.PlayerSpawner.Scripts
{
	public interface IPlayerReboundBroadcastService
	{
		void Broadcast(PlayerRef playerRef, NetworkObject avatar);
	}
}
