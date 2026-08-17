using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroWeatherTypeFogOverride
	{
		public float fogDensity = 0.02f;

		public float fogHeightFalloff = 0.2f;

		public float fogHeight;

		public float fogDensity2 = 0.02f;

		public float fogHeightFalloff2 = 0.2f;

		public float fogHeight2;

		public float fogColorBlend = 0.5f;

		public Color fogColorMod = Color.white;

		public float scattering = 0.015f;

		public float extinction = 0.01f;

		public float anistropy = 0.6f;

		public float fogAttenuationDistance = 400f;

		public float maxHeight = 250f;

		public float baseHeight;

		public float ambientDimmer = 1f;

		public float directLightMultiplier = 1f;

		public float directLightShadowdimmer = 1f;

		public float unityFogDensity = 0.002f;

		public float unityFogStartDistance;

		public float unityFogEndDistance = 1000f;
	}
}
