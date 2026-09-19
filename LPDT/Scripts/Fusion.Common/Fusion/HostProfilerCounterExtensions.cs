namespace Fusion
{
	public static class HostProfilerCounterExtensions
	{
		public static void Add(this in HostProfilerCounter<int> marker)
		{
			marker.Add(1);
		}

		public static void Add(this in HostProfilerCounter<float> counter)
		{
			counter.Add(1f);
		}
	}
}
