using UnityEngine;

namespace DunGen
{
	public class DungeonAttachmentSettings
	{
		public Doorway AttachmentDoorway { get; private set; }

		public Tile AttachmentTile { get; private set; }

		public TileProxy TileProxy { get; private set; }

		public DungeonAttachmentSettings(Doorway attachmentDoorway)
		{
			AttachmentDoorway = attachmentDoorway;
		}

		public DungeonAttachmentSettings(Tile attachmentTile)
		{
			AttachmentTile = attachmentTile;
		}

		public TileProxy GenerateAttachmentProxy(Vector3 upVector, RandomStream randomStream)
		{
			if (AttachmentTile != null)
			{
				if (AttachmentTile.Prefab == null)
				{
					PrepareManuallyPlacedTile(AttachmentTile, upVector, randomStream);
				}
				TileProxy = new TileProxy(AttachmentTile.Prefab, (Doorway doorway, int index) => AttachmentTile.UnusedDoorways.Contains(AttachmentTile.AllDoorways[index]));
				TileProxy.Placement.Position = AttachmentTile.transform.localPosition;
				TileProxy.Placement.Rotation = AttachmentTile.transform.localRotation;
			}
			else if (AttachmentDoorway != null)
			{
				Tile attachmentTile = AttachmentDoorway.Tile;
				if (attachmentTile == null)
				{
					attachmentTile = AttachmentDoorway.GetComponentInParent<Tile>();
					if (attachmentTile == null)
					{
						Debug.LogError("Cannot attach to a doorway that doesn't belong to a Tile. Ensure the Doorway is parented to a GameObject with a Tile component");
						return null;
					}
				}
				if (attachmentTile.Prefab == null)
				{
					PrepareManuallyPlacedTile(attachmentTile, upVector, randomStream);
				}
				if (AttachmentDoorway.Tile.UsedDoorways.Contains(AttachmentDoorway))
				{
					Debug.LogError("Cannot attach dungeon to doorway '" + AttachmentDoorway.name + "' as it is already in use");
				}
				TileProxy = new TileProxy(AttachmentDoorway.Tile.Prefab, (Doorway doorway, int index) => index == attachmentTile.AllDoorways.IndexOf(AttachmentDoorway));
				TileProxy.Placement.Position = AttachmentDoorway.Tile.transform.localPosition;
				TileProxy.Placement.Rotation = AttachmentDoorway.Tile.transform.localRotation;
			}
			return TileProxy;
		}

		private void PrepareManuallyPlacedTile(Tile tileToPrepare, Vector3 upVector, RandomStream randomStream)
		{
			tileToPrepare.Prefab = tileToPrepare.gameObject;
			Doorway[] componentsInChildren = tileToPrepare.GetComponentsInChildren<Doorway>();
			foreach (Doorway doorway in componentsInChildren)
			{
				doorway.Tile = tileToPrepare;
				tileToPrepare.AllDoorways.Add(doorway);
				tileToPrepare.UnusedDoorways.Add(doorway);
				doorway.ProcessDoorwayObjects(isDoorwayInUse: false, randomStream);
			}
			Bounds bounds = ((!tileToPrepare.OverrideAutomaticTileBounds) ? UnityUtil.CalculateProxyBounds(tileToPrepare.gameObject, upVector) : tileToPrepare.TileBoundsOverride);
			tileToPrepare.Placement.LocalBounds = UnityUtil.CondenseBounds(bounds, tileToPrepare.AllDoorways);
		}

		public Tile GetAttachmentTile()
		{
			Tile result = null;
			if (AttachmentTile != null)
			{
				result = AttachmentTile;
			}
			else if (AttachmentDoorway != null)
			{
				result = AttachmentDoorway.Tile;
			}
			return result;
		}
	}
}
