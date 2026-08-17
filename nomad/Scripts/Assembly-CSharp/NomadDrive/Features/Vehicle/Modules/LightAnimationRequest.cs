using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

namespace NomadDrive.Features.Vehicle.Modules
{
	public struct LightAnimationRequest
	{
		public Light light;

		public float targetIntensity;

		public float? targetRange;

		public Renderer emissionRenderer;

		public int emissionMaterialIndex;

		public float targetEmissionIntensity;

		public IReadOnlyList<Light> additionalLights;

		public float duration;

		public Ease ease;

		public AnimationCurve customEase;
	}
}
