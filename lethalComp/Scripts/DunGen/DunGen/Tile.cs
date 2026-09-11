using System;
using System.Collections.Generic;
using System.Linq;
using DunGen.Pooling;
using DunGen.Tags;
using UnityEngine;
using UnityEngine.Serialization;

namespace DunGen
{
	[AddComponentMenu("DunGen/Tile")]
	public class Tile : MonoBehaviour, ISerializationCallbackReceiver
	{
		public const int CurrentFileVersion = 3;

		[SerializeField]
		[FormerlySerializedAs("AllowImmediateRepeats")]
		private bool allowImmediateRepeats = true;

		[SerializeField]
		[Obsolete("'Entrance' is no longer used. Please use the 'Entrances' list instead", false)]
		public Doorway Entrance;

		[SerializeField]
		[Obsolete("'Exit' is no longer used. Please use the 'Exits' list instead", false)]
		public Doorway Exit;

		public bool AllowRotation = true;

		public TileRepeatMode RepeatMode;

		public bool OverrideAutomaticTileBounds;

		public Bounds TileBoundsOverride = new Bounds(Vector3.zero, Vector3.one);

		public List<Doorway> Entrances = new List<Doorway>();

		public List<Doorway> Exits = new List<Doorway>();

		public bool OverrideConnectionChance;

		public float ConnectionChance;

		public TagContainer Tags = new TagContainer();

		public List<Doorway> AllDoorways = new List<Doorway>();

		public List<Doorway> UsedDoorways = new List<Doorway>();

		public List<Doorway> UnusedDoorways = new List<Doorway>();

		[SerializeField]
		private TilePlacementData placement;

		[SerializeField]
		private int fileVersion;

		private BoxCollider triggerVolume;

		private BoxCollider2D triggerVolume2D;

		private readonly List<ITileSpawnEventReceiver> spawnEventReceivers = new List<ITileSpawnEventReceiver>();

		[HideInInspector]
		public Bounds Bounds => base.transform.TransformBounds(Placement.LocalBounds);

		public TilePlacementData Placement
		{
			get
			{
				return placement;
			}
			internal set
			{
				placement = value;
			}
		}

		public Dungeon Dungeon { get; internal set; }

		public GameObject Prefab { get; internal set; }

		public bool HasValidBounds
		{
			get
			{
				if (Placement != null)
				{
					return Placement.LocalBounds.extents.sqrMagnitude > 0f;
				}
				return false;
			}
		}

		public void RefreshTileEventReceivers()
		{
			spawnEventReceivers.Clear();
			GetComponentsInChildren(includeInactive: true, spawnEventReceivers);
		}

		internal void TileSpawned()
		{
			foreach (ITileSpawnEventReceiver spawnEventReceiver in spawnEventReceivers)
			{
				spawnEventReceiver.OnTileSpawned(this);
			}
		}

		internal void TileDespawned()
		{
			Dungeon = null;
			foreach (Doorway allDoorway in AllDoorways)
			{
				allDoorway.ResetInstanceData();
			}
			placement.SetPositionAndRotation(Vector2.zero, Quaternion.identity);
			UsedDoorways.Clear();
			UnusedDoorways.Clear();
			foreach (ITileSpawnEventReceiver spawnEventReceiver in spawnEventReceivers)
			{
				spawnEventReceiver.OnTileDespawned(this);
			}
		}

