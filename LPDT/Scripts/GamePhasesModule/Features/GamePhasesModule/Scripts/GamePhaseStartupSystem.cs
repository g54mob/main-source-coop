using Features.GamePhasesModule.Scripts.Data;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Fusion;

namespace Features.GamePhasesModule.Scripts
{
	public class GamePhaseStartupSystem : IGamePhaseActivation, ISharedModeMasterMigrationHandler
	{
		private readonly LevelModel _levelModel;

		private readonly GamePhasesConfiguration _gamePhasesConfiguration;

		private readonly IGamePhaseService _gamePhaseService;

		private readonly GamePhasesModel _gamePhasesModel;

		public int SharedModeMasterMigrationHandleOrder => 0;

		public GamePhaseStartupSystem(LevelModel levelModel, GamePhasesConfiguration gamePhasesConfiguration, IGamePhaseService gamePhaseService, GamePhasesModel gamePhasesModel)
		{
			_levelModel = levelModel;
			_gamePhasesConfiguration = gamePhasesConfiguration;
			_gamePhaseService = gamePhaseService;
			_gamePhasesModel = gamePhasesModel;
		}

		public void ActivateForCurrentLevel()
		{
			LevelType currentLevel = _levelModel.CurrentLevel;
			_gamePhaseService.ActivateGamePhaseLoop(ResolveGamePhasesData(currentLevel), currentLevel);
		}

		public void OnSharedModeMasterMigrationCompleted(NetworkRunner runner)
		{
			if (!(runner == null) && runner.IsRunning && runner.IsSharedModeMasterClient && _gamePhasesModel.IsActive)
			{
				_gamePhaseService.ResumeAfterMigration(ResolveGamePhasesData(_levelModel.CurrentLevel));
			}
		}

		private GamePhasesData ResolveGamePhasesData(LevelType levelType)
		{
			if (!_gamePhasesConfiguration.GamePhasesByLevel.TryGetValue(levelType, out var value))
			{
				return _gamePhasesConfiguration.DefaultGamePhases;
			}
			return value;
		}
	}
}
