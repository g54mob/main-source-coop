using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EvilCore.EvilPack.EvilLogger;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

namespace EvilCore.Particles
{
	public class ParticlesManager : MonoBehaviour, IParticlesManager
	{
		private class ParticlePool
		{
			public string Key;

			public GameObject Prefab;

			public readonly Queue<PooledParticle> Available = new Queue<PooledParticle>();

			public readonly List<PooledParticle> All = new List<PooledParticle>();
		}

		[Inject]
		private ParticleDatabase _database;

		private readonly Dictionary<string, ParticlePool> _pools = new Dictionary<string, ParticlePool>();

		private readonly Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

		private readonly HashSet<string> _loadingKeys = new HashSet<string>();

		private readonly Dictionary<int, PooledParticle> _activeHandles = new Dictionary<int, PooledParticle>();

		private int _nextHandleId = 1;

		private void OnDestroy()
		{
			ClearAllPools();
		}

		public void PlayOneShot(string key, Vector3 position, Quaternion rotation = default(Quaternion))
		{
			PlayOneShot(key, position, rotation, default(ParticleOverrides));
		}

		public void PlayOneShot(string key, Vector3 position, Quaternion rotation, ParticleOverrides overrides)
		{
			PlayOneShotAsync(key, position, rotation, overrides).Forget();
		}

		public void PlayOneShotAttached(string key, Transform parent, Vector3 localOffset = default(Vector3))
		{
			PlayOneShotAttachedAsync(key, parent, localOffset).Forget();
		}

		private async UniTaskVoid PlayOneShotAsync(string key, Vector3 position, Quaternion rotation, ParticleOverrides overrides)
		{
			GameObject gameObject = await GetOrLoadPrefab(key);
			if (!(gameObject == null))
			{
				PooledParticle fromPool = GetFromPool(key, gameObject);
				if (rotation == default(Quaternion))
				{
					rotation = Quaternion.identity;
				}
				fromPool.transform.SetParent(null);
				fromPool.transform.SetPositionAndRotation(position, rotation);
				int num = _nextHandleId++;
				_activeHandles[num] = fromPool;
				fromPool.Activate(num, isOneShot: true, overrides);
			}
		}

		private async UniTaskVoid PlayOneShotAttachedAsync(string key, Transform parent, Vector3 localOffset)
		{
			GameObject gameObject = await GetOrLoadPrefab(key);
			if (!(gameObject == null) && !(parent == null))
			{
				PooledParticle fromPool = GetFromPool(key, gameObject);
				fromPool.transform.SetParent(parent);
				fromPool.transform.localPosition = localOffset;
				fromPool.transform.localRotation = Quaternion.identity;
				int num = _nextHandleId++;
				_activeHandles[num] = fromPool;
				fromPool.Activate(num, isOneShot: true);
			}
		}

		public ParticleHandle Play(string key, Vector3 position, Quaternion rotation = default(Quaternion))
		{
			if (!_prefabCache.TryGetValue(key, out var value))
			{
				PlayManualAsync(key, position, rotation).Forget();
				return ParticleHandle.Invalid;
			}
			return PlayInternal(key, value, position, rotation);
		}

		public ParticleHandle PlayAttached(string key, Transform parent, Vector3 localOffset = default(Vector3))
		{
			if (!_prefabCache.TryGetValue(key, out var value))
			{
				PlayManualAttachedAsync(key, parent, localOffset).Forget();
				return ParticleHandle.Invalid;
			}
			return PlayAttachedInternal(key, value, parent, localOffset);
		}

		public void Stop(ParticleHandle handle, bool clear = false)
		{
			if (handle.IsValid && _activeHandles.ContainsKey(handle.Id))
			{
				_activeHandles.Remove(handle.Id);
				handle.Pooled.Deactivate();
				ReturnToPoolInternal(handle.Pooled);
			}
		}

		private ParticleHandle PlayInternal(string key, GameObject prefab, Vector3 position, Quaternion rotation)
		{
			PooledParticle fromPool = GetFromPool(key, prefab);
			if (rotation == default(Quaternion))
			{
				rotation = Quaternion.identity;
			}
			fromPool.transform.SetParent(null);
			fromPool.transform.SetPositionAndRotation(position, rotation);
			int num = _nextHandleId++;
			_activeHandles[num] = fromPool;
			fromPool.Activate(num, isOneShot: false);
			return new ParticleHandle(num, fromPool);
		}

		private ParticleHandle PlayAttachedInternal(string key, GameObject prefab, Transform parent, Vector3 localOffset)
		{
			PooledParticle fromPool = GetFromPool(key, prefab);
			fromPool.transform.SetParent(parent);
			fromPool.transform.localPosition = localOffset;
			fromPool.transform.localRotation = Quaternion.identity;
			int num = _nextHandleId++;
			_activeHandles[num] = fromPool;
			fromPool.Activate(num, isOneShot: false);
			return new ParticleHandle(num, fromPool);
		}

