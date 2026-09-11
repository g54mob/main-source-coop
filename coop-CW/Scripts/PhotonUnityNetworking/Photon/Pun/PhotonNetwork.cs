using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Photon.Pun
{
	public static class PhotonNetwork
	{
		private struct RaiseEventBatch : IEquatable<RaiseEventBatch>
		{
			public byte Group;

			public bool Reliable;

			public override int GetHashCode()
			{
				return (Group << 1) + (Reliable ? 1 : 0);
			}

			public bool Equals(RaiseEventBatch other)
			{
				if (Reliable == other.Reliable)
				{
					return Group == other.Group;
				}
				return false;
			}
		}

		private class SerializeViewBatch : IEquatable<SerializeViewBatch>, IEquatable<RaiseEventBatch>
		{
			public readonly RaiseEventBatch Batch;

			public List<object> ObjectUpdates;

			private int defaultSize = ObjectsInOneUpdate;

			private int offset;

			public SerializeViewBatch(RaiseEventBatch batch, int offset)
			{
				Batch = batch;
				ObjectUpdates = new List<object>(defaultSize);
				this.offset = offset;
				for (int i = 0; i < offset; i++)
				{
					ObjectUpdates.Add(null);
				}
			}

			public override int GetHashCode()
			{
				return (Batch.Group << 1) + (Batch.Reliable ? 1 : 0);
			}

			public bool Equals(SerializeViewBatch other)
			{
				return Equals(other.Batch);
			}

			public bool Equals(RaiseEventBatch other)
			{
				if (Batch.Reliable == other.Reliable)
				{
					return Batch.Group == other.Group;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is SerializeViewBatch serializeViewBatch)
				{
					return Batch.Equals(serializeViewBatch.Batch);
				}
				return false;
			}

			public void Clear()
			{
				ObjectUpdates.Clear();
				for (int i = 0; i < offset; i++)
				{
					ObjectUpdates.Add(null);
				}
			}

			public void Add(List<object> viewData)
			{
				if (ObjectUpdates.Count >= ObjectUpdates.Capacity)
				{
					throw new Exception("Can't add. Size exceeded.");
				}
				ObjectUpdates.Add(viewData);
			}
		}

		public const string PunVersion = "2.43";

		private static string gameVersion;

		public static LoadBalancingClient NetworkingClient;

		public static readonly int MAX_VIEW_IDS;

		public const string ServerSettingsFileName = "PhotonServerSettings";

		private static ServerSettings photonServerSettings;

		private const string PlayerPrefsKey = "PUNCloudBestRegion";

		public static ConnectMethod ConnectMethod;

		public static PunLogLevel LogLevel;

		public static bool EnableCloseConnection;

		public static float PrecisionForVectorSynchronization;

		public static float PrecisionForQuaternionSynchronization;

		public static float PrecisionForFloatSynchronization;

		private static bool offlineMode;

		private static Room offlineModeRoom;

		private static bool automaticallySyncScene;

		private static int sendFrequency;

		private static int serializationFrequency;

		private static bool isMessageQueueRunning;

		private static double frametime;

		private static int frame;

		private static Stopwatch StartupStopwatch;

		public static float MinimalTimeScaleToDispatchInFixedUpdate;

		private static int lastUsedViewSubId;

		private static int lastUsedViewSubIdStatic;

		private static readonly HashSet<string> PrefabsWithoutMagicCallback;

		private static DisableIntercept m_DisableIntercept;

		private static readonly ExitGames.Client.Photon.Hashtable SendInstantiateEvHashtable;

		private static readonly RaiseEventOptions SendInstantiateRaiseEventOptions;

		private static HashSet<byte> allowedReceivingGroups;

		private static HashSet<byte> blockedSendingGroups;

		private static HashSet<PhotonView> reusablePVHashset;

		private static NonAllocDictionary<int, PhotonView> photonViewList;

		internal static byte currentLevelPrefix;

		public static bool loadingLevelAndPausedNetwork;

		internal const string CurrentSceneProperty = "curScn";

		internal const string CurrentScenePropertyLoadAsync = "curScnLa";

		private static IPunPrefabPool prefabPool;

		public static bool UseRpcMonoBehaviourCache;

		private static readonly Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache;

		private static Dictionary<string, int> rpcShortcuts;

		public static bool RunRpcCoroutines;

		private static AsyncOperation _AsyncLevelLoadingOperation;

		private static float _levelLoadingProgress;

		private static readonly Type typePunRPC;

		private static readonly Type typePhotonMessageInfo;

		private static readonly object keyByteZero;

		private static readonly object keyByteOne;

		private static readonly object keyByteTwo;

		private static readonly object keyByteThree;

		private static readonly object keyByteFour;

		private static readonly object keyByteFive;

		private static readonly object keyByteSix;

		private static readonly object keyByteSeven;

		private static readonly object keyByteEight;

		private static readonly object[] emptyObjectArray;

		private static readonly Type[] emptyTypeArray;

		internal static List<PhotonView> foundPVs;

		private static readonly ExitGames.Client.Photon.Hashtable removeFilter;

		private static readonly ExitGames.Client.Photon.Hashtable ServerCleanDestroyEvent;

		private static readonly RaiseEventOptions ServerCleanOptions;

		internal static RaiseEventOptions SendToAllOptions;

		internal static RaiseEventOptions SendToOthersOptions;

		internal static RaiseEventOptions SendToSingleOptions;

		private static readonly ExitGames.Client.Photon.Hashtable rpcFilterByViewId;

		private static readonly RaiseEventOptions OpCleanRpcBufferOptions;

		private static ExitGames.Client.Photon.Hashtable rpcEvent;

		private static RaiseEventOptions RpcOptionsToAll;

		public static int ObjectsInOneUpdate;

		private static readonly PhotonStream serializeStreamOut;

		private static readonly PhotonStream serializeStreamIn;

		private static RaiseEventOptions serializeRaiseEvOptions;

		private static readonly Dictionary<RaiseEventBatch, SerializeViewBatch> serializeViewBatches;

		public const int SyncViewId = 0;

		public const int SyncCompressed = 1;

		public const int SyncNullValues = 2;

		public const int SyncFirstValue = 3;

		private static RegionHandler _cachedRegionHandler;

		public static string GameVersion
		{
			get
			{
				return gameVersion;
			}
			set
			{
				gameVersion = value;
				NetworkingClient.AppVersion = string.Format("{0}_{1}", value, "2.43");
			}
		}

		public static string AppVersion => NetworkingClient.AppVersion;

		public static ServerSettings PhotonServerSettings
		{
			get
			{
				if (photonServerSettings == null)
				{
					LoadOrCreateSettings();
				}
				return photonServerSettings;
			}
			private set
			{
				photonServerSettings = value;
			}
		}

		public static string ServerAddress
		{
			get
			{
				if (NetworkingClient == null)
				{
					return "<not connected>";
				}
				return NetworkingClient.CurrentServerAddress;
			}
		}

		public static string CloudRegion
		{
			get
			{
				if (NetworkingClient == null || !IsConnected || Server == ServerConnection.NameServer)
				{
					return null;
				}
				return NetworkingClient.CloudRegion;
			}
		}

		public static string CurrentCluster
		{
			get
			{
				if (NetworkingClient == null)
				{
					return null;
				}
				return NetworkingClient.CurrentCluster;
			}
		}

		public static string BestRegionSummaryInPreferences
		{
			get
			{
				return PlayerPrefs.GetString("PUNCloudBestRegion", null);
			}
			internal set
			{
				if (string.IsNullOrEmpty(value))
				{
					PlayerPrefs.DeleteKey("PUNCloudBestRegion");
				}
				else
				{
					PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
				}
			}
		}

		public static bool IsConnected
		{
			get
			{
				if (OfflineMode)
				{
					return true;
				}
				if (NetworkingClient == null)
				{
					return false;
				}
				return NetworkingClient.IsConnected;
			}
		}

		public static bool IsConnectedAndReady
		{
			get
			{
				if (OfflineMode)
				{
					return true;
				}
				if (NetworkingClient == null)
				{
					return false;
				}
				return NetworkingClient.IsConnectedAndReady;
			}
		}

		public static ClientState NetworkClientState
		{
			get
			{
				if (OfflineMode)
				{
					if (offlineModeRoom == null)
					{
						return ClientState.ConnectedToMasterServer;
					}
					return ClientState.Joined;
				}
				if (NetworkingClient == null)
				{
					return ClientState.Disconnected;
				}
				return NetworkingClient.State;
			}
		}

		public static ServerConnection Server
		{
			get
			{
				if (OfflineMode)
				{
					if (CurrentRoom != null)
					{
						return ServerConnection.GameServer;
					}
					return ServerConnection.MasterServer;
				}
				if (NetworkingClient == null)
				{
					return ServerConnection.NameServer;
				}
				return NetworkingClient.Server;
			}
		}

		public static AuthenticationValues AuthValues
		{
			get
			{
				if (NetworkingClient == null)
				{
					return null;
				}
				return NetworkingClient.AuthValues;
			}
			set
			{
				if (NetworkingClient != null)
				{
					NetworkingClient.AuthValues = value;
				}
			}
		}

		public static TypedLobby CurrentLobby => NetworkingClient.CurrentLobby;

		public static Room CurrentRoom
		{
			get
			{
				if (offlineMode)
				{
					return offlineModeRoom;
				}
				if (NetworkingClient != null)
				{
					return NetworkingClient.CurrentRoom;
				}
				return null;
			}
		}

		public static Player LocalPlayer
		{
			get
			{
				if (NetworkingClient == null)
				{
					return null;
				}
				return NetworkingClient.LocalPlayer;
			}
		}

		public static string NickName
		{
			get
			{
				return NetworkingClient.NickName;
			}
			set
			{
				NetworkingClient.NickName = value;
			}
		}

		public static Player[] PlayerList
		{
			get
			{
				Room currentRoom = CurrentRoom;
				if (currentRoom != null)
				{
					return currentRoom.Players.Values.OrderBy((Player x) => x.ActorNumber).ToArray();
				}
				return new Player[0];
			}
		}

		public static Player[] PlayerListOthers
		{
			get
			{
				Room currentRoom = CurrentRoom;
				if (currentRoom != null)
				{
					return (from x in currentRoom.Players.Values
						orderby x.ActorNumber
						where !x.IsLocal
						select x).ToArray();
				}
				return new Player[0];
			}
		}

		public static bool OfflineMode
		{
			get
			{
				return offlineMode;
			}
			set
			{
				if (value == offlineMode)
				{
					return;
				}
				if (value && IsConnected)
				{
					UnityEngine.Debug.LogError("Can't start OFFLINE mode while connected!");
					return;
				}
				if (NetworkingClient.IsConnected)
				{
					NetworkingClient.Disconnect();
				}
				offlineMode = value;
				if (offlineMode)
				{
					NetworkingClient.ChangeLocalID(-1);
					NetworkingClient.ConnectionCallbackTargets.OnConnectedToMaster();
					return;
				}
				bool num = offlineModeRoom != null;
				if (num)
				{
					LeftRoomCleanup();
				}
				offlineModeRoom = null;
				NetworkingClient.CurrentRoom = null;
				NetworkingClient.ChangeLocalID(-1);
				if (num)
				{
					NetworkingClient.MatchMakingCallbackTargets.OnLeftRoom();
				}
			}
		}

		public static bool AutomaticallySyncScene
		{
			get
			{
				return automaticallySyncScene;
			}
			set
			{
				automaticallySyncScene = value;
				if (automaticallySyncScene && CurrentRoom != null)
				{
					LoadLevelIfSynced();
				}
			}
		}

		public static bool EnableLobbyStatistics => NetworkingClient.EnableLobbyStatistics;

		public static bool InLobby => NetworkingClient.InLobby;

		public static int SendRate
		{
			get
			{
				return 1000 / sendFrequency;
			}
			set
			{
				sendFrequency = 1000 / value;
				if (PhotonHandler.Instance != null)
				{
					PhotonHandler.Instance.UpdateInterval = sendFrequency;
				}
			}
		}

		public static int SerializationRate
		{
			get
			{
				return 1000 / serializationFrequency;
			}
			set
			{
				serializationFrequency = 1000 / value;
				if (PhotonHandler.Instance != null)
				{
					PhotonHandler.Instance.UpdateIntervalOnSerialize = serializationFrequency;
				}
			}
		}

		public static bool IsMessageQueueRunning
		{
			get
			{
				return isMessageQueueRunning;
			}
			set
			{
				isMessageQueueRunning = value;
			}
		}

		public static double Time
		{
			get
			{
				if (UnityEngine.Time.frameCount == frame)
				{
					return frametime;
				}
				frametime = (double)(uint)ServerTimestamp / 1000.0;
				frame = UnityEngine.Time.frameCount;
				return frametime;
			}
		}

		public static int ServerTimestamp
		{
			get
			{
				if (OfflineMode)
				{
					if (StartupStopwatch != null && StartupStopwatch.IsRunning)
					{
						return (int)StartupStopwatch.ElapsedMilliseconds;
					}
					return Environment.TickCount;
				}
				return NetworkingClient.LoadBalancingPeer.ServerTimeInMilliSeconds;
			}
		}

		public static float KeepAliveInBackground
		{
			get
			{
				if (!(PhotonHandler.Instance != null))
				{
					return 60f;
				}
				return Mathf.Round((float)PhotonHandler.Instance.KeepAliveInBackground / 1000f);
			}
			set
			{
				if (PhotonHandler.Instance != null)
				{
					PhotonHandler.Instance.KeepAliveInBackground = (int)Mathf.Round(value * 1000f);
				}
			}
		}

		public static bool IsMasterClient
		{
			get
			{
				if (OfflineMode)
				{
					return true;
				}
				if (NetworkingClient.CurrentRoom != null)
				{
					return NetworkingClient.CurrentRoom.MasterClientId == LocalPlayer.ActorNumber;
				}
				return false;
			}
		}

		public static Player MasterClient
		{
			get
			{
				if (OfflineMode)
				{
					return LocalPlayer;
				}
				if (NetworkingClient == null || NetworkingClient.CurrentRoom == null)
				{
					return null;
				}
				return NetworkingClient.CurrentRoom.GetPlayer(NetworkingClient.CurrentRoom.MasterClientId);
			}
		}

		public static bool InRoom => NetworkClientState == ClientState.Joined;

		public static int CountOfPlayersOnMaster => NetworkingClient.PlayersOnMasterCount;

		public static int CountOfPlayersInRooms => NetworkingClient.PlayersInRoomsCount;

		public static int CountOfPlayers => NetworkingClient.PlayersInRoomsCount + NetworkingClient.PlayersOnMasterCount;

		public static int CountOfRooms => NetworkingClient.RoomsCount;

		public static bool NetworkStatisticsEnabled
		{
			get
			{
				return NetworkingClient.LoadBalancingPeer.TrafficStatsEnabled;
			}
			set
			{
				NetworkingClient.LoadBalancingPeer.TrafficStatsEnabled = value;
			}
		}

		public static int ResentReliableCommands => NetworkingClient.LoadBalancingPeer.ResentReliableCommands;

		public static bool CrcCheckEnabled
		{
			get
			{
				return NetworkingClient.LoadBalancingPeer.CrcEnabled;
			}
			set
			{
				if (!IsConnected)
				{
					NetworkingClient.LoadBalancingPeer.CrcEnabled = value;
				}
				else
				{
					UnityEngine.Debug.Log("Can't change CrcCheckEnabled while being connected. CrcCheckEnabled stays " + NetworkingClient.LoadBalancingPeer.CrcEnabled);
				}
			}
		}

		public static int PacketLossByCrcCheck => NetworkingClient.LoadBalancingPeer.PacketLossByCrc;

		public static int MaxResendsBeforeDisconnect
		{
			get
			{
				return NetworkingClient.LoadBalancingPeer.SentCountAllowance;
			}
			set
			{
				if (value < 3)
				{
					value = 3;
				}
				if (value > 10)
				{
					value = 10;
				}
				NetworkingClient.LoadBalancingPeer.SentCountAllowance = value;
			}
		}

		public static int QuickResends
		{
			get
			{
				return NetworkingClient.LoadBalancingPeer.QuickResendAttempts;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 3)
				{
					value = 3;
				}
				NetworkingClient.LoadBalancingPeer.QuickResendAttempts = (byte)value;
			}
		}

		[Obsolete("Set port overrides in ServerPortOverrides. Not used anymore!")]
		public static bool UseAlternativeUdpPorts { get; set; }

		public static PhotonPortDefinition ServerPortOverrides
		{
			get
			{
				if (NetworkingClient != null)
				{
					return NetworkingClient.ServerPortOverrides;
				}
				return default(PhotonPortDefinition);
			}
			set
			{
				if (NetworkingClient != null)
				{
					NetworkingClient.ServerPortOverrides = value;
				}
			}
		}

		[Obsolete("Use PhotonViewCollection instead for an iterable collection of current photonViews.")]
		public static PhotonView[] PhotonViews
		{
			get
			{
				PhotonView[] array = new PhotonView[photonViewList.Count];
				int num = 0;
				foreach (PhotonView value in photonViewList.Values)
				{
					array[num] = value;
					num++;
				}
				return array;
			}
		}

		public static NonAllocDictionary<int, PhotonView>.ValueIterator PhotonViewCollection => photonViewList.Values;

		public static int ViewCount => photonViewList.Count;

		public static IPunPrefabPool PrefabPool
		{
			get
			{
				return prefabPool;
			}
			set
			{
				if (value == null)
				{
					UnityEngine.Debug.LogWarning("PhotonNetwork.PrefabPool cannot be set to null. It will default back to using the 'DefaultPool' Pool");
					prefabPool = new DefaultPool();
				}
				else
				{
					prefabPool = value;
				}
			}
		}

		public static float LevelLoadingProgress
		{
			get
			{
				if (_AsyncLevelLoadingOperation != null)
				{
					_levelLoadingProgress = _AsyncLevelLoadingOperation.progress;
				}
				else if (_levelLoadingProgress > 0f)
				{
					_levelLoadingProgress = 1f;
				}
				return _levelLoadingProgress;
			}
		}

		private static event Action<PhotonView, Player> OnOwnershipRequestEv;

		private static event Action<PhotonView, Player> OnOwnershipTransferedEv;

		private static event Action<PhotonView, Player> OnOwnershipTransferFailedEv;

		static PhotonNetwork()
		{
			MAX_VIEW_IDS = 1000;
			ConnectMethod = ConnectMethod.NotCalled;
			LogLevel = PunLogLevel.ErrorsOnly;
			EnableCloseConnection = false;
			PrecisionForVectorSynchronization = 9.9E-05f;
			PrecisionForQuaternionSynchronization = 1f;
			PrecisionForFloatSynchronization = 0.01f;
			offlineMode = false;
			offlineModeRoom = null;
			automaticallySyncScene = false;
			sendFrequency = 33;
			serializationFrequency = 100;
			isMessageQueueRunning = true;
			MinimalTimeScaleToDispatchInFixedUpdate = -1f;
			lastUsedViewSubId = 0;
			lastUsedViewSubIdStatic = 0;
			PrefabsWithoutMagicCallback = new HashSet<string>();
			SendInstantiateEvHashtable = new ExitGames.Client.Photon.Hashtable();
			SendInstantiateRaiseEventOptions = new RaiseEventOptions();
			allowedReceivingGroups = new HashSet<byte>();
			blockedSendingGroups = new HashSet<byte>();
			reusablePVHashset = new HashSet<PhotonView>();
			photonViewList = new NonAllocDictionary<int, PhotonView>();
			currentLevelPrefix = 0;
			loadingLevelAndPausedNetwork = false;
			monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();
			RunRpcCoroutines = true;
			_levelLoadingProgress = 0f;
			typePunRPC = typeof(PunRPC);
			typePhotonMessageInfo = typeof(PhotonMessageInfo);
			keyByteZero = (byte)0;
			keyByteOne = (byte)1;
			keyByteTwo = (byte)2;
			keyByteThree = (byte)3;
			keyByteFour = (byte)4;
			keyByteFive = (byte)5;
			keyByteSix = (byte)6;
			keyByteSeven = (byte)7;
			keyByteEight = (byte)8;
			emptyObjectArray = new object[0];
			emptyTypeArray = new Type[0];
			foundPVs = new List<PhotonView>();
			removeFilter = new ExitGames.Client.Photon.Hashtable();
			ServerCleanDestroyEvent = new ExitGames.Client.Photon.Hashtable();
			ServerCleanOptions = new RaiseEventOptions
			{
				CachingOption = EventCaching.RemoveFromRoomCache
			};
			SendToAllOptions = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.All
			};
			SendToOthersOptions = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.Others
			};
			SendToSingleOptions = new RaiseEventOptions
			{
				TargetActors = new int[1]
			};
			rpcFilterByViewId = new ExitGames.Client.Photon.Hashtable();
			OpCleanRpcBufferOptions = new RaiseEventOptions
			{
				CachingOption = EventCaching.RemoveFromRoomCache
			};
			rpcEvent = new ExitGames.Client.Photon.Hashtable();
			RpcOptionsToAll = new RaiseEventOptions();
			ObjectsInOneUpdate = 20;
			serializeStreamOut = new PhotonStream(write: true, null);
			serializeStreamIn = new PhotonStream(write: false, null);
			serializeRaiseEvOptions = new RaiseEventOptions();
			serializeViewBatches = new Dictionary<RaiseEventBatch, SerializeViewBatch>();
			StaticReset();
		}

		private static void StaticReset()
		{
			monoRPCMethodsCache.Clear();
			OfflineMode = false;
			NetworkingClient = new LoadBalancingClient(PhotonServerSettings.AppSettings.Protocol);
			NetworkingClient.LoadBalancingPeer.QuickResendAttempts = 2;
			NetworkingClient.LoadBalancingPeer.SentCountAllowance = 9;
			NetworkingClient.EventReceived -= OnEvent;
			NetworkingClient.EventReceived += OnEvent;
			NetworkingClient.OpResponseReceived -= OnOperation;
			NetworkingClient.OpResponseReceived += OnOperation;
			NetworkingClient.StateChanged -= OnClientStateChanged;
			NetworkingClient.StateChanged += OnClientStateChanged;
			StartupStopwatch = new Stopwatch();
			StartupStopwatch.Start();
			PhotonHandler.Instance.Client = NetworkingClient;
			Application.runInBackground = PhotonServerSettings.RunInBackground;
			PrefabPool = new DefaultPool();
			rpcShortcuts = new Dictionary<string, int>(PhotonServerSettings.RpcList.Count);
			for (int i = 0; i < PhotonServerSettings.RpcList.Count; i++)
			{
				string key = PhotonServerSettings.RpcList[i];
				rpcShortcuts[key] = i;
			}
			CustomTypes.Register();
		}

		public static bool ConnectUsingSettings()
		{
			if (PhotonServerSettings == null)
			{
				UnityEngine.Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
				return false;
			}
			return ConnectUsingSettings(PhotonServerSettings.AppSettings, PhotonServerSettings.StartInOfflineMode);
		}

		public static bool ConnectUsingSettings(AppSettings appSettings, bool startInOfflineMode = false)
		{
			if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
			{
				UnityEngine.Debug.LogWarning("ConnectUsingSettings() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
				return false;
			}
			if (ConnectionHandler.AppQuits)
			{
				UnityEngine.Debug.LogWarning("Can't connect: Application is closing. Unity called OnApplicationQuit().");
				return false;
			}
			if (PhotonServerSettings == null)
			{
				UnityEngine.Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
				return false;
			}
			SetupLogging();
			NetworkingClient.LoadBalancingPeer.TransportProtocol = appSettings.Protocol;
			NetworkingClient.ExpectedProtocol = null;
			NetworkingClient.EnableProtocolFallback = appSettings.EnableProtocolFallback;
			NetworkingClient.AuthMode = appSettings.AuthMode;
			IsMessageQueueRunning = true;
			NetworkingClient.AppId = appSettings.AppIdRealtime;
			GameVersion = appSettings.AppVersion;
			if (startInOfflineMode)
			{
				OfflineMode = true;
				return true;
			}
			if (OfflineMode)
			{
				OfflineMode = false;
				UnityEngine.Debug.LogWarning("ConnectUsingSettings() disabled the offline mode. No longer offline.");
			}
			NetworkingClient.EnableLobbyStatistics = appSettings.EnableLobbyStatistics;
			NetworkingClient.ProxyServerAddress = appSettings.ProxyServer;
			if (appSettings.IsMasterServerAddress)
			{
				if (AuthValues == null)
				{
					AuthValues = new AuthenticationValues(Guid.NewGuid().ToString());
				}
				else if (string.IsNullOrEmpty(AuthValues.UserId))
				{
					AuthValues.UserId = Guid.NewGuid().ToString();
				}
				return ConnectToMaster(appSettings.Server, appSettings.Port, appSettings.AppIdRealtime);
			}
			NetworkingClient.NameServerPortInAppSettings = appSettings.Port;
			if (!appSettings.IsDefaultNameServer)
			{
				NetworkingClient.NameServerHost = appSettings.Server;
			}
			if (appSettings.IsBestRegion)
			{
				return ConnectToBestCloudServer();
			}
			return ConnectToRegion(appSettings.FixedRegion);
		}

		public static bool ConnectToMaster(string masterServerAddress, int port, string appID)
		{
			if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
			{
				UnityEngine.Debug.LogWarning("ConnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
				return false;
			}
			if (ConnectionHandler.AppQuits)
			{
				UnityEngine.Debug.LogWarning("Can't connect: Application is closing. Unity called OnApplicationQuit().");
				return false;
			}
			if (OfflineMode)
			{
				OfflineMode = false;
				UnityEngine.Debug.LogWarning("ConnectToMaster() disabled the offline mode. No longer offline.");
			}
			if (!IsMessageQueueRunning)
			{
				IsMessageQueueRunning = true;
				UnityEngine.Debug.LogWarning("ConnectToMaster() enabled IsMessageQueueRunning. Needs to be able to dispatch incoming messages.");
			}
			SetupLogging();
			ConnectMethod = ConnectMethod.ConnectToMaster;
			NetworkingClient.IsUsingNameServer = false;
			NetworkingClient.MasterServerAddress = ((port == 0) ? masterServerAddress : (masterServerAddress + ":" + port));
			NetworkingClient.AppId = appID;
			return NetworkingClient.ConnectToMasterServer();
		}

		public static bool ConnectToBestCloudServer()
		{
			if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
			{
				UnityEngine.Debug.LogWarning("ConnectToBestCloudServer() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
				return false;
			}
			if (ConnectionHandler.AppQuits)
			{
				UnityEngine.Debug.LogWarning("Can't connect: Application is closing. Unity called OnApplicationQuit().");
				return false;
			}
			SetupLogging();
			ConnectMethod = ConnectMethod.ConnectToBest;
			return NetworkingClient.ConnectToNameServer();
		}

		public static bool ConnectToRegion(string region)
		{
			if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected && NetworkingClient.Server != ServerConnection.NameServer)
			{
				UnityEngine.Debug.LogWarning("ConnectToRegion() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
				return false;
			}
			if (ConnectionHandler.AppQuits)
			{
				UnityEngine.Debug.LogWarning("Can't connect: Application is closing. Unity called OnApplicationQuit().");
				return false;
			}
			SetupLogging();
			ConnectMethod = ConnectMethod.ConnectToRegion;
			if (!string.IsNullOrEmpty(region))
			{
				return NetworkingClient.ConnectToRegionMaster(region);
			}
			return false;
		}

		public static void Disconnect()
		{
			if (OfflineMode)
			{
				OfflineMode = false;
				offlineModeRoom = null;
				NetworkingClient.State = ClientState.Disconnecting;
				NetworkingClient.OnStatusChanged(StatusCode.Disconnect);
			}
			else if (NetworkingClient != null)
			{
				NetworkingClient.Disconnect();
			}
		}

		public static bool Reconnect()
		{
			if (string.IsNullOrEmpty(NetworkingClient.MasterServerAddress))
			{
				UnityEngine.Debug.LogWarning("Reconnect() failed. It seems the client wasn't connected before?! Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
				return false;
			}
			if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
			{
				UnityEngine.Debug.LogWarning("Reconnect() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
				return false;
			}
			if (OfflineMode)
			{
				OfflineMode = false;
				UnityEngine.Debug.LogWarning("Reconnect() disabled the offline mode. No longer offline.");
			}
			if (!IsMessageQueueRunning)
			{
				IsMessageQueueRunning = true;
				UnityEngine.Debug.LogWarning("Reconnect() enabled IsMessageQueueRunning. Needs to be able to dispatch incoming messages.");
			}
			NetworkingClient.IsUsingNameServer = false;
			return NetworkingClient.ReconnectToMaster();
		}

		public static void NetworkStatisticsReset()
		{
			NetworkingClient.LoadBalancingPeer.TrafficStatsReset();
		}

		public static string NetworkStatisticsToString()
		{
			if (NetworkingClient == null || OfflineMode)
			{
				return "Offline or in OfflineMode. No VitalStats available.";
			}
			return NetworkingClient.LoadBalancingPeer.VitalStatsToString(all: false);
		}

		private static bool VerifyCanUseNetwork()
		{
			if (IsConnected)
			{
				return true;
			}
			UnityEngine.Debug.LogError("Cannot send messages when not connected. Either connect to Photon OR use offline mode!");
			return false;
		}

		public static int GetPing()
		{
			return NetworkingClient.LoadBalancingPeer.RoundTripTime;
		}

		public static void FetchServerTimestamp()
		{
			if (NetworkingClient != null)
			{
				NetworkingClient.LoadBalancingPeer.FetchServerTimestamp();
			}
		}

		public static void SendAllOutgoingCommands()
		{
			if (VerifyCanUseNetwork())
			{
				while (NetworkingClient.LoadBalancingPeer.SendOutgoingCommands())
				{
				}
			}
		}

		public static bool CloseConnection(Player kickPlayer)
		{
			if (!VerifyCanUseNetwork())
			{
				return false;
			}
			if (!EnableCloseConnection)
			{
				UnityEngine.Debug.LogError("CloseConnection is disabled. No need to call it.");
				return false;
			}
			if (!LocalPlayer.IsMasterClient)
			{
				UnityEngine.Debug.LogError("CloseConnection: Only the masterclient can kick another player.");
				return false;
			}
			if (kickPlayer == null)
			{
				UnityEngine.Debug.LogError("CloseConnection: No such player connected!");
				return false;
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.TargetActors = new int[1] { kickPlayer.ActorNumber };
			RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
			return NetworkingClient.OpRaiseEvent(203, null, raiseEventOptions2, SendOptions.SendReliable);
		}

		public static bool SetMasterClient(Player masterClientPlayer)
		{
			if (!InRoom || !VerifyCanUseNetwork() || OfflineMode)
			{
				if (LogLevel == PunLogLevel.Informational)
				{
					UnityEngine.Debug.Log("Can not SetMasterClient(). Not in room or in OfflineMode.");
				}
				return false;
			}
			return CurrentRoom.SetMasterClient(masterClientPlayer);
		}

		public static bool JoinRandomRoom()
		{
			return JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null);
		}

		public static bool JoinRandomRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers)
		{
			return JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, null, null);
		}

		public static bool JoinRandomRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter, string[] expectedUsers = null)
		{
			if (OfflineMode)
			{
				if (offlineModeRoom != null)
				{
					UnityEngine.Debug.LogError("JoinRandomRoom failed. In offline mode you still have to leave a room to enter another.");
					return false;
				}
				EnterOfflineRoom("offline room", null, createdRoom: true);
				return true;
			}
			if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
			{
				UnityEngine.Debug.LogError("JoinRandomRoom failed. Client is on " + NetworkingClient.Server.ToString() + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : (" but not ready for operations (State: " + NetworkingClient.State.ToString() + ")")) + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			typedLobby = typedLobby ?? (NetworkingClient.InLobby ? NetworkingClient.CurrentLobby : null);
			OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
			opJoinRandomRoomParams.ExpectedCustomRoomProperties = expectedCustomRoomProperties;
			opJoinRandomRoomParams.ExpectedMaxPlayers = expectedMaxPlayers;
			opJoinRandomRoomParams.MatchingType = matchingType;
			opJoinRandomRoomParams.TypedLobby = typedLobby;
			opJoinRandomRoomParams.SqlLobbyFilter = sqlLobbyFilter;
			opJoinRandomRoomParams.ExpectedUsers = expectedUsers;
			return NetworkingClient.OpJoinRandomRoom(opJoinRandomRoomParams);
		}

		public static bool JoinRandomOrCreateRoom(ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = null, byte expectedMaxPlayers = 0, MatchmakingMode matchingType = MatchmakingMode.FillRoom, TypedLobby typedLobby = null, string sqlLobbyFilter = null, string roomName = null, RoomOptions roomOptions = null, string[] expectedUsers = null)
		{
			if (OfflineMode)
			{
				if (offlineModeRoom != null)
				{
					UnityEngine.Debug.LogError("JoinRandomOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
					return false;
				}
				EnterOfflineRoom("offline room", null, createdRoom: true);
				return true;
			}
			if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
			{
				UnityEngine.Debug.LogError("JoinRandomOrCreateRoom failed. Client is on " + NetworkingClient.Server.ToString() + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : (" but not ready for operations (State: " + NetworkingClient.State.ToString() + ")")) + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			typedLobby = typedLobby ?? (NetworkingClient.InLobby ? NetworkingClient.CurrentLobby : null);
			OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
			opJoinRandomRoomParams.ExpectedCustomRoomProperties = expectedCustomRoomProperties;
			opJoinRandomRoomParams.ExpectedMaxPlayers = expectedMaxPlayers;
			opJoinRandomRoomParams.MatchingType = matchingType;
			opJoinRandomRoomParams.TypedLobby = typedLobby;
			opJoinRandomRoomParams.SqlLobbyFilter = sqlLobbyFilter;
			opJoinRandomRoomParams.ExpectedUsers = expectedUsers;
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.RoomOptions = roomOptions;
			enterRoomParams.Lobby = typedLobby;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return NetworkingClient.OpJoinRandomOrCreateRoom(opJoinRandomRoomParams, enterRoomParams);
		}

		public static bool CreateRoom(string roomName, RoomOptions roomOptions = null, TypedLobby typedLobby = null, string[] expectedUsers = null)
		{
			if (OfflineMode)
			{
				if (offlineModeRoom != null)
				{
					UnityEngine.Debug.LogError("CreateRoom failed. In offline mode you still have to leave a room to enter another.");
					return false;
				}
				EnterOfflineRoom(roomName, roomOptions, createdRoom: true);
				return true;
			}
			if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
			{
				UnityEngine.Debug.LogError("CreateRoom failed. Client is on " + NetworkingClient.Server.ToString() + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : ("but not ready for operations (State: " + NetworkingClient.State.ToString() + ")")) + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			typedLobby = typedLobby ?? (NetworkingClient.InLobby ? NetworkingClient.CurrentLobby : null);
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.RoomOptions = roomOptions;
			enterRoomParams.Lobby = typedLobby;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return NetworkingClient.OpCreateRoom(enterRoomParams);
		}

		public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers = null)
		{
			if (OfflineMode)
			{
				if (offlineModeRoom != null)
				{
					UnityEngine.Debug.LogError("JoinOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
					return false;
				}
				EnterOfflineRoom(roomName, roomOptions, createdRoom: true);
				return true;
			}
			if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
			{
				UnityEngine.Debug.LogError("JoinOrCreateRoom failed. Client is on " + NetworkingClient.Server.ToString() + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : ("but not ready for operations (State: " + NetworkingClient.State.ToString() + ")")) + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			if (string.IsNullOrEmpty(roomName))
			{
				UnityEngine.Debug.LogError("JoinOrCreateRoom failed. A roomname is required. If you don't know one, how will you join?");
				return false;
			}
			typedLobby = typedLobby ?? (NetworkingClient.InLobby ? NetworkingClient.CurrentLobby : null);
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.RoomOptions = roomOptions;
			enterRoomParams.Lobby = typedLobby;
			enterRoomParams.PlayerProperties = LocalPlayer.CustomProperties;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return NetworkingClient.OpJoinOrCreateRoom(enterRoomParams);
		}

		public static bool JoinRoom(string roomName, string[] expectedUsers = null)
		{
			if (OfflineMode)
			{
				if (offlineModeRoom != null)
				{
					UnityEngine.Debug.LogError("JoinRoom failed. In offline mode you still have to leave a room to enter another.");
					return false;
				}
				EnterOfflineRoom(roomName, null, createdRoom: true);
				return true;
			}
			if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
			{
				UnityEngine.Debug.LogError("JoinRoom failed. Client is on " + NetworkingClient.Server.ToString() + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : ("but not ready for operations (State: " + NetworkingClient.State.ToString() + ")")) + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			if (string.IsNullOrEmpty(roomName))
			{
				UnityEngine.Debug.LogError("JoinRoom failed. A roomname is required. If you don't know one, how will you join?");
				return false;
			}
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return NetworkingClient.OpJoinRoom(enterRoomParams);
		}

		public static bool RejoinRoom(string roomName)
		{
			if (OfflineMode)
			{
				UnityEngine.Debug.LogError("RejoinRoom failed due to offline mode.");
				return false;
			}
			if (NetworkingClient.Server != ServerConnection.MasterServer || !IsConnectedAndReady)
			{
				UnityEngine.Debug.LogError("RejoinRoom failed. Client is on " + NetworkingClient.Server.ToString() + " (must be Master Server for matchmaking)" + (IsConnectedAndReady ? " and ready" : ("but not ready for operations (State: " + NetworkingClient.State.ToString() + ")")) + ". Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			if (string.IsNullOrEmpty(roomName))
			{
				UnityEngine.Debug.LogError("RejoinRoom failed. A roomname is required. If you don't know one, how will you join?");
				return false;
			}
			return NetworkingClient.OpRejoinRoom(roomName);
		}

		public static bool ReconnectAndRejoin()
		{
			if (NetworkingClient.LoadBalancingPeer.PeerState != PeerStateValue.Disconnected)
			{
				UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. Can only connect while in state 'Disconnected'. Current state: " + NetworkingClient.LoadBalancingPeer.PeerState);
				return false;
			}
			if (OfflineMode)
			{
				OfflineMode = false;
				UnityEngine.Debug.LogWarning("ReconnectAndRejoin() disabled the offline mode. No longer offline.");
			}
			if (!IsMessageQueueRunning)
			{
				IsMessageQueueRunning = true;
				UnityEngine.Debug.LogWarning("ReconnectAndRejoin() enabled IsMessageQueueRunning. Needs to be able to dispatch incoming messages.");
			}
			return NetworkingClient.ReconnectAndRejoin();
		}

		public static bool LeaveRoom(bool becomeInactive = true)
		{
			if (OfflineMode)
			{
				offlineModeRoom = null;
				NetworkingClient.MatchMakingCallbackTargets.OnLeftRoom();
				NetworkingClient.ConnectionCallbackTargets.OnConnectedToMaster();
				return true;
			}
			if (CurrentRoom == null)
			{
				UnityEngine.Debug.LogWarning("PhotonNetwork.CurrentRoom is null. You don't have to call LeaveRoom() when you're not in one. State: " + NetworkClientState);
			}
			else
			{
				becomeInactive = becomeInactive && CurrentRoom.PlayerTtl != 0;
			}
			return NetworkingClient.OpLeaveRoom(becomeInactive);
		}

		private static void EnterOfflineRoom(string roomName, RoomOptions roomOptions, bool createdRoom)
		{
			offlineModeRoom = new Room(roomName, roomOptions, isOffline: true);
			NetworkingClient.ChangeLocalID(1);
			offlineModeRoom.masterClientId = 1;
			offlineModeRoom.AddPlayer(LocalPlayer);
			offlineModeRoom.LoadBalancingClient = NetworkingClient;
			NetworkingClient.CurrentRoom = offlineModeRoom;
			if (createdRoom)
			{
				NetworkingClient.MatchMakingCallbackTargets.OnCreatedRoom();
			}
			NetworkingClient.MatchMakingCallbackTargets.OnJoinedRoom();
		}

		public static bool JoinLobby()
		{
			return JoinLobby(null);
		}

		public static bool JoinLobby(TypedLobby typedLobby)
		{
			if (IsConnected && Server == ServerConnection.MasterServer)
			{
				return NetworkingClient.OpJoinLobby(typedLobby);
			}
			return false;
		}

		public static bool LeaveLobby()
		{
			if (IsConnected && Server == ServerConnection.MasterServer)
			{
				return NetworkingClient.OpLeaveLobby();
			}
			return false;
		}

		public static bool FindFriends(string[] friendsToFind)
		{
			if (NetworkingClient == null || offlineMode)
			{
				return false;
			}
			return NetworkingClient.OpFindFriends(friendsToFind);
		}

		public static bool GetCustomRoomList(TypedLobby typedLobby, string sqlLobbyFilter)
		{
			return NetworkingClient.OpGetGameList(typedLobby, sqlLobbyFilter);
		}

		public static bool SetPlayerCustomProperties(ExitGames.Client.Photon.Hashtable customProperties)
		{
			if (customProperties == null)
			{
				customProperties = new ExitGames.Client.Photon.Hashtable();
				foreach (object key in LocalPlayer.CustomProperties.Keys)
				{
					customProperties[(string)key] = null;
				}
			}
			return LocalPlayer.SetCustomProperties(customProperties);
		}

		public static void RemovePlayerCustomProperties(string[] customPropertiesToDelete)
		{
			if (customPropertiesToDelete == null || customPropertiesToDelete.Length == 0 || LocalPlayer.CustomProperties == null)
			{
				LocalPlayer.CustomProperties = new ExitGames.Client.Photon.Hashtable();
				return;
			}
			foreach (string key in customPropertiesToDelete)
			{
				if (LocalPlayer.CustomProperties.ContainsKey(key))
				{
					LocalPlayer.CustomProperties.Remove(key);
				}
			}
		}

		public static bool RaiseEvent(byte eventCode, object eventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
		{
			if (offlineMode)
			{
				if (raiseEventOptions.Receivers == ReceiverGroup.Others)
				{
					return true;
				}
				EventData eventData = new EventData
				{
					Code = eventCode
				};
				eventData.Parameters[245] = eventContent;
				eventData.Parameters[254] = 1;
				NetworkingClient.OnEvent(eventData);
				return true;
			}
			if (!InRoom || eventCode >= 200)
			{
				UnityEngine.Debug.LogWarning("RaiseEvent(" + eventCode + ") failed. Your event is not being sent! Check if your are in a Room and the eventCode must be less than 200 (0..199).");
				return false;
			}
			return NetworkingClient.OpRaiseEvent(eventCode, eventContent, raiseEventOptions, sendOptions);
		}

		private static bool RaiseEventInternal(byte eventCode, object eventContent, RaiseEventOptions raiseEventOptions, SendOptions sendOptions)
		{
			if (offlineMode)
			{
				return false;
			}
			if (!InRoom)
			{
				UnityEngine.Debug.LogWarning("RaiseEvent(" + eventCode + ") failed. Your event is not being sent! Check if your are in a Room");
				return false;
			}
			return NetworkingClient.OpRaiseEvent(eventCode, eventContent, raiseEventOptions, sendOptions);
		}

		public static bool AllocateViewID(PhotonView view)
		{
			if (view.ViewID != 0)
			{
				UnityEngine.Debug.LogError("AllocateViewID() can't be used for PhotonViews that already have a viewID. This view is: " + view.ToString());
				return false;
			}
			int viewID = AllocateViewID(LocalPlayer.ActorNumber);
			view.ViewID = viewID;
			return true;
		}

		[Obsolete("Renamed. Use AllocateRoomViewID instead")]
		public static bool AllocateSceneViewID(PhotonView view)
		{
			return AllocateRoomViewID(view);
		}

		public static bool AllocateRoomViewID(PhotonView view)
		{
			if (!IsMasterClient)
			{
				UnityEngine.Debug.LogError("Only the Master Client can AllocateRoomViewID(). Check PhotonNetwork.IsMasterClient!");
				return false;
			}
			if (view.ViewID != 0)
			{
				UnityEngine.Debug.LogError("AllocateRoomViewID() can't be used for PhotonViews that already have a viewID. This view is: " + view.ToString());
				return false;
			}
			int viewID = AllocateViewID(0);
			view.ViewID = viewID;
			return true;
		}

		public static int AllocateViewID(bool roomObject)
		{
			if (roomObject && !LocalPlayer.IsMasterClient)
			{
				UnityEngine.Debug.LogError("Only a Master Client can AllocateViewID() for room objects. This client/player is not a Master Client. Returning an invalid viewID: -1.");
				return 0;
			}
			return AllocateViewID((!roomObject) ? LocalPlayer.ActorNumber : 0);
		}

		public static int AllocateViewID(int ownerId)
		{
			if (ownerId == 0)
			{
				int num = lastUsedViewSubIdStatic;
				int num2 = ownerId * MAX_VIEW_IDS;
				for (int i = 1; i < MAX_VIEW_IDS; i++)
				{
					num = (num + 1) % MAX_VIEW_IDS;
					if (num != 0)
					{
						int num3 = num + num2;
						if (!photonViewList.ContainsKey(num3))
						{
							lastUsedViewSubIdStatic = num;
							return num3;
						}
					}
				}
				throw new Exception($"AllocateViewID() failed. The room (user {ownerId}) is out of 'room' viewIDs. It seems all available are in use.");
			}
			int num4 = lastUsedViewSubId;
			int num5 = ownerId * MAX_VIEW_IDS;
			for (int j = 1; j <= MAX_VIEW_IDS; j++)
			{
				num4 = (num4 + 1) % MAX_VIEW_IDS;
				if (num4 != 0)
				{
					int num6 = num4 + num5;
					if (!photonViewList.ContainsKey(num6))
					{
						lastUsedViewSubId = num4;
						return num6;
					}
				}
			}
			throw new Exception($"AllocateViewID() failed. User {ownerId} is out of viewIDs. It seems all available are in use.");
		}

		public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
		{
			if (CurrentRoom == null)
			{
				UnityEngine.Debug.LogError("Can not Instantiate before the client joined/created a room. State: " + NetworkClientState);
				return null;
			}
			return NetworkInstantiate(new InstantiateParameters(prefabName, position, rotation, group, data, currentLevelPrefix, null, LocalPlayer, ServerTimestamp));
		}

		[Obsolete("Renamed. Use InstantiateRoomObject instead")]
		public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
		{
			return InstantiateRoomObject(prefabName, position, rotation, group, data);
		}

		public static GameObject InstantiateRoomObject(string prefabName, Vector3 position, Quaternion rotation, byte group = 0, object[] data = null)
		{
			if (CurrentRoom == null)
			{
				UnityEngine.Debug.LogError("Can not Instantiate before the client joined/created a room.");
				return null;
			}
			if (LocalPlayer.IsMasterClient)
			{
				return NetworkInstantiate(new InstantiateParameters(prefabName, position, rotation, group, data, currentLevelPrefix, null, LocalPlayer, ServerTimestamp), roomObject: true);
			}
			return null;
		}

		private static GameObject NetworkInstantiate(ExitGames.Client.Photon.Hashtable networkEvent, Player creator)
		{
			string prefabName = (string)networkEvent[keyByteZero];
			int timestamp = (int)networkEvent[keyByteSix];
			int num = (int)networkEvent[keyByteSeven];
			Vector3 position = ((!networkEvent.ContainsKey(keyByteOne)) ? Vector3.zero : ((Vector3)networkEvent[keyByteOne]));
			Quaternion rotation = Quaternion.identity;
			if (networkEvent.ContainsKey(keyByteTwo))
			{
				rotation = (Quaternion)networkEvent[keyByteTwo];
			}
			byte b = 0;
			if (networkEvent.ContainsKey(keyByteThree))
			{
				b = (byte)networkEvent[keyByteThree];
			}
			byte objLevelPrefix = 0;
			if (networkEvent.ContainsKey(keyByteEight))
			{
				objLevelPrefix = (byte)networkEvent[keyByteEight];
			}
			int[] viewIDs = ((!networkEvent.ContainsKey(keyByteFour)) ? new int[1] { num } : ((int[])networkEvent[keyByteFour]));
			object[] data = ((!networkEvent.ContainsKey(keyByteFive)) ? null : ((object[])networkEvent[keyByteFive]));
			if (b != 0 && !allowedReceivingGroups.Contains(b))
			{
				return null;
			}
			return NetworkInstantiate(new InstantiateParameters(prefabName, position, rotation, b, data, objLevelPrefix, viewIDs, creator, timestamp), roomObject: false, instantiateEvent: true);
		}

		private static GameObject NetworkInstantiate(InstantiateParameters parameters, bool roomObject = false, bool instantiateEvent = false)
		{
			if (!InterceptNetworkspawn(ref parameters, roomObject, instantiateEvent))
			{
				UnityEngine.Debug.LogError("Intercepted bad networkspawn, Object " + parameters.prefabName);
				return null;
			}
			GameObject gameObject = null;
			gameObject = prefabPool.Instantiate(parameters.prefabName, parameters.position, parameters.rotation);
			if (gameObject == null)
			{
				UnityEngine.Debug.LogError("Failed to network-Instantiate: " + parameters.prefabName);
				return null;
			}
			if (gameObject.activeSelf)
			{
				UnityEngine.Debug.LogWarning("PrefabPool.Instantiate() should return an inactive GameObject. " + prefabPool.GetType().Name + " returned an active object. PrefabId: " + parameters.prefabName);
			}
			PhotonView[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
			if (photonViewsInChildren.Length == 0)
			{
				UnityEngine.Debug.LogError("PhotonNetwork.Instantiate() can only instantiate objects with a PhotonView component. This prefab does not have one: " + parameters.prefabName);
				return null;
			}
			bool flag = !instantiateEvent && LocalPlayer.Equals(parameters.creator);
			if (flag)
			{
				parameters.viewIDs = new int[photonViewsInChildren.Length];
			}
			for (int i = 0; i < photonViewsInChildren.Length; i++)
			{
				if (flag)
				{
					parameters.viewIDs[i] = (roomObject ? AllocateViewID(0) : AllocateViewID(parameters.creator.ActorNumber));
				}
				PhotonView obj = photonViewsInChildren[i];
				obj.ViewID = 0;
				obj.sceneViewId = 0;
				obj.isRuntimeInstantiated = true;
				obj.lastOnSerializeDataSent = null;
				obj.lastOnSerializeDataReceived = null;
				obj.Prefix = parameters.objLevelPrefix;
				obj.InstantiationId = parameters.viewIDs[0];
				obj.InstantiationData = parameters.data;
				obj.ViewID = parameters.viewIDs[i];
				obj.Group = parameters.group;
			}
			if (flag)
			{
				SendInstantiate(parameters, roomObject);
			}
			gameObject.SetActive(value: true);
			if (!PrefabsWithoutMagicCallback.Contains(parameters.prefabName))
			{
				IPunInstantiateMagicCallback[] components = gameObject.GetComponents<IPunInstantiateMagicCallback>();
				if (components.Length != 0)
				{
					PhotonMessageInfo info = new PhotonMessageInfo(parameters.creator, parameters.timestamp, photonViewsInChildren[0]);
					IPunInstantiateMagicCallback[] array = components;
					for (int j = 0; j < array.Length; j++)
					{
						array[j].OnPhotonInstantiate(info);
					}
				}
				else
				{
					PrefabsWithoutMagicCallback.Add(parameters.prefabName);
				}
			}
			return gameObject;
		}

		private static bool InterceptNetworkspawn(ref InstantiateParameters parameters, bool roomObject, bool instantiateEvent)
		{
			if (m_DisableIntercept == null)
			{
				m_DisableIntercept = UnityEngine.Object.FindObjectOfType<DisableIntercept>();
			}
			if (m_DisableIntercept != null && !m_DisableIntercept.CanIntercept())
			{
				return true;
			}
			if (!instantiateEvent)
			{
				return true;
			}
			switch (parameters.prefabName)
			{
			case "Player":
			case "PlayerData":
			case "ItemDataSyncer":
				return true;
			default:
				if (parameters.creator == null)
				{
					return true;
				}
				if (parameters.creator.IsMasterClient)
				{
					return true;
				}
				UnityEngine.Debug.Log("Player " + parameters.creator.NickName + " tries to spawn something illegal, blocking spawn for " + parameters.prefabName);
				return false;
			}
		}

		internal static bool SendInstantiate(InstantiateParameters parameters, bool roomObject = false)
		{
			int num = parameters.viewIDs[0];
			SendInstantiateEvHashtable.Clear();
			SendInstantiateEvHashtable[keyByteZero] = parameters.prefabName;
			if (parameters.position != Vector3.zero)
			{
				SendInstantiateEvHashtable[keyByteOne] = parameters.position;
			}
			if (parameters.rotation != Quaternion.identity)
			{
				SendInstantiateEvHashtable[keyByteTwo] = parameters.rotation;
			}
			if (parameters.group != 0)
			{
				SendInstantiateEvHashtable[keyByteThree] = parameters.group;
			}
			if (parameters.viewIDs.Length > 1)
			{
				SendInstantiateEvHashtable[keyByteFour] = parameters.viewIDs;
			}
			if (parameters.data != null)
			{
				SendInstantiateEvHashtable[keyByteFive] = parameters.data;
			}
			if (currentLevelPrefix > 0)
			{
				SendInstantiateEvHashtable[keyByteEight] = currentLevelPrefix;
			}
			SendInstantiateEvHashtable[keyByteSix] = ServerTimestamp;
			SendInstantiateEvHashtable[keyByteSeven] = num;
			SendInstantiateRaiseEventOptions.CachingOption = (roomObject ? EventCaching.AddToRoomCacheGlobal : EventCaching.AddToRoomCache);
			return RaiseEventInternal(202, SendInstantiateEvHashtable, SendInstantiateRaiseEventOptions, SendOptions.SendReliable);
		}

		public static void Destroy(PhotonView targetView)
		{
			if (targetView != null)
			{
				RemoveInstantiatedGO(targetView.gameObject, !InRoom);
			}
			else
			{
				UnityEngine.Debug.LogError("Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
			}
		}

		public static void Destroy(GameObject targetGo)
		{
			RemoveInstantiatedGO(targetGo, !InRoom);
		}

		public static void DestroyPlayerObjects(Player targetPlayer)
		{
			if (targetPlayer == null)
			{
				UnityEngine.Debug.LogError("DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
			}
			DestroyPlayerObjects(targetPlayer.ActorNumber);
		}

		public static void DestroyPlayerObjects(int targetPlayerId)
		{
			if (VerifyCanUseNetwork())
			{
				if (LocalPlayer.IsMasterClient || targetPlayerId == LocalPlayer.ActorNumber)
				{
					DestroyPlayerObjects(targetPlayerId, localOnly: false);
				}
				else
				{
					UnityEngine.Debug.LogError("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: " + IsMasterClient);
				}
			}
		}

		public static void DestroyAll()
		{
			if (IsMasterClient)
			{
				DestroyAll(localOnly: false);
			}
			else
			{
				UnityEngine.Debug.LogError("Couldn't call DestroyAll() as only the master client is allowed to call this.");
			}
		}

		public static void RemoveRPCs(Player targetPlayer)
		{
			if (VerifyCanUseNetwork())
			{
				if (!targetPlayer.IsLocal && !IsMasterClient)
				{
					UnityEngine.Debug.LogError("Error; Only the MasterClient can call RemoveRPCs for other players.");
				}
				else
				{
					OpCleanActorRpcBuffer(targetPlayer.ActorNumber);
				}
			}
		}

		public static void RemoveRPCs(PhotonView targetPhotonView)
		{
			if (VerifyCanUseNetwork())
			{
				CleanRpcBufferIfMine(targetPhotonView);
			}
		}

		internal static void RPC(PhotonView view, string methodName, RpcTarget target, bool encrypt, params object[] parameters)
		{
			if (string.IsNullOrEmpty(methodName))
			{
				UnityEngine.Debug.LogError("RPC method name cannot be null or empty.");
			}
			else
			{
				if (!VerifyCanUseNetwork())
				{
					return;
				}
				if (CurrentRoom == null)
				{
					UnityEngine.Debug.LogWarning("RPCs can only be sent in rooms. Call of \"" + methodName + "\" gets executed locally only, if at all.");
					return;
				}
				if (NetworkingClient != null)
				{
					RPC(view, methodName, target, null, encrypt, parameters);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
				}
				NetworkStatistics.AddRPC_Called(methodName);
			}
		}

		internal static void RPC(PhotonView view, string methodName, Player targetPlayer, bool encrypt, params object[] parameters)
		{
			if (!VerifyCanUseNetwork())
			{
				return;
			}
			if (CurrentRoom == null)
			{
				UnityEngine.Debug.LogWarning("RPCs can only be sent in rooms. Call of \"" + methodName + "\" gets executed locally only, if at all.");
				return;
			}
			if (LocalPlayer == null)
			{
				UnityEngine.Debug.LogError("RPC can't be sent to target Player being null! Did not send \"" + methodName + "\" call.");
			}
			if (NetworkingClient != null)
			{
				RPC(view, methodName, RpcTarget.Others, targetPlayer, encrypt, parameters);
			}
			else
			{
				UnityEngine.Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
			}
		}

		public static HashSet<GameObject> FindGameObjectsWithComponent(Type type)
		{
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			Component[] array = (Component[])UnityEngine.Object.FindObjectsOfType(type);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					hashSet.Add(array[i].gameObject);
				}
			}
			return hashSet;
		}

		public static void SetInterestGroups(byte group, bool enabled)
		{
			if (VerifyCanUseNetwork())
			{
				if (enabled)
				{
					byte[] enableGroups = new byte[1] { group };
					SetInterestGroups(null, enableGroups);
				}
				else
				{
					SetInterestGroups(new byte[1] { group }, null);
				}
			}
		}

		public static void LoadLevel(int levelNumber)
		{
			if (!ConnectionHandler.AppQuits)
			{
				UnityEngine.Debug.Log($"Photon is loading level: {levelNumber}");
				if (AutomaticallySyncScene)
				{
					SetLevelInPropsIfSynced(levelNumber);
				}
				IsMessageQueueRunning = false;
				loadingLevelAndPausedNetwork = true;
				_AsyncLevelLoadingOperation = SceneManager.LoadSceneAsync(levelNumber, LoadSceneMode.Single);
			}
		}

		public static void LoadLevel(string levelName)
		{
			UnityEngine.Debug.Log("Photon is loading level: " + levelName);
			if (!ConnectionHandler.AppQuits)
			{
				if (AutomaticallySyncScene)
				{
					SetLevelInPropsIfSynced(levelName);
				}
				IsMessageQueueRunning = false;
				loadingLevelAndPausedNetwork = true;
				_AsyncLevelLoadingOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
			}
		}

		public static bool WebRpc(string name, object parameters, bool sendAuthCookie = false)
		{
			return NetworkingClient.OpWebRpc(name, parameters, sendAuthCookie);
		}

		private static void SetupLogging()
		{
			if (LogLevel == PunLogLevel.ErrorsOnly)
			{
				LogLevel = PhotonServerSettings.PunLogging;
			}
			if (NetworkingClient.LoadBalancingPeer.DebugOut == DebugLevel.ERROR)
			{
				NetworkingClient.LoadBalancingPeer.DebugOut = PhotonServerSettings.AppSettings.NetworkLogging;
			}
		}

		public static void LoadOrCreateSettings(bool reload = false)
		{
			if (reload)
			{
				photonServerSettings = null;
			}
			else if (photonServerSettings != null)
			{
				UnityEngine.Debug.LogWarning("photonServerSettings is not null. Will not LoadOrCreateSettings().");
				return;
			}
			photonServerSettings = (ServerSettings)Resources.Load("PhotonServerSettings", typeof(ServerSettings));
			if (!(photonServerSettings != null) && photonServerSettings == null)
			{
				photonServerSettings = (ServerSettings)ScriptableObject.CreateInstance("ServerSettings");
				if (photonServerSettings == null)
				{
					UnityEngine.Debug.LogError("Failed to create ServerSettings. PUN is unable to run this way. If you deleted it from the project, reload the Editor.");
				}
			}
		}

		public static void AddCallbackTarget(object target)
		{
			if (!(target is PhotonView))
			{
				if (target is IPunOwnershipCallbacks punOwnershipCallbacks)
				{
					OnOwnershipRequestEv += punOwnershipCallbacks.OnOwnershipRequest;
					OnOwnershipTransferedEv += punOwnershipCallbacks.OnOwnershipTransfered;
					OnOwnershipTransferFailedEv += punOwnershipCallbacks.OnOwnershipTransferFailed;
				}
				NetworkingClient.AddCallbackTarget(target);
			}
		}

		public static void RemoveCallbackTarget(object target)
		{
			if (!(target is PhotonView) && NetworkingClient != null)
			{
				if (target is IPunOwnershipCallbacks punOwnershipCallbacks)
				{
					OnOwnershipRequestEv -= punOwnershipCallbacks.OnOwnershipRequest;
					OnOwnershipTransferedEv -= punOwnershipCallbacks.OnOwnershipTransfered;
					OnOwnershipTransferFailedEv -= punOwnershipCallbacks.OnOwnershipTransferFailed;
				}
				NetworkingClient.RemoveCallbackTarget(target);
			}
		}

		internal static string CallbacksToString()
		{
			string[] value = NetworkingClient.ConnectionCallbackTargets.Select((IConnectionCallbacks m) => m.ToString()).ToArray();
			return string.Join(", ", value);
		}

		private static void LeftRoomCleanup()
		{
			if (_AsyncLevelLoadingOperation != null)
			{
				_AsyncLevelLoadingOperation.allowSceneActivation = false;
				_AsyncLevelLoadingOperation = null;
			}
			rpcEvent.Clear();
			bool num = NetworkingClient.CurrentRoom != null && CurrentRoom.AutoCleanUp;
			allowedReceivingGroups = new HashSet<byte>();
			blockedSendingGroups = new HashSet<byte>();
			if (num || offlineModeRoom != null)
			{
				LocalCleanupAnythingInstantiated(destroyInstantiatedGameObjects: true);
			}
		}

		internal static void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
		{
			if (destroyInstantiatedGameObjects)
			{
				HashSet<GameObject> hashSet = new HashSet<GameObject>();
				foreach (PhotonView value in photonViewList.Values)
				{
					if (value.isRuntimeInstantiated)
					{
						hashSet.Add(value.gameObject);
					}
					else
					{
						value.ResetPhotonView(resetOwner: true);
					}
				}
				foreach (GameObject item in hashSet)
				{
					RemoveInstantiatedGO(item, localOnly: true);
				}
			}
			lastUsedViewSubId = 0;
			lastUsedViewSubIdStatic = 0;
		}

		private static void ResetPhotonViewsOnSerialize()
		{
			foreach (PhotonView value in photonViewList.Values)
			{
				value.lastOnSerializeDataSent = null;
			}
		}

		internal static void ExecuteRpc(ExitGames.Client.Photon.Hashtable rpcData, Player sender)
		{
			if (rpcData == null || !rpcData.ContainsKey(keyByteZero))
			{
				UnityEngine.Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString(rpcData));
				return;
			}
			int num = (int)rpcData[keyByteZero];
			int num2 = 0;
			if (rpcData.ContainsKey(keyByteOne))
			{
				num2 = (short)rpcData[keyByteOne];
			}
			string text;
			if (rpcData.ContainsKey(keyByteFive))
			{
				int num3 = (byte)rpcData[keyByteFive];
				if (num3 > PhotonServerSettings.RpcList.Count - 1)
				{
					UnityEngine.Debug.LogError("Could not find RPC with index: " + num3 + ". Going to ignore! Check PhotonServerSettings.RpcList");
					return;
				}
				text = PhotonServerSettings.RpcList[num3];
			}
			else
			{
				text = (string)rpcData[keyByteThree];
			}
			NetworkStatistics.AddRPC_Received(text);
			object[] array = null;
			if (rpcData.ContainsKey(keyByteFour))
			{
				array = (object[])rpcData[keyByteFour];
			}
			PhotonView photonView = GetPhotonView(num);
			if (photonView == null)
			{
				int num4 = num / MAX_VIEW_IDS;
				bool num5 = num4 == NetworkingClient.LocalPlayer.ActorNumber;
				bool flag = sender != null && num4 == sender.ActorNumber;
				if (num5)
				{
					UnityEngine.Debug.LogWarning("Received RPC \"" + text + "\" for viewID " + num + " but this PhotonView does not exist! View was/is ours." + (flag ? " Owner called." : " Remote called.") + " By: " + sender);
				}
				else
				{
					UnityEngine.Debug.LogWarning("Received RPC \"" + text + "\" for viewID " + num + " but this PhotonView does not exist! Was remote PV." + (flag ? " Owner called." : " Remote called.") + " By: " + sender?.ToString() + " Maybe GO was destroyed but RPC not cleaned up.");
				}
				return;
			}
			if (photonView.Prefix != num2)
			{
				UnityEngine.Debug.LogError("Received RPC \"" + text + "\" on viewID " + num + " with a prefix of " + num2 + ", our prefix is " + photonView.Prefix + ". The RPC has been ignored.");
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				UnityEngine.Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString(rpcData));
				return;
			}
			if (LogLevel >= PunLogLevel.Full)
			{
				UnityEngine.Debug.Log("Received RPC: " + text);
			}
			if (photonView.Group != 0 && !allowedReceivingGroups.Contains(photonView.Group))
			{
				return;
			}
			Type[] array2 = null;
			if (array != null && array.Length != 0)
			{
				array2 = new Type[array.Length];
				int num6 = 0;
				foreach (object obj in array)
				{
					if (obj == null)
					{
						array2[num6] = null;
					}
					else
					{
						array2[num6] = obj.GetType();
					}
					num6++;
				}
			}
			int num7 = 0;
			int num8 = 0;
			if (!UseRpcMonoBehaviourCache || photonView.RpcMonoBehaviours == null || photonView.RpcMonoBehaviours.Length == 0)
			{
				photonView.RefreshRpcMonoBehaviourCache();
			}
			for (int j = 0; j < photonView.RpcMonoBehaviours.Length; j++)
			{
				MonoBehaviour monoBehaviour = photonView.RpcMonoBehaviours[j];
				if (monoBehaviour == null)
				{
					UnityEngine.Debug.LogError("ERROR You have missing MonoBehaviours on your gameobjects!");
					continue;
				}
				Type type = monoBehaviour.GetType();
				List<MethodInfo> value = null;
				if (!monoRPCMethodsCache.TryGetValue(type, out value))
				{
					List<MethodInfo> methods = SupportClass.GetMethods(type, typePunRPC);
					monoRPCMethodsCache[type] = methods;
					value = methods;
				}
				if (value == null)
				{
					continue;
				}
				for (int k = 0; k < value.Count; k++)
				{
					MethodInfo methodInfo = value[k];
					if (!methodInfo.Name.Equals(text))
					{
						continue;
					}
					ParameterInfo[] cachedParemeters = methodInfo.GetCachedParemeters();
					num8++;
					if (array == null)
					{
						if (cachedParemeters.Length == 0)
						{
							num7++;
							object obj2 = methodInfo.Invoke(monoBehaviour, null);
							if (RunRpcCoroutines)
							{
								IEnumerator enumerator = null;
								if (obj2 is IEnumerator routine)
								{
									PhotonHandler.Instance.StartCoroutine(routine);
								}
							}
						}
						else
						{
							if (cachedParemeters.Length != 1 || !(cachedParemeters[0].ParameterType == typeof(PhotonMessageInfo)))
							{
								continue;
							}
							int timestamp = (int)rpcData[keyByteTwo];
							num7++;
							object obj3 = methodInfo.Invoke(monoBehaviour, new object[1]
							{
								new PhotonMessageInfo(sender, timestamp, photonView)
							});
							if (RunRpcCoroutines)
							{
								IEnumerator enumerator2 = null;
								if (obj3 is IEnumerator routine2)
								{
									PhotonHandler.Instance.StartCoroutine(routine2);
								}
							}
						}
					}
					else if (cachedParemeters.Length == array.Length)
					{
						if (!CheckTypeMatch(cachedParemeters, array2))
						{
							continue;
						}
						num7++;
						object obj4 = methodInfo.Invoke(monoBehaviour, array);
						if (RunRpcCoroutines)
						{
							IEnumerator enumerator3 = null;
							if (obj4 is IEnumerator routine3)
							{
								PhotonHandler.Instance.StartCoroutine(routine3);
							}
						}
					}
					else if (cachedParemeters.Length == array.Length + 1)
					{
						if (!(cachedParemeters[^1].ParameterType == typeof(PhotonMessageInfo)) || !CheckTypeMatch(cachedParemeters, array2))
						{
							continue;
						}
						int timestamp2 = (int)rpcData[keyByteTwo];
						object[] array3 = new object[array.Length + 1];
						array.CopyTo(array3, 0);
						array3[^1] = new PhotonMessageInfo(sender, timestamp2, photonView);
						num7++;
						object obj5 = methodInfo.Invoke(monoBehaviour, array3);
						if (RunRpcCoroutines)
						{
							IEnumerator enumerator4 = null;
							if (obj5 is IEnumerator routine4)
							{
								PhotonHandler.Instance.StartCoroutine(routine4);
							}
						}
					}
					else
					{
						if (cachedParemeters.Length != 1 || !cachedParemeters[0].ParameterType.IsArray)
						{
							continue;
						}
						num7++;
						object obj6 = methodInfo.Invoke(monoBehaviour, new object[1] { array });
						if (RunRpcCoroutines)
						{
							IEnumerator enumerator5 = null;
							if (obj6 is IEnumerator routine5)
							{
								PhotonHandler.Instance.StartCoroutine(routine5);
							}
						}
					}
				}
			}
			if (num7 == 1)
			{
				return;
			}
			string text2 = string.Empty;
			if (array2 != null)
			{
				_ = array2.Length;
				foreach (Type type2 in array2)
				{
					if (text2 != string.Empty)
					{
						text2 += ", ";
					}
					text2 = ((!(type2 == null)) ? (text2 + type2.Name) : (text2 + "null"));
				}
			}
			GameObject context = ((photonView != null) ? photonView.gameObject : null);
			if (num7 == 0)
			{
				if (num8 == 0)
				{
					UnityEngine.Debug.LogErrorFormat(context, "RPC method '{0}({2})' not found on object with PhotonView {1}. Implement as non-static. Apply [PunRPC]. Components on children are not found. Return type must be void or IEnumerator (if you enable RunRpcCoroutines). RPCs are a one-way message.", text, num, text2);
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat(context, "RPC method '{0}' found on object with PhotonView {1} but has wrong parameters. Implement as '{0}({2})'. PhotonMessageInfo is optional as final parameter.Return type must be void or IEnumerator (if you enable RunRpcCoroutines).", text, num, text2);
				}
			}
			else
			{
				UnityEngine.Debug.LogErrorFormat(context, "RPC method '{0}({2})' found {3}x on object with PhotonView {1}. Only one component should implement it.Return type must be void or IEnumerator (if you enable RunRpcCoroutines).", text, num, text2, num8);
			}
		}

		private static bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
		{
			if (methodParameters.Length < callParameterTypes.Length)
			{
				return false;
			}
			for (int i = 0; i < callParameterTypes.Length; i++)
			{
				Type parameterType = methodParameters[i].ParameterType;
				if (callParameterTypes[i] != null && !parameterType.IsAssignableFrom(callParameterTypes[i]) && (!parameterType.IsEnum || !Enum.GetUnderlyingType(parameterType).IsAssignableFrom(callParameterTypes[i])))
				{
					return false;
				}
			}
			return true;
		}

		public static void DestroyPlayerObjects(int playerId, bool localOnly)
		{
			if (playerId <= 0)
			{
				UnityEngine.Debug.LogError("Failed to Destroy objects of playerId: " + playerId);
				return;
			}
			if (!localOnly)
			{
				OpRemoveFromServerInstantiationsOfPlayer(playerId);
				OpCleanActorRpcBuffer(playerId);
				SendDestroyOfPlayer(playerId);
			}
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			foreach (PhotonView value in photonViewList.Values)
			{
				if (value == null)
				{
					UnityEngine.Debug.LogError("Null view");
				}
				else if (value.CreatorActorNr == playerId)
				{
					hashSet.Add(value.gameObject);
				}
				else if (value.OwnerActorNr == playerId)
				{
					Player owner = value.Owner;
					value.OwnerActorNr = value.CreatorActorNr;
					value.ControllerActorNr = value.CreatorActorNr;
					if (PhotonNetwork.OnOwnershipTransferedEv != null)
					{
						PhotonNetwork.OnOwnershipTransferedEv(value, owner);
					}
				}
			}
			foreach (GameObject item in hashSet)
			{
				RemoveInstantiatedGO(item, localOnly: true);
			}
		}

		public static void DestroyAll(bool localOnly)
		{
			if (!localOnly)
			{
				OpRemoveCompleteCache();
				SendDestroyOfAll();
			}
			LocalCleanupAnythingInstantiated(destroyInstantiatedGameObjects: true);
		}

		internal static void RemoveInstantiatedGO(GameObject go, bool localOnly)
		{
			if (ConnectionHandler.AppQuits)
			{
				return;
			}
			if (go == null)
			{
				UnityEngine.Debug.LogError("Failed to 'network-remove' GameObject because it's null.");
				return;
			}
			go.GetComponentsInChildren(includeInactive: true, foundPVs);
			if (foundPVs.Count <= 0)
			{
				UnityEngine.Debug.LogError("Failed to 'network-remove' GameObject because has no PhotonView components: " + go);
				return;
			}
			PhotonView photonView = foundPVs[0];
			if (!localOnly && !photonView.IsMine)
			{
				UnityEngine.Debug.LogError("Failed to 'network-remove' GameObject. Client is neither owner nor MasterClient taking over for owner who left: " + photonView);
				foundPVs.Clear();
				return;
			}
			if (!localOnly)
			{
				ServerCleanInstantiateAndDestroy(photonView);
			}
			int creatorActorNr = photonView.CreatorActorNr;
			for (int num = foundPVs.Count - 1; num >= 0; num--)
			{
				PhotonView photonView2 = foundPVs[num];
				if (!(photonView2 == null))
				{
					if (num != 0 && photonView2.CreatorActorNr != creatorActorNr)
					{
						photonView2.transform.SetParent(null, worldPositionStays: true);
					}
					else
					{
						photonView2.OnPreNetDestroy(photonView);
						if (photonView2.InstantiationId >= 1)
						{
							LocalCleanPhotonView(photonView2);
						}
						if (!localOnly)
						{
							OpCleanRpcBuffer(photonView2);
						}
					}
				}
			}
			if (LogLevel >= PunLogLevel.Full)
			{
				UnityEngine.Debug.Log("Network destroy Instantiated GO: " + go.name);
			}
			foundPVs.Clear();
			go.SetActive(value: false);
			prefabPool.Destroy(go);
		}

		private static void ServerCleanInstantiateAndDestroy(PhotonView photonView)
		{
			int num;
			if (photonView.isRuntimeInstantiated)
			{
				num = photonView.InstantiationId;
				removeFilter[keyByteSeven] = num;
				ServerCleanOptions.CachingOption = EventCaching.RemoveFromRoomCache;
				RaiseEventInternal(202, removeFilter, ServerCleanOptions, SendOptions.SendReliable);
			}
			else
			{
				num = photonView.ViewID;
			}
			ServerCleanDestroyEvent[keyByteZero] = num;
			ServerCleanOptions.CachingOption = ((!photonView.isRuntimeInstantiated) ? EventCaching.AddToRoomCacheGlobal : EventCaching.DoNotCache);
			RaiseEventInternal(204, ServerCleanDestroyEvent, ServerCleanOptions, SendOptions.SendReliable);
		}

		private static void SendDestroyOfPlayer(int actorNr)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[keyByteZero] = actorNr;
			RaiseEventInternal(207, hashtable, null, SendOptions.SendReliable);
		}

		private static void SendDestroyOfAll()
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[keyByteZero] = -1;
			RaiseEventInternal(207, hashtable, null, SendOptions.SendReliable);
		}

		private static void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
			raiseEventOptions.TargetActors = new int[1] { actorNr };
			RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
			RaiseEventInternal(202, null, raiseEventOptions2, SendOptions.SendReliable);
		}

		internal static void RequestOwnership(int viewID, int fromOwner)
		{
			RaiseEventInternal(209, new int[2] { viewID, fromOwner }, SendToAllOptions, SendOptions.SendReliable);
		}

		internal static void TransferOwnership(int viewID, int playerID)
		{
			RaiseEventInternal(210, new int[2] { viewID, playerID }, SendToAllOptions, SendOptions.SendReliable);
		}

		internal static void OwnershipUpdate(int[] viewOwnerPairs, int targetActor = -1)
		{
			RaiseEventOptions raiseEventOptions;
			if (targetActor == -1)
			{
				raiseEventOptions = SendToOthersOptions;
			}
			else
			{
				SendToSingleOptions.TargetActors[0] = targetActor;
				raiseEventOptions = SendToSingleOptions;
			}
			RaiseEventInternal(212, viewOwnerPairs, raiseEventOptions, SendOptions.SendReliable);
		}

		public static bool LocalCleanPhotonView(PhotonView view)
		{
			view.removedFromLocalViewList = true;
			return photonViewList.Remove(view.ViewID);
		}

		public static PhotonView GetPhotonView(int viewID)
		{
			PhotonView val = null;
			photonViewList.TryGetValue(viewID, out val);
			return val;
		}

		public static void RegisterPhotonView(PhotonView netView)
		{
			if (!Application.isPlaying)
			{
				photonViewList = new NonAllocDictionary<int, PhotonView>();
				return;
			}
			if (netView.ViewID == 0)
			{
				UnityEngine.Debug.Log("PhotonView register is ignored, because viewID is 0. No id assigned yet to: " + netView);
				return;
			}
			PhotonView val = null;
			if (photonViewList.TryGetValue(netView.ViewID, out val))
			{
				if (!(netView != val))
				{
					return;
				}
				UnityEngine.Debug.LogError($"PhotonView ID duplicate found: {netView.ViewID}. New: {netView} old: {val}. Maybe one wasn't destroyed on scene load?! Check for 'DontDestroyOnLoad'. Destroying old entry, adding new.");
				RemoveInstantiatedGO(val.gameObject, localOnly: true);
			}
			photonViewList.Add(netView.ViewID, netView);
			netView.removedFromLocalViewList = false;
			if (LogLevel >= PunLogLevel.Full)
			{
				UnityEngine.Debug.Log("Registered PhotonView: " + netView.ViewID);
			}
		}

		public static void OpCleanActorRpcBuffer(int actorNumber)
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
			raiseEventOptions.TargetActors = new int[1] { actorNumber };
			RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
			RaiseEventInternal(200, null, raiseEventOptions2, SendOptions.SendReliable);
		}

		public static void OpRemoveCompleteCacheOfPlayer(int actorNumber)
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
			raiseEventOptions.TargetActors = new int[1] { actorNumber };
			RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
			RaiseEventInternal(0, null, raiseEventOptions2, SendOptions.SendReliable);
		}

		public static void OpRemoveCompleteCache()
		{
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				CachingOption = EventCaching.RemoveFromRoomCache,
				Receivers = ReceiverGroup.MasterClient
			};
			RaiseEventInternal(0, null, raiseEventOptions, SendOptions.SendReliable);
		}

		private static void RemoveCacheOfLeftPlayers()
		{
			ParameterDictionary parameterDictionary = new ParameterDictionary(2);
			parameterDictionary[244] = (byte)0;
			parameterDictionary[247] = (byte)7;
			NetworkingClient.LoadBalancingPeer.SendOperation(253, parameterDictionary, SendOptions.SendReliable);
		}

		public static void CleanRpcBufferIfMine(PhotonView view)
		{
			if (view.OwnerActorNr != NetworkingClient.LocalPlayer.ActorNumber && !NetworkingClient.LocalPlayer.IsMasterClient)
			{
				UnityEngine.Debug.LogError("Cannot remove cached RPCs on a PhotonView thats not ours! " + view.Owner?.ToString() + " scene: " + view.IsRoomView);
			}
			else
			{
				OpCleanRpcBuffer(view);
			}
		}

		public static void OpCleanRpcBuffer(PhotonView view)
		{
			rpcFilterByViewId[keyByteZero] = view.ViewID;
			RaiseEventInternal(200, rpcFilterByViewId, OpCleanRpcBufferOptions, SendOptions.SendReliable);
		}

		public static void RemoveRPCsInGroup(int group)
		{
			foreach (PhotonView value in photonViewList.Values)
			{
				if (value.Group == group)
				{
					CleanRpcBufferIfMine(value);
				}
			}
		}

		public static bool RemoveBufferedRPCs(int viewId = 0, string methodName = null, int[] callersActorNumbers = null)
		{
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable(2);
			if (viewId != 0)
			{
				hashtable[keyByteZero] = viewId;
			}
			if (!string.IsNullOrEmpty(methodName))
			{
				if (rpcShortcuts.TryGetValue(methodName, out var value))
				{
					hashtable[keyByteFive] = (byte)value;
				}
				else
				{
					hashtable[keyByteThree] = methodName;
				}
			}
			RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.CachingOption = EventCaching.RemoveFromRoomCache;
			if (callersActorNumbers != null)
			{
				raiseEventOptions.TargetActors = callersActorNumbers;
			}
			return RaiseEventInternal(200, hashtable, raiseEventOptions, SendOptions.SendReliable);
		}

		public static void SetLevelPrefix(byte prefix)
		{
			currentLevelPrefix = prefix;
		}

		internal static void RPC(PhotonView view, string methodName, RpcTarget target, Player player, bool encrypt, params object[] parameters)
		{
			if (blockedSendingGroups.Contains(view.Group))
			{
				return;
			}
			if (view.ViewID < 1)
			{
				UnityEngine.Debug.LogError("Illegal view ID:" + view.ViewID + " method: " + methodName + " GO:" + view.gameObject.name);
			}
			if (LogLevel >= PunLogLevel.Full)
			{
				UnityEngine.Debug.Log("Sending RPC \"" + methodName + "\" to target: " + target.ToString() + " or player:" + player?.ToString() + ".");
			}
			rpcEvent.Clear();
			rpcEvent[keyByteZero] = view.ViewID;
			if (view.Prefix > 0)
			{
				rpcEvent[keyByteOne] = (short)view.Prefix;
			}
			rpcEvent[keyByteTwo] = ServerTimestamp;
			int value = 0;
			if (rpcShortcuts.TryGetValue(methodName, out value))
			{
				rpcEvent[keyByteFive] = (byte)value;
			}
			else
			{
				rpcEvent[keyByteThree] = methodName;
			}
			if (parameters != null && parameters.Length != 0)
			{
				rpcEvent[keyByteFour] = parameters;
			}
			SendOptions sendOptions = new SendOptions
			{
				Reliability = true,
				Encrypt = encrypt
			};
			if (player != null)
			{
				if (NetworkingClient.LocalPlayer.ActorNumber == player.ActorNumber)
				{
					ExecuteRpc(rpcEvent, player);
					return;
				}
				RaiseEventOptions raiseEventOptions = new RaiseEventOptions();
				raiseEventOptions.TargetActors = new int[1] { player.ActorNumber };
				RaiseEventOptions raiseEventOptions2 = raiseEventOptions;
				RaiseEventInternal(200, rpcEvent, raiseEventOptions2, sendOptions);
				return;
			}
			switch (target)
			{
			case RpcTarget.All:
				RpcOptionsToAll.InterestGroup = view.Group;
				RaiseEventInternal(200, rpcEvent, RpcOptionsToAll, sendOptions);
				ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
				break;
			case RpcTarget.Others:
			{
				RaiseEventOptions raiseEventOptions8 = new RaiseEventOptions
				{
					InterestGroup = view.Group
				};
				RaiseEventInternal(200, rpcEvent, raiseEventOptions8, sendOptions);
				break;
			}
			case RpcTarget.AllBuffered:
			{
				RaiseEventOptions raiseEventOptions6 = new RaiseEventOptions
				{
					CachingOption = EventCaching.AddToRoomCache
				};
				RaiseEventInternal(200, rpcEvent, raiseEventOptions6, sendOptions);
				ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
				break;
			}
			case RpcTarget.OthersBuffered:
			{
				RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
				{
					CachingOption = EventCaching.AddToRoomCache
				};
				RaiseEventInternal(200, rpcEvent, raiseEventOptions4, sendOptions);
				break;
			}
			case RpcTarget.MasterClient:
			{
				if (NetworkingClient.LocalPlayer.IsMasterClient)
				{
					ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
					break;
				}
				RaiseEventOptions raiseEventOptions7 = new RaiseEventOptions
				{
					Receivers = ReceiverGroup.MasterClient
				};
				RaiseEventInternal(200, rpcEvent, raiseEventOptions7, sendOptions);
				break;
			}
			case RpcTarget.AllViaServer:
			{
				RaiseEventOptions raiseEventOptions5 = new RaiseEventOptions
				{
					InterestGroup = view.Group,
					Receivers = ReceiverGroup.All
				};
				RaiseEventInternal(200, rpcEvent, raiseEventOptions5, sendOptions);
				if (OfflineMode)
				{
					ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
				}
				break;
			}
			case RpcTarget.AllBufferedViaServer:
			{
				RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
				{
					InterestGroup = view.Group,
					Receivers = ReceiverGroup.All,
					CachingOption = EventCaching.AddToRoomCache
				};
				RaiseEventInternal(200, rpcEvent, raiseEventOptions3, sendOptions);
				if (OfflineMode)
				{
					ExecuteRpc(rpcEvent, NetworkingClient.LocalPlayer);
				}
				break;
			}
			default:
				UnityEngine.Debug.LogError("Unsupported target enum: " + target);
				break;
			}
		}

		public static void SetInterestGroups(byte[] disableGroups, byte[] enableGroups)
		{
			if (disableGroups != null)
			{
				if (disableGroups.Length == 0)
				{
					allowedReceivingGroups.Clear();
				}
				else
				{
					for (int i = 0; i < disableGroups.Length; i++)
					{
						byte b = disableGroups[i];
						if (b <= 0)
						{
							UnityEngine.Debug.LogError("Error: PhotonNetwork.SetInterestGroups was called with an illegal group number: " + b + ". The Group number should be at least 1.");
						}
						else if (allowedReceivingGroups.Contains(b))
						{
							allowedReceivingGroups.Remove(b);
						}
					}
				}
			}
			if (enableGroups != null)
			{
				if (enableGroups.Length == 0)
				{
					for (byte b2 = 0; b2 < byte.MaxValue; b2++)
					{
						allowedReceivingGroups.Add(b2);
					}
					allowedReceivingGroups.Add(byte.MaxValue);
				}
				else
				{
					for (int j = 0; j < enableGroups.Length; j++)
					{
						byte b3 = enableGroups[j];
						if (b3 <= 0)
						{
							UnityEngine.Debug.LogError("Error: PhotonNetwork.SetInterestGroups was called with an illegal group number: " + b3 + ". The Group number should be at least 1.");
						}
						else
						{
							allowedReceivingGroups.Add(b3);
						}
					}
				}
			}
			if (!offlineMode)
			{
				NetworkingClient.OpChangeGroups(disableGroups, enableGroups);
			}
		}

		public static void SetSendingEnabled(byte group, bool enabled)
		{
			if (!enabled)
			{
				blockedSendingGroups.Add(group);
			}
			else
			{
				blockedSendingGroups.Remove(group);
			}
		}

		public static void SetSendingEnabled(byte[] disableGroups, byte[] enableGroups)
		{
			if (disableGroups != null)
			{
				foreach (byte item in disableGroups)
				{
					blockedSendingGroups.Add(item);
				}
			}
			if (enableGroups != null)
			{
				foreach (byte item2 in enableGroups)
				{
					blockedSendingGroups.Remove(item2);
				}
			}
		}

		internal static void NewSceneLoaded()
		{
			if (loadingLevelAndPausedNetwork)
			{
				_AsyncLevelLoadingOperation = null;
				loadingLevelAndPausedNetwork = false;
				IsMessageQueueRunning = true;
			}
			else
			{
				SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
			}
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, PhotonView> photonView in photonViewList)
			{
				if (photonView.Value == null)
				{
					list.Add(photonView.Key);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				int key = list[i];
				photonViewList.Remove(key);
			}
			if (list.Count > 0 && LogLevel >= PunLogLevel.Informational)
			{
				UnityEngine.Debug.Log("New level loaded. Removed " + list.Count + " scene view IDs from last level.");
			}
		}

		internal static void RunViewUpdate()
		{
			if (OfflineMode || CurrentRoom == null || CurrentRoom.Players == null || CurrentRoom.Players.Count <= 1)
			{
				return;
			}
			NonAllocDictionary<int, PhotonView>.PairIterator enumerator = photonViewList.GetEnumerator();
			while (enumerator.MoveNext())
			{
				PhotonView value = enumerator.Current.Value;
				if (value.Synchronization == ViewSynchronization.Off || !value.IsMine || !value.isActiveAndEnabled || blockedSendingGroups.Contains(value.Group))
				{
					continue;
				}
				List<object> list = OnSerializeWrite(value);
				if (list != null)
				{
					RaiseEventBatch raiseEventBatch = new RaiseEventBatch
					{
						Reliable = (value.Synchronization == ViewSynchronization.ReliableDeltaCompressed || value.mixedModeIsReliable),
						Group = value.Group
					};
					SerializeViewBatch value2 = null;
					if (!serializeViewBatches.TryGetValue(raiseEventBatch, out value2))
					{
						value2 = new SerializeViewBatch(raiseEventBatch, 2);
						serializeViewBatches.Add(raiseEventBatch, value2);
					}
					value2.Add(list);
					if (value2.ObjectUpdates.Count == value2.ObjectUpdates.Capacity)
					{
						SendSerializeViewBatch(value2);
					}
				}
			}
			Dictionary<RaiseEventBatch, SerializeViewBatch>.Enumerator enumerator2 = serializeViewBatches.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				SendSerializeViewBatch(enumerator2.Current.Value);
			}
		}

		private static void SendSerializeViewBatch(SerializeViewBatch batch)
		{
			if (batch != null && batch.ObjectUpdates.Count > 2)
			{
				serializeRaiseEvOptions.InterestGroup = batch.Batch.Group;
				batch.ObjectUpdates[0] = ServerTimestamp;
				batch.ObjectUpdates[1] = ((currentLevelPrefix != 0) ? ((object)currentLevelPrefix) : null);
				RaiseEventInternal((byte)(batch.Batch.Reliable ? 206 : 201), batch.ObjectUpdates, serializeRaiseEvOptions, batch.Batch.Reliable ? SendOptions.SendReliable : SendOptions.SendUnreliable);
				batch.Clear();
			}
		}

		private static List<object> OnSerializeWrite(PhotonView view)
		{
			if (view.Synchronization == ViewSynchronization.Off)
			{
				return null;
			}
			PhotonMessageInfo info = new PhotonMessageInfo(NetworkingClient.LocalPlayer, ServerTimestamp, view);
			if (view.syncValues == null)
			{
				view.syncValues = new List<object>();
			}
			view.syncValues.Clear();
			serializeStreamOut.SetWriteStream(view.syncValues);
			serializeStreamOut.SendNext(null);
			serializeStreamOut.SendNext(null);
			serializeStreamOut.SendNext(null);
			view.SerializeView(serializeStreamOut, info);
			if (serializeStreamOut.Count <= 3)
			{
				return null;
			}
			List<object> writeStream = serializeStreamOut.GetWriteStream();
			writeStream[0] = view.ViewID;
			writeStream[1] = false;
			writeStream[2] = null;
			if (view.Synchronization == ViewSynchronization.Unreliable)
			{
				return writeStream;
			}
			if (view.Synchronization == ViewSynchronization.UnreliableOnChange)
			{
				if (AlmostEquals(writeStream, view.lastOnSerializeDataSent))
				{
					if (view.mixedModeIsReliable)
					{
						return null;
					}
					view.mixedModeIsReliable = true;
					List<object> lastOnSerializeDataSent = view.lastOnSerializeDataSent;
					view.lastOnSerializeDataSent = writeStream;
					view.syncValues = lastOnSerializeDataSent;
				}
				else
				{
					view.mixedModeIsReliable = false;
					List<object> lastOnSerializeDataSent2 = view.lastOnSerializeDataSent;
					view.lastOnSerializeDataSent = writeStream;
					view.syncValues = lastOnSerializeDataSent2;
				}
				return writeStream;
			}
			if (view.Synchronization == ViewSynchronization.ReliableDeltaCompressed)
			{
				List<object> result = DeltaCompressionWrite(view.lastOnSerializeDataSent, writeStream);
				List<object> lastOnSerializeDataSent3 = view.lastOnSerializeDataSent;
				view.lastOnSerializeDataSent = writeStream;
				view.syncValues = lastOnSerializeDataSent3;
				return result;
			}
			return null;
		}

		private static void OnSerializeRead(object[] data, Player sender, int networkTime, short correctPrefix)
		{
			int viewID = (int)data[0];
			PhotonView photonView = GetPhotonView(viewID);
			if (photonView == null)
			{
				UnityEngine.Debug.LogWarning("Received OnSerialization for view ID " + viewID + ". We have no such PhotonView! Ignore this if you're joining or leaving a room. State: " + NetworkingClient.State);
			}
			else if (photonView.Prefix > 0 && correctPrefix != photonView.Prefix)
			{
				UnityEngine.Debug.LogError("Received OnSerialization for view ID " + viewID + " with prefix " + correctPrefix + ". Our prefix is " + photonView.Prefix);
			}
			else
			{
				if (photonView.Group != 0 && !allowedReceivingGroups.Contains(photonView.Group))
				{
					return;
				}
				if (photonView.Synchronization == ViewSynchronization.ReliableDeltaCompressed)
				{
					object[] array = DeltaCompressionRead(photonView.lastOnSerializeDataReceived, data);
					if (array == null)
					{
						if (LogLevel >= PunLogLevel.Informational)
						{
							UnityEngine.Debug.Log("Skipping packet for " + photonView.name + " [" + photonView.ViewID + "] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game.");
						}
						return;
					}
					photonView.lastOnSerializeDataReceived = array;
					data = array;
				}
				serializeStreamIn.SetReadStream(data, 3);
				photonView.DeserializeView(info: new PhotonMessageInfo(sender, networkTime, photonView), stream: serializeStreamIn);
			}
		}

		private static List<object> DeltaCompressionWrite(List<object> previousContent, List<object> currentContent)
		{
			if (currentContent == null || previousContent == null || previousContent.Count != currentContent.Count)
			{
				return currentContent;
			}
			if (currentContent.Count <= 3)
			{
				return null;
			}
			previousContent[1] = false;
			int num = 0;
			Queue<int> queue = null;
			for (int i = 3; i < currentContent.Count; i++)
			{
				object obj = currentContent[i];
				object two = previousContent[i];
				if (AlmostEquals(obj, two))
				{
					num++;
					previousContent[i] = null;
					continue;
				}
				previousContent[i] = obj;
				if (obj == null)
				{
					if (queue == null)
					{
						queue = new Queue<int>(currentContent.Count);
					}
					queue.Enqueue(i);
				}
			}
			if (num > 0)
			{
				if (num == currentContent.Count - 3)
				{
					return null;
				}
				previousContent[1] = true;
				if (queue != null)
				{
					previousContent[2] = queue.ToArray();
				}
			}
			previousContent[0] = currentContent[0];
			return previousContent;
		}

		private static object[] DeltaCompressionRead(object[] lastOnSerializeDataReceived, object[] incomingData)
		{
			if (!(bool)incomingData[1])
			{
				return incomingData;
			}
			if (lastOnSerializeDataReceived == null)
			{
				return null;
			}
			int[] array = incomingData[2] as int[];
			for (int i = 3; i < incomingData.Length; i++)
			{
				if ((array == null || !array.Contains(i)) && incomingData[i] == null)
				{
					object obj = lastOnSerializeDataReceived[i];
					incomingData[i] = obj;
				}
			}
			return incomingData;
		}

		private static bool AlmostEquals(IList<object> lastData, IList<object> currentContent)
		{
			if (lastData == null && currentContent == null)
			{
				return true;
			}
			if (lastData == null || currentContent == null || lastData.Count != currentContent.Count)
			{
				return false;
			}
			for (int i = 0; i < currentContent.Count; i++)
			{
				object one = currentContent[i];
				object two = lastData[i];
				if (!AlmostEquals(one, two))
				{
					return false;
				}
			}
			return true;
		}

		private static bool AlmostEquals(object one, object two)
		{
			if (one == null || two == null)
			{
				if (one == null)
				{
					return two == null;
				}
				return false;
			}
			if (!one.Equals(two))
			{
				if (one is object target)
				{
					Vector3 second = (Vector3)two;
					if (((Vector3)target).AlmostEquals(second, PrecisionForVectorSynchronization))
					{
						return true;
					}
				}
				else if (one is object target2)
				{
					Vector2 second2 = (Vector2)two;
					if (((Vector2)target2).AlmostEquals(second2, PrecisionForVectorSynchronization))
					{
						return true;
					}
				}
				else if (one is object target3)
				{
					Quaternion second3 = (Quaternion)two;
					if (((Quaternion)target3).AlmostEquals(second3, PrecisionForQuaternionSynchronization))
					{
						return true;
					}
				}
				else if (one is float target4)
				{
					float second4 = (float)two;
					if (target4.AlmostEquals(second4, PrecisionForFloatSynchronization))
					{
						return true;
					}
				}
				return false;
			}
			return true;
		}

		internal static bool GetMethod(MonoBehaviour monob, string methodType, out MethodInfo mi)
		{
			mi = null;
			if (monob == null || string.IsNullOrEmpty(methodType))
			{
				return false;
			}
			List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), null);
			for (int i = 0; i < methods.Count; i++)
			{
				MethodInfo methodInfo = methods[i];
				if (methodInfo.Name.Equals(methodType))
				{
					mi = methodInfo;
					return true;
				}
			}
			return false;
		}

		internal static void LoadLevelIfSynced()
		{
			if (!AutomaticallySyncScene || IsMasterClient || CurrentRoom == null || !CurrentRoom.CustomProperties.ContainsKey("curScn"))
			{
				return;
			}
			object obj = CurrentRoom.CustomProperties["curScn"];
			if (obj is int)
			{
				if (SceneManagerHelper.ActiveSceneBuildIndex != (int)obj)
				{
					LoadLevel((int)obj);
				}
			}
			else if (obj is string && SceneManagerHelper.ActiveSceneName != (string)obj)
			{
				LoadLevel((string)obj);
			}
		}

		internal static void SetLevelInPropsIfSynced(object levelId)
		{
			if (!AutomaticallySyncScene || !IsMasterClient || CurrentRoom == null)
			{
				return;
			}
			if (levelId == null)
			{
				UnityEngine.Debug.LogError("Parameter levelId can't be null!");
				return;
			}
			if (CurrentRoom.CustomProperties.ContainsKey("curScn"))
			{
				object obj = CurrentRoom.CustomProperties["curScn"];
				if (levelId.Equals(obj))
				{
					return;
				}
				int activeSceneBuildIndex = SceneManagerHelper.ActiveSceneBuildIndex;
				string activeSceneName = SceneManagerHelper.ActiveSceneName;
				if ((levelId.Equals(activeSceneBuildIndex) && obj.Equals(activeSceneName)) || (levelId.Equals(activeSceneName) && obj.Equals(activeSceneBuildIndex)))
				{
					return;
				}
			}
			if (_AsyncLevelLoadingOperation != null)
			{
				if (!_AsyncLevelLoadingOperation.isDone)
				{
					UnityEngine.Debug.LogWarning("PUN cancels an ongoing async level load, as another scene should be loaded. Next scene to load: " + levelId);
				}
				_AsyncLevelLoadingOperation.allowSceneActivation = false;
				_AsyncLevelLoadingOperation = null;
			}
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			if (levelId is int)
			{
				hashtable["curScn"] = (int)levelId;
			}
			else if (levelId is string)
			{
				hashtable["curScn"] = (string)levelId;
			}
			else
			{
				UnityEngine.Debug.LogError("Parameter levelId must be int or string!");
			}
			CurrentRoom.SetCustomProperties(hashtable);
			SendAllOutgoingCommands();
		}

		private static void OnEvent(EventData photonEvent)
		{
			int sender = photonEvent.Sender;
			Player player = null;
			if (sender > 0 && NetworkingClient.CurrentRoom != null)
			{
				player = NetworkingClient.CurrentRoom.GetPlayer(sender);
			}
			switch (photonEvent.Code)
			{
			case byte.MaxValue:
				ResetPhotonViewsOnSerialize();
				break;
			case 200:
				ExecuteRpc(photonEvent.CustomData as ExitGames.Client.Photon.Hashtable, player);
				break;
			case 201:
			case 206:
			{
				object[] array2 = (object[])photonEvent[245];
				int networkTime = (int)array2[0];
				short correctPrefix = (short)((array2[1] != null) ? ((byte)array2[1]) : 0);
				object[] array3 = null;
				for (int i = 2; i < array2.Length && array2[i] is object[] data; i++)
				{
					OnSerializeRead(data, player, networkTime, correctPrefix);
				}
				break;
			}
			case 202:
				NetworkInstantiate((ExitGames.Client.Photon.Hashtable)photonEvent.CustomData, player);
				break;
			case 203:
				if (!EnableCloseConnection)
				{
					UnityEngine.Debug.LogWarning("CloseConnection received from " + player?.ToString() + ". PhotonNetwork.EnableCloseConnection is false. Ignoring the request (this client stays in the room).");
				}
				else if (player == null || !player.IsMasterClient)
				{
					UnityEngine.Debug.LogWarning("CloseConnection received from " + player?.ToString() + ". That player is not the Master Client. " + MasterClient?.ToString() + " is.");
				}
				else if (EnableCloseConnection)
				{
					LeaveRoom(becomeInactive: false);
				}
				break;
			case 207:
				if (photonEvent.CustomData is ExitGames.Client.Photon.Hashtable hashtable2)
				{
					int num9 = (int)hashtable2[keyByteZero];
					if (num9 >= 0)
					{
						DestroyPlayerObjects(num9, localOnly: true);
					}
					else
					{
						DestroyAll(localOnly: true);
					}
				}
				break;
			case 254:
				if (CurrentRoom != null && CurrentRoom.AutoCleanUp && (player == null || !player.IsInactive))
				{
					DestroyPlayerObjects(sender, localOnly: true);
				}
				break;
			case 204:
			{
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent.CustomData;
				int key = (int)hashtable[keyByteZero];
				PhotonView val = null;
				if (photonViewList.TryGetValue(key, out val))
				{
					RemoveInstantiatedGO(val.gameObject, localOnly: true);
				}
				else
				{
					UnityEngine.Debug.LogError("Ev Destroy Failed. Could not find PhotonView with instantiationId " + key + ". Sent by actorNr: " + sender);
				}
				break;
			}
			case 209:
			{
				int[] obj2 = (int[])photonEvent.CustomData;
				int num7 = obj2[0];
				int num8 = obj2[1];
				PhotonView photonView3 = GetPhotonView(num7);
				if (photonView3 == null)
				{
					UnityEngine.Debug.LogWarning("Can't find PhotonView of incoming OwnershipRequest. ViewId not found: " + num7);
					break;
				}
				if (LogLevel == PunLogLevel.Informational)
				{
					UnityEngine.Debug.Log(string.Format("OwnershipRequest. actorNr {0} requests view {1} from {2}. current pv owner: {3} is {4}. isMine: {6} master client: {5}", sender, num7, num8, photonView3.OwnerActorNr, photonView3.IsOwnerActive ? "active" : "inactive", MasterClient.ActorNumber, photonView3.IsMine));
				}
				switch (photonView3.OwnershipTransfer)
				{
				case OwnershipOption.Takeover:
				{
					int ownerActorNr = photonView3.OwnerActorNr;
					if (num8 == ownerActorNr || (num8 == 0 && ownerActorNr == MasterClient.ActorNumber) || ownerActorNr == 0)
					{
						Player owner3 = photonView3.Owner;
						photonView3.OwnerActorNr = sender;
						photonView3.ControllerActorNr = sender;
						if (PhotonNetwork.OnOwnershipTransferedEv != null)
						{
							PhotonNetwork.OnOwnershipTransferedEv(photonView3, owner3);
						}
					}
					else if (PhotonNetwork.OnOwnershipTransferFailedEv != null)
					{
						PhotonNetwork.OnOwnershipTransferFailedEv(photonView3, player);
					}
					break;
				}
				case OwnershipOption.Request:
					if (PhotonNetwork.OnOwnershipRequestEv != null)
					{
						PhotonNetwork.OnOwnershipRequestEv(photonView3, player);
					}
					break;
				default:
					UnityEngine.Debug.LogWarning("Ownership mode == " + photonView3.OwnershipTransfer.ToString() + ". Ignoring request.");
					break;
				}
				break;
			}
			case 210:
			{
				int[] obj = (int[])photonEvent.CustomData;
				int num5 = obj[0];
				int num6 = obj[1];
				if (LogLevel >= PunLogLevel.Informational)
				{
					UnityEngine.Debug.Log("Ev OwnershipTransfer. ViewID " + num5 + " to: " + num6 + " Time: " + Environment.TickCount % 1000);
				}
				PhotonView photonView2 = GetPhotonView(num5);
				if (photonView2 != null)
				{
					if (photonView2.OwnershipTransfer == OwnershipOption.Takeover || (photonView2.OwnershipTransfer == OwnershipOption.Request && (player == photonView2.Controller || player == photonView2.Owner)))
					{
						Player owner2 = photonView2.Owner;
						photonView2.OwnerActorNr = num6;
						photonView2.ControllerActorNr = num6;
						if (PhotonNetwork.OnOwnershipTransferedEv != null)
						{
							PhotonNetwork.OnOwnershipTransferedEv(photonView2, owner2);
						}
					}
					else if (LogLevel >= PunLogLevel.Informational)
					{
						if (photonView2.OwnershipTransfer == OwnershipOption.Request)
						{
							UnityEngine.Debug.Log("Failed incoming OwnershipTransfer attempt for '" + photonView2.name + "; " + num5 + " - photonView has OwnershipTransfer set to OwnershipOption.Request, but Player attempting to change owner is not the current owner/controller.");
						}
						else
						{
							UnityEngine.Debug.Log("Failed incoming OwnershipTransfer attempt for '" + photonView2.name + "; " + num5 + " - photonView has OwnershipTransfer set to OwnershipOption.Fixed.");
						}
					}
				}
				else if (LogLevel >= PunLogLevel.ErrorsOnly)
				{
					UnityEngine.Debug.LogErrorFormat("Failed to find a PhotonView with ID={0} for incoming OwnershipTransfer event (newOwnerActorNumber={1}), sender={2}", num5, num6, sender);
				}
				break;
			}
			case 212:
			{
				reusablePVHashset.Clear();
				int[] array = (int[])photonEvent.CustomData;
				int num = 0;
				int num2 = array.Length;
				while (num < num2)
				{
					int num3 = array[num];
					num++;
					int num4 = array[num];
					PhotonView photonView = GetPhotonView(num3);
					if (photonView == null)
					{
						if (LogLevel >= PunLogLevel.ErrorsOnly)
						{
							UnityEngine.Debug.LogErrorFormat("Failed to find a PhotonView with ID={0} for incoming OwnershipUpdate event (newOwnerActorNumber={1}), sender={2}. If you load scenes, make sure to pause the message queue.", num3, num4, sender);
						}
					}
					else
					{
						Player owner = photonView.Owner;
						Player player2 = CurrentRoom.GetPlayer(num4, findMaster: true);
						photonView.OwnerActorNr = num4;
						photonView.ControllerActorNr = num4;
						reusablePVHashset.Add(photonView);
						if (PhotonNetwork.OnOwnershipTransferedEv != null && player2 != owner)
						{
							PhotonNetwork.OnOwnershipTransferedEv(photonView, owner);
						}
					}
					num++;
				}
				{
					foreach (PhotonView item in PhotonViewCollection)
					{
						if (!reusablePVHashset.Contains(item))
						{
							item.RebuildControllerCache();
						}
					}
					break;
				}
			}
			}
		}

		private static void OnOperation(OperationResponse opResponse)
		{
			switch (opResponse.OperationCode)
			{
			case 220:
				if (opResponse.ReturnCode != 0)
				{
					if (LogLevel >= PunLogLevel.Full)
					{
						UnityEngine.Debug.Log("OpGetRegions failed. Will not ping any. ReturnCode: " + opResponse.ReturnCode);
					}
				}
				else if (ConnectMethod == ConnectMethod.ConnectToBest)
				{
					string bestRegionSummaryInPreferences = BestRegionSummaryInPreferences;
					if (LogLevel >= PunLogLevel.Informational)
					{
						UnityEngine.Debug.Log("PUN got region list. Going to ping minimum regions, based on this previous result summary: " + bestRegionSummaryInPreferences);
					}
					NetworkingClient.RegionHandler.PingMinimumOfRegions(OnRegionsPinged, bestRegionSummaryInPreferences);
				}
				break;
			case 226:
				if (Server == ServerConnection.GameServer)
				{
					LoadLevelIfSynced();
				}
				break;
			}
		}

		private static void OnClientStateChanged(ClientState previousState, ClientState state)
		{
			if ((previousState == ClientState.Joined && state == ClientState.Disconnected) || (Server == ServerConnection.GameServer && (state == ClientState.Disconnecting || state == ClientState.DisconnectingFromGameServer)))
			{
				LeftRoomCleanup();
			}
			if (state == ClientState.ConnectedToMasterServer && _cachedRegionHandler != null)
			{
				BestRegionSummaryInPreferences = _cachedRegionHandler.SummaryToCache;
				_cachedRegionHandler = null;
			}
		}

		private static void OnRegionsPinged(RegionHandler regionHandler)
		{
			if (LogLevel >= PunLogLevel.Informational)
			{
				UnityEngine.Debug.Log(regionHandler.GetResults());
			}
			_cachedRegionHandler = regionHandler;
			if (NetworkClientState == ClientState.ConnectedToNameServer && !NetworkingClient.ConnectToRegionMaster(regionHandler.BestRegion.Code))
			{
				NetworkingClient.Disconnect(DisconnectCause.Exception);
			}
		}
	}
}
