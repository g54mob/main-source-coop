using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstChainConstraintsBatch : BurstConstraintsBatchImpl, IChainConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct ChainConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> firstIndex;

			[ReadOnly]
			public NativeArray<int> numIndices;

			[ReadOnly]
			public NativeArray<float2> restLengths;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			public void Execute(int c)
			{
				int num = numIndices[c] - 1;
				int num2 = firstIndex[c];
				float x = restLengths[c].x;
				float y = restLengths[c].y;
				NativeArray<float4> nativeArray = new NativeArray<float4>(num, Allocator.Temp);
				NativeArray<float3> nativeArray2 = new NativeArray<float3>(num, Allocator.Temp);
				for (int i = 0; i < num; i++)
				{
					int num3 = num2 + i;
					float4 obj = positions[particleIndices[num3]];
					float4 float5 = positions[particleIndices[num3 + 1]];
					float4 float6 = obj - float5;
					float num4 = math.length(float6);
					nativeArray[i] = new float4(float6 / (num4 + 1E-07f));
				}
				for (int j = 0; j < num; j++)
				{
					int num5 = num2 + j;
					float num6 = invMasses[particleIndices[num5]];
					float num7 = invMasses[particleIndices[num5 + 1]];
					float4 y2 = ((j > 0) ? nativeArray[j - 1] : float4.zero);
					float4 x2 = nativeArray[j];
					float4 y3 = ((j < num - 1) ? nativeArray[j + 1] : float4.zero);
					nativeArray2[j] = new float3((0f - num6) * math.dot(x2, y2), num6 + num7, (0f - num7) * math.dot(x2, y3));
				}
				for (int k = 0; k < num; k++)
				{
					int num8 = num2 + k;
					float4 x3 = positions[particleIndices[num8]];
					float4 y4 = positions[particleIndices[num8 + 1]];
					float num9 = ((k > 0) ? nativeArray2[k - 1].x : 0f);
					float num10 = ((k > 0) ? nativeArray2[k - 1].y : 0f);
					float num11 = nativeArray2[k].y - num9 * nativeArray2[k].x;
					float3 value = nativeArray2[k];
					if (math.abs(num11) > 1E-07f)
					{
						float num12 = math.distance(x3, y4);
						float num13 = 0f;
						if (num12 >= y)
						{
							num13 = num12 - y;
						}
						else if (num12 <= x)
						{
							num13 = num12 - x;
						}
						value.xy = new float2(value.z / num11, (num13 - num10 * value.x) / num11);
					}
					else
					{
						value.xy = float2.zero;
					}
					nativeArray2[k] = value;
				}
				for (int num14 = num - 1; num14 >= 0; num14--)
				{
					float num15 = ((num14 < num - 1) ? nativeArray2[num14 + 1].z : 0f);
					float3 value2 = nativeArray2[num14];
					value2.z = value2.y - value2.x * num15;
					nativeArray2[num14] = value2;
				}
				for (int l = 0; l < numIndices[c]; l++)
				{
					int index = num2 + l;
					float4 float7 = ((l > 0) ? nativeArray[l - 1] : float4.zero);
					float4 float8 = ((l < numIndices[c] - 1) ? nativeArray[l] : float4.zero);
					float num16 = ((l > 0) ? nativeArray2[l - 1].z : 0f);
					float num17 = ((l < numIndices[c] - 1) ? nativeArray2[l].z : 0f);
					int index2 = particleIndices[index];
					deltas[index2] += invMasses[index2] * (float7 * num16 - float8 * num17);
					counts[index2]++;
				}
			}
		}

		[BurstCompile]
		public struct ApplyChainConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> firstIndex;

			[ReadOnly]
			public NativeArray<int> numIndices;

			[ReadOnly]
			public float sorFactor;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> positions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			public void Execute(int i)
			{
				int num = firstIndex[i];
				int num2 = num + numIndices[i];
				for (int j = num; j < num2; j++)
				{
					int index = particleIndices[j];
					if (counts[index] > 0)
					{
						positions[index] += deltas[index] * sorFactor / counts[index];
						deltas[index] = float4.zero;
						counts[index] = 0;
					}
				}
			}
		}

		private NativeArray<int> firstIndex;

		private NativeArray<int> numIndices;

		private NativeArray<float2> restLengths;

		public BurstChainConstraintsBatch(BurstChainConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Chain;
		}

		public void SetChainConstraints(ObiNativeIntList particleIndices, ObiNativeVector2List restLengths, ObiNativeIntList firstIndex, ObiNativeIntList numIndices, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.firstIndex = firstIndex.AsNativeArray<int>();
			this.numIndices = numIndices.AsNativeArray<int>();
			this.restLengths = restLengths.AsNativeArray<float2>();
			m_ConstraintCount = count;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new ChainConstraintsBatchJob
			{
				particleIndices = particleIndices,
				firstIndex = firstIndex,
				numIndices = numIndices,
				restLengths = restLengths,
				positions = base.solverImplementation.positions,
				invMasses = base.solverImplementation.invMasses,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts
			}, m_ConstraintCount, 4, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return IJobParallelForExtensions.Schedule(new ApplyChainConstraintsBatchJob
			{
				particleIndices = particleIndices,
				firstIndex = firstIndex,
				numIndices = numIndices,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				sorFactor = constraintParameters.SORFactor
			}, m_ConstraintCount, 8, inputDeps);
		}
	}
}
