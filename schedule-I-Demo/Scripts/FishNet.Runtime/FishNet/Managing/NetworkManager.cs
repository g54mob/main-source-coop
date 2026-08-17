using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet.Authenticating;
using FishNet.Component.ColliderRollback;
using FishNet.Connection;
using FishNet.Documenting;
using FishNet.Managing.Client;
using FishNet.Managing.Debugging;
using FishNet.Managing.Logging;
using FishNet.Managing.Object;
using FishNet.Managing.Observing;
using FishNet.Managing.Predicting;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using FishNet.Managing.Statistic;
using FishNet.Managing.Timing;
using FishNet.Managing.Transporting;
using FishNet.Object;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using GameKit.Utilities;
using UnityEngine;

namespace FishNet.Managing
{
	[DefaultExecutionOrder(-32768)]
	[DisallowMultipleComponent]
	[AddComponentMenu("FishNet/Manager/NetworkManager")]
	public sealed class NetworkManager : MonoBehaviour
	{
		public enum HostIterationOrder
		{
			ServerFirst = 0,
			ClientFirst = 1
		}

		public enum PersistenceType
		{
			DestroyNewest = 0,
			DestroyOldest = 1,
			AllowMultiple = 2
		}

		private static List<NetworkManager> _instances = new List<NetworkManager>();

		internal static ushort StartingRpcLinkIndex;

		[Tooltip("True to refresh the DefaultPrefabObjects collection whenever the editor enters play mode. This is an attempt to alleviate the DefaultPrefabObjects scriptable object not refreshing when using multiple editor applications such as ParrelSync.")]
		[SerializeField]
		private bool _refreshDefaultPrefabs;

		[Tooltip("True to have your application run while in the background.")]
		[SerializeField]
		private bool _runInBackground = true;

		[Tooltip("True to make this instance DontDestroyOnLoad. This is typical if you only want one NetworkManager.")]
		[SerializeField]
		private bool _dontDestroyOnLoad = true;

		[Tooltip("Object pool to use for this NetworkManager. Value may be null.")]
		[SerializeField]
		private ObjectPool _objectPool;

		[Tooltip("How to persist when other NetworkManagers are introduced.")]
		[SerializeField]
		private PersistenceType _persistence;

		private bool _canPersist;

		internal const ushort MAXIMUM_FRAMERATE = 500;

		[Tooltip("Logging configuration to use. When empty default logging settings will be used.")]
		[SerializeField]
		private LoggingConfiguration _logging;

		private const string ERROR_LOGGING_PREFIX = "Error - ";

		private const string WARNING_LOGGING_PREFIX = "Warning - ";

		private const string COMMON_LOGGING_PREFIX = "Log - ";

		[Tooltip("Collection to use for spawnable objects.")]
		[SerializeField]
		private PrefabObjects _spawnablePrefabs;

		private Dictionary<ushort, PrefabObjects> _runtimeSpawnablePrefabs = new Dictionary<ushort, PrefabObjects>();

		private Dictionary<string, List<Action<UnityEngine.Component>>> _pendingInvokes = new Dictionary<string, List<Action<UnityEngine.Component>>>();

		private Dictionary<string, UnityEngine.Component> _registeredComponents = new Dictionary<string, UnityEngine.Component>();

		public bool Initialized { get; private set; }

		public static IReadOnlyCollection<NetworkManager> Instances
		{
			get
			{
				for (int i = 0; i < _instances.Count; i++)
				{
					if (_instances[i] == null)
					{
						_instances.RemoveAt(i);
						i--;
					}
				}
				return _instances;
			}
		}

		public bool IsServer => ServerManager.Started;

		public bool IsServerOnly
		{
			get
			{
				if (IsServer)
				{
					return !IsClient;
				}
				return false;
			}
		}

		public bool IsClient
		{
			get
			{
				if (ClientManager.Started)
				{
					return ClientManager.Connection.Authenticated;
				}
				return false;
			}
		}

		public bool IsClientOnly
		{
			get
			{
				if (!IsServer)
				{
					return IsClient;
				}
				return false;
			}
		}

		public bool IsHost
		{
			get
			{
				if (IsServer)
				{
					return IsClient;
				}
				return false;
			}
		}

		public bool IsOffline
		{
			get
			{
				if (!IsServer)
				{
					return !IsClient;
				}
				return false;
			}
		}

