using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class VoxelIslands
	{
		private sealed class Entry
		{
			public int Version = -1;

			public int MinExtent;

			public int MinVoxels;

			public readonly List<VoxelIsland> Weak = new List<VoxelIsland>();
		}

		private const long MaxCells = 884736L;

		private static readonly ConditionalWeakTable<VoxelGrid, Entry> cache = new ConditionalWeakTable<VoxelGrid, Entry>();

		private static readonly List<VoxelIsland> scratchIslands = new List<VoxelIsland>();

		private static bool[] scratchCells = new bool[4096];

		public static int CountWeak(VoxelGrid grid)
		{
			return WeakOf(grid).Count;
		}

		public static IReadOnlyList<VoxelIsland> WeakOf(VoxelGrid grid)
		{
			if (grid == null)
			{
				return Array.Empty<VoxelIsland>();
			}
			int minBoundExtent = VoxelEditorSettings.MinBoundExtent;
			int slimBoundExtent = VoxelEditorSettings.SlimBoundExtent;
			int minIslandVoxels = VoxelEditorSettings.MinIslandVoxels;
			Entry orCreateValue = cache.GetOrCreateValue(grid);
			if (orCreateValue.Version == grid.Version && orCreateValue.MinExtent == minBoundExtent && orCreateValue.MinVoxels == minIslandVoxels)
			{
				return orCreateValue.Weak;
			}
			orCreateValue.Version = grid.Version;
			orCreateValue.MinExtent = minBoundExtent;
			orCreateValue.MinVoxels = minIslandVoxels;
			orCreateValue.Weak.Clear();
			if (minBoundExtent <= 1 && minIslandVoxels <= 1)
			{
				return orCreateValue.Weak;
			}
			scratchIslands.Clear();
			Find(grid, scratchIslands);
			foreach (VoxelIsland scratchIsland in scratchIslands)
			{
				if (!scratchIsland.IsSubstantial(minBoundExtent, slimBoundExtent, minIslandVoxels))
				{
					orCreateValue.Weak.Add(scratchIsland);
				}
			}
			return orCreateValue.Weak;
		}

		public static void Find(VoxelGrid grid, List<VoxelIsland> into)
		{
			if (grid == null || into == null || !grid.TryGetBounds(out var min, out var max))
			{
				return;
			}
			Vector3Int vector3Int = max - min + Vector3Int.one;
			long num = (long)vector3Int.x * (long)vector3Int.y * vector3Int.z;
			if (num <= 0 || num > 884736)
			{
				return;
			}
			if (scratchCells.Length < num)
			{
				scratchCells = new bool[num];
			}
			else
			{
				Array.Clear(scratchCells, 0, (int)num);
			}
			foreach (Vector3Int position in grid.Positions)
			{
				scratchCells[position.x - min.x + vector3Int.x * (position.z - min.z + vector3Int.z * (position.y - min.y))] = true;
			}
			int count = into.Count;
			VoxelIslandFinder.Find(scratchCells, vector3Int.x, vector3Int.y, vector3Int.z, into);
			for (int i = count; i < into.Count; i++)
			{
				VoxelIsland value = into[i];
				value.MinX += min.x;
				value.MaxX += min.x;
				value.MinY += min.y;
				value.MaxY += min.y;
				value.MinZ += min.z;
				value.MaxZ += min.z;
				into[i] = value;
			}
		}
	}
}
