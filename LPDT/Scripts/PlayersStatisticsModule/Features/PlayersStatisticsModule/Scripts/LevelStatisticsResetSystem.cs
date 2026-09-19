using Features.MultiplayerSessionServices.Scripts;

namespace Features.PlayersStatisticsModule.Scripts
{
	public class LevelStatisticsResetSystem : ILevelStatisticsReset
	{
		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		public LevelStatisticsResetSystem(LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel)
		{
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
		}

		public void ResetForNewLevel()
		{
			_levelPlayersGameStatisticsModel.RaiseReset();
		}
	}
}
