using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zorro.Core
{
	public static class TransformExtensions
	{
		public static List<Transform> GetChildrenRecursive(this Transform transform, List<Transform> list = null)
		{
			if (list == null)
			{
				list = new List<Transform>();
			}
			foreach (Transform item in transform)
			{
				list.Add(item);
				list.AddRange(item.GetChildrenRecursive());
			}
			return list;
		}

		public static Transform FindChildRecursive(this Transform transform, string name)
		{
			Transform transform2 = transform.Find(name);
			if (transform2 != null)
			{
				return transform2;
			}
			foreach (Transform item in transform)
			{
				transform2 = item.FindChildRecursive(name);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return null;
		}

		public static void ClearChildren(this Transform transform)
		{
			foreach (Transform item in transform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		public static List<Transform> FindChildrenRecursiveWithSelector(this Transform transform, Func<Transform, bool> selector)
		{
			List<Transform> list = new List<Transform>();
			foreach (Transform item in transform)
			{
				if (selector(item))
				{
					list.Add(item);
				}
				FindChildrenRecursive(item, selector, list);
			}
			return list;
			static void FindChildrenRecursive(Transform transform3, Func<Transform, bool> func, List<Transform> list2)
			{
				foreach (Transform item2 in transform3)
				{
					if (func(item2))
					{
						list2.Add(item2);
					}
					FindChildrenRecursive(item2, func, list2);
				}
			}
		}

		public static void HasComponent<T>(this Transform transform, Action<T> onHas) where T : Component
		{
			T component = transform.GetComponent<T>();
			if (component != null)
			{
				onHas?.Invoke(component);
			}
		}

		public static bool TryGetComponentInChildren<T>(this Transform transform, out T component) where T : Component
		{
			component = transform.GetComponentInChildren<T>();
			return component != null;
		}
	}
}
