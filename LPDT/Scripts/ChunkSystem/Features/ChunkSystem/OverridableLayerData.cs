using System;

namespace Features.ChunkSystem
{
	[Serializable]
	public struct OverridableLayerData
	{
		public string Layer;

		public bool Override;

		public int ActiveChunksCount;
	}
}
