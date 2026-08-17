using System;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public class Matrix3D<T>
	{
		public CoordCube cube;

		public T[] array;

		public int count;

		public T this[int x, int y, int z]
		{
			get
			{
				return array[(z - cube.offset.z) * cube.size.x * cube.size.y + (y - cube.offset.y) * cube.size.x + x - cube.offset.x];
			}
			set
			{
				array[(z - cube.offset.z) * cube.size.x * cube.size.y + (y - cube.offset.y) * cube.size.x + x - cube.offset.x] = value;
			}
		}

		public T this[CoordDir c]
		{
			get
			{
				return array[(c.z - cube.offset.z) * cube.size.x * cube.size.y + (c.y - cube.offset.y) * cube.size.x + c.x - cube.offset.x];
			}
			set
			{
				array[(c.z - cube.offset.z) * cube.size.x * cube.size.y + (c.y - cube.offset.y) * cube.size.x + c.x - cube.offset.x] = value;
			}
		}

		public Matrix2D<T> Matrix2 => new Matrix2D<T>(new CoordRect(cube.offset.x, cube.offset.z, cube.size.x, cube.size.z), array);

		public Matrix3D()
		{
		}

		public Matrix3D(CoordCube cube, T[] array = null)
		{
			this.cube = cube;
			count = cube.size.x * cube.size.y * cube.size.z;
			if (array != null && array.Length < count)
			{
				Debug.LogError("Array length: " + array.Length + " is lower then matrix capacity: " + count);
			}
			if (array != null && array.Length >= count)
			{
				this.array = array;
			}
			else
			{
				this.array = new T[count];
			}
		}

		public Matrix3D(CoordDir offset, CoordDir size, T[] array = null)
		{
			cube = new CoordCube(offset, size);
			count = cube.size.x * cube.size.y * cube.size.z;
			if (array != null && array.Length < count)
			{
				Debug.LogError("Array length: " + array.Length + " is lower then matrix capacity: " + count);
			}
			if (array != null && array.Length >= count)
			{
				this.array = array;
			}
			else
			{
				this.array = new T[count];
			}
		}

		public Matrix3D(int x, int y, int z, T[] array = null)
		{
			cube = new CoordCube(0, 0, 0, x, y, z);
			count = cube.size.x * cube.size.y * cube.size.z;
			if (array != null && array.Length < count)
			{
				Debug.LogError("Array length: " + array.Length + " is lower then matrix capacity: " + count);
			}
			if (array != null && array.Length >= count)
			{
				this.array = array;
			}
			else
			{
				this.array = new T[count];
			}
		}

		public Matrix3D(int ox, int oy, int oz, int sx, int sy, int sz, T[] array = null)
		{
			cube = new CoordCube(ox, oy, oz, sx, sy, sz);
			count = cube.size.x * cube.size.y * cube.size.z;
			if (array != null && array.Length < count)
			{
				Debug.LogError("Array length: " + array.Length + " is lower then matrix capacity: " + count);
			}
			if (array != null && array.Length >= count)
			{
				this.array = array;
			}
			else
			{
				this.array = new T[count];
			}
		}

		public void Fill(T def)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = def;
			}
		}

		public Matrix3D<T> Copy()
		{
			Matrix3D<T> matrix3D = new Matrix3D<T>(cube);
			for (int i = 0; i < array.Length; i++)
			{
				matrix3D.array[i] = array[i];
			}
			return matrix3D;
		}

		public static void CopyData(Matrix3D<T> src, Matrix3D<T> dst)
		{
			for (int i = dst.cube.offset.x; i < dst.cube.offset.x + dst.cube.size.x; i++)
			{
				for (int j = dst.cube.offset.y; j < dst.cube.offset.y + dst.cube.size.y; j++)
				{
					for (int k = dst.cube.offset.z; k < dst.cube.offset.z + dst.cube.size.z; k++)
					{
						if (src.cube.Contains(i, j, k))
						{
							dst[i, j, k] = src[i, j, k];
						}
					}
				}
			}
		}
	}
}
