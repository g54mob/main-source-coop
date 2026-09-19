using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstParticleFrictionConstraintsBatch : BurstConstraintsBatchImpl, IParticleFrictionConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct ParticleFrictionConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<quaternion> orientations;

			[ReadOnly]
			public NativeArray<quaternion> prevOrientations;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<float> invRotationalMasses;

			[ReadOnly]
			public NativeArray<float4> radii;

			[ReadOnly]
			public NativeArray<int> particleMaterialIndices;

			[ReadOnly]
			public NativeArray<BurstCollisionMaterial> collisionMaterials;

			[ReadOnly]
			public NativeArray<int> simplices;

			[ReadOnly]
			public SimplexCounts simplexCounts;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> orientationDeltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> orientationCounts;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<BurstContact> contacts;

			[ReadOnly]
			public NativeArray<ContactEffectiveMasses> effectiveMasses;

			[ReadOnly]
			public BatchData batchData;

			[ReadOnly]
			public float substepTime;

			public void Execute(int workItemIndex)
			{
				batchData.GetConstraintRange(workItemIndex, out var start, out var end);
				for (int i = start; i < end; i++)
				{
					BurstContact value = contacts[i];
					int size;
					int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(value.bodyA, out size);
					int size2;
					int simplexStartAndSize2 = simplexCounts.GetSimplexStartAndSize(value.bodyB, out size2);
					BurstCollisionMaterial burstCollisionMaterial = CombineCollisionMaterials(simplices[simplexStartAndSize], simplices[simplexStartAndSize2]);
					float4 zero = float4.zero;
					float4 zero2 = float4.zero;
					float4 zero3 = float4.zero;
					float num = 0f;
					quaternion quaternion2 = new quaternion(0f, 0f, 0f, 0f);
					float4 zero4 = float4.zero;
					float4 zero5 = float4.zero;
					float4 zero6 = float4.zero;
					float4 zero7 = float4.zero;
					float num2 = 0f;
					quaternion quaternion3 = new quaternion(0f, 0f, 0f, 0f);
					float4 zero8 = float4.zero;
					for (int j = 0; j < size; j++)
					{
						int index = simplices[simplexStartAndSize + j];
						zero += prevPositions[index] * value.pointA[j];
						zero2 += BurstIntegration.DifferentiateLinear(positions[index], prevPositions[index], substepTime) * value.pointA[j];
						zero3 += BurstIntegration.DifferentiateAngular(orientations[index], prevOrientations[index], substepTime) * value.pointA[j];
						num += invRotationalMasses[index] * value.pointA[j];
						quaternion2.value += orientations[index].value * value.pointA[j];
						zero4 += radii[index] * value.pointA[j];
					}
					for (int k = 0; k < size2; k++)
					{
						int index2 = simplices[simplexStartAndSize2 + k];
						zero5 += prevPositions[index2] * value.pointB[k];
						zero6 += BurstIntegration.DifferentiateLinear(positions[index2], prevPositions[index2], substepTime) * value.pointB[k];
						zero7 += BurstIntegration.DifferentiateAngular(orientations[index2], prevOrientations[index2], substepTime) * value.pointB[k];
						num2 += invRotationalMasses[index2] * value.pointB[k];
						quaternion3.value += orientations[index2].value * value.pointB[k];
						zero8 += radii[index2] * value.pointB[k];
					}
					float4 float5 = float4.zero;
					float4 float6 = float4.zero;
					if (burstCollisionMaterial.rollingContacts > 0)
					{
						float5 = -value.normal * BurstMath.EllipsoidRadius(value.normal, quaternion2, zero4.xyz);
						float6 = value.normal * BurstMath.EllipsoidRadius(value.normal, quaternion3, zero8.xyz);
						zero2 += new float4(math.cross(zero3.xyz, float5.xyz), 0f);
						zero6 += new float4(math.cross(zero7.xyz, float6.xyz), 0f);
					}
					float4 relativeVelocity = zero2 - zero6;
					float2 float7 = value.SolveFriction(effectiveMasses[i].TotalTangentInvMass, effectiveMasses[i].TotalBitangentInvMass, relativeVelocity, burstCollisionMaterial.staticFriction, burstCollisionMaterial.dynamicFriction, substepTime);
					if (math.abs(float7.x) > 1E-07f || math.abs(float7.y) > 1E-07f)
					{
						float4 float8 = float7.x * value.tangent;
						float4 float9 = float7.y * value.bitangent;
						float4 float10 = float8 + float9;
						float num3 = BurstMath.BaryScale(value.pointA);
						for (int l = 0; l < size; l++)
						{
							int index3 = simplices[simplexStartAndSize + l];
							deltas[index3] += (float8 * effectiveMasses[i].tangentInvMassA + float9 * effectiveMasses[i].bitangentInvMassA) * substepTime * value.pointA[l] * num3;
							counts[index3]++;
						}
						num3 = BurstMath.BaryScale(value.pointB);
						for (int m = 0; m < size2; m++)
						{
							int index4 = simplices[simplexStartAndSize2 + m];
							deltas[index4] -= (float8 * effectiveMasses[i].tangentInvMassB + float9 * effectiveMasses[i].bitangentInvMassB) * substepTime * value.pointB[m] * num3;
							counts[index4]++;
						}
						if (burstCollisionMaterial.rollingContacts > 0)
						{
							float4 tensor = math.rcp(BurstMath.GetParticleInertiaTensor(zero4, num) + new float4(1E-07f));
							float4 tensor2 = math.rcp(BurstMath.GetParticleInertiaTensor(zero8, num2) + new float4(1E-07f));
							float4x4 a = BurstMath.TransformInertiaTensor(tensor, quaternion2);
							float4x4 a2 = BurstMath.TransformInertiaTensor(tensor2, quaternion3);
							float4 float11 = math.mul(a, new float4(math.cross(float5.xyz, float10.xyz), 0f));
							float4 float12 = -math.mul(a2, new float4(math.cross(float6.xyz, float10.xyz), 0f));
							zero3 += float11;
							zero7 += float12;
							float num4 = math.length(math.mul(a, math.normalizesafe(zero3)));
							float num5 = math.length(math.mul(a2, math.normalizesafe(zero7)));
							float4 rolling_axis = float4.zero;
							float num6 = value.SolveRollingFriction(zero3, zero7, burstCollisionMaterial.rollingFriction, num4, num5, ref rolling_axis);
							float11 += rolling_axis * num6 * num4;
							float12 -= rolling_axis * num6 * num5;
							quaternion quaternion4 = BurstIntegration.AngularVelocityToSpinQuaternion(quaternion2, float11, substepTime);
							quaternion quaternion5 = BurstIntegration.AngularVelocityToSpinQuaternion(quaternion3, float12, substepTime);
							for (int n = 0; n < size; n++)
							{
								int index5 = simplices[simplexStartAndSize + n];
								quaternion value2 = orientationDeltas[index5];
								value2.value += quaternion4.value;
								orientationDeltas[index5] = value2;
								orientationCounts[index5]++;
							}
							for (int num7 = 0; num7 < size2; num7++)
							{
								int index6 = simplices[simplexStartAndSize2 + num7];
								quaternion value3 = orientationDeltas[index6];
								value3.value += quaternion5.value;
								orientationDeltas[index6] = value3;
								orientationCounts[index6]++;
							}
						}
					}
					contacts[i] = value;
				}
			}

			private BurstCollisionMaterial CombineCollisionMaterials(int entityA, int entityB)
			{
				int num = particleMaterialIndices[entityA];
				int num2 = particleMaterialIndices[entityB];
				if (num >= 0 && num2 >= 0)
				{
					return BurstCollisionMaterial.CombineWith(collisionMaterials[num], collisionMaterials[num2]);
				}
				if (num >= 0)
				{
					return collisionMaterials[num];
				}
				if (num2 >= 0)
				{
					return collisionMaterials[num2];
				}
				return default(BurstCollisionMaterial);
			}
		}

		public BatchData batchData;

		public BurstParticleFrictionConstraintsBatch(BurstParticleFrictionConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.ParticleFriction;
		}

		public BurstParticleFrictionConstraintsBatch(BatchData batchData)
		{
			this.batchData = batchData;
		}

		public override JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return inputDeps;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return new ParticleFrictionConstraintsBatchJob
			{
				positions = base.solverImplementation.positions,
				prevPositions = base.solverImplementation.prevPositions,
				orientations = base.solverImplementation.orientations,
				prevOrientations = base.solverImplementation.prevOrientations,
				invMasses = base.solverImplementation.invMasses,
				invRotationalMasses = base.solverImplementation.invRotationalMasses,
				radii = base.solverImplementation.principalRadii,
				particleMaterialIndices = base.solverImplementation.collisionMaterials,
				collisionMaterials = ObiColliderWorld.GetInstance().collisionMaterials.AsNativeArray<BurstCollisionMaterial>(),
				simplices = base.solverImplementation.simplices,
				simplexCounts = base.solverImplementation.simplexCounts,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				contacts = ((BurstSolverImpl)base.constraints.solver).abstraction.particleContacts.AsNativeArray<BurstContact>(),
				effectiveMasses = ((BurstSolverImpl)base.constraints.solver).abstraction.particleContactEffectiveMasses.AsNativeArray<ContactEffectiveMasses>(),
				batchData = batchData,
				substepTime = substepTime
			}.Schedule(innerloopBatchCount: (!batchData.isLast) ? 1 : batchData.workItemCount, arrayLength: batchData.workItemCount, dependsOn: inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return new ApplyBatchedCollisionConstraintsBatchJob
			{
				contacts = base.solverAbstraction.particleContacts.AsNativeArray<BurstContact>(),
				simplices = base.solverImplementation.simplices,
				simplexCounts = base.solverImplementation.simplexCounts,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				orientations = base.solverImplementation.orientations,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				constraintParameters = constraintParameters,
				batchData = batchData
			}.Schedule(innerloopBatchCount: (!batchData.isLast) ? 1 : batchData.workItemCount, arrayLength: batchData.workItemCount, dependsOn: inputDeps);
		}
	}
}
