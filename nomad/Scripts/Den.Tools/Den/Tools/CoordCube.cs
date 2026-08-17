using System;

namespace Den.Tools
{
	[Serializable]
	public struct CoordCube
	{
		public CoordDir offset;

		public CoordDir size;

		public CoordDir Max
		{
			get
			{
				return offset + size;
			}
			set
			{
				offset = value - size;
			}
		}

		public CoordDir Min
		{
			get
			{
				return offset;
			}
			set
			{
				offset = value;
			}
		}

		public CoordDir Center => offset + size / 2;

		public int this[int x, int y, int z] => (z - offset.z) * size.x * size.y + (y - offset.y) * size.x + x - offset.x;

		public int this[CoordDir c] => (c.z - offset.z) * size.x * size.y + (c.y - offset.y) * size.x + c.x - offset.x;

		public CoordRect rect => new CoordRect(offset.coord, size.coord);

		public CoordCube(CoordDir offset, CoordDir size)
		{
			this.offset = offset;
			this.size = size;
		}

		public CoordCube(int offsetX, int offsetY, int offsetZ, int sizeX, int sizeY, int sizeZ)
		{
			offset = new CoordDir(offsetX, offsetY, offsetZ, 7);
			size = new CoordDir(sizeX, sizeY, sizeZ, 7);
		}

		public static bool operator ==(CoordCube c1, CoordCube c2)
		{
			if (c1.offset.x == c2.offset.x && c1.offset.y == c2.offset.y && c1.offset.z == c2.offset.z && c1.size.x == c2.size.x && c1.size.y == c2.size.y)
			{
				return c1.size.z == c2.size.z;
			}
			return false;
		}

		public static bool operator !=(CoordCube c1, CoordCube c2)
		{
			if (c1.offset.x == c2.offset.x && c1.offset.y == c2.offset.y && c1.offset.z == c2.offset.z && c1.size.x == c2.size.x && c1.size.y == c2.size.y)
			{
				return c1.size.z != c2.size.z;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return offset.GetHashCode() ^ size.GetHashCode();
		}

		public static CoordCube operator *(CoordCube c, int s)
		{
			return new CoordCube(c.offset * s, c.size * s);
		}

		public static CoordCube operator /(CoordCube c, int s)
		{
			return new CoordCube(c.offset / s, c.size / s);
		}

		public bool Contains(CoordDir c)
		{
			if (c.x >= offset.x && c.x < offset.x + size.x && c.y >= offset.y && c.y < offset.y + size.y && c.z >= offset.z)
			{
				return c.z < offset.z + size.z;
			}
			return false;
		}

		public bool Contains(int x, int y, int z)
		{
			if (x >= offset.x && x < offset.x + size.x && y >= offset.y && y < offset.y + size.y && z >= offset.z)
			{
				return z < offset.z + size.z;
			}
			return false;
		}

		public int GetPos(int x, int y, int z)
		{
			return (z - offset.z) * size.x * size.y + (y - offset.y) * size.x + x - offset.x;
		}

		public int GetPos(CoordDir c)
		{
			return (c.z - offset.z) * size.x * size.y + (c.y - offset.y) * size.x + c.x - offset.x;
		}

		public CoordDir GetCoord(int e)
		{
			return new CoordDir(e % size.x + offset.x, e / size.x % size.y + offset.y, e / (size.x * size.y) + offset.z);
		}

		public void Encapsulate(CoordDir coord)
		{
			if (coord.x < offset.x)
			{
				size.x += offset.x - coord.x;
				offset.x = coord.x;
			}
			if (coord.x >= offset.x + size.x)
			{
				size.x = coord.x - offset.x + 1;
			}
			if (coord.y < offset.y)
			{
				size.y += offset.y - coord.y;
				offset.y = coord.y;
			}
			if (coord.y >= offset.y + size.y)
			{
				size.y = coord.y - offset.y + 1;
			}
			if (coord.z < offset.z)
			{
				size.z += offset.z - coord.z;
				offset.z = coord.z;
			}
			if (coord.z >= offset.z + size.z)
			{
				size.z = coord.z - offset.z + 1;
			}
		}
	}
}
