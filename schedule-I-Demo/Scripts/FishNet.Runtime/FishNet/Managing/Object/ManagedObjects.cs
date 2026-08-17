using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Component.Observing;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Managing.Server;
using FishNet.Managing.Utility;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Transporting;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Managing.Object
{
	public abstract class ManagedObjects
	{
		public Dictionary<int, NetworkObject> Spawned = new Dictionary<int, NetworkObject>();

		protected Dictionary<ulong, NetworkObject> SceneObjects_Internal = new Dictionary<ulong, NetworkObject>();

		private HashGrid _hashGrid;

		protected NetworkManager NetworkManager { get; private set; }

		public IReadOnlyDictionary<ulong, NetworkObject> SceneObjects => SceneObjects_Internal;

		protected internal virtual int GetNextNetworkObjectId(bool errorCheck = true)
		{
			return 65535;
		}

		protected virtual void Initialize(NetworkManager manager)
		{
			NetworkManager = manager;
			manager.TryGetInstance<HashGrid>(out _hashGrid);
		}

		internal void SubscribeToSceneLoaded(bool subscribe)
		{
			if (subscribe)
			{
				SceneManager.sceneLoaded += SceneManager_sceneLoaded;
			}
			else
			{
				SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
			}
		}

		protected internal virtual void SceneManager_sceneLoaded(Scene s, LoadSceneMode arg1)
		{
		}

		internal virtual void NetworkObjectUnexpectedlyDestroyed(NetworkObject nob, bool asServer)
		{
			if (!(nob == null))
			{
				RemoveFromSpawned(nob, unexpectedlyDestroyed: true, asServer);
			}
		}

		protected virtual void RemoveFromSpawned(NetworkObject nob, bool unexpectedlyDestroyed, bool asServer)
		{
			Spawned.Remove(nob.ObjectId);
			if (unexpectedlyDestroyed && nob.IsSceneObject)
			{
				RemoveFromSceneObjects(nob);
			}
		}

		internal virtual void Despawn(NetworkObject nob, DespawnType despawnType, bool asServer)
		{
			if (nob == null)
			{
				NetworkManager.LogWarning("Cannot despawn a null NetworkObject.");
				return;
			}
			bool flag = false;
			if (!nob.IsNested)
			{
				if (asServer)
				{
					if (!nob.IsSceneObject)
					{
						if (nob.Observers.Contains(NetworkManager.ClientManager.Connection))
						{
							NetworkManager.ServerManager.Objects.AddToPending(nob);
						}
						else
						{
							flag = true;
						}
					}
				}
				else
				{
					bool isServer = NetworkManager.IsServer;
					if (!nob.IsSceneObject)
					{
						flag = !isServer || NetworkManager.ServerManager.Objects.RemoveFromPending(nob.ObjectId);
					}
				}
			}
			nob.Deinitialize(asServer);
			if (asServer)
			{
				MatchCondition.RemoveFromMatchWithoutRebuild(nob, NetworkManager);
			}
			RemoveFromSpawned(nob, unexpectedlyDestroyed: false, asServer);
			if (flag)
			{
				if (despawnType == DespawnType.Destroy)
				{
					UnityEngine.Object.Destroy(nob.gameObject);
				}
				else
				{
					NetworkManager.StorePooledInstantiated(nob, asServer);
				}
				return;
			}
			if (asServer)
			{
				if (!NetworkManager.IsClient)
				{
					nob.gameObject.SetActive(value: false);
				}
			}
			else if (!NetworkManager.IsServer)
			{
				nob.gameObject.SetActive(value: false);
			}
			else if (NetworkManager.ServerManager.Objects.Spawned.ContainsKey(nob.ObjectId))
			{
				nob.SetRenderersVisible(visible: false);
			}
			else
			{
				nob.gameObject.SetActive(value: false);
			}
			if (!asServer)
			{
				return;
			}
			foreach (NetworkObject childNetworkObject in nob.ChildNetworkObjects)
			{
				if (childNetworkObject != null && !childNetworkObject.IsDeinitializing)
				{
					Despawn(childNetworkObject, despawnType, asServer);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void UpdateNetworkBehavioursForSceneObject(NetworkObject nob, bool asServer)
		{
			if (asServer || !NetworkManager.IsServer)
			{
				InitializePrefab(nob, -1);
			}
		}

		public static void InitializePrefab(NetworkObject prefab, int index, ushort? collectionId = null)
		{
			if (prefab == null)
			{
				return;
			}
			if (index != -1)
			{
				prefab.PrefabId = (ushort)index;
				if (collectionId.HasValue)
				{
					prefab.SpawnableCollectionId = collectionId.Value;
				}
			}
			byte componentIndex = 0;
			prefab.UpdateNetworkBehaviours(null, ref componentIndex);
		}

		internal virtual void DespawnWithoutSynchronization(bool asServer)
		{
			foreach (NetworkObject value in Spawned.Values)
			{
				if (!(value == null))
				{
					DespawnWithoutSynchronization(value, asServer, value.GetDefaultDespawnType(), removeFromSpawned: false);
				}
			}
			Spawned.Clear();
		}

		internal virtual void DespawnWithoutSynchronization(NetworkObject nob, bool asServer, DespawnType despawnType, bool removeFromSpawned)
		{
			if (nob == null)
			{
				return;
			}
			nob.Deinitialize(asServer);
			if (asServer || (!asServer && !NetworkManager.IsServer))
			{
				if (removeFromSpawned)
				{
					RemoveFromSpawned(nob, unexpectedlyDestroyed: false, asServer);
				}
				if (nob.IsSceneObject)
				{
					nob.gameObject.SetActive(value: false);
				}
				else if (despawnType == DespawnType.Destroy)
				{
					UnityEngine.Object.Destroy(nob.gameObject);
				}
				else
				{
					NetworkManager.StorePooledInstantiated(nob, asServer);
				}
			}
		}

		internal virtual void AddToSpawned(NetworkObject nob, bool asServer)
		{
			Spawned[nob.ObjectId] = nob;
		}

		protected internal void AddToSceneObjects(NetworkObject nob)
		{
			SceneObjects_Internal[nob.SceneId] = nob;
		}

		protected internal void RemoveFromSceneObjects(NetworkObject nob)
		{
			SceneObjects_Internal.Remove(nob.SceneId);
		}

		protected internal void RemoveFromSceneObjects(ulong sceneId)
		{
			SceneObjects_Internal.Remove(sceneId);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected internal NetworkObject GetSpawnedNetworkObject(int objectId)
		{
			if (!Spawned.TryGetValueIL2CPP(objectId, out var value))
			{
				NetworkManager.LogError($"Spawned NetworkObject not found for ObjectId {objectId}.");
			}
			return value;
		}

		protected internal void SkipDataLength(ushort packetId, PooledReader reader, int dataLength, int rpcLinkObjectId = -1)
		{
			if (dataLength == -1)
			{
				string value = ((packetId < NetworkManager.StartingRpcLinkIndex) ? $"NetworkBehaviour could not be found for packetId {(PacketId)packetId}. Remaining data will be purged." : ((rpcLinkObjectId == -1) ? $"RPCLink of Id {(PacketId)packetId} could not be found. Remaining data will be purged." : $"ObjectId {rpcLinkObjectId} for RPCLink {(PacketId)packetId} could not be found."));
				NetworkManager.LogError(value);
				reader.Clear();
			}
			else if (dataLength >= 0)
			{
				reader.Skip(Math.Min(dataLength, reader.Remaining));
			}
			else if (dataLength == -2)
			{
				reader.Clear();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void ParseReplicateRpc(PooledReader reader, NetworkConnection conn, Channel channel)
		{
			NetworkBehaviour networkBehaviour = reader.ReadNetworkBehaviour();
			int packetLength = Packets.GetPacketLength(8, reader, channel);
			if (networkBehaviour != null)
			{
				networkBehaviour.OnReplicateRpc(null, reader, conn, channel);
			}
			else
			{
				SkipDataLength(8, reader, packetLength);
			}
		}

		protected void ReadTransformProperties(Reader reader, out Vector3? localPosition, out Quaternion? localRotation, out Vector3? localScale)
		{
			byte whole = reader.ReadByte();
			if (ChangedTransformPropertiesEnum.Contains((ChangedTransformProperties)whole, ChangedTransformProperties.LocalPosition))
			{
				localPosition = reader.ReadVector3();
			}
			else
			{
				localPosition = null;
			}
			if (ChangedTransformPropertiesEnum.Contains((ChangedTransformProperties)whole, ChangedTransformProperties.LocalRotation))
			{
				localRotation = reader.ReadQuaternion(NetworkManager.ServerManager.SpawnPacking.Rotation);
			}
			else
			{
				localRotation = null;
			}
			if (ChangedTransformPropertiesEnum.Contains((ChangedTransformProperties)whole, ChangedTransformProperties.LocalScale))
			{
				localScale = reader.ReadVector3();
			}
			else
			{
				localScale = null;
			}
		}

		internal void WriteSpawn_Server(NetworkObject nob, NetworkConnection connection, Writer writer)
		{
			PooledWriter pooledWriter = WriterPool.Retrieve();
			pooledWriter.WritePacketId(PacketId.ObjectSpawn);
			pooledWriter.WriteNetworkObjectForSpawn(nob);
			if (NetworkManager.ServerManager.ShareIds || connection == nob.Owner)
			{
				pooledWriter.WriteNetworkConnection(nob.Owner);
			}
			else
			{
				pooledWriter.WriteInt16(-1);
			}
			bool flag = nob.CurrentParentNetworkObject != null;
			bool isSceneObject = nob.IsSceneObject;
			SpawnType spawnType = SpawnType.Unset;
			spawnType = ((!isSceneObject) ? ((SpawnType)((uint)spawnType | (uint)(nob.IsGlobal ? 8 : 4))) : (spawnType | SpawnType.Scene));
			if (flag)
			{
				spawnType |= SpawnType.Nested;
			}
			pooledWriter.WriteByte((byte)spawnType);
			pooledWriter.WriteByte(nob.ComponentIndex);
			WriteChangedTransformProperties(nob, isSceneObject, flag, pooledWriter);
			if (flag)
			{
				pooledWriter.WriteNetworkObjectId(nob.CurrentParentNetworkObject);
			}
			Transform t;
			if (isSceneObject)
			{
				pooledWriter.WriteUInt64(nob.SceneId, AutoPackType.Unpacked);
			}
			else
			{
				t = nob.transform.parent;
				if (t != null)
				{
					NetworkBehaviour component = t.GetComponent<NetworkBehaviour>();
					if (component == null)
					{
						NetworkObject component2 = t.GetComponent<NetworkObject>();
						if (!ParentIsSpawned(component2))
						{
							pooledWriter.WriteByte(0);
						}
						else
						{
							pooledWriter.WriteByte(1);
							pooledWriter.WriteNetworkObjectId(component2);
						}
					}
					else if (!ParentIsSpawned(component.NetworkObject))
					{
						pooledWriter.WriteByte(0);
					}
					else
					{
						pooledWriter.WriteByte(2);
						pooledWriter.WriteNetworkBehaviour(component);
					}
				}
				else
				{
					pooledWriter.WriteByte(0);
				}
				pooledWriter.WriteNetworkObjectId(nob.PrefabId);
			}
			writer.WriteBytes(pooledWriter.GetBuffer(), 0, pooledWriter.Length);
			PooledWriter pooledWriter2 = WriterPool.Retrieve();
			NetworkBehaviour[] networkBehaviours = nob.NetworkBehaviours;
			for (int i = 0; i < networkBehaviours.Length; i++)
			{
				networkBehaviours[i].WriteRpcLinks(pooledWriter2);
			}
			writer.WriteBytesAndSize(pooledWriter2.GetBuffer(), 0, pooledWriter2.Length);
			pooledWriter2.Reset();
			networkBehaviours = nob.NetworkBehaviours;
			for (int i = 0; i < networkBehaviours.Length; i++)
			{
				networkBehaviours[i].WriteSyncTypesForSpawn(pooledWriter2, connection);
			}
			writer.WriteBytesAndSize(pooledWriter2.GetBuffer(), 0, pooledWriter2.Length);
			pooledWriter.Store();
			pooledWriter2.Store();
			bool ParentIsSpawned(NetworkObject pNob)
			{
				bool flag2 = pNob == null;
				if (flag2 || !pNob.IsSpawned)
				{
					if (!flag2)
					{
						NetworkManager.LogWarning("Parent " + t.name + " is not spawned. " + nob.name + " will not have it's parent sent in the spawn message.");
					}
					return false;
				}
				return true;
			}
		}

		protected void WriteChangedTransformProperties(NetworkObject nob, bool sceneObject, bool nested, Writer headerWriter)
		{
			ChangedTransformProperties transformChanges;
			if (sceneObject || nested)
			{
				transformChanges = nob.GetTransformChanges(nob.SerializedTransformProperties);
			}
			else
			{
				PrefabObjects prefabObjects = NetworkManager.GetPrefabObjects<PrefabObjects>(nob.SpawnableCollectionId, createIfMissing: false);
				transformChanges = nob.GetTransformChanges(prefabObjects.GetObject(asServer: true, nob.PrefabId).gameObject);
			}
			headerWriter.WriteByte((byte)transformChanges);
			if (transformChanges != ChangedTransformProperties.Unset)
			{
				if (ChangedTransformPropertiesEnum.Contains(transformChanges, ChangedTransformProperties.LocalPosition))
				{
					headerWriter.WriteVector3(nob.transform.localPosition);
				}
				if (ChangedTransformPropertiesEnum.Contains(transformChanges, ChangedTransformProperties.LocalRotation))
				{
					headerWriter.WriteQuaternion(nob.transform.localRotation, NetworkManager.ServerManager.SpawnPacking.Rotation);
				}
				if (ChangedTransformPropertiesEnum.Contains(transformChanges, ChangedTransformProperties.LocalScale))
				{
					headerWriter.WriteVector3(nob.transform.localScale);
				}
			}
		}

		protected void WriteDespawn(NetworkObject nob, DespawnType despawnType, Writer everyoneWriter)
		{
			everyoneWriter.WritePacketId(PacketId.ObjectDespawn);
			everyoneWriter.WriteNetworkObjectForDespawn(nob, despawnType);
		}

		internal void GetTransformProperties(Vector3? readPos, Quaternion? readRot, Vector3? readScale, Transform defaultTransform, out Vector3 pos, out Quaternion rot, out Vector3 scale)
		{
			pos = ((!readPos.HasValue) ? defaultTransform.localPosition : readPos.Value);
			rot = ((!readRot.HasValue) ? defaultTransform.localRotation : readRot.Value);
			scale = ((!readScale.HasValue) ? defaultTransform.localScale : readScale.Value);
		}

		internal NetworkObject GetSceneNetworkObject(ulong sceneId)
		{
			SceneObjects_Internal.TryGetValueIL2CPP(sceneId, out var value);
			if (value == null)
			{
				NetworkManager.LogError($"SceneId of {sceneId} not found in SceneObjects. This may occur if your scene differs between client and server, if client does not have the scene loaded, or if networked scene objects do not have a SceneCondition. See ObserverManager in the documentation for more on conditions.");
			}
			return value;
		}

		protected bool CanPredictedSpawn(NetworkObject nob, NetworkConnection spawner, NetworkConnection owner, bool asServer, Reader reader = null)
		{
			if (!nob.AllowPredictedSpawning)
			{
				if (asServer)
				{
					spawner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection {spawner.ClientId} tried to spawn an object {nob.name} which does not support predicted spawning.");
				}
				else
				{
					NetworkManager.LogError("Object " + nob.name + " does not support predicted spawning. Modify the NetworkObject component settings to allow predicted spawning.");
				}
				reader?.Clear();
				return false;
			}
			if (nob.transform.parent != null)
			{
				if (asServer)
				{
					spawner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection {spawner.ClientId} tried to spawn an object that is not root.");
				}
				else
				{
					NetworkManager.LogError("Predicted spawning as a child is not supported.");
				}
				reader?.Clear();
				return false;
			}
			if (nob.ChildNetworkObjects.Count > 0)
			{
				if (asServer)
				{
					spawner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection {spawner.ClientId} tried to spawn an object {nob.name} which has nested NetworkObjects.");
				}
				else
				{
					NetworkManager.LogError("Predicted spawning prefabs which contain nested NetworkObjects is not yet supported but will be in a later release.");
				}
				reader?.Clear();
				return false;
			}
			if ((asServer && !nob.PredictedSpawn.OnTrySpawnServer(spawner, owner)) || (!asServer && !nob.PredictedSpawn.OnTrySpawnClient()))
			{
				reader?.Clear();
				return false;
			}
			return true;
		}

		protected bool CanPredictedDespawn(NetworkObject nob, NetworkConnection despawner, bool asServer, Reader reader = null)
		{
			if (!nob.AllowPredictedDespawning)
			{
				if (asServer)
				{
					despawner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection {despawner.ClientId} tried to despawn an object {nob.name} which does not support predicted despawning.");
				}
				else
				{
					NetworkManager.LogError("Object " + nob.name + " does not support predicted despawning. Modify the PredictedSpawn component settings to allow predicted despawning.");
				}
				reader?.Clear();
				return false;
			}
			if (nob.transform.parent != null)
			{
				if (asServer)
				{
					despawner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection {despawner.ClientId} tried to despawn an object that is not root.");
				}
				else
				{
					NetworkManager.LogError("Predicted despawning as a child is not supported.");
				}
				reader?.Clear();
				return false;
			}
			if (nob.ChildNetworkObjects.Count > 0)
			{
				if (asServer)
				{
					despawner.Kick(KickReason.ExploitAttempt, LoggingType.Common, $"Connection {despawner.ClientId} tried to despawn an object {nob.name} which has nested NetworkObjects.");
				}
				else
				{
					NetworkManager.LogError("Predicted despawning prefabs which contain nested NetworkObjects is not yet supported but will be in a later release.");
				}
				reader?.Clear();
				return false;
			}
			if ((asServer && !nob.PredictedSpawn.OnTryDepawnServer(despawner)) || (!asServer && !nob.PredictedSpawn.OnTryDespawnClient()))
			{
				reader?.Clear();
				return false;
			}
			return true;
		}
	}
}
