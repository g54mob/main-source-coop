namespace Features.StoreModule.Scripts
{
	public class StoreRewardFactory
	{
		private readonly StoreRewardServices _services;

		public StoreRewardFactory(StoreRewardServices services)
		{
			_services = services;
		}

		public StoreRewardBase Create(StoreRewardDataBase rewardData)
		{
			return rewardData?.CreateReward(_services);
		}
	}
}