		internal void AddTriggerVolume(bool use2dCollider)
		{
			if (use2dCollider)
			{
				if (triggerVolume2D == null)
				{
					triggerVolume2D = base.gameObject.AddComponent<BoxCollider2D>();
				}
				triggerVolume2D.offset = Placement.LocalBounds.center;
				triggerVolume2D.size = Placement.LocalBounds.size;
				triggerVolume2D.isTrigger = true;
			}
			else
			{
				if (triggerVolume == null)
				{
					triggerVolume = base.gameObject.AddComponent<BoxCollider>();
				}
				triggerVolume.center = Placement.LocalBounds.center;
				triggerVolume.size = Placement.LocalBounds.size;
				triggerVolume.isTrigger = true;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (!(other == null) && other.gameObject.TryGetComponent<DungenCharacter>(out var component))
			{
				component.OnTileEntered(this);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!(other == null) && other.gameObject.TryGetComponent<DungenCharacter>(out var component))
			{
				component.OnTileEntered(this);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!(other == null) && other.gameObject.TryGetComponent<DungenCharacter>(out var component))
			{
				component.OnTileExited(this);
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!(other == null) && other.gameObject.TryGetComponent<DungenCharacter>(out var component))
			{
				component.OnTileExited(this);
			}
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Bounds? bounds = null;
			if (OverrideAutomaticTileBounds)
			{
				bounds = base.transform.TransformBounds(TileBoundsOverride);
			}
			else if (placement != null)
			{
				bounds = Bounds;
			}
			if (bounds.HasValue)
			{
				Gizmos.DrawWireCube(bounds.Value.center, bounds.Value.size);
			}
		}

		public IEnumerable<Tile> GetAdjacentTiles()
		{
			return UsedDoorways.Select((Doorway x) => x.ConnectedDoorway.Tile).Distinct();
		}

		public bool IsAdjacentTo(Tile other)
		{
			foreach (Doorway usedDoorway in UsedDoorways)
			{
				if (usedDoorway.ConnectedDoorway.Tile == other)
				{
					return true;
				}
			}
			return false;
		}

		public Doorway GetEntranceDoorway()
		{
			foreach (Doorway usedDoorway in UsedDoorways)
			{
				Tile tile = usedDoorway.ConnectedDoorway.Tile;
				if (Placement.IsOnMainPath)
				{
					if (tile.Placement.IsOnMainPath && Placement.PathDepth > tile.Placement.PathDepth)
					{
						return usedDoorway;
					}
				}
				else if (tile.Placement.IsOnMainPath || Placement.Depth > tile.Placement.Depth)
				{
					return usedDoorway;
				}
			}
			return null;
		}

		public Doorway GetExitDoorway()
		{
			foreach (Doorway usedDoorway in UsedDoorways)
			{
				Tile tile = usedDoorway.ConnectedDoorway.Tile;
				if (Placement.IsOnMainPath)
				{
					if (tile.Placement.IsOnMainPath && Placement.PathDepth < tile.Placement.PathDepth)
					{
						return usedDoorway;
					}
				}
				else if (!tile.Placement.IsOnMainPath && Placement.Depth < tile.Placement.Depth)
				{
					return usedDoorway;
				}
			}
			return null;
		}

		public bool RecalculateBounds()
		{
			if (Placement == null)
			{
				Placement = new TilePlacementData();
			}
			Bounds localBounds = Placement.LocalBounds;
			if (OverrideAutomaticTileBounds)
			{
				Placement.LocalBounds = TileBoundsOverride;
			}
			else
			{
				Bounds bounds = UnityUtil.CalculateObjectBounds(base.gameObject, includeInactive: false, DunGenSettings.Instance.BoundsCalculationsIgnoreSprites);
				bounds = UnityUtil.CondenseBounds(bounds, GetComponentsInChildren<Doorway>(includeInactive: true));
				bounds = base.transform.InverseTransformBounds(bounds);
				Placement.LocalBounds = bounds;
			}
			Bounds localBounds2 = Placement.LocalBounds;
			bool result = localBounds2 != localBounds;
			if (localBounds2.size.x <= 0f || localBounds2.size.y <= 0f || localBounds2.size.z <= 0f)
			{
				Debug.LogError($"Tile prefab '{base.gameObject}' has automatic bounds that are zero or negative in size. The bounding volume for this tile will need to be manually defined.", base.gameObject);
			}
			return result;
		}

		public void CopyBoundsFrom(Tile otherTile)
		{
			if (!(otherTile == null))
			{
				if (Placement == null)
				{
					Placement = new TilePlacementData();
				}
				Placement.LocalBounds = otherTile.Placement.LocalBounds;
			}
		}

		public void OnBeforeSerialize()
		{
			fileVersion = 3;
		}

		public void OnAfterDeserialize()
		{
			if (fileVersion < 1)
			{
				RepeatMode = ((!allowImmediateRepeats) ? TileRepeatMode.DisallowImmediate : TileRepeatMode.Allow);
			}
			if (fileVersion < 2)
			{
				if (Entrances == null)
				{
					Entrances = new List<Doorway>();
				}
				if (Exits == null)
				{
					Exits = new List<Doorway>();
				}
				if (Entrance != null)
				{
					Entrances.Add(Entrance);
				}
				if (Exit != null)
				{
					Exits.Add(Exit);
				}
				Entrance = null;
				Exit = null;
			}
		}
	}
}
