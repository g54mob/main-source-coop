using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Object;
using GameKit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FishNet.Utility.Extension
{
	public static class Scenes
	{
		public static void GetSceneNetworkObjects(Scene s, bool firstOnly, bool errorOnDuplicates, ref List<NetworkObject> result)
		{
			if (!s.IsValid() || (!Application.isPlaying && !s.isLoaded))
			{
				return;
			}
			List<NetworkObject> list = CollectionCaches<NetworkObject>.RetrieveList();
			List<NetworkObject> list2 = CollectionCaches<NetworkObject>.RetrieveList();
			List<GameObject> list3 = CollectionCaches<GameObject>.RetrieveList();
			Dictionary<ulong, NetworkObject> sceneIds = CollectionCaches<ulong, NetworkObject>.RetrieveDictionary();
			s.GetRootGameObjects(list3);
			foreach (GameObject item in list3)
			{
				item.GetComponentsInChildren(includeInactive: true, list);
				if (list.Count <= 0)
				{
					continue;
				}
				if (firstOnly)
				{
					foreach (NetworkObject item2 in list)
					{
						item2.GetComponentsInParent(includeInactive: true, list2);
						if (list2.Count == 1 && !TryDisplayDuplicateError(item2))
						{
							result.Add(item2);
						}
					}
					continue;
				}
				foreach (NetworkObject item3 in list)
				{
					if (!TryDisplayDuplicateError(item3))
					{
						result.Add(item3);
					}
				}
			}
			CollectionCaches<ulong, NetworkObject>.Store(sceneIds);
			bool TryDisplayDuplicateError(NetworkObject nob)
			{
				if (!errorOnDuplicates)
				{
					return false;
				}
				if (!nob.IsSceneObject)
				{
					return false;
				}
				ulong sceneId = nob.SceneId;
				if (sceneIds.TryGetValue(sceneId, out var value))
				{
					NetworkManager.StaticLogError($"Object {nob.name} and {value.name} in scene {nob.gameObject.scene.name} have the same sceneId of {sceneId}. This will result in spawning errors. Exit play mode and use the Fish-Networking menu to rebuild sceneIds for scene {nob.gameObject.scene.name}.");
					return true;
				}
				sceneIds[sceneId] = nob;
				return false;
			}
		}
	}
}
