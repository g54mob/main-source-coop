using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using Portningsbolaget.Platforms;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zorro.Core;
using Zorro.Core.CLI;

public class SurfaceNetworkHandler : MonoBehaviourPunCallbacks
{
	[SerializeField]
	private PickupSpawner m_VideoCameraSpawner;

	public bool firstDay;

	private const int DAYS_PER_QUOTA = 3;

	private SteamLobbyHandler m_SteamLobby;

	private static bool m_Started;

	private bool m_HeadingToUnderWorld;

	private Action<RoomStatsHolder> m_OnStatsChangedAction;

	private PhotonView m_View;

	private VoiceConnection m_VoiceConnection;

	private static ICollection<Player> PlayersAliveFromUnderWorld;

	public Action ReturnToSurfaceAction;

	private bool m_RequestedSleep;

	public Action StartGameAction;

	private bool m_FailedQuota;

	public static SurfaceNetworkHandler Instance { get; private set; }

	public static bool HasStarted => m_Started;

	public static bool ReturnedFromLostWorldWithCamera { get; private set; }

	public static RoomStatsHolder RoomStats { get; private set; }

	public ShopHandler ShopHandler { get; private set; }

	public static int NumberOfPlayersAliveFromUnderWorld => PlayersAliveFromUnderWorld?.Count ?? 0;

	private void Awake()
	{
		Debug.Log("SurfaceNetworkHandler Awake! InRoom : " + PhotonNetwork.InRoom);
		Instance = this;
		m_VoiceConnection = UnityEngine.Object.FindObjectOfType<VoiceConnection>();
		ShopHandler = UnityEngine.Object.FindObjectOfType<ShopHandler>();
	}

	private void Start()
	{
		if (PhotonNetwork.InRoom)
		{
			InitSurface();
			if (m_Started)
			{
				RPCA_OpenDoor();
				OpenComputerRoomDoor();
				ReturnToSurfaceAction?.Invoke();
			}
		}
	}

	[PunRPC]
	public void OpenComputerRoomDoor()
	{
		if (RoomStats.CurrentRun >= 1)
		{
			UnityEngine.Object.FindObjectOfType<ComputerRoomDoor>().OpenDoor();
			PlatformManager.UnlockAchievement(Achievements.ACH_MONITOR_ROOM);
		}
	}

