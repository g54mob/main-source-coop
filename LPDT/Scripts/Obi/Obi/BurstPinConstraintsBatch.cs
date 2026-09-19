using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstPinConstraintsBatch : BurstConstraintsBatchImpl, IPinConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct ClearPinsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> colliderIndices;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<BurstRigidbody> rigidbodies;

			public unsafe void Execute(int i)
			{
				int num = colliderIndices[i];
				if (num >= 0)
				{
					int rigidbodyIndex = shapes[num].rigidbodyIndex;
					if (rigidbodyIndex >= 0)
					{
						BurstRigidbody* unsafePtr = (BurstRigidbody*)rigidbodies.GetUnsafePtr();
						Interlocked.Exchange(ref unsafePtr[rigidbodyIndex].constraintCount, 0);
					}
				}
			}
		}

		[BurstCompile]
		public struct UpdatePinsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> colliderIndices;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<BurstRigidbody> rigidbodies;

			public unsafe void Execute(int i)
			{
				int num = colliderIndices[i];
				if (num >= 0)
				{
					int rigidbodyIndex = shapes[num].rigidbodyIndex;
					if (rigidbodyIndex >= 0)
					{
						BurstRigidbody* unsafePtr = (BurstRigidbody*)rigidbodies.GetUnsafePtr();
						Interlocked.Increment(ref unsafePtr[rigidbodyIndex].constraintCount);
					}
				}
			}
		}

		[BurstCompile]
		public struct PinConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> colliderIndices;

			[ReadOnly]
			public NativeArray<float4> offsets;

			[ReadOnly]
			public NativeArray<float2> stiffnesses;

			[ReadOnly]
			public NativeArray<quaternion> restDarboux;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> lambdas;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<quaternion> orientations;

			[ReadOnly]
			public NativeArray<float> invRotationalMasses;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[ReadOnly]
			public NativeArray<BurstAffineTransform> transforms;

			[ReadOnly]
			public NativeArray<BurstRigidbody> rigidbodies;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> rigidbodyLinearDeltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
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

			[ReadOnly]
			public BurstInertialFrame inertialFrame;

			[ReadOnly]
			public float stepTime;

			[ReadOnly]
			public float substepTime;

			[ReadOnly]
			public float timeLeft;

			[ReadOnly]
			public int steps;

			[ReadOnly]
			public int activeConstraintCount;

			public void Execute(int i)
			{
				int index = particleIndices[i];
				int num = colliderIndices[i];
				if (num < 0)
				{
					return;
				}
				int rigidbodyIndex = shapes[num].rigidbodyIndex;
				float num2 = stepTime * (float)steps;
				float num3 = timeLeft / substepTime;
				float2 float5 = stiffnesses[i].xy / (substepTime * substepTime);
				float4 float6 = math.lerp(prevPositions[index], positions[index], num3);
				float4 float7 = transforms[num].TransformPoint(offsets[i]);
				float4 float8 = float7;
				quaternion quaternion2 = transforms[num].rotation;
				float num4 = 0f;
				float num5 = 0f;
				if (rigidbodyIndex >= 0)
				{
					BurstRigidbody burstRigidbody = rigidbodies[rigidbodyIndex];
					float4 rigidbodyVelocityAtPoint = BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, inertialFrame.frame.InverseTransformPoint(float7), rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame);
					float8 = BurstIntegration.IntegrateLinear(float8, inertialFrame.frame.TransformVector(rigidbodyVelocityAtPoint), num2);
					quaternion2 = BurstIntegration.IntegrateAngular(quaternion2, burstRigidbody.angularVelocity + rigidbodyAngularDeltas[rigidbodyIndex], stepTime);
					num4 = burstRigidbody.inverseMass * (float)burstRigidbody.constraintCount;
					num5 = BurstMath.RotationalInvMass(burstRigidbody.inverseInertiaTensor, float7 - burstRigidbody.com, math.normalizesafe(inertialFrame.frame.TransformPoint(float6) - float8)) * (float)burstRigidbody.constraintCount;
				}
				float8 = inertialFrame.frame.InverseTransformPoint(float8);
				quaternion2 = math.mul(math.conjugate(inertialFrame.frame.rotation), quaternion2);
				float4 obj = float6 - float8;
				float num6 = math.length(obj);
				float4 float9 = obj / (num6 + 1E-07f);
				float4 value = lambdas[i];
				float num7 = (0f - num6 - float5.x * value.w) / (invMasses[index] + num4 + num5 + float5.x + 1E-07f);
				value.w += num7;
				float4 float10 = num7 * float9;
				deltas[index] += float10 * invMasses[index] / num3;
				counts[index]++;
				if (rigidbodyIndex >= 0)
				{
					BurstMath.ApplyImpulse(rigidbodyIndex, -float10 / num2, inertialFrame.frame.InverseTransformPoint(float7), rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame.frame);
				}
				if (num5 > 0f || invRotationalMasses[index] > 0f)
				{
					quaternion quaternion3 = math.mul(math.conjugate(orientations[index]), quaternion2);
					quaternion quaternion4 = default(quaternion);
					quaternion4.value = quaternion3.value + restDarboux[i].value;
					quaternion3.value -= restDarboux[i].value;
					if (math.lengthsq(quaternion3.value) > math.lengthsq(quaternion4.value))
					{
						quaternion3 = quaternion4;
					}
					float3 float11 = (quaternion3.value.xyz - float5.y * value.xyz) / (float5.y + invRotationalMasses[index] + num5 + 1E-07f);
					value.xyz += float11;
					quaternion b = new quaternion(float11[0], float11[1], float11[2], 0f);
					quaternion value2 = orientationDeltas[index];
					value2.value += math.mul(quaternion2, b).value * invRotationalMasses[index] / num3;
					orientationDeltas[index] = value2;
					orientationCounts[index]++;
					if (rigidbodyIndex >= 0)
					{
						BurstMath.ApplyDeltaQuaternion(rigidbodyIndex, quaternion2, -math.mul(orientations[index], b).value * num5, rigidbodyAngularDeltas, inertialFrame.frame, num2);
					}
				}
				lambdas[i] = value;
			}
		}

		[BurstCompile]
		public struct ApplyPinConstraintsBatchJob : IJob
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

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> orientations;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> orientationDeltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> orientationCounts;

			[ReadOnly]
			public int activeConstraintCount;

			public void Execute()
			{
				for (int i = 0; i < activeConstraintCount; i++)
				{
					int index = particleIndices[i];
					if (counts[index] > 0)
					{
						positions[index] += deltas[index] * sorFactor / counts[index];
						deltas[index] = float4.zero;
						counts[index] = 0;
					}
					if (orientationCounts[index] > 0)
					{
						quaternion q = orientations[index];
						q.value += orientationDeltas[index].value * sorFactor / orientationCounts[index];
						orientations[index] = math.normalize(q);
						orientationDeltas[index] = new quaternion(0f, 0f, 0f, 0f);
						orientationCounts[index] = 0;
					}
				}
			}
		}

		[BurstCompile]
		public struct ProjectRenderablePositionsJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> colliderIndices;

			[ReadOnly]
			public NativeArray<float4> offsets;

			[ReadOnly]
			public NativeArray<float2> stiffnesses;

			[ReadOnly]
			public NativeArray<quaternion> restDarboux;

			[ReadOnly]
			public NativeArray<BurstAffineTransform> transforms;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> renderablePositions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> renderableOrientations;

			[ReadOnly]
			public BurstInertialFrame inertialFrame;

			public void Execute(int i)
			{
				int index = particleIndices[i];
				int num = colliderIndices[i];
				if (num >= 0 && !(offsets[i].w < 0.5f))
				{
					BurstAffineTransform burstAffineTransform = inertialFrame.frame.Inverse() * transforms[num];
					renderablePositions[index] = burstAffineTransform.TransformPoint(offsets[i]);
					if (stiffnesses[i].y < 10000f)
					{
						renderableOrientations[index] = math.mul(burstAffineTransform.rotation, restDarboux[i]);
					}
				}
			}
		}

		private NativeArray<int> colliderIndices;

		private NativeArray<float4> offsets;

		private NativeArray<quaternion> restDarbouxVectors;

		private NativeArray<float2> stiffnesses;

		public BurstPinConstraintsBatch(BurstPinConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Pin;
		}

		public void SetPinConstraints(ObiNativeIntList particleIndices, ObiNativeIntList colliderIndices, ObiNativeVector4List offsets, ObiNativeQuaternionList restDarbouxVectors, ObiNativeFloatList stiffnesses, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.colliderIndices = colliderIndices.AsNativeArray<int>();
			this.offsets = offsets.AsNativeArray<float4>();
			this.restDarbouxVectors = restDarbouxVectors.AsNativeArray<quaternion>();
			this.stiffnesses = stiffnesses.AsNativeArray<float2>();
			base.lambdas = lambdas.AsNativeArray<float>();
			m_ConstraintCount = count;
		}

		public override JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			inputDeps = IJobParallelForExtensions.Schedule(new ClearPinsJob
			{
				colliderIndices = colliderIndices,
				shapes = ObiColliderWorld.GetInstance().colliderShapes.AsNativeArray<BurstColliderShape>(),
				rigidbodies = ObiColliderWorld.GetInstance().rigidbodies.AsNativeArray<BurstRigidbody>()
			}, m_ConstraintCount, 128, inputDeps);
			inputDeps = IJobParallelForExtensions.Schedule(new UpdatePinsJob
			{
				colliderIndices = colliderIndices,
				shapes = ObiColliderWorld.GetInstance().colliderShapes.AsNativeArray<BurstColliderShape>(),
				rigidbodies = ObiColliderWorld.GetInstance().rigidbodies.AsNativeArray<BurstRigidbody>()
			}, m_ConstraintCount, 128, inputDeps);
			return base.Initialize(inputDeps, stepTime, substepTime, steps, timeLeft);
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new PinConstraintsBatchJob
			{
				particleIndices = particleIndices,
				colliderIndices = colliderIndices,
				offsets = offsets,
				stiffnesses = stiffnesses,
				restDarboux = restDarbouxVectors,
				lambdas = lambdas.Reinterpret<float, float4>(),
				positions = base.solverImplementation.positions,
				prevPositions = base.solverImplementation.prevPositions,
				invMasses = base.solverImplementation.invMasses,
				orientations = base.solverImplementation.orientations,
				invRotationalMasses = base.solverImplementation.invRotationalMasses,
				shapes = ObiColliderWorld.GetInstance().colliderShapes.AsNativeArray<BurstColliderShape>(),
				transforms = ObiColliderWorld.GetInstance().colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				rigidbodies = ObiColliderWorld.GetInstance().rigidbodies.AsNativeArray<BurstRigidbody>(),
				rigidbodyLinearDeltas = base.solverImplementation.abstraction.rigidbodyLinearDeltas.AsNativeArray<float4>(),
				rigidbodyAngularDeltas = base.solverImplementation.abstraction.rigidbodyAngularDeltas.AsNativeArray<float4>(),
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				inertialFrame = ((BurstSolverImpl)base.constraints.solver).inertialFrame,
				stepTime = stepTime,
				steps = steps,
				substepTime = substepTime,
				timeLeft = timeLeft,
				activeConstraintCount = m_ConstraintCount
			}, m_ConstraintCount, 16, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return new ApplyPinConstraintsBatchJob
			{
				particleIndices = particleIndices,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				orientations = base.solverImplementation.orientations,
				orientationDeltas = base.solverImplementation.orientationDeltas,
				orientationCounts = base.solverImplementation.orientationConstraintCounts,
				sorFactor = constraintParameters.SORFactor,
				activeConstraintCount = m_ConstraintCount
			}.Schedule(inputDeps);
		}

		public JobHandle ProjectRenderablePositions(JobHandle inputDeps)
		{
			return IJobParallelForExtensions.Schedule(new ProjectRenderablePositionsJob
			{
				particleIndices = particleIndices,
				colliderIndices = colliderIndices,
				offsets = offsets,
				stiffnesses = stiffnesses,
				restDarboux = restDarbouxVectors,
				transforms = ObiColliderWorld.GetInstance().colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				renderablePositions = base.solverImplementation.renderablePositions,
				renderableOrientations = base.solverImplementation.renderableOrientations,
				inertialFrame = ((BurstSolverImpl)base.constraints.solver).inertialFrame
			}, m_ConstraintCount, 16, inputDeps);
		}
	}
}
