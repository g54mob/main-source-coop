using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class PaintBrushPreview : MonoBehaviour
	{
		private const float Inset = 0.08f;

		private const float NormalOffset = 0.015f;

		private static readonly Vector3Int[] NeighborDirs = new Vector3Int[6]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(-1, 0, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(0, 0, 1),
			new Vector3Int(0, 0, -1)
		};

		private MeshFilter meshFilter;

		private Material material;

		private Mesh mesh;

		public static PaintBrushPreview Create(Transform parent)
		{
			GameObject obj = new GameObject("PaintBrushPreview");
			obj.transform.SetParent(parent, worldPositionStays: false);
			PaintBrushPreview paintBrushPreview = obj.AddComponent<PaintBrushPreview>();
			paintBrushPreview.Init();
			return paintBrushPreview;
		}

		private void Init()
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			Shader shader = Shader.Find("Mimicraft/HighlightUnlit");
			if (shader != null)
			{
				material = new Material(shader);
			}
			meshRenderer.sharedMaterial = material;
			mesh = new Mesh
			{
				name = "PaintBrushPreviewMesh"
			};
			meshFilter.sharedMesh = mesh;
			base.gameObject.SetActive(value: false);
		}

		public void Show(VoxelGrid grid, IReadOnlyList<Vector3Int> positions, Color color)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			foreach (Vector3Int position in positions)
			{
				Vector3Int[] neighborDirs = NeighborDirs;
				foreach (Vector3Int vector3Int in neighborDirs)
				{
					if (!grid.Contains(position + vector3Int))
					{
						AddQuad(position, vector3Int, vertices, triangles);
					}
				}
			}
			Finish(vertices, triangles, color);
		}

		public void ShowFaces(IReadOnlyList<(Vector3Int Position, int Face)> faces, Color color)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<int> triangles = new List<int>();
			foreach (var (pos, num) in faces)
			{
				AddQuad(pos, FaceAxes.Normals[num], vertices, triangles);
			}
			Finish(vertices, triangles, color);
		}

		private static void AddQuad(Vector3Int pos, Vector3Int dir, List<Vector3> vertices, List<int> triangles)
		{
			FaceAxes.GetBasis(dir, out var right, out var up);
			Vector3 vector = right;
			Vector3 vector2 = up;
			Vector3 vector3 = dir;
			Vector3 vector4 = pos + new Vector3(0.5f, 0.5f, 0.5f) + vector3 * 0.515f;
			float num = 0.42000002f;
			int count = vertices.Count;
			vertices.Add(vector4 - vector * num - vector2 * num);
			vertices.Add(vector4 + vector * num - vector2 * num);
			vertices.Add(vector4 + vector * num + vector2 * num);
			vertices.Add(vector4 - vector * num + vector2 * num);
			triangles.Add(count);
			triangles.Add(count + 1);
			triangles.Add(count + 2);
			triangles.Add(count);
			triangles.Add(count + 2);
			triangles.Add(count + 3);
		}

		private void Finish(List<Vector3> vertices, List<int> triangles, Color color)
		{
			if (vertices.Count == 0)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetTriangles(triangles, 0);
			mesh.RecalculateBounds();
			if (material != null)
			{
				material.color = color;
			}
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}
