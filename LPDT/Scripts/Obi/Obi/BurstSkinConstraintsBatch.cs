using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstSkinConstraintsBatch : BurstConstraintsBatchImpl, ISkinConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct SkinConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<float4> skinPoints;

			[ReadOnly]
			public NativeArray<float4> skinNormals;

			[ReadOnly]
			public NativeArray<float3> skinRadiiBackstop;

			[ReadOnly]
			public NativeArray<float> skinCompliance;

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
				float x = skinRadiiBackstop[i].x;
				float y = skinRadiiBackstop[i].y;
				float num = y + skinRadiiBackstop[i].z;
				float num2 = skinCompliance[i] / deltaTimeSqr;
				int index = particleIndices[i];
				if (invMasses[index] > 0f)
				{
					float4 float5 = positions[index] - skinPoints[i];
					float4 float6 = positions[index] - (skinPoints[i] - skinNormals[i] * num);
					float num3 = math.length(float5);
					float num4 = math.length(float6);
					float num5 = math.max(0f, num3 - x);
					float num6 = (0f - num5 - num2 * lambdas[i]) / (1f + num2);
					lambdas[i] += num6;
					deltas[index] += num6 * float5 / (num3 + 1E-07f);
					counts[index]++;
					num5 = math.min(0f, num4 - y);
					deltas[index] -= num5 * float6 / (num4 + 1E-07f);
				}
			}
		}

		[BurstCompile]
		public struct ApplySkinConstraintsBatchJob : IJobParallelFor
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
				int index = particleIndices[i];
				if (counts[index] > 0)
				{
					positions[index] += deltas[index] * sorFactor / counts[index];
					deltas[index] = float4.zero;
					counts[index] = 0;
				}
			}
		}

		private NativeArray<float4> skinPoints;

		private NativeArray<float4> skinNormals;

		private NativeArray<float> skinRadiiBackstop;

		private NativeArray<float> skinCompliance;

		public BurstSkinConstraintsBatch(BurstSkinConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Skin;
		}

		public void SetSkinConstraints(ObiNativeIntList particleIndices, ObiNativeVector4List skinPoints, ObiNativeVector4List skinNormals, ObiNativeFloatList skinRadiiBackstop, ObiNativeFloatList skinCompliance, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.skinPoints = skinPoints.AsNativeArray<float4>();
			this.skinNormals = skinNormals.AsNativeArray<float4>();
			this.skinRadiiBackstop = skinRadiiBackstop.AsNativeArray<float>();
			this.skinCompliance = skinCompliance.AsNativeArray<float>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new SkinConstraintsBatchJob
			{
				particleIndices = particleIndices,
				skinPoints = skinPoints,
				skinNormals = skinNormals,
				skinRadiiBackstop = skinRadiiBackstop.Reinterpret<float, float3>(),
				skinCompliance = skinCompliance,
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
			return IJobParallelForExtensions.Schedule(new ApplySkinConstraintsBatchJob
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
