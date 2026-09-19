using System;
using Features.SkinChangeModule.Scripts;
using Features.SkinConfiguration.Scripts;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public class ApplySkinStoreRewardData : StoreRewardDataBase
	{
		public SkinPartType SkinPartType;

		public SkinType SkinId;

		public override string GetStableId()
		{
			return $"Skin_{SkinPartType}_{SkinId}";
		}

		public override string GetDisplayName()
		{
			return $"Skin {SkinPartType} {SkinId}";
		}

		public override CardItemType GetCardItemType()
		{
			return CardItemType.Skin;
		}

		public override StoreRewardBase CreateReward(StoreRewardServices services)
		{
			return new ApplySkinStoreReward(services.SkinChangeService, SkinPartType, SkinId);
		}
	}
}
