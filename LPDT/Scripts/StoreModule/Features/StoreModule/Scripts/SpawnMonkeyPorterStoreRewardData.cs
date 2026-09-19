using System;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class SpawnMonkeyPorterStoreRewardData : StoreRewardDataBase
	{
		public CardItemType CardItemType = CardItemType.Tool;

		public override string GetStableId()
		{
			return "MonkeyPorter";
		}

		public override string GetDisplayName()
		{
			return "Monkey Porter";
		}

		public override CardItemType GetCardItemType()
		{
			return CardItemType;
		}

		public override StoreRewardBase CreateReward(StoreRewardServices services)
		{
			return new SpawnMonkeyPorterStoreReward(services.MonkeyPorterRunModel);
		}
	}
}
