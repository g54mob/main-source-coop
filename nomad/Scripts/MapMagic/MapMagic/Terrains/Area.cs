using System;
using Den.Tools;
using UnityEngine;

namespace MapMagic.Terrains
{
	[Serializable]
	public class Area
	{
		[Serializable]
		public class Dimensions
		{
			public CoordRect rect;

			public Vector2D worldPos;

			public Vector2D worldSize;

			public Vector3 PixelSize => new Vector3(worldSize.x / (float)(rect.size.x - 1), 1f, worldSize.z / (float)(rect.size.z - 1));

			public Dimensions()
			{
			}

			public Dimensions(CoordRect rect, Vector2D worldPos, Vector2D worldSize)
			{
				this.rect = rect;
				this.worldPos = worldPos;
				this.worldSize = worldSize;
			}

			public Dimensions(Vector2D worldPos, Vector2D worldSize, int resolution)
			{
				this.worldPos = worldPos;
				this.worldSize = worldSize;
				Vector3 vector = 1f * (Vector3)worldSize / resolution;
				Coord offset = new Coord(Mathf.FloorToInt(worldPos.x / vector.x), Mathf.FloorToInt(worldPos.z / vector.x));
				rect = new CoordRect(offset, new Coord(resolution, resolution));
			}

			public override string ToString()
			{
				string[] obj = new string[6]
				{
					"Rect:",
					rect.ToString(),
					" Pos:",
					null,
					null,
					null
				};
				Vector2D vector2D = worldPos;
				obj[3] = vector2D.ToString();
				obj[4] = " Size:";
				vector2D = worldSize;
				obj[5] = vector2D.ToString();
				return string.Concat(obj);
			}

			public Vector3 CoordToWorld(int x, int z)
			{
				float num = 1f * (float)(x - rect.offset.x) / (float)rect.size.x;
				float num2 = 1f * (float)(z - rect.offset.z) / (float)rect.size.z;
				float x2 = num * worldSize.x + worldPos.x;
				float z2 = num2 * worldSize.z + worldPos.z;
				return new Vector3(x2, 0f, z2);
			}

			public bool Contains(Vector3 pos)
			{
				if (pos.x < worldPos.x || pos.x > worldPos.x + worldSize.x || pos.z < worldPos.z || pos.z > worldPos.z + worldSize.z)
				{
					return false;
				}
				return true;
			}
		}

		public Dimensions active;

		public Dimensions full;

		public int Margins => (full.rect.size.x - active.rect.size.x) / 2;

		public Vector3 PixelSize => active.PixelSize;

		public Coord Coord => new Coord(active.rect.offset.x / active.rect.size.x, active.rect.offset.z / active.rect.size.z);

		public override string ToString()
		{
			return "Active:(" + active.ToString() + "), Full:(" + full.ToString() + ")";
		}

		private static CoordRect WorldToPixels(Vector3 worldPos, Vector3 worldSize, int resolution)
		{
			Vector3 vector = 1f * worldSize / resolution;
			return new CoordRect(new Coord(Mathf.FloorToInt(worldPos.x / vector.x), Mathf.FloorToInt(worldPos.z / vector.x)), new Coord(resolution, resolution));
		}

		private static Dimensions GetFullDimensions(Dimensions active, int margins)
		{
			Vector3 pixelSize = active.PixelSize;
			return new Dimensions(new CoordRect(new Coord(active.rect.offset.x - margins, active.rect.offset.z - margins), new Coord(active.rect.size.x + margins * 2, active.rect.size.z + margins * 2)), new Vector2D(active.worldPos.x - (float)margins * pixelSize.x, active.worldPos.z - (float)margins * pixelSize.z), new Vector2D(active.worldSize.x + (float)margins * pixelSize.x * 2f, active.worldSize.z + (float)margins * pixelSize.z * 2f));
		}

