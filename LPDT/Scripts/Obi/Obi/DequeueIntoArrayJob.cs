using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace Obi
{
	[BurstCompile]
	public struct DequeueIntoArrayJob<T> : IJob where T : unmanaged
	{
		public int StartIndex;

		public NativeQueue<T> InputQueue;

		[WriteOnly]
		public NativeArray<T> OutputArray;

		public void Execute()
		{
			int count = InputQueue.Count;
			for (int i = StartIndex; i < StartIndex + count; i++)
			{
				OutputArray[i] = InputQueue.Dequeue();
			}
		}
	}
}
