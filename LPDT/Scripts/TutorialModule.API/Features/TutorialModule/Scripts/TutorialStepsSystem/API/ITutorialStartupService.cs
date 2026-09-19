namespace Features.TutorialModule.Scripts.TutorialStepsSystem.API
{
	public interface ITutorialStartupService
	{
		void StartBaseTutorial();

		void CompleteBaseTutorial();

		void EndCurrentTutorial();

		void Dispose();
	}
}
