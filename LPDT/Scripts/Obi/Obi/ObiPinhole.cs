using System;
using UnityEngine;

namespace Obi
{
	[AddComponentMenu("Physics/Obi/Obi Pinhole", 820)]
	[RequireComponent(typeof(ObiRopeBase))]
	[ExecuteInEditMode]
	public class ObiPinhole : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private ObiRopeBase m_Rope;

		[SerializeField]
		[HideInInspector]
		private Transform m_Target;

		[Range(0f, 1f)]
		[SerializeField]
		[HideInInspector]
		private float m_Position;

		[SerializeField]
		[HideInInspector]
		private bool m_LimitRange;

		[MinMax(0f, 1f)]
		[SerializeField]
		[HideInInspector]
		private Vector2 m_Range = new Vector2(0f, 1f);

		[Range(0f, 1f)]
		[SerializeField]
		[HideInInspector]
		private float m_Friction;

		[SerializeField]
		[HideInInspector]
		private float m_MotorSpeed;

		[SerializeField]
		[HideInInspector]
		private float m_MotorForce;

		[SerializeField]
		[HideInInspector]
		private float m_Compliance;

		[SerializeField]
		[HideInInspector]
		private bool m_ClampAtEnds = true;

		[SerializeField]
		[HideInInspector]
		private ObiPinholeConstraintsBatch.PinholeEdge currentEdge;

		[SerializeField]
		[HideInInspector]
		public ObiPinholeConstraintsBatch.PinholeEdge firstEdge;

		[SerializeField]
		[HideInInspector]
		public ObiPinholeConstraintsBatch.PinholeEdge lastEdge;

		[NonSerialized]
		private ObiPinholeConstraintsBatch pinBatch;

		[NonSerialized]
		private ObiColliderBase attachedCollider;

		[NonSerialized]
		private int attachedColliderHandleIndex;

		[NonSerialized]
		private Vector3 m_PositionOffset;

		[NonSerialized]
		private bool m_ParametersDirty = true;

		[NonSerialized]
		private bool m_PositionDirty;

		[NonSerialized]
		private bool m_RangeDirty;

		[Delayed]
		public float breakThreshold = float.PositiveInfinity;

		public ObiActor rope => m_Rope;

		public float edgeCoordinate => currentEdge.coordinate;

		public int edgeIndex => currentEdge.edgeIndex;

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

		public float position
		{
			get
			{
				return m_Position;
			}
			set
			{
				if (!Mathf.Approximately(value, m_Position))
				{
					m_Position = value;
					CalculateMu();
				}
			}
		}

		public bool limitRange
		{
			get
			{
				return m_LimitRange;
			}
			set
			{
				if (m_LimitRange != value)
				{
					m_LimitRange = value;
					CalculateRange();
				}
			}
		}

		public Vector2 range
		{
			get
			{
				return m_Range;
			}
			set
			{
				m_Range = value;
				CalculateRange();
			}
		}

		public bool isBound
		{
			get
			{
				if (m_Target != null)
				{
					return currentEdge.edgeIndex >= 0;
				}
				return false;
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
				if (!Mathf.Approximately(value, m_Compliance))
				{
					m_Compliance = value;
					m_ParametersDirty = true;
				}
			}
		}

		public float friction
		{
			get
			{
				return m_Friction;
			}
			set
			{
				if (!Mathf.Approximately(value, m_Friction))
				{
					m_Friction = value;
					m_ParametersDirty = true;
				}
			}
		}

		public float motorSpeed
		{
			get
			{
				return m_MotorSpeed;
			}
			set
			{
				if (!Mathf.Approximately(value, m_MotorSpeed))
				{
					m_MotorSpeed = value;
					m_ParametersDirty = true;
				}
			}
		}

		public float motorForce
		{
			get
			{
				return m_MotorForce;
			}
			set
			{
				if (!Mathf.Approximately(value, m_MotorForce))
				{
					m_MotorForce = value;
					m_ParametersDirty = true;
				}
			}
		}

		public bool clampAtEnds
		{
			get
			{
				return m_ClampAtEnds;
			}
			set
			{
				if (m_ClampAtEnds != value)
				{
					m_ClampAtEnds = value;
					m_ParametersDirty = true;
				}
			}
		}

		public float relativeVelocity { get; private set; }

		private void OnEnable()
		{
			m_Rope = GetComponent<ObiRopeBase>();
			m_Rope.OnBlueprintLoaded += Actor_OnBlueprintLoaded;
			m_Rope.OnSimulationStart += Actor_OnSimulationStart;
			m_Rope.OnRequestReadback += Actor_OnRequestReadback;
			if (m_Rope.solver != null)
			{
				Actor_OnBlueprintLoaded(m_Rope, m_Rope.sourceBlueprint);
			}
			EnablePinhole();
		}

