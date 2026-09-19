using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Obi
{
	public class BurstShapeMatchingConstraintsBatch : BurstConstraintsBatchImpl, IShapeMatchingConstraintsBatchImpl, IConstraintsBatchImpl
	{
		[BurstCompile]
		public struct ShapeMatchingCalculateRestJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> firstIndex;

			[ReadOnly]
			public NativeArray<int> numIndices;

			public NativeArray<float4> restComs;

			[ReadOnly]
			public NativeArray<float4> coms;

			public NativeArray<float4x4> Aqq;

			[ReadOnly]
			public NativeArray<float4x4> deformation;

			[ReadOnly]
			public NativeArray<float4> restPositions;

			[ReadOnly]
			public NativeArray<quaternion> restOrientations;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<float> invRotationalMasses;

			[ReadOnly]
			public NativeArray<float4> principalRadii;

			public void Execute(int i)
			{
				RecalculateRestData(i, ref particleIndices, ref firstIndex, ref restComs, ref Aqq, ref deformation, ref numIndices, ref invMasses, ref invRotationalMasses, ref restPositions, ref restOrientations, ref principalRadii);
			}
		}

		[BurstCompile]
		public struct ShapeMatchingConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> firstIndex;

			[ReadOnly]
			public NativeArray<int> numIndices;

			[ReadOnly]
			public NativeArray<int> explicitGroup;

			[ReadOnly]
			public NativeArray<float> shapeMaterialParameters;

			public NativeArray<float4> restComs;

			public NativeArray<float4> coms;

			public NativeArray<quaternion> constraintOrientations;

			public NativeArray<float4x4> Aqq;

			public NativeArray<float4x4> linearTransforms;

			public NativeArray<float4x4> deformation;

			[ReadOnly]
			public NativeArray<float4> positions;

			[ReadOnly]
			public NativeArray<float4> restPositions;

			[ReadOnly]
			public NativeArray<quaternion> restOrientations;

			[ReadOnly]
			public NativeArray<float> invMasses;

			[ReadOnly]
			public NativeArray<float> invRotationalMasses;

			[ReadOnly]
			public NativeArray<float4> principalRadii;

			[NativeDisableContainerSafetyRestriction]
			[NativeDisableParallelForRestriction]
			public NativeArray<quaternion> orientations;

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
				float num = 10000f;
				coms[i] = float4.zero;
				float4x4 zero = float4x4.zero;
				float4x4 zero2 = float4x4.zero;
				for (int j = 0; j < numIndices[i]; j++)
				{
					int index = particleIndices[firstIndex[i] + j];
					float num2 = num;
					if (invMasses[index] > 1f / num)
					{
						num2 = 1f / invMasses[index];
					}
					coms[i] += positions[index] * num2;
					float4x4 a = orientations[index].toMatrix();
					float4x4 v = restOrientations[index].toMatrix();
					a[3][3] = 0f;
					v[3][3] = 0f;
					zero2 += math.mul(a, math.mul(BurstMath.GetParticleInertiaTensor(principalRadii[index], invRotationalMasses[index]).asDiagonal(), math.transpose(v)));
					float4 row = restPositions[index];
					row[3] = 0f;
					zero += num2 * BurstMath.multrnsp4(positions[index], row);
				}
				if (restComs[i][3] < 1E-07f)
				{
					return;
				}
				coms[i] /= restComs[i][3];
				float4 row2 = restComs[i];
				row2[3] = 0f;
				zero -= restComs[i][3] * BurstMath.multrnsp4(coms[i], row2);
				float4x4 float4x5 = zero2 + math.mul(zero, math.transpose(deformation[i]));
				float4x5[3][3] = 1f;
				linearTransforms[i] = math.mul(float4x5, Aqq[i]);
				constraintOrientations[i] = BurstMath.ExtractRotation(float4x5, constraintOrientations[i], 5);
				float4x4 float4x6 = constraintOrientations[i].toMatrix();
				float4x6[3][3] = 0f;
				if (explicitGroup[i] > 0)
				{
					for (int k = 0; k < numIndices[i]; k++)
					{
						int index = particleIndices[firstIndex[i] + k];
						orientations[index] = math.mul(constraintOrientations[i], restOrientations[index]);
					}
				}
				else
				{
					int index2 = particleIndices[firstIndex[i]];
					orientations[index2] = math.mul(constraintOrientations[i], restOrientations[index2]);
				}
				float4x4 a2 = math.mul(float4x6, deformation[i]);
				for (int l = 0; l < numIndices[i]; l++)
				{
					int index = particleIndices[firstIndex[i] + l];
					float4 float5 = coms[i] + math.mul(a2, restPositions[index] - restComs[i]);
					deltas[index] += (float5 - positions[index]) * shapeMaterialParameters[i * 5];
					counts[index]++;
				}
				float num3 = shapeMaterialParameters[i * 5 + 1];
				float num4 = shapeMaterialParameters[i * 5 + 2];
				float num5 = shapeMaterialParameters[i * 5 + 3];
				float num6 = shapeMaterialParameters[i * 5 + 4];
				if (num4 > 0f)
				{
					float4x6[3][3] = 1f;
					float4x4 float4x7 = math.mul(math.transpose(float4x6), linearTransforms[i]) - float4x4.identity;
					float num7 = float4x7.frobeniusNorm();
					if (num7 > num3)
					{
						deformation[i] = math.mul(float4x4.identity + num4 * float4x7, deformation[i]);
						float4x7 = deformation[i] - float4x4.identity;
						num7 = float4x7.frobeniusNorm();
						if (num7 > num6)
						{
							deformation[i] = float4x4.identity + num6 * float4x7 / num7;
						}
						if (num5 == 0f)
						{
							RecalculateRestData(i, ref particleIndices, ref firstIndex, ref restComs, ref Aqq, ref deformation, ref numIndices, ref invMasses, ref invRotationalMasses, ref restPositions, ref restOrientations, ref principalRadii);
						}
					}
				}
				if (num5 > 0f)
				{
					deformation[i] += (float4x4.identity - deformation[i]) * math.min(num5 * deltaTime, 1f);
					RecalculateRestData(i, ref particleIndices, ref firstIndex, ref restComs, ref Aqq, ref deformation, ref numIndices, ref invMasses, ref invRotationalMasses, ref restPositions, ref restOrientations, ref principalRadii);
				}
			}
		}

		[BurstCompile]
		public struct ApplyShapeMatchingConstraintsBatchJob : IJobParallelFor
		{
			[ReadOnly]
			public NativeArray<int> particleIndices;

			[ReadOnly]
			public NativeArray<int> firstIndex;

			[ReadOnly]
			public NativeArray<int> numIndices;

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
				int num = firstIndex[i];
				int num2 = num + numIndices[i];
				for (int j = num; j < num2; j++)
				{
					int index = particleIndices[j];
					if (counts[index] > 0)
					{
						positions[index] += deltas[index] * sorFactor / counts[index];
						deltas[index] = float4.zero;
						counts[index] = 0;
					}
				}
			}
		}

		private NativeArray<int> firstIndex;

		private NativeArray<int> numIndices;

		private NativeArray<int> explicitGroup;

		private NativeArray<float> shapeMaterialParameters;

		private NativeArray<float4> restComs;

		private NativeArray<float4> coms;

		private NativeArray<quaternion> constraintOrientations;

		private NativeArray<float4x4> Aqq;

		private NativeArray<float4x4> linearTransforms;

		private NativeArray<float4x4> plasticDeformations;

		private bool m_RecalculateRestShape;

		public BurstShapeMatchingConstraintsBatch(BurstShapeMatchingConstraints constraints)
		{
			m_Constraints = constraints;
			m_ConstraintType = Oni.ConstraintType.ShapeMatching;
		}

		public void SetShapeMatchingConstraints(ObiNativeIntList particleIndices, ObiNativeIntList firstIndex, ObiNativeIntList numIndices, ObiNativeIntList explicitGroup, ObiNativeFloatList shapeMaterialParameters, ObiNativeVector4List restComs, ObiNativeVector4List coms, ObiNativeQuaternionList constraintOrientations, ObiNativeMatrix4x4List linearTransforms, ObiNativeMatrix4x4List plasticDeformations, ObiNativeFloatList lambdas, int count)
		{
			base.particleIndices = particleIndices.AsNativeArray<int>();
			this.firstIndex = firstIndex.AsNativeArray<int>();
			this.numIndices = numIndices.AsNativeArray<int>();
			this.explicitGroup = explicitGroup.AsNativeArray<int>();
			this.shapeMaterialParameters = shapeMaterialParameters.AsNativeArray<float>();
			this.restComs = restComs.AsNativeArray<float4>();
			this.coms = coms.AsNativeArray<float4>();
			this.constraintOrientations = constraintOrientations.AsNativeArray<quaternion>();
			this.linearTransforms = linearTransforms.AsNativeArray<float4x4>();
			this.plasticDeformations = plasticDeformations.AsNativeArray<float4x4>();
			if (Aqq.IsCreated)
			{
				Aqq.Dispose();
			}
			Aqq = new NativeArray<float4x4>(count, Allocator.Persistent);
			m_ConstraintCount = count;
		}

		public override void Destroy()
		{
			if (Aqq.IsCreated)
			{
				Aqq.Dispose();
			}
		}

		public override JobHandle Initialize(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			return inputDeps;
		}

		public override JobHandle Evaluate(JobHandle inputDeps, float stepTime, float substepTime, int steps, float timeLeft)
		{
			if (m_RecalculateRestShape)
			{
				m_RecalculateRestShape = false;
				inputDeps = IJobParallelForExtensions.Schedule(new ShapeMatchingCalculateRestJob
				{
					particleIndices = particleIndices,
					firstIndex = firstIndex,
					numIndices = numIndices,
					restComs = restComs,
					coms = coms,
					Aqq = Aqq,
					deformation = plasticDeformations,
					restPositions = base.solverAbstraction.restPositions.AsNativeArray<float4>(),
					restOrientations = base.solverAbstraction.restOrientations.AsNativeArray<quaternion>(),
					principalRadii = base.solverAbstraction.principalRadii.AsNativeArray<float4>(),
					invMasses = base.solverAbstraction.invMasses.AsNativeArray<float>(),
					invRotationalMasses = base.solverAbstraction.invRotationalMasses.AsNativeArray<float>()
				}, numIndices.Length, 64, inputDeps);
			}
			return IJobParallelForExtensions.Schedule(new ShapeMatchingConstraintsBatchJob
			{
				particleIndices = particleIndices,
				firstIndex = firstIndex,
				numIndices = numIndices,
				explicitGroup = explicitGroup,
				shapeMaterialParameters = shapeMaterialParameters,
				restComs = restComs,
				coms = coms,
				constraintOrientations = constraintOrientations,
				Aqq = Aqq,
				linearTransforms = linearTransforms,
				deformation = plasticDeformations,
				positions = base.solverImplementation.positions,
				restPositions = base.solverImplementation.restPositions,
				orientations = base.solverImplementation.orientations,
				restOrientations = base.solverImplementation.restOrientations,
				invMasses = base.solverImplementation.invMasses,
				invRotationalMasses = base.solverImplementation.invRotationalMasses,
				principalRadii = base.solverImplementation.principalRadii,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				deltaTime = substepTime
			}, m_ConstraintCount, 4, inputDeps);
		}

		public override JobHandle Apply(JobHandle inputDeps, float substepTime)
		{
			Oni.ConstraintParameters constraintParameters = base.solverAbstraction.GetConstraintParameters(m_ConstraintType);
			return IJobParallelForExtensions.Schedule(new ApplyShapeMatchingConstraintsBatchJob
			{
				particleIndices = particleIndices,
				firstIndex = firstIndex,
				numIndices = numIndices,
				positions = base.solverImplementation.positions,
				deltas = base.solverImplementation.positionDeltas,
				counts = base.solverImplementation.positionConstraintCounts,
				sorFactor = constraintParameters.SORFactor
			}, m_ConstraintCount, 8, inputDeps);
		}

		public void CalculateRestShapeMatching()
		{
			m_RecalculateRestShape = true;
		}

		protected static void RecalculateRestData(int i, ref NativeArray<int> particleIndices, ref NativeArray<int> firstIndex, ref NativeArray<float4> restComs, ref NativeArray<float4x4> Aqq, ref NativeArray<float4x4> deformation, ref NativeArray<int> numIndices, ref NativeArray<float> invMasses, ref NativeArray<float> invRotationalMasses, ref NativeArray<float4> restPositions, ref NativeArray<quaternion> restOrientations, ref NativeArray<float4> principalRadii)
		{
			int num = 0;
			float num2 = 10000f;
			restComs[i] = float4.zero;
			Aqq[i] = float4x4.zero;
			float4 column = float4.zero;
			float4x4 zero = float4x4.zero;
			float4x4 zero2 = float4x4.zero;
			for (int j = 0; j < numIndices[i]; j++)
			{
				num = particleIndices[firstIndex[i] + j];
				float num3 = num2;
				if (invMasses[num] > 1f / num2)
				{
					num3 = 1f / invMasses[num];
				}
				column += restPositions[num] * num3;
				float4x4 float4x5 = restOrientations[num].toMatrix();
				float4x5[3][3] = 0f;
				zero2 += math.mul(float4x5, math.mul(BurstMath.GetParticleInertiaTensor(principalRadii[num], invRotationalMasses[num]).asDiagonal(), math.transpose(float4x5)));
				float4 column2 = restPositions[num];
				column2[3] = 0f;
				zero += num3 * BurstMath.multrnsp4(in column2, column2);
			}
			if (!(column[3] < 1E-07f))
			{
				column.xyz /= column[3];
				restComs[i] = column;
				column[3] = 0f;
				zero -= restComs[i][3] * BurstMath.multrnsp4(in column, column);
				zero[3][3] = 1f;
				Aqq[i] = math.inverse(zero2 + math.mul(deformation[i], math.mul(zero, math.transpose(deformation[i]))));
			}
		}
	}
}
