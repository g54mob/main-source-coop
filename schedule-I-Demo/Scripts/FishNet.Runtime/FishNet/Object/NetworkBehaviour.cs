using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Component.ColliderRollback;
using FishNet.Connection;
using FishNet.Documenting;
using FishNet.Managing;
using FishNet.Managing.Client;
using FishNet.Managing.Logging;
using FishNet.Managing.Observing;
using FishNet.Managing.Predicting;
using FishNet.Managing.Scened;
using FishNet.Managing.Server;
using FishNet.Managing.Timing;
using FishNet.Managing.Transporting;
using FishNet.Object.Delegating;
using FishNet.Object.Helping;
using FishNet.Object.Prediction;
using FishNet.Object.Prediction.Delegating;
using FishNet.Object.Synchronizing;
using FishNet.Object.Synchronizing.Internal;
using FishNet.Observing;
using FishNet.Serializing;
using FishNet.Serializing.Helping;
using FishNet.Transporting;
using FishNet.Utility.Performance;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Object
{
	public abstract class NetworkBehaviour : MonoBehaviour
	{
		private struct BufferedRpc
		{
			public PooledWriter Writer;

			public Channel Channel;

			public DataOrderType OrderType;

			public BufferedRpc(PooledWriter writer, Channel channel, DataOrderType orderType)
			{
				Writer = writer;
				Channel = channel;
				OrderType = orderType;
			}
		}

		private class SyncTypeWriter
		{
			public ReadPermission ReadPermission;

			public PooledWriter[] Writers { get; private set; }

			public SyncTypeWriter(ReadPermission readPermission)
			{
				ReadPermission = readPermission;
				Writers = new PooledWriter[2];
				for (int i = 0; i < Writers.Length; i++)
				{
					Writers[i] = WriterPool.Retrieve();
				}
			}

			public void Reset()
			{
				if (Writers != null)
				{
					for (int i = 0; i < Writers.Length; i++)
					{
						Writers[i].Reset();
					}
				}
			}
		}

		private bool _onStartNetworkCalled;

		private bool _onStopNetworkCalled;

		[SerializeField]
		[HideInInspector]
		private byte _componentIndexCache = byte.MaxValue;

		private TransportManager _transportManagerCache;

		[SerializeField]
		[HideInInspector]
		private NetworkObject _networkObjectCache;

		private bool _initializedOnceServer;

		private bool _initializedOnceClient;

		internal bool ClientHasReconcileData;

		private uint _lastReplicateTick;

		private readonly Dictionary<uint, ReplicateRpcDelegate> _replicateRpcDelegates = new Dictionary<uint, ReplicateRpcDelegate>();

		private readonly Dictionary<uint, ReconcileRpcDelegate> _reconcileRpcDelegates = new Dictionary<uint, ReconcileRpcDelegate>();

		private bool _predictionInitialized;

		private Rigidbody _predictionRigidbody;

		private Rigidbody2D _predictionRigidbody2d;

		private Vector3 _lastMayChangePosition;

		private Quaternion _lastMayChangeRotation;

		private Vector3 _lastMayChangeScale;

		private int _remainingResends;

		private uint _lastSentReplicateTick;

		private uint _lastReceivedReplicateTick;

		private uint _lastReceivedReconcileTick;

		private uint _lastReconcileTick;

		private Dictionary<uint, RpcLinkType> _rpcLinks = new Dictionary<uint, RpcLinkType>();

		private readonly Dictionary<uint, ServerRpcDelegate> _serverRpcDelegates = new Dictionary<uint, ServerRpcDelegate>();

		private readonly Dictionary<uint, ClientRpcDelegate> _observersRpcDelegates = new Dictionary<uint, ClientRpcDelegate>();

		private readonly Dictionary<uint, ClientRpcDelegate> _targetRpcDelegates = new Dictionary<uint, ClientRpcDelegate>();

		private uint _rpcMethodCount;

		private byte _rpcHashSize = 1;

		private Dictionary<uint, BufferedRpc> _bufferedRpcs = new Dictionary<uint, BufferedRpc>();

		private HashSet<NetworkConnection> _networkConnectionCache = new HashSet<NetworkConnection>();

		private const int MAXIMUM_RPC_HEADER_SIZE = 10;

		private SyncTypeWriter[] _syncTypeWriters;

		private Dictionary<uint, SyncBase> _syncVars = new Dictionary<uint, SyncBase>();

		internal bool SyncVarDirty;

		private Dictionary<uint, SyncBase> _syncObjects = new Dictionary<uint, SyncBase>();

		internal bool SyncObjectDirty;

		private static ReadPermission[] _readPermissions;

		private List<SyncVarReadDelegate> _syncVarReadDelegates = new List<SyncVarReadDelegate>();

		[APIExclude]
		public bool OnStartServerCalled { get; private set; }

		[APIExclude]
		public bool OnStartClientCalled { get; private set; }

		public bool IsSpawned => _networkObjectCache.IsSpawned;

		public byte ComponentIndex
		{
			get
			{
				return _componentIndexCache;
			}
			private set
			{
				_componentIndexCache = value;
			}
		}

		public NetworkObject NetworkObject => _networkObjectCache;

		public bool IsReconciling { get; internal set; }

		public bool IsDeinitializing => _networkObjectCache.IsDeinitializing;

		public NetworkManager NetworkManager => _networkObjectCache.NetworkManager;

		public ServerManager ServerManager => _networkObjectCache.ServerManager;

		public ClientManager ClientManager => _networkObjectCache.ClientManager;

		public ObserverManager ObserverManager => _networkObjectCache.ObserverManager;

		public TransportManager TransportManager => _networkObjectCache.TransportManager;

		public TimeManager TimeManager => _networkObjectCache.TimeManager;

		public FishNet.Managing.Scened.SceneManager SceneManager => _networkObjectCache.SceneManager;

		public PredictionManager PredictionManager => _networkObjectCache.PredictionManager;

		public RollbackManager RollbackManager => _networkObjectCache.RollbackManager;

		public NetworkObserver NetworkObserver => _networkObjectCache.NetworkObserver;

		public bool IsClientInitialized => _networkObjectCache.IsClientInitialized;

		public bool IsClient => _networkObjectCache.IsClient;

		public bool IsClientOnly => _networkObjectCache.IsClientOnly;

		public bool IsServerInitialized => _networkObjectCache.IsServerInitialized;

		public bool IsServer => _networkObjectCache.IsServer;

		public bool IsServerOnly => _networkObjectCache.IsServerOnly;

		public bool IsHost => _networkObjectCache.IsHost;

		public bool IsOffline => _networkObjectCache.IsOffline;

		public bool IsNetworked => _networkObjectCache.IsNetworked;

		public HashSet<NetworkConnection> Observers => _networkObjectCache.Observers;

		public bool IsOwner => _networkObjectCache.IsOwner;

		public NetworkConnection Owner
		{
			get
			{
				if (_networkObjectCache == null)
				{
					return NetworkManager.EmptyConnection;
				}
				return _networkObjectCache.Owner;
			}
		}

		public int OwnerId => _networkObjectCache.OwnerId;

		public int ObjectId => _networkObjectCache.ObjectId;

		public NetworkConnection LocalConnection => _networkObjectCache.LocalConnection;

		internal void InvokeSyncTypeOnStartCallbacks(bool asServer)
		{
			foreach (SyncBase value in _syncVars.Values)
			{
				value.OnStartCallback(asServer);
			}
			foreach (SyncBase value2 in _syncObjects.Values)
			{
				value2.OnStartCallback(asServer);
			}
		}

		internal void InvokeSyncTypeOnStopCallbacks(bool asServer)
		{
			foreach (SyncBase value in _syncVars.Values)
			{
				value.OnStopCallback(asServer);
			}
			foreach (SyncBase value2 in _syncObjects.Values)
			{
				value2.OnStopCallback(asServer);
			}
		}

		internal void InvokeOnNetwork(bool start)
		{
			if (start)
			{
				if (!_onStartNetworkCalled)
				{
					OnStartNetwork_Internal();
				}
			}
			else if (!_onStopNetworkCalled)
			{
				OnStopNetwork_Internal();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnStartNetwork_Internal()
		{
			_onStartNetworkCalled = true;
			_onStopNetworkCalled = false;
			OnStartNetwork();
		}

		public virtual void OnStartNetwork()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnStopNetwork_Internal()
		{
			_onStopNetworkCalled = true;
			_onStartNetworkCalled = false;
			OnStopNetwork();
		}

		public virtual void OnStopNetwork()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnStartServer_Internal()
		{
			OnStartServerCalled = true;
			OnStartServer();
		}

		public virtual void OnStartServer()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnStopServer_Internal()
		{
			OnStartServerCalled = false;
			ReturnRpcLinks();
			OnStopServer();
		}

		public virtual void OnStopServer()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnOwnershipServer_Internal(NetworkConnection prevOwner)
		{
			ClearReplicateCache(asServer: true);
			OnOwnershipServer(prevOwner);
		}

		public virtual void OnOwnershipServer(NetworkConnection prevOwner)
		{
		}

		public virtual void OnSpawnServer(NetworkConnection connection)
		{
		}

		public virtual void OnDespawnServer(NetworkConnection connection)
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnStartClient_Internal()
		{
			OnStartClientCalled = true;
			OnStartClient();
		}

		public virtual void OnStartClient()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnStopClient_Internal()
		{
			OnStartClientCalled = false;
			OnStopClient();
		}

		public virtual void OnStopClient()
		{
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnOwnershipClient_Internal(NetworkConnection prevOwner)
		{
			if (IsOwner || prevOwner == LocalConnection)
			{
				ClearReplicateCache(asServer: false);
			}
			OnOwnershipClient(prevOwner);
		}

		public virtual void OnOwnershipClient(NetworkConnection prevOwner)
		{
		}

		public override string ToString()
		{
			return $"Name [{base.gameObject.name}] ComponentId [{ComponentIndex}] NetworkObject Name [{_networkObjectCache.name}] NetworkObject Id [{_networkObjectCache.ObjectId}]";
		}

		internal void Preinitialize_Internal(NetworkObject nob, bool asServer)
		{
			_transportManagerCache = nob.TransportManager;
			InitializeOnceSyncTypes(asServer);
			if (asServer)
			{
				InitializeRpcLinks();
				_initializedOnceServer = true;
			}
			else
			{
				_initializedOnceClient = true;
			}
		}

		internal void Deinitialize(bool asServer)
		{
		}

		internal void SerializeComponents(NetworkObject nob, byte componentIndex)
		{
			_networkObjectCache = nob;
			ComponentIndex = componentIndex;
		}

		internal void InitializeIfDisabled()
		{
			if (!base.gameObject.activeInHierarchy)
			{
				NetworkInitializeIfDisabled();
			}
		}

		[CodegenMakePublic]
		[APIExclude]
		public virtual void NetworkInitializeIfDisabled()
		{
		}

		protected virtual void Reset()
		{
		}

		protected virtual void OnValidate()
		{
		}

		internal void ResetState()
		{
			SyncTypes_ResetState();
			ClearReplicateCache();
			ClearBuffedRpcs();
		}

		private NetworkObject TryAddNetworkObject()
		{
			return null;
		}

		public bool CanLog(LoggingType loggingType)
		{
			if (!(NetworkManager == null))
			{
				return NetworkManager.CanLog(loggingType);
			}
			return false;
		}

		public uint GetLastReconcileTick()
		{
			return _lastReconcileTick;
		}

		internal void SetLastReconcileTick(uint value, bool updateGlobals = true)
		{
			_lastReconcileTick = value;
			if (updateGlobals)
			{
				PredictionManager.LastReconcileTick = value;
			}
		}

		public uint GetLastReplicateTick()
		{
			return _lastReplicateTick;
		}

		private void SetLastReplicateTick(uint value, bool updateGlobals = true)
		{
			_lastReplicateTick = value;
			if (updateGlobals)
			{
				Owner.LocalReplicateTick = TimeManager.LocalTick;
				PredictionManager.LastReplicateTick = value;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenMakePublic]
		public void RegisterReplicateRpc(uint hash, ReplicateRpcDelegate del)
		{
			_replicateRpcDelegates[hash] = del;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenMakePublic]
		public void RegisterReconcileRpc(uint hash, ReconcileRpcDelegate del)
		{
			_reconcileRpcDelegates[hash] = del;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnReplicateRpc(uint? methodHash, PooledReader reader, NetworkConnection sendingClient, Channel channel)
		{
			if (!methodHash.HasValue)
			{
				methodHash = ReadRpcHash(reader);
			}
			ReplicateRpcDelegate value;
			if (sendingClient == null)
			{
				_networkObjectCache.NetworkManager.LogError($"NetworkConnection is null. Replicate {methodHash.Value} on {base.gameObject.name}, behaviour {GetType().Name} will not complete. Remainder of packet may become corrupt.");
			}
			else if (_replicateRpcDelegates.TryGetValueIL2CPP(methodHash.Value, out value))
			{
				value(reader, sendingClient, channel);
			}
			else
			{
				_networkObjectCache.NetworkManager.LogWarning($"Replicate not found for hash {methodHash.Value} on {base.gameObject.name}, behaviour {GetType().Name}. Remainder of packet may become corrupt.");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnReconcileRpc(uint? methodHash, PooledReader reader, Channel channel)
		{
			if (!methodHash.HasValue)
			{
				methodHash = ReadRpcHash(reader);
			}
			if (_reconcileRpcDelegates.TryGetValueIL2CPP(methodHash.Value, out var value))
			{
				value(reader, channel);
			}
			else
			{
				_networkObjectCache.NetworkManager.LogWarning($"Reconcile not found for hash {methodHash.Value}. Remainder of packet may become corrupt.");
			}
		}

		public void ClearReplicateCache(bool asServer)
		{
			ResetLastPredictionTicks();
			ClearReplicateCache_Virtual(asServer);
		}

		public void ClearReplicateCache()
		{
			ResetLastPredictionTicks();
			ClearReplicateCache_Virtual(asServer: true);
			ClearReplicateCache_Virtual(asServer: false);
		}

		[CodegenMakePublic]
		public virtual void ClearReplicateCache_Virtual(bool asServer)
		{
		}

		private void ResetLastPredictionTicks()
		{
			_lastSentReplicateTick = 0u;
			_lastReceivedReplicateTick = 0u;
			_lastReceivedReconcileTick = 0u;
			SetLastReconcileTick(0u, updateGlobals: false);
			SetLastReplicateTick(0u, updateGlobals: false);
		}

		private void Owner_SendReplicateRpc<T>(uint hash, List<T> replicates, Channel channel) where T : IReplicateData
		{
			if (!IsSpawnedWithWarning())
			{
				return;
			}
			int count = replicates.Count;
			if (count - 1 < 0)
			{
				return;
			}
			int num = Mathf.Min(PredictionManager.RedundancyCount, count);
			int num2 = count - num;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (_lastSentReplicateTick != 0)
			{
				uint num3 = TimeManager.LocalTick - GetLastReplicateTick();
				num2 += (int)(num3 - 1);
				if (num2 >= replicates.Count)
				{
					return;
				}
			}
			_lastSentReplicateTick = TimeManager.LocalTick;
			PooledWriter pooledWriter = WriterPool.Retrieve(1000);
			pooledWriter.WriteReplicate(replicates, num2);
			_transportManagerCache.CheckSetReliableChannel(pooledWriter.Length + 10, ref channel);
			PooledWriter pooledWriter2 = CreateRpc(hash, pooledWriter, PacketId.Replicate, channel);
			NetworkManager.TransportManager.SendToServer((byte)channel, pooledWriter2.GetArraySegment(), splitLargeMessages: false);
			if (channel == Channel.Reliable)
			{
				replicates.Clear();
				_remainingResends = 0;
			}
			pooledWriter.StoreLength();
			pooledWriter2.StoreLength();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Server_SendReconcileRpc<T>(uint hash, T reconcileData, Channel channel)
		{
			if (IsSpawned && Owner.IsActive)
			{
				PooledWriter pooledWriter = WriterPool.Retrieve();
				pooledWriter.WriteUInt32(GetLastReplicateTick());
				pooledWriter.Write(reconcileData);
				RpcLinkType value;
				PooledWriter pooledWriter2 = ((!_rpcLinks.TryGetValueIL2CPP(hash, out value)) ? CreateRpc(hash, pooledWriter, PacketId.Reconcile, channel) : CreateLinkedRpc(value, pooledWriter, channel));
				_networkObjectCache.NetworkManager.TransportManager.SendToClient((byte)channel, pooledWriter2.GetArraySegment(), Owner);
				pooledWriter.Store();
				pooledWriter2.Store();
			}
		}

		protected internal bool PredictedTransformMayChange()
		{
			if (TimeManager.PhysicsMode == PhysicsMode.Disabled)
			{
				return false;
			}
			if (!_predictionInitialized)
			{
				_predictionInitialized = true;
				_predictionRigidbody = GetComponentInParent<Rigidbody>();
				_predictionRigidbody2d = GetComponentInParent<Rigidbody2D>();
			}
			float num = 4E-06f;
			bool num2 = (base.transform.position - _lastMayChangePosition).sqrMagnitude > num;
			bool flag = (base.transform.rotation.eulerAngles - _lastMayChangeRotation.eulerAngles).sqrMagnitude > num;
			bool flag2 = (base.transform.localScale - _lastMayChangeScale).sqrMagnitude > num;
			bool flag3 = num2 || flag || flag2;
			bool result = flag3 || (_predictionRigidbody != null && (_predictionRigidbody.velocity != Vector3.zero || _predictionRigidbody.angularVelocity != Vector3.zero)) || (_predictionRigidbody2d != null && (_predictionRigidbody2d.velocity != Vector2.zero || _predictionRigidbody2d.angularVelocity != 0f));
			if (flag3)
			{
				_lastMayChangePosition = base.transform.position;
				_lastMayChangeRotation = base.transform.rotation;
				_lastMayChangeScale = base.transform.localScale;
			}
			return result;
		}

		[CodegenMakePublic]
		public bool Replicate_ExitEarly_A(bool asServer, bool replaying, bool allowServerControl)
		{
			bool isOwner = IsOwner;
			if (asServer)
			{
				if (!Owner.IsActive && !allowServerControl)
				{
					ClearReplicateCache(asServer: true);
					return true;
				}
				if (isOwner)
				{
					ClearReplicateCache();
					return true;
				}
			}
			else
			{
				if (replaying && IsServer)
				{
					return true;
				}
				if (!isOwner)
				{
					ClearReplicateCache(asServer: false);
					return true;
				}
			}
			return false;
		}

		[CodegenMakePublic]
		public void Replicate_NonOwner<T>(ReplicateUserLogicDelegate<T> del, BasicQueue<T> q, T serverControlData, bool allowServerControl, Channel channel) where T : IReplicateData
		{
			if (allowServerControl && !Owner.IsValid)
			{
				uint localTick = TimeManager.LocalTick;
				serverControlData.SetTick(localTick);
				SetLastReplicateTick(localTick);
				del(serverControlData, asServer: true, channel, replaying: false);
				return;
			}
			int count = q.Count;
			if (count > 0)
			{
				ReplicateData(q.Dequeue());
				count--;
				PredictionManager predictionManager = PredictionManager;
				bool num = !predictionManager.DropExcessiveReplicates;
				int queuedInputs = predictionManager.QueuedInputs;
				if (num && count > queuedInputs)
				{
					byte maximumReplicateConsumeCount = predictionManager.MaximumReplicateConsumeCount;
					int b = count - queuedInputs;
					int num2 = Mathf.Min(maximumReplicateConsumeCount, b);
					for (int i = 0; i < num2; i++)
					{
						ReplicateData(q.Dequeue());
					}
				}
				_remainingResends = predictionManager.RedundancyCount;
			}
			else
			{
				del(default(T), asServer: true, channel, replaying: false);
			}
			void ReplicateData(T data)
			{
				uint tick = data.GetTick();
				SetLastReplicateTick(tick);
				del(data, asServer: true, channel, replaying: false);
			}
		}

		[CodegenMakePublic]
		public void Replicate_Owner<T>(ReplicateUserLogicDelegate<T> del, uint methodHash, List<T> replicates, T data, Channel channel) where T : IReplicateData
		{
			if (!IsServer)
			{
				Func<T, bool> isDefault = GeneratedComparer<T>.IsDefault;
				if (isDefault == null)
				{
					NetworkManager.LogError("ReplicateComparers not found for type " + typeof(T).FullName);
					return;
				}
				if (replicates.Count == 0)
				{
					_lastSentReplicateTick = 0u;
				}
				PredictionManager predictionManager = NetworkManager.PredictionManager;
				bool flag = isDefault(data);
				bool flag2 = PredictedTransformMayChange();
				if (predictionManager.UsingRigidbodies || flag2 || !flag)
				{
					_remainingResends = predictionManager.RedundancyCount;
				}
				if (_remainingResends > 0)
				{
					int num = TimeManager.TickRate * 2;
					if (replicates.Count >= num)
					{
						int num2 = num / 2;
						for (int i = 0; i < num2; i++)
						{
							replicates[i].Dispose();
						}
						replicates.RemoveRange(0, num2);
					}
					uint localTick = TimeManager.LocalTick;
					data.SetTick(localTick);
					replicates.Add(data);
				}
				if (_remainingResends > 0)
				{
					_remainingResends--;
					Owner_SendReplicateRpc(methodHash, replicates, channel);
					SetLastReplicateTick(TimeManager.LocalTick);
				}
			}
			del(data, asServer: false, channel, replaying: false);
		}

		[CodegenMakePublic]
		public void Replicate_Reader<T>(PooledReader reader, NetworkConnection sender, T[] arrBuffer, BasicQueue<T> replicates, Channel channel) where T : IReplicateData
		{
			PredictionManager predictionManager = PredictionManager;
			int num = reader.ReadReplicate(ref arrBuffer, TimeManager.LastPacketTick);
			if (OwnerMatches(sender))
			{
				if (num > predictionManager.RedundancyCount)
				{
					sender.Kick(reader, KickReason.ExploitAttempt, LoggingType.Common, "Connection " + sender.ToString() + " sent too many past replicates. Connection will be kicked immediately.");
				}
				else
				{
					Replicate_HandleReceivedReplicate(num, arrBuffer, replicates, channel);
				}
			}
		}

		private void Replicate_HandleReceivedReplicate<T>(int receivedReplicatesCount, T[] arrBuffer, BasicQueue<T> replicates, Channel channel) where T : IReplicateData
		{
			PredictionManager predictionManager = PredictionManager;
			int num = ((!predictionManager.DropExcessiveReplicates) ? (TimeManager.TickRate * 2) : predictionManager.GetMaximumServerReplicates());
			for (int i = 0; i < receivedReplicatesCount; i++)
			{
				uint tick = arrBuffer[i].GetTick();
				if (tick > _lastReceivedReplicateTick)
				{
					if (replicates.Count >= num)
					{
						replicates.Dequeue().Dispose();
					}
					replicates.Enqueue(arrBuffer[i]);
					_lastReceivedReplicateTick = tick;
				}
			}
			if (IsServer && Owner.IsValid)
			{
				Owner.AddAverageQueueCount((ushort)replicates.Count, TimeManager.LocalTick);
			}
		}

		[CodegenMakePublic]
		public bool Reconcile_ExitEarly_A(bool asServer, out Channel channel)
		{
			channel = Channel.Unreliable;
			if (asServer)
			{
				if (_remainingResends <= 0)
				{
					return true;
				}
				_remainingResends--;
				if (_remainingResends == 0)
				{
					channel = Channel.Reliable;
				}
			}
			else
			{
				if (!ClientHasReconcileData)
				{
					return true;
				}
				if (IsServer)
				{
					PredictionManager.InvokeOnReconcile(this, before: true);
					PredictionManager.InvokeOnReconcile(this, before: false);
					return true;
				}
			}
			return false;
		}

		[CodegenMakePublic]
		public void Reconcile_Server<T>(uint methodHash, T data, Channel channel) where T : IReconcileData
		{
			uint lastReplicateTick = _lastReplicateTick;
			data.SetTick(lastReplicateTick);
			SetLastReconcileTick(lastReplicateTick);
			PredictionManager.InvokeServerReconcile(this, before: true);
			Server_SendReconcileRpc(methodHash, data, channel);
			PredictionManager.InvokeServerReconcile(this, before: false);
		}

		[CodegenMakePublic]
		public void Reconcile_Client<T, T2>(ReconcileUserLogicDelegate<T> reconcileDel, ReplicateUserLogicDelegate<T2> replicateULDel, List<T2> replicates, T data, Channel channel) where T : IReconcileData where T2 : IReplicateData
		{
			uint tick = data.GetTick();
			if (replicates.Count > 0 && replicates[0].GetTick() > tick)
			{
				return;
			}
			Scene scene = base.gameObject.scene;
			PhysicsScene physicsScene = scene.GetPhysicsScene();
			PhysicsScene2D physicsScene2D = scene.GetPhysicsScene2D();
			SetLastReconcileTick(tick);
			PredictionManager.InvokeOnReconcile(this, before: true);
			reconcileDel?.Invoke(data, asServer: false, channel);
			bool flag = TimeManager.PhysicsMode == PhysicsMode.TimeManager;
			if (flag)
			{
				Physics.SyncTransforms();
				Physics2D.SyncTransforms();
			}
			int num = -1;
			for (int i = 0; i < replicates.Count; i++)
			{
				if (replicates[i].GetTick() == tick)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				replicates.Clear();
			}
			else
			{
				replicates.RemoveRange(0, num + 1);
			}
			int count = replicates.Count;
			float step = (float)TimeManager.TickDelta;
			for (int j = 0; j < count; j++)
			{
				T2 data2 = replicates[j];
				uint tick2 = data2.GetTick();
				PredictionManager.InvokeOnReplicateReplay(scene, tick2, physicsScene, physicsScene2D, before: true);
				replicateULDel(data2, asServer: false, channel, replaying: true);
				if (flag)
				{
					physicsScene.Simulate(step);
					physicsScene2D.Simulate(step);
				}
				PredictionManager.InvokeOnReplicateReplay(scene, tick2, physicsScene, physicsScene2D, before: false);
			}
			PredictionManager.InvokeOnReconcile(this, before: false);
		}

		public void Reconcile_Reader<T>(PooledReader reader, ref T data, Channel channel) where T : IReconcileData
		{
			uint num = reader.ReadUInt32();
			T val = reader.Read<T>();
			if (num > _lastReceivedReconcileTick && IsOwner)
			{
				data = val;
				data.SetTick(num);
				ClientHasReconcileData = true;
				_lastReceivedReconcileTick = num;
			}
		}

		public bool OwnerMatches(NetworkConnection connection)
		{
			return _networkObjectCache.Owner == connection;
		}

		public void Despawn(GameObject go, DespawnType? despawnType = null)
		{
			if (!IsNetworkObjectNull(warn: true))
			{
				_networkObjectCache.Despawn(go, despawnType);
			}
		}

		public void Despawn(NetworkObject nob, DespawnType? despawnType = null)
		{
			if (!IsNetworkObjectNull(warn: true))
			{
				_networkObjectCache.Despawn(nob, despawnType);
			}
		}

		public void Despawn(DespawnType? despawnType = null)
		{
			if (!IsNetworkObjectNull(warn: true))
			{
				_networkObjectCache.Despawn(despawnType);
			}
		}

		public void Spawn(GameObject go, NetworkConnection ownerConnection = null, Scene scene = default(Scene))
		{
			if (!IsNetworkObjectNull(warn: true))
			{
				_networkObjectCache.Spawn(go, ownerConnection, scene);
			}
		}

		public void Spawn(NetworkObject nob, NetworkConnection ownerConnection = null, Scene scene = default(Scene))
		{
			if (!IsNetworkObjectNull(warn: true))
			{
				_networkObjectCache.Spawn(nob, ownerConnection, scene);
			}
		}

		private bool IsNetworkObjectNull(bool warn)
		{
			bool num = _networkObjectCache == null;
			if (num && warn)
			{
				NetworkManager.LogWarning("NetworkObject is null. This can occur if this object is not spawned, or initialized yet.");
			}
			return num;
		}

		public void RemoveOwnership()
		{
			_networkObjectCache.GiveOwnership(null, asServer: true);
		}

		public void GiveOwnership(NetworkConnection newOwner)
		{
			_networkObjectCache.GiveOwnership(newOwner, asServer: true);
		}

		public void RegisterInvokeOnInstance<T>(Action<UnityEngine.Component> handler) where T : UnityEngine.Component
		{
			_networkObjectCache.RegisterInvokeOnInstance<T>(handler);
		}

		public void UnregisterInvokeOnInstance<T>(Action<UnityEngine.Component> handler) where T : UnityEngine.Component
		{
			_networkObjectCache.UnregisterInvokeOnInstance<T>(handler);
		}

		public T GetInstance<T>() where T : UnityEngine.Component
		{
			return _networkObjectCache.GetInstance<T>();
		}

		public void RegisterInstance<T>(T component, bool replace = true) where T : UnityEngine.Component
		{
			_networkObjectCache.RegisterInstance(component, replace);
		}

		public bool TryRegisterInstance<T>(T component) where T : UnityEngine.Component
		{
			return _networkObjectCache.TryRegisterInstance(component);
		}

		public void UnregisterInstance<T>() where T : UnityEngine.Component
		{
			_networkObjectCache.UnregisterInstance<T>();
		}

		private void InitializeRpcLinks()
		{
			ServerManager serverManager = NetworkManager.ServerManager;
			foreach (uint key in _observersRpcDelegates.Keys)
			{
				if (!MakeLink(key, RpcType.Observers))
				{
					return;
				}
			}
			foreach (uint key2 in _targetRpcDelegates.Keys)
			{
				if (!MakeLink(key2, RpcType.Target))
				{
					return;
				}
			}
			foreach (uint key3 in _reconcileRpcDelegates.Keys)
			{
				if (!MakeLink(key3, RpcType.Reconcile))
				{
					break;
				}
			}
			bool MakeLink(uint rpcHash, RpcType rpcType)
			{
				if (serverManager.GetRpcLink(out var value))
				{
					_rpcLinks[rpcHash] = new RpcLinkType(value, rpcType);
					return true;
				}
				return false;
			}
		}

		private int GetEstimatedRpcHeaderLength()
		{
			return 20;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private PooledWriter CreateLinkedRpc(RpcLinkType link, PooledWriter methodWriter, Channel channel)
		{
			int estimatedRpcHeaderLength = GetEstimatedRpcHeaderLength();
			int length = methodWriter.Length;
			PooledWriter pooledWriter = WriterPool.Retrieve(estimatedRpcHeaderLength + length);
			pooledWriter.WriteUInt16(link.LinkIndex);
			if (channel == Channel.Reliable)
			{
				pooledWriter.WriteLength(methodWriter.Length);
			}
			pooledWriter.WriteArraySegment(methodWriter.GetArraySegment());
			return pooledWriter;
		}

		private void ReturnRpcLinks()
		{
			if (_rpcLinks.Count != 0)
			{
				ServerManager?.StoreRpcLinks(_rpcLinks);
				_rpcLinks.Clear();
			}
		}

		internal void WriteRpcLinks(Writer writer)
		{
			PooledWriter pooledWriter = WriterPool.Retrieve();
			foreach (KeyValuePair<uint, RpcLinkType> rpcLink in _rpcLinks)
			{
				pooledWriter.WriteUInt16(rpcLink.Value.LinkIndex);
				pooledWriter.WriteUInt16((ushort)rpcLink.Key);
				pooledWriter.WriteByte((byte)rpcLink.Value.RpcType);
			}
			writer.WriteBytesAndSize(pooledWriter.GetBuffer(), 0, pooledWriter.Length);
			pooledWriter.Store();
		}

		internal void SendBufferedRpcs(NetworkConnection conn)
		{
			TransportManager transportManager = _networkObjectCache.NetworkManager.TransportManager;
			foreach (BufferedRpc value in _bufferedRpcs.Values)
			{
				transportManager.SendToClient((byte)value.Channel, value.Writer.GetArraySegment(), conn, splitLargeMessages: true, value.OrderType);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		[CodegenMakePublic]
		public void RegisterServerRpc(uint hash, ServerRpcDelegate del)
		{
			if (_serverRpcDelegates.TryGetValueIL2CPP(hash, out var value))
			{
				NetworkManager.StaticLogError($"ServerRpc hash {hash} registered multiple times. First registration by {value.Method.DeclaringType.GetType().FullName}. New registration by {GetType().FullName}.");
				return;
			}
			_serverRpcDelegates[hash] = del;
			IncreaseRpcMethodCount();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		[CodegenMakePublic]
		public void RegisterObserversRpc(uint hash, ClientRpcDelegate del)
		{
			if (_observersRpcDelegates.TryGetValueIL2CPP(hash, out var value))
			{
				NetworkManager.StaticLogWarning($"ObserverRpc hash {hash} registered multiple times. First registration by {value.Method.DeclaringType.GetType().FullName}. New registration by {GetType().FullName}.");
				return;
			}
			_observersRpcDelegates[hash] = del;
			IncreaseRpcMethodCount();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		[CodegenMakePublic]
		public void RegisterTargetRpc(uint hash, ClientRpcDelegate del)
		{
			if (_targetRpcDelegates.TryGetValueIL2CPP(hash, out var value))
			{
				NetworkManager.StaticLogError($"TargetRpc hash {hash} registered multiple times. First registration by {value.Method.DeclaringType.GetType().FullName}. New registration by {GetType().FullName}.");
				return;
			}
			_targetRpcDelegates[hash] = del;
			IncreaseRpcMethodCount();
		}

		private void IncreaseRpcMethodCount()
		{
			_rpcMethodCount++;
			if (_rpcMethodCount <= 255)
			{
				_rpcHashSize = 1;
			}
			else
			{
				_rpcHashSize = 2;
			}
		}

		public void ClearBuffedRpcs()
		{
			foreach (BufferedRpc value in _bufferedRpcs.Values)
			{
				value.Writer.Store();
			}
			_bufferedRpcs.Clear();
		}

		private uint ReadRpcHash(PooledReader reader)
		{
			if (_rpcHashSize == 1)
			{
				return reader.ReadByte();
			}
			return reader.ReadUInt16();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnServerRpc(PooledReader reader, NetworkConnection sendingClient, Channel channel)
		{
			uint num = ReadRpcHash(reader);
			ServerRpcDelegate value;
			if (sendingClient == null)
			{
				_networkObjectCache.NetworkManager.LogError($"NetworkConnection is null. ServerRpc {num} on object {base.gameObject.name} [id {ObjectId}] will not complete. Remainder of packet may become corrupt.");
			}
			else if (_serverRpcDelegates.TryGetValueIL2CPP(num, out value))
			{
				value(reader, channel, sendingClient);
			}
			else
			{
				_networkObjectCache.NetworkManager.LogWarning($"ServerRpc not found for hash {num} on object {base.gameObject.name} [id {ObjectId}]. Remainder of packet may become corrupt.");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnObserversRpc(uint? methodHash, PooledReader reader, Channel channel)
		{
			if (!methodHash.HasValue)
			{
				methodHash = ReadRpcHash(reader);
			}
			if (_observersRpcDelegates.TryGetValueIL2CPP(methodHash.Value, out var value))
			{
				value(reader, channel);
			}
			else
			{
				_networkObjectCache.NetworkManager.LogWarning($"ObserversRpc not found for hash {methodHash.Value} on object {base.gameObject.name} [id {ObjectId}] . Remainder of packet may become corrupt.");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnTargetRpc(uint? methodHash, PooledReader reader, Channel channel)
		{
			if (!methodHash.HasValue)
			{
				methodHash = ReadRpcHash(reader);
			}
			if (_targetRpcDelegates.TryGetValueIL2CPP(methodHash.Value, out var value))
			{
				value(reader, channel);
			}
			else
			{
				_networkObjectCache.NetworkManager.LogWarning($"TargetRpc not found for hash {methodHash.Value} on object {base.gameObject.name} [id {ObjectId}] . Remainder of packet may become corrupt.");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenMakePublic]
		public void SendServerRpc(uint hash, PooledWriter methodWriter, Channel channel, DataOrderType orderType)
		{
			if (IsSpawnedWithWarning())
			{
				_transportManagerCache.CheckSetReliableChannel(methodWriter.Length + 10, ref channel);
				PooledWriter pooledWriter = CreateRpc(hash, methodWriter, PacketId.ServerRpc, channel);
				_networkObjectCache.NetworkManager.TransportManager.SendToServer((byte)channel, pooledWriter.GetArraySegment(), splitLargeMessages: true, orderType);
				pooledWriter.StoreLength();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[APIExclude]
		[CodegenMakePublic]
		public void SendObserversRpc(uint hash, PooledWriter methodWriter, Channel channel, DataOrderType orderType, bool bufferLast, bool excludeServer, bool excludeOwner)
		{
			if (!IsSpawnedWithWarning())
			{
				return;
			}
			_transportManagerCache.CheckSetReliableChannel(methodWriter.Length + 10, ref channel);
			RpcLinkType value;
			PooledWriter pooledWriter = ((!_rpcLinks.TryGetValueIL2CPP(hash, out value)) ? CreateRpc(hash, methodWriter, PacketId.ObserversRpc, channel) : CreateLinkedRpc(value, methodWriter, channel));
			SetNetworkConnectionCache(excludeServer, excludeOwner);
			_networkObjectCache.NetworkManager.TransportManager.SendToClients((byte)channel, pooledWriter.GetArraySegment(), _networkObjectCache.Observers, _networkConnectionCache, splitLargeMessages: true, orderType);
			if (bufferLast)
			{
				if (_bufferedRpcs.TryGetValueIL2CPP(hash, out var value2))
				{
					value2.Writer.StoreLength();
				}
				_bufferedRpcs[hash] = new BufferedRpc(pooledWriter, channel, orderType);
			}
			else
			{
				pooledWriter.StoreLength();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[CodegenMakePublic]
		public void SendTargetRpc(uint hash, PooledWriter methodWriter, Channel channel, DataOrderType orderType, NetworkConnection target, bool excludeServer, bool validateTarget = true)
		{
			if (!IsSpawnedWithWarning())
			{
				return;
			}
			_transportManagerCache.CheckSetReliableChannel(methodWriter.Length + 10, ref channel);
			if (validateTarget)
			{
				if (target == null)
				{
					_networkObjectCache.NetworkManager.LogWarning("Action cannot be completed as no Target is specified.");
					return;
				}
				if (!_networkObjectCache.Observers.Contains(target))
				{
					_networkObjectCache.NetworkManager.LogWarning($"Action cannot be completed as Target is not an observer for object {base.gameObject.name} [id {ObjectId}].");
					return;
				}
			}
			if (!excludeServer || !target.IsLocalClient)
			{
				RpcLinkType value;
				PooledWriter pooledWriter = ((!_rpcLinks.TryGetValueIL2CPP(hash, out value)) ? CreateRpc(hash, methodWriter, PacketId.TargetRpc, channel) : CreateLinkedRpc(value, methodWriter, channel));
				_networkObjectCache.NetworkManager.TransportManager.SendToClient((byte)channel, pooledWriter.GetArraySegment(), target, splitLargeMessages: true, orderType);
				pooledWriter.Store();
			}
		}

		private void SetNetworkConnectionCache(bool addClientHost, bool addOwner)
		{
			_networkConnectionCache.Clear();
			if (addClientHost && IsClient)
			{
				_networkConnectionCache.Add(LocalConnection);
			}
			if (addOwner && Owner.IsValid)
			{
				_networkConnectionCache.Add(Owner);
			}
		}

		private bool IsSpawnedWithWarning()
		{
			bool isSpawned = IsSpawned;
			if (!isSpawned)
			{
				_networkObjectCache.NetworkManager.LogWarning($"Action cannot be completed as object {base.gameObject.name} [Id {ObjectId}] is not spawned.");
			}
			return isSpawned;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private PooledWriter CreateRpc(uint hash, PooledWriter methodWriter, PacketId packetId, Channel channel)
		{
			int estimatedRpcHeaderLength = GetEstimatedRpcHeaderLength();
			int length = methodWriter.Length;
			PooledWriter pooledWriter = WriterPool.Retrieve(estimatedRpcHeaderLength + length);
			pooledWriter.WritePacketId(packetId);
			pooledWriter.WriteNetworkBehaviour(this);
			if (channel == Channel.Reliable)
			{
				pooledWriter.WriteLength(length + _rpcHashSize);
			}
			WriteRpcHash(hash, pooledWriter);
			pooledWriter.WriteArraySegment(methodWriter.GetArraySegment());
			return pooledWriter;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void WriteRpcHash(uint hash, PooledWriter writer)
		{
			if (_rpcHashSize == 1)
			{
				writer.WriteByte((byte)hash);
			}
			else
			{
				writer.WriteUInt16((byte)hash);
			}
		}

		[CodegenMakePublic]
		public void RegisterSyncVarRead(SyncVarReadDelegate del)
		{
			_syncVarReadDelegates.Add(del);
		}

		internal void RegisterSyncType(SyncBase sb, uint index)
		{
			if (sb.IsSyncObject)
			{
				_syncObjects.Add(index, sb);
			}
			else
			{
				_syncVars.Add(index, sb);
			}
		}

		internal bool DirtySyncType(bool isSyncObject)
		{
			if (!IsServer)
			{
				return false;
			}
			if (_networkObjectCache.Observers.Count == 0 && !_networkObjectCache.PredictedSpawner.IsValid)
			{
				return false;
			}
			bool num = (isSyncObject ? SyncObjectDirty : SyncVarDirty);
			if (isSyncObject)
			{
				SyncObjectDirty = true;
			}
			else
			{
				SyncVarDirty = true;
			}
			if (!num)
			{
				_networkObjectCache.NetworkManager.ServerManager.Objects.SetDirtySyncType(this, isSyncObject);
			}
			return true;
		}

		private void InitializeOnceSyncTypes(bool asServer)
		{
			if (asServer)
			{
				if (!_initializedOnceServer)
				{
					if (_readPermissions == null)
					{
						Array values = Enum.GetValues(typeof(ReadPermission));
						_readPermissions = new ReadPermission[values.Length];
						int num = 0;
						foreach (ReadPermission item in values)
						{
							_readPermissions[num] = item;
							num++;
						}
					}
					_syncTypeWriters = new SyncTypeWriter[_readPermissions.Length];
					for (int i = 0; i < _syncTypeWriters.Length; i++)
					{
						_syncTypeWriters[i] = new SyncTypeWriter(_readPermissions[i]);
					}
				}
				else
				{
					for (int j = 0; j < _syncTypeWriters.Length; j++)
					{
						_syncTypeWriters[j].Reset();
					}
				}
			}
			foreach (SyncBase value in _syncVars.Values)
			{
				value.PreInitialize(_networkObjectCache.NetworkManager);
			}
			foreach (SyncBase value2 in _syncObjects.Values)
			{
				value2.PreInitialize(_networkObjectCache.NetworkManager);
			}
		}

		internal void OnSyncType(PooledReader reader, int length, bool isSyncObject, bool asServer = false)
		{
			int position = reader.Position;
			while (reader.Position - position < length)
			{
				byte b = reader.ReadByte();
				if (isSyncObject)
				{
					if (_syncObjects.TryGetValueIL2CPP(b, out var value))
					{
						value.Read(reader, asServer);
					}
					else
					{
						NetworkManager.LogWarning($"SyncObject not found for index {b} on {base.transform.name}. Remainder of packet may become corrupt.");
					}
					continue;
				}
				bool flag = false;
				for (int i = 0; i < _syncVarReadDelegates.Count; i++)
				{
					if (_syncVarReadDelegates[i](reader, b, asServer))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					NetworkManager.LogWarning($"SyncVar not found for index {b} on {base.transform.name}. Remainder of packet may become corrupt.");
				}
			}
		}

		internal bool WriteDirtySyncTypes(bool isSyncObject, bool ignoreInterval = false)
		{
			if (!IsSpawned)
			{
				SyncTypes_ResetState();
				return true;
			}
			if (isSyncObject && (!SyncObjectDirty || _syncObjects.Count == 0))
			{
				return true;
			}
			if (!isSyncObject && (!SyncVarDirty || _syncVars.Count == 0))
			{
				return true;
			}
			bool flag = false;
			uint tick = _networkObjectCache.NetworkManager.TimeManager.Tick;
			bool flag2 = false;
			bool flag3 = false;
			foreach (SyncBase value in (isSyncObject ? _syncObjects : _syncVars).Values)
			{
				if (!value.IsDirty)
				{
					continue;
				}
				flag2 = true;
				if (!ignoreInterval && !value.SyncTimeMet(tick))
				{
					continue;
				}
				if (!flag)
				{
					flag = true;
					for (int i = 0; i < _syncTypeWriters.Length; i++)
					{
						_syncTypeWriters[i].Reset();
					}
				}
				byte b = (byte)value.Channel;
				value.ResetDirty();
				if (value.Settings.ReadPermission == ReadPermission.OwnerOnly && !_networkObjectCache.Owner.IsValid)
				{
					continue;
				}
				flag3 = true;
				PooledWriter pooledWriter = null;
				for (int j = 0; j < _syncTypeWriters.Length; j++)
				{
					if (_syncTypeWriters[j].ReadPermission == value.Settings.ReadPermission)
					{
						if (b >= _syncTypeWriters[j].Writers.Length)
						{
							b = 0;
						}
						pooledWriter = _syncTypeWriters[j].Writers[b];
						break;
					}
				}
				if (pooledWriter == null)
				{
					NetworkManager.LogError($"Writer couldn't be found for permissions {value.Settings.ReadPermission} on channel {b}.");
				}
				else
				{
					value.WriteDelta(pooledWriter);
				}
			}
			if (!flag2)
			{
				if (isSyncObject)
				{
					SyncObjectDirty = false;
				}
				else
				{
					SyncVarDirty = false;
				}
				return true;
			}
			if (flag3)
			{
				for (int k = 0; k < _syncTypeWriters.Length; k++)
				{
					for (byte b2 = 0; b2 < _syncTypeWriters[k].Writers.Length; b2++)
					{
						PooledWriter pooledWriter2 = _syncTypeWriters[k].Writers[b2];
						if (pooledWriter2.Length > 0)
						{
							PooledWriter pooledWriter3 = WriterPool.Retrieve();
							PacketId pid = (isSyncObject ? PacketId.SyncObject : PacketId.SyncVar);
							pooledWriter3.WritePacketId(pid);
							PooledWriter pooledWriter4 = WriterPool.Retrieve();
							pooledWriter4.WriteNetworkBehaviour(this);
							if (!isSyncObject || b2 == 0)
							{
								pooledWriter4.WriteBytesAndSize(pooledWriter2.GetBuffer(), 0, pooledWriter2.Length);
							}
							else
							{
								pooledWriter4.WriteBytes(pooledWriter2.GetBuffer(), 0, pooledWriter2.Length);
							}
							pooledWriter3.WriteArraySegment(pooledWriter4.GetArraySegment());
							pooledWriter4.Store();
							if (_syncTypeWriters[k].ReadPermission == ReadPermission.OwnerOnly)
							{
								_networkObjectCache.NetworkManager.TransportManager.SendToClient(b2, pooledWriter3.GetArraySegment(), _networkObjectCache.Owner);
							}
							else
							{
								bool addOwner = _syncTypeWriters[k].ReadPermission == ReadPermission.ExcludeOwner;
								SetNetworkConnectionCache(addClientHost: false, addOwner);
								_networkObjectCache.NetworkManager.TransportManager.SendToClients(b2, pooledWriter3.GetArraySegment(), _networkObjectCache.Observers, _networkConnectionCache);
							}
							pooledWriter3.Store();
						}
					}
				}
			}
			return false;
		}

		internal void SyncTypes_ResetState()
		{
			foreach (SyncBase value in _syncVars.Values)
			{
				byte b = (byte)value.SyncIndex;
				value.ResetState();
				if (b < _syncVarReadDelegates.Count)
				{
					_syncVarReadDelegates[b]?.Invoke(null, b, asServer: true);
				}
			}
			SyncObjectDirty = false;
			SyncVarDirty = false;
		}

		[CodegenMakePublic]
		public virtual void ResetSyncVarFields()
		{
		}

		internal void WriteSyncTypesForSpawn(PooledWriter writer, NetworkConnection conn)
		{
			WriteSyncType(_syncVars);
			WriteSyncType(_syncObjects);
			void WriteSyncType(Dictionary<uint, SyncBase> collection)
			{
				PooledWriter pooledWriter = WriterPool.Retrieve();
				foreach (SyncBase value in collection.Values)
				{
					if (conn != null)
					{
						bool flag = conn == _networkObjectCache.Owner;
						ReadPermission readPermission = value.Settings.ReadPermission;
						if ((readPermission == ReadPermission.OwnerOnly && !flag) || (readPermission == ReadPermission.ExcludeOwner && flag))
						{
							continue;
						}
					}
					value.WriteFull(pooledWriter);
				}
				writer.WriteBytesAndSize(pooledWriter.GetBuffer(), 0, pooledWriter.Length);
				pooledWriter.Store();
			}
		}

		[Obsolete("This method does not function.")]
		protected void DirtySyncType(object syncType)
		{
		}
	}
}
