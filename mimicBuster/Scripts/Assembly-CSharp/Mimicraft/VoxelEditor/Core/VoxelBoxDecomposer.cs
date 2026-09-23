using System;
using System.Collections.Generic;

namespace Mimicraft.VoxelEditor.Core
{
	public static class VoxelBoxDecomposer
	{
		private static bool[] used = new bool[4096];

		private static bool[] coarse = new bool[4096];

		private const int MergeCeiling = 512;

		public static void Decompose(bool[] occupancy, int sizeX, int sizeY, int sizeZ, List<VoxelBox> into)
		{
			if (occupancy == null || into == null || sizeX <= 0 || sizeY <= 0 || sizeZ <= 0)
			{
				return;
			}
			int num = sizeX * sizeY * sizeZ;
			if (occupancy.Length < num)
			{
				return;
			}
			if (used.Length < num)
			{
				used = new bool[num];
			}
			else
			{
				Array.Clear(used, 0, num);
			}
			for (int i = 0; i < sizeY; i++)
			{
				for (int j = 0; j < sizeZ; j++)
				{
					int num2 = (i * sizeZ + j) * sizeX;
					for (int k = 0; k < sizeX; k++)
					{
						if (!occupancy[num2 + k] || used[num2 + k])
						{
							continue;
						}
						int l;
						for (l = k; l + 1 < sizeX && occupancy[num2 + l + 1] && !used[num2 + l + 1]; l++)
						{
						}
						int m;
						for (m = j; m + 1 < sizeZ && RunFree(occupancy, (i * sizeZ + m + 1) * sizeX, k, l); m++)
						{
						}
						int n;
						for (n = i; n + 1 < sizeY && RectFree(occupancy, n + 1, j, m, k, l, sizeX, sizeZ); n++)
						{
						}
						for (int num3 = i; num3 <= n; num3++)
						{
							for (int num4 = j; num4 <= m; num4++)
							{
								int num5 = (num3 * sizeZ + num4) * sizeX;
								for (int num6 = k; num6 <= l; num6++)
								{
									used[num5 + num6] = true;
								}
							}
						}
						into.Add(new VoxelBox
						{
							MinX = k,
							MinY = i,
							MinZ = j,
							SizeX = l - k + 1,
							SizeY = n - i + 1,
							SizeZ = m - j + 1
						});
					}
				}
			}
		}

		public static void DecomposeCapped(bool[] occupancy, int sizeX, int sizeY, int sizeZ, int maxBoxes, List<VoxelBox> into)
		{
			if (into == null)
			{
				return;
			}
			maxBoxes = Math.Max(1, maxBoxes);
			int count = into.Count;
			Decompose(occupancy, sizeX, sizeY, sizeZ, into);
			if (into.Count - count > maxBoxes)
			{
				List<VoxelBox> list = new List<VoxelBox>(into.Count - count);
				for (int i = count; i < into.Count; i++)
				{
					list.Add(into[i]);
				}
				into.RemoveRange(count, into.Count - count);
				int num = 2;
				while (list.Count > 512)
				{
					Coarsened(occupancy, sizeX, sizeY, sizeZ, num, list);
					num *= 2;
				}
				Merge(list, maxBoxes);
				into.AddRange(list);
			}
		}

		private static void Coarsened(bool[] occupancy, int sizeX, int sizeY, int sizeZ, int factor, List<VoxelBox> boxes)
		{
			boxes.Clear();
			int num = (sizeX + factor - 1) / factor;
			int num2 = (sizeY + factor - 1) / factor;
			int num3 = (sizeZ + factor - 1) / factor;
			if (num == 1 && num2 == 1 && num3 == 1)
			{
				boxes.Add(new VoxelBox
				{
					SizeX = sizeX,
					SizeY = sizeY,
					SizeZ = sizeZ
				});
				return;
			}
			int num4 = num * num2 * num3;
			if (coarse.Length < num4)
			{
				coarse = new bool[num4];
			}
			else
			{
				Array.Clear(coarse, 0, num4);
			}
			for (int i = 0; i < sizeY; i++)
			{
				int num5 = i / factor;
				for (int j = 0; j < sizeZ; j++)
				{
					int num6 = j / factor;
					int num7 = (i * sizeZ + j) * sizeX;
					int num8 = (num5 * num3 + num6) * num;
					for (int k = 0; k < sizeX; k++)
					{
						if (occupancy[num7 + k])
						{
							coarse[num8 + k / factor] = true;
						}
					}
				}
			}
			Decompose(coarse, num, num2, num3, boxes);
			for (int l = 0; l < boxes.Count; l++)
			{
				VoxelBox value = boxes[l];
				int num9 = value.MinX * factor;
				int num10 = value.MinY * factor;
				int num11 = value.MinZ * factor;
				value.SizeX = Math.Min((value.MinX + value.SizeX) * factor, sizeX) - num9;
				value.SizeY = Math.Min((value.MinY + value.SizeY) * factor, sizeY) - num10;
				value.SizeZ = Math.Min((value.MinZ + value.SizeZ) * factor, sizeZ) - num11;
				value.MinX = num9;
				value.MinY = num10;
				value.MinZ = num11;
				boxes[l] = value;
			}
		}

