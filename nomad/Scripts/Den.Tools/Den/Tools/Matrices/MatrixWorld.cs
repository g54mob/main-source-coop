using System;
using UnityEngine;

namespace Den.Tools.Matrices
{
	[Serializable]
	public class MatrixWorld : Matrix, ICloneable
	{
		public Vector3 worldPos;

		public Vector3 worldSize;

		public Vector3 WorldMax => worldPos + worldSize;

		public Vector2D PixelSize => new Vector2D(worldSize.x / (float)(rect.size.x - 1), worldSize.z / (float)(rect.size.z - 1));

		public MatrixWorld()
		{
			arr = new float[0];
			rect = new CoordRect(0, 0, 0, 0);
		}

		public MatrixWorld(CoordRect rect, Vector2D worldPos, Vector2D worldSize, float worldHeight, float[] array = null)
		{
			base.rect = rect;
			count = rect.Count;
			DefineArray(array);
			this.worldPos = (Vector3)worldPos;
			this.worldSize = (Vector3)worldSize;
			this.worldSize.y = worldHeight;
		}

		public MatrixWorld(CoordRect rect, Vector3 worldPos, Vector3 worldSize)
		{
			base.rect = rect;
			count = rect.Count;
			arr = new float[rect.size.x * rect.size.z];
			this.worldPos = worldPos;
			this.worldSize = worldSize;
		}

		public MatrixWorld(Coord offset, Coord size, Vector3 worldPos, Vector3 worldSize)
		{
			rect = new CoordRect(offset, size);
			count = rect.Count;
			arr = new float[rect.size.x * rect.size.z];
			this.worldPos = worldPos;
			this.worldSize = worldSize;
		}

		public MatrixWorld(Matrix matrix, Vector3 worldPos, Vector3 worldSize)
		{
			rect = matrix.rect;
			count = rect.Count;
			arr = matrix.arr;
			this.worldPos = worldPos;
			this.worldSize = worldSize;
		}

		public MatrixWorld(Matrix matrix, Vector2D worldPos, Vector2D worldSize, float worldHeight)
		{
			rect = matrix.rect;
			count = rect.Count;
			arr = matrix.arr;
			this.worldPos = (Vector3)worldPos;
			this.worldSize = new Vector3(worldSize.x, worldHeight, worldSize.x);
		}

		public MatrixWorld(MatrixWorld mw)
		{
			rect = mw.rect;
			count = rect.Count;
			arr = new float[mw.arr.Length];
			Array.Copy(mw.arr, arr, mw.arr.Length);
			worldPos = mw.worldPos;
			worldSize = mw.worldSize;
		}

		public object Clone()
		{
			return new MatrixWorld(this);
		}

		public Coord WorldToPixel(float x, float z)
		{
			float num = (x - worldPos.x) / worldSize.x;
			float num2 = (z - worldPos.z) / worldSize.z;
			float num3 = num * (float)(rect.size.x - 1) + (float)rect.offset.x;
			float num4 = num2 * (float)(rect.size.z - 1) + (float)rect.offset.z;
			num3 += 0.5f;
			float num5 = num4 + 0.5f;
			int num6 = (int)num3;
			if (num3 < 0f)
			{
				num6--;
			}
			if (num6 == rect.offset.x + rect.size.x)
			{
				num6--;
			}
			int num7 = (int)num5;
			if (num5 < 0f)
			{
				num7--;
			}
			if (num7 == rect.offset.z + rect.size.z)
			{
				num7--;
			}
			return new Coord(num6, num7);
		}

		public Vector3 WorldToPixelInterpolated(float x, float z)
		{
			float num = (x - worldPos.x) / worldSize.x;
			float num2 = (z - worldPos.z) / worldSize.z;
			float x2 = num * (float)(rect.size.x - 1) + (float)rect.offset.x;
			float z2 = num2 * (float)(rect.size.z - 1) + (float)rect.offset.z;
			return new Vector3(x2, 0f, z2);
		}

		public int WorldDistToPixel(float worldX)
		{
			float num = worldX / worldSize.x * (float)rect.size.x;
			int num2 = (int)num;
			if (num < 0f)
			{
				num2--;
			}
			if (num2 == rect.offset.x + rect.size.x)
			{
				num2--;
			}
			return num2;
		}

		public float WorldDistToPixelInterpolated(float worldX)
		{
			return worldX / worldSize.x * (float)rect.size.x;
		}

		public Vector3 PixelToWorld(float x, float z)
		{
			float num = 1f * (x - (float)rect.offset.x) / (float)(rect.size.x - 1);
			float num2 = 1f * (z - (float)rect.offset.z) / (float)(rect.size.z - 1);
			float x2 = num * worldSize.x + worldPos.x;
			float z2 = num2 * worldSize.z + worldPos.z;
			return new Vector3(x2, 0f, z2);
		}

		public float PixelDistToWorld(int mapX)
		{
			return ((float)mapX + 0.5f - (float)rect.offset.x) / (float)rect.size.x * worldSize.x;
		}

		public CoordRect WorldRectToPixels(Vector2D wOffset, Vector2D wSize)
		{
			Vector2D vector2D = wOffset + wSize;
			Coord coord = WorldToPixel(wOffset.x, wOffset.z);
			Coord coord2 = WorldToPixel(vector2D.x, vector2D.z);
			return new CoordRect(coord, coord2 - coord);
		}

		public bool ContainsWorldValue(float x, float z)
		{
			if (x > worldPos.x && x < worldPos.x + worldSize.x && z > worldPos.z)
			{
				return z < worldPos.z + worldSize.z;
			}
			return false;
		}

		public virtual float GetWorldValue(float x, float z)
		{
			Coord c = WorldToPixel(x, z);
			return base[c];
		}

		public void SetWorldValue(float x, float z, float val)
		{
			float num = (x - worldPos.x) / worldSize.x;
			float num2 = (z - worldPos.z) / worldSize.z;
			float num3 = num * (float)rect.size.x + (float)rect.offset.x;
			float num4 = num2 * (float)rect.size.z + (float)rect.offset.z;
			int num5 = (int)num3;
			if (num3 < 0f)
			{
				num5--;
			}
			if (num5 == rect.offset.x + rect.size.x)
			{
				num5--;
			}
			int num6 = (int)num4;
			if (num4 < 0f)
			{
				num6--;
			}
			if (num6 == rect.offset.z + rect.size.z)
			{
				num6--;
			}
			arr[(num6 - rect.offset.z) * rect.size.x + num5 - rect.offset.x] = val;
		}

		public virtual float GetWorldInterpolatedValue(float x, float z, bool roundToShort = false)
		{
			float num = (x - worldPos.x) / worldSize.x;
			float num2 = (z - worldPos.z) / worldSize.z;
			if (num > 1f)
			{
				num = 1f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			float fx = num * (float)(rect.size.x - 1) + (float)rect.offset.x;
			float fz = num2 * (float)(rect.size.z - 1) + (float)rect.offset.z;
			return GetInterpolated(fx, fz, roundToShort);
		}
	}
}
