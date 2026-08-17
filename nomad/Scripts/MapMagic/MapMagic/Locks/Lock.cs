using System;
using System.Collections.Generic;
using Den.Tools;
using MapMagic.Core;
using MapMagic.Nodes;
using MapMagic.Products;
using MapMagic.Terrains;
using UnityEngine;

namespace MapMagic.Locks
{
	[Serializable]
	public class Lock
	{
		public bool locked;

		public Vector3 worldPos;

		public float worldRadius = 100f;

		public float worldTransition = 20f;

		public bool rescaleDraft = true;

		public bool relativeHeight;

		public string guiName = "Location";

		public bool guiExpanded = true;

		public float guiHeight;

		private static Dictionary<TileData, Dictionary<Lock, LockDataSet>> lockDatas = new Dictionary<TileData, Dictionary<Lock, LockDataSet>>();

		private static Dictionary<Lock, LockDataSet> mainDatasDict;

		private static Dictionary<Lock, LockDataSet> draftDatasDict;

		[RuntimeInitializeOnLoadMethod]
		private static void Subscribe()
		{
			TerrainTile.OnBeforeTilePrepare = (Action<TerrainTile, TileData>)Delegate.Combine(TerrainTile.OnBeforeTilePrepare, new Action<TerrainTile, TileData>(OnTilePrepare_ReadLocks));
			TerrainTile.OnBeforeTileGenerate = (Action<TerrainTile, TileData, StopToken>)Delegate.Combine(TerrainTile.OnBeforeTileGenerate, new Action<TerrainTile, TileData, StopToken>(OnGenerateStarted_ResizeDrafts));
			Graph.OnOutputFinalized = (Action<Type, TileData, IApplyData, StopToken>)Delegate.Combine(Graph.OnOutputFinalized, new Action<Type, TileData, IApplyData, StopToken>(OnOutputFinalized_WriteLocksInThread));
			TerrainTile.OnAllComplete = (Action<MapMagicObject>)Delegate.Combine(TerrainTile.OnAllComplete, new Action<MapMagicObject>(OnAllComplete_FlushAllLocks));
			TerrainTile.OnBeforeResetTerrain = (Action<TerrainTile>)Delegate.Combine(TerrainTile.OnBeforeResetTerrain, new Action<TerrainTile>(OnTerrainReset_ReadLocks));
			TerrainTile.OnAfterResetTerrain = (Action<TerrainTile>)Delegate.Combine(TerrainTile.OnAfterResetTerrain, new Action<TerrainTile>(OnTerrainReset_WriteLocks));
		}

		public static void OnTilePrepare_ReadLocks(TerrainTile tile, TileData tileData)
		{
			List<Lock> list = null;
			Lock[] locks = tile.mapMagic.locks;
			for (int i = 0; i < locks.Length; i++)
			{
				if (locks[i].locked && locks[i].IsIntersecting(tileData.area.active) && (!tileData.isDraft || locks[i].rescaleDraft))
				{
					if (list == null)
					{
						list = new List<Lock>();
					}
					list.Add(locks[i]);
				}
			}
			if (list == null)
			{
				return;
			}
			if (!lockDatas.TryGetValue(tileData, out var value))
			{
				value = new Dictionary<Lock, LockDataSet>();
				lockDatas.Add(tileData, value);
			}
			Terrain terrain = tile.GetTerrain(tileData.isDraft);
			int count = list.Count;
			for (int j = 0; j < count; j++)
			{
				Lock obj = list[j];
				if (!value.ContainsKey(obj))
				{
					LockDataSet lockDataSet = new LockDataSet();
					lockDataSet.Read(terrain, obj);
					value.Add(obj, lockDataSet);
				}
			}
		}

		public static void OnGenerateStarted_ResizeDrafts(TerrainTile tile, TileData draftTileData, StopToken stop)
		{
			if (!draftTileData.isDraft)
			{
				return;
			}
			TileData tileData = tile.main?.data;
			if (tileData == null || !lockDatas.TryGetValue(draftTileData, out var value) || !lockDatas.TryGetValue(tileData, out var value2))
			{
				return;
			}
			foreach (KeyValuePair<Lock, LockDataSet> item in value)
			{
				Lock key = item.Key;
				if (key.rescaleDraft)
				{
					LockDataSet value3 = item.Value;
					if (value2.TryGetValue(key, out var value4))
					{
						LockDataSet.Resize(value4, value3);
					}
				}
			}
		}

		public static void OnOutputFinalized_WriteLocksInThread(Type type, TileData tileData, IApplyData applyData, StopToken stop)
		{
			if (!lockDatas.TryGetValue(tileData, out var value))
			{
				return;
			}
			foreach (KeyValuePair<Lock, LockDataSet> item in value)
			{
				Lock key = item.Key;
				LockDataSet value2 = item.Value;
				if (key.locked)
				{
					bool flag = key.relativeHeight;
					if (key.IsIntersecting(tileData.area.active) && !key.IsContained(tileData.area.active))
					{
						flag = false;
					}
					value2.WriteInThread(applyData, flag);
				}
			}
		}

