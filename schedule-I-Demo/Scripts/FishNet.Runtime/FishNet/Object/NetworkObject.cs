using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Broadcast;
using FishNet.Component.ColliderRollback;
using FishNet.Component.Observing;
using FishNet.Component.Ownership;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Observing;
using FishNet.Managing.Predicting;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using FishNet.Managing.Timing;
using FishNet.Managing.Transporting;
using FishNet.Observing;
using FishNet.Serializing;
using FishNet.Transporting;
using FishNet.Utility.Extension;
using FishNet.Utility.Performance;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Object
{
	[DisallowMultipleComponent]
	public class NetworkObject : MonoBehaviour
	{
		public delegate void HostVisibilityUpdatedDelegate(bool prevVisible, bool nextVisible);

		[NonSerialized]
		internal bool ActiveDuringEdit;

		[SerializeField]
		[HideInInspector]
		private NetworkBehaviour[] _networkBehaviours;

		[SerializeField]
		[HideInInspector]
		internal TransformProperties SerializedTransformProperties;

		[NonSerialized]
		internal NetworkObjectState State;

		[Tooltip("True if the object will always initialize as a networked object. When false the object will not automatically initialize over the network. Using Spawn() on an object will always set that instance as networked.")]
		[SerializeField]
		private bool _isNetworked = true;

		[Tooltip("True to make this object global, and added to the DontDestroyOnLoad scene. This value may only be set for instantiated objects, and can be changed if done immediately after instantiating.")]
		[SerializeField]
		private bool _isGlobal;

		[Tooltip("Order to initialize this object's callbacks when spawned with other NetworkObjects in the same tick. Default value is 0, negative values will execute callbacks first.")]
		[SerializeField]
		private sbyte _initializeOrder;

		[SerializeField]
		[Tooltip("How to handle this object when it despawns. Scene objects are never destroyed when despawning.")]
		private DespawnType _defaultDespawnType;

		private bool _disabledNetworkBehavioursInitialized;

		public const int UNSET_OBJECTID_VALUE = 65535;

		public const int UNSET_PREFABID_VALUE = 65535;

		[HideInInspector]
		public NetworkObserver NetworkObserver;

		[HideInInspector]
		public HashSet<NetworkConnection> Observers = new HashSet<NetworkConnection>();

		internal GridEntry HashGridEntry;

		private bool _networkObserverInitiliazed;

		[NonSerialized]
		private Renderer[] _renderers;

		private bool _renderersPopulated;

		private bool _lastClientHostVisibility;

		private HashGrid _hashGrid;

		private float _nextHashGridUpdateTime;

		private bool _isStatic;

		private Vector2Int _hashGridPosition = HashGrid.UnsetGridPosition;

		private NetworkConnection _owner;

		[SerializeField]
		[HideInInspector]
		private uint _scenePathHash;

		private List<ushort> _rpcLinkIndexes;

		[field: SerializeField]
		[field: HideInInspector]
		public bool IsNested { get; private set; }

		public NetworkConnection PredictedSpawner { get; private set; } = NetworkManager.EmptyConnection;

		public bool IsSceneObject => SceneId != 0;

		[field: SerializeField]
		[field: HideInInspector]
		public byte ComponentIndex { get; private set; }

		public int ObjectId { get; private set; }

		internal bool IsDeinitializing { get; private set; } = true;

		[field: SerializeField]
		[field: HideInInspector]
		public PredictedSpawn PredictedSpawn { get; private set; }

		public NetworkBehaviour[] NetworkBehaviours
		{
			get
			{
				return _networkBehaviours;
			}
			private set
			{
				_networkBehaviours = value;
			}
		}

		[field: SerializeField]
		[field: HideInInspector]
		public NetworkObject ParentNetworkObject { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public List<NetworkObject> ChildNetworkObjects { get; private set; } = new List<NetworkObject>();

		[HideInInspector]
		public NetworkObject RuntimeParentNetworkObject { get; private set; }

		[HideInInspector]
		internal NetworkObject CurrentParentNetworkObject
		{
			get
			{
				if (RuntimeParentNetworkObject != null)
				{
					return RuntimeParentNetworkObject;
				}
				if (ParentNetworkObject != null)
				{
					return ParentNetworkObject;
				}
				return null;
			}
		}

		public Transform RuntimeParentTransform { get; private set; }

		[HideInInspector]
		public List<NetworkObject> RuntimeChildNetworkObjects { get; private set; }

		public bool IsNetworked
		{
			get
			{
				return _isNetworked;
			}
			private set
			{
				_isNetworked = value;
			}
		}

		public bool IsGlobal
		{
			get
			{
				return _isGlobal;
			}
			private set
			{
				_isGlobal = value;
			}
		}

		internal bool AllowPredictedSpawning
		{
			get
			{
				if (!(PredictedSpawn == null))
				{
					return PredictedSpawn.GetAllowSpawning();
				}
				return false;
			}
		}

		internal bool AllowPredictedDespawning
		{
			get
			{
				if (!(PredictedSpawn == null))
				{
					return PredictedSpawn.GetAllowDespawning();
				}
				return false;
			}
		}

		internal bool AllowPredictedSyncTypes
		{
			get
			{
				if (!(PredictedSpawn == null))
				{
					return PredictedSpawn.GetAllowSyncTypes();
				}
				return false;
			}
		}

		public bool IsClientInitialized { get; private set; }

		[Obsolete("Use IsClientInitialized.")]
		public bool ClientInitialized => IsClientInitialized;

		public bool IsClient
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.IsClient;
				}
				return false;
			}
		}

		public bool IsClientOnly
		{
			get
			{
				if (IsClient)
				{
					return !IsServer;
				}
				return false;
			}
		}

		public bool IsServerInitialized { get; private set; }

		public bool IsServer
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.IsServer;
				}
				return false;
			}
		}

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

		public bool IsHost
		{
			get
			{
				if (IsClient)
				{
					return IsServer;
				}
				return false;
			}
		}

		public bool IsOffline
		{
			get
			{
				if (!IsClient)
				{
					return !IsServer;
				}
				return false;
			}
		}

		public bool IsOwner
		{
			get
			{
				if (!IsClientInitialized)
				{
					return false;
				}
				return Owner.IsLocalClient;
			}
		}

		public NetworkConnection Owner
		{
			get
			{
				if (_owner == null)
				{
					return NetworkManager.EmptyConnection;
				}
				return _owner;
			}
			private set
			{
				_owner = value;
			}
		}

		public int OwnerId
		{
			get
			{
				if (Owner.IsValid)
				{
					return Owner.ClientId;
				}
				return -1;
			}
		}

		public bool IsSpawned
		{
			get
			{
				if (!IsDeinitializing)
				{
					return ObjectId != 65535;
				}
				return false;
			}
		}

		public NetworkConnection LocalConnection
		{
			get
			{
				if (!(NetworkManager == null))
				{
					return NetworkManager.ClientManager.Connection;
				}
				return new NetworkConnection();
			}
		}

		public NetworkManager NetworkManager { get; private set; }

		public ServerManager ServerManager { get; private set; }

		public ClientManager ClientManager { get; private set; }

		public ObserverManager ObserverManager { get; private set; }

		public TransportManager TransportManager { get; private set; }

		public TimeManager TimeManager { get; private set; }

		public FishNet.Managing.Scened.SceneManager SceneManager { get; private set; }

		public PredictionManager PredictionManager { get; private set; }

		public RollbackManager RollbackManager { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public ushort PrefabId { get; internal set; }

		[field: SerializeField]
		[field: HideInInspector]
		public ushort SpawnableCollectionId { get; internal set; }

		[field: SerializeField]
		[field: HideInInspector]
		internal ulong SceneId { get; private set; }

		[field: SerializeField]
		[field: HideInInspector]
		public ulong AssetPathHash { get; private set; }

		public event HostVisibilityUpdatedDelegate OnHostVisibilityUpdated;

		public event Action<NetworkObject> OnObserversActive;

		public void Broadcast<T>(T message, bool requireAuthenticated = true, Channel channel = Channel.Reliable) where T : struct, IBroadcast
		{
			if (NetworkManager == null)
			{
				NetworkManager.StaticLogWarning("Cannot send broadcast from " + base.gameObject.name + ", NetworkManager reference is null. This may occur if the object is not spawned or initialized.");
			}
			else
			{
				NetworkManager.ServerManager.Broadcast(Observers, message, requireAuthenticated, channel);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void InitializeCallbacks(bool asServer, bool invokeSyncTypeCallbacks)
		{
			for (int i = 0; i < NetworkBehaviours.Length; i++)
			{
				NetworkBehaviours[i].InvokeOnNetwork(start: true);
			}
			if (asServer)
			{
				for (int j = 0; j < NetworkBehaviours.Length; j++)
				{
					NetworkBehaviours[j].OnStartServer_Internal();
				}
				for (int k = 0; k < NetworkBehaviours.Length; k++)
				{
					NetworkBehaviours[k].OnOwnershipServer_Internal(NetworkManager.EmptyConnection);
				}
			}
			else
			{
				for (int l = 0; l < NetworkBehaviours.Length; l++)
				{
					NetworkBehaviours[l].OnStartClient_Internal();
				}
				for (int m = 0; m < NetworkBehaviours.Length; m++)
				{
					NetworkBehaviours[m].OnOwnershipClient_Internal(NetworkManager.EmptyConnection);
				}
			}
			if (invokeSyncTypeCallbacks)
			{
				InvokeOnStartSyncTypeCallbacks(asServer: true);
			}
		}

		internal void InvokeOnStartSyncTypeCallbacks(bool asServer)
		{
			for (int i = 0; i < NetworkBehaviours.Length; i++)
			{
				NetworkBehaviours[i].InvokeSyncTypeOnStartCallbacks(asServer);
			}
		}

		internal void InvokeOnStopSyncTypeCallbacks(bool asServer)
		{
			for (int i = 0; i < NetworkBehaviours.Length; i++)
			{
				NetworkBehaviours[i].InvokeSyncTypeOnStopCallbacks(asServer);
			}
		}

		internal void OnSpawnServer(NetworkConnection conn)
		{
			for (int i = 0; i < NetworkBehaviours.Length; i++)
			{
				NetworkBehaviours[i].SendBufferedRpcs(conn);
			}
			for (int j = 0; j < NetworkBehaviours.Length; j++)
			{
				NetworkBehaviours[j].OnSpawnServer(conn);
			}
		}

		internal void InvokeOnServerDespawn(NetworkConnection conn)
		{
			for (int i = 0; i < NetworkBehaviours.Length; i++)
			{
				NetworkBehaviours[i].OnDespawnServer(conn);
			}
		}

		internal void InvokeStopCallbacks(bool asServer)
		{
			for (int i = 0; i < NetworkBehaviours.Length; i++)
			{
				NetworkBehaviours[i].InvokeSyncTypeOnStopCallbacks(asServer);
			}
			if (asServer)
			{
				for (int j = 0; j < NetworkBehaviours.Length; j++)
				{
					NetworkBehaviours[j].OnStopServer_Internal();
				}
			}
			else
			{
				for (int k = 0; k < NetworkBehaviours.Length; k++)
				{
					NetworkBehaviours[k].OnStopClient_Internal();
				}
			}
			if (asServer || (!asServer && !IsServer))
			{
				for (int l = 0; l < NetworkBehaviours.Length; l++)
				{
					NetworkBehaviours[l].InvokeOnNetwork(start: false);
				}
			}
		}

		private void InvokeOwnership(NetworkConnection prevOwner, bool asServer)
		{
			if (asServer)
			{
				for (int i = 0; i < NetworkBehaviours.Length; i++)
				{
					NetworkBehaviours[i].OnOwnershipServer_Internal(prevOwner);
				}
			}
			else if (!IsOwner || IsServer || !(prevOwner == Owner))
			{
				for (int j = 0; j < NetworkBehaviours.Length; j++)
				{
					NetworkBehaviours[j].OnOwnershipClient_Internal(prevOwner);
				}
			}
		}

		public void SetIsNetworked(bool value)
		{
			IsNetworked = value;
		}

		public void SetIsGlobal(bool value)
		{
			if (IsNested && !CurrentParentNetworkObject.IsGlobal)
			{
				NetworkManager.StaticLogWarning("Object " + base.gameObject.name + " cannot change IsGlobal because it is nested and the parent NetorkObject is not global.");
				return;
			}
			if (!IsDeinitializing)
			{
				NetworkManager.StaticLogWarning("Object " + base.gameObject.name + " cannot change IsGlobal as it's already initialized. IsGlobal may only be changed immediately after instantiating.");
				return;
			}
			if (IsSceneObject)
			{
				NetworkManager.StaticLogWarning("Object " + base.gameObject.name + " cannot have be global because it is a scene object. Only instantiated objects may be global.");
				return;
			}
			_networkObserverInitiliazed = false;
			IsGlobal = value;
		}

		public sbyte GetInitializeOrder()
		{
			return _initializeOrder;
		}

		public DespawnType GetDefaultDespawnType()
		{
			return _defaultDespawnType;
		}

		public void SetDefaultDespawnType(DespawnType despawnType)
		{
			_defaultDespawnType = despawnType;
		}

		public override string ToString()
		{
			return $"Name [{base.gameObject.name}] Id [{ObjectId}]";
		}

		protected virtual void Awake()
		{
			_isStatic = base.gameObject.isStatic;
			RuntimeChildNetworkObjects = CollectionCaches<NetworkObject>.RetrieveList();
			SetChildDespawnedState();
		}

		protected virtual void Start()
		{
			TryStartDeactivation();
		}

		private void OnDisable()
		{
			if (IsDeinitializing && Owner.IsValid)
			{
				Owner.RemoveObject(this);
			}
			else
			{
				if (!IsServer || IsNested || !base.gameObject.activeSelf)
				{
					return;
				}
				bool flag = false;
				Transform parent = base.transform.parent;
				while (parent != null)
				{
					if (parent.TryGetComponent<NetworkObject>(out var component))
					{
						if (component != ParentNetworkObject)
						{
							break;
						}
						if (component.IsDeinitializing)
						{
							flag = true;
							break;
						}
					}
					parent = parent.parent;
				}
				if (flag)
				{
					Despawn();
				}
			}
		}

		private void OnDestroy()
		{
			if (IsDeinitializing)
			{
				return;
			}
			Owner?.RemoveObject(this);
			NetworkObserver?.Deinitialize(destroyed: true);
			if (NetworkManager != null)
			{
				if (NetworkManager.IsServer)
				{
					NetworkManager.ServerManager.Objects.NetworkObjectUnexpectedlyDestroyed(this, asServer: true);
				}
				if (NetworkManager.IsClient)
				{
					NetworkManager.ClientManager.Objects.NetworkObjectUnexpectedlyDestroyed(this, asServer: false);
				}
			}
			if (IsServer)
			{
				InvokeStopCallbacks(asServer: true);
			}
			if (IsClient)
			{
				InvokeStopCallbacks(asServer: false);
			}
			if (Owner.IsValid)
			{
				Owner.RemoveObject(this);
			}
			Observers.Clear();
			RuntimeParentNetworkObject?.RuntimeChildNetworkObjects.Remove(this);
			CollectionCaches<NetworkObject>.Store(RuntimeChildNetworkObjects);
			IsDeinitializing = true;
			SetDeinitializedStatus();
		}

		private void InitializeNetworkBehavioursIfDisabled()
		{
			if (!_disabledNetworkBehavioursInitialized)
			{
				_disabledNetworkBehavioursInitialized = true;
				for (int i = 0; i < NetworkBehaviours.Length; i++)
				{
					NetworkBehaviours[i].InitializeIfDisabled();
				}
			}
		}

		private void SetChildGlobalState()
		{
			if (IsGlobal)
			{
				for (int i = 0; i < ChildNetworkObjects.Count; i++)
				{
					ChildNetworkObjects[i].SetIsGlobal(value: true);
				}
			}
		}

		private void SetChildDespawnedState()
		{
			for (int i = 0; i < ChildNetworkObjects.Count; i++)
			{
				NetworkObject networkObject = ChildNetworkObjects[i];
				if (!networkObject.gameObject.activeSelf)
				{
					networkObject.State = NetworkObjectState.Despawned;
				}
			}
		}

		internal void TryStartDeactivation()
		{
			if (!IsNetworked)
			{
				return;
			}
			if (IsGlobal && !IsSceneObject && !IsNested)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			if (NetworkManager == null || (!NetworkManager.IsClient && !NetworkManager.IsServer))
			{
				if (IsSceneObject)
				{
					ActiveDuringEdit = true;
				}
				base.gameObject.SetActive(value: false);
			}
		}

		internal void SetInitializedStatus(bool isInitialized, bool asServer)
		{
			if (asServer)
			{
				IsServerInitialized = isInitialized;
			}
			else
			{
				IsClientInitialized = isInitialized;
			}
		}

		private void SetDeinitializedStatus()
		{
			IsServerInitialized = false;
			IsClientInitialized = false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Preinitialize_Internal(NetworkManager networkManager, int objectId, NetworkConnection owner, bool asServer)
		{
			if (!networkManager.DoubleLogic(asServer))
			{
				State = NetworkObjectState.Spawned;
				InitializeNetworkBehavioursIfDisabled();
				IsDeinitializing = false;
				NetworkManager = networkManager;
				ServerManager = networkManager.ServerManager;
				ClientManager = networkManager.ClientManager;
				ObserverManager = networkManager.ObserverManager;
				TransportManager = networkManager.TransportManager;
				TimeManager = networkManager.TimeManager;
				SceneManager = networkManager.SceneManager;
				PredictionManager = networkManager.PredictionManager;
				RollbackManager = networkManager.RollbackManager;
				SetOwner(owner);
				ObjectId = objectId;
				AddDefaultNetworkObserverConditions();
			}
			if (!asServer && !IsServer && !IsOwner)
			{
				_ = TimeManager.Tick - TimeManager.LastPacketTick;
				_ = 0;
			}
			for (int i = 0; i < NetworkBehaviours.Length; i++)
			{
				NetworkBehaviours[i].Preinitialize_Internal(this, asServer);
			}
			if (asServer)
			{
				if (networkManager.TryGetInstance<HashGrid>(out _hashGrid))
				{
					_hashGridPosition = _hashGrid.GetHashGridPosition(this);
					HashGridEntry = _hashGrid.GetGridEntry(this);
				}
				NetworkObserver.Initialize(this);
			}
			_networkObserverInitiliazed = true;
			owner?.AddObject(this);
		}

		public void SetParent(NetworkBehaviour nb)
		{
			if (!InvalidParent(nb.NetworkObject))
			{
				UpdateParent(nb.NetworkObject, nb);
			}
		}

		public void SetParent(NetworkObject nob)
		{
			if (!InvalidParent(nob))
			{
				UpdateParent(nob, null);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UnsetParent()
		{
			UpdateParent(null, null);
		}

		private void UpdateParent(NetworkObject nob, NetworkBehaviour nb)
		{
			RuntimeParentNetworkObject?.RuntimeChildNetworkObjects.Remove(this);
			if (nob == null && nb == null)
			{
				RuntimeParentNetworkObject = null;
				RuntimeParentTransform = null;
				base.transform.SetParent(null);
			}
			else
			{
				Transform transform = ((nb != null) ? nb.transform : nob.transform);
				RuntimeParentNetworkObject = nob;
				RuntimeParentTransform = transform;
				nob.RuntimeChildNetworkObjects.Add(this);
				base.transform.SetParent(transform);
			}
			NetworkManager?.ServerManager.Objects.RebuildObservers(this);
		}

		private bool InvalidParent(NetworkObject nob)
		{
			if (IsSceneObject)
			{
				return false;
			}
			if (nob == RuntimeParentNetworkObject)
			{
				return true;
			}
			if (nob.IsGlobal && !IsGlobal)
			{
				NetworkManager.LogWarning(nob.name + " is a global NetworkObject but " + base.gameObject.name + " is not. Only global NetworkObjects can be set as a child of another global NetworkObject.");
				return true;
			}
			if (nob == this)
			{
				NetworkManager.LogWarning(base.gameObject.name + " cannot be set as a child of itself.");
				return true;
			}
			if (ParentNetworkObject != null && ParentNetworkObject != nob)
			{
				NetworkManager.LogWarning(base.gameObject.name + " cannot have the parent changed because it is a nested NetworkObject.");
				return true;
			}
			return false;
		}

		internal T AddAndSerialize<T>() where T : NetworkBehaviour
		{
			int num = NetworkBehaviours.Length;
			T val = base.gameObject.AddComponent<T>();
			Array.Resize(ref _networkBehaviours, num + 1);
			_networkBehaviours[num] = val;
			val.SerializeComponents(this, (byte)num);
			return val;
		}

		internal void UpdateNetworkBehaviours(NetworkObject parentNob, ref byte componentIndex)
		{
			if (componentIndex == 0)
			{
				if (IsNested)
				{
					return;
				}
				byte b = byte.MaxValue;
				if (GetComponentsInChildren<NetworkObject>(includeInactive: true).Length > b)
				{
					Debug.LogError($"The number of child NetworkObjects on {base.gameObject.name} exceeds the maximum of {b}.");
					return;
				}
			}
			PredictedSpawn = GetComponent<PredictedSpawn>();
			ComponentIndex = componentIndex;
			ParentNetworkObject = parentNob;
			List<Transform> list = CollectionCaches<Transform>.RetrieveList();
			ChildNetworkObjects.Clear();
			list.Add(base.transform);
			for (int i = 0; i < list.Count; i++)
			{
				Transform transform = list[i];
				for (int j = 0; j < transform.childCount; j++)
				{
					Transform child = transform.GetChild(j);
					if (child.TryGetComponent<NetworkObject>(out var component))
					{
						if (IsSceneObject == component.IsSceneObject)
						{
							ChildNetworkObjects.Add(component);
						}
					}
					else
					{
						list.Add(child);
					}
				}
			}
			List<NetworkBehaviour> list2 = CollectionCaches<NetworkBehaviour>.RetrieveList();
			List<NetworkBehaviour> results = CollectionCaches<NetworkBehaviour>.RetrieveList();
			for (int k = 0; k < list.Count; k++)
			{
				results.Clear();
				list[k].GetNetworkBehavioursNonAlloc(ref results);
				list2.AddRange(results);
			}
			int count = list2.Count;
			NetworkBehaviours = new NetworkBehaviour[count];
			for (int l = 0; l < count; l++)
			{
				NetworkBehaviours[l] = list2[l];
				NetworkBehaviours[l].SerializeComponents(this, (byte)l);
			}
			CollectionCaches<Transform>.Store(list);
			CollectionCaches<NetworkBehaviour>.Store(list2);
			CollectionCaches<NetworkBehaviour>.Store(results);
			foreach (NetworkObject childNetworkObject in ChildNetworkObjects)
			{
				componentIndex++;
				childNetworkObject.UpdateNetworkBehaviours(this, ref componentIndex);
			}
			SetChildGlobalState();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Initialize(bool asServer, bool invokeSyncTypeCallbacks)
		{
			SetInitializedStatus(isInitialized: true, asServer);
			InitializeCallbacks(asServer, invokeSyncTypeCallbacks);
		}

		internal void Deinitialize(bool asServer)
		{
			InvokeStopCallbacks(asServer);
			for (int i = 0; i < NetworkBehaviours.Length; i++)
			{
				NetworkBehaviours[i].Deinitialize(asServer);
			}
			if (asServer)
			{
				NetworkObserver?.Deinitialize(destroyed: false);
				IsDeinitializing = true;
			}
			else
			{
				if (ClientManager.Connection.LevelOfDetails.TryGetValue(this, out var value))
				{
					ObjectCaches<NetworkConnection.LevelOfDetailData>.Store(value);
				}
				ClientManager.Connection.LevelOfDetails.Remove(this);
				if (!NetworkManager.IsServer)
				{
					IsDeinitializing = true;
				}
				RemoveClientRpcLinkIndexes();
			}
			SetInitializedStatus(isInitialized: false, asServer);
			if (asServer)
			{
				Observers.Clear();
			}
		}

		[Obsolete("This is no longer used. Remove any calls to this method.")]
		public void ResetForObjectPool()
		{
		}

		public void ResetState()
		{
			if (!IsDeinitializing)
			{
				string text = "NetworkObject " + ToString() + " is being reset prior to calling deinitialize. To prevent future errors this object will be destroyed.";
				if (NetworkManager == null)
				{
					Debug.LogError(text);
				}
				else
				{
					NetworkManager.LogError(text);
				}
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			int num = NetworkBehaviours.Length;
			for (int i = 0; i < num; i++)
			{
				NetworkBehaviours[i].ResetState();
			}
			State = NetworkObjectState.Unset;
			SetOwner(NetworkManager.EmptyConnection);
			NetworkObserver?.Deinitialize(destroyed: false);
			NetworkManager = null;
			ServerManager = null;
			ClientManager = null;
			ObserverManager = null;
			TransportManager = null;
			TimeManager = null;
			SceneManager = null;
			RollbackManager = null;
			ObjectId = 0;
		}

		public void RemoveOwnership()
		{
			GiveOwnership(null, asServer: true);
		}

		public void GiveOwnership(NetworkConnection newOwner)
		{
			GiveOwnership(newOwner, asServer: true);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void GiveOwnership(NetworkConnection newOwner, bool asServer)
		{
			if (asServer)
			{
				if (!NetworkManager.IsServer)
				{
					NetworkManager.LogWarning("Ownership cannot be given for object " + base.gameObject.name + ". Only server may give ownership.");
					return;
				}
				if (newOwner == Owner && asServer)
				{
					return;
				}
				if (newOwner != null && newOwner.IsActive && !newOwner.LoadedStartScenes(asServer: true))
				{
					NetworkManager.LogWarning($"Ownership has been transfered to ConnectionId {newOwner.ClientId} but this is not recommended until after they have loaded start scenes. You can be notified when a connection loads start scenes by using connection.OnLoadedStartScenes on the connection, or SceneManager.OnClientLoadStartScenes.");
				}
			}
			bool flag = newOwner != null && newOwner.IsActive;
			NetworkConnection networkConnection = Owner;
			if (networkConnection == null)
			{
				networkConnection = NetworkManager.EmptyConnection;
			}
			SetOwner(newOwner);
			if (asServer || !NetworkManager.IsHost)
			{
				if (flag)
				{
					newOwner.AddObject(this);
				}
				if (networkConnection != newOwner)
				{
					networkConnection.RemoveObject(this);
				}
			}
			InvokeOwnership(networkConnection, asServer);
			if (!asServer)
			{
				return;
			}
			if (flag)
			{
				ServerManager.Objects.RebuildObservers(this, newOwner);
			}
			PooledWriter pooledWriter = WriterPool.Retrieve();
			pooledWriter.WritePacketId(PacketId.OwnershipChange);
			pooledWriter.WriteNetworkObject(this);
			pooledWriter.WriteNetworkConnection(Owner);
			if (NetworkManager.ServerManager.ShareIds)
			{
				NetworkManager.TransportManager.SendToClients(0, pooledWriter.GetArraySegment(), this);
			}
			else
			{
				if (networkConnection.IsActive)
				{
					NetworkManager.TransportManager.SendToClient(0, pooledWriter.GetArraySegment(), networkConnection);
				}
				if (flag)
				{
					NetworkManager.TransportManager.SendToClient(0, pooledWriter.GetArraySegment(), newOwner);
				}
			}
			pooledWriter.Store();
			if (networkConnection.IsActive)
			{
				ServerManager.Objects.RebuildObservers(networkConnection);
			}
		}

		internal void InitializePredictedObject_Server(NetworkManager manager, NetworkConnection predictedSpawner)
		{
			NetworkManager = manager;
			PredictedSpawner = predictedSpawner;
		}

		internal void PreinitializePredictedObject_Client(NetworkManager manager, int objectId, NetworkConnection owner, NetworkConnection predictedSpawner)
		{
			PredictedSpawner = predictedSpawner;
			Preinitialize_Internal(manager, objectId, owner, asServer: false);
		}

		internal void DeinitializePredictedObject_Client()
		{
			base.gameObject.SetActive(value: false);
		}

		private void SetOwner(NetworkConnection owner)
		{
			Owner = owner;
		}

		internal ChangedTransformProperties GetTransformChanges(TransformProperties stp)
		{
			ChangedTransformProperties changedTransformProperties = ChangedTransformProperties.Unset;
			if (base.transform.localPosition != stp.Position)
			{
				changedTransformProperties |= ChangedTransformProperties.LocalPosition;
			}
			if (base.transform.localRotation != stp.Rotation)
			{
				changedTransformProperties |= ChangedTransformProperties.LocalRotation;
			}
			if (base.transform.localScale != stp.LocalScale)
			{
				changedTransformProperties |= ChangedTransformProperties.LocalScale;
			}
			return changedTransformProperties;
		}

		internal ChangedTransformProperties GetTransformChanges(GameObject prefab)
		{
			Transform transform = prefab.transform;
			ChangedTransformProperties changedTransformProperties = ChangedTransformProperties.Unset;
			if (base.transform.position != transform.position)
			{
				changedTransformProperties |= ChangedTransformProperties.LocalPosition;
			}
			if (base.transform.rotation != transform.rotation)
			{
				changedTransformProperties |= ChangedTransformProperties.LocalRotation;
			}
			if (base.transform.localScale != transform.localScale)
			{
				changedTransformProperties |= ChangedTransformProperties.LocalScale;
			}
			return changedTransformProperties;
		}

		internal void UpdateForNetworkObject(bool force)
		{
			if (_hashGrid == null || _isStatic)
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			if (force || !(unscaledTime < _nextHashGridUpdateTime))
			{
				_nextHashGridUpdateTime = unscaledTime + 1f;
				Vector2Int hashGridPosition = _hashGrid.GetHashGridPosition(this);
				if (hashGridPosition != _hashGridPosition)
				{
					_hashGridPosition = hashGridPosition;
					HashGridEntry = _hashGrid.GetGridEntry(hashGridPosition);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void UpdateRenderers(bool updateVisibility = true)
		{
			UpdateRenderers_Internal(updateVisibility);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetRenderersVisible(bool visible, bool force = false)
		{
			if (force || NetworkObserver.UpdateHostVisibility)
			{
				if (!_renderersPopulated)
				{
					UpdateRenderers_Internal(updateVisibility: false);
					_renderersPopulated = true;
				}
				UpdateRenderVisibility(visible);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void UpdateRenderers_Internal(bool updateVisibility)
		{
			_renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
			List<Renderer> list = new List<Renderer>();
			Renderer[] renderers = _renderers;
			foreach (Renderer renderer in renderers)
			{
				if (renderer.enabled)
				{
					list.Add(renderer);
				}
			}
			if (list.Count != _renderers.Length)
			{
				_renderers = list.ToArray();
			}
			if (updateVisibility)
			{
				UpdateRenderVisibility(_lastClientHostVisibility);
			}
		}

		private void UpdateRenderVisibility(bool visible)
		{
			bool flag = false;
			Renderer[] renderers = _renderers;
			int num = renderers.Length;
			for (int i = 0; i < num; i++)
			{
				Renderer renderer = renderers[i];
				if (renderer == null)
				{
					flag = true;
					break;
				}
				renderer.enabled = visible;
			}
			this.OnHostVisibilityUpdated?.Invoke(_lastClientHostVisibility, visible);
			_lastClientHostVisibility = visible;
			if (flag)
			{
				UpdateRenderers();
			}
		}

		private void AddDefaultNetworkObserverConditions()
		{
			if (!_networkObserverInitiliazed)
			{
				NetworkObserver = NetworkManager.ObserverManager.AddDefaultConditions(this);
			}
		}

		internal bool RemoveObserver(NetworkConnection connection)
		{
			int count = Observers.Count;
			bool num = Observers.Remove(connection);
			if (num)
			{
				TryInvokeOnObserversActive(count);
			}
			return num;
		}

		internal ObserverStateChange RebuildObservers(NetworkConnection connection, bool timedOnly)
		{
			if (!connection.IsValid)
			{
				NetworkManager.LogWarning("An invalid connection was used when rebuilding observers.");
				return ObserverStateChange.Unchanged;
			}
			if (!connection.IsActive)
			{
				Observers.Remove(connection);
				return ObserverStateChange.Unchanged;
			}
			if (IsDeinitializing)
			{
				return ObserverStateChange.Unchanged;
			}
			UpdateForNetworkObject(!timedOnly);
			int count = Observers.Count;
			ObserverStateChange observerStateChange = NetworkObserver.RebuildObservers(connection, timedOnly);
			switch (observerStateChange)
			{
			case ObserverStateChange.Added:
				Observers.Add(connection);
				break;
			case ObserverStateChange.Removed:
				Observers.Remove(connection);
				break;
			}
			if (observerStateChange != ObserverStateChange.Unchanged)
			{
				TryInvokeOnObserversActive(count);
			}
			return observerStateChange;
		}

		private void TryInvokeOnObserversActive(int startCount)
		{
			if ((Observers.Count > 0 && startCount == 0) || (Observers.Count == 0 && startCount > 0))
			{
				this.OnObserversActive?.Invoke(this);
			}
		}

		public NetworkBehaviour GetNetworkBehaviour(byte componentIndex, bool error)
		{
			if (componentIndex >= NetworkBehaviours.Length && error)
			{
				string value = $"ComponentIndex of {componentIndex} is out of bounds on {base.gameObject.name} [id {ObjectId}]. This may occur if you have modified your gameObject/prefab without saving it, or the scene.";
				if (NetworkManager == null)
				{
					NetworkManager.StaticLogError(value);
				}
				else
				{
					NetworkManager.LogError(value);
				}
			}
			return NetworkBehaviours[componentIndex];
		}

		public void Despawn(GameObject go, DespawnType? despawnType = null)
		{
			NetworkManager?.ServerManager.Despawn(go, despawnType);
		}

		public void Despawn(NetworkObject nob, DespawnType? despawnType = null)
		{
			NetworkManager?.ServerManager.Despawn(nob, despawnType);
		}

		public void Despawn(DespawnType? despawnType = null)
		{
			NetworkManager?.ServerManager.Despawn(this, despawnType);
		}

		public void Spawn(GameObject go, NetworkConnection ownerConnection = null, Scene scene = default(Scene))
		{
			NetworkManager?.ServerManager.Spawn(go, ownerConnection, scene);
		}

		public void Spawn(NetworkObject nob, NetworkConnection ownerConnection = null, Scene scene = default(Scene))
		{
			NetworkManager?.ServerManager.Spawn(nob, ownerConnection, scene);
		}

		public void SetLocalOwnership(NetworkConnection caller)
		{
			NetworkConnection owner = Owner;
			SetOwner(caller);
			int num = NetworkBehaviours.Length;
			for (int i = 0; i < num; i++)
			{
				NetworkBehaviours[i].OnOwnershipClient_Internal(owner);
			}
			num = ChildNetworkObjects.Count;
			for (int j = 0; j < num; j++)
			{
				ChildNetworkObjects[j].SetLocalOwnership(caller);
			}
		}

		public void RegisterInvokeOnInstance<T>(Action<UnityEngine.Component> handler) where T : UnityEngine.Component
		{
			NetworkManager.RegisterInvokeOnInstance<T>(handler);
		}

		public void UnregisterInvokeOnInstance<T>(Action<UnityEngine.Component> handler) where T : UnityEngine.Component
		{
			NetworkManager.UnregisterInvokeOnInstance<T>(handler);
		}

		public bool HasInstance<T>() where T : UnityEngine.Component
		{
			return NetworkManager.HasInstance<T>();
		}

		public T GetInstance<T>() where T : UnityEngine.Component
		{
			return NetworkManager.GetInstance<T>();
		}

		public void RegisterInstance<T>(T component, bool replace = true) where T : UnityEngine.Component
		{
			NetworkManager.RegisterInstance(component, replace);
		}

		public bool TryRegisterInstance<T>(T component) where T : UnityEngine.Component
		{
			return NetworkManager.TryRegisterInstance(component);
		}

		public bool TryGetInstance<T>(out T component) where T : UnityEngine.Component
		{
			return NetworkManager.TryGetInstance<T>(out component);
		}

		public void UnregisterInstance<T>() where T : UnityEngine.Component
		{
			NetworkManager.UnregisterInstance<T>();
		}

		public void SetAssetPathHash(ulong value)
		{
			AssetPathHash = value;
		}

		internal void ClearRuntimeSceneObject()
		{
			if (!Application.isPlaying)
			{
				Debug.LogError("ClearRuntimeSceneObject may only be called at runtime.");
			}
			else
			{
				SceneId = 0uL;
			}
		}

		internal void SetRpcLinkIndexes(List<ushort> values)
		{
			_rpcLinkIndexes = values;
		}

		internal void RemoveClientRpcLinkIndexes()
		{
			NetworkManager.ClientManager.Objects.RemoveLinkIndexes(_rpcLinkIndexes);
		}

		internal void WriteDirtySyncTypes()
		{
			NetworkBehaviour[] networkBehaviours = NetworkBehaviours;
			int num = networkBehaviours.Length;
			for (int i = 0; i < num; i++)
			{
				NetworkBehaviour obj = networkBehaviours[i];
				obj.WriteDirtySyncTypes(isSyncObject: true, ignoreInterval: true);
				obj.WriteDirtySyncTypes(isSyncObject: false, ignoreInterval: true);
			}
		}
	}
}
