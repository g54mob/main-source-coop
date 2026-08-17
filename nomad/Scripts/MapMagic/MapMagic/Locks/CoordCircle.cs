using Den.Tools;
using UnityEngine;

namespace MapMagic.Locks
{
	public struct CoordCircle
	{
		public Coord center;

		public int radius;

		public int transition;

		public int fullRadius;

		public CoordRect rect;

		public CoordCircle(Terrain terrain, int resolution, Vector3 worldCenter, float worldRadius, float worldTransition)
		{
			Vector3 localPosition = terrain.transform.parent.localPosition;
			Vector3 size = terrain.terrainData.size;
			center = Coord.Round((worldCenter.x - localPosition.x) / size.x * (float)resolution, (worldCenter.z - localPosition.z) / size.z * (float)resolution);
			radius = (int)(worldRadius / size.x * (float)resolution);
			transition = (int)(worldTransition / size.x * (float)resolution);
			fullRadius = radius + transition;
			rect = new CoordRect(center, radius + transition);
			rect = CoordRect.Intersected(rect, new CoordRect(0, 0, resolution, resolution));
		}
	}
}
