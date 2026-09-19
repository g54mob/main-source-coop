namespace Fusion.Statistics
{
	public class FusionMemoryStatisticsSnapshot
	{
		public const int BUCKET_COUNT = 57;

		public FusionAllocatorMemorySnapshot ObjectAllocatorMemorySnapshot = default(FusionAllocatorMemorySnapshot);

		public FusionAllocatorMemorySnapshot GeneralAllocatorMemorySnapshot = default(FusionAllocatorMemorySnapshot);

		internal void CollectData(Simulation simulation)
		{
			simulation.GetGeneralAllocatorMemorySnapshot(ref GeneralAllocatorMemorySnapshot);
			simulation.GetObjectAllocatorMemorySnapshot(ref ObjectAllocatorMemorySnapshot);
		}
	}
}
