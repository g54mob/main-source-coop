using System;
using System.Collections.Generic;

namespace Mimicraft.Export
{
	public static class VoxelExportMesher
	{
		public sealed class Result
		{
			public readonly List<ExportMesh> Meshes = new List<ExportMesh>();

			public int AtlasWidth;

			public int AtlasHeight;

			public byte[] AtlasRgba;

			public int QuadCount;
		}

		private struct FaceQuad
		{
			public int Piece;

			public int Face;

			public int Slice;

			public int U0;

			public int V0;

			public int W;

			public int H;

			public int X;

			public int Y;
		}

		public const int MaxAtlasSize = 16384;

		private static readonly int[] NormalAxis = new int[6] { 2, 2, 1, 1, 0, 0 };

		private static readonly int[] NormalSign = new int[6] { -1, 1, 1, -1, -1, 1 };

		private static readonly int[] AxisU = new int[6] { 0, 0, 0, 0, 2, 2 };

		private static readonly int[] AxisV = new int[6] { 1, 1, 2, 2, 1, 1 };

		public static Result Build(IReadOnlyList<IVoxelSource> pieces, IReadOnlyList<string> names, int pixelsPerVoxel, int padding, bool powerOfTwo)
		{
			pixelsPerVoxel = Math.Max(1, pixelsPerVoxel);
			padding = Math.Max(0, padding);
			List<FaceQuad> list = new List<FaceQuad>();
			for (int i = 0; i < pieces.Count; i++)
			{
				Sweep(pieces[i], i, list);
			}
			Result result = new Result
			{
				QuadCount = list.Count
			};
			if (list.Count == 0)
			{
				return result;
			}
			Pack(list, pixelsPerVoxel, padding, powerOfTwo, out var width, out var height);
			result.AtlasWidth = width;
			result.AtlasHeight = height;
			result.AtlasRgba = new byte[width * height * 4];
			for (int j = 0; j < pieces.Count; j++)
			{
				result.Meshes.Add(new ExportMesh
				{
					Name = ((names != null && j < names.Count) ? (names[j] ?? "") : "")
				});
			}
			List<Dictionary<(int, int, int), int>> list2 = new List<Dictionary<(int, int, int), int>>();
			for (int k = 0; k < pieces.Count; k++)
			{
				list2.Add(new Dictionary<(int, int, int), int>());
			}
			foreach (FaceQuad item in list)
			{
				Paint(pieces[item.Piece], item, pixelsPerVoxel, padding, result.AtlasRgba, width, height);
				Emit(item, pixelsPerVoxel, width, height, result.Meshes[item.Piece], list2[item.Piece]);
			}
			return result;
		}

		private static void Sweep(IVoxelSource source, int piece, List<FaceQuad> quads)
		{
			if (source == null || !source.TryGetBounds(out var minX, out var minY, out var minZ, out var maxX, out var maxY, out var maxZ))
			{
				return;
			}
			int[] array = new int[3] { minX, minY, minZ };
			int[] array2 = new int[3] { maxX, maxY, maxZ };
			int[] array3 = new int[3];
			for (int i = 0; i < 6; i++)
			{
				int num = NormalAxis[i];
				int num2 = AxisU[i];
				int num3 = AxisV[i];
				int num4 = NormalSign[i];
				int num5 = array2[num2] - array[num2] + 1;
				int num6 = array2[num3] - array[num3] + 1;
				bool[] array4 = new bool[num5 * num6];
				for (int j = array[num]; j <= array2[num]; j++)
				{
					bool flag = false;
					for (int k = 0; k < num6; k++)
					{
						for (int l = 0; l < num5; l++)
						{
							array3[num] = j;
							array3[num2] = array[num2] + l;
							array3[num3] = array[num3] + k;
							bool flag2 = source.IsSolid(array3[0], array3[1], array3[2]);
							if (flag2)
							{
								array3[num] = j + num4;
								flag2 = !source.IsSolid(array3[0], array3[1], array3[2]);
							}
							array4[k * num5 + l] = flag2;
							flag = flag || flag2;
						}
					}
					if (!flag)
					{
						continue;
					}
					for (int m = 0; m < num6; m++)
					{
						for (int n = 0; n < num5; n++)
						{
							if (!array4[m * num5 + n])
							{
								continue;
							}
							int num7;
							for (num7 = 1; n + num7 < num5 && array4[m * num5 + n + num7]; num7++)
							{
							}
							int num8;
							for (num8 = 1; m + num8 < num6 && RowSet(array4, num5, n, m + num8, num7); num8++)
							{
							}
							for (int num9 = 0; num9 < num8; num9++)
							{
								for (int num10 = 0; num10 < num7; num10++)
								{
									array4[(m + num9) * num5 + n + num10] = false;
								}
							}
							quads.Add(new FaceQuad
							{
								Piece = piece,
								Face = i,
								Slice = j,
								U0 = array[num2] + n,
								V0 = array[num3] + m,
								W = num7,
								H = num8
							});
						}
					}
				}
			}
		}

		private static bool RowSet(bool[] mask, int stride, int u0, int row, int width)
		{
			for (int i = 0; i < width; i++)
			{
				if (!mask[row * stride + u0 + i])
				{
					return false;
				}
			}
			return true;
		}

