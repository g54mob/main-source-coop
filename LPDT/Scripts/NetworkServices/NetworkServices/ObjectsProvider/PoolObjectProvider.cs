#define FUSION_LOGLEVEL_TRACE
using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Zenject;

namespace NetworkServices.ObjectsProvider
{
	public class PoolObjectProvider : NetworkObjectProviderDefault, IForbiddableObjectProvider
	{
		private readonly Dictionary<NetworkPrefabId, Queue<NetworkObject>> _objectPools = new Dictionary<NetworkPrefabId, Queue<NetworkObject>>();

		private readonly Dictionary<NetworkObject, IPoolableObject> _poolableObjectCache = new Dictionary<NetworkObject, IPoolableObject>();

		private int _maxPoolCount;

		private bool _isAcquireInstanceAllowed = true;

		public bool IsAcquireInstanceAllowed => _isAcquireInstanceAllowed;

		public void SetMaxPoolCount(int count)
		{
			_maxPoolCount = count;
		}

		private void OnDestroy()
		{
			Cleanup();
		}

		public override NetworkObjectAcquireResult AcquirePrefabInstance(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject instance)
		{
			instance = null;
			if (!_isAcquireInstanceAllowed)
			{
				return NetworkObjectAcquireResult.Ignore;
			}
			if (DelayIfSceneManagerIsBusy && runner.SceneManager.IsBusy)
			{
				return NetworkObjectAcquireResult.Retry;
			}
			NetworkObject prefab;
			NetworkObjectAcquireResult networkObjectAcquireResult = TryLoadPrefab(runner, in context, out prefab);
			if (networkObjectAcquireResult != NetworkObjectAcquireResult.Success)
			{
				return networkObjectAcquireResult;
			}
			NetworkObjectAcquireResult networkObjectAcquireResult2 = TryGetInstance(prefab, in context, out instance);
			if (networkObjectAcquireResult2 != NetworkObjectAcquireResult.Success)
			{
				return networkObjectAcquireResult2;
			}
			if (context.DontDestroyOnLoad)
			{
				runner.MakeDontDestroyOnLoad(instance.gameObject);
			}
			else
			{
				runner.MoveToRunnerScene(instance.gameObject);
			}
			runner.Prefabs.AddInstance(context.PrefabId);
			return NetworkObjectAcquireResult.Success;
		}

		public override void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
		{
			NetworkObject networkObject = context.Object;
			if (!context.IsBeingDestroyed)
			{
				if (context.TypeId.IsPrefab && CanBeReturnedToPool(context.TypeId.AsPrefabId, networkObject))
				{
					ReturnToPool(context.TypeId.AsPrefabId, networkObject);
					if (_poolableObjectCache.TryGetValue(networkObject, out var value))
					{
						value.OnDeactivated();
					}
				}
				else
				{
					if (_poolableObjectCache.TryGetValue(networkObject, out var value2))
					{
						value2.OnDespawned();
						_poolableObjectCache.Remove(networkObject);
					}
					UnityEngine.Object.Destroy(networkObject.gameObject);
				}
			}
			if (context.TypeId.IsPrefab)
			{
				runner.Prefabs.RemoveInstance(context.TypeId.AsPrefabId);
			}
		}

		private static NetworkObjectAcquireResult TryLoadPrefab(NetworkRunner runner, in NetworkPrefabAcquireContext context, out NetworkObject prefab)
		{
			prefab = null;
			try
			{
				prefab = runner.Prefabs.Load(context.PrefabId, context.IsSynchronous);
			}
			catch (Exception arg)
			{
				Log.Error($"Failed to load prefab: {arg}");
				return NetworkObjectAcquireResult.Failed;
			}
			if (!prefab)
			{
				return NetworkObjectAcquireResult.Retry;
			}
			return NetworkObjectAcquireResult.Success;
		}

		private NetworkObjectAcquireResult TryGetInstance(NetworkObject prefab, in NetworkPrefabAcquireContext context, out NetworkObject instance)
		{
			if (IsFreeInstanceInPool(context.PrefabId))
			{
				instance = GetInstanceFromPool(context.PrefabId);
				if (_poolableObjectCache.TryGetValue(instance, out var value))
				{
					value.OnActivated();
				}
			}
			else
			{
				if (prefab.TryGetComponent<NetworkObjectSpawnData>(out var component) && component.Injectable)
				{
					NetworkObjectAcquireResult networkObjectAcquireResult = TryInstantiateAndInject(prefab, out instance);
					if (networkObjectAcquireResult != NetworkObjectAcquireResult.Success)
					{
						return networkObjectAcquireResult;
					}
				}
				else
				{
					instance = InstantiateNewPrefab(prefab);
				}
				if (instance.TryGetComponent<IPoolableObject>(out var component2))
				{
					_poolableObjectCache[instance] = component2;
					component2.OnSpawned();
				}
			}
			return NetworkObjectAcquireResult.Success;
		}

