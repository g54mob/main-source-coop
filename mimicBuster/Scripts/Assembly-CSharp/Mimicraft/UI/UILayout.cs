using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mimicraft.UI
{
	public static class UILayout
	{
		private static readonly List<RectTransform> scratch = new List<RectTransform>();

		private static readonly List<(RectTransform Rect, int Depth)> layouts = new List<(RectTransform, int)>();

		public static void RebuildFrom(Transform changed)
		{
			RectTransform rectTransform = changed as RectTransform;
			if ((object)rectTransform == null)
			{
				return;
			}
			Transform parent = changed.parent;
			while (parent != null && parent is RectTransform rectTransform2 && !(rectTransform2.GetComponent<Canvas>() != null))
			{
				if (LaysOut(rectTransform2))
				{
					rectTransform = rectTransform2;
				}
				parent = parent.parent;
			}
			RebuildSubtree(rectTransform);
		}

		public static void RebuildSubtree(RectTransform root)
		{
			if (root == null)
			{
				return;
			}
			layouts.Clear();
			Collect(root, 0);
			layouts.Sort(((RectTransform Rect, int Depth) a, (RectTransform Rect, int Depth) b) => b.Depth.CompareTo(a.Depth));
			foreach (var layout in layouts)
			{
				RectTransform item = layout.Rect;
				if (item != null)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(item);
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(root);
		}

		public static void RebuildFromDeferred(MonoBehaviour owner, Transform changed)
		{
			RebuildFrom(changed);
			if (owner != null && owner.isActiveAndEnabled)
			{
				owner.StartCoroutine(RebuildAtEndOfFrame(changed));
			}
		}

		private static IEnumerator RebuildAtEndOfFrame(Transform changed)
		{
			yield return new WaitForEndOfFrame();
			if (changed != null)
			{
				RebuildFrom(changed);
			}
		}

		private static void Collect(RectTransform root, int depth)
		{
			scratch.Clear();
			root.GetComponentsInChildren(includeInactive: true, scratch);
			foreach (RectTransform item in scratch)
			{
				if (item != null && item != root && LaysOut(item))
				{
					layouts.Add((item, Depth(item, root)));
				}
			}
		}

		private static int Depth(Transform rect, Transform root)
		{
			int num = 0;
			Transform transform = rect;
			while (transform != null && transform != root)
			{
				num++;
				transform = transform.parent;
			}
			return num;
		}

		private static bool LaysOut(Component rect)
		{
			if (!(rect.GetComponent<LayoutGroup>() != null))
			{
				return rect.GetComponent<ContentSizeFitter>() != null;
			}
			return true;
		}
	}
}
