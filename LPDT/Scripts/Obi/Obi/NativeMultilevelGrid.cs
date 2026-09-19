using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Obi
{
	public struct NativeMultilevelGrid<T> : IDisposable where T : unmanaged, IEquatable<T>
	{
		public struct Cell<K> where K : unmanaged, IEquatable<K>
		{
			private int4 coords;

			private UnsafeList<K> contents;

			public int4 Coords => coords;

			public int Length => contents.Length;

			public unsafe void* ContentsPointer => contents.Ptr;

			public K this[int index] => contents.ElementAt(index);

			public Cell(int4 coords)
			{
				this.coords = coords;
				contents = new UnsafeList<K>(4, Allocator.Persistent);
			}

			public void Add(K entity)
			{
				contents.Add(in entity);
			}

			public bool Remove(K entity)
			{
				int num = contents.IndexOf(entity);
				if (num >= 0)
				{
					contents.RemoveAtSwapBack(num);
					return true;
				}
				return false;
			}

			public void Dispose()
			{
				contents.Dispose();
			}
		}

		public const float minSize = 0.01f;

		public const int minLevel = -6;

		public const int maxLevel = 17;

		public NativeParallelHashMap<int4, int> grid;

		public NativeList<Cell<T>> usedCells;

		public NativeParallelHashMap<int, int> populatedLevels;

		public int CellCount => usedCells.Length;

		public NativeMultilevelGrid(int capacity, Allocator label)
		{
			grid = new NativeParallelHashMap<int4, int>(capacity, label);
			usedCells = new NativeList<Cell<T>>(label);
			populatedLevels = new NativeParallelHashMap<int, int>(10, label);
		}

		public void Clear()
		{
			for (int i = 0; i < usedCells.Length; i++)
			{
				usedCells[i].Dispose();
			}
			grid.Clear();
			usedCells.Clear();
			populatedLevels.Clear();
		}

		public void Dispose()
		{
			for (int i = 0; i < usedCells.Length; i++)
			{
				usedCells[i].Dispose();
			}
			grid.Dispose();
			usedCells.Dispose();
			populatedLevels.Dispose();
		}

		public int GetOrCreateCell(int4 cellCoords)
		{
			if (grid.TryGetValue(cellCoords, out var item))
			{
				return item;
			}
			grid.TryAdd(cellCoords, usedCells.Length);
			usedCells.Add(new Cell<T>(cellCoords));
			IncreaseLevelPopulation(cellCoords.w);
			return usedCells.Length - 1;
		}

		public bool TryGetCellIndex(int4 cellCoords, out int cellIndex)
		{
			return grid.TryGetValue(cellCoords, out cellIndex);
		}

		public void RemoveEmpty()
		{
			for (int num = usedCells.Length - 1; num >= 0; num--)
			{
				if (usedCells[num].Length == 0)
				{
					DecreaseLevelPopulation(usedCells[num].Coords.w);
					grid.Remove(usedCells[num].Coords);
					usedCells[num].Dispose();
					usedCells.RemoveAtSwapBack(num);
				}
			}
			for (int i = 0; i < usedCells.Length; i++)
			{
				grid.Remove(usedCells[i].Coords);
				grid.TryAdd(usedCells[i].Coords, i);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GridLevelForSize(float size)
		{
			return math.clamp((int)math.ceil(math.log(size) * 1.442695f), -6, 17) - -6;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CellSizeOfLevel(int level)
		{
			return math.exp2(level + -6);
		}

		public static int4 GetParentCellCoords(int4 cellCoords, int level)
		{
			float num = math.exp2(level - cellCoords[3]);
			int4 result = (int4)math.floor((float4)cellCoords / num);
			result[3] = level;
			return result;
		}

		public void RemoveFromCells(BurstCellSpan span, T content)
		{
			for (int i = span.min[0]; i <= span.max[0]; i++)
			{
				for (int j = span.min[1]; j <= span.max[1]; j++)
				{
					for (int k = span.min[2]; k <= span.max[2]; k++)
					{
						if (TryGetCellIndex(new int4(i, j, k, span.level), out var cellIndex))
						{
							Cell<T> value = usedCells[cellIndex];
							value.Remove(content);
							usedCells[cellIndex] = value;
						}
					}
				}
			}
		}

		public void AddToCells(BurstCellSpan span, T content)
		{
			for (int i = span.min[0]; i <= span.max[0]; i++)
			{
				for (int j = span.min[1]; j <= span.max[1]; j++)
				{
					for (int k = span.min[2]; k <= span.max[2]; k++)
					{
						int orCreateCell = GetOrCreateCell(new int4(i, j, k, span.level));
						Cell<T> value = usedCells[orCreateCell];
						value.Add(content);
						usedCells[orCreateCell] = value;
					}
				}
			}
		}

		public static void GetCellCoordsForBoundsAtLevel(NativeList<int4> coords, BurstAabb bounds, int level, int maxSize = 10)
		{
			coords.Clear();
			float cellSize = CellSizeOfLevel(level);
			int3 int5 = GridHash.Quantize(bounds.min.xyz, cellSize);
			int3 int6 = GridHash.Quantize(bounds.max.xyz, cellSize);
			int6 = int5 + math.min(int6 - int5, new int3(maxSize));
			int3 int7 = int6 - int5 + new int3(1);
			coords.Capacity = int7.x * int7.y * int7.z;
			for (int i = int5[0]; i <= int6[0]; i++)
			{
				for (int j = int5[1]; j <= int6[1]; j++)
				{
					for (int k = int5[2]; k <= int6[2]; k++)
					{
						coords.Add(new int4(i, j, k, level));
					}
				}
			}
		}

		private void IncreaseLevelPopulation(int level)
		{
			int item = 0;
			if (populatedLevels.TryGetValue(level, out item))
			{
				populatedLevels.Remove(level);
			}
			populatedLevels.TryAdd(level, item + 1);
		}

		private void DecreaseLevelPopulation(int level)
		{
			int item = 0;
			if (populatedLevels.TryGetValue(level, out item))
			{
				item--;
				populatedLevels.Remove(level);
				if (item > 0)
				{
					populatedLevels.TryAdd(level, item);
				}
			}
		}
	}
}
