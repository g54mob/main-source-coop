using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DunGen.Generation;
using DunGen.Graph;
using DunGen.Tags;
using UnityEngine;

namespace DunGen
{
	public class Dungeon : MonoBehaviour
	{
		public sealed class Branch
		{
			public int Index { get; }

			public ReadOnlyCollection<Tile> Tiles { get; }

			public Branch(int index, List<Tile> tiles)
			{
				Index = index;
				Tiles = new ReadOnlyCollection<Tile>(tiles);
			}
		}

		public bool DebugRender;

		[SerializeField]
		private DungeonFlow dungeonFlow;

		[SerializeField]
		private List<Tile> allTiles = new List<Tile>();

		[SerializeField]
		private List<Tile> mainPathTiles = new List<Tile>();

		[SerializeField]
		private List<Tile> branchPathTiles = new List<Tile>();

		[SerializeField]
		private List<GameObject> doors = new List<GameObject>();

		[SerializeField]
		private List<DoorwayConnection> connections = new List<DoorwayConnection>();

		[SerializeField]
		private Tile attachmentTile;

		[SerializeField]
		private List<Branch> branches = new List<Branch>();

		public Bounds Bounds { get; protected set; }

		public DungeonFlow DungeonFlow
		{
			get
			{
				return dungeonFlow;
			}
			set
			{
				dungeonFlow = value;
			}
		}

		public ReadOnlyCollection<Tile> AllTiles { get; }

		public ReadOnlyCollection<Tile> MainPathTiles { get; }

		public ReadOnlyCollection<Tile> BranchPathTiles { get; }

		public ReadOnlyCollection<GameObject> Doors { get; }

		public ReadOnlyCollection<DoorwayConnection> Connections { get; }

		public ReadOnlyCollection<Branch> Branches { get; }

		public DungeonGraph ConnectionGraph { get; private set; }

		public TileInstanceSource TileInstanceSource { get; internal set; }

		public static event DungeonTileInstantiatedDelegate TileInstantiated;

		public Dungeon()
		{
			AllTiles = new ReadOnlyCollection<Tile>(allTiles);
			MainPathTiles = new ReadOnlyCollection<Tile>(mainPathTiles);
			BranchPathTiles = new ReadOnlyCollection<Tile>(branchPathTiles);
			Doors = new ReadOnlyCollection<GameObject>(doors);
			Connections = new ReadOnlyCollection<DoorwayConnection>(connections);
			Branches = new ReadOnlyCollection<Branch>(branches);
		}

		private void Start()
		{
			if (allTiles.Count > 0 && ConnectionGraph == null)
			{
				FinaliseDungeonInfo();
			}
		}

		public IEnumerable<Tile> FindTilesWithTag(Tag tag)
		{
			return allTiles.Where((Tile t) => t.Tags.HasTag(tag));
		}

		public IEnumerable<Tile> FindTilesWithAnyTag(params Tag[] tags)
		{
			return allTiles.Where((Tile t) => t.Tags.HasAnyTag(tags));
		}

		public IEnumerable<Tile> FindTilesWithAllTags(params Tag[] tags)
		{
			return allTiles.Where((Tile t) => t.Tags.HasAllTags(tags));
		}

		internal void AddAdditionalDoor(Door door)
		{
			if (door != null && !doors.Contains(door.gameObject))
			{
				doors.Add(door.gameObject);
			}
		}

		internal void PreGenerateDungeon(DungeonGenerator dungeonGenerator)
		{
			DungeonFlow = dungeonGenerator.DungeonFlow;
		}

		internal void PostGenerateDungeon(DungeonGenerator dungeonGenerator)
		{
			FinaliseDungeonInfo();
		}

		private void FinaliseDungeonInfo()
		{
			List<Tile> list = new List<Tile>();
			if (attachmentTile != null)
			{
				list.Add(attachmentTile);
			}
			ConnectionGraph = new DungeonGraph(this, list);
			Bounds = UnityUtil.CombineBounds(allTiles.Select((Tile x) => x.Placement.Bounds).ToArray());
			GatherBranches();
		}

