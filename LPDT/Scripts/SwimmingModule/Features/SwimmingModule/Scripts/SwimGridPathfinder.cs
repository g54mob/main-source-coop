using System.Collections.Generic;
using UnityEngine;

namespace Features.SwimmingModule.Scripts
{
	public sealed class SwimGridPathfinder
	{
		private const int MAX_CELLS = 10000;

		private const float WATER_RAY_EXTRA_HEIGHT = 2f;

		private const float DIAGONAL_COST = 1.4142135f;

		private static readonly Vector2Int[] NeighborOffsets = new Vector2Int[8]
		{
			new Vector2Int(1, 0),
			new Vector2Int(-1, 0),
			new Vector2Int(0, 1),
			new Vector2Int(0, -1),
			new Vector2Int(1, 1),
			new Vector2Int(1, -1),
			new Vector2Int(-1, 1),
			new Vector2Int(-1, -1)
		};

		private readonly SwimPathGridCache _cache;

		private readonly List<Vector3> _rawPath = new List<Vector3>();

		private readonly List<int> _openHeap = new List<int>();

		private readonly Dictionary<int, float> _gScore = new Dictionary<int, float>();

		private readonly Dictionary<int, int> _cameFrom = new Dictionary<int, int>();

		private readonly HashSet<int> _closed = new HashSet<int>();

		private readonly HashSet<int> _openSet = new HashSet<int>();

		public SwimGridPathfinder(SwimPathGridCache cache)
		{
			_cache = cache;
		}

