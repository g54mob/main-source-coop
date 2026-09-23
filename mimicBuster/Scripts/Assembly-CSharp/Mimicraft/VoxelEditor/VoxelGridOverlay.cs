using System.Collections.Generic;
using Mimicraft.VoxelEditor.Core;
using UnityEngine;

namespace Mimicraft.VoxelEditor
{
	public class VoxelGridOverlay : MonoBehaviour
	{
		private const float NormalOffset = 0.01f;

		private static readonly Vector3Int[] FaceDirs = new Vector3Int[6]
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

		private readonly List<Vector3> vertices = new List<Vector3>();

		private readonly List<int> indices = new List<int>();

		private VoxelGrid cachedGrid;

		private int cachedVersion = -1;

		private Color cachedColor;

		public static VoxelGridOverlay Create(Transform parent)
		{
			GameObject obj = new GameObject("VoxelGridOverlay");
			obj.transform.SetParent(parent, worldPositionStays: false);
			VoxelGridOverlay voxelGridOverlay = obj.AddComponent<VoxelGridOverlay>();
			voxelGridOverlay.Init();
			return voxelGridOverlay;
		}

		private void Init()
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
			MeshRenderer meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
			Shader shader = Shader.Find("Mimicraft/SurfaceOverlayUnlit");
			if (shader != null)
			{
				material = new Material(shader);
			}
			meshRenderer.sharedMaterial = material;
			mesh = new Mesh
			{
				name = "VoxelGridOverlayMesh"
			};
			meshFilter.sharedMesh = mesh;
			base.gameObject.SetActive(value: false);
		}

		public void ShowWholeGrid(VoxelGrid grid, Color color)
		{
			if (cachedGrid != grid || cachedVersion != grid.Version || !(cachedColor == color) || !base.gameObject.activeSelf)
			{
				cachedGrid = grid;
				cachedVersion = grid.Version;
				cachedColor = color;
				Rebuild(grid, grid.Positions, color);
			}
		}

		public void Show(VoxelGrid grid, IEnumerable<Vector3Int> voxelPositions, Color color)
		{
			cachedVersion = -1;
			Rebuild(grid, voxelPositions, color);
		}

		public void ShowFaces(IEnumerable<(Vector3Int Position, int Face)> faces, Color color)
		{
			cachedVersion = -1;
			vertices.Clear();
			indices.Clear();
			foreach (var (pos, num) in faces)
			{
				AddFaceOutline(pos, FaceAxes.Normals[num], vertices, indices);
			}
			Upload(color);
		}

		private void Rebuild(VoxelGrid grid, IEnumerable<Vector3Int> voxelPositions, Color color)
		{
			vertices.Clear();
			indices.Clear();
			foreach (Vector3Int voxelPosition in voxelPositions)
			{
				Vector3Int[] faceDirs = FaceDirs;
				foreach (Vector3Int vector3Int in faceDirs)
				{
					if (!grid.Contains(voxelPosition + vector3Int))
					{
						AddFaceOutline(voxelPosition, vector3Int, vertices, indices);
					}
				}
			}
			Upload(color);
		}

		private void Upload(Color color)
		{
			if (vertices.Count == 0)
			{
				base.gameObject.SetActive(value: false);
				return;
			}
			mesh.Clear();
			mesh.SetVertices(vertices);
			mesh.SetIndices(indices, MeshTopology.Lines, 0);
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

		private static void AddFaceOutline(Vector3Int pos, Vector3Int normal, List<Vector3> vertices, List<int> indices)
		{
			FaceAxes.GetBasis(normal, out var right, out var up);
			Vector3 vector = right;
			Vector3 vector2 = up;
			Vector3 vector3 = pos + new Vector3(0.5f, 0.5f, 0.5f) + (Vector3)normal * 0.51f;
			int count = vertices.Count;
			vertices.Add(vector3 - vector * 0.5f - vector2 * 0.5f);
			vertices.Add(vector3 + vector * 0.5f - vector2 * 0.5f);
			vertices.Add(vector3 + vector * 0.5f + vector2 * 0.5f);
			vertices.Add(vector3 - vector * 0.5f + vector2 * 0.5f);
			indices.Add(count);
			indices.Add(count + 1);
			indices.Add(count + 1);
			indices.Add(count + 2);
			indices.Add(count + 2);
			indices.Add(count + 3);
			indices.Add(count + 3);
			indices.Add(count);
		}
	}
}
