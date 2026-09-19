using Unity.Profiling;

namespace Fusion
{
	public readonly struct HostProfilerCategory
	{
		public readonly ProfilerCategory InternalCategory;

		internal HostProfilerCategory(ProfilerCategory category)
		{
			InternalCategory = category;
		}
	}
}
