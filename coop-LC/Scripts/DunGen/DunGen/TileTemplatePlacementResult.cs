using UnityEngine;

namespace DunGen
{
	public abstract class TileTemplatePlacementResult : TilePlacementResult
	{
		public GameObject TileTemplatePrefab { get; private set; }

		public TileTemplatePlacementResult(TileProxy tileTemplate)
		{
			if (tileTemplate != null)
			{
				TileTemplatePrefab = tileTemplate.Prefab;
			}
		}
	}
}
