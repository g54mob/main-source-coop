using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Mirror;
using Steamworks;
using UnityEngine;

public class SteamLobby : MonoBehaviour
{
	private struct ListRequest
	{
		public Action<List<Lobby>> callback;

		public string filterJoinCode;
	}

	private const string HOST_ADDRESS_KEY = "HostAddress";

	public static SteamLobby instance;

	public static CSteamID LobbyID;

	private static bool _voluntaryLeaveInProgress;

	public List<Lobby> allLobbies = new List<Lobby>();

	public Action OnMatchmakingStarted;

	public Action OnMatchmakingEnded;

	private Coroutine _findRoutine;

	[Header("Katılma Güvenliği")]
	[Tooltip("'Joining Party' popup'ı bu süre içinde bağlantı tamamlanmazsa (Steam seviyesinde başarılı ama Mirror hiç bağlanamamış olabilir — host ulaşılamaz, port kapalı vb.) zorla kapatılır ve yarım kalan katılma denemesi temizlenir. Aksi halde oyuncu ekranda süresiz takılı kalabiliyordu.")]
	public float joinTimeoutSeconds = 12f;

	private Coroutine _joinTimeoutRoutine;

	[Header("Quick Match")]
	[Tooltip("Quick Match bu süre boyunca uygun (public, dolu olmayan) bir lobi arar; bulamazsa kendi lobisini kurar.")]
	public float quickMatchTimeoutSeconds = 120f;

	private bool _isJoining;

	private bool _awaitingQuickMatchJoinResult;

	protected Callback<LobbyCreated_t> lobbyCreated;

	protected Callback<GameLobbyJoinRequested_t> joinRequested;

	protected Callback<LobbyEnter_t> lobbyEntered;

	protected Callback<LobbyMatchList_t> lobbyMatchList;

	protected Callback<LobbyChatUpdate_t> lobbyChatUpdate;

	protected Callback<LobbyDataUpdate_t> lobbyDataUpdate;

	private const string JOIN_CODE_KEY = "joincode";

	private const string LOBBY_TYPE_KEY = "lobbytype";

	private const string REGION_KEY = "region";

	private const string STATE_KEY = "state";

	private const string GAME_VERSION_KEY = "gameversion";

	[Header("Liste Ekranı — Gösterim")]
	[Tooltip("Kullanıcı isteği: Steam'in döndürdüğü tüm lobiler (varsayılan üst sınırı 50) yerine, ekranda HER YENİLEMEDE rastgele seçilen en fazla bu kadar lobi gösterilir (katılınabilir olanlar yine en üstte) — bkz. BuildRandomizedDisplayList. Quick Match/JoinByCode gibi diğer tüketiciler HÂLÂ tam listeyi (allLobbies) kullanır, bu sadece görüntüleme sınırı.")]
	public int displayedLobbyCount = 15;

	private readonly Queue<ListRequest> _listRequestQueue = new Queue<ListRequest>();

	private bool _listRequestInFlight;

	private bool _plainRefreshQueued;

	[Header("Liste İsteği Güvenliği")]
	[Tooltip("BUG (bulundu — kullanıcı raporu: 'bazı lobiler listede görünmüyor, refresh düzeltmiyor, sadece oyunu kapatıp açmak düzeltiyor'): SteamMatchmaking.RequestLobbyList() bazen LobbyMatchList_t callback'ini HİÇ tetiklemiyordu — _listRequestInFlight sonsuza kadar true kalıp kuyruğu kilitliyordu, bu yüzden HER SONRAKİ ReloadLobbyList() (Refresh butonu dahil) sessizce kuyruğa girip asla gerçek bir Steam sorgusuna dönüşmüyordu (liste kalıcı olarak bayat kalıyordu). Bu süre içinde yanıt gelmezse kuyruk zorla sıfırlanır ve bir sonraki istek işlenir.")]
	public float listRequestTimeoutSeconds = 8f;

	private Coroutine _listRequestTimeoutRoutine;

