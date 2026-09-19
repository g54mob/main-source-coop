using System;

namespace Features.StoreModule.Scripts
{
	[Serializable]
	public abstract class StoreRewardDataBase
	{
		public abstract string GetStableId();

		public abstract string GetDisplayName();

		public abstract CardItemType GetCardItemType();

		public abstract StoreRewardBase CreateReward(StoreRewardServices services);
	}
}
