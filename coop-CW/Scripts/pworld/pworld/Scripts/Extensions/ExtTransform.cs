using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace pworld.Scripts.Extensions
{
	public static class ExtTransform
	{
		public static float3 PInverseTransformPointF3(this Transform me, float3 point)
		{
			return me.TransformPoint(point);
		}

		public static List<GameObject> PGetChildGameObjects(this GameObject me)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (Transform item in me.transform)
			{
				list.Add(item.gameObject);
			}
			return list;
		}

		public static List<T> PGetComponentsInFistBorn<T>(this GameObject me) where T : Component
		{
			List<T> list = new List<T>();
			foreach (Transform item in me.transform)
			{
				if (item.gameObject.TryGetComponent<T>(out var component))
				{
					list.Add(component);
				}
			}
			if (list.Count <= 0)
			{
				return null;
			}
			return list;
		}

		public static void PScaleAroundRelative(this Transform me, Vector3 pivot, Vector3 scaleFactor)
		{
			Vector3 vector = me.localPosition - pivot;
			vector.Scale(scaleFactor);
			me.localPosition = pivot + vector;
			Vector3 localScale = me.localScale;
			localScale.Scale(scaleFactor);
			me.localScale = localScale;
		}

		public static void PScaleAround(this Transform me, Vector3 pivot, Vector3 newScale)
		{
			Vector3 vector = me.localPosition - pivot;
			Vector3 scale = new Vector3(newScale.x / me.localScale.x, newScale.y / me.localScale.y, newScale.z / me.localScale.z);
			vector.Scale(scale);
			me.localPosition = pivot + vector;
			me.localScale = newScale;
		}

		public static void LookWithUp(this Transform me, Vector3 worldPosition)
		{
			Vector3 vector = worldPosition - me.position;
			Vector3 forward = Vector3.Cross(vector, me.TransformPoint(me.forward));
			me.rotation = Quaternion.LookRotation(forward, vector);
		}

		public static void ScaleToSizeInLocalOf(this Transform me, Vector3 scale, Transform target)
		{
			Vector3 vector = target.transform.TransformVector(scale);
			me.localScale = me.parent.InverseTransformVector(vector);
		}

		public static void SetPosInLocalOf(this Transform me, Vector3 localPos, Transform target)
		{
			Vector3 position = target.transform.TransformPoint(localPos);
			me.position = position;
		}

		public static void KillAllChildren(this Transform me, bool destroyImmediate = false, bool onlyIfActive = false)
		{
			for (int num = me.childCount - 1; num >= 0; num--)
			{
				if (!onlyIfActive || me.GetChild(num).gameObject.activeSelf)
				{
					if (!destroyImmediate)
					{
						UnityEngine.Object.Destroy(me.GetChild(num).gameObject);
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(me.GetChild(num).gameObject);
					}
				}
			}
		}

		public static void SetXPos(this Transform me, float x)
		{
			Vector3 position = me.position;
			position.x = x;
			me.position = position;
		}

		public static void SetYPos(this Transform me, float y)
		{
			Vector3 position = me.position;
			position.y = y;
			me.position = position;
		}

		public static void SetZPos(this Transform me, float z)
		{
			Vector3 position = me.position;
			position.z = z;
			me.position = position;
		}

		public static void SetXPosLocal(this Transform me, float x)
		{
			Vector3 localPosition = me.localPosition;
			localPosition.x = x;
			me.localPosition = localPosition;
		}

		public static Transform Find(this Transform me, string childName, bool ifInActive = false)
		{
			if (!ifInActive)
			{
				me.Find(childName);
			}
			else
			{
				Transform[] componentsInChildren = me.gameObject.GetComponentsInChildren<Transform>(includeInactive: true);
				foreach (Transform transform in componentsInChildren)
				{
					if (transform.name == childName)
					{
						return transform;
					}
				}
			}
			return null;
		}

		public static Vector3 TargetUp(this Transform me)
		{
			return Vector3.Cross(me.forward, Vector3.Lerp(Vector3.right, Vector3.forward, Mathf.Abs(me.forward.x)));
		}

		public static void SetYPosLocal(this Transform me, float y)
		{
			Vector3 localPosition = me.localPosition;
			localPosition.y = y;
			me.localPosition = localPosition;
		}

		public static Vector2Int ToIVec(this Vector2 me)
		{
			return new Vector2Int((int)me.x, (int)me.y);
		}

		public static void SetZPosLocal(this Transform me, float z)
		{
			Vector3 localPosition = me.localPosition;
			localPosition.z = z;
			me.localPosition = localPosition;
		}

		public static Vector3 FindClosest(this Transform me, List<Vector3> list)
		{
			if (list.Count < 1)
			{
				throw new Exception("Dont send me empy lists");
			}
			Vector3 result = Vector3.zero;
			float num = float.PositiveInfinity;
			foreach (Vector3 item in list)
			{
				float num2 = Vector3.Distance(me.transform.position, item);
				if (num2 < num)
				{
					result = item;
					num = num2;
				}
			}
			return result;
		}

		public static List<Transform> GetAllSubChilds(this Transform me)
		{
			List<Transform> list = new List<Transform>();
			if (me.childCount == 0)
			{
				return list;
			}
			if (me.childCount > 1)
			{
				Debug.LogWarning("Too many children");
			}
			Transform child = me.GetChild(0);
			list.Add(child);
			list.AddRange(child.GetAllSubChilds());
			return list;
		}

		public static void SetLocalScaleY(this Transform me, float y)
		{
			Vector3 localScale = me.localScale;
			localScale.y = y;
			me.localScale = localScale;
		}

		public static void AdoptChildrenOf(this Transform me, Transform other)
		{
			for (int num = other.childCount - 1; num >= 0; num--)
			{
				other.GetChild(num).SetParent(me, worldPositionStays: false);
			}
		}

		public static void ForEachChild(this Transform me, Action<Transform> action)
		{
			foreach (Transform item in me)
			{
				action(item);
			}
		}

		public static Transform GetLastChild(this Transform me)
		{
			return me.GetChild(me.childCount - 1);
		}
	}
}
