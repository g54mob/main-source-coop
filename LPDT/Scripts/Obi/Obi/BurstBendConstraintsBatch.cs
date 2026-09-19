using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstBendConstraintsBatch : BurstConstraintsBatchImpl, IBendConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct BendConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<float2> stiffnesses;

			[ReadOnly]
			public NativeArray<float2> plasticity;

			public NativeArray<float> restBends;

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
			public float deltaTime;

			public void Execute(int i)
			{
				int index = particleIndices[i * 3];
				int index2 = particleIndices[i * 3 + 1];
				int index3 = particleIndices[i * 3 + 2];
				float num = invMasses[index];
				float num2 = invMasses[index2];
				float num3 = invMasses[index3];
				float num4 = num + num2 + 2f * num3;
				float4 float5 = positions[index3] - (positions[index] + positions[index2] + positions[index3]) / 3f;
				float num5 = math.length(float5);
				float num6 = num5 - restBends[i];
				num6 = math.max(0f, num6 - stiffnesses[i].x) + math.min(0f, num6 + stiffnesses[i].x);
				if (math.abs(num6) > plasticity[i].x)
				{
					restBends[i] += num6 * plasticity[i].y * deltaTime;
				}
				float num7 = stiffnesses[i].y / (deltaTime * deltaTime);
				float num8 = (0f - num6 - num7 * lambdas[i]) / (num4 + num7 + 1E-07f);
				float4 float6 = num8 * float5 / (num5 + 1E-07f);
				lambdas[i] += num8;
				deltas[index] -= float6 * 2f * num;
				deltas[index2] -= float6 * 2f * num2;
				deltas[index3] += float6 * 4f * num3;
				counts[index]++;
				counts[index2]++;
				counts[index3]++;
			}
		}

		[BurstCompile]
		public struct ApplyBendConstraintsBatchJob : IJobParallelFor
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
				int index = particleIndices[i * 3];
				int index2 = particleIndices[i * 3 + 1];
				int index3 = particleIndices[i * 3 + 2];
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

		private NativeArray<float> restBends;

		private NativeArray<float2> stiffnesses;

		private NativeArray<float2> plasticity;

		private BendConstraintsBatchJob projectConstraints;

		private ApplyBendConstraintsBatchJob applyConstraints;

		public BurstBendConstraintsBatch(BurstBendConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Bending;
		}

		public void SetBendConstraints(ObiNativeIntList particleIndices, ObiNativeFloatList restBends, ObiNativeVector2List bendingStiffnesses, ObiNativeVector2List plasticity, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.restBends = restBends.AsNativeArray<float>();
			stiffnesses = bendingStiffnesses.AsNativeArray<float2>();
			this.plasticity = plasticity.AsNativeArray<float2>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
			projectConstraints.particleIndices = base.particleIndices;
			projectConstraints.restBends = this.restBends;
			projectConstraints.stiffnesses = stiffnesses;
			projectConstraints.plasticity = this.plasticity;
			projectConstraints.lambdas = base.lambdas;
			applyConstraints.particleIndices = base.particleIndices;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			projectConstraints.positions = base.solverImplementation.positions;
			projectConstraints.invMasses = base.solverImplementation.invMasses;
			projectConstraints.deltas = base.solverImplementation.positionDeltas;
			projectConstraints.counts = base.solverImplementation.positionConstraintCounts;
			projectConstraints.deltaTime = substepTime;
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
