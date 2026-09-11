using UnityEngine;

namespace DunGen
{
	public sealed class NoMatchingDoorwayPlacementResult : TilePlacementResult
	{
		public override string DisplayName => "No Doorway Pairs";

		public GameObject FromTilePrefab { get; private set; }

		public NoMatchingDoorwayPlacementResult(TileProxy fromTile)
		{
			if (fromTile != null)
			{
				FromTilePrefab = fromTile.Prefab;
			}
		}
	}
}
