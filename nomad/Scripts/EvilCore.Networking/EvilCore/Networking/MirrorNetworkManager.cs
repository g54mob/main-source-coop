using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using EpicTransport;
using EvilCore.DI;
using EvilCore.EvilPack.EvilLogger;
using Mirror;
using Mirror.SimpleWeb;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using VContainer;
using kcp2k;

namespace EvilCore.Networking
{
	public class MirrorNetworkManager : NetworkManager, INetworkManager
	{
		[FormerlySerializedAs("activeTransport")]
		[FormerlySerializedAs("transportForLocalTests")]
		[SerializeField]
		private TransportType activeTransportType = TransportType.KCP;

		[SerializeField]
		private bool parrelSyncAutoDetect;

		[SerializeField]
		[Tooltip("Editor/local-test only: when a Development Host starts on a local transport (KCP/Telepathy/SimpleWeb) — via ParrelSync auto-host or the dev-panel Host button — continue from the most recent save instead of generating a fresh world. Ignored in Online mode and on ParrelSync clients.")]
		private bool loadLastSave;

		[SerializeField]
		[Min(0f)]
		private float nearHostSpawnRadius = 4f;

		private readonly Dictionary<int, GameObject> _playerObjects = new Dictionary<int, GameObject>();

		private readonly List<INetworkPlayer> _connectedPlayers = new List<INetworkPlayer>();

		private IReadOnlyList<IOnlineService> _onlineServices;

		private IEOSLobbyManager _lobbyManager;

		private ISceneFlowManager _sceneFlowManager;

		private INetworkObjectSpawnWatcher _spawnWatcher;

		private IGameEntryPoint _gameEntryPoint;

		private INetworkErrorService _networkErrorService;

		private bool _isReturningToBootstrap;

		private bool _isQuitting;

		private bool _isSingleplayerSession;

		private string _pendingMultiplayerSlot;

		private bool _clientEverConnected;

		private TransportError? _lastClientError;

		private bool _intentionalDisconnect;

		private string _serverIP = "localhost";

		private Rect _panelRect;

		private bool _mouseOverPanel;

		private GUIStyle _cachedButtonStyle;

		private IGameSaveService _gameSaveService;

		public TransportType ActiveTransportType => activeTransportType;

		public bool IsLocalTestMode => activeTransportType != TransportType.Online;

		public bool IsSingleplayerSession => _isSingleplayerSession;

		public IReadOnlyList<INetworkPlayer> ConnectedPlayers => _connectedPlayers;

		public string NetworkAddress
		{
			get
			{
				return networkAddress;
			}
			set
			{
				networkAddress = value;
			}
		}

		public int MaxConnections => maxConnections;

		public event Action OnConnectedPlayersChanged;

		[Inject]
		private void Construct(IReadOnlyList<IOnlineService> onlineServices, IEOSLobbyManager lobbyManager, ISceneFlowManager sceneFlowManager, INetworkObjectSpawnWatcher spawnWatcher, IGameEntryPoint gameEntryPoint, INetworkErrorService networkErrorService, IGameSaveService gameSaveService)
		{
			_onlineServices = onlineServices;
			_lobbyManager = lobbyManager;
			_sceneFlowManager = sceneFlowManager;
			_spawnWatcher = spawnWatcher;
			_gameEntryPoint = gameEntryPoint;
			_networkErrorService = networkErrorService;
			_gameSaveService = gameSaveService;
			if (IsLocalTestMode)
			{
				HandleParrelSyncAutoDetect();
				return;
			}
			ActivateOnlineServices();
			LoadMainMenuAsync().Forget();
		}

		public override void Awake()
		{
			base.Awake();
			Application.quitting += delegate
			{
				_isQuitting = true;
			};
		}

		public bool TryGetNetworkObjectById(uint networkId, out GameObject networkObject)
		{
			networkObject = null;
			if (NetworkServer.active && NetworkServer.spawned.TryGetValue(networkId, out var value))
			{
				networkObject = value.gameObject;
				return true;
			}
			if (NetworkClient.active && NetworkClient.spawned.TryGetValue(networkId, out value))
			{
				networkObject = value.gameObject;
				return true;
			}
			_ = networkObject == null;
			return false;
		}

