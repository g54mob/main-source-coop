using System;

namespace Features.ChunkSystem
{
	[Serializable]
	public struct ChunkLayerEntry
	{
		public string LayerName;

		public int CellSize;

		public int ActiveChunksCount;

		public ChunkLayerEntry(string layerName, int cellSize, int activeChunksCount)
		{
			LayerName = layerName;
			CellSize = cellSize;
			ActiveChunksCount = activeChunksCount;
		}
	}
}
