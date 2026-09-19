using System.Threading;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstPinholeConstraintsBatch : BurstConstraintsBatchImpl, IPinholeConstraintsBatchImpl, IConstraintsBatchImpl
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
			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int2> edgeRanges;

			[ReadOnly]
			public NativeArray<float2> edgeRangeMus;

			[ReadOnly]
			public NativeArray<float4> offsets;

			[ReadOnly]
			public NativeArray<float> parameters;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float> edgeMus;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float> relativeVelocities;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<int> deformableEdges;

			[ReadOnly]
			public NativeArray<int> colliderIndices;

			[ReadOnly]
			public NativeArray<BurstColliderShape> shapes;

			[ReadOnly]
			public NativeArray<BurstAffineTransform> transforms;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<BurstRigidbody> rigidbodies;

			[ReadOnly]
			public NativeArray<float4> rigidbodyLinearDeltas;

			[ReadOnly]
			public NativeArray<float4> rigidbodyAngularDeltas;

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

			private bool IsEdgeValid(int edgeIndex, int nextEdgeIndex, float mix)
			{
				if (!(mix < 0f))
				{
					return deformableEdges[nextEdgeIndex * 2] == deformableEdges[edgeIndex * 2 + 1];
				}
				return deformableEdges[nextEdgeIndex * 2 + 1] == deformableEdges[edgeIndex * 2];
			}

			private bool ClampToRange(int i, int edgeIndex, ref float mix)
			{
				bool result = false;
				if (edgeIndex == edgeRanges[i].x && mix < edgeRangeMus[i].x)
				{
					mix = edgeRangeMus[i].x;
					result = true;
				}
				if (edgeIndex == edgeRanges[i].y && mix > edgeRangeMus[i].y)
				{
					mix = edgeRangeMus[i].y;
					result = true;
				}
				return result;
			}

			public unsafe void Execute(int i)
			{
				int num = particleIndices[i];
				int num2 = colliderIndices[i];
				if (num2 < 0 || num < 0)
				{
					return;
				}
				int rigidbodyIndex = shapes[num2].rigidbodyIndex;
				if (rigidbodyIndex >= 0)
				{
					BurstRigidbody* unsafePtr = (BurstRigidbody*)rigidbodies.GetUnsafePtr();
					Interlocked.Increment(ref unsafePtr[rigidbodyIndex].constraintCount);
				}
				float dt = stepTime * (float)steps;
				float t = timeLeft / substepTime;
				int index = deformableEdges[num * 2];
				int index2 = deformableEdges[num * 2 + 1];
				int num3 = math.max(0, edgeRanges[i].y - edgeRanges[i].x + 1);
				float4 float5 = transforms[num2].TransformPoint(offsets[i]);
				float4 float6 = float5;
				if (rigidbodyIndex >= 0)
				{
					float4 rigidbodyVelocityAtPoint = BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, inertialFrame.frame.InverseTransformPoint(float5), rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame);
					float6 = BurstIntegration.IntegrateLinear(float6, inertialFrame.frame.TransformVector(rigidbodyVelocityAtPoint), dt);
				}
				float4 p = inertialFrame.frame.InverseTransformPoint(float6);
				float4 float7 = math.lerp(prevPositions[index], positions[index], t);
				float4 float8 = math.lerp(prevPositions[index2], positions[index2], t);
				float num4 = math.length(float7 - float8) + 1E-07f;
				BurstMath.NearestPointOnEdge(float7, float8, p, out var mu, clampToSegment: false);
				float num5 = (mu - edgeMus[i]) / substepTime * num4;
				relativeVelocities[i] = num5;
				float valueToClamp = (parameters[i * 5 + 2] - num5) / substepTime;
				float num6 = parameters[i * 5 + 3] * math.max(math.lerp(invMasses[index], invMasses[index2], mu), 1E-07f);
				num5 += math.clamp(valueToClamp, 0f - num6, num6) * substepTime;
				float end = edgeMus[i] + num5 * substepTime / num4;
				mu = math.lerp(mu, end, parameters[i * 5 + 1]);
				if (!ClampToRange(i, num, ref mu) && (mu < 0f || mu > 1f))
				{
					bool flag = parameters[i * 5 + 4] > 0.5f;
					float num7 = math.length(float7 - float8) * ((mu < 0f) ? (0f - mu) : (mu - 1f));
					for (int j = 0; j < 10; j++)
					{
						int num8 = ((mu < 0f) ? (num - 1) : (num + 1));
						num8 = edgeRanges[i].x + (int)BurstMath.nfmod(num8 - edgeRanges[i].x, num3);
						if (!IsEdgeValid(num, num8, mu))
						{
							if (!flag)
							{
								particleIndices[i] = -1;
								return;
							}
							mu = math.saturate(mu);
							break;
						}
						num = num8;
						index = deformableEdges[num * 2];
						index2 = deformableEdges[num * 2 + 1];
						float7 = math.lerp(prevPositions[index], positions[index], t);
						float8 = math.lerp(prevPositions[index2], positions[index2], t);
						num4 = math.length(float7 - float8) + 1E-07f;
						if (num7 <= num4)
						{
							mu = ((mu < 0f) ? (1f - math.saturate(num7 / num4)) : math.saturate(num7 / num4));
							ClampToRange(i, num, ref mu);
							break;
						}
						if (ClampToRange(i, num, ref mu))
						{
							break;
						}
						num7 -= num4;
					}
				}
				edgeMus[i] = mu;
				particleIndices[i] = num;
			}
		}

		[BurstCompile]
		public struct PinholeConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> colliderIndices;

			[ReadOnly]
			public NativeArray<float4> offsets;

			[ReadOnly]
			public NativeArray<float> parameters;

			[ReadOnly]
			public NativeArray<float> edgeMus;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float> lambdas;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> prevPositions;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<int> deformableEdges;

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
				int num = particleIndices[i];
				int num2 = colliderIndices[i];
				if (num >= 0 && num2 >= 0)
				{
					float num3 = stepTime * (float)steps;
					float num4 = timeLeft / substepTime;
					float num5 = parameters[i * 5] / (substepTime * substepTime);
					int index = deformableEdges[num * 2];
					int index2 = deformableEdges[num * 2 + 1];
					float num6 = edgeMus[i];
					float4 start = math.lerp(prevPositions[index], positions[index], num4);
					float4 end = math.lerp(prevPositions[index2], positions[index2], num4);
					float4 float5 = math.lerp(start, end, num6);
					float4 float6 = transforms[num2].TransformPoint(offsets[i]);
					float4 float7 = float6;
					float num7 = 0f;
					float num8 = 0f;
					int rigidbodyIndex = shapes[num2].rigidbodyIndex;
					if (rigidbodyIndex >= 0)
					{
						BurstRigidbody burstRigidbody = rigidbodies[rigidbodyIndex];
						float4 rigidbodyVelocityAtPoint = BurstMath.GetRigidbodyVelocityAtPoint(rigidbodyIndex, inertialFrame.frame.InverseTransformPoint(float6), rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame);
						float7 = BurstIntegration.IntegrateLinear(float7, inertialFrame.frame.TransformVector(rigidbodyVelocityAtPoint), num3);
						num7 = burstRigidbody.inverseMass * (float)burstRigidbody.constraintCount;
						num8 = BurstMath.RotationalInvMass(burstRigidbody.inverseInertiaTensor, float6 - burstRigidbody.com, math.normalizesafe(inertialFrame.frame.TransformPoint(float5) - float7)) * (float)burstRigidbody.constraintCount;
					}
					float7 = inertialFrame.frame.InverseTransformPoint(float7);
					float4 obj = float5 - float7;
					float num9 = math.length(obj);
					float4 float8 = obj / (num9 + 1E-07f);
					float num10 = (0f - num9 - num5 * lambdas[i]) / (math.lerp(invMasses[index], invMasses[index2], num6) + num7 + num8 + num5 + 1E-07f);
					lambdas[i] += num10;
					float4 float9 = num10 * float8;
					float num11 = BurstMath.BaryScale(new float4(1f - num6, num6, 0f, 0f));
					deltas[index] += float9 * num11 * invMasses[index] * (1f - num6) / num4;
					counts[index]++;
					deltas[index2] += float9 * num11 * invMasses[index2] * num6 / num4;
					counts[index2]++;
					if (rigidbodyIndex >= 0)
					{
						BurstMath.ApplyImpulse(rigidbodyIndex, -float9 / num3, inertialFrame.frame.InverseTransformPoint(float6), rigidbodies, rigidbodyLinearDeltas, rigidbodyAngularDeltas, inertialFrame.frame);
					}
				}
			}
		}

		[BurstCompile]
		public struct ApplyPinholeConstraintsBatchJob : IJob
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public float sorFactor;

			[ReadOnly]
			public NativeArray<int> deformableEdges;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> positions;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<float4> deltas;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<int> counts;

			[ReadOnly]
			public int activeConstraintCount;

			public void Execute()
			{
				for (int i = 0; i < activeConstraintCount; i++)
				{
					int num = particleIndices[i];
					if (num >= 0)
					{
						int index = deformableEdges[num * 2];
						int index2 = deformableEdges[num * 2 + 1];
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
			}
		}

		private NativeArray<int> colliderIndices;

		private NativeArray<float4> offsets;

		private NativeArray<float> edgeMus;

		private NativeArray<int2> edgeRanges;

		private NativeArray<float2> edgeRangeMus;

		private NativeArray<float> parameters;

		private NativeArray<float> relativeVelocities;

		public BurstPinholeConstraintsBatch(BurstPinholeConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.Pinhole;
		}

		public void SetPinholeConstraints(ObiNativeIntList particleIndices, ObiNativeIntList colliderIndices, ObiNativeVector4List offsets, ObiNativeFloatList edgeMus, ObiNativeIntList edgeRanges, ObiNativeFloatList edgeRangeMus, ObiNativeFloatList parameters, ObiNativeFloatList relativeVelocities, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.colliderIndices = colliderIndices.AsNativeArray<int>();
			this.offsets = offsets.AsNativeArray<float4>();
			this.edgeMus = edgeMus.AsNativeArray<float>();
			this.edgeRanges = edgeRanges.AsNativeArray<int2>();
			this.edgeRangeMus = edgeRangeMus.AsNativeArray<float2>();
			this.parameters = parameters.AsNativeArray<float>();
			this.relativeVelocities = relativeVelocities.AsNativeArray<float>();
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
				particleIndices = particleIndices,
				colliderIndices = colliderIndices,
				offsets = offsets,
				edgeMus = edgeMus,
				edgeRangeMus = edgeRangeMus,
				relativeVelocities = relativeVelocities,
				parameters = parameters,
				edgeRanges = edgeRanges,
				positions = base.solverImplementation.positions,
				prevPositions = base.solverImplementation.prevPositions,
				invMasses = base.solverImplementation.invMasses,
				deformableEdges = base.solverImplementation.abstraction.deformableEdges.AsNativeArray<int>(),
				shapes = ObiColliderWorld.GetInstance().colliderShapes.AsNativeArray<BurstColliderShape>(),
				transforms = ObiColliderWorld.GetInstance().colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				rigidbodies = ObiColliderWorld.GetInstance().rigidbodies.AsNativeArray<BurstRigidbody>(),
				rigidbodyLinearDeltas = base.solverImplementation.abstraction.rigidbodyLinearDeltas.AsNativeArray<float4>(),
				rigidbodyAngularDeltas = base.solverImplementation.abstraction.rigidbodyAngularDeltas.AsNativeArray<float4>(),
				inertialFrame = ((BurstSolverImpl)base.constraints.solver).inertialFrame,
				stepTime = stepTime,
				steps = steps,
				substepTime = substepTime,
				timeLeft = timeLeft,
				activeConstraintCount = m_ConstraintCount
			}, m_ConstraintCount, 128, inputDeps);
			return base.Initialize(inputDeps, stepTime, substepTime, steps, timeLeft);
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return IJobParallelForExtensions.Schedule(new PinholeConstraintsBatchJob
			{
				particleIndices = particleIndices,
				colliderIndices = colliderIndices,
				offsets = offsets,
				edgeMus = edgeMus,
				parameters = parameters,
				lambdas = lambdas,
				positions = base.solverImplementation.positions,
				prevPositions = base.solverImplementation.prevPositions,
				invMasses = base.solverImplementation.invMasses,
				deformableEdges = base.solverImplementation.abstraction.deformableEdges.AsNativeArray<int>(),
				shapes = ObiColliderWorld.GetInstance().colliderShapes.AsNativeArray<BurstColliderShape>(),
				transforms = ObiColliderWorld.GetInstance().colliderTransforms.AsNativeArray<BurstAffineTransform>(),
				rigidbodies = ObiColliderWorld.GetInstance().rigidbodies.AsNativeArray<BurstRigidbody>(),
				rigidbodyLinearDeltas = base.solverImplementation.abstraction.rigidbodyLinearDeltas.AsNativeArray<float4>(),
				rigidbodyAngularDeltas = base.solverImplementation.abstraction.rigidbodyAngularDeltas.AsNativeArray<float4>(),
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
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
			return new ApplyPinholeConstraintsBatchJob
			{
				particleIndices = particleIndices,
				deformableEdges = base.solverImplementation.abstraction.deformableEdges.AsNativeArray<int>(),
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				sorFactor = constraintParameters.SORFactor,
				activeConstraintCount = m_ConstraintCount
			}.Schedule(inputDeps);
		}
	}
}
