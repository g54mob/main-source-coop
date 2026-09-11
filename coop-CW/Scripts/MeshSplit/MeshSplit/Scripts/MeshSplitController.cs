using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MeshSplit.Scripts
{
	public class MeshSplitController : MonoBehaviour
	{
		public static readonly int GridSizeMultiplier = 100;

		private const int GizmosDisplayLimit = 100000;

		public bool Verbose;

		public MeshSplitParameters Parameters;

		public bool DrawGridGizmosWhenSelected;

		private Mesh _baseMesh;

		private MeshRenderer _baseRenderer;

		private List<GameObject> m_children = new List<GameObject>();

		public void Split()
		{
			DestroyChildren();
			if (GetUsedAxisCount() < 1)
			{
				throw new Exception("You have to choose at least 1 axis.");
			}
			MeshFilter component = GetComponent<MeshFilter>();
			if ((bool)component)
			{
				_baseMesh = component.sharedMesh;
				if ((bool)_baseRenderer || TryGetComponent<MeshRenderer>(out _baseRenderer))
				{
					_baseRenderer.enabled = false;
				}
				CreateChildren();
				return;
			}
			throw new Exception("MeshFilter component is required.");
		}

		private void CreateChildren()
		{
			List<(Vector3Int gridPoint, Mesh mesh)> list = new MeshSplitter(Parameters, Verbose).Split(_baseMesh);
			list.Sort(delegate((Vector3Int gridPoint, Mesh mesh) a, (Vector3Int gridPoint, Mesh mesh) b)
			{
				for (int i = 0; i < 3; i++)
				{
					int num = a.gridPoint[i].CompareTo(b.gridPoint[i]);
					if (num != 0)
					{
						return num;
					}
				}
				return 0;
			});
			foreach (var (gridPoint, mesh) in list)
			{
				if (mesh.vertexCount > 0)
				{
					CreateChild(gridPoint, mesh);
				}
			}
		}

		private void CreateChild(Vector3Int gridPoint, Mesh mesh)
		{
			string text = $"({(float)gridPoint.x / (float)GridSizeMultiplier:0.##}, {(float)gridPoint.y / (float)GridSizeMultiplier:0.##}, {(float)gridPoint.z / (float)GridSizeMultiplier:0.##})";
			GameObject gameObject = new GameObject
			{
				name = "SubMesh " + text
			};
			gameObject.transform.SetParent(base.transform, worldPositionStays: false);
			if (Parameters.UseParentLayer)
			{
				gameObject.layer = base.gameObject.layer;
			}
			if (Parameters.UseParentStaticFlag)
			{
				gameObject.isStatic = base.gameObject.isStatic;
			}
			gameObject.AddComponent<MeshFilter>().sharedMesh = mesh;
			MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
			if (Parameters.UseParentMeshRendererSettings && (bool)_baseRenderer)
			{
				meshRenderer.sharedMaterial = _baseRenderer.sharedMaterial;
				meshRenderer.sortingOrder = _baseRenderer.sortingOrder;
				meshRenderer.sortingLayerID = _baseRenderer.sortingLayerID;
				meshRenderer.shadowCastingMode = _baseRenderer.shadowCastingMode;
				meshRenderer.receiveShadows = _baseRenderer.receiveShadows;
				meshRenderer.lightProbeUsage = _baseRenderer.lightProbeUsage;
				meshRenderer.rayTracingMode = _baseRenderer.rayTracingMode;
				meshRenderer.reflectionProbeUsage = _baseRenderer.reflectionProbeUsage;
				meshRenderer.staticShadowCaster = _baseRenderer.staticShadowCaster;
				meshRenderer.motionVectorGenerationMode = _baseRenderer.motionVectorGenerationMode;
				meshRenderer.allowOcclusionWhenDynamic = _baseRenderer.allowOcclusionWhenDynamic;
			}
			if (Parameters.GenerateColliders)
			{
				MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
				meshCollider.convex = Parameters.UseConvexColliders;
				meshCollider.sharedMesh = mesh;
			}
			m_children.Add(gameObject);
		}

		private int GetUsedAxisCount()
		{
			return (Parameters.SplitAxes.x ? 1 : 0) + (Parameters.SplitAxes.y ? 1 : 0) + (Parameters.SplitAxes.z ? 1 : 0);
		}

		public void Clear()
		{
			DestroyChildren();
			if ((bool)_baseRenderer || TryGetComponent<MeshRenderer>(out _baseRenderer))
			{
				_baseRenderer.enabled = true;
			}
		}

		private void DestroyChildren()
		{
			int childCount = base.transform.childCount;
			if (m_children.Any((GameObject o) => o == null))
			{
				m_children.Clear();
			}
			if (childCount != m_children.Count)
			{
				IEnumerable<MeshRenderer> enumerable = from child in GetComponentsInChildren<MeshRenderer>()
					where child.name.Contains("SubMesh") && !m_children.Contains(child.gameObject)
					select child;
				int num = 0;
				foreach (MeshRenderer item in enumerable)
				{
					m_children.Add(item.gameObject);
					num++;
				}
				if (Verbose)
				{
					Debug.Log($"found {num} unassigned submeshes");
				}
			}
			foreach (GameObject child in m_children)
			{
				UnityEngine.Object.DestroyImmediate(child.GetComponent<MeshFilter>().sharedMesh);
				UnityEngine.Object.DestroyImmediate(child);
			}
			if (Verbose)
			{
				Debug.Log($"destroyed {m_children.Count} submeshes");
			}
			m_children.Clear();
		}

		private void OnDrawGizmosSelected()
		{
			if (!DrawGridGizmosWhenSelected || !TryGetComponent<MeshFilter>(out var component) || !component.sharedMesh || !TryGetComponent<Renderer>(out var _))
			{
				return;
			}
			Transform transform = base.transform;
			Bounds bounds = component.sharedMesh.bounds;
			float num = (Parameters.SplitAxes.x ? Mathf.Ceil(bounds.extents.x) : 1f);
			float num2 = (Parameters.SplitAxes.y ? Mathf.Ceil(bounds.extents.y) : 1f);
			float num3 = (Parameters.SplitAxes.z ? Mathf.Ceil(bounds.extents.z) : 1f);
			if (num * num2 * num3 / Parameters.GridSize > 100000f)
			{
				return;
			}
			Vector3 center = bounds.center;
			Gizmos.color = new Color(1f, 1f, 1f, 0.3f);
			for (float num4 = 0f - num2; num4 <= num2; num4 += Parameters.GridSize)
			{
				for (float num5 = 0f - num3; num5 <= num3; num5 += Parameters.GridSize)
				{
					Vector3 vector = transform.TransformPoint(center + new Vector3(0f - num, num4, num5));
					Vector3 to = transform.TransformPoint(center + new Vector3(num, num4, num5));
					Gizmos.DrawLine(vector, to);
				}
			}
			for (float num6 = 0f - num; num6 <= num; num6 += Parameters.GridSize)
			{
				for (float num7 = 0f - num3; num7 <= num3; num7 += Parameters.GridSize)
				{
					Vector3 vector2 = transform.TransformPoint(center + new Vector3(num6, 0f - num2, num7));
					Vector3 to2 = transform.TransformPoint(center + new Vector3(num6, num2, num7));
					Gizmos.DrawLine(vector2, to2);
				}
			}
			for (float num8 = 0f - num2; num8 <= num2 + 1f; num8 += Parameters.GridSize)
			{
				for (float num9 = 0f - num; num9 <= num + 1f; num9 += Parameters.GridSize)
				{
					Vector3 vector3 = transform.TransformPoint(center + new Vector3(num9, num8, 0f - num3));
					Vector3 to3 = transform.TransformPoint(center + new Vector3(num9, num8, num3));
					Gizmos.DrawLine(vector3, to3);
				}
			}
		}
	}
}
