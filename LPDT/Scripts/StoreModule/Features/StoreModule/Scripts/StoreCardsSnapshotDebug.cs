using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Features.StoreModule.Scripts
{
	public static class StoreCardsSnapshotDebug
	{
		public static StoreSessionCollectResult Collect(CardsOnTableModel cardsOnTableModel)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int count = cardsOnTableModel.CardsOnTable.Count;
			foreach (StoreCardBehaviour item in cardsOnTableModel.CardsOnTable)
			{
				if (item == null)
				{
					continue;
				}
				if (!item.IsInitialized)
				{
					num++;
					continue;
				}
				if (item.CardData == null)
				{
					num2++;
					continue;
				}
				string id = item.CardData.Id;
				if (string.IsNullOrEmpty(id))
				{
					num3++;
					continue;
				}
				list.Add(id);
				if (item.IsActivated)
				{
					list2.Add(id);
				}
			}
			return new StoreSessionCollectResult(new StoreSessionSnapshot(list, list2), count, num, num2, num3);
		}

		public static void LogStoreClose(StoreSessionCollectResult collectResult, bool isHost, StoreDraftMoneyModel storeDraftMoneyModel = null)
		{
			string text = (isHost ? "Host" : "Client");
			StoreSessionSnapshot snapshot = collectResult.Snapshot;
			Debug.Log("[StoreClose][" + text + "] === Store session snapshot (all players ready) ===");
			Debug.Log($"[StoreClose][{text}] CardsOnTableModel.registered: {collectResult.RegisteredCardCount}");
			Debug.Log($"[StoreClose][{text}] onTable with CardData.Id ({snapshot.OnTableCardIds.Count}): {FormatIds(snapshot.OnTableCardIds)}");
			Debug.Log($"[StoreClose][{text}] purchased IsActivated ({snapshot.PurchasedCardIds.Count}): {FormatIds(snapshot.PurchasedCardIds)}");
			if (collectResult.SkippedNotInitialized > 0 || collectResult.SkippedNullCardData > 0 || collectResult.SkippedEmptyCardId > 0)
			{
				Debug.Log($"[StoreClose][{text}] skipped cards: notInitialized={collectResult.SkippedNotInitialized}, " + $"nullCardData={collectResult.SkippedNullCardData}, emptyCardId={collectResult.SkippedEmptyCardId}");
			}
			LogPeerDataWarnings(collectResult, isHost);
			if (isHost && storeDraftMoneyModel != null)
			{
				Debug.Log($"[StoreClose][Host] draftMoney: {storeDraftMoneyModel.DraftMoney}");
			}
		}

		private static void LogPeerDataWarnings(StoreSessionCollectResult collectResult, bool isHost)
		{
			string text = (isHost ? "Host" : "Client");
			if (collectResult.RegisteredCardCount == 0)
			{
				Debug.LogWarning("[StoreClose][" + text + "] CardsOnTableModel is empty on this peer. CardRegistrar may not have registered spawned cards yet, or store cards were despawned.");
			}
			else if (collectResult.HasRegistrationGap)
			{
				Debug.LogWarning($"[StoreClose][{text}] {collectResult.RegisteredCardCount} cards registered but 0 readable CardData.Id. " + "Likely SetDataRPC / CardData not applied on this peer yet (timing or replication).");
			}
		}

		private static string FormatIds(IReadOnlyList<string> ids)
		{
			if (ids.Count == 0)
			{
				return "-";
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < ids.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(ids[i]);
			}
			return stringBuilder.ToString();
		}
	}
}
