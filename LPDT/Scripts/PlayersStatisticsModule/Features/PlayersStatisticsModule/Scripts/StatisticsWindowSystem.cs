using Features.BeachInteractableCommonModule.Scripts;
using Features.BeachPresetModule.Scripts.Core;
using Features.BeachPresetModule.Scripts.Core.Configurations;
using Features.InputModule.Scripts.Generated;
using Features.LevelModule.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayersStatisticsModule.Scripts.Views.Statistics;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.PlayersStatisticsModule.Scripts
{
	public class StatisticsWindowSystem : ILevelStatisticsWindow
	{
		private readonly StatisticWindow _statisticWindow;

		private readonly IInputService _inputService;

		private readonly TutorialModel _tutorialModel;

		private readonly ILevelService _levelService;

		private readonly BeachByLevelsPresetConfiguration _beachByLevelsPreset;

		private readonly BeachInteractableSpawnModel _beachInteractableSpawnModel;

		private readonly LevelPlayersGameStatisticsModel _levelPlayersGameStatisticsModel;

		public StatisticsWindowSystem(StatisticWindow statisticWindow, IInputService inputService, TutorialModel tutorialModel, ILevelService levelService, BeachByLevelsPresetConfiguration beachByLevelsPreset, BeachInteractableSpawnModel beachInteractableSpawnModel, LevelPlayersGameStatisticsModel levelPlayersGameStatisticsModel)
		{
			_statisticWindow = statisticWindow;
			_inputService = inputService;
			_tutorialModel = tutorialModel;
			_levelService = levelService;
			_beachByLevelsPreset = beachByLevelsPreset;
			_beachInteractableSpawnModel = beachInteractableSpawnModel;
			_levelPlayersGameStatisticsModel = levelPlayersGameStatisticsModel;
		}

		public void ShowForCurrentLevel()
		{
			if (_statisticWindow.WindowStatus == WindowStatus.Closed && !_tutorialModel.IsTutorialInProgress)
			{
				CaptureUpcomingPreview();
				_inputService.DisableArmMap();
				_statisticWindow.Open();
			}
		}

		private void CaptureUpcomingPreview()
		{
			if (!_levelService.TryPeekNextLevelType(out var levelType) || !TryGetNewlyRecordedPreviewType(levelType, out var type))
			{
				_levelPlayersGameStatisticsModel.SetSpawnedNewItem(BeachInteractableType.None);
			}
			else
			{
				_levelPlayersGameStatisticsModel.SetSpawnedNewItem(type);
			}
		}

		private bool TryGetNewlyRecordedPreviewType(LevelType levelType, out BeachInteractableType type)
		{
			type = BeachInteractableType.None;
			if (!_beachByLevelsPreset.PresetsByLevel.TryGetValue(levelType, out var value) || value == null)
			{
				return false;
			}
			foreach (BeachBehaviour item in value.GetEnabledBehavioursInApplyOrder())
			{
				if (item is BeachInteractablesSpawnBehaviour beachInteractablesSpawnBehaviour)
				{
					return beachInteractablesSpawnBehaviour.TryGetNewlyRecordedPreviewType(_beachInteractableSpawnModel.RecordedBeachInteractables, out type);
				}
			}
			return false;
		}
	}
}
