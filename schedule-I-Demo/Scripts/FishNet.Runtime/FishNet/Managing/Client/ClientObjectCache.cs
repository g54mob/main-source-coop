using System;
using System.Collections.Generic;
using FishNet.Connection;
using FishNet.Managing.Object;
using FishNet.Object;
using FishNet.Serializing;
using FishNet.Utility.Extension;
using GameKit.Utilities;
using UnityEngine;

namespace FishNet.Managing.Client
{
	internal class ClientObjectCache
	{
		public enum CacheSearchType
		{
			Any = 0,
			Spawning = 1,
			Despawning = 2
		}

		internal Dictionary<int, NetworkObject> IteratedSpawningObjects = new Dictionary<int, NetworkObject>();

		internal HashSet<int> ReadSpawningObjects = new HashSet<int>();

		private List<CachedNetworkObject> _cachedObjects = new List<CachedNetworkObject>();

		private HashSet<NetworkObject> _iteratedSpawns = new HashSet<NetworkObject>();

		private HashSet<int> _conflictingDespawns = new HashSet<int>();

		private ClientObjects _clientObjects;

		private NetworkManager _networkManager;

		private bool _loggedSameTickWarning;

		private bool _initializeOrderChanged;

		public ClientObjectCache(ClientObjects cobs, NetworkManager networkManager)
		{
			_clientObjects = cobs;
			_networkManager = networkManager;
		}

		public NetworkObject GetInCached(int objectId, CacheSearchType searchType)
		{
			int count = _cachedObjects.Count;
			List<CachedNetworkObject> cachedObjects = _cachedObjects;
			for (int i = 0; i < count; i++)
			{
				CachedNetworkObject cachedNetworkObject = cachedObjects[i];
				if (cachedNetworkObject.ObjectId == objectId)
				{
					if (searchType == CacheSearchType.Any)
					{
						return cachedNetworkObject.NetworkObject;
					}
					bool num = searchType == CacheSearchType.Spawning;
					bool flag = cachedNetworkObject.Action == CachedNetworkObject.ActionType.Spawn;
					if (num == flag)
					{
						return cachedNetworkObject.NetworkObject;
					}
					return null;
				}
			}
			return null;
		}

		public void AddSpawn(NetworkManager manager, ushort collectionId, int objectId, sbyte initializeOrder, int ownerId, SpawnType ost, byte componentIndex, int rootObjectId, int? parentObjectId, byte? parentComponentIndex, int? prefabId, Vector3? localPosition, Quaternion? localRotation, Vector3? localScale, ulong sceneId, string sceneName, string objectName, ArraySegment<byte> rpcLinks, ArraySegment<byte> syncValues)
		{
			_initializeOrderChanged |= initializeOrder != 0;
			CachedNetworkObject cachedNetworkObject = null;
			if (!_initializeOrderChanged)
			{
				cachedNetworkObject = ResettableObjectCaches<CachedNetworkObject>.Retrieve();
				_cachedObjects.Add(cachedNetworkObject);
			}
			else
			{
				int count = _cachedObjects.Count;
				for (int i = 0; i < count; i++)
				{
					CachedNetworkObject cachedNetworkObject2 = _cachedObjects[i];
					if (initializeOrder < cachedNetworkObject2.InitializeOrder)
					{
						cachedNetworkObject = ResettableObjectCaches<CachedNetworkObject>.Retrieve();
						_cachedObjects.Insert(i, cachedNetworkObject);
						break;
					}
				}
				if (cachedNetworkObject == null)
				{
					cachedNetworkObject = ResettableObjectCaches<CachedNetworkObject>.Retrieve();
					_cachedObjects.Add(cachedNetworkObject);
				}
			}
			cachedNetworkObject.InitializeSpawn(manager, collectionId, objectId, initializeOrder, ownerId, ost, componentIndex, rootObjectId, parentObjectId, parentComponentIndex, prefabId, localPosition, localRotation, localScale, sceneId, sceneName, objectName, rpcLinks, syncValues);
			ReadSpawningObjects.Add(objectId);
		}

		public void AddDespawn(int objectId, DespawnType despawnType)
		{
			CachedNetworkObject cachedNetworkObject = ResettableObjectCaches<CachedNetworkObject>.Retrieve();
			_cachedObjects.Add(cachedNetworkObject);
			cachedNetworkObject.InitializeDespawn(objectId, despawnType);
		}