		private void GatherBranches()
		{
			Dictionary<int, List<Tile>> dictionary = new Dictionary<int, List<Tile>>();
			foreach (Tile branchPathTile in branchPathTiles)
			{
				int branchId = branchPathTile.Placement.BranchId;
				if (!dictionary.TryGetValue(branchId, out var value))
				{
					value = (dictionary[branchId] = new List<Tile>());
				}
				value.Add(branchPathTile);
			}
			foreach (KeyValuePair<int, List<Tile>> item in dictionary)
			{
				int key = item.Key;
				List<Tile> value2 = item.Value;
				branches.Add(new Branch(key, value2));
			}
		}

		public void Clear()
		{
			Clear(TileInstanceSource.DespawnTile);
		}

		public void Clear(Action<Tile> destroyTileDelegate)
		{
			foreach (Tile allTile in allTiles)
			{
				destroyTileDelegate(allTile);
			}
			for (int i = 0; i < base.transform.childCount; i++)
			{
				UnityUtil.Destroy(base.transform.GetChild(i).gameObject);
			}
			allTiles.Clear();
			mainPathTiles.Clear();
			branchPathTiles.Clear();
			doors.Clear();
			connections.Clear();
			branches.Clear();
			attachmentTile = null;
		}

		public Doorway GetConnectedDoorway(Doorway doorway)
		{
			foreach (DoorwayConnection connection in connections)
			{
				if (connection.A == doorway)
				{
					return connection.B;
				}
				if (connection.B == doorway)
				{
					return connection.A;
				}
			}
			return null;
		}

		public IEnumerator FromProxy(DungeonProxy proxyDungeon, DungeonGenerator generator, Func<bool> shouldSkipFrame)
		{
			Clear();
			Dictionary<TileProxy, Tile> proxyToTileMap = new Dictionary<TileProxy, Tile>();
			if (generator.AttachmentSettings != null && generator.AttachmentSettings.TileProxy != null)
			{
				TileProxy tileProxy = generator.AttachmentSettings.TileProxy;
				attachmentTile = generator.AttachmentSettings.GetAttachmentTile();
				proxyToTileMap[tileProxy] = attachmentTile;
				DoorwayProxy doorwayProxy = tileProxy.UsedDoorways.First();
				Doorway doorway = attachmentTile.AllDoorways[doorwayProxy.Index];
				doorway.ProcessDoorwayObjects(isDoorwayInUse: true, generator.RandomStream);
				attachmentTile.UsedDoorways.Add(doorway);
				attachmentTile.UnusedDoorways.Remove(doorway);
			}
			foreach (TileProxy allTile in proxyDungeon.AllTiles)
			{
				Tile tile = TileInstanceSource.SpawnTile(allTile.PrefabTile, allTile.Placement.Position, allTile.Placement.Rotation);
				Tile component = tile.GetComponent<Tile>();
				component.Dungeon = this;
				component.Placement = new TilePlacementData(allTile.Placement);
				component.Prefab = allTile.Prefab;
				proxyToTileMap[allTile] = component;
				allTiles.Add(component);
				component.Placement.SetPositionAndRotation(tile.transform.position, tile.transform.rotation);
				if (component.Placement.IsOnMainPath)
				{
					mainPathTiles.Add(component);
				}
				else
				{
					branchPathTiles.Add(component);
				}
				if (generator.TriggerPlacement != TriggerPlacementMode.None)
				{
					component.AddTriggerVolume(generator.TriggerPlacement == TriggerPlacementMode.TwoDimensional);
					component.gameObject.layer = generator.TileTriggerLayer;
				}
				Doorway[] componentsInChildren = tile.GetComponentsInChildren<Doorway>();
				Doorway[] array = componentsInChildren;
				foreach (Doorway doorway2 in array)
				{
					if (!component.AllDoorways.Contains(doorway2))
					{
						doorway2.Tile = component;
						doorway2.placedByGenerator = true;
						doorway2.HideConditionalObjects = false;
						component.AllDoorways.Add(doorway2);
					}
				}
				foreach (DoorwayProxy usedDoorway in allTile.UsedDoorways)
				{
					Doorway doorway3 = componentsInChildren[usedDoorway.Index];
					component.UsedDoorways.Add(doorway3);
					doorway3.ProcessDoorwayObjects(isDoorwayInUse: true, generator.RandomStream);
				}
				foreach (DoorwayProxy unusedDoorway in allTile.UnusedDoorways)
				{
					Doorway doorway4 = componentsInChildren[unusedDoorway.Index];
					component.UnusedDoorways.Add(doorway4);
					doorway4.ProcessDoorwayObjects(isDoorwayInUse: false, generator.RandomStream);
				}
				Dungeon.TileInstantiated?.Invoke(this, component, allTiles.Count, proxyDungeon.AllTiles.Count);
				if (shouldSkipFrame != null && shouldSkipFrame())
				{
					yield return null;
				}
			}
			foreach (ProxyDoorwayConnection connection in proxyDungeon.Connections)
			{
				Tile tile2 = proxyToTileMap[connection.A.TileProxy];
				Tile tile3 = proxyToTileMap[connection.B.TileProxy];
				Doorway doorway5 = tile2.AllDoorways[connection.A.Index];
				Doorway doorway6 = (doorway5.ConnectedDoorway = tile3.AllDoorways[connection.B.Index]);
				doorway6.ConnectedDoorway = doorway5;
				DoorwayConnection item = new DoorwayConnection(doorway5, doorway6);
				connections.Add(item);
				SpawnDoorPrefab(doorway5, doorway6, generator.RandomStream);
			}
		}

