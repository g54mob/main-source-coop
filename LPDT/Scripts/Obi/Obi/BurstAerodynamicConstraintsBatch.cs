using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstAerodynamicConstraintsBatch : BurstConstraintsBatchImpl, IAerodynamicConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct AerodynamicConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			[NativeDisableParallelForRestriction]
			public NativeArray<float> aerodynamicCoeffs;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> normals;

			[ReadOnly]
			public NativeArray<float4> wind;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[NativeDisableContainerSafetyRestriction]
			public NativeArray<float4> velocities;

			[ReadOnly]
			public float deltaTime;

			public void Execute(int i)
			{
				int index = particleIndices[i];
				float num = aerodynamicCoeffs[i * 3];
				float num2 = aerodynamicCoeffs[i * 3 + 1];
				float num3 = aerodynamicCoeffs[i * 3 + 2];
				if (invMasses[index] > 0f)
				{
					float4 float5 = velocities[index] - wind[index];
					float num4 = math.lengthsq(float5);
					if (!(num4 < 1E-07f))
					{
						float4 float6 = float5 / math.sqrt(num4);
						float4 x = normals[index] * math.sign(math.dot(normals[index], float6));
						float num5 = 0.5f * num4 * num;
						float num6 = math.dot(x, float6);
						float3 float7 = math.normalizesafe(math.cross(math.cross(x.xyz, float6.xyz), float6.xyz));
						velocities[index] += ((0f - num2) * float6 + num3 * new float4(float7.xyz, 0f)) * num6 * math.min(num5 * invMasses[index] * deltaTime, 1000f);
					}
				}
			}
		}

		private NativeArray<float> aerodynamicCoeffs;

		public BurstAerodynamicConstraintsBatch(BurstAerodynamicConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Aerodynamics;
		}

		public void SetAerodynamicConstraints(ObiNativeIntList particleIndices, ObiNativeFloatList aerodynamicCoeffs, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.aerodynamicCoeffs = aerodynamicCoeffs.AsNativeArray<float>();
			m_ConstraintCount = count;
		}

		public override JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return inputDeps;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new AerodynamicConstraintsBatchJob
			{
				particleIndices = particleIndices,
				aerodynamicCoeffs = aerodynamicCoeffs,
				positions = base.solverImplementation.positions,
				velocities = base.solverImplementation.velocities,
				normals = base.solverImplementation.normals,
				wind = base.solverImplementation.wind,
				invMasses = base.solverImplementation.invMasses,
				deltaTime = substepTime
			}, m_ConstraintCount, 32, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			return inputDeps;
		}
	}
}
