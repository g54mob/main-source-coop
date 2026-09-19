using Features.TutorialModule.Scripts.TutorialStepsSystem.API;

namespace Features.TutorialModule.Scripts.TutorialCheckpointSystem
{
	public interface ITutorialCheckpointService
	{
		void SetCheckpoint(TutorialStep tutorialStep);

		void ResetCheckpoint();

		bool TryToMoveToCheckpoint();
	}
}
