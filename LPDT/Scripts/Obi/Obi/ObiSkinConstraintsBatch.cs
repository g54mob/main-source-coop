using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiSkinConstraintsBatch : ObiConstraintsBatch
	{
		protected ISkinConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public ObiNativeVector4List skinPoints = new ObiNativeVector4List();

		[HideInInspector]
		public ObiNativeVector4List skinNormals = new ObiNativeVector4List();

		[HideInInspector]
		public ObiNativeFloatList skinRadiiBackstop = new ObiNativeFloatList();

		[HideInInspector]
		public ObiNativeFloatList skinCompliance = new ObiNativeFloatList();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Skin;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiSkinConstraintsBatch(ObiSkinConstraintsData constraints = null)
		{
		}

		public void AddConstraint(int index, Vector4 point, Vector4 normal, float radius, float collisionRadius, float backstop, float stiffness)
		{
			RegisterConstraint();
			particleIndices.Add(index);
			skinPoints.Add(point);
			skinNormals.Add(normal);
			skinRadiiBackstop.Add(radius);
			skinRadiiBackstop.Add(collisionRadius);
			skinRadiiBackstop.Add(backstop);
			skinCompliance.Add(stiffness);
		}

		public override void Clear()
		{
			base.Clear();
			particleIndices.Clear();
			skinPoints.Clear();
			skinNormals.Clear();
			skinRadiiBackstop.Clear();
			skinCompliance.Clear();
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
			particles.Add(particleIndices[index]);
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			particleIndices.Swap(sourceIndex, destIndex);
			skinPoints.Swap(sourceIndex, destIndex);
			skinNormals.Swap(sourceIndex, destIndex);
			skinRadiiBackstop.Swap(sourceIndex * 3, destIndex * 3);
			skinRadiiBackstop.Swap(sourceIndex * 3 + 1, destIndex * 3 + 1);
			skinRadiiBackstop.Swap(sourceIndex * 3 + 2, destIndex * 3 + 2);
			skinCompliance.Swap(sourceIndex, destIndex);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiSkinConstraintsBatch obiSkinConstraintsBatch = other as ObiSkinConstraintsBatch;
			ISkinConstraintsUser skinConstraintsUser = actor as ISkinConstraintsUser;
			if (obiSkinConstraintsBatch != null && skinConstraintsUser != null && skinConstraintsUser.skinConstraintsEnabled)
			{
				particleIndices.ResizeUninitialized(m_ActiveConstraintCount + obiSkinConstraintsBatch.activeConstraintCount);
				skinPoints.ResizeUninitialized(m_ActiveConstraintCount + obiSkinConstraintsBatch.activeConstraintCount);
				skinNormals.ResizeUninitialized(m_ActiveConstraintCount + obiSkinConstraintsBatch.activeConstraintCount);
				skinRadiiBackstop.ResizeUninitialized((m_ActiveConstraintCount + obiSkinConstraintsBatch.activeConstraintCount) * 3);
				skinCompliance.ResizeUninitialized(m_ActiveConstraintCount + obiSkinConstraintsBatch.activeConstraintCount);
				lambdas.ResizeInitialized(m_ActiveConstraintCount + obiSkinConstraintsBatch.activeConstraintCount, 0f);
				skinPoints.CopyFrom(obiSkinConstraintsBatch.skinPoints, 0, m_ActiveConstraintCount, obiSkinConstraintsBatch.activeConstraintCount);
				skinNormals.CopyFrom(obiSkinConstraintsBatch.skinNormals, 0, m_ActiveConstraintCount, obiSkinConstraintsBatch.activeConstraintCount);
				for (int i = 0; i < obiSkinConstraintsBatch.activeConstraintCount; i++)
				{
					Vector3 vector = skinConstraintsUser.GetSkinRadiiBackstop(obiSkinConstraintsBatch, i);
					skinRadiiBackstop[(m_ActiveConstraintCount + i) * 3] = vector.x;
					skinRadiiBackstop[(m_ActiveConstraintCount + i) * 3 + 1] = vector.y;
					skinRadiiBackstop[(m_ActiveConstraintCount + i) * 3 + 2] = vector.z;
					skinCompliance[m_ActiveConstraintCount + i] = skinConstraintsUser.GetSkinCompliance(obiSkinConstraintsBatch, i);
				}
				for (int j = 0; j < obiSkinConstraintsBatch.activeConstraintCount; j++)
				{
					particleIndices[m_ActiveConstraintCount + j] = actor.solverIndices[obiSkinConstraintsBatch.particleIndices[j]];
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as ISkinConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetSkinConstraints(particleIndices, skinPoints, skinNormals, skinRadiiBackstop, skinCompliance, lambdas, m_ActiveConstraintCount);
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			skinPoints.Dispose();
			skinNormals.Dispose();
			skinRadiiBackstop.Dispose();
			skinCompliance.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}
	}
}