		public void Iterate()
		{
			int count = _cachedObjects.Count;
			if (count == 0)
			{
				return;
			}
			try
			{
				HashSet<int> processedIndexes = new HashSet<int>();
				List<CachedNetworkObject> cachedObjects = _cachedObjects;
				_conflictingDespawns.Clear();
				for (int i = 0; i < count; i++)
				{
					if (processedIndexes.Contains(i))
					{
						continue;
					}
					CachedNetworkObject cachedNetworkObject = cachedObjects[i];
					bool flag = cachedNetworkObject.Action == CachedNetworkObject.ActionType.Spawn;
					if (flag && (cachedNetworkObject.IsNested || cachedNetworkObject.HasParent))
					{
						bool isNested = cachedNetworkObject.IsNested;
						int num = (isNested ? cachedNetworkObject.RootObjectId : cachedNetworkObject.ParentObjectId.Value);
						if (GetSpawnedObject(num) == null)
						{
							bool flag2 = false;
							for (int j = i + 1; j < count; j++)
							{
								CachedNetworkObject cachedNetworkObject2 = cachedObjects[j];
								if (cachedNetworkObject2.ObjectId == num)
								{
									flag2 = true;
									if (cachedNetworkObject.Action != CachedNetworkObject.ActionType.Spawn)
									{
										string value = (isNested ? $"ObjectId {num} was found for a nested spawn, but ActionType is not spawn. ComponentIndex {cachedNetworkObject.ComponentIndex} will not be spawned." : $"ObjectId {num} was found for a parented spawn, but ActionType is not spawn. ObjectId {cachedNetworkObject.ObjectId} will not be spawned.");
										_networkManager.LogError(value);
									}
									else
									{
										ProcessObject(cachedNetworkObject2, spawn: true, j);
									}
									break;
								}
							}
							if (!flag2)
							{
								string value = (isNested ? $"ObjectId {num} could not be found for a nested spawn. ComponentIndex {cachedNetworkObject.ComponentIndex} will not be spawned." : $"ObjectId {num} was found for a parented spawn. ObjectId {cachedNetworkObject.ObjectId} will not be spawned.");
								_networkManager.LogError(value);
							}
						}
					}
					ProcessObject(cachedNetworkObject, flag, i);
				}
				for (int k = 0; k < count; k++)
				{
					CachedNetworkObject cachedNetworkObject3 = cachedObjects[k];
					if (cachedNetworkObject3.Action == CachedNetworkObject.ActionType.Spawn && cachedNetworkObject3.NetworkObject != null)
					{
						NetworkBehaviour[] networkBehaviours = cachedNetworkObject3.NetworkObject.NetworkBehaviours;
						foreach (NetworkBehaviour obj in networkBehaviours)
						{
							PooledReader syncValuesReader = cachedNetworkObject3.SyncValuesReader;
							int length = syncValuesReader.ReadInt32();
							obj.OnSyncType(syncValuesReader, length, isSyncObject: false);
							length = syncValuesReader.ReadInt32();
							obj.OnSyncType(syncValuesReader, length, isSyncObject: true);
						}
						if (!_conflictingDespawns.Contains(cachedNetworkObject3.ObjectId) || !_iteratedSpawns.Contains(cachedNetworkObject3.NetworkObject))
						{
							cachedNetworkObject3.NetworkObject.Initialize(asServer: false, invokeSyncTypeCallbacks: false);
						}
					}
				}
				for (int m = 0; m < count; m++)
				{
					CachedNetworkObject cachedNetworkObject4 = cachedObjects[m];
					if (cachedNetworkObject4.Action == CachedNetworkObject.ActionType.Spawn && cachedNetworkObject4.NetworkObject != null)
					{
						cachedNetworkObject4.NetworkObject.InvokeOnStartSyncTypeCallbacks(asServer: false);
					}
				}
				void ProcessObject(CachedNetworkObject cnob, bool spawn, int index)
				{
					processedIndexes.Add(index);
					if (spawn)
					{
						if (cnob.IsSceneObject)
						{
							cnob.NetworkObject = _clientObjects.GetSceneNetworkObject(cnob.SceneId);
						}
						else if (cnob.IsNested)
						{
							cnob.NetworkObject = _clientObjects.GetNestedNetworkObject(cnob);
						}
						else
						{
							cnob.NetworkObject = _clientObjects.GetInstantiatedNetworkObject(cnob);
						}
						if (!_networkManager.IsHost && cnob.NetworkObject != null)
						{
							Transform transform = cnob.NetworkObject.transform;
							_clientObjects.GetTransformProperties(cnob.LocalPosition, cnob.LocalRotation, cnob.LocalScale, transform, out var pos, out var rot, out var scale);
							if (cnob.HasParent)
							{
								if (_networkManager.ClientManager.Objects.Spawned.TryGetValueIL2CPP(cnob.ParentObjectId.Value, out var value2))
								{
									if (!cnob.ParentComponentIndex.HasValue)
									{
										cnob.NetworkObject.SetParent(value2);
									}
									else
									{
										cnob.NetworkObject.SetParent(value2.NetworkBehaviours[cnob.ParentComponentIndex.Value]);
									}
								}
								else
								{
									_networkManager.Log($"Parent NetworkObject Id {cnob.ParentObjectId} could not be found in spawned. NetworkObject {cnob.NetworkObject} will not have it's parent set.");
								}
							}
							transform.SetLocalPositionRotationAndScale(pos, rot, scale);
						}
					}
					else
					{
						cnob.NetworkObject = _clientObjects.GetSpawnedNetworkObject(cnob);
						if (!_networkManager.IsHost && cnob.NetworkObject == null && !cnob.IsNested)
						{
							_networkManager.Log($"NetworkObject for ObjectId of {cnob.ObjectId} was found null. Unable to despawn object. This may occur if a nested NetworkObject had it's parent object unexpectedly destroyed. This incident is often safe to ignore.");
						}
					}
					NetworkObject networkObject = cnob.NetworkObject;
					if (!(networkObject == null))
					{
						if (spawn)
						{
							int objectId;
							NetworkConnection value3;
							if (!_networkManager.IsServer)
							{
								objectId = cnob.ObjectId;
								int ownerId = cnob.OwnerId;
								NetworkConnection connection = _networkManager.ClientManager.Connection;
								if (ownerId == connection.ClientId)
								{
									value3 = connection;
								}
								else if (!_networkManager.ClientManager.Clients.TryGetValueIL2CPP(ownerId, out value3))
								{
									value3 = NetworkManager.EmptyConnection;
								}
							}
							else
							{
								value3 = networkObject.Owner;
								objectId = networkObject.ObjectId;
							}
							networkObject.Preinitialize_Internal(_networkManager, objectId, value3, asServer: false);
							_clientObjects.AddToSpawned(cnob.NetworkObject, asServer: false);
							IteratedSpawningObjects.Add(cnob.ObjectId, cnob.NetworkObject);
							_clientObjects.ApplyRpcLinks(cnob.NetworkObject, cnob.RpcLinkReader);
							_iteratedSpawns.Add(cnob.NetworkObject);
							if (!_networkManager.IsServer && cnob.NetworkObject != null)
							{
								cnob.NetworkObject.gameObject.SetActive(value: true);
							}
						}
						else
						{
							if (_iteratedSpawns.Contains(cnob.NetworkObject))
							{
								if (!_loggedSameTickWarning)
								{
									_loggedSameTickWarning = true;
									_networkManager.LogWarning("NetworkObject " + cnob.NetworkObject.name + " is being despawned on the same tick it's spawned. When this occurs SyncTypes will not be set on other objects during the time of this despawn. In result, if NetworkObject " + cnob.NetworkObject.name + " is referencing a SyncType of another object being spawned this tick, the returned values will be default.");
								}
								_conflictingDespawns.Add(cnob.ObjectId);
								cnob.NetworkObject.gameObject.SetActive(value: true);
								cnob.NetworkObject.Initialize(asServer: false, invokeSyncTypeCallbacks: true);
							}
							IterateDespawn(cnob);
						}
					}
				}
			}
			finally
			{
				Reset();
			}
		}

		private void IterateDespawn(CachedNetworkObject cnob)
		{
			_clientObjects.Despawn(cnob.NetworkObject, cnob.DespawnType, asServer: false);
		}

		internal NetworkObject GetSpawnedObject(int objectId)
		{
			if (!IteratedSpawningObjects.TryGetValue(objectId, out var value))
			{
				(_networkManager.IsHost ? _networkManager.ServerManager.Objects.Spawned : _networkManager.ClientManager.Objects.Spawned).TryGetValue(objectId, out value);
			}
			return value;
		}

		public void Reset()
		{
			_initializeOrderChanged = false;
			foreach (CachedNetworkObject cachedObject in _cachedObjects)
			{
				ResettableObjectCaches<CachedNetworkObject>.Store(cachedObject);
			}
			_cachedObjects.Clear();
			_iteratedSpawns.Clear();
			IteratedSpawningObjects.Clear();
			ReadSpawningObjects.Clear();
		}
	}
}
