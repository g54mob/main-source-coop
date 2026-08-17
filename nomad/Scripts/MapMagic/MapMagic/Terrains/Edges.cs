using System;
using Den.Tools;
using Den.Tools.Matrices;
using UnityEngine;

namespace MapMagic.Terrains
{
	[Serializable]
	public class Edges
	{
		public float[] arr_X;

		public float[] arr_Z;

		public float[] arr_x;

		public float[] arr_z;

		public bool IsEmpty
		{
			get
			{
				if (arr_x.Length == 0 && arr_X.Length == 0 && arr_z.Length == 0)
				{
					return arr_Z.Length == 0;
				}
				return false;
			}
		}

		public int SizeX => arr_x.Length;

		public int SizeZ => arr_z.Length;

		public Edges(int sizeX, int sizeZ)
		{
			arr_x = new float[sizeX];
			arr_X = new float[sizeX];
			arr_z = new float[sizeZ];
			arr_Z = new float[sizeZ];
		}

		public float[] GetArr(Coord dir)
		{
			if (dir.x == 0 && dir.z == -1)
			{
				return arr_x;
			}
			if (dir.x == 0 && dir.z == 1)
			{
				return arr_X;
			}
			if (dir.x == -1 && dir.z == 0)
			{
				return arr_z;
			}
			if (dir.x == 1 && dir.z == 0)
			{
				return arr_Z;
			}
			return null;
		}

		public void ReadFloats2D(float[,] heights2D)
		{
			int length = heights2D.GetLength(1);
			int length2 = heights2D.GetLength(0);
			if (arr_x.Length != length)
			{
				arr_x = new float[length];
			}
			if (arr_X.Length != length)
			{
				arr_X = new float[length];
			}
			if (arr_z.Length != length2)
			{
				arr_z = new float[length2];
			}
			if (arr_Z.Length != length2)
			{
				arr_Z = new float[length2];
			}
			for (int i = 0; i < length; i++)
			{
				arr_x[i] = heights2D[0, i];
				arr_X[i] = heights2D[length2 - 1, i];
			}
			for (int j = 0; j < length2; j++)
			{
				arr_z[j] = heights2D[j, 0];
				arr_Z[j] = heights2D[j, length - 1];
			}
		}

		public void WriteFloats2D(float[,] heights2D)
		{
			int length = heights2D.GetLength(1);
			int length2 = heights2D.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				heights2D[0, i] = arr_x[i];
				heights2D[length2 - 1, i] = arr_X[i];
			}
			for (int j = 0; j < length2; j++)
			{
				heights2D[j, 0] = arr_z[j];
				heights2D[j, length - 1] = arr_Z[j];
			}
		}

		public void ReadSplitFloats2D(float[][,] heights2DSplits)
		{
			int num = 0;
			for (int i = 0; i < heights2DSplits.Length; i++)
			{
				num += heights2DSplits[i].GetLength(0);
			}
			int length = heights2DSplits[0].GetLength(1);
			if (arr_x.Length != num)
			{
				arr_x = new float[num];
			}
			if (arr_X.Length != num)
			{
				arr_X = new float[num];
			}
			if (arr_z.Length != length)
			{
				arr_z = new float[length];
			}
			if (arr_Z.Length != length)
			{
				arr_Z = new float[length];
			}
			float[,] array = heights2DSplits[heights2DSplits.Length - 1];
			int length2 = array.GetLength(0);
			for (int j = 0; j < num; j++)
			{
				arr_x[j] = heights2DSplits[0][0, j];
				arr_X[j] = array[length2 - 1, j];
			}
			int num2 = 0;
			for (int k = 0; k < heights2DSplits.Length; k++)
			{
				int length3 = heights2DSplits[k].GetLength(0);
				for (int l = 0; l < length3; l++)
				{
					arr_z[num2] = heights2DSplits[k][l, 0];
					arr_Z[num2] = heights2DSplits[k][l, length - 1];
					num2++;
				}
			}
		}

