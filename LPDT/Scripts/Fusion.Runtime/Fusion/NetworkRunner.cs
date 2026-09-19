#define FUSION_UNITY
#define TRACE
#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fusion.Async;
using Fusion.Encryption;
using Fusion.Matchmaking.Extensions;
using Fusion.Photon.Realtime;
using Fusion.Protocol;
using Fusion.Sockets;
using Fusion.Sockets.Stun;
using Fusion.Statistics;
using JetBrains.Annotations;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fusion
{
	[AddComponentMenu("Fusion/Network Runner")]
	[DisallowMultipleComponent]
	[HelpURL("https://doc.photonengine.com/fusion/v2/manual/network-runner")]
	[ScriptHelp(BackColor = ScriptHeaderBackColor.Red)]
	[DefaultExecutionOrder(50000)]
	public sealed class NetworkRunner : Behaviour, Simulation.ICallbacks
	{
		public delegate void CloudConnectionLostHandler(NetworkRunner networkRunner, ShutdownReason shutdownReason, bool reconnecting);

		public enum CloudConnectionLostMode : byte
		{
			Disabled = 0,
			QuickRejoin = 1,
			MoveSessionToNewRoom = 2
		}

		public enum BuildTypes
		{
			Debug = 0,
			Release = 1
		}

		public enum BuildStages
		{
			Preview = 0,
			Stable = 1
		}

		public enum States
		{
			Starting = 1,
			Running = 2,
			Shutdown = 3
		}

		[Flags]
		private enum ShutdownFlags
		{
			Regular = 1
		}

		public delegate void OnBeforeSpawned(NetworkRunner runner, NetworkObject obj);

		public delegate void ObjectDelegate(NetworkRunner runner, NetworkObject obj);

		public delegate void FailedToResolveNetworkObjectDelegate(NetworkRunner runner, NetworkId id);

		private struct DeferredShutdownParams
		{
			public bool ShutdownRequested;

			public ShutdownReason ShutdownReason;

			public bool DestroyGO;
		}

		private enum SimulationPhase
		{
			None = 0,
			Update = 1,
			Render = 2
		}

		private enum CreateInstanceResult
		{
			Success = 0,
			Failed = 1,
			InProgress = 2,
			Ignore = 3
		}

		[Flags]
		private enum AttachOptions
		{
			LocalSpawn = 1,
			AttachExisting = 2
		}

		[Flags]
		private enum SpawnFlagsInternal
		{
			DontDestroyOnLoad = 1,
			SharedModeStateAuthMasterClient = 2,
			SharedModeStateAuthLocalPlayer = 4,
			Synchronous = 0x10000
		}

		private readonly struct SpawnArgs
		{
			public readonly NetworkObjectTypeId TypeId;

			public readonly Vector3? Position;

			public readonly Quaternion? Rotation;

			public readonly PlayerRef? InputAuthority;

			public readonly object OnBeforeSpawned;

			public readonly NetworkObjectSpawnDelegate Spawned;

			public readonly SpawnFlagsInternal SpawnFlags;

			public readonly NetworkObject ResumeNO;

			public bool Synchronous => (SpawnFlags & SpawnFlagsInternal.Synchronous) != 0;

			public bool DontDestroyOnLoad => (SpawnFlags & SpawnFlagsInternal.DontDestroyOnLoad) != 0;

			public bool? MasterClientOverride
			{
				get
				{
					if ((SpawnFlags & SpawnFlagsInternal.SharedModeStateAuthMasterClient) != 0)
					{
						return true;
					}
					if ((SpawnFlags & SpawnFlagsInternal.SharedModeStateAuthLocalPlayer) != 0)
					{
						return false;
					}
					return null;
				}
			}

			public SpawnArgs(in SpawnArgs other, NetworkObjectSpawnDelegate del)
			{
				this = other;
				if (Spawned != null)
				{
					Spawned = (NetworkObjectSpawnDelegate)Delegate.Combine(Spawned, del);
				}
				else
				{
					Spawned = del;
				}
			}

			public SpawnArgs(NetworkObjectTypeId typeId, Vector3? position, Quaternion? rotation, PlayerRef? inputAuthority, object onBeforeSpawned, NetworkSpawnFlags flags, NetworkObjectSpawnDelegate spawned, bool synchronous, NetworkObject resumeNO)
			{
				TypeId = typeId;
				Position = position;
				Rotation = rotation;
				InputAuthority = inputAuthority;
				OnBeforeSpawned = onBeforeSpawned;
				Spawned = spawned;
				ResumeNO = resumeNO;
				SpawnFlags = (SpawnFlagsInternal)flags;
				if (synchronous)
				{
					SpawnFlags |= SpawnFlagsInternal.Synchronous;
				}
			}

			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("[").Append("TypeId").Append(": ")
					.Append(TypeId);
				if (Position.HasValue)
				{
					stringBuilder.Append(", ").Append("Position").Append(": ")
						.Append(Position);
				}
				if (Rotation.HasValue)
				{
					stringBuilder.Append(", ").Append("Rotation").Append(": ")
						.Append(Rotation);
				}
				if (InputAuthority.HasValue)
				{
					stringBuilder.Append(", ").Append("InputAuthority").Append(": ")
						.Append(InputAuthority);
				}
				if (OnBeforeSpawned != null)
				{
					stringBuilder.Append(", ").Append("OnBeforeSpawned").Append(": ")
						.Append(OnBeforeSpawned);
				}
				if (Spawned != null)
				{
					stringBuilder.Append(", ").Append("Spawned").Append(": ")
						.Append(Spawned);
				}
				if (SpawnFlags != 0)
				{
					stringBuilder.Append(", ").Append("SpawnFlags").Append(": ")
						.Append(SpawnFlags);
				}
				stringBuilder.Append("]");
				return stringBuilder.ToString();
			}
		}

		private HostMigration _lastHostMigrationInfo;

		private byte[] _hostSnapshotTempData;

		private const int HostSnapshotTransferDataSize = 4096;

		internal volatile int LastSnapshotTick = -1;

		internal volatile int LastConfirmedSnapshotTick = -1;

		public static CloudConnectionLostHandler CloudConnectionLost;

		private static CloudConnectionLostMode _cloudConnectionLostCurrentMode = CloudConnectionLostMode.MoveSessionToNewRoom;

		[NonSerialized]
		private DeferredShutdownParams _deferredShutdownParams = default(DeferredShutdownParams);

		[NonSerialized]
		internal Simulation _simulation;

		[NonSerialized]
		private SimulationPhase _simulationPhase;

		[NonSerialized]
		private ShutdownFlags _simulationShutdown = ShutdownFlags.Regular;

		[NonSerialized]
		private SimulationBehaviourUpdater _behaviourUpdater;

		[NonSerialized]
		private List<INetworkRunnerCallbacks> _callbacks;

		[NonSerialized]
		private List<NetworkId> _destroyIdsBuffer = new List<NetworkId>();

		[NonSerialized]
		private readonly Queue<SpawnArgs> _spawnQueue = new Queue<SpawnArgs>();

		internal TaskCompletionSource<bool> _initializeOperation;

		internal bool OnGameStartedInvoked;

		private Queue<ISpawned> _spawnedSimBehaviourQueue;

		private NetworkRunnerFlags _flags;

		[NonSerialized]
		private NetworkProjectConfig _config;

		[NonSerialized]
		private int _ticksExecuted;

		[NonSerialized]
		private INetworkRunnerUpdater _updater;

		[NonSerialized]
		private INetworkObjectInitializer _objectInitializer;

		[NonSerialized]
		private INetworkObjectProvider _objectProvider;

		[NonSerialized]
		internal byte[] _connectionToken;

		[NonSerialized]
		private Dictionary<NetworkObjectTypeId, NetworkObject> _attachableInstances = new Dictionary<NetworkObjectTypeId, NetworkObject>();

		[NonSerialized]
		private bool? _provideInput;

		[NonSerialized]
		private int? _overrideInterestCellSize;

		[NonSerialized]
		private Vector3Int? _overrideInterestCellGridSize;

		private CancellationTokenSource OperationsCancellationTokenSource = new CancellationTokenSource();

		private readonly List<NetworkObject> _remotePrefabsWaitingForSpawnedCallback = new List<NetworkObject>();

		private readonly Queue<NetworkId> _remoteCreateQueue = new Queue<NetworkId>();

		private readonly Queue<NetworkId> _remoteCreateNestedQueue = new Queue<NetworkId>();

		private readonly Queue<NetworkId> _remoteDestroyQueue = new Queue<NetworkId>();

		private Action<NetworkRunner> _onGameStartAction;

		private Dictionary<NetworkObjectTypeId, NetworkObjectTypeDescriptor> _knownTypes = new Dictionary<NetworkObjectTypeId, NetworkObjectTypeDescriptor>();

		public static NetworkRunnerRpcEventDelegate DebugRpcEvent;

		internal Stack<NetworkObjectInactivityGuard> _inactivityGuardPool = new Stack<NetworkObjectInactivityGuard>();

		private static List<NetworkRunner> _instances = new List<NetworkRunner>();

		private bool _simulateMultiPeerPhysicsScenes = true;

		private INetworkSceneManager _sceneManager;

		[NonSerialized]
		private NetworkSceneInfo _sceneInfoInitial;

		[NonSerialized]
		private NetworkSceneInfoChangeSource _sceneInfoChangeSource = NetworkSceneInfoChangeSource.Initial;

		[NonSerialized]
		private NetworkSceneInfo _sceneInfoSnapshot;

		[NonSerialized]
		private TaskCompletionSource<int> _sceneLoadInitialTCS = new TaskCompletionSource<int>();

		private const bool DefaultSetActiveOnLoad = false;

		private bool _alreadyInitialized = false;

		public Func<string, ServerConnection, string> CloudAddressRewriter = null;

		internal AsyncOperationHandler<ShutdownReason> StartGameOperation;

		internal CloudServices _cloudServices;

		private static string _cachedRegionSummary = string.Empty;

		public bool IsResume => _simulation != null && _simulation.IsResume && _initializeOperation != null && !_initializeOperation.Task.IsCompleted;

		public static CloudConnectionLostMode CloudConnectionLostCurrentMode
		{
			get
			{
				return _cloudConnectionLostCurrentMode;
			}
			set
			{
				InternalLogStreams.LogDebug?.Log($"Cloud Connection Lost Mode set to {value}");
				_cloudConnectionLostCurrentMode = value;
			}
		}

		public static BuildTypes BuildType => BuildTypes.Debug;

		public static BuildStages BuildStage => BuildStages.Stable;

		public bool IsSimulationUpdating => _simulationPhase == SimulationPhase.Update;

		internal bool IsInitialized => _initializeOperation != null && _initializeOperation.Task.IsCompleted && _initializeOperation.Task.Result;

		private bool IsForwardingUnregisteredRpcsEnabled => _flags.Has(NetworkRunnerFlags.AllowUnrecognizedRpcs);

		public bool ProvideInput
		{
			get
			{
				return _provideInput == true;
			}
			set
			{
				_provideInput = value;
			}
		}

		public Topologies Topology => _simulation?.Config.Topology ?? ((Topologies)0);

		internal Simulation Simulation => _simulation;

		public SimulationModes Mode => _simulation?.Mode ?? ((SimulationModes)0);

		public SimulationStages Stage => _simulation?.Stage ?? ((SimulationStages)0);

		public float DeltaTime => _simulation?.DeltaTime ?? 0f;

		public float SimulationTime => (float)(_simulation?.Time ?? 0.0);

		public float LocalRenderTime
		{
			get
			{
				float val = (float)(_simulation?._time.Now().Local - (double)DeltaTime).GetValueOrDefault();
				return Math.Max(val, 0f);
			}
		}

		public float RemoteRenderTime => (float)(_simulation?._time.Now().Remote ?? 0.0);

		public bool IsRunning => _simulation?.IsRunning ?? false;

		public bool IsShutdown => _simulationShutdown != (ShutdownFlags)0;

		public bool HasCustomPlugin => _flags.Has(NetworkRunnerFlags.CustomPlugin);

		internal bool IsShutdownDeferred => _deferredShutdownParams.ShutdownRequested;

		private bool IsRegularShutdown
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (_simulationShutdown & ShutdownFlags.Regular) != 0;
			}
		}

		public float LocalAlpha => _simulation?.LocalAlpha ?? 0f;

		public Tick LatestServerTick => _simulation?.LatestServerTick ?? ((Tick)0);

		public bool IsStarting => !IsRunning && !IsShutdown;

		public bool IsClient => _simulation?.IsClient ?? false;

		public bool IsConnectedToServer => IsClient && ((Simulation.Client)_simulation).IsConnectedToServer;

		public bool IsServer => _simulation?.IsServer ?? false;

		public bool IsPlayer => _simulation?.IsPlayer ?? false;

		public bool IsSinglePlayer => _simulation?.IsSinglePlayer ?? false;

		public bool IsLastTick => _simulation?.IsLastTick ?? false;

		public bool IsFirstTick => _simulation?.IsFirstTick ?? false;

		public bool IsForward => _simulation?.IsForward ?? false;

		public bool IsResimulation => _simulation?.IsResimulation ?? false;

		public int TickRate => _simulation?.TickRate ?? 0;

		public States State => IsShutdown ? States.Shutdown : ((!IsRunning) ? States.Starting : States.Running);

		public PlayerRef LocalPlayer => _simulation?.LocalPlayer ?? default(PlayerRef);

		public Tick InputTick
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _simulation?.InputTick ?? default(Tick);
			}
		}

		public Tick Tick
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return _simulation?.Tick ?? default(Tick);
			}
		}

		public NetworkProjectConfig Config => _config;

		public NetworkPrefabTable Prefabs => _config?.PrefabTable;

		public int TicksExecuted => _ticksExecuted;

		public IEnumerable<PlayerRef> ActivePlayers => _simulation?.ActivePlayers ?? Enumerable.Empty<PlayerRef>();

		public INetworkObjectProvider ObjectProvider => _objectProvider;

		public int ReliableDataSendRate
		{
			get
			{
				return _simulation?.ReliableDataSendRate ?? 0;
			}
			set
			{
				if (_simulation != null)
				{
					_simulation.ReliableDataSendRate = value;
				}
				else
				{
					InternalLogStreams.LogError?.Log("Cannot set ReliableDataSendRate before NetworkRunner has started. Await the completion of the StartGame task before accessing this property.");
				}
			}
		}

		public ref Simulation.AreaOfInterestState AreaOfInterest => ref _simulation.AreaOfInterestInstance;

		public NetAddress LocalAddress => _simulation?.LocalAddress ?? default(NetAddress);

		public INetworkSceneManager SceneManager => _sceneManager;

		internal CancellationToken OperationsCancellationToken
		{
			get
			{
				if (OperationsCancellationTokenSource == null || OperationsCancellationTokenSource.IsCancellationRequested)
				{
					InternalLogStreams.LogWarn?.Log("Trying to access an invalid OperationsCancellationTokenSource");
					return CancellationToken.None;
				}
				return OperationsCancellationTokenSource.Token;
			}
		}

		public HitboxManager LagCompensation => GetBehaviour<HitboxManager>();

		public static IReadOnlyList<NetworkRunner> Instances => _instances;

		public int FixedUpdateCount { get; private set; }

		public bool IsSceneAuthority => IsServer || IsSharedModeMasterClient;

		public bool IsSceneManagerBusy
		{
			get
			{
				if (_sceneLoadInitialTCS != null)
				{
					return true;
				}
				INetworkSceneManager sceneManager = _sceneManager;
				if (sceneManager != null && sceneManager.IsBusy)
				{
					return true;
				}
				return false;
			}
		}

		public NetworkSceneInfo SceneInfo
		{
			get
			{
				if (_simulation != null && _simulation.TryGetStruct(NetworkId.SceneInfo, out var meta))
				{
					return meta.GetStructData<NetworkSceneInfo>();
				}
				return default(NetworkSceneInfo);
			}
		}

		public Scene SimulationUnityScene => SceneManager?.MainRunnerScene ?? default(Scene);

		bool Simulation.ICallbacks.CanReceivePlayerJoinLeaveCallbacks => IsInitialized && !IsSceneManagerBusy;

		PlayerRef Simulation.ICallbacks.LocalPlayerRef => IsSinglePlayer ? PlayerRef.FromIndex(1) : (_cloudServices?.LocalPlayerRef ?? default(PlayerRef));

		bool Simulation.ICallbacks.IsSharedModeMasterClient => IsSharedModeMasterClient;

		private bool IsServerOrPlugin => IsServer;

		public bool CanSpawn => IsServer || Topology == Topologies.Shared;

		public bool IsCloudReady => _cloudServices?.IsCloudReady == true;

		public bool IsInSession => _cloudServices?.IsInRoom == true;

		public string UserId => IsCloudReady ? _cloudServices.UserId : null;

		public AuthenticationValues AuthenticationValues => IsCloudReady ? _cloudServices.AuthenticationValues : null;

		public GameMode GameMode { get; private set; }

		public SessionInfo SessionInfo { get; private set; } = new SessionInfo();

		public LobbyInfo LobbyInfo { get; private set; } = new LobbyInfo();

		public ConnectionType CurrentConnectionType
		{
			get
			{
				if (IsConnectedToServer)
				{
					if (((Simulation.Client)_simulation).ServerAddress.IsRelayAddr)
					{
						return ConnectionType.Relayed;
					}
					return ConnectionType.Direct;
				}
				return ConnectionType.None;
			}
		}

		public NATType NATType => (_cloudServices != null) ? _cloudServices.NATType : NATType.Invalid;

		public bool IsSharedModeMasterClient => GameMode == GameMode.Shared && IsClient && (_cloudServices?.IsMasterClient ?? false);

		public event ObjectDelegate ObjectAcquired;

		public event FailedToResolveNetworkObjectDelegate FailedToResolveNetworkObject;

		public static event NetworkRunnerDebugEventDelegate DebugEvent;

		public async Task<bool> PushHostMigrationSnapshot()
		{
			try
			{
				return await SendHostMigrationSnapshot();
			}
			catch (Exception ex)
			{
				Exception e = ex;
				InternalLogStreams.LogError?.Log(e);
			}
			return false;
		}

		public bool ClearHostMigrationSnapshot()
		{
			try
			{
				return SendHostMigrationClear();
			}
			catch (Exception error)
			{
				InternalLogStreams.LogError?.Log(error);
			}
			return false;
		}

		public unsafe Dictionary<PlayerRef, NetworkId> GetResumeSnapshotNetworkObjectPlayerObjects()
		{
			Assert.Check(IsServer, "Only a Server instance can execute this action");
			Assert.Check(Simulation.IsResume, "Current Simulation does not come from a Resume Server Snapshot");
			Dictionary<PlayerRef, NetworkId> dictionary = new Dictionary<PlayerRef, NetworkId>();
			Simulation.Server server = (Simulation.Server)Simulation;
			(Dictionary<NetworkId, NetworkObjectHeaderPtr>, Dictionary<NetworkId, List<NetworkId>>) resumeObjectHeader = server.GetResumeObjectHeader();
			Dictionary<NetworkId, NetworkObjectHeaderPtr> item = resumeObjectHeader.Item1;
			Dictionary<NetworkId, List<NetworkId>> item2 = resumeObjectHeader.Item2;
			Simulation.PlayerSimulationData playerSimulationData = default(Simulation.PlayerSimulationData);
			foreach (NetworkObjectHeaderPtr value in item.Values)
			{
				if (value.Type.IsStruct && !(value.Type != NetworkObjectTypeId.PlayerData))
				{
					playerSimulationData.Actor = 0;
					playerSimulationData.Object = NetworkId.None;
					playerSimulationData.Player = PlayerRef.None;
					Simulation.PlayerSimulationData* ptr = &playerSimulationData;
					*ptr = *(Simulation.PlayerSimulationData*)((byte*)value.Ptr + (nint)20 * (nint)4 + (nint)14 * (nint)4);
					if (playerSimulationData.Player != PlayerRef.None && playerSimulationData.Object != NetworkId.None)
					{
						InternalLogStreams.LogTraceHostMigration?.Log($"Restore PlayerSimulationData: {playerSimulationData.Player} <> {playerSimulationData.Object}");
						dictionary[playerSimulationData.Player] = playerSimulationData.Object;
					}
				}
			}
			return dictionary;
		}

		public IEnumerable<NetworkObject> GetResumeSnapshotNetworkObjects()
		{
			Assert.Check(IsServer, "Only a Server instance can execute this action");
			Assert.Check(Simulation.IsResume, "Current Simulation does not come from a Resume Server Snapshot");
			Simulation.Server server = (Simulation.Server)Simulation;
			var (headerMapping, nestedMapping) = server.GetResumeObjectHeader();
			foreach (NetworkObjectHeaderPtr header in headerMapping.Values)
			{
				if (!header.Type.IsSceneObject && !header.Type.IsStruct)
				{
					NetworkObject resumeObj;
					try
					{
						resumeObj = GetNetworkObjectFromResumeSnapshot(header, headerMapping, nestedMapping);
					}
					catch
					{
						continue;
					}
					if (BehaviourUtils.IsAlive(resumeObj))
					{
						yield return resumeObj;
						resumeObj.RuntimeFlags |= NetworkObjectRuntimeFlags.IsDestroyed;
						UnityEngine.Object.Destroy(resumeObj.gameObject);
					}
				}
			}
		}

		public IEnumerable<(NetworkObject, NetworkObjectHeaderPtr)> GetResumeSnapshotNetworkSceneObjects()
		{
			Assert.Check(IsServer, "Only a Server instance can execute this action");
			Assert.Check(Simulation.IsResume, "Current Simulation does not come from a Resume Server Snapshot");
			Simulation.Server server = (Simulation.Server)Simulation;
			(Dictionary<NetworkId, NetworkObjectHeaderPtr>, Dictionary<NetworkId, List<NetworkId>>) resumeObjectHeader = server.GetResumeObjectHeader();
			var (headerMapping, _) = resumeObjectHeader;
			_ = resumeObjectHeader.Item2;
			foreach (NetworkObjectHeaderPtr header in headerMapping.Values)
			{
				if (!header.Type.IsSceneObject || header.Type.IsStruct)
				{
					continue;
				}
				if (!Simulation.TryGetSceneInstance(header.Type, out var resumeObj))
				{
					InternalLogStreams.LogTraceHostMigration?.Warn($"Unable to find Scene Object with ID {header.Id}");
					continue;
				}
				if (BehaviourUtils.IsAlive(resumeObj))
				{
					yield return (resumeObj, header);
				}
				resumeObj = null;
			}
		}

		private void SetHostMigrationBandwidth(int bytePerSecond)
		{
		}

		private IEnumerator RunHostMigrationResume(NetworkRunnerInitializeArgs args)
		{
			yield return new WaitUntil(() => !IsSceneManagerBusy);
			args.HostMigrationResume?.Invoke(this);
			CallbackInterfaceInvoker.IAfterHostMigration(_behaviourUpdater);
			Simulation simulation = Simulation;
			if (simulation is Simulation.Server server)
			{
				server.DisposeHostMigration();
			}
			SetInitializationDone(args);
		}

		private unsafe NetworkObject GetNetworkObjectFromResumeSnapshot(NetworkObjectHeaderPtr networkObjectPtr, Dictionary<NetworkId, NetworkObjectHeaderPtr> headerList, Dictionary<NetworkId, List<NetworkId>> nestedMapping)
		{
			if (networkObjectPtr.Ptr->Type.IsSceneObject)
			{
				return null;
			}
			bool dontDestroyOnLoad = networkObjectPtr.Ptr->Flags.Has(NetworkObjectHeaderFlags.DontDestroyOnLoad);
			NetworkObject result;
			CreateInstanceResult createInstanceResult = TryAcquireInstance(networkObjectPtr.Ptr->Type, null, out result, synchronous: true, dontDestroyOnLoad);
			if (createInstanceResult == CreateInstanceResult.Success)
			{
				InitializeTempNetworkObjectInstance(networkObjectPtr.Ptr, result);
				if (nestedMapping.TryGetValue(networkObjectPtr.Ptr->Id, out var value))
				{
					for (int i = 0; i < value.Count; i++)
					{
						NetworkId key = value[i];
						NetworkObjectHeaderPtr networkObjectHeaderPtr = headerList[key];
						NetworkObject instance = result.NestedObjects[i];
						Assert.Check(networkObjectHeaderPtr.Ptr->NestingRoot.Equals(networkObjectPtr.Ptr->Id), "Nested NetworkObject with wrong NetworkId for the Nesting Root");
						InitializeTempNetworkObjectInstance(networkObjectHeaderPtr.Ptr, instance);
					}
				}
			}
			else
			{
				InternalLogStreams.LogError?.Log($"Failed to create instance for {*networkObjectPtr.Ptr} with {createInstanceResult}");
			}
			return result;
		}

		private unsafe void InitializeTempNetworkObjectInstance(NetworkObjectHeader* header, NetworkObject instance)
		{
			instance.Ptr = (int*)header;
			if (instance.NetworkedBehaviours != null)
			{
				int num = NetworkObject.GetBehaviourDataOffset(instance);
				for (int i = 0; i < instance.NetworkedBehaviours.Length; i++)
				{
					instance.NetworkedBehaviours[i].WordOffset = num;
					int wordCount = NetworkBehaviourUtils.GetWordCount(instance.NetworkedBehaviours[i]);
					instance.NetworkedBehaviours[i].WordCount = wordCount;
					instance.NetworkedBehaviours[i].MakeOwned(this, instance, i);
					instance.NetworkedBehaviours[i].Ptr = instance.Ptr + instance.NetworkedBehaviours[i].WordOffset;
					num += wordCount;
				}
			}
		}

		internal void SetupHostMigration(HostMigration hostMigration)
		{
			_lastHostMigrationInfo = hostMigration;
		}

		internal void StartHostMigration(Snapshot snapshot = null)
		{
			InternalLogStreams.LogTraceHostMigration?.Log($"StartHostMigration: Has Snapshot? {snapshot != null}");
			Assert.Always(_lastHostMigrationInfo != null, "Invalid Host Migration info");
			GameMode gameMode = GameMode.Client;
			switch (_lastHostMigrationInfo.PeerMode)
			{
			case PeerMode.Server:
				gameMode = GameMode.Host;
				break;
			case PeerMode.Client:
				gameMode = GameMode.Client;
				break;
			default:
				Assert.Fail("Invalid New Game Mode on Host Migration.");
				break;
			}
			CloudCommunicator cloudCommunicator = _cloudServices.ExtractCommunicator();
			HostMigrationToken migrationToken = new HostMigrationToken(snapshot, cloudCommunicator, gameMode);
			InvokeHostMigration(migrationToken);
		}

		private void InvokeHostMigration(HostMigrationToken migrationToken)
		{
			try
			{
				for (int i = 0; i < _callbacks.Count; i++)
				{
					_callbacks[i].OnHostMigration(this, migrationToken);
				}
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(error);
			}
		}

		private bool SendHostMigrationClear()
		{
			if (!ChecksForHostMigration())
			{
				return false;
			}
			Simulation simulation = _simulation;
			if (simulation is Simulation.Server server && simulation.MetaMigrationCount > 0)
			{
				server.ClearHostMigrationData();
			}
			_cloudServices.SendClearSnapshot();
			return true;
		}

		internal Task<bool> SendHostMigrationSnapshot()
		{
			if (!ChecksForHostMigration())
			{
				return Task.FromResult(result: false);
			}
			if (LastConfirmedSnapshotTick < LastSnapshotTick)
			{
				InternalLogStreams.LogDebug?.Warn($"Host Snapshot for Tick {LastSnapshotTick} was not confirmed yet. Ignore.");
				return Task.FromResult(result: false);
			}
			if (_hostSnapshotTempData == null)
			{
				_hostSnapshotTempData = new byte[4096];
			}
			return Task.Run(delegate
			{
				int lastSnapshotTick = LastSnapshotTick;
				if (GetServerSnapshot(ref _hostSnapshotTempData, out var tick, out var idCounter, out var length) && lastSnapshotTick == Interlocked.CompareExchange(ref LastSnapshotTick, tick, lastSnapshotTick))
				{
					_cloudServices.SendStateSnapshot(_hostSnapshotTempData, length, tick, idCounter);
					return true;
				}
				return false;
			}, OperationsCancellationToken);
		}

		private bool ChecksForHostMigration()
		{
			if (!IsServer || GameMode != GameMode.Host || !IsInitialized || !IsCloudReady)
			{
				InternalLogStreams.LogDebug?.Warn("Fusion peer is not running or is not connected to the Photon Cloud. Ignore.");
				return false;
			}
			switch (_cloudServices.CurrentJoinStage)
			{
			case JoinProcessStage.Idle:
			case JoinProcessStage.Joining:
				InternalLogStreams.LogDebug?.Warn("Fusion peer is waiting for Join confirmation. Ignore.");
				return false;
			case JoinProcessStage.Fail:
				InternalLogStreams.LogDebug?.Warn("Fusion peer failed to join Session. Ignore.");
				return false;
			default:
				if ((int)_cloudServices.CurrentProtocolMessageVersion < 8)
				{
					InternalLogStreams.LogDebug?.Warn("Fusion Plugin does not support Host Migration. Ignore.");
					return false;
				}
				return true;
			}
		}

		private bool GetServerSnapshot(ref byte[] data, out Tick tick, out uint idCounter, out int length)
		{
			if (_simulation is Simulation.Server server)
			{
				length = server.WriteHostMigrationData(ref data, data.Length);
				tick = server.Tick;
				idCounter = server.IdCounter;
				return length > 0;
			}
			tick = 0;
			idCounter = 0u;
			length = 0;
			return false;
		}

		internal static void ResetStatics()
		{
			_instances.Clear();
		}

		private void OnValidate()
		{
			if ((bool)GetComponent<NetworkObject>())
			{
				InternalLogStreams.LogWarn?.Log("NetworkRunner will not work properly with NetworkObject on the same GameObject.");
			}
		}

		public void Disconnect(PlayerRef player, byte[] token = null)
		{
			if (_simulation != null)
			{
				if (_simulation is Simulation.Server server)
				{
					server.Disconnect(player, token);
				}
				else
				{
					InternalLogStreams.LogError?.Log(this, "Only server can disconnect players");
				}
			}
		}

		internal void Disconnect(NetAddress address)
		{
			if (_simulation != null)
			{
				if (_simulation is Simulation.Server server)
				{
					server.Disconnect(address);
				}
				else
				{
					InternalLogStreams.LogError?.Log(this, "Only server can disconnect players");
				}
			}
		}

		internal void RequestRejoinAllPlayers(string sessionID)
		{
			foreach (PlayerRef activePlayer in ActivePlayers)
			{
				RequestRejoin(activePlayer, sessionID);
			}
		}

		internal void RequestRejoin(PlayerRef playerRef, string sessionID)
		{
			if (_simulation != null)
			{
				if (_simulation is Simulation.Server server)
				{
					server.RequestRejoin(playerRef, sessionID);
				}
				else
				{
					InternalLogStreams.LogError?.Log(this, "Only server can request players to re-join session");
				}
			}
		}

		internal void Connect(NetAddress address, byte[] token, byte[] uniqueId)
		{
			if (IsServer)
			{
				throw new InvalidOperationException("Only clients can connect");
			}
			((Simulation.Client)Simulation).Connect(address, token, uniqueId);
		}

		[EditorButton("Shutdown", EditorButtonVisibility.PlayMode, 0, false)]
		internal void ShutdownAction()
		{
			Shutdown();
		}

		public Task Shutdown(bool destroyGameObject = true, ShutdownReason shutdownReason = ShutdownReason.Ok, bool forceShutdownProcedure = false)
		{
			if (_simulationPhase != SimulationPhase.None)
			{
				InternalLogStreams.LogDebug?.Log(this, $"Deferring shutdown with ({destroyGameObject}, {shutdownReason}, {forceShutdownProcedure}) due to phase being {_simulationPhase}");
				_deferredShutdownParams = new DeferredShutdownParams
				{
					ShutdownRequested = true,
					ShutdownReason = shutdownReason,
					DestroyGO = destroyGameObject
				};
				_simulation?.NotifyWaitingForShutdown();
				return Task.CompletedTask;
			}
			_deferredShutdownParams = default(DeferredShutdownParams);
			InternalLogStreams.LogDebug?.Log(this, "Starting to Shutdown with (" + string.Format("{0}={1}, ", "destroyGameObject", destroyGameObject) + string.Format("{0}={1}, ", "shutdownReason", shutdownReason) + string.Format("{0}={1}, ", "forceShutdownProcedure", forceShutdownProcedure) + string.Format("{0}={1})", "IsShutdown", IsShutdown));
			RegisterNetworkCallbacks();
			if (IsShutdown)
			{
				RemoveInstance(this);
				if (!IsRegularShutdown && forceShutdownProcedure)
				{
					InvokeOnShutdownCallbacks();
					return ContinueTasksWithDestroy(new Task[1] { DisconnectFromCloud() });
				}
				return Task.CompletedTask;
			}
			_simulationShutdown |= ShutdownFlags.Regular;
			try
			{
				_simulation?.ShutdownNativeSocket();
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(error);
			}
			try
			{
				_updater?.Shutdown(this);
			}
			catch (Exception error2)
			{
				InternalLogStreams.LogException?.Log(error2);
			}
			RemoveInstance(this);
			InvokeOnShutdownCallbacks();
			if (_simulation?.Objects != null)
			{
				foreach (NetworkObjectMeta item in _simulation.Objects.Values.ToList())
				{
					if (BehaviourUtils.IsAlive(item.Instance))
					{
						DestroyNetworkObjectInternal(item.Instance, NetworkObjectDestroyFlags.DestroyedByShutdown);
					}
				}
			}
			List<SimulationBehaviour> list = new List<SimulationBehaviour>();
			_behaviourUpdater?.GetAllSimulationBehaviours(list);
			foreach (SimulationBehaviour item2 in list)
			{
				if (item2 is IDespawned despawned)
				{
					try
					{
						despawned.Despawned(this, hasState: false);
					}
					catch (Exception error3)
					{
						InternalLogStreams.LogException?.Log(error3);
					}
				}
			}
			_objectProvider?.Shutdown(this);
			_simulation?.Dispose();
			_simulation = null;
			_sceneManager?.Shutdown();
			_sceneManager = null;
			GameMode = (GameMode)0;
			SessionInfo = new SessionInfo();
			return ContinueTasksWithDestroy(new Task[1] { DisconnectFromCloud() });
			Task ContinueTasksWithDestroy(params Task[] precedingTasks)
			{
				return TaskManager.ContinueWhenAll(precedingTasks, delegate
				{
					InternalLogStreams.LogDebug?.Log(this, "Shutdown complete.");
					if (destroyGameObject && (bool)this && (bool)base.gameObject)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
					if (!OperationsCancellationTokenSource.IsCancellationRequested)
					{
						OperationsCancellationTokenSource.Cancel();
					}
					OperationsCancellationTokenSource.Dispose();
					return Task.CompletedTask;
				}, OperationsCancellationToken);
			}
			void InvokeOnShutdownCallbacks()
			{
				for (int i = 0; i < _callbacks.Count; i++)
				{
					try
					{
						_callbacks[i].OnShutdown(this, shutdownReason);
					}
					catch (Exception error4)
					{
						InternalLogStreams.LogException?.Log(this, error4);
					}
				}
			}
		}

		private INetSocket CreateCloudSocket()
		{
			if (_cloudServices == null || !_cloudServices.IsCloudReady)
			{
				throw new InvalidOperationException("Fusion Relay Client is not ready. Make sure the call Runner.ConnectToCloud before start with Runner.StartGame");
			}
			if (!_cloudServices.IsNATPunchthroughEnabled || FusionPlatform.IsWebGL)
			{
				return new NetSocketRelay(_cloudServices.Communicator);
			}
			return new NetSocketHybrid(_cloudServices.Communicator);
		}

		private void SetInitializationDone(NetworkRunnerInitializeArgs args)
		{
			_initializeOperation?.TrySetResult(result: true);
			_cloudServices?.StartBackgroundCloudServices();
		}

		internal void OnRuntimeConfigReady()
		{
			OnGameStartedInvoked = true;
			while (_spawnedSimBehaviourQueue.Count > 0)
			{
				ISpawned spawned = _spawnedSimBehaviourQueue.Dequeue();
				try
				{
					spawned.Spawned();
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
				}
			}
			StartGameOperation?.SetResult(ShutdownReason.Ok);
			InvokeOnGameStartedCallback();
		}

		private bool TryGetInterfaceWithDefaultType<T>(string defaultTypeName, out T result) where T : class
		{
			result = GetComponent<T>();
			if (result != null)
			{
				return true;
			}
			Type type = Type.GetType(defaultTypeName);
			if (type != null)
			{
				InternalLogStreams.LogDebug?.Log(this, "No " + typeof(T).FullName + " provided and there is no matching component, creating " + defaultTypeName);
				result = (T)(object)base.gameObject.AddComponent(type);
				return true;
			}
			result = null;
			return false;
		}

		internal void InvokeOnGameStartedCallback()
		{
			try
			{
				_onGameStartAction?.Invoke(this);
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(this, error);
			}
		}

		internal Task<bool> Initialize(NetworkRunnerInitializeArgs args)
		{
			_initializeOperation = new TaskCompletionSource<bool>();
			_onGameStartAction = args.OnGameStarted;
			OnGameStartedInvoked = false;
			_spawnedSimBehaviourQueue = new Queue<ISpawned>();
			if (!args.SimulationMode.HasValue)
			{
				throw new InvalidOperationException("SimulationMode must have a value");
			}
			if (!args.Address.HasValue && !args.IsSinglePlayer)
			{
				throw new InvalidOperationException("Address must have a value");
			}
			if (args.Config == null)
			{
				throw new InvalidOperationException("Config must have a value");
			}
			if (_callbacks == null)
			{
				_callbacks = new List<INetworkRunnerCallbacks>();
			}
			INetSocket netSocket = ((!args.IsSinglePlayer) ? CreateCloudSocket() : new NetSocketNull());
			Assert.Check(netSocket, "socket");
			_flags = args.Flags;
			_config = SetupNetworkProjectConfig(args);
			_connectionToken = args.ConnectionToken;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (args.ObjectProvider == null)
			{
				string text = "Fusion.NetworkObjectProviderDefault, Fusion.Unity";
				if (!TryGetInterfaceWithDefaultType<INetworkObjectProvider>(text, out _objectProvider))
				{
					InternalLogStreams.LogError?.Log(this, "No ObjectProvider passed and the default provider component type (" + text + ") was not found. Fusion will not be able to spawn prefabs.");
					_objectProvider = new NetworkObjectProviderDummy();
				}
			}
			else
			{
				_objectProvider = args.ObjectProvider;
			}
			if (_overrideInterestCellSize.HasValue)
			{
				_config.Simulation.InterestManagementConfig.AreaOfInterestCellSize = _overrideInterestCellSize.Value;
			}
			if (_overrideInterestCellGridSize.HasValue)
			{
				_config.Simulation.InterestManagementConfig.AreaOfInterestCellGridSize = _overrideInterestCellGridSize.Value;
			}
			SimulationArgs args2 = default(SimulationArgs);
			args2.Mode = args.SimulationMode.Value;
			args2.Config = _config;
			args2.Callbacks = this;
			args2.Socket = netSocket;
			args2.Address = args.Address.GetValueOrDefault();
			args2.ResumeTick = args.ResumeTick.GetValueOrDefault();
			args2.ResumeState = args.ResumeState;
			args2.ResumeNetworkId = args.ResumeId.GetValueOrDefault();
			if (args2.IsServer)
			{
				_simulation = new Simulation.Server(args2);
			}
			else
			{
				args2.ResumeTick = default(Tick);
				args2.ResumeState = null;
				args2.ResumeNetworkId = default(NetworkId);
				_simulation = new Simulation.Client(args2);
			}
			_simulation.Runner = this;
			_behaviourUpdater = new SimulationBehaviourUpdater(_config);
			_behaviourUpdater.BuildTypeOrder(args.CustomCallbackInterfaces);
			_simulationShutdown = (ShutdownFlags)0;
			_deferredShutdownParams = default(DeferredShutdownParams);
			if (args.SceneManager == null)
			{
				string text2 = "Fusion.NetworkSceneManagerDefault, Fusion.Unity";
				if (!TryGetInterfaceWithDefaultType<INetworkSceneManager>(text2, out _sceneManager))
				{
					InternalLogStreams.LogError?.Log(this, "No SceneManager passed and the default provider component type (" + text2 + ") was not found. Fusion will not be able to attach to scene NetworkObjects.");
					_sceneManager = new NetworkSceneManagerDummy();
				}
			}
			else
			{
				_sceneManager = args.SceneManager;
			}
			if (args.Scene.HasValue)
			{
				Assert.Always(_simulation.TryGetStruct(NetworkId.SceneInfo, out var meta), "_simulation.TryGetStruct(NetworkId.SceneInfo, out var sceneInfoMeta)");
				_sceneInfoInitial = (meta.GetStructData<NetworkSceneInfo>() = args.Scene.GetValueOrDefault());
			}
			if (args.Updater == null)
			{
				_updater = new NetworkRunnerUpdaterDefault();
			}
			else
			{
				_updater = args.Updater;
			}
			if (args.ObjectInitializer == null)
			{
				_objectInitializer = new NetworkObjectInitializerUnity();
			}
			else
			{
				_objectInitializer = args.ObjectInitializer;
			}
			_objectProvider?.Initialize(this);
			try
			{
				_sceneManager.Initialize(this);
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(this, error);
			}
			SimulationBehaviour[] componentsInChildren = GetComponentsInChildren<SimulationBehaviour>();
			foreach (SimulationBehaviour simulationBehaviour in componentsInChildren)
			{
				if (simulationBehaviour.enabled)
				{
					AddSimulationBehaviour(simulationBehaviour);
				}
			}
			if (_config.LagCompensation.Enabled)
			{
				GetSingleton<HitboxManager>();
			}
			InternalLogStreams.LogDebug?.Log(this, string.Format("Starting with {0}:\n{1}", "NetworkProjectConfig", _config));
			AddInstance(this);
			if (!_provideInput.HasValue)
			{
				ProvideInput = Simulation.IsPlayer;
			}
			try
			{
				_updater.Initialize(this);
			}
			catch (Exception error2)
			{
				InternalLogStreams.LogException?.Log(this, error2);
			}
			_cachedRegionSummary = _cloudServices?.CachedRegionSummary ?? string.Empty;
			if (Simulation.IsServer && Simulation.IsResume)
			{
				StartCoroutine(RunHostMigrationResume(args));
			}
			else
			{
				SetInitializationDone(args);
			}
			return _initializeOperation.Task;
		}

		public void SinglePlayerPause()
		{
			Simulation.SinglePlayerSetPaused(paused: true);
		}

		public void SinglePlayerContinue()
		{
			Simulation.SinglePlayerSetPaused(paused: false);
		}

		public void SinglePlayerPause(bool paused)
		{
			Simulation.SinglePlayerSetPaused(paused);
		}

		public int GetInterfaceListsCount(Type type)
		{
			Assert.Check(type.IsInterface, "type.IsInterface");
			return _behaviourUpdater.GetCallbackCount(type);
		}

		public SimulationBehaviourListScope GetInterfaceListHead(Type type, int index, out SimulationBehaviour head)
		{
			return _behaviourUpdater.GetCallbackHead(type, index, out head);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SimulationBehaviour GetInterfaceListPrev(SimulationBehaviour behaviour)
		{
			return behaviour.Prev;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SimulationBehaviour GetInterfaceListNext(SimulationBehaviour behaviour)
		{
			return behaviour.Next;
		}

		public static Task<List<RegionInfo>> GetAvailableRegions(string appId = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			return FusionRealtimeProxy.GetEnabledRegions(appId, cancellationToken);
		}

		public int? GetPlayerActorId(PlayerRef player)
		{
			switch (Simulation.Config.Topology)
			{
			case Topologies.ClientServer:
			{
				if (Simulation.IsServer && _cloudServices != null && _cloudServices.TryGetActorIdByUniqueId(Simulation.GetPlayerUniqueId(player), out var actorId))
				{
					return actorId;
				}
				break;
			}
			case Topologies.Shared:
				return Simulation.GetPlayerActorId(player);
			}
			return null;
		}

		public string GetPlayerUserId(PlayerRef player = default(PlayerRef))
		{
			if (!IsCloudReady)
			{
				return null;
			}
			if (LocalPlayer == player || player == default(PlayerRef))
			{
				return UserId;
			}
			int? playerActorId = GetPlayerActorId(player);
			return (!playerActorId.HasValue) ? null : _cloudServices?.GetActorUserID(playerActorId.Value);
		}

		public void SetPlayerObject(PlayerRef player, NetworkObject networkObject)
		{
			if (BehaviourUtils.IsNull(networkObject) || Exists(networkObject))
			{
				Simulation.SetPlayerObjectId(player, networkObject);
			}
			else
			{
				InternalLogStreams.LogDebug?.Error(this, $"Invalid {networkObject}");
			}
		}

		public NetworkObject GetPlayerObject(PlayerRef player)
		{
			TryGetPlayerObject(player, out var networkObject);
			return networkObject;
		}

		public bool TryGetPlayerObject(PlayerRef player, out NetworkObject networkObject)
		{
			if (player.IsRealPlayer && Simulation.TryGetMeta(Simulation.GetPlayerObjectId(player), out var meta) && BehaviourUtils.IsAlive(meta.Instance))
			{
				networkObject = meta.Instance;
				return true;
			}
			networkObject = null;
			return false;
		}

		public List<T> GetAllBehaviours<T>() where T : SimulationBehaviour
		{
			List<T> result = new List<T>();
			GetAllBehaviours(result);
			return result;
		}

		public List<NetworkObject> GetAllNetworkObjects()
		{
			List<NetworkObject> result = new List<NetworkObject>();
			if (_simulation == null)
			{
				InternalLogStreams.LogError?.Log(this, "Simulation is not initialized.");
				return result;
			}
			GetAllNetworkObjects(result);
			return result;
		}

		public void GetAllNetworkObjects(List<NetworkObject> result)
		{
			result.Clear();
			foreach (KeyValuePair<NetworkId, NetworkObjectMeta> @object in _simulation.Objects)
			{
				if ((bool)@object.Value.Instance)
				{
					result.Add(@object.Value.Instance);
				}
			}
		}

		public void GetAllBehaviours<T>(List<T> result) where T : SimulationBehaviour
		{
			SimulationBehaviour[] allBehaviours = GetAllBehaviours(typeof(T));
			for (int i = 0; i < allBehaviours.Length; i++)
			{
				SimulationBehaviour simulationBehaviour = allBehaviours[i];
				while (BehaviourUtils.IsNotNull(simulationBehaviour) && simulationBehaviour.Flags.HasNot(SimulationBehaviourRuntimeFlags.PendingRemoval))
				{
					Assert.Check(simulationBehaviour.Object != null, "head.Object != null");
					Assert.Check(simulationBehaviour.Object.Meta != null, "head.Object.Meta");
					result.Add((T)simulationBehaviour);
					simulationBehaviour = simulationBehaviour.Next;
				}
			}
		}

		public double GetPlayerRtt(PlayerRef playerRef)
		{
			return Simulation.GetPlayerRtt(playerRef);
		}

		public (double average, double last) GetRttToPhotonCloud()
		{
			return (_cloudServices?.RoundTripTime).GetValueOrDefault((0.0, 0.0));
		}

		[MustUseReturnValue]
		internal RpcBuilder CreateRpcBuilder(uint messageType, int payloadByteCount, NetworkBehaviour targetBehaviour, PlayerRef targetPlayer)
		{
			if (!NetworkTypesMeta.Instance.TryGetRpcMeta(messageType, out var meta))
			{
				throw new InvalidOperationException($"Rpc with key {messageType} not found, has the assembly been weaved?");
			}
			return new RpcBuilder(Simulation, meta, payloadByteCount, targetBehaviour, targetPlayer);
		}

		public bool IsPlayerValid(PlayerRef player)
		{
			return Simulation.PlayerValid(player);
		}

		public byte[] GetPlayerConnectionToken(PlayerRef player = default(PlayerRef))
		{
			if (player == LocalPlayer || player == PlayerRef.None)
			{
				return _connectionToken;
			}
			if (IsServer)
			{
				return Simulation.GetPlayerConnectionToken(player);
			}
			return null;
		}

		public ConnectionType GetPlayerConnectionType(PlayerRef player)
		{
			if (IsServer && player != LocalPlayer)
			{
				NetAddress playerAddress = Simulation.GetPlayerAddress(player);
				if (!playerAddress.Equals(default(NetAddress)))
				{
					return playerAddress.IsRelayAddr ? ConnectionType.Relayed : ConnectionType.Direct;
				}
			}
			return ConnectionType.None;
		}

		public SimulationBehaviour[] GetAllBehaviours(Type type)
		{
			return _behaviourUpdater.GetTypeHeads(type);
		}

		public void AddCallbacks(params INetworkRunnerCallbacks[] callbacks)
		{
			if (_callbacks == null)
			{
				_callbacks = new List<INetworkRunnerCallbacks>();
			}
			foreach (INetworkRunnerCallbacks item in callbacks)
			{
				if (!_callbacks.Contains(item))
				{
					_callbacks.Add(item);
				}
			}
		}

		public void RemoveCallbacks(params INetworkRunnerCallbacks[] callbacks)
		{
			if (_callbacks == null)
			{
				_callbacks = new List<INetworkRunnerCallbacks>();
			}
			foreach (INetworkRunnerCallbacks item in callbacks)
			{
				if (_callbacks.Contains(item))
				{
					_callbacks.Remove(item);
				}
			}
		}

		internal void OnApplicationQuit()
		{
			StunClient.Reset();
			Shutdown();
		}

		public void RenderInternal()
		{
			if (!Simulation.HasObject(NetworkId.RuntimeConfig) || IsRegularShutdown || _simulation == null)
			{
				return;
			}
			NetworkProjectConfig.InvokeRenderModes invokeRenderMode = _config.InvokeRenderMode;
			if (1 == 0)
			{
			}
			bool flag;
			switch (invokeRenderMode)
			{
			case NetworkProjectConfig.InvokeRenderModes.NotInBatchMode:
				if (!Application.isBatchMode)
				{
					goto default;
				}
				flag = false;
				break;
			case NetworkProjectConfig.InvokeRenderModes.NotInEditorAsServer:
				if (Mode != SimulationModes.Server || !Application.isEditor)
				{
					goto default;
				}
				flag = false;
				break;
			case NetworkProjectConfig.InvokeRenderModes.NotInBatchModeAndEditorAsServer:
				if (!Application.isBatchMode && (Mode != SimulationModes.Server || !Application.isEditor))
				{
					goto default;
				}
				flag = false;
				break;
			default:
				flag = true;
				break;
			}
			if (1 == 0)
			{
			}
			if (flag)
			{
				try
				{
					_simulationPhase = SimulationPhase.Render;
					_simulation.InterpolateSequenceIncrement();
					_behaviourUpdater.InvokeRender();
					CallbackInterfaceInvoker.IAfterRender(_behaviourUpdater);
				}
				finally
				{
					_simulationPhase = SimulationPhase.None;
				}
			}
			if (_deferredShutdownParams.ShutdownRequested)
			{
				Shutdown(_deferredShutdownParams.DestroyGO, _deferredShutdownParams.ShutdownReason);
			}
		}

		private void Awake()
		{
			if (_callbacks == null)
			{
				_callbacks = new List<INetworkRunnerCallbacks>();
			}
			RegisterNetworkCallbacks();
			AddInstance(this);
			TaskManager.Setup();
		}

		private void OnDisable()
		{
			DebugOnDisable();
		}

		private void OnDestroy()
		{
			DebugOnDestroy();
			Shutdown(destroyGameObject: false);
		}

		private void Update()
		{
			_cloudServices?.Update();
		}

		public void SetMasterClient(PlayerRef player)
		{
			if (IsSharedModeMasterClient && !(player == LocalPlayer))
			{
				PromoteNewMasterClient(player);
			}
		}

		public PlayerRef GetMasterClient()
		{
			if (GameMode == GameMode.Single)
			{
				return LocalPlayer;
			}
			if (Topology != Topologies.Shared)
			{
				return PlayerRef.Invalid;
			}
			int masterClientActorId = _cloudServices.MasterClientActorId;
			if (masterClientActorId == 0)
			{
				return PlayerRef.Invalid;
			}
			foreach (PlayerRef activePlayer in _simulation.ActivePlayers)
			{
				if (_simulation.GetPlayerActorId(activePlayer) == masterClientActorId)
				{
					return activePlayer;
				}
			}
			return PlayerRef.Invalid;
		}

		public void RequestMasterClientReassign()
		{
			if (!IsSharedModeMasterClient)
			{
				if (!_config.AllowMasterClientReassignRequest)
				{
					throw new InvalidOperationException("Not allowed master client request operation, enable in NetworkProjectConfig.");
				}
				PromoteNewMasterClient(LocalPlayer);
			}
		}

		private void PromoteNewMasterClient(PlayerRef player)
		{
			if (Topology != Topologies.Shared || Simulation == null)
			{
				return;
			}
			if (!player.IsRealPlayer)
			{
				InternalLogStreams.LogDebug?.Error($"{player} is not a valid player index.");
				return;
			}
			int? playerActorId = Simulation.GetPlayerActorId(player);
			if (!playerActorId.HasValue)
			{
				InternalLogStreams.LogDebug?.Error($"Was not possible to get the actor id for {player}.");
			}
			else
			{
				_cloudServices.SendChangeMasterClient(playerActorId.Value);
			}
		}

		public void UpdateInternal(double dt)
		{
			if (IsClient)
			{
			}
			Assert.Check(!_deferredShutdownParams.ShutdownRequested, "_deferredShutdownParams.ShutdownRequested == false");
			if (dt != 0.0)
			{
				if (IsRegularShutdown)
				{
					return;
				}
				if (_simulation != null)
				{
					try
					{
						if (_simulation.IsPaused)
						{
							Assert.Check(_simulation.IsSinglePlayer, "Simulation is paused, but is not running in SinglePlayer Mode");
							return;
						}
						_simulationPhase = SimulationPhase.Update;
						RegisterNetworkCallbacks();
						InvokeBeforeUpdate();
						ProcessSpawnQueue();
						_ticksExecuted = _simulation.Update(dt);
						InvokeAfterUpdate();
					}
					catch (Exception error)
					{
						InternalLogStreams.LogException?.Log(this, error);
						Shutdown(destroyGameObject: true, ShutdownReason.Error);
					}
					finally
					{
						_simulationPhase = SimulationPhase.None;
						_simulation?.UpdatePoolPerformanceCounters();
					}
				}
			}
			if (_deferredShutdownParams.ShutdownRequested)
			{
				Shutdown(_deferredShutdownParams.DestroyGO, _deferredShutdownParams.ShutdownReason);
			}
			if (OnGameStartedInvoked)
			{
				_simulation?._fusionStatsManager.SwapSnapshots();
				_behaviourUpdater.FinishBehaviourStatisticsPendingSnapshot();
			}
		}

		private void RegisterNetworkCallbacks()
		{
			if (!this || !base.gameObject || _callbacks == null || _callbacks.Count != 0)
			{
				return;
			}
			INetworkRunnerCallbacks[] componentsInChildren = base.gameObject.GetComponentsInChildren<INetworkRunnerCallbacks>();
			INetworkRunnerCallbacks[] array = componentsInChildren;
			foreach (INetworkRunnerCallbacks networkRunnerCallbacks in array)
			{
				MonoBehaviour monoBehaviour = networkRunnerCallbacks as MonoBehaviour;
				if ((bool)monoBehaviour && monoBehaviour.enabled)
				{
					AddCallbacks(networkRunnerCallbacks);
				}
			}
		}

		public void SendReliableDataToPlayer(PlayerRef player, ReliableKey key, ReadOnlySpan<byte> data)
		{
			if (Simulation.IsPlayer && Simulation.LocalPlayer == player)
			{
				Simulation.Callbacks.OnReliableData(player, key, local: true, data);
			}
			else if (IsServer)
			{
				if (Simulation.TryGetConnectionIndexForPlayer(player, out var connectionIndex))
				{
					Simulation.SendReliableData(connectionIndex, player, key, data);
				}
			}
			else
			{
				Simulation.SendReliableData(0, player, key, data);
			}
		}

		public void SendReliableDataToServer(ReliableKey key, ReadOnlySpan<byte> data)
		{
			if (IsClient)
			{
				Simulation.SendReliableData(0, PlayerRef.None, key, data);
			}
			else
			{
				Simulation.Callbacks.OnReliableData(PlayerRef.None, key, local: true, data);
			}
		}

		public void SetPlayerAlwaysInterested(PlayerRef player, NetworkObject networkObject, bool alwaysInterested)
		{
			if (Exists(networkObject) && networkObject.HasStateAuthority)
			{
				Simulation.SetPlayerAlwaysInterested(player, networkObject, alwaysInterested);
			}
		}

		public unsafe T? GetInputForPlayer<T>(PlayerRef player) where T : unmanaged, INetworkInput
		{
			SimulationInput inputForPlayer = _simulation.GetInputForPlayer(player);
			if (inputForPlayer != null && NetworkInput.FromRaw(inputForPlayer.Data, Simulation.Config.InputDataWordCount).TryGet<T>(out var input))
			{
				return input;
			}
			return null;
		}

		public unsafe NetworkInput? GetRawInputForPlayer(PlayerRef player)
		{
			SimulationInput inputForPlayer = _simulation.GetInputForPlayer(player);
			if (inputForPlayer != null)
			{
				return NetworkInput.FromRaw(inputForPlayer.Data, Simulation.Config.InputDataWordCount);
			}
			return null;
		}

		public bool RequestStateAuthority(NetworkId id)
		{
			Assert.Check(id.IsValid, "{0} is not a valid NetworkID.", id);
			return Simulation.RequestStateAuthority(id, wants: true);
		}

		public bool ReleaseStateAuthority(NetworkId id)
		{
			Assert.Check(id.IsValid, "{0} is not a valid NetworkID.", id);
			return Simulation.RequestStateAuthority(id, wants: false);
		}

		public bool TryGetInputForPlayer<T>(PlayerRef player, out T input) where T : unmanaged, INetworkInput
		{
			T? inputForPlayer = GetInputForPlayer<T>(player);
			if (inputForPlayer.HasValue)
			{
				input = inputForPlayer.Value;
				return true;
			}
			input = default(T);
			return false;
		}

		public NetworkObject FindObject(NetworkId networkId)
		{
			TryFindObject(networkId, out var networkObject);
			return networkObject;
		}

		[ContractAnnotation("=> true, networkObject: notnull; => false, networkObject: null")]
		public bool TryFindObject(NetworkId objectId, out NetworkObject networkObject)
		{
			if (Simulation != null && Simulation.TryGetMeta(objectId, out var meta) && BehaviourUtils.IsAlive(meta.Instance))
			{
				networkObject = meta.Instance;
				return true;
			}
			networkObject = null;
			return false;
		}

		public bool TryFindBehaviour(NetworkBehaviourId behaviourId, out NetworkBehaviour behaviour)
		{
			if (TryFindObject(behaviourId.Object, out var networkObject) && behaviourId.Behaviour >= 0 && behaviourId.Behaviour < networkObject.NetworkedBehaviours.Length)
			{
				behaviour = networkObject.NetworkedBehaviours[behaviourId.Behaviour];
				return true;
			}
			behaviour = null;
			return false;
		}

		public bool TryFindBehaviour<T>(NetworkBehaviourId id, out T behaviour) where T : NetworkBehaviour
		{
			if (TryFindBehaviour(id, out var behaviour2))
			{
				return BehaviourUtils.IsAlive(behaviour = behaviour2 as T);
			}
			behaviour = null;
			return false;
		}

		public T TryGetNetworkedBehaviourFromNetworkedObjectRef<T>(NetworkId networkId) where T : NetworkBehaviour
		{
			if (TryFindObject(networkId, out var networkObject))
			{
				if (networkObject.TryGetBehaviour<T>(out var behaviour))
				{
					return behaviour;
				}
				return networkObject.GetBehaviour<T>();
			}
			return null;
		}

		public NetworkId TryGetObjectRefFromNetworkedBehaviour(NetworkBehaviour behaviour)
		{
			if (BehaviourUtils.IsAlive(behaviour) && behaviour.Object.IsValid)
			{
				return behaviour.Object.Id;
			}
			return default(NetworkId);
		}

		public NetworkBehaviourId TryGetNetworkedBehaviourId(NetworkBehaviour behaviour)
		{
			if (BehaviourUtils.IsAlive(behaviour) && behaviour.Object.IsValid)
			{
				NetworkBehaviourId result = default(NetworkBehaviourId);
				result.Behaviour = behaviour.ObjectIndex;
				result.Object = behaviour.Object.Id;
				return result;
			}
			return default(NetworkBehaviourId);
		}

		public bool SetIsSimulated(NetworkObject obj, bool simulate)
		{
			if (Exists(obj))
			{
				if (Simulation.Topology == Topologies.Shared && !Simulation.IsLocalSimulationStateAuthority(ref obj.Header))
				{
					InternalLogStreams.LogWarn?.Log("Can't set simulation state for objects you don't have state authority over in shared mode");
					return false;
				}
				((Simulation.ICallbacks)this).ObjectIsSimulatedChanged(obj.Id, simulate);
				return true;
			}
			return false;
		}

		[Obsolete("Use InterestManagementConfig instead.")]
		public void SetAreaOfInterestGrid(int x, int y, int z)
		{
			Assert.Always(!IsRunning, "Cannot set SetAreaOfInterestGrid while runner is running. Call before calling StartGame.");
			if (Topology == Topologies.Shared)
			{
				throw new Exception("Can't change grid size in shared mode");
			}
			if (!_overrideInterestCellSize.HasValue)
			{
				throw new Exception("Call SetAreaOfInterestCellSize first.");
			}
			Assert.Check(x > 0, "x > 0");
			Assert.Check(y > 0, "y > 0");
			Assert.Check(z > 0, "z > 0");
			_overrideInterestCellGridSize = new Vector3Int(x / _overrideInterestCellSize.Value, y / _overrideInterestCellSize.Value, z / _overrideInterestCellSize.Value);
		}

		[Obsolete("Use InterestManagementConfig instead.")]
		public void SetAreaOfInterestCellSize(int size)
		{
			Assert.Always(!IsRunning, "Cannot set SetAreaOfInterestCellSize while runner is running. Call before calling StartGame.");
			if (Topology == Topologies.Shared)
			{
				throw new Exception("Can't change cell size in shared mode");
			}
			Assert.Check(size >= 4, "size >= Simulation.AreaOfInterestState.MIN_CELL_SIZE");
			_overrideInterestCellSize = size;
		}

		public List<NetworkId> GetObjectsInAreaOfInterestForPlayer(PlayerRef player)
		{
			return _simulation.GetObjectsInAreaOfInterestForPlayer(player);
		}

		public void GetAreaOfInterestGizmoData(List<(Vector3 center, Vector3 size, int playerCount, int objectCount)> result)
		{
			if (_simulation != null)
			{
				_simulation.GetAreaOfInterestGizmoData(result);
			}
		}

		public bool TryGetFusionStatistics(out FusionStatisticsManager statisticsManager)
		{
			if (_simulation != null)
			{
				statisticsManager = _simulation._fusionStatsManager;
				return true;
			}
			statisticsManager = null;
			return false;
		}

		public bool TryGetBehaviourStatistics(Type behaviourType, out BehaviourStatisticsSnapshot behaviourStatisticsSnapshot)
		{
			if (_behaviourUpdater != null && _behaviourUpdater.TryGetBehaviourStatisticsSnapshot(behaviourType, out behaviourStatisticsSnapshot))
			{
				return true;
			}
			behaviourStatisticsSnapshot = null;
			return false;
		}

		public bool Exists(NetworkObject obj)
		{
			NetworkObjectMeta meta;
			return _simulation != null && BehaviourUtils.IsNotNull(obj) && _simulation.TryGetMeta(obj.Id, out meta) && (object)obj == meta.Instance;
		}

		public bool Exists(NetworkId id)
		{
			return id.IsValid && _simulation != null && _simulation.HasObject(id);
		}

		public void Despawn(NetworkObject networkObject)
		{
			if (BehaviourUtils.IsNull(networkObject) || networkObject.Meta == null)
			{
				return;
			}
			bool flag = Simulation.IsLocalSimulationStateAuthority(ref networkObject.Header);
			bool flag2 = networkObject.StateAuthority == PlayerRef.None && Simulation.IsMasterClient;
			if ((flag || flag2) && Exists(networkObject))
			{
				if (!BehaviourUtils.IsSame(this, networkObject.Runner))
				{
					throw new InvalidOperationException("Object does not belong to this runner");
				}
				DestroyNetworkObjectInternal(networkObject, NetworkObjectDestroyFlags.DestroyState | NetworkObjectDestroyFlags.DestroyedByDespawn);
			}
		}

		public T GetSingleton<T>() where T : SimulationBehaviour
		{
			if (!TryGetBehaviour<T>(out var behaviour))
			{
				AddGlobal(behaviour = AddBehaviour<T>());
			}
			return behaviour;
		}

		public bool HasSingleton<T>() where T : SimulationBehaviour
		{
			T behaviour;
			return TryGetBehaviour<T>(out behaviour);
		}

		public void DestroySingleton<T>() where T : SimulationBehaviour
		{
			if (TryGetBehaviour<T>(out var behaviour))
			{
				behaviour.MakeUnowned();
				RemoveGlobal(behaviour);
				Behaviour.DestroyBehaviour(behaviour);
			}
		}

		public void AddGlobal(SimulationBehaviour instance)
		{
			Assert.Check(instance.Runner == null, "instance.Runner == null");
			Assert.Check(instance.Object == null, "instance.Object == null");
			Assert.Check(!(instance is NetworkBehaviour), "instance is NetworkBehaviour == false");
			AddSimulationBehaviour(instance);
		}

		public void RemoveGlobal(SimulationBehaviour instance)
		{
			Assert.Check(instance.Runner == this, "instance.Runner == this");
			Assert.Check(instance.Object == null, "instance.Object == null");
			Assert.Check(!(instance is NetworkBehaviour), "instance is NetworkBehaviour == false");
			RemoveSimulationBehavior(instance);
		}

		internal void AddSimulationBehaviour(SimulationBehaviour behaviour)
		{
			Assert.Always(BehaviourUtils.IsAlive(behaviour), "Behaviour is not alive");
			Assert.Always(!(behaviour is NetworkBehaviour), "NetworkBehaviour should not be added to SimulationBehaviour list");
			behaviour.Flags |= SimulationBehaviourRuntimeFlags.IsGlobal;
			behaviour.Flags &= ~SimulationBehaviourRuntimeFlags.IsUnityDisabled;
			behaviour.MakeOwned(this, null);
			if (_behaviourUpdater == null)
			{
				throw new NullReferenceException("SimulationBehaviourUpdater is null. Are you trying to AddSimulationBehaviour on a NetworkRunner which has not yet been started?");
			}
			_behaviourUpdater.AddBehaviour(behaviour, skipFirstCall: false);
			if (!(behaviour is ISpawned spawned))
			{
				return;
			}
			if (OnGameStartedInvoked)
			{
				try
				{
					spawned.Spawned();
					return;
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
					return;
				}
			}
			_spawnedSimBehaviourQueue.Enqueue(spawned);
		}

		internal void RemoveSimulationBehavior(SimulationBehaviour behaviour)
		{
			behaviour.MakeUnowned();
			if (_behaviourUpdater == null)
			{
				throw new NullReferenceException("SimulationBehaviourUpdater is null. Are you trying to RemoveSimulationBehavior on a NetworkRunner which has not yet been started?");
			}
			_behaviourUpdater.RemoveBehaviour(behaviour);
			if (behaviour is IDespawned despawned)
			{
				try
				{
					despawned.Despawned(this, hasState: false);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
				}
			}
		}

		internal void DestroyNetworkObjectInternal(NetworkObject obj, NetworkObjectDestroyFlags flags)
		{
			Assert.Check(obj != null, "obj != null");
			Assert.Check((object)obj.Runner == this || obj.Runner == null, "object.ReferenceEquals(obj.Runner, this) || obj.Runner == null");
			InternalLogStreams.LogTraceObject?.Log(obj, string.Format("{0} with {1}", "DestroyNetworkObjectInternal", flags));
			bool flag = !flags.Has(NetworkObjectDestroyFlags.DestroyedByShutdown) && Exists(obj);
			bool flag2 = flags.Has(NetworkObjectDestroyFlags.DestroyedByEngine);
			PlayerRef stateAuthority = obj.StateAuthority;
			if (flags.HasNot(NetworkObjectDestroyFlags.DestroyedByEngine) && IsClient && Topology == Topologies.Shared)
			{
				FusionUtility.PruneMixedAuthorityHierarchy(obj.transform, obj.transform.parent, stateAuthority);
			}
			int count = _destroyIdsBuffer.Count;
			if (flag)
			{
				_destroyIdsBuffer.Add(obj.Id);
				if (obj.RuntimeFlags.Has(NetworkObjectRuntimeFlags.OwnsNestedObjects))
				{
					NetworkObject[] nestedObjects = obj.NestedObjects;
					foreach (NetworkObject networkObject in nestedObjects)
					{
						_destroyIdsBuffer.Add(networkObject.Id);
					}
				}
			}
			int count2 = _destroyIdsBuffer.Count;
			NetworkId id = obj.Id;
			NetworkObjectTypeId networkTypeId = obj.NetworkTypeId;
			if (id.IsValid)
			{
				if (obj.RuntimeFlags.Has(NetworkObjectRuntimeFlags.Spawned))
				{
					InvokeDespawnedCallback(obj, flag);
				}
				else
				{
					InternalLogStreams.LogTraceObject?.Log(obj, "Not despawning when cleaning up, not spawned");
				}
				bool flag3 = !flag2 && obj.RuntimeFlags.Has(NetworkObjectRuntimeFlags.OwnsNestedObjects);
				if (flag3)
				{
					NetworkObject[] nestedObjects2 = obj.NestedObjects;
					foreach (NetworkObject networkObject2 in nestedObjects2)
					{
						if (networkObject2.RuntimeFlags.Has(NetworkObjectRuntimeFlags.Spawned))
						{
							InvokeDespawnedCallback(networkObject2, flag);
						}
					}
				}
				DeinitializeObjectUnassignRunner(obj);
				if (flag3)
				{
					NetworkObject[] nestedObjects3 = obj.NestedObjects;
					foreach (NetworkObject networkObject3 in nestedObjects3)
					{
						if (BehaviourUtils.IsAlive(networkObject3) && networkObject3.Id.IsValid)
						{
							DeinitializeObjectUnassignRunner(networkObject3);
						}
					}
				}
			}
			else
			{
				obj.ResetNetworkState();
			}
			bool flag4 = _attachableInstances.Remove(networkTypeId);
			NetworkObjectReleaseContext context = new NetworkObjectReleaseContext(obj, networkTypeId, flag2);
			InternalLogStreams.LogTraceObject?.Log(obj, $"Releasing {context} (preexisting: {flag4})");
			_objectProvider.ReleaseInstance(this, in context);
			if (flag)
			{
				Assert.Check(count2 <= _destroyIdsBuffer.Count, "destroyIdEnd <= _destroyIdsBuffer.Count");
				Assert.Check(count <= _destroyIdsBuffer.Count, "destroyIdStart <= _destroyIdsBuffer.Count");
				for (int l = count; l < count2; l++)
				{
					NetworkId networkId = _destroyIdsBuffer[l];
					Simulation simulation = _simulation;
					if (simulation == null || !simulation.TryGetMeta(networkId, out var meta))
					{
						continue;
					}
					if (flags.Has(NetworkObjectDestroyFlags.DestroyState))
					{
						if (flags.HasNot(NetworkObjectDestroyFlags.DestroyedByReplicator) && IsClient && stateAuthority != meta.StateAuthority && !_simulation.IsLocalSimulationStateAuthority(ref meta.Header))
						{
							InternalLogStreams.LogTraceObject?.Log(this, $"Not going to destroy state object {networkId}: does not have the authority");
							EnqueueStateObjectWantingNetworkObject(ref meta.Header);
						}
						else
						{
							_simulation.DestroyStateObject(networkId);
						}
					}
					else if (flag2)
					{
						Assert.Check(!IsServer, "!IsServer");
						InternalLogStreams.LogTraceObject?.Log(this, $"Requeuing {networkId}");
						EnqueueStateObjectWantingNetworkObject(ref meta.Header);
					}
				}
				_destroyIdsBuffer.RemoveRange(count, count2 - count);
			}
			obj.Meta?.UnlinkInstance(obj);
			Assert.Check(obj.Meta == null, "obj.Meta == null");
		}

		public void Attach(NetworkObject networkObject, PlayerRef? inputAuthority = null, bool allocate = true, bool? masterClientObjectOverride = null)
		{
			if (BehaviourUtils.IsNull(networkObject))
			{
				throw new ArgumentNullException("networkObject");
			}
			if (!networkObject.NetworkTypeId.IsValid)
			{
				throw new ArgumentException("NetworkObject has invalid NetworkTypeId", "networkObject");
			}
			if (!VerifyBakedData(networkObject))
			{
				throw new InvalidOperationException($"Object {LogUtils.GetDump(networkObject)} either has not been baked or the baked data is outdated.");
			}
			if (allocate)
			{
				InvokeObjectAcquired(networkObject);
			}
			InitializeNetworkObjectAssignRunner(networkObject);
			_attachableInstances.Add(networkObject.NetworkTypeId, networkObject);
			if (allocate)
			{
				Simulation simulation = Simulation;
				NetworkId nextId = Simulation.GetNextId();
				int wordCount = NetworkObject.GetWordCount(networkObject);
				NetworkObjectTypeId networkTypeId = networkObject.NetworkTypeId;
				int behaviourCount = networkObject.NetworkedBehaviours.Length;
				NetworkObjectHeaderFlags flags = FlagsFromInstance(networkObject);
				NetworkObjectMeta meta = simulation.AllocateObject(nextId, wordCount, networkTypeId, behaviourCount, default(NetworkId), default(NetworkObjectNestingKey), flags);
				InitializeNetworkObjectInstance(meta, networkObject, inputAuthority, AttachOptions.LocalSpawn, masterClientObjectOverride);
				if (IsAwakeAtInitialization(networkObject))
				{
					InitializeNetworkObjectState(networkObject);
					InvokeBeforeSpawnedCallbacks(networkObject, AttachOptions.LocalSpawn, null);
					InvokeSpawnedCallback(networkObject);
					InvokeAfterSpawnedCallback(networkObject);
				}
			}
			else
			{
				networkObject.gameObject.SetActive(value: false);
			}
		}

		public void AddPlayerAreaOfInterest(PlayerRef player, Vector3 center, float radius)
		{
			if (IsServer && LocalPlayer == player)
			{
				return;
			}
			SimulationConnection result2;
			if (IsClient)
			{
				if (Topology != Topologies.ClientServer && !(player != LocalPlayer))
				{
					int maxSharedRadius = AreaOfInterest.MaxSharedRadius;
					if (radius > (float)maxSharedRadius)
					{
						InternalLogStreams.LogDebug?.Warn($"Area of Interest Radius has been exceeded. Clamping to {maxSharedRadius}. To increase the maxium radius, increase CellGridSize in the NetworkProjectConfig.");
						radius = maxSharedRadius;
					}
					SimulationMessageInternal_SetAreaOfInterest buffer = default(SimulationMessageInternal_SetAreaOfInterest);
					buffer.Center = center;
					buffer.Radius = radius;
					Simulation.SendInternalSimulationMessage(SimulationMessageInternalTypes.SetAreaOfInterest, buffer);
					if (Simulation.TryGetSimulationConnectionForPlayer(player, out var result))
					{
						result.AreaOfInterestCells.Clear();
						_simulation.AreaOfInterestInstance.SphereToCells(center, radius, result.AreaOfInterestCells);
					}
				}
			}
			else if (Simulation.TryGetSimulationConnectionForPlayer(player, out result2))
			{
				_simulation.AreaOfInterestInstance.SphereToCells(center, radius, result2.AreaOfInterestCells);
			}
		}

		public void ClearPlayerAreaOfInterest(PlayerRef player)
		{
			if ((!IsServer || !(LocalPlayer == player)) && !IsClient && Simulation.TryGetSimulationConnectionForPlayer(player, out var result))
			{
				result.AreaOfInterestCells.Clear();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool? IsInterestedIn(NetworkObject obj, PlayerRef player)
		{
			if (_simulation != null && BehaviourUtils.IsNotNull(obj) && obj.Meta != null)
			{
				return _simulation.IsInterestedIn(obj.Meta, player);
			}
			return null;
		}

		public void SetBehaviourReplicateToAll(NetworkBehaviour behaviour, bool replicate)
		{
			if (IsClient)
			{
				return;
			}
			behaviour.DefaultReplicated = replicate;
			foreach (SimulationConnection value in _simulation._connections.Values)
			{
				SetBehaviourReplicateTo(behaviour, value, replicate, forceCreate: false);
			}
		}

		public void SetBehaviourReplicateTo(NetworkBehaviour behaviour, PlayerRef player, bool replicate)
		{
			if (!IsClient && _simulation.TryGetSimulationConnectionForPlayer(player, out var result))
			{
				SetBehaviourReplicateTo(behaviour, result, replicate, forceCreate: true);
			}
		}

		private void SetBehaviourReplicateTo(NetworkBehaviour behaviour, SimulationConnection sc, bool replicate, bool forceCreate)
		{
			if (!Exists(behaviour.Object))
			{
				return;
			}
			NetworkObjectConnectionData objectData = sc.GetObjectData(behaviour.Object.Id, forceCreate);
			if (objectData == null)
			{
				return;
			}
			ulong num = (ulong)(1L << behaviour.ObjectIndex);
			bool flag = (objectData.Filter & num) == num;
			if (flag != replicate)
			{
				if (replicate)
				{
					objectData.Filter |= num;
				}
				else
				{
					objectData.Filter &= ~num;
				}
				objectData.SetTickSent(sc, 0, null, "SetBehaviourReplicateTo");
				objectData.SetTickAck(sc, 0, "SetBehaviourReplicateTo");
			}
		}

		public void Attach(NetworkObject[] networkObjects, PlayerRef? inputAuthority = null, bool allocate = true, bool? masterClientObjectOverride = null)
		{
			if (networkObjects == null)
			{
				throw new ArgumentNullException("networkObjects");
			}
			for (int i = 0; i < networkObjects.Length; i++)
			{
				NetworkObject networkObject = networkObjects[i];
				if (networkObject == null)
				{
					throw new ArgumentException($"NetworkObject[{i}] is null", "networkObjects");
				}
				if (!networkObject.NetworkTypeId.IsValid)
				{
					throw new ArgumentException($"NetworkObject[{i}] has an invalid type id", "networkObjects");
				}
				if (!VerifyBakedData(networkObject))
				{
					throw new InvalidOperationException($"Scene object {LogUtils.GetDump(networkObject)} either has not been baked or the baked data is outdated.");
				}
				if (allocate)
				{
					InvokeObjectAcquired(networkObject);
				}
				if (!IsPreexistingAtInitialization(networkObject))
				{
					InitializeNetworkObjectAssignRunner(networkObject);
				}
			}
			foreach (NetworkObject networkObject2 in networkObjects)
			{
				if (_attachableInstances.ContainsKey(networkObject2.NetworkTypeId))
				{
					InternalLogStreams.LogError?.Log(this, $"Object with type id {networkObject2.NetworkTypeId} has already been attached or is waiting to be attached to");
				}
				else
				{
					_attachableInstances.Add(networkObject2.NetworkTypeId, networkObject2);
				}
			}
			if (allocate)
			{
				foreach (NetworkObject networkObject3 in networkObjects)
				{
					if (IsPreexistingAtInitialization(networkObject3))
					{
						Assert.Check(networkObject3.Meta != null, "obj.Meta != null");
						continue;
					}
					Simulation simulation = Simulation;
					NetworkId nextId = Simulation.GetNextId();
					int wordCount = NetworkObject.GetWordCount(networkObject3);
					NetworkObjectTypeId networkTypeId = networkObject3.NetworkTypeId;
					int behaviourCount = networkObject3.NetworkedBehaviours.Length;
					NetworkObjectHeaderFlags flags = FlagsFromInstance(networkObject3);
					NetworkObjectMeta meta = simulation.AllocateObject(nextId, wordCount, networkTypeId, behaviourCount, default(NetworkId), default(NetworkObjectNestingKey), flags);
					InitializeNetworkObjectInstance(meta, networkObject3, inputAuthority, AttachOptions.LocalSpawn, masterClientObjectOverride);
				}
				foreach (NetworkObject networkObject4 in networkObjects)
				{
					if (IsAwakeAtInitialization(networkObject4) && !IsPreexistingAtInitialization(networkObject4))
					{
						InitializeNetworkObjectState(networkObject4);
					}
				}
				foreach (NetworkObject networkObject5 in networkObjects)
				{
					if (IsAwakeAtInitialization(networkObject5))
					{
						InvokeBeforeSpawnedCallbacks(networkObject5, (!IsPreexistingAtInitialization(networkObject5)) ? AttachOptions.LocalSpawn : ((AttachOptions)0), null);
					}
				}
				foreach (NetworkObject networkObject6 in networkObjects)
				{
					if (IsAwakeAtInitialization(networkObject6))
					{
						InvokeSpawnedCallback(networkObject6);
					}
				}
				foreach (NetworkObject networkObject7 in networkObjects)
				{
					if (IsAwakeAtInitialization(networkObject7))
					{
						InvokeAfterSpawnedCallback(networkObject7);
					}
				}
				if (IsSharedModeMasterClient && _simulation.Config.AreaOfInterestEnabled)
				{
					SimulationConnection simulationConnectionForLocalPlayer = _simulation.GetSimulationConnectionForLocalPlayer();
					foreach (NetworkObject networkObject8 in networkObjects)
					{
						simulationConnectionForLocalPlayer.GetObjectData(networkObject8, create: true);
					}
				}
			}
			else
			{
				foreach (NetworkObject networkObject9 in networkObjects)
				{
					networkObject9.gameObject.SetActive(value: false);
				}
			}
		}

		private bool VerifyBakedData(NetworkObject obj)
		{
			bool flag = true;
			switch (obj.Flags.GetVersion())
			{
			case NetworkObjectFlags.V2:
				return true;
			case NetworkObjectFlags.V1:
				if (!obj.Flags.HasNot(NetworkObjectFlags.HasMainNetworkTRSP) || (obj.NetworkedBehaviours != null && obj.NetworkedBehaviours.Length != 0 && obj.NetworkedBehaviours[0] is NetworkTRSP))
				{
					int i;
					for (i = 0; i < obj.NetworkedBehaviours.Length; i++)
					{
						if (!(obj.NetworkedBehaviours[i] is NetworkTRSP) || (object)obj.NetworkedBehaviours[i].gameObject != obj.gameObject)
						{
							continue;
						}
						if (i != 0)
						{
							NetworkBehaviour[] networkedBehaviours = obj.NetworkedBehaviours;
							NetworkBehaviour[] networkedBehaviours2 = obj.NetworkedBehaviours;
							int num = i;
							NetworkBehaviour networkBehaviour = obj.NetworkedBehaviours[i];
							NetworkBehaviour networkBehaviour2 = obj.NetworkedBehaviours[0];
							networkedBehaviours[0] = networkBehaviour;
							networkedBehaviours2[num] = networkBehaviour2;
							if (BehaviourUtils.IsNotNull(obj.NetworkedBehaviours[i].Object))
							{
								Assert.Check(obj.NetworkedBehaviours[i].ObjectIndex == i, "obj.NetworkedBehaviours[i].ObjectIndex == i");
								obj.NetworkedBehaviours[0].ObjectIndex = 0;
								obj.NetworkedBehaviours[i].ObjectIndex = i;
							}
							InternalLogStreams.LogTraceObject?.Log(obj, $"Reordered behaviours: 0<->{i}");
						}
						if (obj.Flags.HasNot(NetworkObjectFlags.HasMainNetworkTRSP))
						{
							InternalLogStreams.LogTraceObject?.Log(obj, "Added HasMainNetworkTRSP flag");
							obj.Flags |= NetworkObjectFlags.HasMainNetworkTRSP;
						}
						break;
					}
					if (i >= obj.NetworkedBehaviours.Length && obj.Flags.Has(NetworkObjectFlags.HasMainNetworkTRSP))
					{
						throw new InvalidOperationException($"The object {obj} needs to be rebaked");
					}
					NetworkObject[] nestedObjects = obj.NestedObjects;
					if (nestedObjects != null && nestedObjects.Length != 0)
					{
						NetworkObject[] nestedObjects2 = obj.NestedObjects;
						foreach (NetworkObject obj2 in nestedObjects2)
						{
							VerifyBakedData(obj2);
						}
					}
				}
				obj.Flags = obj.Flags.SetCurrentVersion();
				return true;
			default:
				return false;
			}
		}

		internal void AttachActivatedByUser(NetworkObject networkObject)
		{
			AttachOptions attachOptions = NetworkObjectFlagsToAttachOptions(networkObject.RuntimeFlags);
			InternalLogStreams.LogTraceObject?.Log(networkObject, $"AttachActivatedByUser ({attachOptions})");
			Assert.Check(networkObject.IsValid, "Expected object to be valid {0}", LogUtils.GetDump(networkObject));
			Assert.Check(networkObject.RuntimeFlags.Has(NetworkObjectRuntimeFlags.NotAwakeWhenAttaching), "Expected not awake when attaching {0}", LogUtils.GetDump(networkObject));
			if ((attachOptions & AttachOptions.LocalSpawn) == AttachOptions.LocalSpawn)
			{
				InitializeNetworkObjectState(networkObject);
			}
			InvokeBeforeSpawnedCallbacks(networkObject, attachOptions, null);
			InvokeSpawnedCallback(networkObject);
			InvokeAfterSpawnedCallback(networkObject);
		}

		public int RegisterSceneObjects(SceneRef scene, NetworkObject[] objects, NetworkSceneLoadId loadId = default(NetworkSceneLoadId))
		{
			if (!scene.IsValid)
			{
				throw new ArgumentException("scene");
			}
			if (objects == null)
			{
				throw new ArgumentNullException("objects");
			}
			objects = objects.Where((NetworkObject o) => !o.IsValid).ToArray();
			int result = 0;
			NetworkObject[] array = objects;
			foreach (NetworkObject networkObject in array)
			{
				networkObject.NetworkTypeId = NetworkObjectTypeId.FromSceneRefAndObjectIndex(scene, result++, loadId);
			}
			if (IsSharedModeMasterClient)
			{
				Assert.Check(IsSceneAuthority, "IsSceneAuthority");
				foreach (NetworkId item in _remoteCreateQueue)
				{
					if (!_simulation.TryGetMeta(item, out var meta) || !meta.Type.IsSceneObject)
					{
						continue;
					}
					NetworkSceneObjectId asSceneObjectId = meta.Type.AsSceneObjectId;
					if (!(asSceneObjectId.Scene != scene) && !(asSceneObjectId.LoadId != loadId) && asSceneObjectId.ObjectId < objects.Length)
					{
						NetworkObject networkObject2 = objects[asSceneObjectId.ObjectId];
						if (IsPreexistingAtInitialization(networkObject2))
						{
							InternalLogStreams.LogTraceObject?.Warn(networkObject2, $"Object already marked as preexisting for {meta.Type}, ignoring {meta.Id}");
							continue;
						}
						InitializeNetworkObjectAssignRunner(networkObject2);
						networkObject2.RuntimeFlags |= NetworkObjectRuntimeFlags.PreexistingObject;
						InitializeNetworkObjectInstance(meta, networkObject2, null, (AttachOptions)0, null);
						InternalLogStreams.LogTraceObject?.Log(networkObject2, $"Preexisting object {meta.Type} found and initialized");
					}
				}
			}
			NetworkObject[] networkObjects = objects;
			bool isSceneAuthority = IsSceneAuthority;
			Attach(networkObjects, null, isSceneAuthority);
			return result;
		}

		internal void InvokeOnBeforeHitboxRegistration()
		{
			using (HostProfiler.Markers.InvokeOnBeforeHitboxRegistration())
			{
				CallbackInterfaceInvoker.IBeforeHitboxRegistration(_behaviourUpdater);
			}
		}

		private CreateInstanceResult TryAcquireInstance(NetworkObjectTypeId typeId, NetworkObjectMeta meta, out NetworkObject result, bool synchronous = true, bool dontDestroyOnLoad = false)
		{
			if (meta != null)
			{
				Assert.Check(meta.Type == typeId, "Header's type mismatch {0} vs {1}", meta.Type, typeId);
			}
			if (typeId.IsNone)
			{
				InternalLogStreams.LogError?.Log(string.Format("Invalid type id: {0}, header: {1}", typeId, (meta != null) ? ((object)meta.Header) : "null"));
				result = null;
				return CreateInstanceResult.Failed;
			}
			result = null;
			_attachableInstances.TryGetValue(typeId, out var value);
			NetworkObjectAcquireResult networkObjectAcquireResult;
			NetworkObject result2;
			try
			{
				NetworkObjectAcquireContext context = new NetworkObjectAcquireContext(typeId, meta, synchronous, dontDestroyOnLoad, value);
				using (HostProfiler.Markers.INetworkObjectProviderAcquireInstance())
				{
					networkObjectAcquireResult = _objectProvider.AcquireInstance(this, in context, out result2);
				}
			}
			catch (Exception ex)
			{
				InternalLogStreams.LogError?.Log(string.Format("{0}.{1} threw an exception for {2}: {3}", "INetworkObjectProvider", "AcquireInstance", typeId, ex));
				return CreateInstanceResult.Failed;
			}
			switch (networkObjectAcquireResult)
			{
			case NetworkObjectAcquireResult.Success:
				if (BehaviourUtils.IsAlive(result2))
				{
					InternalLogStreams.LogTraceObject?.Log(result2, $"Acquired instance of {typeId} for {meta?.Id ?? default(NetworkId)}");
					if (!VerifyBakedData(result2))
					{
						throw new InvalidOperationException($"Object {LogUtils.GetDump(result2)} either has not been baked or the baked data is outdated.");
					}
					result = result2;
					if (result2 != value)
					{
						InitializeNetworkObjectAssignRunner(result2, typeId);
					}
					else
					{
						Assert.Always(result2.Runner == this, "Instance is not owned by this runner {0} {1} vs {2}", LogUtils.GetDump(result2), this, result2.Runner);
					}
					InvokeObjectAcquired(result);
					Assert.Check(result2.Runner == this, "instance.Runner == this");
					return CreateInstanceResult.Success;
				}
				InternalLogStreams.LogError?.Log(string.Format("{0}.{1} returned {2}, but the instance is not alive", "INetworkObjectProvider", "AcquireInstance", NetworkObjectAcquireResult.Success));
				return CreateInstanceResult.Failed;
			case NetworkObjectAcquireResult.Retry:
				return CreateInstanceResult.InProgress;
			case NetworkObjectAcquireResult.Failed:
				return CreateInstanceResult.Failed;
			case NetworkObjectAcquireResult.Ignore:
				return CreateInstanceResult.Ignore;
			default:
				InternalLogStreams.LogError?.Log(string.Format("Unknown result from {0}.{1}: {2}", "INetworkObjectProvider", "AcquireInstance", networkObjectAcquireResult));
				return CreateInstanceResult.Failed;
			}
		}

		private unsafe void DeinitializeObjectUnassignRunner(NetworkObject obj)
		{
			for (int i = 0; i < obj.NetworkedBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = obj.NetworkedBehaviours[i];
				_behaviourUpdater.RemoveBehaviour(networkBehaviour);
				networkBehaviour.MakeUnowned();
				networkBehaviour.Ptr = default(int*);
			}
			obj.ResetNetworkState();
		}

		private unsafe void InitializeNetworkObjectAssignRunner(NetworkObject instance, NetworkObjectTypeId? typeId = null)
		{
			Assert.Always(!instance.Id.IsValid, "The instance has already been initialized {0}", LogUtils.GetDump(instance));
			Assert.Check(instance.Ptr == null, "instance.Ptr == default");
			Assert.Always(instance.Runner == null, "The {0} is already owned {1}", LogUtils.GetDump(instance), instance.Runner);
			if (typeId.HasValue)
			{
				instance.NetworkTypeId = typeId.Value;
			}
			else
			{
				Assert.Always(instance.NetworkTypeId != default(NetworkObjectTypeId), "The instance has no type id {0}", LogUtils.GetDump(instance));
			}
			instance.MakeOwned(this);
			for (int i = 0; i < instance.NetworkedBehaviours.Length; i++)
			{
				instance.NetworkedBehaviours[i].MakeOwned(this, instance, i);
			}
			if (instance.NetworkTypeId.IsPrefab)
			{
				for (int j = 0; j < instance.NestedObjects.Length; j++)
				{
					InitializeNetworkObjectAssignRunner(instance.NestedObjects[j], NetworkObjectTypeId.FromPrefabNestedObject(instance.NetworkTypeId.AsPrefabId, j));
				}
			}
			Assert.Check(instance.RuntimeFlags.HasNone(NetworkObjectRuntimeFlags.ClearMask), "Had some leftover runtime flags {0} {1}", LogUtils.GetDump(instance), instance.RuntimeFlags & NetworkObjectRuntimeFlags.ClearMask);
			instance.RuntimeFlags &= ~NetworkObjectRuntimeFlags.ClearMask;
			if (instance.RuntimeFlags.HasNot(NetworkObjectRuntimeFlags.HadAwake))
			{
				Assert.Check(!instance.gameObject.activeInHierarchy, "!instance.gameObject.activeInHierarchy");
				if (instance.RuntimeFlags.HasNot(NetworkObjectRuntimeFlags.NotAwakeWhenAttaching))
				{
					instance.RuntimeFlags |= NetworkObjectRuntimeFlags.NotAwakeWhenAttaching;
					AddInactiveObjectGuard(instance);
				}
			}
		}

		private NetworkObjectTypeDescriptor CreateDescriptorFromInstance(NetworkObject instance)
		{
			OffsetSetBuilder offsetSetBuilder = default(OffsetSetBuilder);
			if (instance.NetworkedBehaviours != null)
			{
				NetworkBehaviour[] networkedBehaviours = instance.NetworkedBehaviours;
				foreach (NetworkBehaviour networkBehaviour in networkedBehaviours)
				{
					int[] customSharedModeReadOnlyOffsets = NetworkBehaviourUtils.GetCustomSharedModeReadOnlyOffsets(networkBehaviour.GetType(), networkBehaviour.WordOffset);
					int[] array = customSharedModeReadOnlyOffsets;
					foreach (int value in array)
					{
						offsetSetBuilder.Add(value);
					}
				}
			}
			return new NetworkObjectTypeDescriptor
			{
				PluginAuthorityOffsets = offsetSetBuilder.MakeOffsetSet()
			};
		}

		private NetworkObjectHeaderFlags FlagsFromInstance(NetworkObject instance)
		{
			NetworkObjectHeaderFlags networkObjectHeaderFlags = (NetworkObjectHeaderFlags)0;
			if (instance.Flags.Has(NetworkObjectFlags.AllowStateAuthorityOverride) && instance.Flags.HasNot(NetworkObjectFlags.MasterClientObject))
			{
				networkObjectHeaderFlags |= NetworkObjectHeaderFlags.AllowStateAuthorityOverride;
			}
			if (instance.Flags.Has(NetworkObjectFlags.DestroyWhenStateAuthorityLeaves) && instance.Flags.HasNot(NetworkObjectFlags.MasterClientObject))
			{
				networkObjectHeaderFlags |= NetworkObjectHeaderFlags.DestroyWhenStateAuthorityLeaves;
			}
			if (instance.EnableInterpolation)
			{
				networkObjectHeaderFlags |= NetworkObjectHeaderFlags.EnableInterpolation;
			}
			NetworkObjectInterestModes networkObjectInterestModes = instance.ObjectInterest;
			if (networkObjectInterestModes == NetworkObjectInterestModes.AreaOfInterest)
			{
				if (instance.Flags.Has(NetworkObjectFlags.HasMainNetworkTRSP))
				{
					networkObjectHeaderFlags |= NetworkObjectHeaderFlags.AreaOfInterest;
				}
				else
				{
					networkObjectInterestModes = NetworkObjectInterestModes.Global;
					InternalLogStreams.LogDebug?.Warn(instance, "Networked object does not have a main NetworkTRSP behaviour but has ObjectInterest set to AreaOfInterest, forcing ObjectInterest to AllPlayers to ensure consistency");
				}
			}
			if (instance.Flags.Has(NetworkObjectFlags.HasMainNetworkTRSP))
			{
				Assert.Check(instance.NetworkedBehaviours.Any((NetworkBehaviour x) => x is NetworkTRSP), "instance.NetworkedBehaviours.Any(x => x is NetworkTRSP)");
				networkObjectHeaderFlags |= NetworkObjectHeaderFlags.HasMainNetworkTRSP;
			}
			if (networkObjectInterestModes == NetworkObjectInterestModes.Global)
			{
				networkObjectHeaderFlags |= NetworkObjectHeaderFlags.GlobalObjectInterest;
			}
			return networkObjectHeaderFlags;
		}

		private unsafe void InitializeNetworkObjectInstance(NetworkObjectMeta meta, NetworkObject instance, PlayerRef? inputAuthority, AttachOptions options, bool? masterClientObjectOverride)
		{
			using (HostProfiler.Markers.InitializeNetworkObjectInstance())
			{
				Assert.Always(!instance.Id.IsValid, "The instance has already been initialized {0} {1}", LogUtils.GetDump(instance), meta.Id);
				Assert.Check(instance.Ptr == null, "instance.Ptr == default");
				Assert.Check(instance.Runner == this, "Should have called InitializeNetworkObjectAssignRunner before");
				Assert.Check(BehaviourUtils.IsNull(meta.Instance), "IsNull(meta.Instance)");
				bool flag = (options & AttachOptions.LocalSpawn) == AttachOptions.LocalSpawn;
				meta.LinkInstance(instance);
				UnityPreInitialize(meta, options);
				Assert.Check(meta.BehaviourCount == instance.NetworkedBehaviours.Length, "Behaviour count mismatch {0} {1} for {2} ({3})", meta.BehaviourCount, instance.NetworkedBehaviours.Length, meta.Id, meta.Type);
				int num = NetworkObject.GetBehaviourDataOffset(instance);
				for (int i = 0; i < instance.NetworkedBehaviours.Length; i++)
				{
					instance.NetworkedBehaviours[i].Flags &= ~SimulationBehaviourRuntimeFlags.ClearMask;
					int wordCount = NetworkBehaviourUtils.GetWordCount(instance.NetworkedBehaviours[i]);
					instance.NetworkedBehaviours[i].Init(instance.Ptr, num, wordCount);
					num += wordCount;
				}
				if (HasCustomPlugin)
				{
					NetworkObjectTypeId key = instance.NetworkTypeId;
					if (key.Kind == NetworkTypeIdKind.SceneObject)
					{
						NetworkSceneObjectId asSceneObjectId = key.AsSceneObjectId;
						key = NetworkObjectTypeId.FromSceneObjectId(new NetworkSceneObjectId(asSceneObjectId.Scene, asSceneObjectId.ObjectId));
					}
					if (!_knownTypes.TryGetValue(key, out var value))
					{
						value = CreateDescriptorFromInstance(instance);
						_knownTypes.Add(key, value);
					}
					meta.TypeDescriptor = value;
				}
				if (flag)
				{
					instance.Defaults();
					if (_simulation.Topology == Topologies.Shared)
					{
						if (masterClientObjectOverride.GetValueOrDefault(instance.Flags.Has(NetworkObjectFlags.MasterClientObject)))
						{
							if (!Simulation.IsMasterClient)
							{
								InternalLogStreams.LogError?.Log(instance, "Non-master clients cannot spawn with MasterClient authority, spawning with local authority instead. This is caused by passing SharedModeStateAuthMasterClient or when \"Is Master Client Object\" is checked in the inspector and SharedModeStateAuthLocalPlayer flag is not passed.");
								_simulation.SetStateAuthority(meta, LocalPlayer);
							}
							else
							{
								_simulation.SetStateAuthority(meta, PlayerRef.InternalMasterClientIdentifier);
							}
						}
						else
						{
							_simulation.SetStateAuthority(meta, LocalPlayer);
						}
					}
					Simulation.Replicator.OnObjectSpawnedLocal(meta.Id);
				}
				if (inputAuthority.HasValue)
				{
					instance.AssignInputAuthority(inputAuthority.Value);
				}
				if (flag && meta.Flags.Has(NetworkObjectHeaderFlags.GlobalObjectInterest))
				{
					_simulation.AddToGlobalObjectInterest(meta);
				}
				_behaviourUpdater.AddObject(this, instance, _simulation.IsInTick, Simulation.IsLocalSimulationStateAuthority(ref meta.Header) || Simulation.IsLocalSimulationInputAuthority(ref meta.Header));
			}
		}

		private void UnityPreInitialize(NetworkObjectMeta meta, AttachOptions options)
		{
			NetworkObject instance = meta.Instance;
			if (_config.NetworkIdIsObjectName)
			{
				instance.gameObject.name = meta.Id.ToNamePrefixString() + instance.gameObject.name;
			}
			if (instance.RuntimeFlags.Has(NetworkObjectRuntimeFlags.HadAwake))
			{
				if (!instance.gameObject.activeSelf)
				{
					InternalLogStreams.LogTraceObject?.Log(instance, "Activating when initializing");
					instance.gameObject.SetActive(value: true);
				}
			}
			else
			{
				Assert.Check(instance.RuntimeFlags.Has(NetworkObjectRuntimeFlags.NotAwakeWhenAttaching), "Expected not to be awoken {0}", LogUtils.GetDump(instance));
				Assert.Check(!instance.gameObject.activeInHierarchy, "Expected not be active {0}", LogUtils.GetDump(instance));
				InternalLogStreams.LogTraceObject?.Log(instance, "Delaying activation");
				instance.RuntimeFlags |= AttachOptionsToNetworkObjectFlags(options);
			}
		}

		private unsafe void InitializeNetworkObjectState(NetworkObject instance)
		{
			Assert.Check(instance.Id.IsValid, "Already despawned {0}", LogUtils.GetDump(instance));
			Assert.Check(instance.Ptr != null, "Already despawned {0}", LogUtils.GetDump(instance));
			Assert.Check(_objectInitializer != null, "_objectInitializer != null");
			_objectInitializer.InitializeNetworkState(instance);
		}

		private void InvokeBeforeSpawnedCallbacks(NetworkObject instance, AttachOptions options, OnBeforeSpawned onBeforeSpawned)
		{
			if ((options & AttachOptions.LocalSpawn) == AttachOptions.LocalSpawn)
			{
				for (int i = 0; i < instance.NetworkedBehaviours.Length; i++)
				{
					if (instance.NetworkedBehaviours[i] is ILocalPrefabCreated localPrefabCreated)
					{
						try
						{
							localPrefabCreated.LocalPrefabCreated();
						}
						catch (Exception error)
						{
							InternalLogStreams.LogException?.Log(error);
						}
					}
				}
			}
			else
			{
				for (int j = 0; j < instance.NetworkedBehaviours.Length; j++)
				{
					if (instance.NetworkedBehaviours[j] is IRemotePrefabCreated remotePrefabCreated)
					{
						try
						{
							remotePrefabCreated.RemotePrefabCreated();
						}
						catch (Exception error2)
						{
							InternalLogStreams.LogException?.Log(error2);
						}
					}
				}
			}
			if (onBeforeSpawned != null)
			{
				try
				{
					onBeforeSpawned(this, instance);
				}
				catch (Exception error3)
				{
					InternalLogStreams.LogException?.Log(this, error3);
				}
			}
		}

		private void InvokeSpawnedCallback(NetworkObject instance)
		{
			InternalLogStreams.LogTraceObject?.Log(instance, "Spawning");
			Assert.Check(instance.RuntimeFlags.HasNot(NetworkObjectRuntimeFlags.Spawned), "Already spawned {0} {1} {2} {3}", BehaviourUtils.GetName(this), instance.Id, instance.GetHashCode(), BehaviourUtils.GetName(instance));
			instance.RuntimeFlags |= NetworkObjectRuntimeFlags.Spawned;
			for (int i = 0; i < instance.NetworkedBehaviours.Length; i++)
			{
				instance.NetworkedBehaviours[i].DebugNotifySpawned();
				instance.NetworkedBehaviours[i].PreSpawned();
				try
				{
					instance.NetworkedBehaviours[i].Spawned();
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
				}
			}
		}

		internal void InvokeDespawnedCallback(NetworkObject instance, bool hasState)
		{
			InternalLogStreams.LogTraceObject?.Log(instance, $"Despawning {hasState}");
			Assert.Check(instance.RuntimeFlags.Has(NetworkObjectRuntimeFlags.Spawned), "Not spawned {0}", instance.Name);
			instance.RuntimeFlags &= ~NetworkObjectRuntimeFlags.Spawned;
			for (int i = 0; i < instance.NetworkedBehaviours.Length; i++)
			{
				instance.NetworkedBehaviours[i].DebugNotifyDespawned();
				try
				{
					instance.NetworkedBehaviours[i].Despawned(this, hasState);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
				}
			}
		}

		private void InvokeAfterSpawnedCallback(NetworkObject instance)
		{
			for (int i = 0; i < instance.NetworkedBehaviours.Length; i++)
			{
				if (instance.NetworkedBehaviours[i] is IAfterSpawned afterSpawned)
				{
					try
					{
						afterSpawned.AfterSpawned();
					}
					catch (Exception error)
					{
						InternalLogStreams.LogException?.Log(error);
					}
				}
			}
		}

		private void InvokeObjectAcquired(NetworkObject instance)
		{
			this.ObjectAcquired?.Invoke(this, instance);
		}

		internal void InvokeFailedToResolveNetworkObject(NetworkId id)
		{
			this.FailedToResolveNetworkObject?.Invoke(this, id);
		}

		private void InvokeBeforeUpdate()
		{
			CallbackInterfaceInvoker.IBeforeUpdate(_behaviourUpdater);
		}

		private void InvokeAfterUpdate()
		{
			CallbackInterfaceInvoker.IAfterUpdate(_behaviourUpdater);
		}

		internal static NetworkProjectConfig SetupNetworkProjectConfig(NetworkRunnerInitializeArgs args)
		{
			return args.Config.Init(8, args.PlayerCount, Math.Max(NetworkTypesMeta.Instance.GetMaxInputWordCount(), args.InputWordCount.GetValueOrDefault() + 1));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("No longer used")]
		public RpcTargetStatus GetRpcTargetStatus(PlayerRef target)
		{
			return RpcTargetStatus.Unreachable;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool HasAnyActiveConnections()
		{
			return Simulation.HasAnyActiveConnections();
		}

		private static NetworkObjectRuntimeFlags AttachOptionsToNetworkObjectFlags(AttachOptions options)
		{
			NetworkObjectRuntimeFlags networkObjectRuntimeFlags = NetworkObjectRuntimeFlags.None;
			if ((options & AttachOptions.LocalSpawn) == AttachOptions.LocalSpawn)
			{
				networkObjectRuntimeFlags |= NetworkObjectRuntimeFlags.AttachOptionLocalSpawn;
			}
			return networkObjectRuntimeFlags;
		}

		private static AttachOptions NetworkObjectFlagsToAttachOptions(NetworkObjectRuntimeFlags flags)
		{
			AttachOptions attachOptions = (AttachOptions)0;
			if (flags.Has(NetworkObjectRuntimeFlags.AttachOptionLocalSpawn))
			{
				attachOptions |= AttachOptions.LocalSpawn;
			}
			return attachOptions;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsAwakeAtInitialization(NetworkObject obj)
		{
			return obj.RuntimeFlags.Has(NetworkObjectRuntimeFlags.HadAwake);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsPreexistingAtInitialization(NetworkObject obj)
		{
			return obj.RuntimeFlags.Has(NetworkObjectRuntimeFlags.PreexistingObject);
		}

		private void DebugOnDestroy()
		{
			InternalLogStreams.LogTrace?.Log(this, "OnDestroy");
		}

		private void DebugOnDisable()
		{
			InternalLogStreams.LogTrace?.Log(this, "OnDisable");
		}

		internal static bool TryGetPrettyRunnerName(StringBuilder output, NetworkRunner runner)
		{
			if ((object)runner == null || runner.Config?.PeerMode != NetworkProjectConfig.PeerModes.Multiple)
			{
				return false;
			}
			PlayerRef playerRef = runner.Simulation?.LocalPlayer ?? default(PlayerRef);
			if (playerRef.IsRealPlayer)
			{
				output.Append("[P").Append(playerRef.AsIndex).Append("] ");
			}
			else
			{
				output.Append("[P-] ");
			}
			output.Append(runner.DebugNameThreadSafe);
			return true;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetAllSimulationStatics()
		{
			NetworkBehaviourUtils.ResetStatics();
			ResetStatics();
		}

		internal void SetupEncryption(EncryptionToken token)
		{
			if (_config.EncryptionConfig.EnableEncryption)
			{
				if (token == null)
				{
					InternalLogStreams.LogDebug?.Warn("Setup Encryption: no token, ignoring...");
				}
				else if (Simulation?._netSocket == null)
				{
					InternalLogStreams.LogDebug?.Warn("Setup Encryption: no socket, ignoring...");
				}
				else if (!_cloudServices.IsEncryptionEnabled)
				{
					InternalLogStreams.LogDebug?.Error("Setup Encryption: Photon Cloud Encryption is disabled, make sure to use any Datagram Encryption mode, ignoring...");
				}
				else
				{
					Simulation?._netSocket?.SetupEncryption(token.Key, token.KeyEncrypted);
				}
			}
		}

		private void AddInactiveObjectGuard(NetworkObject obj)
		{
			NetworkObjectInactivityGuard networkObjectInactivityGuard;
			if (_inactivityGuardPool.Count > 0)
			{
				networkObjectInactivityGuard = _inactivityGuardPool.Pop();
				Assert.Check(networkObjectInactivityGuard, "guard");
				InternalLogStreams.LogTraceObject?.Log(obj, "NetworkObjectInactivityGuard: reusing a guard from the pool");
			}
			else
			{
				GameObject gameObject = new GameObject("NetworkObjectInactivityGuard");
				networkObjectInactivityGuard = gameObject.AddComponent<NetworkObjectInactivityGuard>();
				gameObject.hideFlags = (Config.HideNetworkObjectInactivityGuard ? HideFlags.HideAndDontSave : (HideFlags.DontSave | HideFlags.NotEditable));
				InternalLogStreams.LogTraceObject?.Log(obj, "NetworkObjectInactivityGuard: allocated a new guard");
			}
			Assert.Check(networkObjectInactivityGuard.Object == null, "guard.Object == null");
			networkObjectInactivityGuard.Object = obj;
			networkObjectInactivityGuard.transform.SetParent(obj.transform);
		}

		public static List<NetworkRunner>.Enumerator GetInstancesEnumerator()
		{
			return _instances.GetEnumerator();
		}

		private static bool AddInstance(NetworkRunner runner)
		{
			if (!_instances.Contains(runner))
			{
				_instances.Add(runner);
				return true;
			}
			return false;
		}

		private static bool RemoveInstance(NetworkRunner runner)
		{
			return _instances.Remove(runner);
		}

		private void SimulatePhysicsScenes(float fixedDeltaTime)
		{
			PhysicsScene physicsScene = GetPhysicsScene();
			if (physicsScene.IsValid() && Physics.autoSimulation && physicsScene != Physics.defaultPhysicsScene)
			{
				physicsScene.Simulate(fixedDeltaTime);
			}
			PhysicsScene2D physicsScene2D = GetPhysicsScene2D();
			if (physicsScene2D.IsValid() && Physics2D.simulationMode == SimulationMode2D.FixedUpdate && physicsScene2D != Physics2D.defaultPhysicsScene)
			{
				physicsScene2D.Simulate(fixedDeltaTime);
			}
		}

		private void FixedUpdate()
		{
			FixedUpdateCount++;
			if (_simulateMultiPeerPhysicsScenes && IsRunning && Config.PeerMode == NetworkProjectConfig.PeerModes.Multiple)
			{
				SimulatePhysicsScenes(Time.fixedDeltaTime);
			}
		}

		public void SetSimulateMultiPeerPhysics(bool value)
		{
			_simulateMultiPeerPhysicsScenes = value;
		}

		public bool TryGetPhysicsInfo(out NetworkPhysicsInfo info)
		{
			if (_simulation != null && _simulation.TryGetStruct(NetworkId.PhysicsInfo, out var meta))
			{
				info = meta.GetStructData<NetworkPhysicsInfo>();
				return true;
			}
			info = default(NetworkPhysicsInfo);
			return false;
		}

		public bool TrySetPhysicsInfo(NetworkPhysicsInfo info)
		{
			if (!IsSceneAuthority)
			{
				throw new InvalidOperationException("The runner does not have the scene authority");
			}
			if (_simulation != null && _simulation.TryGetStruct(NetworkId.PhysicsInfo, out var meta))
			{
				meta.GetStructData<NetworkPhysicsInfo>() = info;
				return true;
			}
			return false;
		}

		public bool TryGetSceneInfo(out NetworkSceneInfo sceneInfo)
		{
			if (_simulation != null && _simulation.TryGetStruct(NetworkId.SceneInfo, out var meta))
			{
				sceneInfo = meta.GetStructData<NetworkSceneInfo>();
				return true;
			}
			sceneInfo = default(NetworkSceneInfo);
			return false;
		}

		private SceneRef ValidateSceneName(string sceneName)
		{
			if (string.IsNullOrEmpty(sceneName))
			{
				throw new ArgumentException("Scene name is null or empty", "sceneName");
			}
			if (SceneManager == null)
			{
				throw new InvalidOperationException("SceneManager not initialized");
			}
			SceneRef sceneRef = SceneManager.GetSceneRef(sceneName);
			if (!sceneRef.IsValid)
			{
				throw new ArgumentOutOfRangeException("Failed to get a SceneRef for \"" + sceneName + "\"");
			}
			return sceneRef;
		}

		private SceneRef ValidateSceneRef(SceneRef sceneRef)
		{
			if (!sceneRef.IsValid)
			{
				throw new ArgumentException("Invalid scene reference", "sceneRef");
			}
			if (SceneManager == null)
			{
				throw new InvalidOperationException("SceneManager not initialized");
			}
			return sceneRef;
		}

		private NetworkSceneAsyncOp ValidateSceneOp(NetworkSceneAsyncOp op)
		{
			if (!op.IsValid)
			{
				throw new ArgumentException("Invalid scene operation", "op");
			}
			return op;
		}

		public SceneRef GetSceneRef(string sceneNameOrPath)
		{
			if (SceneManager == null)
			{
				return default(SceneRef);
			}
			return SceneManager.GetSceneRef(sceneNameOrPath);
		}

		public SceneRef GetSceneRef(GameObject gameObj)
		{
			if (SceneManager == null)
			{
				return default(SceneRef);
			}
			return SceneManager.GetSceneRef(gameObj);
		}

		public bool MoveGameObjectToScene(GameObject gameObj, SceneRef sceneRef)
		{
			return SceneManager?.MoveGameObjectToScene(gameObj, sceneRef) ?? false;
		}

		public bool MoveGameObjectToSameScene(GameObject gameObj, GameObject other)
		{
			SceneRef sceneRef = GetSceneRef(other);
			if (!sceneRef.IsValid)
			{
				return false;
			}
			return MoveGameObjectToScene(gameObj, sceneRef);
		}

		public NetworkSceneAsyncOp LoadScene(string sceneName, LoadSceneParameters parameters, bool setActiveOnLoad = false)
		{
			return LoadScene(ValidateSceneName(sceneName), parameters, setActiveOnLoad);
		}

		public NetworkSceneAsyncOp LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, LocalPhysicsMode localPhysicsMode = LocalPhysicsMode.None, bool setActiveOnLoad = false)
		{
			return LoadScene(sceneName, new LoadSceneParameters(loadSceneMode, localPhysicsMode), setActiveOnLoad);
		}

		public NetworkSceneAsyncOp LoadScene(SceneRef sceneRef, LoadSceneMode loadSceneMode = LoadSceneMode.Single, LocalPhysicsMode localPhysicsMode = LocalPhysicsMode.None, bool setActiveOnLoad = false)
		{
			return LoadScene(sceneRef, new LoadSceneParameters(loadSceneMode, localPhysicsMode), setActiveOnLoad);
		}

		public NetworkSceneAsyncOp UnloadScene(string sceneName)
		{
			return UnloadScene(ValidateSceneName(sceneName));
		}

		public NetworkSceneAsyncOp LoadScene(SceneRef sceneRef, LoadSceneParameters parameters, bool setActiveOnLoad = false)
		{
			Assert.Check(_simulation != null, "_simulation != null");
			sceneRef = ValidateSceneRef(sceneRef);
			if (!_simulation.TryGetStruct(NetworkId.SceneInfo, out var meta))
			{
				return NetworkSceneAsyncOp.FromError(sceneRef, new InvalidOperationException($"Failed to find {NetworkId.SceneInfo}"));
			}
			ref NetworkSceneInfo structData = ref meta.GetStructData<NetworkSceneInfo>();
			int num = structData.AddSceneRef(sceneRef, parameters.loadSceneMode, parameters.localPhysicsMode, setActiveOnLoad);
			if (num < 0)
			{
				return NetworkSceneAsyncOp.FromError(sceneRef, new ArgumentException($"Failed to add {sceneRef}", "sceneRef"));
			}
			Assert.Check(structData.Scenes[num] == sceneRef, "info.Scenes[index] == sceneRef");
			_sceneInfoSnapshot = structData;
			if (_sceneLoadInitialTCS == null)
			{
				InternalLogStreams.LogTraceSceneInfo?.Log(this, $"Load scene {sceneRef} with {parameters} {Simulation.DumpObject(NetworkId.SceneInfo)}");
				return ValidateSceneOp(SceneManager.LoadScene(sceneRef, structData.SceneParams[num]));
			}
			InternalLogStreams.LogTraceSceneInfo?.Log(this, $"Load scene {sceneRef} with {parameters} deferred {Simulation.DumpObject(NetworkId.SceneInfo)}");
			NetworkLoadSceneParameters sceneParameters = structData.SceneParams[num];
			return ValidateSceneOp(NetworkSceneAsyncOp.FromDeferred(sceneRef, _sceneLoadInitialTCS.Task, this, (NetworkRunner runner, SceneRef x) => runner.SceneManager.LoadScene(x, sceneParameters)));
		}

		public NetworkSceneAsyncOp UnloadScene(SceneRef sceneRef)
		{
			Assert.Check(_simulation != null, "_simulation != null");
			sceneRef = ValidateSceneRef(sceneRef);
			InternalLogStreams.LogTraceSceneInfo?.Log(this, $"Unload scene {sceneRef} called");
			if (!_simulation.TryGetStruct(NetworkId.SceneInfo, out var meta))
			{
				return NetworkSceneAsyncOp.FromError(sceneRef, new InvalidOperationException($"Failed to find {NetworkId.SceneInfo}"));
			}
			ref NetworkSceneInfo structData = ref meta.GetStructData<NetworkSceneInfo>();
			if (!structData.RemoveSceneRef(sceneRef))
			{
				return NetworkSceneAsyncOp.FromError(sceneRef, new ArgumentException($"Failed to remove {sceneRef}", "sceneRef"));
			}
			_sceneInfoSnapshot = structData;
			if (_sceneLoadInitialTCS == null)
			{
				return ValidateSceneOp(SceneManager.UnloadScene(sceneRef));
			}
			return ValidateSceneOp(NetworkSceneAsyncOp.FromDeferred(sceneRef, _sceneLoadInitialTCS.Task, this, (NetworkRunner runner, SceneRef x) => runner.SceneManager.UnloadScene(x)));
		}

		public void InvokeSceneLoadStart(SceneRef sceneRef)
		{
			try
			{
				for (int i = 0; i < _callbacks.Count; i++)
				{
					_callbacks[i].OnSceneLoadStart(this);
				}
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(this, error);
			}
			CallbackInterfaceInvoker.ISceneLoadStart(_behaviourUpdater, sceneRef);
		}

		public void InvokeSceneLoadDone(in SceneLoadDoneArgs info)
		{
			try
			{
				for (int i = 0; i < _callbacks.Count; i++)
				{
					_callbacks[i].OnSceneLoadDone(this);
				}
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(this, error);
			}
			CallbackInterfaceInvoker.ISceneLoadDone(_behaviourUpdater, in info);
		}

		public static NetworkRunner GetRunnerForGameObject(GameObject gameObject)
		{
			return GetRunnerForScene(gameObject.scene);
		}

		public static NetworkRunner GetRunnerForScene(Scene scene)
		{
			foreach (NetworkRunner instance in Instances)
			{
				if (!BehaviourUtils.IsNull(instance))
				{
					INetworkSceneManager sceneManager = instance.SceneManager;
					if (sceneManager != null && sceneManager.IsRunnerScene(scene))
					{
						return instance;
					}
				}
			}
			return null;
		}

		public PhysicsScene GetPhysicsScene()
		{
			if (IsRunning)
			{
				if (SceneManager.TryGetPhysicsScene3D(out var scene3D))
				{
					return scene3D;
				}
				return default(PhysicsScene);
			}
			return Physics.defaultPhysicsScene;
		}

		public PhysicsScene2D GetPhysicsScene2D()
		{
			if (IsRunning)
			{
				if (SceneManager.TryGetPhysicsScene2D(out var scene2D))
				{
					return scene2D;
				}
				return default(PhysicsScene2D);
			}
			return Physics2D.defaultPhysicsScene;
		}

		public GameObject InstantiateInRunnerScene(GameObject original, Vector3 position, Quaternion rotation)
		{
			Scene previousActiveScene;
			bool flag = EnsureRunnerSceneIsActive(out previousActiveScene);
			GameObject gameObject = UnityEngine.Object.Instantiate(original, position, rotation);
			MoveToRunnerScene(gameObject);
			if (flag)
			{
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(previousActiveScene);
			}
			return gameObject;
		}

		public GameObject InstantiateInRunnerScene(GameObject original)
		{
			Scene previousActiveScene;
			bool flag = EnsureRunnerSceneIsActive(out previousActiveScene);
			GameObject gameObject = UnityEngine.Object.Instantiate(original);
			MoveToRunnerScene(gameObject);
			if (flag)
			{
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(previousActiveScene);
			}
			return gameObject;
		}

		public T InstantiateInRunnerScene<T>(T original) where T : Component
		{
			Scene previousActiveScene;
			bool flag = EnsureRunnerSceneIsActive(out previousActiveScene);
			T val = UnityEngine.Object.Instantiate(original);
			MoveToRunnerScene(val.gameObject);
			if (flag)
			{
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(previousActiveScene);
			}
			return val;
		}

		public T InstantiateInRunnerScene<T>(T original, Vector3 position, Quaternion rotation) where T : Component
		{
			Scene previousActiveScene;
			bool flag = EnsureRunnerSceneIsActive(out previousActiveScene);
			T val = UnityEngine.Object.Instantiate(original, position, rotation);
			MoveToRunnerScene(val.gameObject);
			if (flag)
			{
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(previousActiveScene);
			}
			return val;
		}

		public bool EnsureRunnerSceneIsActive(out Scene previousActiveScene)
		{
			Scene scene = SceneManager?.MainRunnerScene ?? default(Scene);
			if (!scene.IsValid() || scene == UnityEngine.SceneManagement.SceneManager.GetActiveScene())
			{
				previousActiveScene = default(Scene);
				return false;
			}
			previousActiveScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			if (!UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene))
			{
				previousActiveScene = default(Scene);
				return false;
			}
			return true;
		}

		public void MoveToRunnerScene<T>(T component) where T : Component
		{
			MoveToRunnerScene(component.gameObject);
		}

		public void MoveToRunnerScene(GameObject instance, SceneRef? targetSceneRef = null)
		{
			SceneManager?.MoveGameObjectToScene(instance, targetSceneRef.GetValueOrDefault());
		}

		public void MakeDontDestroyOnLoad(GameObject obj)
		{
			SceneManager?.MakeDontDestroyOnLoad(obj);
		}

		private void SceneInfoUpdate()
		{
			if (_sceneInfoChangeSource != NetworkSceneInfoChangeSource.None)
			{
				if (!_simulation.TryGetStruct(NetworkId.SceneInfo, out var meta))
				{
					Assert.AlwaysFail("Expected to be able to get scene info");
				}
				ref NetworkSceneInfo structData = ref meta.GetStructData<NetworkSceneInfo>();
				NetworkSceneInfoChangeSource sceneInfoChangeSource = _sceneInfoChangeSource;
				_sceneInfoChangeSource = NetworkSceneInfoChangeSource.None;
				InternalLogStreams.LogTraceSceneInfo?.Log(this, $"Handling pending scene info chang. Data: {structData}");
				if (sceneInfoChangeSource == NetworkSceneInfoChangeSource.Initial)
				{
					Assert.Check(_sceneLoadInitialTCS != null, "_sceneLoadInitialTCS != null");
					NetworkSceneInfo prevInfo = default(NetworkSceneInfo);
					_sceneInfoSnapshot = structData;
					SceneInfoSyncSceneManager(sceneInfoChangeSource, ref _sceneInfoInitial, ref prevInfo);
				}
				else
				{
					NetworkSceneInfo prevInfo2 = _sceneInfoSnapshot;
					_sceneInfoSnapshot = structData;
					SceneInfoSyncSceneManager(sceneInfoChangeSource, ref structData, ref prevInfo2);
				}
				if (_sceneLoadInitialTCS != null)
				{
					InternalLogStreams.LogTraceSceneInfo?.Log(this, "Initial scene load, completing tcs");
					_sceneLoadInitialTCS.SetResult(0);
					_sceneLoadInitialTCS = null;
				}
			}
		}

		private void SceneInfoSyncSceneManager(NetworkSceneInfoChangeSource changeSource, ref NetworkSceneInfo sceneInfo, ref NetworkSceneInfo prevInfo)
		{
			if (_sceneManager.OnSceneInfoChanged(sceneInfo, changeSource))
			{
				InternalLogStreams.LogTraceSceneManager?.Log(this, "Scene manager handled scene change event");
				return;
			}
			if (prevInfo.Equals(sceneInfo))
			{
				InternalLogStreams.LogTraceSceneManager?.Log(this, "Ignoring scene info change as it is the same as the last one");
				return;
			}
			for (int i = 0; i < prevInfo.SceneCount; i++)
			{
				Assert.Check(prevInfo.Scenes[i].IsValid, "Invalid previous scene at {0}", i);
			}
			for (int j = 0; j < sceneInfo.SceneCount; j++)
			{
				Assert.Check(sceneInfo.Scenes[j].IsValid, "Invalid scene at {0}", j);
			}
			bool flag = false;
			if (sceneInfo.SceneCount > 0 && sceneInfo.SceneParams[0].IsSingleLoad && (prevInfo.SceneCount == 0 || prevInfo.Scenes[0] != sceneInfo.Scenes[0] || prevInfo.SceneParams[0] != sceneInfo.SceneParams[0]))
			{
				flag = true;
			}
			if (!flag)
			{
				for (int k = 0; k < prevInfo.SceneCount; k++)
				{
					if (sceneInfo.IndexOf(prevInfo.Scenes[k], prevInfo.SceneParams[k]) < 0)
					{
						InternalLogStreams.LogTraceSceneInfo?.Log($"Unloading scene {prevInfo.Scenes[k]}");
						ValidateSceneOp(SceneManager.UnloadScene(prevInfo.Scenes[k])).AddOnCompleted(delegate(NetworkSceneAsyncOp op)
						{
							OnRemoteSceneLoadCompleted(op);
						});
					}
				}
			}
			for (int num = 0; num < sceneInfo.SceneCount; num++)
			{
				if (prevInfo.IndexOf(sceneInfo.Scenes[num], sceneInfo.SceneParams[num]) < 0)
				{
					InternalLogStreams.LogTraceSceneInfo?.Log($"Loading scene {sceneInfo.Scenes[num]} with {sceneInfo.SceneParams[num]}");
					ValidateSceneOp(SceneManager.LoadScene(sceneInfo.Scenes[num], sceneInfo.SceneParams[num])).AddOnCompleted(delegate(NetworkSceneAsyncOp op)
					{
						OnRemoteSceneUnloadCompleted(op);
					});
				}
			}
		}

		private void OnRemoteSceneLoadCompleted(NetworkSceneAsyncOp asyncOp)
		{
			if (asyncOp.Error != null)
			{
				InternalLogStreams.LogError?.Log(this, $"Failed to load scene {asyncOp.SceneRef} with error {asyncOp.Error}");
			}
		}

		private void OnRemoteSceneUnloadCompleted(NetworkSceneAsyncOp asyncOp)
		{
			if (asyncOp.Error != null)
			{
				InternalLogStreams.LogError?.Log(this, $"Failed to unload scene {asyncOp.SceneRef} with error {asyncOp.Error}");
			}
		}

		void Simulation.ICallbacks.ObjectIsSimulatedChanged(NetworkId id, bool simulated)
		{
			if (!TryFindObject(id, out var networkObject) || networkObject.IsInSimulation == simulated)
			{
				return;
			}
			if (simulated)
			{
				networkObject.RuntimeFlags |= NetworkObjectRuntimeFlags.InSimulation;
				if (networkObject is ISimulationEnter simulationEnter)
				{
					try
					{
						simulationEnter.SimulationEnter();
					}
					catch (Exception error)
					{
						InternalLogStreams.LogException?.Log(error);
					}
				}
				for (int i = 0; i < networkObject.NetworkedBehaviours.Length; i++)
				{
					try
					{
						networkObject.NetworkedBehaviours[i].Flags |= SimulationBehaviourRuntimeFlags.InSimulation;
						if (networkObject.NetworkedBehaviours[i] is ISimulationEnter simulationEnter2)
						{
							simulationEnter2.SimulationEnter();
						}
					}
					catch (Exception error2)
					{
						InternalLogStreams.LogException?.Log(this, error2);
					}
				}
				return;
			}
			networkObject.RuntimeFlags &= ~NetworkObjectRuntimeFlags.InSimulation;
			if (networkObject is ISimulationExit simulationExit)
			{
				try
				{
					simulationExit.SimulationExit();
				}
				catch (Exception error3)
				{
					InternalLogStreams.LogException?.Log(error3);
				}
			}
			for (int j = 0; j < networkObject.NetworkedBehaviours.Length; j++)
			{
				try
				{
					networkObject.NetworkedBehaviours[j].Flags &= ~SimulationBehaviourRuntimeFlags.InSimulation;
					if (networkObject.NetworkedBehaviours[j] is ISimulationExit simulationExit2)
					{
						simulationExit2.SimulationExit();
					}
				}
				catch (Exception error4)
				{
					InternalLogStreams.LogException?.Log(this, error4);
				}
			}
		}

		void Simulation.ICallbacks.ObjectInputAuthorityChanged(NetworkId id, bool gained)
		{
			if (!TryFindObject(id, out var networkObject))
			{
				return;
			}
			if (gained)
			{
				if (networkObject is IInputAuthorityGained inputAuthorityGained)
				{
					try
					{
						inputAuthorityGained.InputAuthorityGained();
					}
					catch (Exception error)
					{
						InternalLogStreams.LogException?.Log(this, error);
					}
				}
				for (int i = 0; i < networkObject.NetworkedBehaviours.Length; i++)
				{
					try
					{
						if (networkObject.NetworkedBehaviours[i] is IInputAuthorityGained inputAuthorityGained2)
						{
							inputAuthorityGained2.InputAuthorityGained();
						}
					}
					catch (Exception error2)
					{
						InternalLogStreams.LogException?.Log(this, error2);
					}
				}
				return;
			}
			if (networkObject is IInputAuthorityLost inputAuthorityLost)
			{
				try
				{
					inputAuthorityLost.InputAuthorityLost();
				}
				catch (Exception error3)
				{
					InternalLogStreams.LogException?.Log(this, error3);
				}
			}
			for (int j = 0; j < networkObject.NetworkedBehaviours.Length; j++)
			{
				try
				{
					if (networkObject.NetworkedBehaviours[j] is IInputAuthorityLost inputAuthorityLost2)
					{
						inputAuthorityLost2.InputAuthorityLost();
					}
				}
				catch (Exception error4)
				{
					InternalLogStreams.LogException?.Log(this, error4);
				}
			}
		}

		void Simulation.ICallbacks.ObjectStateAuthorityChanged(NetworkId id, bool gained)
		{
			if (!TryFindObject(id, out var networkObject))
			{
				return;
			}
			if (networkObject is IStateAuthorityChanged stateAuthorityChanged)
			{
				try
				{
					stateAuthorityChanged.StateAuthorityChanged();
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
			for (int i = 0; i < networkObject.NetworkedBehaviours.Length; i++)
			{
				try
				{
					if (networkObject.NetworkedBehaviours[i] is IStateAuthorityChanged stateAuthorityChanged2)
					{
						stateAuthorityChanged2.StateAuthorityChanged();
					}
				}
				catch (Exception error2)
				{
					InternalLogStreams.LogException?.Log(this, error2);
				}
			}
		}

		RemoteObjectCreateResult Simulation.ICallbacks.OnRemoteCreate(PlayerRef player, [In][RequiresLocation] ref NetworkObjectHeader header)
		{
			EnqueueStateObjectWantingNetworkObject(ref header);
			return RemoteObjectCreateResult.Allowed;
		}

		private void EnqueueStateObjectWantingNetworkObject([In][RequiresLocation] ref NetworkObjectHeader header)
		{
			if (_objectProvider != null && !header.Flags.Has(NetworkObjectHeaderFlags.Struct) && !header.Id.IsReserved)
			{
				if (header.NestingRoot.IsValid)
				{
					_remoteCreateNestedQueue.Enqueue(header.Id);
				}
				else
				{
					_remoteCreateQueue.Enqueue(header.Id);
				}
			}
		}

		bool Simulation.ICallbacks.OnRemoteDestroy(PlayerRef player, NetworkId id)
		{
			_remoteDestroyQueue.Enqueue(id);
			return true;
		}

		void Simulation.ICallbacks.UpdateRemotePrefabs()
		{
			if (IsSceneManagerBusy)
			{
				InternalLogStreams.LogTraceObject?.Log(this, "Not updating remote prefabs, scene manager is busy");
				return;
			}
			if (IsClient && !IsSharedModeMasterClient && !_simulation.HasObject(NetworkId.SceneInfo))
			{
				InternalLogStreams.LogTraceObject?.Log(this, "Not updating remote prefabs because scene info is not valid");
				return;
			}
			Assert.Check(_remotePrefabsWaitingForSpawnedCallback.Count == 0, "_remotePrefabsWaitingForSpawnedCallback.Count == 0");
			CallbackInterfaceInvoker.IBeforeUpdateRemotePrefabs(_behaviourUpdater);
			try
			{
				if (_remoteDestroyQueue.Count > 0 || _remoteCreateQueue.Count > 0 || _remoteCreateNestedQueue.Count > 0)
				{
					InternalLogStreams.LogTraceObject?.Log(this, "UpdateRemotePrefabs will do some work. Destroys: " + string.Join(",", _remoteDestroyQueue) + "; Creates: " + string.Join(",", _remoteCreateQueue) + "; Create nested: " + string.Join(",", _remoteCreateNestedQueue) + ";");
				}
				int count = _remoteCreateQueue.Count;
				while (count-- > 0)
				{
					NetworkId networkId = _remoteCreateQueue.Dequeue();
					if (!_simulation.TryGetMeta(networkId, out var meta) || BehaviourUtils.IsNotNull(meta.Instance))
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"Ignoring remote spawn of {networkId}: no such object");
						continue;
					}
					NetworkObject result;
					CreateInstanceResult createInstanceResult = TryAcquireInstance(meta.Type, meta, out result, synchronous: false, meta.Flags.Has(NetworkObjectHeaderFlags.DontDestroyOnLoad));
					switch (createInstanceResult)
					{
					case CreateInstanceResult.InProgress:
						InternalLogStreams.LogTraceObject?.Log(this, $"Requeuing remote spawn of {networkId}: create in progress");
						_remoteCreateQueue.Enqueue(networkId);
						continue;
					case CreateInstanceResult.Ignore:
						InternalLogStreams.LogTraceObject?.Log(this, $"Ignoring remote spawn of {networkId}: provider returned ignore");
						meta.LocalFlags |= NetworkObjectMetaFlags.InstanceWillNotBeCreated;
						continue;
					case CreateInstanceResult.Failed:
						InternalLogStreams.LogError?.Log(this, $"Ignoring remote spawn of {networkId}: failed to create instance, not going to retry");
						meta.LocalFlags |= NetworkObjectMetaFlags.InstanceWillNotBeCreated;
						continue;
					}
					Assert.Check(createInstanceResult == CreateInstanceResult.Success, "createResult == CreateInstanceResult.Success");
					Assert.Check(BehaviourUtils.IsNotNull(result), "IsNotNull(instance)");
					if (result.Id.IsValid && result.Id != meta.Id)
					{
						InternalLogStreams.LogWarn?.Log(result, $"Object (type: {meta.Type} is already attached to a different id: {meta.Id}");
						_remoteCreateQueue.Enqueue(networkId);
						continue;
					}
					NetworkObject[] nestedObjects = result.NestedObjects;
					foreach (NetworkObject networkObject in nestedObjects)
					{
						if (!networkObject.IsValid && networkObject.gameObject.activeSelf)
						{
							InternalLogStreams.LogTraceObject?.Log(networkObject, $"Deactivating unattached nested object of {meta.Id} ({result.name})");
							networkObject.gameObject.SetActive(value: false);
						}
					}
					InternalLogStreams.LogTraceObject?.Log(this, $"Handled remote spawn of {networkId}: instance acquired");
					InstanceAcquired(meta, result);
				}
				int count2 = _remoteCreateNestedQueue.Count;
				while (count2-- > 0)
				{
					NetworkId networkId2 = _remoteCreateNestedQueue.Dequeue();
					if (!_simulation.TryGetMeta(networkId2, out var meta2) || BehaviourUtils.IsNotNull(meta2.Instance))
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"Ignoring remote nested spawn of {networkId2}: no such object");
						continue;
					}
					if (!_simulation.TryGetMeta(meta2.NestingRoot, out var meta3))
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"Requeuing remote nested spawn of {networkId2}: root {meta2.NestingRoot} does not exist yet");
						_remoteCreateNestedQueue.Enqueue(networkId2);
						continue;
					}
					if (meta3.LocalFlags.Has(NetworkObjectMetaFlags.InstanceWillNotBeCreated))
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"Ignoring remote nested spawn of {networkId2}: root {meta2.NestingRoot} will not have an instance");
						continue;
					}
					NetworkObject instance = meta3.Instance;
					if (BehaviourUtils.IsNull(instance))
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"Requeuing remote nested spawn of {networkId2}: root {meta2.NestingRoot} exists, but does not have an instance yet");
						_remoteCreateNestedQueue.Enqueue(networkId2);
						continue;
					}
					int num = meta2.NestingKey.Value - 1;
					if (num < 0 || num >= instance.NestedObjects.Length)
					{
						InternalLogStreams.LogError?.Log(this, $"Nesting key out of bounds: {meta2.NestingKey} {instance.NestedObjects.Length}, won't try to create nested object");
						continue;
					}
					NetworkObject networkObject2 = instance.NestedObjects[num];
					if (BehaviourUtils.IsNull(networkObject2))
					{
						InternalLogStreams.LogError?.Log(this, $"Nesting key {meta2.NestingKey} is valid for {meta2.Id}, but the instance is null - won't try to create nested object");
					}
					else if (networkObject2.Id.IsValid && networkObject2.Id != meta2.Id)
					{
						InternalLogStreams.LogWarn?.Log(networkObject2, $"Object (type: {meta2.Type} is already attached to a different id: {meta2.Id}");
						_remoteCreateNestedQueue.Enqueue(networkId2);
					}
					else
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"Handled remote nested spawn of {networkId2}: instance acquired");
						networkObject2.RuntimeFlags |= NetworkObjectRuntimeFlags.IsNested;
						InstanceAcquired(meta2, networkObject2);
					}
				}
				while (_remoteDestroyQueue.Count > 0)
				{
					NetworkId networkId3 = _remoteDestroyQueue.Dequeue();
					if (!_simulation.TryGetMeta(networkId3, out var meta4))
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"DeleteRemotePrefab for {networkId3}, but it doesn't exist");
					}
					else if (BehaviourUtils.IsAlive(meta4.Instance))
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"Destroy remote prefab: {meta4.Id} tick: {Simulation.Tick} {Simulation.Stage}");
						Assert.Check(meta4.Id == meta4.Instance.Id, "Object {0} seems to have been attached to a different id ({1}) already {2}", BehaviourUtils.GetName(meta4.Instance), meta4.Instance.Id, meta4);
						DestroyNetworkObjectInternal(meta4.Instance, NetworkObjectDestroyFlags.DestroyState | NetworkObjectDestroyFlags.DestroyedByReplicator);
					}
					else
					{
						InternalLogStreams.LogTraceObject?.Log(this, $"Handled remote destroy of {networkId3}: instance does not exist, so just destroy state");
						_simulation.DestroyStateObject(networkId3);
					}
				}
			}
			finally
			{
				try
				{
					for (int j = 0; j < _remotePrefabsWaitingForSpawnedCallback.Count; j++)
					{
						NetworkObject networkObject3 = _remotePrefabsWaitingForSpawnedCallback[j];
						Assert.Check(BehaviourUtils.IsAlive(networkObject3), "Remote prefab destroyed before having a chance to invoke Spawned");
						if (!networkObject3.Id.IsValid)
						{
							InternalLogStreams.LogWarn?.Log(networkObject3, "This object has been spawned and despawned in the same tick");
							_remotePrefabsWaitingForSpawnedCallback[j] = null;
						}
					}
					foreach (NetworkObject item in _remotePrefabsWaitingForSpawnedCallback)
					{
						if (!BehaviourUtils.IsNull(item) && IsAwakeAtInitialization(item))
						{
							InvokeBeforeSpawnedCallbacks(item, (AttachOptions)0, null);
						}
					}
					foreach (NetworkObject item2 in _remotePrefabsWaitingForSpawnedCallback)
					{
						if (!BehaviourUtils.IsNull(item2))
						{
							Assert.Check(BehaviourUtils.IsAlive(item2), "Remote prefab destroyed before having a chance to invoke Spawned");
							if (IsAwakeAtInitialization(item2))
							{
								InvokeSpawnedCallback(item2);
							}
						}
					}
					foreach (NetworkObject item3 in _remotePrefabsWaitingForSpawnedCallback)
					{
						if (!BehaviourUtils.IsNull(item3) && IsAwakeAtInitialization(item3))
						{
							InvokeAfterSpawnedCallback(item3);
						}
					}
				}
				finally
				{
					_remotePrefabsWaitingForSpawnedCallback.Clear();
				}
				CallbackInterfaceInvoker.IAfterUpdateRemotePrefabs(_behaviourUpdater);
			}
			void InstanceAcquired(NetworkObjectMeta networkObjectMeta, NetworkObject networkObject4)
			{
				if (networkObject4.Id.IsValid && networkObject4.Id == networkObjectMeta.Id && networkObject4.Id == networkObjectMeta.Id)
				{
					Assert.Fail("Already initialized for the same ID: {0}", LogUtils.GetDump(networkObject4));
				}
				Assert.Check(BehaviourUtils.IsAlive(networkObject4), "The instance has been destroyed {0} {1}", networkObjectMeta.Type, networkObjectMeta.Id);
				InitializeNetworkObjectInstance(networkObjectMeta, networkObject4, null, (AttachOptions)0, null);
				_remotePrefabsWaitingForSpawnedCallback.Add(networkObject4);
			}
		}

		private void ProcessSpawnQueue()
		{
			if (IsSceneManagerBusy || (Topology == Topologies.Shared && !LocalPlayer.IsRealPlayer))
			{
				return;
			}
			int count = _spawnQueue.Count;
			while (count-- > 0)
			{
				SpawnArgs args = _spawnQueue.Dequeue();
				NetworkSpawnOp networkSpawnOp = SpawnInternal(in args);
				if (networkSpawnOp.IsSpawned)
				{
					InternalLogStreams.LogTraceObject?.Log(networkSpawnOp.Object, $"Queued spawn completed for {args}");
				}
				else if (networkSpawnOp.IsQueued)
				{
					InternalLogStreams.LogTraceObject?.Log(this, $"Queued spawn for {args} has been requeued");
				}
				else if (args.Spawned == null)
				{
					InternalLogStreams.LogError?.Log(this, $"Queued spawn failed for {args}: {networkSpawnOp.Status}");
				}
				else
				{
					InternalLogStreams.LogTraceObject?.Log(this, $"Queued spawn failed for {args}: {networkSpawnOp.Status}");
				}
			}
		}

		void Simulation.ICallbacks.OnBeforeCopyPreviousState()
		{
			CallbackInterfaceInvoker.IBeforeCopyPreviousState(_behaviourUpdater);
		}

		void Simulation.ICallbacks.OnTick()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			try
			{
				Time.fixedDeltaTime = _simulation.DeltaTime;
				_behaviourUpdater.InvokeFixedUpdateNetwork(_simulation.Stage, _simulation.Mode, _simulation.Topology);
			}
			finally
			{
				Time.fixedDeltaTime = fixedDeltaTime;
			}
		}

		void Simulation.ICallbacks.OnServerStart()
		{
		}

		void Simulation.ICallbacks.OnClientStart()
		{
		}

		unsafe void Simulation.ICallbacks.OnInputMissing(SimulationInput input)
		{
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnInputMissing(this, input.Player, NetworkInput.FromRaw(input.Data, Simulation.Config.InputDataWordCount));
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		unsafe void Simulation.ICallbacks.OnInput(SimulationInput input)
		{
			if (!ProvideInput)
			{
				return;
			}
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnInput(this, NetworkInput.FromRaw(input.Data, Simulation.Config.InputDataWordCount));
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		void Simulation.ICallbacks.OnObjectChanged(in NetworkObjectChange change)
		{
			InternalLogStreams.LogTraceSendRecv?.Log(this, DumpChange("[Read]", in change));
			NetworkRunner.DebugEvent?.Invoke(new NetworkRunnerDebugEvent(this, NetworkRunnerDebugEventType.ObjectRead, in change));
			if (change.Id == NetworkId.SceneInfo)
			{
				_sceneInfoChangeSource = NetworkSceneInfoChangeSource.Remote;
				InternalLogStreams.LogTraceSceneInfo?.Log(this, "PendingSceneInfoChanges set to true for " + Simulation.DumpObject(NetworkId.SceneInfo));
			}
		}

		void Simulation.ICallbacks.OnObjectWrite(in NetworkObjectChange change)
		{
			InternalLogStreams.LogTraceSendRecv?.Log(this, DumpChange("[Write]", in change));
			NetworkRunner.DebugEvent?.Invoke(new NetworkRunnerDebugEvent(this, NetworkRunnerDebugEventType.ObjectWrite, in change));
		}

		private string DumpChange(string prefix, in NetworkObjectChange change)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Sim: ");
			stringBuilder.Append(_simulation?.GetHashCode());
			stringBuilder.Append(": ");
			stringBuilder.Append(_simulation?.Tick);
			stringBuilder.Append(": ");
			stringBuilder.Append(prefix);
			stringBuilder.Append(" ");
			stringBuilder.Append(change.ChangeType.ToString());
			stringBuilder.Append(" of ");
			stringBuilder.Append(change.Id.ToString());
			if (IsServer)
			{
				stringBuilder.Append(" for ");
				stringBuilder.Append(change.Player.ToString());
			}
			if (change.WordsAffected.Length > 0)
			{
				bool flag = true;
				ReadOnlySpan<(int, int)> wordsAffected = change.WordsAffected;
				for (int i = 0; i < wordsAffected.Length; i++)
				{
					(int, int) tuple = wordsAffected[i];
					stringBuilder.Append(flag ? ": " : ", ");
					stringBuilder.Append("[").Append(tuple.Item1).Append("]=")
						.AppendFormat("{0:X8}", tuple.Item2);
					flag = false;
				}
			}
			return stringBuilder.ToString();
		}

		bool Simulation.ICallbacks.OnStateAuthorityRequested(PlayerRef player, NetworkObjectMeta meta, bool acquire)
		{
			return true;
		}

		void Simulation.ICallbacks.OnBeforeSimulation(int forwardTickCount)
		{
			CallbackInterfaceInvoker.IBeforeSimulation(_behaviourUpdater, forwardTickCount);
		}

		void Simulation.ICallbacks.OnAfterSimulation()
		{
		}

		void Simulation.ICallbacks.OnBeforeClientSidePredictionReset()
		{
			CallbackInterfaceInvoker.IBeforeClientPredictionReset(_behaviourUpdater);
		}

		void Simulation.ICallbacks.OnAfterClientSidePredictionReset()
		{
			CallbackInterfaceInvoker.IAfterClientPredictionReset(_behaviourUpdater);
		}

		void Simulation.ICallbacks.OnBeforeAllTicks(bool resimulation, int tickCount)
		{
			CallbackInterfaceInvoker.IBeforeAllTicks(_behaviourUpdater, resimulation, tickCount);
		}

		void Simulation.ICallbacks.OnAfterAllTicks(bool resimulation, int tickCount)
		{
			CallbackInterfaceInvoker.IAfterAllTicks(_behaviourUpdater, resimulation, tickCount);
		}

		void Simulation.ICallbacks.OnBeforeTick()
		{
			SceneInfoUpdate();
			CallbackInterfaceInvoker.IBeforeTick(_behaviourUpdater);
		}

		void Simulation.ICallbacks.OnAfterTick()
		{
			CallbackInterfaceInvoker.IAfterTick(_behaviourUpdater);
		}

		void Simulation.ICallbacks.ObjectEnterAOI(PlayerRef player, NetworkId id)
		{
			if (!TryFindObject(id, out var networkObject))
			{
				return;
			}
			if (networkObject is IInterestEnter interestEnter)
			{
				try
				{
					interestEnter.InterestEnter(player);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(error);
				}
			}
			for (int i = 0; i < networkObject.NetworkedBehaviours.Length; i++)
			{
				if (networkObject.NetworkedBehaviours[i] is IInterestEnter interestEnter2)
				{
					try
					{
						interestEnter2.InterestEnter(player);
					}
					catch (Exception error2)
					{
						InternalLogStreams.LogException?.Log(error2);
					}
				}
			}
			for (int j = 0; j < _callbacks.Count; j++)
			{
				try
				{
					_callbacks[j].OnObjectEnterAOI(this, networkObject, player);
				}
				catch (Exception error3)
				{
					InternalLogStreams.LogException?.Log(this, error3);
				}
			}
		}

		void Simulation.ICallbacks.ObjectExitAOI(PlayerRef player, NetworkId id)
		{
			if (!TryFindObject(id, out var networkObject))
			{
				return;
			}
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnObjectExitAOI(this, networkObject, player);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
			if (networkObject is IInterestExit interestExit)
			{
				try
				{
					interestExit.InterestExit(player);
				}
				catch (Exception error2)
				{
					InternalLogStreams.LogException?.Log(error2);
				}
			}
			for (int j = 0; j < networkObject.NetworkedBehaviours.Length; j++)
			{
				if (networkObject.NetworkedBehaviours[j] is IInterestExit interestExit2)
				{
					try
					{
						interestExit2.InterestExit(player);
					}
					catch (Exception error3)
					{
						InternalLogStreams.LogException?.Log(error3);
					}
				}
			}
		}

		void Simulation.ICallbacks.OnConnectedToServer()
		{
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnConnectedToServer(this);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		void Simulation.ICallbacks.OnDisconnectedFromServer(NetDisconnectReason reason)
		{
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnDisconnectedFromServer(this, reason);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		void Simulation.ICallbacks.OnConnectionFailed(NetAddress remoteAddress, NetConnectFailedReason reason)
		{
			ShutdownReason shutdownReason = ShutdownReason.Error;
			string customMsg = string.Empty;
			switch (reason)
			{
			case NetConnectFailedReason.Timeout:
				shutdownReason = ShutdownReason.ConnectionTimeout;
				customMsg = "Connection Timeout";
				break;
			case NetConnectFailedReason.ServerFull:
				shutdownReason = ShutdownReason.GameIsFull;
				customMsg = "Game Is Full";
				break;
			case NetConnectFailedReason.ServerRefused:
				shutdownReason = ShutdownReason.ConnectionRefused;
				customMsg = "Connection Refused";
				break;
			}
			StartGameOperation?.SetException(new StartGameException(shutdownReason, customMsg));
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnConnectFailed(this, remoteAddress, reason);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		void Simulation.ICallbacks.OnReliableData(PlayerRef player, ReliableKey key, bool local, ReadOnlySpan<byte> dataArray)
		{
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnReliableDataReceived(this, player, key, dataArray);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		void Simulation.ICallbacks.OnReliableDataProgress(PlayerRef player, ReliableKey key, float progress)
		{
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnReliableDataProgress(this, player, key, progress);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		void Simulation.ICallbacks.PlayerJoined(PlayerRef player)
		{
			Assert.Check(!IsSceneManagerBusy, "!IsSceneManagerBusy");
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnPlayerJoined(this, player);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
			CallbackInterfaceInvoker.IPlayerJoined(_behaviourUpdater, player);
		}

		void Simulation.ICallbacks.PlayerLeft(PlayerRef player)
		{
			Assert.Check(!IsSceneManagerBusy, "!IsSceneManagerBusy");
			CallbackInterfaceInvoker.IPlayerLeft(_behaviourUpdater, player);
			for (int i = 0; i < _callbacks.Count; i++)
			{
				try
				{
					_callbacks[i].OnPlayerLeft(this, player);
				}
				catch (Exception error)
				{
					InternalLogStreams.LogException?.Log(this, error);
				}
			}
		}

		OnConnectionRequestReply Simulation.ICallbacks.OnConnectionRequest(NetAddress remoteAddress, byte[] token)
		{
			if (_callbacks.Count > 0)
			{
				NetworkRunnerCallbackArgs.ConnectRequest connectRequest = new NetworkRunnerCallbackArgs.ConnectRequest();
				connectRequest.RemoteAddress = remoteAddress;
				for (int i = 0; i < _callbacks.Count; i++)
				{
					try
					{
						_callbacks[i].OnConnectRequest(this, connectRequest, token);
					}
					catch (Exception error)
					{
						InternalLogStreams.LogException?.Log(this, error);
					}
				}
				if (connectRequest.Result.HasValue)
				{
					return connectRequest.Result.Value;
				}
			}
			return OnConnectionRequestReply.Ok;
		}

		void Simulation.ICallbacks.OnInternalConnectionAttempt(int attempt, int totalConnectionAttempts, out bool shouldChange, out NetAddress newAddress)
		{
			shouldChange = false;
			newAddress = default(NetAddress);
			if (IsCloudReady)
			{
				_cloudServices.OnInternalConnectionAttempt(attempt, totalConnectionAttempts, out shouldChange, out newAddress);
			}
		}

		void Simulation.ICallbacks.OnRejoinRequest(string sessionId)
		{
			if (IsCloudReady && IsClient)
			{
				_cloudServices.TriggerMoveSessionToNewRoom(sessionId);
			}
		}

		private NetworkSpawnOp SpawnInternal(in SpawnArgs args)
		{
			NetworkObjectSpawnDelegate spawnedCallback = args.Spawned;
			if (IsClient && Topology == Topologies.ClientServer)
			{
				return Failed(NetworkSpawnStatus.FailedClientCantSpawn);
			}
			Assert.Check(args.TypeId.IsValid, "args.TypeId.IsValid");
			if (Topology == Topologies.Shared && !LocalPlayer.IsRealPlayer)
			{
				if (args.Synchronous)
				{
					return Failed(NetworkSpawnStatus.FailedLocalPlayerNotYetSet);
				}
				return Incomplete(in args);
			}
			if (Simulation == null)
			{
				return args.Synchronous ? Failed(NetworkSpawnStatus.FailedSimulationNotRunning) : Incomplete(in args);
			}
			NetworkObject result;
			switch (TryAcquireInstance(args.TypeId, null, out result, args.Synchronous, args.DontDestroyOnLoad))
			{
			case CreateInstanceResult.InProgress:
				if (args.Synchronous && !_config.EnqueueIncompleteSynchronousSpawns)
				{
					return Failed(NetworkSpawnStatus.FailedToLoadPrefabSynchronously);
				}
				return Incomplete(in args);
			case CreateInstanceResult.Success:
			{
				ApplySpawnArgsUnity(result, in args);
				result.IsResume = IsResume;
				NetworkId id = (IsResume ? CheckIdOrGetNewId(args.ResumeNO) : Simulation.GetNextId());
				Simulation simulation = Simulation;
				int wordCount = NetworkObject.GetWordCount(result);
				NetworkObjectTypeId typeId = args.TypeId;
				int behaviourCount = result.NetworkedBehaviours.Length;
				NetworkObjectHeaderFlags flags = (NetworkObjectHeaderFlags)((int)((uint)FlagsFromInstance(result) | (uint)(args.DontDestroyOnLoad ? 64 : 0)) | (IsClient ? 4 : 0));
				NetworkObjectMeta networkObjectMeta = simulation.AllocateObject(id, wordCount, typeId, behaviourCount, default(NetworkId), default(NetworkObjectNestingKey), flags);
				AttachOptions options = AttachOptions.LocalSpawn;
				InitializeNetworkObjectInstance(networkObjectMeta, result, args.InputAuthority, options, args.MasterClientOverride);
				result.RuntimeFlags |= NetworkObjectRuntimeFlags.OwnsNestedObjects;
				for (int i = 0; i < result.NestedObjects?.Length; i++)
				{
					Assert.Check(args.TypeId.IsPrefab, "args.TypeId.IsPrefab");
					NetworkObject networkObject = result.NestedObjects[i];
					networkObject.RuntimeFlags |= NetworkObjectRuntimeFlags.IsNested;
					networkObject.RuntimeFlags |= NetworkObjectRuntimeFlags.OwnsNestedObjects;
					NetworkId id2 = (IsResume ? CheckIdOrGetNewId((args.ResumeNO != null) ? args.ResumeNO.NestedObjects[i] : null) : Simulation.GetNextId());
					NetworkObjectMeta meta = Simulation.AllocateObject(id2, NetworkObject.GetWordCount(networkObject), NetworkObjectTypeId.FromPrefabNestedObject(args.TypeId.AsPrefabId, i), networkObject.NetworkedBehaviours.Length, networkObjectMeta.Id, new NetworkObjectNestingKey(i + 1), FlagsFromInstance(networkObject));
					InitializeNetworkObjectInstance(meta, networkObject, args.InputAuthority, options, args.MasterClientOverride);
				}
				if (IsAwakeAtInitialization(result))
				{
					InitializeNetworkObjectState(result);
					NetworkObject[] nestedObjects = result.NestedObjects;
					foreach (NetworkObject networkObject2 in nestedObjects)
					{
						if (IsAwakeAtInitialization(networkObject2))
						{
							InitializeNetworkObjectState(networkObject2);
						}
					}
					InvokeBeforeSpawnedCallbacks(result, options, args.OnBeforeSpawned as OnBeforeSpawned);
					NetworkObject[] nestedObjects2 = result.NestedObjects;
					foreach (NetworkObject networkObject3 in nestedObjects2)
					{
						if (IsAwakeAtInitialization(networkObject3))
						{
							InvokeBeforeSpawnedCallbacks(networkObject3, options, null);
						}
					}
					InvokeSpawnedCallback(result);
					NetworkObject[] nestedObjects3 = result.NestedObjects;
					foreach (NetworkObject networkObject4 in nestedObjects3)
					{
						if (IsAwakeAtInitialization(networkObject4))
						{
							InvokeSpawnedCallback(networkObject4);
						}
					}
					InvokeAfterSpawnedCallback(result);
					NetworkObject[] nestedObjects4 = result.NestedObjects;
					foreach (NetworkObject networkObject5 in nestedObjects4)
					{
						if (IsAwakeAtInitialization(networkObject5))
						{
							InvokeAfterSpawnedCallback(networkObject5);
						}
					}
				}
				else
				{
					Assert.Check(!result.gameObject.activeInHierarchy, "Expected to be inactive {0}", result.Name);
				}
				if (IsClient && _config.Simulation.Topology == Topologies.Shared)
				{
					Simulation.Replicator.OnObjectSpawnedLocal(networkObjectMeta.Id);
					NetworkObject[] nestedObjects5 = result.NestedObjects;
					foreach (NetworkObject networkObject6 in nestedObjects5)
					{
						Assert.Check(BehaviourUtils.IsNotNull(networkObject6), "IsNotNull(nested)");
						if (BehaviourUtils.IsAlive(networkObject6))
						{
							if (networkObject6.Id.IsValid)
							{
								Simulation.Replicator.OnObjectSpawnedLocal(networkObject6.Id);
							}
							else
							{
								InternalLogStreams.LogTraceObject?.Warn(networkObject6, "Not invoking OnObjectSpawnedLocal for nested object because it has an invalid ID");
							}
						}
						else
						{
							InternalLogStreams.LogTraceObject?.Warn(networkObject6, "Not invoking OnObjectSpawnedLocal for nested object because it is not alive");
						}
					}
				}
				return Complete(result);
			}
			default:
				return Failed(NetworkSpawnStatus.FailedToCreateInstance);
			}
			NetworkId CheckIdOrGetNewId(NetworkObject obj)
			{
				return (obj != null && obj.Id.IsValid) ? obj.Id : Simulation.GetNextId();
			}
			NetworkSpawnOp Complete(NetworkObject instance)
			{
				NetworkSpawnOp result2 = new NetworkSpawnOp(this, NetworkSpawnStatus.Spawned, instance);
				spawnedCallback?.Invoke(result2);
				instance.IsResume = false;
				return result2;
			}
			NetworkSpawnOp Failed(NetworkSpawnStatus status)
			{
				NetworkSpawnOp result2 = new NetworkSpawnOp(this, status, (NetworkObject)null);
				spawnedCallback?.Invoke(result2);
				return result2;
			}
			NetworkSpawnOp Incomplete(in SpawnArgs spawnArgs)
			{
				if (spawnArgs.Synchronous)
				{
					_spawnQueue.Enqueue(spawnArgs);
					return new NetworkSpawnOp(this, NetworkSpawnStatus.Queued, (NetworkObject)null);
				}
				NetworkSpawnOp.AsyncOpData asyncOp = new NetworkSpawnOp.AsyncOpData
				{
					Status = NetworkSpawnStatus.Queued,
					Object = null
				};
				_spawnQueue.Enqueue(new SpawnArgs(in spawnArgs, delegate(NetworkSpawnOp op)
				{
					asyncOp.Complete(in op);
				}));
				return new NetworkSpawnOp(this, NetworkSpawnStatus.Queued, asyncOp);
			}
		}

		[Conditional("FUSION_UNITY")]
		private static void ApplySpawnArgsUnity(NetworkObject obj, in SpawnArgs spawnArgs)
		{
			bool hasValue = spawnArgs.Position.HasValue;
			bool hasValue2 = spawnArgs.Rotation.HasValue;
			if (hasValue)
			{
				obj.transform.position = spawnArgs.Position.Value;
			}
			if (hasValue2)
			{
				obj.transform.rotation = ((spawnArgs.Rotation.Value == default(Quaternion)) ? Quaternion.identity : spawnArgs.Rotation.Value);
			}
		}

		[Conditional("false")]
		private static void ApplySpawnArgsPlugin(NetworkObject obj, in SpawnArgs spawnArgs)
		{
			if (!spawnArgs.Position.HasValue && !spawnArgs.Rotation.HasValue)
			{
				return;
			}
			if (obj.Flags.HasNot(NetworkObjectFlags.HasMainNetworkTRSP))
			{
				InternalLogStreams.LogWarn?.Log(obj, "Spawn position or rotation used, but no HasMainNetworkTRSP flag baked");
				return;
			}
			NetworkBehaviour[] networkedBehaviours = obj.NetworkedBehaviours;
			if (networkedBehaviours == null || networkedBehaviours.Length == 0 || !(obj.NetworkedBehaviours[0] is NetworkTRSP))
			{
				InternalLogStreams.LogWarn?.Log(obj, "Spawn position or rotation used, but NetworkTRSP is not the first behaviour");
				return;
			}
			ref NetworkTRSPData reference = ref obj.NetworkedBehaviours[0].ReinterpretState<NetworkTRSPData>();
			if (spawnArgs.Position.HasValue)
			{
				reference.Position = spawnArgs.Position.Value;
			}
			if (spawnArgs.Rotation.HasValue)
			{
				reference.Rotation = spawnArgs.Rotation.Value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool CanAssignInputAuthority(NetworkObject networkObject, PlayerRef player)
		{
			int result;
			if (Topology != Topologies.ClientServer)
			{
				Simulation simulation = Simulation;
				result = ((simulation != null && simulation.IsLocalSimulationStateAuthority(ref networkObject.Header) && LocalPlayer == player) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
			return (byte)result != 0;
		}

		public T Spawn<T>(T prefab, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0) where T : SimulationBehaviour
		{
			if (BehaviourUtils.IsNull(prefab))
			{
				throw new ArgumentNullException("prefab");
			}
			if (!prefab.TryGetBehaviour<NetworkObject>(out var behaviour))
			{
				throw new ArgumentException("No NetworkObject component", "prefab");
			}
			if (Topology == Topologies.Shared && behaviour.Flags.Has(NetworkObjectFlags.MasterClientObject) && !IsSharedModeMasterClient)
			{
				throw new ArgumentException("Non master client tried to spawn MasterClientObject. Only the master client can spawn MasterClientObjects", "prefab");
			}
			NetworkObject networkObject = Spawn(behaviour, position, rotation, inputAuthority, onBeforeSpawned, flags);
			T component = null;
			if (BehaviourUtils.IsNotNull(networkObject) && !networkObject.TryGetComponent<T>(out component))
			{
				InternalLogStreams.LogError?.Log(this, "Found no " + typeof(T).FullName + " on the GameObject " + component.name + ". The prefab was instantiated.");
			}
			return component;
		}

		public NetworkObject Spawn(GameObject prefab, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (prefab == null)
			{
				throw new ArgumentNullException("prefab");
			}
			if (!prefab.TryGetComponent<NetworkObject>(out var component))
			{
				throw new ArgumentException("No NetworkObject component", "prefab");
			}
			return Spawn(component, position, rotation, inputAuthority, onBeforeSpawned, flags);
		}

		public NetworkObject Spawn(NetworkObject prefab, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (BehaviourUtils.IsNull(prefab))
			{
				throw new ArgumentNullException("prefab");
			}
			if (Topology == Topologies.Shared && prefab.Flags.Has(NetworkObjectFlags.MasterClientObject) && !IsSharedModeMasterClient)
			{
				throw new ArgumentException("Non master client tried to spawn MasterClientObject. Only the master client can spawn MasterClientObjects", "prefab");
			}
			NetworkObjectTypeId typeId = prefab.NetworkTypeId;
			if (!typeId.IsValid)
			{
				if (!prefab.TryGetComponent<NetworkObjectPrefabData>(out var component))
				{
					throw new InvalidOperationException($"Prefab {prefab} has not been added to the PrefabTable.");
				}
				if (_objectProvider == null)
				{
					throw new InvalidOperationException("Can't resolve prefab, no object provider");
				}
				NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, component.Guid);
				if (!prefabId.IsValid)
				{
					throw new InvalidOperationException($"Prefab {prefab} has been baked with a guid {component.Guid}, but such guid failed to be translated into a prefab id by the object provider.");
				}
				typeId = (prefab.NetworkTypeId = prefabId);
			}
			NetworkObject resumeNO = ((IsResume && prefab.Id.IsValid) ? prefab : null);
			return SpawnInternal(new SpawnArgs(typeId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(typeId);
		}

		public NetworkObject Spawn(NetworkPrefabRef prefabRef, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (!prefabRef.IsValid)
			{
				throw new ArgumentException("Not valid.", "prefabRef");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, (NetworkObjectGuid)prefabRef);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException($"Prefab {prefabRef} failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(prefabId);
		}

		public NetworkObject Spawn(string nameOrPath, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (string.IsNullOrEmpty(nameOrPath))
			{
				throw new ArgumentException("Not valid.", "nameOrPath");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, nameOrPath);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException("Prefab \"" + nameOrPath + "\" failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(prefabId);
		}

		public NetworkObject Spawn(NetworkObjectGuid prefabGuid, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (!prefabGuid.IsValid)
			{
				throw new ArgumentException("Not valid.", "prefabGuid");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, prefabGuid);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException($"Prefab {prefabGuid} failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(prefabId);
		}

		public NetworkObject Spawn(NetworkPrefabId typeId, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (!typeId.IsValid)
			{
				throw new ArgumentException("typeId");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(typeId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(typeId);
		}

		public NetworkSpawnStatus TrySpawn<T>(T prefab, out T obj, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0) where T : SimulationBehaviour
		{
			if (BehaviourUtils.IsNull(prefab))
			{
				throw new ArgumentNullException("prefab");
			}
			if (!prefab.TryGetBehaviour<NetworkObject>(out var behaviour))
			{
				throw new ArgumentException("No NetworkObject component", "prefab");
			}
			if (Topology == Topologies.Shared && behaviour.Flags.Has(NetworkObjectFlags.MasterClientObject) && !IsSharedModeMasterClient)
			{
				throw new ArgumentException("Non master client tried to spawn MasterClientObject. Only the master client can spawn MasterClientObjects", "prefab");
			}
			NetworkObject obj2;
			NetworkSpawnStatus result = TrySpawn(behaviour, out obj2, position, rotation, inputAuthority, onBeforeSpawned, flags);
			obj = null;
			if (BehaviourUtils.IsNotNull(obj2) && !obj2.TryGetComponent<T>(out obj))
			{
				InternalLogStreams.LogError?.Log(this, "Found no " + typeof(T).FullName + " on the GameObject " + obj.name + ". The prefab was instantiated.");
			}
			return result;
		}

		public NetworkSpawnStatus TrySpawn(GameObject prefab, out NetworkObject obj, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (prefab == null)
			{
				throw new ArgumentNullException("prefab");
			}
			if (!prefab.TryGetComponent<NetworkObject>(out var component))
			{
				throw new ArgumentException("No NetworkObject component", "prefab");
			}
			return TrySpawn(component, out obj, position, rotation, inputAuthority, onBeforeSpawned, flags);
		}

		public NetworkSpawnStatus TrySpawn(NetworkObject prefab, out NetworkObject obj, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (BehaviourUtils.IsNull(prefab))
			{
				throw new ArgumentNullException("prefab");
			}
			if (Topology == Topologies.Shared && prefab.Flags.Has(NetworkObjectFlags.MasterClientObject) && !IsSharedModeMasterClient)
			{
				throw new ArgumentException("Non master client tried to spawn MasterClientObject. Only the master client can spawn MasterClientObjects", "prefab");
			}
			NetworkObjectTypeId typeId = prefab.NetworkTypeId;
			if (!typeId.IsValid)
			{
				if (!prefab.TryGetComponent<NetworkObjectPrefabData>(out var component))
				{
					throw new InvalidOperationException($"Prefab {prefab} has not been added to the PrefabTable.");
				}
				if (_objectProvider == null)
				{
					throw new InvalidOperationException("Can't resolve prefab, no object provider");
				}
				NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, component.Guid);
				if (!prefabId.IsValid)
				{
					throw new InvalidOperationException($"Prefab {prefab} has been baked with a guid {component.Guid}, but such guid failed to be translated into a prefab id by the object provider.");
				}
				typeId = (prefab.NetworkTypeId = prefabId);
			}
			NetworkObject resumeNO = ((IsResume && prefab.Id.IsValid) ? prefab : null);
			return SpawnInternal(new SpawnArgs(typeId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(out obj);
		}

		public NetworkSpawnStatus TrySpawn(NetworkPrefabRef prefabRef, out NetworkObject obj, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (!prefabRef.IsValid)
			{
				throw new ArgumentException("Not valid.", "prefabRef");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, (NetworkObjectGuid)prefabRef);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException($"Prefab {prefabRef} failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(out obj);
		}

		public NetworkSpawnStatus TrySpawn(string nameOrPath, out NetworkObject obj, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (string.IsNullOrEmpty(nameOrPath))
			{
				throw new ArgumentException("Not valid.", "nameOrPath");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, nameOrPath);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException("Prefab \"" + nameOrPath + "\" failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(out obj);
		}

		public NetworkSpawnStatus TrySpawn(NetworkObjectGuid prefabGuid, out NetworkObject obj, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (!prefabGuid.IsValid)
			{
				throw new ArgumentException("Not valid.", "prefabGuid");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, prefabGuid);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException($"Prefab {prefabGuid} failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(out obj);
		}

		public NetworkSpawnStatus TrySpawn(NetworkPrefabId typeId, out NetworkObject obj, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0)
		{
			if (!typeId.IsValid)
			{
				throw new ArgumentException("typeId");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(typeId, position, rotation, inputAuthority, onBeforeSpawned, flags, null, synchronous: true, resumeNO)).ConsumeSyncSpawn(out obj);
		}

		public NetworkSpawnOp SpawnAsync<T>(T prefab, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0, NetworkObjectSpawnDelegate onCompleted = null) where T : SimulationBehaviour
		{
			if (BehaviourUtils.IsNull(prefab))
			{
				throw new ArgumentNullException("prefab");
			}
			if (!prefab.TryGetBehaviour<NetworkObject>(out var behaviour))
			{
				throw new ArgumentException("No NetworkObject component", "prefab");
			}
			if (Topology == Topologies.Shared && behaviour.Flags.Has(NetworkObjectFlags.MasterClientObject) && !IsSharedModeMasterClient)
			{
				throw new ArgumentException("Non master client tried to spawn MasterClientObject. Only the master client can spawn MasterClientObjects", "prefab");
			}
			T component = null;
			NetworkSpawnOp result = SpawnAsync(behaviour, position, rotation, inputAuthority, onBeforeSpawned, flags, onCompleted);
			NetworkObject networkObject = null;
			if (result.Status == NetworkSpawnStatus.Spawned)
			{
				networkObject = result.Object;
			}
			if (BehaviourUtils.IsNotNull(networkObject) && !networkObject.TryGetComponent<T>(out component))
			{
				InternalLogStreams.LogError?.Log(this, "Found no " + typeof(T).FullName + " on the GameObject " + component.name + ". The prefab was instantiated.");
			}
			return result;
		}

		public NetworkSpawnOp SpawnAsync(GameObject prefab, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0, NetworkObjectSpawnDelegate onCompleted = null)
		{
			if (prefab == null)
			{
				throw new ArgumentNullException("prefab");
			}
			if (!prefab.TryGetComponent<NetworkObject>(out var component))
			{
				throw new ArgumentException("No NetworkObject component", "prefab");
			}
			return SpawnAsync(component, position, rotation, inputAuthority, onBeforeSpawned, flags, onCompleted);
		}

		public NetworkSpawnOp SpawnAsync(NetworkObject prefab, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0, NetworkObjectSpawnDelegate onCompleted = null)
		{
			if (BehaviourUtils.IsNull(prefab))
			{
				throw new ArgumentNullException("prefab");
			}
			if (Topology == Topologies.Shared && prefab.Flags.Has(NetworkObjectFlags.MasterClientObject) && !IsSharedModeMasterClient)
			{
				throw new ArgumentException("Non master client tried to spawn MasterClientObject. Only the master client can spawn MasterClientObjects", "prefab");
			}
			NetworkObjectTypeId typeId = prefab.NetworkTypeId;
			if (!typeId.IsValid)
			{
				if (!prefab.TryGetComponent<NetworkObjectPrefabData>(out var component))
				{
					throw new InvalidOperationException($"Prefab {prefab} has not been added to the PrefabTable.");
				}
				if (_objectProvider == null)
				{
					throw new InvalidOperationException("Can't resolve prefab, no object provider");
				}
				NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, component.Guid);
				if (!prefabId.IsValid)
				{
					throw new InvalidOperationException($"Prefab {prefab} has been baked with a guid {component.Guid}, but such guid failed to be translated into a prefab id by the object provider.");
				}
				typeId = (prefab.NetworkTypeId = prefabId);
			}
			NetworkObject resumeNO = ((IsResume && prefab.Id.IsValid) ? prefab : null);
			return SpawnInternal(new SpawnArgs(typeId, position, rotation, inputAuthority, onBeforeSpawned, flags, onCompleted, synchronous: false, resumeNO));
		}

		public NetworkSpawnOp SpawnAsync(NetworkPrefabRef prefabRef, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0, NetworkObjectSpawnDelegate onCompleted = null)
		{
			if (!prefabRef.IsValid)
			{
				throw new ArgumentException("Not valid.", "prefabRef");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, (NetworkObjectGuid)prefabRef);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException($"Prefab {prefabRef} failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, onCompleted, synchronous: false, resumeNO));
		}

		public NetworkSpawnOp SpawnAsync(string nameOrPath, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0, NetworkObjectSpawnDelegate onCompleted = null)
		{
			if (string.IsNullOrEmpty(nameOrPath))
			{
				throw new ArgumentException("Not valid.", "nameOrPath");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, nameOrPath);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException("Prefab \"" + nameOrPath + "\" failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, onCompleted, synchronous: false, resumeNO));
		}

		public NetworkSpawnOp SpawnAsync(NetworkObjectGuid prefabGuid, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0, NetworkObjectSpawnDelegate onCompleted = null)
		{
			if (!prefabGuid.IsValid)
			{
				throw new ArgumentException("Not valid.", "prefabGuid");
			}
			NetworkPrefabId prefabId = _objectProvider.GetPrefabId(this, prefabGuid);
			if (!prefabId.IsValid)
			{
				throw new InvalidOperationException($"Prefab {prefabGuid} failed to be translated into a prefab id by the object provider.");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(prefabId, position, rotation, inputAuthority, onBeforeSpawned, flags, onCompleted, synchronous: false, resumeNO));
		}

		public NetworkSpawnOp SpawnAsync(NetworkPrefabId typeId, Vector3? position = null, Quaternion? rotation = null, PlayerRef? inputAuthority = null, OnBeforeSpawned onBeforeSpawned = null, NetworkSpawnFlags flags = (NetworkSpawnFlags)0, NetworkObjectSpawnDelegate onCompleted = null)
		{
			if (!typeId.IsValid)
			{
				throw new ArgumentException("typeId");
			}
			NetworkObject resumeNO = null;
			return SpawnInternal(new SpawnArgs(typeId, position, rotation, inputAuthority, onBeforeSpawned, flags, onCompleted, synchronous: false, resumeNO));
		}

		public async Task<StartGameResult> JoinSessionLobby(SessionLobby sessionLobby, string lobbyID = null, AuthenticationValues authentication = null, FusionAppSettings customAppSettings = null, CancellationToken cancellationToken = default(CancellationToken), bool useCachedRegions = true)
		{
			InternalLogStreams.LogDebug?.Log(this, $"Joining Lobby {sessionLobby} {lobbyID}");
			try
			{
				await ConnectToCloud(authentication, customAppSettings, null, null, cancellationToken, useCachedRegions);
				if (!IsCloudReady || _cloudServices.IsInRoom)
				{
					throw new StartGameException();
				}
				if (await _cloudServices.JoinSessionLobby(sessionLobby, lobbyID, LobbyType.Default, cancellationToken) != 0)
				{
					throw new StartGameException();
				}
				_simulationShutdown = (ShutdownFlags)0;
			}
			catch (Exception e)
			{
				return await ShutdownAndBuildResult(e);
			}
			return new StartGameResult();
		}

		public Task<StartGameResult> StartGame(StartGameArgs args)
		{
			if (_alreadyInitialized)
			{
				InternalLogStreams.LogError?.Log("Failed: NetworkRunner should not be reused.");
				return Task.FromResult(new StartGameResult(ShutdownReason.OperationCanceled));
			}
			_alreadyInitialized = true;
			if (_cloudServices?.IsInLobby != true && (IsStarting || IsRunning))
			{
				return Task.FromResult(new StartGameResult(ShutdownReason.AlreadyRunning));
			}
			args.GameMode = args.HostMigrationToken?.GameMode ?? args.GameMode;
			args.DisableNATPunchthrough |= FusionPlatform.IsWebGL;
			args.Config = (args.Config ?? NetworkProjectConfig.Global).Copy();
			InternalLogStreams.LogDebug?.Log(this, $"SDK: {Versioning.GetCurrentVersion}");
			InternalLogStreams.LogDebug?.Log(this, $"StartGame: {args}");
			GameMode = args.GameMode;
			_simulationShutdown = (ShutdownFlags)0;
			switch (args.GameMode)
			{
			case GameMode.Single:
				return StartGameModeSinglePlayer(args);
			case GameMode.Shared:
			case GameMode.Server:
			case GameMode.Host:
			case GameMode.Client:
			case GameMode.AutoHostOrClient:
				if (FusionPlatform.IsWebGL)
				{
					GameMode gameMode = args.GameMode;
					GameMode gameMode2 = gameMode;
					if ((uint)(gameMode2 - 3) <= 3u)
					{
						InternalLogStreams.LogDebug?.Warn(this, $"The GameMode {args.GameMode} is not intended for use on WebGL builds. " + "For more information, please refer to the Fusion Introduction Page at https://doc.photonengine.com/fusion/v2/fusion-intro. Consider using Shared Mode or switching to a platform that fully supports the selected GameMode.");
						if (!args.Config.AllowClientServerModesInWebGL && args.GameMode != GameMode.Client)
						{
							InternalLogStreams.LogDebug?.Error(this, $"The GameMode {args.GameMode} is not allowed in WebGL builds by default. If you still want to use it, please enable it in the NetworkProjectConfig.");
							return Task.FromResult(new StartGameResult(ShutdownReason.IncompatibleConfiguration));
						}
					}
				}
				return StartGameModeCloud(args);
			default:
				GameMode = (GameMode)0;
				return Task.FromResult(new StartGameResult(ShutdownReason.IncompatibleConfiguration));
			}
		}

		internal Task ConnectToCloud(AuthenticationValues authentication = null, FusionAppSettings customAppSettings = null, RealtimeClient realtimeClient = null, CloudCommunicator externalCommunicator = null, CancellationToken cancellationToken = default(CancellationToken), bool useCachedRegions = false)
		{
			FusionAppSettings fusionAppSettings = customAppSettings;
			if (fusionAppSettings == null && PhotonAppSettings.TryGetGlobal(out var settings))
			{
				fusionAppSettings = settings.AppSettings;
			}
			if (fusionAppSettings == null)
			{
				throw new InvalidOperationException("Photon Application Settings not found.");
			}
			if (useCachedRegions && !string.IsNullOrEmpty(_cachedRegionSummary))
			{
				fusionAppSettings.BestRegionSummaryFromStorage = _cachedRegionSummary;
			}
			if (_cloudServices == null)
			{
				_cloudServices = new CloudServices(this, externalCommunicator, realtimeClient, fusionAppSettings);
			}
			SessionInfo = new SessionInfo(this);
			if (_cloudServices.IsCloudReady)
			{
				InternalLogStreams.LogDebug?.Log(this, "Skip connecting to Photon Cloud. Already connected and ready.");
				return Task.CompletedTask;
			}
			return _cloudServices.ConnectToCloud(fusionAppSettings, authentication, cancellationToken);
		}

		private async Task DisconnectFromCloud()
		{
			if (_cloudServices != null)
			{
				await _cloudServices.DisconnectFromCloud();
				_cloudServices.Dispose();
				_cloudServices = null;
			}
		}

		private async Task<StartGameResult> StartGameModeSinglePlayer(StartGameArgs args)
		{
			StartGameOperation = new AsyncOperationHandler<ShutdownReason>(args.StartGameCancellationToken, 120f, "StartGame Timed out");
			try
			{
				if (args.StartGameCancellationToken != default(CancellationToken))
				{
					args.StartGameCancellationToken.ThrowIfCancellationRequested();
				}
				if (args.Config.Simulation.TickRateSelection.Equals(Fusion.TickRate.Zero))
				{
					args.Config.Simulation.TickRateSelection = Fusion.TickRate.GetDefault(args.GameMode);
				}
				NetworkRunnerInitializeArgs runnerArgs = new NetworkRunnerInitializeArgs
				{
					Scene = args.Scene,
					SimulationMode = SimulationModes.Host,
					Address = null,
					PlayerCount = 1,
					Config = args.Config,
					OnGameStarted = args.OnGameStarted,
					ObjectProvider = args.ObjectProvider,
					SceneManager = args.SceneManager,
					Updater = args.Updater,
					ObjectInitializer = args.ObjectInitializer,
					CustomCallbackInterfaces = args.CustomCallbackInterfaces,
					ConnectionToken = args.ConnectionToken
				};
				await Initialize(runnerArgs);
				ShutdownReason result = await StartGameOperation.Task;
				SessionInfo = new SessionInfo(this);
				if (args.SessionProperties != null)
				{
					SessionInfo.UpdateCustomProperties(args.SessionProperties);
				}
				InternalLogStreams.LogDebug?.Log($"StartGame Operation Done: {result}");
				return new StartGameResult();
			}
			catch (Exception e)
			{
				return await ShutdownAndBuildResult(e);
			}
			finally
			{
				StartGameOperation = null;
			}
			IL_0402:
			throw null;
		}

		private async Task<StartGameResult> StartGameModeCloud(StartGameArgs args)
		{
			StartGameOperation = new AsyncOperationHandler<ShutdownReason>(args.StartGameCancellationToken, 120f, "StartGame Timed out");
			SimulationModes? simulationMode = null;
			args.Config.Simulation.Topology = Topologies.ClientServer;
			switch (args.GameMode)
			{
			case GameMode.Server:
				simulationMode = SimulationModes.Server;
				break;
			case GameMode.Host:
				simulationMode = SimulationModes.Host;
				break;
			case GameMode.Client:
				simulationMode = SimulationModes.Client;
				break;
			case GameMode.Shared:
				simulationMode = SimulationModes.Client;
				args.Config.Simulation.Topology = Topologies.Shared;
				break;
			default:
				throw new StartGameException(ShutdownReason.InvalidArguments, string.Format("{0} set to {1}, which is invalid in this context", "GameMode", GameMode));
			case GameMode.AutoHostOrClient:
				break;
			}
			try
			{
				if (args.StartGameCancellationToken != default(CancellationToken))
				{
					args.StartGameCancellationToken.ThrowIfCancellationRequested();
					args.StartGameCancellationToken.Register(delegate
					{
						Shutdown(destroyGameObject: true, ShutdownReason.OperationCanceled);
					});
				}
				if (args.SessionProperties != null)
				{
					if (args.SessionProperties.Count > 10)
					{
						throw new StartGameException(ShutdownReason.InvalidArguments, "Max number of Custom Session Properties reached, only 10 properties are allowed.");
					}
					int customPropertiesSize = RealtimeExtensionsDictionaryProperties.CalculateTotalSize(args.SessionProperties);
					if (customPropertiesSize > 500)
					{
						throw new StartGameException(ShutdownReason.InvalidArguments, $"Max size of Custom Session Properties reached, current size of {customPropertiesSize} bytes, max 500 bytes are allowed.");
					}
				}
				if (args.Config.Simulation.PlayerCount == 0)
				{
					InternalLogStreams.LogError?.Log("NetworkRunner started with a PlayerCount of 0 in NetworkProjectConfig.SimulationConfig. No client will be able to connect.");
				}
				if (args.Config.Simulation.TickRateSelection.Equals(Fusion.TickRate.Zero))
				{
					args.Config.Simulation.TickRateSelection = Fusion.TickRate.GetDefault(args.GameMode);
				}
				if (args.GameMode == GameMode.Shared)
				{
					if (!args.Config.Simulation.TickRateSelection.IsValidForShared(out var invalidReason))
					{
						InternalLogStreams.LogWarn?.Log("The tick rate selection set in NetworkProjectConfig is invalid for Shared Mode: " + invalidReason + " Using the default Shared Mode tick rate configuration:" + $"\n{Fusion.TickRate.Resolve(Fusion.TickRate.SharedDefault)} instead of the requested one:\n{Fusion.TickRate.Resolve(args.Config.Simulation.TickRateSelection)}");
						args.Config.Simulation.TickRateSelection = Fusion.TickRate.SharedDefault;
					}
					ref SimulationConfig.DataConsistency objectDataConsistency = ref args.Config.Simulation.ObjectDataConsistency;
					SimulationConfig.DataConsistency requiredConsistency = SimulationConfig.DataConsistency.Full;
					if (objectDataConsistency != requiredConsistency)
					{
						InternalLogStreams.LogWarn?.Log(this, "Invalid ObjectDataConsistency. Shared Mode started with ObjectDataConsistency in " + $"NetworkProjectConfig set to: {objectDataConsistency}\n" + $"Overriding with ObjectDataConsistency:{requiredConsistency}");
						objectDataConsistency = requiredConsistency;
					}
					Assert.Always(objectDataConsistency == SimulationConfig.DataConsistency.Full, "objectDataConsistency == SimulationConfig.DataConsistency.Full");
				}
				await ConnectToCloud(args.AuthValues, args.CustomPhotonAppSettings, args.RealtimeClient, args.HostMigrationToken?.CloudCommunicator, args.StartGameCancellationToken, args.UseCachedRegions ?? true);
				_cloudServices.IsNATPunchthroughEnabled = !args.DisableNATPunchthrough;
				_cloudServices.CustomSTUNServer = args.CustomSTUNServer;
				await _cloudServices.EnterRoom(args);
				_cloudServices.OnRoomChanged();
				if (GameMode == GameMode.AutoHostOrClient && !simulationMode.HasValue)
				{
					if (!_cloudServices.IsMasterClient)
					{
						SimulationModes? simulationModes = SimulationModes.Client;
						simulationMode = simulationModes;
						GameMode = GameMode.Client;
					}
					else
					{
						SimulationModes? simulationModes = SimulationModes.Host;
						simulationMode = simulationModes;
						GameMode = GameMode.Host;
					}
				}
				if (!simulationMode.HasValue)
				{
					throw new StartGameException(ShutdownReason.Error, "Invalid Simulation Mode");
				}
				_cloudServices.UpdateInitializeArgs(new NetworkRunnerInitializeArgs
				{
					SimulationMode = simulationMode,
					Scene = args.Scene,
					Address = args.Address.GetValueOrDefault(NetAddress.Any(0)),
					PublicAddress = args.CustomPublicAddress,
					Config = args.Config,
					PlayerCount = args.PlayerCount,
					OnGameStarted = args.OnGameStarted,
					ObjectProvider = args.ObjectProvider,
					SceneManager = args.SceneManager,
					Updater = args.Updater,
					CustomCallbackInterfaces = args.CustomCallbackInterfaces,
					ConnectionToken = args.ConnectionToken,
					ResumeState = args.HostMigrationToken?.ResumeState,
					ResumeTick = args.HostMigrationToken?.ResumeTick,
					ResumeId = args.HostMigrationToken?.ResumeId,
					HostMigrationResume = args.HostMigrationResume
				});
				InternalLogStreams.LogDebug?.Log("Starting StartGame Operation");
				await _cloudServices.SendJoin(args.StartGameCancellationToken, args.PlayerUniqueId);
				ShutdownReason result = await StartGameOperation.Task;
				InternalLogStreams.LogDebug?.Log($"StartGame Operation Done: {result}");
				if (args.SessionProperties != null)
				{
					SessionInfo.UpdateCustomProperties(args.SessionProperties);
				}
			}
			catch (Exception e)
			{
				return await ShutdownAndBuildResult(e);
			}
			finally
			{
				StartGameOperation = null;
			}
			return new StartGameResult();
		}

		private async Task<StartGameResult> ShutdownAndBuildResult(Exception e)
		{
			InternalLogStreams.LogWarn?.Log(e.ToString());
			StartGameResult result = StartGameResult.BuildGameResultFromException(e);
			InternalLogStreams.LogDebug?.Error(this, $"StartGame Failed: {result}");
			await Shutdown(destroyGameObject: true, result.ShutdownReason);
			return result;
		}

		internal void InvokeSessionListUpdated(List<SessionInfo> sessionList)
		{
			try
			{
				for (int i = 0; i < _callbacks.Count; i++)
				{
					_callbacks[i].OnSessionListUpdated(this, sessionList);
				}
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(this, error);
			}
		}

		internal void InvokeCustomAuthenticationResponse(Dictionary<string, object> data)
		{
			try
			{
				for (int i = 0; i < _callbacks.Count; i++)
				{
					_callbacks[i].OnCustomAuthenticationResponse(this, data);
				}
			}
			catch (Exception error)
			{
				InternalLogStreams.LogException?.Log(this, error);
			}
		}
	}
}
