using System.Collections.Generic;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class GridBounds
	{
		public static bool TryCompute(VoxelGrid grid, out Vector3Int min, out Vector3Int max)
		{
			return grid.TryGetBounds(out min, out max);
		}

		public static bool TryCompute(IEnumerable<Vector3Int> positions, out Vector3Int min, out Vector3Int max)
		{
			min = default(Vector3Int);
			max = default(Vector3Int);
			bool flag = false;
			foreach (Vector3Int position in positions)
			{
				if (!flag)
				{
					min = position;
					max = position;
					flag = true;
				}
				else
				{
					min = Vector3Int.Min(min, position);
					max = Vector3Int.Max(max, position);
				}
			}
			return flag;
		}

		public static bool TryComputeExtent(VoxelGrid grid, out Vector3Int extent)
		{
			if (!grid.TryGetBounds(out var min, out var max))
			{
				extent = Vector3Int.zero;
				return false;
			}
			extent = max - min + Vector3Int.one;
			return true;
		}

		public static bool TryComputeExtent(IEnumerable<Vector3Int> positions, out Vector3Int extent)
		{
			if (!TryCompute(positions, out var min, out var max))
			{
				extent = Vector3Int.zero;
				return false;
			}
			extent = max - min + Vector3Int.one;
			return true;
		}
	}
}
