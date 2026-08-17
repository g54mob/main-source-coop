using System;
using System.Collections.Generic;
using Den.Tools;
using MapMagic.Terrains;
using UnityEngine;

namespace MapMagic.Core
{
	[Serializable]
	public class TerrainTileManager : TileManager<TerrainTile>, ISerializationCallbackReceiver
	{
		[SerializeField]
		public TerrainTile[] customTiles = new TerrainTile[0];

		public Dictionary<Coord, TerrainTile> pinned = new Dictionary<Coord, TerrainTile>();

		public Coord[] serializedPinnedCoords = new Coord[0];

		public void Pin(Coord coord, bool asDraft, MonoBehaviour holder = null)
		{
			grid.TryGetValue(coord, out var value);
			if (value == null)
			{
				value = ConstructTile(holder);
				grid.Add(coord, value);
			}
			else
			{
				value.Pin(asDraft);
			}
			value.Pin(asDraft);
			value.Move(coord, (camCoords != null) ? TileManager<TerrainTile>.GetRemoteness(coord, camCoords) : 0f);
			if (!pinned.ContainsKey(coord))
			{
				pinned.Add(coord, value);
			}
		}

		public void Unpin(Coord coord)
		{
			if (pinned.ContainsKey(coord))
			{
				pinned.Remove(coord);
				grid[coord].Remove();
				grid.Remove(coord);
			}
		}

		public void Deploy(Coord[] camCoords, MonoBehaviour holder = null)
		{
			Deploy(camCoords, pinned, holder);
		}

		public IEnumerable<TerrainTile> All()
		{
			foreach (TerrainTile item in Tiles())
			{
				yield return item;
			}
			for (int i = 0; i < customTiles.Length; i++)
			{
				yield return customTiles[i];
			}
		}

		public IEnumerable<Rect> AllWorldRects()
		{
			foreach (TerrainTile item in All())
			{
				yield return item.WorldRect;
			}
		}

		public IEnumerable<Terrain> AllActiveTerrains()
		{
			foreach (TerrainTile item in All())
			{
				yield return item.ActiveTerrain;
			}
		}

		public void PinCustom(TerrainTile tile)
		{
			if (!customTiles.Contains(tile))
			{
				ArrayTools.Add(ref customTiles, tile);
			}
		}

		public void UnpinCustom(TerrainTile tile)
		{
			if (customTiles.Contains(tile))
			{
				ArrayTools.Remove(ref customTiles, tile);
			}
		}

		public override void RemoveNulls()
		{
			base.RemoveNulls();
			for (int num = customTiles.Length - 1; num >= 0; num--)
			{
				if (customTiles[num] == null || customTiles[num].IsNull)
				{
					ArrayTools.RemoveAt(ref customTiles, num);
				}
			}
		}

		public override TerrainTile Closest()
		{
			float num = 2.1474836E+09f;
			TerrainTile result = null;
			foreach (KeyValuePair<Coord, TerrainTile> item in grid)
			{
				TerrainTile value = item.Value;
				if (value.distance < num)
				{
					num = value.distance;
					result = item.Value;
				}
			}
			return result;
		}

		public TerrainTile ClosestMain()
		{
			float num = 2.1474836E+09f;
			TerrainTile result = null;
			foreach (KeyValuePair<Coord, TerrainTile> item in grid)
			{
				TerrainTile value = item.Value;
				if (value.main != null && value.distance < num)
				{
					num = value.distance;
					result = item.Value;
				}
			}
			return result;
		}

		public TerrainTile FindByWorldPosition(float x, float z)
		{
			foreach (TerrainTile item in All())
			{
				if (item.ContainsWorldPosition(x, z))
				{
					return item;
				}
			}
			return null;
		}

		public TerrainTile FindByTerrain(Terrain terrain)
		{
			foreach (TerrainTile item in All())
			{
				if (item.main?.terrain == terrain)
				{
					return item;
				}
				if (item.draft?.terrain == terrain)
				{
					return item;
				}
			}
			return null;
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();
			if (serializedPinnedCoords.Length != pinned.Count)
			{
				serializedPinnedCoords = new Coord[pinned.Count];
			}
			int num = 0;
			foreach (KeyValuePair<Coord, TerrainTile> item in pinned)
			{
				serializedPinnedCoords[num] = item.Key;
				num++;
			}
		}

		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();
			for (int i = 0; i < serializedPinnedCoords.Length; i++)
			{
				Coord key = serializedPinnedCoords[i];
				if (grid.TryGetValue(key, out var value) && !pinned.ContainsKey(key))
				{
					pinned.Add(key, value);
				}
			}
		}
	}
}
