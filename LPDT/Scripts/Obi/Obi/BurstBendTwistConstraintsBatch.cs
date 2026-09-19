using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstBendTwistConstraintsBatch : BurstConstraintsBatchImpl, IBendTwistConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct BendTwistConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> orientationIndices;

			[ReadOnly]
			public NativeArray<float3> stiffnesses;

			[ReadOnly]
			public NativeArray<float2> plasticity;

			public NativeArray<quaternion> restDarboux;

			public NativeArray<float3> lambdas;

			[ReadOnly]
			public NativeArray<quaternion> orientations;

			[ReadOnly]
			public NativeArray<float> invRotationalMasses;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> orientationDeltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> orientationCounts;

			[ReadOnly]
			public float deltaTime;

			public void Execute(int i)
			{
				int index = orientationIndices[i * 2];
				int index2 = orientationIndices[i * 2 + 1];
				float num = invRotationalMasses[index];
				float num2 = invRotationalMasses[index2];
				float3 float5 = stiffnesses[i] / (deltaTime * deltaTime);
				quaternion value = restDarboux[i];
				quaternion quaternion2 = math.mul(math.conjugate(orientations[index]), orientations[index2]);
				quaternion quaternion3 = default(quaternion);
				quaternion3.value = quaternion2.value + value.value;
				quaternion2.value -= value.value;
				if (math.lengthsq(quaternion2.value) > math.lengthsq(quaternion3.value))
				{
					quaternion2 = quaternion3;
				}
				if (math.lengthsq(quaternion2.value.xyz) > plasticity[i].x * plasticity[i].x)
				{
					value.value += quaternion2.value * plasticity[i].y * deltaTime;
					restDarboux[i] = value;
				}
				float3 float6 = (quaternion2.value.xyz - float5 * lambdas[i]) / (float5 + new float3(num + num2 + 1E-07f));
				quaternion b = new quaternion(float6[0], float6[1], float6[2], 0f);
				quaternion value2 = orientationDeltas[index];
				quaternion value3 = orientationDeltas[index2];
				value2.value += math.mul(orientations[index2], b).value * num;
				value3.value -= math.mul(orientations[index], b).value * num2;
				orientationDeltas[index] = value2;
				orientationDeltas[index2] = value3;
				orientationCounts[index]++;
				orientationCounts[index2]++;
				lambdas[i] += float6;
			}
		}

		[BurstCompile]
		public struct ApplyBendTwistConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> orientationIndices;

			[ReadOnly]
			public float sorFactor;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> orientations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> orientationDeltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> orientationCounts;

			public void Execute(int i)
			{
				int index = orientationIndices[i * 2];
				int index2 = orientationIndices[i * 2 + 1];
				if (orientationCounts[index] > 0)
				{
					quaternion q = orientations[index];
					q.value += orientationDeltas[index].value * sorFactor / orientationCounts[index];
					orientations[index] = math.normalize(q);
					orientationDeltas[index] = new quaternion(0f, 0f, 0f, 0f);
					orientationCounts[index] = 0;
				}
				if (orientationCounts[index2] > 0)
				{
					quaternion q2 = orientations[index2];
					q2.value += orientationDeltas[index2].value * sorFactor / orientationCounts[index2];
					orientations[index2] = math.normalize(q2);
					orientationDeltas[index2] = new quaternion(0f, 0f, 0f, 0f);
					orientationCounts[index2] = 0;
				}
			}
		}

		private NativeArray<int> orientationIndices;

		private NativeArray<quaternion> restDarboux;

		private NativeArray<float3> stiffnesses;

		private NativeArray<float2> plasticity;

		public BurstBendTwistConstraintsBatch(BurstBendTwistConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.BendTwist;
		}

		public void SetBendTwistConstraints(ObiNativeIntList orientationIndices, ObiNativeQuaternionList restDarboux, ObiNativeVector3List stiffnesses, ObiNativeVector2List plasticity, ObiNativeFloatList lambdas, int count)
		{
			this.orientationIndices = orientationIndices.AsNativeArray<int>();
			this.restDarboux = restDarboux.AsNativeArray<quaternion>();
			this.stiffnesses = stiffnesses.AsNativeArray<float3>();
			this.plasticity = plasticity.AsNativeArray<float2>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new BendTwistConstraintsBatchJob
			{
				orientationIndices = orientationIndices,
				restDarboux = restDarboux,
				stiffnesses = stiffnesses,
				plasticity = plasticity,
				lambdas = lambdas.Reinterpret<float, float3>(),
				orientations = base.solverImplementation.orientations,
				invRotationalMasses = base.solverImplementation.invRotationalMasses,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				deltaTime = substepTime
			}, m_ConstraintCount, 32, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return IJobParallelForExtensions.Schedule(new ApplyBendTwistConstraintsBatchJob
			{
				orientationIndices = orientationIndices,
				orientations = base.solverImplementation.orientations,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				sorFactor = constraintParameters.SORFactor
			}, m_ConstraintCount, 64, inputDeps);
		}
	}
}
