using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public static class SaveLoadHandler
{
	public static void FillSaveData(Save save)
	{
		RoomStatsHolder roomStats = SurfaceNetworkHandler.RoomStats;
		if (roomStats == null)
		{
			Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Save), "RoomStatsHolder is null!");
			Debug.LogError("RoomStatsHolder is null!");
			return;
		}
		save.SerializedSave = new SerializedSave
		{
			CurrentDay = roomStats.CurrentDay,
			CurrentQuota = roomStats.CurrentQuota,
			CurrentQuotaDay = roomStats.CurrentQuotaDay,
			Money = roomStats.Money,
			InventoryItems = SerializeInventoryItems(),
			SurfaceItems = SerializeSurfaceItems(),
			SaveID = save.SerializedSave.SaveID,
			PickableNetworkDeals = GetPickableNetworkDeals(),
			SaveIndex = save.SerializedSave.SaveIndex,
			MatchSeed = GameAPI.matchSeed,
			LastPlayedLevel = roomStats.LevelToPlay,
			TimeOfDay = TimeOfDayHandler.TimeOfDay
		};
		if (NetworkDealBoss.activeDeal != null)
		{
			save.SerializedNetworkDeals = new NetworkDealBase.SerializedNetworkDeal[1] { NetworkDealBoss.activeDeal.GetSerialized() };
			Debug.Log("Saving Active Network Deal: " + NetworkDealBoss.activeDeal.GetType());
		}
		else
		{
			save.SerializedNetworkDeals = Array.Empty<NetworkDealBase.SerializedNetworkDeal>();
		}
	}

	private static SerializedPickableNetworkDeal[] GetPickableNetworkDeals()
	{
		return SurfaceNetworkHandler.RoomStats.NetworkDealsToSelect.Select((NetworkDealBase networkDealBase) => new SerializedPickableNetworkDeal(networkDealBase)).ToArray();
	}

	private static SavedSurfaceItem[] SerializeSurfaceItems()
	{
		PersistentObjectInfo[] array = RetrievableSingleton<PersistentObjectsHolder>.Instance.FindPersistantSurfaceObjectsLite();
		List<SavedSurfaceItem> list = new List<SavedSurfaceItem>();
		PersistentObjectInfo[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			SavedSurfaceItem item = new SavedSurfaceItem(array2[i]);
			list.Add(item);
		}
		return list.ToArray();
	}

	private static SavedInventoryItem[] SerializeInventoryItems()
	{
		List<SavedInventoryItem> list = new List<SavedInventoryItem>();
		Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
		foreach (Photon.Realtime.Player player in playerList)
		{
			if (GlobalPlayerData.TryGetPlayerData(player, out var globalPlayerData))
			{
				foreach (ItemDescriptor item3 in globalPlayerData.inventory.GetItems())
				{
					if (item3.item.itemType != Item.ItemType.Camera)
					{
						SavedInventoryItem item = new SavedInventoryItem(item3.item);
						list.Add(item);
					}
				}
				if (globalPlayerData.inventory.emote != null)
				{
					SavedInventoryItem item2 = new SavedInventoryItem(globalPlayerData.inventory.emote);
					list.Add(item2);
				}
			}
			else
			{
				Debug.LogError("Player: " + player.NickName + " Has no items?");
			}
		}
		return list.ToArray();
	}

	public static void LoadSaveData(Save currentSave, RoomStatsHolder roomStatsHolder)
	{
		SerializedSave serializedSave = currentSave.SerializedSave;
		roomStatsHolder.CurrentDay = serializedSave.CurrentDay;
		roomStatsHolder.CurrentQuota = serializedSave.CurrentQuota;
		roomStatsHolder.CurrentQuotaDay = serializedSave.CurrentQuotaDay;
		roomStatsHolder.Money = serializedSave.Money;
		roomStatsHolder.CalculateNewQuota(consumeQuota: false);
		roomStatsHolder.MatchSeed = serializedSave.MatchSeed;
		roomStatsHolder.LevelToPlay = serializedSave.LastPlayedLevel;
		if (serializedSave.TimeOfDay != TimeOfDay.Morning)
		{
			TimeOfDayHandler.SetTimeOfDay(serializedSave.TimeOfDay);
		}
		NetworkDealBase.SerializedNetworkDeal[] serializedNetworkDeals = currentSave.SerializedNetworkDeals;
		if (serializedNetworkDeals.Length >= 1)
		{
			Debug.Log("Loading Network Deal: " + serializedNetworkDeals.First().GetType()?.ToString() + " Count: " + serializedNetworkDeals.Length);
			NetworkDealBoss.activeDeal = serializedNetworkDeals.First().UnSerialize();
			NetworkDealBoss.activeDeal.Refresh();
		}
		UnityEngine.Object.FindObjectOfType<SpawnPointsForInventory>().SpawnItems(serializedSave.InventoryItems);
		SpawnSurfaceItems(serializedSave.SurfaceItems);
		SerializedPickableNetworkDeal[] pickableNetworkDeals = serializedSave.PickableNetworkDeals;
		if (pickableNetworkDeals != null && pickableNetworkDeals.Length != 0)
		{
			Debug.Log("Loading Pickable Network Deals: " + pickableNetworkDeals.First().GetType()?.ToString() + " Count: " + pickableNetworkDeals.Length);
			roomStatsHolder.SetNetworkDealsToSelect(pickableNetworkDeals.Select((SerializedPickableNetworkDeal serializedDeal) => serializedDeal.Deserialize()).ToArray());
		}
	}

	private static void SpawnSurfaceItems(SavedSurfaceItem[] currentSaveSurfaceItems)
	{
		foreach (SavedSurfaceItem savedSurfaceItem in currentSaveSurfaceItems)
		{
			if (!ItemDatabase.TryGetItemFromPersistentID(savedSurfaceItem.GetPersistentID(), out var item))
			{
				Debug.Log("Item not found in database: " + savedSurfaceItem.GetPersistentID().ToString());
			}
			else
			{
				PickupHandler.CreatePickup(pos: new Vector3(savedSurfaceItem.posX, savedSurfaceItem.posY, savedSurfaceItem.posZ), rot: new Quaternion(savedSurfaceItem.rotX, savedSurfaceItem.rotY, savedSurfaceItem.rotZ, savedSurfaceItem.rotW), itemID: item.id, data: new ItemInstanceData(Guid.NewGuid()));
			}
		}
	}
}