		internal PredictionManager PredictionManager { get; private set; }

		public ServerManager ServerManager { get; private set; }

		public ClientManager ClientManager { get; private set; }

		public TransportManager TransportManager { get; private set; }

		public TimeManager TimeManager { get; private set; }

		public SceneManager SceneManager { get; private set; }

		public ObserverManager ObserverManager { get; private set; }

		[Obsolete("Use ServerManager.GetAuthenticator or ServerManager.SetAuthenticator instead.")]
		public Authenticator Authenticator => ServerManager.Authenticator;

		public DebugManager DebugManager { get; private set; }

		public StatisticsManager StatisticsManager { get; private set; }

		[APIExclude]
		public static NetworkConnection EmptyConnection { get; private set; } = new NetworkConnection();

		public ObjectPool ObjectPool => _objectPool;

		public RollbackManager RollbackManager { get; private set; }

		public PrefabObjects SpawnablePrefabs
		{
			get
			{
				return _spawnablePrefabs;
			}
			set
			{
				_spawnablePrefabs = value;
			}
		}

		public IReadOnlyDictionary<ushort, PrefabObjects> RuntimeSpawnablePrefabs => _runtimeSpawnablePrefabs;

		private void Awake()
		{
			InitializeLogging();
			if (!ValidateSpawnablePrefabs(print: true))
			{
				return;
			}
			if (StartingRpcLinkIndex == 0)
			{
				StartingRpcLinkIndex = (ushort)(Enums.GetHighestValue<PacketId>() + 1);
			}
			if (SpawnablePrefabs != null && SpawnablePrefabs is DefaultPrefabObjects)
			{
				DefaultPrefabObjects defaultPrefabObjects = (DefaultPrefabObjects)SpawnablePrefabs;
				DefaultPrefabObjects defaultPrefabObjects2 = ScriptableObject.CreateInstance<DefaultPrefabObjects>();
				defaultPrefabObjects2.AddObjects(defaultPrefabObjects.Prefabs.ToList());
				defaultPrefabObjects2.Sort();
				SpawnablePrefabs = defaultPrefabObjects2;
			}
			_canPersist = CanInitialize();
			if (_canPersist)
			{
				if (TryGetComponent<NetworkObject>(out var _))
				{
					LogError("NetworkObject component found on the NetworkManager object " + base.gameObject.name + ". This is not allowed and will cause problems. Remove the NetworkObject component from this object.");
				}
				SpawnablePrefabs.InitializePrefabRange(0);
				SpawnablePrefabs.SetCollectionId(0);
				SetDontDestroyOnLoad();
				SetRunInBackground();
				DebugManager = GetOrCreateComponent<DebugManager>();
				TransportManager = GetOrCreateComponent<TransportManager>();
				ServerManager = GetOrCreateComponent<ServerManager>();
				ClientManager = GetOrCreateComponent<ClientManager>();
				TimeManager = GetOrCreateComponent<TimeManager>();
				SceneManager = GetOrCreateComponent<SceneManager>();
				ObserverManager = GetOrCreateComponent<ObserverManager>();
				RollbackManager = GetOrCreateComponent<RollbackManager>();
				PredictionManager = GetOrCreateComponent<PredictionManager>();
				StatisticsManager = GetOrCreateComponent<StatisticsManager>();
				if (_objectPool == null)
				{
					_objectPool = GetOrCreateComponent<DefaultObjectPool>();
				}
				InitializeComponents();
				_instances.Add(this);
				Initialized = true;
			}
		}

		private void Start()
		{
			ServerManager.StartForHeadless();
		}

		private void OnDestroy()
		{
			_instances.Remove(this);
		}

		private void InitializeComponents()
		{
			TimeManager.InitializeOnce_Internal(this);
			TimeManager.OnLateUpdate += TimeManager_OnLateUpdate;
			SceneManager.InitializeOnce_Internal(this);
			TransportManager.InitializeOnce_Internal(this);
			ClientManager.InitializeOnce_Internal(this);
			ServerManager.InitializeOnce_Internal(this);
			ObserverManager.InitializeOnce_Internal(this);
			RollbackManager.InitializeOnce_Internal(this);
			PredictionManager.InitializeOnce(this);
			StatisticsManager.InitializeOnce_Internal(this);
			_objectPool.InitializeOnce(this);
		}

