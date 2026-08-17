using NomadDrive.Features.Attachables;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.SignalLight
{
	public class VehicleSignalLight : AttachableObject
	{
		[SerializeField]
		private SignalLightSide signalLightSide;

		[SerializeField]
		private Renderer emissionRenderer;

		[SerializeField]
		private int emissionMaterialIndex;

		[SerializeField]
		[Min(0f)]
		private float onIntensity = 2f;

		[SerializeField]
		[Range(0f, 1f)]
		private float onEmissionIntensity = 1f;

		[SerializeField]
		[Min(0f)]
		private float blinkAttackDuration = 0.02f;

		[SerializeField]
		[Min(0f)]
		private float blinkReleaseDuration = 0.08f;

		public SignalLightSide SignalLightSide => signalLightSide;

		public VehicleSignalLightSlot SignalLightSlot { get; set; }

		public Light Light { get; private set; }

		public Renderer EmissionRenderer => emissionRenderer;

		public int EmissionMaterialIndex => emissionMaterialIndex;

		public float OnIntensity => onIntensity;

		public float OnEmissionIntensity => onEmissionIntensity;

		public float BlinkAttackDuration => blinkAttackDuration;

		public float BlinkReleaseDuration => blinkReleaseDuration;

		protected override void Awake()
		{
			base.Awake();
			Light = GetComponentInChildren<Light>(includeInactive: true);
			if (Light != null)
			{
				Light.enabled = false;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			OnDetached.AddListener(DisableLight);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			OnDetached.RemoveListener(DisableLight);
		}

		private void DisableLight()
		{
			if (Light != null)
			{
				Light.enabled = false;
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
