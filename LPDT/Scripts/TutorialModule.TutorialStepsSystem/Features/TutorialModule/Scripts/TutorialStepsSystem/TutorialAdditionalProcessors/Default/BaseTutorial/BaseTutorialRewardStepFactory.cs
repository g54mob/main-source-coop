using System;
using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial;
using Zenject;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialRewardStepFactory : IBaseTutorialRewardStepFactory
	{
		private readonly DiContainer _diContainer;

		public BaseTutorialRewardStepFactory(DiContainer diContainer)
		{
			_diContainer = diContainer;
		}

		public ITutorialStep CreateRewardStep(CardItemType rewardItemType)
		{
			if ((uint)(rewardItemType - 3) <= 1u)
			{
				return _diContainer.Instantiate<BaseTutorial_Default_RumInteraction>();
			}
			throw new ArgumentException($"Failed to create reward tutorial step for {rewardItemType} because it does not exist");
		}
	}
}