		private void OnDisable()
		{
			DisablePinhole();
			m_Rope.OnBlueprintLoaded -= Actor_OnBlueprintLoaded;
			m_Rope.OnSimulationStart -= Actor_OnSimulationStart;
			m_Rope.OnRequestReadback -= Actor_OnRequestReadback;
		}

		private void OnValidate()
		{
			m_Rope = GetComponent<ObiRopeBase>();
			m_ParametersDirty = true;
			m_PositionDirty = true;
			m_RangeDirty = true;
		}

		private void Actor_OnBlueprintLoaded(ObiActor act, ObiActorBlueprint blueprint)
		{
			Bind();
		}

		private void Actor_OnSimulationStart(ObiActor act, float stepTime, float substepTime)
		{
			UpdatePinhole();
			BreakPinhole(substepTime);
		}

		private void Actor_OnRequestReadback(ObiActor actor)
		{
			if (!base.enabled || !m_Rope.isLoaded || !isBound)
			{
				return;
			}
			ObiSolver solver = m_Rope.solver;
			ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints = m_Rope.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiConstraints<ObiPinholeConstraintsBatch>;
			ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints2 = solver.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiConstraints<ObiPinholeConstraintsBatch>;
			if (obiConstraints != null && pinBatch != null && obiConstraints.batchCount <= obiConstraints2.batchCount)
			{
				int num = obiConstraints.batches.IndexOf(pinBatch);
				if (num >= 0 && num < rope.solverBatchOffsets[3].Count)
				{
					ObiPinholeConstraintsBatch obiPinholeConstraintsBatch = obiConstraints2.batches[num];
					obiPinholeConstraintsBatch.particleIndices.Readback();
					obiPinholeConstraintsBatch.edgeMus.Readback();
					obiPinholeConstraintsBatch.relativeVelocities.Readback();
				}
			}
		}

		private void ClampMuToRange()
		{
			if (m_LimitRange)
			{
				float ropeCoordinate = lastEdge.GetRopeCoordinate(m_Rope);
				float ropeCoordinate2 = firstEdge.GetRopeCoordinate(m_Rope);
				if (m_Position > ropeCoordinate)
				{
					m_Position = ropeCoordinate;
					currentEdge.edgeIndex = m_Rope.GetEdgeAt(m_Position, out currentEdge.coordinate);
					m_PositionDirty = true;
				}
				else if (m_Position < ropeCoordinate2)
				{
					m_Position = ropeCoordinate2;
					currentEdge.edgeIndex = m_Rope.GetEdgeAt(m_Position, out currentEdge.coordinate);
					m_PositionDirty = true;
				}
			}
		}

		public void CalculateMu()
		{
			currentEdge.edgeIndex = m_Rope.GetEdgeAt(m_Position, out currentEdge.coordinate);
			ClampMuToRange();
			m_PositionDirty = true;
		}

		public void CalculateRange()
		{
			if (m_LimitRange)
			{
				firstEdge.edgeIndex = m_Rope.GetEdgeAt(m_Range.x, out firstEdge.coordinate);
				lastEdge.edgeIndex = m_Rope.GetEdgeAt(m_Range.y, out lastEdge.coordinate);
			}
			else
			{
				firstEdge.edgeIndex = m_Rope.GetEdgeAt(0f, out firstEdge.coordinate);
				lastEdge.edgeIndex = m_Rope.GetEdgeAt(1f, out lastEdge.coordinate);
				firstEdge.coordinate = float.MinValue;
				lastEdge.coordinate = float.MaxValue;
			}
			ClampMuToRange();
			m_RangeDirty = true;
		}

		public void Bind()
		{
			DisablePinhole();
			if (m_Target != null && m_Rope.isLoaded)
			{
				Matrix4x4 matrix4x = m_Target.worldToLocalMatrix * m_Rope.solver.transform.localToWorldMatrix;
				ObiRopeBlueprintBase obiRopeBlueprintBase = m_Rope.sharedBlueprint as ObiRopeBlueprintBase;
				if (obiRopeBlueprintBase != null && obiRopeBlueprintBase.deformableEdges != null)
				{
					currentEdge.edgeIndex = m_Rope.GetEdgeAt(m_Position, out currentEdge.coordinate);
					if (currentEdge.edgeIndex >= 0)
					{
						CalculateRange();
						m_RangeDirty = false;
						m_PositionDirty = false;
						int index = obiRopeBlueprintBase.deformableEdges[currentEdge.edgeIndex * 2];
						int index2 = obiRopeBlueprintBase.deformableEdges[currentEdge.edgeIndex * 2 + 1];
						Vector4 vector = Vector4.Lerp(m_Rope.solver.positions[m_Rope.solverIndices[index]], m_Rope.solver.positions[m_Rope.solverIndices[index2]], currentEdge.coordinate);
						m_PositionOffset = matrix4x.MultiplyPoint3x4(vector);
					}
				}
			}
			else
			{
				currentEdge.edgeIndex = -1;
			}
			EnablePinhole();
		}

