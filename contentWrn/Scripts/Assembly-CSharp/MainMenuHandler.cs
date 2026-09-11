using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayEveryWare.VideoEncoding;
using Portningsbolaget;
using Portningsbolaget.Photon;
using Portningsbolaget.Platforms;
using Portningsbolaget.Utilities;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zorro.Core;
using Zorro.Settings;

[DefaultExecutionOrder(1)]
public class MainMenuHandler : MonoBehaviourPunCallbacks
{
	public MainMenuUIHandler UIHandler;

	private const int MAX_PLAYERS = 4;

	private const string GAME_SCENE = "SurfaceScene";

	private const string MATCHMAKING_CODE = "C";

	private const string MATCHMAKING_LANGUAGE = "L";

	private const string MATCHMAKING_PRIVATE = "P";

	private const string MATCHMAKING_PLATFORMS = "PL";

	private const string MATCHMAKING_MODS = "M";

	private static MainMenuHandler _instance;

	public static bool CrossPlatform;

	private bool m_Hosting;

	private Coroutine m_JoinRandomCoroutine;

	private bool m_AwaitingJoinEvent;

	private ConnectionState m_PreviousState;

	private bool m_waitingForResponse;

	private static bool m_CheckedSteamCMD;

	private Dictionary<string, RoomInfo> m_cachedRoomList = new Dictionary<string, RoomInfo>();

	public static MainMenuHandler Instance => _instance;

	public static SteamLobbyHandler SteamLobbyHandler { get; set; }

	private void Awake()
	{
		_instance = this;
		CommandLineArgumentReader.Init();
		m_PreviousState = RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.CurrentState;
		RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<NoneState>();
		StartCoroutine(SetPhotonNameAsync());
		if (PhotonNetwork.InRoom)
		{
			Debug.Log("Leaving Room: " + PhotonNetwork.CurrentRoom.Name);
			PhotonNetwork.LeaveRoom();
		}
		ResetGame();
		StartCoroutine(PhotonStateNotifier());
	}

	public static void ResetGame()
	{
		SurfaceNetworkHandler.ResetSurface(init: false);
		TimeOfDayHandler.SetTimeOfDay(TimeOfDay.Evening);
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

	private string GetPrettyStateName()
	{
		ClientState networkClientState = PhotonNetwork.NetworkClientState;
		if (networkClientState == ClientState.Authenticating || networkClientState == ClientState.ConnectingToMasterServer || (uint)(networkClientState - 15) <= 2u)
		{
			return LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Connecting);
		}
		return networkClientState.ToString();
	}

