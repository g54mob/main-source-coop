using System;
using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Particle Attachment", 820)]
	[RequireComponent(typeof(ObiActor))]
	[ExecuteInEditMode]
	public class ObiParticleAttachment : MonoBehaviour
	{
		public enum AttachmentType
		{
			Static = 0,
			Dynamic = 1
		}

		[SerializeField]
		[HideInInspector]
		private ObiActor m_Actor;

		[SerializeField]
		[HideInInspector]
		private Transform m_Target;

		[SerializeField]
		[HideInInspector]
		private ObiParticleGroup m_ParticleGroup;

		[SerializeField]
		[HideInInspector]
		private AttachmentType m_AttachmentType;

		[SerializeField]
		[HideInInspector]
		private bool m_ConstrainOrientation;

		[SerializeField]
		[HideInInspector]
		private bool m_Projection;

		[SerializeField]
		[HideInInspector]
		private float m_Compliance;

		[NonSerialized]
		private ObiPinConstraintsBatch pinBatch;

		[NonSerialized]
		private ObiColliderBase attachedCollider;

		[NonSerialized]
		private int attachedColliderHandleIndex;

		[NonSerialized]
		private int[] m_SolverIndices;

		[NonSerialized]
		private Vector3[] m_PositionOffsets;

		[NonSerialized]
		private Quaternion[] m_OrientationOffsets;

		[Delayed]
		public float breakThreshold = float.PositiveInfinity;

		public ObiActor actor => m_Actor;

		public Transform target
		{
			get
			{
				return m_Target;
			}
			set
			{
				if (value != m_Target)
				{
					m_Target = value;
					Bind();
				}
			}
		}

		public ObiParticleGroup particleGroup
		{
			get
			{
				return m_ParticleGroup;
			}
			set
			{
				if (value != m_ParticleGroup)
				{
					m_ParticleGroup = value;
					Bind();
				}
			}
		}

		public bool isBound
		{
			get
			{
				if (m_Target != null && m_SolverIndices != null)
				{
					return m_PositionOffsets != null;
				}
				return false;
			}
		}

		public AttachmentType attachmentType
		{
			get
			{
				return m_AttachmentType;
			}
			set
			{
				if (value != m_AttachmentType)
				{
					DisableAttachment(m_AttachmentType);
					m_AttachmentType = value;
					EnableAttachment(m_AttachmentType);
				}
			}
		}

		public bool constrainOrientation
		{
			get
			{
				return m_ConstrainOrientation;
			}
			set
			{
				if (value != m_ConstrainOrientation)
				{
					DisableAttachment(m_AttachmentType);
					m_ConstrainOrientation = value;
					EnableAttachment(m_AttachmentType);
				}
			}
		}

		public bool projection
		{
			get
			{
				return m_Projection;
			}
			set
			{
				if (value != m_Projection)
				{
					DisableAttachment(m_AttachmentType);
					m_Projection = value;
					EnableAttachment(m_AttachmentType);
				}
			}
		}

		public float compliance
		{
			get
			{
				return m_Compliance;
			}
			set
			{
				if (Mathf.Approximately(value, m_Compliance))
				{
					return;
				}
				m_Compliance = value;
				if (m_AttachmentType == AttachmentType.Dynamic && pinBatch != null)
				{
					for (int i = 0; i < m_SolverIndices.Length; i++)
					{
						pinBatch.stiffnesses[i * 2] = m_Compliance;
					}
				}
			}
		}

		private void OnEnable()
		{
			m_Actor = GetComponent<ObiActor>();
			m_Actor.OnBlueprintLoaded += Actor_OnBlueprintLoaded;
			m_Actor.OnSimulationStart += Actor_OnSimulate;
			if (m_Actor.solver != null)
			{
				Actor_OnBlueprintLoaded(m_Actor, m_Actor.sourceBlueprint);
			}
			EnableAttachment(m_AttachmentType);
		}

		private void OnDisable()
		{
			DisableAttachment(m_AttachmentType);
			m_Actor.OnBlueprintLoaded -= Actor_OnBlueprintLoaded;
			m_Actor.OnSimulationStart -= Actor_OnSimulate;
		}

		private void OnValidate()
		{
			m_Actor = GetComponent<ObiActor>();
			DisableAttachment(AttachmentType.Static);
			DisableAttachment(AttachmentType.Dynamic);
			EnableAttachment(m_AttachmentType);
		}

		private void Actor_OnBlueprintLoaded(ObiActor act, ObiActorBlueprint blueprint)
		{
			Bind();
		}

		private void Actor_OnSimulate(ObiActor act, float stepTime, float substepTime)
		{
			UpdateAttachment();
			BreakDynamicAttachment(substepTime);
		}

		private void Bind()
		{
			DisableAttachment(m_AttachmentType);
			if (m_Target != null && m_ParticleGroup != null && m_Actor.isLoaded)
			{
				Matrix4x4 matrix4x = m_Target.worldToLocalMatrix * m_Actor.solver.transform.localToWorldMatrix;
				m_SolverIndices = new int[m_ParticleGroup.Count];
				m_PositionOffsets = new Vector3[m_ParticleGroup.Count];
				m_OrientationOffsets = new Quaternion[m_ParticleGroup.Count];
				for (int i = 0; i < m_ParticleGroup.Count; i++)
				{
					int num = m_ParticleGroup.particleIndices[i];
					if (num >= 0 && num < m_Actor.solverIndices.count)
					{
						m_SolverIndices[i] = m_Actor.solverIndices[num];
						m_PositionOffsets[i] = matrix4x.MultiplyPoint3x4(m_Actor.solver.positions[m_SolverIndices[i]]);
						continue;
					}
					Debug.LogError("The particle group '" + m_ParticleGroup.name + "' references a particle that does not exist in the actor '" + m_Actor.name + "'.");
					m_SolverIndices = null;
					m_PositionOffsets = null;
					m_OrientationOffsets = null;
					return;
				}
				if (m_Actor.usesOrientedParticles)
				{
					Quaternion rotation = matrix4x.rotation;
					for (int j = 0; j < m_ParticleGroup.Count; j++)
					{
						int num2 = m_ParticleGroup.particleIndices[j];
						if (num2 >= 0 && num2 < m_Actor.solverIndices.count)
						{
							m_OrientationOffsets[j] = rotation * m_Actor.solver.orientations[m_SolverIndices[j]];
						}
					}
				}
			}
			else
			{
				m_PositionOffsets = null;
				m_OrientationOffsets = null;
			}
			EnableAttachment(m_AttachmentType);
		}

		private void EnableAttachment(AttachmentType type)
		{
			if (!base.enabled || !m_Actor.isLoaded || !isBound)
			{
				return;
			}
			ObiSolver solver = m_Actor.solver;
			switch (type)
			{
			case AttachmentType.Dynamic:
			{
				ObiPinConstraintsData obiPinConstraintsData = m_Actor.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiPinConstraintsData;
				attachedCollider = m_Target.GetComponent<ObiColliderBase>();
				if (obiPinConstraintsData != null && attachedCollider != null && pinBatch == null)
				{
					pinBatch = new ObiPinConstraintsBatch(obiPinConstraintsData);
					for (int k = 0; k < m_SolverIndices.Length; k++)
					{
						pinBatch.AddConstraint(m_SolverIndices[k], attachedCollider, m_PositionOffsets[k], m_OrientationOffsets[k], m_Compliance, (!constrainOrientation) ? 10000 : 0, projection);
						pinBatch.activeConstraintCount++;
					}
					obiPinConstraintsData.AddBatch(pinBatch);
					attachedColliderHandleIndex = -1;
					if (attachedCollider.Handle != null)
					{
						attachedColliderHandleIndex = attachedCollider.Handle.index;
					}
					m_Actor.SetConstraintsDirty(Oni.ConstraintType.Pin);
				}
				break;
			}
			case AttachmentType.Static:
			{
				for (int i = 0; i < m_SolverIndices.Length; i++)
				{
					if (m_SolverIndices[i] >= 0 && m_SolverIndices[i] < solver.invMasses.count)
					{
						solver.invMasses[m_SolverIndices[i]] = 0f;
					}
				}
				if (m_Actor.usesOrientedParticles && m_ConstrainOrientation)
				{
					for (int j = 0; j < m_SolverIndices.Length; j++)
					{
						if (m_SolverIndices[j] >= 0 && m_SolverIndices[j] < solver.invRotationalMasses.count)
						{
							solver.invRotationalMasses[m_SolverIndices[j]] = 0f;
						}
					}
				}
				m_Actor.UpdateParticleProperties();
				break;
			}
			}
		}

		private void DisableAttachment(AttachmentType type)
		{
			if (!isBound)
			{
				return;
			}
			switch (type)
			{
			case AttachmentType.Dynamic:
				if (pinBatch == null)
				{
					break;
				}
				if (m_Actor.GetConstraintsByType(Oni.ConstraintType.Pin) is ObiConstraints<ObiPinConstraintsBatch> obiConstraints)
				{
					obiConstraints.RemoveBatch(pinBatch);
					if (actor.isLoaded)
					{
						m_Actor.SetConstraintsDirty(Oni.ConstraintType.Pin);
					}
				}
				attachedCollider = null;
				pinBatch = null;
				attachedColliderHandleIndex = -1;
				break;
			case AttachmentType.Static:
			{
				ObiSolver solver = m_Actor.solver;
				ObiActorBlueprint sourceBlueprint = m_Actor.sourceBlueprint;
				for (int i = 0; i < m_SolverIndices.Length; i++)
				{
					int num = m_SolverIndices[i];
					if (num >= 0 && num < solver.invMasses.count)
					{
						solver.invMasses[num] = sourceBlueprint.invMasses[i];
					}
				}
				if (m_Actor.usesOrientedParticles)
				{
					for (int j = 0; j < m_SolverIndices.Length; j++)
					{
						int num2 = m_SolverIndices[j];
						if (num2 >= 0 && num2 < solver.invRotationalMasses.count)
						{
							solver.invRotationalMasses[num2] = sourceBlueprint.invRotationalMasses[j];
						}
					}
				}
				m_Actor.UpdateParticleProperties();
				break;
			}
			}
		}

		private void UpdateAttachment()
		{
			if (base.enabled && m_Actor.isLoaded && isBound)
			{
				ObiSolver solver = m_Actor.solver;
				switch (m_AttachmentType)
				{
				case AttachmentType.Dynamic:
					if (attachedCollider != null && attachedCollider.Handle != null && attachedCollider.Handle.index != attachedColliderHandleIndex)
					{
						attachedColliderHandleIndex = attachedCollider.Handle.index;
						m_Actor.SetConstraintsDirty(Oni.ConstraintType.Pin);
					}
					break;
				case AttachmentType.Static:
				{
					ObiActorBlueprint sourceBlueprint = m_Actor.sourceBlueprint;
					bool activeInHierarchy = m_Target.gameObject.activeInHierarchy;
					Matrix4x4 matrix4x = solver.transform.worldToLocalMatrix * m_Target.localToWorldMatrix;
					for (int i = 0; i < m_SolverIndices.Length; i++)
					{
						int num = m_SolverIndices[i];
						if (num >= 0 && num < solver.invMasses.count)
						{
							if (activeInHierarchy)
							{
								solver.invMasses[num] = 0f;
								solver.velocities[num] = Vector3.zero;
								ObiNativeVector4List startPositions = solver.startPositions;
								ObiNativeVector4List endPositions = solver.endPositions;
								Vector4 vector = (solver.positions[num] = matrix4x.MultiplyPoint3x4(m_PositionOffsets[i]));
								Vector4 value = (endPositions[num] = vector);
								startPositions[num] = value;
							}
							else
							{
								solver.invMasses[num] = sourceBlueprint.invMasses[i];
							}
						}
					}
					if (!m_Actor.usesOrientedParticles || !m_ConstrainOrientation)
					{
						break;
					}
					Quaternion rotation = matrix4x.rotation;
					for (int j = 0; j < m_SolverIndices.Length; j++)
					{
						int num2 = m_SolverIndices[j];
						if (num2 >= 0 && num2 < solver.invRotationalMasses.count)
						{
							if (activeInHierarchy)
							{
								solver.invRotationalMasses[num2] = 0f;
								solver.angularVelocities[num2] = Vector3.zero;
								ObiNativeQuaternionList startOrientations = solver.startOrientations;
								ObiNativeQuaternionList endOrientations = solver.endOrientations;
								Quaternion quaternion = (solver.orientations[num2] = rotation * m_OrientationOffsets[j]);
								Quaternion value2 = (endOrientations[num2] = quaternion);
								startOrientations[num2] = value2;
							}
							else
							{
								solver.invRotationalMasses[num2] = sourceBlueprint.invRotationalMasses[j];
							}
						}
					}
					break;
				}
				}
			}
			else if (!isBound && attachedColliderHandleIndex >= 0)
			{
				attachedColliderHandleIndex = -1;
				m_Actor.SetConstraintsDirty(Oni.ConstraintType.Pin);
			}
		}

		private void BreakDynamicAttachment(float substepTime)
		{
			if (!base.enabled || m_AttachmentType != AttachmentType.Dynamic || !m_Actor.isLoaded || !isBound)
			{
				return;
			}
			ObiSolver solver = m_Actor.solver;
			ObiConstraints<ObiPinConstraintsBatch> obiConstraints = m_Actor.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
			ObiConstraints<ObiPinConstraintsBatch> obiConstraints2 = solver.GetConstraintsByType(Oni.ConstraintType.Pin) as ObiConstraints<ObiPinConstraintsBatch>;
			bool flag = false;
			if (obiConstraints != null && pinBatch != null && obiConstraints.batchCount <= obiConstraints2.batchCount)
			{
				int num = obiConstraints.batches.IndexOf(pinBatch);
				if (num >= 0 && num < actor.solverBatchOffsets[9].Count)
				{
					int num2 = actor.solverBatchOffsets[9][num];
					ObiPinConstraintsBatch obiPinConstraintsBatch = obiConstraints2.batches[num];
					float num3 = substepTime * substepTime;
					for (int i = 0; i < pinBatch.activeConstraintCount; i++)
					{
						if (pinBatch.pinBodies[i] != attachedCollider.Handle)
						{
							pinBatch.pinBodies[i] = attachedCollider.Handle;
							flag = true;
						}
						if ((0f - obiPinConstraintsBatch.lambdas[(num2 + i) * 4 + 3]) / num3 > breakThreshold)
						{
							pinBatch.DeactivateConstraint(i);
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				m_Actor.SetConstraintsDirty(Oni.ConstraintType.Pin);
			}
		}
	}
}
