using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiShapeMatchingConstraintsBatch : ObiConstraintsBatch
	{
		protected IShapeMatchingConstraintsBatchImpl m_BatchImpl;

		public ObiNativeIntList firstIndex = new ObiNativeIntList();

		public ObiNativeIntList numIndices = new ObiNativeIntList();

		public ObiNativeIntList explicitGroup = new ObiNativeIntList();

		public ObiNativeFloatList materialParameters = new ObiNativeFloatList();

		public ObiNativeVector4List restComs = new ObiNativeVector4List();

		public ObiNativeVector4List coms = new ObiNativeVector4List();

		public ObiNativeQuaternionList orientations = new ObiNativeQuaternionList();

		public ObiNativeMatrix4x4List linearTransforms = new ObiNativeMatrix4x4List();

		public ObiNativeMatrix4x4List plasticDeformations = new ObiNativeMatrix4x4List();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.ShapeMatching;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiShapeMatchingConstraintsBatch(ObiShapeMatchingConstraintsData constraints = null)
		{
		}

		public void AddConstraint(int[] indices, bool isExplicit)
		{
			RegisterConstraint();
			firstIndex.Add(particleIndices.count);
			numIndices.Add(indices.Length);
			explicitGroup.Add(isExplicit ? 1 : 0);
			particleIndices.AddRange(indices);
			materialParameters.AddRange(new float[5] { 1f, 1f, 1f, 1f, 1f });
		}

		public override void Clear()
		{
			base.Clear();
			firstIndex.Clear();
			numIndices.Clear();
			explicitGroup.Clear();
			particleIndices.Clear();
			materialParameters.Clear();
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
			int num = firstIndex[index];
			int num2 = numIndices[index];
			for (int i = num; i < num + num2; i++)
			{
				particles.Add(particleIndices[i]);
			}
		}

		public void RemoveParticleFromConstraint(int constraintIndex, int particleIndex)
		{
			int num = firstIndex[constraintIndex];
			int num2 = numIndices[constraintIndex];
			int num3 = 0;
			for (int num4 = num + num2 - 1; num4 >= num; num4--)
			{
				if (particleIndices[num4] == particleIndex)
				{
					num3++;
					particleIndices.RemoveAt(num4);
				}
			}
			numIndices[constraintIndex] -= num3;
			for (int i = constraintIndex + 1; i < base.constraintCount; i++)
			{
				firstIndex[i] -= num3;
			}
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			firstIndex.Swap(sourceIndex, destIndex);
			numIndices.Swap(sourceIndex, destIndex);
			explicitGroup.Swap(sourceIndex, destIndex);
			for (int i = 0; i < 5; i++)
			{
				materialParameters.Swap(sourceIndex * 5 + i, destIndex * 5 + i);
			}
			restComs.Swap(sourceIndex, destIndex);
			coms.Swap(sourceIndex, destIndex);
			orientations.Swap(sourceIndex, destIndex);
			linearTransforms.Swap(sourceIndex, destIndex);
			plasticDeformations.Swap(sourceIndex, destIndex);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiShapeMatchingConstraintsBatch obiShapeMatchingConstraintsBatch = other as ObiShapeMatchingConstraintsBatch;
			IShapeMatchingConstraintsUser shapeMatchingConstraintsUser = actor as IShapeMatchingConstraintsUser;
			if (obiShapeMatchingConstraintsBatch != null && shapeMatchingConstraintsUser != null && shapeMatchingConstraintsUser.shapeMatchingConstraintsEnabled)
			{
				int count = particleIndices.count;
				int num = 0;
				for (int i = 0; i < obiShapeMatchingConstraintsBatch.constraintCount; i++)
				{
					num += obiShapeMatchingConstraintsBatch.numIndices[i];
				}
				particleIndices.ResizeUninitialized(count + num);
				firstIndex.ResizeUninitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount);
				numIndices.ResizeUninitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount);
				explicitGroup.ResizeUninitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount);
				materialParameters.ResizeUninitialized((m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount) * 5);
				restComs.ResizeUninitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount);
				coms.ResizeUninitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount);
				orientations.ResizeUninitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount);
				linearTransforms.ResizeUninitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount);
				plasticDeformations.ResizeInitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount, Matrix4x4.identity);
				lambdas.ResizeInitialized(m_ActiveConstraintCount + obiShapeMatchingConstraintsBatch.activeConstraintCount, 0f);
				numIndices.CopyFrom(obiShapeMatchingConstraintsBatch.numIndices, 0, m_ActiveConstraintCount, obiShapeMatchingConstraintsBatch.activeConstraintCount);
				explicitGroup.CopyFrom(obiShapeMatchingConstraintsBatch.explicitGroup, 0, m_ActiveConstraintCount, obiShapeMatchingConstraintsBatch.activeConstraintCount);
				orientations.CopyReplicate(actor.actorLocalToSolverMatrix.rotation, m_ActiveConstraintCount, obiShapeMatchingConstraintsBatch.activeConstraintCount);
				for (int j = 0; j < num; j++)
				{
					particleIndices[count + j] = actor.solverIndices[obiShapeMatchingConstraintsBatch.particleIndices[j]];
				}
				for (int k = 0; k < obiShapeMatchingConstraintsBatch.activeConstraintCount; k++)
				{
					firstIndex[m_ActiveConstraintCount + k] = obiShapeMatchingConstraintsBatch.firstIndex[k] + count;
					materialParameters[(m_ActiveConstraintCount + k) * 5] = obiShapeMatchingConstraintsBatch.materialParameters[k * 5] * shapeMatchingConstraintsUser.deformationResistance;
					materialParameters[(m_ActiveConstraintCount + k) * 5 + 1] = obiShapeMatchingConstraintsBatch.materialParameters[k * 5 + 1] * shapeMatchingConstraintsUser.plasticYield;
					materialParameters[(m_ActiveConstraintCount + k) * 5 + 2] = obiShapeMatchingConstraintsBatch.materialParameters[k * 5 + 2] * shapeMatchingConstraintsUser.plasticCreep;
					materialParameters[(m_ActiveConstraintCount + k) * 5 + 3] = obiShapeMatchingConstraintsBatch.materialParameters[k * 5 + 3] * shapeMatchingConstraintsUser.plasticRecovery;
					materialParameters[(m_ActiveConstraintCount + k) * 5 + 4] = obiShapeMatchingConstraintsBatch.materialParameters[k * 5 + 4] * shapeMatchingConstraintsUser.maxDeformation;
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IShapeMatchingConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetShapeMatchingConstraints(particleIndices, firstIndex, numIndices, explicitGroup, materialParameters, restComs, coms, orientations, linearTransforms, plasticDeformations, lambdas, m_ActiveConstraintCount);
				m_BatchImpl.CalculateRestShapeMatching();
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			firstIndex.Dispose();
			numIndices.Dispose();
			explicitGroup.Dispose();
			materialParameters.Dispose();
			restComs.Dispose();
			coms.Dispose();
			orientations.Dispose();
			linearTransforms.Dispose();
			plasticDeformations.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}

		public void RecalculateRestShapeMatching()
		{
			if (m_BatchImpl != null)
			{
				m_BatchImpl.CalculateRestShapeMatching();
			}
		}
	}
}
