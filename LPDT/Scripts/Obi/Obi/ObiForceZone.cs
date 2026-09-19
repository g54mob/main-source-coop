using UnityEngine;

namespace Obi
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(ObiCollider))]
	public class ObiForceZone : MonoBehaviour
	{
		[SerializeProperty("sourceCollider")]
		[SerializeField]
		private ObiCollider m_SourceCollider;

		protected ObiForceZoneHandle forcezoneHandle;

		public ForceZone.ZoneType type;

		public ForceZone.ForceMode mode;

		public float intensity;

		[Header("Damping")]
		public ForceZone.DampingDirection dampingDir;

		public float damping;

		[Header("Falloff")]
		public float minDistance;

		public float maxDistance;

		[Min(0f)]
		public float falloffPower = 1f;

		[Header("Tint")]
		public Color color = Color.clear;

		[Header("Pulse")]
		public float pulseIntensity;

		public float pulseFrequency;

		public float pulseSeed;

		protected float intensityVariation;

		public ObiCollider SourceCollider
		{
			get
			{
				return m_SourceCollider;
			}
			set
			{
				if (value != null && value.gameObject != base.gameObject)
				{
					Debug.LogError("The ObiCollider component must reside in the same GameObject as ObiForceZone.");
					return;
				}
				RemoveCollider();
				m_SourceCollider = value;
				AddCollider();
			}
		}

		public ObiForceZoneHandle Handle
		{
			get
			{
				if (forcezoneHandle == null)
				{
					ObiColliderWorld instance = ObiColliderWorld.GetInstance();
					forcezoneHandle = instance.CreateForceZone();
					forcezoneHandle.owner = this;
				}
				return forcezoneHandle;
			}
		}

		public void OnEnable()
		{
			forcezoneHandle = ObiColliderWorld.GetInstance().CreateForceZone();
			forcezoneHandle.owner = this;
			FindSourceCollider();
		}

		public void OnDisable()
		{
			RemoveCollider();
			ObiColliderWorld.GetInstance().DestroyForceZone(forcezoneHandle);
		}

		private void FindSourceCollider()
		{
			if (SourceCollider == null)
			{
				SourceCollider = GetComponent<ObiCollider>();
			}
			else
			{
				AddCollider();
			}
		}

		private void AddCollider()
		{
			if (m_SourceCollider != null)
			{
				m_SourceCollider.ForceZone = this;
			}
		}

		private void RemoveCollider()
		{
			if (m_SourceCollider != null)
			{
				m_SourceCollider.ForceZone = null;
			}
		}

		public virtual void UpdateIfNeeded()
		{
			if (Handle.isValid)
			{
				ForceZone value = ObiColliderWorld.GetInstance().forceZones[Handle.index];
				value.type = type;
				value.mode = mode;
				value.intensity = intensity + intensityVariation;
				value.minDistance = minDistance;
				value.maxDistance = maxDistance;
				value.falloffPower = falloffPower;
				value.damping = damping;
				value.dampingDir = dampingDir;
				value.color = color;
				ObiColliderWorld.GetInstance().forceZones[Handle.index] = value;
			}
		}

		public void Update()
		{
			if (Application.isPlaying)
			{
				intensityVariation = Mathf.PerlinNoise(Time.time * pulseFrequency, pulseSeed) * pulseIntensity;
			}
		}
	}
}
