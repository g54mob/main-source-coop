using System;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtGameObject
	{
		public static bool IsInLayer(this GameObject me, LayerMask layerMask)
		{
			return ((1 << me.layer) & (int)layerMask) != 0;
		}

		public static void SetLayer(this GameObject me, int layer, bool includeChildren = false)
		{
			me.layer = layer;
			if (!includeChildren)
			{
				return;
			}
			foreach (Transform item in me.transform)
			{
				item.gameObject.SetLayer(layer, includeChildren);
			}
		}

		public static T PGetComponentInChildrenButNotMe<T>(this GameObject me, bool includeInActive = false) where T : MonoBehaviour
		{
			T[] componentsInChildren = me.GetComponentsInChildren<T>(includeInActive);
			foreach (T val in componentsInChildren)
			{
				if (val.gameObject != me)
				{
					return val;
				}
			}
			return null;
		}

		public static bool _g<T>(this GameObject me, out T target) where T : Component
		{
			try
			{
				target = me.GetComponent<T>();
			}
			catch (ArgumentException ex)
			{
				Debug.Log("Cant find " + typeof(T)?.ToString() + "   " + ex);
				target = null;
				return false;
			}
			return true;
		}

		public static bool _gp<T>(this GameObject me, out T target)
		{
			target = me.GetComponentInParent<T>();
			return target != null;
		}

		public static bool _gc<T>(this GameObject me, out T target)
		{
			target = me.GetComponentInChildren<T>();
			return target != null;
		}

		private static bool CompareGOS(GameObject[] go1, GameObject[] go2)
		{
			if (go2 == null)
			{
				return false;
			}
			if (go1.Length != go2.Length)
			{
				return false;
			}
			for (int i = 0; i < go1.Length; i++)
			{
				if (!(go1[i] == go2[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
