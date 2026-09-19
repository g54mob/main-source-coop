using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager
{
	[CompilerGenerated]
	private sealed class _003CAssignRolesWhenReady_003Ed__45 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MyNetworkManager _003C_003E4__this;

		private float _003Ctimeout_003E5__2;

		private float _003Ct_003E5__3;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CAssignRolesWhenReady_003Ed__45(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			MyNetworkManager myNetworkManager = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForSeconds(0.3f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Ctimeout_003E5__2 = 5f;
				_003Ct_003E5__3 = 0f;
				goto IL_008e;
			case 2:
				_003C_003E1__state = -1;
				goto IL_008e;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_008e:
				if (GameManager.Instance == null && _003Ct_003E5__3 < _003Ctimeout_003E5__2)
				{
					_003Ct_003E5__3 += Time.deltaTime;
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				if (GameManager.Instance == null)
				{
					UnityEngine.Debug.LogError("[NM] Game scene'de GameManager bulunamadı!");
					return false;
				}
				_003Ct_003E5__3 = 0f;
				break;
			}
			if (_003Ct_003E5__3 < myNetworkManager.allPlayersReadyTimeout && !myNetworkManager.AllConnectionsHaveIdentity())
			{
				_003Ct_003E5__3 += Time.deltaTime;
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
			}
			myNetworkManager.KickUnreadyConnections();
			foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
			{
				if (value != null && value.identity != null)
				{
					GameManager.Instance.RegisterPlayer(value);
				}
			}
			GameManager.Instance.PrepareNewRound();
			GameManager.Instance.AssignRolesWithVolunteers(myNetworkManager.GetHunterVolunteers());
			myNetworkManager._initialRoleAssignmentDone = true;
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public static bool isMulitplayer;

	public List<MyClient> allClients = new List<MyClient>();

	[Header("Sahneler")]
	[Scene]
	public string lobbyScene;

	[Tooltip("Farm haritası (Game_Farm.unity)")]
	[Scene]
	public string gameSceneFarm;

	[Tooltip("Forest haritası (Game_Forest.unity)")]
	[Scene]
	public string gameSceneForest;

	[Tooltip("Lobiyi kurarken (CreateLobbyScreen) seçilen harita — sahneler arası korunur (bu obje DontDestroyOnLoad)")]
	public MapType SelectedMap;

	[Header("Lobby Ayarları (sahneler arası korunur)")]
	[Tooltip("Oyunda kaç avcı olsun (gönüllüler önce, kalanlar rastgele — en az 1 hayvan kalacak şekilde sınırlanır)")]
	public int HunterCount = 1;

	[Tooltip("Lobiden Game sahnesine geçişte, sunucu rol ataması yapmadan önce TÜM oyuncuların sahnesinin yüklenmesini (identity almasını) en fazla bu kadar (sn) bekler. Süre dolunca hâlâ yüklenmemiş bağlantılar ATILIR (Kick) ve round geri kalanla başlar — bkz. AssignRolesWhenReady/AllConnectionsHaveIdentity/KickUnreadyConnections. Yavaş bir bilgisayarın sahneyi gerçekten yükleyebilmesi için cömert tutulmalı (varsayılan 90sn = 1:30dk).")]
	public float allPlayersReadyTimeout = 90f;

	[Tooltip("Toplam oyun süresi (saniye) — default 10dk")]
	public float GameDurationSeconds = 600f;

	[Tooltip("Avcı başına mermi hakkı")]
	public int HunterAmmo = 4;

	[Tooltip("Sessizlik sonrası buzzing'in tetiklenme süresi (sn) — 0 = buzzing kapalı")]
	public int BuzzingInterval = 30;

	[Tooltip("Avcı yanlış (bot) hedef vurunca oyun süresinden düşülecek ceza (sn)")]
	public float HunterPenaltySeconds = 30f;

	[Tooltip("Rol/kayıt panelinde (RoleAssignmentUI) izin verilen maksimum ses kaydı süresi (sn)")]
	public float MaxRecordSeconds = 3f;

	public AnimalSoundLevel AnimalSound = AnimalSoundLevel.Medium;

	public NpcPopulationLevel NpcPopulation = NpcPopulationLevel.Medium;

	private readonly Dictionary<int, bool> _hunterVolunteers = new Dictionary<int, bool>();

	private readonly Dictionary<int, PlayerInfoData> _playerInfoCache = new Dictionary<int, PlayerInfoData>();

	private const int PlayerLayer = 8;

	private bool _initialRoleAssignmentDone;

	private bool _wasConnectedClient;

	public string ActiveGameScene
	{
		get
		{
			if (SelectedMap != MapType.Forest)
			{
				return gameSceneFarm;
			}
			return gameSceneForest;
		}
	}

	public bool IsInGameScene { get; private set; }

	public static MyNetworkManager Singleton => NetworkManager.singleton as MyNetworkManager;

	public override void Awake()
	{
		base.Awake();
		Physics.IgnoreLayerCollision(8, 8, ignore: true);
	}

	public override void OnServerAddPlayer(NetworkConnectionToClient conn)
	{
		base.OnServerAddPlayer(conn);
		StartCoroutine(OnPlayerAddedDelayed(conn));
	}

	public override void OnClientDisconnect()
	{
		base.OnClientDisconnect();
		UnityEngine.Debug.Log("[NM] Client disconnect — menüye dönülüyor.");
		bool flag = SteamLobby.ConsumeVoluntaryLeaveFlag();
		if (!NetworkServer.active && !flag && _wasConnectedClient)
		{
			SteamLobby.NotifyHostLeft();
		}
		_wasConnectedClient = false;
		SteamLobby.instance?.EndJoinAttempt();
		LoadingScreen.Instance?.Hide();
		if (SteamLobby.instance != null && SteamLobby.LobbyID.m_SteamID != 0L)
		{
			SteamMatchmaking.LeaveLobby(SteamLobby.LobbyID);
			SteamLobby.LobbyID = new CSteamID(0uL);
		}
		SteamFriends.ClearRichPresence();
		ReturnToMenuScene();
	}

	public override void OnStopHost()
	{
		base.OnStopHost();
		UnityEngine.Debug.Log("[NM] Host durdu — lobby kapandı.");
		SteamFriends.ClearRichPresence();
		LoadingScreen.Instance?.Hide();
		if (SteamLobby.LobbyID.m_SteamID != 0L)
		{
			SteamMatchmaking.LeaveLobby(SteamLobby.LobbyID);
			SteamLobby.LobbyID = new CSteamID(0uL);
		}
	}

	private void ReturnToMenuScene()
	{
		string.IsNullOrEmpty(offlineScene);
	}

	private IEnumerator OnPlayerAddedDelayed(NetworkConnectionToClient conn)
	{
		float timeout = 5f;
		float t = 0f;
		while ((conn == null || conn.identity == null) && t < timeout)
		{
			t += Time.deltaTime;
			yield return null;
		}
		if (conn == null)
		{
			UnityEngine.Debug.LogWarning("[NM] conn null.");
			yield break;
		}
		if (conn.identity == null)
		{
			UnityEngine.Debug.LogError("[NM] identity timeout.");
			yield break;
		}
		MyClient client = conn.identity.GetComponent<MyClient>();
		t = 0f;
		while (client == null && t < timeout)
		{
			t += Time.deltaTime;
			yield return null;
			if (conn.identity != null)
			{
				client = conn.identity.GetComponent<MyClient>();
			}
		}
		if (client == null)
		{
			UnityEngine.Debug.LogError("[NM] MyClient bulunamadı.");
			yield break;
		}
		if (_playerInfoCache.TryGetValue(conn.connectionId, out var value) && !string.IsNullOrEmpty(value.username))
		{
			client.NetworkplayerInfo = value;
		}
		else
		{
			CSteamID steamIDFriend = (SteamManager.Initialized ? SteamUser.GetSteamID() : CSteamID.Nil);
			string username = (SteamManager.Initialized ? SteamFriends.GetFriendPersonaName(steamIDFriend) : "Host");
			PlayerInfoData value2 = (client.NetworkplayerInfo = new PlayerInfoData(username, steamIDFriend.m_SteamID));
			_playerInfoCache[conn.connectionId] = value2;
		}
		if (!allClients.Contains(client))
		{
			allClients.Add(client);
		}
		if (MainMenu.instance != null)
		{
			MainMenu.instance.AddPlayerToParty(new CSteamID(client.playerInfo.steamId));
		}
		if (conn.identity.GetComponent<PlayerRoleData>() == null)
		{
			conn.identity.gameObject.AddComponent<PlayerRoleData>();
		}
		if (!_hunterVolunteers.ContainsKey(conn.connectionId))
		{
			_hunterVolunteers[conn.connectionId] = false;
		}
		if (IsInGameScene && GameManager.Instance != null)
		{
			GameManager.Instance.RegisterPlayer(conn);
			if (_initialRoleAssignmentDone)
			{
				GameManager.Instance.AssignRoleToLateJoiner(conn);
			}
			if (VoiceNetwork.Instance != null)
			{
				VoiceNetwork.Instance.SendAllRecordsTo(conn);
			}
		}
	}

	public void CachePlayerInfo(int connectionId, PlayerInfoData info)
	{
		_playerInfoCache[connectionId] = info;
	}

	public override void OnServerDisconnect(NetworkConnectionToClient conn)
	{
		if (conn.identity != null)
		{
			MyClient component = conn.identity.GetComponent<MyClient>();
			if (component != null)
			{
				CSteamID steamID = new CSteamID(component.playerInfo.steamId);
				if (MainMenu.instance != null)
				{
					MainMenu.instance.RemovePlayerFromParty(steamID);
				}
				allClients.Remove(component);
			}
			if (GameManager.Instance != null)
			{
				GameManager.Instance.UnregisterPlayer(conn);
			}
		}
		_hunterVolunteers.Remove(conn.connectionId);
		base.OnServerDisconnect(conn);
	}

	[Server]
	public void SetHunterVolunteer(int connectionId, bool wantsHunter)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void MyNetworkManager::SetHunterVolunteer(System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		_hunterVolunteers[connectionId] = wantsHunter;
		UnityEngine.Debug.Log($"[NM] conn {connectionId} gönüllü avcı: {wantsHunter}");
	}

	[Server]
	public List<int> GetHunterVolunteers()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.Generic.List`1<System.Int32> MyNetworkManager::GetHunterVolunteers()' called when server was not active");
			return null;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, bool> hunterVolunteer in _hunterVolunteers)
		{
			if (hunterVolunteer.Value)
			{
				list.Add(hunterVolunteer.Key);
			}
		}
		return list;
	}

	[Server]
	public void SetHunterCount(int value)
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void MyNetworkManager::SetHunterCount(System.Int32)' called when server was not active");
		}
		else
		{
			HunterCount = Mathf.Max(0, value);
		}
	}

	public void SetSelectedMap(MapType map)
	{
		SelectedMap = map;
	}

	[Server]
	public void StartGameScene()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void MyNetworkManager::StartGameScene()' called when server was not active");
			return;
		}
		string activeGameScene = ActiveGameScene;
		if (string.IsNullOrEmpty(activeGameScene))
		{
			UnityEngine.Debug.LogError($"[NM] {SelectedMap} için sahne atanmamış! (gameSceneFarm/gameSceneForest)");
			return;
		}
		SteamLobby.instance?.MarkInGame();
		UnityEngine.Debug.Log($"[NM] Game scene'e geçiliyor... ({SelectedMap})");
		ServerChangeScene(activeGameScene);
	}

	[Server]
	public void ReturnToLobby()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void MyNetworkManager::ReturnToLobby()' called when server was not active");
		}
		else if (!string.IsNullOrEmpty(lobbyScene))
		{
			_hunterVolunteers.Clear();
			SteamLobby.instance?.MarkInLobby();
			ServerChangeScene(lobbyScene);
		}
	}

	public override void OnServerSceneChanged(string sceneName)
	{
		base.OnServerSceneChanged(sceneName);
		IsInGameScene = sceneName == gameSceneFarm || sceneName == gameSceneForest;
		UnityEngine.Debug.Log($"[NM] Server sahne değişti: {sceneName}, IsInGameScene={IsInGameScene}");
		StartCoroutine(ReapplyPlayerInfoAfterScene());
		if (IsInGameScene)
		{
			_initialRoleAssignmentDone = false;
			StartCoroutine(AssignRolesWhenReady());
		}
	}

	public override void OnClientSceneChanged()
	{
		base.OnClientSceneChanged();
		string path = SceneManager.GetActiveScene().path;
		IsInGameScene = path == gameSceneFarm || path == gameSceneForest;
		UnityEngine.Debug.Log($"[NM] Client sahne değişti, IsInGameScene={IsInGameScene}");
		if (!IsInGameScene)
		{
			LoadingScreen.Instance?.Hide();
		}
	}

	public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
	{
		base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
		LoadingScreen.Instance?.Show(allPlayersReadyTimeout + 30f);
	}

	private IEnumerator ReapplyPlayerInfoAfterScene()
	{
		yield return new WaitForSeconds(0.5f);
		foreach (NetworkConnectionToClient value2 in NetworkServer.connections.Values)
		{
			if (value2 != null && !(value2.identity == null))
			{
				MyClient component = value2.identity.GetComponent<MyClient>();
				if (!(component == null) && _playerInfoCache.TryGetValue(value2.connectionId, out var value) && !string.IsNullOrEmpty(value.username))
				{
					component.NetworkplayerInfo = value;
				}
			}
		}
	}

	[IteratorStateMachine(typeof(_003CAssignRolesWhenReady_003Ed__45))]
	[Server]
	private IEnumerator AssignRolesWhenReady()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Collections.IEnumerator MyNetworkManager::AssignRolesWhenReady()' called when server was not active");
			return null;
		}
		return new _003CAssignRolesWhenReady_003Ed__45(0)
		{
			_003C_003E4__this = this
		};
	}

	[Server]
	private void KickUnreadyConnections()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Debug.LogWarning("[Server] function 'System.Void MyNetworkManager::KickUnreadyConnections()' called when server was not active");
			return;
		}
		List<NetworkConnectionToClient> list = new List<NetworkConnectionToClient>();
		foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
		{
			if (value != null && value.identity == null)
			{
				list.Add(value);
			}
		}
		foreach (NetworkConnectionToClient item in list)
		{
			UnityEngine.Debug.LogWarning($"[NM] conn {item.connectionId} allPlayersReadyTimeout ({allPlayersReadyTimeout}sn) içinde sahnesini yükleyemedi — atılıyor.");
			item.Disconnect();
		}
	}

	private bool AllConnectionsHaveIdentity()
	{
		foreach (NetworkConnectionToClient value in NetworkServer.connections.Values)
		{
			if (value != null && value.identity == null)
			{
				return false;
			}
		}
		return true;
	}

	public override void OnStartClient()
	{
		if (isMulitplayer)
		{
			if (MainMenu.instance != null)
			{
				MainMenu.instance.SetMenuState(MenuState.InParty);
			}
			SteamLobby.instance?.EndJoinAttempt();
		}
		if (!NetworkServer.active)
		{
			_wasConnectedClient = true;
		}
		base.OnStartClient();
	}

	public override void OnStopClient()
	{
		if (isMulitplayer && MainMenu.instance != null)
		{
			MainMenu.instance.SetMenuState(MenuState.Home);
		}
		LoadingScreen.Instance?.Hide();
		base.OnStopClient();
	}

	public void SetMultiplayer(bool value)
	{
		isMulitplayer = value;
		NetworkServer.dontListen = !value;
	}
}
