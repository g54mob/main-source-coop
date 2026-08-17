using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Matrix2D<T>
	{
		public CoordRect rect;

		public int count;

		public int pos;

		public T[] arr;

		public const bool native = true;

		public T this[int x, int z]
		{
			get
			{
				return arr[(z - rect.offset.z) * rect.size.x + x - rect.offset.x];
			}
			set
			{
				arr[(z - rect.offset.z) * rect.size.x + x - rect.offset.x] = value;
			}
		}

		public T this[float x, float z]
		{
			get
			{
				int num = (int)x;
				if (x < 1f)
				{
					num--;
				}
				int num2 = (int)z;
				if (z < 1f)
				{
					num2--;
				}
				return arr[(num2 - rect.offset.z) * rect.size.x + num - rect.offset.x];
			}
			set
			{
				int num = (int)x;
				if (x < 1f)
				{
					num--;
				}
				int num2 = (int)z;
				if (z < 1f)
				{
					num2--;
				}
				arr[(num2 - rect.offset.z) * rect.size.x + num - rect.offset.x] = value;
			}
		}

		public T this[Coord c]
		{
			get
			{
				return arr[(c.z - rect.offset.z) * rect.size.x + c.x - rect.offset.x];
			}
			set
			{
				arr[(c.z - rect.offset.z) * rect.size.x + c.x - rect.offset.x] = value;
			}
		}

		public T this[Vector2 pos]
		{
			get
			{
				int num = (int)(pos.x + 0.5f);
				if (pos.x < 0f)
				{
					num--;
				}
				int num2 = (int)(pos.y + 0.5f);
				if (pos.y < 0f)
				{
					num2--;
				}
				return arr[(num2 - rect.offset.z) * rect.size.x + num - rect.offset.x];
			}
			set
			{
				int num = (int)(pos.x + 0.5f);
				if (pos.x < 0f)
				{
					num--;
				}
				int num2 = (int)(pos.y + 0.5f);
				if (pos.y < 0f)
				{
					num2--;
				}
				arr[(num2 - rect.offset.z) * rect.size.x + num - rect.offset.x] = value;
			}
		}

		public Matrix2D()
		{
		}

		public Matrix2D(int x, int z, T[] array = null)
		{
			rect = new CoordRect(0, 0, x, z);
			count = x * z;
			if (array != null && array.Length < count)
			{
				Debug.LogError("Array length: " + array.Length + " is lower then matrix capacity: " + count);
			}
			if (array != null && array.Length >= count)
			{
				arr = array;
			}
			else
			{
				arr = new T[count];
			}
		}

		public Matrix2D(CoordRect rect, T[] array = null)
		{
			this.rect = rect;
			count = rect.size.x * rect.size.z;
			if (array != null && array.Length < count)
			{
				Debug.Log("Array length: " + array.Length + " is lower then matrix capacity: " + count);
			}
			if (array != null && array.Length >= count)
			{
				arr = array;
			}
			else
			{
				arr = new T[count];
			}
		}

		public Matrix2D(Coord offset, Coord size, T[] array = null)
		{
			rect = new CoordRect(offset, size);
			count = rect.size.x * rect.size.z;
			if (array != null && array.Length < count)
			{
				Debug.Log("Array length: " + array.Length + " is lower then matrix capacity: " + count);
			}
			if (array != null && array.Length >= count)
			{
				arr = array;
			}
			else
			{
				arr = new T[count];
			}
		}

		public Matrix2D(Matrix2D<T> src)
		{
			rect = src.rect;
			count = src.count;
			arr = new T[count];
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = src.arr[i];
			}
		}

		public T CheckGet(int x, int z)
		{
			if (x >= rect.offset.x && x < rect.offset.x + rect.size.x && z >= rect.offset.z && z < rect.offset.z + rect.size.z)
			{
				return arr[(z - rect.offset.z) * rect.size.x + x - rect.offset.x];
			}
			return default(T);
		}

		public int Pos(Coord coord)
		{
			return (coord.z - rect.offset.z) * rect.size.x + coord.x - rect.offset.x;
		}

		public int Pos(int posX, int posZ)
		{
			return (posZ - rect.offset.z) * rect.size.x + posX - rect.offset.x;
		}

		public Coord CoordByNum(int num)
		{
			int num2 = num / rect.size.x;
			return new Coord(num - num2 * rect.size.x + rect.offset.x, num2 + rect.offset.z);
		}

		public T GetRaw(int x, int z)
		{
			return arr[z * rect.size.x + x];
		}

		public void SetRaw(int x, int z, T value)
		{
			arr[z * rect.size.x + x] = value;
		}

		public void Clear()
		{
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = default(T);
			}
		}

		public void ChangeRect(CoordRect newRect, bool forceNewArray = false)
		{
			rect = newRect;
			count = newRect.size.x * newRect.size.z;
			if (arr.Length != count || forceNewArray)
			{
				arr = new T[count];
			}
		}

		public virtual object Clone()
		{
			return Clone(null);
		}

		public Matrix2D<T> Clone(Matrix2D<T> result)
		{
			if (result == null)
			{
				result = new Matrix2D<T>(rect);
			}
			result.rect = rect;
			result.pos = pos;
			result.count = count;
			if (result.arr.Length != arr.Length)
			{
				result.arr = new T[arr.Length];
			}
			for (int i = 0; i < arr.Length; i++)
			{
				result.arr[i] = arr[i];
			}
			return result;
		}

		public void Fill(T v)
		{
			for (int i = 0; i < count; i++)
			{
				arr[i] = v;
			}
		}

		public void Fill(Matrix2D<T> m, bool removeBorders = false)
		{
			CoordRect centerRect = CoordRect.Intersected(rect, m.rect);
			Coord min = centerRect.Min;
			Coord max = centerRect.Max;
			for (int i = min.x; i < max.x; i++)
			{
				for (int j = min.z; j < max.z; j++)
				{
					this[i, j] = m[i, j];
				}
			}
			if (removeBorders)
			{
				RemoveBorders(centerRect);
			}
		}

		public void SetPos(int x, int z)
		{
			pos = (z - rect.offset.z) * rect.size.x + x - rect.offset.x;
		}

		public void SetPos(int x, int z, int s)
		{
			pos = (z - rect.offset.z) * rect.size.x + x - rect.offset.x + s * rect.size.x * rect.size.z;
		}

		public void MoveX()
		{
			pos++;
		}

		public void MoveZ()
		{
			pos += rect.size.x;
		}

		public void MovePrevX()
		{
			pos--;
		}

		public void MovePrevZ()
		{
			pos -= rect.size.x;
		}

		public void RemoveBorders()
		{
			Coord min = rect.Min;
			Coord coord = rect.Max - 1;
			for (int i = min.x; i <= coord.x; i++)
			{
				SetPos(i, min.z);
				arr[pos] = arr[pos + rect.size.x];
			}
			for (int j = min.x; j <= coord.x; j++)
			{
				SetPos(j, coord.z);
				arr[pos] = arr[pos - rect.size.x];
			}
			for (int k = min.z; k <= coord.z; k++)
			{
				SetPos(min.x, k);
				arr[pos] = arr[pos + 1];
			}
			for (int l = min.z; l <= coord.z; l++)
			{
				SetPos(coord.x, l);
				arr[pos] = arr[pos - 1];
			}
		}

		public void RemoveBorders(int borderMinX, int borderMinZ, int borderMaxX, int borderMaxZ)
		{
			Coord min = rect.Min;
			Coord max = rect.Max;
			if (borderMinZ != 0)
			{
				for (int i = min.x; i < max.x; i++)
				{
					T value = this[i, min.z + borderMinZ];
					for (int j = min.z; j < min.z + borderMinZ; j++)
					{
						this[i, j] = value;
					}
				}
			}
			if (borderMaxZ != 0)
			{
				for (int k = min.x; k < max.x; k++)
				{
					T value2 = this[k, max.z - borderMaxZ];
					for (int l = max.z - borderMaxZ; l < max.z; l++)
					{
						this[k, l] = value2;
					}
				}
			}
			if (borderMinX != 0)
			{
				for (int m = min.z; m < max.z; m++)
				{
					T value3 = this[min.x + borderMinX, m];
					for (int n = min.x; n < min.x + borderMinX; n++)
					{
						this[n, m] = value3;
					}
				}
			}
			if (borderMaxX == 0)
			{
				return;
			}
			for (int num = min.z; num < max.z; num++)
			{
				T value4 = this[max.x - borderMaxX, num];
				for (int num2 = max.x - borderMaxX; num2 < max.x; num2++)
				{
					this[num2, num] = value4;
				}
			}
		}

		public void RemoveBorders(CoordRect centerRect)
		{
			RemoveBorders(Mathf.Max(0, centerRect.offset.x - rect.offset.x), Mathf.Max(0, centerRect.offset.z - rect.offset.z), Mathf.Max(0, rect.Max.x - centerRect.Max.x + 1), Mathf.Max(0, rect.Max.z - centerRect.Max.z + 1));
		}
	}
}
