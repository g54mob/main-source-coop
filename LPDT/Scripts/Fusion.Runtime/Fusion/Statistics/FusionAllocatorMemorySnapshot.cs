namespace Fusion.Statistics
{
	public struct FusionAllocatorMemorySnapshot
	{
		public int TotalBytesFree;

		public int TotalBytesUsed;

		public int TotalFreeBlocks;

		public int TotalBlocks;

		public int[] BucketFullBlocksCount;

		public int[] BucketAllocatedSegmentsCount;

		public int[] BucketFreeSegmentsCount;

		public int[] BucketSegmentCapacity;

		public int[] BucketUsedSegmentsCount;
	}
}
