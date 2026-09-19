using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Obi
{
	[CreateAssetMenu(fileName = "rope blueprint", menuName = "Obi/Rope Blueprint", order = 140)]
	public class ObiRopeBlueprint : ObiRopeBlueprintBase
	{
		public int pooledParticles = 100;

		public const float DEFAULT_PARTICLE_MASS = 0.1f;

		protected override IEnumerator Initialize()
		{
			if (path.ControlPointCount < 2)
			{
				ClearParticleGroups();
				path.InsertControlPoint(0, Vector3.left, Vector3.left * 0.25f, Vector3.right * 0.25f, Vector3.up, 0.1f, 1f, 1f, ObiUtils.MakeFilter(65535, 1), Color.white, "control point");
				path.InsertControlPoint(1, Vector3.right, Vector3.left * 0.25f, Vector3.right * 0.25f, Vector3.up, 0.1f, 1f, 1f, ObiUtils.MakeFilter(65535, 1), Color.white, "control point");
			}
			path.RecalculateLenght(Matrix4x4.identity, 1E-05f, 7);
			List<Vector3> particlePositions = new List<Vector3>();
			List<float> particleThicknesses = new List<float>();
			List<float> particleInvMasses = new List<float>();
			List<int> particleFilters = new List<int>();
			List<Color> particleColors = new List<Color>();
			if (!path.Closed)
			{
				particlePositions.Add(path.points.GetPositionAtMu(path.Closed, 0f));
				particleThicknesses.Add(path.thicknesses.GetAtMu(path.Closed, 0f));
				particleInvMasses.Add(ObiUtils.MassToInvMass(path.masses.GetAtMu(path.Closed, 0f)));
				particleFilters.Add(path.filters.GetAtMu(path.Closed, 0f));
				particleColors.Add(path.colors.GetAtMu(path.Closed, 0f));
			}
			groups[0].particleIndices.Clear();
			groups[0].particleIndices.Add(0);
			ReadOnlyCollection<float> lengthTable = path.ArcLengthTable;
			int spans = path.GetSpanCount();
			for (int i = 0; i < spans; i++)
			{
				int index = i * (path.ArcLengthSamples + 1);
				int index2 = (i + 1) * (path.ArcLengthSamples + 1);
				float num = lengthTable[index];
				float num2 = lengthTable[index2] - num;
				int num3 = 1 + Mathf.FloorToInt(num2 / thickness * resolution);
				float num4 = num2 / (float)num3;
				for (int j = 0; j < num3; j++)
				{
					float muAtLenght = path.GetMuAtLenght(num + num4 * (float)(j + 1));
					particlePositions.Add(path.points.GetPositionAtMu(path.Closed, muAtLenght));
					particleThicknesses.Add(path.thicknesses.GetAtMu(path.Closed, muAtLenght));
					particleInvMasses.Add(ObiUtils.MassToInvMass(path.masses.GetAtMu(path.Closed, muAtLenght)));
					particleFilters.Add(path.filters.GetAtMu(path.Closed, muAtLenght));
					particleColors.Add(path.colors.GetAtMu(path.Closed, muAtLenght));
				}
				if (!path.Closed || i != spans - 1)
				{
					groups[i + 1].particleIndices.Clear();
					groups[i + 1].particleIndices.Add(particlePositions.Count - 1);
				}
				if (i % 100 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope: generating particles...", (float)i / (float)spans);
				}
			}
			m_ActiveParticleCount = particlePositions.Count;
			totalParticles = m_ActiveParticleCount + pooledParticles;
			int numSegments = m_ActiveParticleCount - ((!path.Closed) ? 1 : 0);
			if (numSegments > 0)
			{
				m_InterParticleDistance = path.Length / (float)numSegments;
			}
			else
			{
				m_InterParticleDistance = 0f;
			}
			positions = new Vector3[totalParticles];
			restPositions = new Vector4[totalParticles];
			velocities = new Vector3[totalParticles];
			invMasses = new float[totalParticles];
			principalRadii = new Vector3[totalParticles];
			filters = new int[totalParticles];
			colors = new Color[totalParticles];
			restLengths = new float[totalParticles];
			for (int i = 0; i < m_ActiveParticleCount; i++)
			{
				invMasses[i] = particleInvMasses[i];
				positions[i] = particlePositions[i];
				restPositions[i] = positions[i];
				restPositions[i][3] = 1f;
				principalRadii[i] = Vector3.one * particleThicknesses[i] * thickness;
				filters[i] = particleFilters[i];
				colors[i] = particleColors[i];
				if (i % 100 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope: generating particles...", (float)i / (float)m_ActiveParticleCount);
				}
			}
			CreateDeformableEdges(numSegments);
			CreateSimplices(numSegments);
			IEnumerator dc = CreateDistanceConstraints();
			while (dc.MoveNext())
			{
				yield return dc.Current;
			}
			IEnumerator bc = CreateBendingConstraints();
			while (bc.MoveNext())
			{
				yield return bc.Current;
			}
			IEnumerator ac = CreateAerodynamicConstraints();
			while (ac.MoveNext())
			{
				yield return ac.Current;
			}
			m_RestLength = 0f;
			float[] array = restLengths;
			foreach (float num5 in array)
			{
				m_RestLength += num5;
			}
		}

		protected virtual IEnumerator CreateDistanceConstraints()
		{
			distanceConstraintsData = new ObiDistanceConstraintsData();
			distanceConstraintsData.AddBatch(new ObiDistanceConstraintsBatch());
			distanceConstraintsData.AddBatch(new ObiDistanceConstraintsBatch());
			for (int i = 0; i < totalParticles - 1; i++)
			{
				ObiDistanceConstraintsBatch obiDistanceConstraintsBatch = distanceConstraintsData.batches[i % 2];
				if (i < m_ActiveParticleCount - 1)
				{
					Vector2Int indices = new Vector2Int(i, i + 1);
					restLengths[i] = Vector3.Distance(positions[indices.x], positions[indices.y]);
					obiDistanceConstraintsBatch.AddConstraint(indices, restLengths[i]);
					obiDistanceConstraintsBatch.activeConstraintCount++;
				}
				else
				{
					restLengths[i] = m_InterParticleDistance;
					obiDistanceConstraintsBatch.AddConstraint(Vector2Int.zero, 0f);
				}
				if (i % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope: generating structural constraints...", (float)i / (float)(totalParticles - 1));
				}
			}
			if (path.Closed)
			{
				ObiDistanceConstraintsBatch obiDistanceConstraintsBatch2 = new ObiDistanceConstraintsBatch();
				distanceConstraintsData.AddBatch(obiDistanceConstraintsBatch2);
				Vector2Int indices2 = new Vector2Int(m_ActiveParticleCount - 1, 0);
				restLengths[m_ActiveParticleCount - 2] = Vector3.Distance(positions[indices2.x], positions[indices2.y]);
				obiDistanceConstraintsBatch2.AddConstraint(indices2, restLengths[m_ActiveParticleCount - 2]);
				obiDistanceConstraintsBatch2.activeConstraintCount++;
			}
		}

		protected virtual IEnumerator CreateBendingConstraints()
		{
			bendConstraintsData = new ObiBendConstraintsData();
			bendConstraintsData.AddBatch(new ObiBendConstraintsBatch());
			bendConstraintsData.AddBatch(new ObiBendConstraintsBatch());
			bendConstraintsData.AddBatch(new ObiBendConstraintsBatch());
			for (int i = 0; i < totalParticles - 2; i++)
			{
				ObiBendConstraintsBatch obiBendConstraintsBatch = bendConstraintsData.batches[i % 3];
				Vector3Int indices = new Vector3Int(i, i + 2, i + 1);
				float restBend = 0f;
				obiBendConstraintsBatch.AddConstraint(indices, restBend);
				if (i < m_ActiveParticleCount - 2)
				{
					obiBendConstraintsBatch.activeConstraintCount++;
				}
				if (i % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope: generating structural constraints...", (float)i / (float)(totalParticles - 2));
				}
			}
			if (path.Closed)
			{
				ObiBendConstraintsBatch obiBendConstraintsBatch2 = new ObiBendConstraintsBatch();
				bendConstraintsData.AddBatch(obiBendConstraintsBatch2);
				Vector3Int indices2 = new Vector3Int(m_ActiveParticleCount - 2, 0, m_ActiveParticleCount - 1);
				obiBendConstraintsBatch2.AddConstraint(indices2, 0f);
				obiBendConstraintsBatch2.activeConstraintCount++;
				ObiBendConstraintsBatch obiBendConstraintsBatch3 = new ObiBendConstraintsBatch();
				bendConstraintsData.AddBatch(obiBendConstraintsBatch3);
				indices2 = new Vector3Int(m_ActiveParticleCount - 1, 1, 0);
				obiBendConstraintsBatch3.AddConstraint(indices2, 0f);
				obiBendConstraintsBatch3.activeConstraintCount++;
			}
		}
	}
}
