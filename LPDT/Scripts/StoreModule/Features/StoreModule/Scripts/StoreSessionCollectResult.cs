namespace Features.StoreModule.Scripts
{
	public sealed class StoreSessionCollectResult
	{
		public StoreSessionSnapshot Snapshot { get; }

		public int RegisteredCardCount { get; }

		public int SkippedNotInitialized { get; }

		public int SkippedNullCardData { get; }

		public int SkippedEmptyCardId { get; }

		public bool HasReadableOnTableCards => Snapshot.OnTableCardIds.Count > 0;

		public bool HasRegistrationGap
		{
			get
			{
				if (RegisteredCardCount > 0)
				{
					return Snapshot.OnTableCardIds.Count == 0;
				}
				return false;
			}
		}

		public StoreSessionCollectResult(StoreSessionSnapshot snapshot, int registeredCardCount, int skippedNotInitialized, int skippedNullCardData, int skippedEmptyCardId)
		{
			Snapshot = snapshot;
			RegisteredCardCount = registeredCardCount;
			SkippedNotInitialized = skippedNotInitialized;
			SkippedNullCardData = skippedNullCardData;
			SkippedEmptyCardId = skippedEmptyCardId;
		}
	}
}
