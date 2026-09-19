using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Obi
{
	public abstract class ObiActorBlueprint : ScriptableObject, IObiParticleCollection
	{
		public delegate void BlueprintCallback(ObiActorBlueprint blueprint);

		[HideInInspector]
		[SerializeField]
		protected uint m_Checksum;

		[HideInInspector]
		[SerializeField]
		protected bool m_Empty = true;

		[HideInInspector]
		[SerializeField]
		protected bool m_Edited;

		[HideInInspector]
		[SerializeField]
		protected int m_ActiveParticleCount;

		[HideInInspector]
		[SerializeField]
		protected int m_InitialActiveParticleCount;

		[HideInInspector]
		[SerializeField]
		protected Bounds _bounds;

		[HideInInspector]
		public Vector3[] positions;

		[HideInInspector]
		public Vector4[] restPositions;

		[HideInInspector]
		public Vector4[] restNormals;

		[HideInInspector]
		public Quaternion[] orientations;

		[HideInInspector]
		public Quaternion[] restOrientations;

		[HideInInspector]
		public Vector3[] velocities;

		[HideInInspector]
		public Vector3[] angularVelocities;

		[HideInInspector]
		public float[] invMasses;

		[HideInInspector]
		public float[] invRotationalMasses;

		[FormerlySerializedAs("phases")]
		[HideInInspector]
		public int[] filters;

		[HideInInspector]
		public Vector3[] principalRadii;

		[HideInInspector]
		public Color[] colors;

		[HideInInspector]
		public int[] points;

		[HideInInspector]
		public int[] edges;

		[HideInInspector]
		public int[] triangles;

		[HideInInspector]
		public ObiDistanceConstraintsData distanceConstraintsData;

		[HideInInspector]
		public ObiBendConstraintsData bendConstraintsData;

		[HideInInspector]
		public ObiSkinConstraintsData skinConstraintsData;

		[HideInInspector]
		public ObiTetherConstraintsData tetherConstraintsData;

		[HideInInspector]
		public ObiStretchShearConstraintsData stretchShearConstraintsData;

		[HideInInspector]
		public ObiBendTwistConstraintsData bendTwistConstraintsData;

		[HideInInspector]
		public ObiShapeMatchingConstraintsData shapeMatchingConstraintsData;

		[HideInInspector]
		public ObiAerodynamicConstraintsData aerodynamicConstraintsData;

		[HideInInspector]
		public ObiChainConstraintsData chainConstraintsData;

		[HideInInspector]
		public ObiVolumeConstraintsData volumeConstraintsData;

		[HideInInspector]
		public List<ObiParticleGroup> groups = new List<ObiParticleGroup>();

		public uint checksum => m_Checksum;

		public int particleCount
		{
			get
			{
				if (positions == null)
				{
					return 0;
				}
				return positions.Length;
			}
		}

		public int activeParticleCount => m_ActiveParticleCount;

		public Oni.SimplexType simplexTypes => (Oni.SimplexType)(1 | ((edges != null) ? 2 : 0) | ((triangles != null) ? 4 : 0));

		public bool usesOrientedParticles
		{
			get
			{
				if (invRotationalMasses != null && invRotationalMasses.Length != 0 && orientations != null)
				{
					return orientations.Length != 0;
				}
				return false;
			}
		}

		public virtual bool usesTethers => false;

		public bool edited
		{
			get
			{
				return m_Edited;
			}
			set
			{
				m_Edited = value;
			}
		}

		public bool empty => m_Empty;

		public Bounds bounds => _bounds;

		public event BlueprintCallback OnBlueprintGenerate;

		public bool IsParticleActive(int index)
		{
			return index < m_ActiveParticleCount;
		}

		protected virtual void SwapWithFirstInactiveParticle(int index)
		{
			positions.Swap(index, m_ActiveParticleCount);
			restPositions.Swap(index, m_ActiveParticleCount);
			restNormals.Swap(index, m_ActiveParticleCount);
			orientations.Swap(index, m_ActiveParticleCount);
			restOrientations.Swap(index, m_ActiveParticleCount);
			velocities.Swap(index, m_ActiveParticleCount);
			angularVelocities.Swap(index, m_ActiveParticleCount);
			invMasses.Swap(index, m_ActiveParticleCount);
			invRotationalMasses.Swap(index, m_ActiveParticleCount);
			filters.Swap(index, m_ActiveParticleCount);
			principalRadii.Swap(index, m_ActiveParticleCount);
			colors.Swap(index, m_ActiveParticleCount);
			m_Edited = true;
		}

		public bool ActivateParticle(int index)
		{
			if (IsParticleActive(index))
			{
				return false;
			}
			SwapWithFirstInactiveParticle(index);
			m_ActiveParticleCount++;
			return true;
		}

		public bool DeactivateParticle(int index)
		{
			if (!IsParticleActive(index))
			{
				return false;
			}
			m_ActiveParticleCount--;
			SwapWithFirstInactiveParticle(index);
			return true;
		}

		public void RecalculateBounds()
		{
			if (positions.Length != 0)
			{
				_bounds = new Bounds(positions[0], Vector3.zero);
				for (int i = 1; i < positions.Length; i++)
				{
					_bounds.Encapsulate(positions[i]);
				}
			}
			else
			{
				_bounds = default(Bounds);
			}
		}

		protected void GenerateChecksum()
		{
			using MemoryStream memoryStream = new MemoryStream();
			if (positions != null)
			{
				Vector3[] array = positions;
				foreach (Vector3 v in array)
				{
					memoryStream.Concatenate(v);
				}
			}
			if (orientations != null)
			{
				Quaternion[] array2 = orientations;
				foreach (Quaternion q in array2)
				{
					memoryStream.Concatenate(q);
				}
			}
			memoryStream.Flush();
			m_Checksum = ObiUtils.Adler32(memoryStream.ToArray());
		}

		public IEnumerable<IObiConstraints> GetConstraints()
		{
			if (distanceConstraintsData != null && distanceConstraintsData.batchCount > 0)
			{
				yield return distanceConstraintsData;
			}
			if (bendConstraintsData != null && bendConstraintsData.batchCount > 0)
			{
				yield return bendConstraintsData;
			}
			if (skinConstraintsData != null && skinConstraintsData.batchCount > 0)
			{
				yield return skinConstraintsData;
			}
			if (tetherConstraintsData != null && tetherConstraintsData.batchCount > 0)
			{
				yield return tetherConstraintsData;
			}
			if (stretchShearConstraintsData != null && stretchShearConstraintsData.batchCount > 0)
			{
				yield return stretchShearConstraintsData;
			}
			if (bendTwistConstraintsData != null && bendTwistConstraintsData.batchCount > 0)
			{
				yield return bendTwistConstraintsData;
			}
			if (shapeMatchingConstraintsData != null && shapeMatchingConstraintsData.batchCount > 0)
			{
				yield return shapeMatchingConstraintsData;
			}
			if (aerodynamicConstraintsData != null && aerodynamicConstraintsData.batchCount > 0)
			{
				yield return aerodynamicConstraintsData;
			}
			if (chainConstraintsData != null && chainConstraintsData.batchCount > 0)
			{
				yield return chainConstraintsData;
			}
			if (volumeConstraintsData != null && volumeConstraintsData.batchCount > 0)
			{
				yield return volumeConstraintsData;
			}
		}

		public IObiConstraints GetConstraintsByType(Oni.ConstraintType type)
		{
			return type switch
			{
				Oni.ConstraintType.Distance => distanceConstraintsData, 
				Oni.ConstraintType.Bending => bendConstraintsData, 
				Oni.ConstraintType.Skin => skinConstraintsData, 
				Oni.ConstraintType.Tether => tetherConstraintsData, 
				Oni.ConstraintType.BendTwist => bendTwistConstraintsData, 
				Oni.ConstraintType.StretchShear => stretchShearConstraintsData, 
				Oni.ConstraintType.ShapeMatching => shapeMatchingConstraintsData, 
				Oni.ConstraintType.Aerodynamics => aerodynamicConstraintsData, 
				Oni.ConstraintType.Chain => chainConstraintsData, 
				Oni.ConstraintType.Volume => volumeConstraintsData, 
				_ => null, 
			};
		}

		public int GetParticleRuntimeIndex(int blueprintIndex)
		{
			return blueprintIndex;
		}

		public Vector3 GetParticlePosition(int index)
		{
			if (positions != null && index < positions.Length)
			{
				return positions[index];
			}
			return Vector3.zero;
		}

		public Quaternion GetParticleOrientation(int index)
		{
			if (orientations != null && index < orientations.Length)
			{
				return orientations[index];
			}
			return Quaternion.identity;
		}

		public Vector3 GetParticleRestPosition(int index)
		{
			if (restPositions != null && index < restPositions.Length)
			{
				return restPositions[index];
			}
			return Vector3.zero;
		}

		public Quaternion GetParticleRestOrientation(int index)
		{
			if (restOrientations != null && index < restOrientations.Length)
			{
				return restOrientations[index];
			}
			return Quaternion.identity;
		}

		public void GetParticleAnisotropy(int index, ref Vector4 b1, ref Vector4 b2, ref Vector4 b3)
		{
			if (orientations != null && index < orientations.Length)
			{
				Quaternion quaternion = orientations[index];
				b1 = quaternion * Vector3.right;
				b2 = quaternion * Vector3.up;
				b3 = quaternion * Vector3.forward;
				b1[3] = principalRadii[index][0];
				b2[3] = principalRadii[index][1];
				b3[3] = principalRadii[index][2];
			}
			else
			{
				float num = (b3[3] = principalRadii[index][0]);
				float value = (b2[3] = num);
				b1[3] = value;
			}
		}

		public float GetParticleMaxRadius(int index)
		{
			if (principalRadii != null && index < principalRadii.Length)
			{
				return principalRadii[index][0];
			}
			return 0f;
		}

		public Color GetParticleColor(int index)
		{
			if (colors != null && index < colors.Length)
			{
				return colors[index];
			}
			return Color.white;
		}

		public void GenerateImmediate()
		{
			IEnumerator enumerator = Generate();
			while (enumerator.MoveNext())
			{
			}
		}

		public IEnumerator Generate()
		{
			Clear();
			IEnumerator g = Initialize();
			while (g.MoveNext())
			{
				yield return g.Current;
			}
			RecalculateBounds();
			m_Empty = false;
			m_InitialActiveParticleCount = m_ActiveParticleCount;
			foreach (IObiConstraints constraint in GetConstraints())
			{
				for (int i = 0; i < constraint.batchCount; i++)
				{
					constraint.GetBatch(i).initialActiveConstraintCount = constraint.GetBatch(i).activeConstraintCount;
				}
			}
			CommitBlueprintChanges();
			this.OnBlueprintGenerate?.Invoke(this);
		}

		public virtual void CommitBlueprintChanges()
		{
			GenerateChecksum();
		}

		public void Clear()
		{
			m_Empty = true;
			edited = false;
			m_ActiveParticleCount = 0;
			positions = null;
			restPositions = null;
			restNormals = null;
			orientations = null;
			restOrientations = null;
			velocities = null;
			angularVelocities = null;
			invMasses = null;
			invRotationalMasses = null;
			filters = null;
			principalRadii = null;
			colors = null;
			points = null;
			edges = null;
			triangles = null;
			distanceConstraintsData = null;
			bendConstraintsData = null;
			skinConstraintsData = null;
			tetherConstraintsData = null;
			bendTwistConstraintsData = null;
			stretchShearConstraintsData = null;
			shapeMatchingConstraintsData = null;
			aerodynamicConstraintsData = null;
			chainConstraintsData = null;
			volumeConstraintsData = null;
		}

		public ObiParticleGroup InsertNewParticleGroup(string name, int index, bool saveImmediately = true)
		{
			if (index >= 0 && index <= groups.Count)
			{
				ObiParticleGroup obiParticleGroup = ScriptableObject.CreateInstance<ObiParticleGroup>();
				obiParticleGroup.SetSourceBlueprint(this);
				obiParticleGroup.name = name;
				groups.Insert(index, obiParticleGroup);
				edited = true;
				return obiParticleGroup;
			}
			return null;
		}

		public ObiParticleGroup AppendNewParticleGroup(string name, bool saveImmediately = true)
		{
			return InsertNewParticleGroup(name, groups.Count, saveImmediately);
		}

		public bool RemoveParticleGroupAt(int index, bool saveImmediately = true)
		{
			if (index >= 0 && index < groups.Count)
			{
				ObiParticleGroup obiParticleGroup = groups[index];
				groups.RemoveAt(index);
				if (obiParticleGroup != null)
				{
					Object.DestroyImmediate(obiParticleGroup, allowDestroyingAssets: true);
				}
				edited = true;
				return true;
			}
			return false;
		}

		public bool SetParticleGroupName(int index, string name, bool saveImmediately = true)
		{
			if (index >= 0 && index < groups.Count)
			{
				groups[index].name = name;
				edited = true;
				return true;
			}
			return false;
		}

		public void ClearParticleGroups(bool registerUndo = true, bool saveImmediately = true)
		{
			if (groups.Count == 0)
			{
				return;
			}
			for (int i = 0; i < groups.Count; i++)
			{
				if (groups[i] != null)
				{
					Object.DestroyImmediate(groups[i], allowDestroyingAssets: true);
				}
			}
			groups.Clear();
		}

		private bool IsParticleSharedInConstraint(int index, List<int> particles, bool[] selected)
		{
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < particles.Count; i++)
			{
				flag |= particles[i] == index;
				flag2 |= !selected[particles[i]];
				if (flag && flag2)
				{
					return true;
				}
			}
			return false;
		}

		private bool DoesParticleShareConstraints(IObiConstraints constraints, int index, List<int> particles, bool[] selected)
		{
			bool flag = false;
			for (int i = 0; i < constraints.batchCount; i++)
			{
				IObiConstraintsBatch batch = constraints.GetBatch(i);
				for (int j = 0; j < batch.activeConstraintCount; j++)
				{
					particles.Clear();
					batch.GetParticlesInvolved(j, particles);
					if (flag |= IsParticleSharedInConstraint(index, particles, selected))
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		private void DeactivateConstraintsWithInactiveParticles(IObiConstraints constraints, List<int> particles)
		{
			for (int i = 0; i < constraints.batchCount; i++)
			{
				IObiConstraintsBatch batch = constraints.GetBatch(i);
				for (int num = batch.activeConstraintCount - 1; num >= 0; num--)
				{
					particles.Clear();
					batch.GetParticlesInvolved(num, particles);
					for (int j = 0; j < particles.Count; j++)
					{
						if (!IsParticleActive(particles[j]))
						{
							batch.DeactivateConstraint(num);
							break;
						}
					}
				}
			}
			edited = true;
		}

		private void ParticlesSwappedInGroups(int index, int newIndex)
		{
			foreach (ObiParticleGroup group in groups)
			{
				for (int i = 0; i < group.particleIndices.Count; i++)
				{
					if (group.particleIndices[i] == newIndex)
					{
						group.particleIndices[i] = index;
					}
					else if (group.particleIndices[i] == index)
					{
						group.particleIndices[i] = newIndex;
					}
				}
			}
			edited = true;
		}

		public virtual void RemoveSelectedParticles(ref bool[] selected, bool optimize = true)
		{
			List<int> particles = new List<int>();
			for (int num = activeParticleCount - 1; num >= 0; num--)
			{
				if (selected[num])
				{
					bool flag = false;
					if (optimize)
					{
						flag |= DoesParticleShareConstraints(distanceConstraintsData, num, particles, selected);
						flag |= DoesParticleShareConstraints(bendConstraintsData, num, particles, selected);
						flag |= DoesParticleShareConstraints(shapeMatchingConstraintsData, num, particles, selected);
					}
					if (!flag && DeactivateParticle(num))
					{
						selected.Swap(num, m_ActiveParticleCount);
						foreach (IObiConstraints constraint in GetConstraints())
						{
							for (int i = 0; i < constraint.batchCount; i++)
							{
								constraint.GetBatch(i).ParticlesSwapped(num, m_ActiveParticleCount);
							}
						}
						ParticlesSwappedInGroups(num, m_ActiveParticleCount);
					}
				}
			}
			foreach (IObiConstraints constraint2 in GetConstraints())
			{
				DeactivateConstraintsWithInactiveParticles(constraint2, particles);
			}
			CommitBlueprintChanges();
			edited = true;
		}

		public void RestoreRemovedParticles()
		{
			m_ActiveParticleCount = m_InitialActiveParticleCount;
			foreach (IObiConstraints constraint in GetConstraints())
			{
				for (int i = 0; i < constraint.batchCount; i++)
				{
					constraint.GetBatch(i).activeConstraintCount = constraint.GetBatch(i).initialActiveConstraintCount;
				}
			}
			CommitBlueprintChanges();
		}

		public virtual void GenerateTethers(bool[] selected)
		{
		}

		public virtual void ClearTethers()
		{
		}

		protected abstract IEnumerator Initialize();
	}
}
