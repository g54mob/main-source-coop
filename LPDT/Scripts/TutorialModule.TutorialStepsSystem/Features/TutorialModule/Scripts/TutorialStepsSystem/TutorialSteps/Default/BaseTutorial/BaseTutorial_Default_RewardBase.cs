using System;
using System.Linq;
using Features.StoreModule.Scripts;
using Features.TutorialModule.Scripts.TutorialStepsSystem.API.Configurations;
using Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialSteps.Default.BaseTutorial
{
	public abstract class BaseTutorial_Default_RewardBase : BaseTutorialStep
	{
		protected readonly BaseTutorialChosenRewardDataHolder BaseTutorialChosenRewardDataHolder;

		protected readonly TutorialStepsConfiguration TutorialStepsConfiguration;

		private readonly SpawnedRewardsModel _spawnedRewardsModel;

		public override event Action OnStepEnd;

		public BaseTutorial_Default_RewardBase(SpawnedRewardsModel spawnedRewardsModel, BaseTutorialChosenRewardDataHolder baseTutorialChosenRewardDataHolder, TutorialStepsConfiguration tutorialStepsConfiguration)
		{
			_spawnedRewardsModel = spawnedRewardsModel;
			BaseTutorialChosenRewardDataHolder = baseTutorialChosenRewardDataHolder;
			TutorialStepsConfiguration = tutorialStepsConfiguration;
		}

		public override void ActivateStep()
		{
			base.ActivateStep();
			EnsureRewardChosen();
		}

		protected void EndStep()
		{
			OnStepEnd?.Invoke();
		}

		public override void DeActivateStep()
		{
			Dispose();
		}

		public override void RevertStep()
		{
		}

		public override void SkipStep()
		{
			OnStepEnd?.Invoke();
		}

		private void EnsureRewardChosen()
		{
			if (BaseTutorialChosenRewardDataHolder.ChosenReward != null)
			{
				OnRewardEnsured();
			}
			else if (_spawnedRewardsModel.SpawnedRewards.Count > 0)
			{
				SetChosenReward();
			}
			else
			{
				_spawnedRewardsModel.OnAllRewardsSpawned += SetChosenReward;
			}
		}

		private void SetChosenReward()
		{
			SpawnedReward chosenReward = _spawnedRewardsModel.SpawnedRewards.FirstOrDefault((SpawnedReward x) => x.SpawnedRewardType == TutorialStepsConfiguration.InteractedCardItemType);
			BaseTutorialChosenRewardDataHolder.ChosenReward = chosenReward;
			OnRewardEnsured();
		}

		protected virtual void OnRewardEnsured()
		{
		}
	}
}
