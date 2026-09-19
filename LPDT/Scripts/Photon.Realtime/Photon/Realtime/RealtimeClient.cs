#define SUPPORTED_UNITY
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using Photon.Client;

namespace Photon.Realtime
{
	public class RealtimeClient : IPhotonPeerListener
	{
		private enum ClientWorkflowOption
		{
			Default = 0,
			GetRegionsAndPing = 1,
			GetRegionsOnly = 2
		}

		public readonly PhotonPeer RealtimePeer;

		public string LogPrefix;

		public LogLevel LogLevel = LogLevel.Warning;

		public int LogStatsInterval = 5000;

		private int lastStatsLogTime;

		public EncryptionMode EncryptionMode;

		public string NameServerHost = "ns.photonengine.io";

		public ProtocolPorts ProtocolPorts = new ProtocolPorts();

		public Func<string, ServerConnection, string> AddressRewriter;

		private ClientState state;

		internal ConnectionCallbacksContainer ConnectionCallbackTargets;

		internal MatchMakingCallbacksContainer MatchMakingCallbackTargets;

		internal InRoomCallbacksContainer InRoomCallbackTargets;

		internal LobbyCallbacksContainer LobbyCallbackTargets;

		internal ErrorInfoCallbacksContainer ErrorInfoCallbackTargets;

		public SystemConnectionSummary SystemConnectionSummary;

		private TypedLobby targetLobbyCache;

		private readonly List<TypedLobbyInfo> lobbyStatistics = new List<TypedLobbyInfo>();

		public readonly Player LocalPlayer = new Player(string.Empty, -1, isLocal: true);

		private JoinType lastJoinType;

		private EnterRoomArgs enterRoomArgumentsCache;

		private OperationResponse failedRoomEntryOperation;

		private const int FriendRequestListMax = 512;

		private string[] friendListRequested;

		public RegionHandler RegionHandler;

		private readonly Queue<CallbackTargetChange> callbackTargetChanges = new Queue<CallbackTargetChange>();

		private readonly HashSet<object> callbackTargets = new HashSet<object>();

		private ClientWorkflowOption clientWorkflow;

		public EventBetter CallbackMessage = new EventBetter();

		private readonly Pool<ParameterDictionary> paramDictionaryPool = new Pool<ParameterDictionary>(() => new ParameterDictionary(), delegate(ParameterDictionary x)
		{
			x.Clear();
		}, 1);

		public const string Version = "5.1.10";

		[Obsolete("Use RealtimePeer")]
		public PhotonPeer LoadBalancingPeer => RealtimePeer;

		public LogLevel LogLevelPeer
		{
			get
			{
				return RealtimePeer.LogLevel;
			}
			set
			{
				RealtimePeer.LogLevel = value;
			}
		}

		public SerializationProtocol SerializationProtocol
		{
			get
			{
				return RealtimePeer.SerializationProtocolType;
			}
			set
			{
				RealtimePeer.SerializationProtocolType = value;
			}
		}

		public AppSettings AppSettings { get; private set; }

		[Obsolete("Use RealtimeClient.AppSettings.GetAppId(this.ClientType) instead.")]
		public string AppId
		{
			get
			{
				if (AppSettings == null)
				{
					return null;
				}
				return AppSettings.GetAppId(ClientType);
			}
		}

		[Obsolete("Use AppSettings.AppVersion instead. ConnectUsingSettings() will overwrite the AppVersion!")]
		public string AppVersion
		{
			get
			{
				if (AppSettings == null)
				{
					return null;
				}
				return AppSettings.AppVersion;
			}
		}

		public ClientAppType ClientType { get; set; }

		public AuthenticationValues AuthValues { get; set; }

		private object TokenForInit
		{
			get
			{
				if (AppSettings.AuthMode == AuthModeOption.Auth || AuthValues == null)
				{
					return null;
				}
				return AuthValues.Token;
			}
		}

		public string NameServerAddress => GetNameServerAddress();

		public string CurrentServerAddress => RealtimePeer.ServerAddress;

		public string MasterServerAddress { get; set; }

		public string GameServerAddress { get; protected internal set; }

		public ServerConnection Server { get; private set; }

		public ClientState State
		{
			get
			{
				return state;
			}
			set
			{
				if (state != value)
				{
					ClientState arg = state;
					state = value;
					this.StateChanged?.Invoke(arg, state);
				}
			}
		}

		public ConnectionHandler Handler { get; private set; }

		public bool IsConnected
		{
			get
			{
				if (State != ClientState.PeerCreated)
				{
					return State != ClientState.Disconnected;
				}
				return false;
			}
		}

		public bool IsConnectedAndReady
		{
			get
			{
				switch (State)
				{
				case ClientState.PeerCreated:
				case ClientState.Authenticating:
				case ClientState.DisconnectingFromMasterServer:
				case ClientState.ConnectingToGameServer:
				case ClientState.Joining:
				case ClientState.Leaving:
				case ClientState.DisconnectingFromGameServer:
				case ClientState.ConnectingToMasterServer:
				case ClientState.Disconnecting:
				case ClientState.Disconnected:
				case ClientState.ConnectingToNameServer:
				case ClientState.DisconnectingFromNameServer:
					return false;
				default:
					return true;
				}
			}
		}

		public DisconnectCause DisconnectedCause { get; protected set; }

		public bool InLobby => CurrentLobby != null;

		public TypedLobby CurrentLobby { get; internal set; }

		public string NickName
		{
			get
			{
				return LocalPlayer.NickName;
			}
			set
			{
				if (LocalPlayer != null)
				{
					LocalPlayer.NickName = value;
				}
			}
		}

		public string UserId
		{
			get
			{
				if (AuthValues != null)
				{
					return AuthValues.UserId;
				}
				return null;
			}
			set
			{
				if (AuthValues == null)
				{
					AuthValues = new AuthenticationValues();
				}
				AuthValues.UserId = value;
			}
		}

		public Room CurrentRoom { get; set; }

		public bool InRoom
		{
			get
			{
				if (state == ClientState.Joined)
				{
					return CurrentRoom != null;
				}
				return false;
			}
		}

		public int PlayersOnMasterCount { get; internal set; }

		public int PlayersInRoomsCount { get; internal set; }

		public int RoomsCount { get; internal set; }

		public bool IsFetchingFriendList => friendListRequested != null;

		public string CurrentCluster { get; private set; }

		public string CurrentRegion { get; private set; }

		[Obsolete("Use CurrentRegion instead.")]
		public string CloudRegion => CurrentRegion;

		public string SummaryToCache => RegionHandler?.SummaryToCache;

		public event Action<ClientState, ClientState> StateChanged;

		public event Action<EventData> EventReceived;

		public event Action<bool, object> MessageReceived;

		public event Action<OperationResponse> OpResponseReceived;

		public RealtimeClient(ConnectionProtocol protocol = ConnectionProtocol.Udp)
		{
			AppSettings = new AppSettings();
			RealtimePeer = new PhotonPeer(this, protocol);
			ConnectionCallbackTargets = new ConnectionCallbacksContainer(this);
			MatchMakingCallbackTargets = new MatchMakingCallbacksContainer(this);
			InRoomCallbackTargets = new InRoomCallbacksContainer(this);
			LobbyCallbackTargets = new LobbyCallbacksContainer(this);
			ErrorInfoCallbackTargets = new ErrorInfoCallbacksContainer(this);
			SerializationProtocol = SerializationProtocol.GpBinaryV18;
			CustomTypesUnity.Register();
			ConfigUnitySockets();
			State = ClientState.PeerCreated;
		}

		public virtual bool ConnectUsingSettings(AppSettings appSettings)
		{
			if (RealtimePeer.PeerState != PeerStateValue.Disconnected && State != ClientState.ConnectedToNameServer)
			{
				return false;
			}
			if (appSettings == null)
			{
				Log.Error("ConnectUsingSettings() failed. The appSettings can't be null.'", LogLevel, LogPrefix);
				return false;
			}
			AppSettings = new AppSettings(appSettings);
			if (!ClientTypeChecks())
			{
				Log.Error("Can not connect. AppId or ClientType not set correctly.", LogLevel, LogPrefix);
				return false;
			}
			GameServerAddress = string.Empty;
			MasterServerAddress = string.Empty;
			LogLevel = AppSettings.ClientLogging;
			RealtimePeer.LogLevel = AppSettings.NetworkLogging;
			CurrentRegion = null;
			DisconnectedCause = DisconnectCause.None;
			SystemConnectionSummary = null;
			if (AuthValues != null)
			{
				AuthValues.Token = null;
			}
			if (State == ClientState.ConnectedToNameServer)
			{
				return CallAuthenticate();
			}
			if (IPAddress.TryParse(AppSettings.Server, out var _))
			{
				if (AppSettings.Protocol == ConnectionProtocol.WebSocket || AppSettings.Protocol == ConnectionProtocol.WebSocketSecure)
				{
					Log.Error("AppSettings.Server is an IP address. Can not use WS or WSS protocols with IP addresses.", LogLevel, LogPrefix);
					return false;
				}
				if (AppSettings.AuthMode == AuthModeOption.AuthOnceWss)
				{
					AppSettings.AuthMode = AuthModeOption.AuthOnce;
				}
			}
			if (AppSettings.AuthMode == AuthModeOption.AuthOnceWss)
			{
				RealtimePeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
			}
			else
			{
				RealtimePeer.TransportProtocol = AppSettings.Protocol;
			}
			if (Handler == null)
			{
				Handler = ConnectionHandler.BuildInstance(this, ClientType.ToString());
			}
			Handler.StartFallbackSendAckThread();
			if (AppSettings.UseNameServer)
			{
				Server = ServerConnection.NameServer;
				if (!appSettings.IsDefaultNameServer)
				{
					NameServerHost = appSettings.Server;
				}
				if (!RealtimePeer.Connect(NameServerAddress, AppSettings.GetAppId(ClientType), TokenForInit, null, AppSettings.ProxyServer))
				{
					return false;
				}
				State = ClientState.ConnectingToNameServer;
			}
			else
			{
				Server = ServerConnection.MasterServer;
				int port = (appSettings.IsDefaultPort ? ProtocolPorts.Get(RealtimePeer.TransportProtocol, ServerConnection.MasterServer) : appSettings.Port);
				MasterServerAddress = ToProtocolAddress(appSettings.Server, port, RealtimePeer.TransportProtocol);
				if (!RealtimePeer.Connect(MasterServerAddress, AppSettings.GetAppId(ClientType), TokenForInit, null, AppSettings.ProxyServer))
				{
					return false;
				}
				State = ClientState.ConnectingToMasterServer;
			}
			return true;
		}

		private bool ClientTypeChecks()
		{
			if (ClientType == ClientAppType.Detect)
			{
				ClientAppType clientAppType = AppSettings.ClientTypeDetect();
				if (clientAppType == ClientAppType.Detect)
				{
					Log.Error("ConnectUsingSettings requires that the AppSettings contain exactly one value set out of AppIdRealtime, AppIdFusion or AppIdQuantum.");
					return false;
				}
				ClientType = clientAppType;
			}
			return true;
		}

		public void GetRegions(AppSettings appSettings, bool ping = true)
		{
			if (State == ClientState.Disconnected || State == ClientState.PeerCreated)
			{
				clientWorkflow = (ping ? ClientWorkflowOption.GetRegionsAndPing : ClientWorkflowOption.GetRegionsOnly);
				ConnectUsingSettings(appSettings);
				AppSettings.BestRegionSummaryFromStorage = null;
			}
		}

