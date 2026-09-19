using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public interface IBeachPrefabSpawnService
	{
		BeachPrefabSpawnHandle CreateHandle();

		GameObject Spawn(BeachPrefabSpawnHandle handle, GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale, Transform parent = null);

		void Clear(BeachPrefabSpawnHandle handle);
	}
}
