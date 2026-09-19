using System;
using Fusion;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class SpawnItemStoreRewardData : StoreRewardDataBase
	{
		public NetworkBehaviour RewardItemPrefab;

		public CardItemType CardItemType;

		public override string GetStableId()
		{
			if (!(RewardItemPrefab != null))
			{
				return string.Empty;
			}
			return RewardItemPrefab.name;
		}

		public override string GetDisplayName()
		{
			if (!(RewardItemPrefab != null))
			{
				return "SpawnItem";
			}
			return RewardItemPrefab.name;
		}

		public override CardItemType GetCardItemType()
		{
			return CardItemType;
		}

		public override StoreRewardBase CreateReward(StoreRewardServices services)
		{
			return new SpawnItemStoreReward(services.CardItemSpawner, RewardItemPrefab);
		}
	}
}
