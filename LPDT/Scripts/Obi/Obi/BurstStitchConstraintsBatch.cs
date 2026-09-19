using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstStitchConstraintsBatch : BurstConstraintsBatchImpl, IStitchConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct StitchConstraintsBatchJob : IJob
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<float> stiffnesses;

			public NativeArray<float> lambdas;

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

			[ReadOnly]
			public float deltaTimeSqr;

			[ReadOnly]
			public int activeConstraintCount;

			public void Execute()
			{
				for (int i = 0; i < activeConstraintCount; i++)
				{
					int index = particleIndices[i * 2];
					int index2 = particleIndices[i * 2 + 1];
					float num = invMasses[index];
					float num2 = invMasses[index2];
					float num3 = stiffnesses[i] / deltaTimeSqr;
					float4 float5 = positions[index] - positions[index2];
					float num4 = math.length(float5);
					float num5 = (0f - num4 - num3 * lambdas[i]) / (num + num2 + num3 + 1E-07f);
					float4 float6 = num5 * float5 / (num4 + 1E-07f);
					lambdas[i] += num5;
					deltas[index] += float6 * num;
					deltas[index2] -= float6 * num2;
					counts[index]++;
					counts[index2]++;
				}
			}
		}

		[BurstCompile]
		public struct ApplyStitchConstraintsBatchJob : IJob
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

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

			[ReadOnly]
			public int activeConstraintCount;

			public void Execute()
			{
				for (int i = 0; i < activeConstraintCount; i++)
				{
					int index = particleIndices[i * 2];
					int index2 = particleIndices[i * 2 + 1];
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
				}
			}
		}

		private NativeArray<float> stiffnesses;

		public BurstStitchConstraintsBatch(BurstStitchConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Stitch;
		}

		public void SetStitchConstraints(ObiNativeIntList particleIndices, ObiNativeFloatList stiffnesses, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.stiffnesses = stiffnesses.AsNativeArray<float>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return new StitchConstraintsBatchJob
			{
				particleIndices = particleIndices,
				stiffnesses = stiffnesses,
				lambdas = lambdas,
				positions = base.solverImplementation.positions,
				invMasses = base.solverImplementation.invMasses,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				deltaTimeSqr = substepTime * substepTime,
				activeConstraintCount = m_ConstraintCount
			}.Schedule(inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return new ApplyStitchConstraintsBatchJob
			{
				particleIndices = particleIndices,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				sorFactor = constraintParameters.SORFactor,
				activeConstraintCount = m_ConstraintCount
			}.Schedule(inputDeps);
		}
	}
}