		public void WriteSplitFloats2D(float[][,] heights2D)
		{
			int num = arr_x.Length;
			int num2 = arr_z.Length;
			float[,] array = heights2D[heights2D.Length - 1];
			int length = array.GetLength(0);
			for (int i = 0; i < num; i++)
			{
				heights2D[0][0, i] = arr_x[i];
				array[length - 1, i] = arr_X[i];
			}
			int num3 = 0;
			for (int j = 0; j < heights2D.Length; j++)
			{
				int length2 = heights2D[j].GetLength(0);
				for (int k = 0; k < length2; k++)
				{
					heights2D[j][k, 0] = arr_z[num3];
					heights2D[j][k, num2 - 1] = arr_Z[num3];
					num3++;
				}
			}
		}

		public void ReadRawFloat(byte[] bytes)
		{
			int num = (int)Mathf.Sqrt(bytes.Length / 4);
			if (arr_x.Length != num)
			{
				arr_x = new float[num];
			}
			if (arr_X.Length != num)
			{
				arr_X = new float[num];
			}
			if (arr_z.Length != num)
			{
				arr_z = new float[num];
			}
			if (arr_Z.Length != num)
			{
				arr_Z = new float[num];
			}
			Matrix.FloatToBytes floatToBytes = new Matrix.FloatToBytes();
			for (int i = 0; i < num; i++)
			{
				int num2 = i * 4;
				floatToBytes.b0 = bytes[num2];
				floatToBytes.b1 = bytes[num2 + 1];
				floatToBytes.b2 = bytes[num2 + 2];
				floatToBytes.b3 = bytes[num2 + 3];
				arr_x[i] = floatToBytes.f * 2f;
				num2 = (i + num * (num - 1)) * 4;
				floatToBytes.b0 = bytes[num2];
				floatToBytes.b1 = bytes[num2 + 1];
				floatToBytes.b2 = bytes[num2 + 2];
				floatToBytes.b3 = bytes[num2 + 3];
				arr_X[i] = floatToBytes.f * 2f;
			}
			for (int j = 0; j < num; j++)
			{
				int num3 = j * num * 4;
				floatToBytes.b0 = bytes[num3];
				floatToBytes.b1 = bytes[num3 + 1];
				floatToBytes.b2 = bytes[num3 + 2];
				floatToBytes.b3 = bytes[num3 + 3];
				arr_z[j] = floatToBytes.f * 2f;
				num3 = (j * num + num - 1) * 4;
				floatToBytes.b0 = bytes[num3];
				floatToBytes.b1 = bytes[num3 + 1];
				floatToBytes.b2 = bytes[num3 + 2];
				floatToBytes.b3 = bytes[num3 + 3];
				arr_Z[j] = floatToBytes.f * 2f;
			}
		}

		public void WriteRawFloat(byte[] bytes)
		{
			int num = (int)Mathf.Sqrt(bytes.Length / 4);
			Matrix.FloatToBytes floatToBytes = new Matrix.FloatToBytes();
			for (int i = 0; i < num; i++)
			{
				floatToBytes.f = arr_x[i] / 2f;
				int num2 = i * 4;
				bytes[num2] = floatToBytes.b0;
				bytes[num2 + 1] = floatToBytes.b1;
				bytes[num2 + 2] = floatToBytes.b2;
				bytes[num2 + 3] = floatToBytes.b3;
				floatToBytes.f = arr_X[i] / 2f;
				num2 = (i + num * (num - 1)) * 4;
				bytes[num2] = floatToBytes.b0;
				bytes[num2 + 1] = floatToBytes.b1;
				bytes[num2 + 2] = floatToBytes.b2;
				bytes[num2 + 3] = floatToBytes.b3;
			}
			for (int j = 0; j < num; j++)
			{
				floatToBytes.f = arr_z[j] / 2f;
				int num3 = j * num * 4;
				bytes[num3] = floatToBytes.b0;
				bytes[num3 + 1] = floatToBytes.b1;
				bytes[num3 + 2] = floatToBytes.b2;
				bytes[num3 + 3] = floatToBytes.b3;
				floatToBytes.f = arr_Z[j] / 2f;
				num3 = (j * num + num - 1) * 4;
				bytes[num3] = floatToBytes.b0;
				bytes[num3 + 1] = floatToBytes.b1;
				bytes[num3 + 2] = floatToBytes.b2;
				bytes[num3 + 3] = floatToBytes.b3;
			}
		}

