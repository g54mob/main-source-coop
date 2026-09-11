using System.Collections.Generic;
using UnityEngine;

namespace DunGen.Collision
{
	public class SpatialHashBroadphase : ICollisionBroadphase
	{
		public SpatialHashGrid<Bounds> SpatialHashGrid { get; private set; }

		public void Init(BroadphaseSettings settings, DungeonGenerator dungeonGenerator)
		{
			if (settings is SpatialHashBroadphaseSettings spatialHashBroadphaseSettings)
			{
				SpatialHashGrid = new SpatialHashGrid<Bounds>(spatialHashBroadphaseSettings.CellSize, (Bounds b) => b, dungeonGenerator.UpDirection);
			}
		}

		public void Insert(Bounds bounds)
		{
			SpatialHashGrid.Insert(bounds);
		}

		public void Query(Bounds bounds, ref List<Bounds> results)
		{
			SpatialHashGrid.Query(bounds, ref results);
		}

		public void Remove(Bounds bounds)
		{
			SpatialHashGrid.Remove(bounds);
		}

		public void DrawDebug(float duration = 0f)
		{
			SpatialHashGrid.DrawDebug(duration);
		}
	}
}
