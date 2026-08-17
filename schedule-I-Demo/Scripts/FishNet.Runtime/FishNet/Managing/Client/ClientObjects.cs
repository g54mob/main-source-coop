using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Connection;
using FishNet.Documenting;
using FishNet.Managing.Object;
using FishNet.Managing.Server;
using FishNet.Managing.Utility;
using FishNet.Object;
using FishNet.Object.Helping;
using FishNet.Serializing;
using FishNet.Transporting;
using FishNet.Utility.Extension;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Managing.Client
{
	public class ClientObjects : ManagedObjects
	{
		internal List<NetworkObject> LocalClientSpawned = new List<NetworkObject>();

		private ClientObjectCache _objectCache;

		private Dictionary<ushort, RpcLink> _rpcLinks = new Dictionary<ushort, RpcLink>();

		internal ClientObjects(NetworkManager networkManager)
		{
			base.Initialize(networkManager);
			_objectCache = new ClientObjectCache(this, networkManager);
		}

		internal void OnServerConnectionState(ServerConnectionStateArgs args)
		{
			if (args.ConnectionState != LocalConnectionState.Started && base.NetworkManager.IsClient && args.TransportIndex == base.NetworkManager.ClientManager.GetTransportIndex())
			{
				base.NetworkManager.ClientManager.StopConnection();
			}
		}

		internal void OnClientConnectionState(ClientConnectionStateArgs args)
		{
			if (args.ConnectionState == LocalConnectionState.Started)
			{
				return;
			}
			_objectCache.Reset();
			if (!base.NetworkManager.IsServer)
			{
				base.DespawnWithoutSynchronization(asServer: false);
			}
			else
			{
				foreach (NetworkObject value in Spawned.Values)
				{
					value.InvokeStopCallbacks(asServer: false);
					value.SetInitializedStatus(isInitialized: false, asServer: false);
				}
			}
			Spawned.Clear();
			SceneObjects_Internal.Clear();
			LocalClientSpawned.Clear();
		}

		[APIExclude]
		protected internal override void SceneManager_sceneLoaded(Scene s, LoadSceneMode arg1)
		{
			base.SceneManager_sceneLoaded(s, arg1);
			if (base.NetworkManager.IsClient)
			{
				RegisterAndDespawnSceneObjects(s);
			}
		}

		internal override void AddToSpawned(NetworkObject nob, bool asServer)
		{
			LocalClientSpawned.Add(nob);
			base.AddToSpawned(nob, asServer);
			if (base.NetworkManager.IsServer)
			{
				nob.SetRenderersVisible(visible: true);
			}
		}

		protected override void RemoveFromSpawned(NetworkObject nob, bool unexpectedlyDestroyed, bool asServer)
		{
			LocalClientSpawned.Remove(nob);
			base.RemoveFromSpawned(nob, unexpectedlyDestroyed, asServer);
		}

		internal void PredictedSpawn(NetworkObject networkObject, NetworkConnection ownerConnection)
		{
			Queue<int> predictedObjectIds = base.NetworkManager.ClientManager.Connection.PredictedObjectIds;
			if (predictedObjectIds.Count == 0)
			{
				base.NetworkManager.LogError("Predicted spawn for object " + networkObject.name + " failed because no more predicted ObjectIds remain. This usually occurs when the client is spawning excessively before the server can respond. Increasing ReservedObjectIds within the ServerManager component or reducing spawn rate could prevent this problem.");
				return;
			}
			networkObject.PreinitializePredictedObject_Client(base.NetworkManager, predictedObjectIds.Dequeue(), ownerConnection, base.NetworkManager.ClientManager.Connection);
			base.NetworkManager.ClientManager.Objects.AddToSpawned(networkObject, asServer: false);
			networkObject.Initialize(asServer: false, invokeSyncTypeCallbacks: true);
			PooledWriter pooledWriter = WriterPool.Retrieve();
			WriteSpawn(networkObject, pooledWriter);
			base.NetworkManager.TransportManager.SendToServer(0, pooledWriter.GetArraySegment());
			pooledWriter.Store();
		}

		public void WriteSpawn(NetworkObject nob, Writer writer)
		{
			PooledWriter pooledWriter = WriterPool.Retrieve();
			pooledWriter.WritePacketId(PacketId.ObjectSpawn);
			pooledWriter.WriteNetworkObjectForSpawn(nob);
			pooledWriter.WriteNetworkConnection(nob.Owner);
			bool isSceneObject = nob.IsSceneObject;
			SpawnType spawnType = SpawnType.Unset;
			spawnType = ((!isSceneObject) ? ((SpawnType)((uint)spawnType | (uint)(nob.IsGlobal ? 8 : 4))) : (spawnType | SpawnType.Scene));
			pooledWriter.WriteByte((byte)spawnType);
			pooledWriter.WriteByte(nob.ComponentIndex);
			WriteChangedTransformProperties(nob, isSceneObject, nested: false, pooledWriter);
			if (isSceneObject)
			{
				pooledWriter.WriteUInt64(nob.SceneId, AutoPackType.Unpacked);
			}
			else
			{
				pooledWriter.WriteByte(0);
				pooledWriter.WriteNetworkObjectId(nob.PrefabId);
			}
			writer.WriteBytes(pooledWriter.GetBuffer(), 0, pooledWriter.Length);
			if (nob.AllowPredictedSyncTypes)
			{
				PooledWriter pooledWriter2 = WriterPool.Retrieve();
				NetworkBehaviour[] networkBehaviours = nob.NetworkBehaviours;
				for (int i = 0; i < networkBehaviours.Length; i++)
				{
					networkBehaviours[i].WriteSyncTypesForSpawn(pooledWriter2, null);
				}
				writer.WriteBytesAndSize(pooledWriter2.GetBuffer(), 0, pooledWriter2.Length);
				pooledWriter2.Store();
			}
			pooledWriter.Store();
		}

		internal void PredictedDespawn(NetworkObject networkObject)
		{
			PooledWriter pooledWriter = WriterPool.Retrieve();
			WriteDepawn(networkObject, pooledWriter);
			base.NetworkManager.TransportManager.SendToServer(0, pooledWriter.GetArraySegment());
			pooledWriter.Store();
			networkObject.DeinitializePredictedObject_Client();
		}

		public void WriteDepawn(NetworkObject nob, Writer writer)
		{
			writer.WritePacketId(PacketId.ObjectDespawn);
			writer.WriteNetworkObject(nob);
		}

		internal void RegisterAndDespawnSceneObjects()
		{
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				RegisterAndDespawnSceneObjects(SceneManager.GetSceneAt(i));
			}
		}

		private void RegisterAndDespawnSceneObjects(Scene s)
		{
			List<NetworkObject> result = CollectionCaches<NetworkObject>.RetrieveList();
			Scenes.GetSceneNetworkObjects(s, firstOnly: false, errorOnDuplicates: true, ref result);
			int count = result.Count;
			for (int i = 0; i < count; i++)
			{
				NetworkObject networkObject = result[i];
				if (!networkObject.IsSceneObject)
				{
					continue;
				}
				UpdateNetworkBehavioursForSceneObject(networkObject, asServer: false);
				if (networkObject.IsNetworked && networkObject.IsNetworked)
				{
					AddToSceneObjects(networkObject);
					if (!base.NetworkManager.IsServer)
					{
						networkObject.gameObject.SetActive(value: false);
					}
				}
			}
			CollectionCaches<NetworkObject>.Store(result);
		}

		internal override void NetworkObjectUnexpectedlyDestroyed(NetworkObject nob, bool asServer)
		{
			nob.RemoveClientRpcLinkIndexes();
			base.NetworkObjectUnexpectedlyDestroyed(nob, asServer);
		}

		internal void ParseOwnershipChange(PooledReader reader)
		{
			NetworkObject networkObject = reader.ReadNetworkObject();
			NetworkConnection newOwner = reader.ReadNetworkConnection();
			if (networkObject != null)
			{
				networkObject.GiveOwnership(newOwner, asServer: false);
			}
			else
			{
				base.NetworkManager.LogWarning("NetworkBehaviour could not be found when trying to parse OwnershipChange packet.");
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ParseSyncType(PooledReader reader, bool isSyncObject, Channel channel)
		{
			ushort packetId = (ushort)(isSyncObject ? 13 : 7);
			NetworkBehaviour networkBehaviour = reader.ReadNetworkBehaviour();
			int packetLength = Packets.GetPacketLength(packetId, reader, channel);
			if (networkBehaviour != null)
			{
				if (packetLength > 0)
				{
					networkBehaviour.OnSyncType(reader, packetLength, isSyncObject);
				}
			}
			else
			{
				SkipDataLength(packetId, reader, packetLength);
			}
		}

		internal void ParsePredictedSpawnResult(Reader reader)
		{
			reader.ReadNetworkObjectId();
			if (reader.ReadBoolean())
			{
				int num = reader.ReadNetworkObjectId();
				if (num != 65535)
				{
					base.NetworkManager.ClientManager.Connection.PredictedObjectIds.Enqueue(num);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ParseReconcileRpc(PooledReader reader, Channel channel)
		{
			NetworkBehaviour networkBehaviour = reader.ReadNetworkBehaviour();
			int packetLength = Packets.GetPacketLength(16, reader, channel);
			if (networkBehaviour != null)
			{
				networkBehaviour.OnReconcileRpc(null, reader, channel);
			}
			else
			{
				SkipDataLength(9, reader, packetLength);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ParseObserversRpc(PooledReader reader, Channel channel)
		{
			NetworkBehaviour networkBehaviour = reader.ReadNetworkBehaviour();
			int packetLength = Packets.GetPacketLength(9, reader, channel);
			if (networkBehaviour != null)
			{
				networkBehaviour.OnObserversRpc(null, reader, channel);
			}
			else
			{
				SkipDataLength(9, reader, packetLength);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ParseTargetRpc(PooledReader reader, Channel channel)
		{
			NetworkBehaviour networkBehaviour = reader.ReadNetworkBehaviour();
			int packetLength = Packets.GetPacketLength(10, reader, channel);
			if (networkBehaviour != null)
			{
				networkBehaviour.OnTargetRpc(null, reader, channel);
			}
			else
			{
				SkipDataLength(10, reader, packetLength);
			}
		}

		internal void CacheSpawn(PooledReader reader)
		{
			sbyte initializeOrder;
			ushort collectionid;
			bool spawned;
			int num = reader.ReadNetworkObjectForSpawn(out initializeOrder, out collectionid, out spawned);
			int ownerId = reader.ReadNetworkConnectionId();
			SpawnType spawnType = (SpawnType)reader.ReadByte();
			byte componentIndex = reader.ReadByte();
			ReadTransformProperties(reader, out var localPosition, out var localRotation, out var localScale);
			int rootObjectId = (SpawnTypeEnum.Contains(spawnType, SpawnType.Nested) ? reader.ReadNetworkObjectId() : 0);
			bool num2 = SpawnTypeEnum.Contains(spawnType, SpawnType.Scene);
			int? parentObjectId = null;
			byte? parentComponentIndex = null;
			int? prefabId = null;
			ulong sceneId = 0uL;
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (num2)
			{
				ReadSceneObject(reader, out sceneId);
			}
			else
			{
				ReadSpawnedObject(reader, out parentObjectId, out parentComponentIndex, out prefabId);
			}
			ArraySegment<byte> arraySegment = reader.ReadArraySegmentAndSize();
			ArraySegment<byte> syncValues = reader.ReadArraySegmentAndSize();
			if (!base.NetworkManager.IsServerOnly && Spawned.TryGetValue(num, out var value))
			{
				if (!value.PredictedSpawner.IsValid)
				{
					base.NetworkManager.LogWarning($"Received a spawn objectId of {num} which was already found in spawned, and was not predicted. This sometimes may occur on clientHost when the server destroys an object unexpectedly before the clientHost gets the spawn message.");
					return;
				}
				PooledReader pooledReader = ReaderPool.Retrieve(arraySegment, base.NetworkManager);
				ApplyRpcLinks(value, pooledReader);
				pooledReader.Store();
			}
			else
			{
				_objectCache.AddSpawn(base.NetworkManager, collectionid, num, initializeOrder, ownerId, spawnType, componentIndex, rootObjectId, parentObjectId, parentComponentIndex, prefabId, localPosition, localRotation, localScale, sceneId, empty, empty2, arraySegment, syncValues);
			}
		}

		internal void CacheDespawn(PooledReader reader)
		{
			DespawnType dt;
			int objectId = reader.ReadNetworkObjectForDepawn(out dt);
			_objectCache.AddDespawn(objectId, dt);
		}

		internal void IterateObjectCache()
		{
			_objectCache.Iterate();
		}

		internal NetworkObject GetNestedNetworkObject(CachedNetworkObject cnob)
		{
			int rootObjectId = cnob.RootObjectId;
			byte componentIndex = cnob.ComponentIndex;
			NetworkObject spawnedObject = _objectCache.GetSpawnedObject(rootObjectId);
			if (spawnedObject == null)
			{
				base.NetworkManager.LogError($"Nested spawned object with componentIndex of {componentIndex} and a parentId of {rootObjectId} could not be spawned because parent was not found.");
				return null;
			}
			NetworkObject networkObject = null;
			List<NetworkObject> childNetworkObjects = spawnedObject.ChildNetworkObjects;
			for (int i = 0; i < childNetworkObjects.Count; i++)
			{
				if (childNetworkObjects[i].ComponentIndex == componentIndex)
				{
					networkObject = childNetworkObjects[i];
					break;
				}
			}
			if (networkObject == null)
			{
				base.NetworkManager.LogError($"Nested spawned object with componentIndex of {componentIndex} could not be found as a child NetworkObject of {spawnedObject.name}.");
				return null;
			}
			return networkObject;
		}

		internal void ApplyRpcLinks(NetworkObject nob, Reader reader)
		{
			List<ushort> list = new List<ushort>();
			NetworkBehaviour[] networkBehaviours = nob.NetworkBehaviours;
			foreach (NetworkBehaviour networkBehaviour in networkBehaviours)
			{
				int num = reader.ReadInt32();
				int position = reader.Position;
				while (reader.Position - position < num)
				{
					ushort num2 = reader.ReadUInt16();
					RpcLink link = new RpcLink(nob.ObjectId, networkBehaviour.ComponentIndex, reader.ReadUInt16(), (RpcType)reader.ReadByte());
					SetRpcLink(num2, link);
					list.Add(num2);
				}
			}
			nob.SetRpcLinkIndexes(list);
		}

		internal NetworkObject GetInstantiatedNetworkObject(CachedNetworkObject cnob)
		{
			if (!cnob.PrefabId.HasValue)
			{
				base.NetworkManager.LogError($"PrefabId for {cnob.ObjectId} is null. Object will not spawn.");
				return null;
			}
			NetworkManager networkManager = base.NetworkManager;
			int value = cnob.PrefabId.Value;
			if (value == 65535)
			{
				base.NetworkManager.LogError("Spawned object has an invalid prefabId. Make sure all objects which are being spawned over the network are within SpawnableObjects on the NetworkManager.");
				return null;
			}
			ushort collectionId = cnob.CollectionId;
			PrefabObjects prefabObjects = networkManager.GetPrefabObjects<PrefabObjects>(collectionId, createIfMissing: false);
			if (prefabObjects == null && collectionId > 0)
			{
				networkManager.LogError($"PrefabObjects collection is not found for CollectionId {collectionId}. Be sure to add your addressables NetworkObject prefabs to the collection on server and client before attempting to spawn them over the network.");
				return null;
			}
			NetworkObject value3;
			if (!networkManager.IsHost)
			{
				Transform parent = null;
				if (cnob.ParentObjectId.HasValue)
				{
					int value2 = cnob.ParentObjectId.Value;
					NetworkObject spawnedObject = _objectCache.GetSpawnedObject(value2);
					if (spawnedObject == null)
					{
						NetworkObject networkObject = prefabObjects.GetObject(asServer: false, value);
						networkManager.LogError($"NetworkObject not found for ObjectId {value2}. Prefab {networkObject.name} will be instantiated without parent synchronization.");
					}
					else if (cnob.ParentIsNetworkBehaviour)
					{
						byte componentIndex = cnob.ComponentIndex;
						NetworkBehaviour networkBehaviour = spawnedObject.GetNetworkBehaviour(componentIndex, error: false);
						if (networkBehaviour != null)
						{
							parent = networkBehaviour.transform;
						}
						else
						{
							NetworkObject networkObject2 = prefabObjects.GetObject(asServer: false, value);
							networkManager.LogError($"NetworkBehaviour on index {componentIndex} could nto be found within NetworkObject {spawnedObject.name} with ObjectId {value2}. Prefab {networkObject2.name} will be instantiated without parent synchronization.");
						}
					}
					else
					{
						parent = spawnedObject.transform;
					}
				}
				value3 = networkManager.GetPooledInstantiated(value, collectionId, asServer: false);
				value3.transform.SetParent(parent, worldPositionStays: true);
				bool isGlobal = SpawnTypeEnum.Contains(cnob.SpawnType, SpawnType.InstantiatedGlobal);
				value3.SetIsGlobal(isGlobal);
			}
			else
			{
				ServerObjects objects = networkManager.ServerManager.Objects;
				if (!objects.Spawned.TryGetValueIL2CPP(cnob.ObjectId, out value3))
				{
					value3 = objects.GetFromPending(cnob.ObjectId);
				}
				if (value3 == null)
				{
					networkManager.LogWarning($"ObjectId {cnob.ObjectId} could not be found in Server spawned, nor Server pending despawn. This may occur as clientHost when objects are destroyed before the client receives a despawn packet. In most cases this may be ignored.");
				}
			}
			return value3;
		}

		internal NetworkObject GetSpawnedNetworkObject(CachedNetworkObject cnob)
		{
			if (Spawned.TryGetValueIL2CPP(cnob.ObjectId, out var value))
			{
				return value;
			}
			return _objectCache.GetInCached(cnob.ObjectId, ClientObjectCache.CacheSearchType.Any);
		}

		private void ReadSceneObject(PooledReader reader, out ulong sceneId)
		{
			sceneId = reader.ReadUInt64(AutoPackType.Unpacked);
		}

		private void ReadSpawnedObject(PooledReader reader, out int? parentObjectId, out byte? parentComponentIndex, out int? prefabId)
		{
			SpawnParentType spawnParentType = (SpawnParentType)reader.ReadByte();
			parentObjectId = null;
			parentComponentIndex = null;
			switch (spawnParentType)
			{
			case SpawnParentType.NetworkObject:
			{
				int num = reader.ReadNetworkObjectId();
				if (num != 65535)
				{
					parentObjectId = num;
				}
				break;
			}
			case SpawnParentType.NetworkBehaviour:
			{
				reader.ReadNetworkBehaviour(out var objectId, out var componentIndex, _objectCache.ReadSpawningObjects);
				if (objectId != 65535)
				{
					parentObjectId = objectId;
					parentComponentIndex = componentIndex;
				}
				break;
			}
			}
			prefabId = (ushort)reader.ReadNetworkObjectId();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ParseRpcLink(PooledReader reader, ushort index, Channel channel)
		{
			int packetLength = Packets.GetPacketLength(ushort.MaxValue, reader, channel);
			NetworkObject value2;
			if (!_rpcLinks.TryGetValueIL2CPP(index, out var value))
			{
				SkipDataLength(index, reader, packetLength);
			}
			else if (Spawned.TryGetValueIL2CPP(value.ObjectId, out value2))
			{
				NetworkBehaviour networkBehaviour = value2.NetworkBehaviours[value.ComponentIndex];
				if (value.RpcType == RpcType.Target)
				{
					networkBehaviour.OnTargetRpc(value.RpcHash, reader, channel);
				}
				else if (value.RpcType == RpcType.Observers)
				{
					networkBehaviour.OnObserversRpc(value.RpcHash, reader, channel);
				}
				else if (value.RpcType == RpcType.Reconcile)
				{
					networkBehaviour.OnReconcileRpc(value.RpcHash, reader, channel);
				}
			}
			else
			{
				SkipDataLength(index, reader, packetLength, value.ObjectId);
			}
		}

		internal void SetRpcLink(ushort linkIndex, RpcLink link)
		{
			_rpcLinks[linkIndex] = link;
		}

		internal void RemoveLinkIndexes(List<ushort> values)
		{
			if (values != null)
			{
				for (int i = 0; i < values.Count; i++)
				{
					_rpcLinks.Remove(values[i]);
				}
			}
		}
	}
}
