using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Bone", 882)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(100)]
	public class ObiBone : ObiActor, IStretchShearConstraintsUser, IBendTwistConstraintsUser, ISkinConstraintsUser, IAerodynamicConstraintsUser
	{
		[Serializable]
		public class BonePropertyCurve
		{
			[Min(0f)]
			public float multiplier;

			public AnimationCurve curve;

			public BonePropertyCurve(float multiplier, float curveValue)
			{
				this.multiplier = multiplier;
				curve = new AnimationCurve(new Keyframe(0f, curveValue), new Keyframe(1f, curveValue));
			}

			public float Evaluate(float time)
			{
				return curve.Evaluate(time) * multiplier;
			}
		}

		[Serializable]
		public class IgnoredBone
		{
			public Transform bone;

			public bool ignoreChildren;
		}

		[NonSerialized]
		protected ObiBoneBlueprint m_BoneBlueprint;

		[SerializeField]
		protected bool m_SelfCollisions;

		[SerializeField]
		protected BonePropertyCurve _radius = new BonePropertyCurve(0.1f, 1f);

		[SerializeField]
		protected BonePropertyCurve _mass = new BonePropertyCurve(0.1f, 1f);

		[SerializeField]
		protected BonePropertyCurve _rotationalMass = new BonePropertyCurve(0.1f, 1f);

		[SerializeField]
		protected bool _skinConstraintsEnabled = true;

		[SerializeField]
		protected BonePropertyCurve _skinCompliance = new BonePropertyCurve(0.01f, 1f);

		[SerializeField]
		protected BonePropertyCurve _skinRadius = new BonePropertyCurve(0.1f, 1f);

		[SerializeField]
		protected bool _stretchShearConstraintsEnabled = true;

		[SerializeField]
		protected BonePropertyCurve _stretchCompliance = new BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected BonePropertyCurve _shear1Compliance = new BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected BonePropertyCurve _shear2Compliance = new BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected bool _bendTwistConstraintsEnabled = true;

		[SerializeField]
		protected BonePropertyCurve _torsionCompliance = new BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected BonePropertyCurve _bend1Compliance = new BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected BonePropertyCurve _bend2Compliance = new BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected BonePropertyCurve _plasticYield = new BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected BonePropertyCurve _plasticCreep = new BonePropertyCurve(0f, 1f);

		[SerializeField]
		protected bool _aerodynamicsEnabled = true;

		[SerializeField]
		protected BonePropertyCurve _drag = new BonePropertyCurve(0.05f, 1f);

		[SerializeField]
		protected BonePropertyCurve _lift = new BonePropertyCurve(0.02f, 1f);

		[Tooltip("Filter used for collision detection.")]
		[SerializeField]
		private int filter = ObiUtils.MakeFilter(65535, 1);

		public bool fixRoot = true;

		public bool stretchBones = true;

		public List<IgnoredBone> ignored = new List<IgnoredBone>();

		public int Filter
		{
			get
			{
				return filter;
			}
			set
			{
				if (filter != value)
				{
					filter = value;
					UpdateFilter();
				}
			}
		}

		public bool selfCollisions
		{
			get
			{
				return m_SelfCollisions;
			}
			set
			{
				if (value != m_SelfCollisions)
				{
					m_SelfCollisions = value;
					SetSelfCollisions(m_SelfCollisions);
				}
			}
		}

		public BonePropertyCurve radius
		{
			get
			{
				return _radius;
			}
			set
			{
				_radius = value;
				UpdateRadius();
			}
		}

		public BonePropertyCurve mass
		{
			get
			{
				return _mass;
			}
			set
			{
				_mass = value;
				UpdateMasses();
			}
		}

		public BonePropertyCurve rotationalMass
		{
			get
			{
				return _rotationalMass;
			}
			set
			{
				_rotationalMass = value;
				UpdateMasses();
			}
		}

		public bool skinConstraintsEnabled
		{
			get
			{
				return _skinConstraintsEnabled;
			}
			set
			{
				if (value != _skinConstraintsEnabled)
				{
					_skinConstraintsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.Skin);
				}
			}
		}

		public BonePropertyCurve skinCompliance
		{
			get
			{
				return _skinCompliance;
			}
			set
			{
				_skinCompliance = value;
				SetConstraintsDirty(Oni.ConstraintType.Skin);
			}
		}

		public BonePropertyCurve skinRadius
		{
			get
			{
				return _skinRadius;
			}
			set
			{
				_skinRadius = value;
				SetConstraintsDirty(Oni.ConstraintType.Skin);
			}
		}

		public bool stretchShearConstraintsEnabled
		{
			get
			{
				return _stretchShearConstraintsEnabled;
			}
			set
			{
				if (value != _stretchShearConstraintsEnabled)
				{
					_stretchShearConstraintsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.StretchShear);
				}
			}
		}

		public BonePropertyCurve stretchCompliance
		{
			get
			{
				return _stretchCompliance;
			}
			set
			{
				_stretchCompliance = value;
				SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public BonePropertyCurve shear1Compliance
		{
			get
			{
				return _shear1Compliance;
			}
			set
			{
				_shear1Compliance = value;
				SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public BonePropertyCurve shear2Compliance
		{
			get
			{
				return _shear2Compliance;
			}
			set
			{
				_shear2Compliance = value;
				SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			}
		}

		public bool bendTwistConstraintsEnabled
		{
			get
			{
				return _bendTwistConstraintsEnabled;
			}
			set
			{
				if (value != _bendTwistConstraintsEnabled)
				{
					_bendTwistConstraintsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.BendTwist);
				}
			}
		}

		public BonePropertyCurve torsionCompliance
		{
			get
			{
				return _torsionCompliance;
			}
			set
			{
				_torsionCompliance = value;
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public BonePropertyCurve bend1Compliance
		{
			get
			{
				return _bend1Compliance;
			}
			set
			{
				_bend1Compliance = value;
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public BonePropertyCurve bend2Compliance
		{
			get
			{
				return _bend2Compliance;
			}
			set
			{
				_bend2Compliance = value;
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public BonePropertyCurve plasticYield
		{
			get
			{
				return _plasticYield;
			}
			set
			{
				_plasticYield = value;
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public BonePropertyCurve plasticCreep
		{
			get
			{
				return _plasticCreep;
			}
			set
			{
				_plasticCreep = value;
				SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			}
		}

		public bool aerodynamicsEnabled
		{
			get
			{
				return _aerodynamicsEnabled;
			}
			set
			{
				if (value != _aerodynamicsEnabled)
				{
					_aerodynamicsEnabled = value;
					SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
				}
			}
		}

		public BonePropertyCurve drag
		{
			get
			{
				return _drag;
			}
			set
			{
				_drag = value;
				SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			}
		}

		public BonePropertyCurve lift
		{
			get
			{
				return _lift;
			}
			set
			{
				_lift = value;
				SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			}
		}

		public override ObiActorBlueprint sourceBlueprint => m_BoneBlueprint;

		public ObiBoneBlueprint boneBlueprint
		{
			get
			{
				return m_BoneBlueprint;
			}
			set
			{
				if (m_BoneBlueprint != value)
				{
					RemoveFromSolver();
					ClearState();
					m_BoneBlueprint = value;
					AddToSolver();
				}
			}
		}

		protected override void Awake()
		{
			m_BoneBlueprint = ScriptableObject.CreateInstance<ObiBoneBlueprint>();
			UpdateBlueprint();
			base.Awake();
		}

		protected override void OnDestroy()
		{
			if (m_BoneBlueprint != null)
			{
				UnityEngine.Object.DestroyImmediate(m_BoneBlueprint);
			}
			base.OnDestroy();
		}

		protected override void OnValidate()
		{
			base.OnValidate();
			UpdateFilter();
			UpdateRadius();
			UpdateMasses();
			SetupRuntimeConstraints();
		}

		public void UpdateBlueprint()
		{
			if (m_BoneBlueprint != null)
			{
				m_BoneBlueprint.root = base.transform;
				m_BoneBlueprint.ignored = ignored;
				m_BoneBlueprint.mass = mass;
				m_BoneBlueprint.rotationalMass = rotationalMass;
				m_BoneBlueprint.radius = radius;
				m_BoneBlueprint.GenerateImmediate();
			}
		}

		internal override void LoadBlueprint()
		{
			base.LoadBlueprint();
			base.solver.renderablePositions.Readback(async: false);
			base.solver.renderableOrientations.Readback(async: false);
			base.solver.orientations.Readback(async: false);
			base.solver.angularVelocities.Readback(async: false);
			SetupRuntimeConstraints();
			ResetToCurrentShape();
		}

		internal override void UnloadBlueprint()
		{
			ResetParticles();
			CopyParticleDataToTransforms();
			base.UnloadBlueprint();
		}

		public override void RequestReadback()
		{
			base.RequestReadback();
			base.solver.orientations.Readback();
			base.solver.angularVelocities.Readback();
			base.solver.renderablePositions.Readback();
			base.solver.renderableOrientations.Readback();
		}

		public override void SimulationEnd(float simulatedTime, float substepTime)
		{
			base.SimulationEnd(simulatedTime, substepTime);
			base.solver.orientations.WaitForReadback();
			base.solver.angularVelocities.WaitForReadback();
			base.solver.renderablePositions.WaitForReadback();
			base.solver.renderableOrientations.WaitForReadback();
		}

		private void SetupRuntimeConstraints()
		{
			SetConstraintsDirty(Oni.ConstraintType.Skin);
			SetConstraintsDirty(Oni.ConstraintType.StretchShear);
			SetConstraintsDirty(Oni.ConstraintType.BendTwist);
			SetConstraintsDirty(Oni.ConstraintType.Aerodynamics);
			SetSelfCollisions(selfCollisions);
			SetSimplicesDirty();
			UpdateFilter();
		}

		public override void ProvideDeformableEdges(ObiNativeIntList deformableEdges)
		{
			ObiBoneBlueprint obiBoneBlueprint = base.sharedBlueprint as ObiBoneBlueprint;
			if (obiBoneBlueprint != null && obiBoneBlueprint.deformableEdges != null)
			{
				for (int i = 0; i < obiBoneBlueprint.deformableEdges.Length; i++)
				{
					deformableEdges.Add(solverIndices[obiBoneBlueprint.deformableEdges[i]]);
				}
			}
		}

		private void FixRoot()
		{
			if (base.isLoaded)
			{
				int index = solverIndices[0];
				Matrix4x4 matrix4x = base.actorLocalToSolverMatrix;
				Quaternion rotation = matrix4x.rotation;
				base.solver.invMasses[index] = 0f;
				base.solver.invRotationalMasses[index] = 0f;
				base.solver.velocities[index] = Vector4.zero;
				base.solver.angularVelocities[index] = Vector4.zero;
				ObiNativeVector4List startPositions = base.solver.startPositions;
				ObiNativeVector4List endPositions = base.solver.endPositions;
				Vector4 vector = (base.solver.positions[index] = matrix4x.MultiplyPoint3x4(Vector3.zero));
				Vector4 value = (endPositions[index] = vector);
				startPositions[index] = value;
				ObiNativeQuaternionList startOrientations = base.solver.startOrientations;
				ObiNativeQuaternionList endOrientations = base.solver.endOrientations;
				Quaternion quaternion = (base.solver.orientations[index] = rotation * boneBlueprint.orientations[0]);
				Quaternion value2 = (endOrientations[index] = quaternion);
				startOrientations[index] = value2;
			}
		}

		private void UpdateFilter()
		{
			for (int i = 0; i < base.particleCount; i++)
			{
				boneBlueprint.filters[i] = filter;
				if (base.isLoaded)
				{
					base.solver.filters[solverIndices[i]] = filter;
				}
			}
		}

		public void UpdateRadius()
		{
			for (int i = 0; i < base.particleCount; i++)
			{
				float normalizedLength;
				ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(i, out normalizedLength);
				Vector3 vector = Vector3.one * ((obiBoneOverride != null) ? obiBoneOverride.radius.Evaluate(normalizedLength) : radius.Evaluate(normalizedLength));
				boneBlueprint.principalRadii[i] = vector;
				if (base.isLoaded)
				{
					base.solver.principalRadii[solverIndices[i]] = vector;
				}
			}
		}

		public void UpdateMasses()
		{
			for (int i = 0; i < base.particleCount; i++)
			{
				float normalizedLength;
				ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(i, out normalizedLength);
				float num = ObiUtils.MassToInvMass((obiBoneOverride != null) ? obiBoneOverride.mass.Evaluate(normalizedLength) : mass.Evaluate(normalizedLength));
				float num2 = ObiUtils.MassToInvMass((obiBoneOverride != null) ? obiBoneOverride.rotationalMass.Evaluate(normalizedLength) : rotationalMass.Evaluate(normalizedLength));
				boneBlueprint.invMasses[i] = num;
				boneBlueprint.invRotationalMasses[i] = num2;
				if (base.isLoaded)
				{
					base.solver.invMasses[solverIndices[i]] = num;
					base.solver.invRotationalMasses[solverIndices[i]] = num2;
				}
			}
		}

		public Vector3 GetSkinRadiiBackstop(ObiSkinConstraintsBatch batch, int constraintIndex)
		{
			float normalizedLength;
			ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(batch.particleIndices[constraintIndex], out normalizedLength);
			return new Vector3((obiBoneOverride != null) ? obiBoneOverride.skinRadius.Evaluate(normalizedLength) : skinRadius.Evaluate(normalizedLength), 0f, 0f);
		}

		public float GetSkinCompliance(ObiSkinConstraintsBatch batch, int constraintIndex)
		{
			float normalizedLength;
			ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(batch.particleIndices[constraintIndex], out normalizedLength);
			if (!(obiBoneOverride != null))
			{
				return skinCompliance.Evaluate(normalizedLength);
			}
			return obiBoneOverride.skinCompliance.Evaluate(normalizedLength);
		}

		public Vector3 GetBendTwistCompliance(ObiBendTwistConstraintsBatch batch, int constraintIndex)
		{
			float normalizedLength;
			ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(batch.particleIndices[constraintIndex * 2], out normalizedLength);
			if (obiBoneOverride != null)
			{
				return new Vector3(obiBoneOverride.bend1Compliance.Evaluate(normalizedLength), obiBoneOverride.bend2Compliance.Evaluate(normalizedLength), obiBoneOverride.torsionCompliance.Evaluate(normalizedLength));
			}
			return new Vector3(bend1Compliance.Evaluate(normalizedLength), bend2Compliance.Evaluate(normalizedLength), torsionCompliance.Evaluate(normalizedLength));
		}

		public Vector2 GetBendTwistPlasticity(ObiBendTwistConstraintsBatch batch, int constraintIndex)
		{
			float normalizedLength;
			ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(batch.particleIndices[constraintIndex * 2], out normalizedLength);
			if (obiBoneOverride != null)
			{
				return new Vector2(obiBoneOverride.plasticYield.Evaluate(normalizedLength), obiBoneOverride.plasticCreep.Evaluate(normalizedLength));
			}
			return new Vector2(plasticYield.Evaluate(normalizedLength), plasticCreep.Evaluate(normalizedLength));
		}

		public Vector3 GetStretchShearCompliance(ObiStretchShearConstraintsBatch batch, int constraintIndex)
		{
			float normalizedLength;
			ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(batch.particleIndices[constraintIndex * 2], out normalizedLength);
			if (obiBoneOverride != null)
			{
				return new Vector3(obiBoneOverride.shear1Compliance.Evaluate(normalizedLength), obiBoneOverride.shear2Compliance.Evaluate(normalizedLength), obiBoneOverride.stretchCompliance.Evaluate(normalizedLength));
			}
			return new Vector3(shear1Compliance.Evaluate(normalizedLength), shear2Compliance.Evaluate(normalizedLength), stretchCompliance.Evaluate(normalizedLength));
		}

		public float GetDrag(ObiAerodynamicConstraintsBatch batch, int constraintIndex)
		{
			float normalizedLength;
			ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(batch.particleIndices[constraintIndex], out normalizedLength);
			if (!(obiBoneOverride != null))
			{
				return drag.Evaluate(normalizedLength);
			}
			return obiBoneOverride.drag.Evaluate(normalizedLength);
		}

		public float GetLift(ObiAerodynamicConstraintsBatch batch, int constraintIndex)
		{
			float normalizedLength;
			ObiBoneOverride obiBoneOverride = boneBlueprint.GetOverride(batch.particleIndices[constraintIndex], out normalizedLength);
			if (!(obiBoneOverride != null))
			{
				return lift.Evaluate(normalizedLength);
			}
			return obiBoneOverride.lift.Evaluate(normalizedLength);
		}

		public void FixedUpdate()
		{
			ResetReferenceOrientations();
		}

		public override void SimulationStart(float timeToSimulate, float substepTime)
		{
			base.SimulationStart(timeToSimulate, substepTime);
			if (fixRoot)
			{
				FixRoot();
			}
			UpdateRestShape();
		}

		public void LateUpdate()
		{
			if (Application.isPlaying && base.isActiveAndEnabled)
			{
				CopyParticleDataToTransforms();
			}
		}

		public void ResetToCurrentShape()
		{
			if (!base.isLoaded)
			{
				return;
			}
			Matrix4x4 worldToLocalMatrix = base.solver.transform.worldToLocalMatrix;
			for (int i = 0; i < base.particleCount; i++)
			{
				Transform transform = boneBlueprint.transforms[i];
				int index = solverIndices[i];
				base.solver.velocities[index] = Vector4.zero;
				base.solver.angularVelocities[index] = Vector4.zero;
				ObiNativeVector4List startPositions = base.solver.startPositions;
				ObiNativeVector4List endPositions = base.solver.endPositions;
				Vector4 vector = (base.solver.positions[index] = worldToLocalMatrix.MultiplyPoint3x4(transform.position));
				Vector4 value = (endPositions[index] = vector);
				startPositions[index] = value;
				Quaternion quaternion = transform.rotation * Quaternion.Inverse(boneBlueprint.restOrientations[i]);
				ObiNativeQuaternionList startOrientations = base.solver.startOrientations;
				ObiNativeQuaternionList endOrientations = base.solver.endOrientations;
				Quaternion quaternion2 = (base.solver.orientations[index] = worldToLocalMatrix.rotation * quaternion * boneBlueprint.root2WorldR * boneBlueprint.orientations[i]);
				Quaternion value2 = (endOrientations[index] = quaternion2);
				startOrientations[index] = value2;
			}
			if (GetConstraintsByType(Oni.ConstraintType.BendTwist) is ObiConstraints<ObiBendTwistConstraintsBatch> obiConstraints)
			{
				for (int j = 0; j < obiConstraints.batchCount; j++)
				{
					ObiBendTwistConstraintsBatch obiBendTwistConstraintsBatch = obiConstraints.GetBatch(j) as ObiBendTwistConstraintsBatch;
					for (int k = 0; k < obiBendTwistConstraintsBatch.activeConstraintCount; k++)
					{
						int num = obiBendTwistConstraintsBatch.particleIndices[k * 2];
						int num2 = obiBendTwistConstraintsBatch.particleIndices[k * 2 + 1];
						Quaternion quaternion5 = boneBlueprint.transforms[num].rotation * Quaternion.Inverse(boneBlueprint.restOrientations[num]);
						Quaternion quaternion6 = boneBlueprint.transforms[num2].rotation * Quaternion.Inverse(boneBlueprint.restOrientations[num2]);
						Quaternion q = quaternion5 * boneBlueprint.root2WorldR * boneBlueprint.orientations[num];
						Quaternion q2 = quaternion6 * boneBlueprint.root2WorldR * boneBlueprint.orientations[num2];
						obiBendTwistConstraintsBatch.restDarbouxVectors[k] = ObiUtils.RestDarboux(q, q2);
					}
				}
			}
			if (!(GetConstraintsByType(Oni.ConstraintType.Skin) is ObiConstraints<ObiSkinConstraintsBatch> obiConstraints2))
			{
				return;
			}
			for (int l = 0; l < obiConstraints2.batchCount; l++)
			{
				ObiSkinConstraintsBatch obiSkinConstraintsBatch = obiConstraints2.GetBatch(l) as ObiSkinConstraintsBatch;
				for (int m = 0; m < obiSkinConstraintsBatch.activeConstraintCount; m++)
				{
					int index2 = obiSkinConstraintsBatch.particleIndices[m];
					obiSkinConstraintsBatch.skinPoints[m] = base.solver.transform.worldToLocalMatrix.MultiplyPoint3x4(boneBlueprint.transforms[index2].position);
				}
			}
		}

		private void ResetReferenceOrientations()
		{
			if (boneBlueprint != null)
			{
				for (int i = 1; i < boneBlueprint.restTransformOrientations.Count; i++)
				{
					boneBlueprint.transforms[i].localRotation = boneBlueprint.restTransformOrientations[i];
				}
			}
		}

		private void UpdateRestShape()
		{
			ObiConstraints<ObiBendTwistConstraintsBatch> obiConstraints = GetConstraintsByType(Oni.ConstraintType.BendTwist) as ObiConstraints<ObiBendTwistConstraintsBatch>;
			ObiConstraints<ObiBendTwistConstraintsBatch> obiConstraints2 = base.solver.GetConstraintsByType(Oni.ConstraintType.BendTwist) as ObiConstraints<ObiBendTwistConstraintsBatch>;
			if (bendTwistConstraintsEnabled && obiConstraints != null && obiConstraints2 != null)
			{
				for (int i = 0; i < solverBatchOffsets[7].Count; i++)
				{
					ObiBendTwistConstraintsBatch obiBendTwistConstraintsBatch = obiConstraints.GetBatch(i) as ObiBendTwistConstraintsBatch;
					ObiBendTwistConstraintsBatch obiBendTwistConstraintsBatch2 = obiConstraints2.batches[i];
					int num = solverBatchOffsets[7][i];
					if (obiBendTwistConstraintsBatch2.restDarbouxVectors.isCreated)
					{
						if (obiBendTwistConstraintsBatch2.restDarbouxVectors.computeBuffer == null)
						{
							obiBendTwistConstraintsBatch2.restDarbouxVectors.SafeAsComputeBuffer<Vector4>();
						}
						for (int j = 0; j < obiBendTwistConstraintsBatch.activeConstraintCount; j++)
						{
							int num2 = obiBendTwistConstraintsBatch.particleIndices[j * 2];
							int num3 = obiBendTwistConstraintsBatch.particleIndices[j * 2 + 1];
							Quaternion quaternion = boneBlueprint.transforms[num2].rotation * Quaternion.Inverse(boneBlueprint.restOrientations[num2]);
							Quaternion quaternion2 = boneBlueprint.transforms[num3].rotation * Quaternion.Inverse(boneBlueprint.restOrientations[num3]);
							Quaternion q = quaternion * boneBlueprint.root2WorldR * boneBlueprint.orientations[num2];
							Quaternion q2 = quaternion2 * boneBlueprint.root2WorldR * boneBlueprint.orientations[num3];
							obiBendTwistConstraintsBatch2.restDarbouxVectors[num + j] = ObiUtils.RestDarboux(q, q2);
						}
						obiBendTwistConstraintsBatch2.restDarbouxVectors.Upload();
					}
				}
			}
			ObiConstraints<ObiSkinConstraintsBatch> obiConstraints3 = GetConstraintsByType(Oni.ConstraintType.Skin) as ObiConstraints<ObiSkinConstraintsBatch>;
			ObiConstraints<ObiSkinConstraintsBatch> obiConstraints4 = base.solver.GetConstraintsByType(Oni.ConstraintType.Skin) as ObiConstraints<ObiSkinConstraintsBatch>;
			if (!skinConstraintsEnabled || obiConstraints3 == null || obiConstraints4 == null)
			{
				return;
			}
			for (int k = 0; k < solverBatchOffsets[13].Count; k++)
			{
				ObiSkinConstraintsBatch obiSkinConstraintsBatch = obiConstraints3.GetBatch(k) as ObiSkinConstraintsBatch;
				ObiSkinConstraintsBatch obiSkinConstraintsBatch2 = obiConstraints4.batches[k];
				int num4 = solverBatchOffsets[13][k];
				if (obiSkinConstraintsBatch2.skinPoints.isCreated)
				{
					if (obiSkinConstraintsBatch2.skinPoints.computeBuffer == null)
					{
						obiSkinConstraintsBatch2.skinPoints.SafeAsComputeBuffer<Vector4>();
					}
					for (int l = 0; l < obiSkinConstraintsBatch.activeConstraintCount; l++)
					{
						int index = obiSkinConstraintsBatch.particleIndices[l];
						obiSkinConstraintsBatch2.skinPoints[num4 + l] = base.solver.transform.worldToLocalMatrix.MultiplyPoint3x4(boneBlueprint.transforms[index].position);
					}
					obiSkinConstraintsBatch2.skinPoints.Upload();
				}
			}
		}

		private void CopyParticleDataToTransforms()
		{
			if (!base.isLoaded || !(boneBlueprint != null))
			{
				return;
			}
			for (int i = 1; i < base.particleCount; i++)
			{
				Transform transform = boneBlueprint.transforms[i];
				if (stretchBones)
				{
					transform.position = GetParticlePosition(solverIndices[i]);
				}
				Quaternion quaternion = GetParticleOrientation(solverIndices[i]) * Quaternion.Inverse(boneBlueprint.root2WorldR * boneBlueprint.orientations[i]);
				transform.rotation = quaternion * boneBlueprint.restOrientations[i];
			}
		}
	}
}
