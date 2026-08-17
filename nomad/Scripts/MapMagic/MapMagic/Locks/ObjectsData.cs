using System.Collections.Generic;
using Den.Tools;
using Den.Tools.Matrices;
using MapMagic.Nodes;
using MapMagic.Nodes.ObjectsGenerators;
using UnityEngine;

namespace MapMagic.Locks
{
	public class ObjectsData : ILockData
	{
		public Transform lockParent;

		public Dictionary<GameObject, Vector3> lockedObjsPoses = new Dictionary<GameObject, Vector3>();

		public Dictionary<GameObject, Vector3> adjustedObjsPoses = new Dictionary<GameObject, Vector3>();

		public Vector2D center;

		public float radius;

		public float transition;

		public float terrainSize;

		public float terrainHeight;

		private Vector2D min;

		private Vector2D max;

		private bool heightChanged;

		public void Read(Terrain terrain, Lock lk)
		{
			if (terrain.name == "Draft Terrain")
			{
				return;
			}
			Vector2D vector2D = (Vector2D)terrain.transform.parent.parent.position;
			center = (Vector2D)lk.worldPos + vector2D;
			radius = lk.worldRadius;
			transition = lk.worldTransition;
			terrainSize = terrain.terrainData.size.x;
			terrainHeight = terrain.terrainData.size.y;
			min = center - radius - transition;
			max = center + radius + transition;
			string text = "LockedObjects " + lk.guiName;
			if (lockParent == null)
			{
				Transform parent = terrain.transform.parent;
				int childCount = parent.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = parent.GetChild(i);
					if (child.name == text || ((Vector2D)child.localPosition == center && child.name.StartsWith("LockedObject")))
					{
						lockParent = child;
						break;
					}
				}
			}
			if (lockParent == null)
			{
				GameObject gameObject = new GameObject();
				gameObject.transform.parent = terrain.transform.parent;
				gameObject.transform.localPosition = (Vector3)center;
				lockParent = gameObject.transform;
			}
			lockParent.gameObject.name = text;
			lockedObjsPoses.Clear();
			adjustedObjsPoses.Clear();
			int childCount2 = lockParent.childCount;
			for (int j = 0; j < childCount2; j++)
			{
				Transform child2 = lockParent.GetChild(j);
				lockedObjsPoses.Add(child2.gameObject, child2.position - (Vector3)vector2D);
			}
			ObjectsPool componentInChildren = terrain.transform.parent.GetComponentInChildren<ObjectsPool>();
			if (componentInChildren != null)
			{
				for (int num = componentInChildren.transform.childCount - 1; num >= 0; num--)
				{
					Transform child3 = componentInChildren.transform.GetChild(num);
					Vector3 localPosition = child3.localPosition;
					if (!(localPosition.x < min.x) && !(localPosition.x > max.x) && !(localPosition.z < min.z) && !(localPosition.z > max.z))
					{
						float num2 = (center.x - localPosition.x) * (center.x - localPosition.x) + (center.z - localPosition.z) * (center.z - localPosition.z);
						if (num2 < (radius + transition) * (radius + transition))
						{
							componentInChildren.Depool(child3.gameObject);
							if (num2 < radius * radius)
							{
								lockedObjsPoses.Add(child3.gameObject, child3.transform.position - (Vector3)vector2D);
								child3.parent = lockParent;
							}
							else
							{
								Object.DestroyImmediate(child3.gameObject);
							}
						}
					}
				}
			}
			foreach (GameObject key in lockedObjsPoses.Keys)
			{
				key.transform.parent = lockParent;
			}
		}

		public void WriteInThread(IApplyData applyData)
		{
			if (!(applyData is ObjectsOutput.ApplyObjectsData applyObjectsData))
			{
				return;
			}
			for (int i = 0; i < applyObjectsData.transitions.Length; i++)
			{
				List<Transition> list = applyObjectsData.transitions[i];
				_ = list.Count;
				for (int num = list.Count - 1; num >= 0; num--)
				{
					Vector3 pos = list[num].pos;
					if (!(pos.x < min.x) && !(pos.x > max.x) && !(pos.z < min.z) && !(pos.z > max.z) && (center.x - pos.x) * (center.x - pos.x) + (center.z - pos.z) * (center.z - pos.z) < (radius + transition) * (radius + transition))
					{
						list.RemoveAt(num);
					}
				}
			}
		}

		public void WriteInApply(Terrain terrain, bool resizeTerrain = false)
		{
			if (!heightChanged || terrain.name == "Draft Terrain")
			{
				return;
			}
			Dictionary<GameObject, Vector3> dictionary = ((adjustedObjsPoses.Count == 0) ? lockedObjsPoses : adjustedObjsPoses);
			int childCount = lockParent.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = lockParent.GetChild(i);
				if (dictionary.TryGetValue(child.gameObject, out var value))
				{
					child.position = new Vector3(child.position.x, value.y, child.position.z);
				}
			}
			heightChanged = false;
		}

		public void ApplyHeightDelta(Matrix srcHeights, Matrix dstHeights)
		{
			heightChanged = true;
			_ = center - (radius + transition);
			float num = radius * 2f + transition * 2f;
			adjustedObjsPoses.Clear();
			foreach (KeyValuePair<GameObject, Vector3> lockedObjsPose in lockedObjsPoses)
			{
				Vector3 value = lockedObjsPose.Value;
				GameObject key = lockedObjsPose.Key;
				Vector2D vector2D = ((Vector2D)value - center) / num;
				vector2D += 0.5f;
				new Vector2D(vector2D.x * (float)srcHeights.rect.size.x + (float)srcHeights.rect.offset.x, vector2D.z * (float)srcHeights.rect.size.z + (float)srcHeights.rect.offset.z);
				float relative = srcHeights.GetRelative(vector2D.x, vector2D.z);
				float num2 = dstHeights.GetRelative(vector2D.x, vector2D.z) - relative;
				value.y += num2 * terrainHeight;
				adjustedObjsPoses.Add(key, value);
			}
		}

		public void ResizeFrom(ILockData src)
		{
		}

		private static void EnlistObjsWithinRange(Vector3 center, float range, Transform parent, List<(GameObject, float)> list)
		{
			int childCount = parent.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = parent.GetChild(i);
				Vector3 position = child.position;
				if (!(position.x < center.x - range) && !(position.x > center.x + range) && !(position.z < center.z - range) && !(position.z > center.z + range) && Mathf.Sqrt((center.x - position.x) * (center.x - position.x) + (center.z - position.z) * (center.z - position.z)) < range)
				{
					GameObject gameObject = child.gameObject;
					list.Add((gameObject, gameObject.transform.position.y));
				}
			}
		}
	}
}
