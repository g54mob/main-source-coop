using UnityEngine;
using UnityEngine.Serialization;

namespace Obi
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Collider))]
	public class ObiCollider : ObiColliderBase
	{
		[SerializeProperty("sourceCollider")]
		[FormerlySerializedAs("SourceCollider")]
		[SerializeField]
		private Collider m_SourceCollider;

		[SerializeProperty("distanceField")]
		[FormerlySerializedAs("distanceField")]
		[SerializeField]
		private ObiDistanceField m_DistanceField;

		public Collider sourceCollider
		{
			get
			{
				return m_SourceCollider;
			}
			set
			{
				if (value != null && value.gameObject != base.gameObject)
				{
					Debug.LogError("The Collider component must reside in the same GameObject as ObiCollider.");
					return;
				}
				RemoveCollider();
				m_SourceCollider = value;
				AddCollider();
			}
		}

		public ObiDistanceField distanceField
		{
			get
			{
				return m_DistanceField;
			}
			set
			{
				if (m_DistanceField != value)
				{
					m_DistanceField = value;
					CreateTracker();
				}
			}
		}

		protected override void CreateTracker()
		{
			if (tracker != null)
			{
				tracker.Destroy();
				tracker = null;
			}
			if (distanceField != null)
			{
				tracker = new ObiDistanceFieldShapeTracker(this, m_SourceCollider, distanceField);
			}
			else if (m_SourceCollider is SphereCollider)
			{
				tracker = new ObiSphereShapeTracker(this, (SphereCollider)m_SourceCollider);
			}
			else if (m_SourceCollider is BoxCollider)
			{
				tracker = new ObiBoxShapeTracker(this, (BoxCollider)m_SourceCollider);
			}
			else if (m_SourceCollider is CapsuleCollider)
			{
				tracker = new ObiCapsuleShapeTracker(this, (CapsuleCollider)m_SourceCollider);
			}
			else if (m_SourceCollider is CharacterController)
			{
				tracker = new ObiCharacterControllerShapeTracker(this, (CharacterController)m_SourceCollider);
			}
			else if (m_SourceCollider is TerrainCollider)
			{
				tracker = new ObiTerrainShapeTracker(this, (TerrainCollider)m_SourceCollider);
			}
			else if (m_SourceCollider is MeshCollider)
			{
				tracker = new ObiMeshShapeTracker(this, (MeshCollider)m_SourceCollider);
			}
			else
			{
				Debug.LogWarning("Collider type not supported by Obi.");
			}
		}

		protected override Component GetUnityCollider(ref bool enabled)
		{
			if (m_SourceCollider != null)
			{
				enabled = m_SourceCollider.enabled;
			}
			return m_SourceCollider;
		}

		protected override void FindSourceCollider()
		{
			if (sourceCollider == null)
			{
				sourceCollider = GetComponent<Collider>();
			}
			else
			{
				AddCollider();
			}
		}
	}
}