		private NetworkObjectAcquireResult TryInstantiateAndInject(NetworkObject prefab, out NetworkObject instance)
		{
			instance = null;
			DiContainer diContainer = NetworkObjectInjectionContainer.Resolve();
			bool activeSelf = prefab.gameObject.activeSelf;
			prefab.gameObject.SetActive(value: false);
			try
			{
				instance = InstantiateNewPrefab(prefab);
				diContainer.InjectGameObject(instance.gameObject);
				instance.gameObject.SetActive(activeSelf);
			}
			catch (Exception arg)
			{
				prefab.gameObject.SetActive(activeSelf);
				if (NetworkObjectInjectionContainer.IsSessionContextMissing())
				{
					if (instance != null)
					{
						UnityEngine.Object.Destroy(instance.gameObject);
					}
					instance = null;
					return NetworkObjectAcquireResult.Retry;
				}
				Log.Error($"Failed to load prefab: {arg}");
				return NetworkObjectAcquireResult.Failed;
			}
			prefab.gameObject.SetActive(activeSelf);
			return NetworkObjectAcquireResult.Success;
		}

		private bool IsFreeInstanceInPool(NetworkPrefabId contextPrefabId)
		{
			if (_objectPools.TryGetValue(contextPrefabId, out var value))
			{
				return value.Any((NetworkObject x) => x != null && x.IsValid);
			}
			return false;
		}

		private NetworkObject GetInstanceFromPool(NetworkPrefabId contextPrefabId)
		{
			Queue<NetworkObject> queue = _objectPools[contextPrefabId];
			NetworkObject networkObject = null;
			bool flag = false;
			int num = 0;
			while (!flag && num < queue.Count)
			{
				networkObject = queue.Dequeue();
				if (networkObject != null && networkObject.IsValid)
				{
					if (_poolableObjectCache.TryGetValue(networkObject, out var value))
					{
						value.PerformTakeFromPool(networkObject);
					}
					else
					{
						networkObject.gameObject.SetActive(value: true);
					}
					flag = true;
					num++;
				}
			}
			return networkObject;
		}

		private NetworkObject InstantiateNewPrefab(NetworkObject prefab)
		{
			return UnityEngine.Object.Instantiate(prefab);
		}

		private bool CanBeReturnedToPool(NetworkPrefabId prefabId, NetworkObject instance)
		{
			if (instance.TryGetComponent<INotPoolableObject>(out var _))
			{
				return false;
			}
			if (!_objectPools.TryGetValue(prefabId, out var value))
			{
				return true;
			}
			if (_maxPoolCount > 0)
			{
				return value.Count < _maxPoolCount;
			}
			return true;
		}

		private void ReturnToPool(NetworkPrefabId prefabId, NetworkObject instance)
		{
			if (!_objectPools.TryGetValue(prefabId, out var value))
			{
				value = new Queue<NetworkObject>();
				_objectPools[prefabId] = value;
			}
			value.Enqueue(instance);
			if (_poolableObjectCache.TryGetValue(instance, out var value2))
			{
				value2.PerformReturnToPool(instance);
			}
			else
			{
				instance.gameObject.SetActive(value: false);
			}
		}

		public void Cleanup()
		{
			_objectPools.Clear();
			_poolableObjectCache.Clear();
		}

		public void CleanPoolsForScenes(List<string> scenes)
		{
			CleanObjectPoolsForScenes(scenes);
			CleanPoolableObjectCacheForScenes(scenes);
		}

		private void CleanObjectPoolsForScenes(List<string> scenes)
		{
			List<NetworkPrefabId> list = new List<NetworkPrefabId>();
			foreach (KeyValuePair<NetworkPrefabId, Queue<NetworkObject>> objectPool in _objectPools)
			{
				Queue<NetworkObject> value = objectPool.Value;
				if (TryGetFirstNonNullPoolObject(value, out var result) && scenes.Contains(result.gameObject.scene.name))
				{
					list.Add(objectPool.Key);
				}
			}
			list.ForEach(delegate(NetworkPrefabId x)
			{
				_objectPools.Remove(x);
			});
		}

		private void CleanPoolableObjectCacheForScenes(List<string> scenes)
		{
			List<NetworkObject> list = new List<NetworkObject>();
			foreach (KeyValuePair<NetworkObject, IPoolableObject> item in _poolableObjectCache)
			{
				if (scenes.Contains(item.Key.gameObject.scene.name))
				{
					list.Add(item.Key);
				}
			}
			list.ForEach(delegate(NetworkObject x)
			{
				_poolableObjectCache.Remove(x);
			});
		}

		private bool TryGetFirstNonNullPoolObject(Queue<NetworkObject> pool, out NetworkObject result)
		{
			foreach (NetworkObject item in pool)
			{
				if (item != null)
				{
					result = item;
					return true;
				}
			}
			result = null;
			return false;
		}

		public void ForbidAcquireInstance()
		{
			_isAcquireInstanceAllowed = false;
		}

		public void AllowAcquireInstance()
		{
			_isAcquireInstanceAllowed = true;
		}
	}
}
