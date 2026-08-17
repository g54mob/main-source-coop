using System;

namespace Enviro
{
	[Serializable]
	public class EnviroVolumetricCloudsQualitySettings
	{
		public bool volumetricClouds = true;

		public bool lightningSupport = true;

		public bool variableBottomNoise;

		public int downsampling = 4;

		public int stepsLayer1 = 128;

		public float blueNoiseIntensity = 1f;

		public float reprojectionBlendTime = 10f;

		public float lodDistance = 0.25f;
	}
}