		public SwimWaterGrid GetOrBuildGrid(GameObject waterSource, LayerMask waterMask, LayerMask obstacleMask, float cellSize, float agentRadius)
		{
			if (waterSource == null)
			{
				return null;
			}
			if (_cache.TryGet(waterSource, out var grid))
			{
				return grid;
			}
			if (!waterSource.TryGetComponent<Collider>(out var component))
			{
				return null;
			}
			Bounds bounds = component.bounds;
			float num = ResolveCellSize(bounds, cellSize);
			int num2 = Mathf.Max(1, Mathf.CeilToInt(bounds.size.x / num));
			int num3 = Mathf.Max(1, Mathf.CeilToInt(bounds.size.z / num));
			bool[] array = new bool[num2 * num3];
			float[] array2 = new float[num2 * num3];
			float x = bounds.min.x;
			float z = bounds.min.z;
			float y = bounds.max.y + 2f;
			float maxDistance = bounds.size.y + 4f;
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					int num4 = i * num2 + j;
					float num5 = x + ((float)j + 0.5f) * num;
					float num6 = z + ((float)i + 0.5f) * num;
					if (!Physics.Raycast(new Vector3(num5, y, num6), Vector3.down, out var hitInfo, maxDistance, waterMask, QueryTriggerInteraction.Collide))
					{
						array[num4] = false;
						array2[num4] = bounds.center.y;
					}
					else if (hitInfo.collider != null && hitInfo.collider.gameObject != waterSource)
					{
						array[num4] = false;
						array2[num4] = bounds.center.y;
					}
					else
					{
						array2[num4] = hitInfo.point.y;
						array[num4] = !IsCellBlocked(num5, hitInfo.point.y, num6, agentRadius, obstacleMask);
					}
				}
			}
			SwimWaterGrid swimWaterGrid = new SwimWaterGrid(waterSource, bounds, num, agentRadius, num2, num3, array, array2);
			_cache.Set(swimWaterGrid);
			return swimWaterGrid;
		}

		public bool TryFindPath(Vector3 from, Vector3 to, SwimWaterGrid grid, LayerMask obstacleMask, float agentRadius, List<Vector3> pathOut)
		{
			pathOut.Clear();
			if (grid == null)
			{
				return false;
			}
			if (!grid.TryFindNearestWalkable(from, out var x, out var z))
			{
				return false;
			}
			if (!grid.TryFindNearestWalkable(to, out var x2, out var z2))
			{
				return false;
			}
			if (!TryAStar(grid, x, z, x2, z2, _rawPath))
			{
				return false;
			}
			StringPull(_rawPath, obstacleMask, agentRadius);
			pathOut.AddRange(_rawPath);
			if (pathOut.Count == 0)
			{
				pathOut.Add(grid.CellToWorld(x2, z2));
			}
			Vector3 vector = pathOut[pathOut.Count - 1];
			Vector3 vector2 = new Vector3(to.x, vector.y, to.z);
			if (HorizontalSqrDistance(vector, vector2) > 0.0001f && HasLineOfSight(vector, vector2, obstacleMask, agentRadius))
			{
				pathOut.Add(vector2);
			}
			return pathOut.Count > 0;
		}

		private static bool IsCellBlocked(float worldX, float surfaceY, float worldZ, float agentRadius, LayerMask obstacleMask)
		{
			float num = Mathf.Max(0.05f, agentRadius);
			return Physics.CheckSphere(new Vector3(worldX, surfaceY + num + 0.05f, worldZ), num, obstacleMask, QueryTriggerInteraction.Ignore);
		}

		private static float ResolveCellSize(Bounds bounds, float requestedCellSize)
		{
			float num = Mathf.Max(0.1f, requestedCellSize);
			int num2 = Mathf.Max(1, Mathf.CeilToInt(bounds.size.x / num));
			int num3 = Mathf.Max(1, Mathf.CeilToInt(bounds.size.z / num));
			if (num2 * num3 <= 10000)
			{
				return num;
			}
			float num4 = Mathf.Max(0.01f, bounds.size.x * bounds.size.z);
			return Mathf.Max(0.1f, Mathf.Sqrt(num4 / 10000f));
		}

		private bool TryAStar(SwimWaterGrid grid, int startX, int startZ, int goalX, int goalZ, List<Vector3> pathOut)
		{
			pathOut.Clear();
			_openHeap.Clear();
			_gScore.Clear();
			_cameFrom.Clear();
			_closed.Clear();
			_openSet.Clear();
			int num = grid.Index(startX, startZ);
			int num2 = grid.Index(goalX, goalZ);
			if (num == num2)
			{
				pathOut.Add(grid.CellToWorld(startX, startZ));
				return true;
			}
			_gScore[num] = 0f;
			_openHeap.Add(num);
			_openSet.Add(num);
			while (_openHeap.Count > 0)
			{
				int num3 = PopOpenHeap(goalX, goalZ, grid);
				if (num3 == num2)
				{
					ReconstructPath(grid, num3, pathOut);
					return true;
				}
				_closed.Add(num3);
				int num4 = num3 % grid.Width;
				int num5 = num3 / grid.Width;
				for (int i = 0; i < NeighborOffsets.Length; i++)
				{
					Vector2Int vector2Int = NeighborOffsets[i];
					int x = num4 + vector2Int.x;
					int z = num5 + vector2Int.y;
					if (!grid.IsWalkable(x, z) || (vector2Int.x != 0 && vector2Int.y != 0 && (!grid.IsWalkable(num4 + vector2Int.x, num5) || !grid.IsWalkable(num4, num5 + vector2Int.y))))
					{
						continue;
					}
					int num6 = grid.Index(x, z);
					if (_closed.Contains(num6))
					{
						continue;
					}
					float num7 = ((vector2Int.x == 0 || vector2Int.y == 0) ? 1f : 1.4142135f);
					float num8 = _gScore[num3] + num7;
					if (!_gScore.TryGetValue(num6, out var value) || !(num8 >= value))
					{
						_cameFrom[num6] = num3;
						_gScore[num6] = num8;
						if (_openSet.Add(num6))
						{
							_openHeap.Add(num6);
						}
					}
				}
			}
			return false;
		}

		private int PopOpenHeap(int goalX, int goalZ, SwimWaterGrid grid)
		{
			int index = 0;
			float num = float.MaxValue;
			for (int i = 0; i < _openHeap.Count; i++)
			{
				int num2 = _openHeap[i];
				int x = num2 % grid.Width;
				int z = num2 / grid.Width;
				float num3 = _gScore[num2] + Heuristic(x, z, goalX, goalZ);
				if (!(num3 >= num))
				{
					num = num3;
					index = i;
				}
			}
			int num4 = _openHeap[index];
			int index2 = _openHeap.Count - 1;
			_openHeap[index] = _openHeap[index2];
			_openHeap.RemoveAt(index2);
			_openSet.Remove(num4);
			return num4;
		}

		private static float Heuristic(int x, int z, int goalX, int goalZ)
		{
			int a = Mathf.Abs(x - goalX);
			int b = Mathf.Abs(z - goalZ);
			int num = Mathf.Min(a, b);
			int num2 = Mathf.Max(a, b);
			return (float)num * 1.4142135f + (float)(num2 - num);
		}

		private void ReconstructPath(SwimWaterGrid grid, int current, List<Vector3> pathOut)
		{
			List<int> list = new List<int>();
			int num = current;
			list.Add(num);
			int value;
			while (_cameFrom.TryGetValue(num, out value))
			{
				num = value;
				list.Add(num);
			}
			list.Reverse();
			for (int i = 0; i < list.Count; i++)
			{
				int x = list[i] % grid.Width;
				int z = list[i] / grid.Width;
				pathOut.Add(grid.CellToWorld(x, z));
			}
		}

		private void StringPull(List<Vector3> path, LayerMask obstacleMask, float agentRadius)
		{
			if (path.Count <= 2)
			{
				return;
			}
			List<Vector3> list = new List<Vector3>(path.Count);
			list.Add(path[0]);
			int num = 0;
			while (num < path.Count - 1)
			{
				int num2 = num + 1;
				for (int num3 = path.Count - 1; num3 > num; num3--)
				{
					if (HasLineOfSight(path[num], path[num3], obstacleMask, agentRadius))
					{
						num2 = num3;
						break;
					}
				}
				list.Add(path[num2]);
				num = num2;
			}
			path.Clear();
			path.AddRange(list);
		}

		private static bool HasLineOfSight(Vector3 from, Vector3 to, LayerMask obstacleMask, float agentRadius)
		{
			Vector3 vector = new Vector3(from.x, from.y + agentRadius, from.z);
			Vector3 vector2 = new Vector3(to.x, from.y + agentRadius, to.z) - vector;
			float magnitude = vector2.magnitude;
			if (magnitude <= 0.0001f)
			{
				return true;
			}
			Vector3 direction = vector2 / magnitude;
			RaycastHit hitInfo;
			return !Physics.SphereCast(vector, agentRadius, direction, out hitInfo, magnitude, obstacleMask, QueryTriggerInteraction.Ignore);
		}

		private static float HorizontalSqrDistance(Vector3 a, Vector3 b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return num * num + num2 * num2;
		}
	}
}