		private static void Pack(List<FaceQuad> quads, int pixelsPerVoxel, int padding, bool powerOfTwo, out int width, out int height)
		{
			int count = quads.Count;
			int[] array = new int[count];
			long num = 0L;
			int val = 1;
			for (int i = 0; i < count; i++)
			{
				array[i] = i;
				int num2 = quads[i].W * pixelsPerVoxel + padding * 2;
				int num3 = quads[i].H * pixelsPerVoxel + padding * 2;
				num += (long)num2 * (long)num3;
				val = Math.Max(val, num2);
			}
			Array.Sort(array, delegate(int x, int y)
			{
				int num6 = quads[y].H.CompareTo(quads[x].H);
				return (num6 == 0) ? quads[y].W.CompareTo(quads[x].W) : num6;
			});
			int num4 = Math.Max(val, (int)Math.Ceiling(Math.Sqrt(num)));
			if (powerOfTwo)
			{
				num4 = NextPowerOfTwo(num4);
			}
			int num5;
			while (true)
			{
				if (num4 > 16384)
				{
					throw new InvalidOperationException($"The texture would be larger than {16384} pixels across. " + "Lower Pixels Per Voxel or the padding.");
				}
				num5 = Shelve(quads, array, pixelsPerVoxel, padding, num4);
				if (num5 <= num4 || (!powerOfTwo && num5 <= 16384))
				{
					break;
				}
				num4 = (powerOfTwo ? (num4 * 2) : (num4 + Math.Max(16, num4 / 4)));
			}
			width = num4;
			height = (powerOfTwo ? NextPowerOfTwo(num5) : num5);
		}

		private static int Shelve(List<FaceQuad> quads, int[] order, int pixelsPerVoxel, int padding, int width)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (int index in order)
			{
				FaceQuad value = quads[index];
				int num4 = value.W * pixelsPerVoxel + padding * 2;
				int val = value.H * pixelsPerVoxel + padding * 2;
				if (num + num4 > width)
				{
					num = 0;
					num2 += num3;
					num3 = 0;
				}
				value.X = num + padding;
				value.Y = num2 + padding;
				quads[index] = value;
				num += num4;
				num3 = Math.Max(num3, val);
			}
			return num2 + num3;
		}

		private static int NextPowerOfTwo(int value)
		{
			int num;
			for (num = 1; num < value; num <<= 1)
			{
			}
			return num;
		}

		private static void Paint(IVoxelSource source, FaceQuad quad, int pixelsPerVoxel, int padding, byte[] rgba, int width, int height)
		{
			int num = NormalAxis[quad.Face];
			int num2 = AxisU[quad.Face];
			int num3 = AxisV[quad.Face];
			int num4 = quad.W * pixelsPerVoxel;
			int num5 = quad.H * pixelsPerVoxel;
			int[] array = new int[3];
			for (int i = -padding; i < num5 + padding; i++)
			{
				int num6 = quad.Y + i;
				if (num6 < 0 || num6 >= height)
				{
					continue;
				}
				int num7 = Math.Min(Math.Max(i, 0), num5 - 1) / pixelsPerVoxel;
				for (int j = -padding; j < num4 + padding; j++)
				{
					int num8 = quad.X + j;
					if (num8 >= 0 && num8 < width)
					{
						int num9 = Math.Min(Math.Max(j, 0), num4 - 1) / pixelsPerVoxel;
						array[num] = quad.Slice;
						array[num2] = quad.U0 + num9;
						array[num3] = quad.V0 + num7;
						uint num10 = source.FaceColor(array[0], array[1], array[2], quad.Face);
						int num11 = (num6 * width + num8) * 4;
						rgba[num11] = (byte)(num10 & 0xFF);
						rgba[num11 + 1] = (byte)((num10 >> 8) & 0xFF);
						rgba[num11 + 2] = (byte)((num10 >> 16) & 0xFF);
						rgba[num11 + 3] = byte.MaxValue;
					}
				}
			}
		}

		private static void Emit(FaceQuad quad, int pixelsPerVoxel, int atlasWidth, int atlasHeight, ExportMesh mesh, Dictionary<(int, int, int), int> weld)
		{
			int a = NormalAxis[quad.Face];
			int u = AxisU[quad.Face];
			int v = AxisV[quad.Face];
			int num = NormalSign[quad.Face];
			int plane = quad.Slice + ((num > 0) ? 1 : 0);
			int cu = quad.U0 + quad.W;
			int cv = quad.V0 + quad.H;
			float tx = (float)quad.X / (float)atlasWidth;
			float ty = (float)quad.Y / (float)atlasHeight;
			float tx2 = (float)(quad.X + quad.W * pixelsPerVoxel) / (float)atlasWidth;
			float ty2 = (float)(quad.Y + quad.H * pixelsPerVoxel) / (float)atlasHeight;
			AddCorner(mesh, weld, a, u, v, plane, quad.U0, quad.V0, num, tx, ty);
			AddCorner(mesh, weld, a, u, v, plane, cu, quad.V0, num, tx2, ty);
			AddCorner(mesh, weld, a, u, v, plane, cu, cv, num, tx2, ty2);
			AddCorner(mesh, weld, a, u, v, plane, quad.U0, cv, num, tx, ty2);
		}

		private static void AddCorner(ExportMesh mesh, Dictionary<(int, int, int), int> weld, int a, int u, int v, int plane, int cu, int cv, int sign, float tx, float ty)
		{
			int[] array = new int[3];
			array[a] = plane;
			array[u] = cu;
			array[v] = cv;
			(int, int, int) key = (array[0], array[1], array[2]);
			if (!weld.TryGetValue(key, out var value))
			{
				value = (weld[key] = mesh.PointCount);
				mesh.Positions.Add(array[0]);
				mesh.Positions.Add(array[1]);
				mesh.Positions.Add(array[2]);
			}
			mesh.Corners.Add(value);
			float[] array2 = new float[3];
			array2[a] = sign;
			mesh.Normals.Add(array2[0]);
			mesh.Normals.Add(array2[1]);
			mesh.Normals.Add(array2[2]);
			mesh.Uvs.Add(tx);
			mesh.Uvs.Add(ty);
		}
	}
}
