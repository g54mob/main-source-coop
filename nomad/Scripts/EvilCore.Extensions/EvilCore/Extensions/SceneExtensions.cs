using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EvilCore.Extensions
{
	public static class SceneExtensions
	{
		public static T FindObjectOfType<T>(this Scene scene, bool includeInactive = false) where T : Component
		{
			foreach (GameObject item in from r in scene.GetRootGameObjects()
				where r.activeSelf || includeInactive
				select r)
			{
				if (item.TryGetComponentInChildren<T>(out var component, includeInactive))
				{
					return component;
				}
			}
			return null;
		}

		public static T[] FindObjectsOfType<T>(this Scene scene, bool includeInactive = false)
		{
			return (from r in scene.GetRootGameObjects()
				where r.activeSelf || includeInactive
				select r).SelectMany((GameObject go) => go.GetComponentsInChildren<T>(includeInactive)).ToArray();
		}

		public static int ObjectsCount(this Scene scene, bool includeInactive = false)
		{
			return (from r in scene.GetRootGameObjects()
				where r.activeSelf || includeInactive
				select r).SelectMany((GameObject r) => r.GetComponentsInChildren<Transform>(includeInactive)).Count();
		}
	}
}
