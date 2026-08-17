using System;

namespace Den.Tools
{
	[Serializable]
	public class FloatEdges : Edges<float>
	{
		public FloatEdges()
		{
		}

		public FloatEdges(CoordRect rect)
		{
			base.rect = rect;
			array = new float[(rect.size.x + rect.size.z) * 2];
		}

		public void ReadFloats2D(float[,] heights2D)
		{
			int length = heights2D.GetLength(1);
			int length2 = heights2D.GetLength(0);
			if (rect.size.x > length || rect.size.z > length2)
			{
				string[] obj = new string[6]
				{
					"Float[",
					length.ToString(),
					",",
					length2.ToString(),
					"] is smaller than the edges rect ",
					null
				};
				CoordRect coordRect = rect;
				obj[5] = coordRect.ToString();
				throw new Exception(string.Concat(obj));
			}
			for (int i = 0; i < rect.size.x; i++)
			{
				array[i] = heights2D[0, i];
				array[rect.size.x + i] = heights2D[length2 - 1, i];
			}
			for (int j = 0; j < rect.size.z; j++)
			{
				array[rect.size.x * 2 + j] = heights2D[j, 0];
				array[rect.size.x * 2 + rect.size.z + j] = heights2D[j, length - 1];
			}
		}

		public void ReadDelta2D(float[,] heights2D)
		{
			int length = heights2D.GetLength(1);
			int length2 = heights2D.GetLength(0);
			if (rect.size.x > length || rect.size.z > length2)
			{
				string[] obj = new string[6]
				{
					"Float[",
					length.ToString(),
					",",
					length2.ToString(),
					"] is smaller than the edges rect ",
					null
				};
				CoordRect coordRect = rect;
				obj[5] = coordRect.ToString();
				throw new Exception(string.Concat(obj));
			}
			for (int i = 0; i < rect.size.x; i++)
			{
				array[i] = heights2D[0, i] - heights2D[1, i];
				array[rect.size.x + i] = heights2D[length2 - 1, i] - heights2D[length2 - 2, i];
			}
			for (int j = 0; j < rect.size.z; j++)
			{
				array[rect.size.x * 2 + j] = heights2D[j, 0] - heights2D[j, 1];
				array[rect.size.x * 2 + rect.size.z + j] = heights2D[j, length - 1] - heights2D[j, length - 2];
			}
		}

		public void ReadFloats3D(float[,,] splats2D, int ch)
		{
			int length = splats2D.GetLength(1);
			int length2 = splats2D.GetLength(0);
			if (rect.size.x > length || rect.size.z > length2)
			{
				string[] obj = new string[6]
				{
					"Float[",
					length.ToString(),
					",",
					length2.ToString(),
					"] is smaller than the edges rect ",
					null
				};
				CoordRect coordRect = rect;
				obj[5] = coordRect.ToString();
				throw new Exception(string.Concat(obj));
			}
			for (int i = 0; i < rect.size.x; i++)
			{
				array[i] = splats2D[0, i, ch];
				array[rect.size.x + i] = splats2D[length2 - 1, i, ch];
			}
			for (int j = 0; j < rect.size.z; j++)
			{
				array[rect.size.x * 2 + j] = splats2D[j, 0, ch];
				array[rect.size.x * 2 + rect.size.z + j] = splats2D[j, length - 1, ch];
			}
		}

		public void ReadCornersFloats2D(float[,] heights2D)
		{
			int length = heights2D.GetLength(1);
			int length2 = heights2D.GetLength(0);
			if (rect.size.x > length || rect.size.z > length2)
			{
				string[] obj = new string[6]
				{
					"Float[",
					length.ToString(),
					",",
					length2.ToString(),
					"] is smaller than the edges rect ",
					null
				};
				CoordRect coordRect = rect;
				obj[5] = coordRect.ToString();
				throw new Exception(string.Concat(obj));
			}
			array[0] = heights2D[0, 0];
			array[rect.size.x] = heights2D[length2 - 1, 0];
			array[rect.size.x - 1] = heights2D[0, length - 1];
			array[rect.size.x * 2 - 1] = heights2D[length2 - 1, length - 1];
		}

		public static void Weld_z(FloatEdges edges, float[,] heights2D)
		{
			for (int i = 0; i < edges.rect.size.x; i++)
			{
				float num = edges.array[edges.rect.size.x + i];
				heights2D[0, i] = num;
			}
		}

		public static void Weld_Z(FloatEdges edges, float[,] heights2D)
		{
			for (int i = 0; i < edges.rect.size.x; i++)
			{
				float num = edges.array[i];
				heights2D[edges.rect.size.z - 1, i] = num;
			}
		}

		public static void Weld_x(FloatEdges edges, float[,] heights2D)
		{
			for (int i = 0; i < edges.rect.size.z; i++)
			{
				float num = edges.array[edges.rect.size.x * 2 + edges.rect.size.z + i];
				heights2D[i, 0] = num;
			}
		}

		public static void Weld_X(FloatEdges edges, float[,] heights2D)
		{
			for (int i = 0; i < edges.rect.size.z; i++)
			{
				float num = edges.array[edges.rect.size.x * 2 + i];
				heights2D[i, edges.rect.size.x - 1] = num;
			}
		}

