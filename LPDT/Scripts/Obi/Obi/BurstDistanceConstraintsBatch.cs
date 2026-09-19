using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstDistanceConstraintsBatch : BurstConstraintsBatchImpl, IDistanceConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct DistanceConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<float> restLengths;

			[ReadOnly]
			public NativeArray<float2> stiffnesses;

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
				float num3 = stiffnesses[i].x / deltaTimeSqr;
				float4 float5 = positions[index] - positions[index2];
				float num4 = math.length(float5);
				float num5 = num4 - restLengths[i];
				float num6 = (0f - (num5 - math.max(math.min(num5, 0f), 0f - stiffnesses[i].y)) - num3 * lambdas[i]) / (num + num2 + num3 + 1E-07f);
				float4 float6 = num6 * float5 / (num4 + 1E-07f);
				lambdas[i] += num6;
				deltas[index] += float6 * num;
				deltas[index2] -= float6 * num2;
				counts[index]++;
				counts[index2]++;
			}
		}

		[BurstCompile]
		public struct ApplyDistanceConstraintsBatchJob : IJobParallelFor
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

			public void Execute(int i)
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

		private NativeArray<float> restLengths;

		private NativeArray<float2> stiffnesses;

		private DistanceConstraintsBatchJob projectConstraints;

		private ApplyDistanceConstraintsBatchJob applyConstraints;

		public BurstDistanceConstraintsBatch(BurstDistanceConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Distance;
		}

		public void SetDistanceConstraints(ObiNativeIntList particleIndices, ObiNativeFloatList restLengths, ObiNativeVector2List stiffnesses, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.restLengths = restLengths.AsNativeArray<float>();
			this.stiffnesses = stiffnesses.AsNativeArray<float2>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
			projectConstraints.particleIndices = base.particleIndices;
			projectConstraints.restLengths = this.restLengths;
			projectConstraints.stiffnesses = this.stiffnesses;
			projectConstraints.lambdas = base.lambdas;
			applyConstraints.particleIndices = base.particleIndices;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			projectConstraints.positions = base.solverImplementation.positions;
			projectConstraints.invMasses = base.solverImplementation.invMasses;
			projectConstraints.deltas = base.solverImplementation.positionDeltas;
			projectConstraints.counts = base.solverImplementation.positionConstraintCounts;
			projectConstraints.deltaTimeSqr = substepTime * substepTime;
			return IJobParallelForExtensions.Schedule(projectConstraints, m_ConstraintCount, 32, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			applyConstraints.positions = base.solverImplementation.positions;
			applyConstraints.deltas = base.solverImplementation.positionDeltas;
			applyConstraints.counts = base.solverImplementation.positionConstraintCounts;
			applyConstraints.sorFactor = constraintParameters.SORFactor;
			return IJobParallelForExtensions.Schedule(applyConstraints, m_ConstraintCount, 64, inputDeps);
		}
	}
}
