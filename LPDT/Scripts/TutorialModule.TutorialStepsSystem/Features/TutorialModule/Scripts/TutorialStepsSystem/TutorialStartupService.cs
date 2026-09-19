using Features.ProgressSavingModule.Scripts.Implementation;
using Features.TutorialModule.Scripts.TutorialCheckpointSystem;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class TutorialStartupService : ITutorialStartupService
	{
		private readonly ITutorialCheckpointService _tutorialCheckpointService;

		private readonly BaseTutorialService _baseTutorialService;

		private readonly TutorialModel _tutorialModel;

		private readonly ISavingService _savingService;

		public TutorialStartupService(BaseTutorialService baseTutorialService, TutorialModel tutorialModel, ISavingService savingService, ITutorialCheckpointService tutorialCheckpointService)
		{
			_savingService = savingService;
			_baseTutorialService = baseTutorialService;
			_tutorialModel = tutorialModel;
			_tutorialCheckpointService = tutorialCheckpointService;
		}

		public void Dispose()
		{
			_baseTutorialService.OnTutorialEnded -= CompleteBaseTutorial;
			_baseTutorialService.Dispose();
		}

		public void StartBaseTutorial()
		{
			_baseTutorialService.Initialize();
			_tutorialModel.SetIsTutorialInProgress(isTutorialInProgress: true);
			if (!_tutorialCheckpointService.TryToMoveToCheckpoint())
			{
				_baseTutorialService.ActivateFirstStep();
			}
			_baseTutorialService.OnTutorialEnded += CompleteBaseTutorial;
		}

		public void CompleteBaseTutorial()
		{
			_baseTutorialService.OnTutorialEnded -= CompleteBaseTutorial;
			_tutorialModel.SetIsTutorialInProgress(isTutorialInProgress: false);
			_tutorialModel.IsBaseTutorialCompleted = true;
			_savingService.SaveDataForGroup(SavingGroup.GameData);
		}

		public void EndCurrentTutorial()
		{
			if (_tutorialModel.IsTutorialInProgress && !_tutorialModel.IsBaseTutorialCompleted)
			{
				_baseTutorialService.EndTutorial();
			}
		}
	}
}
