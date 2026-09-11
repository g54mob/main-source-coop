using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Unity.Collections;
using UnityEngine;
using Zorro.Core;
using Zorro.Core.Serizalization;

public class RoomStatsHolder
{
	private const string CAMERA_UPGRADES_KEY = "cu";

	private const string MONEY_KEY = "m";

	private const string DAY_KEY = "d";

	private const string DAY_QUOTA_KEY = "q";

	private const string DAYS_PER_QUOTA_KEY = "dpq";

	private const string CURRENT_QUOTA_KEY = "cq";

	private const string QUOTA_SIZE_KEY = "qs";

	private const string DAY_SEED_KEY = "s";

	private const string NETWORK_DEALS_TO_SELECT_KEY = "nds";

	private const string SAVE_SEED_KEY = "gs";

	private const string LAST_PLAYED_KEY = "lp";

	private int m_quotaToReachInternal;

	private int currentQuoutaInternal;

	private SurfaceNetworkHandler m_SurfaceHandler;

	private Action m_OnUpdateAction;

	public Action<int> OnAddQuota;

	public int DaysPerQutoa { get; private set; }

	public int Money { get; internal set; }

	public int CurrentDay { get; internal set; }

	public int CurrentQuotaDay { get; internal set; }

	public int CurrentRun => Mathf.FloorToInt((float)(CurrentDay - 1) / 3f);

	public bool ComputerRoomDoorUnlocked => CurrentDay != CurrentQuotaDay;

	public int QuotaToReach
	{
		get
		{
			return m_quotaToReachInternal;
		}
		private set
		{
			Debug.Log("Setting Quota to reach To: " + value + " From: " + m_quotaToReachInternal);
			m_quotaToReachInternal = value;
		}
	}

	public int CurrentQuota
	{
		get
		{
			return currentQuoutaInternal;
		}
		internal set
		{
			Debug.Log("Setting Quota To: " + value + " From: " + currentQuoutaInternal);
			currentQuoutaInternal = value;
		}
	}

	public NetworkDealBase[] NetworkDealsToSelect { get; private set; }

	public CameraUpgradeTable CurrentCamera { get; private set; }

	public bool IsQuotaDay => m_QuotaDay;

	private bool m_QuotaDay => CurrentQuotaDay >= DaysPerQutoa;

	public bool ReceivedQuota { get; private set; }

	public int MatchSeed { get; set; }

	public int LevelToPlay { get; set; }

	public RoomStatsHolder(SurfaceNetworkHandler handler, int startMoney, int startQuotaToReachToReach, int daysPerQuota)
	{
		m_SurfaceHandler = handler;
		Money = startMoney;
		QuotaToReach = startQuotaToReachToReach;
		DaysPerQutoa = daysPerQuota;
		CurrentDay = 0;
		CurrentQuotaDay = 0;
		CurrentQuota = 0;
		NetworkDealsToSelect = Array.Empty<NetworkDealBase>();
		ReceivedQuota = false;
		CurrentCamera = new CameraUpgradeTable();
		LevelToPlay = -1;
		NextDay();
		NewMapToPlay();
		OnStatsUpdated();
		MatchSeed = UnityEngine.Random.Range(0, 1000);
	}

	public void AddMoney(int money)
	{
		Money += money;
		OnStatsUpdated();
	}

	public void AddCameraUpgrade(byte upgrade)
	{
		CurrentCamera.AddUpgrade(upgrade);
	}

	public void RemoveMoney(int money)
	{
		Money -= money;
		if (NetworkDealBoss.me != null)
		{
			NetworkDealBoss.me.OnMoneyRemoved(money);
		}
		OnStatsUpdated();
	}