	private void OnDestroy()
	{
		_instance = null;
		TimeOfDayHandler.SetTimeOfDay(TimeOfDay.Morning);
	}

	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Steam_Title);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Steam_Body);
			Modal.Show(localizedString, localizedString2, new ModalOption[1]
			{
				new ModalOption(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Ok))
			}, delegate
			{
				Application.Quit();
			});
		}
		else if (CheckAVXSupport())
		{
			Debug.Log("MainMenuHandler Start");
			Debug.Log($"Photon State: {PhotonNetwork.NetworkClientState}");
			IPlatform platform = PlatformManager.Platform;
			platform.OnJoinedSession = (Action<string, string>)Delegate.Combine(platform.OnJoinedSession, new Action<string, string>(OnJoinedSession));
			if (m_PreviousState is JoiningSpecificRoomFromRoomState joiningSpecificRoomFromRoomState)
			{
				Debug.Log("Joining Specific Room From Room State: " + joiningSpecificRoomFromRoomState.RoomToJoin);
				JoinRoom(joiningSpecificRoomFromRoomState.RoomToJoin);
			}
		}
	}

	private void OnJoinedSession(string region, string room)
	{
		JoinRoom(room);
	}

	private bool CheckAVXSupport()
	{
		if (!NativeVeMethods.cpu_has_required_simd_extensions())
		{
			Debug.LogError("CPU does not support AVX, showing error and exiting.");
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_Warning);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_Lacking_AVX);
			Modal.Show(localizedString, localizedString2, new ModalOption[1]
			{
				new ModalOption(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Ok))
			}, delegate
			{
				Application.Quit();
			});
			return false;
		}
		Debug.Log("AVX check succeeded.");
		return true;
	}

	public void JoinRoom(string room)
	{
		if (!PhotonNetwork.OfflineMode)
		{
			StartCoroutine(PhotonAuthManager.PhotonAuth.Authenticate(OnPhotonAuthenticated));
			StartCoroutine(WaitForPhotonState(ClientState.ConnectedToMasterServer, delegate
			{
				CheckRoomCode(room.ToUpper());
			}));
		}
	}

	private void CheckRoomCode(string code)
	{
		if (Utilities.CodeToModState(code[1]))
		{
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_Warning);
			string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_ModWarning_Body);
			string localizedString3 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Modal_Continue);
			string localizedString4 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Continue);
			string text = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel).ToUpper();
			ModalOption[] options = new ModalOption[2]
			{
				new ModalOption(localizedString4, delegate
				{
					JoinRoomInternal(code, Utilities.CodeToRegion(code[0]));
				}),
				new ModalOption(text, delegate
				{
					UIHandler.TransistionToPage<MainMenuMainPage>();
				})
			};
			Modal.Show(localizedString, localizedString2 + "\n" + localizedString3, options);
		}
		else
		{
			JoinRoomInternal(code, Utilities.CodeToRegion(code[0]));
		}
	}

	private void JoinRoomInternal(string room, string region)
	{
		Debug.Log("Got PhotonRoomKey " + room + " And Region " + region + " Switching Scene...");
		if (region != PhotonNetwork.CloudRegion || PhotonNetwork.InRoom)
		{
			Debug.Log("Not On Same Cloud Region, Needs Switching: Current: " + PhotonNetwork.CloudRegion + " Wanted: " + region);
			JoiningSpecificRegionState joiningSpecificRegionState = RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<JoiningSpecificRegionState>();
			joiningSpecificRegionState.RegionToJoin = region;
			joiningSpecificRegionState.RoomToJoin = room;
		}
		else
		{
			RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<JoiningSpecificRoomState>().RoomToJoin = room;
		}
		LoadGameScene();
	}

	private void OnJoinedSteamLobby(string region, string room, bool forceRegion)
	{
		Debug.Log("Got PhotonRoomKey From Steam " + room + " And Region " + region + " Switching Scene...");
		if (region != PhotonNetwork.CloudRegion || forceRegion)
		{
			Debug.Log("Not On Same Cloud Region, Needs Switching: Current: " + PhotonNetwork.CloudRegion + " Wanted: " + region);
			JoiningSpecificRegionState joiningSpecificRegionState = RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<JoiningSpecificRegionState>();
			joiningSpecificRegionState.RegionToJoin = region;
			joiningSpecificRegionState.RoomToJoin = room;
		}
		else
		{
			RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<JoiningSpecificRoomState>().RoomToJoin = room;
		}
		LoadGameScene();
	}

	private void LoadGameScene()
	{
		if (SceneManager.GetActiveScene().name != "NewMainMenuOptimized")
		{
			ResetGame();
		}
		Debug.Log("Loading Game Scene: SurfaceScene");
		PhotonNetwork.LoadLevel("SurfaceScene");
	}

	private void DeleteTempFolder()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(RecordingsHandler.GetDirectory());
		try
		{
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete(recursive: true);
				Debug.Log("Deleting Folder: " + directoryInfo.FullName);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed to delete directory: " + directoryInfo.FullName + " " + ex.Message);
			Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Delete_TempFolder), directoryInfo.FullName + "\n" + ex.Message);
		}
	}

	private void OnPhotonAuthenticated()
	{
		ConnectToPhoton();
		Debug.Log("Setting up SteamLobbyHandler");
		if (SteamLobbyHandler != null)
		{
			SteamLobbyHandler.LeaveLobby();
		}
		else
		{
			SteamLobbyHandler = new SteamLobbyHandler(4, useSteamNetwork: true);
		}
		SteamLobbyHandler.ClearAndReset(OnJoinedSteamLobby);
		RichPresenceHandler.Clear();
		RichPresenceHandler.SetGroupSize(0, 0);
		RichPresenceHandler.SetPresenceState(RichPresenceState.Status_MainMenu);
		DeleteTempFolder();
	}

	private void ConnectToPhoton()
	{
		string matchmakingVersion = PlatformsConfig.GetMatchmakingVersion();
		PhotonNetwork.SerializationRate = 30;
		PhotonNetwork.SendRate = 30;
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.GameVersion = matchmakingVersion;
		PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = matchmakingVersion;
		PhotonNetwork.ConnectUsingSettings();
		Debug.Log("Photon Start" + PhotonNetwork.NetworkClientState.ToString() + " using app version: " + matchmakingVersion);
	}

	private IEnumerator SetPhotonNameAsync()
	{
		PhotonNetwork.NickName = "Артём";
		yield return new WaitUntil(() => PlatformManager.Platform.Initialized);
		PhotonNetwork.NickName = PlatformManager.Platform.NickName;
		Debug.Log("Setting name to " + PhotonNetwork.NickName);
	}

	private static IEnumerator PhotonStateNotifier()
	{
		yield return null;
		ClientState prevState = PhotonNetwork.NetworkClientState;
		Debug.Log("PHOTON_STATE: Starting State=" + PhotonNetwork.NetworkClientState);
		while (true)
		{
			if (PhotonNetwork.NetworkClientState != prevState)
			{
				Debug.Log("PHOTON_STATE: PrevState=" + prevState.ToString() + ", NewState=" + PhotonNetwork.NetworkClientState);
				prevState = PhotonNetwork.NetworkClientState;
			}
			yield return null;
		}
	}

	private IEnumerator WaitForPhotonState(ClientState state, Action callback)
	{
		MainMenuLoadingPage mainMenuLoadingPage = UIHandler.TransistionToPage<MainMenuLoadingPage>();
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Connecting);
		mainMenuLoadingPage.SetText(localizedString);
		while (PhotonNetwork.NetworkClientState != state)
		{
			Debug.Log("Waiting for Photon... " + PhotonNetwork.NetworkClientState);
			yield return new WaitForSeconds(1f);
		}
		callback?.Invoke();
	}

	public void Host(int saveIndex)
	{
		if (PhotonNetwork.OfflineMode)
		{
			HostInternal(saveIndex);
			return;
		}
		StartCoroutine(PhotonAuthManager.PhotonAuth.Authenticate(OnPhotonAuthenticated));
		StartCoroutine(WaitForPhotonState(ClientState.ConnectedToMasterServer, delegate
		{
			HostInternal(saveIndex);
		}));
	}

	private void HostInternal(int saveIndex)
	{
		if (!m_Hosting)
		{
			MainMenuLoadingPage mainMenuLoadingPage = UIHandler.TransistionToPage<MainMenuLoadingPage>();
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HostingGame);
			mainMenuLoadingPage.SetText(localizedString);
			SaveSystem.SelectCurrentSave(saveIndex);
			SaveSystem.UsingSave(b: true);
			SteamLobbyHandler.HostMatch(OnLobbyHosted, privateMatch: true);
			m_Hosting = true;
		}
	}

	public void SilentHost()
	{
		if (!m_Hosting)
		{
			SaveSystem.UsingSave(b: false);
			SteamLobbyHandler.HostMatch(OnLobbyHosted, privateMatch: false);
			m_Hosting = true;
		}
	}

	public void JoinRandom()
	{
		if (!PhotonNetwork.OfflineMode)
		{
			StartCoroutine(PhotonAuthManager.PhotonAuth.Authenticate(OnPhotonAuthenticated));
			StartCoroutine(WaitForPhotonState(ClientState.ConnectedToMasterServer, JoinRandomInternal));
		}
	}

	private void JoinRandomInternal()
	{
		if (m_JoinRandomCoroutine != null)
		{
			Debug.LogError("JoinRandom is still being processed");
		}
		else
		{
			m_JoinRandomCoroutine = StartCoroutine(JoinRandomCoroutine());
		}
		IEnumerator JoinRandomCoroutine()
		{
			MainMenuLoadingPage loadingPage = UIHandler.TransistionToPage<MainMenuLoadingPage>();
			string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.JoinRandom);
			List<Setting> settings = GameHandler.Instance.SettingsHandler.GetSettings(SettingCategory.Matchmaking);
			bool flag = false;
			PlatformUtility.PlatformFamily platforms = (CrossPlatform ? ((PlatformUtility.PlatformFamily)(-1)) : PlatformUtility.CurrentPlatform);
			Debug.Log(string.Format("Targeted Platform: {0}", (platforms < PlatformUtility.PlatformFamily.None) ? "All" : ((object)platforms)));
			foreach (Setting item in settings)
			{
				if (item is LanguageMatchmakingSetting languageMatchmakingSetting)
				{
					LanguageMatchmakingSetting.MatchmakingLanguage language = (LanguageMatchmakingSetting.MatchmakingLanguage)languageMatchmakingSetting.Value;
					if (language != LanguageMatchmakingSetting.MatchmakingLanguage.None)
					{
						Debug.Log($"Joining Random Photon Room: {language}");
						loadingPage.SetText(localizedString + Environment.NewLine + language);
						PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable
						{
							{ "P", 0 },
							{ "L", language },
							{
								"PL",
								(int)platforms
							},
							{
								"M",
								GameHandler.GetPluginHash()
							}
						}, 4);
						m_AwaitingJoinEvent = true;
						while (m_AwaitingJoinEvent)
						{
							yield return null;
						}
						flag = PhotonNetwork.CurrentRoom != null;
						if (flag)
						{
							Debug.Log("Joined Random Photon Room: " + PhotonNetwork.CurrentRoom.Name);
							break;
						}
						Debug.Log($"Found No Photon Room: {language}");
					}
				}
			}
			if (!flag)
			{
				Debug.Log("Found No Matches: Hosting");
				PreferredLanguageMatchmakingSetting preferredLanguageMatchmakingSetting = (PreferredLanguageMatchmakingSetting)settings.Find((Setting s) => s is PreferredLanguageMatchmakingSetting);
				LanguageMatchmakingSetting.MatchmakingLanguage language = (LanguageMatchmakingSetting.MatchmakingLanguage)preferredLanguageMatchmakingSetting.Value;
				MainMenuLoadingPage mainMenuLoadingPage = UIHandler.TransistionToPage<MainMenuLoadingPage>();
				localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.HostingGame);
				mainMenuLoadingPage.SetText(localizedString + Environment.NewLine + language);
				SilentHost();
			}
			m_JoinRandomCoroutine = null;
		}
	}

	public void CanPlayMultiplayer(bool showModal, Action<bool> onClick)
	{
		if (!m_waitingForResponse)
		{
			m_waitingForResponse = true;
			PlatformUtility.LoadServerSettings(delegate
			{
				m_waitingForResponse = false;
				onClick?.Invoke(obj: true);
			});
		}
	}

	private void ShowPlayOfflineModal(Action<bool> onClick)
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PlayOffline);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.PlayOffline);
		Modal.Show(localizedString, localizedString2, new ModalOption[2]
		{
			new ModalOption(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Yes), delegate
			{
				PhotonNetwork.Disconnect();
				PhotonNetwork.OfflineMode = true;
				onClick?.Invoke(obj: false);
			}),
			new ModalOption(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Cancel))
		});
	}

	public override void OnCreatedRoom()
	{
		base.OnCreatedRoom();
		Debug.Log("Hosted Photon Room: " + PhotonNetwork.CurrentRoom.Name + " Cluster: " + PhotonNetwork.NetworkingClient.GameServerAddress);
		SteamLobbyHandler.SetLobbyName(PhotonNetwork.CurrentRoom.Name);
		SteamLobbyHandler.SetLobbyRegion();
		SteamLobbyHandler.SetHostLobbyType();
		SteamLobbyHandler.SetLobbyLanguage((LanguageMatchmakingSetting.MatchmakingLanguage)GameHandler.Instance.SettingsHandler.GetSetting<PreferredLanguageMatchmakingSetting>().Value);
		RetrievableSingleton<ConnectionStateHandler>.Instance.StateMachine.SwitchState<InRoomState>();
		LoadGameScene();
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		base.OnCreateRoomFailed(returnCode, message);
		Debug.LogError($"Failed to create room (error code {returnCode}): {message}");
		Modal.ShowError(LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_CreateRoom), message);
		SteamLobbyHandler.LeaveLobby();
		RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect();
	}

	private void OnLobbyHosted(ulong obj, bool privateMatch)
	{
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_CreateRoom);
		if (obj == 0L)
		{
			Debug.LogError("Something went wrong hosting the Lobby, aborting Photon Create Room");
			Modal.ShowError(localizedString, "");
			RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect();
			return;
		}
		Debug.Log($"Lobby Hosted: {obj} Hosting PhotonRoom Now");
		LanguageMatchmakingSetting.MatchmakingLanguage value = (LanguageMatchmakingSetting.MatchmakingLanguage)GameHandler.Instance.SettingsHandler.GetSetting<PreferredLanguageMatchmakingSetting>().Value;
		PlatformUtility.PlatformFamily platformFamily = (CrossPlatform ? ((PlatformUtility.PlatformFamily)(-1)) : PlatformUtility.CurrentPlatform);
		Debug.Log(string.Format("Targeted Platform: {0}", (platformFamily < PlatformUtility.PlatformFamily.None) ? "All" : ((object)platformFamily)));
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = false;
		roomOptions.MaxPlayers = 4;
		roomOptions.Plugins = new string[1] { "ContentWarningPlugin" };
		roomOptions.CustomRoomPropertiesForLobby = new string[4] { "P", "L", "PL", "M" };
		roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
		{
			{
				"C",
				obj.ToString()
			},
			{
				"P",
				privateMatch ? 1 : 0
			},
			{ "L", value },
			{
				"PL",
				(int)platformFamily
			},
			{
				"M",
				GameHandler.GetPluginHash()
			}
		};
		m_Hosting = false;
		if (!PhotonNetwork.CreateRoom(Utilities.CreateRandomName(PhotonNetwork.CloudRegion, PluginHandler.AllPlugins.Count > 0), roomOptions, TypedLobby.Default))
		{
			Debug.LogError("Error While Hosting Photon Room...");
			Modal.ShowError(localizedString, "");
			RetrievableSingleton<ConnectionStateHandler>.Instance.Disconnect();
			return;
		}
		Debug.Log("Created Photon Room... ");
		if (!SaveSystem.HaveCurrentSave && SaveSystem.USING_SAVE)
		{
			SaveSystem.MakeNewSave();
		}
	}

	public override void OnConnectedToMaster()
	{
		string cloudRegion = PhotonNetwork.CloudRegion;
		Debug.Log("Connected to Photon Master Server: " + cloudRegion);
		if (PhotonAuthManager.PhotonAuth is SteamPhotonAuthManager steamPhotonAuthManager)
		{
			SteamPhotonAuthManager.CancelAuthTicket(steamPhotonAuthManager.AuthTicket);
		}
		CheckSteamCMD();
	}

	private void CheckSteamCMD()
	{
		if (!m_CheckedSteamCMD)
		{
			m_CheckedSteamCMD = true;
			if (SteamApps.GetLaunchCommandLine(out var pszCommandLine, 1024) > 0)
			{
				Debug.Log("Steam CMD Line " + pszCommandLine);
			}
		}
	}

	public override void OnCustomAuthenticationFailed(string errorMessage)
	{
		Debug.LogError("Failed to authenticate with Photon: " + errorMessage);
		string localizedString = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Error_Auth_Failed);
		string localizedString2 = LocalizationKeys.GetLocalizedString(LocalizationKeys.Keys.Ok);
		Modal.Show(localizedString, errorMessage, new ModalOption[1]
		{
			new ModalOption(localizedString2)
		}, delegate
		{
			Application.Quit();
		});
		if (PhotonAuthManager.PhotonAuth is SteamPhotonAuthManager steamPhotonAuthManager)
		{
			SteamPhotonAuthManager.CancelAuthTicket(steamPhotonAuthManager.AuthTicket);
		}
	}

	private void UpdateCachedRoomList(List<RoomInfo> roomList)
	{
		foreach (RoomInfo room in roomList)
		{
			Debug.Log($"Room Info: {room}");
			if (room.RemovedFromList)
			{
				m_cachedRoomList.Remove(room.Name);
			}
			else
			{
				m_cachedRoomList[room.Name] = room;
			}
		}
	}

	public override void OnJoinedLobby()
	{
		m_cachedRoomList.Clear();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		UpdateCachedRoomList(roomList);
	}

	public override void OnLeftLobby()
	{
		m_cachedRoomList.Clear();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		m_cachedRoomList.Clear();
	}

	public override void OnJoinedRoom()
	{
		m_AwaitingJoinEvent = false;
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		m_AwaitingJoinEvent = false;
	}
}
