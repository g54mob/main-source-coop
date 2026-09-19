using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiDistanceConstraintsBatch : ObiConstraintsBatch, IStructuralConstraintBatch
	{
		[NonSerialized]
		protected IDistanceConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public ObiNativeFloatList restLengths = new ObiNativeFloatList();

		[HideInInspector]
		public ObiNativeVector2List stiffnesses = new ObiNativeVector2List();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Distance;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiDistanceConstraintsBatch(ObiDistanceConstraintsData constraints = null)
		{
		}

		public void AddConstraint(Vector2Int indices, float restLength)
		{
			RegisterConstraint();
			particleIndices.Add(indices[0]);
			particleIndices.Add(indices[1]);
			restLengths.Add(restLength);
			stiffnesses.Add(Vector2.zero);
		}

		public override void Clear()
		{
			base.Clear();
			restLengths.Clear();
			stiffnesses.Clear();
		}

		public float GetRestLength(int index)
		{
			return restLengths[index];
		}

		public void SetRestLength(int index, float restLength)
		{
			restLengths[index] = restLength;
		}

		public ParticlePair GetParticleIndices(int index)
		{
			return new ParticlePair(particleIndices[index * 2], particleIndices[index * 2 + 1]);
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
			particles.Add(particleIndices[index * 2]);
			particles.Add(particleIndices[index * 2 + 1]);
		}

		protected override void CopyConstraint(ObiConstraintsBatch batch, int constraintIndex)
		{
			if (batch is ObiDistanceConstraintsBatch)
			{
				ObiDistanceConstraintsBatch obiDistanceConstraintsBatch = batch as ObiDistanceConstraintsBatch;
				RegisterConstraint();
				particleIndices.Add(batch.particleIndices[constraintIndex * 2]);
				particleIndices.Add(batch.particleIndices[constraintIndex * 2 + 1]);
				restLengths.Add(obiDistanceConstraintsBatch.restLengths[constraintIndex]);
				stiffnesses.Add(obiDistanceConstraintsBatch.stiffnesses[constraintIndex]);
				ActivateConstraint(base.constraintCount - 1);
			}
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			particleIndices.Swap(sourceIndex * 2, destIndex * 2);
			particleIndices.Swap(sourceIndex * 2 + 1, destIndex * 2 + 1);
			restLengths.Swap(sourceIndex, destIndex);
			stiffnesses.Swap(sourceIndex, destIndex);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiDistanceConstraintsBatch obiDistanceConstraintsBatch = other as ObiDistanceConstraintsBatch;
			IDistanceConstraintsUser distanceConstraintsUser = actor as IDistanceConstraintsUser;
			if (obiDistanceConstraintsBatch != null && distanceConstraintsUser != null && distanceConstraintsUser.distanceConstraintsEnabled)
			{
				particleIndices.ResizeUninitialized((m_ActiveConstraintCount + obiDistanceConstraintsBatch.activeConstraintCount) * 2);
				restLengths.ResizeUninitialized(m_ActiveConstraintCount + obiDistanceConstraintsBatch.activeConstraintCount);
				stiffnesses.ResizeUninitialized(m_ActiveConstraintCount + obiDistanceConstraintsBatch.activeConstraintCount);
				lambdas.ResizeInitialized(m_ActiveConstraintCount + obiDistanceConstraintsBatch.activeConstraintCount, 0f);
				for (int i = 0; i < obiDistanceConstraintsBatch.activeConstraintCount * 2; i++)
				{
					particleIndices[m_ActiveConstraintCount * 2 + i] = actor.solverIndices[obiDistanceConstraintsBatch.particleIndices[i]];
				}
				for (int j = 0; j < obiDistanceConstraintsBatch.activeConstraintCount; j++)
				{
					float num = obiDistanceConstraintsBatch.restLengths[j] * distanceConstraintsUser.stretchingScale;
					restLengths[m_ActiveConstraintCount + j] = num;
					stiffnesses[m_ActiveConstraintCount + j] = new Vector2(distanceConstraintsUser.stretchCompliance, distanceConstraintsUser.maxCompression * num);
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IDistanceConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetDistanceConstraints(particleIndices, restLengths, stiffnesses, lambdas, m_ActiveConstraintCount);
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			restLengths.Dispose();
			stiffnesses.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}
	}
}
