using System.Collections;
using UnityEngine;

namespace Obi
{
	public abstract class ObiRopeBlueprintBase : ObiActorBlueprint
	{
		[HideInInspector]
		[SerializeField]
		public ObiPath path = new ObiPath();

		public float thickness = 0.1f;

		[Range(0f, 1f)]
		public float resolution = 1f;

		[HideInInspector]
		[SerializeField]
		protected float m_InterParticleDistance;

		[HideInInspector]
		[SerializeField]
		protected int totalParticles;

		[HideInInspector]
		[SerializeField]
		protected float m_RestLength;

		[HideInInspector]
		public int[] deformableEdges;

		[HideInInspector]
		public float[] restLengths;

		public float interParticleDistance => m_InterParticleDistance;

		public float restLength => m_RestLength;

		public void OnEnable()
		{
			path.OnPathChanged.AddListener(base.GenerateImmediate);
			path.OnControlPointAdded.AddListener(ControlPointAdded);
			path.OnControlPointRemoved.AddListener(ControlPointRemoved);
			path.OnControlPointRenamed.AddListener(ControlPointRenamed);
		}

		public void OnDisable()
		{
			path.OnPathChanged.RemoveAllListeners();
			path.OnControlPointAdded.RemoveAllListeners();
			path.OnControlPointRemoved.RemoveAllListeners();
			path.OnControlPointRenamed.RemoveAllListeners();
		}

		protected void ControlPointAdded(int index)
		{
			InsertNewParticleGroup(path.GetName(index), index);
		}

		protected void ControlPointRenamed(int index)
		{
			SetParticleGroupName(index, path.GetName(index));
		}

		protected void ControlPointRemoved(int index)
		{
			RemoveParticleGroupAt(index);
		}

		protected virtual IEnumerator CreateAerodynamicConstraints()
		{
			aerodynamicConstraintsData = new ObiAerodynamicConstraintsData();
			ObiAerodynamicConstraintsBatch aeroBatch = new ObiAerodynamicConstraintsBatch();
			aerodynamicConstraintsData.AddBatch(aeroBatch);
			for (int i = 0; i < totalParticles; i++)
			{
				aeroBatch.AddConstraint(i, 2f * principalRadii[i].x, 1f, 1f);
				if (i % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope generating aerodynamic constraints...", (float)i / (float)totalParticles);
				}
			}
			for (int j = 0; j < aerodynamicConstraintsData.batches.Count; j++)
			{
				aerodynamicConstraintsData.batches[j].activeConstraintCount = m_ActiveParticleCount;
			}
		}

		protected void CreateDeformableEdges(int numSegments)
		{
			deformableEdges = new int[numSegments * 2];
			for (int i = 0; i < numSegments; i++)
			{
				deformableEdges[i * 2] = i % base.activeParticleCount;
				deformableEdges[i * 2 + 1] = (i + 1) % base.activeParticleCount;
			}
		}

		protected void CreateSimplices(int numSegments)
		{
			edges = new int[numSegments * 2];
			for (int i = 0; i < numSegments; i++)
			{
				edges[i * 2] = i % base.activeParticleCount;
				edges[i * 2 + 1] = (i + 1) % base.activeParticleCount;
			}
		}

		protected override IEnumerator Initialize()
		{
			yield return null;
		}
	}
}
