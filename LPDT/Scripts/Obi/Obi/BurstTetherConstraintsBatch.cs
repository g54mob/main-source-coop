using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstTetherConstraintsBatch : BurstConstraintsBatchImpl, ITetherConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct TetherConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<float2> maxLengthScale;

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

			public void Execute(int i)
			{
				int index = particleIndices[i * 2];
				int index2 = particleIndices[i * 2 + 1];
				float num = invMasses[index];
				float num2 = invMasses[index2];
				float num3 = stiffnesses[i] / deltaTimeSqr;
				float4 float5 = positions[index] - positions[index2];
				float num4 = math.length(float5);
				float num5 = num4 - maxLengthScale[i].x * maxLengthScale[i].y;
				if (num5 > 0f)
				{
					float num6 = (0f - num5 - num3 * lambdas[i]) / (num + num2 + num3 + 1E-07f);
					float4 float6 = num6 * float5 / (num4 + 1E-07f);
					lambdas[i] += num6;
					deltas[index] += float6 * num;
					counts[index]++;
				}
			}
		}

		[BurstCompile]
		public struct ApplyTetherConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeSlice<int> particleIndices;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> positions;

			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			[ReadOnly]
			public float sorFactor;

			public void Execute(int index)
			{
				int index2 = particleIndices[index * 2];
				if (counts[index2] > 0)
				{
					positions[index2] += deltas[index2] * sorFactor / counts[index2];
					deltas[index2] = float4.zero;
					counts[index2] = 0;
				}
			}
		}

		private NativeArray<float2> maxLengthScale;

		private NativeArray<float> stiffnesses;

		public BurstTetherConstraintsBatch(BurstTetherConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Tether;
		}

		public void SetTetherConstraints(ObiNativeIntList particleIndices, ObiNativeVector2List maxLengthScale, ObiNativeFloatList stiffnesses, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.maxLengthScale = maxLengthScale.AsNativeArray<float2>();
			this.stiffnesses = stiffnesses.AsNativeArray<float>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new TetherConstraintsBatchJob
			{
				particleIndices = particleIndices,
				maxLengthScale = maxLengthScale,
				stiffnesses = stiffnesses,
				lambdas = lambdas,
				positions = base.solverImplementation.positions,
				invMasses = base.solverImplementation.invMasses,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				deltaTimeSqr = substepTime * substepTime
			}, m_ConstraintCount, 32, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return IJobParallelForExtensions.Schedule(new ApplyTetherConstraintsBatchJob
			{
				particleIndices = particleIndices,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				sorFactor = constraintParameters.SORFactor
			}, m_ConstraintCount, 64, inputDeps);
		}
	}
}
