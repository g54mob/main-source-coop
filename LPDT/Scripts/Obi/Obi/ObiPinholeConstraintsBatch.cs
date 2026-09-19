using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[Serializable]
	public class ObiPinholeConstraintsBatch : ObiConstraintsBatch
	{
		[Serializable]
		public struct PinholeEdge
		{
			public int edgeIndex;

			public float coordinate;

			public PinholeEdge(int edgeIndex, float coordinate)
			{
				this.edgeIndex = edgeIndex;
				this.coordinate = coordinate;
			}

			public float GetRopeCoordinate(ObiActor actor)
			{
				int deformableEdgeCount = actor.GetDeformableEdgeCount();
				if (deformableEdgeCount <= 0)
				{
					return 0f;
				}
				return Mathf.Clamp01(((float)edgeIndex + coordinate) / (float)deformableEdgeCount);
			}
		}

		protected IPinholeConstraintsBatchImpl m_BatchImpl;

		[HideInInspector]
		public List<ObiColliderHandle> pinBodies = new List<ObiColliderHandle>();

		[HideInInspector]
		public List<ObiActor> pinActors = new List<ObiActor>();

		[HideInInspector]
		public ObiNativeIntList colliderIndices = new ObiNativeIntList();

		[HideInInspector]
		public ObiNativeVector4List offsets = new ObiNativeVector4List();

		[HideInInspector]
		public ObiNativeFloatList edgeMus = new ObiNativeFloatList();

		[HideInInspector]
		public ObiNativeIntList edgeRanges = new ObiNativeIntList();

		[HideInInspector]
		public ObiNativeFloatList edgeRangeMus = new ObiNativeFloatList();

		[HideInInspector]
		public ObiNativeFloatList parameters = new ObiNativeFloatList();

		[HideInInspector]
		public ObiNativeFloatList relativeVelocities = new ObiNativeFloatList();

		public override Oni.ConstraintType constraintType => Oni.ConstraintType.Pinhole;

		public override IConstraintsBatchImpl implementation => m_BatchImpl;

		public ObiPinholeConstraintsBatch(ObiPinholeConstraintsData constraints = null)
		{
		}

		public void AddConstraint(PinholeEdge edge, PinholeEdge firstEdge, PinholeEdge lastEdge, ObiActor actor, ObiColliderBase body, Vector3 offset, float compliance, float friction, float motorSpeed, float motorForce, bool clampAtEnds)
		{
			RegisterConstraint();
			particleIndices.Add(edge.edgeIndex);
			edgeMus.Add(edge.coordinate);
			edgeRanges.Add(firstEdge.edgeIndex);
			edgeRanges.Add(lastEdge.edgeIndex);
			edgeRangeMus.Add(firstEdge.coordinate);
			edgeRangeMus.Add(lastEdge.coordinate);
			pinBodies.Add((body != null) ? body.Handle : new ObiColliderHandle());
			pinActors.Add(actor);
			colliderIndices.Add((body != null) ? body.Handle.index : (-1));
			offsets.Add(offset);
			parameters.Add(compliance);
			parameters.Add(friction);
			parameters.Add(motorSpeed);
			parameters.Add(motorForce);
			parameters.Add(clampAtEnds ? 1 : 0);
			relativeVelocities.Add(0f);
		}

		public override void Clear()
		{
			base.Clear();
			particleIndices.Clear();
			pinBodies.Clear();
			colliderIndices.Clear();
			offsets.Clear();
			edgeMus.Clear();
			edgeRanges.Clear();
			edgeRangeMus.Clear();
			parameters.Clear();
			relativeVelocities.Clear();
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
			edgeMus.Swap(sourceIndex, destIndex);
			edgeRanges.Swap(sourceIndex * 2, destIndex * 2);
			edgeRanges.Swap(sourceIndex * 2 + 1, destIndex * 2 + 1);
			edgeRangeMus.Swap(sourceIndex * 2, destIndex * 2);
			edgeRangeMus.Swap(sourceIndex * 2 + 1, destIndex * 2 + 1);
			for (int i = 0; i < 5; i++)
			{
				parameters.Swap(sourceIndex * 5 + i, destIndex * 5 + i);
			}
			relativeVelocities.Swap(sourceIndex, destIndex);
		}

		public override void Merge(ObiActor actor, IObiConstraintsBatch other)
		{
			if (!(other is ObiPinholeConstraintsBatch obiPinholeConstraintsBatch))
			{
				return;
			}
			particleIndices.ResizeUninitialized(m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount);
			colliderIndices.ResizeUninitialized(m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount);
			offsets.ResizeUninitialized(m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount);
			edgeRanges.ResizeUninitialized((m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount) * 2);
			edgeRangeMus.ResizeUninitialized((m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount) * 2);
			edgeMus.ResizeUninitialized(m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount);
			parameters.ResizeUninitialized((m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount) * 5);
			relativeVelocities.ResizeInitialized(m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount, 0f);
			lambdas.ResizeInitialized(m_ActiveConstraintCount + obiPinholeConstraintsBatch.activeConstraintCount, 0f);
			edgeMus.CopyFrom(obiPinholeConstraintsBatch.edgeMus, 0, m_ActiveConstraintCount, obiPinholeConstraintsBatch.activeConstraintCount);
			edgeRangeMus.CopyFrom(obiPinholeConstraintsBatch.edgeRangeMus, 0, m_ActiveConstraintCount * 2, obiPinholeConstraintsBatch.activeConstraintCount * 2);
			offsets.CopyFrom(obiPinholeConstraintsBatch.offsets, 0, m_ActiveConstraintCount, obiPinholeConstraintsBatch.activeConstraintCount);
			relativeVelocities.CopyFrom(obiPinholeConstraintsBatch.relativeVelocities, 0, m_ActiveConstraintCount, obiPinholeConstraintsBatch.activeConstraintCount);
			parameters.CopyFrom(obiPinholeConstraintsBatch.parameters, 0, m_ActiveConstraintCount * 5, obiPinholeConstraintsBatch.activeConstraintCount * 5);
			for (int i = 0; i < obiPinholeConstraintsBatch.activeConstraintCount; i++)
			{
				int value = -1;
				int num = -1;
				int num2 = -1;
				if (obiPinholeConstraintsBatch.pinActors[i] != null)
				{
					value = obiPinholeConstraintsBatch.pinActors[i].deformableEdgesOffset + obiPinholeConstraintsBatch.particleIndices[i];
					num = obiPinholeConstraintsBatch.pinActors[i].deformableEdgesOffset + obiPinholeConstraintsBatch.edgeRanges[i * 2];
					num2 = obiPinholeConstraintsBatch.pinActors[i].deformableEdgesOffset + obiPinholeConstraintsBatch.edgeRanges[i * 2 + 1];
				}
				edgeRanges[(m_ActiveConstraintCount + i) * 2] = num;
				edgeRanges[(m_ActiveConstraintCount + i) * 2 + 1] = num2;
				particleIndices[m_ActiveConstraintCount + i] = Mathf.Clamp(value, num, num2);
				colliderIndices[m_ActiveConstraintCount + i] = ((obiPinholeConstraintsBatch.pinBodies[i] != null) ? obiPinholeConstraintsBatch.pinBodies[i].index : (-1));
			}
			base.Merge(actor, other);
		}

		public override void AddToSolver(ObiSolver solver)
		{
			if (solver != null && solver.implementation != null)
			{
				m_BatchImpl = solver.implementation.CreateConstraintsBatch(constraintType) as IPinholeConstraintsBatchImpl;
				if (m_BatchImpl != null)
				{
					m_BatchImpl.SetPinholeConstraints(particleIndices, colliderIndices, offsets, edgeMus, edgeRanges, edgeRangeMus, parameters, relativeVelocities, lambdas, m_ActiveConstraintCount);
				}
			}
		}

		public override void RemoveFromSolver(ObiSolver solver)
		{
			base.RemoveFromSolver(solver);
			edgeRanges.Dispose();
			colliderIndices.Dispose();
			offsets.Dispose();
			parameters.Dispose();
			if (solver != null && solver.implementation != null)
			{
				solver.implementation.DestroyConstraintsBatch(m_BatchImpl);
			}
		}
	}
}
