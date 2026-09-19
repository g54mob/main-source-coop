using Features.CoroutineUtils.Scripts;
using Features.DeviceModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialStepsFactorySystems.API;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem
{
	public class BaseTutorialStepsQuery : TutorialStepsQuery
	{
		public BaseTutorialStepsQuery(TutorialStepFactoryHolder tutorialStepFactoryHolder, CurrentTutorialStepModel currentTutorialStepModel, ICoroutineRunner coroutineRunner, IDeviceService deviceService)
			: base(tutorialStepFactoryHolder, currentTutorialStepModel, coroutineRunner, deviceService)
		{
		}
	}
}