	public void NextDay()
	{
		ReceivedQuota = false;
		RegenerateSeed();
		GenerateBudget();
		if (m_QuotaDay)
		{
			if (!CalculateIfReachedQuota())
			{
				Debug.Log("Did Not Reach Quota, Plz Die");
				m_SurfaceHandler.FailedQuota();
				return;
			}
			ResetCurrentQuotaDay();
			NewMapToPlay();
			CurrentDay++;
			CurrentQuotaDay++;
			Debug.Log("Reached Quota of: " + QuotaToReach + " Team Quota: " + CurrentQuota);
			CalculateNewQuota();
			m_SurfaceHandler.NewWeek(CurrentRun);
		}
		else
		{
			CurrentDay++;
			CurrentQuotaDay++;
		}
		Debug.Log("NEW DAY: " + CurrentDay + " Current QuotaDay: " + CurrentQuotaDay + "Days Left To Complete Quota Of: " + QuotaToReach + " : " + (DaysPerQutoa - CurrentQuotaDay));
		OnStatsUpdated();
		if (CurrentDay > 1)
		{
			SaveSystem.SaveToDisk();
		}
	}

	public int GetDaysLeft()
	{
		return DaysPerQutoa - CurrentQuotaDay;
	}

	private void RegenerateSeed()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			GameAPI.seed = UnityEngine.Random.Range(0, int.MaxValue);
			Debug.Log("Master Regen ew Seed: " + GameAPI.seed);
		}
	}

	private void GenerateBudget()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			GameAPI.GenerateNewBudget(CurrentDay, CurrentQuotaDay);
		}
	}

	public void CalculateNewQuota(bool consumeQuota = true)
	{
		int runIndex = Mathf.FloorToInt((float)(CurrentDay - 1) / (float)DaysPerQutoa + 1E-08f);
		Debug.Log("Calculating New Quota: " + runIndex + " DaysPerQuota: " + DaysPerQutoa + " CurrentDay: " + CurrentDay);
		QuotaToReach = BigNumbers.GetQuota(runIndex);
		if (consumeQuota)
		{
			CurrentQuota = 0;
			Debug.Log("Consuming Quota: " + QuotaToReach + " Current Quota: " + CurrentQuota);
		}
	}

	private void ResetCurrentQuotaDay()
	{
		CurrentQuotaDay = 0;
	}

	private void NewMapToPlay()
	{
		int num = UnityEngine.Random.Range(0, 3);
		for (int i = 0; i < 100; i++)
		{
			if (num != LevelToPlay)
			{
				break;
			}
			num = UnityEngine.Random.Range(0, 3);
		}
		LevelToPlay = num;
	}

	public bool CalculateIfReachedQuota()
	{
		if (CurrentQuota >= QuotaToReach)
		{
			return true;
		}
		if (BigNumbers.ViewsToString(BigNumbers.GetScoreToViews(CurrentQuota, GameAPI.CurrentDay)) == BigNumbers.ViewsToString(BigNumbers.GetScoreToViews(QuotaToReach, GameAPI.CurrentDay)))
		{
			return true;
		}
		return false;
	}

	private void OnStatsUpdated()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			VerboseDebug.Log("Changing RoomStats: Money: " + Money + " Day: " + CurrentDay);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("m", Money);
			hashtable.Add("d", CurrentDay);
			hashtable.Add("q", CurrentQuotaDay);
			hashtable.Add("qs", QuotaToReach);
			hashtable.Add("cq", CurrentQuota);
			hashtable.Add("s", GameAPI.seed);
			hashtable.Add("gs", MatchSeed);
			hashtable.Add("lp", LevelToPlay);
			hashtable.Add("cu", CurrentCamera.GetUpgradeData());
			hashtable.Add("nds", SerializeNetworkDealsToSelect());
			PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
		}
	}

	private byte[] SerializeNetworkDealsToSelect()
	{
		BinarySerializer binarySerializer = new BinarySerializer(20, Allocator.Temp);
		binarySerializer.WriteByte((byte)NetworkDealsToSelect.Length);
		NetworkDealBase[] networkDealsToSelect = NetworkDealsToSelect;
		foreach (NetworkDealBase networkDealBase in networkDealsToSelect)
		{
			byte index = networkDealBase.GetIndex();
			binarySerializer.WriteByte(index);
			binarySerializer.WriteByte((byte)networkDealBase.difficulty);
			binarySerializer.WriteByte(networkDealBase.reward.GetIndex());
		}
		byte[] result = binarySerializer.buffer.ToByteArray();
		binarySerializer.Dispose();
		return result;
	}

	private void DeserializeNetworkDeals(byte[] networkDeals)
	{
		BinaryDeserializer binaryDeserializer = new BinaryDeserializer(networkDeals, Allocator.Temp);
		byte b = binaryDeserializer.ReadByte();
		NetworkDealsToSelect = new NetworkDealBase[b];
		for (int i = 0; i < b; i++)
		{
			NetworkDealsToSelect[i] = SingletonAsset<NetworkDealDataBase>.Instance.GetDealFromIndex(binaryDeserializer.ReadByte());
			DIFFICULTY dif = (DIFFICULTY)binaryDeserializer.ReadByte();
			DealRewardBase rewardFromIndex = SingletonAsset<DealRewardDatabase>.Instance.GetRewardFromIndex(binaryDeserializer.ReadByte());
			NetworkDealsToSelect[i].Init(rewardFromIndex, dif);
		}
		binaryDeserializer.Dispose();
	}

	public void Update(Hashtable propertiesThatChanged)
	{
		string views = BigNumbers.ViewsToString(BigNumbers.GetScoreToViews(CurrentQuota, CurrentDay));
		string quota = BigNumbers.ViewsToString(BigNumbers.GetScoreToViews(QuotaToReach, CurrentDay));
		RichPresenceHandler.SetRoomsStats(views, quota, CurrentDay);
		if (PhotonNetwork.IsMasterClient)
		{
			Debug.Log("MasterClient Should Not Update RoomStats!");
			if (NetworkDealBoss.me != null)
			{
				NetworkDealBoss.me.OnStatusUpdated();
			}
			return;
		}
		int money = Money;
		int currentDay = CurrentDay;
		int quotaToReach = QuotaToReach;
		int seed = GameAPI.seed;
		int matchSeed = GameAPI.matchSeed;
		int currentQuota = CurrentQuota;
		byte[] upgradeData = CurrentCamera.GetUpgradeData();
		if (propertiesThatChanged.ContainsKey("m"))
		{
			Money = (int)propertiesThatChanged["m"];
			if (Money != money)
			{
				Debug.Log("RoomStats Changed, Money: " + Money);
			}
		}
		if (propertiesThatChanged.ContainsKey("d"))
		{
			CurrentDay = (int)propertiesThatChanged["d"];
			if (CurrentDay != currentDay)
			{
				Debug.Log("RoomStats Changed, Day: " + CurrentDay);
			}
		}
		if (propertiesThatChanged.ContainsKey("dpq") && DaysPerQutoa == 0)
		{
			DaysPerQutoa = (int)propertiesThatChanged["dpq"];
			Debug.Log("Got DaysPerQuota: dpq");
		}
		if (propertiesThatChanged.ContainsKey("q"))
		{
			CurrentQuotaDay = (int)propertiesThatChanged["q"];
			VerboseDebug.Log("Got Current Quota Day: " + CurrentQuotaDay);
		}
		if (propertiesThatChanged.ContainsKey("qs"))
		{
			QuotaToReach = (int)propertiesThatChanged["qs"];
			if (quotaToReach != QuotaToReach)
			{
				VerboseDebug.Log("Got New QuotaSize: " + QuotaToReach);
			}
		}
		if (propertiesThatChanged.ContainsKey("cq"))
		{
			CurrentQuota = (int)propertiesThatChanged["cq"];
			if (currentQuota != QuotaToReach)
			{
				Debug.Log("Got New Current Quota: " + CurrentQuota);
			}
		}
		if (propertiesThatChanged.ContainsKey("s"))
		{
			GameAPI.seed = (int)propertiesThatChanged["s"];
			if (seed != GameAPI.seed)
			{
				Debug.Log("Got New Seed: " + GameAPI.seed);
			}
		}
		if (propertiesThatChanged.ContainsKey("gs"))
		{
			MatchSeed = (int)propertiesThatChanged["gs"];
			if (matchSeed != GameAPI.matchSeed)
			{
				Debug.Log("Got New Save Seed: " + GameAPI.matchSeed);
			}
		}
		if (propertiesThatChanged.ContainsKey("lp"))
		{
			LevelToPlay = (int)propertiesThatChanged["lp"];
		}
		if (propertiesThatChanged.ContainsKey("cu"))
		{
			byte[] array = (byte[])propertiesThatChanged["cu"];
			if (upgradeData != array)
			{
				VerboseDebug.Log("Got New Camera Upgrades! " + array.Length);
				CurrentCamera = new CameraUpgradeTable(array);
				if (array != Array.Empty<byte>())
				{
					m_SurfaceHandler.ShopHandler.ForceUpdate();
				}
			}
		}
		if (propertiesThatChanged.ContainsKey("nds"))
		{
			byte[] array2 = (byte[])propertiesThatChanged["nds"];
			if (array2 != null)
			{
				Debug.Log("Got Network Deals To Select: " + array2.Length);
				DeserializeNetworkDeals(array2);
			}
		}
		if (CurrentQuotaDay == DaysPerQutoa)
		{
			VerboseDebug.Log("Last Day To Complete Quota!");
		}
		m_OnUpdateAction?.Invoke();
		if (NetworkDealBoss.me != null)
		{
			NetworkDealBoss.me.OnStatusUpdated();
		}
	}

	public void AddOnUpdateAction(Action a)
	{
		m_OnUpdateAction = (Action)Delegate.Combine(m_OnUpdateAction, a);
	}

	public bool CanAfford(int cost)
	{
		return Money >= cost;
	}

	public void AddQuota(int quotaToAdd)
	{
		ReceivedQuota = true;
		CurrentQuota += quotaToAdd;
		try
		{
			if (NetworkDealBoss.me != null)
			{
				NetworkDealBoss.me.OnAddQuota(quotaToAdd);
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
		OnStatsUpdated();
	}

	public void SetNetworkDealsToSelect(NetworkDealBase[] deals)
	{
		NetworkDealsToSelect = deals;
		OnStatsUpdated();
	}

	public void LoadFromSave(Save currentSave)
	{
		Debug.Log("Loading save: " + currentSave);
		if (currentSave == null)
		{
			Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_NoSave), "");
			Debug.LogError("No Save Found!");
			return;
		}
		SaveLoadHandler.LoadSaveData(currentSave, this);
		if (QuotaToReach == 0)
		{
			int currentQuota = CurrentQuota;
			CalculateNewQuota();
			CurrentQuota = currentQuota;
		}
		Debug.Log("Loaded Save! " + currentSave);
		OnStatsUpdated();
	}

	private void Print()
	{
		Debug.Log($"Current Day {CurrentDay} Quota {CurrentQuota} Quota Day {CurrentQuotaDay} Money {Money}");
	}

	public bool HasUpgrade(byte cameraUpgradeID)
	{
		return CurrentCamera.HaveUpgrade(cameraUpgradeID);
	}

	public void ResetCameraUpgrades()
	{
		Debug.Log("Reseting Camera Upgrades!");
		CurrentCamera.ClearUpgrades();
		OnStatsUpdated();
	}

	public void SetCurrentRun(int run)
	{
		int currentDay = run * DaysPerQutoa;
		SetCurrentDay(currentDay);
	}

	public void SetCurrentDay(int day)
	{
		CurrentDay = day;
		ResetCurrentQuotaDay();
		NextDay();
		CalculateNewQuota();
		OnStatsUpdated();
		Debug.Log("Setting Day to: " + day);
	}
}
