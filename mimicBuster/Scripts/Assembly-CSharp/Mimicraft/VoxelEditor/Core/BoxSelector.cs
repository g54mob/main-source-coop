using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class BoxSelector
	{
		private static readonly Vector2Int[] PlaneNeighbours = new Vector2Int[4]
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1)
		};

		private static readonly HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

		private static readonly Queue<Vector2Int> frontier = new Queue<Vector2Int>();

		public static bool TryGetPlaneCell(Ray localRay, Vector3Int anchorVoxel, Vector3Int faceNormal, out Vector3Int cell)
		{
			Vector3 inPoint = anchorVoxel + new Vector3(0.5f, 0.5f, 0.5f) + (Vector3)faceNormal * 0.5f;
			if (!new Plane(faceNormal, inPoint).Raycast(localRay, out var enter))
			{
				cell = default(Vector3Int);
				return false;
			}
			Vector3 vector = localRay.GetPoint(enter) - (Vector3)faceNormal * 0.5f;
			cell = new Vector3Int(Mathf.FloorToInt(vector.x), Mathf.FloorToInt(vector.y), Mathf.FloorToInt(vector.z));
			return true;
		}

		public static List<Vector3Int> Select(VoxelGrid grid, Vector3Int anchorVoxel, Vector3Int faceNormal, Vector3Int currentCell)
		{
			FaceAxes.GetBasis(faceNormal, out var right, out var up);
			int num = Dot(anchorVoxel, right);
			int num2 = Dot(anchorVoxel, up);
			int b = Dot(currentCell, right);
			int b2 = Dot(currentCell, up);
			int num3 = Mathf.Min(num, b);
			int num4 = Mathf.Max(num, b);
			int num5 = Mathf.Min(num2, b2);
			int num6 = Mathf.Max(num2, b2);
			int num7 = Dot(anchorVoxel, faceNormal);
			List<Vector3Int> list = new List<Vector3Int>();
			Vector2Int vector2Int = new Vector2Int(num, num2);
			if (!IsSelectable(grid, vector2Int, right, up, faceNormal, num7))
			{
				return list;
			}
			visited.Clear();
			frontier.Clear();
			visited.Add(vector2Int);
			frontier.Enqueue(vector2Int);
			while (frontier.Count > 0)
			{
				Vector2Int vector2Int2 = frontier.Dequeue();
				list.Add(right * vector2Int2.x + up * vector2Int2.y + faceNormal * num7);
				Vector2Int[] planeNeighbours = PlaneNeighbours;
				foreach (Vector2Int vector2Int3 in planeNeighbours)
				{
					Vector2Int vector2Int4 = vector2Int2 + vector2Int3;
					if (vector2Int4.x >= num3 && vector2Int4.x <= num4 && vector2Int4.y >= num5 && vector2Int4.y <= num6 && visited.Add(vector2Int4) && IsSelectable(grid, vector2Int4, right, up, faceNormal, num7))
					{
						frontier.Enqueue(vector2Int4);
					}
				}
			}
			return list;
		}

		private static bool IsSelectable(VoxelGrid grid, Vector2Int cell, Vector3Int right, Vector3Int up, Vector3Int faceNormal, int depth)
		{
			Vector3Int vector3Int = right * cell.x + up * cell.y + faceNormal * depth;
			if (grid.Contains(vector3Int))
			{
				return !grid.Contains(vector3Int + faceNormal);
			}
			return false;
		}

		private static int Dot(Vector3Int a, Vector3Int b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}
	}
}