		[Conditional("UNITY_WEBGL")]
		private void CheckConnectSetupWebGl()
		{
		}

		private bool CallConnect(ServerConnection serverType)
		{
			if (ConnectionHandler.AppQuits)
			{
				return false;
			}
			if (State == ClientState.Disconnecting)
			{
				Log.Error("CallConnect() failed. Can't connect while disconnecting (still). Current state: " + State, LogLevel, LogPrefix);
				return false;
			}
			if (serverType != ServerConnection.NameServer)
			{
				if (AppSettings.AuthMode != AuthModeOption.Auth && TokenForInit == null)
				{
					Log.Error("Connect() failed. Can't connect to " + serverType.ToString() + " with Token == null in AuthMode: " + AppSettings.AuthMode, LogLevel, LogPrefix);
					return false;
				}
				RealtimePeer.TransportProtocol = AppSettings.Protocol;
			}
			string serverAddress = null;
			ClientState clientState = ClientState.Disconnected;
			switch (serverType)
			{
			case ServerConnection.NameServer:
				serverAddress = GetNameServerAddress();
				clientState = ClientState.ConnectingToNameServer;
				if (AuthValues != null)
				{
					AuthValues.Token = null;
				}
				RealtimePeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
				break;
			case ServerConnection.MasterServer:
				serverAddress = MasterServerAddress;
				clientState = ClientState.ConnectingToMasterServer;
				break;
			case ServerConnection.GameServer:
				serverAddress = GameServerAddress;
				clientState = ClientState.ConnectingToGameServer;
				break;
			}
			bool num = RealtimePeer.Connect(serverAddress, AppSettings.GetAppId(ClientType), TokenForInit, null, AppSettings.ProxyServer);
			if (num)
			{
				DisconnectedCause = DisconnectCause.None;
				SystemConnectionSummary = null;
				Server = serverType;
				State = clientState;
			}
			return num;
		}

		public bool ReconnectToMaster()
		{
			if (RealtimePeer.PeerState != PeerStateValue.Disconnected)
			{
				return false;
			}
			if (string.IsNullOrEmpty(MasterServerAddress))
			{
				return false;
			}
			if (AuthValues == null || AuthValues.Token == null)
			{
				return false;
			}
			return CallConnect(ServerConnection.MasterServer);
		}

		public bool ReconnectAndRejoin(object ticket = null)
		{
			if (RealtimePeer.PeerState != PeerStateValue.Disconnected)
			{
				return false;
			}
			if (string.IsNullOrEmpty(GameServerAddress))
			{
				return false;
			}
			if (enterRoomArgumentsCache == null)
			{
				return false;
			}
			if (AuthValues == null || AuthValues.Token == null)
			{
				return false;
			}
			lastJoinType = JoinType.JoinRoom;
			enterRoomArgumentsCache.JoinMode = JoinMode.RejoinOnly;
			enterRoomArgumentsCache.Ticket = ticket;
			return CallConnect(ServerConnection.GameServer);
		}

		public void Disconnect()
		{
			Disconnect(DisconnectCause.DisconnectByClientLogic);
		}

		internal void Disconnect(DisconnectCause cause = DisconnectCause.DisconnectByClientLogic)
		{
			if (State != ClientState.Disconnecting && State != ClientState.Disconnected && State != ClientState.PeerCreated)
			{
				State = ClientState.Disconnecting;
				DisconnectedCause = cause;
				RealtimePeer.Disconnect();
			}
		}

		private void DisconnectToReconnect()
		{
			switch (Server)
			{
			case ServerConnection.NameServer:
				State = ClientState.DisconnectingFromNameServer;
				break;
			case ServerConnection.MasterServer:
				State = ClientState.DisconnectingFromMasterServer;
				break;
			case ServerConnection.GameServer:
				State = ClientState.DisconnectingFromGameServer;
				break;
			}
			RealtimePeer.Disconnect();
		}

		public void SimulateConnectionLoss(bool simulateTimeout)
		{
			if (simulateTimeout)
			{
				RealtimePeer.NetworkSimulationSettings.IncomingLossPercentage = 100;
				RealtimePeer.NetworkSimulationSettings.OutgoingLossPercentage = 100;
			}
			RealtimePeer.IsSimulationEnabled = simulateTimeout;
		}

		private bool CallAuthenticate()
		{
			if (AppSettings.UseNameServer && Server != ServerConnection.NameServer && (AuthValues == null || AuthValues.Token == null))
			{
				Log.Error($"Authenticate without Token is only allowed on Name Server. Will not authenticate on {Server}: {CurrentServerAddress}. State: {State}", LogLevel, LogPrefix);
				return false;
			}
			if (AuthValues != null)
			{
				AuthValues.AreValid();
			}
			if (!string.IsNullOrEmpty(AppSettings.FixedRegion) && Server == ServerConnection.NameServer)
			{
				CurrentRegion = AppSettings.FixedRegion;
			}
			if (AppSettings.AuthMode == AuthModeOption.Auth)
			{
				if (!CheckIfOpCanBeSent(230, Server, "Authenticate"))
				{
					return false;
				}
				return OpAuthenticate(AppSettings.GetAppId(ClientType), AppSettings.AppVersion, AuthValues, CurrentRegion, AppSettings.EnableLobbyStatistics && Server == ServerConnection.MasterServer);
			}
			if (!CheckIfOpCanBeSent(231, Server, "AuthenticateOnce"))
			{
				return false;
			}
			ConnectionProtocol protocol = AppSettings.Protocol;
			return OpAuthenticateOnce(AppSettings.GetAppId(ClientType), AppSettings.AppVersion, AuthValues, CurrentRegion, EncryptionMode, protocol);
		}

		public void Service()
		{
			RealtimePeer.Service();
			LogStats();
		}

		public bool DispatchIncomingCommands()
		{
			return RealtimePeer.DispatchIncomingCommands();
		}

		public bool SendOutgoingCommands()
		{
			bool result = RealtimePeer.SendOutgoingCommands();
			LogStats();
			return result;
		}

		private void LogStats()
		{
			if ((int)LogLevel >= 3 && LogStatsInterval > 0 && State != ClientState.Disconnected && RealtimePeer.ConnectionTime - lastStatsLogTime >= LogStatsInterval)
			{
				lastStatsLogTime = RealtimePeer.ConnectionTime;
			}
		}

		private string ConnectLog(string prefix)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string appId = AppSettings.GetAppId(ClientType);
			string text = ((!string.IsNullOrEmpty(appId) && appId.Length > 8) ? appId.Substring(0, 8) : appId);
			string text2 = ((AuthValues != null) ? AuthValues.AuthType.ToString() : "N/A");
			string text3 = (string.IsNullOrEmpty(CurrentRegion) ? "" : ("(" + CurrentRegion + ")"));
			string text4 = DateTime.UtcNow.ToShortTimeString();
			stringBuilder.Append($"{prefix} UTC: {text4} AppID: \"{text}***\" AppVersion: \"{AppSettings.AppVersion}\" Auth: {text2} Client: v{PhotonPeer.Version} ({RealtimePeer.TargetFramework}, {RealtimePeer.SocketImplementation.Name}, {EncryptionMode}) Server: {CurrentServerAddress} {text3}");
			return stringBuilder.ToString();
		}

		[Conditional("SUPPORTED_UNITY")]
		private void ConfigUnitySockets()
		{
			Type type = null;
			type = Type.GetType("Photon.Client.SocketWebTcp, PhotonWebSocket", throwOnError: false);
			if (type == null)
			{
				type = Type.GetType("Photon.Client.SocketWebTcp, Assembly-CSharp-firstpass", throwOnError: false);
			}
			if (type == null)
			{
				type = Type.GetType("Photon.Client.SocketWebTcp, Assembly-CSharp", throwOnError: false);
			}
			if (type != null)
			{
				RealtimePeer.SocketImplementationConfig[ConnectionProtocol.WebSocket] = type;
				RealtimePeer.SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = type;
			}
		}

		private string GetNameServerAddress()
		{
			ushort port = ProtocolPorts.Get(RealtimePeer.TransportProtocol, ServerConnection.NameServer);
			if (AppSettings.UseNameServer && AppSettings.Port != 0)
			{
				port = AppSettings.Port;
			}
			return ToProtocolAddress(NameServerHost, port, RealtimePeer.TransportProtocol);
		}

		private string ToProtocolAddress(string address, int port, ConnectionProtocol protocol)
		{
			string empty = string.Empty;
			switch (protocol)
			{
			case ConnectionProtocol.Udp:
			case ConnectionProtocol.Tcp:
				return $"{address}:{port}";
			case ConnectionProtocol.WebSocket:
				empty = "ws://";
				break;
			case ConnectionProtocol.WebSocketSecure:
				empty = "wss://";
				break;
			default:
				throw new ArgumentOutOfRangeException($"Can not handle protocol: {protocol}.");
			}
			Uri uri = new Uri(empty + address);
			string text = $"{uri.Scheme}://{uri.Host}:{port}{uri.AbsolutePath}".TrimEnd('/');
			if (AddressRewriter != null)
			{
				text = AddressRewriter(text, ServerConnection.NameServer);
			}
			return text;
		}

		protected internal static string ReplacePortWithAlternative(string address, ushort replacementPort)
		{
			if (string.IsNullOrEmpty(address) || replacementPort == 0)
			{
				return address;
			}
			if (address.StartsWith("ws"))
			{
				return new UriBuilder(address)
				{
					Port = replacementPort
				}.ToString();
			}
			UriBuilder uriBuilder = new UriBuilder("scheme://" + address);
			return $"{uriBuilder.Host}:{replacementPort}";
		}

		private string GetMatchmakingHash(TypedLobby lobbyInArgs)
		{
			string text = "";
			text = ((lobbyInArgs != null) ? lobbyInArgs.ToString() : ((CurrentLobby == null) ? TypedLobby.Default.ToString() : CurrentLobby.ToString()));
			string text2 = (AppSettings.GetAppId(ClientType) + AppSettings.AppVersion + CurrentRegion + CurrentCluster).GetStableHashCode().ToString("x");
			return "MMH: " + text2 + " " + text;
		}

		private void ReadoutProperties(PhotonHashtable gameProperties, PhotonHashtable actorProperties, int targetActorNr)
		{
			if (CurrentRoom == null)
			{
				Log.Error("Reading properties requires a CurrentRoom.", LogLevel, LogPrefix);
				return;
			}
			if (gameProperties != null)
			{
				CurrentRoom.InternalCacheProperties(gameProperties);
				if (InRoom)
				{
					InRoomCallbackTargets.OnRoomPropertiesUpdate(gameProperties);
				}
			}
			if (actorProperties == null || actorProperties.Count <= 0)
			{
				return;
			}
			if (targetActorNr > 0)
			{
				Player player = CurrentRoom.GetPlayer(targetActorNr);
				if (player != null)
				{
					PhotonHashtable photonHashtable = ReadoutPropertiesForActorNr(actorProperties, targetActorNr);
					player.InternalCacheProperties(photonHashtable);
					InRoomCallbackTargets.OnPlayerPropertiesUpdate(player, photonHashtable);
				}
				return;
			}
			foreach (object key in actorProperties.Keys)
			{
				int num = (int)key;
				if (num != 0)
				{
					PhotonHashtable photonHashtable2 = (PhotonHashtable)actorProperties[key];
					string nickName = (string)photonHashtable2[byte.MaxValue];
					Player player2 = CurrentRoom.GetPlayer(num);
					if (player2 == null)
					{
						player2 = new Player(nickName, num, isLocal: false, photonHashtable2);
						CurrentRoom.StorePlayer(player2);
					}
					player2.InternalCacheProperties(photonHashtable2);
				}
			}
		}