		private static void Merge(List<VoxelBox> boxes, int max)
		{
			int count = boxes.Count;
			if (count <= max)
			{
				return;
			}
			bool[] array = new bool[count];
			int[] array2 = new int[count];
			long[] array3 = new long[count];
			VoxelBox[] array4 = boxes.ToArray();
			for (int i = 0; i < count; i++)
			{
				array[i] = true;
			}
			for (int j = 0; j < count; j++)
			{
				BestPartner(array4, array, j, count, array2, array3);
			}
			int num = count;
			int num2 = count * 8;
			while (num > max && num2-- > 0)
			{
				int num3 = -1;
				long num4 = long.MaxValue;
				for (int k = 0; k < count; k++)
				{
					if (array[k] && array2[k] >= 0 && array3[k] < num4)
					{
						num4 = array3[k];
						num3 = k;
					}
				}
				if (num3 < 0)
				{
					break;
				}
				int num5 = array2[num3];
				if (!array[num5])
				{
					BestPartner(array4, array, num3, count, array2, array3);
					continue;
				}
				long num6 = Waste(array4[num3], array4[num5]);
				if (num6 != array3[num3])
				{
					array3[num3] = num6;
					continue;
				}
				array4[num3] = Union(array4[num3], array4[num5]);
				array[num5] = false;
				num--;
				BestPartner(array4, array, num3, count, array2, array3);
				for (int l = 0; l < count; l++)
				{
					if (array[l] && l != num3)
					{
						long num7 = Waste(array4[l], array4[num3]);
						if (num7 < array3[l])
						{
							array3[l] = num7;
							array2[l] = num3;
						}
					}
				}
			}
			while (num > max)
			{
				int num8 = -1;
				int num9 = -1;
				for (int m = 0; m < count; m++)
				{
					if (num9 >= 0)
					{
						break;
					}
					if (array[m])
					{
						if (num8 < 0)
						{
							num8 = m;
						}
						else
						{
							num9 = m;
						}
					}
				}
				if (num9 < 0)
				{
					break;
				}
				array4[num8] = Union(array4[num8], array4[num9]);
				array[num9] = false;
				num--;
			}
			boxes.Clear();
			for (int n = 0; n < count; n++)
			{
				if (array[n])
				{
					boxes.Add(array4[n]);
				}
			}
		}

		private static void BestPartner(VoxelBox[] list, bool[] alive, int i, int n, int[] partner, long[] cost)
		{
			partner[i] = -1;
			cost[i] = long.MaxValue;
			for (int j = 0; j < n; j++)
			{
				if (j != i && alive[j])
				{
					long num = Waste(list[i], list[j]);
					if (num < cost[i])
					{
						cost[i] = num;
						partner[i] = j;
					}
				}
			}
		}

		private static VoxelBox Union(VoxelBox a, VoxelBox b)
		{
			int num = Math.Min(a.MinX, b.MinX);
			int num2 = Math.Min(a.MinY, b.MinY);
			int num3 = Math.Min(a.MinZ, b.MinZ);
			return new VoxelBox
			{
				MinX = num,
				MinY = num2,
				MinZ = num3,
				SizeX = Math.Max(a.MinX + a.SizeX, b.MinX + b.SizeX) - num,
				SizeY = Math.Max(a.MinY + a.SizeY, b.MinY + b.SizeY) - num2,
				SizeZ = Math.Max(a.MinZ + a.SizeZ, b.MinZ + b.SizeZ) - num3
			};
		}

		private static long Waste(VoxelBox a, VoxelBox b)
		{
			return Math.Max(0L, (long)Union(a, b).Volume - (long)a.Volume - b.Volume);
		}

		private static bool RunFree(bool[] occupancy, int row, int x0, int x1)
		{
			for (int i = x0; i <= x1; i++)
			{
				if (!occupancy[row + i] || used[row + i])
				{
					return false;
				}
			}
			return true;
		}

		private static bool RectFree(bool[] occupancy, int y, int z0, int z1, int x0, int x1, int sizeX, int sizeZ)
		{
			for (int i = z0; i <= z1; i++)
			{
				if (!RunFree(occupancy, (y * sizeZ + i) * sizeX, x0, x1))
				{
					return false;
				}
			}
			return true;
		}
	}
}
