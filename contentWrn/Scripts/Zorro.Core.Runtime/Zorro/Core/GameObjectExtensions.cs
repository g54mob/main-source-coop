using UnityEngine;

namespace Zorro.Core
{
	public static class GameObjectExtensions
	{
		public static void SetLayerRecursivly(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			foreach (Transform item in gameObject.transform)
			{
				item.gameObject.SetLayerRecursivly(layer);
			}
		}

		public static T FetchComponent<T>(this GameObject gameObject) where T : Component
		{
			T val = gameObject.GetComponent<T>();
			if (val == null)
			{
				val = gameObject.AddComponent<T>();
			}
			return val;
		}
	}
}
