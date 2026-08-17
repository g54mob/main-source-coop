using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Den.Tools
{
	[Serializable]
	public struct CoordRect : IEnumerable<Coord>, IEnumerable
	{
		public enum TileMode
		{
			Clamp = 0,
			Tile = 1,
			PingPong = 2
		}

		public Coord offset;

		public Coord size;

		public Coord Max
		{
			get
			{
				return offset + size;
			}
			set
			{
				size = value - offset;
			}
		}

		public int MaxX
		{
			get
			{
				return offset.x + size.x;
			}
			set
			{
				size.x = value - offset.x;
			}
		}

		public int MaxZ
		{
			get
			{
				return offset.z + size.z;
			}
			set
			{
				size.z = value - offset.z;
			}
		}

		public Coord Min
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

		public Coord Center => offset + size / 2;

		public Vector3 CenterVector3 => new Vector3((float)offset.x + (float)size.x / 2f, 0f, (float)offset.z + (float)size.z / 2f);

		public int Count => size.x * size.z;

		public Vector4 vector4 => new Vector4(offset.x, offset.z, size.x, size.z);

		public CoordRect(Coord offset, Coord size)
		{
			this.offset = offset;
			this.size = size;
		}

		public CoordRect(int offsetX, int offsetZ, int sizeX, int sizeZ)
		{
			offset = new Coord(offsetX, offsetZ);
			size = new Coord(sizeX, sizeZ);
		}

		public CoordRect(float offsetX, float offsetZ, float sizeX, float sizeZ)
		{
			offset = new Coord((int)offsetX, (int)offsetZ);
			size = new Coord((int)sizeX, (int)sizeZ);
		}

		public CoordRect(Rect r)
		{
			offset = new Coord((int)r.x, (int)r.y);
			size = new Coord((int)r.width, (int)r.height);
		}

		public CoordRect(Coord center, int radius)
		{
			offset = center - radius;
			size = new Coord(radius * 2, radius * 2);
		}

		public CoordRect(Vector2D center, float radius)
		{
			Coord coord = Coord.Round(center);
			int num = (int)(radius + 1f);
			offset = coord - num;
			size = new Coord(num + 1 + num);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return offset.x * 100000000 + offset.z * 1000000 + size.x * 1000 + size.z;
		}

		public int GetPos(Coord c)
		{
			return (c.z - offset.z) * size.x + c.x - offset.x;
		}

		public int GetPos(int x, int z)
		{
			return (z - offset.z) * size.x + x - offset.x;
		}

		public Coord GetCoord(int pos)
		{
			int num = pos / size.x + offset.z;
			return new Coord(pos - (num - offset.z) * size.x + offset.x, num);
		}

		public static bool operator >(CoordRect c1, CoordRect c2)
		{
			return c1.size > c2.size;
		}

		public static bool operator <(CoordRect c1, CoordRect c2)
		{
			return c1.size < c2.size;
		}

		public static bool operator ==(CoordRect c1, CoordRect c2)
		{
			if (c1.offset == c2.offset)
			{
				return c1.size == c2.size;
			}
			return false;
		}

		public static bool operator !=(CoordRect c1, CoordRect c2)
		{
			if (!(c1.offset != c2.offset))
			{
				return c1.size != c2.size;
			}
			return true;
		}

		public static CoordRect operator *(CoordRect c, int s)
		{
			return new CoordRect(c.offset * s, c.size * s);
		}

		public static CoordRect operator *(CoordRect c, float s)
		{
			return new CoordRect(c.offset * s, c.size * s);
		}

		public static CoordRect operator /(CoordRect c, int s)
		{
			return new CoordRect(c.offset / s, c.size / s);
		}

		public static explicit operator CoordRect(Vector4 vec)
		{
			return new CoordRect((int)vec.x, (int)vec.y, (int)vec.z, (int)vec.w);
		}

		public static explicit operator Vector4(CoordRect cr)
		{
			return new Vector4(cr.offset.x, cr.offset.z, cr.size.x, cr.size.z);
		}

		public static explicit operator CoordRect(Rect r)
		{
			return new CoordRect((int)r.x, (int)r.y, (int)r.width, (int)r.height);
		}

		public static explicit operator Rect(CoordRect cr)
		{
			return new Rect(cr.offset.x, cr.offset.z, cr.size.x, cr.size.z);
		}

		public IEnumerator<Coord> GetEnumerator()
		{
			Coord min = offset;
			Coord max = offset + size;
			for (int x = min.x; x < max.x; x++)
			{
				for (int z = min.z; z < max.z; z++)
				{
					yield return new Coord(x, z);
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			Coord min = offset;
			Coord max = offset + size;
			for (int x = min.x; x < max.x; x++)
			{
				for (int z = min.z; z < max.z; z++)
				{
					yield return new Coord(x, z);
				}
			}
		}

		public void Expand(int v)
		{
			offset.x -= v;
			offset.z -= v;
			size.x += v * 2;
			size.z += v * 2;
		}

		public CoordRect Expanded(int v)
		{
			return new CoordRect(offset.x - v, offset.z - v, size.x + v * 2, size.z + v * 2);
		}

		public void Contract(int v)
		{
			offset.x += v;
			offset.z += v;
			size.x -= v * 2;
			size.z -= v * 2;
		}

		public CoordRect Contracted(int v)
		{
			return new CoordRect(offset.x + v, offset.z + v, size.x - v * 2, size.z - v * 2);
		}

		public void Clamp(Coord min, Coord max)
		{
			Coord max2 = Max;
			offset = Coord.Max(min, offset);
			size = Coord.Min(max - offset, max2 - offset);
			size.ClampPositive();
		}

		public void ClampLine(ref Coord from, ref Coord to)
		{
			if (!Contains(from) || !Contains(to))
			{
				_ = (from - to).vector2.normalized;
			}
		}

		public static CoordRect Intersected(CoordRect c1, CoordRect c2)
		{
			c1.Clamp(c2.Min, c2.Max);
			return c1;
		}

		public static CoordRect Intersected(CoordRect c1, CoordRect c2, CoordRect c3)
		{
			c1.Clamp(c2.Min, c2.Max);
			c1.Clamp(c3.Min, c3.Max);
			return c1;
		}

		public static bool IsIntersecting(CoordRect c1, CoordRect c2)
		{
			if (c2.Contains(c1.offset.x, c1.offset.z) || c2.Contains(c1.offset.x + c1.size.x, c1.offset.z) || c2.Contains(c1.offset.x, c1.offset.z + c1.size.z) || c2.Contains(c1.offset.x + c1.size.x, c1.offset.z + c1.size.z))
			{
				return true;
			}
			if (c1.Contains(c2.offset.x, c2.offset.z) || c1.Contains(c2.offset.x + c2.size.x, c2.offset.z) || c1.Contains(c2.offset.x, c2.offset.z + c1.size.z) || c1.Contains(c2.offset.x + c2.size.x, c2.offset.z + c2.size.z))
			{
				return true;
			}
			return false;
		}

		public static CoordRect Combined(CoordRect rect1, CoordRect rect2)
		{
			Coord coord = Coord.Min(rect1.offset, rect2.offset);
			Coord coord2 = Coord.Max(rect1.Max, rect2.Max);
			return new CoordRect(coord, coord2 - coord);
		}

		public static CoordRect Combined(CoordRect[] rects)
		{
			Coord coord = new Coord(2000000000, 2000000000);
			Coord coord2 = new Coord(-2000000000, -2000000000);
			for (int i = 0; i < rects.Length; i++)
			{
				if (rects[i].offset.x < coord.x)
				{
					coord.x = rects[i].offset.x;
				}
				if (rects[i].offset.z < coord.z)
				{
					coord.z = rects[i].offset.z;
				}
				if (rects[i].offset.x + rects[i].size.x > coord2.x)
				{
					coord2.x = rects[i].offset.x + rects[i].size.x;
				}
				if (rects[i].offset.z + rects[i].size.z > coord2.z)
				{
					coord2.z = rects[i].offset.z + rects[i].size.z;
				}
			}
			return new CoordRect(coord, coord2 - coord);
		}

		public void Encapsulate(Coord coord)
		{
			if (coord.x < offset.x)
			{
				size.x += offset.x - coord.x;
				offset.x = coord.x;
			}
			if (coord.x > offset.x + size.x)
			{
				size.x = coord.x - offset.x;
			}
			if (coord.z < offset.z)
			{
				size.z += offset.z - coord.z;
				offset.z = coord.z;
			}
			if (coord.z > offset.z + size.z)
			{
				size.z = coord.z - offset.z;
			}
		}

		public static CoordRect WorldToPixel(Vector2D worldPos, Vector2D worldSize, Vector2D pixelSize, bool inclusive = true)
		{
			if (pixelSize.x == 0f || pixelSize.z == 0f)
			{
				throw new Exception("Cell size is zero");
			}
			Coord coord;
			Coord coord2;
			if (inclusive)
			{
				coord = new Coord(Mathf.FloorToInt(worldPos.x / pixelSize.x), Mathf.FloorToInt(worldPos.z / pixelSize.z));
				coord2 = new Coord(Mathf.CeilToInt((worldPos.x + worldSize.x) / pixelSize.x), Mathf.CeilToInt((worldPos.z + worldSize.z) / pixelSize.z));
			}
			else
			{
				coord = new Coord(Mathf.CeilToInt(worldPos.x / pixelSize.x), Mathf.CeilToInt(worldPos.z / pixelSize.z));
				coord2 = new Coord(Mathf.FloorToInt((worldPos.x + worldSize.x) / pixelSize.x), Mathf.FloorToInt((worldPos.z + worldSize.z) / pixelSize.z));
			}
			return new CoordRect(coord, coord2 - coord);
		}

		public bool Contains(Coord coord)
		{
			if (coord.x >= offset.x && coord.x < offset.x + size.x && coord.z >= offset.z)
			{
				return coord.z < offset.z + size.z;
			}
			return false;
		}

		public bool Contains(int x, int z)
		{
			if (x - offset.x >= 0 && x - offset.x < size.x && z - offset.z >= 0)
			{
				return z - offset.z < size.z;
			}
			return false;
		}

		public bool Contains(float x, float z)
		{
			if (x - (float)offset.x >= 0f && x - (float)offset.x < (float)size.x && z - (float)offset.z >= 0f)
			{
				return z - (float)offset.z < (float)size.z;
			}
			return false;
		}

		public bool Contains(Vector2 pos)
		{
			if (pos.x - (float)offset.x >= 0f && pos.x - (float)offset.x < (float)size.x && pos.y - (float)offset.z >= 0f)
			{
				return pos.y - (float)offset.z < (float)size.z;
			}
			return false;
		}

		public bool Contains(Vector3 pos)
		{
			if (pos.x - (float)offset.x >= 0f && pos.x - (float)offset.x < (float)size.x && pos.z - (float)offset.z >= 0f)
			{
				return pos.z - (float)offset.z < (float)size.z;
			}
			return false;
		}

		public bool Contains(float x, float z, float margins)
		{
			if (x - (float)offset.x >= margins && x - (float)offset.x < (float)size.x - margins && z - (float)offset.z >= margins)
			{
				return z - (float)offset.z < (float)size.z - margins;
			}
			return false;
		}

		public bool Contains(CoordRect r)
		{
			if (r.offset.x >= offset.x && r.offset.x + r.size.x <= offset.x + size.x && r.offset.z >= offset.z)
			{
				return r.offset.z + r.size.z <= offset.z + size.z;
			}
			return false;
		}

		public bool ContainsOrIntersects(CoordRect r)
		{
			if (r.offset.x > offset.x - r.size.x && r.offset.x + r.size.x < offset.x + size.x + r.size.x && r.offset.z > offset.z - r.size.z)
			{
				return r.offset.z + r.size.z < offset.z + size.z + r.size.z;
			}
			return false;
		}

		public Coord Tile(Coord coord, TileMode tileMode)
		{
			coord.x -= offset.x;
			coord.z -= offset.z;
			switch (tileMode)
			{
			case TileMode.Clamp:
				if (coord.x < 0)
				{
					coord.x = 0;
				}
				if (coord.x >= size.x)
				{
					coord.x = size.x - 1;
				}
				if (coord.z < 0)
				{
					coord.z = 0;
				}
				if (coord.z >= size.z)
				{
					coord.z = size.z - 1;
				}
				break;
			case TileMode.Tile:
				coord.x %= size.x;
				if (coord.x < 0)
				{
					coord.x = size.x + coord.x;
				}
				coord.z %= size.z;
				if (coord.z < 0)
				{
					coord.z = size.z + coord.z;
				}
				break;
			case TileMode.PingPong:
				coord.x %= size.x * 2;
				if (coord.x < 0)
				{
					coord.x = size.x * 2 + coord.x;
				}
				if (coord.x >= size.x)
				{
					coord.x = size.x * 2 - coord.x - 1;
				}
				coord.z %= size.z * 2;
				if (coord.z < 0)
				{
					coord.z = size.z * 2 + coord.z;
				}
				if (coord.z >= size.z)
				{
					coord.z = size.z * 2 - coord.z - 1;
				}
				break;
			}
			coord.x += offset.x;
			coord.z += offset.z;
			return coord;
		}

		public void Tile(ref Coord coord, TileMode tileMode)
		{
			coord.x -= offset.x;
			coord.z -= offset.z;
			switch (tileMode)
			{
			case TileMode.Clamp:
				if (coord.x < 0)
				{
					coord.x = 0;
				}
				if (coord.x >= size.x)
				{
					coord.x = size.x - 1;
				}
				if (coord.z < 0)
				{
					coord.z = 0;
				}
				if (coord.z >= size.z)
				{
					coord.z = size.z - 1;
				}
				break;
			case TileMode.Tile:
				coord.x %= size.x;
				if (coord.x < 0)
				{
					coord.x = size.x + coord.x;
				}
				coord.z %= size.z;
				if (coord.z < 0)
				{
					coord.z = size.z + coord.z;
				}
				break;
			case TileMode.PingPong:
				coord.x %= size.x * 2;
				if (coord.x < 0)
				{
					coord.x = size.x * 2 + coord.x;
				}
				if (coord.x >= size.x)
				{
					coord.x = size.x * 2 - coord.x - 1;
				}
				coord.z %= size.z * 2;
				if (coord.z < 0)
				{
					coord.z = size.z * 2 + coord.z;
				}
				if (coord.z >= size.z)
				{
					coord.z = size.z * 2 - coord.z - 1;
				}
				break;
			}
			coord.x += offset.x;
			coord.z += offset.z;
		}

		public override string ToString()
		{
			return base.ToString() + ": offsetX:" + offset.x + " offsetZ:" + offset.z + " sizeX:" + size.x + " sizeZ:" + size.z;
		}

		public void DrawGizmo()
		{
		}

		[Obsolete]
		public static CoordRect PickIntersectingCells(CoordRect rect, int cellRes)
		{
			int num = rect.offset.x + rect.size.x;
			int num2 = rect.offset.z + rect.size.z;
			int num3 = rect.offset.x / cellRes;
			if (rect.offset.x < 0 && rect.offset.x % cellRes != 0)
			{
				num3--;
			}
			int num4 = rect.offset.z / cellRes;
			if (rect.offset.z < 0 && rect.offset.z % cellRes != 0)
			{
				num4--;
			}
			int num5 = num / cellRes;
			if (num >= 0 && num % cellRes != 0)
			{
				num5++;
			}
			int num6 = num2 / cellRes;
			if (num2 >= 0 && num2 % cellRes != 0)
			{
				num6++;
			}
			return new CoordRect(num3, num4, num5 - num3, num6 - num4);
		}

		[Obsolete]
		public static CoordRect PickIntersectingCellsByPos(float rectMinX, float rectMinZ, float rectMaxX, float rectMaxZ, float cellSize)
		{
			int num = (int)(rectMinX / cellSize);
			if (rectMinX < 0f && rectMinX != (float)num * cellSize)
			{
				num--;
			}
			int num2 = (int)(rectMinZ / cellSize);
			if (rectMinZ < 0f && rectMinZ != (float)num2 * cellSize)
			{
				num2--;
			}
			int num3 = (int)(rectMaxX / cellSize);
			if (rectMaxX >= 0f && rectMaxX != (float)num3 * cellSize)
			{
				num3++;
			}
			int num4 = (int)(rectMaxZ / cellSize);
			if (rectMaxZ >= 0f && rectMaxZ != (float)num4 * cellSize)
			{
				num4++;
			}
			return new CoordRect(num, num2, num3 - num, num4 - num2);
		}

		[Obsolete]
		public static CoordRect PickIntersectingCellsByPos(Vector3 pos, float range, float cellSize = 1f)
		{
			return PickIntersectingCellsByPos(pos.x - range, pos.z - range, pos.x + range, pos.z + range, cellSize);
		}

		[Obsolete]
		public static CoordRect PickIntersectingCellsByPos(Rect rect, float cellSize = 1f)
		{
			return PickIntersectingCellsByPos(rect.position.x, rect.position.y, rect.position.x + rect.size.x, rect.position.y + rect.size.y, cellSize);
		}

		[Obsolete]
		public CoordRect MapSized(int resolution)
		{
			return new CoordRect(Mathf.RoundToInt((float)offset.x / (1f * (float)size.x / (float)resolution)), Mathf.RoundToInt((float)offset.z / (1f * (float)size.z / (float)resolution)), resolution, resolution);
		}

		[Obsolete]
		public int GetPos(float x, float z)
		{
			int num = (int)(x + 0.5f);
			if (x < 1f)
			{
				num--;
			}
			int num2 = (int)(z + 0.5f);
			if (z < 1f)
			{
				num2--;
			}
			return (num2 - offset.z) * size.x + num - offset.x;
		}

		[Obsolete]
		public string Encode()
		{
			return "offsetX=" + offset.x + " offsetZ=" + offset.z + " sizeX=" + size.x + " sizeZ=" + size.z;
		}

		[Obsolete]
		public void Decode(string[] lineMembers)
		{
			offset.x = (int)lineMembers[2].Parse(typeof(int));
			offset.z = (int)lineMembers[3].Parse(typeof(int));
			size.x = (int)lineMembers[4].Parse(typeof(int));
			size.z = (int)lineMembers[5].Parse(typeof(int));
		}
	}
}
