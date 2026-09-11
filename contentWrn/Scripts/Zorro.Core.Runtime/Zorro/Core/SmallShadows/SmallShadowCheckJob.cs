using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Jobs;

namespace Zorro.Core.SmallShadows
{
	[BurstCompile]
	public struct SmallShadowCheckJob : IJobParallelForTransform
	{
		[ReadOnly]
		public float DistanceFactor;

		public NativeArray<bool> MeshShadowCastingStates;

		[ReadOnly]
		public NativeArray<float> MeshMaxDistances;

		public float3 CameraPos;

		[WriteOnly]
		internal NativeQueue<CheckResult>.ParallelWriter CheckResults;

		public void Execute(int index, TransformAccess transform)
		{
			float num = math.distancesq(transform.position, CameraPos);
			float num2 = MeshMaxDistances[index] * DistanceFactor;
			bool flag = num < num2 * num2;
			bool flag2 = MeshShadowCastingStates[index];
			if (flag != flag2)
			{
				MeshShadowCastingStates[index] = flag;
				CheckResults.Enqueue(new CheckResult(flag, index));
			}
		}
	}
}