		internal void UpdateFramerate()
		{
			bool started = ClientManager.Started;
			bool started2 = ServerManager.Started;
			int num = 0;
			if (started && started2)
			{
				num = Math.Max(ServerManager.FrameRate, ClientManager.FrameRate);
			}
			else if (started)
			{
				num = ClientManager.FrameRate;
			}
			else if (started2)
			{
				num = ServerManager.FrameRate;
			}
			if (num > 0)
			{
				Application.targetFrameRate = num;
			}
		}

		private void TimeManager_OnLateUpdate()
		{
			SetRunInBackground();
		}

		private bool CanInitialize()
		{
			if (_persistence == PersistenceType.AllowMultiple)
			{
				return true;
			}
			List<NetworkManager> list = Instances.ToList();
			if (list.Count == 0)
			{
				return true;
			}
			NetworkManager networkManager = list[0];
			if (_persistence == PersistenceType.DestroyNewest)
			{
				Log($"NetworkManager on object {base.gameObject.name} is being destroyed due to persistence type {_persistence}. A NetworkManager instance already exist on {networkManager.name}.");
				UnityEngine.Object.Destroy(base.gameObject);
				return false;
			}
			if (_persistence == PersistenceType.DestroyOldest)
			{
				Log($"NetworkManager on object {networkManager.name} is being destroyed due to persistence type {_persistence}. A NetworkManager instance has been created on {base.gameObject.name}.");
				UnityEngine.Object.Destroy(networkManager.gameObject);
				return true;
			}
			Log($"Persistance type of {_persistence} is unhandled on {base.gameObject.name}. Initialization will not proceed.");
			return false;
		}

		private bool ValidateSpawnablePrefabs(bool print)
		{
			if (SpawnablePrefabs == null && !string.IsNullOrEmpty(base.gameObject.scene.name))
			{
				if (print)
				{
					Debug.LogError("SpawnablePrefabs is null on " + base.gameObject.name + ". Select the NetworkManager in scene " + base.gameObject.scene.name + " and choose a prefabs file. Choosing DefaultPrefabObjects will automatically populate prefabs for you.");
				}
				return false;
			}
			return true;
		}

		private void SetDontDestroyOnLoad()
		{
			if (_dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(this);
			}
		}

		private void SetRunInBackground()
		{
			Application.runInBackground = _runInBackground;
		}

		private T GetOrCreateComponent<T>(T presetValue = null) where T : UnityEngine.Component
		{
			if (presetValue != null)
			{
				return presetValue;
			}
			if (base.gameObject.TryGetComponent<T>(out var component))
			{
				return component;
			}
			return base.gameObject.AddComponent<T>();
		}

