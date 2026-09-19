namespace Features.StoreModule.Scripts
{
	public readonly struct PendingRewardEntry
	{
		public int CardNetId { get; }

		public int CardDataId { get; }

		public int ColorPacked { get; }

		public int TargetPlayerId { get; }

		public PendingRewardEntry(int cardNetId, int cardDataId, int colorPacked, int targetPlayerId)
		{
			CardNetId = cardNetId;
			CardDataId = cardDataId;
			ColorPacked = colorPacked;
			TargetPlayerId = targetPlayerId;
		}
	}
}
