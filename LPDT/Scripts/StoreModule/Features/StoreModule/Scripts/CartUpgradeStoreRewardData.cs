using System;
using Features.CartUpgradesModule.Scripts.Core;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class CartUpgradeStoreRewardData : StoreRewardDataBase
	{
		public CartUpgradeModule TargetTier = CartUpgradeModule.Size;

		public override string GetStableId()
		{
			return $"CartUpgrade_{(int)TargetTier}";
		}

		public override string GetDisplayName()
		{
			return $"Cart Upgrade {TargetTier}";
		}

		public override CardItemType GetCardItemType()
		{
			return CardItemType.Upgrade;
		}

		public override StoreRewardBase CreateReward(StoreRewardServices services)
		{
			return new CartUpgradeStoreReward(services.CartUpgradeService, TargetTier);
		}
	}
}
