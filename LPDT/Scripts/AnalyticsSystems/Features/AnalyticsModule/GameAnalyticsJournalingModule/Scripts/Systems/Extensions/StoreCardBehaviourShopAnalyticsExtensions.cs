using Features.StoreModule.Scripts;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems.Extensions
{
	public static class StoreCardBehaviourShopAnalyticsExtensions
	{
		public static string GetShopAnalyticsItemName(this StoreCardBehaviour card)
		{
			if (card == null || !card.IsInitialized || card.CardData == null)
			{
				return "Unknown";
			}
			string text = card.CardData.GetRewardData()?.GetDisplayName();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			if (!string.IsNullOrEmpty(card.CardData.Id))
			{
				return card.CardData.Id;
			}
			return "Unknown";
		}
	}
}
