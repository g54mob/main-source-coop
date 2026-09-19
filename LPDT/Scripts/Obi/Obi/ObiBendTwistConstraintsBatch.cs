using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiBendTwistConstraintsBatch : ObiConstraintsBatch
	{
		protected IBendTwistConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public ObiNativeQuaternionList restDarbouxVectors = new ObiNativeQuaternionList();

		[HideInInspector]
		public ObiNativeVector3List stiffnesses = new ObiNativeVector3List();

		[HideInInspector]
		public ObiNativeVector2List plasticity = new ObiNativeVector2List();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.BendTwist;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiBendTwistConstraintsBatch(ObiBendTwistConstraintsData constraints = null)
		{
		}

		public void AddConstraint(Vector2Int indices, Quaternion restDarboux)
		{
			RegisterConstraint();
			particleIndices.Add(indices[0]);
			particleIndices.Add(indices[1]);
			restDarbouxVectors.Add(restDarboux);
			stiffnesses.Add(Vector3.zero);
			plasticity.Add(Vector2.zero);
		}

		public override void Clear()
		{
			base.Clear();
			particleIndices.Clear();
			restDarbouxVectors.Clear();
			stiffnesses.Clear();
			plasticity.Clear();
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
			particles.Add(particleIndices[index * 2]);
			particles.Add(particleIndices[index * 2 + 1]);
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			particleIndices.Swap(sourceIndex * 2, destIndex * 2);
			particleIndices.Swap(sourceIndex * 2 + 1, destIndex * 2 + 1);
			restDarbouxVectors.Swap(sourceIndex, destIndex);
			stiffnesses.Swap(sourceIndex, destIndex);
			plasticity.Swap(sourceIndex, destIndex);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiBendTwistConstraintsBatch obiBendTwistConstraintsBatch = other as ObiBendTwistConstraintsBatch;
			IBendTwistConstraintsUser bendTwistConstraintsUser = actor as IBendTwistConstraintsUser;
			if (obiBendTwistConstraintsBatch != null && bendTwistConstraintsUser != null && bendTwistConstraintsUser.bendTwistConstraintsEnabled)
			{
				particleIndices.ResizeUninitialized((m_ActiveConstraintCount + obiBendTwistConstraintsBatch.activeConstraintCount) * 2);
				restDarbouxVectors.ResizeUninitialized(m_ActiveConstraintCount + obiBendTwistConstraintsBatch.activeConstraintCount);
				stiffnesses.ResizeUninitialized(m_ActiveConstraintCount + obiBendTwistConstraintsBatch.activeConstraintCount);
				plasticity.ResizeUninitialized(m_ActiveConstraintCount + obiBendTwistConstraintsBatch.activeConstraintCount);
				lambdas.ResizeInitialized((m_ActiveConstraintCount + obiBendTwistConstraintsBatch.activeConstraintCount) * 3, 0f);
				restDarbouxVectors.CopyFrom(obiBendTwistConstraintsBatch.restDarbouxVectors, 0, m_ActiveConstraintCount, obiBendTwistConstraintsBatch.activeConstraintCount);
				for (int i = 0; i < obiBendTwistConstraintsBatch.activeConstraintCount; i++)
				{
					stiffnesses[m_ActiveConstraintCount + i] = bendTwistConstraintsUser.GetBendTwistCompliance(obiBendTwistConstraintsBatch, i);
					plasticity[m_ActiveConstraintCount + i] = bendTwistConstraintsUser.GetBendTwistPlasticity(obiBendTwistConstraintsBatch, i);
				}
				for (int j = 0; j < obiBendTwistConstraintsBatch.activeConstraintCount * 2; j++)
				{
					particleIndices[m_ActiveConstraintCount * 2 + j] = actor.solverIndices[obiBendTwistConstraintsBatch.particleIndices[j]];
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IBendTwistConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetBendTwistConstraints(particleIndices, restDarbouxVectors, stiffnesses, plasticity, lambdas, m_ActiveConstraintCount);
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			restDarbouxVectors.Dispose();
			plasticity.Dispose();
			stiffnesses.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}
	}
}