		public static void Weld(FloatEdges edges, Coord direction, float[,] heights2D)
		{
			if (direction.x == 1)
			{
				Weld_X(edges, heights2D);
			}
			else if (direction.x == -1)
			{
				Weld_x(edges, heights2D);
			}
			else if (direction.z == 1)
			{
				Weld_Z(edges, heights2D);
			}
			else if (direction.z == -1)
			{
				Weld_z(edges, heights2D);
			}
		}

		public static void Weld_z(FloatEdges edges, float[,,] splats3D, int ch)
		{
			for (int i = 0; i < edges.rect.size.x; i++)
			{
				float num = edges.array[edges.rect.size.x + i];
				splats3D[0, i, ch] = num;
			}
		}

		public static void Weld_Z(FloatEdges edges, float[,,] splats3D, int ch)
		{
			for (int i = 0; i < edges.rect.size.x; i++)
			{
				float num = edges.array[i];
				splats3D[edges.rect.size.z - 1, i, ch] = num;
			}
		}

		public static void Weld_x(FloatEdges edges, float[,,] splats3D, int ch)
		{
			for (int i = 0; i < edges.rect.size.z; i++)
			{
				float num = edges.array[edges.rect.size.x * 2 + edges.rect.size.z + i];
				splats3D[i, 0, ch] = num;
			}
		}

		public static void Weld_X(FloatEdges edges, float[,,] splats3D, int ch)
		{
			for (int i = 0; i < edges.rect.size.z; i++)
			{
				float num = edges.array[edges.rect.size.x * 2 + i];
				splats3D[i, edges.rect.size.x - 1, ch] = num;
			}
		}

		public static void Weld(FloatEdges edges, Coord direction, float[,,] splats3D, int ch)
		{
			if (direction.x == 1)
			{
				Weld_X(edges, splats3D, ch);
			}
			else if (direction.x == -1)
			{
				Weld_x(edges, splats3D, ch);
			}
			else if (direction.z == 1)
			{
				Weld_Z(edges, splats3D, ch);
			}
			else if (direction.z == -1)
			{
				Weld_z(edges, splats3D, ch);
			}
		}

		public static void Weld_z(FloatEdges edges, FloatEdges deltas, float[,] heights2D, int margins = 10)
		{
			for (int i = 0; i < edges.rect.size.x; i++)
			{
				float num = edges.array[edges.rect.size.x + i];
				float num2 = deltas.array[edges.rect.size.x + i];
				heights2D[0, i] = num;
				float num3 = num + num2 - heights2D[1, i];
				heights2D[1, i] = num + num2;
				for (int j = 0; j < margins; j++)
				{
					heights2D[2 + j, i] += num3 * (1f - 1f * (float)j / (float)margins);
				}
			}
		}

		public static void Weld_Z(FloatEdges edges, FloatEdges deltas, float[,] heights2D, int margins = 10)
		{
			for (int i = 0; i < edges.rect.size.x; i++)
			{
				float num = edges.array[i];
				float num2 = deltas.array[i];
				heights2D[edges.rect.size.z - 1, i] = num;
				float num3 = num + num2 - heights2D[edges.rect.size.z - 2, i];
				heights2D[edges.rect.size.z - 2, i] = num + num2;
				for (int j = 0; j < margins; j++)
				{
					heights2D[edges.rect.size.z - 3 - j, i] += num3 * (1f - 1f * (float)j / (float)margins);
				}
			}
		}

		public static void Weld_x(FloatEdges edges, FloatEdges deltas, float[,] heights2D, int margins = 10)
		{
			for (int i = 0; i < edges.rect.size.z; i++)
			{
				float num = edges.array[edges.rect.size.x * 2 + edges.rect.size.z + i];
				float num2 = deltas.array[edges.rect.size.x * 2 + edges.rect.size.z + i];
				heights2D[i, 0] = num;
				float num3 = num + num2 - heights2D[i, 1];
				heights2D[i, 1] = num + num2;
				for (int j = 0; j < margins; j++)
				{
					heights2D[i, 2 + j] += num3 * (1f - 1f * (float)j / (float)margins);
				}
			}
		}

		public static void Weld_X(FloatEdges edges, FloatEdges deltas, float[,] heights2D, int margins = 10)
		{
			for (int i = 0; i < edges.rect.size.z; i++)
			{
				float num = edges.array[edges.rect.size.x * 2 + i];
				float num2 = deltas.array[edges.rect.size.x * 2 + i];
				heights2D[i, edges.rect.size.x - 1] = num;
				float num3 = num + num2 - heights2D[i, edges.rect.size.x - 2];
				heights2D[i, edges.rect.size.x - 2] = num + num2;
				for (int j = 0; j < margins; j++)
				{
					heights2D[i, edges.rect.size.x - 3 - j] += num3 * (1f - 1f * (float)j / (float)margins);
				}
			}
		}

		public static void Weld(FloatEdges edges, FloatEdges deltas, Coord direction, float[,] heights2D, int margins = 10)
		{
			if (direction.x == 1)
			{
				Weld_X(edges, deltas, heights2D, margins);
			}
			else if (direction.x == -1)
			{
				Weld_x(edges, deltas, heights2D, margins);
			}
			else if (direction.z == 1)
			{
				Weld_Z(edges, deltas, heights2D, margins);
			}
			else if (direction.z == -1)
			{
				Weld_z(edges, deltas, heights2D, margins);
			}
		}
	}
}
