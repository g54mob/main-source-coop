using System.Collections.Generic;
using System.Linq;
using DunGen.Graph;
using UnityEngine;

namespace DunGen
{
	public sealed class DungeonProxy
	{
		public List<TileProxy> AllTiles = new List<TileProxy>();

		public List<TileProxy> MainPathTiles = new List<TileProxy>();

		public List<TileProxy> BranchPathTiles = new List<TileProxy>();

		public List<ProxyDoorwayConnection> Connections = new List<ProxyDoorwayConnection>();

		private Transform visualsRoot;

		private Dictionary<TileProxy, GameObject> tileVisuals = new Dictionary<TileProxy, GameObject>();

		public DungeonProxy(Transform debugVisualsRoot = null)
		{
			visualsRoot = debugVisualsRoot;
		}

		public void ClearDebugVisuals()
		{
			GameObject[] array = tileVisuals.Values.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				Object.DestroyImmediate(array[i]);
			}
			tileVisuals.Clear();
		}

		public void MakeConnection(DoorwayProxy a, DoorwayProxy b)
		{
			DoorwayProxy.Connect(a, b);
			ProxyDoorwayConnection item = new ProxyDoorwayConnection(a, b);
			Connections.Add(item);
		}

		public void RemoveLastConnection()
		{
			RemoveConnection(Connections.Last());
		}

		public void RemoveConnection(ProxyDoorwayConnection connection)
		{
			connection.A.Disconnect();
			Connections.Remove(connection);
		}

		internal void AddTile(TileProxy tile)
		{
			AllTiles.Add(tile);
			if (tile.Placement.IsOnMainPath)
			{
				MainPathTiles.Add(tile);
			}
			else
			{
				BranchPathTiles.Add(tile);
			}
			if (visualsRoot != null)
			{
				GameObject gameObject = Object.Instantiate(tile.Prefab, visualsRoot);
				gameObject.name = "DEBUG_VISUALS_" + tile.Prefab.name;
				gameObject.transform.localPosition = tile.Placement.Position;
				gameObject.transform.localRotation = tile.Placement.Rotation;
				tileVisuals[tile] = gameObject;
			}
		}

		internal void RemoveTile(TileProxy tile)
		{
			AllTiles.Remove(tile);
			if (tile.Placement.IsOnMainPath)
			{
				MainPathTiles.Remove(tile);
			}
			else
			{
				BranchPathTiles.Remove(tile);
			}
			if (tileVisuals.TryGetValue(tile, out var value))
			{
				Object.DestroyImmediate(value);
				tileVisuals.Remove(tile);
			}
		}

		internal void ConnectOverlappingDoorways(float globalChance, DungeonFlow dungeonFlow, RandomStream randomStream)
		{
			DoorwayProxy[] array = AllTiles.SelectMany((TileProxy t) => t.UnusedDoorways).ToArray();
			float num = 1f;
			Dictionary<Vector3Int, List<DoorwayProxy>> dictionary = new Dictionary<Vector3Int, List<DoorwayProxy>>();
			DoorwayProxy[] array2 = array;
			foreach (DoorwayProxy doorwayProxy in array2)
			{
				Vector3 position = doorwayProxy.Position;
				Vector3Int key = new Vector3Int(Mathf.FloorToInt(position.x / num), Mathf.FloorToInt(position.y / num), Mathf.FloorToInt(position.z / num));
				if (!dictionary.TryGetValue(key, out var value))
				{
					value = (dictionary[key] = new List<DoorwayProxy>());
				}
				value.Add(doorwayProxy);
			}
			HashSet<(DoorwayProxy, DoorwayProxy)> hashSet = new HashSet<(DoorwayProxy, DoorwayProxy)>();
			array2 = array;
			foreach (DoorwayProxy doorwayProxy2 in array2)
			{
				Vector3 position2 = doorwayProxy2.Position;
				Vector3Int vector3Int = new Vector3Int(Mathf.FloorToInt(position2.x / num), Mathf.FloorToInt(position2.y / num), Mathf.FloorToInt(position2.z / num));
				for (int num3 = -1; num3 <= 1; num3++)
				{
					for (int num4 = -1; num4 <= 1; num4++)
					{
						for (int num5 = -1; num5 <= 1; num5++)
						{
							Vector3Int key2 = vector3Int + new Vector3Int(num3, num4, num5);
							if (!dictionary.TryGetValue(key2, out var value2))
							{
								continue;
							}
							foreach (DoorwayProxy item in value2)
							{
								if (doorwayProxy2 == item || doorwayProxy2.TileProxy == item.TileProxy || doorwayProxy2.Used || item.Used || hashSet.Contains((doorwayProxy2, item)) || hashSet.Contains((item, doorwayProxy2)))
								{
									continue;
								}
								hashSet.Add((doorwayProxy2, item));
								if ((doorwayProxy2.Position - item.Position).sqrMagnitude >= 1E-05f)
								{
									continue;
								}
								ProposedConnection connection = new ProposedConnection(this, doorwayProxy2.TileProxy, item.TileProxy, doorwayProxy2, item);
								if (!dungeonFlow.CanDoorwaysConnect(connection))
								{
									continue;
								}
								if (dungeonFlow.RestrictConnectionToSameSection)
								{
									bool flag = doorwayProxy2.TileProxy.Placement.GraphLine == item.TileProxy.Placement.GraphLine;
									if (doorwayProxy2.TileProxy.Placement.GraphLine == null)
									{
										flag = false;
									}
									if (!flag)
									{
										continue;
									}
								}
								float num6 = globalChance;
								if (doorwayProxy2.TileProxy.PrefabTile.OverrideConnectionChance && item.TileProxy.PrefabTile.OverrideConnectionChance)
								{
									num6 = Mathf.Min(doorwayProxy2.TileProxy.PrefabTile.ConnectionChance, item.TileProxy.PrefabTile.ConnectionChance);
								}
								else if (doorwayProxy2.TileProxy.PrefabTile.OverrideConnectionChance)
								{
									num6 = doorwayProxy2.TileProxy.PrefabTile.ConnectionChance;
								}
								else if (item.TileProxy.PrefabTile.OverrideConnectionChance)
								{
									num6 = item.TileProxy.PrefabTile.ConnectionChance;
								}
								if (!(num6 <= 0f) && randomStream.NextDouble() < (double)num6)
								{
									MakeConnection(doorwayProxy2, item);
								}
							}
						}
					}
				}
			}
		}
	}
}
