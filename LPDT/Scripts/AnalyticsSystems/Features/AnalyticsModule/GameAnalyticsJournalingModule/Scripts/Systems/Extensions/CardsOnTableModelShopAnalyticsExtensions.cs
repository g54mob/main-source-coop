using System.Collections.Generic;
using Features.StoreModule.Scripts;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems.Extensions
{
	public static class CardsOnTableModelShopAnalyticsExtensions
	{
		public static void CollectShopOfferItemNames(this CardsOnTableModel cardsOnTableModel, List<string> offerItemNames)
		{
			offerItemNames.Clear();
			foreach (StoreCardBehaviour item in cardsOnTableModel.CardsOnTable)
			{
				if (!(item == null) && item.IsInitialized && item.CardData != null)
				{
					offerItemNames.Add(item.GetShopAnalyticsItemName());
				}
			}
		}

		public static void CollectShopPurchaseItemNames(this CardsOnTableModel cardsOnTableModel, List<string> purchaseItemNames)
		{
			purchaseItemNames.Clear();
			foreach (StoreCardBehaviour item in cardsOnTableModel.CardsOnTable)
			{
				if (!(item == null) && item.IsInitialized && item.CardData != null && item.IsActivated)
				{
					purchaseItemNames.Add(item.GetShopAnalyticsItemName());
				}
			}
		}
	}
}
