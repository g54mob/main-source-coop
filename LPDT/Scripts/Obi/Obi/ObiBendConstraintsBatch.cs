using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiBendConstraintsBatch : ObiConstraintsBatch
	{
		protected IBendConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public ObiNativeFloatList restBends = new ObiNativeFloatList();

		[HideInInspector]
		public ObiNativeVector2List bendingStiffnesses = new ObiNativeVector2List();

		[HideInInspector]
		public ObiNativeVector2List plasticity = new ObiNativeVector2List();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Bending;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiBendConstraintsBatch(ObiBendConstraintsData constraints = null)
		{
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiBendConstraintsBatch obiBendConstraintsBatch = other as ObiBendConstraintsBatch;
			IBendConstraintsUser bendConstraintsUser = actor as IBendConstraintsUser;
			if (obiBendConstraintsBatch != null && bendConstraintsUser != null && bendConstraintsUser.bendConstraintsEnabled)
			{
				particleIndices.ResizeUninitialized((m_ActiveConstraintCount + obiBendConstraintsBatch.activeConstraintCount) * 3);
				restBends.ResizeUninitialized(m_ActiveConstraintCount + obiBendConstraintsBatch.activeConstraintCount);
				bendingStiffnesses.ResizeUninitialized(m_ActiveConstraintCount + obiBendConstraintsBatch.activeConstraintCount);
				plasticity.ResizeUninitialized(m_ActiveConstraintCount + obiBendConstraintsBatch.activeConstraintCount);
				lambdas.ResizeInitialized(m_ActiveConstraintCount + obiBendConstraintsBatch.activeConstraintCount, 0f);
				restBends.CopyFrom(obiBendConstraintsBatch.restBends, 0, m_ActiveConstraintCount, obiBendConstraintsBatch.activeConstraintCount);
				bendingStiffnesses.CopyReplicate(new Vector2(bendConstraintsUser.maxBending, bendConstraintsUser.bendCompliance), m_ActiveConstraintCount, obiBendConstraintsBatch.activeConstraintCount);
				plasticity.CopyReplicate(new Vector2(bendConstraintsUser.plasticYield, bendConstraintsUser.plasticCreep), m_ActiveConstraintCount, obiBendConstraintsBatch.activeConstraintCount);
				for (int i = 0; i < obiBendConstraintsBatch.activeConstraintCount * 3; i++)
				{
					particleIndices[m_ActiveConstraintCount * 3 + i] = actor.solverIndices[obiBendConstraintsBatch.particleIndices[i]];
				}
				base.Merge(actor, other);
			}
		}

		public void AddConstraint(Vector3Int indices, float restBend)
		{
			RegisterConstraint();
			particleIndices.Add(indices[0]);
			particleIndices.Add(indices[1]);
			particleIndices.Add(indices[2]);
			restBends.Add(restBend);
			bendingStiffnesses.Add(Vector2.zero);
			plasticity.Add(Vector2.zero);
		}

		public override void Clear()
		{
			base.Clear();
			restBends.Clear();
			bendingStiffnesses.Clear();
			plasticity.Clear();
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
			particles.Add(particleIndices[index * 3]);
			particles.Add(particleIndices[index * 3 + 1]);
			particles.Add(particleIndices[index * 3 + 2]);
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			particleIndices.Swap(sourceIndex * 3, destIndex * 3);
			particleIndices.Swap(sourceIndex * 3 + 1, destIndex * 3 + 1);
			particleIndices.Swap(sourceIndex * 3 + 2, destIndex * 3 + 2);
			restBends.Swap(sourceIndex, destIndex);
			bendingStiffnesses.Swap(sourceIndex, destIndex);
			plasticity.Swap(sourceIndex, destIndex);
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IBendConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetBendConstraints(particleIndices, restBends, bendingStiffnesses, plasticity, lambdas, m_ActiveConstraintCount);
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			restBends.Dispose();
			bendingStiffnesses.Dispose();
			plasticity.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}
	}
}
