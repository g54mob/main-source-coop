using System;
using Fusion;

namespace Features.PlayerSpawner.Scripts
{
	public class PlayerReboundModel
	{
		public event Action<PlayerRef, NetworkObject> OnPlayerRebound;

		public event Action<PlayerRef> OnLocalAvatarDespawned;

		public void NotifyRebound(PlayerRef playerRef, NetworkObject avatar)
		{
			this.OnPlayerRebound?.Invoke(playerRef, avatar);
		}

		public void NotifyAvatarDespawned(PlayerRef playerRef)
		{
			this.OnLocalAvatarDespawned?.Invoke(playerRef);
		}
	}
}
