using Features.PlayerSpawner.Scripts;
using Fusion;

namespace Features.BootstrapModule.Scripts.Systems
{
	public class PlayerReboundBroadcastSystem : IPlayerReboundBroadcastService
	{
		private readonly PlayerReboundModel _playerReboundModel;

		public PlayerReboundBroadcastSystem(PlayerReboundModel playerReboundModel)
		{
			_playerReboundModel = playerReboundModel;
		}

		public void Broadcast(PlayerRef playerRef, NetworkObject avatar)
		{
			_playerReboundModel.NotifyRebound(playerRef, avatar);
		}
	}
}
