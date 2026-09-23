using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mimicraft.VoxelEditor.Core
{
	public static class PieceShape
	{
		private sealed class Entry
		{
			public int Version = -1;

			public readonly PieceShapeData Data = new PieceShapeData();
		}

		public const int MaxBoxes = 32;

		private const long MaxCells = 884736L;

		private static readonly ConditionalWeakTable<VoxelGrid, Entry> cache = new ConditionalWeakTable<VoxelGrid, Entry>();

		private static bool[] cells = new bool[4096];

		private static readonly PieceShapeData Empty = new PieceShapeData();

		public static PieceShapeData Of(VoxelGrid grid)
		{
			if (grid == null)
			{
				return Empty;
			}
			Entry orCreateValue = cache.GetOrCreateValue(grid);
			if (orCreateValue.Version == grid.Version)
			{
				return orCreateValue.Data;
			}
			orCreateValue.Version = grid.Version;
			Build(grid, orCreateValue.Data);
			return orCreateValue.Data;
		}

		private static void Build(VoxelGrid grid, PieceShapeData into)
		{
			into.Clear();
			if (!grid.TryGetBounds(out var min, out var max))
			{
				return;
			}
			Vector3Int vector3Int = max - min + Vector3Int.one;
			long num = (long)vector3Int.x * (long)vector3Int.y * vector3Int.z;
			if (num <= 0 || num > 884736)
			{
				return;
			}
			if (cells.Length < num)
			{
				cells = new bool[num];
			}
			else
			{
				Array.Clear(cells, 0, (int)num);
			}
			foreach (Vector3Int position in grid.Positions)
			{
				cells[position.x - min.x + vector3Int.x * (position.z - min.z + vector3Int.z * (position.y - min.y))] = true;
			}
			FromOccupancy(cells, min, vector3Int.x, vector3Int.y, vector3Int.z, into);
		}

		public static void FromOccupancy(bool[] occupancy, Vector3Int origin, int sizeX, int sizeY, int sizeZ, PieceShapeData into)
		{
			into.Clear();
			VoxelBoxDecomposer.DecomposeCapped(occupancy, sizeX, sizeY, sizeZ, 32, into.Boxes);
			bool flag = true;
			for (int i = 0; i < into.Boxes.Count; i++)
			{
				VoxelBox value = into.Boxes[i];
				value.MinX += origin.x;
				value.MinY += origin.y;
				value.MinZ += origin.z;
				into.Boxes[i] = value;
				Vector3Int vector3Int = new Vector3Int(value.MinX, value.MinY, value.MinZ);
				Vector3Int vector3Int2 = vector3Int + new Vector3Int(value.SizeX, value.SizeY, value.SizeZ) - Vector3Int.one;
				if (flag)
				{
					into.Min = vector3Int;
					into.Max = vector3Int2;
					flag = false;
				}
				else
				{
					into.Min = Vector3Int.Min(into.Min, vector3Int);
					into.Max = Vector3Int.Max(into.Max, vector3Int2);
				}
			}
			into.Finish();
		}
	}
}
