using System;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public class BaseTutorial_Default_RewardInteraction : ITutorialStep, IDisposable
	{
		private readonly TutorialStepsConfiguration _tutorialStepsConfiguration;

		private readonly IBaseTutorialRewardStepFactory _baseTutorialRewardStepFactory;

		private ITutorialStep _rewardStep;

		public ITutorialCustomData CustomData => _rewardStep.CustomData;

		public event Action OnStepEnd;

		public BaseTutorial_Default_RewardInteraction(TutorialStepsConfiguration tutorialStepsConfiguration, IBaseTutorialRewardStepFactory baseTutorialRewardStepFactory)
		{
			_tutorialStepsConfiguration = tutorialStepsConfiguration;
			_baseTutorialRewardStepFactory = baseTutorialRewardStepFactory;
		}

		public void ActivateStep()
		{
			_rewardStep = _baseTutorialRewardStepFactory.CreateRewardStep(_tutorialStepsConfiguration.InteractedCardItemType);
			_rewardStep.OnStepEnd += EndStep;
			_rewardStep.ActivateStep();
		}

		public void DeActivateStep()
		{
			_rewardStep.DeActivateStep();
		}

		public void SkipStep()
		{
			_rewardStep.SkipStep();
		}

		public void RevertStep()
		{
			_rewardStep.RevertStep();
		}

		public void Dispose()
		{
			_rewardStep.OnStepEnd -= EndStep;
			_rewardStep.Dispose();
		}

		private void EndStep()
		{
			this.OnStepEnd?.Invoke();
		}
	}
}