		public void ReadDelta2D(float[,] heights2D)
		{
			int length = heights2D.GetLength(1);
			int length2 = heights2D.GetLength(0);
			if (arr_x.Length != length)
			{
				arr_x = new float[length];
			}
			if (arr_X.Length != length)
			{
				arr_X = new float[length];
			}
			if (arr_z.Length != length2)
			{
				arr_z = new float[length2];
			}
			if (arr_Z.Length != length2)
			{
				arr_Z = new float[length2];
			}
			for (int i = 0; i < length; i++)
			{
				arr_x[i] = heights2D[0, i] - heights2D[1, i];
				arr_X[i] = heights2D[length2 - 1, i] - heights2D[length2 - 2, i];
			}
			for (int j = 0; j < length2; j++)
			{
				arr_z[j] = heights2D[j, 0] - heights2D[j, 1];
				arr_Z[j] = heights2D[j, length - 1] - heights2D[j, length - 2];
			}
		}

		public void ReadFloats3D(float[,,] splats2D, int ch)
		{
			int length = splats2D.GetLength(1);
			int length2 = splats2D.GetLength(0);
			if (arr_x.Length != length)
			{
				arr_x = new float[length];
			}
			if (arr_X.Length != length)
			{
				arr_X = new float[length];
			}
			if (arr_z.Length != length2)
			{
				arr_z = new float[length2];
			}
			if (arr_Z.Length != length2)
			{
				arr_Z = new float[length2];
			}
			for (int i = 0; i < length; i++)
			{
				arr_x[i] = splats2D[0, i, ch];
				arr_X[i] = splats2D[length2 - 1, i, ch];
			}
			for (int j = 0; j < length2; j++)
			{
				arr_z[j] = splats2D[j, 0, ch];
				arr_Z[j] = splats2D[j, length - 1, ch];
			}
		}

		public void ReadSplats(float[,,] splats, int ch)
		{
			int length = splats.GetLength(1);
			int length2 = splats.GetLength(0);
			if (arr_x.Length != length)
			{
				arr_x = new float[length];
			}
			if (arr_X.Length != length)
			{
				arr_X = new float[length];
			}
			if (arr_z.Length != length2)
			{
				arr_z = new float[length2];
			}
			if (arr_Z.Length != length2)
			{
				arr_Z = new float[length2];
			}
			for (int i = 0; i < length; i++)
			{
				arr_x[i] = splats[0, i, ch];
				arr_X[i] = splats[length2 - 1, i, ch];
			}
			for (int j = 0; j < length2; j++)
			{
				arr_z[j] = splats[j, 0, ch];
				arr_Z[j] = splats[j, length - 1, ch];
			}
		}

		public void WriteSplats(float[,,] splats, int ch)
		{
			int length = splats.GetLength(1);
			int length2 = splats.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				splats[0, i, ch] = arr_x[i];
				splats[length2 - 1, i, ch] = arr_X[i];
			}
			for (int j = 0; j < length2; j++)
			{
				splats[j, 0, ch] = arr_z[j];
				splats[j, length - 1, ch] = arr_Z[j];
			}
		}

		public void ReadColors(Color[] colors, int ch)
		{
			int num = (int)Mathf.Sqrt(colors.Length);
			if (arr_x.Length != num)
			{
				arr_x = new float[num];
			}
			if (arr_X.Length != num)
			{
				arr_X = new float[num];
			}
			if (arr_z.Length != num)
			{
				arr_z = new float[num];
			}
			if (arr_Z.Length != num)
			{
				arr_Z = new float[num];
			}
			for (int i = 0; i < num; i++)
			{
				arr_x[i] = colors[i][ch];
				arr_X[i] = colors[i + num * (num - 1)][ch];
			}
			for (int j = 0; j < num; j++)
			{
				arr_z[j] = colors[j * num][ch];
				arr_Z[j] = colors[j * num + num - 1][ch];
			}
		}

		public void WriteColors(Color[] colors, int ch)
		{
			int num = (int)Mathf.Sqrt(colors.Length);
			for (int i = 0; i < num; i++)
			{
				colors[i][ch] = arr_x[i];
				colors[i + num * (num - 1)][ch] = arr_X[i];
			}
			for (int j = 0; j < num; j++)
			{
				colors[j * num][ch] = arr_z[j];
				colors[j * num + num - 1][ch] = arr_Z[j];
			}
		}
	}
}
