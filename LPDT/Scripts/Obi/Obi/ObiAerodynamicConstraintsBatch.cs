using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiAerodynamicConstraintsBatch : ObiConstraintsBatch
	{
		protected IAerodynamicConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public ObiNativeFloatList aerodynamicCoeffs = new ObiNativeFloatList();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Aerodynamics;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiAerodynamicConstraintsBatch(ObiAerodynamicConstraintsData constraints = null)
		{
		}

		public void AddConstraint(int index, float area, float drag, float lift)
		{
			RegisterConstraint();
			particleIndices.Add(index);
			aerodynamicCoeffs.Add(area);
			aerodynamicCoeffs.Add(drag);
			aerodynamicCoeffs.Add(lift);
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
			particles.Add(particleIndices[index]);
		}

		public override void Clear()
		{
			base.Clear();
			particleIndices.Clear();
			aerodynamicCoeffs.Clear();
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			particleIndices.Swap(sourceIndex, destIndex);
			aerodynamicCoeffs.Swap(sourceIndex * 3, destIndex * 3);
			aerodynamicCoeffs.Swap(sourceIndex * 3 + 1, destIndex * 3 + 1);
			aerodynamicCoeffs.Swap(sourceIndex * 3 + 2, destIndex * 3 + 2);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiAerodynamicConstraintsBatch obiAerodynamicConstraintsBatch = other as ObiAerodynamicConstraintsBatch;
			IAerodynamicConstraintsUser aerodynamicConstraintsUser = actor as IAerodynamicConstraintsUser;
			if (obiAerodynamicConstraintsBatch != null && aerodynamicConstraintsUser != null && aerodynamicConstraintsUser.aerodynamicsEnabled)
			{
				particleIndices.ResizeUninitialized(m_ActiveConstraintCount + obiAerodynamicConstraintsBatch.activeConstraintCount);
				aerodynamicCoeffs.ResizeUninitialized((m_ActiveConstraintCount + obiAerodynamicConstraintsBatch.activeConstraintCount) * 3);
				for (int i = 0; i < obiAerodynamicConstraintsBatch.activeConstraintCount; i++)
				{
					particleIndices[m_ActiveConstraintCount + i] = actor.solverIndices[obiAerodynamicConstraintsBatch.particleIndices[i]];
					aerodynamicCoeffs[(m_ActiveConstraintCount + i) * 3] = obiAerodynamicConstraintsBatch.aerodynamicCoeffs[i * 3];
					aerodynamicCoeffs[(m_ActiveConstraintCount + i) * 3 + 1] = aerodynamicConstraintsUser.GetDrag(obiAerodynamicConstraintsBatch, i);
					aerodynamicCoeffs[(m_ActiveConstraintCount + i) * 3 + 2] = aerodynamicConstraintsUser.GetLift(obiAerodynamicConstraintsBatch, i);
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IAerodynamicConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetAerodynamicConstraints(particleIndices, aerodynamicCoeffs, m_ActiveConstraintCount);
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			aerodynamicCoeffs.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}
	}
}
