using UnityEngine;

namespace Features.SwimmingModule.Scripts
{
	public sealed class SwimWaterGrid
	{
		private readonly bool[] _walkable;

		private readonly float[] _surfaceY;

		public GameObject WaterSource { get; }

		public Bounds Bounds { get; }

		public float CellSize { get; }

		public float AgentRadius { get; }

		public int Width { get; }

		public int Depth { get; }

		public float OriginX { get; }

		public float OriginZ { get; }

		public SwimWaterGrid(GameObject waterSource, Bounds bounds, float cellSize, float agentRadius, int width, int depth, bool[] walkable, float[] surfaceY)
		{
			WaterSource = waterSource;
			Bounds = bounds;
			CellSize = cellSize;
			AgentRadius = agentRadius;
			Width = width;
			Depth = depth;
			_walkable = walkable;
			_surfaceY = surfaceY;
			OriginX = bounds.min.x;
			OriginZ = bounds.min.z;
		}

		public bool IsInside(int x, int z)
		{
			if (x >= 0 && z >= 0 && x < Width)
			{
				return z < Depth;
			}
			return false;
		}

		public bool IsWalkable(int x, int z)
		{
			if (IsInside(x, z))
			{
				return _walkable[Index(x, z)];
			}
			return false;
		}

		public float GetSurfaceY(int x, int z)
		{
			if (!IsInside(x, z))
			{
				return Bounds.center.y;
			}
			return _surfaceY[Index(x, z)];
		}

		public int Index(int x, int z)
		{
			return z * Width + x;
		}

		public Vector3 CellToWorld(int x, int z)
		{
			return new Vector3(OriginX + ((float)x + 0.5f) * CellSize, z: OriginZ + ((float)z + 0.5f) * CellSize, y: GetSurfaceY(x, z));
		}

		public void WorldToCell(Vector3 world, out int x, out int z)
		{
			x = Mathf.FloorToInt((world.x - OriginX) / CellSize);
			z = Mathf.FloorToInt((world.z - OriginZ) / CellSize);
		}

		public bool TryFindNearestWalkable(Vector3 world, out int x, out int z)
		{
			WorldToCell(world, out var x2, out var z2);
			if (IsWalkable(x2, z2))
			{
				x = x2;
				z = z2;
				return true;
			}
			int num = Mathf.Max(Width, Depth);
			for (int i = 1; i <= num; i++)
			{
				for (int j = -i; j <= i; j++)
				{
					for (int k = -i; k <= i; k++)
					{
						if (Mathf.Abs(k) == i || Mathf.Abs(j) == i)
						{
							int num2 = x2 + k;
							int num3 = z2 + j;
							if (IsWalkable(num2, num3))
							{
								x = num2;
								z = num3;
								return true;
							}
						}
					}
				}
			}
			x = 0;
			z = 0;
			return false;
		}
	}
}
