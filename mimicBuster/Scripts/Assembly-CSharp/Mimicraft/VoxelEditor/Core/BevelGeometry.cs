using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class BevelGeometry
	{
		public const int MaxRadius = 256;

		private static readonly Vector3Int[] Units = new Vector3Int[3]
		{
			new Vector3Int(1, 0, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(0, 0, 1)
		};

		public static void FindHandles(VoxelGrid grid, List<BevelEdge> edges, List<BevelCorner> corners)
		{
			edges.Clear();
			corners.Clear();
			if (grid == null || grid.Count == 0)
			{
				return;
			}
			Dictionary<(int, Vector3Int), int> dictionary = new Dictionary<(int, Vector3Int), int>();
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Vector3Int signs = CrossSigns(i, j);
					int item = i * 4 + j;
					HashSet<Vector3Int> hashSet = new HashSet<Vector3Int>();
					foreach (Vector3Int position in grid.Positions)
					{
						if (HasEdge(grid, position, i, signs))
						{
							hashSet.Add(position);
						}
					}
					Vector3Int vector3Int = Units[i];
					foreach (Vector3Int item2 in hashSet)
					{
						if (!hashSet.Contains(item2 - vector3Int))
						{
							int k;
							for (k = 1; hashSet.Contains(item2 + vector3Int * k); k++)
							{
							}
							int count = edges.Count;
							edges.Add(new BevelEdge(i, signs, item2, k));
							for (int l = 0; l < k; l++)
							{
								dictionary[(item, item2 + vector3Int * l)] = count;
							}
						}
					}
				}
			}
			foreach (Vector3Int position2 in grid.Positions)
			{
				for (int m = 0; m < 8; m++)
				{
					Vector3Int signs2 = new Vector3Int(((m & 1) != 0) ? 1 : (-1), ((m & 2) != 0) ? 1 : (-1), ((m & 4) != 0) ? 1 : (-1));
					if (HasCorner(grid, position2, signs2))
					{
						corners.Add(new BevelCorner(position2, signs2, edges[dictionary[(ConfigFor(0, signs2), position2)]], edges[dictionary[(ConfigFor(1, signs2), position2)]], edges[dictionary[(ConfigFor(2, signs2), position2)]]));
					}
				}
			}
		}

		private static bool HasEdge(VoxelGrid grid, Vector3Int cell, int axis, Vector3Int signs)
		{
			for (int i = 0; i < 3; i++)
			{
				if (i != axis && grid.Contains(cell + Units[i] * signs[i]))
				{
					return false;
				}
			}
			return true;
		}

		private static bool HasCorner(VoxelGrid grid, Vector3Int cell, Vector3Int signs)
		{
			for (int i = 0; i < 3; i++)
			{
				if (grid.Contains(cell + Units[i] * signs[i]))
				{
					return false;
				}
			}
			return true;
		}

		private static Vector3Int CrossSigns(int axis, int signBits)
		{
			Vector3Int result = new Vector3Int(1, 1, 1);
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				if (i != axis)
				{
					result[i] = (((signBits & (1 << num)) != 0) ? 1 : (-1));
					num++;
				}
			}
			return result;
		}

		private static int ConfigFor(int axis, Vector3Int signs)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 3; i++)
			{
				if (i != axis)
				{
					if (signs[i] > 0)
					{
						num |= 1 << num2;
					}
					num2++;
				}
			}
			return axis * 4 + num;
		}

		public static List<Vector3Int> Compute(VoxelGrid grid, in BevelEdge edge, int radius, BevelProfile profile)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			if (grid == null || radius <= 0)
			{
				return list;
			}
			CutEdge(grid, in edge, Clamp(grid, radius), profile, list, new HashSet<Vector3Int>());
			return list;
		}

		public static List<Vector3Int> Compute(VoxelGrid grid, in BevelCorner corner, int radius, BevelProfile profile)
		{
			List<Vector3Int> list = new List<Vector3Int>();
			if (grid == null || radius <= 0)
			{
				return list;
			}
			radius = Clamp(grid, radius);
			HashSet<Vector3Int> seen = new HashSet<Vector3Int>();
			for (int i = 0; i < 3; i++)
			{
				CutEdge(grid, corner.Edge(i), radius, profile, list, seen);
			}
			CutVertex(grid, in corner, radius, profile, list, seen);
			return list;
		}

		private static int Clamp(VoxelGrid grid, int radius)
		{
			radius = Mathf.Min(radius, 256);
			if (GridBounds.TryCompute(grid, out var min, out var max))
			{
				Vector3Int vector3Int = max - min + Vector3Int.one;
				radius = Mathf.Min(radius, Mathf.Max(vector3Int.x, Mathf.Max(vector3Int.y, vector3Int.z)));
			}
			return radius;
		}

		private static void CutEdge(VoxelGrid grid, in BevelEdge edge, int radius, BevelProfile profile, List<Vector3Int> removed, HashSet<Vector3Int> seen)
		{
			int num = ((edge.Axis == 0) ? 1 : 0);
			int num2 = ((edge.Axis == 2) ? 1 : 2);
			for (int i = edge.Start; i <= edge.End; i++)
			{
				for (int j = 0; j < radius; j++)
				{
					for (int k = 0; k < radius; k++)
					{
						if (CutsAcross(j, k, radius, profile))
						{
							Vector3Int cell = edge.Cell;
							cell[edge.Axis] = i;
							cell[num] -= edge.Signs[num] * j;
							cell[num2] -= edge.Signs[num2] * k;
							if (grid.Contains(cell) && !seen.Contains(cell) && ReachesFace(grid, cell, num, edge.Signs[num], j) && ReachesFace(grid, cell, num2, edge.Signs[num2], k))
							{
								seen.Add(cell);
								removed.Add(cell);
							}
						}
					}
				}
			}
		}

		private static void CutVertex(VoxelGrid grid, in BevelCorner corner, int radius, BevelProfile profile, List<Vector3Int> removed, HashSet<Vector3Int> seen)
		{
			for (int i = 0; i < radius; i++)
			{
				for (int j = 0; j < radius; j++)
				{
					for (int k = 0; k < radius; k++)
					{
						if (CutsVertex(i, j, k, radius, profile))
						{
							Vector3Int vector3Int = corner.Cell - new Vector3Int(corner.Signs.x * i, corner.Signs.y * j, corner.Signs.z * k);
							if (grid.Contains(vector3Int) && !seen.Contains(vector3Int) && ReachesFace(grid, vector3Int, 0, corner.Signs.x, i) && ReachesFace(grid, vector3Int, 1, corner.Signs.y, j) && ReachesFace(grid, vector3Int, 2, corner.Signs.z, k))
							{
								seen.Add(vector3Int);
								removed.Add(vector3Int);
							}
						}
					}
				}
			}
		}

		private static bool CutsAcross(int da, int db, int radius, BevelProfile profile)
		{
			if (profile == BevelProfile.Linear)
			{
				return da + db < radius;
			}
			float num = radius - da;
			float num2 = radius - db;
			return num * num + num2 * num2 > (float)radius * (float)radius;
		}

		private static bool CutsVertex(int dx, int dy, int dz, int radius, BevelProfile profile)
		{
			if (profile == BevelProfile.Linear)
			{
				return dx + dy + dz < 2 * radius;
			}
			float num = radius - dx;
			float num2 = radius - dy;
			float num3 = radius - dz;
			return num * num + num2 * num2 + num3 * num3 > (float)radius * (float)radius;
		}

		private static bool ReachesFace(VoxelGrid grid, Vector3Int p, int axis, int sign, int depth)
		{
			Vector3Int vector3Int = Units[axis] * sign;
			for (int i = 1; i <= depth; i++)
			{
				if (!grid.Contains(p + vector3Int * i))
				{
					return false;
				}
			}
			return !grid.Contains(p + vector3Int * (depth + 1));
		}

		public static Vector3 OutwardDirection(Vector3Int signs, int edgeAxis)
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < 3; i++)
			{
				if (i != edgeAxis)
				{
					zero[i] = ((signs[i] > 0) ? 1f : (-1f));
				}
			}
			if (!(zero == Vector3.zero))
			{
				return zero.normalized;
			}
			return Vector3.up;
		}
	}
}
