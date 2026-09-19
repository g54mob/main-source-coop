using System;
using Features.StoreModule.Scripts;

namespace Features.TutorialModule.Scripts.TutorialStepsSystem.TutorialAdditionalProcessors.Default.BaseTutorial
{
	public class BaseTutorialChosenRewardDataHolder
	{
		private SpawnedReward _chosenReward;

		public SpawnedReward ChosenReward
		{
			get
			{
				return _chosenReward;
			}
			set
			{
				_chosenReward = value;
				this.OnChosenRewardChanged?.Invoke(_chosenReward);
			}
		}

		public event Action<SpawnedReward> OnChosenRewardChanged;
	}
}
