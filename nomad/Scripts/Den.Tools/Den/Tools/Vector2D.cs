using System;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public struct Vector2D : IEquatable<Vector2D>
	{
		public float x;

		public float z;

		public const float kEpsilon = 1E-05f;

		public const float kEpsilonNormalSqrt = 1E-15f;

		public static readonly Vector2D zero = new Vector2D(0f, 0f);

		public static readonly Vector2D one = new Vector2D(1f, 1f);

		public float this[int c]
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
				if (x == 0f)
				{
					x = value;
				}
				else
				{
					z = value;
				}
			}
		}

		public static Vector2D Zero { get; } = new Vector2D(0f, 0f);

		public static Vector2D One { get; } = new Vector2D(1f, 1f);

		public static Vector2D PositiveInfinity { get; } = new Vector2D(1f / 0f, 1f / 0f);

		public static Vector2D NegativeInfinity { get; } = new Vector2D(-1f / 0f, -1f / 0f);

		public float SqrMagnitude => x * x + z * z;

		public float Magnitude => (float)Math.Sqrt(x * x + z * z);

		public Vector2D Normalized
		{
			get
			{
				float num = (float)Math.Sqrt(x * x + z * z);
				if (!(num > 1E-05f))
				{
					return new Vector2D(0f, 0f);
				}
				return new Vector2D(x / num, z / num);
			}
		}

		public Vector2D(float x, float z)
		{
			this.x = x;
			this.z = z;
		}

		public Vector2D(float v)
		{
			x = v;
			z = v;
		}

		public void ClampPositive()
		{
			if (x < 0f)
			{
				x = 0f;
			}
			if (z < 0f)
			{
				z = 0f;
			}
		}

		public static float Dot(Vector2D lhs, Vector2D rhs)
		{
			return lhs.x * rhs.x + lhs.z * rhs.z;
		}

		public static float Distance(Vector2D a, Vector2D b)
		{
			float num = a.x - b.x;
			float num2 = a.z - b.z;
			return (float)Math.Sqrt(num * num + num2 * num2);
		}

		public static Vector2D Lerp(Vector2D a, Vector2D b, float t)
		{
			if (t > 1f)
			{
				t = 1f;
			}
			if (t < 0f)
			{
				t = 0f;
			}
			return new Vector2D(a.x + (b.x - a.x) * t, a.z + (b.z - a.z) * t);
		}

		public static Vector2D Min(Vector2D lhs, Vector2D rhs)
		{
			return new Vector2D((lhs.x < rhs.x) ? lhs.x : rhs.x, (lhs.z < rhs.z) ? lhs.z : rhs.z);
		}

		public static Vector2D Max(Vector2D lhs, Vector2D rhs)
		{
			return new Vector2D((lhs.x > rhs.x) ? lhs.x : rhs.x, (lhs.z > rhs.z) ? lhs.z : rhs.z);
		}

		public static Vector2D Normalize(Vector3 v)
		{
			float num = (float)Math.Sqrt(v.x * v.x + v.z * v.z);
			if (!(num > 1E-05f))
			{
				return new Vector2D(0f, 0f);
			}
			return new Vector2D(v.x / num, v.z / num);
		}

		public void Normalize()
		{
			float num = (float)Math.Sqrt(x * x + z * z);
			if (num > 1E-05f)
			{
				x /= num;
				z /= num;
			}
			else
			{
				x = 0f;
				z = 0f;
			}
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ (z.GetHashCode() << 2);
		}

		public bool Equals(Vector2D other)
		{
			if (x == other.x)
			{
				return z == other.z;
			}
			return false;
		}

		public override bool Equals(object other)
		{
			if (!(other is Vector2D vector2D))
			{
				return false;
			}
			if (x == vector2D.x)
			{
				return z == vector2D.z;
			}
			return false;
		}

		public static (Vector2D, Vector2D) Intersected(Vector2D pos1, Vector2D size1, Vector2D pos2, Vector2D size2)
		{
			Vector2D vector2D = Max(pos1, pos2);
			Vector2D item = Min(pos1 + size1, pos2 + size2) - vector2D;
			item.ClampPositive();
			return (vector2D, item);
		}

		public static bool Intersects(Vector2D pos1, Vector2D size1, Vector2D pos2, Vector2D size2)
		{
			Vector2D vector2D = Max(pos1, pos2);
			Vector2D vector2D2 = Min(pos1 + size1, pos2 + size2) - vector2D;
			if (vector2D2.x > 0f && vector2D2.z > 0f)
			{
				return true;
			}
			return false;
		}

		public static bool Contains(Vector2D pos, Vector2D size, Vector2D pos2)
		{
			if (pos2.x > pos.x && pos2.x < pos.x + size.x && pos2.z > pos.z)
			{
				return pos2.z < pos.z + size.z;
			}
			return false;
		}

		public override string ToString()
		{
			return $"({x:F1}, {z:F1})";
		}

		public static Vector2D operator +(Vector2D a, Vector2D b)
		{
			return new Vector2D(a.x + b.x, a.z + b.z);
		}

		public static Vector2D operator +(Vector2D a, float b)
		{
			return new Vector2D(a.x + b, a.z + b);
		}

		public static Vector2D operator -(Vector2D a, Vector2D b)
		{
			return new Vector2D(a.x - b.x, a.z - b.z);
		}

		public static Vector2D operator -(Vector2D a, float b)
		{
			return new Vector2D(a.x - b, a.z - b);
		}

		public static Vector2D operator *(Vector2D a, Vector2D b)
		{
			return new Vector2D(a.x * b.x, a.z * b.z);
		}

		public static Vector2D operator /(Vector2D a, Vector2D b)
		{
			return new Vector2D(a.x / b.x, a.z / b.z);
		}

		public static Vector2D operator -(Vector2D a)
		{
			return new Vector2D(0f - a.x, 0f - a.z);
		}

		public static Vector2D operator *(Vector2D a, float d)
		{
			return new Vector2D(a.x * d, a.z * d);
		}

		public static Vector2D operator *(float d, Vector2D a)
		{
			return new Vector2D(a.x * d, a.z * d);
		}

		public static Vector2D operator /(Vector2D a, float d)
		{
			return new Vector2D(a.x / d, a.z / d);
		}

		public static Vector2D operator /(float d, Vector2D a)
		{
			return new Vector2D(d / a.x, d / a.z);
		}

		public static Vector3 operator *(Vector3 a, Vector2D b)
		{
			return new Vector3(a.x * b.x, a.y, a.z * b.z);
		}

		public static Vector3 operator /(Vector3 a, Vector2D b)
		{
			return new Vector3(a.x / b.x, a.y, a.z / b.z);
		}

		public static Vector3 operator +(Vector3 a, Vector2D b)
		{
			return new Vector3(a.x + b.x, a.y, a.z + b.z);
		}

		public static Vector3 operator -(Vector3 a, Vector2D b)
		{
			return new Vector3(a.x - b.x, a.y, a.z - b.z);
		}

		public static Vector3 operator *(Vector2D a, Vector3 b)
		{
			return new Vector3(a.x * b.x, b.y, a.z * b.z);
		}

		public static Vector3 operator /(Vector2D a, Vector3 b)
		{
			return new Vector3(a.x / b.x, b.y, a.z / b.z);
		}

		public static Vector3 operator +(Vector2D a, Vector3 b)
		{
			return new Vector3(a.x + b.x, b.y, a.z + b.z);
		}

		public static Vector3 operator -(Vector2D a, Vector3 b)
		{
			return new Vector3(a.x - b.x, b.y, a.z - b.z);
		}

		public static bool operator ==(Vector2D lhs, Vector2D rhs)
		{
			float num = lhs.x - rhs.x;
			float num2 = lhs.z - rhs.z;
			return num * num + num2 * num2 < 9.9999994E-11f;
		}

		public static bool operator !=(Vector2D lhs, Vector2D rhs)
		{
			return !(lhs == rhs);
		}

		public static explicit operator Vector2D(Vector3 v)
		{
			return new Vector2D(v.x, v.z);
		}

		public static explicit operator Vector3(Vector2D v)
		{
			return new Vector3(v.x, 0f, v.z);
		}

		public static explicit operator Vector2D(Vector2 v)
		{
			return new Vector2D(v.x, v.y);
		}

		public static explicit operator Vector2(Vector2D v)
		{
			return new Vector3(v.x, v.z);
		}

		public static explicit operator Vector2D(float v)
		{
			return new Vector2D(v, v);
		}

		public Coord RoundToCoord()
		{
			return new Coord((int)((x < 0f) ? (x - 1f) : (x + 0.5f)), (int)((z < 0f) ? (z - 1f) : (z + 0.5f)));
		}
	}
}
