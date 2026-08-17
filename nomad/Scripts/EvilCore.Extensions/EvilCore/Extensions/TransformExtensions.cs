using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EvilCore.Extensions
{
	public static class TransformExtensions
	{
		public static void SetPositionX(this Transform transform, float x)
		{
			transform.position = transform.position.WithX(x);
		}

		public static void SetPositionY(this Transform transform, float y)
		{
			transform.position = transform.position.WithY(y);
		}

		public static void SetPositionZ(this Transform transform, float z)
		{
			transform.position = transform.position.WithZ(z);
		}

		public static void SetPositionXY(this Transform transform, float x, float y)
		{
			transform.position = transform.position.WithXY(x, y);
		}

		public static void SetPositionXY(this Transform transform, Vector2 position)
		{
			transform.position = transform.position.WithXY(position);
		}

		public static void SetPositionXZ(this Transform transform, float x, float z)
		{
			transform.position = transform.position.WithXZ(x, z);
		}

		public static void SetPositionXZ(this Transform transform, Vector2 position)
		{
			transform.position = transform.position.WithXZ(position);
		}

		public static void SetPositionYZ(this Transform transform, float y, float z)
		{
			transform.position = transform.position.WithYZ(y, z);
		}

		public static void SetPositionYZ(this Transform transform, Vector2 position)
		{
			transform.position = transform.position.WithYZ(position);
		}

		public static void SetLocalPositionX(this Transform transform, float x)
		{
			transform.localPosition = transform.localPosition.WithX(x);
		}

		public static void SetLocalPositionY(this Transform transform, float y)
		{
			transform.localPosition = transform.localPosition.WithY(y);
		}

		public static void SetLocalPositionZ(this Transform transform, float z)
		{
			transform.localPosition = transform.localPosition.WithZ(z);
		}

		public static void SetLocalPositionXY(this Transform transform, float x, float y)
		{
			transform.localPosition = transform.localPosition.WithXY(x, y);
		}

		public static void SetLocalPositionXY(this Transform transform, Vector2 position)
		{
			transform.localPosition = transform.localPosition.WithXY(position);
		}

		public static void SetLocalPositionXZ(this Transform transform, float x, float z)
		{
			transform.localPosition = transform.localPosition.WithXZ(x, z);
		}

		public static void SetLocalPositionXZ(this Transform transform, Vector2 position)
		{
			transform.localPosition = transform.localPosition.WithXZ(position);
		}

		public static void SetLocalPositionYZ(this Transform transform, float y, float z)
		{
			transform.localPosition = transform.localPosition.WithYZ(y, z);
		}

		public static void SetLocalPositionYZ(this Transform transform, Vector2 position)
		{
			transform.localPosition = transform.localPosition.WithYZ(position);
		}

		public static void SetEulerAnglesX(this Transform transform, float x)
		{
			transform.eulerAngles = transform.eulerAngles.WithX(x);
		}

		public static void SetEulerAnglesY(this Transform transform, float y)
		{
			transform.eulerAngles = transform.eulerAngles.WithY(y);
		}

		public static void SetEulerAnglesZ(this Transform transform, float z)
		{
			transform.eulerAngles = transform.eulerAngles.WithZ(z);
		}

		public static void SetEulerAnglesXY(this Transform transform, float x, float y)
		{
			transform.eulerAngles = transform.eulerAngles.WithXY(x, y);
		}

		public static void SetEulerAnglesXY(this Transform transform, Vector2 eulers)
		{
			transform.eulerAngles = transform.eulerAngles.WithXY(eulers);
		}

		public static void SetEulerAnglesXZ(this Transform transform, float x, float z)
		{
			transform.eulerAngles = transform.eulerAngles.WithXZ(x, z);
		}

		public static void SetEulerAnglesXZ(this Transform transform, Vector2 eulers)
		{
			transform.eulerAngles = transform.eulerAngles.WithXZ(eulers);
		}

		public static void SetEulerAnglesYZ(this Transform transform, float y, float z)
		{
			transform.eulerAngles = transform.eulerAngles.WithYZ(y, z);
		}

		public static void SetEulerAnglesYZ(this Transform transform, Vector2 eulers)
		{
			transform.eulerAngles = transform.eulerAngles.WithYZ(eulers);
		}

		public static void SetLocalEulerAnglesX(this Transform transform, float x)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithX(x);
		}

		public static void SetLocalEulerAnglesY(this Transform transform, float y)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithY(y);
		}

		public static void SetLocalEulerAnglesZ(this Transform transform, float z)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithZ(z);
		}

		public static void SetLocalEulerAnglesXY(this Transform transform, float x, float y)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithXY(x, y);
		}

		public static void SetLocalEulerAnglesXY(this Transform transform, Vector2 eulers)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithXY(eulers);
		}

		public static void SetLocalEulerAnglesXZ(this Transform transform, float x, float z)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithXZ(x, z);
		}

		public static void SetLocalEulerAnglesXZ(this Transform transform, Vector2 eulers)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithXZ(eulers);
		}

		public static void SetLocalEulerAnglesYZ(this Transform transform, float y, float z)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithYZ(y, z);
		}

		public static void SetLocalEulerAnglesYZ(this Transform transform, Vector2 eulers)
		{
			transform.localEulerAngles = transform.localEulerAngles.WithYZ(eulers);
		}

		public static void SetLocalScaleX(this Transform transform, float x)
		{
			transform.localScale = transform.localScale.WithX(x);
		}

		public static void SetLocalScaleY(this Transform transform, float y)
		{
			transform.localScale = transform.localScale.WithY(y);
		}

		public static void SetLocalScaleZ(this Transform transform, float z)
		{
			transform.localScale = transform.localScale.WithZ(z);
		}

		public static void SetLocalScaleXY(this Transform transform, float x, float y)
		{
			transform.localScale = transform.localScale.WithXY(x, y);
		}

		public static void SetLocalScaleXY(this Transform transform, Vector2 scale)
		{
			transform.localScale = transform.localScale.WithXY(scale);
		}

		public static void SetLocalScaleXZ(this Transform transform, float x, float z)
		{
			transform.localScale = transform.localScale.WithXZ(x, z);
		}

		public static void SetLocalScaleXZ(this Transform transform, Vector2 scale)
		{
			transform.localScale = transform.localScale.WithXZ(scale);
		}

		public static void SetLocalScaleYZ(this Transform transform, float y, float z)
		{
			transform.localScale = transform.localScale.WithYZ(y, z);
		}

		public static void SetLocalScaleYZ(this Transform transform, Vector2 scale)
		{
			transform.localScale = transform.localScale.WithYZ(scale);
		}

		public static void SetLocalScale(this Transform transform, float scale)
		{
			Vector3 localScale = (transform.localScale = new Vector3(scale, scale, scale));
			transform.localScale = localScale;
		}

		public static void Reset(this Transform transform, bool useWorldSpace = false)
		{
			if (useWorldSpace)
			{
				transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
			}
			else
			{
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
			}
			transform.localScale = Vector3.one;
		}

		public static void SetToPreviousSibling(this Transform transform)
		{
			transform.SetSiblingIndex(Mathf.Max(transform.GetSiblingIndex() - 1, 0));
		}

		public static void SetToNextSibling(this Transform transform)
		{
			int b = ((transform.parent != null) ? transform.parent.childCount : transform.gameObject.scene.rootCount) - 1;
			transform.SetSiblingIndex(Mathf.Min(transform.GetSiblingIndex() + 1, b));
		}

		public static Transform GetPreviousSiblingTransform(this Transform transform)
		{
			if (transform.GetSiblingIndex() == 0)
			{
				return null;
			}
			if ((bool)transform.parent)
			{
				return transform.parent.GetChild(transform.GetSiblingIndex() - 1);
			}
			return transform.gameObject.scene.GetRootGameObjects()[transform.GetSiblingIndex() - 1].transform;
		}

		public static Transform GetNextSiblingTransform(this Transform transform)
		{
			if ((bool)transform.parent)
			{
				if (transform.GetSiblingIndex() == transform.parent.childCount - 1)
				{
					return null;
				}
				return transform.parent.GetChild(transform.GetSiblingIndex() + 1);
			}
			if (transform.GetSiblingIndex() == transform.gameObject.scene.rootCount - 1)
			{
				return null;
			}
			return transform.gameObject.scene.GetRootGameObjects()[transform.GetSiblingIndex() + 1].transform;
		}

		public static List<Transform> GetAllSiblingObjects(this Transform transform, bool includeThis = true)
		{
			List<Transform> list;
			if ((bool)transform.parent)
			{
				list = new List<Transform>(transform.parent.childCount);
				for (int i = 0; i < transform.parent.childCount; i++)
				{
					Transform child = transform.parent.GetChild(i);
					if (includeThis || (!includeThis && child != transform))
					{
						list.Add(child);
					}
				}
			}
			else
			{
				GameObject[] rootGameObjects = transform.gameObject.scene.GetRootGameObjects();
				list = new List<Transform>(rootGameObjects.Length);
				for (int j = 0; j < rootGameObjects.Length; j++)
				{
					Transform transform2 = rootGameObjects[j].transform;
					if (includeThis || (!includeThis && transform2 != transform))
					{
						list.Add(transform2);
					}
				}
			}
			return list;
		}

		public static List<Transform> GetChilds(this Transform transform)
		{
			return transform.Cast<Transform>().ToList();
		}

		public static Transform GetRandomChild(this Transform transform)
		{
			return transform.GetChild(UnityEngine.Random.Range(0, transform.childCount));
		}

		public static void AddChilds(this Transform transform, params Transform[] childs)
		{
			transform.AddChilds((IEnumerable<Transform>)childs);
		}

		public static void AddChilds(this Transform transform, IEnumerable<Transform> childs)
		{
			foreach (Transform child in childs)
			{
				child.parent = transform;
			}
		}

		public static void DestroyChilds(this Transform transform)
		{
			transform.GetChilds().ForEach(delegate(Transform child)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			});
		}

		public static void DestroyChildsWhere(this Transform transform, Predicate<Transform> predicate)
		{
			foreach (Transform item in from c in transform.GetChilds()
				where predicate(c)
				select c)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}

		public static void DestroyChild(this Transform transform, int index)
		{
			UnityEngine.Object.Destroy(transform.GetChild(index).gameObject);
		}

		public static void DestroyFirstChild(this Transform transform)
		{
			transform.DestroyChild(0);
		}

		public static void DestroyLastChild(this Transform transform)
		{
			transform.DestroyChild(transform.childCount - 1);
		}

		public static float GetLowestPoint<T>(this Transform origin) where T : Collider
		{
			return origin.GetComponent<T>().bounds.min.y;
		}
	}
}
