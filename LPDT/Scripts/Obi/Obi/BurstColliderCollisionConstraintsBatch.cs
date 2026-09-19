using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstColliderCollisionConstraintsBatch : BurstConstraintsBatchImpl, IColliderCollisionConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct UpdateContactsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<quaternion> prevOrientations;

			[ReadOnly]
			public NativeArray<float4> velocities;

			[ReadOnly]
			public NativeArray<float4> radii;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<float> invRotationalMasses;

			[ReadOnly]
			public NativeArray<int> particleMaterialIndices;

			[ReadOnly]
			public NativeArray<BurstCollisionMaterial> collisionMaterials;

			[ReadOnly]
			public NativeArray<int> simplices;

			[ReadOnly]
			public SimplexCounts simplexCounts;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[ReadOnly]
			public NativeArray<BurstAffineTransform> transforms;

			[ReadOnly]
			public NativeArray<BurstRigidbody> rigidbodies;

			[ReadOnly]
			public NativeArray<float4> rigidbodyLinearDeltas;

			[ReadOnly]
			public NativeArray<float4> rigidbodyAngularDeltas;

			public NativeArray<ContactEffectiveMasses> effectiveMasses;

			public NativeArray<BurstContact> contacts;

			[ReadOnly]
			public BurstInertialFrame inertialFrame;

			public void Execute(int i)
			{
				BurstContact value = contacts[i];
				ContactEffectiveMasses value2 = effectiveMasses[i];
				int size;
				int simplexStartAndSize = simplexCounts.GetSimplexStartAndSize(value.bodyA, out size);
				int num = particleMaterialIndices[simplices[simplexStartAndSize]];
				bool flag = num >= 0 && collisionMaterials[num].rollingContacts > 0;
				float4 zero = float4.zero;
				float4 zero2 = float4.zero;
				quaternion orientation = new quaternion(0f, 0f, 0f, 0f);
				float num2 = 0f;
				float invRotationalMass = 0f;
				float num3 = 0f;
				for (int j = 0; j < size; j++)
				{
					int index = simplices[simplexStartAndSize + j];
					zero += velocities[index] * value.pointA[j];
					zero2 += prevPositions[index] * value.pointA[j];
					orientation.value += prevOrientations[index].value * value.pointA[j];
					num2 += invMasses[index] * value.pointA[j];
					invRotationalMass = invRotationalMasses[index] * value.pointA[j];
					num3 += BurstMath.EllipsoidRadius(value.normal, prevOrientations[index], radii[index].xyz) * value.pointA[j];
				}
				int rigidbodyIndex = shapes[value.bodyB].rigidbodyIndex;
				if (rigidbodyIndex >= 0)
				{
					zero -= BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, value.pointB, rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame);
					int materialIndex = shapes[value.bodyB].materialIndex;
					flag |= materialIndex >= 0 && collisionMaterials[materialIndex].rollingContacts > 0;
				}
				value.distance = math.dot(zero2 - value.pointB, value.normal) - num3;
				float4 contactPoint = value.pointB + value.normal * value.distance;
				value.CalculateTangent(zero);
				float4 inverseInertiaTensor = math.rcp(BurstMath.GetParticleInertiaTensor(num3, invRotationalMass) + new float4(1E-07f));
				value2.CalculateContactMassesA(num2, inverseInertiaTensor, zero2, orientation, contactPoint, value.normal, value.tangent, value.bitangent, flag);
				if (rigidbodyIndex >= 0)
				{
					value2.CalculateContactMassesB(rigidbodies[rigidbodyIndex], in inertialFrame.frame, value.pointB, value.normal, value.tangent, value.bitangent);
				}
				else
				{
					value2.ClearContactMassesB();
				}
				contacts[i] = value;
				effectiveMasses[i] = value2;
			}
		}

		[BurstCompile]
		public struct CollisionConstraintsBatchJob : IJob
		{
			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<quaternion> orientations;

			[ReadOnly]
			public NativeArray<quaternion> prevOrientations;

			[ReadOnly]
			public NativeArray<float> invMasses;

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
			public NativeArray<float4> positions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			public NativeArray<BurstContact> contacts;

			[ReadOnly]
			public NativeArray<ContactEffectiveMasses> effectiveMasses;

			[ReadOnly]
			public BurstInertialFrame inertialFrame;

			[ReadOnly]
			public Oni.ConstraintParameters constraintParameters;

			[ReadOnly]
			public Oni.SolverParameters solverParameters;

			[ReadOnly]
			public float stepTime;

			[ReadOnly]
			public float substepTime;

			[ReadOnly]
			public float timeLeft;

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
					float num = stepTime * (float)steps;
					float num2 = timeLeft / substepTime;
					BurstCollisionMaterial burstCollisionMaterial = CombineCollisionMaterials(simplices[simplexStartAndSize], bodyB);
					float4 zero = float4.zero;
					float4 zero2 = float4.zero;
					float num3 = 0f;
					for (int j = 0; j < size; j++)
					{
						int index = simplices[simplexStartAndSize + j];
						zero += positions[index] * value.pointA[j];
						zero2 += prevPositions[index] * value.pointA[j];
						num3 += BurstMath.EllipsoidRadius(value.normal, orientations[index], radii[index].xyz) * value.pointA[j];
					}
					float4 posA = math.lerp(zero2, zero, num2);
					posA += -value.normal * num3;
					float4 pointB = value.pointB;
					if (rigidbodyIndex >= 0)
					{
						pointB += BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, value.pointB, rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame) * num;
					}
					float num4 = value.SolveAdhesion(effectiveMasses[i].TotalNormalInvMass, posA, pointB, burstCollisionMaterial.stickDistance, burstCollisionMaterial.stickiness, stepTime);
					num4 += value.SolvePenetration(effectiveMasses[i].TotalNormalInvMass, posA, pointB, solverParameters.maxDepenetration * stepTime);
					if (math.abs(num4) > 1E-07f)
					{
						float4 float5 = num4 * value.normal * BurstMath.BaryScale(value.pointA) / num2;
						for (int k = 0; k < size; k++)
						{
							int index2 = simplices[simplexStartAndSize + k];
							deltas[index2] += float5 * invMasses[index2] * value.pointA[k];
							counts[index2]++;
						}
						if (constraintParameters.evaluationOrder == Oni.ConstraintParameters.EvaluationOrder.Sequential)
						{
							for (int l = 0; l < size; l++)
							{
								BurstConstraintsBatchImpl.ApplyPositionDelta(simplices[simplexStartAndSize + l], constraintParameters.SORFactor, ref positions, ref deltas, ref counts);
							}
						}
						if (rigidbodyIndex >= 0)
						{
							BurstMath.ApplyImpulse(rigidbodyIndex, (0f - num4) / num * value.normal, value.pointB, rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame.frame);
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

		public BurstColliderCollisionConstraintsBatch(BurstColliderCollisionConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Collision;
		}

		public override JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new UpdateContactsJob
			{
				prevPositions = base.solverImplementation.prevPositions,
				prevOrientations = base.solverImplementation.prevOrientations,
				velocities = base.solverImplementation.velocities,
				radii = base.solverImplementation.principalRadii,
				invMasses = base.solverImplementation.invMasses,
				invRotationalMasses = base.solverImplementation.invRotationalMasses,
				particleMaterialIndices = base.solverImplementation.collisionMaterials,
				collisionMaterials = ObiColliderWorld.GetInstance().collisionMaterials.AsNativeArray<BurstCollisionMaterial>(),
				simplices = base.solverImplementation.simplices,
				simplexCounts = base.solverImplementation.simplexCounts,
				shapes = ObiColliderWorld.GetInstance().colliderShapes.AsNativeArray<BurstColliderShape>(),
				transforms = ObiColliderWorld.GetInstance().colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				rigidbodies = ObiColliderWorld.GetInstance().rigidbodies.AsNativeArray<BurstRigidbody>(),
				rigidbodyLinearDeltas = base.solverImplementation.abstraction.rigidbodyLinearDeltas.AsNativeArray<float4>(),
				rigidbodyAngularDeltas = base.solverImplementation.abstraction.rigidbodyAngularDeltas.AsNativeArray<float4>(),
				contacts = ((BurstSolverImpl)base.constraints.solver).abstraction.colliderContacts.AsNativeArray<BurstContact>(),
				effectiveMasses = ((BurstSolverImpl)base.constraints.solver).abstraction.contactEffectiveMasses.AsNativeArray<ContactEffectiveMasses>(),
				inertialFrame = ((BurstSolverImpl)base.constraints.solver).inertialFrame
			}, ((BurstSolverImpl)base.constraints.solver).abstraction.colliderContacts.count, 128, inputDeps);
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return new CollisionConstraintsBatchJob
			{
				positions = base.solverImplementation.positions,
				prevPositions = base.solverImplementation.prevPositions,
				orientations = base.solverImplementation.orientations,
				prevOrientations = base.solverImplementation.prevOrientations,
				invMasses = base.solverImplementation.invMasses,
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
				deltas = base.solverAbstraction.positionDeltas.AsNativeArray<float4>(),
				counts = base.solverAbstraction.positionConstraintCounts.AsNativeArray<int>(),
				contacts = ((BurstSolverImpl)base.constraints.solver).abstraction.colliderContacts.AsNativeArray<BurstContact>(),
				effectiveMasses = ((BurstSolverImpl)base.constraints.solver).abstraction.contactEffectiveMasses.AsNativeArray<ContactEffectiveMasses>(),
				inertialFrame = ((BurstSolverImpl)base.constraints.solver).inertialFrame,
				constraintParameters = constraintParameters,
				solverParameters = base.solverAbstraction.parameters,
				steps = steps,
				timeLeft = timeLeft,
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
