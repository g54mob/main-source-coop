namespace Obi
{
	public struct ChunkData
	{
		public int rendererIndex;

		public int offset;

		public ChunkData(int rendererIndex, int offset)
		{
			this.rendererIndex = rendererIndex;
			this.offset = offset;
		}
	}
}
