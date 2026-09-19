using Features.GameUpdaterModule;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Data;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class TutorialUISystem : TutorialUISystemBehaviour
	{
		public TutorialUISystem(IWindowsService windowsService, BaseTutorialService baseTutorialService, CurrentTutorialStepModel currentTutorialStepModel, TutorialModel tutorialModel, IGameUpdater gameUpdater, TutorialStepsConfiguration tutorialStepsConfiguration)
			: base(windowsService, baseTutorialService, currentTutorialStepModel, tutorialModel, gameUpdater, tutorialStepsConfiguration)
		{
		}

		protected override void HandleBaseTutorialStarted()
		{
			base.HandleBaseTutorialStarted();
		}
	}
}
