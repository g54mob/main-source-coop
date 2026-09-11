using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Portningsbolaget.Photon;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zorro.Core;

public class Connection : MonoBehaviourPunCallbacks
{
	public bool connectToGlobalDev;

	public bool findCustomSpawnPoint;

	public bool offline;

	public static Connection instance;

	private const string GAME_SCENE = "SurfaceScene";

	public bool hasJoined;

	private HAuthTicket m_Ticket;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		ConnectionState connectionState = RetrievableSingleton<ConnectionStateHandler>.Instance.CheckState();
		if (connectionState is NoneState)
		{
			PhotonNetwork.SerializationRate = 30;
			PhotonNetwork.SendRate = 30;
			if (SteamManager.Initialized)
			{
				PhotonNetwork.NickName = SteamFriends.GetPersonaName();
			}
			if (offline)
			{
				PhotonNetwork.OfflineMode = true;
				PhotonNetwork.JoinRandomOrCreateRoom(null, 0);
			}
			else if (connectToGlobalDev && connectToGlobalDev)
			{
				PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "GlobalDev";
				PhotonNetwork.ConnectUsingSettings();
			}
		}
		else
		{
			if (connectionState is InRoomState)
			{
				return;
			}
			if (connectionState is JoiningSpecificRoomState joiningSpecificRoomState)
			{
				Debug.Log("Joining specific room: " + joiningSpecificRoomState.RoomToJoin);
				PhotonNetwork.JoinRoom(joiningSpecificRoomState.RoomToJoin);
			}
			else if (connectionState is JoiningSpecificRegionState joiningSpecificRegionState)
			{
				Debug.Log("Joining specific region: " + joiningSpecificRegionState.RegionToJoin);
				if (PhotonNetwork.IsConnected)
				{
					PhotonNetwork.Disconnect();
				}
				HAuthTicket ticket;
				string steamAuthTicket = SteamPhotonAuthManager.GetSteamAuthTicket(out ticket);
				PhotonNetwork.AuthValues = new AuthenticationValues();
				PhotonNetwork.AuthValues.UserId = SteamUser.GetSteamID().ToString();
				PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Steam;
				PhotonNetwork.AuthValues.AddAuthParameter("ticket", steamAuthTicket);
				PhotonNetwork.ConnectToRegion(joiningSpecificRegionState.RegionToJoin);
			}
		}
	}

	public void OnDestroy()
	{
	}

	private void OnJoinedSession(string region, string room)
	{
		Debug.Log("Got PhotonRoomKey " + room + " And Region " + region + " Switching Scene...");
		JoiningSpecificRoomFromRoomState joiningSpecificRoomFromRoomState = RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<JoiningSpecificRoomFromRoomState>();
		joiningSpecificRoomFromRoomState.RegionToJoin = region;
		joiningSpecificRoomFromRoomState.RoomToJoin = room;
		if (PhotonNetwork.IsConnected)
		{
			PhotonNetwork.Disconnect();
		}
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster, region: " + PhotonNetwork.CloudRegion);
		SteamPhotonAuthManager.CancelAuthTicket(m_Ticket);
		ConnectionState connectionState = RetrievableSingleton<ConnectionStateHandler>.Instance.CheckState();
		if (connectionState is DisconnectingState)
		{
			return;
		}
		if (!(connectionState is JoiningSpecificRegionState joiningSpecificRegionState))
		{
			if (connectionState is JoiningSpecificRoomFromRoomState joiningSpecificRoomFromRoomState)
			{
				JoiningSpecificRegionState joiningSpecificRegionState2 = RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<JoiningSpecificRegionState>();
				joiningSpecificRegionState2.RegionToJoin = joiningSpecificRoomFromRoomState.RegionToJoin;
				joiningSpecificRegionState2.RoomToJoin = joiningSpecificRoomFromRoomState.RoomToJoin;
				LoadGameScene("SurfaceScene");
			}
			else
			{
				Debug.Log("DEV Joining Room, GameVersion: " + PhotonNetwork.GameVersion);
				PhotonNetwork.JoinRandomOrCreateRoom(null, 0);
			}
		}
		else
		{
			RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<JoiningSpecificRoomState>();
			PhotonNetwork.JoinRoom(joiningSpecificRegionState.RoomToJoin);
		}
	}

	private void LoadGameScene(string sceneName)
	{
		if (SceneManager.GetActiveScene().name != "NewMainMenuOptimized")
		{
			ResetGame(init: true);
		}
		Debug.Log("Loading Game Scene: " + sceneName);
		PhotonNetwork.LoadLevel(sceneName);
	}

	public static void ResetGame(bool init)
	{
		SurfaceNetworkHandler.ResetSurface(init);
		NetworkStatistics.ResetStats();
		GlobalPlayerData.Cleanup();
		NetworkDealBoss.Cleanup();
		RetrievableSingleton<PersistentObjectsHolder>.Instance.ResetPersistantObjects();
		SaveSystem.Init();
		RetrievableSingleton<RecordingsHandler>.RemoveInstance();
		Player.justDied = false;
		RetrievableResourceSingleton<TransitionHandler>.Instance.AbortTransition();
		DiveBellTransitionSound.Remove();
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
		ConnectionState connectionState = RetrievableSingleton<ConnectionStateHandler>.Instance.CheckState();
		if (connectionState is JoiningSpecificRoomState || connectionState is NoneState)
		{
			RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<InRoomState>();
		}
		if (findCustomSpawnPoint)
		{
			GetComponent<SpawnHandler>().SpawnLocalPlayer(Spawns.CustomSpawn);
		}
		else if (PhotonGameLobbyHandler.IsSurface)
		{
			GetComponent<SpawnHandler>().SpawnLocalPlayer(Spawns.House);
		}
		else
		{
			GetComponent<SpawnHandler>().SpawnLocalPlayer(Spawns.DiveBell);
		}
	}

	public override void OnCustomAuthenticationFailed(string debugMessage)
	{
		base.OnCustomAuthenticationFailed(debugMessage);
		SteamPhotonAuthManager.CancelAuthTicket(m_Ticket);
	}

	public void SpawnLocalPlayer(ConnectionState state)
	{
		Debug.Log("Spawning local player");
		Transform transform = null;
		if (findCustomSpawnPoint)
		{
			transform = Object.FindObjectOfType<SpawnPoint>().transform;
		}
		else if (Player.justDied)
		{
			transform = Hospital.instance.transform;
		}
		else if ((bool)DiveBellParent.instance)
		{
			transform = DiveBellParent.instance.GetSpawn().transform;
		}
		else
		{
			SpawnPoint[] array = Object.FindObjectsOfType<SpawnPoint>();
			if (array.Length == 1)
			{
				transform = array[0].transform;
			}
			else
			{
				bool flag = true;
				if (state is InRoomState)
				{
					if (PhotonNetwork.IsMasterClient)
					{
						RoomStatsHolder roomStats = SurfaceNetworkHandler.RoomStats;
						if ((roomStats != null && roomStats.CurrentDay > 1) || TimeOfDayHandler.TimeOfDay == TimeOfDay.Evening)
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				transform = (flag ? array.First((SpawnPoint s) => s.UseOnStart).transform : array.First((SpawnPoint s) => !s.UseOnStart).transform);
			}
		}
		PhotonNetwork.Instantiate("Player", transform.transform.position, transform.transform.rotation, 0);
		hasJoined = true;
	}

	public override void OnJoinRoomFailed(short returnCode, string message)
	{
		base.OnJoinRoomFailed(returnCode, message);
		Debug.LogError("Join room failed: " + message);
		Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Join), message);
		RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect();
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		base.OnCreateRoomFailed(returnCode, message);
		Debug.LogError("Create room failed: " + message);
	}
}
