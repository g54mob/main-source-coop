using System;
using System.Collections.Generic;
using Den.Tools.Matrices;
using UnityEngine;

namespace Den.Tools.Splines
{
	public static class SplineMatrixOps
	{
		public struct MetaLine
		{
			public Coord c0;

			public Coord c1;

			public Coord c2;

			public Coord c3;

			public byte count;

			public bool closed;

			public const int length = 256;

			public Coord this[int n]
			{
				get
				{
					return n switch
					{
						0 => c0, 
						1 => c1, 
						2 => c2, 
						3 => c3, 
						_ => throw new Exception($"PixelLine array index ({n}) out of range"), 
					};
				}
				set
				{
					switch (n)
					{
					case 0:
						c0 = value;
						break;
					case 1:
						c1 = value;
						break;
					case 2:
						c2 = value;
						break;
					case 3:
						c3 = value;
						break;
					default:
						throw new Exception($"PixelLine array index ({n}) out of range");
					}
				}
			}

			public Coord Last => count switch
			{
				1 => c0, 
				2 => c1, 
				3 => c2, 
				4 => c3, 
				_ => throw new Exception($"PixelLine array index ({count - 1}) out of range"), 
			};

			public static void AddToList(List<Coord> list, MetaLine line)
			{
				if (line.count != 0)
				{
					if (line.count >= 1)
					{
						list.Add(line.c0);
					}
					if (line.count >= 2)
					{
						list.Add(line.c1);
					}
					if (line.count >= 3)
					{
						list.Add(line.c2);
					}
					if (line.count == 4)
					{
						list.Add(line.c3);
					}
					if (line.closed)
					{
						list.Add(line.c0);
					}
				}
			}

			public MetaLine(Coord c0, Coord c1, Coord c2, Coord c3)
			{
				this.c0 = c0;
				this.c1 = c1;
				this.c2 = c2;
				this.c3 = c3;
				count = 4;
				closed = false;
			}

			public MetaLine(int c0x, int c0z, int c1x, int c1z, int c2x, int c2z, int c3x, int c3z)
			{
				c0.x = c0x;
				c0.z = c0z;
				c1.x = c1x;
				c1.z = c1z;
				c2.x = c2x;
				c2.z = c2z;
				c3.x = c3x;
				c3.z = c3z;
				count = 4;
				closed = false;
			}

			public MetaLine(int c0x, int c0z, int c1x, int c1z, int c2x, int c2z)
			{
				c0.x = c0x;
				c0.z = c0z;
				c1.x = c1x;
				c1.z = c1z;
				c2.x = c2x;
				c2.z = c2z;
				c3.x = 0;
				c3.z = 0;
				count = 3;
				closed = false;
			}

			public MetaLine(int c0x, int c0z, int c1x, int c1z)
			{
				c0.x = c0x;
				c0.z = c0z;
				c1.x = c1x;
				c1.z = c1z;
				c2.x = 0;
				c2.z = 0;
				c3.x = 0;
				c3.z = 0;
				count = 2;
				closed = false;
			}
		}

		public static void Stroke(SplineSys spline, MatrixWorld matrix, bool white = false, float intensity = 1f, bool antialiased = false, bool padOnePixel = false)
		{
			Line[] lines = spline.lines;
			foreach (Line line in lines)
			{
				for (int j = 0; j < line.segments.Length; j++)
				{
					int num = (int)(line.segments[j].length / matrix.PixelSize.x * 0.1f + 1f);
					Vector3 pos = line.segments[j].start.pos;
					Vector3 vector = matrix.WorldToPixelInterpolated(pos.x, pos.z);
					float valStart = (white ? intensity : (pos.y / matrix.worldSize.y));
					for (int k = 0; k < num; k++)
					{
						float p = ((num != 1) ? (1f * (float)k / (float)(num - 1)) : 1f);
						Vector3 point = line.segments[j].GetPoint(p);
						float num2 = (white ? intensity : (point.y / matrix.worldSize.y));
						point = matrix.WorldToPixelInterpolated(point.x, point.z);
						matrix.Line((Vector2D)vector, (Vector2D)point, valStart, num2, antialiased, padOnePixel, k == num - 1);
						vector = point;
						valStart = num2;
					}
				}
			}
		}

