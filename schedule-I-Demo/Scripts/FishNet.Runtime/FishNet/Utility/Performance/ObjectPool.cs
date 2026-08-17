using System;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;

namespace FishNet.Utility.Performance
{
	public abstract class ObjectPool : MonoBehaviour
	{
		protected NetworkManager NetworkManager { get; private set; }

		public virtual void InitializeOnce(NetworkManager nm)
		{
			NetworkManager = nm;
		}

		[Obsolete("Use RetrieveObject(int, ushort, bool)")]
		public abstract NetworkObject RetrieveObject(int prefabId, bool asServer);

		public virtual NetworkObject RetrieveObject(int prefabId, ushort collectionId, bool asServer)
		{
			return null;
		}

		public virtual NetworkObject RetrieveObject(int prefabId, ushort collectionId, Vector3 position, Quaternion rotation, bool asServer)
		{
			return null;
		}

		public abstract void StoreObject(NetworkObject instantiated, bool asServer);

		public virtual void CacheObjects(NetworkObject prefab, int count, bool asServer)
		{
		}
	}
}