		internal void ClearClientsCollection(Dictionary<int, NetworkConnection> clients, int transportIndex = -1)
		{
			bool flag = transportIndex < 0;
			List<int> list = CollectionCaches<int>.RetrieveList();
			foreach (KeyValuePair<int, NetworkConnection> client in clients)
			{
				NetworkConnection value = client.Value;
				if (!flag)
				{
					if (value.TransportIndex == transportIndex)
					{
						list.Add(client.Key);
						value.Dispose();
					}
				}
				else
				{
					value.Dispose();
				}
			}
			if (flag)
			{
				clients.Clear();
			}
			else
			{
				foreach (int item in list)
				{
					clients.Remove(item);
				}
			}
			CollectionCaches<int>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NetworkObject GetPooledInstantiated(NetworkObject prefab, bool asServer)
		{
			return GetPooledInstantiated(prefab, prefab.transform.position, prefab.transform.rotation, asServer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NetworkObject GetPooledInstantiated(NetworkObject prefab, Vector3 position, Quaternion rotation, bool asServer)
		{
			return GetPooledInstantiated(prefab.PrefabId, prefab.SpawnableCollectionId, position, rotation, asServer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use GetPooledInstantiated(NetworkObject,bool).")]
		public NetworkObject GetPooledInstantiated(NetworkObject prefab, ushort collectionId, bool asServer)
		{
			return GetPooledInstantiated(prefab.PrefabId, collectionId, asServer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NetworkObject GetPooledInstantiated(GameObject prefab, bool asServer)
		{
			if (!prefab.TryGetComponent<NetworkObject>(out var component))
			{
				LogError($"NetworkObject was not found on {prefab}. An instantiated NetworkObject cannot be returned.");
				return null;
			}
			return GetPooledInstantiated(component.PrefabId, component.SpawnableCollectionId, asServer);
		}

		[Obsolete("Use GetPooledInstantiated(GameObject, bool).")]
		public NetworkObject GetPooledInstantiated(GameObject prefab, ushort collectionId, bool asServer)
		{
			return GetPooledInstantiated(prefab, asServer);
		}

		public NetworkObject GetPooledInstantiated(GameObject prefab, Vector3 position, Quaternion rotation, bool asServer)
		{
			if (!prefab.TryGetComponent<NetworkObject>(out var component))
			{
				LogError($"NetworkObject was not found on {prefab}. An instantiated NetworkObject cannot be returned.");
				return null;
			}
			return GetPooledInstantiated(component.PrefabId, component.SpawnableCollectionId, position, rotation, asServer);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use GetPooledInstantiated(int, ushort, bool).")]
		public NetworkObject GetPooledInstantiated(int prefabId, bool asServer)
		{
			return GetPooledInstantiated(prefabId, 0, asServer);
		}

		public NetworkObject GetPooledInstantiated(int prefabId, ushort collectionId, bool asServer)
		{
			return _objectPool.RetrieveObject(prefabId, collectionId, asServer);
		}

		public NetworkObject GetPooledInstantiated(int prefabId, ushort collectionId, Vector3 position, Quaternion rotation, bool asServer)
		{
			return _objectPool.RetrieveObject(prefabId, collectionId, position, rotation, asServer);
		}

		[Obsolete("Use StorePooledInstantiated(NetworkObject, bool)")]
		public void StorePooledInstantiated(NetworkObject instantiated, int prefabId, bool asServer)
		{
			StorePooledInstantiated(instantiated, asServer);
		}

		public void StorePooledInstantiated(NetworkObject instantiated, bool asServer)
		{
			if (instantiated.IsSpawned)
			{
				LogWarning("NetworkObject " + instantiated.ToString() + " cannot be stored because it is still spawned. The object will be destroyed instead.");
				UnityEngine.Object.Destroy(instantiated);
			}
			else if (instantiated.IsNested)
			{
				Log("NetworkObject " + instantiated.ToString() + " cannot be stored because it is a nested prefab.");
			}
			else
			{
				_objectPool.StoreObject(instantiated, asServer);
			}
		}

		public void CacheObjects(NetworkObject prefab, int count, bool asServer)
		{
			_objectPool.CacheObjects(prefab, count, asServer);
		}

		private void InitializeLogging()
		{
			if (_logging == null)
			{
				_logging = ScriptableObject.CreateInstance<LevelLoggingConfiguration>();
			}
			else
			{
				_logging = _logging.Clone();
			}
			_logging.InitializeOnce();
		}

		[APIExclude]
		public static bool StaticCanLog(LoggingType loggingType)
		{
			NetworkManager networkManager = InstanceFinder.NetworkManager;
			if (!(networkManager == null))
			{
				return networkManager.CanLog(loggingType);
			}
			return false;
		}

		public bool CanLog(LoggingType loggingType)
		{
			return _logging.CanLog(loggingType);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		public static void StaticLog(string value)
		{
			InstanceFinder.NetworkManager?.Log(value);
		}

		public void Log(string value)
		{
			_logging.Log(value);
		}

		public void Log(LoggingType loggingType, string value)
		{
			switch (loggingType)
			{
			case LoggingType.Common:
				_logging.Log(value);
				break;
			case LoggingType.Warning:
				_logging.LogWarning(value);
				break;
			case LoggingType.Error:
				_logging.LogError(value);
				break;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		public static void StaticLogWarning(string value)
		{
			InstanceFinder.NetworkManager?.LogWarning(value);
		}

		public void LogWarning(string value)
		{
			_logging.LogWarning(value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		public static void StaticLogError(string value)
		{
			InstanceFinder.NetworkManager?.LogError(value);
		}

		public void LogError(string value)
		{
			_logging.LogError(value);
		}

		public PrefabObjects GetPrefabObjects<T>(ushort spawnableCollectionId, bool createIfMissing) where T : PrefabObjects
		{
			if (spawnableCollectionId == 0)
			{
				if (createIfMissing)
				{
					LogError("SpawnableCollectionId cannot be 0 when create missing is true.");
					return null;
				}
				return SpawnablePrefabs;
			}
			if (!_runtimeSpawnablePrefabs.TryGetValue(spawnableCollectionId, out var value))
			{
				if (!createIfMissing)
				{
					return null;
				}
				value = ScriptableObject.CreateInstance<T>();
				value.SetCollectionId(spawnableCollectionId);
				_runtimeSpawnablePrefabs[spawnableCollectionId] = value;
			}
			return value;
		}

		public bool RemoveSpawnableCollection(ushort spawnableCollectionId)
		{
			return _runtimeSpawnablePrefabs.Remove(spawnableCollectionId);
		}

		public int GetPrefabIndex(GameObject prefab, bool asServer)
		{
			int objectCount = SpawnablePrefabs.GetObjectCount();
			for (int i = 0; i < objectCount; i++)
			{
				if (SpawnablePrefabs.GetObject(asServer, i).gameObject == prefab)
				{
					return i;
				}
			}
			return -1;
		}

		public NetworkObject GetPrefab(int prefabId, bool asServer)
		{
			return SpawnablePrefabs.GetObject(asServer, prefabId);
		}

		public void RegisterInvokeOnInstance<T>(Action<UnityEngine.Component> handler) where T : UnityEngine.Component
		{
			if (!TryGetInstance<T>(out var result))
			{
				string instanceName = GetInstanceName<T>();
				if (!_pendingInvokes.TryGetValue(instanceName, out var value))
				{
					value = new List<Action<UnityEngine.Component>>();
					_pendingInvokes[instanceName] = value;
				}
				value.Add(handler);
			}
			else
			{
				handler(result);
			}
		}

		public void UnregisterInvokeOnInstance<T>(Action<UnityEngine.Component> handler) where T : UnityEngine.Component
		{
			string instanceName = GetInstanceName<T>();
			if (_pendingInvokes.TryGetValue(instanceName, out var value))
			{
				value.Remove(handler);
			}
		}

		public bool HasInstance<T>() where T : UnityEngine.Component
		{
			T result;
			return TryGetInstance<T>(out result);
		}

		public T GetInstance<T>() where T : UnityEngine.Component
		{
			if (TryGetInstance<T>(out var result))
			{
				return result;
			}
			LogWarning("Component " + GetInstanceName<T>() + " is not registered. To avoid this warning use TryGetInstance(T).");
			return null;
		}

		[Obsolete("Use GetInstance() or TryGetInstance(T).")]
		public T GetInstance<T>(bool warn = true) where T : UnityEngine.Component
		{
			if (!TryGetInstance<T>(out var result) && warn)
			{
				LogWarning("Component " + GetInstanceName<T>() + " is not registered.");
			}
			return result;
		}

		public bool TryGetInstance<T>(out T result) where T : UnityEngine.Component
		{
			string instanceName = GetInstanceName<T>();
			if (_registeredComponents.TryGetValue(instanceName, out var value))
			{
				result = (T)value;
				return true;
			}
			result = null;
			return false;
		}

		public void RegisterInstance<T>(T component, bool replace = true) where T : UnityEngine.Component
		{
			string instanceName = GetInstanceName<T>();
			if (_registeredComponents.ContainsKey(instanceName) && !replace)
			{
				LogWarning("Component " + instanceName + " is already registered.");
				return;
			}
			_registeredComponents[instanceName] = component;
			RemoveNullPendingDelegates();
			if (_pendingInvokes.TryGetValue(instanceName, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					value[i](component);
				}
				value.Clear();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryRegisterInstance<T>(T component) where T : UnityEngine.Component
		{
			string instanceName = GetInstanceName<T>();
			if (_registeredComponents.ContainsKey(instanceName))
			{
				return false;
			}
			RegisterInstance(component, replace: false);
			return true;
		}

		public void UnregisterInstance<T>() where T : UnityEngine.Component
		{
			string instanceName = GetInstanceName<T>();
			_registeredComponents.Remove(instanceName);
		}

		private void RemoveNullPendingDelegates()
		{
			foreach (List<Action<UnityEngine.Component>> value in _pendingInvokes.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					if (value[i] == null)
					{
						value.RemoveAt(i);
						i--;
					}
				}
			}
		}

		private string GetInstanceName<T>()
		{
			return typeof(T).FullName;
		}
	}
}
