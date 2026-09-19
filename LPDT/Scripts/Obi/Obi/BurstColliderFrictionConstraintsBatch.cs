using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstColliderFrictionConstraintsBatch : BurstConstraintsBatchImpl, IColliderFrictionConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct FrictionConstraintsBatchJob : IJob
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
			public NativeArray<int> simplices;

			[ReadOnly]
			public SimplexCounts simplexCounts;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[ReadOnly]
			public NativeArray<BurstAffineTransform> transforms;

			[ReadOnly]
			public NativeArray<BurstCollisionMaterial> collisionMaterials;

			[ReadOnly]
			public NativeArray<BurstRigidbody> rigidbodies;

			public NativeArray<float4> rigidbodyLinearDeltas;

			public NativeArray<float4> rigidbodyAngularDeltas;

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

			public NativeArray<BurstContact> contacts;

			[ReadOnly]
			public NativeArray<ContactEffectiveMasses> effectiveMasses;

			[ReadOnly]
			public BurstInertialFrame inertialFrame;

			[ReadOnly]
			public float stepTime;

			[ReadOnly]
			public float substepTime;

			[ReadOnly]
			public int steps;

			public void Execute()
			{
				for (int i = 0; i < contacts.Length; i++)
				{
					BurstContact value = contacts[i];
					int size;
					int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(value.bodyA, out size);
					int bodyB = value.bodyB;
					if (shapes[bodyB].isTrigger)
					{
						continue;
					}
					int rigidbodyIndex = shapes[bodyB].rigidbodyIndex;
					BurstCollisionMaterial burstCollisionMaterial = CombineCollisionMaterials(simplices[simplexStartAndSize], bodyB);
					float4 float5 = float4.zero;
					float4 float6 = float4.zero;
					float4 zero = float4.zero;
					float4 zero2 = float4.zero;
					float4 zero3 = float4.zero;
					float num = 0f;
					quaternion quaternion2 = new quaternion(0f, 0f, 0f, 0f);
					float4 zero4 = float4.zero;
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
					float4 relativeVelocity = zero2;
					if (burstCollisionMaterial.rollingContacts > 0)
					{
						float5 = -value.normal * BurstMath.EllipsoidRadius(value.normal, quaternion2, zero4.xyz);
						relativeVelocity += new float4(math.cross(zero3.xyz, float5.xyz), 0f);
					}
					if (rigidbodyIndex >= 0)
					{
						float6 = inertialFrame.frame.TransformPoint(value.pointB) - rigidbodies[rigidbodyIndex].com;
						relativeVelocity -= BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, value.pointB, rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame);
					}
					float2 float7 = value.SolveFriction(effectiveMasses[i].TotalTangentInvMass, effectiveMasses[i].TotalBitangentInvMass, relativeVelocity, burstCollisionMaterial.staticFriction, burstCollisionMaterial.dynamicFriction, stepTime);
					if (math.abs(float7.x) > 1E-07f || math.abs(float7.y) > 1E-07f)
					{
						float4 float8 = float7.x * value.tangent;
						float4 float9 = float7.y * value.bitangent;
						float4 float10 = float8 + float9;
						float num2 = BurstMath.BaryScale(value.pointA);
						for (int k = 0; k < size; k++)
						{
							int index2 = simplices[simplexStartAndSize + k];
							deltas[index2] += (float8 * effectiveMasses[i].tangentInvMassA + float9 * effectiveMasses[i].bitangentInvMassA) * substepTime * value.pointA[k] * num2;
							counts[index2]++;
						}
						if (rigidbodyIndex >= 0)
						{
							BurstMath.ApplyImpulse(rigidbodyIndex, -float10, value.pointB, rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame.frame);
						}
						if (burstCollisionMaterial.rollingContacts > 0)
						{
							float4x4 a = BurstMath.TransformInertiaTensor(math.rcp(BurstMath.GetParticleInertiaTensor(zero4, num) + new float4(1E-07f)), quaternion2);
							float4 float11 = math.mul(a, new float4(math.cross(float5.xyz, float10.xyz), 0f));
							float4 float12 = float4.zero;
							zero3 += float11;
							float4 float13 = float4.zero;
							float num3 = math.length(math.mul(a, math.normalizesafe(zero3)));
							float num4 = 0f;
							if (rigidbodyIndex >= 0)
							{
								float12 = math.mul(-rigidbodies[rigidbodyIndex].inverseInertiaTensor, new float4(math.cross(float6.xyz, float10.xyz), 0f));
								float13 = rigidbodies[rigidbodyIndex].angularVelocity + float12;
								num4 = math.length(math.mul(rigidbodies[rigidbodyIndex].inverseInertiaTensor, math.normalizesafe(float13)));
							}
							float4 rolling_axis = float4.zero;
							float num5 = value.SolveRollingFriction(zero3, float13, burstCollisionMaterial.rollingFriction, num3, num4, ref rolling_axis);
							float11 += rolling_axis * num5 * num3;
							float12 -= rolling_axis * num5 * num4;
							quaternion quaternion3 = BurstIntegration.AngularVelocityToSpinQuaternion(quaternion2, float11, substepTime);
							for (int l = 0; l < size; l++)
							{
								int index3 = simplices[simplexStartAndSize + l];
								quaternion value2 = orientationDeltas[index3];
								value2.value += quaternion3.value;
								orientationDeltas[index3] = value2;
								orientationCounts[index3]++;
							}
							if (rigidbodyIndex >= 0)
							{
								float4 value3 = rigidbodyAngularDeltas[rigidbodyIndex];
								value3 += float12;
								rigidbodyAngularDeltas[rigidbodyIndex] = value3;
							}
						}
					}
					contacts[i] = value;
				}
			}

			private BurstCollisionMaterial CombineCollisionMaterials(int entityA, int entityB)
			{
				int num = particleMaterialIndices[entityA];
				int materialIndex = shapes[entityB].materialIndex;
				if (materialIndex >= 0 && num >= 0)
				{
					return BurstCollisionMaterial.CombineWith(collisionMaterials[num], collisionMaterials[materialIndex]);
				}
				if (num >= 0)
				{
					return collisionMaterials[num];
				}
				if (materialIndex >= 0)
				{
					return collisionMaterials[materialIndex];
				}
				return default(BurstCollisionMaterial);
			}
		}

		public BurstColliderFrictionConstraintsBatch(BurstColliderFrictionConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Friction;
		}

		public override JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return inputDeps;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return new FrictionConstraintsBatchJob
			{
				positions = base.solverImplementation.positions,
				prevPositions = base.solverImplementation.prevPositions,
				orientations = base.solverImplementation.orientations,
				prevOrientations = base.solverImplementation.prevOrientations,
				invMasses = base.solverImplementation.invMasses,
				invRotationalMasses = base.solverImplementation.invRotationalMasses,
				radii = base.solverImplementation.principalRadii,
				particleMaterialIndices = base.solverImplementation.collisionMaterials,
				simplices = base.solverImplementation.simplices,
				simplexCounts = base.solverImplementation.simplexCounts,
				shapes = ObiColliderWorld.GetInstance().colliderShapes.AsNativeArray<BurstColliderShape>(),
				transforms = ObiColliderWorld.GetInstance().colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				collisionMaterials = ObiColliderWorld.GetInstance().collisionMaterials.AsNativeArray<BurstCollisionMaterial>(),
				rigidbodies = ObiColliderWorld.GetInstance().rigidbodies.AsNativeArray<BurstRigidbody>(),
				rigidbodyLinearDeltas = base.solverImplementation.abstraction.rigidbodyLinearDeltas.AsNativeArray<float4>(),
				rigidbodyAngularDeltas = base.solverImplementation.abstraction.rigidbodyAngularDeltas.AsNativeArray<float4>(),
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				contacts = ((BurstSolverImpl)base.constraints.solver).abstraction.colliderContacts.AsNativeArray<BurstContact>(),
				effectiveMasses = ((BurstSolverImpl)base.constraints.solver).abstraction.contactEffectiveMasses.AsNativeArray<ContactEffectiveMasses>(),
				inertialFrame = ((BurstSolverImpl)base.constraints.solver).inertialFrame,
				steps = steps,
				stepTime = stepTime,
				substepTime = substepTime
			}.Schedule(inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return new ApplyCollisionConstraintsBatchJob
			{
				contacts = ((BurstSolverImpl)base.constraints.solver).abstraction.colliderContacts.AsNativeArray<BurstContact>(),
				simplices = base.solverImplementation.simplices,
				simplexCounts = base.solverImplementation.simplexCounts,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				orientations = base.solverImplementation.orientations,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				constraintParameters = constraintParameters
			}.Schedule(inputDeps);
		}
	}
}
