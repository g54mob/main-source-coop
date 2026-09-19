using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiTetherConstraintsBatch : ObiConstraintsBatch
	{
		protected ITetherConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public ObiNativeVector2List maxLengthsScales = new ObiNativeVector2List();

		[HideInInspector]
		public ObiNativeFloatList stiffnesses = new ObiNativeFloatList();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Tether;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiTetherConstraintsBatch(ObiTetherConstraintsData constraints = null)
		{
		}

		public void AddConstraint(Vector2Int indices, float maxLength, float scale)
		{
			RegisterConstraint();
			particleIndices.Add(indices[0]);
			particleIndices.Add(indices[1]);
			maxLengthsScales.Add(new Vector2(maxLength, scale));
			stiffnesses.Add(0f);
		}

		public override void Clear()
		{
			base.Clear();
			particleIndices.Clear();
			maxLengthsScales.Clear();
			stiffnesses.Clear();
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
			maxLengthsScales.Swap(sourceIndex, destIndex);
			stiffnesses.Swap(sourceIndex, destIndex);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiTetherConstraintsBatch obiTetherConstraintsBatch = other as ObiTetherConstraintsBatch;
			ITetherConstraintsUser tetherConstraintsUser = actor as ITetherConstraintsUser;
			if (obiTetherConstraintsBatch != null && tetherConstraintsUser != null && tetherConstraintsUser.tetherConstraintsEnabled)
			{
				particleIndices.ResizeUninitialized((m_ActiveConstraintCount + obiTetherConstraintsBatch.activeConstraintCount) * 2);
				maxLengthsScales.ResizeUninitialized(m_ActiveConstraintCount + obiTetherConstraintsBatch.activeConstraintCount);
				stiffnesses.ResizeUninitialized(m_ActiveConstraintCount + obiTetherConstraintsBatch.activeConstraintCount);
				lambdas.ResizeInitialized(m_ActiveConstraintCount + obiTetherConstraintsBatch.activeConstraintCount, 0f);
				stiffnesses.CopyReplicate(tetherConstraintsUser.tetherCompliance, m_ActiveConstraintCount, obiTetherConstraintsBatch.activeConstraintCount);
				for (int i = 0; i < obiTetherConstraintsBatch.activeConstraintCount * 2; i++)
				{
					particleIndices[m_ActiveConstraintCount * 2 + i] = actor.solverIndices[obiTetherConstraintsBatch.particleIndices[i]];
				}
				for (int j = 0; j < obiTetherConstraintsBatch.activeConstraintCount; j++)
				{
					maxLengthsScales[m_ActiveConstraintCount + j] = new Vector2(obiTetherConstraintsBatch.maxLengthsScales[j].x, tetherConstraintsUser.tetherScale);
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as ITetherConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetTetherConstraints(particleIndices, maxLengthsScales, stiffnesses, lambdas, m_ActiveConstraintCount);
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			maxLengthsScales.Dispose();
			stiffnesses.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}

		public void SetParameters(float compliance, float scale)
		{
			for (int i = 0; i < stiffnesses.count; i++)
			{
				stiffnesses[i] = compliance;
				maxLengthsScales[i] = new Vector2(maxLengthsScales[i].x, scale);
			}
		}
	}
}
