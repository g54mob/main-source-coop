using System.Collections.Generic;

namespace Features.StoreModule.Scripts
{
	public sealed class StoreSessionSnapshot
	{
		public IReadOnlyList<string> OnTableCardIds { get; }

		public IReadOnlyList<string> PurchasedCardIds { get; }

		public StoreSessionSnapshot(IReadOnlyList<string> onTableCardIds, IReadOnlyList<string> purchasedCardIds)
		{
			OnTableCardIds = onTableCardIds;
			PurchasedCardIds = purchasedCardIds;
		}
	}
}