		public static void OnTileApplied_WriteLocksInApply(TerrainTile tile, TileData tileData, StopToken stop)
		{
			if (!lockDatas.TryGetValue(tileData, out var value))
			{
				return;
			}
			Terrain terrain = tile.GetTerrain(tileData.isDraft);
			foreach (LockDataSet value2 in value.Values)
			{
				value2.WriteInApply(terrain, resizeTerrain: false);
			}
			if (!tileData.isDraft)
			{
				lockDatas.Remove(tileData);
			}
		}

		public static void OnAllComplete_FlushAllLocks(MapMagicObject mapMagic)
		{
			lockDatas.Clear();
		}

		public static void OnTerrainReset_ReadLocks(TerrainTile tile)
		{
			List<Lock> list = null;
			Lock[] locks = tile.mapMagic.locks;
			for (int i = 0; i < locks.Length; i++)
			{
				if (locks[i].locked && locks[i].IsIntersecting(tile.WorldRect))
				{
					if (list == null)
					{
						list = new List<Lock>();
					}
					list.Add(locks[i]);
				}
			}
			if (list == null)
			{
				return;
			}
			if (tile.main != null)
			{
				mainDatasDict = new Dictionary<Lock, LockDataSet>();
			}
			if (tile.draft != null)
			{
				draftDatasDict = new Dictionary<Lock, LockDataSet>();
			}
			foreach (Lock item in list)
			{
				if (tile.main != null)
				{
					LockDataSet lockDataSet = new LockDataSet();
					lockDataSet.Read(tile.main.terrain, item);
					mainDatasDict.Add(item, lockDataSet);
				}
				if (tile.draft != null)
				{
					LockDataSet lockDataSet2 = new LockDataSet();
					lockDataSet2.Read(tile.draft.terrain, item);
					draftDatasDict.Add(item, lockDataSet2);
				}
			}
		}

		public static void OnTerrainReset_WriteLocks(TerrainTile tile)
		{
			if (tile.main != null && mainDatasDict != null)
			{
				foreach (LockDataSet value in mainDatasDict.Values)
				{
					value.WriteInApply(tile.main.terrain, resizeTerrain: true);
				}
			}
			if (tile.draft != null && draftDatasDict != null)
			{
				foreach (LockDataSet value2 in draftDatasDict.Values)
				{
					value2.WriteInApply(tile.draft.terrain, resizeTerrain: true);
				}
			}
			mainDatasDict = null;
			draftDatasDict = null;
		}

		public bool IsIntersecting(Terrain terrain)
		{
			float num = worldRadius + worldTransition;
			Vector3 localPosition = terrain.transform.localPosition;
			Vector3 size = terrain.terrainData.size;
			if (localPosition.x > worldPos.x + num || localPosition.x + size.x < worldPos.x - num || localPosition.z > worldPos.z + num || localPosition.z + size.z < worldPos.z - num)
			{
				return false;
			}
			return true;
		}

		public bool IsIntersecting(Area.Dimensions dim)
		{
			float num = worldRadius + worldTransition;
			if (dim.worldPos.x > worldPos.x + num || dim.worldPos.x + dim.worldSize.x < worldPos.x - num || dim.worldPos.z > worldPos.z + num || dim.worldPos.z + dim.worldSize.z < worldPos.z - num)
			{
				return false;
			}
			return true;
		}

		public bool IsIntersecting(Rect rect)
		{
			Vector2 min = rect.min;
			Vector2 max = rect.max;
			float num = worldRadius + worldTransition;
			if (worldPos.x + num < min.x || worldPos.x - num > max.x || worldPos.z + num < min.y || worldPos.z - num > max.y)
			{
				return false;
			}
			return true;
		}

		public bool IsContained(Area.Dimensions dim)
		{
			float num = worldRadius + worldTransition;
			if (worldPos.x - num > dim.worldPos.x && worldPos.x + num < dim.worldPos.x + dim.worldSize.x && worldPos.z - num > dim.worldPos.z && worldPos.z + num < dim.worldPos.z + dim.worldSize.z)
			{
				return true;
			}
			return false;
		}

		public bool IsContained(Rect rect)
		{
			Vector2 min = rect.min;
			Vector2 max = rect.max;
			float num = worldRadius + worldTransition;
			if (worldPos.x - num > min.x && worldPos.x + num < max.x && worldPos.z - num > min.y && worldPos.z + num < max.y)
			{
				return true;
			}
			return false;
		}

		public bool IsContainedInAll(IEnumerable<Rect> rects)
		{
			CoordRect c = new CoordRect(new Coord((int)(worldPos.x + 0.5f), (int)(worldPos.z + 0.5f)), (int)(worldRadius + worldTransition));
			int num = c.size.x * c.size.z;
			foreach (Rect rect in rects)
			{
				CoordRect coordRect = CoordRect.Intersected(c2: new CoordRect((int)(rect.position.x + 0.5f), (int)(rect.position.y + 0.5f), (int)(rect.size.x + 0.5f), (int)(rect.size.y + 0.5f)), c1: c);
				num -= coordRect.size.x * coordRect.size.z;
			}
			return num <= 0;
		}

		public bool IsContainedInAny(IEnumerable<Rect> rects)
		{
			bool result = false;
			foreach (Rect rect in rects)
			{
				bool flag = IsContained(rect);
				if (IsIntersecting(rect) && !flag)
				{
					return false;
				}
				if (flag)
				{
					result = true;
				}
			}
			return result;
		}
	}
}
