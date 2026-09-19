namespace Features.TutorialModule.Scripts.TutorialStepsSystem.API
{
	public interface ITutorialService
	{
		void SkipCurrentTutorialStep();

		void SkipTutorialToStep(TutorialStep tutorialStep);

		void ActivateTutorialStep(TutorialStep tutorialStep);

		void SkipAllTutorialsSilently();
	}
}
