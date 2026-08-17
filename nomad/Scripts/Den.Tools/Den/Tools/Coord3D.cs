using System;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public struct Coord3D
	{
		public int x;

		public int y;

		public int z;

		public static readonly Coord3D up = new Coord3D(0, 1, 0);

		public static readonly Coord3D down = new Coord3D(0, -1, 0);

		public static readonly Coord3D front = new Coord3D(0, 0, 1);

		public static readonly Coord3D back = new Coord3D(0, 0, -1);

		public static readonly Coord3D left = new Coord3D(-1, 0, 0);

		public static readonly Coord3D right = new Coord3D(1, 0, 0);

		public int Minimal
		{
			get
			{
				int num = ((x < z) ? x : z);
				if (num >= y)
				{
					return y;
				}
				return num;
			}
		}

		public int Maximal
		{
			get
			{
				int num = ((x > z) ? x : z);
				if (num <= y)
				{
					return y;
				}
				return num;
			}
		}

		public int SqrMagnitude => x * x + y * y + z * z;

		public float Magnitude => Mathf.Sqrt(x * x + y * y + z * z);

		public static Coord3D zero => new Coord3D(0, 0, 0);

		public static bool operator >(Coord3D c1, Coord3D c2)
		{
			if (c1.x > c2.x && c1.y > c2.y)
			{
				return c1.z > c2.z;
			}
			return false;
		}

		public static bool operator <(Coord3D c1, Coord3D c2)
		{
			if (c1.x < c2.x && c1.y < c2.y)
			{
				return c1.z < c2.z;
			}
			return false;
		}

		public static bool operator ==(Coord3D c1, Coord3D c2)
		{
			if (c1.x == c2.x && c1.y == c2.y)
			{
				return c1.z == c2.z;
			}
			return false;
		}

		public static bool operator !=(Coord3D c1, Coord3D c2)
		{
			if (c1.x == c2.x && c1.y == c2.y)
			{
				return c1.z != c2.z;
			}
			return true;
		}

		public static Coord3D operator +(Coord3D c, int s)
		{
			return new Coord3D(c.x + s, c.y + s, c.z + s);
		}

		public static Coord3D operator +(Coord3D c1, Coord3D c2)
		{
			return new Coord3D(c1.x + c2.x, c1.y + c2.y, c1.z + c2.z);
		}

		public static Coord3D operator -(Coord3D c)
		{
			return new Coord3D(-c.x, -c.y, -c.z);
		}

		public static Coord3D operator -(Coord3D c, int s)
		{
			return new Coord3D(c.x - s, c.y - s, c.z - s);
		}

		public static Coord3D operator -(Coord3D c1, Coord3D c2)
		{
			return new Coord3D(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
		}

		public static Coord3D operator *(Coord3D c, int s)
		{
			return new Coord3D(c.x * s, c.y * s, c.z * s);
		}

		public static Vector3 operator *(Coord3D c, Vector3 s)
		{
			return new Vector3((float)c.x * s.x, (float)c.y * s.y, (float)c.z * s.z);
		}

		public static Coord3D operator *(Coord3D c1, Coord3D c2)
		{
			return new Coord3D(c1.x * c2.x, c1.y * c2.y, c1.z * c2.z);
		}

		public static Coord3D operator *(Coord3D c, float s)
		{
			return new Coord3D((int)((float)c.x * s), (int)((float)c.y * s), (int)((float)c.z * s));
		}

		public static Coord3D operator /(Coord3D c, int s)
		{
			return new Coord3D(c.x / s, c.y / s, c.z / s);
		}

		public static Coord3D operator /(Coord3D c, float s)
		{
			return new Coord3D((int)((float)c.x / s), (int)((float)c.y / s), (int)((float)c.z / s));
		}

		public override bool Equals(object obj)
		{
			if (obj is Coord3D coord3D)
			{
				if (coord3D.x == x && coord3D.y == y)
				{
					return coord3D.z == z;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return x * 1000000 + y * 1000 + z;
		}

		public static explicit operator Coord3D(Vector3 v)
		{
			return new Coord3D((int)v.x, (int)v.y, (int)v.z);
		}

		public static explicit operator Vector3(Coord3D c)
		{
			return new Vector3(c.x, c.y, c.z);
		}

		public Coord3D(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override string ToString()
		{
			return $"{base.ToString()} x:{x}, y{y}, z:{z}";
		}
	}
}
