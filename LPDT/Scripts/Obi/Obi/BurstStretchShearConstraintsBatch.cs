using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstStretchShearConstraintsBatch : BurstConstraintsBatchImpl, IStretchShearConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct StretchShearConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> orientationIndices;

			[ReadOnly]
			public NativeArray<float> restLengths;

			[ReadOnly]
			public NativeArray<quaternion> restOrientations;

			[ReadOnly]
			public NativeArray<float3> stiffnesses;

			public NativeArray<float3> lambdas;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<quaternion> orientations;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<float> invRotationalMasses;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> orientationDeltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> orientationCounts;

			[ReadOnly]
			public float deltaTimeSqr;

			public void Execute(int i)
			{
				int index = particleIndices[i * 2];
				int index2 = particleIndices[i * 2 + 1];
				int index3 = orientationIndices[i];
				float num = invMasses[index];
				float num2 = invMasses[index2];
				float3 float5 = stiffnesses[i] / deltaTimeSqr;
				float3 float6 = math.rotate(restOrientations[i], new float3(0f, 0f, 1f));
				quaternion q = math.mul(orientations[index3], restOrientations[i]);
				float3 float7 = math.rotate(math.conjugate(q), (positions[index2] - positions[index]).xyz) / (restLengths[i] + 1E-07f);
				float7[2] -= 1f;
				float3 float8 = new float3((num + num2) / (restLengths[i] + 1E-07f) + invRotationalMasses[index3] * 4f * restLengths[i]);
				float3 float9 = (float7 - float5 * lambdas[i]) / (float8 + float5 + 1E-07f);
				lambdas[i] += float9;
				float9 = math.mul(q, float9);
				deltas[index] += new float4(float9, 0f) * num;
				deltas[index2] -= new float4(float9, 0f) * num2;
				quaternion b = math.mul(b: math.conjugate(new quaternion(float6.x, float6.y, float6.z, 0f)), a: orientations[index3]);
				quaternion quaternion2 = math.mul(new quaternion(float9[0], float9[1], float9[2], 0f), b);
				quaternion2.value *= 2f * invRotationalMasses[index3] * restLengths[i];
				quaternion value = orientationDeltas[index3];
				value.value += quaternion2.value;
				orientationDeltas[index3] = value;
				counts[index]++;
				counts[index2]++;
				orientationCounts[index3]++;
			}
		}

		[BurstCompile]
		public struct ApplyStretchShearConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> orientationIndices;

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
				int index = particleIndices[i * 2];
				int index2 = particleIndices[i * 2 + 1];
				int index3 = orientationIndices[i];
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
				if (orientationCounts[index3] > 0)
				{
					quaternion q = orientations[index3];
					q.value += orientationDeltas[index3].value * sorFactor / orientationCounts[index3];
					orientations[index3] = math.normalize(q);
					orientationDeltas[index3] = new quaternion(0f, 0f, 0f, 0f);
					orientationCounts[index3] = 0;
				}
			}
		}

		private NativeArray<int> orientationIndices;

		private NativeArray<float> restLengths;

		private NativeArray<quaternion> restOrientations;

		private NativeArray<float3> stiffnesses;

		public BurstStretchShearConstraintsBatch(BurstStretchShearConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.StretchShear;
		}

		public void SetStretchShearConstraints(ObiNativeIntList particleIndices, ObiNativeIntList orientationIndices, ObiNativeFloatList restLengths, ObiNativeQuaternionList restOrientations, ObiNativeVector3List stiffnesses, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.orientationIndices = orientationIndices.AsNativeArray<int>();
			this.restLengths = restLengths.AsNativeArray<float>();
			this.restOrientations = restOrientations.AsNativeArray<quaternion>();
			this.stiffnesses = stiffnesses.AsNativeArray<float3>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new StretchShearConstraintsBatchJob
			{
				particleIndices = particleIndices,
				orientationIndices = orientationIndices,
				restLengths = restLengths,
				restOrientations = restOrientations,
				stiffnesses = stiffnesses,
				lambdas = lambdas.Reinterpret<float, float3>(),
				positions = base.solverImplementation.positions,
				orientations = base.solverImplementation.orientations,
				invMasses = base.solverImplementation.invMasses,
				invRotationalMasses = base.solverImplementation.invRotationalMasses,
				deltas = base.solverImplementation.positionDeltas,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				deltaTimeSqr = substepTime * substepTime
			}, m_ConstraintCount, 32, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return IJobParallelForExtensions.Schedule(new ApplyStretchShearConstraintsBatchJob
			{
				particleIndices = particleIndices,
				orientationIndices = orientationIndices,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				orientations = base.solverImplementation.orientations,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				sorFactor = constraintParameters.SORFactor
			}, m_ConstraintCount, 64, inputDeps);
		}
	}
}
