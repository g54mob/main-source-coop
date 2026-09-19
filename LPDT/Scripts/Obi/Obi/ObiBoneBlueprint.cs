using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[CreateAssetMenu(fileName = "bone blueprint", menuName = "Obi/Bone Blueprint", order = 142)]
	public class ObiBoneBlueprint : ObiActorBlueprint
	{
		public Transform root;

		public const float DEFAULT_PARTICLE_MASS = 0.1f;

		public const float DEFAULT_PARTICLE_ROTATIONAL_MASS = 0.1f;

		public const float DEFAULT_PARTICLE_RADIUS = 0.05f;

		[HideInInspector]
		public List<Transform> transforms = new List<Transform>();

		[HideInInspector]
		public List<Quaternion> restTransformOrientations = new List<Quaternion>();

		[HideInInspector]
		public List<int> parentIndices = new List<int>();

		[HideInInspector]
		public List<float> normalizedLengths = new List<float>();

		[HideInInspector]
		public int[] deformableEdges;

		[NonSerialized]
		[HideInInspector]
		public List<ObiBone.IgnoredBone> ignored;

		[NonSerialized]
		[HideInInspector]
		public ObiBone.BonePropertyCurve mass;

		[NonSerialized]
		[HideInInspector]
		public ObiBone.BonePropertyCurve rotationalMass;

		[NonSerialized]
		[HideInInspector]
		public ObiBone.BonePropertyCurve radius;

		public Quaternion root2WorldR;

		private GraphColoring colorizer;

		private ObiBone.IgnoredBone GetIgnoredBone(Transform bone)
		{
			for (int i = 0; i < ignored.Count; i++)
			{
				if (ignored[i].bone == bone)
				{
					return ignored[i];
				}
			}
			return null;
		}

		public ObiBoneOverride GetOverride(int particleIndex, out float normalizedLength)
		{
			int num = particleIndex;
			normalizedLength = normalizedLengths[num];
			while (num >= 0)
			{
				if (transforms[num].TryGetComponent<ObiBoneOverride>(out var component))
				{
					normalizedLength = 1f - (1f - normalizedLengths[particleIndex]) / Mathf.Max(1E-07f, 1f - normalizedLengths[num]);
					return component;
				}
				num = parentIndices[num];
			}
			return null;
		}

		protected override IEnumerator Initialize()
		{
			ClearParticleGroups();
			transforms.Clear();
			restTransformOrientations.Clear();
			parentIndices.Clear();
			normalizedLengths.Clear();
			List<Vector3> particlePositions = new List<Vector3>();
			List<Quaternion> particleOrientations = new List<Quaternion>();
			Matrix4x4 worldToLocalMatrix = root.transform.worldToLocalMatrix;
			Quaternion rotation = worldToLocalMatrix.rotation;
			root2WorldR = Quaternion.Inverse(rotation);
			Queue<Transform> queue = new Queue<Transform>();
			queue.Enqueue(root);
			parentIndices.Add(-1);
			normalizedLengths.Add(0f);
			float num = 0f;
			while (queue.Count > 0)
			{
				Transform transform = queue.Dequeue();
				if (!(transform != null))
				{
					continue;
				}
				ObiBone.IgnoredBone ignoredBone = GetIgnoredBone(transform);
				if (ignoredBone == null)
				{
					transforms.Add(transform);
					restTransformOrientations.Add(transform.localRotation);
					particlePositions.Add(worldToLocalMatrix.MultiplyPoint3x4(transform.position));
					particleOrientations.Add(rotation * transform.rotation);
				}
				if (ignoredBone != null && ignoredBone.ignoreChildren)
				{
					continue;
				}
				foreach (Transform item in transform)
				{
					ignoredBone = GetIgnoredBone(item);
					if (ignoredBone == null)
					{
						int num2 = transforms.Count - 1;
						parentIndices.Add(num2);
						float num3 = Vector3.Distance(item.position, transform.position);
						float num4 = normalizedLengths[num2] + num3;
						num = Mathf.Max(num, num4);
						normalizedLengths.Add(num4);
					}
					queue.Enqueue(item);
				}
			}
			if (num > 0f)
			{
				for (int i = 0; i < normalizedLengths.Count; i++)
				{
					normalizedLengths[i] /= num;
				}
			}
			Vector3[] array = new Vector3[parentIndices.Count];
			int[] array2 = new int[parentIndices.Count];
			for (int j = 0; j < parentIndices.Count; j++)
			{
				int num5 = parentIndices[j];
				if (num5 >= 0)
				{
					Vector3 vector = particlePositions[j] - particlePositions[num5];
					array[num5] += vector;
					array2[num5]++;
				}
			}
			for (int k = 0; k < parentIndices.Count; k++)
			{
				if (array2[k] > 0)
				{
					particleOrientations[k] = Quaternion.LookRotation(array[k] / array2[k]);
				}
				else if (parentIndices[k] >= 0)
				{
					particleOrientations[k] = particleOrientations[parentIndices[k]];
				}
			}
			m_ActiveParticleCount = particlePositions.Count;
			positions = new Vector3[m_ActiveParticleCount];
			orientations = new Quaternion[m_ActiveParticleCount];
			velocities = new Vector3[m_ActiveParticleCount];
			angularVelocities = new Vector3[m_ActiveParticleCount];
			invMasses = new float[m_ActiveParticleCount];
			invRotationalMasses = new float[m_ActiveParticleCount];
			principalRadii = new Vector3[m_ActiveParticleCount];
			filters = new int[m_ActiveParticleCount];
			restPositions = new Vector4[m_ActiveParticleCount];
			restOrientations = new Quaternion[m_ActiveParticleCount];
			colors = new Color[m_ActiveParticleCount];
			for (int l = 0; l < m_ActiveParticleCount; l++)
			{
				invMasses[l] = ObiUtils.MassToInvMass((mass != null) ? mass.Evaluate(normalizedLengths[l]) : 0.1f);
				invRotationalMasses[l] = ObiUtils.MassToInvMass((rotationalMass != null) ? rotationalMass.Evaluate(normalizedLengths[l]) : 0.1f);
				positions[l] = particlePositions[l];
				restPositions[l] = positions[l];
				restPositions[l][3] = 1f;
				orientations[l] = particleOrientations[l];
				restOrientations[l] = transforms[l].rotation;
				principalRadii[l] = Vector3.one * ((radius != null) ? radius.Evaluate(normalizedLengths[l]) : 0.05f);
				filters[l] = ObiUtils.MakeFilter(65535, 0);
				colors[l] = Color.white;
				ObiParticleGroup obiParticleGroup = ScriptableObject.CreateInstance<ObiParticleGroup>();
				obiParticleGroup.SetSourceBlueprint(this);
				obiParticleGroup.name = transforms[l].name;
				obiParticleGroup.particleIndices.Add(l);
				groups.Add(obiParticleGroup);
				if (l % 100 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRod: generating particles...", (float)l / (float)m_ActiveParticleCount);
				}
			}
			colorizer = new GraphColoring(m_ActiveParticleCount);
			CreateDeformableEdges();
			CreateSimplices();
			IEnumerator dc = CreateStretchShearConstraints(particlePositions);
			while (dc.MoveNext())
			{
				yield return dc.Current;
			}
			IEnumerator bc = CreateBendTwistConstraints(particlePositions);
			while (bc.MoveNext())
			{
				yield return bc.Current;
			}
			IEnumerator sc = CreateSkinConstraints(particlePositions);
			while (sc.MoveNext())
			{
				yield return sc.Current;
			}
			IEnumerator ac = CreateAerodynamicConstraints();
			while (ac.MoveNext())
			{
				yield return ac.Current;
			}
			yield return new CoroutineJob.ProgressInfo("ObiBone: complete", 1f);
		}

		protected void CreateDeformableEdges()
		{
			deformableEdges = new int[(parentIndices.Count - 1) * 2];
			for (int i = 0; i < parentIndices.Count - 1; i++)
			{
				deformableEdges[i * 2] = i + 1;
				deformableEdges[i * 2 + 1] = parentIndices[i + 1];
			}
		}

		protected void CreateSimplices()
		{
			edges = new int[(parentIndices.Count - 1) * 2];
			for (int i = 0; i < parentIndices.Count - 1; i++)
			{
				edges[i * 2] = i + 1;
				edges[i * 2 + 1] = parentIndices[i + 1];
			}
		}

		protected virtual IEnumerator CreateAerodynamicConstraints()
		{
			aerodynamicConstraintsData = new ObiAerodynamicConstraintsData();
			ObiAerodynamicConstraintsBatch aeroBatch = new ObiAerodynamicConstraintsBatch();
			aerodynamicConstraintsData.AddBatch(aeroBatch);
			for (int i = 0; i < m_ActiveParticleCount; i++)
			{
				aeroBatch.AddConstraint(i, 2f * principalRadii[i].x, 1f, 1f);
				if (i % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiRope generating aerodynamic constraints...", (float)i / (float)m_ActiveParticleCount);
				}
			}
			for (int j = 0; j < aerodynamicConstraintsData.batches.Count; j++)
			{
				aerodynamicConstraintsData.batches[j].activeConstraintCount = m_ActiveParticleCount;
			}
		}

		protected virtual IEnumerator CreateStretchShearConstraints(List<Vector3> particlePositions)
		{
			colorizer.Clear();
			for (int i = 1; i < particlePositions.Count; i++)
			{
				int num = parentIndices[i];
				if (num >= 0)
				{
					colorizer.AddConstraint(new int[2] { num, i });
				}
			}
			stretchShearConstraintsData = new ObiStretchShearConstraintsData();
			List<int> constraintColors = new List<int>();
			IEnumerator colorize = colorizer.Colorize("ObiBone: coloring stretch/shear constraints...", constraintColors);
			while (colorize.MoveNext())
			{
				yield return colorize.Current;
			}
			IReadOnlyList<int> particleIndices = colorizer.particleIndices;
			IReadOnlyList<int> constraintIndices = colorizer.constraintIndices;
			int i2 = 0;
			while (i2 < constraintColors.Count)
			{
				int num2 = constraintColors[i2];
				int num3 = constraintIndices[i2];
				if (num2 >= stretchShearConstraintsData.batchCount)
				{
					stretchShearConstraintsData.AddBatch(new ObiStretchShearConstraintsBatch());
				}
				int num4 = particleIndices[num3];
				int num5 = particleIndices[num3 + 1];
				Vector3 vector = particlePositions[num5] - particlePositions[num4];
				Quaternion restOrientation = Quaternion.LookRotation(Quaternion.Inverse(orientations[num4]) * vector);
				stretchShearConstraintsData.batches[num2].AddConstraint(new Vector2Int(num4, num5), num4, vector.magnitude, restOrientation);
				stretchShearConstraintsData.batches[num2].activeConstraintCount++;
				if (i2 % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiBone: generating stretch constraints...", i2 / constraintColors.Count);
				}
				int num6 = i2 + 1;
				i2 = num6;
			}
		}

		protected virtual IEnumerator CreateBendTwistConstraints(List<Vector3> particlePositions)
		{
			colorizer.Clear();
			for (int i = 1; i < particlePositions.Count; i++)
			{
				int num = parentIndices[i];
				if (num >= 0)
				{
					colorizer.AddConstraint(new int[2] { num, i });
				}
			}
			bendTwistConstraintsData = new ObiBendTwistConstraintsData();
			List<int> constraintColors = new List<int>();
			IEnumerator colorize = colorizer.Colorize("ObiBone: colorizing bend/twist constraints...", constraintColors);
			while (colorize.MoveNext())
			{
				yield return colorize.Current;
			}
			IReadOnlyList<int> particleIndices = colorizer.particleIndices;
			IReadOnlyList<int> constraintIndices = colorizer.constraintIndices;
			int i2 = 0;
			while (i2 < constraintColors.Count)
			{
				int num2 = constraintColors[i2];
				int num3 = constraintIndices[i2];
				if (num2 >= bendTwistConstraintsData.batchCount)
				{
					bendTwistConstraintsData.AddBatch(new ObiBendTwistConstraintsBatch());
				}
				int num4 = particleIndices[num3];
				int num5 = particleIndices[num3 + 1];
				Quaternion restDarboux = ObiUtils.RestDarboux(orientations[num4], orientations[num5]);
				bendTwistConstraintsData.batches[num2].AddConstraint(new Vector2Int(num4, num5), restDarboux);
				bendTwistConstraintsData.batches[num2].activeConstraintCount++;
				if (i2 % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiBone: generating bend constraints...", i2 / constraintColors.Count);
				}
				int num6 = i2 + 1;
				i2 = num6;
			}
		}

		protected virtual IEnumerator CreateSkinConstraints(List<Vector3> particlePositions)
		{
			skinConstraintsData = new ObiSkinConstraintsData();
			ObiSkinConstraintsBatch skinBatch = new ObiSkinConstraintsBatch();
			skinConstraintsData.AddBatch(skinBatch);
			int i = 0;
			while (i < particlePositions.Count)
			{
				skinBatch.AddConstraint(i, particlePositions[i], Vector3.up, 0f, 0f, 0f, 0f);
				skinBatch.activeConstraintCount++;
				if (i % 500 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiCloth: generating skin constraints...", (float)i / (float)particlePositions.Count);
				}
				int num = i + 1;
				i = num;
			}
		}
	}
}