		private PhotonHashtable ReadoutPropertiesForActorNr(PhotonHashtable actorProperties, int actorNr)
		{
			if (actorProperties.ContainsKey(actorNr))
			{
				return (PhotonHashtable)actorProperties[actorNr];
			}
			return actorProperties;
		}

		public void ChangeLocalID(int newID)
		{
			if (LocalPlayer != null)
			{
				if (CurrentRoom == null)
				{
					LocalPlayer.ChangeLocalID(newID);
					LocalPlayer.RoomReference = null;
				}
				else
				{
					CurrentRoom.RemovePlayer(LocalPlayer);
					LocalPlayer.ChangeLocalID(newID);
					CurrentRoom.StorePlayer(LocalPlayer);
				}
			}
		}

		private void GameEnteredOnGameServer(OperationResponse operationResponse)
		{
			CurrentRoom = CreateRoom(enterRoomArgumentsCache.RoomName, enterRoomArgumentsCache.RoomOptions);
			CurrentRoom.RealtimeClient = this;
			CurrentRoom.Lobby = enterRoomArgumentsCache.Lobby;
			int newID = (int)operationResponse[254];
			ChangeLocalID(newID);
			if (operationResponse.Parameters.ContainsKey(252))
			{
				int[] actorsInGame = (int[])operationResponse.Parameters[252];
				UpdatedActorList(actorsInGame);
			}
			if (operationResponse.Parameters.ContainsKey(201))
			{
				string text = (string)operationResponse.Parameters[201];
				if (!string.IsNullOrEmpty(text))
				{
					string.Equals(text, "webhooks", StringComparison.InvariantCultureIgnoreCase);
				}
			}
			PhotonHashtable actorProperties = (PhotonHashtable)operationResponse[249];
			PhotonHashtable gameProperties = (PhotonHashtable)operationResponse[248];
			ReadoutProperties(gameProperties, actorProperties, 0);
			if (operationResponse.Parameters.TryGetValue(191, out var value))
			{
				CurrentRoom.InternalCacheRoomFlags((int)value);
			}
			if (CurrentRoom.SuppressRoomEvents)
			{
				State = ClientState.Joined;
				LocalPlayer.UpdateNickNameOnJoined();
				if (lastJoinType == JoinType.CreateRoom || (lastJoinType == JoinType.JoinOrCreateRoom && LocalPlayer.ActorNumber == 1))
				{
					MatchMakingCallbackTargets.OnCreatedRoom();
				}
				MatchMakingCallbackTargets.OnJoinedRoom();
			}
		}

		private void UpdatedActorList(int[] actorsInGame)
		{
			if (actorsInGame == null)
			{
				return;
			}
			foreach (int num in actorsInGame)
			{
				if (num != 0 && CurrentRoom.GetPlayer(num) == null)
				{
					CurrentRoom.StorePlayer(new Player(string.Empty, num, isLocal: false));
				}
			}
		}

		protected internal virtual Room CreateRoom(string roomName, RoomOptions opt)
		{
			return new Room(roomName, opt);
		}

