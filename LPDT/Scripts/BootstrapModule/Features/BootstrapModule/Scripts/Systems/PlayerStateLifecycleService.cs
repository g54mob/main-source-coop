using Features.LineArmModule.Scripts;
using Features.Movement.Scripts;
using Features.NavigationModule.Scripts;
using Features.PlayerSpawner.Scripts;
using Features.StatsUsageModule.Scripts.StatsData;
using Fusion;

namespace Features.BootstrapModule.Scripts.Systems
{
	public class PlayerStateLifecycleService : IPlayerStateLifecycleService
	{
		private readonly SpawnedPlayersModel _spawnedPlayersModel;

		private readonly LineArmsModel _lineArmsModel;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly PlayerRaycastPointsModel _playerRaycastPointsModel;

		private readonly IPlayerReboundBroadcastService _playerReboundBroadcastService;

		public PlayerStateLifecycleService(SpawnedPlayersModel spawnedPlayersModel, LineArmsModel lineArmsModel, PlayerMovableModel playerMovableModel, SpawnedEntityStatsModel spawnedEntityStatsModel, PlayerRaycastPointsModel playerRaycastPointsModel, IPlayerReboundBroadcastService playerReboundBroadcastService)
		{
			_spawnedPlayersModel = spawnedPlayersModel;
			_lineArmsModel = lineArmsModel;
			_playerMovableModel = playerMovableModel;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_playerRaycastPointsModel = playerRaycastPointsModel;
			_playerReboundBroadcastService = playerReboundBroadcastService;
		}

		public void PurgePlayerState(int playerId)
		{
			PurgePlayerState(PlayerRef.FromIndex(playerId));
		}

		public void PurgePlayerState(PlayerRef playerRef)
		{
			if (!(playerRef == PlayerRef.None))
			{
				int playerId = playerRef.PlayerId;
				_spawnedPlayersModel.UnregisterPlayer(playerRef);
				_lineArmsModel.UnregisterPlayer(playerId);
				_playerMovableModel.PurgePlayer(playerRef);
				_spawnedEntityStatsModel.UnregisterPlayerStats(playerId);
				_playerRaycastPointsModel.UnregisterPlayer(playerRef);
				_playerReboundBroadcastService.Broadcast(playerRef, null);
			}
		}
	}
}