	private void InitSurface()
	{
		Debug.Log("Initializing Surface");
		m_View = GetComponent<PhotonView>();
		m_SteamLobby = MainMenuHandler.SteamLobbyHandler;
		if (RoomStats == null)
		{
			RoomStats = new RoomStatsHolder(this, SingletonAsset<BigNumbers>.Instance.StartMoney, BigNumbers.GetQuota(0), 3);
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.CurrentRoom.IsOpen = true;
				PhotonNetwork.CurrentRoom.IsVisible = true;
				PhotonGameLobbyHandler.Instance.SetCurrentObjective(new InviteFriendsObjective());
				CheckSave();
			}
			else
			{
				OnRoomPropertiesUpdate(PhotonNetwork.CurrentRoom.CustomProperties);
			}
			if (RoomStats.CurrentDay <= 1)
			{
				firstDay = true;
				string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HelmetWelcome);
				HelmetText.Instance.SetHelmetText(localizedString, 3f);
				Debug.Log("NEW RUN!");
				if (PhotonNetwork.IsMasterClient)
				{
					SaveSystem.SaveToDisk();
					SpawnSurfacePickups();
				}
			}
			else if (PhotonNetwork.IsMasterClient)
			{
				m_VideoCameraSpawner.SpawnMe(force: true);
			}
			if (m_SteamLobby != null)
			{
				m_SteamLobby.OpenLobby();
			}
			SpawnHandler.Instance.SpawnLocalPlayer(Spawns.House);
		}
		else
		{
			Debug.Log("Should do next day here but waiting for upload?");
			if (TimeOfDayHandler.TimeOfDay == TimeOfDay.Evening)
			{
				RichPresenceHandler.SetPresenceState(RichPresenceState.Status_AtHouse);
				if (PhotonNetwork.IsMasterClient)
				{
					ReturnedFromLostWorldWithCamera = CheckIfCameraIsPresent(includeBrokencamera: true);
					if (!ReturnedFromLostWorldWithCamera)
					{
						RoomStats.ResetCameraUpgrades();
						if (PhotonNetwork.IsMasterClient)
						{
							PhotonGameLobbyHandler.Instance.SetCurrentObjective(new GoToBedFailedObjective());
							if (RoomStats.IsQuotaDay && !RoomStats.CalculateIfReachedQuota())
							{
								NextDay();
							}
						}
					}
					else if (PhotonNetwork.IsMasterClient)
					{
						PhotonGameLobbyHandler.Instance.SetCurrentObjective(new ExtractVideoObjective());
					}
					if (!m_FailedQuota)
					{
						CheckForHospitalBill();
					}
				}
				if (!Player.justDied && !m_FailedQuota)
				{
					SpawnHandler.Instance.SpawnLocalPlayer(Spawns.DiveBell);
				}
			}
		}
		if (!m_FailedQuota)
		{
			ShopHandler.InitShopHandler();
		}
	}

	private void CheckForHospitalBill()
	{
		List<Photon.Realtime.Player> list = PhotonNetwork.CurrentRoom.Players.Values.ToList();
		List<(int, int)> hospitalBill = new List<(int, int)>();
		foreach (Player alivePlayer in PlayersAliveFromUnderWorld)
		{
			Photon.Realtime.Player player = list.Find((Photon.Realtime.Player p) => p.ActorNumber == alivePlayer.refs.view.OwnerActorNr);
			if (player != null)
			{
				list.Remove(player);
			}
		}
		string text = "Checking Hospital Bill, Dead Players Are: ";
		int count = list.Count;
		float num = 0.1f;
		int num2 = Mathf.RoundToInt((float)RoomStats.Money * (num * (float)count));
		int item = Mathf.RoundToInt((float)num2 / (float)count);
		foreach (Photon.Realtime.Player item2 in list)
		{
			text = text + item2.NickName + " ID: " + item2.ActorNumber + " \n";
			hospitalBill.Add((item2.ActorNumber, item));
		}
		MonoFunctions.instance.DelayCall(delegate
		{
			SendHospitalBill(hospitalBill);
		}, 3f);
		text = text + " Total Bill is: " + num2 + "$";
		Debug.Log(text);
	}

	private void SendHospitalBill(List<(int, int)> hospitalBill)
	{
		foreach (var item in hospitalBill)
		{
			m_View.RPC("RPCA_HospitalBill", RpcTarget.All, item.Item1, item.Item2);
		}
	}

	[PunRPC]
	public void RPCA_HospitalBill(int actorNumber, int moneyToRemove)
	{
		Photon.Realtime.Player player = PhotonNetwork.PlayerList.First((Photon.Realtime.Player player2) => player2.ActorNumber == actorNumber);
		if (PhotonNetwork.IsMasterClient)
		{
			RoomStats.RemoveMoney(moneyToRemove);
		}
		UserInterface.ShowMoneyNotification(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HospitalBill).Replace("{InsertName}", player.NickName), $"${moneyToRemove}", MoneyCellUI.MoneyCellType.HospitalBill);
		if (player.IsLocal && PlatformManager.Platform is SteamRuntimeManager steamRuntimeManager)
		{
			steamRuntimeManager.AddDeath(moneyToRemove);
		}
	}

	private void SpawnSurfacePickups()
	{
		PickupSpawner[] array = UnityEngine.Object.FindObjectsOfType<PickupSpawner>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SpawnMe();
		}
	}

	private void CheckSave()
	{
		if (SaveSystem.HaveCurrentSave)
		{
			Save currentSave = SaveSystem.CurrentSave;
			if (currentSave.Valid && currentSave.SerializedSave.CurrentDay > 0)
			{
				LoadSave();
			}
		}
	}

	private void LoadSave()
	{
		RoomStats.LoadFromSave(SaveSystem.CurrentSave);
	}

	public static void SetPlayersAliveFromUnderworld(ICollection<Player> playersInside)
	{
		if (playersInside == null)
		{
			playersInside = new List<Player>();
		}
		PlayersAliveFromUnderWorld = playersInside;
		string text = "Players alive from underworld: \n";
		foreach (Player item in PlayersAliveFromUnderWorld)
		{
			text = text + item.refs.view.Owner.NickName + "\n";
		}
		VerboseDebug.Log(text);
	}

	public static void ReturnFromLostWorld()
	{
		TimeOfDayHandler.SetTimeOfDay(TimeOfDay.Evening);
	}

	public static void ResetReturningFromLostWorld()
	{
		TimeOfDayHandler.SetTimeOfDay(TimeOfDay.Morning);
	}

	public static void ResetSurface(bool init, bool resetTimeOfDay = true)
	{
		if (init)
		{
			m_Started = false;
		}
		RoomStats = null;
		if (resetTimeOfDay)
		{
			ResetReturningFromLostWorld();
		}
		RetrievableSingleton<PersistentObjectsHolder>.Instance.ResetPersistantObjects();
	}

	public override void OnJoinedRoom()
	{
		base.OnJoinedRoom();
		InitSurface();
	}

	public override void OnLeftRoom()
	{
		m_Started = false;
	}

	private void Update()
	{
		if (TimeOfDayHandler.TimeOfDay == TimeOfDay.Evening && PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
		{
			CheckForSleep();
		}
		CheckForMasterClientCommands();
	}

	private void CheckForSleep()
	{
		if (PlayerHandler.instance.AllPlayersAsleep())
		{
			RequestSleep();
		}
	}

	private void CheckForMasterClientCommands()
	{
		_ = PhotonNetwork.IsMasterClient;
	}

	private void NextDay()
	{
		if (!m_Started)
		{
			Debug.LogError("Can't go to next day before started game");
			return;
		}
		Debug.Log("Next Day");
		RoomStats.NextDay();
		Debug.Log("Getting Money each day for debug");
		OnMoneyAdd(SingletonAsset<BigNumbers>.Instance.MoneyPerRound);
	}

	private void OnMoneyAdd(int moneyAdded)
	{
		if (m_Started)
		{
			Debug.Log("Adding money: " + moneyAdded);
			RoomStats.AddMoney(moneyAdded);
		}
	}

	private void OnMoneySubtract(int moneySubtracted)
	{
		if (m_Started)
		{
			RoomStats.RemoveMoney(moneySubtracted);
		}
	}

	[PunRPC]
	public void RequestSleep()
	{
		if (TimeOfDayHandler.TimeOfDay != TimeOfDay.Evening)
		{
			Debug.LogError("Got this sleep call when not evening, not good...");
		}
		else if (!m_RequestedSleep)
		{
			Debug.Log("Requesting Sleep!");
			m_RequestedSleep = true;
			if (PhotonNetwork.IsMasterClient)
			{
				m_View.RPC("RPCA_Sleep", RpcTarget.All);
			}
			else
			{
				m_View.RPC("RequestSleep", RpcTarget.MasterClient);
			}
		}
	}

	[PunRPC]
	private void RPCA_Sleep()
	{
		m_RequestedSleep = false;
		ResetReturningFromLostWorld();
		UnityEngine.Object.FindObjectOfType<ShopHandler>().InitShopScreen();
		RetrievableResourceSingleton<TransitionHandler>.Instance.TransitionToBlack(0f, OnSlept, 0f);
	}

	private void OnSlept()
	{
		Action action = Player.localPlayer.WakeUp;
		if ((bool)Player.localPlayer && Player.localPlayer.refs.headPos.position.y <= 6.5f)
		{
			PlatformManager.UnlockAchievement(Achievements.ACH_CEILING_SLEEP);
		}
		if (PhotonNetwork.IsMasterClient)
		{
			NextDay();
			if (RoomStats.Money >= 20)
			{
				PhotonGameLobbyHandler.Instance.SetCurrentObjective(new BuyEquipmentObjective());
			}
			else
			{
				PhotonGameLobbyHandler.Instance.SetCurrentObjective(new EnterDiveBellDay2Objective());
			}
			int daysLocaleKey = -1;
			if (RoomStats != null)
			{
				int daysLeft = RoomStats.GetDaysLeft();
				if (daysLeft == 0)
				{
					daysLocaleKey = 63;
				}
				else
				{
					daysLocaleKey = 62;
				}
				if (!CheckIfCameraIsPresent(includeBrokencamera: false))
				{
					m_VideoCameraSpawner.SpawnMe(force: true);
				}
				ReturnedFromLostWorldWithCamera = true;
				action = (Action)Delegate.Combine(action, (Action)delegate
				{
					m_View.RPC("RPCA_HelmetText", RpcTarget.All, daysLocaleKey, daysLeft);
				});
			}
			RetrievableResourceSingleton<TransitionHandler>.Instance.FadeOut(action);
		}
		RetrievableResourceSingleton<TransitionHandler>.Instance.FadeOut(action);
	}

	[PunRPC]
	private void RPCM_StartGame()
	{
		if (PhotonNetwork.IsMasterClient && !m_Started)
		{
			m_View.RPC("RPCA_OpenDoor", RpcTarget.All);
			m_Started = true;
			Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
			if (NetworkDealBoss.me != null)
			{
				NetworkDealBoss.me.HardSyncDeal();
			}
			PhotonGameLobbyHandler.Instance.SetCurrentObjective(new PickupTheCameraObjective());
			string text = $"{playerList.Length} Players\n";
			Photon.Realtime.Player[] array = playerList;
			foreach (Photon.Realtime.Player player in array)
			{
				text = text + player.NickName + "\n";
			}
			Debug.Log("Starting Game, Locking In Players\n" + text);
			if (m_SteamLobby != null)
			{
				m_SteamLobby.HideLobby();
			}
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.CurrentRoom.IsOpen = false;
				PhotonNetwork.CurrentRoom.IsVisible = false;
			}
			if (RoomStats.CurrentDay != RoomStats.CurrentQuotaDay)
			{
				m_View.RPC("OpenComputerRoomDoor", RpcTarget.All);
			}
			StartActivity();
			StartGameAction?.Invoke();
		}
	}

	[PunRPC]
	private void RPCA_OpenDoor()
	{
		VerboseDebug.Log("Opening Door!");
		StartGameDoorInteractable startGameDoorInteractable = UnityEngine.Object.FindObjectOfType<StartGameDoorInteractable>();
		startGameDoorInteractable.transform.root.GetComponentInChildren<Animator>().Play("DoorAnim");
		startGameDoorInteractable.gameObject.SetActive(value: false);
		m_Started = true;
		RichPresenceHandler.SetPresenceState(RichPresenceState.Status_AtHouse);
	}

	private void StartActivity()
	{
		if (PlayerHandler.instance.players.Count == 1)
		{
			return;
		}
		Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
		string text = PlatformUtility.PlatformFamily.Playstation.ToString();
		Photon.Realtime.Player[] array = playerList;
		foreach (Photon.Realtime.Player player in array)
		{
			if ((string)player.CustomProperties["PlatformFamily"] == text)
			{
				Debug.Log(player.NickName + " is Starting Match");
				m_View.RPC("RPC_StartMatch", player);
				break;
			}
		}
	}

	[PunRPC]
	private void RPC_StartMatch()
	{
	}

	public bool PreCheckHeadToUnderWorld()
	{
		if (!m_Started)
		{
			Debug.LogError("Cant head to underworld before started game");
			return false;
		}
		if (!CheckIfCameraIsPresent(includeBrokencamera: false))
		{
			VideoCamera[] array = UnityEngine.Object.FindObjectsOfType<VideoCamera>();
			for (int i = 0; i < array.Length; i++)
			{
				PhotonView component = array[i].transform.parent.GetComponent<PhotonView>();
				if (component != null)
				{
					PhotonNetwork.Destroy(component);
				}
			}
			m_VideoCameraSpawner.SpawnMe(force: true);
			m_View.RPC("RPCA_HelmetText", RpcTarget.All, 64, -1);
			return false;
		}
		if (m_HeadingToUnderWorld)
		{
			return false;
		}
		m_HeadingToUnderWorld = true;
		return true;
	}

	private bool CheckIfCameraIsPresent(bool includeBrokencamera)
	{
		Photon.Realtime.Player[] playerList = PhotonNetwork.PlayerList;
		foreach (Photon.Realtime.Player player in playerList)
		{
			if (GlobalPlayerData.TryGetPlayerData(player, out var globalPlayerData))
			{
				foreach (ItemDescriptor item in globalPlayerData.inventory.GetItems())
				{
					if (item.item == m_VideoCameraSpawner.ItemToSpawn)
					{
						VerboseDebug.Log("Found Camera! " + player.NickName + " Has it!");
						return true;
					}
					if (includeBrokencamera && item.item.itemType == Item.ItemType.Camera)
					{
						VerboseDebug.Log("Found Broken Camera! " + player.NickName + " Has it!");
						return true;
					}
				}
			}
			else
			{
				Debug.LogError("Cant find playerData for Player: " + player.NickName + " Bug!?");
			}
		}
		foreach (Pickup item2 in UnityEngine.Object.FindObjectOfType<DiveBellPickupDetector>().CheckForPickups())
		{
			if (item2.itemInstance.item == m_VideoCameraSpawner.ItemToSpawn)
			{
				Debug.Log("Found Camera On Ground!");
				return true;
			}
			if (includeBrokencamera && item2.itemInstance.item.itemType == Item.ItemType.Camera)
			{
				Debug.Log("Found Broken Camera On Ground!");
				return true;
			}
		}
		return false;
	}

	public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		base.OnRoomPropertiesUpdate(propertiesThatChanged);
		if (RoomStats == null)
		{
			Debug.LogError("RoomStats Null!?", this);
			return;
		}
		RoomStats.Update(propertiesThatChanged);
		m_OnStatsChangedAction?.Invoke(RoomStats);
	}

	public void FailedQuota()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			Action a = delegate
			{
				m_View.RPC("RPC_LoadScene", RpcTarget.All, "SurfaceScene");
			};
			UnityEngine.Object.FindObjectOfType<EndGameScreen>().StartWatching(a);
			m_View.RPC("RPC_QuotaFailed", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPCA_HelmetText(int messageLocaleKey, int daysLeft)
	{
		string empty = string.Empty;
		empty = ((messageLocaleKey != 62) ? LocalizationKeys.GetLocalizedString((LocalizationKeys.Keys)messageLocaleKey) : LocalizationKeys.GetLocalizedString((LocalizationKeys.Keys)messageLocaleKey).Replace("{0}", (daysLeft + 1).ToString()));
		HelmetText.Instance.SetHelmetText(empty, 3f);
	}

	[PunRPC]
	public void RPC_QuotaFailed()
	{
		Debug.Log("RPC Quota Failed");
		m_FailedQuota = true;
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HelmetFailed);
		HelmetText.Instance.SetHelmetText(localizedString, 2f);
		SaveSystem.DeleteCurrentSave();
		ResetSurface(init: false);
		if (Player.localPlayer.TryGetInventory(out var o))
		{
			o.Clear();
		}
	}

	private IEnumerator FailCoroutine()
	{
		yield return new WaitForSecondsRealtime(3f);
		Debug.Log("Restarting In 3...");
		yield return new WaitForSecondsRealtime(1f);
		Debug.Log("Restarting In 2...");
		yield return new WaitForSecondsRealtime(1f);
		Debug.Log("Restarting In 1...");
		yield return new WaitForSecondsRealtime(1f);
		if (PhotonNetwork.IsMasterClient)
		{
			m_View.RPC("RPC_LoadScene", RpcTarget.All, "SurfaceScene");
		}
	}

	public void AddOnStatsUpdateCallBack(Action<RoomStatsHolder> onQuotaUpdate)
	{
		m_OnStatsChangedAction = (Action<RoomStatsHolder>)Delegate.Combine(m_OnStatsChangedAction, onQuotaUpdate);
	}

	[PunRPC]
	private void RPC_LoadScene(string level)
	{
		Debug.Log("RPC Load Scene To Level: " + level + " Stopping Queue");
		PhotonNetwork.IsMessageQueueRunning = false;
		SceneManager.LoadScene(level);
	}

	[ConsoleCommand]
	public static void SetCurrentRun(int run)
	{
		RoomStats.SetCurrentRun(run);
		HelmetText.Instance.SetHelmetText("Day " + RoomStats.CurrentDay, 3f);
	}

	[ConsoleCommand]
	public static void SetCurrentDay(int day)
	{
		RoomStats.SetCurrentDay(day);
		HelmetText.Instance.SetHelmetText("Day " + RoomStats.CurrentDay, 3f);
	}

	[ConsoleCommand]
	public static void AddQuota(int quota)
	{
		RoomStats.AddQuota(quota);
	}

	public void RequestStartGame()
	{
		if (!HasStarted)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				RPCM_StartGame();
			}
			else
			{
				m_View.RPC("RPCM_StartGame", RpcTarget.MasterClient);
			}
		}
	}

	public void NewWeek(int currentRun)
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		if (currentRun >= 1)
		{
			List<NetworkDealBase> weightedRandomDeal = SingletonAsset<NetworkDealDataBase>.Instance.GetWeightedRandomDeal(new List<DIFFICULTY>
			{
				DIFFICULTY.veryEasy,
				DIFFICULTY.easy,
				DIFFICULTY.medium,
				DIFFICULTY.hard,
				DIFFICULTY.veryHard
			}, 3, allowDuplicates: false);
			foreach (NetworkDealBase item in weightedRandomDeal)
			{
				item.Init(SingletonAsset<DealRewardDatabase>.Instance.GetRandom(), item.AllowedDifficulties.GetRandom());
			}
			RoomStats.SetNetworkDealsToSelect(weightedRandomDeal.ToArray());
		}
		m_View.RPC("RPCA_OnNewWeek", RpcTarget.All, currentRun);
	}

	[PunRPC]
	public void RPCA_OnNewWeek(int currentRun)
	{
		currentRun = Mathf.Clamp(currentRun, 0, int.MaxValue);
		MetaProgressionHandler.AddMetaCoins(BigNumbers.GetMetaScoreFromRun(currentRun));
		if (currentRun == 1)
		{
			OpenComputerRoomDoor();
		}
	}
}