		private async UniTaskVoid PlayManualAsync(string key, Vector3 position, Quaternion rotation)
		{
			GameObject gameObject = await GetOrLoadPrefab(key);
			if (!(gameObject == null))
			{
				PlayInternal(key, gameObject, position, rotation);
			}
		}

		private async UniTaskVoid PlayManualAttachedAsync(string key, Transform parent, Vector3 localOffset)
		{
			GameObject gameObject = await GetOrLoadPrefab(key);
			if (!(gameObject == null) && !(parent == null))
			{
				PlayAttachedInternal(key, gameObject, parent, localOffset);
			}
		}

		public void Warmup(string key, int count)
		{
			WarmupAsync(key, count).Forget();
		}

		public void ClearAllPools()
		{
			foreach (KeyValuePair<int, PooledParticle> activeHandle in _activeHandles)
			{
				if (activeHandle.Value != null)
				{
					activeHandle.Value.Deactivate();
				}
			}
			_activeHandles.Clear();
			foreach (KeyValuePair<string, ParticlePool> pool in _pools)
			{
				foreach (PooledParticle item in pool.Value.All)
				{
					if (item != null)
					{
						Object.Destroy(item.gameObject);
					}
				}
			}
			_pools.Clear();
		}

		public void ReturnToPool(PooledParticle pooled)
		{
			_activeHandles.Remove(pooled.ActiveId);
			pooled.Deactivate();
			ReturnToPoolInternal(pooled);
		}

		private void ReturnToPoolInternal(PooledParticle pooled)
		{
			if (_pools.TryGetValue(pooled.Key, out var value))
			{
				pooled.transform.SetParent(base.transform);
				value.Available.Enqueue(pooled);
			}
		}

		private async UniTaskVoid WarmupAsync(string key, int count)
		{
			GameObject gameObject = await GetOrLoadPrefab(key);
			if (!(gameObject == null))
			{
				ParticlePool orCreatePool = GetOrCreatePool(key, gameObject);
				for (int i = orCreatePool.Available.Count; i < count; i++)
				{
					PooledParticle item = CreatePooledInstance(key, gameObject, orCreatePool);
					orCreatePool.Available.Enqueue(item);
				}
			}
		}

		private PooledParticle GetFromPool(string key, GameObject prefab)
		{
			ParticlePool orCreatePool = GetOrCreatePool(key, prefab);
			if (orCreatePool.Available.Count > 0)
			{
				PooledParticle pooledParticle = orCreatePool.Available.Dequeue();
				if (pooledParticle != null)
				{
					return pooledParticle;
				}
			}
			return CreatePooledInstance(key, prefab, orCreatePool);
		}

		private ParticlePool GetOrCreatePool(string key, GameObject prefab)
		{
			if (_pools.TryGetValue(key, out var value))
			{
				return value;
			}
			value = new ParticlePool
			{
				Key = key,
				Prefab = prefab
			};
			_pools[key] = value;
			return value;
		}

		private PooledParticle CreatePooledInstance(string key, GameObject prefab, ParticlePool pool)
		{
			GameObject obj = Object.Instantiate(prefab, base.transform);
			obj.name = "[Pool] " + key;
			obj.SetActive(value: false);
			PooledParticle pooledParticle = obj.AddComponent<PooledParticle>();
			pooledParticle.Initialize(key, this);
			pool.All.Add(pooledParticle);
			return pooledParticle;
		}

		private async UniTask<GameObject> GetOrLoadPrefab(string key)
		{
			if (_prefabCache.TryGetValue(key, out var value))
			{
				return value;
			}
			if (_loadingKeys.Contains(key))
			{
				await UniTask.WaitUntil(() => !_loadingKeys.Contains(key));
				GameObject value2;
				return _prefabCache.TryGetValue(key, out value2) ? value2 : null;
			}
			_loadingKeys.Add(key);
			try
			{
				return await LoadPrefabAsync(key);
			}
			finally
			{
				_loadingKeys.Remove(key);
			}
		}

		private async UniTask<GameObject> LoadPrefabAsync(string key)
		{
			if (!_database.TryGetEntry(key, out var entry))
			{
				return null;
			}
			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(entry.address);
			await handle.ToUniTask();
			if (handle.Status != AsyncOperationStatus.Succeeded)
			{
				EvilLogger.LogError("[ParticlesManager] Failed to load particle prefab: " + key, "LoadPrefabAsync", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\_Core\\Particle\\ParticlesManager.cs", 302);
				return null;
			}
			GameObject result = handle.Result;
			_prefabCache[key] = result;
			return result;
		}
	}
}
