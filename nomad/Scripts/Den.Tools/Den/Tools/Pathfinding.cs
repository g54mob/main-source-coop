using System.Collections.Generic;
using Den.Tools.GUI;
using Den.Tools.Matrices;

namespace Den.Tools
{
	public static class Pathfinding
	{
		public class Factors
		{
			[Val("Incline")]
			public float incline = 0.25f;

			public float lowland = 0.25f;

			public float highland;

			public float straighten = 0.25f;

			public float distance = 0.25f;

			private float this[int num]
			{
				get
				{
					return num switch
					{
						0 => incline, 
						1 => lowland, 
						2 => highland, 
						3 => straighten, 
						_ => distance, 
					};
				}
				set
				{
					switch (num)
					{
					case 0:
						incline = value;
						break;
					case 1:
						lowland = value;
						break;
					case 2:
						highland = value;
						break;
					case 3:
						straighten = value;
						break;
					default:
						distance = value;
						break;
					}
				}
			}

			public float Incline
			{
				set
				{
					SetNormalizedVal(0, value);
				}
			}

			public float Lowland
			{
				set
				{
					SetNormalizedVal(1, value);
				}
			}

			public float Highland
			{
				set
				{
					SetNormalizedVal(2, value);
				}
			}

			public float Straighten
			{
				set
				{
					SetNormalizedVal(3, value);
				}
			}

			public float Distance
			{
				set
				{
					SetNormalizedVal(4, value);
				}
			}

			private void SetNormalizedVal(int num, float val)
			{
				if (val < 0f)
				{
					val = 0f;
				}
				if (val > 1f)
				{
					val = 1f;
				}
				this[num] = 0f;
				float num2 = incline + lowland + highland + straighten + distance;
				for (int i = 0; i < 5; i++)
				{
					this[i] = ((num2 != 0f) ? (this[i] / num2 * (1f - val)) : ((1f - val) / 4f));
				}
				this[num] = val;
			}
		}

		public class FixedList<T>
		{
			public T[] arr;

			public T count;

			public FixedList(int capacity)
			{
				arr = new T[capacity];
			}

			public FixedList(T[] arr)
			{
				this.arr = arr;
			}
		}

		private static readonly Coord[] neigDiagonalFirst = new Coord[8]
		{
			new Coord(-1, -1),
			new Coord(-1, 1),
			new Coord(1, 1),
			new Coord(1, -1),
			new Coord(0, -1),
			new Coord(-1, 0),
			new Coord(0, 1),
			new Coord(1, 0)
		};

		private static readonly Coord[] neigLineFirst = new Coord[8]
		{
			new Coord(0, -1),
			new Coord(-1, 0),
			new Coord(0, 1),
			new Coord(1, 0),
			new Coord(-1, -1),
			new Coord(-1, 1),
			new Coord(1, 1),
			new Coord(1, -1)
		};

		private static readonly bool[] directionRnd = new bool[101]
		{
			true, false, false, false, true, false, true, false, true, false,
			true, true, true, false, true, true, true, false, true, false,
			true, true, false, false, false, false, false, false, false, false,
			false, true, true, true, false, false, true, true, true, true,
			false, false, false, true, false, false, false, false, true, false,
			true, false, true, false, true, false, true, false, false, true,
			false, true, true, false, false, true, false, true, true, true,
			false, true, true, false, true, true, false, false, false, false,
			false, true, false, true, false, true, false, true, true, false,
			false, true, false, false, true, true, false, false, false, false,
			true
		};