		private static Dimensions GetActiveDimensions(Dimensions full, int margins)
		{
			Vector3 pixelSize = full.PixelSize;
			return new Dimensions(new CoordRect(full.rect.offset.x + margins, full.rect.offset.z + margins, full.rect.size.x - margins * 2, full.rect.size.z - margins * 2), new Vector2D(full.worldPos.x + (float)margins * pixelSize.x, full.worldPos.z + (float)margins * pixelSize.z), new Vector2D(full.worldSize.x - (float)margins * pixelSize.x * 2f, full.worldSize.z - (float)margins * pixelSize.z * 2f));
		}

		public Area()
		{
		}

		public Area(Vector2D activeWorldPos, Vector2D activeWorldSize, CoordRect activePixelRect, int margins)
		{
			active = new Dimensions(activePixelRect, activeWorldPos, activeWorldSize);
			full = GetFullDimensions(active, margins);
		}

		public Area(CoordRect activeWorldRect, CoordRect activePixelRect, int margins)
		{
			active = new Dimensions(activePixelRect, activeWorldRect.offset.vector2d, activeWorldRect.size.vector2d);
			full = GetFullDimensions(active, margins);
		}

		public Area(Vector2D activeWorldPos, Vector2D activeWorldSize, int activeResolution, int margins)
		{
			active = new Dimensions(WorldToPixels((Vector3)activeWorldPos, (Vector3)activeWorldSize, activeResolution), activeWorldPos, activeWorldSize);
			full = GetFullDimensions(active, margins);
		}

		public Area(Terrain terrain, int margins = 2)
		{
			Vector2D vector2D = (Vector2D)terrain.transform.position;
			Vector2D vector2D2 = (Vector2D)terrain.terrainData.size;
			active = new Dimensions(WorldToPixels((Vector3)vector2D, (Vector3)vector2D2, terrain.terrainData.heightmapResolution - 1), vector2D, vector2D2);
			full = GetFullDimensions(active, margins);
		}

		public Area(Coord coord, int activeResolution, int margins, Vector2D tileSize)
		{
			active = new Dimensions(new CoordRect(coord.x * activeResolution, coord.z * activeResolution, activeResolution, activeResolution), new Vector2D((float)coord.x * tileSize.x, (float)coord.z * tileSize.z), new Vector2D(tileSize.x, tileSize.z));
			full = GetFullDimensions(active, margins);
		}

		public Area(Area src)
		{
			active = new Dimensions(src.active.rect, src.active.worldPos, src.active.worldSize);
			full = new Dimensions(src.full.rect, src.full.worldPos, src.full.worldSize);
		}

		public static Area FromActive(Vector2D activeWorldPos, Vector2D activeWorldSize, int activeResolution, int margins)
		{
			Area obj = new Area
			{
				active = new Dimensions(WorldToPixels((Vector3)activeWorldPos, (Vector3)activeWorldSize, activeResolution), activeWorldPos, activeWorldSize)
			};
			obj.full = GetFullDimensions(obj.active, margins);
			return obj;
		}

		public static Area FromFull(Vector2D fullWorldPos, Vector2D fullWorldSize, int fullResolution, int margins)
		{
			Area obj = new Area
			{
				full = new Dimensions(WorldToPixels((Vector3)fullWorldPos, (Vector3)fullWorldSize, fullResolution), fullWorldPos, fullWorldSize)
			};
			obj.active = GetActiveDimensions(obj.full, margins);
			return obj;
		}

		public void MoveTo(Vector2D activeWorldPos)
		{
			active = new Dimensions(WorldToPixels((Vector3)activeWorldPos, (Vector3)active.worldSize, active.rect.size.x), activeWorldPos, active.worldSize);
			full = GetFullDimensions(active, Margins);
		}

		public void MoveTo(Coord coord, int activeResolution, Vector2D activeWorldPos)
		{
			active.rect = new CoordRect(coord.x * activeResolution, coord.z * activeResolution, activeResolution, activeResolution);
			active.worldPos = new Vector2D((float)coord.x * activeWorldPos.x, (float)coord.z * activeWorldPos.z);
			active.worldSize = activeWorldPos;
			full = GetFullDimensions(active, Margins);
		}
	}
}
