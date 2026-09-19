namespace Obi
{
	public struct DistanceFieldHeader
	{
		public int firstNode;

		public int nodeCount;

		public DistanceFieldHeader(int firstNode, int nodeCount)
		{
			this.firstNode = firstNode;
			this.nodeCount = nodeCount;
		}
	}
}
