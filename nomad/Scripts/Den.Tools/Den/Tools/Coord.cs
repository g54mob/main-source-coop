using System;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public struct Coord
	{
		public int x;

		public int z;

		public int this[int c]
		{
			get
			{
				if (c != 0)
				{
					return z;
				}
				return x;
			}
			set
			{
				if (x == 0)
				{
					x = value;
				}
				else
				{
					z = value;
				}
			}
		}

		public int Minimal
		{
			get
			{
				if (x >= z)
				{
					return z;
				}
				return x;
			}
		}

		public int Maximal
		{
			get
			{
				if (x <= z)
				{
					return z;
				}
				return x;
			}
		}

		public int SqrMagnitude => x * x + z * z;

		public float Magnitude => Mathf.Sqrt(x * x + z * z);

		public Vector4 vector4 => new Vector4(x, 0f, z, 0f);

		public Vector3 vector3 => new Vector3(x, 0f, z);

		public Vector2 vector2 => new Vector2(x, z);

		public Vector2D vector2d => new Vector2D(x, z);

		public static Coord zero => new Coord(0, 0);

		public static bool operator >(Coord c1, Coord c2)
		{
			if (c1.x > c2.x)
			{
				return c1.z > c2.z;
			}
			return false;
		}

		public static bool operator <(Coord c1, Coord c2)
		{
			if (c1.x < c2.x)
			{
				return c1.z < c2.z;
			}
			return false;
		}

		public static bool operator ==(Coord c1, Coord c2)
		{
			if (c1.x == c2.x)
			{
				return c1.z == c2.z;
			}
			return false;
		}

		public static bool operator !=(Coord c1, Coord c2)
		{
			if (c1.x == c2.x)
			{
				return c1.z != c2.z;
			}
			return true;
		}

		public static Coord operator +(Coord c, int s)
		{
			return new Coord(c.x + s, c.z + s);
		}

		public static Coord operator +(Coord c1, Coord c2)
		{
			return new Coord(c1.x + c2.x, c1.z + c2.z);
		}

		public static Coord operator -(Coord c)
		{
			return new Coord(-c.x, -c.z);
		}

		public static Coord operator -(Coord c, int s)
		{
			return new Coord(c.x - s, c.z - s);
		}

		public static Coord operator -(Coord c1, Coord c2)
		{
			return new Coord(c1.x - c2.x, c1.z - c2.z);
		}

		public static Coord operator *(Coord c, int s)
		{
			return new Coord(c.x * s, c.z * s);
		}

		public static Vector2 operator *(Coord c, Vector2 s)
		{
			return new Vector2((float)c.x * s.x, (float)c.z * s.y);
		}

		public static Vector3 operator *(Coord c, Vector3 s)
		{
			return new Vector3((float)c.x * s.x, s.y, (float)c.z * s.z);
		}

		public static Coord operator *(Coord c1, Coord c2)
		{
			return new Coord(c1.x * c2.x, c1.z * c2.z);
		}

		public static Coord operator *(Coord c, float s)
		{
			return new Coord((int)((float)c.x * s), (int)((float)c.z * s));
		}

		public static Coord operator /(Coord c, int s)
		{
			return new Coord(c.x / s, c.z / s);
		}

		public static Coord operator /(Coord c, float s)
		{
			return new Coord((int)((float)c.x / s), (int)((float)c.z / s));
		}

		public override bool Equals(object obj)
		{
			if (obj is Coord coord)
			{
				if (coord.x == x)
				{
					return coord.z == z;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return x * 10000000 + z;
		}

		public static explicit operator Coord(Vector2D v)
		{
			return new Coord((int)v.x, (int)v.z);
		}

		public static explicit operator Vector2D(Coord c)
		{
			return new Vector2D(c.x, c.z);
		}

		public static explicit operator Vector3(Coord c)
		{
			return new Vector3(c.x, 0f, c.z);
		}

		public static explicit operator Coord(int i)
		{
			return new Coord(i, i);
		}

		public Coord(int x, int z)
		{
			this.x = x;
			this.z = z;
		}

		public Coord(int x)
		{
			this.x = x;
			z = x;
		}

		public static Coord PickCell(int ix, int iz, int cellRes)
		{
			int num = ix / cellRes;
			if (ix < 0 && ix != num * cellRes)
			{
				num--;
			}
			int num2 = iz / cellRes;
			if (iz < 0 && iz != num2 * cellRes)
			{
				num2--;
			}
			return new Coord(num, num2);
		}

		public static Coord PickCell(Coord c, int cellRes)
		{
			return PickCell(c.x, c.z, cellRes);
		}

		public static Coord PickCellByPos(float fx, float fz, float cellSize = 1f)
		{
			int num = (int)(fx / cellSize);
			if (fx < 0f && fx != (float)num * cellSize)
			{
				num--;
			}
			int num2 = (int)(fz / cellSize);
			if (fz < 0f && fz != (float)num2 * cellSize)
			{
				num2--;
			}
			return new Coord(num, num2);
		}

		public static Coord PickCellByPos(float fx, float fz, Vector2D cellSize)
		{
			int num = (int)(fx / cellSize.x);
			if (fx < 0f)
			{
				num--;
			}
			int num2 = (int)(fz / cellSize.z);
			if (fz < 0f)
			{
				num2--;
			}
			return new Coord(num, num2);
		}

		public static Coord PickCellByPos(Vector3 v, float cellSize = 1f)
		{
			return PickCellByPos(v.x, v.z, cellSize);
		}

		public static Coord Floor(Vector3 v)
		{
			if (v.x < 0f)
			{
				v.x -= 1f;
			}
			if (v.z < 0f)
			{
				v.z -= 1f;
			}
			return new Coord((int)v.x, (int)v.z);
		}

		public static Coord Floor(Vector2D v)
		{
			if (v.x < 0f)
			{
				v.x -= 1f;
			}
			if (v.z < 0f)
			{
				v.z -= 1f;
			}
			return new Coord((int)v.x, (int)v.z);
		}

		public static Coord Floor(float x, float z)
		{
			if (x < 0f)
			{
				x -= 1f;
			}
			if (z < 0f)
			{
				z -= 1f;
			}
			return new Coord((int)x, (int)z);
		}

		public static Coord Ceil(Vector2D v)
		{
			if (v.x < 0f)
			{
				v.x -= 1f;
			}
			if (v.z < 0f)
			{
				v.z -= 1f;
			}
			return new Coord((int)(v.x + 1f), (int)(v.z + 1f));
		}

		public static Coord Ceil(float x, float z)
		{
			if (x < 0f)
			{
				x -= 1f;
			}
			if (z < 0f)
			{
				z -= 1f;
			}
			return new Coord((int)(x + 1f), (int)(z + 1f));
		}

		public static Coord Round(Vector3 v)
		{
			if (v.x < 0f)
			{
				v.x -= 1f;
			}
			if (v.z < 0f)
			{
				v.z -= 1f;
			}
			return new Coord((int)(v.x + 0.5f), (int)(v.z + 0.5f));
		}

		public static Coord Round(Vector2D v)
		{
			if (v.x < 0f)
			{
				v.x -= 1f;
			}
			if (v.z < 0f)
			{
				v.z -= 1f;
			}
			return new Coord((int)(v.x + 0.5f), (int)(v.z + 0.5f));
		}

		public static Coord Round(float x, float z)
		{
			if (x < 0f)
			{
				x -= 1f;
			}
			if (z < 0f)
			{
				z -= 1f;
			}
			return new Coord((int)(x + 0.5f), (int)(z + 0.5f));
		}

		public void Clamp(int sizeX, int sizeZ)
		{
			if (x > sizeX)
			{
				x = sizeX;
			}
			if (z > sizeZ)
			{
				z = sizeZ;
			}
		}

		public void ClampPositive()
		{
			x = Mathf.Max(0, x);
			z = Mathf.Max(0, z);
		}

		public void ClampByRect(CoordRect rect)
		{
			if (x < rect.offset.x)
			{
				x = rect.offset.x;
			}
			if (x >= rect.offset.x + rect.size.x)
			{
				x = rect.offset.x + rect.size.x - 1;
			}
			if (z < rect.offset.z)
			{
				z = rect.offset.z;
			}
			if (z >= rect.offset.z + rect.size.z)
			{
				z = rect.offset.z + rect.size.z - 1;
			}
		}

		public static Coord Min(Coord c1, Coord c2)
		{
			int num = ((c1.x < c2.x) ? c1.x : c2.x);
			int num2 = ((c1.z < c2.z) ? c1.z : c2.z);
			return new Coord(num, num2);
		}

		public static Coord Max(Coord c1, Coord c2)
		{
			int num = ((c1.x > c2.x) ? c1.x : c2.x);
			int num2 = ((c1.z > c2.z) ? c1.z : c2.z);
			return new Coord(num, num2);
		}

		public Coord BaseFloor(int cellSize)
		{
			return new Coord((x >= 0) ? (x / cellSize) : ((x + 1) / cellSize - 1), (z >= 0) ? (z / cellSize) : ((z + 1) / cellSize - 1));
		}

		public override string ToString()
		{
			return base.ToString() + " x:" + x + " z:" + z;
		}

		public static float Distance(Coord c1, Coord c2)
		{
			int num = c1.x - c2.x;
			int num2 = c1.z - c2.z;
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		public static float DistanceSq(Coord c1, Coord c2)
		{
			int num = c1.x - c2.x;
			if (num < 0)
			{
				num = -num;
			}
			int num2 = c1.z - c2.z;
			if (num2 < 0)
			{
				num2 = -num2;
			}
			return num * num + num2 * num2;
		}

		public static int DistanceAxisAligned(Coord c1, Coord c2)
		{
			int num = c1.x - c2.x;
			if (num < 0)
			{
				num = -num;
			}
			int num2 = c1.z - c2.z;
			if (num2 < 0)
			{
				num2 = -num2;
			}
			if (num <= num2)
			{
				return num2;
			}
			return num;
		}

		public static int DistanceManhattan(Coord c1, Coord c2)
		{
			int num = c1.x - c2.x;
			if (num < 0)
			{
				num = -num;
			}
			int num2 = c1.z - c2.z;
			if (num2 < 0)
			{
				num2 = -num2;
			}
			return num + num2;
		}

		public static int DistanceAxisAligned(Coord c, CoordRect rect)
		{
			int num = rect.offset.x - c.x;
			int num2 = c.x - rect.offset.x - rect.size.x;
			int num3 = ((num >= 0) ? num : ((num2 >= 0) ? num2 : 0));
			int num4 = rect.offset.z - c.z;
			int num5 = c.z - rect.offset.z - rect.size.z;
			int num6 = ((num4 >= 0) ? num4 : ((num5 >= 0) ? num5 : 0));
			if (num3 > num6)
			{
				return num3;
			}
			return num6;
		}

		public static float DistanceAxisPriority(Coord c1, Coord c2)
		{
			int num = c1.x - c2.x;
			if (num < 0)
			{
				num = -num;
			}
			int num2 = c1.z - c2.z;
			if (num2 < 0)
			{
				num2 = -num2;
			}
			int num3 = ((num > num2) ? num : num2);
			int num4 = ((num < num2) ? num : num2);
			return (float)num3 + 1f * (float)num4 / (float)(num3 + 1);
		}

		public IEnumerable<Coord> DistanceStep(int i, int dist)
		{
			yield return new Coord(x - i, z - dist);
			yield return new Coord(x - dist, z + i);
			yield return new Coord(x + i, z + dist);
			yield return new Coord(x + dist, z - i);
			yield return new Coord(x + i + 1, z - dist);
			yield return new Coord(x - dist, z - i - 1);
			yield return new Coord(x - i - 1, z + dist);
			yield return new Coord(x + dist, z + i + 1);
		}

		public IEnumerable<Coord> DistancePerimeter(int dist)
		{
			for (int i = 0; i < dist; i++)
			{
				foreach (Coord item in DistanceStep(i, dist))
				{
					yield return item;
				}
			}
		}

		public IEnumerable<Coord> DistanceArea(int maxDist)
		{
			yield return this;
			for (int i = 0; i < maxDist; i++)
			{
				foreach (Coord item in DistancePerimeter(i))
				{
					yield return item;
				}
			}
		}

		public IEnumerable<Coord> DistanceArea(CoordRect rect)
		{
			int maxDist = Mathf.Max(x - rect.offset.x, rect.Max.x - x, z - rect.offset.z, rect.Max.z - z) + 1;
			if (rect.Contains(this))
			{
				yield return this;
			}
			for (int i = 0; i < maxDist; i++)
			{
				foreach (Coord item in DistancePerimeter(i))
				{
					if (rect.Contains(item))
					{
						yield return item;
					}
				}
			}
		}

		public static IEnumerable<Coord> MultiDistanceArea(Coord[] coords, int maxDist)
		{
			if (coords.Length == 0)
			{
				yield break;
			}
			for (int c = 0; c < coords.Length; c++)
			{
				yield return coords[c];
			}
			for (int c = 0; c < maxDist; c++)
			{
				for (int i = 0; i < c; i++)
				{
					for (int j = 0; j < coords.Length; j++)
					{
						foreach (Coord item in coords[j].DistanceStep(i, c))
						{
							yield return item;
						}
					}
				}
			}
		}

		public Vector3 ToVector3(float cellSize)
		{
			return new Vector3((float)x * cellSize, 0f, (float)z * cellSize);
		}

		public Vector2 ToVector2(float cellSize)
		{
			return new Vector2((float)x * cellSize, (float)z * cellSize);
		}

		public Rect ToRect(float cellSize)
		{
			return new Rect((float)x * cellSize, (float)z * cellSize, cellSize, cellSize);
		}

		public CoordRect ToCoordRect(int cellSize)
		{
			return new CoordRect(x * cellSize, z * cellSize, cellSize, cellSize);
		}

		public float GetFalloff(Vector2D center, float radius, float hardness, int smooth = 1)
		{
			float num = ((float)x - center.x) * ((float)x - center.x) + ((float)z - center.z) * ((float)z - center.z);
			if (num > radius * radius)
			{
				return 0f;
			}
			if (num < radius * hardness * radius * hardness)
			{
				return 1f;
			}
			float num2 = Mathf.Sqrt(num);
			float num3 = radius * hardness;
			float num4 = 1f - (num2 - num3) / (radius - num3);
			for (int i = 0; i < smooth; i++)
			{
				num4 = 3f * num4 * num4 - 2f * num4 * num4 * num4;
			}
			return num4;
		}

		public float GetInterpolatedPercent(Vector2D pos)
		{
			float num = pos.x - (float)x;
			if (num < 0f)
			{
				num = 0f - num;
			}
			if (num > 1f)
			{
				return 0f;
			}
			float num2 = pos.z - (float)z;
			if (num2 < 0f)
			{
				num2 = 0f - num2;
			}
			if (num2 > 1f)
			{
				return 0f;
			}
			return (1f - num) * (1f - num2);
		}

		public float GetInterpolatedFalloff(Vector2D center, float radius, float hardness, int smooth = 1)
		{
			if (radius > 2f)
			{
				return GetFalloff(center, radius, hardness, smooth);
			}
			if (radius > 1f)
			{
				return GetFalloff(center, radius, hardness, smooth) * (radius - 1f) + GetInterpolatedPercent(center) * (2f - radius);
			}
			return GetInterpolatedPercent(center) * radius;
		}

		public string Encode()
		{
			return "x=" + x + " z=" + z;
		}

		public void Decode(string[] lineMembers)
		{
			x = (int)lineMembers[2].Parse(typeof(int));
			z = (int)lineMembers[3].Parse(typeof(int));
		}
	}
}
