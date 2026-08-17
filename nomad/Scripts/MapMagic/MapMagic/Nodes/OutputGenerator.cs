using MapMagic.Products;
using UnityEngine;

namespace MapMagic.Nodes
{
	public abstract class OutputGenerator : Generator
	{
		public abstract OutputLevel OutputLevel { get; }

		public abstract void ClearApplied(TileData data, Terrain terrain);
	}
}
