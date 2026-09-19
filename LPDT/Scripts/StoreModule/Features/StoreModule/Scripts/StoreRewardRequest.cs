namespace Features.StoreModule.Scripts
{
	public class StoreRewardRequest
	{
		public StoreCardBehaviour StoreCard { get; }

		public int TargetPlayerId { get; }

		public StoreRewardRequest(StoreCardBehaviour storeCard, int targetPlayerId)
		{
			StoreCard = storeCard;
			TargetPlayerId = targetPlayerId;
		}
	}
}
