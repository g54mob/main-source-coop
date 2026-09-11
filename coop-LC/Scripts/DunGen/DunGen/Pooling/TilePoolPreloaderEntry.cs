using System;

namespace DunGen.Pooling
{
	[Serializable]
	public sealed class TilePoolPreloaderEntry
	{
		public Tile TilePrefab;

		public int Count = 1;
	}
}
