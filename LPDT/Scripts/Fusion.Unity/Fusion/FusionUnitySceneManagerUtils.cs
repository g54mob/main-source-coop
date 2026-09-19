using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

namespace Fusion
{
	public static class FusionUnitySceneManagerUtils
	{
		public class SceneEqualityComparer : IEqualityComparer<Scene>
		{
			public bool Equals(Scene x, Scene y)
			{
				return x.handle == y.handle;
			}

			public int GetHashCode(Scene obj)
			{
				return obj.handle.GetHashCode();
			}
		}

		private static readonly List<GameObject> _reusableGameObjectList = new List<GameObject>();

		public static bool IsAddedToBuildSettings(this Scene scene)
		{
			if (scene.buildIndex < 0)
			{
				return false;
			}
			if (scene.buildIndex >= SceneManager.sceneCountInBuildSettings)
			{
				return false;
			}
			return true;
		}

		public static bool CanBeUnloaded(this Scene scene)
		{
			if (!scene.isLoaded)
			{
				return false;
			}
			for (int i = 0; i < SceneManager.sceneCount; i++)
			{
				Scene sceneAt = SceneManager.GetSceneAt(i);
				if (sceneAt != scene && sceneAt.isLoaded)
				{
					return true;
				}
			}
			return false;
		}

		public static ulong GetRawHandle(this Scene scene)
		{
			return (ulong)scene.handle;
		}

		public static int CompareRawHandle(this Scene scene, int id)
		{
			ulong value = (ulong)id;
			return scene.GetRawHandle().CompareTo(value);
		}

