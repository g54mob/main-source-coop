using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public interface IBaseTutorialRewardStepFactory
	{
		ITutorialStep CreateRewardStep(CardItemType rewardItemType);
	}
}
