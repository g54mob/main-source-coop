using System.Collections.Generic;
using NomadDrive.Features.Attachables;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Parts.Headlights
{
	public class Headlight : AttachableObject
	{
		[SerializeField]
		private Light mainLight;

		[SerializeField]
		private List<Light> additionalLights = new List<Light>();

		[SerializeField]
		private Renderer emissionRenderer;

		[SerializeField]
		private int emissionMaterialIndex;

		[SerializeField]
		private float lowBeamRange = 10f;

		[SerializeField]
		private float highBeamRange = 25f;

		[SerializeField]
		[Min(0f)]
		private float lowBeamIntensity = 1f;

		[SerializeField]
		[Min(0f)]
		private float highBeamIntensity = 2f;

		[SerializeField]
		[Range(0f, 1f)]
		private float lowBeamEmissionIntensity = 0.8f;

		[SerializeField]
		[Range(0f, 1f)]
		private float highBeamEmissionIntensity = 1f;

		[SerializeField]
		[Min(0f)]
		private float onFadeDuration = 0.4f;

		[SerializeField]
		[Min(0f)]
		private float offFadeDuration = 0.6f;

		[SerializeField]
		private AnimationCurve transitionEase;

		public HeadlightSlot HeadlightSlot { get; set; }

		public Light Light { get; set; }

		public IReadOnlyList<Light> AdditionalLights => additionalLights;

		public Renderer EmissionRenderer => emissionRenderer;

		public int EmissionMaterialIndex => emissionMaterialIndex;

		public float LowBeamRange => lowBeamRange;

		public float HighBeamRange => highBeamRange;

		public float LowBeamIntensity => lowBeamIntensity;

		public float HighBeamIntensity => highBeamIntensity;

		public float LowBeamEmissionIntensity => lowBeamEmissionIntensity;

		public float HighBeamEmissionIntensity => highBeamEmissionIntensity;

		public float OnFadeDuration => onFadeDuration;

		public float OffFadeDuration => offFadeDuration;

		public AnimationCurve TransitionEase => transitionEase;

		protected override void Awake()
		{
			base.Awake();
			Light = ((mainLight != null) ? mainLight : GetComponentInChildren<Light>());
			if (Light != null)
			{
				Light.enabled = false;
			}
			SetAdditionalLights(on: false);
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
			SetAdditionalLights(on: false);
		}

		private void SetAdditionalLights(bool on)
		{
			for (int i = 0; i < additionalLights.Count; i++)
			{
				if (additionalLights[i] != null)
				{
					additionalLights[i].enabled = on;
				}
			}
		}

		public override bool Weaved()
		{
			return true;
		}
	}
}
