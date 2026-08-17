using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIEffectInternal
{
	internal static class ComponentExtensions
	{
		public static T[] GetComponentsInChildren<T>(this Component self, int depth) where T : Component
		{
			List<T> toRelease = InternalListPool<T>.Rent();
			self.GetComponentsInChildren_Internal(toRelease, depth);
			T[] result = toRelease.ToArray();
			InternalListPool<T>.Return(ref toRelease);
			return result;
		}

		public static void GetComponentsInChildren<T>(this Component self, List<T> results, int depth) where T : Component
		{
			results.Clear();
			self.GetComponentsInChildren_Internal(results, depth);
		}

		private static void GetComponentsInChildren_Internal<T>(this Component self, List<T> results, int depth) where T : Component
		{
			if (!self || results == null || depth < 0)
			{
				return;
			}
			Transform transform = self.transform;
			if (transform.TryGetComponent<T>(out var component))
			{
				results.Add(component);
			}
			if (depth - 1 >= 0)
			{
				int childCount = transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					transform.GetChild(i).GetComponentsInChildren_Internal(results, depth - 1);
				}
			}
		}

		public static T GetOrAddComponent<T>(this Component self) where T : Component
		{
			if (!self)
			{
				return null;
			}
			if (!self.TryGetComponent<T>(out var component))
			{
				return self.gameObject.AddComponent<T>();
			}
			return component;
		}

		public static T GetRootComponent<T>(this Component self) where T : Component
		{
			T result = null;
			Transform transform = self.transform;
			while ((bool)transform)
			{
				if (transform.TryGetComponent<T>(out var component))
				{
					result = component;
				}
				transform = transform.parent;
			}
			return result;
		}

		public static T GetComponentInParent<T>(this Component self, bool includeSelf, Transform stopAfter, Predicate<T> valid) where T : Component
		{
			Transform transform = (includeSelf ? self.transform : self.transform.parent);
			while ((bool)transform)
			{
				if (transform.TryGetComponent<T>(out var component) && valid(component))
				{
					return component;
				}
				if (transform == stopAfter)
				{
					return null;
				}
				transform = transform.parent;
			}
			return null;
		}

		public static void AddComponentOnChildren<T>(this Component self, HideFlags hideFlags, bool includeSelf) where T : Component
		{
			if (self == null)
			{
				return;
			}
			if (includeSelf && !self.TryGetComponent<T>(out var component))
			{
				self.gameObject.AddComponent<T>().hideFlags = hideFlags;
			}
			int childCount = self.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = self.transform.GetChild(i);
				if (!child.TryGetComponent<T>(out component))
				{
					child.gameObject.AddComponent<T>().hideFlags = hideFlags;
				}
			}
		}

		public static void AddComponentOnChildren<T>(this Component self, bool includeSelf) where T : Component
		{
			if (self == null)
			{
				return;
			}
			if (includeSelf && !self.TryGetComponent<T>(out var component))
			{
				self.gameObject.AddComponent<T>();
			}
			int childCount = self.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = self.transform.GetChild(i);
				if (!child.TryGetComponent<T>(out component))
				{
					child.gameObject.AddComponent<T>();
				}
			}
		}
	}
}
