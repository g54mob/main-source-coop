using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Obi
{
	[CreateAssetMenu(fileName = "rod blueprint", menuName = "Obi/Rod Blueprint", order = 141)]
	public class ObiRodBlueprint : ObiRopeBlueprintBase
	{
		public bool keepInitialShape = true;

		public const float DEFAULT_PARTICLE_MASS = 0.1f;

		public const float DEFAULT_PARTICLE_ROTATIONAL_MASS = 0.01f;

		protected override IEnumerator Initialize()
		{
			if (path.ControlPointCount < 2)
			{
				ClearParticleGroups();
				path.InsertControlPoint(0, Vector3.left, Vector3.left * 0.25f, Vector3.right * 0.25f, Vector3.up, 0.1f, 0.01f, 1f, ObiUtils.MakeFilter(65535, 1), Color.white, "control point");
				path.InsertControlPoint(1, Vector3.right, Vector3.left * 0.25f, Vector3.right * 0.25f, Vector3.up, 0.1f, 0.01f, 1f, ObiUtils.MakeFilter(65535, 1), Color.white, "control point");
			}
			path.RecalculateLenght(Matrix4x4.identity, 1E-05f, 7);
			List<Vector3> particlePositions = new List<Vector3>();
			List<Vector3> particleNormals = new List<Vector3>();
			List<float> particleThicknesses = new List<float>();
			List<float> particleInvMasses = new List<float>();
			List<float> particleInvRotationalMasses = new List<float>();
			List<int> particleFilters = new List<int>();
			List<Color> particleColors = new List<Color>();
			if (!path.Closed)
			{
				particlePositions.Add(path.points.GetPositionAtMu(path.Closed, 0f));
				particleNormals.Add(path.normals.GetAtMu(path.Closed, 0f));
				particleThicknesses.Add(path.thicknesses.GetAtMu(path.Closed, 0f));
				particleInvMasses.Add(ObiUtils.MassToInvMass(path.masses.GetAtMu(path.Closed, 0f)));
				particleInvRotationalMasses.Add(ObiUtils.MassToInvMass(path.rotationalMasses.GetAtMu(path.Closed, 0f)));
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
					particleNormals.Add(path.normals.GetAtMu(path.Closed, muAtLenght));
					particleThicknesses.Add(path.thicknesses.GetAtMu(path.Closed, muAtLenght));
					particleInvMasses.Add(ObiUtils.MassToInvMass(path.masses.GetAtMu(path.Closed, muAtLenght)));
					particleInvRotationalMasses.Add(ObiUtils.MassToInvMass(path.rotationalMasses.GetAtMu(path.Closed, muAtLenght)));
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
			totalParticles = m_ActiveParticleCount;
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
			orientations = new Quaternion[totalParticles];
			velocities = new Vector3[totalParticles];
			angularVelocities = new Vector3[totalParticles];
			invMasses = new float[totalParticles];
			invRotationalMasses = new float[totalParticles];
			principalRadii = new Vector3[totalParticles];
			filters = new int[totalParticles];
			restPositions = new Vector4[totalParticles];
			restOrientations = new Quaternion[totalParticles];
			colors = new Color[totalParticles];
			restLengths = new float[totalParticles];
			for (int i = 0; i < m_ActiveParticleCount; i++)
			{
				invMasses[i] = particleInvMasses[i];
				invRotationalMasses[i] = particleInvRotationalMasses[i];
				positions[i] = particlePositions[i];
				restPositions[i] = positions[i];
				restPositions[i][3] = 1f;
				principalRadii[i] = Vector3.one * particleThicknesses[i] * thickness;
				filters[i] = particleFilters[i];
				colors[i] = particleColors[i];
				if (i % 100 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRod: generating particles...", (float)i / (float)m_ActiveParticleCount);
				}
			}
			CreateDeformableEdges(numSegments);
			CreateSimplices(numSegments);
			IEnumerator dc = CreateStretchShearConstraints(particleNormals);
			while (dc.MoveNext())
			{
				yield return dc.Current;
			}
			IEnumerator bc = CreateBendTwistConstraints();
			while (bc.MoveNext())
			{
				yield return bc.Current;
			}
			IEnumerator ac = CreateAerodynamicConstraints();
			while (ac.MoveNext())
			{
				yield return ac.Current;
			}
			IEnumerator cc = CreateChainConstraints();
			while (cc.MoveNext())
			{
				yield return cc.Current;
			}
		}

		protected virtual IEnumerator CreateStretchShearConstraints(List<Vector3> particleNormals)
		{
			stretchShearConstraintsData = new ObiStretchShearConstraintsData();
			stretchShearConstraintsData.AddBatch(new ObiStretchShearConstraintsBatch());
			stretchShearConstraintsData.AddBatch(new ObiStretchShearConstraintsBatch());
			ObiPathFrame frame = ObiPathFrame.Identity;
			for (int i = 0; i < totalParticles - 1; i++)
			{
				ObiStretchShearConstraintsBatch obiStretchShearConstraintsBatch = stretchShearConstraintsData.batches[i % 2];
				Vector2Int indices = new Vector2Int(i, i + 1);
				Vector3 vector = positions[indices.y] - positions[indices.x];
				restLengths[i] = vector.magnitude;
				frame.Transport(positions[indices.x], vector.normalized, 0f);
				orientations[i] = Quaternion.LookRotation(frame.tangent, particleNormals[indices.x]);
				restOrientations[i] = orientations[i];
				orientations[indices.y] = orientations[i];
				restOrientations[indices.y] = orientations[i];
				obiStretchShearConstraintsBatch.AddConstraint(indices, indices.x, restLengths[i], Quaternion.identity);
				obiStretchShearConstraintsBatch.activeConstraintCount++;
				if (i % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRod: generating structural constraints...", (float)i / (float)(totalParticles - 1));
				}
			}
			if (path.Closed)
			{
				ObiStretchShearConstraintsBatch obiStretchShearConstraintsBatch2 = new ObiStretchShearConstraintsBatch();
				stretchShearConstraintsData.AddBatch(obiStretchShearConstraintsBatch2);
				Vector2Int indices2 = new Vector2Int(m_ActiveParticleCount - 1, 0);
				Vector3 vector2 = positions[indices2.y] - positions[indices2.x];
				restLengths[m_ActiveParticleCount - 2] = vector2.magnitude;
				frame.Transport(positions[indices2.x], vector2.normalized, 0f);
				orientations[m_ActiveParticleCount - 1] = Quaternion.LookRotation(frame.tangent, particleNormals[indices2.x]);
				restOrientations[m_ActiveParticleCount - 1] = orientations[m_ActiveParticleCount - 1];
				obiStretchShearConstraintsBatch2.AddConstraint(indices2, indices2.x, restLengths[m_ActiveParticleCount - 2], Quaternion.identity);
				obiStretchShearConstraintsBatch2.activeConstraintCount++;
			}
			m_RestLength = 0f;
			float[] array = restLengths;
			foreach (float num in array)
			{
				m_RestLength += num;
			}
		}

		protected virtual IEnumerator CreateBendTwistConstraints()
		{
			bendTwistConstraintsData = new ObiBendTwistConstraintsData();
			bendTwistConstraintsData.AddBatch(new ObiBendTwistConstraintsBatch());
			bendTwistConstraintsData.AddBatch(new ObiBendTwistConstraintsBatch());
			for (int i = 0; i < totalParticles - 1; i++)
			{
				ObiBendTwistConstraintsBatch obiBendTwistConstraintsBatch = bendTwistConstraintsData.batches[i % 2];
				Vector2Int indices = new Vector2Int(i, i + 1);
				obiBendTwistConstraintsBatch.AddConstraint(restDarboux: keepInitialShape ? ObiUtils.RestDarboux(orientations[indices.x], orientations[indices.y]) : Quaternion.identity, indices: indices);
				obiBendTwistConstraintsBatch.activeConstraintCount++;
				if (i % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRod: generating structural constraints...", (float)i / (float)(totalParticles - 1));
				}
			}
			if (path.Closed)
			{
				ObiBendTwistConstraintsBatch obiBendTwistConstraintsBatch2 = new ObiBendTwistConstraintsBatch();
				bendTwistConstraintsData.AddBatch(obiBendTwistConstraintsBatch2);
				Vector2Int indices2 = new Vector2Int(m_ActiveParticleCount - 1, 0);
				Quaternion restDarboux = (keepInitialShape ? ObiUtils.RestDarboux(orientations[indices2.x], orientations[indices2.y]) : Quaternion.identity);
				obiBendTwistConstraintsBatch2.AddConstraint(indices2, restDarboux);
				obiBendTwistConstraintsBatch2.activeConstraintCount++;
			}
		}

		protected virtual IEnumerator CreateChainConstraints()
		{
			chainConstraintsData = new ObiChainConstraintsData();
			ObiChainConstraintsBatch obiChainConstraintsBatch = new ObiChainConstraintsBatch();
			chainConstraintsData.AddBatch(obiChainConstraintsBatch);
			int[] array = new int[m_ActiveParticleCount + (path.Closed ? 1 : 0)];
			for (int i = 0; i < m_ActiveParticleCount; i++)
			{
				array[i] = i;
			}
			if (path.Closed)
			{
				array[m_ActiveParticleCount] = 0;
			}
			obiChainConstraintsBatch.AddConstraint(array, m_InterParticleDistance, 1f, 1f);
			obiChainConstraintsBatch.activeConstraintCount++;
			yield return 0;
		}
	}
}
