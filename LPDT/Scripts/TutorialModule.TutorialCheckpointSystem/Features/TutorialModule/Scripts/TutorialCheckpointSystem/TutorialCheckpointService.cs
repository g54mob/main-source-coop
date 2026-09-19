using Features.TutorialModule.Scripts.TutorialStepsSystem.API;

namespace Features.TutorialModule.Scripts.TutorialCheckpointSystem
{
	public class TutorialCheckpointService : ITutorialCheckpointService
	{
		private readonly TutorialCheckPointModel _tutorialCheckPointModel;

		private readonly ITutorialService _tutorialService;

		public TutorialCheckpointService(TutorialCheckPointModel tutorialCheckPointModel, ITutorialService tutorialService)
		{
			_tutorialService = tutorialService;
			_tutorialCheckPointModel = tutorialCheckPointModel;
		}

		public void SetCheckpoint(TutorialStep tutorialStep)
		{
			_tutorialCheckPointModel.SavedTutorialStep = tutorialStep;
		}

		public void ResetCheckpoint()
		{
			_tutorialCheckPointModel.SavedTutorialStep = TutorialStep.None;
		}

		public bool TryToMoveToCheckpoint()
		{
			if (_tutorialCheckPointModel.SavedTutorialStep == TutorialStep.None)
			{
				return false;
			}
			_tutorialService.ActivateTutorialStep(_tutorialCheckPointModel.SavedTutorialStep);
			return true;
		}
	}
}