		private void SpawnDoorPrefab(Doorway a, Doorway b, RandomStream randomStream)
		{
			if (a.HasDoorPrefabInstance || b.HasDoorPrefabInstance)
			{
				return;
			}
			bool flag = a.ConnectorPrefabWeights.HasAnyViableEntries();
			bool flag2 = b.ConnectorPrefabWeights.HasAnyViableEntries();
			if (!flag && !flag2)
			{
				return;
			}
			Doorway doorway = ((!(flag && flag2)) ? (flag ? a : b) : ((a.DoorPrefabPriority < b.DoorPrefabPriority) ? b : a));
			GameObject random = doorway.ConnectorPrefabWeights.GetRandom(randomStream);
			if (random != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(random, doorway.transform);
				gameObject.transform.localPosition = doorway.DoorPrefabPositionOffset;
				if (doorway.AvoidRotatingDoorPrefab)
				{
					gameObject.transform.rotation = Quaternion.Euler(doorway.DoorPrefabRotationOffset);
				}
				else
				{
					gameObject.transform.localRotation = Quaternion.Euler(doorway.DoorPrefabRotationOffset);
				}
				doors.Add(gameObject);
				DungeonUtil.AddAndSetupDoorComponent(this, gameObject, doorway);
				a.SetUsedPrefab(gameObject);
				b.SetUsedPrefab(gameObject);
			}
		}

		public void OnDrawGizmos()
		{
			if (DebugRender)
			{
				DebugDraw();
			}
		}

		public void DebugDraw()
		{
			Color red = Color.red;
			Color green = Color.green;
			Color blue = Color.blue;
			Color b = new Color(0.5f, 0f, 0.5f);
			float a = 0.75f;
			foreach (Tile allTile in allTiles)
			{
				Bounds bounds = allTile.Placement.Bounds;
				bounds.size *= 1.01f;
				Color color = (allTile.Placement.IsOnMainPath ? Color.Lerp(red, green, allTile.Placement.NormalizedDepth) : Color.Lerp(blue, b, allTile.Placement.NormalizedDepth));
				color.a = a;
				Gizmos.color = color;
				Gizmos.DrawCube(bounds.center, bounds.size);
			}
		}
	}
}
