using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class TutorialService : ITutorialService
	{
		private readonly BaseTutorialService _baseTutorialService;

		private readonly BaseTutorialSkipService _baseTutorialSkipService;

		private readonly TutorialModel _tutorialModel;

		public TutorialService(BaseTutorialService baseTutorialService, TutorialModel tutorialModel, BaseTutorialSkipService baseTutorialSkipService)
		{
			_baseTutorialService = baseTutorialService;
			_tutorialModel = tutorialModel;
			_baseTutorialSkipService = baseTutorialSkipService;
		}

		public void SkipCurrentTutorialStep()
		{
			if (_tutorialModel.IsTutorialInProgress && !_tutorialModel.IsBaseTutorialCompleted)
			{
				_baseTutorialService.SkipCurrentStep();
			}
		}

		public void SkipTutorialToStep(TutorialStep tutorialStep)
		{
			if (_tutorialModel.IsTutorialInProgress && !_tutorialModel.IsBaseTutorialCompleted)
			{
				_baseTutorialService.SkipToStep(tutorialStep);
			}
		}

		public void ActivateTutorialStep(TutorialStep tutorialStep)
		{
			if (_tutorialModel.IsTutorialInProgress && !_tutorialModel.IsBaseTutorialCompleted)
			{
				_baseTutorialService.ActivateStep(tutorialStep);
			}
		}

		public void SkipAllTutorialsSilently()
		{
			_baseTutorialSkipService.SkipTutorial();
		}
	}
}