		private void EnablePinhole()
		{
			if (!base.enabled || !m_Rope.isLoaded || !isBound)
			{
				return;
			}
			ObiPinholeConstraintsData obiPinholeConstraintsData = m_Rope.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiPinholeConstraintsData;
			attachedCollider = m_Target.GetComponent<ObiColliderBase>();
			if (obiPinholeConstraintsData != null && attachedCollider != null && pinBatch == null)
			{
				pinBatch = new ObiPinholeConstraintsBatch(obiPinholeConstraintsData);
				pinBatch.AddConstraint(currentEdge, firstEdge, lastEdge, m_Rope, attachedCollider, m_PositionOffset, m_Compliance, m_Friction, m_MotorSpeed, m_MotorForce, m_ClampAtEnds);
				pinBatch.activeConstraintCount++;
				obiPinholeConstraintsData.AddBatch(pinBatch);
				attachedColliderHandleIndex = -1;
				if (attachedCollider.Handle != null)
				{
					attachedColliderHandleIndex = attachedCollider.Handle.index;
				}
				m_Rope.SetConstraintsDirty(Oni.ConstraintType.Pinhole);
			}
		}

		private void DisablePinhole()
		{
			if (!isBound || pinBatch == null)
			{
				return;
			}
			if (m_Rope.GetConstraintsByType(Oni.ConstraintType.Pinhole) is ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints)
			{
				obiConstraints.RemoveBatch(pinBatch);
				if (rope.isLoaded)
				{
					m_Rope.SetConstraintsDirty(Oni.ConstraintType.Pinhole);
				}
			}
			attachedCollider = null;
			pinBatch = null;
			attachedColliderHandleIndex = -1;
		}

		private void UpdatePinhole()
		{
			if (base.enabled && m_Rope.isLoaded && isBound)
			{
				UpdateEdgeCoordinate();
				UpdateParameters();
				if (attachedCollider != null && attachedCollider.Handle != null && attachedCollider.Handle.index != attachedColliderHandleIndex)
				{
					attachedColliderHandleIndex = attachedCollider.Handle.index;
					m_Rope.SetConstraintsDirty(Oni.ConstraintType.Pinhole);
				}
			}
			else if (!isBound && attachedColliderHandleIndex >= 0)
			{
				attachedColliderHandleIndex = -1;
				m_Rope.SetConstraintsDirty(Oni.ConstraintType.Pinhole);
			}
		}

		private void UpdateParameters()
		{
			if (!base.enabled || !m_Rope.isLoaded || !isBound || !m_ParametersDirty)
			{
				return;
			}
			ObiSolver solver = m_Rope.solver;
			ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints = m_Rope.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiConstraints<ObiPinholeConstraintsBatch>;
			ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints2 = solver.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiConstraints<ObiPinholeConstraintsBatch>;
			if (obiConstraints == null || pinBatch == null || obiConstraints.batchCount > obiConstraints2.batchCount)
			{
				return;
			}
			int num = obiConstraints.batches.IndexOf(pinBatch);
			if (num >= 0 && num < rope.solverBatchOffsets[3].Count)
			{
				int num2 = rope.solverBatchOffsets[3][num];
				ObiPinholeConstraintsBatch obiPinholeConstraintsBatch = obiConstraints2.batches[num];
				for (int i = 0; i < pinBatch.activeConstraintCount; i++)
				{
					obiPinholeConstraintsBatch.parameters[(num2 + i) * 5] = m_Compliance;
					obiPinholeConstraintsBatch.parameters[(num2 + i) * 5 + 1] = m_Friction;
					obiPinholeConstraintsBatch.parameters[(num2 + i) * 5 + 2] = m_MotorSpeed;
					obiPinholeConstraintsBatch.parameters[(num2 + i) * 5 + 3] = m_MotorForce;
					obiPinholeConstraintsBatch.parameters[(num2 + i) * 5 + 4] = (m_ClampAtEnds ? 1 : 0);
				}
				obiPinholeConstraintsBatch.parameters.Upload();
				m_ParametersDirty = false;
			}
		}

