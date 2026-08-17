using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FishNet.Managing.Object;
using FishNet.Object;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Utility.Performance
{
	public class DefaultObjectPool : ObjectPool
	{
		private List<Dictionary<int, Stack<NetworkObject>>> _cache = new List<Dictionary<int, Stack<NetworkObject>>>();

		[Tooltip("True if to use object pooling.")]
		[SerializeField]
		private bool _enabled = true;

		private Dictionary<int, Transform> _objectParents = new Dictionary<int, Transform>();

		private const string OBJECTS_PARENT_NAME = "DefaultObjectPool Parent";

		public IReadOnlyCollection<Dictionary<int, Stack<NetworkObject>>> Cache => _cache;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override NetworkObject RetrieveObject(int prefabId, bool asServer)
		{
			return RetrieveObject(prefabId, 0, asServer);
		}

		public override NetworkObject RetrieveObject(int prefabId, ushort collectionId, Vector3 position, Quaternion rotation, bool asServer)
		{
			PrefabObjects prefabObjects = base.NetworkManager.GetPrefabObjects<PrefabObjects>(collectionId, createIfMissing: false);
			if (!_enabled)
			{
				return UnityEngine.Object.Instantiate(prefabObjects.GetObject(asServer, prefabId), position, rotation);
			}
			Stack<NetworkObject> orCreateCache = GetOrCreateCache(collectionId, prefabId);
			NetworkObject networkObject;
			do
			{
				if (orCreateCache.Count == 0)
				{
					networkObject = UnityEngine.Object.Instantiate(prefabObjects.GetObject(asServer, prefabId), position, rotation);
					break;
				}
				networkObject = orCreateCache.Pop();
				if (networkObject != null)
				{
					networkObject.transform.SetPositionAndRotation(position, rotation);
				}
			}
			while (networkObject == null);
			networkObject.gameObject.SetActive(value: true);
			return networkObject;
		}

		public override NetworkObject RetrieveObject(int prefabId, ushort collectionId, bool asServer)
		{
			PrefabObjects prefabObjects = base.NetworkManager.GetPrefabObjects<PrefabObjects>(collectionId, createIfMissing: false);
			if (!_enabled)
			{
				return UnityEngine.Object.Instantiate(prefabObjects.GetObject(asServer, prefabId));
			}
			Stack<NetworkObject> orCreateCache = GetOrCreateCache(collectionId, prefabId);
			NetworkObject networkObject;
			do
			{
				if (orCreateCache.Count == 0)
				{
					networkObject = UnityEngine.Object.Instantiate(prefabObjects.GetObject(asServer, prefabId));
					break;
				}
				networkObject = orCreateCache.Pop();
			}
			while (networkObject == null);
			networkObject.gameObject.SetActive(value: true);
			return networkObject;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override void StoreObject(NetworkObject instantiated, bool asServer)
		{
			if (!_enabled)
			{
				UnityEngine.Object.Destroy(instantiated.gameObject);
				return;
			}
			instantiated.gameObject.SetActive(value: false);
			instantiated.ResetState();
			Transform objectStoreParent = GetObjectStoreParent(instantiated);
			instantiated.transform.SetParent(objectStoreParent);
			GetOrCreateCache(instantiated.SpawnableCollectionId, instantiated.PrefabId).Push(instantiated);
		}

		public override void CacheObjects(NetworkObject prefab, int count, bool asServer)
		{
			if (!_enabled || count <= 0 || prefab == null)
			{
				return;
			}
			if (prefab.PrefabId == ushort.MaxValue)
			{
				InstanceFinder.NetworkManager.LogError("Pefab " + prefab.name + " has an invalid prefabId and cannot be cached.");
				return;
			}
			Stack<NetworkObject> orCreateCache = GetOrCreateCache(prefab.SpawnableCollectionId, prefab.PrefabId);
			for (int i = 0; i < count; i++)
			{
				NetworkObject networkObject = UnityEngine.Object.Instantiate(prefab);
				networkObject.gameObject.SetActive(value: false);
				orCreateCache.Push(networkObject);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearPool()
		{
			int count = _cache.Count;
			for (int i = 0; i < count; i++)
			{
				ClearPool(i);
			}
		}

		public void ClearPool(int collectionId)
		{
			if (collectionId >= _cache.Count)
			{
				return;
			}
			Dictionary<int, Stack<NetworkObject>> dictionary = _cache[collectionId];
			foreach (Stack<NetworkObject> value in dictionary.Values)
			{
				while (value.Count > 0)
				{
					NetworkObject networkObject = value.Pop();
					if (networkObject != null)
					{
						UnityEngine.Object.Destroy(networkObject.gameObject);
					}
				}
			}
			dictionary.Clear();
		}

		private Transform GetObjectStoreParent(NetworkObject nob)
		{
			int handle = nob.gameObject.scene.handle;
			_objectParents.TryGetValue(handle, out var value);
			if (value == null)
			{
				value = new GameObject("DefaultObjectPool Parent").transform;
				value.gameObject.AddComponent<DefaultObjectPoolContainer>().Initialize(this);
				SceneManager.MoveGameObjectToScene(value.gameObject, nob.gameObject.scene);
				_objectParents[handle] = value;
			}
			return value;
		}

		internal void ObjectsDestroyed(DefaultObjectPoolContainer container)
		{
			_objectParents.Remove(container.gameObject.scene.handle);
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList();
			int childCount = container.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				NetworkObject component = container.transform.GetChild(i).GetComponent<NetworkObject>();
				if (component != null)
				{
					list.Add(component);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			List<Dictionary<int, HashSet<NetworkObject>>> list2 = new List<Dictionary<int, HashSet<NetworkObject>>>();
			for (int j = 0; j < _cache.Count; j++)
			{
				list2.Add(new Dictionary<int, HashSet<NetworkObject>>());
			}
			foreach (NetworkObject item in list)
			{
				if (item.SpawnableCollectionId >= list2.Count)
				{
					continue;
				}
				Dictionary<int, HashSet<NetworkObject>> dictionary = list2[item.SpawnableCollectionId];
				if (!dictionary.TryGetValueIL2CPP(item.PrefabId, out var value))
				{
					value = CollectionCaches<NetworkObject>.RetrieveHashSet();
					Stack<NetworkObject> orCreateCache = GetOrCreateCache(item.SpawnableCollectionId, item.PrefabId);
					_ = orCreateCache.Count;
					while (orCreateCache.Count > 0)
					{
						value.Add(orCreateCache.Pop());
					}
					dictionary[item.PrefabId] = value;
				}
				value.Remove(item);
			}
			CollectionCaches<NetworkObject>.Store(list);
			for (int k = 0; k < list2.Count; k++)
			{
				Dictionary<int, Stack<NetworkObject>> dict = _cache[k];
				foreach (KeyValuePair<int, HashSet<NetworkObject>> item2 in list2[k])
				{
					if (dict.TryGetValueIL2CPP(item2.Key, out var value2))
					{
						foreach (NetworkObject item3 in item2.Value)
						{
							value2.Push(item3);
						}
					}
					else
					{
						Debug.LogError($"Stack could not be found for {item2.Key}.");
					}
					CollectionCaches<NetworkObject>.Store(item2.Value);
				}
			}
		}

		private Stack<NetworkObject> GetOrCreateCache(int collectionId, int prefabId)
		{
			if (collectionId >= _cache.Count)
			{
				while (_cache.Count <= collectionId)
				{
					Dictionary<int, Stack<NetworkObject>> item = new Dictionary<int, Stack<NetworkObject>>();
					_cache.Add(item);
				}
			}
			Dictionary<int, Stack<NetworkObject>> dictionary = _cache[collectionId];
			if (!dictionary.TryGetValueIL2CPP(prefabId, out var value))
			{
				value = (dictionary[prefabId] = new Stack<NetworkObject>());
			}
			return value;
		}
	}
}
