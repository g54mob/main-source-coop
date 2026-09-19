using Features.BeachPresetModule.Scripts.Core.Interfaces;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Services
{
	public class BeachPrefabSpawnService : IBeachPrefabSpawnService
	{
		public BeachPrefabSpawnHandle CreateHandle()
		{
			return new BeachPrefabSpawnHandle();
		}

		public GameObject Spawn(BeachPrefabSpawnHandle handle, GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale, Transform parent = null)
		{
			if (handle == null || prefab == null)
			{
				return null;
			}
			Transform parent2 = ((parent != null) ? parent : GetRuntimeRoot(handle));
			GameObject gameObject = Object.Instantiate(prefab, position, rotation, parent2);
			gameObject.transform.localScale = localScale;
			handle.SpawnedObjects.Add(gameObject);
			return gameObject;
		}

		public void Clear(BeachPrefabSpawnHandle handle)
		{
			if (handle == null)
			{
				return;
			}
			for (int num = handle.SpawnedObjects.Count - 1; num >= 0; num--)
			{
				if (handle.SpawnedObjects[num] != null)
				{
					Object.Destroy(handle.SpawnedObjects[num]);
				}
			}
			handle.SpawnedObjects.Clear();
		}

		private static Transform GetRuntimeRoot(BeachPrefabSpawnHandle handle)
		{
			if (handle.RuntimeRoot != null)
			{
				return handle.RuntimeRoot;
			}
			GameObject gameObject = new GameObject("Beach Preset Runtime Effects");
			handle.RuntimeRoot = gameObject.transform;
			return handle.RuntimeRoot;
		}
	}
}
