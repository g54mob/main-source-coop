using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QFSW.QC.Utilities
{
	public static class GameObjectExtensions
	{
		private static readonly Dictionary<string, GameObject> GameObjectCache = new Dictionary<string, GameObject>();

		private static readonly List<GameObject> RootGameObjectBuffer = new List<GameObject>();

		public static GameObject Find(string name, bool includeInactive = false)
		{
			if (GameObjectCache.TryGetValue(name, out var value) && (bool)value && (value.activeInHierarchy || includeInactive) && value.name == name)
			{
				return value;
			}
			value = GameObject.Find(name);
			if ((bool)value)
			{
				return GameObjectCache[name] = value;
			}
			if (includeInactive)
			{
				int sceneCountInBuildSettings = SceneManager.sceneCountInBuildSettings;
				for (int i = 0; i < sceneCountInBuildSettings; i++)
				{
					Scene sceneByBuildIndex = SceneManager.GetSceneByBuildIndex(i);
					if (!sceneByBuildIndex.isLoaded)
					{
						continue;
					}
					RootGameObjectBuffer.Clear();
					sceneByBuildIndex.GetRootGameObjects(RootGameObjectBuffer);
					foreach (GameObject item in RootGameObjectBuffer)
					{
						value = Find(name, item);
						if ((bool)value)
						{
							return GameObjectCache[name] = value;
						}
					}
				}
				value = (from x in Resources.FindObjectsOfTypeAll<GameObject>()
					where !x.hideFlags.HasFlag(HideFlags.HideInHierarchy)
					select x).FirstOrDefault((GameObject x) => x.name == name);
				if ((bool)value)
				{
					return GameObjectCache[name] = value;
				}
			}
			return null;
		}

		public static GameObject Find(string name, GameObject root)
		{
			if (root.name == name)
			{
				return root;
			}
			for (int i = 0; i < root.transform.childCount; i++)
			{
				GameObject gameObject = Find(name, root.transform.GetChild(i).gameObject);
				if ((bool)gameObject)
				{
					return gameObject;
				}
			}
			return null;
		}
	}
}
