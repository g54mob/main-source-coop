using System;
using FishNet.Managing.Object;
using FishNet.Object;
using FishNet.Serializing;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.Scripting;

namespace FishNet.Managing.Client
{
	[Preserve]
	internal class CachedNetworkObject : IResettable
	{
		public enum ActionType
		{
			Unset = 0,
			Spawn = 1,
			Despawn = 2
		}

		public ushort CollectionId;

		public int ObjectId;

		public sbyte InitializeOrder;

		public int OwnerId;

		public SpawnType SpawnType;

		public DespawnType DespawnType;

		public byte ComponentIndex;

		public int RootObjectId;

		public int? ParentObjectId;

		public byte? ParentComponentIndex;

		public int? PrefabId;

		public Vector3? LocalPosition;

		public Quaternion? LocalRotation;

		public Vector3? LocalScale;

		public ulong SceneId;

		public ArraySegment<byte> RpcLinks;

		public ArraySegment<byte> SyncValues;

		public NetworkObject NetworkObject;

		public bool IsNested => ComponentIndex > 0;

		public bool IsSceneObject => SceneId != 0;

		public bool HasParent => ParentObjectId.HasValue;

		public bool ParentIsNetworkBehaviour
		{
			get
			{
				if (HasParent)
				{
					return ParentComponentIndex.HasValue;
				}
				return false;
			}
		}

		public ActionType Action { get; private set; }

		public PooledReader RpcLinkReader { get; private set; }

		public PooledReader SyncValuesReader { get; private set; }

		public void InitializeSpawn(NetworkManager manager, ushort collectionId, int objectId, sbyte initializeOrder, int ownerId, SpawnType objectSpawnType, byte componentIndex, int rootObjectId, int? parentObjectId, byte? parentComponentIndex, int? prefabId, Vector3? localPosition, Quaternion? localRotation, Vector3? localScale, ulong sceneId, string sceneName, string objectName, ArraySegment<byte> rpcLinks, ArraySegment<byte> syncValues)
		{
			ResetState();
			Action = ActionType.Spawn;
			CollectionId = collectionId;
			ObjectId = objectId;
			InitializeOrder = initializeOrder;
			OwnerId = ownerId;
			SpawnType = objectSpawnType;
			ComponentIndex = componentIndex;
			RootObjectId = rootObjectId;
			ParentObjectId = parentObjectId;
			ParentComponentIndex = parentComponentIndex;
			PrefabId = prefabId;
			LocalPosition = localPosition;
			LocalRotation = localRotation;
			LocalScale = localScale;
			SceneId = sceneId;
			RpcLinks = rpcLinks;
			SyncValues = syncValues;
			RpcLinkReader = ReaderPool.Retrieve(rpcLinks, manager);
			SyncValuesReader = ReaderPool.Retrieve(syncValues, manager);
		}

		public void InitializeDespawn(int objectId, DespawnType despawnType)
		{
			ResetState();
			Action = ActionType.Despawn;
			DespawnType = despawnType;
			ObjectId = objectId;
		}

		public void ResetState()
		{
			NetworkObject = null;
			if (RpcLinkReader != null)
			{
				ReaderPool.Store(RpcLinkReader);
				RpcLinkReader = null;
			}
			if (SyncValuesReader != null)
			{
				ReaderPool.Store(SyncValuesReader);
				SyncValuesReader = null;
			}
		}

		public void InitializeState()
		{
		}

		~CachedNetworkObject()
		{
			NetworkObject = null;
		}
	}
}
