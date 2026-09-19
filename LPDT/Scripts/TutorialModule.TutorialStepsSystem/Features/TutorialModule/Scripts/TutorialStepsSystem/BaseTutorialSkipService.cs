using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class BaseTutorialSkipService : TutorialSkipBehaviour
	{
		private readonly TutorialModel _tutorialModel;

		public BaseTutorialSkipService(TutorialModel tutorialModel)
		{
			_tutorialModel = tutorialModel;
		}

		public override void SkipTutorial()
		{
			_tutorialModel.IsBaseTutorialCompleted = true;
			_tutorialModel.SetIsTutorialInProgress(isTutorialInProgress: false);
		}
	}
}
