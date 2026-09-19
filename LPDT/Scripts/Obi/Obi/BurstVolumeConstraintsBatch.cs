using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstVolumeConstraintsBatch : BurstConstraintsBatchImpl, IVolumeConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct VolumeConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> triangles;

			[ReadOnly]
			public NativeArray<int> firstTriangle;

			[ReadOnly]
			public NativeArray<int> numTriangles;

			[ReadOnly]
			public NativeArray<float> restVolumes;

			[ReadOnly]
			public NativeArray<float2> pressureStiffness;

			public NativeArray<float> lambdas;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> gradients;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			[ReadOnly]
			public float deltaTimeSqr;

			public void Execute(int i)
			{
				float num = pressureStiffness[i].y / deltaTimeSqr;
				for (int j = 0; j < numTriangles[i]; j++)
				{
					int num2 = (firstTriangle[i] + j) * 3;
					int index = triangles[num2];
					int index2 = triangles[num2 + 1];
					int index3 = triangles[num2 + 2];
					gradients[index] = new float4(0f, 0f, 0f, 1f);
					gradients[index2] = new float4(0f, 0f, 0f, 1f);
					gradients[index3] = new float4(0f, 0f, 0f, 1f);
				}
				float num3 = 0f;
				for (int k = 0; k < numTriangles[i]; k++)
				{
					int num4 = (firstTriangle[i] + k) * 3;
					int index4 = triangles[num4];
					int index5 = triangles[num4 + 1];
					int index6 = triangles[num4 + 2];
					gradients[index4] += new float4(math.cross(positions[index5].xyz, positions[index6].xyz), 0f);
					gradients[index5] += new float4(math.cross(positions[index6].xyz, positions[index4].xyz), 0f);
					gradients[index6] += new float4(math.cross(positions[index4].xyz, positions[index5].xyz), 0f);
					num3 += math.dot(math.cross(positions[index4].xyz, positions[index5].xyz), positions[index6].xyz) / 6f;
				}
				float num5 = 0f;
				for (int l = 0; l < numTriangles[i]; l++)
				{
					int num6 = (firstTriangle[i] + l) * 3;
					int index7 = triangles[num6];
					int index8 = triangles[num6 + 1];
					int index9 = triangles[num6 + 2];
					num5 += invMasses[index7] * math.lengthsq(gradients[index7].xyz) * gradients[index7].w;
					gradients[index7] = new float4(gradients[index7].xyz, 0f);
					num5 += invMasses[index8] * math.lengthsq(gradients[index8].xyz) * gradients[index8].w;
					gradients[index8] = new float4(gradients[index8].xyz, 0f);
					num5 += invMasses[index9] * math.lengthsq(gradients[index9].xyz) * gradients[index9].w;
					gradients[index9] = new float4(gradients[index9].xyz, 0f);
				}
				float num7 = (0f - (num3 - pressureStiffness[i].x * restVolumes[i]) - num * lambdas[i]) / (num5 + num + 1E-07f);
				lambdas[i] += num7;
				for (int m = 0; m < numTriangles[i]; m++)
				{
					int num8 = (firstTriangle[i] + m) * 3;
					int index10 = triangles[num8];
					int index11 = triangles[num8 + 1];
					int index12 = triangles[num8 + 2];
					deltas[index10] += num7 * invMasses[index10] * gradients[index10];
					counts[index10]++;
					deltas[index11] += num7 * invMasses[index11] * gradients[index11];
					counts[index11]++;
					deltas[index12] += num7 * invMasses[index12] * gradients[index12];
					counts[index12]++;
				}
			}
		}

		[BurstCompile]
		public struct ApplyVolumeConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> triangles;

			[ReadOnly]
			public NativeArray<int> firstTriangle;

			[ReadOnly]
			public NativeArray<int> numTriangles;

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
				for (int j = 0; j < numTriangles[i]; j++)
				{
					int num = (firstTriangle[i] + j) * 3;
					int index = triangles[num];
					int index2 = triangles[num + 1];
					int index3 = triangles[num + 2];
					if (counts[index] > 0)
					{
						positions[index] += deltas[index] * sorFactor / counts[index];
						deltas[index] = float4.zero;
						counts[index] = 0;
					}
					if (counts[index2] > 0)
					{
						positions[index2] += deltas[index2] * sorFactor / counts[index2];
						deltas[index2] = float4.zero;
						counts[index2] = 0;
					}
					if (counts[index3] > 0)
					{
						positions[index3] += deltas[index3] * sorFactor / counts[index3];
						deltas[index3] = float4.zero;
						counts[index3] = 0;
					}
				}
			}
		}

		private NativeArray<int> firstTriangle;

		private NativeArray<int> numTriangles;

		private NativeArray<float> restVolumes;

		private NativeArray<float2> pressureStiffness;

		public BurstVolumeConstraintsBatch(BurstVolumeConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Volume;
		}

		public void SetVolumeConstraints(ObiNativeIntList triangles, ObiNativeIntList firstTriangle, ObiNativeIntList numTriangles, ObiNativeFloatList restVolumes, ObiNativeVector2List pressureStiffness, ObiNativeFloatList lambdas, int count)
		{
			particleIndices = triangles.AsNativeArray<int>();
			this.firstTriangle = firstTriangle.AsNativeArray<int>();
			this.numTriangles = numTriangles.AsNativeArray<int>();
			this.restVolumes = restVolumes.AsNativeArray<float>();
			this.pressureStiffness = pressureStiffness.AsNativeArray<float2>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new VolumeConstraintsBatchJob
			{
				triangles = particleIndices,
				firstTriangle = firstTriangle,
				numTriangles = numTriangles,
				restVolumes = restVolumes,
				pressureStiffness = pressureStiffness,
				lambdas = lambdas,
				positions = base.solverImplementation.positions,
				invMasses = base.solverImplementation.invMasses,
				gradients = base.solverImplementation.fluidData,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				deltaTimeSqr = substepTime * substepTime
			}, m_ConstraintCount, 4, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return IJobParallelForExtensions.Schedule(new ApplyVolumeConstraintsBatchJob
			{
				triangles = particleIndices,
				firstTriangle = firstTriangle,
				numTriangles = numTriangles,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				sorFactor = constraintParameters.SORFactor
			}, m_ConstraintCount, 8, inputDeps);
		}
	}
}