		public static float CalcWeight(Coord coord, Coord dir, MatrixWorld heights, Matrix mask, Factors factors)
		{
			CoordRect coordRect = heights?.rect ?? mask.rect;
			int num = (coord.z - coordRect.offset.z) * coordRect.size.x + coord.x - coordRect.offset.x;
			int num2 = num + coordRect.size.x * dir.z + dir.x;
			float num3 = 1f;
			if (dir.x * dir.z != 0)
			{
				num3 = 1.4142135f;
			}
			float num4 = num3;
			float num5 = 1f;
			if (mask != null)
			{
				num5 = mask.arr[num2];
				num5 = ((!(num5 > 0.999f)) ? (1f / (1f - mask.arr[num2]) + 1f) : 3.4028235E+38f);
			}
			float num6 = 1f;
			if (heights != null)
			{
				float num7 = heights.arr[num2];
				float num8 = heights.worldSize.x / (float)heights.rect.size.x;
				float num9 = num7 - heights.arr[num];
				if (num9 < 0f)
				{
					num9 = 0f - num9;
				}
				num9 *= heights.worldSize.y;
				num9 /= num8 * num3;
				num6 = num9 / factors.incline;
				num6 = ((!(num6 > 0.999f)) ? (1f / (1f - num6)) : 3.4028235E+38f);
			}
			float num10 = 1f;
			return num5 * num4 * num6 * num10;
		}

		public static void FillDirs(Matrix2D<Coord> dirs, Coord to, MatrixWorld heights, Matrix mask, Factors factors, Matrix weights = null, FixedList<int> changedPoses = null, FixedList<int> newChangedPoses = null, int maxIterations = -1)
		{
			CoordRect rect = heights?.rect ?? mask.rect;
			Coord offset = rect.offset;
			Coord coord = rect.offset + rect.size;
			if (weights == null)
			{
				weights = new Matrix(rect);
			}
			if (dirs == null)
			{
				dirs = new Matrix2D<Coord>(rect);
			}
			if (changedPoses == null)
			{
				changedPoses = new FixedList<int>(10000);
			}
			if (newChangedPoses == null)
			{
				newChangedPoses = new FixedList<int>(10000);
			}
			weights.Fill(3.4028235E+38f);
			weights[to] = 0f;
			dirs.Fill(default(Coord));
			changedPoses.arr[0] = rect.GetPos(to);
			changedPoses.count = 1;
			if (maxIterations < 0)
			{
				maxIterations = rect.size.x + rect.size.z;
			}
			_ = rect.Min;
			_ = rect.Max;
			for (int i = 0; i < maxIterations; i++)
			{
				for (int j = 0; j < changedPoses.count; j++)
				{
					int num = changedPoses.arr[j];
					Coord coord2 = rect.GetCoord(num);
					if (coord2.x < offset.x || coord2.x > coord.x - 1 || coord2.z < offset.z || coord2.z > coord.z - 1)
					{
						return;
					}
					float num2 = weights.arr[num];
					for (int k = 0; k < 8; k++)
					{
						Coord coord3 = neigDiagonalFirst[k];
						if ((coord2.x != offset.x || coord3.x != -1) && (coord2.x != coord.x - 1 || coord3.x != 1) && (coord2.z != offset.z || coord3.z != -1) && (coord2.z != coord.z - 1 || coord3.z != 1))
						{
							int num3 = num + rect.size.x * coord3.z + coord3.x;
							float num4 = weights.arr[num3];
							float num5 = CalcWeight(coord2, coord3, heights, mask, factors) + num2;
							if (num5 < num4 - 0.0001f)
							{
								weights.arr[num3] = num5;
								dirs.arr[num3] = coord3;
								newChangedPoses.arr[newChangedPoses.count] = num3;
								newChangedPoses.count++;
							}
						}
					}
				}
				FixedList<int> fixedList = changedPoses;
				changedPoses = newChangedPoses;
				newChangedPoses = fixedList;
				newChangedPoses.count = 0;
			}
		}

		public static Coord[] DrawPath(Matrix2D<Coord> dirs, Coord from, Coord to)
		{
			List<Coord> list = new List<Coord>();
			list.Add(from);
			Coord coord = from;
			for (int i = 0; i < 1000; i++)
			{
				coord -= dirs[coord];
				list.Add(coord);
				if (coord == to)
				{
					break;
				}
			}
			return list.ToArray();
		}
	}
}