		public static string Dump(this Scene scene)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[UnityScene:");
			if (scene.IsValid())
			{
				stringBuilder.Append(scene.name);
				stringBuilder.Append(", isLoaded:").Append(scene.isLoaded);
				stringBuilder.Append(", buildIndex:").Append(scene.buildIndex);
				stringBuilder.Append(", isDirty:").Append(scene.isDirty);
				stringBuilder.Append(", path:").Append(scene.path);
				stringBuilder.Append(", rootCount:").Append(scene.rootCount);
				stringBuilder.Append(", isSubScene:").Append(scene.isSubScene);
			}
			else
			{
				stringBuilder.Append("<Invalid>");
			}
			stringBuilder.Append(", handle:").Append(scene.handle.ToString());
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		public static string Dump(this LoadSceneParameters loadSceneParameters)
		{
			return $"[LoadSceneParameters: {loadSceneParameters.loadSceneMode}, localPhysicsMode:{loadSceneParameters.localPhysicsMode}]";
		}

		public static int GetSceneBuildIndex(string nameOrPath)
		{
			if (nameOrPath.IndexOf('/', StringComparison.Ordinal) >= 0)
			{
				return SceneUtility.GetBuildIndexByScenePath(nameOrPath);
			}
			for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
			{
				if (MemoryExtensions.Equals(GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)), nameOrPath, StringComparison.OrdinalIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		public static void GetRootGameObjectsInHierarchyOrder(this Scene scene, List<GameObject> rootGameObjects)
		{
			scene.GetRootGameObjects(rootGameObjects ?? throw new ArgumentNullException("rootGameObjects"));
			rootGameObjects.Sort((GameObject a, GameObject b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
		}

		public static T GetSingleComponentOrThrow<T>(this Scene scene, bool includeInactive = false) where T : Component
		{
			List<GameObject> value;
			using (CollectionPool<List<GameObject>, GameObject>.Get(out value))
			{
				scene.GetRootGameObjects(value);
				T val = null;
				foreach (GameObject item in value)
				{
					if (!includeInactive && !item.activeInHierarchy)
					{
						continue;
					}
					T componentInChildren = item.GetComponentInChildren<T>(includeInactive);
					if ((bool)componentInChildren)
					{
						if ((bool)val)
						{
							throw new InvalidOperationException("Multiple components of type " + typeof(T).FullName + " found");
						}
						val = componentInChildren;
					}
				}
				if (val == null)
				{
					throw new InvalidOperationException("Components of type " + typeof(T).FullName + " not found");
				}
				return val;
			}
		}

		public static T GetComponentInHierarchyOrder<T>(this Scene scene, bool includeInactive = false) where T : class
		{
			List<GameObject> value;
			using (CollectionPool<List<GameObject>, GameObject>.Get(out value))
			{
				scene.GetRootGameObjectsInHierarchyOrder(value);
				foreach (GameObject item in value)
				{
					if (includeInactive || item.activeInHierarchy)
					{
						T componentInChildren = item.GetComponentInChildren<T>(includeInactive);
						if (componentInChildren != null)
						{
							return componentInChildren;
						}
					}
				}
				return null;
			}
		}

		public static T[] GetComponentsInHierarchyOrder<T>(this Scene scene, bool includeInactive = false) where T : class
		{
			List<GameObject> value;
			using (CollectionPool<List<GameObject>, GameObject>.Get(out value))
			{
				scene.GetRootGameObjectsInHierarchyOrder(value);
				return GetComponentsInHierarchyOrder<T>(value, includeInactive);
			}
		}

		public static T[] GetComponentsInHierarchyOrder<T>(IList<GameObject> roots, bool includeInactive = false) where T : class
		{
			List<T> value;
			using (CollectionPool<List<T>, T>.Get(out value))
			{
				List<T> value2;
				using (CollectionPool<List<T>, T>.Get(out value2))
				{
					foreach (GameObject root in roots)
					{
						if (includeInactive || root.activeInHierarchy)
						{
							value.Clear();
							root.GetComponentsInChildren(includeInactive, value);
							value2.AddRange(value);
						}
					}
					return value2.ToArray();
				}
			}
		}

		internal static ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> nameOrPath)
		{
			int num = nameOrPath.LastIndexOf('/');
			int num2 = 0;
			num2 = ((num >= 0) ? (num + 1) : 0);
			int num3 = nameOrPath.LastIndexOf('.');
			if (num3 >= num2)
			{
				return nameOrPath.Slice(num2, num3 - num2);
			}
			return nameOrPath.Slice(num2);
		}

		public static LocalPhysicsMode GetLocalPhysicsMode(this Scene scene)
		{
			LocalPhysicsMode localPhysicsMode = LocalPhysicsMode.None;
			if (scene.GetPhysicsScene() != Physics.defaultPhysicsScene)
			{
				localPhysicsMode |= LocalPhysicsMode.Physics3D;
			}
			if (scene.GetPhysicsScene2D() != Physics2D.defaultPhysicsScene)
			{
				localPhysicsMode |= LocalPhysicsMode.Physics2D;
			}
			return localPhysicsMode;
		}

		public static T[] GetComponents<T>(this Scene scene, bool includeInactive) where T : Component
		{
			GameObject[] rootObjects;
			return scene.GetComponents<T>(includeInactive, out rootObjects);
		}

		public static T[] GetComponents<T>(this Scene scene, bool includeInactive, out GameObject[] rootObjects) where T : Component
		{
			rootObjects = scene.GetRootGameObjects();
			List<T> list = new List<T>();
			List<T> list2 = new List<T>();
			GameObject[] array = rootObjects;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponentsInChildren(includeInactive, list);
				foreach (T item in list)
				{
					list2.Add(item);
				}
			}
			return list2.ToArray();
		}

		public static void GetComponents<T>(this Scene scene, List<T> results, bool includeInactive) where T : Component
		{
			List<GameObject> reusableGameObjectList = _reusableGameObjectList;
			scene.GetRootGameObjects(reusableGameObjectList);
			results.Clear();
			List<T> list = new List<T>();
			foreach (GameObject item in reusableGameObjectList)
			{
				item.GetComponentsInChildren(includeInactive, list);
				foreach (T item2 in list)
				{
					results.Add(item2);
				}
			}
		}

		public static T FindComponent<T>(this Scene scene, bool includeInactive = false) where T : Component
		{
			List<GameObject> reusableGameObjectList = _reusableGameObjectList;
			scene.GetRootGameObjects(reusableGameObjectList);
			foreach (GameObject item in reusableGameObjectList)
			{
				T componentInChildren = item.GetComponentInChildren<T>(includeInactive);
				if (componentInChildren != null)
				{
					return componentInChildren;
				}
			}
			return null;
		}

		public static int GetSceneIndex(IList<string> scenePathsOrNames, string nameOrPath)
		{
			if (nameOrPath.IndexOf('/') >= 0)
			{
				int num = scenePathsOrNames.IndexOf(nameOrPath);
				if (num != -1)
				{
					return num;
				}
				GetFileNameWithoutExtensionPosition(nameOrPath, out var index, out var length);
				for (int i = 0; i < scenePathsOrNames.Count; i++)
				{
					string text = scenePathsOrNames[i];
					if (text.Length == length && string.Compare(text, 0, nameOrPath, index, length, ignoreCase: true) == 0)
					{
						return i;
					}
				}
				return -1;
			}
			for (int j = 0; j < scenePathsOrNames.Count; j++)
			{
				string text2 = scenePathsOrNames[j];
				GetFileNameWithoutExtensionPosition(text2, out var index2, out var length2);
				if (length2 == nameOrPath.Length && string.Compare(text2, index2, nameOrPath, 0, length2, ignoreCase: true) == 0)
				{
					return j;
				}
			}
			return -1;
		}

		public static void GetFileNameWithoutExtensionPosition(string nameOrPath, out int index, out int length)
		{
			int num = nameOrPath.LastIndexOf('/');
			if (num >= 0)
			{
				index = num + 1;
			}
			else
			{
				index = 0;
			}
			int num2 = nameOrPath.LastIndexOf('.');
			if (num2 > index)
			{
				length = num2 - index;
			}
			else
			{
				length = nameOrPath.Length - index;
			}
		}
	}
}
