using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace CorgiGodRays
{
	[Serializable]
	public class GodRaysVolume : VolumeComponent, IPostProcessComponent
	{
		public FloatParameter MainLightIntensity = new FloatParameter(0f);

		public FloatParameter AdditionalLightsIntensity = new FloatParameter(0f);

		public ClampedFloatParameter MainLightScattering = new ClampedFloatParameter(0.5f, -1f, 1f);

		public ClampedFloatParameter AdditionalLightsScattering = new ClampedFloatParameter(0.5f, -1f, 1f);

		public ColorParameter Tint = new ColorParameter(Color.white, hdr: true, showAlpha: true, showEyeDropper: true);

		public bool IsActive()
		{
			return true;
		}

		public bool IsTileCompatible()
		{
			return false;
		}
	}
}
