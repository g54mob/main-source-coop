using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiChainConstraintsBatch : ObiConstraintsBatch
	{
		protected IChainConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public ObiNativeIntList firstParticle = new ObiNativeIntList();

		[HideInInspector]
		public ObiNativeIntList numParticles = new ObiNativeIntList();

		[HideInInspector]
		public ObiNativeVector2List lengths = new ObiNativeVector2List();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Chain;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiChainConstraintsBatch(ObiChainConstraintsData constraints = null)
		{
		}

		public void AddConstraint(int[] indices, float restLength, float stretchStiffness, float compressionStiffness)
		{
			RegisterConstraint();
			firstParticle.Add(particleIndices.count);
			numParticles.Add(indices.Length);
			particleIndices.AddRange(indices);
			lengths.Add(new Vector2(restLength, restLength));
		}

		public override void Clear()
		{
			base.Clear();
			particleIndices.Clear();
			firstParticle.Clear();
			numParticles.Clear();
			lengths.Clear();
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			firstParticle.Swap(sourceIndex, destIndex);
			numParticles.Swap(sourceIndex, destIndex);
			lengths.Swap(sourceIndex, destIndex);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiChainConstraintsBatch obiChainConstraintsBatch = other as ObiChainConstraintsBatch;
			IChainConstraintsUser chainConstraintsUser = actor as IChainConstraintsUser;
			if (obiChainConstraintsBatch != null && chainConstraintsUser != null && chainConstraintsUser.chainConstraintsEnabled)
			{
				int count = particleIndices.count;
				int num = 0;
				for (int i = 0; i < obiChainConstraintsBatch.activeConstraintCount; i++)
				{
					num += obiChainConstraintsBatch.numParticles[i];
				}
				particleIndices.ResizeUninitialized(count + num);
				firstParticle.ResizeUninitialized(m_ActiveConstraintCount + obiChainConstraintsBatch.activeConstraintCount);
				numParticles.ResizeUninitialized(m_ActiveConstraintCount + obiChainConstraintsBatch.activeConstraintCount);
				lengths.ResizeUninitialized(m_ActiveConstraintCount + obiChainConstraintsBatch.activeConstraintCount);
				lambdas.ResizeInitialized(m_ActiveConstraintCount + obiChainConstraintsBatch.activeConstraintCount, 0f);
				numParticles.CopyFrom(obiChainConstraintsBatch.numParticles, 0, m_ActiveConstraintCount, obiChainConstraintsBatch.activeConstraintCount);
				for (int j = 0; j < num; j++)
				{
					particleIndices[count + j] = actor.solverIndices[obiChainConstraintsBatch.particleIndices[j]];
				}
				for (int k = 0; k < obiChainConstraintsBatch.activeConstraintCount; k++)
				{
					firstParticle[m_ActiveConstraintCount + k] = obiChainConstraintsBatch.firstParticle[k] + count;
					lengths[m_ActiveConstraintCount + k] = new Vector2(obiChainConstraintsBatch.lengths[k].y * chainConstraintsUser.tightness, obiChainConstraintsBatch.lengths[k].y);
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IChainConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetChainConstraints(particleIndices, lengths, firstParticle, numParticles, m_ActiveConstraintCount);
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			firstParticle.Dispose();
			numParticles.Dispose();
			lengths.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}
	}
}