		private void UpdateEdgeCoordinate()
		{
			if (!base.enabled || !m_Rope.isLoaded || !isBound)
			{
				return;
			}
			ObiSolver solver = m_Rope.solver;
			ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints = m_Rope.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiConstraints<ObiPinholeConstraintsBatch>;
			ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints2 = solver.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiConstraints<ObiPinholeConstraintsBatch>;
			if (obiConstraints == null || pinBatch == null || obiConstraints.batchCount > obiConstraints2.batchCount)
			{
				return;
			}
			int num = obiConstraints.batches.IndexOf(pinBatch);
			if (num < 0 || num >= rope.solverBatchOffsets[3].Count)
			{
				return;
			}
			int num2 = rope.solverBatchOffsets[3][num];
			ObiPinholeConstraintsBatch obiPinholeConstraintsBatch = obiConstraints2.batches[num];
			obiPinholeConstraintsBatch.particleIndices.WaitForReadback();
			obiPinholeConstraintsBatch.edgeMus.WaitForReadback();
			obiPinholeConstraintsBatch.relativeVelocities.WaitForReadback();
			if (m_RangeDirty)
			{
				for (int i = 0; i < pinBatch.activeConstraintCount; i++)
				{
					obiPinholeConstraintsBatch.edgeRanges[(num2 + i) * 2] = m_Rope.deformableEdgesOffset + firstEdge.edgeIndex;
					obiPinholeConstraintsBatch.edgeRanges[(num2 + i) * 2 + 1] = m_Rope.deformableEdgesOffset + lastEdge.edgeIndex;
					obiPinholeConstraintsBatch.edgeRangeMus[(num2 + i) * 2] = firstEdge.coordinate;
					obiPinholeConstraintsBatch.edgeRangeMus[(num2 + i) * 2 + 1] = lastEdge.coordinate;
				}
				obiPinholeConstraintsBatch.edgeRanges.Upload();
				obiPinholeConstraintsBatch.edgeRangeMus.Upload();
				m_RangeDirty = false;
			}
			if (m_PositionDirty)
			{
				for (int j = 0; j < pinBatch.activeConstraintCount; j++)
				{
					obiPinholeConstraintsBatch.particleIndices[num2 + j] = m_Rope.deformableEdgesOffset + currentEdge.edgeIndex;
					obiPinholeConstraintsBatch.edgeMus[num2 + j] = currentEdge.coordinate;
				}
				obiPinholeConstraintsBatch.particleIndices.Upload();
				obiPinholeConstraintsBatch.edgeMus.Upload();
				m_PositionDirty = false;
			}
			else
			{
				for (int k = 0; k < pinBatch.activeConstraintCount; k++)
				{
					currentEdge.coordinate = obiPinholeConstraintsBatch.edgeMus[num2 + k];
					currentEdge.edgeIndex = obiPinholeConstraintsBatch.particleIndices[num2 + k] - m_Rope.deformableEdgesOffset;
					m_Position = currentEdge.GetRopeCoordinate(m_Rope);
				}
			}
			for (int l = 0; l < pinBatch.activeConstraintCount; l++)
			{
				relativeVelocity = obiPinholeConstraintsBatch.relativeVelocities[num2 + l];
			}
		}

		private void BreakPinhole(float substepTime)
		{
			if (!base.enabled || !m_Rope.isLoaded || !isBound)
			{
				return;
			}
			ObiSolver solver = m_Rope.solver;
			ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints = m_Rope.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiConstraints<ObiPinholeConstraintsBatch>;
			ObiConstraints<ObiPinholeConstraintsBatch> obiConstraints2 = solver.GetConstraintsByType(Oni.ConstraintType.Pinhole) as ObiConstraints<ObiPinholeConstraintsBatch>;
			bool flag = false;
			if (obiConstraints != null && pinBatch != null && obiConstraints.batchCount <= obiConstraints2.batchCount)
			{
				int num = obiConstraints.batches.IndexOf(pinBatch);
				if (num >= 0 && num < rope.solverBatchOffsets[3].Count)
				{
					int num2 = rope.solverBatchOffsets[3][num];
					ObiPinholeConstraintsBatch obiPinholeConstraintsBatch = obiConstraints2.batches[num];
					float num3 = substepTime * substepTime;
					for (int i = 0; i < pinBatch.activeConstraintCount; i++)
					{
						if (pinBatch.pinBodies[i] != attachedCollider.Handle)
						{
							pinBatch.pinBodies[i] = attachedCollider.Handle;
							flag = true;
						}
						if ((0f - obiPinholeConstraintsBatch.lambdas[num2 + i]) / num3 > breakThreshold)
						{
							pinBatch.DeactivateConstraint(i);
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				m_Rope.SetConstraintsDirty(Oni.ConstraintType.Pinhole);
			}
		}
	}
}
