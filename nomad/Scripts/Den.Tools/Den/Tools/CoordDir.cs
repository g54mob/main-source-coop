using System;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public struct CoordDir
	{
		public int x;

		public int y;

		public int z;

		public byte dir;

		public static readonly byte[] oppositeDir = new byte[6] { 1, 0, 3, 2, 5, 4 };

		public static readonly int[] dirToPosX = new int[6] { 0, 0, 1, -1, 0, 0 };

		public static readonly int[] dirToPosY = new int[6] { 1, -1, 0, 0, 0, 0 };

		public static readonly int[] dirToPosZ = new int[6] { 0, 0, 0, 0, 1, -1 };

		public static readonly CoordDir[] neigsLut = new CoordDir[72]
		{
			new CoordDir(0, 0, 1, 0),
			new CoordDir(0, 0, 0, 4),
			new CoordDir(0, 1, 1, 5),
			new CoordDir(1, 0, 0, 0),
			new CoordDir(0, 0, 0, 2),
			new CoordDir(1, 1, 0, 3),
			new CoordDir(0, 0, -1, 0),
			new CoordDir(0, 0, 0, 5),
			new CoordDir(0, 1, -1, 4),
			new CoordDir(-1, 0, 0, 0),
			new CoordDir(0, 0, 0, 3),
			new CoordDir(-1, 1, 0, 2),
			new CoordDir(0, 0, 1, 1),
			new CoordDir(0, 0, 0, 4),
			new CoordDir(0, -1, 1, 5),
			new CoordDir(-1, 0, 0, 1),
			new CoordDir(0, 0, 0, 3),
			new CoordDir(-1, -1, 0, 2),
			new CoordDir(0, 0, -1, 1),
			new CoordDir(0, 0, 0, 5),
			new CoordDir(0, -1, -1, 4),
			new CoordDir(1, 0, 0, 1),
			new CoordDir(0, 0, 0, 2),
			new CoordDir(1, -1, 0, 3),
			new CoordDir(0, 1, 0, 2),
			new CoordDir(0, 0, 0, 0),
			new CoordDir(1, 1, 0, 1),
			new CoordDir(0, 0, 1, 2),
			new CoordDir(0, 0, 0, 4),
			new CoordDir(1, 0, 1, 5),
			new CoordDir(0, -1, 0, 2),
			new CoordDir(0, 0, 0, 1),
			new CoordDir(1, -1, 0, 0),
			new CoordDir(0, 0, -1, 2),
			new CoordDir(0, 0, 0, 5),
			new CoordDir(1, 0, -1, 4),
			new CoordDir(0, 1, 0, 3),
			new CoordDir(0, 0, 0, 0),
			new CoordDir(-1, 1, 0, 1),
			new CoordDir(0, 0, -1, 3),
			new CoordDir(0, 0, 0, 5),
			new CoordDir(-1, 0, -1, 4),
			new CoordDir(0, -1, 0, 3),
			new CoordDir(0, 0, 0, 1),
			new CoordDir(-1, -1, 0, 0),
			new CoordDir(0, 0, 1, 3),
			new CoordDir(0, 0, 0, 4),
			new CoordDir(-1, 0, 1, 5),
			new CoordDir(1, 0, 0, 4),
			new CoordDir(0, 0, 0, 2),
			new CoordDir(1, 0, 1, 3),
			new CoordDir(0, 1, 0, 4),
			new CoordDir(0, 0, 0, 0),
			new CoordDir(0, 1, 1, 1),
			new CoordDir(-1, 0, 0, 4),
			new CoordDir(0, 0, 0, 3),
			new CoordDir(-1, 0, 1, 2),
			new CoordDir(0, -1, 0, 4),
			new CoordDir(0, 0, 0, 1),
			new CoordDir(0, -1, 1, 0),
			new CoordDir(1, 0, 0, 5),
			new CoordDir(0, 0, 0, 2),
			new CoordDir(1, 0, -1, 3),
			new CoordDir(0, -1, 0, 5),
			new CoordDir(0, 0, 0, 1),
			new CoordDir(0, -1, -1, 0),
			new CoordDir(-1, 0, 0, 5),
			new CoordDir(0, 0, 0, 3),
			new CoordDir(-1, 0, -1, 2),
			new CoordDir(0, 1, 0, 5),
			new CoordDir(0, 0, 0, 0),
			new CoordDir(0, 1, -1, 1)
		};

		public CoordDir opposite => new CoordDir(x + dirToPosX[dir], y + dirToPosY[dir], z + dirToPosZ[dir], oppositeDir[dir]);

		public static CoordDir empty => new CoordDir(0, 0, 0, 7);

		public bool exists => dir != 7;

		public Vector3 center => new Vector3((float)x + 0.5f, (float)y + 0.5f, (float)z + 0.5f);

		public Vector3 pos => new Vector3(x, y, z);

		public int this[int i]
		{
			get
			{
				return i switch
				{
					1 => y, 
					2 => z, 
					_ => x, 
				};
			}
			set
			{
				switch (i)
				{
				default:
					x = value;
					break;
				case 1:
					y = value;
					break;
				case 2:
					z = value;
					break;
				}
			}
		}

		public int BlockMagnitude2 => Mathf.Abs(x) + Mathf.Abs(z);

		public int BlockMagnitude3 => Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z);

		public Vector3 vector3 => new Vector3(x, y, z);

		public Vector3 vector3centered => new Vector3((float)x + 0.5f, (float)y + 0.5f, (float)z + 0.5f);

		public Coord coord => new Coord(x, z);

		public CoordDir(bool empty)
		{
			x = 0;
			y = 0;
			z = 0;
			dir = 0;
			if (!empty)
			{
				dir = 7;
			}
		}

		public CoordDir(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			dir = 7;
		}

		public CoordDir(int x, int y, int z, byte d)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			dir = d;
		}

		public CoordDir(CoordDir c, byte d)
		{
			x = c.x;
			y = c.y;
			z = c.z;
			dir = d;
		}

		public static CoordDir operator +(CoordDir a, CoordDir b)
		{
			return new CoordDir(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static CoordDir operator +(CoordDir a, int i)
		{
			return new CoordDir(a.x + i, a.y + i, a.z + i);
		}

		public static CoordDir operator -(CoordDir a, CoordDir b)
		{
			return new CoordDir(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public static CoordDir operator -(CoordDir a, int i)
		{
			return new CoordDir(a.x - i, a.y - i, a.z - i);
		}

		public static CoordDir operator ++(CoordDir a)
		{
			return new CoordDir(a.x + 1, a.y + 1, a.z + 1);
		}

		public static CoordDir operator *(CoordDir a, int s)
		{
			return new CoordDir(a.x * s, a.y * s, a.z * s);
		}

		public static CoordDir operator *(CoordDir a, CoordDir b)
		{
			return new CoordDir(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static CoordDir operator /(CoordDir a, int s)
		{
			return new CoordDir(a.x / s, a.y / s, a.z / s);
		}

		public static CoordDir operator /(CoordDir a, CoordDir b)
		{
			return new CoordDir(a.x / b.x, a.y / b.y, a.z / b.z);
		}

		public static bool operator ==(CoordDir a, CoordDir b)
		{
			if (a.x == b.x && a.y == b.y && a.z == b.z)
			{
				return a.dir == b.dir;
			}
			return false;
		}

		public static bool operator !=(CoordDir a, CoordDir b)
		{
			if (a.x == b.x && a.y == b.y && a.z == b.z)
			{
				return a.dir != b.dir;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return (y & 0xFFFFFFF) | ((x & 0xFFFF) << 16) | (z & 0xFFFF);
		}

		public static bool operator >(CoordDir a, CoordDir b)
		{
			if (a.x <= b.x)
			{
				return a.z > b.z;
			}
			return true;
		}

		public static bool operator >=(CoordDir a, CoordDir b)
		{
			if (a.x < b.x)
			{
				return a.z >= b.z;
			}
			return true;
		}

		public static bool operator <(CoordDir a, CoordDir b)
		{
			if (a.x >= b.x)
			{
				return a.z < b.z;
			}
			return true;
		}

		public static bool operator <=(CoordDir a, CoordDir b)
		{
			if (a.x > b.x)
			{
				return a.z <= b.z;
			}
			return true;
		}

		public static CoordDir Min(CoordDir[] coords)
		{
			CoordDir result = new CoordDir(2147483647, 2147483647, 2147483647, 7);
			for (int i = 0; i < coords.Length; i++)
			{
				if (coords[i].x < result.x)
				{
					result.x = coords[i].x;
				}
				if (coords[i].y < result.y)
				{
					result.y = coords[i].y;
				}
				if (coords[i].z < result.z)
				{
					result.z = coords[i].z;
				}
			}
			return result;
		}

		public static CoordDir Max(CoordDir[] coords)
		{
			CoordDir result = new CoordDir(-2147483648, -2147483648, -2147483648, 7);
			for (int i = 0; i < coords.Length; i++)
			{
				if (coords[i].x > result.x)
				{
					result.x = coords[i].x;
				}
				if (coords[i].y > result.y)
				{
					result.y = coords[i].y;
				}
				if (coords[i].z > result.z)
				{
					result.z = coords[i].z;
				}
			}
			return result;
		}

		public CoordDir GetChunkCoord(CoordDir worldCoord, int chunkSize)
		{
			return new CoordDir((worldCoord.x >= 0) ? (worldCoord.x / chunkSize) : ((worldCoord.x + 1) / chunkSize - 1), 0, (worldCoord.z >= 0) ? (worldCoord.z / chunkSize) : ((worldCoord.z + 1) / chunkSize - 1));
		}

		public override string ToString()
		{
			return "x:" + x + " y:" + y + " z:" + z + " dir:" + dir;
		}

		public string ToString(bool writeDir)
		{
			return x + "," + y + "," + z + (writeDir ? (" dir:" + dir) : "");
		}

		public static byte NormalToDir(Vector3 normal)
		{
			float num = ((normal.x > 0f) ? normal.x : (0f - normal.x));
			float num2 = ((normal.y > 0f) ? normal.y : (0f - normal.y));
			float num3 = ((normal.z > 0f) ? normal.z : (0f - normal.z));
			if (num2 > num && num2 > num3)
			{
				if (normal.y > 0f)
				{
					return 0;
				}
				return 1;
			}
			if (num > num2 && num > num3)
			{
				if (normal.x > 0f)
				{
					return 2;
				}
				return 3;
			}
			if (num3 > num2 && num3 > num)
			{
				if (normal.z > 0f)
				{
					return 4;
				}
				return 5;
			}
			return 7;
		}
	}
}