		public async UniTask<(bool success, GameObject networkObject)> TryGetNetworkObjectByIdAsync(uint networkId, int maxAttempts = 50, int delayBetweenAttemptsMs = 100, CancellationToken cancellationToken = default(CancellationToken))
		{
			for (int attempt = 0; attempt < maxAttempts; attempt++)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return (false, null);
				}
				if (TryGetNetworkObjectById(networkId, out var networkObject))
				{
					_ = attempt;
					_ = 0;
					return (true, networkObject);
				}
				if (attempt < maxAttempts - 1)
				{
					await UniTask.Delay(delayBetweenAttemptsMs, ignoreTimeScale: false, PlayerLoopTiming.Update, cancellationToken);
				}
			}
			return (false, null);
		}

		public async UniTask<(bool success, GameObject networkObject)> TryGetNetworkObjectByIdWithBackoffAsync(uint networkId, int maxAttempts = 30, int initialDelayMs = 100, int maxDelayMs = 2000, float backoffMultiplier = 1.5f, CancellationToken cancellationToken = default(CancellationToken))
		{
			float currentDelayMs = initialDelayMs;
			float totalWaitTimeMs = 0f;
			for (int attempt = 0; attempt < maxAttempts; attempt++)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					return (false, null);
				}
				if (TryGetNetworkObjectById(networkId, out var networkObject))
				{
					_ = attempt;
					_ = 0;
					return (true, networkObject);
				}
				if (attempt < maxAttempts - 1)
				{
					int delayToApply = Mathf.RoundToInt(currentDelayMs);
					await UniTask.Delay(delayToApply, ignoreTimeScale: false, PlayerLoopTiming.Update, cancellationToken);
					totalWaitTimeMs += (float)delayToApply;
					currentDelayMs = Mathf.Min(currentDelayMs * backoffMultiplier, maxDelayMs);
				}
			}
			return (false, null);
		}

		public override void OnStartServer()
		{
			base.OnStartServer();
			InvokeRepeating("UpdatePlayerPings", 1f, 2f);
		}

		public override void OnStartHost()
		{
			base.OnStartHost();
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			_clientEverConnected = false;
			_lastClientError = null;
			_intentionalDisconnect = false;
		}

		public override void OnServerConnect(NetworkConnectionToClient conn)
		{
			base.OnServerConnect(conn);
			if (conn == NetworkServer.connections[0])
			{
				_gameEntryPoint?.OnHostStarted();
			}
			else
			{
				_gameEntryPoint?.OnClientStarted();
			}
		}

		public override void OnServerDisconnect(NetworkConnectionToClient conn)
		{
			if (_playerObjects.TryGetValue(conn.connectionId, out var value) && value != null)
			{
				INetworkPlayer component = value.GetComponent<INetworkPlayer>();
				if (component != null)
				{
					UnregisterPlayer(component);
				}
			}
			_playerObjects.Remove(conn.connectionId);
			RescueOwnedObjects(conn);
			base.OnServerDisconnect(conn);
		}

		private static void RescueOwnedObjects(NetworkConnectionToClient conn)
		{
			if (conn == null)
			{
				return;
			}
			foreach (NetworkIdentity item in new List<NetworkIdentity>(conn.owned))
			{
				if (!(item == null) && item.TryGetComponent<IConnectionOwnedCleanup>(out var component))
				{
					try
					{
						component.OnOwnerDisconnecting(conn);
					}
					catch (Exception arg)
					{
						EvilLogger.LogError($"[NetworkManager] OnOwnerDisconnecting failed for '{item.name}' (netId {item.netId}): {arg}", "RescueOwnedObjects", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\MirrorNetworkManager\\MirrorNetworkManager.cs", 300);
					}
				}
			}
		}

		public override void OnStopServer()
		{
			CancelInvoke("UpdatePlayerPings");
			base.OnStopServer();
		}

		public override void OnStopHost()
		{
			base.OnStopHost();
			if (_isSingleplayerSession)
			{
				_isSingleplayerSession = false;
				RestoreDefaultTransport();
				return;
			}
			IEOSLobbyManager lobbyManager = _lobbyManager;
			if (lobbyManager != null && lobbyManager.IsInLobby)
			{
				_lobbyManager.LeaveLobby();
			}
		}

		public override void OnStopClient()
		{
			base.OnStopClient();
			_spawnWatcher?.ClearAllPendingLookups();
			IEOSLobbyManager lobbyManager = _lobbyManager;
			if (lobbyManager != null && lobbyManager.IsInLobby)
			{
				_lobbyManager.LeaveLobby();
			}
			ReturnToBootstrap();
		}

		public override void OnServerChangeScene(string newSceneName)
		{
			base.OnServerChangeScene(newSceneName);
		}

		public override void OnServerSceneChanged(string sceneName)
		{
			base.OnServerSceneChanged(sceneName);
		}

		public override void OnClientConnect()
		{
			base.OnClientConnect();
			_clientEverConnected = true;
		}

		public override void OnClientDisconnect()
		{
			base.OnClientDisconnect();
			ReportClientDisconnect();
			_spawnWatcher?.ClearAllPendingLookups();
			ReturnToBootstrap();
		}

		private void ReportClientDisconnect()
		{
			if (!IsLocalTestMode && !_isSingleplayerSession && !_isQuitting && !_isReturningToBootstrap && !_intentionalDisconnect)
			{
				IEOSLobbyManager lobbyManager = _lobbyManager;
				if (lobbyManager == null || !lobbyManager.WasKicked)
				{
					NetworkErrorType type = (_clientEverConnected ? NetworkErrorType.ConnectionLost : ((_lastClientError == TransportError.Timeout) ? NetworkErrorType.ConnectionTimeout : NetworkErrorType.ConnectionFailed));
					_networkErrorService?.Report(type, _lastClientError?.ToString() ?? "disconnected");
				}
			}
			_clientEverConnected = false;
			_lastClientError = null;
			_intentionalDisconnect = false;
		}

		public void RegisterPlayer(INetworkPlayer player)
		{
			if (!_connectedPlayers.Contains(player))
			{
				_connectedPlayers.Add(player);
				this.OnConnectedPlayersChanged?.Invoke();
			}
		}

		public void UnregisterPlayer(INetworkPlayer player)
		{
			if (_connectedPlayers.Remove(player))
			{
				this.OnConnectedPlayersChanged?.Invoke();
			}
		}

		public override void OnServerAddPlayer(NetworkConnectionToClient conn)
		{
			Vector3 zero = Vector3.zero;
			GameObject gameObject = UnityEngine.Object.Instantiate(playerPrefab, zero, Quaternion.identity);
			NetworkServer.AddPlayerForConnection(conn, gameObject);
			_playerObjects[conn.connectionId] = gameObject;
			INetworkPlayer component = gameObject.GetComponent<INetworkPlayer>();
			component?.ServerSetEosProductUserId(conn.address);
			TrySpawnNearHost(conn, component);
		}

		private void TrySpawnNearHost(NetworkConnectionToClient conn, INetworkPlayer joiningPlayer)
		{
			if (joiningPlayer != null)
			{
				LocalConnectionToClient localConnection = NetworkServer.localConnection;
				if (conn != localConnection && !(localConnection?.identity == null))
				{
					Vector3 position = localConnection.identity.transform.position;
					Vector2 vector = UnityEngine.Random.insideUnitCircle * nearHostSpawnRadius;
					Vector3 position2 = position + new Vector3(vector.x, 0f, vector.y);
					joiningPlayer.ServerApplyInitialSpawnNearHost(position2);
				}
			}
		}

		public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
		{
			base.OnServerError(conn, error, reason);
			if (error != TransportError.Unexpected || string.IsNullOrEmpty(reason) || (!reason.Contains("Unknown Product User ID") && !reason.Contains("Unknown product ID")))
			{
				EvilLogger.LogError($"Server Error: {error} - {reason}", "OnServerError", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\MirrorNetworkManager\\MirrorNetworkManager.cs", 478);
			}
		}

		public override void OnClientError(TransportError error, string reason)
		{
			base.OnClientError(error, reason);
			_lastClientError = error;
			EvilLogger.LogError($"Client Error: {error} - {reason}", "OnClientError", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\MirrorNetworkManager\\MirrorNetworkManager.cs", 488);
		}

		public static void SetObjectVisibility(NetworkIdentity identity, bool forceHidden = false)
		{
			if (!(identity == null))
			{
				if (forceHidden)
				{
					identity.visibility = Visibility.ForceHidden;
				}
				else
				{
					identity.visibility = Visibility.Default;
				}
			}
		}

		public void Disconnect()
		{
			_intentionalDisconnect = true;
			if (NetworkServer.active && NetworkClient.active)
			{
				StopHost();
			}
			else
			{
				if (!NetworkClient.active)
				{
					return;
				}
				StopClient();
				if (base.mode != NetworkManagerMode.Offline)
				{
					_spawnWatcher?.ClearAllPendingLookups();
					IEOSLobbyManager lobbyManager = _lobbyManager;
					if (lobbyManager != null && lobbyManager.IsInLobby)
					{
						_lobbyManager.LeaveLobby();
					}
					ReturnToBootstrap();
					NetworkClient.Shutdown();
				}
			}
		}

		public void DisconnectByAddress(string address)
		{
			if (!NetworkServer.active)
			{
				return;
			}
			foreach (KeyValuePair<int, NetworkConnectionToClient> connection in NetworkServer.connections)
			{
				NetworkConnectionToClient value = connection.Value;
				if (value == null || value.connectionId == 0 || !(value.address == address))
				{
					continue;
				}
				if (_playerObjects.TryGetValue(value.connectionId, out var value2) && value2 != null)
				{
					INetworkPlayer component = value2.GetComponent<INetworkPlayer>();
					if (component != null)
					{
						UnregisterPlayer(component);
					}
					RescueOwnedObjects(value);
					NetworkServer.DestroyPlayerForConnection(value);
				}
				_playerObjects.Remove(value.connectionId);
				value.Disconnect();
				break;
			}
		}

		private void UpdatePlayerPings()
		{
			foreach (KeyValuePair<int, NetworkConnectionToClient> connection in NetworkServer.connections)
			{
				NetworkConnectionToClient value = connection.Value;
				if (!(value?.identity == null))
				{
					value.identity.GetComponent<INetworkPlayer>()?.ServerSetPing((ushort)Mathf.RoundToInt((float)(value.rtt * 1000.0)));
				}
			}
		}

		private void ReturnToBootstrap()
		{
			if (Application.isPlaying && !_isQuitting && !_isReturningToBootstrap)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				if (_sceneFlowManager != null && _sceneFlowManager.IsGameSceneLoaded)
				{
					_isReturningToBootstrap = true;
					ReturnToBootstrapAsync().Forget();
				}
			}
		}

		private async UniTaskVoid ReturnToBootstrapAsync()
		{
			try
			{
				await _sceneFlowManager.TransitionToMainMenuAsync();
			}
			catch (Exception ex)
			{
				EvilLogger.LogError("[NetworkManager] Failed to return to main menu: " + ex.Message, "ReturnToBootstrapAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\MirrorNetworkManager\\MirrorNetworkManager.cs", 624);
			}
			finally
			{
				_isReturningToBootstrap = false;
			}
		}

		private void ActivateOnlineServices()
		{
			foreach (IOnlineService onlineService in _onlineServices)
			{
				onlineService.Activate();
			}
		}

		private async UniTaskVoid LoadMainMenuAsync()
		{
			await UniTask.Yield();
			await _sceneFlowManager.LoadMainMenuSceneAsync();
		}

		private async UniTaskVoid StartHostWithSceneLoad()
		{
			if (loadLastSave)
			{
				TrySelectLastSaveForContinue();
			}
			await _sceneFlowManager.LoadGameSceneAsync();
			StartHost();
		}

		private void TrySelectLastSaveForContinue()
		{
			if (_gameSaveService == null)
			{
				return;
			}
			SaveSlotInfo[] saveSlots = _gameSaveService.GetSaveSlots();
			if (saveSlots == null || saveSlots.Length == 0)
			{
				return;
			}
			SaveSlotInfo[] array = saveSlots;
			for (int i = 0; i < array.Length; i++)
			{
				SaveSlotInfo saveSlotInfo = array[i];
				_gameSaveService.SelectSlotForContinue(saveSlotInfo.SlotId);
				if (_gameSaveService.IsLoadedWorld)
				{
					break;
				}
			}
		}

		private async UniTaskVoid ConnectAsClientWithSceneLoad()
		{
			networkAddress = _serverIP;
			await _sceneFlowManager.LoadGameSceneAsync();
			StartClient();
		}

		private void HandleParrelSyncAutoDetect()
		{
		}

		public void BeginSinglePlayerSession()
		{
			_isSingleplayerSession = true;
			EnableOfflineTransport();
		}

		public void StartSinglePlayerHost(string worldName, int seed)
		{
			StartSinglePlayerHostAsync(worldName, seed).Forget();
		}

		private async UniTaskVoid StartSinglePlayerHostAsync(string worldName, int seed)
		{
			BeginSinglePlayerSession();
			_gameSaveService?.MarkNewGame(worldName);
			_lobbyManager?.SetPendingWorldSeed(seed);
			await _sceneFlowManager.LoadGameSceneAsync();
			StartHost();
		}

		public void StartSinglePlayerContinue(string slotId)
		{
			StartSinglePlayerContinueAsync(slotId).Forget();
		}

		private async UniTaskVoid StartSinglePlayerContinueAsync(string slotId)
		{
			BeginSinglePlayerSession();
			_gameSaveService?.SelectSlotForContinue(slotId);
			await _sceneFlowManager.LoadGameSceneAsync();
			StartHost();
		}

		public void ActivateMultiplayer()
		{
			if (_isSingleplayerSession)
			{
				if (_gameSaveService == null)
				{
					EvilLogger.LogError("[SinglePlayer] ActivateMultiplayer failed — no save service to capture the session.", "ActivateMultiplayer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\MirrorNetworkManager\\MirrorNetworkManager.cs", 784);
					return;
				}
				Time.timeScale = 1f;
				_gameSaveService.SaveNow();
				_pendingMultiplayerSlot = _gameSaveService.ActiveSlotId;
				Disconnect();
			}
		}

		public string ConsumePendingMultiplayerSlot()
		{
			string pendingMultiplayerSlot = _pendingMultiplayerSlot;
			_pendingMultiplayerSlot = null;
			return pendingMultiplayerSlot;
		}

		private void EnableOfflineTransport()
		{
			KcpTransport component = GetComponent<KcpTransport>();
			if (component == null)
			{
				EvilLogger.LogError("[Transport] KcpTransport component missing — cannot start an offline single-player host.", "EnableOfflineTransport", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Networking\\_Core\\MirrorNetworkManager\\MirrorNetworkManager.cs", 822);
			}
			else
			{
				SetActiveTransport(component);
			}
		}

		private void RestoreDefaultTransport()
		{
			Transport transport = activeTransportType switch
			{
				TransportType.KCP => GetComponent<KcpTransport>(), 
				TransportType.Telepathy => GetComponent<TelepathyTransport>(), 
				TransportType.SimpleWeb => GetComponent<SimpleWebTransport>(), 
				TransportType.Online => GetComponent<EosTransport>(), 
				_ => null, 
			};
			if (transport != null)
			{
				SetActiveTransport(transport);
			}
		}

		private void SetActiveTransport(Transport target)
		{
			KcpTransport component = GetComponent<KcpTransport>();
			TelepathyTransport component2 = GetComponent<TelepathyTransport>();
			SimpleWebTransport component3 = GetComponent<SimpleWebTransport>();
			EosTransport component4 = GetComponent<EosTransport>();
			if ((bool)component)
			{
				component.enabled = target == component;
			}
			if ((bool)component2)
			{
				component2.enabled = target == component2;
			}
			if ((bool)component3)
			{
				component3.enabled = target == component3;
			}
			if ((bool)component4)
			{
				component4.enabled = target == component4;
			}
			transport = target;
			Transport.active = target;
		}

		private new void OnValidate()
		{
			KcpTransport component = GetComponent<KcpTransport>();
			TelepathyTransport component2 = GetComponent<TelepathyTransport>();
			SimpleWebTransport component3 = GetComponent<SimpleWebTransport>();
			EosTransport component4 = GetComponent<EosTransport>();
			Transport transport = null;
			switch (activeTransportType)
			{
			case TransportType.KCP:
				transport = component;
				break;
			case TransportType.Telepathy:
				transport = component2;
				break;
			case TransportType.SimpleWeb:
				transport = component3;
				break;
			case TransportType.Online:
				transport = component4;
				break;
			}
			if ((bool)component)
			{
				component.enabled = transport == component;
			}
			if ((bool)component2)
			{
				component2.enabled = transport == component2;
			}
			if ((bool)component3)
			{
				component3.enabled = transport == component3;
			}
			if ((bool)component4)
			{
				component4.enabled = transport == component4;
			}
			if (transport != null)
			{
				base.transport = transport;
			}
		}

		private new void Update()
		{
			if (IsLocalTestMode && _mouseOverPanel && EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}

		private void OnGUI()
		{
			if (!IsLocalTestMode)
			{
				return;
			}
			float num = 200f;
			float height = 120f;
			float x = (float)Screen.width - num - 10f;
			float y = 10f;
			_panelRect = new Rect(x, y, num, height);
			Vector2 mousePosition = Event.current.mousePosition;
			_mouseOverPanel = _panelRect.Contains(mousePosition);
			GUILayout.BeginArea(_panelRect);
			if (!NetworkClient.active && !NetworkServer.active)
			{
				if (DevPanelButton("Host"))
				{
					StartHostWithSceneLoad().Forget();
				}
				GUILayout.BeginHorizontal();
				GUILayout.Label("IP:", GUILayout.Width(18f));
				_serverIP = GUILayout.TextField(_serverIP, GUILayout.Width(162f));
				GUILayout.EndHorizontal();
				if (DevPanelButton("Join"))
				{
					ConnectAsClientWithSceneLoad().Forget();
				}
			}
			else
			{
				string text = ((NetworkServer.active && NetworkClient.active) ? "HOST" : "CLIENT");
				GUILayout.Label("Status: " + text, GUI.skin.box, GUILayout.Width(180f));
				if (DevPanelButton("Disconnect"))
				{
					Disconnect();
				}
			}
			GUILayout.EndArea();
		}

		private bool DevPanelButton(string text)
		{
			if (_cachedButtonStyle == null)
			{
				_cachedButtonStyle = new GUIStyle(GUI.skin.button);
			}
			Rect rect = GUILayoutUtility.GetRect(new GUIContent(text), _cachedButtonStyle, GUILayout.Width(180f));
			Vector2 mousePosition = Event.current.mousePosition;
			bool num = rect.Contains(mousePosition);
			if (num)
			{
				_cachedButtonStyle.normal.background = GUI.skin.button.hover.background;
				_cachedButtonStyle.normal.textColor = GUI.skin.button.hover.textColor;
			}
			else
			{
				_cachedButtonStyle.normal.background = GUI.skin.button.normal.background;
				_cachedButtonStyle.normal.textColor = GUI.skin.button.normal.textColor;
			}
			GUI.Button(rect, text, _cachedButtonStyle);
			if (num && Input.GetMouseButtonDown(0))
			{
				return true;
			}
			return false;
		}

		void INetworkManager.StartHost()
		{
			StartHost();
		}

		void INetworkManager.StartClient()
		{
			StartClient();
		}
	}
}
