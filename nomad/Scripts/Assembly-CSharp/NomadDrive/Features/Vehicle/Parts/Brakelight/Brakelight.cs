using NomadDrive.Features.Attachables;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Brakelight
{
	public class Brakelight : AttachableObject
	{
		[SerializeField]
		private Renderer emissionRenderer;

		[SerializeField]
		private int emissionMaterialIndex;

		[SerializeField]
		[Min(0f)]
		private float idleIntensity = 0.5f;

		[SerializeField]
		[Min(0f)]
		private float brakeIntensity = 2f;

		[SerializeField]
		[Min(0f)]
		private float fadeDuration = 0.3f;

		[SerializeField]
		[Range(0f, 1f)]
		private float idleEmissionIntensity = 0.3f;

		[SerializeField]
		[Range(0f, 1f)]
		private float brakeEmissionIntensity = 1f;

		[SerializeField]
		private AnimationCurve transitionEase;

		public BrakelightSlot BrakelightSlot { get; set; }

		public Light Light { get; set; }

		public Renderer EmissionRenderer => emissionRenderer;

		public int EmissionMaterialIndex => emissionMaterialIndex;

		public float IdleIntensity => idleIntensity;

		public float BrakeIntensity => brakeIntensity;

		public float FadeDuration => fadeDuration;

		public float IdleEmissionIntensity => idleEmissionIntensity;

		public float BrakeEmissionIntensity => brakeEmissionIntensity;

		public AnimationCurve TransitionEase => transitionEase;

		protected override void Awake()
		{
			base.Awake();
			Light = GetComponentInChildren<Light>();
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