	private List<Lobby> _lastDisplayedLobbies = new List<Lobby>();

	private string _pendingJoinCode;

	private bool _pendingIsPublic = true;

	public string CurrentJoinCode
	{
		get
		{
			if (LobbyID.m_SteamID == 0L)
			{
				return "";
			}
			return SteamMatchmaking.GetLobbyData(LobbyID, "joincode");
		}
	}

	public bool IsMatchmaking { get; private set; }

	public static event Action OnHostLeftLobby;

	public static event Action OnJoinFailed;

	public static void NotifyHostLeft()
	{
		SteamLobby.OnHostLeftLobby?.Invoke();
	}

	public static void NotifyJoinFailed()
	{
		SteamLobby.OnJoinFailed?.Invoke();
	}

	public static bool ConsumeVoluntaryLeaveFlag()
	{
		bool voluntaryLeaveInProgress = _voluntaryLeaveInProgress;
		_voluntaryLeaveInProgress = false;
		return voluntaryLeaveInProgress;
	}

	private void Awake()
	{
		if (!(instance != null) || !(instance != this))
		{
			instance = this;
			lobbyMatchList = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);
		}
	}

	private void Start()
	{
		if (!(instance != this) && SteamManager.Initialized)
		{
			lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
			joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
			lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
			lobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
			lobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
			ReloadLobbyList();
		}
	}

	private void OnDestroy()
	{
		lobbyMatchList?.Dispose();
		lobbyCreated?.Dispose();
		joinRequested?.Dispose();
		lobbyEntered?.Dispose();
		lobbyChatUpdate?.Dispose();
		lobbyDataUpdate?.Dispose();
		if (instance == this)
		{
			instance = null;
		}
	}

	public void ReloadLobbyList()
	{
		if (!_plainRefreshQueued)
		{
			_plainRefreshQueued = true;
			EnqueueListRequest(null);
		}
	}

	private void EnqueueListRequest(Action<List<Lobby>> callback, string filterJoinCode = null)
	{
		_listRequestQueue.Enqueue(new ListRequest
		{
			callback = callback,
			filterJoinCode = filterJoinCode
		});
		TryStartNextListRequest();
	}

	private void TryStartNextListRequest()
	{
		if (!_listRequestInFlight && _listRequestQueue.Count != 0)
		{
			_listRequestInFlight = true;
			SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
			string filterJoinCode = _listRequestQueue.Peek().filterJoinCode;
			if (!string.IsNullOrEmpty(filterJoinCode))
			{
				SteamMatchmaking.AddRequestLobbyListStringFilter("joincode", filterJoinCode, ELobbyComparison.k_ELobbyComparisonEqual);
			}
			SteamMatchmaking.RequestLobbyList();
			if (_listRequestTimeoutRoutine != null)
			{
				StopCoroutine(_listRequestTimeoutRoutine);
			}
			_listRequestTimeoutRoutine = StartCoroutine(ListRequestTimeoutRoutine());
		}
	}

	private IEnumerator ListRequestTimeoutRoutine()
	{
		yield return new WaitForSeconds(listRequestTimeoutSeconds);
		Debug.LogWarning("[SteamLobby] RequestLobbyList zaman aşımına uğradı (callback hiç gelmedi) — istek kuyruğu zorla sıfırlanıyor.");
		_listRequestTimeoutRoutine = null;
		_listRequestInFlight = false;
		if (_listRequestQueue.Count > 0)
		{
			ListRequest listRequest = _listRequestQueue.Dequeue();
			if (listRequest.callback == null)
			{
				_plainRefreshQueued = false;
			}
			List<Lobby> obj = (string.IsNullOrEmpty(listRequest.filterJoinCode) ? allLobbies : new List<Lobby>());
			listRequest.callback?.Invoke(obj);
		}
		TryStartNextListRequest();
	}

	private void OnLobbyMatchList(LobbyMatchList_t param)
	{
		if (_listRequestTimeoutRoutine != null)
		{
			StopCoroutine(_listRequestTimeoutRoutine);
			_listRequestTimeoutRoutine = null;
		}
		if (_listRequestQueue.Count > 0 && !string.IsNullOrEmpty(_listRequestQueue.Peek().filterJoinCode))
		{
			List<Lobby> list = new List<Lobby>();
			for (int i = 0; i < param.m_nLobbiesMatching; i++)
			{
				CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(i);
				string lobbyData = SteamMatchmaking.GetLobbyData(lobbyByIndex, "name");
				list.Add(new Lobby(lobbyByIndex, lobbyData)
				{
					joinCode = SteamMatchmaking.GetLobbyData(lobbyByIndex, "joincode"),
					lobbyType = SteamMatchmaking.GetLobbyData(lobbyByIndex, "lobbytype"),
					region = SteamMatchmaking.GetLobbyData(lobbyByIndex, "region"),
					state = SteamMatchmaking.GetLobbyData(lobbyByIndex, "state"),
					memberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyByIndex),
					maxMembers = SteamMatchmaking.GetLobbyMemberLimit(lobbyByIndex)
				});
			}
			_listRequestInFlight = false;
			if (_listRequestQueue.Count > 0)
			{
				_listRequestQueue.Dequeue().callback?.Invoke(list);
			}
			TryStartNextListRequest();
			return;
		}
		allLobbies.Clear();
		for (int j = 0; j < param.m_nLobbiesMatching; j++)
		{
			CSteamID lobbyByIndex2 = SteamMatchmaking.GetLobbyByIndex(j);
			string lobbyData2 = SteamMatchmaking.GetLobbyData(lobbyByIndex2, "lobbytype");
			if (!(lobbyData2 != "public") || !(lobbyData2 != "inviteonly"))
			{
				string lobbyData3 = SteamMatchmaking.GetLobbyData(lobbyByIndex2, "name");
				Lobby item = new Lobby(lobbyByIndex2, lobbyData3)
				{
					joinCode = SteamMatchmaking.GetLobbyData(lobbyByIndex2, "joincode"),
					lobbyType = lobbyData2,
					region = SteamMatchmaking.GetLobbyData(lobbyByIndex2, "region"),
					state = SteamMatchmaking.GetLobbyData(lobbyByIndex2, "state"),
					memberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyByIndex2),
					maxMembers = SteamMatchmaking.GetLobbyMemberLimit(lobbyByIndex2)
				};
				allLobbies.Add(item);
				if (LobbyListUI.Instance != null && LobbyListUI.Instance.IsScreenVisible)
				{
					SteamMatchmaking.RequestLobbyData(lobbyByIndex2);
				}
			}
		}
		allLobbies.Sort((Lobby a, Lobby b) => (a.IsJoinable != b.IsJoinable) ? ((!a.IsJoinable) ? 1 : (-1)) : b.memberCount.CompareTo(a.memberCount));
		LobbyListUI.Instance?.PopulateList(BuildRandomizedDisplayList());
		_listRequestInFlight = false;
		if (_listRequestQueue.Count > 0)
		{
			ListRequest listRequest = _listRequestQueue.Dequeue();
			if (listRequest.callback == null)
			{
				_plainRefreshQueued = false;
			}
			listRequest.callback?.Invoke(allLobbies);
		}
		TryStartNextListRequest();
	}

	private List<Lobby> BuildRandomizedDisplayList()
	{
		List<Lobby> list = new List<Lobby>(allLobbies);
		for (int num = list.Count - 1; num > 0; num--)
		{
			int num2 = UnityEngine.Random.Range(0, num + 1);
			int index = num;
			List<Lobby> list2 = list;
			int index2 = num2;
			Lobby lobby = list[num2];
			Lobby lobby2 = list[num];
			Lobby lobby3 = (list[index] = lobby);
			lobby3 = (list2[index2] = lobby2);
		}
		list.Sort((Lobby a, Lobby b) => (a.IsJoinable != b.IsJoinable) ? ((!a.IsJoinable) ? 1 : (-1)) : 0);
		if (list.Count > displayedLobbyCount)
		{
			list.RemoveRange(displayedLobbyCount, list.Count - displayedLobbyCount);
		}
		_lastDisplayedLobbies = list;
		return list;
	}

	private void OnLobbyDataUpdate(LobbyDataUpdate_t callback)
	{
		if (callback.m_bSuccess == 0 || callback.m_ulSteamIDMember != callback.m_ulSteamIDLobby)
		{
			return;
		}
		CSteamID lobbyID = new CSteamID(callback.m_ulSteamIDLobby);
		int num = allLobbies.FindIndex((Lobby l) => l.lobbyID == lobbyID);
		if (num < 0)
		{
			return;
		}
		string lobbyData = SteamMatchmaking.GetLobbyData(lobbyID, "lobbytype");
		if (!(lobbyData != "public") || !(lobbyData != "inviteonly"))
		{
			Lobby lobby = allLobbies[num];
			lobby.lobbyType = lobbyData;
			lobby.state = SteamMatchmaking.GetLobbyData(lobbyID, "state");
			lobby.memberCount = SteamMatchmaking.GetNumLobbyMembers(lobbyID);
			lobby.maxMembers = SteamMatchmaking.GetLobbyMemberLimit(lobbyID);
			if (LobbyListUI.Instance != null && LobbyListUI.Instance.IsScreenVisible)
			{
				LobbyListUI.Instance.PopulateList(_lastDisplayedLobbies);
			}
		}
	}

	public void CreateLobby()
	{
		CreateLobby(isPublic: true);
	}

	public void CreateLobby(bool isPublic)
	{
		LoadingScreen.Instance?.Show();
		_pendingIsPublic = isPublic;
		_pendingJoinCode = GenerateUniqueJoinCode();
		SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, ((MyNetworkManager)NetworkManager.singleton).maxConnections);
	}

	private string GenerateUniqueJoinCode()
	{
		HashSet<string> hashSet = new HashSet<string>();
		foreach (Lobby allLobby in allLobbies)
		{
			if (!string.IsNullOrEmpty(allLobby.joinCode))
			{
				hashSet.Add(allLobby.joinCode);
			}
		}
		int num = 0;
		string text;
		do
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 6; i++)
			{
				stringBuilder.Append("ABCDEFGHJKLMNPQRSTUVWXYZ23456789"[UnityEngine.Random.Range(0, "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".Length)]);
			}
			text = stringBuilder.ToString();
			num++;
		}
		while (hashSet.Contains(text) && num < 50);
		return text;
	}

	public bool JoinLobby(CSteamID lobby)
	{
		if (_isJoining)
		{
			Debug.LogWarning("[SteamLobby] Zaten bir katılma denemesi sürüyor — yeni istek görmezden geliniyor.");
			return false;
		}
		string lobbyData = SteamMatchmaking.GetLobbyData(lobby, "lobbytype");
		if ((lobbyData ?? "").ToLower() != "public")
		{
			Debug.LogWarning("[SteamLobby] Invite-only (ya da tipi henüz belirsiz: '" + lobbyData + "') lobiye doğrudan katılma engellendi — sadece Steam daveti veya lobi kodu ile katılınabilir.");
			return false;
		}
		string lobbyData2 = SteamMatchmaking.GetLobbyData(lobby, "state");
		if ((string.IsNullOrEmpty(lobbyData2) ? "lobby" : lobbyData2).ToLower() == "ingame")
		{
			Debug.LogWarning("[SteamLobby] Lobi artık oyunda — katılma iptal edildi, liste tazeleniyor.");
			ReloadLobbyList();
			return false;
		}
		int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(lobby);
		int lobbyMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(lobby);
		if (lobbyMemberLimit > 0 && numLobbyMembers >= lobbyMemberLimit)
		{
			Debug.LogWarning("[SteamLobby] Lobi artık dolu — katılma iptal edildi, liste tazeleniyor.");
			ReloadLobbyList();
			return false;
		}
		BeginJoinAttempt();
		_isJoining = true;
		SteamMatchmaking.JoinLobby(lobby);
		return true;
	}

	private void BeginJoinAttempt()
	{
		LoadingScreen.Instance?.Show(30f);
		if (_joinTimeoutRoutine != null)
		{
			StopCoroutine(_joinTimeoutRoutine);
		}
		_joinTimeoutRoutine = StartCoroutine(JoinTimeoutRoutine());
	}

	public void EndJoinAttempt()
	{
		_isJoining = false;
		if (_joinTimeoutRoutine != null)
		{
			StopCoroutine(_joinTimeoutRoutine);
			_joinTimeoutRoutine = null;
		}
	}

	private IEnumerator JoinTimeoutRoutine()
	{
		yield return new WaitForSeconds(joinTimeoutSeconds);
		Debug.LogWarning("[SteamLobby] Katılma zaman aşımına uğradı (bağlantı hiç kurulamamış olabilir) — temizleniyor.");
		_joinTimeoutRoutine = null;
		_isJoining = false;
		LoadingScreen.Instance?.Hide();
		if (NetworkClient.active && !NetworkServer.active)
		{
			((MyNetworkManager)NetworkManager.singleton).StopClient();
		}
		ReloadLobbyList();
		if (_awaitingQuickMatchJoinResult)
		{
			_awaitingQuickMatchJoinResult = false;
		}
		else
		{
			NotifyJoinFailed();
		}
	}

	private void OnLobbyCreated(LobbyCreated_t callback)
	{
		if (callback.m_eResult != EResult.k_EResultOK)
		{
			LoadingScreen.Instance?.Hide();
			return;
		}
		EndMatchmaking();
		string friendPersonaName = SteamFriends.GetFriendPersonaName(SteamUser.GetSteamID());
		LobbyID = new CSteamID(callback.m_ulSteamIDLobby);
		((MyNetworkManager)NetworkManager.singleton).StartHost();
		SteamMatchmaking.SetLobbyData(LobbyID, "HostAddress", SteamUser.GetSteamID().ToString());
		SteamMatchmaking.SetLobbyData(LobbyID, "name", friendPersonaName);
		SteamMatchmaking.SetLobbyData(LobbyID, "displayable", "true");
		SteamMatchmaking.SetLobbyData(LobbyID, "state", "lobby");
		SteamMatchmaking.SetLobbyData(LobbyID, "joincode", _pendingJoinCode ?? "");
		SteamMatchmaking.SetLobbyData(LobbyID, "lobbytype", _pendingIsPublic ? "public" : "inviteonly");
		SteamMatchmaking.SetLobbyData(LobbyID, "gameversion", Application.version);
		SetLobbyLocation();
		SetRegionData();
		Debug.Log("[SteamLobby] Lobby kuruldu. Kod: " + _pendingJoinCode);
	}

	public void SetLobbyState(string state)
	{
		if (LobbyID.m_SteamID != 0L && NetworkServer.active)
		{
			SteamMatchmaking.SetLobbyData(LobbyID, "state", state);
			Debug.Log("[SteamLobby] Lobby durumu: " + state);
		}
	}

	public void MarkInGame()
	{
		SetLobbyState("ingame");
	}

	public void MarkInLobby()
	{
		SetLobbyState("lobby");
	}

	private void SetRegionData()
	{
		string twoLetterISORegionName = RegionInfo.CurrentRegion.TwoLetterISORegionName;
		SteamMatchmaking.SetLobbyData(LobbyID, "region", twoLetterISORegionName);
	}

	public void JoinByCode(string code, Action<bool> result = null)
	{
		if (string.IsNullOrWhiteSpace(code))
		{
			result?.Invoke(obj: false);
			return;
		}
		code = code.Trim().ToUpper();
		EnqueueListRequest(delegate(List<Lobby> lobbies)
		{
			Lobby lobby = ((lobbies.Count > 0) ? lobbies[0] : null);
			if (lobby != null)
			{
				if ((lobby.state ?? "lobby").ToLower() == "ingame")
				{
					Debug.Log("[SteamLobby] Lobby oyunda — kodla katılınamaz.");
					result?.Invoke(obj: false);
				}
				else
				{
					int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(lobby.lobbyID);
					int lobbyMemberLimit = SteamMatchmaking.GetLobbyMemberLimit(lobby.lobbyID);
					if (lobbyMemberLimit > 0 && numLobbyMembers >= lobbyMemberLimit)
					{
						Debug.Log("[SteamLobby] Lobby artık dolu — kodla katılınamaz.");
						result?.Invoke(obj: false);
					}
					else
					{
						BeginJoinAttempt();
						SteamMatchmaking.JoinLobby(lobby.lobbyID);
						result?.Invoke(obj: true);
					}
				}
			}
			else
			{
				Debug.Log("[SteamLobby] Kod bulunamadı: " + code);
				result?.Invoke(obj: false);
			}
		}, code);
	}

	public void RefreshLobbyList()
	{
		ReloadLobbyList();
	}

	private void OnJoinRequest(GameLobbyJoinRequested_t callback)
	{
		BeginJoinAttempt();
		SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
	}

	public void LeaveLobby()
	{
		bool active = NetworkServer.active;
		_voluntaryLeaveInProgress = true;
		SteamFriends.ClearRichPresence();
		if (LobbyID.IsValid() || LobbyID.m_SteamID != 0L)
		{
			SteamMatchmaking.LeaveLobby(LobbyID);
		}
		LobbyID = new CSteamID(0uL);
		MyNetworkManager myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
		if (active)
		{
			myNetworkManager.StopHost();
		}
		else
		{
			myNetworkManager.StopClient();
		}
	}

	private void OnLobbyChatUpdate(LobbyChatUpdate_t callback)
	{
		EChatMemberStateChange rgfChatMemberStateChange = (EChatMemberStateChange)callback.m_rgfChatMemberStateChange;
		if (rgfChatMemberStateChange == EChatMemberStateChange.k_EChatMemberStateChangeLeft || rgfChatMemberStateChange == EChatMemberStateChange.k_EChatMemberStateChangeDisconnected || rgfChatMemberStateChange == EChatMemberStateChange.k_EChatMemberStateChangeKicked)
		{
			CSteamID cSteamID = new CSteamID(callback.m_ulSteamIDUserChanged);
			CSteamID lobbyOwner = SteamMatchmaking.GetLobbyOwner(LobbyID);
			bool flag = cSteamID == lobbyOwner || lobbyOwner.m_SteamID == 0;
			if (!NetworkServer.active && flag)
			{
				Debug.Log("[SteamLobby] Host ayrıldı — lobby kapandı, menüye dönülüyor.");
				ForceLeaveToMenu();
			}
		}
	}

	private void ForceLeaveToMenu()
	{
		SteamFriends.ClearRichPresence();
		if (LobbyID.m_SteamID != 0L)
		{
			SteamMatchmaking.LeaveLobby(LobbyID);
		}
		LobbyID = new CSteamID(0uL);
		MyNetworkManager myNetworkManager = (MyNetworkManager)NetworkManager.singleton;
		if (NetworkClient.active)
		{
			myNetworkManager.StopClient();
		}
	}

	private void OnLobbyEntered(LobbyEnter_t callback)
	{
		EChatRoomEnterResponse eChatRoomEnterResponse = (EChatRoomEnterResponse)callback.m_EChatRoomEnterResponse;
		if (callback.m_bLocked || eChatRoomEnterResponse != EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
		{
			Debug.LogWarning($"[SteamLobby] Lobiye katılma başarısız: {eChatRoomEnterResponse} (locked={callback.m_bLocked})");
			if (callback.m_bLocked)
			{
				SteamMatchmaking.LeaveLobby((CSteamID)callback.m_ulSteamIDLobby);
			}
			EndJoinAttempt();
			LoadingScreen.Instance?.Hide();
			if (_awaitingQuickMatchJoinResult)
			{
				_awaitingQuickMatchJoinResult = false;
			}
			else
			{
				EndMatchmaking();
				NotifyJoinFailed();
			}
			ReloadLobbyList();
			return;
		}
		EndMatchmaking();
		LobbyID = new CSteamID(callback.m_ulSteamIDLobby);
		Debug.Log($"Entered Lobby {LobbyID}");
		if (!NetworkServer.active)
		{
			string lobbyData = SteamMatchmaking.GetLobbyData(LobbyID, "gameversion");
			if (!string.IsNullOrEmpty(lobbyData) && lobbyData != Application.version)
			{
				Debug.LogWarning("[SteamLobby] Versiyon uyuşmazlığı (host: " + lobbyData + ", local: " + Application.version + ") — katılma iptal edildi.");
				SteamMatchmaking.LeaveLobby(LobbyID);
				LobbyID = new CSteamID(0uL);
				EndJoinAttempt();
				LoadingScreen.Instance?.Hide();
				if (_awaitingQuickMatchJoinResult)
				{
					_awaitingQuickMatchJoinResult = false;
				}
				else
				{
					MenuManager.ShowVersionMismatchPopup();
				}
				ReloadLobbyList();
				return;
			}
		}
		SteamFriends.SetRichPresence("connect", "+connect_lobby " + LobbyID.m_SteamID);
		SteamFriends.SetRichPresence("status", "Lobide");
		if (!NetworkServer.active)
		{
			((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(value: true);
			((MyNetworkManager)NetworkManager.singleton).networkAddress = SteamMatchmaking.GetLobbyData(LobbyID, "HostAddress");
			((MyNetworkManager)NetworkManager.singleton).StartClient();
		}
	}

	public void Leave()
	{
		SteamFriends.ClearRichPresence();
		SteamMatchmaking.LeaveLobby(LobbyID);
		LobbyID = new CSteamID(0uL);
	}

	public static void SetLobbyLocation()
	{
		SteamNetworkingUtils.GetLocalPingLocation(out var result);
		SteamNetworkingUtils.ConvertPingLocationToString(ref result, out var pszBuf, 1024);
		SteamMatchmaking.SetLobbyData(LobbyID, "location", pszBuf);
	}

	public void FindMatch()
	{
		if (!IsMatchmaking)
		{
			_findRoutine = StartCoroutine(FindMatchRoutine());
		}
	}

	public void CancelMatchmaking()
	{
		if (IsMatchmaking)
		{
			EndMatchmaking();
			Debug.Log("[SteamLobby] Matchmaking iptal edildi.");
		}
	}

	private void EndMatchmaking()
	{
		if (IsMatchmaking)
		{
			IsMatchmaking = false;
			if (_findRoutine != null)
			{
				StopCoroutine(_findRoutine);
				_findRoutine = null;
			}
			OnMatchmakingEnded?.Invoke();
		}
	}

	private IEnumerator FindMatchRoutine()
	{
		IsMatchmaking = true;
		OnMatchmakingStarted?.Invoke();
		float timeout = quickMatchTimeoutSeconds;
		float elapsed = 0f;
		while (elapsed < timeout)
		{
			ReloadLobbyList();
			yield return new WaitForSeconds(1f);
			elapsed += 1f;
			foreach (Lobby allLobby in allLobbies)
			{
				if ((allLobby.lobbyType ?? "").ToLower() != "public" || SteamMatchmaking.GetNumLobbyMembers(allLobby.lobbyID) >= NetworkManager.singleton.maxConnections)
				{
					continue;
				}
				_awaitingQuickMatchJoinResult = true;
				if (!JoinLobby(allLobby.lobbyID))
				{
					_awaitingQuickMatchJoinResult = false;
					continue;
				}
				while (_awaitingQuickMatchJoinResult)
				{
					yield return null;
				}
			}
		}
		Debug.Log("[SteamLobby] Müsait lobby yok — kendi lobby kuruluyor.");
		_findRoutine = null;
		CreateLobby(isPublic: true);
	}
}
