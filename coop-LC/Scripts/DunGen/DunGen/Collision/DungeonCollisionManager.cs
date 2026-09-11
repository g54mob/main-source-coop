using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace DunGen.Collision
{
	public class DungeonCollisionManager
	{
		private static readonly ProfilerMarker initPerfMarker = new ProfilerMarker("DungeonCollisionManager.Initialize");

		private static readonly ProfilerMarker preCachePerfMarker = new ProfilerMarker("DungeonCollisionManager.PreCacheCounds");

		private static readonly ProfilerMarker addTilePerMarker = new ProfilerMarker("DungeonCollisionManager.AddTile");

		private static readonly ProfilerMarker removeTilePerfMarker = new ProfilerMarker("DungeonCollisionManager.RemoveTile");

		private static readonly ProfilerMarker collisionBroadPhasePerfMarker = new ProfilerMarker("DungeonCollisionManager.BroadPhase");

		private static readonly ProfilerMarker collisionNarrowPhasePerfMarker = new ProfilerMarker("DungeonCollisionManager.NarrowPhase");

		private readonly List<Bounds> cachedBounds = new List<Bounds>();

		private readonly List<TileProxy> tiles = new List<TileProxy>();

		private List<Bounds> boundsToCheck = new List<Bounds>();

		public DungeonCollisionSettings Settings { get; set; }

		public ICollisionBroadphase Broadphase { get; private set; }

		public virtual void Initialize(DungeonGenerator dungeonGenerator)
		{
			using (initPerfMarker.Auto())
			{
				Clear();
				PreCacheBounds(dungeonGenerator);
				InitializeBroadphase(dungeonGenerator);
			}
		}

		protected virtual void Clear()
		{
			tiles.Clear();
			cachedBounds.Clear();
			boundsToCheck.Clear();
		}

		protected virtual void PreCacheBounds(DungeonGenerator dungeonGenerator)
		{
			using (preCachePerfMarker.Auto())
			{
				cachedBounds.Clear();
				if (Settings.AvoidCollisionsWithOtherDungeons || dungeonGenerator.AttachmentSettings != null)
				{
					Tile[] array = UnityUtil.FindObjectsByType<Tile>();
					foreach (Tile tile in array)
					{
						cachedBounds.Add(tile.Placement.Bounds);
					}
				}
				foreach (Bounds additionalCollisionBound in Settings.AdditionalCollisionBounds)
				{
					cachedBounds.Add(additionalCollisionBound);
				}
			}
		}

		protected virtual void InitializeBroadphase(DungeonGenerator dungeonGenerator)
		{
			BroadphaseSettings broadphaseSettings = DunGenSettings.Instance.BroadphaseSettings;
			if (broadphaseSettings == null)
			{
				Broadphase = null;
				return;
			}
			Broadphase = broadphaseSettings.Create();
			Broadphase.Init(broadphaseSettings, dungeonGenerator);
			foreach (Bounds cachedBound in cachedBounds)
			{
				Broadphase.Insert(cachedBound);
			}
		}

		public virtual void AddTile(TileProxy tile)
		{
			using (addTilePerMarker.Auto())
			{
				tiles.Add(tile);
				Broadphase?.Insert(tile.Placement.Bounds);
			}
		}

		public virtual void RemoveTile(TileProxy tile)
		{
			using (removeTilePerfMarker.Auto())
			{
				tiles.Remove(tile);
				Broadphase?.Remove(tile.Placement.Bounds);
			}
		}

		public virtual bool IsCollidingWithAnyTile(AxisDirection upDirection, TileProxy prospectiveNewTile, TileProxy previousTile)
		{
			bool flag = false;
			using (collisionBroadPhasePerfMarker.Auto())
			{
				UpdateBoundsToCheck(prospectiveNewTile, previousTile);
			}
			using (collisionNarrowPhasePerfMarker.Auto())
			{
				for (int i = 0; i < boundsToCheck.Count; i++)
				{
					Bounds boundsB = boundsToCheck[i];
					bool flag2 = previousTile != null && i == 0;
					float maxOverlap = (flag2 ? Settings.OverlapThreshold : (0f - Settings.Padding));
					if (Settings.DisallowOverhangs && !flag2)
					{
						if (UnityUtil.AreBoundsOverlappingOrOverhanging(prospectiveNewTile.Placement.Bounds, boundsB, upDirection, maxOverlap))
						{
							flag = true;
							break;
						}
					}
					else if (UnityUtil.AreBoundsOverlapping(prospectiveNewTile.Placement.Bounds, boundsB, maxOverlap))
					{
						flag = true;
						break;
					}
				}
			}
			if (Settings.AdditionalCollisionsPredicate != null)
			{
				flag = Settings.AdditionalCollisionsPredicate(prospectiveNewTile.Placement.Bounds, flag);
			}
			return flag;
		}

		protected virtual void UpdateBoundsToCheck(TileProxy prospectiveNewTile, TileProxy previousTile)
		{
			boundsToCheck.Clear();
			if (Broadphase != null)
			{
				Broadphase.Query(prospectiveNewTile.Placement.Bounds, ref boundsToCheck);
				if (previousTile != null)
				{
					Bounds previousBounds = previousTile.Placement.Bounds;
					int num = boundsToCheck.FindIndex((Bounds b) => b.Equals(previousBounds));
					if (num != -1)
					{
						boundsToCheck.RemoveAt(num);
						boundsToCheck.Insert(0, previousBounds);
					}
					else
					{
						boundsToCheck.Insert(0, previousBounds);
					}
				}
				return;
			}
			if (previousTile != null)
			{
				boundsToCheck.Add(previousTile.Placement.Bounds);
			}
			foreach (TileProxy tile in tiles)
			{
				if (tile != previousTile)
				{
					boundsToCheck.Add(tile.Placement.Bounds);
				}
			}
			boundsToCheck.AddRange(cachedBounds);
		}
	}
}
