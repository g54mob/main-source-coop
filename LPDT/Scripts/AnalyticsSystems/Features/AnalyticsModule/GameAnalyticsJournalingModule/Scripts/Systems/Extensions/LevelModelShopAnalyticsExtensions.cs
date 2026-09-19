using System.Collections.Generic;
using Features.LevelModule.Scripts;

namespace Features.AnalyticsModule.GameAnalyticsJournalingModule.Scripts.Systems.Extensions
{
	public static class LevelModelShopAnalyticsExtensions
	{
		private const string ShopEventNamePrefix = "Shop:";

		private const string OfferAction = "Offer";

		private const string PurchaseAction = "Purchase";

		public static string GetShopAnalyticsLevelCode(this LevelModel levelModel)
		{
			return $"L{levelModel.CurrentSequenceLevelNumber:D2}";
		}

		public static List<string> BuildShopStoreAnalyticsEventNames(this LevelModel levelModel, IReadOnlyList<string> offerItemNames, IReadOnlyList<string> purchaseItemNames)
		{
			string shopAnalyticsLevelCode = levelModel.GetShopAnalyticsLevelCode();
			List<string> list = new List<string>(offerItemNames.Count + purchaseItemNames.Count);
			for (int i = 0; i < offerItemNames.Count; i++)
			{
				list.Add(BuildShopAnalyticsEvent(shopAnalyticsLevelCode, "Offer", offerItemNames[i]));
			}
			for (int j = 0; j < purchaseItemNames.Count; j++)
			{
				list.Add(BuildShopAnalyticsEvent(shopAnalyticsLevelCode, "Purchase", purchaseItemNames[j]));
			}
			return list;
		}

		private static string BuildShopAnalyticsEvent(string shopLevel, string action, string itemDisplayName)
		{
			return "Shop:" + shopLevel + ":" + action + ":" + SanitizeItemName(itemDisplayName);
		}

		private static string SanitizeItemName(string itemDisplayName)
		{
			if (string.IsNullOrEmpty(itemDisplayName))
			{
				return "Unknown";
			}
			return itemDisplayName.Replace(":", "_");
		}
	}
}
