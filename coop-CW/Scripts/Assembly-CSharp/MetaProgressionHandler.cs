using System;
using System.Collections.Generic;
using System.Linq;
using Portningsbolaget.Platforms;
using UnityEngine;
using Zorro.Core.CLI;
using Zorro.Core.Serizalization;

public class MetaProgressionHandler : RetrievableSingleton<MetaProgressionHandler>
{
	private int metaCoins;

	private HashSet<int> unlockedHats = new HashSet<int>();

	private HashSet<int> unlockedIslandUpgrades = new HashSet<int>();

	public int MetaCoins => metaCoins;

	protected override void OnCreated()
	{
		base.OnCreated();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public static int[] GetUnlockedIslandUpgrades()
	{
		if ((bool)Player.localPlayer)
		{
			foreach (int unlockedIslandUpgrade in RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedIslandUpgrades)
			{
				switch (unlockedIslandUpgrade)
				{
				case 0:
					Debug.Log("[Achievement] Unlocked Pool");
					PlatformManager.UnlockAchievement(Achievements.ACH_POOL);
					break;
				case 1:
					Debug.Log("[Achievement] Unlocked Cinema!");
					PlatformManager.UnlockAchievement(Achievements.ACH_CINEMA);
					break;
				case 2:
					Debug.Log("[Achievement] Unlocked PodCast");
					PlatformManager.UnlockAchievement(Achievements.ACH_PODCAST);
					break;
				case 3:
					Debug.Log("[Achievement] Unlocked GreenScreen");
					PlatformManager.UnlockAchievement(Achievements.ACH_GREENSCREEN);
					break;
				case 4:
					Debug.Log("[Achievement] Unlocked Trampoline!");
					Debug.Log(unlockedIslandUpgrade);
					PlatformManager.UnlockAchievement(Achievements.ACH_TRAMPOLINE);
					break;
				}
			}
		}
		return RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedIslandUpgrades.ToArray();
	}

	public static int[] GetUnlockedHats()
	{
		return RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedHats.ToArray();
	}

	public static bool CanAffordPurchase(int amount)
	{
		return RetrievableSingleton<MetaProgressionHandler>.Instance.MetaCoins >= amount;
	}

	[ConsoleCommand]
	public static void UnlockAllHats()
	{
		for (int i = 0; i < HatDatabase.instance.hats.Length; i++)
		{
			_ = HatDatabase.instance.hats[i];
			UnlockHat(i);
		}
		RetrievableSingleton<MetaProgressionHandler>.Instance.UpdateAndSave();
	}

	[ConsoleCommand]
	public static void ClearAllUnlockedHatsHats()
	{
		RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedHats.Clear();
		RetrievableSingleton<MetaProgressionHandler>.Instance.UpdateAndSave();
	}

	[ConsoleCommand]
	public static void AddMetaCoins(int amount)
	{
		UserInterface.ShowMoneyNotification("Meta Coins Received", amount.ToString(), MoneyCellUI.MoneyCellType.MetaCoins);
		SetMetaCoins(RetrievableSingleton<MetaProgressionHandler>.Instance.MetaCoins + amount);
	}

	public static void RemoveMetaCoins(int amount)
	{
		SetMetaCoins(RetrievableSingleton<MetaProgressionHandler>.Instance.MetaCoins - amount);
	}

	[ConsoleCommand]
	public static void SetMetaCoins(int amount)
	{
		RetrievableSingleton<MetaProgressionHandler>.Instance.metaCoins = amount;
		RetrievableSingleton<MetaProgressionHandler>.Instance.UpdateAndSave();
	}

	[ConsoleCommand]
	public static void EquipHat(int hatIndex)
	{
		Player.localPlayer.Call_EquipHat(hatIndex);
	}

	private void UpdateAndSave()
	{
		SerializedMetaProgression serializedMetaProgression = new SerializedMetaProgression
		{
			MetaCoins = metaCoins,
			UnlockedHats = unlockedHats.ToArray(),
			UnlockedIslandUpgrades = unlockedIslandUpgrades.ToArray()
		};
		SaveSystem.SaveMetaData(FlatJsonSerializer.Serialize(new object[1] { serializedMetaProgression }));
	}

	public static void Init()
	{
		RetrievableSingleton<MetaProgressionHandler>.Instance.Initialize();
		SaveSystem.TryLoadMetaData(OnLoadedMetaData);
	}

	private static void OnLoadedMetaData(bool success, string data)
	{
		if (!success)
		{
			Debug.Log("No meta progression data found, creating new save");
			RetrievableSingleton<MetaProgressionHandler>.Instance.UpdateAndSave();
			return;
		}
		try
		{
			if (FlatJsonSerializer.Deserialize(data).TryGetObject<SerializedMetaProgression>(out var obj))
			{
				if (obj.MetaCoins < 0)
				{
					obj.MetaCoins = 1000;
					Modal.ShowError("Looks like something happened with your meta coins", "We've reset your meta coins to 1000. Sorry for the inconvenience.");
				}
				RetrievableSingleton<MetaProgressionHandler>.Instance.metaCoins = obj.MetaCoins;
				RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedHats = new HashSet<int>(obj.UnlockedHats);
				RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedIslandUpgrades = new HashSet<int>(obj.UnlockedIslandUpgrades);
				Debug.Log("Meta progression data loaded");
			}
			else
			{
				Debug.LogError("Failed to deserialize meta progression data");
			}
		}
		catch (Exception arg)
		{
			Debug.LogError($"Failed to load meta progression data: {arg}");
			SaveSystem.DeleteMetaData();
		}
		RetrievableSingleton<MetaProgressionHandler>.Instance.UpdateAndSave();
	}

	private void Initialize()
	{
		Debug.Log("MetaProgressionHandler initialized");
	}

	public static void UnlockIslandUpgrade(IslandUnlock unlock)
	{
		RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedIslandUpgrades.Add(unlock.GetID());
		RetrievableSingleton<MetaProgressionHandler>.Instance.UpdateAndSave();
	}

	public static void UnlockHat(int hat)
	{
		RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedHats.Add(hat);
		RetrievableSingleton<MetaProgressionHandler>.Instance.UpdateAndSave();
		CheckIfUnlockedAllHats();
	}

	public static void CheckIfUnlockedAllHats()
	{
		for (int i = 0; i < HatDatabase.instance.hats.Length; i++)
		{
			if (!RetrievableSingleton<MetaProgressionHandler>.Instance.unlockedHats.Contains(i))
			{
				Debug.Log("Does Not Have All Hats, Missing: " + HatDatabase.instance.hats[i]);
				return;
			}
		}
		PlatformManager.UnlockAchievement(Achievements.ACH_ALL_HATS);
	}
}