		public static void Silhouette(SplineSys spline, MatrixWorld matrix)
		{
			Stroke(spline, matrix, white: true, 0.5f);
			Silhouette(spline, matrix, matrix);
		}

		public static void Silhouette(SplineSys spline, MatrixWorld strokeMatrix, MatrixWorld dstMatrix)
		{
			if (strokeMatrix != dstMatrix)
			{
				dstMatrix.Fill(strokeMatrix);
			}
			CoordRect rect = dstMatrix.rect;
			Coord min = rect.Min;
			Coord max = rect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					int num = (j - rect.offset.z) * rect.size.x + i - rect.offset.x;
					if (dstMatrix.arr[num] < 0.01f)
					{
						Vector3 point = dstMatrix.PixelToWorld(i, j);
						bool flag = spline.Handness(point) >= 0f;
						dstMatrix.PaintBucket(new Coord(i, j), flag ? 0.75f : 0.25f);
					}
				}
			}
		}

		public static void PaintBucket(this MatrixWorld matrix, Coord coord, float val, float threshold = 0.0001f, int maxIterations = 10)
		{
			CoordRect rect = matrix.rect;
			Coord min = rect.Min;
			Coord max = rect.Max;
			MatrixOps.Stripe stripe = new MatrixOps.Stripe(Mathf.Max(rect.size.x, rect.size.z));
			stripe.length = rect.size.x;
			matrix[coord] = -256f;
			MatrixOps.ReadRow(stripe, matrix, coord.x, matrix.rect.offset.z);
			PaintBucketMaskStripe(stripe, threshold);
			MatrixOps.WriteRow(stripe, matrix, coord.x, matrix.rect.offset.z);
			for (int i = 0; i < maxIterations; i++)
			{
				bool flag = false;
				for (int j = min.z; j < max.z; j++)
				{
					MatrixOps.ReadLine(stripe, matrix, rect.offset.x, j);
					flag = PaintBucketMaskStripe(stripe, threshold) || flag;
					MatrixOps.WriteLine(stripe, matrix, rect.offset.x, j);
				}
				for (int k = min.x; k < max.x; k++)
				{
					MatrixOps.ReadRow(stripe, matrix, k, matrix.rect.offset.z);
					flag = PaintBucketMaskStripe(stripe, threshold) || flag;
					MatrixOps.WriteRow(stripe, matrix, k, matrix.rect.offset.z);
				}
				if (!flag)
				{
					break;
				}
			}
			for (int l = 0; l < matrix.arr.Length; l++)
			{
				if (matrix.arr[l] < -255f)
				{
					matrix.arr[l] = val;
				}
			}
		}

		private static bool PaintBucketMaskStripe(MatrixOps.Stripe stripe, float threshold = 0.0001f)
		{
			bool result = false;
			bool flag = false;
			for (int i = 0; i < stripe.length; i++)
			{
				if (stripe.arr[i] < -255f)
				{
					flag = true;
				}
				else if (stripe.arr[i] < threshold)
				{
					if (flag)
					{
						stripe.arr[i] = -256f;
						result = true;
					}
				}
				else
				{
					flag = false;
				}
			}
			flag = false;
			for (int num = stripe.length - 1; num >= 0; num--)
			{
				if (stripe.arr[num] < -255f)
				{
					flag = true;
				}
				else if (stripe.arr[num] < threshold)
				{
					if (flag)
					{
						stripe.arr[num] = -256f;
						result = true;
					}
				}
				else
				{
					flag = false;
				}
			}
			return result;
		}

		public static void CombineSilhouetteSpread(Matrix silhouetteMatrix, Matrix spreadMatrix, Matrix dstMatrix)
		{
			for (int i = 0; i < dstMatrix.count; i++)
			{
				float num = spreadMatrix.arr[i];
				float num2 = silhouetteMatrix.arr[i];
				dstMatrix.arr[i] = ((num2 > 0.5f) ? num : (1f - num));
			}
		}

		public static List<MetaLine> MatrixToMetaLines(Matrix matrix, float threshold)
		{
			Coord size = matrix.rect.size;
			List<MetaLine> list = new List<MetaLine>();
			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.z; j++)
				{
					int num = j * size.x + i;
					if (!(matrix.arr[num] < threshold))
					{
						int num2 = 0;
						if (j == size.z - 1 || matrix.arr[num + size.z] >= threshold)
						{
							num2 |= 8;
						}
						if (j == 0 || matrix.arr[num - size.z] >= threshold)
						{
							num2 |= 4;
						}
						if (i == 0 || matrix.arr[num - 1] >= threshold)
						{
							num2 |= 2;
						}
						if (i == size.x - 1 || matrix.arr[num + 1] >= threshold)
						{
							num2 |= 1;
						}
						switch (num2)
						{
						case 0:
							list.Add(new MetaLine(i, j, i, j + 1, i + 1, j + 1, i + 1, j)
							{
								closed = true
							});
							break;
						case 1:
							list.Add(new MetaLine(i + 1, j, i, j, i, j + 1, i + 1, j + 1));
							break;
						case 4:
							list.Add(new MetaLine(i, j, i, j + 1, i + 1, j + 1, i + 1, j));
							break;
						case 2:
							list.Add(new MetaLine(i, j + 1, i + 1, j + 1, i + 1, j, i, j));
							break;
						case 8:
							list.Add(new MetaLine(i + 1, j + 1, i + 1, j, i, j, i, j + 1));
							break;
						case 3:
							list.Add(new MetaLine(i, j + 1, i + 1, j + 1));
							list.Add(new MetaLine(i + 1, j, i, j));
							break;
						case 12:
							list.Add(new MetaLine(i, j, i, j + 1));
							list.Add(new MetaLine(i + 1, j + 1, i + 1, j));
							break;
						case 5:
							list.Add(new MetaLine(i, j, i, j + 1, i + 1, j + 1));
							break;
						case 6:
							list.Add(new MetaLine(i, j + 1, i + 1, j + 1, i + 1, j));
							break;
						case 10:
							list.Add(new MetaLine(i + 1, j + 1, i + 1, j, i, j));
							break;
						case 9:
							list.Add(new MetaLine(i + 1, j, i, j, i, j + 1));
							break;
						case 7:
							list.Add(new MetaLine(i, j + 1, i + 1, j + 1));
							break;
						case 14:
							list.Add(new MetaLine(i + 1, j + 1, i + 1, j));
							break;
						case 11:
							list.Add(new MetaLine(i + 1, j, i, j));
							break;
						case 13:
							list.Add(new MetaLine(i, j, i, j + 1));
							break;
						default:
							throw new Exception("Impossible pixels combination found");
						case 15:
							break;
						}
					}
				}
			}
			return list;
		}

		public static List<List<Coord>> WeldMetaLines(List<MetaLine> metaLines)
		{
			Dictionary<Coord, int> dictionary = new Dictionary<Coord, int>(metaLines.Count);
			int count = metaLines.Count;
			for (int i = 0; i < count; i++)
			{
				dictionary.Add(metaLines[i].c0, i);
			}
			Stack<Coord> stack = new Stack<Coord>();
			HashSet<Coord> hashSet = new HashSet<Coord>();
			for (int j = 0; j < count; j++)
			{
				hashSet.Add(metaLines[j].Last);
			}
			for (int k = 0; k < count; k++)
			{
				if (!hashSet.Contains(metaLines[k].c0))
				{
					stack.Push(metaLines[k].c0);
				}
			}
			List<List<Coord>> list = new List<List<Coord>>();
			List<Coord> list2 = new List<Coord>();
			while (dictionary.Count != 0)
			{
				Coord key = ((stack.Count == 0) ? dictionary.AnyKey() : stack.Pop());
				for (int l = 0; l < count + 2; l++)
				{
					if (dictionary.TryGetValue(key, out var value))
					{
						MetaLine.AddToList(list2, metaLines[value]);
						dictionary.Remove(key);
						key = metaLines[value].Last;
						continue;
					}
					list.Add(list2);
					list2 = new List<Coord>();
					break;
				}
			}
			return list;
		}
	}
}
