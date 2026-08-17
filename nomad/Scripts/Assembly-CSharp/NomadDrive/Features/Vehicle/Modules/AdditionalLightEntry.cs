using System;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	[Serializable]
	public class AdditionalLightEntry
	{
		public string name = "Light";

		public Light light;

		public Renderer emissionRenderer;

		[Min(0f)]
		public int emissionMaterialIndex;

		[Min(0f)]
		public float intensity = 1f;

		[Range(0f, 1f)]
		public float emissionIntensity = 1f;

		[Min(0f)]
		public float fadeDuration;

		public AdditionalLightBinding binding;
	}
}
