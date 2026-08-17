using System.Collections.Generic;
using UnityEngine;

namespace GameKit.Utilities
{
	public static class Transforms
	{
		public static Vector3 GetOnScreenPosition(this RectTransform rectTransform, Vector3 desiredPosition, Vector2 padding)
		{
			Vector2 vector = new Vector2(rectTransform.localScale.x, rectTransform.localScale.y);
			float num = rectTransform.sizeDelta.x * vector.x / 2f + padding.x;
			float num2 = (float)Screen.width - (desiredPosition.x + num);
			if (num2 < 0f)
			{
				desiredPosition.x += num2;
			}
			num2 = desiredPosition.x - num;
			if (num2 < 0f)
			{
				desiredPosition.x = num;
			}
			float num3 = rectTransform.sizeDelta.y * vector.y / 2f + padding.y;
			num2 = (float)Screen.height - (desiredPosition.y + num3);
			if (num2 < 0f)
			{
				desiredPosition.y += num2;
			}
			num2 = desiredPosition.y - num3;
			if (num2 < 0f)
			{
				desiredPosition.y = num3;
			}
			return desiredPosition;
		}

		public static void SetParentAndKeepTransform(this Transform src, Transform parent)
		{
			Vector3 position = src.position;
			Quaternion rotation = src.rotation;
			Vector3 localScale = src.localScale;
			src.SetParent(parent);
			src.position = position;
			src.rotation = rotation;
			src.localScale = localScale;
		}

		public static void DestroyChildren(this Transform t, bool destroyImmediately = false)
		{
			foreach (Transform item in t)
			{
				if (destroyImmediately)
				{
					Object.DestroyImmediate(item.gameObject);
				}
				else
				{
					Object.Destroy(item.gameObject);
				}
			}
		}

		public static void DestroyChildren<T>(this Transform t, bool destroyImmediately = false) where T : MonoBehaviour
		{
			T[] componentsInChildren = t.GetComponentsInChildren<T>();
			foreach (T val in componentsInChildren)
			{
				if (destroyImmediately)
				{
					Object.DestroyImmediate(val.gameObject);
				}
				else
				{
					Object.Destroy(val.gameObject);
				}
			}
		}

		public static void GetComponentsInChildren<T>(this Transform parent, List<T> results, bool includeParent = true, bool includeInactive = false) where T : Component
		{
			if (!includeParent)
			{
				List<T> list = CollectionCaches<T>.RetrieveList();
				for (int i = 0; i < parent.childCount; i++)
				{
					parent.GetChild(i).GetComponentsInChildren(includeInactive, list);
					results.AddRange(list);
				}
				CollectionCaches<T>.Store(list);
			}
			else
			{
				parent.GetComponentsInChildren(includeInactive, results);
			}
		}

		public static Vector3 GetPosition(this Transform t, bool localSpace)
		{
			if (!localSpace)
			{
				return t.position;
			}
			return t.localPosition;
		}

		public static Quaternion GetRotation(this Transform t, bool localSpace)
		{
			if (!localSpace)
			{
				return t.rotation;
			}
			return t.localRotation;
		}

		public static Vector3 GetScale(this Transform t)
		{
			return t.localScale;
		}

		public static void SetPosition(this Transform t, bool localSpace, Vector3 pos)
		{
			if (localSpace)
			{
				t.localPosition = pos;
			}
			else
			{
				t.position = pos;
			}
		}

		public static void SetRotation(this Transform t, bool localSpace, Quaternion rot)
		{
			if (localSpace)
			{
				t.localRotation = rot;
			}
			else
			{
				t.rotation = rot;
			}
		}

		public static void SetScale(this Transform t, Vector3 scale)
		{
			t.localScale = scale;
		}
	}
}
