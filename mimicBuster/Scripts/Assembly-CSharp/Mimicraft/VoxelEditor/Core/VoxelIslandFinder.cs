using System;
using System.Collections.Generic;

namespace Mimicraft.VoxelEditor.Core
{
	public static class VoxelIslandFinder
	{
		private static int[] runX0 = new int[256];

		private static int[] runX1 = new int[256];

		private static int[] runRow = new int[256];

		private static int[] parent = new int[256];

		private static int[] weight = new int[256];

		private static int[] slot = new int[256];

		private static int[] rowStart = new int[64];

		public static void Find(bool[] occupancy, int sizeX, int sizeY, int sizeZ, List<VoxelIsland> into)
		{
			if (occupancy == null || into == null || sizeX <= 0 || sizeY <= 0 || sizeZ <= 0)
			{
				return;
			}
			int num = sizeY * sizeZ;
			if (occupancy.Length < num * sizeX)
			{
				return;
			}
			if (rowStart.Length < num + 1)
			{
				rowStart = new int[num + 1];
			}
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				rowStart[i] = num2;
				int num3 = i * sizeX;
				int j = 0;
				while (j < sizeX)
				{
					if (!occupancy[num3 + j])
					{
						j++;
						continue;
					}
					int num4 = j;
					for (; j < sizeX && occupancy[num3 + j]; j++)
					{
					}
					if (num2 == runX0.Length)
					{
						Grow(num2 * 2);
					}
					runX0[num2] = num4;
					runX1[num2] = j - 1;
					runRow[num2] = i;
					parent[num2] = num2;
					weight[num2] = 1;
					num2++;
				}
			}
			rowStart[num] = num2;
			if (num2 == 0)
			{
				return;
			}
			for (int k = 0; k < sizeY; k++)
			{
				for (int l = 0; l < sizeZ; l++)
				{
					int num5 = k * sizeZ + l;
					if (rowStart[num5] == rowStart[num5 + 1])
					{
						continue;
					}
					if (l + 1 < sizeZ)
					{
						Join(num5, num5 + 1);
					}
					if (k + 1 < sizeY)
					{
						int num6 = num5 + sizeZ;
						if (l > 0)
						{
							Join(num5, num6 - 1);
						}
						Join(num5, num6);
						if (l + 1 < sizeZ)
						{
							Join(num5, num6 + 1);
						}
					}
				}
			}
			_ = into.Count;
			for (int m = 0; m < num2; m++)
			{
				if (Root(m) == m)
				{
					slot[m] = -1;
				}
			}
			for (int n = 0; n < num2; n++)
			{
				int num7 = Root(n);
				int num8 = runRow[n] / sizeZ;
				int num9 = runRow[n] - num8 * sizeZ;
				if (slot[num7] < 0)
				{
					slot[num7] = into.Count;
					into.Add(new VoxelIsland
					{
						MinX = runX0[n],
						MaxX = runX1[n],
						MinY = num8,
						MaxY = num8,
						MinZ = num9,
						MaxZ = num9,
						Voxels = runX1[n] - runX0[n] + 1
					});
					continue;
				}
				VoxelIsland value = into[slot[num7]];
				value.Voxels += runX1[n] - runX0[n] + 1;
				if (runX0[n] < value.MinX)
				{
					value.MinX = runX0[n];
				}
				if (runX1[n] > value.MaxX)
				{
					value.MaxX = runX1[n];
				}
				if (num8 < value.MinY)
				{
					value.MinY = num8;
				}
				if (num8 > value.MaxY)
				{
					value.MaxY = num8;
				}
				if (num9 < value.MinZ)
				{
					value.MinZ = num9;
				}
				if (num9 > value.MaxZ)
				{
					value.MaxZ = num9;
				}
				into[slot[num7]] = value;
			}
		}

		private static void Join(int rowA, int rowB)
		{
			int num = rowStart[rowA];
			int num2 = rowStart[rowA + 1];
			int num3 = rowStart[rowB];
			int num4 = rowStart[rowB + 1];
			while (num < num2 && num3 < num4)
			{
				if (runX0[num] <= runX1[num3] + 1 && runX0[num3] <= runX1[num] + 1 && parent[num] != parent[num3])
				{
					Union(num, num3);
				}
				if (runX1[num] < runX1[num3])
				{
					num++;
				}
				else
				{
					num3++;
				}
			}
		}

		private static int Root(int i)
		{
			while (parent[i] != i)
			{
				parent[i] = parent[parent[i]];
				i = parent[i];
			}
			return i;
		}

		private static void Union(int a, int b)
		{
			int num = Root(a);
			int num2 = Root(b);
			if (num != num2)
			{
				if (weight[num] < weight[num2])
				{
					int num3 = num;
					num = num2;
					num2 = num3;
				}
				parent[num2] = num;
				weight[num] += weight[num2];
			}
		}

		private static void Grow(int size)
		{
			Array.Resize(ref runX0, size);
			Array.Resize(ref runX1, size);
			Array.Resize(ref runRow, size);
			Array.Resize(ref parent, size);
			Array.Resize(ref weight, size);
			Array.Resize(ref slot, size);
		}
	}
}
