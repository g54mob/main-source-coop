using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiVolumeConstraintsBatch : ObiConstraintsBatch
	{
		protected IVolumeConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public ObiNativeIntList firstTriangle = new ObiNativeIntList();

		[HideInInspector]
		public ObiNativeIntList numTriangles = new ObiNativeIntList();

		[HideInInspector]
		public ObiNativeFloatList restVolumes = new ObiNativeFloatList();

		[HideInInspector]
		public ObiNativeVector2List pressureStiffness = new ObiNativeVector2List();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Volume;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiVolumeConstraintsBatch(ObiVolumeConstraintsData constraints = null)
		{
		}

		public void AddConstraint(int[] triangles, float restVolume)
		{
			RegisterConstraint();
			firstTriangle.Add(particleIndices.count / 3);
			numTriangles.Add(triangles.Length / 3);
			restVolumes.Add(restVolume);
			pressureStiffness.Add(new Vector2(1f, 0f));
			particleIndices.AddRange(triangles);
		}

		public override void Clear()
		{
			base.Clear();
			particleIndices.Clear();
			firstTriangle.Clear();
			numTriangles.Clear();
			restVolumes.Clear();
			pressureStiffness.Clear();
		}

		public override void GetParticlesInvolved(int index, List<int> particles)
		{
		}

		protected override void SwapConstraints(int sourceIndex, int destIndex)
		{
			firstTriangle.Swap(sourceIndex, destIndex);
			numTriangles.Swap(sourceIndex, destIndex);
			restVolumes.Swap(sourceIndex, destIndex);
			pressureStiffness.Swap(sourceIndex, destIndex);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			ObiVolumeConstraintsBatch obiVolumeConstraintsBatch = other as ObiVolumeConstraintsBatch;
			IVolumeConstraintsUser volumeConstraintsUser = actor as IVolumeConstraintsUser;
			if (obiVolumeConstraintsBatch != null && volumeConstraintsUser != null && volumeConstraintsUser.volumeConstraintsEnabled)
			{
				int count = particleIndices.count;
				int num = 0;
				for (int i = 0; i < obiVolumeConstraintsBatch.constraintCount; i++)
				{
					num += obiVolumeConstraintsBatch.numTriangles[i];
				}
				particleIndices.ResizeUninitialized(count + num * 3);
				firstTriangle.ResizeUninitialized(firstTriangle.count + obiVolumeConstraintsBatch.activeConstraintCount);
				numTriangles.ResizeUninitialized(numTriangles.count + obiVolumeConstraintsBatch.activeConstraintCount);
				restVolumes.ResizeUninitialized(m_ActiveConstraintCount + obiVolumeConstraintsBatch.activeConstraintCount);
				pressureStiffness.ResizeUninitialized(m_ActiveConstraintCount + obiVolumeConstraintsBatch.activeConstraintCount);
				lambdas.ResizeInitialized(m_ActiveConstraintCount + obiVolumeConstraintsBatch.activeConstraintCount, 0f);
				numTriangles.CopyFrom(obiVolumeConstraintsBatch.numTriangles, 0, m_ActiveConstraintCount, obiVolumeConstraintsBatch.activeConstraintCount);
				restVolumes.CopyFrom(obiVolumeConstraintsBatch.restVolumes, 0, m_ActiveConstraintCount, obiVolumeConstraintsBatch.activeConstraintCount);
				pressureStiffness.CopyReplicate(new Vector2(volumeConstraintsUser.pressure, volumeConstraintsUser.compressionCompliance), m_ActiveConstraintCount, obiVolumeConstraintsBatch.activeConstraintCount);
				for (int j = 0; j < num * 3; j++)
				{
					particleIndices[count + j] = actor.solverIndices[obiVolumeConstraintsBatch.particleIndices[j]];
				}
				for (int k = 0; k < obiVolumeConstraintsBatch.activeConstraintCount + 1; k++)
				{
					firstTriangle[m_ActiveConstraintCount + k] = count / 3 + obiVolumeConstraintsBatch.firstTriangle[k];
				}
				base.Merge(actor, other);
			}
		}

		public override void AddToSolver(ObiSolver solver)
		{
			m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IVolumeConstraintsBatchImpl;
			if (m_BatchImpl != null)
			{
				m_BatchImpl.SetVolumeConstraints(particleIndices, firstTriangle, numTriangles, restVolumes, pressureStiffness, lambdas, m_ActiveConstraintCount);
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			firstTriangle.Dispose();
			numTriangles.Dispose();
			restVolumes.Dispose();
			pressureStiffness.Dispose();
			solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
		}

		public void SetParameters(float compliance, float pressure)
		{
			Vector2 value = new Vector2(pressure, compliance);
			for (int i = 0; i < pressureStiffness.count; i++)
			{
				pressureStiffness[i] = value;
			}
		}
	}
}
