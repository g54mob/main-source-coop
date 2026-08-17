using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Managing.Object;
using FishNet.Managing.Scened;
using FishNet.Managing.Timing;
using FishNet.Managing.Utility;
using FishNet.Object;
using FishNet.Observing;
using FishNet.Serializing;
using FishNet.Transporting;
using FishNet.Utility.Extension;
using GameKit.Utilities;
using GameKit.Utilities.Types;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Managing.Server
{
	public class ServerObjects : ManagedObjects
	{
		internal Dictionary<int, uint> RecentlyDespawnedIds = new Dictionary<int, uint>();

		private Queue<int> _objectIdCache = new Queue<int>();

		private List<NetworkBehaviour> _dirtySyncVarBehaviours = new List<NetworkBehaviour>(20);

		private List<NetworkBehaviour> _dirtySyncObjectBehaviours = new List<NetworkBehaviour>(20);

		private Dictionary<int, NetworkObject> _pendingDestroy = new Dictionary<int, NetworkObject>();

		private List<(int, Scene)> _loadedScenes = new List<(int, Scene)>();

		private List<NetworkObject> _spawnCache = new List<NetworkObject>();

		private bool _scenesLoading;

		private List<NetworkObject> _observerChangedObjectsCache = new List<NetworkObject>(100);

		private List<NetworkObject> _timedNetworkObservers = new List<NetworkObject>();

		private int _nextTimedObserversIndex;

		private PooledWriter _writer = new PooledWriter();

		private uint _cleanRecentlyDespawnedMaxTicks => base.NetworkManager.TimeManager.TimeToTicks(30.0, TickRounding.RoundUp);

		public event Action<NetworkConnection> OnPreDestroyClientObjects;

		internal Queue<int> GetObjectIdCache()
		{
			return _objectIdCache;
		}

		internal ServerObjects(NetworkManager networkManager)
		{
			base.Initialize(networkManager);
			networkManager.SceneManager.OnLoadStart += SceneManager_OnLoadStart;
			networkManager.SceneManager.OnActiveSceneSetInternal += SceneManager_OnActiveSceneSet;
			networkManager.TimeManager.OnUpdate += TimeManager_OnUpdate;
		}

		private void TimeManager_OnUpdate()
		{
			if (!base.NetworkManager.IsServer)
			{
				_scenesLoading = false;
				_loadedScenes.Clear();
				return;
			}
			CleanRecentlyDespawned();
			if (!_scenesLoading)
			{
				IterateLoadedScenes(ignoreFrameRestriction: false);
			}
			Observers_OnUpdate();
		}

		internal void WriteDirtySyncTypes()
		{
			IterateCollection(_dirtySyncVarBehaviours, isSyncObject: false);
			IterateCollection(_dirtySyncObjectBehaviours, isSyncObject: true);
			static void IterateCollection(List<NetworkBehaviour> collection, bool isSyncObject)
			{
				for (int i = 0; i < collection.Count; i++)
				{
					if (collection[i].WriteDirtySyncTypes(isSyncObject))
					{
						collection.RemoveAt(i);
						i--;
					}
				}
			}
		}

		internal void SetDirtySyncType(NetworkBehaviour nb, bool isSyncObject)
		{
			if (isSyncObject)
			{
				_dirtySyncObjectBehaviours.Add(nb);
			}
			else
			{
				_dirtySyncVarBehaviours.Add(nb);
			}
		}

		internal void OnServerConnectionState(ServerConnectionStateArgs args)
		{
			if (args.ConnectionState == LocalConnectionState.Started)
			{
				if (base.NetworkManager.ServerManager.OneServerStarted())
				{
					BuildObjectIdCache();
					SetupSceneObjects();
				}
			}
			else if (!base.NetworkManager.ServerManager.AnyServerStarted())
			{
				base.DespawnWithoutSynchronization(asServer: true);
				SceneObjects_Internal.Clear();
				_objectIdCache.Clear();
				base.NetworkManager.ClearClientsCollection(base.NetworkManager.ServerManager.Clients);
			}
			else
			{
				base.NetworkManager.ClearClientsCollection(base.NetworkManager.ServerManager.Clients, args.TransportIndex);
			}
		}

		internal void ClientDisconnected(NetworkConnection connection)
		{
			RemoveFromObserversWithoutSynchronization(connection);
			this.OnPreDestroyClientObjects?.Invoke(connection);
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList();
			foreach (NetworkObject @object in connection.Objects)
			{
				list.Add(@object);
			}
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				if (!list[i].IsDeinitializing)
				{
					base.NetworkManager.ServerManager.Despawn(list[i]);
				}
			}
			CollectionCaches<NetworkObject>.Store(list);
		}

		private void BuildObjectIdCache()
		{
			_objectIdCache.Clear();
			List<int> list = new List<int>();
			for (int i = 0; i < 65534; i++)
			{
				list.Add(i);
			}
			list.Shuffle();
			int count = list.Count;
			for (int j = 0; j < count; j++)
			{
				_objectIdCache.Enqueue(list[j]);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CacheObjectId(NetworkObject nob)
		{
			if (nob.ObjectId != 65535)
			{
				CacheObjectId(nob.ObjectId);
			}
		}

		internal void CacheObjectId(int id)
		{
			_objectIdCache.Enqueue(id);
		}

		protected internal override int GetNextNetworkObjectId(bool errorCheck = true)
		{
			if (_objectIdCache.Count == 0)
			{
				base.NetworkManager.LogError($"No more available ObjectIds. How the heck did you manage to have {ushort.MaxValue} objects spawned at once?");
				return -1;
			}
			return _objectIdCache.Dequeue();
		}

		private void SceneManager_OnLoadStart(SceneLoadStartEventArgs obj)
		{
			_scenesLoading = true;
		}

		private void SceneManager_OnActiveSceneSet()
		{
			_scenesLoading = false;
			IterateLoadedScenes(ignoreFrameRestriction: true);
		}

		internal void IterateLoadedScenes(bool ignoreFrameRestriction)
		{
			if (!base.NetworkManager.ServerManager.Started)
			{
				_loadedScenes.Clear();
			}
			for (int i = 0; i < _loadedScenes.Count; i++)
			{
				(int, Scene) tuple = _loadedScenes[i];
				if (ignoreFrameRestriction || Time.frameCount > tuple.Item1)
				{
					SetupSceneObjects(tuple.Item2);
					_loadedScenes.RemoveAt(i);
					i--;
				}
			}
		}

		protected internal override void SceneManager_sceneLoaded(Scene s, LoadSceneMode arg1)
		{
			base.SceneManager_sceneLoaded(s, arg1);
			if (base.NetworkManager.ServerManager.Started)
			{
				_loadedScenes.Add((Time.frameCount, s));
			}
		}

		protected internal void SetupSceneObjects()
		{
			for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
			{
				SetupSceneObjects(UnityEngine.SceneManagement.SceneManager.GetSceneAt(i));
			}
			Scene scene = DDOL.GetDDOL().gameObject.scene;
			if (scene.isLoaded)
			{
				SetupSceneObjects(scene);
			}
		}

		private void SetupSceneObjects(Scene s)
		{
			if (!s.IsValid())
			{
				return;
			}
			List<NetworkObject> result = CollectionCaches<NetworkObject>.RetrieveList();
			Scenes.GetSceneNetworkObjects(s, firstOnly: false, errorOnDuplicates: true, ref result);
			bool initializationOrderChanged = false;
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList();
			foreach (NetworkObject item in result)
			{
				OrderRootByInitializationOrder(item, list, ref initializationOrderChanged);
			}
			OrderNestedByInitializationOrder(list);
			CollectionCaches<NetworkObject>.Store(result);
			bool isHost = base.NetworkManager.IsHost;
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				NetworkObject networkObject = list[i];
				if (!networkObject.IsNetworked || !networkObject.IsSceneObject || !networkObject.IsDeinitializing)
				{
					continue;
				}
				UpdateNetworkBehavioursForSceneObject(networkObject, asServer: true);
				AddToSceneObjects(networkObject);
				if (networkObject.ActiveDuringEdit || networkObject.gameObject.activeInHierarchy)
				{
					if (!isHost)
					{
						SetupWithoutSynchronization(networkObject);
					}
					else
					{
						SpawnWithoutChecks(networkObject);
					}
				}
			}
			CollectionCaches<NetworkObject>.Store(list);
		}

		private void SetupWithoutSynchronization(NetworkObject nob, NetworkConnection ownerConnection = null, int? objectId = null)
		{
			if (nob.IsNetworked)
			{
				if (!objectId.HasValue)
				{
					objectId = GetNextNetworkObjectId();
				}
				nob.Preinitialize_Internal(base.NetworkManager, objectId.Value, ownerConnection, asServer: true);
				base.AddToSpawned(nob, asServer: true);
				nob.gameObject.SetActive(value: true);
				nob.Initialize(asServer: true, invokeSyncTypeCallbacks: true);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Spawn(NetworkObject networkObject, NetworkConnection ownerConnection = null, Scene scene = default(Scene))
		{
			bool flag = false;
			if (networkObject == null)
			{
				base.NetworkManager.LogError("Specified networkObject is null.");
				return;
			}
			if (!base.NetworkManager.ServerManager.Started)
			{
				if (!base.NetworkManager.ClientManager.Started)
				{
					base.NetworkManager.LogWarning("Cannot spawn object because server nor client are active.");
					return;
				}
				if (!base.NetworkManager.PredictionManager.GetAllowPredictedSpawning())
				{
					base.NetworkManager.LogWarning("Cannot spawn object because server is not active and predicted spawning is not enabled.");
					return;
				}
				if (!CanPredictedSpawn(networkObject, base.NetworkManager.ClientManager.Connection, ownerConnection, asServer: false))
				{
					return;
				}
				flag = true;
			}
			if (!networkObject.gameObject.scene.IsValid())
			{
				base.NetworkManager.LogError(networkObject.name + " is a prefab. You must instantiate the prefab first, then use Spawn on the instantiated copy.");
				return;
			}
			if (ownerConnection != null && ownerConnection.IsActive && !ownerConnection.LoadedStartScenes(!flag))
			{
				base.NetworkManager.LogWarning(networkObject.name + " was spawned but it's recommended to not spawn objects for connections until they have loaded start scenes. You can be notified when a connection loads start scenes by using connection.OnLoadedStartScenes on the connection, or SceneManager.OnClientLoadStartScenes.");
			}
			if (networkObject.IsSpawned)
			{
				base.NetworkManager.LogWarning(networkObject.name + " is already spawned.");
				return;
			}
			if (networkObject.CurrentParentNetworkObject != null && !networkObject.CurrentParentNetworkObject.IsSpawned)
			{
				base.NetworkManager.LogError($"{networkObject.name} cannot be spawned because it has a parent NetworkObject {networkObject.CurrentParentNetworkObject} which is not spawned.");
				return;
			}
			if (scene.IsValid())
			{
				if (networkObject.transform.parent != null)
				{
					base.NetworkManager.LogError($"{networkObject.name} cannot be moved to scene name {scene.name}, handle {scene.handle} because {networkObject.name} is not root and only root objects may be moved.");
					return;
				}
				UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(networkObject.gameObject, scene);
			}
			if (flag)
			{
				base.NetworkManager.ClientManager.Objects.PredictedSpawn(networkObject, ownerConnection);
			}
			else
			{
				SpawnWithoutChecks(networkObject, ownerConnection);
			}
		}

		private void SpawnWithoutChecks(NetworkObject networkObject, NetworkConnection ownerConnection = null, int? objectId = null)
		{
			networkObject.SetIsNetworked(value: true);
			_spawnCache.Add(networkObject);
			SetupWithoutSynchronization(networkObject, ownerConnection, objectId);
			foreach (NetworkObject childNetworkObject in networkObject.ChildNetworkObjects)
			{
				if (childNetworkObject.gameObject.activeInHierarchy || childNetworkObject.State == NetworkObjectState.Spawned)
				{
					SpawnWithoutChecks(childNetworkObject, ownerConnection);
				}
			}
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList();
			list.AddRange(_spawnCache);
			_spawnCache.Clear();
			RebuildObservers(list);
			int count = list.Count;
			if (base.NetworkManager.IsClient)
			{
				int num = count;
				for (int i = 0; i < num; i++)
				{
					list[i].SetRenderersVisible(networkObject.Observers.Contains(base.NetworkManager.ClientManager.Connection));
				}
			}
			CollectionCaches<NetworkObject>.Store(list);
		}

		internal void ReadPredictedSpawn(PooledReader reader, NetworkConnection conn)
		{
			sbyte initializeOrder;
			ushort collectionid;
			bool spawned;
			int num = reader.ReadNetworkObjectForSpawn(out initializeOrder, out collectionid, out spawned);
			if (!conn.PredictedObjectIds.Contains(num))
			{
				reader.Clear();
				conn.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection {conn.ClientId} used predicted spawning with a non-reserved objectId of {num}.");
				return;
			}
			NetworkConnection networkConnection = reader.ReadNetworkConnection();
			SpawnType whole = (SpawnType)reader.ReadByte();
			reader.ReadByte();
			ReadTransformProperties(reader, out var localPosition, out var localRotation, out var localScale);
			bool isGlobal = false;
			NetworkObject nob;
			if (SpawnTypeEnum.Contains(whole, SpawnType.Scene))
			{
				ulong sceneId = reader.ReadUInt64(AutoPackType.Unpacked);
				nob = GetSceneNetworkObject(sceneId);
				if (!CanPredictedSpawn(nob, conn, networkConnection, asServer: true))
				{
					return;
				}
			}
			else
			{
				reader.ReadByte();
				int num2 = reader.ReadNetworkObjectId();
				if (num2 == 65535)
				{
					reader.Clear();
					conn.Kick(KickReason.UnusualActivity, LoggingType.Common, $"Spawned object has an invalid prefabId of {num2}. Make sure all objects which are being spawned over the network are within SpawnableObjects on the NetworkManager. Connection {conn.ClientId} will be kicked immediately.");
					return;
				}
				PrefabObjects prefabObjects = base.NetworkManager.GetPrefabObjects<PrefabObjects>(collectionid, createIfMissing: false);
				if (prefabObjects == null)
				{
					reader.Clear();
					conn.Kick(KickReason.UnusualActivity, LoggingType.Common, $"PrefabObjects collection is not found for CollectionId {collectionid}. Be sure to add your addressables NetworkObject prefabs to the collection on server and client before attempting to spawn them over the network. Connection {conn.ClientId} will be kicked immediately.");
					return;
				}
				NetworkObject nob2 = prefabObjects.GetObject(asServer: true, num2);
				if (!CanPredictedSpawn(nob2, conn, networkConnection, asServer: true))
				{
					return;
				}
				nob = base.NetworkManager.GetPooledInstantiated(num2, collectionid, asServer: false);
				isGlobal = SpawnTypeEnum.Contains(whole, SpawnType.InstantiatedGlobal);
			}
			Transform transform = nob.transform;
			transform.SetParent(null, worldPositionStays: true);
			GetTransformProperties(localPosition, localRotation, localScale, transform, out var pos, out var rot, out var scale);
			transform.SetLocalPositionRotationAndScale(pos, rot, scale);
			nob.SetIsGlobal(isGlobal);
			nob.InitializePredictedObject_Server(base.NetworkManager, conn);
			if (nob.AllowPredictedSyncTypes)
			{
				PooledReader pooledReader = ReaderPool.Retrieve(reader.ReadArraySegmentAndSize(), base.NetworkManager);
				NetworkBehaviour[] networkBehaviours = nob.NetworkBehaviours;
				foreach (NetworkBehaviour obj in networkBehaviours)
				{
					int length = pooledReader.ReadInt32();
					obj.OnSyncType(pooledReader, length, isSyncObject: false, asServer: true);
					length = pooledReader.ReadInt32();
					obj.OnSyncType(pooledReader, length, isSyncObject: true, asServer: true);
				}
				pooledReader.Store();
			}
			SpawnWithoutChecks(nob, networkConnection, num);
			WriteResponse(success: true);
			void WriteResponse(bool success)
			{
				PooledWriter pooledWriter = WriterPool.Retrieve();
				pooledWriter.WritePacketId(PacketId.PredictedSpawnResult);
				pooledWriter.WriteNetworkObjectId(nob.ObjectId);
				pooledWriter.WriteBoolean(success);
				if (success)
				{
					Queue<int> objectIdCache = base.NetworkManager.ServerManager.Objects.GetObjectIdCache();
					int num3 = 65535;
					int num4 = ((objectIdCache.Count > 0) ? objectIdCache.Dequeue() : num3);
					pooledWriter.WriteNetworkObjectId(num4);
					if (num4 != num3)
					{
						conn.PredictedObjectIds.Enqueue(num4);
					}
				}
				conn.SendToClient(0, pooledWriter.GetArraySegment());
			}
		}

		private void CleanRecentlyDespawned()
		{
			if (!base.NetworkManager.TimeManager.FrameTicked)
			{
				return;
			}
			List<int> list = CollectionCaches<int>.RetrieveList();
			uint cleanRecentlyDespawnedMaxTicks = _cleanRecentlyDespawnedMaxTicks;
			uint localTick = base.NetworkManager.TimeManager.LocalTick;
			int num = Mathf.Max(20, (int)((float)RecentlyDespawnedIds.Count * 0.05f));
			int num2 = 0;
			foreach (KeyValuePair<int, uint> recentlyDespawnedId in RecentlyDespawnedIds)
			{
				if ((long)(localTick - recentlyDespawnedId.Value) > (long)cleanRecentlyDespawnedMaxTicks)
				{
					list.Add(recentlyDespawnedId.Key);
				}
				num2++;
				if (num2 == num)
				{
					break;
				}
			}
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				RecentlyDespawnedIds.Remove(list[i]);
			}
			CollectionCaches<int>.Store(list);
		}

		public bool RecentlyDespawned(int objectId, uint ticks)
		{
			if (!RecentlyDespawnedIds.TryGetValue(objectId, out var value))
			{
				return false;
			}
			return base.NetworkManager.TimeManager.LocalTick - value <= ticks;
		}

		internal void AddToPending(NetworkObject nob)
		{
			_pendingDestroy[nob.ObjectId] = nob;
		}

		internal bool RemoveFromPending(int objectId)
		{
			return _pendingDestroy.Remove(objectId);
		}

		internal NetworkObject GetFromPending(int objectId)
		{
			_pendingDestroy.TryGetValue(objectId, out var value);
			return value;
		}

		internal void DestroyPending()
		{
			foreach (NetworkObject value in _pendingDestroy.Values)
			{
				if (value != null)
				{
					UnityEngine.Object.Destroy(value.gameObject);
				}
			}
			_pendingDestroy.Clear();
		}

		internal override void Despawn(NetworkObject networkObject, DespawnType despawnType, bool asServer)
		{
			bool flag = false;
			if (networkObject == null)
			{
				base.NetworkManager.LogWarning("NetworkObject cannot be despawned because it is null.");
				return;
			}
			if (networkObject.IsDeinitializing)
			{
				base.NetworkManager.LogWarning("Object " + networkObject.name + " cannot be despawned because it is already deinitializing.");
				return;
			}
			if (!base.NetworkManager.ServerManager.Started)
			{
				if (!base.NetworkManager.ClientManager.Started)
				{
					base.NetworkManager.LogWarning("Cannot despawn object because server nor client are active.");
					return;
				}
				if (!base.NetworkManager.PredictionManager.GetAllowPredictedSpawning())
				{
					base.NetworkManager.LogWarning("Cannot despawn object because server is not active and predicted spawning is not enabled.");
					return;
				}
				if (!CanPredictedDespawn(networkObject, base.NetworkManager.ClientManager.Connection, asServer: false))
				{
					return;
				}
				flag = true;
			}
			if (!networkObject.gameObject.scene.IsValid())
			{
				base.NetworkManager.LogError(networkObject.name + " is a prefab. You must instantiate the prefab first, then use Spawn on the instantiated copy.");
				return;
			}
			if (flag)
			{
				base.NetworkManager.ClientManager.Objects.PredictedDespawn(networkObject);
				return;
			}
			FinalizeDespawn(networkObject, despawnType);
			RecentlyDespawnedIds[networkObject.ObjectId] = base.NetworkManager.TimeManager.LocalTick;
			base.Despawn(networkObject, despawnType, asServer);
		}

		internal override void NetworkObjectUnexpectedlyDestroyed(NetworkObject nob, bool asServer)
		{
			FinalizeDespawn(nob, DespawnType.Destroy);
			base.NetworkObjectUnexpectedlyDestroyed(nob, asServer);
		}

		private void FinalizeDespawn(NetworkObject nob, DespawnType despawnType)
		{
			if (!(nob != null) || nob.ObjectId == 65535)
			{
				return;
			}
			nob.WriteDirtySyncTypes();
			List<NetworkBehaviour> dirtySyncObjectBehaviours = _dirtySyncObjectBehaviours;
			List<NetworkBehaviour> dirtySyncVarBehaviours = _dirtySyncVarBehaviours;
			NetworkBehaviour[] networkBehaviours = nob.NetworkBehaviours;
			foreach (NetworkBehaviour networkBehaviour in networkBehaviours)
			{
				if (networkBehaviour.SyncObjectDirty)
				{
					dirtySyncObjectBehaviours.Remove(networkBehaviour);
				}
				if (networkBehaviour.SyncVarDirty)
				{
					dirtySyncVarBehaviours.Remove(networkBehaviour);
				}
			}
			WriteDespawnAndSend(nob, despawnType);
			CacheObjectId(nob);
		}

		private void WriteDespawnAndSend(NetworkObject nob, DespawnType despawnType)
		{
			PooledWriter pooledWriter = WriterPool.Retrieve();
			WriteDespawn(nob, despawnType, pooledWriter);
			ArraySegment<byte> arraySegment = pooledWriter.GetArraySegment();
			List<NetworkConnection> list = CollectionCaches<NetworkConnection>.RetrieveList();
			list.AddRange(nob.Observers);
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				NetworkConnection networkConnection = list[i];
				nob.InvokeOnServerDespawn(networkConnection);
				base.NetworkManager.TransportManager.SendToClient(0, arraySegment, networkConnection);
			}
			pooledWriter.Store();
			CollectionCaches<NetworkConnection>.Store(list);
		}

		internal void ReadPredictedDespawn(Reader reader, NetworkConnection conn)
		{
			NetworkObject networkObject = reader.ReadNetworkObject();
			if (networkObject == null)
			{
				reader.Clear();
				return;
			}
			if (!networkObject.AllowPredictedDespawning)
			{
				reader.Clear();
				conn.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection {conn.ClientId} used predicted despawning for object {networkObject.name} when it does not support predicted despawning.");
			}
			networkObject.Despawn();
		}

		private void Observers_OnUpdate()
		{
			UpdateTimedObservers();
		}

		private void UpdateTimedObservers()
		{
			if (!base.NetworkManager.IsServer || !base.NetworkManager.TimeManager.FrameTicked)
			{
				return;
			}
			int count = _timedNetworkObservers.Count;
			if (count == 0)
			{
				return;
			}
			double num = 1.0 + (double)(float)((double)base.NetworkManager.ServerManager.Clients.Count * 0.005 + (double)_timedNetworkObservers.Count * 0.0005);
			double time = 0.5 * num;
			uint num2 = base.NetworkManager.TimeManager.TimeToTicks(time, TickRounding.RoundUp);
			int num3 = Mathf.CeilToInt((float)count / (float)num2);
			if (num3 > _timedNetworkObservers.Count)
			{
				num3 = _timedNetworkObservers.Count;
			}
			List<NetworkConnection> list = RetrieveAuthenticatedConnections();
			List<NetworkObject> list2 = CollectionCaches<NetworkObject>.RetrieveList();
			for (int i = 0; i < num3; i++)
			{
				if (_nextTimedObserversIndex >= _timedNetworkObservers.Count)
				{
					_nextTimedObserversIndex = 0;
				}
				list2.Add(_timedNetworkObservers[_nextTimedObserversIndex++]);
			}
			RebuildObservers(list2, list, timedOnly: true);
			CollectionCaches<NetworkConnection>.Store(list);
			CollectionCaches<NetworkObject>.Store(list2);
		}

		public void AddTimedNetworkObserver(NetworkObject networkObject)
		{
			_timedNetworkObservers.Add(networkObject);
		}

		public void RemoveTimedNetworkObserver(NetworkObject networkObject)
		{
			_timedNetworkObservers.Remove(networkObject);
		}

		private List<NetworkConnection> RetrieveAuthenticatedConnections()
		{
			List<NetworkConnection> list = CollectionCaches<NetworkConnection>.RetrieveList();
			foreach (NetworkConnection value in base.NetworkManager.ServerManager.Clients.Values)
			{
				if (value.Authenticated)
				{
					list.Add(value);
				}
			}
			return list;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private List<NetworkObject> RetrieveOrderedSpawnedObjects()
		{
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList();
			bool initializationOrderChanged = false;
			foreach (NetworkObject value in Spawned.Values)
			{
				OrderRootByInitializationOrder(value, list, ref initializationOrderChanged);
			}
			OrderNestedByInitializationOrder(list);
			return list;
		}

		private void OrderRootByInitializationOrder(NetworkObject nob, List<NetworkObject> cache, ref bool initializationOrderChanged)
		{
			if (nob.IsNested)
			{
				return;
			}
			sbyte initializeOrder = nob.GetInitializeOrder();
			initializationOrderChanged |= initializeOrder != 0;
			int count = cache.Count;
			if (!initializationOrderChanged || count == 0)
			{
				cache.Add(nob);
				return;
			}
			if (initializeOrder >= cache[count - 1].GetInitializeOrder())
			{
				cache.Add(nob);
				return;
			}
			for (int i = 0; i < count; i++)
			{
				if (initializeOrder <= cache[i].GetInitializeOrder())
				{
					cache.Insert(i, nob);
					break;
				}
			}
		}

		private void OrderNestedByInitializationOrder(List<NetworkObject> cache)
		{
			for (int i = 0; i < cache.Count; i++)
			{
				NetworkObject networkObject = cache[i];
				if (!networkObject.IsNested)
				{
					int index = i;
					AddChildNetworkObjects(networkObject, ref index);
				}
			}
			void AddChildNetworkObjects(NetworkObject n, ref int reference)
			{
				foreach (NetworkObject childNetworkObject in n.ChildNetworkObjects)
				{
					cache.Insert(++reference, childNetworkObject);
					AddChildNetworkObjects(childNetworkObject, ref reference);
				}
			}
		}

		private void RemoveFromObserversWithoutSynchronization(NetworkConnection connection)
		{
			List<NetworkObject> observerChangedObjectsCache = _observerChangedObjectsCache;
			foreach (NetworkObject value in Spawned.Values)
			{
				if (value.RemoveObserver(connection))
				{
					observerChangedObjectsCache.Add(value);
				}
			}
			for (int i = 0; i < observerChangedObjectsCache.Count; i++)
			{
				observerChangedObjectsCache[i].InvokeOnServerDespawn(connection);
			}
			observerChangedObjectsCache.Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(bool timedOnly = false)
		{
			List<NetworkObject> list = RetrieveOrderedSpawnedObjects();
			List<NetworkConnection> list2 = RetrieveAuthenticatedConnections();
			RebuildObservers(list, list2, timedOnly);
			CollectionCaches<NetworkObject>.Store(list);
			CollectionCaches<NetworkConnection>.Store(list2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(NetworkObject nob, bool timedOnly = false)
		{
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList(nob);
			List<NetworkConnection> list2 = RetrieveAuthenticatedConnections();
			RebuildObservers(list, list2, timedOnly);
			CollectionCaches<NetworkObject>.Store(list);
			CollectionCaches<NetworkConnection>.Store(list2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(NetworkConnection connection, bool timedOnly = false)
		{
			List<NetworkObject> list = RetrieveOrderedSpawnedObjects();
			List<NetworkConnection> list2 = CollectionCaches<NetworkConnection>.RetrieveList(connection);
			RebuildObservers(list, list2, timedOnly);
			CollectionCaches<NetworkObject>.Store(list);
			CollectionCaches<NetworkConnection>.Store(list2);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use RebuildObservers IList variant instead.")]
		public void RebuildObservers(IEnumerable<NetworkObject> nobs, bool timedOnly = false)
		{
			List<NetworkConnection> list = RetrieveAuthenticatedConnections();
			RebuildObservers(nobs, list, timedOnly);
			CollectionCaches<NetworkConnection>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use RebuildObservers IList variant instead.")]
		public void RebuildObservers(IEnumerable<NetworkConnection> connections, bool timedOnly = false)
		{
			List<NetworkObject> list = RetrieveOrderedSpawnedObjects();
			RebuildObservers(list, connections, timedOnly);
			CollectionCaches<NetworkObject>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use RebuildObservers IList variant instead.")]
		public void RebuildObservers(IEnumerable<NetworkObject> nobs, NetworkConnection conn, bool timedOnly = false)
		{
			List<NetworkConnection> list = CollectionCaches<NetworkConnection>.RetrieveList(conn);
			RebuildObservers(nobs, list, timedOnly);
			CollectionCaches<NetworkConnection>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use RebuildObservers IList variant instead.")]
		public void RebuildObservers(NetworkObject networkObject, IEnumerable<NetworkConnection> connections, bool timedOnly = false)
		{
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList(networkObject);
			RebuildObservers(list, connections, timedOnly);
			CollectionCaches<NetworkObject>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Obsolete("Use RebuildObservers IList variant instead.")]
		public void RebuildObservers(IEnumerable<NetworkObject> nobs, IEnumerable<NetworkConnection> conns, bool timedOnly = false)
		{
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList();
			foreach (NetworkConnection conn in conns)
			{
				list.Clear();
				foreach (NetworkObject nob in nobs)
				{
					RebuildObservers(nob, conn, list, timedOnly);
				}
				if (_writer.Length <= 0)
				{
					continue;
				}
				base.NetworkManager.TransportManager.SendToClient(0, _writer.GetArraySegment(), conn);
				_writer.Reset();
				foreach (NetworkObject item in list)
				{
					item.OnSpawnServer(conn);
				}
			}
			CollectionCaches<NetworkObject>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(IList<NetworkObject> nobs, bool timedOnly = false)
		{
			List<NetworkConnection> list = RetrieveAuthenticatedConnections();
			RebuildObservers(nobs, list, timedOnly);
			CollectionCaches<NetworkConnection>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(IList<NetworkConnection> connections, bool timedOnly = false)
		{
			List<NetworkObject> list = RetrieveOrderedSpawnedObjects();
			RebuildObservers(list, connections, timedOnly);
			CollectionCaches<NetworkObject>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(IList<NetworkObject> nobs, NetworkConnection conn, bool timedOnly = false)
		{
			List<NetworkConnection> list = CollectionCaches<NetworkConnection>.RetrieveList(conn);
			RebuildObservers(nobs, list, timedOnly);
			CollectionCaches<NetworkConnection>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(NetworkObject networkObject, IList<NetworkConnection> connections, bool timedOnly = false)
		{
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList(networkObject);
			RebuildObservers(list, connections, timedOnly);
			CollectionCaches<NetworkObject>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(IList<NetworkObject> nobs, IList<NetworkConnection> conns, bool timedOnly = false)
		{
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList();
			int count = conns.Count;
			for (int i = 0; i < count; i++)
			{
				list.Clear();
				NetworkConnection networkConnection = conns[i];
				int count2 = nobs.Count;
				for (int j = 0; j < count2; j++)
				{
					RebuildObservers(nobs[j], networkConnection, list, timedOnly);
				}
				if (_writer.Length <= 0)
				{
					continue;
				}
				base.NetworkManager.TransportManager.SendToClient(0, _writer.GetArraySegment(), networkConnection);
				_writer.Reset();
				foreach (NetworkObject item in list)
				{
					item.OnSpawnServer(networkConnection);
				}
			}
			CollectionCaches<NetworkObject>.Store(list);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void RebuildObservers(NetworkObject nob, NetworkConnection conn, bool timedOnly = false)
		{
			if (ApplicationState.IsQuitting())
			{
				return;
			}
			_writer.Reset();
			conn.UpdateHashGridPositions(!timedOnly);
			ObserverStateChange observerStateChange = nob.RebuildObservers(conn, timedOnly);
			switch (observerStateChange)
			{
			case ObserverStateChange.Added:
				WriteSpawn_Server(nob, conn, _writer);
				break;
			case ObserverStateChange.Removed:
			{
				if (conn.LevelOfDetails.TryGetValue(nob, out var value))
				{
					ObjectCaches<NetworkConnection.LevelOfDetailData>.Store(value);
				}
				conn.LevelOfDetails.Remove(nob);
				WriteDespawn(nob, nob.GetDefaultDespawnType(), _writer);
				break;
			}
			default:
				return;
			}
			base.NetworkManager.TransportManager.SendToClient(0, _writer.GetArraySegment(), conn);
			if (observerStateChange == ObserverStateChange.Added)
			{
				nob.OnSpawnServer(conn);
			}
			foreach (NetworkObject runtimeChildNetworkObject in nob.RuntimeChildNetworkObjects)
			{
				RebuildObservers(runtimeChildNetworkObject, conn, timedOnly);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void RebuildObservers(NetworkObject nob, NetworkConnection conn, List<NetworkObject> addedNobs, bool timedOnly = false)
		{
			if (ApplicationState.IsQuitting())
			{
				return;
			}
			conn.UpdateHashGridPositions(!timedOnly);
			switch (nob.RebuildObservers(conn, timedOnly))
			{
			case ObserverStateChange.Added:
				WriteSpawn_Server(nob, conn, _writer);
				addedNobs.Add(nob);
				break;
			case ObserverStateChange.Removed:
			{
				if (conn.LevelOfDetails.TryGetValue(nob, out var value))
				{
					ObjectCaches<NetworkConnection.LevelOfDetailData>.Store(value);
				}
				conn.LevelOfDetails.Remove(nob);
				WriteDespawn(nob, nob.GetDefaultDespawnType(), _writer);
				break;
			}
			default:
				return;
			}
			foreach (NetworkObject runtimeChildNetworkObject in nob.RuntimeChildNetworkObjects)
			{
				RebuildObservers(runtimeChildNetworkObject, conn, addedNobs, timedOnly);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ParseServerRpc(PooledReader reader, NetworkConnection conn, Channel channel)
		{
			NetworkBehaviour networkBehaviour = reader.ReadNetworkBehaviour();
			int packetLength = Packets.GetPacketLength(8, reader, channel);
			if (networkBehaviour != null)
			{
				networkBehaviour.OnServerRpc(reader, conn, channel);
			}
			else
			{
				SkipDataLength(8, reader, packetLength);
			}
		}
	}
}
