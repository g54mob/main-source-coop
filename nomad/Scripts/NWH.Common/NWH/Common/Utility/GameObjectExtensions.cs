using UnityEngine;

namespace NWH.Common.Utility
{
	public static class GameObjectExtensions
	{
		public static Bounds FindBoundsIncludeChildren(this GameObject gameObject)
		{
			Bounds result = default(Bounds);
			MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer meshRenderer in componentsInChildren)
			{
				result.Encapsulate(meshRenderer.bounds);
			}
			return result;
		}

		public static T GetComponentInParent<T>(this Transform transform, bool includeInactive = true) where T : Component
		{
			Transform transform2 = transform;
			T val = null;
			while ((bool)transform2 && !val)
			{
				if (includeInactive || transform2.gameObject.activeSelf)
				{
					val = transform2.GetComponent<T>();
				}
				transform2 = transform2.parent;
			}
			return val;
		}

		public static T GetComponentInParentsOrChildren<T>(this Transform transform, bool includeInactive = true) where T : Component
		{
			T val = transform.GetComponentInParent<T>(includeInactive);
			if (val == null)
			{
				val = transform.GetComponentInChildren<T>(includeInactive);
			}
			return val;
		}
	}
}
