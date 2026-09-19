using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public abstract class ObiActor : MonoBehaviour, IObiParticleCollection
	{
		public class ObiActorSolverArgs : EventArgs
		{
			public ObiSolver solver { get; }

			public ObiActorSolverArgs(ObiSolver solver)
			{
				this.solver = solver;
			}
		}

		private struct BufferedForces
		{
			public bool dirty;

			public Vector4 force;

			public Vector4 acceleration;

			public Vector4 impulse;

			public Vector4 velChange;

			public Vector4 angularForce;

			public Vector4 angularAcceleration;

			public Vector4 angularImpulse;

			public Vector4 angularVelChange;

			public void Clear()
			{
				force = Vector4.zero;
				acceleration = Vector4.zero;
				impulse = Vector4.zero;
				velChange = Vector4.zero;
				angularForce = Vector4.zero;
				angularAcceleration = Vector4.zero;
				angularImpulse = Vector4.zero;
				angularVelChange = Vector4.zero;
			}
		}

		public delegate void ActorCallback(ObiActor actor);

		public delegate void ActorStepCallback(ObiActor actor, float simulatedTime, float substepTime);

		public delegate void ActorBlueprintCallback(ObiActor actor, ObiActorBlueprint blueprint);

		[HideInInspector]
		protected ObiNativeIntList m_ActiveParticleCount;

		[HideInInspector]
		public ObiNativeIntList solverIndices;

		[HideInInspector]
		public List<int>[] solverBatchOffsets;

		protected ObiSolver m_Solver;

		protected bool m_Loaded;

		public int groupID;

		private ObiActorBlueprint m_State;

		private ObiActorBlueprint m_BlueprintInstance;

		private ObiPinConstraintsData m_PinConstraints;

		private ObiPinholeConstraintsData m_PinholeConstraints;

		private BufferedForces bufferedForces;

		[SerializeField]
		[HideInInspector]
		protected ObiCollisionMaterial m_CollisionMaterial;

		[SerializeField]
		[HideInInspector]
		protected bool m_SurfaceCollisions;

		[SerializeField]
		[HideInInspector]
		[Min(1E-07f)]
		protected float m_MassScale = 1f;

		public int deformableEdgesOffset { get; protected set; }

		public ObiSolver solver => m_Solver;

		public bool isLoaded
		{
			get
			{
				if (m_Solver != null)
				{
					return m_Loaded;
				}
				return false;
			}
		}

		public ObiCollisionMaterial collisionMaterial
		{
			get
			{
				return m_CollisionMaterial;
			}
			set
			{
				if (m_CollisionMaterial != value)
				{
					m_CollisionMaterial = value;
					UpdateCollisionMaterials();
				}
			}
		}

		public virtual bool surfaceCollisions
		{
			get
			{
				return m_SurfaceCollisions;
			}
			set
			{
				if (value != m_SurfaceCollisions)
				{
					m_SurfaceCollisions = value;
					if (m_Solver != null)
					{
						m_Solver.dirtySimplices |= simplexTypes;
					}
				}
			}
		}

		public float massScale
		{
			get
			{
				return m_MassScale;
			}
			set
			{
				if (Mathf.Abs(m_MassScale - value) > 1E-07f)
				{
					SetMassScale(value);
				}
			}
		}

		public int particleCount
		{
			get
			{
				if (!(sourceBlueprint != null))
				{
					return 0;
				}
				return sourceBlueprint.particleCount;
			}
		}

		public int activeParticleCount
		{
			get
			{
				if (m_ActiveParticleCount == null)
				{
					return 0;
				}
				return m_ActiveParticleCount[0];
			}
		}

		public ObiNativeIntList activeParticleCountBuffer => m_ActiveParticleCount;

		public bool usesOrientedParticles
		{
			get
			{
				if (sourceBlueprint != null && sourceBlueprint.invRotationalMasses != null && sourceBlueprint.invRotationalMasses.Length != 0 && sourceBlueprint.orientations != null && sourceBlueprint.orientations.Length != 0 && sourceBlueprint.restOrientations != null)
				{
					return sourceBlueprint.restOrientations.Length != 0;
				}
				return false;
			}
		}

		public virtual bool usesAnisotropicParticles => false;

		public Oni.SimplexType simplexTypes
		{
			get
			{
				if (!(sourceBlueprint != null))
				{
					return Oni.SimplexType.Point;
				}
				return sourceBlueprint.simplexTypes;
			}
		}

		public Matrix4x4 actorLocalToSolverMatrix
		{
			get
			{
				if (m_Solver != null)
				{
					return m_Solver.transform.worldToLocalMatrix * base.transform.localToWorldMatrix;
				}
				return base.transform.localToWorldMatrix;
			}
		}

		public Matrix4x4 actorSolverToLocalMatrix
		{
			get
			{
				if (m_Solver != null)
				{
					return base.transform.worldToLocalMatrix * m_Solver.transform.localToWorldMatrix;
				}
				return base.transform.worldToLocalMatrix;
			}
		}

		public abstract ObiActorBlueprint sourceBlueprint { get; }

		public ObiActorBlueprint sharedBlueprint
		{
			get
			{
				if (m_BlueprintInstance != null)
				{
					return m_BlueprintInstance;
				}
				return sourceBlueprint;
			}
		}

		public ObiActorBlueprint blueprint
		{
			get
			{
				if (m_BlueprintInstance == null && sourceBlueprint != null)
				{
					m_BlueprintInstance = UnityEngine.Object.Instantiate(sourceBlueprint);
				}
				return m_BlueprintInstance;
			}
		}

		public event ActorBlueprintCallback OnBlueprintLoaded;

		public event ActorBlueprintCallback OnBlueprintUnloaded;

		public event ActorBlueprintCallback OnBlueprintRegenerated;

		public event ActorStepCallback OnSimulationStart;

		public event ActorStepCallback OnCollisionDetectionStart;

		public event ActorStepCallback OnSubstepsStart;

		public event ActorStepCallback OnSimulationEnd;

		public event ActorCallback OnRequestReadback;

		public event ActorStepCallback OnInterpolate;

		protected virtual void Awake()
		{
			m_ActiveParticleCount = new ObiNativeIntList();
			m_ActiveParticleCount.Add(0);
		}

		protected virtual void OnDestroy()
		{
			m_ActiveParticleCount.Dispose();
			m_ActiveParticleCount = null;
			if (m_BlueprintInstance != null)
			{
				UnityEngine.Object.DestroyImmediate(m_BlueprintInstance);
			}
		}

		protected virtual void OnEnable()
		{
			solverBatchOffsets = new List<int>[18];
			for (int i = 0; i < solverBatchOffsets.Length; i++)
			{
				solverBatchOffsets[i] = new List<int>();
			}
			m_PinConstraints = new ObiPinConstraintsData();
			m_PinholeConstraints = new ObiPinholeConstraintsData();
			m_Solver = GetComponentInParent<ObiSolver>();
			AddToSolver();
		}

		protected virtual void OnDisable()
		{
			RemoveFromSolver();
		}

		protected virtual void OnValidate()
		{
			UpdateCollisionMaterials();
		}

		private void OnTransformParentChanged()
		{
			if (base.isActiveAndEnabled)
			{
				SetSolver(GetComponentInParent<ObiSolver>());
			}
		}

		public void AddToSolver()
		{
			if (m_Solver != null)
			{
				if (sourceBlueprint != null)
				{
					sourceBlueprint.OnBlueprintGenerate += OnBlueprintRegenerate;
				}
				m_Solver.AddActor(this);
			}
		}

		public void RemoveFromSolver()
		{
			if (m_Solver != null)
			{
				if (sourceBlueprint != null)
				{
					sourceBlueprint.OnBlueprintGenerate -= OnBlueprintRegenerate;
				}
				m_Solver.RemoveActor(this);
			}
		}

		protected void SetSolver(ObiSolver newSolver)
		{
			if (newSolver != m_Solver)
			{
				RemoveFromSolver();
				m_Solver = newSolver;
				AddToSolver();
			}
		}

		protected void SetMassScale(float scale)
		{
			if (!Application.isPlaying || !isLoaded || particleCount <= 0)
			{
				return;
			}
			scale = Mathf.Max(1E-07f, scale);
			for (int i = 0; i < particleCount; i++)
			{
				int index = solverIndices[i];
				if (m_Solver.invMasses[index] > 0f)
				{
					m_Solver.invMasses[index] = sharedBlueprint.invMasses[i] / scale;
				}
				if (m_Solver.invRotationalMasses[index] > 0f && sharedBlueprint.invRotationalMasses != null && i < sharedBlueprint.invRotationalMasses.Length)
				{
					m_Solver.invRotationalMasses[index] = sharedBlueprint.invRotationalMasses[i] / scale;
				}
			}
			UpdateParticleProperties();
		}

		protected virtual void OnBlueprintRegenerate(ObiActorBlueprint blueprint)
		{
			RemoveFromSolver();
			if (m_BlueprintInstance != null)
			{
				UnityEngine.Object.DestroyImmediate(m_BlueprintInstance);
			}
			AddToSolver();
			this.OnBlueprintRegenerated?.Invoke(this, blueprint);
		}

		protected void UpdateCollisionMaterials()
		{
			if (!isLoaded)
			{
				return;
			}
			int value = ((m_CollisionMaterial != null) ? m_CollisionMaterial.handle.index : (-1));
			for (int i = 0; i < solverIndices.count; i++)
			{
				if (solverIndices[i] < solver.collisionMaterials.count)
				{
					solver.collisionMaterials[solverIndices[i]] = value;
				}
			}
		}

		public virtual void ProvideDeformableTriangles(ObiNativeIntList deformableTriangles, ObiNativeVector2List deformableUVs)
		{
		}

		public virtual void ProvideDeformableEdges(ObiNativeIntList deformableEdges)
		{
		}

		public virtual int GetDeformableEdgeCount()
		{
			return 0;
		}

		public virtual bool CopyParticle(int actorSourceIndex, int actorDestIndex)
		{
			if (!isLoaded || actorSourceIndex < 0 || actorSourceIndex >= solverIndices.count || actorDestIndex < 0 || actorDestIndex >= solverIndices.count)
			{
				return false;
			}
			int index = solverIndices[actorSourceIndex];
			int index2 = solverIndices[actorDestIndex];
			m_Solver.prevPositions[index2] = m_Solver.prevPositions[index];
			m_Solver.restPositions[index2] = m_Solver.restPositions[index];
			ObiNativeVector4List endPositions = m_Solver.endPositions;
			ObiNativeVector4List startPositions = m_Solver.startPositions;
			Vector4 vector = (m_Solver.positions[index2] = m_Solver.positions[index]);
			Vector4 value = (startPositions[index2] = vector);
			endPositions[index2] = value;
			m_Solver.prevOrientations[index2] = m_Solver.prevOrientations[index];
			m_Solver.restOrientations[index2] = m_Solver.restOrientations[index];
			ObiNativeQuaternionList endOrientations = m_Solver.endOrientations;
			ObiNativeQuaternionList startOrientations = m_Solver.startOrientations;
			Quaternion quaternion = (m_Solver.orientations[index2] = m_Solver.orientations[index]);
			Quaternion value2 = (startOrientations[index2] = quaternion);
			endOrientations[index2] = value2;
			m_Solver.velocities[index2] = m_Solver.velocities[index];
			m_Solver.angularVelocities[index2] = m_Solver.angularVelocities[index];
			m_Solver.invMasses[index2] = m_Solver.invMasses[index];
			m_Solver.invRotationalMasses[index2] = m_Solver.invRotationalMasses[index];
			m_Solver.principalRadii[index2] = m_Solver.principalRadii[index];
			m_Solver.phases[index2] = m_Solver.phases[index];
			m_Solver.filters[index2] = m_Solver.filters[index];
			m_Solver.colors[index2] = m_Solver.colors[index];
			return true;
		}

		public void TeleportParticle(int actorIndex, Vector3 position)
		{
			if (isLoaded && actorIndex >= 0 && actorIndex < solverIndices.count)
			{
				int index = solverIndices[actorIndex];
				Vector4 vector = (Vector4)position - m_Solver.positions[index];
				m_Solver.positions[index] += vector;
				m_Solver.prevPositions[index] += vector;
				m_Solver.endPositions[index] += vector;
				m_Solver.startPositions[index] += vector;
			}
		}

		public virtual Matrix4x4 Teleport(Vector3 position, Quaternion rotation)
		{
			if (!isLoaded)
			{
				return Matrix4x4.identity;
			}
			Matrix4x4 result = solver.transform.worldToLocalMatrix * Matrix4x4.TRS(position, Quaternion.identity, Vector3.one) * Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one) * Matrix4x4.TRS(Vector3.zero, Quaternion.Inverse(base.transform.rotation), Vector3.one) * Matrix4x4.TRS(-base.transform.position, Quaternion.identity, Vector3.one) * solver.transform.localToWorldMatrix;
			Quaternion rotation2 = result.rotation;
			for (int i = 0; i < solverIndices.count; i++)
			{
				int index = solverIndices[i];
				ObiNativeVector4List positions = m_Solver.positions;
				ObiNativeVector4List prevPositions = m_Solver.prevPositions;
				ObiNativeVector4List endPositions = m_Solver.endPositions;
				Vector4 vector = (m_Solver.startPositions[index] = result.MultiplyPoint3x4(m_Solver.positions[index]));
				Vector4 vector3 = (endPositions[index] = vector);
				Vector4 value = (prevPositions[index] = vector3);
				positions[index] = value;
				ObiNativeQuaternionList orientations = m_Solver.orientations;
				ObiNativeQuaternionList prevOrientations = m_Solver.prevOrientations;
				ObiNativeQuaternionList endOrientations = m_Solver.endOrientations;
				Quaternion quaternion = (m_Solver.startOrientations[index] = rotation2 * m_Solver.orientations[index]);
				Quaternion quaternion3 = (endOrientations[index] = quaternion);
				Quaternion value2 = (prevOrientations[index] = quaternion3);
				orientations[index] = value2;
				m_Solver.velocities[index] = Vector4.zero;
				m_Solver.angularVelocities[index] = Vector4.zero;
			}
			base.transform.position = position;
			base.transform.rotation = rotation;
			return result;
		}

		protected virtual void SwapWithFirstInactiveParticle(int actorIndex)
		{
			m_Solver.particleToActor[solverIndices[actorIndex]].indexInActor = activeParticleCount;
			m_Solver.particleToActor[solverIndices[activeParticleCount]].indexInActor = actorIndex;
			solverIndices.Swap(actorIndex, activeParticleCount);
		}

		public virtual bool ActivateParticle()
		{
			if (activeParticleCount >= particleCount)
			{
				return false;
			}
			Vector4 value = m_Solver.principalRadii[solverIndices[activeParticleCount]];
			value.w = 1f;
			m_Solver.principalRadii[solverIndices[activeParticleCount]] = value;
			m_ActiveParticleCount[0]++;
			m_Solver.dirtyActiveParticles = true;
			m_Solver.dirtySimplices |= simplexTypes;
			return true;
		}

		public virtual bool DeactivateParticle(int actorIndex)
		{
			if (!IsParticleActive(actorIndex))
			{
				return false;
			}
			m_ActiveParticleCount[0]--;
			Vector4 value = m_Solver.principalRadii[solverIndices[actorIndex]];
			value.w = 0f;
			m_Solver.principalRadii[solverIndices[actorIndex]] = value;
			SwapWithFirstInactiveParticle(actorIndex);
			m_Solver.dirtyActiveParticles = true;
			m_Solver.dirtySimplices |= simplexTypes;
			return true;
		}

		public virtual bool IsParticleActive(int actorIndex)
		{
			return actorIndex < activeParticleCount;
		}

		public virtual void SetSelfCollisions(bool selfCollisions)
		{
			if (!(m_Solver != null) || !Application.isPlaying || !isLoaded)
			{
				return;
			}
			for (int i = 0; i < particleCount; i++)
			{
				if (selfCollisions)
				{
					m_Solver.phases[solverIndices[i]] |= 16777216;
				}
				else
				{
					m_Solver.phases[solverIndices[i]] &= -16777217;
				}
			}
		}

		public virtual void SetDirectionalCollisions(bool dfCollisions)
		{
			if (!(m_Solver != null) || !Application.isPlaying || !isLoaded)
			{
				return;
			}
			for (int i = 0; i < particleCount; i++)
			{
				Vector4 value = m_Solver.normals[solverIndices[i]];
				if (dfCollisions)
				{
					value.w = sharedBlueprint.restNormals[i].w;
				}
				else
				{
					value.w = 0f;
				}
				m_Solver.normals[solverIndices[i]] = value;
			}
		}

		public virtual void SetOneSided(bool oneSided)
		{
			if (!(m_Solver != null) || !Application.isPlaying || !isLoaded)
			{
				return;
			}
			for (int i = 0; i < particleCount; i++)
			{
				if (oneSided)
				{
					m_Solver.phases[solverIndices[i]] |= 67108864;
				}
				else
				{
					m_Solver.phases[solverIndices[i]] &= -67108865;
				}
			}
		}

		public void SetSimplicesDirty()
		{
			if (m_Solver != null)
			{
				m_Solver.dirtySimplices |= simplexTypes;
			}
		}

		public void SetConstraintsDirty(Oni.ConstraintType constraintType)
		{
			if (m_Solver != null)
			{
				m_Solver.dirtyConstraints |= 1 << (int)constraintType;
			}
		}

		public void SetRenderingDirty(Oni.RenderingSystemType rendererType)
		{
			if (m_Solver != null)
			{
				m_Solver.dirtyRendering |= (int)rendererType;
			}
		}

		public IObiConstraints GetConstraintsByType(Oni.ConstraintType type)
		{
			switch (type)
			{
			case Oni.ConstraintType.Pin:
				return m_PinConstraints;
			case Oni.ConstraintType.Pinhole:
				return m_PinholeConstraints;
			default:
				if (sharedBlueprint != null)
				{
					return sharedBlueprint.GetConstraintsByType(type);
				}
				return null;
			}
		}

		public virtual void UpdateParticleProperties()
		{
		}

		public int GetParticleRuntimeIndex(int actorIndex)
		{
			if (isLoaded)
			{
				return solverIndices[actorIndex];
			}
			return actorIndex;
		}

		public Vector3 GetParticlePosition(int solverIndex)
		{
			if (isLoaded)
			{
				return m_Solver.transform.TransformPoint(m_Solver.renderablePositions[solverIndex]);
			}
			return Vector3.zero;
		}

		public Quaternion GetParticleOrientation(int solverIndex)
		{
			if (isLoaded)
			{
				return m_Solver.transform.rotation * m_Solver.renderableOrientations[solverIndex];
			}
			return Quaternion.identity;
		}

		public Vector3 GetParticleRestPosition(int solverIndex)
		{
			if (isLoaded)
			{
				return m_Solver.restPositions[solverIndex];
			}
			return Vector3.zero;
		}

		public Quaternion GetParticleRestOrientation(int solverIndex)
		{
			if (isLoaded)
			{
				return m_Solver.restOrientations[solverIndex];
			}
			return Quaternion.identity;
		}

		public void GetParticleAnisotropy(int solverIndex, ref Vector4 b1, ref Vector4 b2, ref Vector4 b3)
		{
			if (isLoaded && usesAnisotropicParticles)
			{
				b1 = m_Solver.transform.TransformDirection(m_Solver.renderableOrientations[solverIndex] * Vector3.right);
				b2 = m_Solver.transform.TransformDirection(m_Solver.renderableOrientations[solverIndex] * Vector3.up);
				b3 = m_Solver.transform.TransformDirection(m_Solver.renderableOrientations[solverIndex] * Vector3.forward);
				b1[3] = m_Solver.maxScale * m_Solver.renderableRadii[solverIndex][0];
				b2[3] = m_Solver.maxScale * m_Solver.renderableRadii[solverIndex][1];
				b3[3] = m_Solver.maxScale * m_Solver.renderableRadii[solverIndex][2];
			}
			else
			{
				float num = (b3[3] = m_Solver.maxScale * m_Solver.renderableRadii[solverIndex][0]);
				float value = (b2[3] = num);
				b1[3] = value;
			}
		}

		public float GetParticleMaxRadius(int solverIndex)
		{
			if (isLoaded)
			{
				return m_Solver.maxScale * m_Solver.principalRadii[solverIndex][0];
			}
			return 0f;
		}

		public Color GetParticleColor(int solverIndex)
		{
			if (isLoaded)
			{
				return m_Solver.colors[solverIndex];
			}
			return Color.white;
		}

		public void SetFilterCategory(int newCategory)
		{
			newCategory = Mathf.Clamp(newCategory, 0, 15);
			for (int i = 0; i < particleCount; i++)
			{
				int index = solverIndices[i];
				int maskFromFilter = ObiUtils.GetMaskFromFilter(solver.filters[index]);
				solver.filters[index] = ObiUtils.MakeFilter(maskFromFilter, newCategory);
			}
		}

		public void SetFilterMask(int newMask)
		{
			newMask = Mathf.Clamp(newMask, 0, 65535);
			for (int i = 0; i < particleCount; i++)
			{
				int index = solverIndices[i];
				int categoryFromFilter = ObiUtils.GetCategoryFromFilter(solver.filters[index]);
				solver.filters[index] = ObiUtils.MakeFilter(newMask, categoryFromFilter);
			}
		}

		public void SetMass(float mass)
		{
			if (Application.isPlaying && isLoaded && activeParticleCount > 0)
			{
				float value = 1f / (mass / (float)activeParticleCount);
				for (int i = 0; i < activeParticleCount; i++)
				{
					int index = solverIndices[i];
					m_Solver.invMasses[index] = value;
					m_Solver.invRotationalMasses[index] = value;
				}
				UpdateParticleProperties();
			}
		}

		public float GetMass(out Vector3 com)
		{
			float num = 0f;
			com = Vector3.zero;
			if (Application.isPlaying && isLoaded && activeParticleCount > 0)
			{
				Vector4 zero = Vector4.zero;
				for (int i = 0; i < activeParticleCount; i++)
				{
					if (m_Solver.invMasses[solverIndices[i]] > 0f)
					{
						float num2 = 1f / m_Solver.invMasses[solverIndices[i]];
						num += num2;
						zero += m_Solver.positions[solverIndices[i]] * num2;
					}
				}
				com = zero;
				if (num > float.Epsilon)
				{
					com /= num;
				}
			}
			return num;
		}

		public void AddForce(Vector3 force, ForceMode forceMode)
		{
			if (force.sqrMagnitude > Mathf.Epsilon)
			{
				bufferedForces.dirty = true;
			}
			switch (forceMode)
			{
			case ForceMode.Force:
				bufferedForces.force += (Vector4)force;
				break;
			case ForceMode.Acceleration:
				bufferedForces.acceleration += (Vector4)force;
				break;
			case ForceMode.Impulse:
				bufferedForces.impulse += (Vector4)force;
				break;
			case ForceMode.VelocityChange:
				bufferedForces.velChange += (Vector4)force;
				break;
			case (ForceMode)3:
			case (ForceMode)4:
				break;
			}
		}

		public void AddTorque(Vector3 force, ForceMode forceMode)
		{
			if (force.sqrMagnitude > Mathf.Epsilon)
			{
				bufferedForces.dirty = true;
			}
			switch (forceMode)
			{
			case ForceMode.Force:
				bufferedForces.angularForce += (Vector4)force;
				break;
			case ForceMode.Acceleration:
				bufferedForces.angularAcceleration += (Vector4)force;
				break;
			case ForceMode.Impulse:
				bufferedForces.angularImpulse += (Vector4)force;
				break;
			case ForceMode.VelocityChange:
				bufferedForces.angularVelChange += (Vector4)force;
				break;
			case (ForceMode)3:
			case (ForceMode)4:
				break;
			}
		}

		private void LoadBlueprintParticles(ObiActorBlueprint bp)
		{
			Matrix4x4 matrix4x = actorLocalToSolverMatrix;
			Quaternion rotation = matrix4x.rotation;
			for (int i = 0; i < solverIndices.count; i++)
			{
				int index = solverIndices[i];
				if (bp.positions != null && i < bp.positions.Length)
				{
					ObiNativeVector4List endPositions = m_Solver.endPositions;
					ObiNativeVector4List startPositions = m_Solver.startPositions;
					ObiNativeVector4List prevPositions = m_Solver.prevPositions;
					Vector4 vector = (m_Solver.positions[index] = matrix4x.MultiplyPoint3x4(bp.positions[i]));
					Vector4 vector3 = (prevPositions[index] = vector);
					Vector4 value = (startPositions[index] = vector3);
					endPositions[index] = value;
					m_Solver.renderablePositions[index] = matrix4x.MultiplyPoint3x4(bp.positions[i]);
				}
				if (bp.orientations != null && i < bp.orientations.Length)
				{
					ObiNativeQuaternionList endOrientations = m_Solver.endOrientations;
					ObiNativeQuaternionList startOrientations = m_Solver.startOrientations;
					ObiNativeQuaternionList prevOrientations = m_Solver.prevOrientations;
					Quaternion quaternion = (m_Solver.orientations[index] = rotation * bp.orientations[i]);
					Quaternion quaternion3 = (prevOrientations[index] = quaternion);
					Quaternion value2 = (startOrientations[index] = quaternion3);
					endOrientations[index] = value2;
					m_Solver.renderableOrientations[index] = rotation * bp.orientations[i];
				}
				if (bp.restNormals != null && i < bp.restNormals.Length)
				{
					m_Solver.normals[index] = bp.restNormals[i];
				}
				if (bp.restPositions != null && i < bp.restPositions.Length)
				{
					m_Solver.restPositions[index] = bp.restPositions[i];
				}
				if (bp.restOrientations != null && i < bp.restOrientations.Length)
				{
					m_Solver.restOrientations[index] = bp.restOrientations[i];
				}
				if (bp.velocities != null && i < bp.velocities.Length)
				{
					m_Solver.velocities[index] = matrix4x.MultiplyVector(bp.velocities[i]);
				}
				if (bp.angularVelocities != null && i < bp.angularVelocities.Length)
				{
					m_Solver.angularVelocities[index] = matrix4x.MultiplyVector(bp.angularVelocities[i]);
				}
				if (bp.invMasses != null && i < bp.invMasses.Length)
				{
					m_Solver.invMasses[index] = bp.invMasses[i] / m_MassScale;
				}
				if (bp.invRotationalMasses != null && i < bp.invRotationalMasses.Length)
				{
					m_Solver.invRotationalMasses[index] = bp.invRotationalMasses[i] / m_MassScale;
				}
				if (bp.principalRadii != null && i < bp.principalRadii.Length)
				{
					Vector4 value3 = bp.principalRadii[i];
					value3.w = ((i < sourceBlueprint.activeParticleCount) ? 1 : 0);
					m_Solver.principalRadii[index] = value3;
				}
				else
				{
					m_Solver.principalRadii[index] = Vector4.zero;
				}
				if (bp.filters != null && i < bp.filters.Length)
				{
					m_Solver.filters[index] = bp.filters[i];
				}
				if (bp.colors != null && i < bp.colors.Length)
				{
					m_Solver.colors[index] = bp.colors[i];
				}
				m_Solver.phases[index] = ObiUtils.MakePhase(groupID, (ObiUtils.ParticleFlags)0);
			}
			m_ActiveParticleCount[0] = sourceBlueprint.activeParticleCount;
			m_Solver.dirtyActiveParticles = true;
			m_Solver.dirtyDeformableTriangles = true;
			m_Solver.dirtyDeformableEdges = true;
			m_Solver.dirtySimplices |= simplexTypes;
			m_Solver.dirtyConstraints |= -1;
			UpdateCollisionMaterials();
		}

		private void UnloadBlueprintParticles()
		{
			m_ActiveParticleCount[0] = 0;
			m_Solver.dirtyActiveParticles = true;
			m_Solver.dirtyDeformableTriangles = true;
			m_Solver.dirtyDeformableEdges = true;
			m_Solver.dirtySimplices |= simplexTypes;
			m_Solver.dirtyConstraints |= -1;
		}

		public void ResetParticles()
		{
			if (!isLoaded)
			{
				return;
			}
			Matrix4x4 matrix4x = actorLocalToSolverMatrix;
			Quaternion rotation = matrix4x.rotation;
			for (int i = 0; i < particleCount; i++)
			{
				int index = solverIndices[i];
				ObiNativeVector4List startPositions = solver.startPositions;
				ObiNativeVector4List endPositions = solver.endPositions;
				Vector4 vector = (solver.positions[index] = matrix4x.MultiplyPoint3x4(sourceBlueprint.positions[i]));
				Vector4 value = (endPositions[index] = vector);
				startPositions[index] = value;
				solver.velocities[index] = matrix4x.MultiplyVector(sourceBlueprint.velocities[i]);
				if (usesOrientedParticles)
				{
					ObiNativeQuaternionList startOrientations = solver.startOrientations;
					ObiNativeQuaternionList endOrientations = solver.endOrientations;
					Quaternion quaternion = (solver.orientations[index] = rotation * sourceBlueprint.orientations[i]);
					Quaternion value2 = (endOrientations[index] = quaternion);
					startOrientations[index] = value2;
					solver.angularVelocities[index] = matrix4x.MultiplyVector(sourceBlueprint.angularVelocities[i]);
				}
			}
		}

		public bool SaveStateToBlueprint(ObiActorBlueprint bp)
		{
			if (bp == null || !m_Loaded)
			{
				return false;
			}
			Matrix4x4 inverse = actorLocalToSolverMatrix.inverse;
			_ = inverse.rotation;
			for (int i = 0; i < solverIndices.count; i++)
			{
				int num = solverIndices[i];
				if (bp.positions != null && m_Solver.positions != null && num < m_Solver.positions.count && i < bp.positions.Length)
				{
					bp.positions[i] = inverse.MultiplyPoint3x4(m_Solver.positions[num]);
				}
				if (bp.velocities != null && m_Solver.velocities != null && num < m_Solver.velocities.count && i < bp.velocities.Length)
				{
					bp.velocities[i] = inverse.MultiplyVector(m_Solver.velocities[num]);
				}
			}
			return true;
		}

		protected void StoreState()
		{
			UnityEngine.Object.DestroyImmediate(m_State);
			m_State = UnityEngine.Object.Instantiate(sourceBlueprint);
			SaveStateToBlueprint(m_State);
		}

		public void ClearState()
		{
			UnityEngine.Object.DestroyImmediate(m_State);
		}

		internal virtual void LoadBlueprint()
		{
			ObiActorBlueprint bp = sharedBlueprint;
			if (Application.isPlaying)
			{
				bp = ((m_State != null) ? m_State : sourceBlueprint);
			}
			m_Loaded = true;
			LoadBlueprintParticles(bp);
			this.OnBlueprintLoaded?.Invoke(this, bp);
		}

		internal virtual void UnloadBlueprint()
		{
			if (Application.isPlaying)
			{
				StoreState();
			}
			m_Loaded = false;
			UnloadBlueprintParticles();
			this.OnBlueprintUnloaded?.Invoke(this, sharedBlueprint);
		}

		public virtual void SimulationStart(float timeToSimulate, float substepTime)
		{
			this.OnSimulationStart?.Invoke(this, timeToSimulate, substepTime);
			if (!bufferedForces.dirty)
			{
				return;
			}
			Vector3 com;
			float mass = GetMass(out com);
			if (!float.IsInfinity(mass))
			{
				foreach (int solverIndex in solverIndices)
				{
					Vector4 vector = bufferedForces.force / m_Solver.invMasses[solverIndex] / mass;
					vector += bufferedForces.acceleration / m_Solver.invMasses[solverIndex];
					vector += bufferedForces.impulse / m_Solver.invMasses[solverIndex] / mass / timeToSimulate;
					vector += bufferedForces.velChange / m_Solver.invMasses[solverIndex] / timeToSimulate;
					m_Solver.externalForces[solverIndex] += vector;
					vector = bufferedForces.angularForce / m_Solver.invMasses[solverIndex] / mass;
					vector += bufferedForces.angularAcceleration / m_Solver.invMasses[solverIndex];
					vector += bufferedForces.angularImpulse / m_Solver.invMasses[solverIndex] / mass / timeToSimulate;
					vector += bufferedForces.angularVelChange / m_Solver.invMasses[solverIndex] / timeToSimulate;
					m_Solver.externalForces[solverIndex] += (Vector4)Vector3.Cross(vector, (Vector3)m_Solver.positions[solverIndex] - com);
				}
			}
			bufferedForces.Clear();
		}

		public virtual void CollisionDetectionStart(float simulatedTime, float substepTime)
		{
			this.OnCollisionDetectionStart?.Invoke(this, simulatedTime, substepTime);
		}

		public virtual void SubstepsStart(float simulatedTime, float substepTime)
		{
			this.OnSubstepsStart?.Invoke(this, simulatedTime, substepTime);
		}

		public virtual void SimulationEnd(float simulatedTime, float substepTime)
		{
			this.OnSimulationEnd?.Invoke(this, simulatedTime, substepTime);
		}

		public virtual void RequestReadback()
		{
			this.OnRequestReadback?.Invoke(this);
		}

		public virtual void Interpolate(float simulatedTime, float substepTime)
		{
			if (!Application.isPlaying && isLoaded)
			{
				Matrix4x4 matrix4x = actorLocalToSolverMatrix;
				Quaternion rotation = matrix4x.rotation;
				for (int i = 0; i < solverIndices.count; i++)
				{
					int index = solverIndices[i];
					if (sourceBlueprint.positions != null && i < sourceBlueprint.positions.Length)
					{
						ObiNativeVector4List renderablePositions = m_Solver.renderablePositions;
						ObiNativeVector4List positions = m_Solver.positions;
						ObiNativeVector4List startPositions = m_Solver.startPositions;
						Vector4 vector = (m_Solver.endPositions[index] = matrix4x.MultiplyPoint3x4(sourceBlueprint.positions[i]));
						Vector4 vector3 = (startPositions[index] = vector);
						Vector4 value = (positions[index] = vector3);
						renderablePositions[index] = value;
					}
					if (sourceBlueprint.orientations != null && i < sourceBlueprint.orientations.Length)
					{
						ObiNativeQuaternionList renderableOrientations = m_Solver.renderableOrientations;
						ObiNativeQuaternionList orientations = m_Solver.orientations;
						ObiNativeQuaternionList startOrientations = m_Solver.startOrientations;
						Quaternion quaternion = (m_Solver.endOrientations[index] = rotation * sourceBlueprint.orientations[i]);
						Quaternion quaternion3 = (startOrientations[index] = quaternion);
						Quaternion value2 = (orientations[index] = quaternion3);
						renderableOrientations[index] = value2;
					}
				}
			}
			this.OnInterpolate?.Invoke(this, simulatedTime, substepTime);
		}

		public virtual void OnSolverVisibilityChanged(bool visible)
		{
		}
	}
}
