using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class SelectionHighlight : MonoBehaviour
	{
		private const float Inset = 0.05f;

		private const float NormalOffset = 0.012f;

		private MeshFilter meshFilter;

		private Mesh mesh;

		public static SelectionHighlight Create(Transform parent)
		{
			GameObject obj = new GameObject("SelectionHighlight");
			obj.transform.SetParent(parent, worldPositionStays: false);
			SelectionHighlight selectionHighlight = obj.AddComponent<SelectionHighlight>();
			selectionHighlight.Init();
			return selectionHighlight;
		}

		private void Init()
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				meshRenderer.sharedMaterial = new Material(shader)
				{
					color = new Color(0.25f, 0.85f, 1f, 0.5f)
				};
			}
			mesh = new Mesh
			{
				name = "SelectionHighlightMesh"
			};
			meshFilter.sharedMesh = mesh;
			base.gameObject.SetActive(value: false);
		}

		public void Show(IReadOnlyList<Vector3Int> voxelPositions, Vector3Int faceNormal)
		{
			FaceAxes.GetBasis(faceNormal, out var right, out var up);
			Vector3 vector = right;
			Vector3 vector2 = up;
			Vector3 vector3 = faceNormal;
			List<Vector3> list = new List<Vector3>(voxelPositions.Count * 4);
			List<int> list2 = new List<int>(voxelPositions.Count * 6);
			float num = 0.45f;
			foreach (Vector3Int voxelPosition in voxelPositions)
			{
				Vector3 vector4 = voxelPosition + new Vector3(0.5f, 0.5f, 0.5f) + vector3 * 0.512f;
				int count = list.Count;
				list.Add(vector4 - vector * num - vector2 * num);
				list.Add(vector4 + vector * num - vector2 * num);
				list.Add(vector4 + vector * num + vector2 * num);
				list.Add(vector4 - vector * num + vector2 * num);
				list2.Add(count);
				list2.Add(count + 1);
				list2.Add(count + 2);
				list2.Add(count);
				list2.Add(count + 2);
				list2.Add(count + 3);
			}
			mesh.Clear();
			mesh.SetVertices(list);
			mesh.SetTriangles(list2, 0);
			mesh.RecalculateBounds();
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
