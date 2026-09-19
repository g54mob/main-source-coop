using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Fusion;

namespace Features.LevelModule.Scripts.LevelTransition
{
	public class LevelTransitionBeachReadinessService : ILevelTransitionBeachReadiness
	{
		private readonly MultiplayerModel _multiplayerModel;

		private readonly BeachOccupancyModel _beachOccupancyModel;

		private readonly IPlayerStateService _playerStateService;

		public LevelTransitionBeachReadinessService(MultiplayerModel multiplayerModel, BeachOccupancyModel beachOccupancyModel, IPlayerStateService playerStateService)
		{
			_multiplayerModel = multiplayerModel;
			_beachOccupancyModel = beachOccupancyModel;
			_playerStateService = playerStateService;
		}

		public bool AreAllActivePlayersInBeach()
		{
			NetworkRunner networkRunner = _multiplayerModel.NetworkRunner;
			if (networkRunner == null || !networkRunner.IsRunning)
			{
				return false;
			}
			LevelTransitionArea.RescanAllAreas();
			int num = 0;
			foreach (PlayerRef activePlayer in networkRunner.ActivePlayers)
			{
				if (!_playerStateService.IsPlayerDead(activePlayer.PlayerId))
				{
					num++;
					if (!_beachOccupancyModel.Players.ContainsKey(activePlayer))
					{
						return false;
					}
				}
			}
			return num > 0;
		}
	}
}