		private bool CheckIfOpAllowedOnServer(byte opCode, ServerConnection serverConnection)
		{
			switch (serverConnection)
			{
			case ServerConnection.MasterServer:
				switch (opCode)
				{
				case 217:
				case 218:
				case 221:
				case 222:
				case 225:
				case 226:
				case 227:
				case 228:
				case 229:
				case 230:
				case 231:
					return true;
				}
				break;
			case ServerConnection.GameServer:
				switch (opCode)
				{
				case 218:
				case 226:
				case 227:
				case 230:
				case 231:
				case 248:
				case 251:
				case 252:
				case 253:
				case 254:
					return true;
				}
				break;
			case ServerConnection.NameServer:
				if (opCode == 218 || opCode == 220 || (uint)(opCode - 230) <= 1u)
				{
					return true;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("serverConnection", serverConnection, null);
			}
			return false;
		}

		private bool CheckIfOpCanBeSent(byte opCode, ServerConnection serverConnection, string opName)
		{
			if (!CheckIfOpAllowedOnServer(opCode, serverConnection))
			{
				Log.Error($"Operation {opName} ({opCode}) not allowed on current server ({serverConnection})", LogLevel, LogPrefix);
				return false;
			}
			if (!CheckIfClientIsReadyToCallOperation(opCode))
			{
				if (opCode == 253 && (State == ClientState.Leaving || State == ClientState.Disconnecting || State == ClientState.DisconnectingFromGameServer))
				{
					return false;
				}
				Log.Error($"Operation {opName} ({opCode}) not called because client is not connected or not ready yet. Client state: {Enum.GetName(typeof(ClientState), State)}", LogLevel, LogPrefix);
				return false;
			}
			if (RealtimePeer.PeerState != PeerStateValue.Connected)
			{
				Log.Error($"Operation {opName} ({opCode}) can't be sent because peer is not connected. Peer state: {RealtimePeer.PeerState}", LogLevel, LogPrefix);
				return false;
			}
			return true;
		}

		private bool CheckIfClientIsReadyToCallOperation(byte opCode)
		{
			switch (opCode)
			{
			case 230:
			case 231:
				if (!IsConnectedAndReady && State != ClientState.ConnectingToNameServer && State != ClientState.ConnectingToMasterServer)
				{
					return State == ClientState.ConnectingToGameServer;
				}
				return true;
			case 248:
			case 251:
			case 252:
			case 253:
			case 254:
				return InRoom;
			case 226:
			case 227:
				if (State != ClientState.ConnectedToMasterServer && !InLobby)
				{
					return State == ClientState.ConnectedToGameServer;
				}
				return true;
			case 228:
				return InLobby;
			case 217:
			case 221:
			case 222:
			case 225:
			case 229:
				if (State != ClientState.ConnectedToMasterServer)
				{
					return InLobby;
				}
				return true;
			case 220:
				return State == ClientState.ConnectedToNameServer;
			default:
				return IsConnected;
			}
		}

		public virtual void DebugReturn(LogLevel level, string message)
		{
			switch (level)
			{
			case LogLevel.Error:
				Log.Error(message, RealtimePeer.LogLevel, LogPrefix);
				break;
			case LogLevel.Off:
			case LogLevel.Warning:
			case LogLevel.Info:
			case LogLevel.Debug:
				break;
			}
		}

		private void CallbackRoomEnterFailed(OperationResponse operationResponse)
		{
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.OperationCode == 226)
				{
					MatchMakingCallbackTargets.OnJoinRoomFailed(operationResponse.ReturnCode, operationResponse.DebugMessage);
				}
				else if (operationResponse.OperationCode == 227)
				{
					MatchMakingCallbackTargets.OnCreateRoomFailed(operationResponse.ReturnCode, operationResponse.DebugMessage);
				}
				else if (operationResponse.OperationCode == 225)
				{
					MatchMakingCallbackTargets.OnJoinRandomFailed(operationResponse.ReturnCode, operationResponse.DebugMessage);
				}
			}
		}

		public virtual void OnOperationResponse(OperationResponse operationResponse)
		{
			if (operationResponse.Parameters.ContainsKey(221))
			{
				if (AuthValues == null)
				{
					AuthValues = new AuthenticationValues();
				}
				AuthValues.Token = operationResponse.Parameters[221];
			}
			if (operationResponse.ReturnCode == 32743)
			{
				Disconnect(DisconnectCause.DisconnectByOperationLimit);
			}
			switch (operationResponse.OperationCode)
			{
			case 230:
			case 231:
			{
				if (operationResponse.ReturnCode != 0)
				{
					Log.Error($"{operationResponse.ToStringFull()} Server: {Server} Address: {RealtimePeer.ServerAddress}", LogLevel, LogPrefix);
					switch (operationResponse.ReturnCode)
					{
					case short.MaxValue:
						DisconnectedCause = DisconnectCause.InvalidAuthentication;
						break;
					case 32755:
						DisconnectedCause = DisconnectCause.CustomAuthenticationFailed;
						ConnectionCallbackTargets.OnCustomAuthenticationFailed(operationResponse.DebugMessage);
						break;
					case 32756:
						DisconnectedCause = DisconnectCause.InvalidRegion;
						break;
					case 32757:
						DisconnectedCause = DisconnectCause.MaxCcuReached;
						break;
					case -3:
					case -2:
						DisconnectedCause = DisconnectCause.OperationNotAllowedInCurrentState;
						break;
					case 32753:
						DisconnectedCause = DisconnectCause.AuthenticationTicketExpired;
						break;
					}
					Disconnect(DisconnectedCause);
					break;
				}
				if (Server == ServerConnection.NameServer || Server == ServerConnection.MasterServer)
				{
					if (operationResponse.Parameters.ContainsKey(225))
					{
						string text3 = (string)operationResponse.Parameters[225];
						if (!string.IsNullOrEmpty(text3))
						{
							UserId = text3;
							LocalPlayer.UserId = text3;
						}
					}
					if (operationResponse.Parameters.ContainsKey(202))
					{
						NickName = (string)operationResponse.Parameters[202];
					}
					if (operationResponse.Parameters.ContainsKey(192))
					{
						SetupEncryption((Dictionary<byte, object>)operationResponse.Parameters[192]);
					}
				}
				if (Server == ServerConnection.NameServer)
				{
					if (AppSettings.AuthMode == AuthModeOption.AuthOnceWss && RealtimePeer.TransportProtocol != AppSettings.Protocol)
					{
						RealtimePeer.TransportProtocol = AppSettings.Protocol;
					}
					string text4 = operationResponse[196] as string;
					if (!string.IsNullOrEmpty(text4))
					{
						CurrentCluster = text4;
					}
					MasterServerAddress = operationResponse[230] as string;
					ushort num2 = ProtocolPorts.Get(RealtimePeer.TransportProtocol, ServerConnection.MasterServer);
					if (num2 != 0)
					{
						MasterServerAddress = ReplacePortWithAlternative(MasterServerAddress, num2);
					}
					if (AddressRewriter != null)
					{
						MasterServerAddress = AddressRewriter(MasterServerAddress, ServerConnection.MasterServer);
					}
					DisconnectToReconnect();
				}
				else if (Server == ServerConnection.MasterServer)
				{
					State = ClientState.ConnectedToMasterServer;
					if (failedRoomEntryOperation == null)
					{
						ConnectionCallbackTargets.OnConnectedToMaster();
					}
					else
					{
						CallbackRoomEnterFailed(failedRoomEntryOperation);
						failedRoomEntryOperation = null;
					}
					if (AppSettings.AuthMode != AuthModeOption.Auth)
					{
						OpSettings(AppSettings.EnableLobbyStatistics);
					}
				}
				else if (Server == ServerConnection.GameServer)
				{
					State = ClientState.Joining;
					enterRoomArgumentsCache.OnGameServer = true;
					if (lastJoinType == JoinType.JoinRoom || lastJoinType == JoinType.JoinRandomRoom || lastJoinType == JoinType.JoinRandomOrCreateRoom || lastJoinType == JoinType.JoinOrCreateRoom)
					{
						OpJoinRoomIntern(enterRoomArgumentsCache);
					}
					else if (lastJoinType == JoinType.CreateRoom)
					{
						OpCreateRoomIntern(enterRoomArgumentsCache);
					}
					break;
				}
				Dictionary<string, object> dictionary = (Dictionary<string, object>)operationResponse[245];
				if (dictionary != null)
				{
					ConnectionCallbackTargets.OnCustomAuthenticationResponse(dictionary);
				}
				break;
			}
			case 220:
				if (operationResponse.ReturnCode == short.MaxValue)
				{
					Log.Error(string.Format("GetRegions failed. AppId is unknown on the (cloud) server. " + operationResponse.DebugMessage), LogLevel, LogPrefix);
					Disconnect(DisconnectCause.InvalidAuthentication);
					break;
				}
				if (operationResponse.ReturnCode != 0)
				{
					Log.Error($"GetRegions failed. Can't provide regions list. ReturnCode: {operationResponse.ReturnCode}: {operationResponse.DebugMessage}", LogLevel, LogPrefix);
					Disconnect(DisconnectCause.InvalidAuthentication);
					break;
				}
				if (RegionHandler == null)
				{
					RegionHandler = new RegionHandler(ProtocolPorts.Get(ConnectionProtocol.Udp, ServerConnection.MasterServer));
				}
				if (RegionHandler.IsPinging)
				{
					return;
				}
				RegionHandler.SetRegions(operationResponse, this);
				if (clientWorkflow == ClientWorkflowOption.GetRegionsOnly)
				{
					Disconnect();
					ConnectionCallbackTargets.OnRegionListReceived(RegionHandler);
					return;
				}
				if (clientWorkflow == ClientWorkflowOption.Default)
				{
					ConnectionCallbackTargets.OnRegionListReceived(RegionHandler);
				}
				if (State == ClientState.ConnectedToNameServer)
				{
					RegionHandler.PingMinimumOfRegions(OnRegionPingCompleted, AppSettings.BestRegionSummaryFromStorage);
				}
				break;
			case 225:
			case 226:
			case 227:
			{
				if (operationResponse.ReturnCode != 0)
				{
					if (Server == ServerConnection.GameServer)
					{
						failedRoomEntryOperation = operationResponse;
						DisconnectToReconnect();
					}
					else
					{
						State = (InLobby ? ClientState.JoinedLobby : ClientState.ConnectedToMasterServer);
						CallbackRoomEnterFailed(operationResponse);
					}
					break;
				}
				if (Server == ServerConnection.GameServer)
				{
					GameEnteredOnGameServer(operationResponse);
					break;
				}
				GameServerAddress = (string)operationResponse[230];
				ushort num = ProtocolPorts.Get(RealtimePeer.TransportProtocol, ServerConnection.GameServer);
				if (num != 0)
				{
					GameServerAddress = ReplacePortWithAlternative(GameServerAddress, num);
				}
				if (AddressRewriter != null)
				{
					GameServerAddress = AddressRewriter(GameServerAddress, ServerConnection.GameServer);
				}
				string text2 = operationResponse[byte.MaxValue] as string;
				if (!string.IsNullOrEmpty(text2))
				{
					enterRoomArgumentsCache.RoomName = text2;
				}
				DisconnectToReconnect();
				break;
			}
			case 217:
			{
				if (operationResponse.ReturnCode != 0)
				{
					Log.Error("GetGameList failed: " + operationResponse.ToStringFull(), LogLevel, LogPrefix);
					break;
				}
				List<RoomInfo> list2 = new List<RoomInfo>();
				PhotonHashtable photonHashtable = (PhotonHashtable)operationResponse[222];
				foreach (string key in photonHashtable.Keys)
				{
					list2.Add(new RoomInfo(key, (PhotonHashtable)photonHashtable[key]));
				}
				LobbyCallbackTargets.OnRoomListUpdate(list2);
				break;
			}
			case 229:
				CurrentLobby = targetLobbyCache;
				targetLobbyCache = null;
				State = ClientState.JoinedLobby;
				LobbyCallbackTargets.OnJoinedLobby();
				break;
			case 228:
				CurrentLobby = null;
				targetLobbyCache = null;
				State = ClientState.ConnectedToMasterServer;
				LobbyCallbackTargets.OnLeftLobby();
				break;
			case 254:
				DisconnectToReconnect();
				break;
			case 222:
			{
				if (operationResponse.ReturnCode != 0)
				{
					Log.Error("OpFindFriends failed: " + operationResponse.ToStringFull(), LogLevel, LogPrefix);
					friendListRequested = null;
					break;
				}
				bool[] array = operationResponse[1] as bool[];
				string[] array2 = operationResponse[2] as string[];
				List<FriendInfo> list = new List<FriendInfo>(friendListRequested.Length);
				for (int i = 0; i < friendListRequested.Length; i++)
				{
					FriendInfo friendInfo = new FriendInfo();
					friendInfo.UserId = friendListRequested[i];
					friendInfo.Room = array2[i];
					friendInfo.IsOnline = array[i];
					list.Insert(i, friendInfo);
				}
				friendListRequested = null;
				MatchMakingCallbackTargets.OnFriendListUpdate(list);
				break;
			}
			}
			this.OpResponseReceived?.Invoke(operationResponse);
		}

		public virtual void OnStatusChanged(StatusCode statusCode)
		{
			switch (statusCode)
			{
			case StatusCode.Connect:
				if (State == ClientState.ConnectingToNameServer)
				{
					Server = ServerConnection.NameServer;
				}
				if (State == ClientState.ConnectingToGameServer)
				{
					Server = ServerConnection.GameServer;
				}
				if (State == ClientState.ConnectingToMasterServer)
				{
					Server = ServerConnection.MasterServer;
					AppSettings.EnableProtocolFallback = false;
				}
				if ((int)LogLevel >= 3)
				{
					lastStatsLogTime = RealtimePeer.ConnectionTime;
				}
				if (RealtimePeer.TransportProtocol != ConnectionProtocol.WebSocketSecure)
				{
					if (Server == ServerConnection.NameServer || AppSettings.AuthMode == AuthModeOption.Auth)
					{
						RealtimePeer.EstablishEncryption();
					}
					break;
				}
				goto case StatusCode.EncryptionEstablished;
			case StatusCode.EncryptionEstablished:
				if (Server == ServerConnection.NameServer)
				{
					State = ClientState.ConnectedToNameServer;
					if (string.IsNullOrEmpty(AppSettings.FixedRegion))
					{
						OpGetRegions();
						break;
					}
				}
				else if (AppSettings.AuthMode == AuthModeOption.AuthOnce || AppSettings.AuthMode == AuthModeOption.AuthOnceWss)
				{
					break;
				}
				if (CallAuthenticate())
				{
					State = ClientState.Authenticating;
				}
				else
				{
					Log.Error($"OpAuthenticate failed. Check log output and AuthValues. State: {State}", LogLevel, LogPrefix);
				}
				break;
			case StatusCode.Disconnect:
			{
				friendListRequested = null;
				bool flag = CurrentRoom != null;
				CurrentRoom = null;
				ChangeLocalID(-1);
				CurrentLobby = null;
				targetLobbyCache = null;
				if (Server == ServerConnection.GameServer && flag)
				{
					MatchMakingCallbackTargets.OnLeftRoom();
				}
				if (RealtimePeer.TransportProtocol != AppSettings.Protocol)
				{
					RealtimePeer.TransportProtocol = AppSettings.Protocol;
				}
				switch (State)
				{
				case ClientState.ConnectWithFallbackProtocol:
					AppSettings.EnableProtocolFallback = false;
					AppSettings.Protocol = ConnectionProtocol.WebSocketSecure;
					RealtimePeer.TransportProtocol = ConnectionProtocol.WebSocketSecure;
					AppSettings.UseNameServer = true;
					AppSettings.Port = 0;
					if (AuthValues != null)
					{
						AuthValues.Token = null;
					}
					if (RealtimePeer.Connect(NameServerAddress, AppSettings.GetAppId(ClientType), TokenForInit, null, AppSettings.ProxyServer))
					{
						State = ClientState.ConnectingToNameServer;
					}
					break;
				case ClientState.PeerCreated:
				case ClientState.Disconnecting:
					Handler.StopFallbackSendAckThread();
					State = ClientState.Disconnected;
					ConnectionCallbackTargets.OnDisconnected(DisconnectedCause);
					break;
				case ClientState.DisconnectingFromGameServer:
				case ClientState.DisconnectingFromNameServer:
					CallConnect(ServerConnection.MasterServer);
					break;
				case ClientState.DisconnectingFromMasterServer:
					CallConnect(ServerConnection.GameServer);
					break;
				default:
					Handler.StopFallbackSendAckThread();
					State = ClientState.Disconnected;
					ConnectionCallbackTargets.OnDisconnected(DisconnectedCause);
					break;
				case ClientState.Disconnected:
					break;
				}
				break;
			}
			case StatusCode.DisconnectByServerUserLimit:
				Log.Error("MaxCcuReached. Connection rejected due to the AppId CCU limit.", LogLevel, LogPrefix);
				DisconnectedCause = DisconnectCause.MaxCcuReached;
				State = ClientState.Disconnecting;
				break;
			case StatusCode.DnsExceptionOnConnect:
				DisconnectedCause = DisconnectCause.DnsExceptionOnConnect;
				State = ClientState.Disconnecting;
				break;
			case StatusCode.ServerAddressInvalid:
				DisconnectedCause = DisconnectCause.ServerAddressInvalid;
				State = ClientState.Disconnecting;
				break;
			case StatusCode.SecurityExceptionOnConnect:
			case StatusCode.ExceptionOnConnect:
			case StatusCode.EncryptionFailedToEstablish:
				SystemConnectionSummary = new SystemConnectionSummary(this);
				DisconnectedCause = DisconnectCause.ExceptionOnConnect;
				if (AppSettings.EnableProtocolFallback && (State == ClientState.ConnectingToNameServer || State == ClientState.ConnectingToMasterServer) && RealtimePeer.UsedProtocol != ConnectionProtocol.WebSocketSecure)
				{
					State = ClientState.ConnectWithFallbackProtocol;
				}
				else
				{
					State = ClientState.Disconnecting;
				}
				break;
			case StatusCode.Exception:
			case StatusCode.SendError:
			case StatusCode.ExceptionOnReceive:
				SystemConnectionSummary = new SystemConnectionSummary(this);
				DisconnectedCause = DisconnectCause.Exception;
				State = ClientState.Disconnecting;
				break;
			case StatusCode.DisconnectByServerTimeout:
				SystemConnectionSummary = new SystemConnectionSummary(this);
				DisconnectedCause = DisconnectCause.ServerTimeout;
				State = ClientState.Disconnecting;
				break;
			case StatusCode.DisconnectByServerLogic:
				DisconnectedCause = DisconnectCause.DisconnectByServerLogic;
				State = ClientState.Disconnecting;
				break;
			case StatusCode.DisconnectByServerReasonUnknown:
				DisconnectedCause = DisconnectCause.DisconnectByServerReasonUnknown;
				State = ClientState.Disconnecting;
				break;
			case StatusCode.TimeoutDisconnect:
				SystemConnectionSummary = new SystemConnectionSummary(this);
				DisconnectedCause = DisconnectCause.ClientTimeout;
				if (AppSettings.EnableProtocolFallback && (State == ClientState.ConnectingToNameServer || State == ClientState.ConnectingToMasterServer) && RealtimePeer.UsedProtocol != ConnectionProtocol.WebSocketSecure)
				{
					State = ClientState.ConnectWithFallbackProtocol;
				}
				else
				{
					State = ClientState.Disconnecting;
				}
				break;
			case (StatusCode)1027:
			case (StatusCode)1028:
			case (StatusCode)1029:
			case (StatusCode)1031:
			case (StatusCode)1032:
			case (StatusCode)1033:
			case (StatusCode)1034:
			case (StatusCode)1035:
			case (StatusCode)1036:
			case (StatusCode)1037:
			case (StatusCode)1038:
			case (StatusCode)1045:
			case (StatusCode)1046:
			case (StatusCode)1047:
				break;
			}
		}

		public virtual void OnEvent(EventData photonEvent)
		{
			int sender = photonEvent.Sender;
			Player player = ((CurrentRoom != null) ? CurrentRoom.GetPlayer(sender) : null);
			switch (photonEvent.Code)
			{
			case 229:
			case 230:
			{
				List<RoomInfo> list = new List<RoomInfo>();
				PhotonHashtable photonHashtable2 = (PhotonHashtable)photonEvent[222];
				foreach (string key in photonHashtable2.Keys)
				{
					list.Add(new RoomInfo(key, (PhotonHashtable)photonHashtable2[key]));
				}
				LobbyCallbackTargets.OnRoomListUpdate(list);
				break;
			}
			case byte.MaxValue:
			{
				PhotonHashtable photonHashtable = (PhotonHashtable)photonEvent[249];
				if (player == null)
				{
					if (sender > 0)
					{
						player = new Player(string.Empty, sender, isLocal: false, photonHashtable);
						CurrentRoom.StorePlayer(player);
					}
				}
				else
				{
					player.InternalCacheProperties(photonHashtable);
					player.IsInactive = false;
					player.HasRejoined = sender != LocalPlayer.ActorNumber;
				}
				if (sender == LocalPlayer.ActorNumber)
				{
					int[] actorsInGame = (int[])photonEvent[252];
					UpdatedActorList(actorsInGame);
					player.HasRejoined = enterRoomArgumentsCache.JoinMode == JoinMode.RejoinOnly;
					State = ClientState.Joined;
					LocalPlayer.UpdateNickNameOnJoined();
					if (lastJoinType == JoinType.CreateRoom || (lastJoinType == JoinType.JoinOrCreateRoom && LocalPlayer.ActorNumber == 1))
					{
						MatchMakingCallbackTargets.OnCreatedRoom();
					}
					MatchMakingCallbackTargets.OnJoinedRoom();
				}
				else
				{
					InRoomCallbackTargets.OnPlayerEnteredRoom(player);
				}
				break;
			}
			case 254:
				if (player != null)
				{
					bool flag = false;
					if (photonEvent.Parameters.ContainsKey(233))
					{
						flag = (bool)photonEvent.Parameters[233];
					}
					player.IsInactive = flag;
					player.HasRejoined = false;
					if (!flag)
					{
						CurrentRoom.RemovePlayer(sender);
					}
				}
				if (photonEvent.Parameters.ContainsKey(203))
				{
					int num = (int)photonEvent[203];
					if (num != 0)
					{
						CurrentRoom.MasterClientId = num;
						InRoomCallbackTargets.OnMasterClientSwitched(CurrentRoom.GetPlayer(num));
					}
				}
				InRoomCallbackTargets.OnPlayerLeftRoom(player);
				break;
			case 253:
			{
				int num2 = 0;
				if (photonEvent.Parameters.ContainsKey(253))
				{
					num2 = (int)photonEvent[253];
				}
				PhotonHashtable gameProperties = null;
				PhotonHashtable actorProperties = null;
				if (num2 == 0)
				{
					gameProperties = (PhotonHashtable)photonEvent[251];
				}
				else
				{
					actorProperties = (PhotonHashtable)photonEvent[251];
				}
				ReadoutProperties(gameProperties, actorProperties, num2);
				break;
			}
			case 226:
				PlayersInRoomsCount = (int)photonEvent[229];
				RoomsCount = (int)photonEvent[228];
				PlayersOnMasterCount = (int)photonEvent[227];
				break;
			case 224:
			{
				string[] array = photonEvent[213] as string[];
				int[] array2 = photonEvent[229] as int[];
				int[] array3 = photonEvent[228] as int[];
				ByteArraySlice byteArraySlice = photonEvent[212] as ByteArraySlice;
				bool flag2 = byteArraySlice != null;
				byte[] array4 = ((!flag2) ? (photonEvent[212] as byte[]) : byteArraySlice.Buffer);
				lobbyStatistics.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					TypedLobbyInfo item = new TypedLobbyInfo(array[i], (LobbyType)array4[i], array2[i], array3[i]);
					lobbyStatistics.Add(item);
				}
				if (flag2)
				{
					byteArraySlice.Release();
				}
				LobbyCallbackTargets.OnLobbyStatisticsUpdate(lobbyStatistics);
				break;
			}
			case 251:
				ErrorInfoCallbackTargets.OnErrorInfo(new ErrorInfo(photonEvent));
				break;
			case 223:
				if (AuthValues == null)
				{
					AuthValues = new AuthenticationValues();
				}
				AuthValues.Token = photonEvent[221];
				break;
			}
			UpdateCallbackTargets();
			this.EventReceived?.Invoke(photonEvent);
		}

		public virtual void OnMessage(bool isRawMessage, object message)
		{
			UpdateCallbackTargets();
			this.MessageReceived?.Invoke(isRawMessage, message);
		}

		public void OnDisconnectMessage(DisconnectMessage obj)
		{
			Log.Error($"OnDisconnectMessage. Code: {obj.Code} Msg: \"{obj.DebugMessage}\". Debug Info: {obj.Parameters}", LogLevel, LogPrefix);
			Disconnect(DisconnectCause.DisconnectByDisconnectMessage);
		}

		private void OnRegionPingCompleted(RegionHandler regionHandler)
		{
			if (LogLevel != LogLevel.Info)
			{
				_ = LogLevel;
				_ = 4;
			}
			if (clientWorkflow == ClientWorkflowOption.GetRegionsAndPing)
			{
				Disconnect();
				ConnectionCallbackTargets.OnRegionListReceived(regionHandler);
				return;
			}
			if (regionHandler.BestRegion != null)
			{
				CurrentRegion = regionHandler.BestRegion.Code;
			}
			if (State == ClientState.ConnectedToNameServer)
			{
				CallAuthenticate();
			}
			else
			{
				CallConnect(ServerConnection.NameServer);
			}
		}

		private void SetupEncryption(Dictionary<byte, object> encryptionData)
		{
			switch ((EncryptionMode)(byte)encryptionData[0])
			{
			case EncryptionMode.PayloadEncryption:
			{
				byte[] secret = (byte[])encryptionData[1];
				RealtimePeer.InitPayloadEncryption(secret);
				break;
			}
			case EncryptionMode.DatagramEncryptionGCM:
			{
				byte[] encryptionSecret = (byte[])encryptionData[1];
				RealtimePeer.InitDatagramEncryption(encryptionSecret, null);
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void AddCallbackTarget(object target)
		{
			callbackTargetChanges.Enqueue(new CallbackTargetChange(target, addTarget: true));
		}

		public void RemoveCallbackTarget(object target)
		{
			callbackTargetChanges.Enqueue(new CallbackTargetChange(target, addTarget: false));
		}

		protected internal void UpdateCallbackTargets()
		{
			while (callbackTargetChanges.Count > 0)
			{
				CallbackTargetChange callbackTargetChange = callbackTargetChanges.Dequeue();
				if (callbackTargetChange.AddTarget)
				{
					if (callbackTargets.Contains(callbackTargetChange.Target))
					{
						continue;
					}
					callbackTargets.Add(callbackTargetChange.Target);
				}
				else
				{
					if (!callbackTargets.Contains(callbackTargetChange.Target))
					{
						continue;
					}
					callbackTargets.Remove(callbackTargetChange.Target);
				}
				UpdateCallbackTarget(callbackTargetChange, InRoomCallbackTargets);
				UpdateCallbackTarget(callbackTargetChange, ConnectionCallbackTargets);
				UpdateCallbackTarget(callbackTargetChange, MatchMakingCallbackTargets);
				UpdateCallbackTarget(callbackTargetChange, LobbyCallbackTargets);
				UpdateCallbackTarget(callbackTargetChange, ErrorInfoCallbackTargets);
				if (callbackTargetChange.Target is IOnEventCallback onEventCallback)
				{
					if (callbackTargetChange.AddTarget)
					{
						EventReceived += onEventCallback.OnEvent;
					}
					else
					{
						EventReceived -= onEventCallback.OnEvent;
					}
				}
				if (callbackTargetChange.Target is IOnMessageCallback onMessageCallback)
				{
					if (callbackTargetChange.AddTarget)
					{
						MessageReceived += onMessageCallback.OnMessage;
					}
					else
					{
						MessageReceived -= onMessageCallback.OnMessage;
					}
				}
			}
		}

		private void UpdateCallbackTarget<T>(CallbackTargetChange change, List<T> container) where T : class
		{
			if (change.Target is T item)
			{
				if (change.AddTarget)
				{
					container.Add(item);
				}
				else
				{
					container.Remove(item);
				}
			}
		}

		public virtual bool OpGetRegions()
		{
			if (!CheckIfOpCanBeSent(220, Server, "GetRegions"))
			{
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			parameterDictionary[224] = AppSettings.GetAppId(ClientType);
			bool result = RealtimePeer.SendOperation(220, parameterDictionary, new SendOptions
			{
				Reliability = true,
				Encrypt = true
			});
			paramDictionaryPool.Release(parameterDictionary);
			return result;
		}

		public bool OpFindFriends(string[] friendsToFind, FindFriendsArgs args = null)
		{
			if (!CheckIfOpCanBeSent(222, Server, "FindFriends"))
			{
				return false;
			}
			if (IsFetchingFriendList)
			{
				return false;
			}
			if (friendsToFind == null || friendsToFind.Length == 0)
			{
				Log.Error("OpFindFriends skipped: friendsToFind array is null or empty.", LogLevel, LogPrefix);
				return false;
			}
			if (friendsToFind.Length > 512)
			{
				Log.Error($"OpFindFriends skipped: friendsToFind array exceeds allowed length of {512}.", LogLevel, LogPrefix);
				return false;
			}
			List<string> list = new List<string>(friendsToFind.Length);
			foreach (string text in friendsToFind)
			{
				if (!string.IsNullOrEmpty(text) && !text.Equals(UserId) && !list.Contains(text))
				{
					list.Add(text);
				}
			}
			if (list.Count == 0)
			{
				Log.Error("OpFindFriends failed. No friends to find (check warnings).", LogLevel, LogPrefix);
				return false;
			}
			string[] array = list.ToArray();
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			if (array != null && array.Length != 0)
			{
				parameterDictionary[1] = array;
			}
			if (args != null)
			{
				parameterDictionary[2] = args.ToIntFlags();
			}
			SendOptions sendOptions = new SendOptions
			{
				Reliability = true,
				Encrypt = true
			};
			bool flag = RealtimePeer.SendOperation(222, parameterDictionary, sendOptions);
			paramDictionaryPool.Release(parameterDictionary);
			friendListRequested = (flag ? array : null);
			return flag;
		}

		public virtual bool OpJoinLobby(TypedLobby lobby = null)
		{
			if (!CheckIfOpCanBeSent(229, Server, "JoinLobby"))
			{
				return false;
			}
			if (lobby == null)
			{
				lobby = TypedLobby.Default;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			if (lobby != null && !lobby.IsDefault)
			{
				parameterDictionary[213] = lobby.Name;
				parameterDictionary[212] = (byte)lobby.Type;
			}
			bool num = RealtimePeer.SendOperation(229, parameterDictionary, SendOptions.SendReliable);
			paramDictionaryPool.Release(parameterDictionary);
			if (num)
			{
				targetLobbyCache = lobby;
				State = ClientState.JoiningLobby;
			}
			return num;
		}

		public bool OpLeaveLobby()
		{
			if (!CheckIfOpCanBeSent(228, Server, "LeaveLobby"))
			{
				return false;
			}
			return RealtimePeer.SendOperation(228, null, SendOptions.SendReliable);
		}

		private void RoomOptionsToOpParameters(ParameterDictionary op, RoomOptions roomOptions, bool usePropertiesKey = false)
		{
			if (roomOptions == null)
			{
				roomOptions = new RoomOptions();
			}
			PhotonHashtable photonHashtable = new PhotonHashtable();
			photonHashtable[(byte)253] = roomOptions.IsOpen;
			photonHashtable[(byte)254] = roomOptions.IsVisible;
			photonHashtable[(byte)250] = ((roomOptions.CustomRoomPropertiesForLobby == null) ? new object[0] : roomOptions.CustomRoomPropertiesForLobby);
			photonHashtable.Merge(roomOptions.CustomRoomProperties);
			if (roomOptions.MaxPlayers > 0)
			{
				byte b = (byte)((roomOptions.MaxPlayers <= 255) ? ((byte)roomOptions.MaxPlayers) : 0);
				photonHashtable[byte.MaxValue] = b;
				photonHashtable[(byte)243] = roomOptions.MaxPlayers;
			}
			if (!usePropertiesKey)
			{
				op[248] = photonHashtable;
			}
			else
			{
				op[251] = photonHashtable;
			}
			int num = 0;
			if (roomOptions.CleanupCacheOnLeave)
			{
				op[241] = true;
				num |= 2;
			}
			else
			{
				op[241] = false;
				photonHashtable[(byte)249] = false;
			}
			num |= 1;
			op[232] = true;
			if (roomOptions.PlayerTtl > 0 || roomOptions.PlayerTtl == -1)
			{
				op[235] = roomOptions.PlayerTtl;
			}
			if (roomOptions.EmptyRoomTtl > 0)
			{
				op[236] = roomOptions.EmptyRoomTtl;
			}
			if (roomOptions.SuppressRoomEvents)
			{
				num |= 4;
				op[237] = true;
			}
			if (roomOptions.SuppressPlayerInfo)
			{
				num |= 0x40;
			}
			if (roomOptions.Plugins != null)
			{
				op[204] = roomOptions.Plugins;
			}
			if (roomOptions.PublishUserId)
			{
				num |= 8;
				op[239] = true;
			}
			if (roomOptions.DeleteNullProperties)
			{
				num |= 0x10;
			}
			if (roomOptions.BroadcastPropsChangeToAll)
			{
				num |= 0x20;
			}
			op[191] = num;
		}

		public bool OpJoinRandomRoom(JoinRandomRoomArgs joinRandomRoomArgs = null)
		{
			if (!CheckIfOpCanBeSent(225, Server, "JoinRandomGame"))
			{
				return false;
			}
			if (joinRandomRoomArgs == null)
			{
				joinRandomRoomArgs = new JoinRandomRoomArgs();
			}
			if (!joinRandomRoomArgs.ExpectedCustomRoomProperties.CustomPropKeyTypesValid(NullOrZeroAccepted: true))
			{
				Log.Error("OpJoinRandomRoom() expected properties must use key type of string or int.", LogLevel, LogPrefix);
				return false;
			}
			PhotonHashtable photonHashtable = new PhotonHashtable();
			photonHashtable.Merge(joinRandomRoomArgs.ExpectedCustomRoomProperties);
			if (joinRandomRoomArgs.ExpectedMaxPlayers > 0)
			{
				byte b = (byte)((joinRandomRoomArgs.ExpectedMaxPlayers <= 255) ? ((byte)joinRandomRoomArgs.ExpectedMaxPlayers) : 0);
				photonHashtable[byte.MaxValue] = b;
				if (joinRandomRoomArgs.ExpectedMaxPlayers > 255)
				{
					photonHashtable[(byte)243] = joinRandomRoomArgs.ExpectedMaxPlayers;
				}
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			SendOptions sendOptions = new SendOptions
			{
				Reliability = true
			};
			if (photonHashtable.Count > 0)
			{
				parameterDictionary[248] = photonHashtable;
			}
			if (joinRandomRoomArgs.MatchingType != MatchmakingMode.FillRoom)
			{
				parameterDictionary[223] = (byte)joinRandomRoomArgs.MatchingType;
			}
			if (joinRandomRoomArgs.Lobby != null && !joinRandomRoomArgs.Lobby.IsDefault)
			{
				parameterDictionary[213] = joinRandomRoomArgs.Lobby.Name;
				parameterDictionary[212] = (byte)joinRandomRoomArgs.Lobby.Type;
			}
			if (!string.IsNullOrEmpty(joinRandomRoomArgs.SqlLobbyFilter))
			{
				parameterDictionary[245] = joinRandomRoomArgs.SqlLobbyFilter;
			}
			if (joinRandomRoomArgs.ExpectedUsers != null && joinRandomRoomArgs.ExpectedUsers.Length != 0)
			{
				parameterDictionary[238] = joinRandomRoomArgs.ExpectedUsers;
				sendOptions.Encrypt = true;
			}
			if (joinRandomRoomArgs.Ticket != null)
			{
				parameterDictionary[190] = joinRandomRoomArgs.Ticket;
			}
			parameterDictionary[188] = true;
			bool num = RealtimePeer.SendOperation(225, parameterDictionary, sendOptions);
			paramDictionaryPool.Release(parameterDictionary);
			if (num)
			{
				State = ClientState.Joining;
				lastJoinType = JoinType.JoinRandomRoom;
				enterRoomArgumentsCache = new EnterRoomArgs();
				enterRoomArgumentsCache.Lobby = ((CurrentLobby != null && !CurrentLobby.IsDefault && joinRandomRoomArgs.Lobby == null) ? CurrentLobby : joinRandomRoomArgs.Lobby);
				enterRoomArgumentsCache.ExpectedUsers = joinRandomRoomArgs.ExpectedUsers;
				if (joinRandomRoomArgs.Ticket != null)
				{
					enterRoomArgumentsCache.Ticket = joinRandomRoomArgs.Ticket;
				}
			}
			return num;
		}

		public bool OpJoinRandomOrCreateRoom(JoinRandomRoomArgs joinRandomRoomArgs = null, EnterRoomArgs createRoomArgs = null)
		{
			if (!CheckIfOpCanBeSent(225, Server, "OpJoinRandomOrCreateRoom"))
			{
				return false;
			}
			if (joinRandomRoomArgs == null)
			{
				joinRandomRoomArgs = new JoinRandomRoomArgs();
			}
			if (createRoomArgs == null)
			{
				createRoomArgs = new EnterRoomArgs();
			}
			if (!joinRandomRoomArgs.ExpectedCustomRoomProperties.CustomPropKeyTypesValid(NullOrZeroAccepted: true))
			{
				Log.Error("OpJoinRandomOrCreateRoom() expected properties must use key type of string or int.", LogLevel, LogPrefix);
				return false;
			}
			createRoomArgs.JoinMode = JoinMode.CreateIfNotExists;
			PhotonHashtable photonHashtable = new PhotonHashtable();
			photonHashtable.Merge(joinRandomRoomArgs.ExpectedCustomRoomProperties);
			if (joinRandomRoomArgs.ExpectedMaxPlayers > 0)
			{
				byte b = (byte)((joinRandomRoomArgs.ExpectedMaxPlayers <= 255) ? ((byte)joinRandomRoomArgs.ExpectedMaxPlayers) : 0);
				photonHashtable[byte.MaxValue] = b;
				if (joinRandomRoomArgs.ExpectedMaxPlayers > 255)
				{
					photonHashtable[(byte)243] = joinRandomRoomArgs.ExpectedMaxPlayers;
				}
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			SendOptions sendOptions = new SendOptions
			{
				Reliability = true
			};
			if (photonHashtable.Count > 0)
			{
				parameterDictionary[248] = photonHashtable;
			}
			if (joinRandomRoomArgs.MatchingType != MatchmakingMode.FillRoom)
			{
				parameterDictionary[223] = (byte)joinRandomRoomArgs.MatchingType;
			}
			if (joinRandomRoomArgs.Lobby != null && !joinRandomRoomArgs.Lobby.IsDefault)
			{
				parameterDictionary[213] = joinRandomRoomArgs.Lobby.Name;
				parameterDictionary[212] = (byte)joinRandomRoomArgs.Lobby.Type;
			}
			if (!string.IsNullOrEmpty(joinRandomRoomArgs.SqlLobbyFilter))
			{
				parameterDictionary[245] = joinRandomRoomArgs.SqlLobbyFilter;
			}
			if (joinRandomRoomArgs.ExpectedUsers != null && joinRandomRoomArgs.ExpectedUsers.Length != 0)
			{
				parameterDictionary[238] = joinRandomRoomArgs.ExpectedUsers;
				sendOptions.Encrypt = true;
			}
			if (joinRandomRoomArgs.Ticket != null)
			{
				parameterDictionary[190] = joinRandomRoomArgs.Ticket;
			}
			parameterDictionary[215] = (byte)1;
			parameterDictionary[188] = true;
			if (!string.IsNullOrEmpty(createRoomArgs.RoomName))
			{
				parameterDictionary[byte.MaxValue] = createRoomArgs.RoomName;
			}
			bool num = RealtimePeer.SendOperation(225, parameterDictionary, sendOptions);
			paramDictionaryPool.Release(parameterDictionary);
			if (num)
			{
				State = ClientState.Joining;
				lastJoinType = JoinType.JoinRandomOrCreateRoom;
				enterRoomArgumentsCache = EnterRoomArgs.ShallowCopyToNewArgs(createRoomArgs);
				enterRoomArgumentsCache.Lobby = ((CurrentLobby != null && !CurrentLobby.IsDefault && joinRandomRoomArgs.Lobby == null) ? CurrentLobby : joinRandomRoomArgs.Lobby);
				enterRoomArgumentsCache.ExpectedUsers = joinRandomRoomArgs.ExpectedUsers;
				if (joinRandomRoomArgs.Ticket != null)
				{
					enterRoomArgumentsCache.Ticket = joinRandomRoomArgs.Ticket;
				}
			}
			return num;
		}

		public bool OpCreateRoom(EnterRoomArgs enterRoomArgs)
		{
			if (!CheckIfOpCanBeSent(227, Server, "CreateGame"))
			{
				return false;
			}
			if (enterRoomArgs == null)
			{
				Log.Error("OpCreateRoom() failed. Parameter enterRoomArgs can not be null.", LogLevel, LogPrefix);
				return false;
			}
			if (!(enterRoomArgs.OnGameServer = Server == ServerConnection.GameServer))
			{
				enterRoomArgumentsCache = enterRoomArgs;
				enterRoomArgumentsCache.Lobby = ((CurrentLobby != null && !CurrentLobby.IsDefault && enterRoomArgs.Lobby == null) ? CurrentLobby : enterRoomArgs.Lobby);
			}
			bool num = OpCreateRoomIntern(enterRoomArgs);
			if (num)
			{
				lastJoinType = JoinType.CreateRoom;
				State = ClientState.Joining;
			}
			return num;
		}

		public bool OpJoinOrCreateRoom(EnterRoomArgs enterRoomArgs)
		{
			if (!CheckIfOpCanBeSent(226, Server, "JoinOrCreateRoom"))
			{
				return false;
			}
			if (enterRoomArgs == null)
			{
				Log.Error("OpJoinOrCreateRoom() failed. Parameter enterRoomArgs can not be null.", LogLevel, LogPrefix);
				return false;
			}
			bool flag = Server == ServerConnection.GameServer;
			enterRoomArgs.JoinMode = JoinMode.CreateIfNotExists;
			enterRoomArgs.OnGameServer = flag;
			if (!flag)
			{
				enterRoomArgumentsCache = enterRoomArgs;
				enterRoomArgumentsCache.Lobby = ((CurrentLobby != null && !CurrentLobby.IsDefault && enterRoomArgs.Lobby == null) ? CurrentLobby : enterRoomArgs.Lobby);
				if (enterRoomArgs.Ticket != null)
				{
					enterRoomArgumentsCache.Ticket = enterRoomArgs.Ticket;
				}
			}
			bool num = OpJoinRoomIntern(enterRoomArgs);
			if (num)
			{
				lastJoinType = JoinType.JoinOrCreateRoom;
				State = ClientState.Joining;
			}
			return num;
		}

		public bool OpJoinRoom(EnterRoomArgs enterRoomArgs)
		{
			if (!CheckIfOpCanBeSent(226, Server, "JoinRoom"))
			{
				return false;
			}
			if (enterRoomArgs == null)
			{
				Log.Error("OpJoinRoom() failed. Parameter enterRoomArgs can not be null.", LogLevel, LogPrefix);
				return false;
			}
			if (!(enterRoomArgs.OnGameServer = Server == ServerConnection.GameServer))
			{
				enterRoomArgumentsCache = enterRoomArgs;
				enterRoomArgumentsCache.Lobby = null;
			}
			bool num = OpJoinRoomIntern(enterRoomArgs);
			if (num)
			{
				lastJoinType = ((enterRoomArgs.JoinMode != JoinMode.CreateIfNotExists) ? JoinType.JoinRoom : JoinType.JoinOrCreateRoom);
				State = ClientState.Joining;
			}
			return num;
		}

		private bool OpCreateRoomIntern(EnterRoomArgs opArgs)
		{
			if (opArgs == null)
			{
				Log.Error("OpCreateRoom() failed. Parameter opArgs must be non null.", LogLevel, LogPrefix);
				return false;
			}
			if (opArgs.RoomOptions == null)
			{
				opArgs.RoomOptions = new RoomOptions();
			}
			if (!opArgs.RoomOptions.CustomRoomProperties.CustomPropKeyTypesValid(NullOrZeroAccepted: true))
			{
				Log.Error("OpCreateRoom() failed. Custom Room Properties contains key which is not string nor int.", LogLevel, LogPrefix);
				return false;
			}
			if (!opArgs.RoomOptions.CustomRoomPropertiesForLobby.CustomPropKeyTypesValid(NullOrZeroAccepted: true))
			{
				Log.Error("OpCreateRoom() failed. RoomOptions.CustomRoomPropertiesForLobby can be null, have zero items or all items must be int or string.", LogLevel, LogPrefix);
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			SendOptions sendOptions = new SendOptions
			{
				Reliability = true
			};
			if (!string.IsNullOrEmpty(opArgs.RoomName))
			{
				parameterDictionary[byte.MaxValue] = opArgs.RoomName;
			}
			if (opArgs.Lobby != null && !opArgs.Lobby.IsDefault)
			{
				parameterDictionary[213] = opArgs.Lobby.Name;
				parameterDictionary[212] = (byte)opArgs.Lobby.Type;
			}
			if (opArgs.ExpectedUsers != null && opArgs.ExpectedUsers.Length != 0)
			{
				parameterDictionary[238] = opArgs.ExpectedUsers;
				sendOptions.Encrypt = true;
			}
			if (opArgs.Ticket != null)
			{
				parameterDictionary[190] = opArgs.Ticket;
			}
			if (opArgs.OnGameServer)
			{
				if (LocalPlayer != null)
				{
					if (!string.IsNullOrEmpty(LocalPlayer.NickName))
					{
						if (LocalPlayer.CustomProperties == null)
						{
							LocalPlayer.CustomProperties = new PhotonHashtable();
						}
						LocalPlayer.CustomProperties[byte.MaxValue] = LocalPlayer.NickName;
					}
					if (LocalPlayer.CustomProperties != null && LocalPlayer.CustomProperties.Count > 0)
					{
						parameterDictionary[249] = LocalPlayer.CustomProperties;
					}
				}
				parameterDictionary[250] = true;
				RoomOptionsToOpParameters(parameterDictionary, opArgs.RoomOptions);
			}
			bool result = RealtimePeer.SendOperation(227, parameterDictionary, sendOptions);
			paramDictionaryPool.Release(parameterDictionary);
			return result;
		}

		private bool OpJoinRoomIntern(EnterRoomArgs opArgs)
		{
			if (opArgs == null)
			{
				Log.Error("OpJoinRoom() failed. Parameter opArgs must be non null.", LogLevel, LogPrefix);
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			SendOptions sendOptions = new SendOptions
			{
				Reliability = true
			};
			if (!string.IsNullOrEmpty(opArgs.RoomName))
			{
				parameterDictionary[byte.MaxValue] = opArgs.RoomName;
			}
			if (opArgs.JoinMode == JoinMode.CreateIfNotExists)
			{
				parameterDictionary[215] = (byte)1;
				if (opArgs.Lobby != null && !opArgs.Lobby.IsDefault)
				{
					parameterDictionary[213] = opArgs.Lobby.Name;
					parameterDictionary[212] = (byte)opArgs.Lobby.Type;
				}
			}
			else if (opArgs.JoinMode == JoinMode.RejoinOnly)
			{
				parameterDictionary[215] = (byte)3;
			}
			if (opArgs.ExpectedUsers != null && opArgs.ExpectedUsers.Length != 0)
			{
				parameterDictionary[238] = opArgs.ExpectedUsers;
				sendOptions.Encrypt = true;
			}
			if (opArgs.Ticket != null)
			{
				parameterDictionary[190] = opArgs.Ticket;
			}
			if (opArgs.OnGameServer)
			{
				if (LocalPlayer != null && opArgs.JoinMode != JoinMode.RejoinOnly)
				{
					if (!string.IsNullOrEmpty(LocalPlayer.NickName))
					{
						if (LocalPlayer.CustomProperties == null)
						{
							LocalPlayer.CustomProperties = new PhotonHashtable();
						}
						LocalPlayer.CustomProperties[byte.MaxValue] = LocalPlayer.NickName;
					}
					if (LocalPlayer.CustomProperties != null && LocalPlayer.CustomProperties.Count > 0)
					{
						parameterDictionary[249] = LocalPlayer.CustomProperties;
					}
				}
				parameterDictionary[250] = true;
				RoomOptionsToOpParameters(parameterDictionary, opArgs.RoomOptions);
			}
			bool result = RealtimePeer.SendOperation(226, parameterDictionary, sendOptions);
			paramDictionaryPool.Release(parameterDictionary);
			return result;
		}

		public virtual bool OpLeaveRoom(bool becomeInactive)
		{
			if (!CheckIfOpCanBeSent(254, Server, "LeaveRoom"))
			{
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			if (becomeInactive)
			{
				parameterDictionary[233] = true;
			}
			bool num = RealtimePeer.SendOperation(254, parameterDictionary, SendOptions.SendReliable);
			paramDictionaryPool.Release(parameterDictionary);
			if (num)
			{
				State = ClientState.Leaving;
				GameServerAddress = string.Empty;
				enterRoomArgumentsCache = null;
			}
			return num;
		}

		public bool OpRejoinRoom(string roomName, object ticket = null)
		{
			if (!CheckIfOpCanBeSent(226, Server, "RejoinRoom"))
			{
				return false;
			}
			bool onGameServer = Server == ServerConnection.GameServer;
			EnterRoomArgs enterRoomArgs = new EnterRoomArgs();
			enterRoomArgs.RoomName = roomName;
			enterRoomArgs.OnGameServer = onGameServer;
			enterRoomArgs.JoinMode = JoinMode.RejoinOnly;
			enterRoomArgs.Ticket = ticket;
			enterRoomArgumentsCache = enterRoomArgs;
			bool num = OpJoinRoomIntern(enterRoomArgs);
			if (num)
			{
				lastJoinType = JoinType.JoinRoom;
				State = ClientState.Joining;
			}
			return num;
		}

		public bool OpSetCustomPropertiesOfActor(int actorNr, PhotonHashtable propertiesToSet, PhotonHashtable expectedProperties = null)
		{
			if (!propertiesToSet.CustomPropKeyTypesValid())
			{
				Log.Error("OpSetCustomPropertiesOfActor() failed. Parameter propertiesToSet must be non-null, not empty and contain only int or string keys.", LogLevel, LogPrefix);
				return false;
			}
			if (CurrentRoom == null)
			{
				Log.Error("OpSetCustomPropertiesOfActor() failed because the client is not in a room. Use LocalPlayer.SetCustomProperties() to change this player's properties even while not in a room.", LogLevel, LogPrefix);
				return false;
			}
			return OpSetPropertiesOfActor(actorNr, propertiesToSet, expectedProperties);
		}

		protected internal bool OpSetPropertiesOfActor(int actorNr, PhotonHashtable actorProperties, PhotonHashtable expectedProperties = null)
		{
			if (!CheckIfOpCanBeSent(252, Server, "SetProperties"))
			{
				return false;
			}
			if (actorNr <= 0 || actorProperties == null || actorProperties.Count == 0)
			{
				Log.Error("OpSetPropertiesOfActor() failed. Parameter actorProperties must be non-null and not empty.", LogLevel, LogPrefix);
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			parameterDictionary.Add(251, actorProperties);
			parameterDictionary.Add(254, actorNr);
			parameterDictionary.Add(250, value: true);
			if (expectedProperties != null && expectedProperties.Count != 0)
			{
				parameterDictionary.Add(231, expectedProperties);
			}
			bool num = RealtimePeer.SendOperation(252, parameterDictionary, SendOptions.SendReliable);
			paramDictionaryPool.Release(parameterDictionary);
			if (num && !CurrentRoom.BroadcastPropertiesChangeToAll && (expectedProperties == null || expectedProperties.Count == 0))
			{
				Player player = CurrentRoom.GetPlayer(actorNr);
				if (player != null)
				{
					player.InternalCacheProperties(actorProperties);
					InRoomCallbackTargets.OnPlayerPropertiesUpdate(player, actorProperties);
				}
			}
			return num;
		}

		public bool OpSetCustomPropertiesOfRoom(PhotonHashtable propertiesToSet, PhotonHashtable expectedProperties = null)
		{
			if (!propertiesToSet.CustomPropKeyTypesValid())
			{
				Log.Error("OpSetCustomPropertiesOfRoom() failed. Parameter propertiesToSet must be non-null, not empty and contain only int or string keys.", LogLevel, LogPrefix);
				return false;
			}
			return OpSetPropertiesOfRoom(propertiesToSet, expectedProperties);
		}

		protected internal bool OpSetPropertyOfRoom(byte propCode, object value)
		{
			PhotonHashtable photonHashtable = new PhotonHashtable();
			photonHashtable[propCode] = value;
			return OpSetPropertiesOfRoom(photonHashtable);
		}

		protected internal bool OpSetPropertiesOfRoom(PhotonHashtable gameProperties, PhotonHashtable expectedProperties = null)
		{
			if (!CheckIfOpCanBeSent(252, Server, "SetProperties"))
			{
				return false;
			}
			if (gameProperties == null || gameProperties.Count == 0)
			{
				Log.Error("OpSetPropertiesOfRoom() failed. Parameter gameProperties must not be null nor empty.", LogLevel, LogPrefix);
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			parameterDictionary.Add(251, gameProperties);
			parameterDictionary.Add(250, value: true);
			if (expectedProperties != null && expectedProperties.Count != 0)
			{
				parameterDictionary.Add(231, expectedProperties);
			}
			bool num = RealtimePeer.SendOperation(252, parameterDictionary, SendOptions.SendReliable);
			paramDictionaryPool.Release(parameterDictionary);
			if (num && !CurrentRoom.BroadcastPropertiesChangeToAll && (expectedProperties == null || expectedProperties.Count == 0))
			{
				CurrentRoom.InternalCacheProperties(gameProperties);
				InRoomCallbackTargets.OnRoomPropertiesUpdate(gameProperties);
			}
			return num;
		}

		public virtual bool OpChangeGroups(byte[] groupsToRemove, byte[] groupsToAdd)
		{
			if (!CheckIfOpCanBeSent(248, Server, "ChangeGroups"))
			{
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			if (groupsToRemove != null)
			{
				parameterDictionary[239] = groupsToRemove;
			}
			if (groupsToAdd != null)
			{
				parameterDictionary[238] = groupsToAdd;
			}
			bool result = RealtimePeer.SendOperation(248, parameterDictionary, SendOptions.SendReliable);
			paramDictionaryPool.Release(parameterDictionary);
			return result;
		}

		public virtual bool OpGetGameList(TypedLobby lobby, string queryData)
		{
			if (!CheckIfOpCanBeSent(217, Server, "GetGameList"))
			{
				return false;
			}
			if (string.IsNullOrEmpty(queryData))
			{
				Log.Error("Operation GetGameList requires a filter (queryData).", LogLevel, LogPrefix);
				return false;
			}
			if (lobby == null || lobby.Type != LobbyType.Sql || lobby.IsDefault)
			{
				Log.Error("Operation GetGameList can only be used for named lobbies of type SqlLobby.", LogLevel, LogPrefix);
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			parameterDictionary[213] = lobby.Name;
			parameterDictionary[212] = (byte)lobby.Type;
			parameterDictionary[245] = queryData;
			bool result = RealtimePeer.SendOperation(217, parameterDictionary, SendOptions.SendReliable);
			paramDictionaryPool.Release(parameterDictionary);
			return result;
		}

		public bool OpSetCustomPropertiesOfActor(int actorNr, PhotonHashtable actorProperties)
		{
			if (!actorProperties.CustomPropKeyTypesValid())
			{
				Log.Error("For OpSetCustomPropertiesOfActor the actorProperties can only contain keys of type string.", LogLevel, LogPrefix);
				return false;
			}
			return OpSetPropertiesOfActor(actorNr, actorProperties);
		}

		public bool OpSetCustomPropertiesOfRoom(PhotonHashtable gameProperties)
		{
			if (!gameProperties.CustomPropKeyTypesValid())
			{
				Log.Error("For OpSetCustomPropertiesOfRoom the gameProperties can only contain keys of type string.", LogLevel, LogPrefix);
				return false;
			}
			return OpSetPropertiesOfRoom(gameProperties);
		}

		public virtual bool OpAuthenticate(string appId, string appVersion, AuthenticationValues authValues, string regionCode, bool getLobbyStatistics)
		{
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			if (getLobbyStatistics)
			{
				parameterDictionary[211] = true;
			}
			if (authValues != null && authValues.Token != null)
			{
				parameterDictionary[221] = authValues.Token;
				bool result = RealtimePeer.SendOperation(230, parameterDictionary, SendOptions.SendReliable);
				paramDictionaryPool.Release(parameterDictionary);
				return result;
			}
			parameterDictionary[220] = appVersion;
			parameterDictionary[224] = appId;
			parameterDictionary[210] = regionCode;
			if (authValues != null)
			{
				if (!string.IsNullOrEmpty(authValues.UserId))
				{
					parameterDictionary[225] = authValues.UserId;
				}
				if (authValues.AuthType != CustomAuthenticationType.None)
				{
					parameterDictionary[217] = (byte)authValues.AuthType;
					if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
					{
						parameterDictionary[216] = authValues.AuthGetParameters;
					}
					if (authValues.AuthPostData != null)
					{
						parameterDictionary[214] = authValues.AuthPostData;
					}
				}
			}
			bool result2 = RealtimePeer.SendOperation(230, parameterDictionary, new SendOptions
			{
				Reliability = true,
				Encrypt = true
			});
			paramDictionaryPool.Release(parameterDictionary);
			return result2;
		}

		public virtual bool OpAuthenticateOnce(string appId, string appVersion, AuthenticationValues authValues, string regionCode, EncryptionMode encryptionMode, ConnectionProtocol expectedProtocol)
		{
			if (encryptionMode == EncryptionMode.DatagramEncryptionGCM && expectedProtocol != ConnectionProtocol.Udp)
			{
				Log.Error($"OpAuthenticateOnce() failed. Can not use EncryptionMode '{encryptionMode}' on protocol {expectedProtocol}", LogLevel, LogPrefix);
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			if (authValues != null && authValues.Token != null)
			{
				parameterDictionary[221] = authValues.Token;
				bool result = RealtimePeer.SendOperation(231, parameterDictionary, SendOptions.SendReliable);
				paramDictionaryPool.Release(parameterDictionary);
				return result;
			}
			parameterDictionary[195] = (byte)expectedProtocol;
			parameterDictionary[193] = (byte)encryptionMode;
			parameterDictionary[220] = appVersion;
			parameterDictionary[224] = appId;
			parameterDictionary[210] = regionCode;
			if (authValues != null)
			{
				if (!string.IsNullOrEmpty(authValues.UserId))
				{
					parameterDictionary[225] = authValues.UserId;
				}
				if (authValues.AuthType != CustomAuthenticationType.None)
				{
					parameterDictionary[217] = (byte)authValues.AuthType;
					if (authValues.Token != null)
					{
						parameterDictionary[221] = authValues.Token;
					}
					else
					{
						if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
						{
							parameterDictionary[216] = authValues.AuthGetParameters;
						}
						if (authValues.AuthPostData != null)
						{
							parameterDictionary[214] = authValues.AuthPostData;
						}
					}
				}
			}
			bool result2 = RealtimePeer.SendOperation(231, parameterDictionary, new SendOptions
			{
				Reliability = true,
				Encrypt = true
			});
			paramDictionaryPool.Release(parameterDictionary);
			return result2;
		}

		public virtual bool OpRaiseEvent(byte eventCode, object customEventContent, RaiseEventArgs raiseEventArgs, SendOptions sendOptions)
		{
			if (!CheckIfOpCanBeSent(253, Server, "RaiseEvent"))
			{
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			try
			{
				if (raiseEventArgs.CachingOption != EventCaching.DoNotCache)
				{
					parameterDictionary.Add(247, (byte)raiseEventArgs.CachingOption);
				}
				switch (raiseEventArgs.CachingOption)
				{
				case EventCaching.SliceSetIndex:
				case EventCaching.SlicePurgeIndex:
				case EventCaching.SlicePurgeUpToIndex:
					return RealtimePeer.SendOperation(253, parameterDictionary, sendOptions);
				case EventCaching.RemoveFromRoomCacheForActorsLeft:
				case EventCaching.SliceIncreaseIndex:
					return RealtimePeer.SendOperation(253, parameterDictionary, sendOptions);
				case EventCaching.RemoveFromRoomCache:
					if (raiseEventArgs.TargetActors != null)
					{
						parameterDictionary.Add(252, raiseEventArgs.TargetActors);
					}
					break;
				default:
					if (raiseEventArgs.TargetActors != null)
					{
						parameterDictionary.Add(252, raiseEventArgs.TargetActors);
					}
					else if (raiseEventArgs.InterestGroup != 0)
					{
						parameterDictionary.Add(240, raiseEventArgs.InterestGroup);
					}
					else if (raiseEventArgs.Receivers != ReceiverGroup.Others)
					{
						parameterDictionary.Add(246, (byte)raiseEventArgs.Receivers);
					}
					break;
				}
				parameterDictionary.Add(244, eventCode);
				if (customEventContent != null)
				{
					parameterDictionary.Add(245, customEventContent);
				}
				return RealtimePeer.SendOperation(253, parameterDictionary, sendOptions);
			}
			finally
			{
				paramDictionaryPool.Release(parameterDictionary);
			}
		}

		public bool OpCreateMatchmakingTicket(int[] actorsToInclude)
		{
			if (!CheckIfOpCanBeSent(253, Server, "OpCreateMatchmakingTicket (RaiseEvent())"))
			{
				return false;
			}
			int[] value = new int[1] { LocalPlayer.ActorNumber };
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			try
			{
				object[] value2 = new object[2]
				{
					(byte)1,
					actorsToInclude
				};
				parameterDictionary.Add(244, EventCode.CommandEvent);
				parameterDictionary.Add(252, value);
				parameterDictionary.Add(245, value2);
				return RealtimePeer.SendOperation(253, parameterDictionary, SendOptions.SendReliable);
			}
			finally
			{
				paramDictionaryPool.Release(parameterDictionary);
			}
		}

		protected internal bool OpSettings(bool receiveLobbyStats)
		{
			if (!receiveLobbyStats)
			{
				return false;
			}
			if (!IsConnectedAndReady || Server != ServerConnection.MasterServer)
			{
				return false;
			}
			ParameterDictionary parameterDictionary = paramDictionaryPool.Acquire();
			parameterDictionary[0] = receiveLobbyStats;
			bool result = RealtimePeer.SendOperation(218, parameterDictionary, SendOptions.SendReliable);
			paramDictionaryPool.Release(parameterDictionary);
			return result;
		}
	}
}
