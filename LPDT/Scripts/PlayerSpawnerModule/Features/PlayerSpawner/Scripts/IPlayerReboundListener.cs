using Fusion;

namespace Features.PlayerSpawner.Scripts
{
	public interface IPlayerReboundListener
	{
		void OnPlayerRebound(PlayerRef playerRef, NetworkObject avatar);
	}
}
