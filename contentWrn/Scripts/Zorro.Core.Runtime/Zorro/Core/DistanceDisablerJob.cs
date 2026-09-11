using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Jobs;

namespace Zorro.Core
{
	[BurstCompile]
	public struct DistanceDisablerJob : IJobParallelForTransform
	{
		public float3 CameraPosition;

		public NativeArray<DistanceDisablerData> DistanceDisablerData;

		public NativeQueue<DistanceDisablerEvent>.ParallelWriter DistanceDisablerEventQueue;

		public void Execute(int index, TransformAccess transform)
		{
			DistanceDisablerData value = DistanceDisablerData[index];
			bool flag = math.distance(transform.position, CameraPosition) > value.distance;
			if (flag != value.culled)
			{
				value.culled = flag;
				DistanceDisablerData[index] = value;
				DistanceDisablerEventQueue.Enqueue(new DistanceDisablerEvent
				{
					Culled = flag,
					Index = index
				});
			}
		}
	}
}
