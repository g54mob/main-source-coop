namespace Obi
{
	public struct WorkItem
	{
		public const int minWorkItemSize = 64;

		public unsafe fixed int constraints[64];

		public int constraintCount;

		public unsafe bool Add(int constraintIndex)
		{
			fixed (int* ptr = constraints)
			{
				ptr[constraintCount] = constraintIndex;
			}
			return ++constraintCount == 64;
		}
	}
}
