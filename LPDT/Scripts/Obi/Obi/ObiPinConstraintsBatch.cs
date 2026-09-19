using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiPinConstraintsBatch : ObiConstraintsBatch
	{
		protected IPinConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public List<ObiColliderHandle> pinBodies = new List<ObiColliderHandle>();

		[HideInInspector]
		public ObiNativeIntList colliderIndices = new ObiNativeIntList();

		[HideInInspector]
		public ObiNativeVector4List offsets = new ObiNativeVector4List();

		[HideInInspector]
		public ObiNativeQuaternionList restDarbouxVectors = new ObiNativeQuaternionList();

		[HideInInspector]
		public ObiNativeFloatList stiffnesses = new ObiNativeFloatList();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Pin;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiPinConstraintsBatch(ObiPinConstraintsData constraints = null)
		{
		}

		public void AddConstraint(int solverIndex, ObiColliderBase body, Vector3 offset, Quaternion restDarboux, float linearCompliance, float rotationalCompliance, bool projectRenderable = false)
		{
			RegisterConstraint();
			particleIndices.Add(solverIndex);
			pinBodies.Add((body != null) ? body.Handle : new ObiColliderHandle());
			colliderIndices.Add((body != null) ? body.Handle.index : (-1));
			offsets.Add(new Vector4(offset.x, offset.y, offset.z, projectRenderable ? 1 : 0));
			restDarbouxVectors.Add(restDarboux);
			stiffnesses.Add(linearCompliance);
			stiffnesses.Add(rotationalCompliance);
		}

		public override void Clear()
		{
			base.Clear();
			particleIndices.Clear();
			pinBodies.Clear();
			colliderIndices.Clear();
			offsets.Clear();
			restDarbouxVectors.Clear();
			stiffnesses.Clear();
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
			particles.Add(particleIndices[index]);
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			particleIndices.Swap(sourceIndex, destIndex);
			pinBodies.Swap(sourceIndex, destIndex);
			colliderIndices.Swap(sourceIndex, destIndex);
			offsets.Swap(sourceIndex, destIndex);
			restDarbouxVectors.Swap(sourceIndex, destIndex);
			stiffnesses.Swap(sourceIndex * 2, destIndex * 2);
			stiffnesses.Swap(sourceIndex * 2 + 1, destIndex * 2 + 1);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			if (other is ObiPinConstraintsBatch obiPinConstraintsBatch)
			{
				particleIndices.ResizeUninitialized(m_ActiveConstraintCount + obiPinConstraintsBatch.activeConstraintCount);
				colliderIndices.ResizeUninitialized(m_ActiveConstraintCount + obiPinConstraintsBatch.activeConstraintCount);
				offsets.ResizeUninitialized(m_ActiveConstraintCount + obiPinConstraintsBatch.activeConstraintCount);
				restDarbouxVectors.ResizeUninitialized(m_ActiveConstraintCount + obiPinConstraintsBatch.activeConstraintCount);
				stiffnesses.ResizeUninitialized((m_ActiveConstraintCount + obiPinConstraintsBatch.activeConstraintCount) * 2);
				lambdas.ResizeInitialized((m_ActiveConstraintCount + obiPinConstraintsBatch.activeConstraintCount) * 4, 0f);
				offsets.CopyFrom(obiPinConstraintsBatch.offsets, 0, m_ActiveConstraintCount, obiPinConstraintsBatch.activeConstraintCount);
				restDarbouxVectors.CopyFrom(obiPinConstraintsBatch.restDarbouxVectors, 0, m_ActiveConstraintCount, obiPinConstraintsBatch.activeConstraintCount);
				stiffnesses.CopyFrom(obiPinConstraintsBatch.stiffnesses, 0, m_ActiveConstraintCount * 2, obiPinConstraintsBatch.activeConstraintCount * 2);
				for (int i = 0; i < obiPinConstraintsBatch.activeConstraintCount; i++)
				{
					particleIndices[m_ActiveConstraintCount + i] = obiPinConstraintsBatch.particleIndices[i];
					colliderIndices[m_ActiveConstraintCount + i] = ((obiPinConstraintsBatch.pinBodies[i] != null) ? obiPinConstraintsBatch.pinBodies[i].index : (-1));
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			if (solver != null && solver.implementation != null)
			{
				m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IPinConstraintsBatchImpl;
				if (m_BatchImpl != null)
				{
					m_BatchImpl.SetPinConstraints(particleIndices, colliderIndices, offsets, restDarbouxVectors, stiffnesses, lambdas, m_ActiveConstraintCount);
				}
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			restDarbouxVectors.Dispose();
			colliderIndices.Dispose();
			offsets.Dispose();
			stiffnesses.Dispose();
			if (solver != null && solver.implementation != null)
			{
				solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
			}
		}
	}
}
