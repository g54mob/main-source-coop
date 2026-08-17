using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroVolumetricCloudsQuality
	{
		public bool volumetricClouds = true;

		public bool lightningSupport = true;

		public bool variableBottomNoise;

		[Range(1f, 6f)]
		public int downsampling = 4;

		[Range(32f, 256f)]
		public int stepsLayer1 = 128;

		[Range(32f, 256f)]
		public int stepsLayer2 = 64;

		[Range(0f, 2f)]
		public float blueNoiseIntensity = 1f;

		[Range(0f, 10f)]
		public float reprojectionBlendTime = 10f;

		[Range(0f, 1f)]
		public float lodDistance = 0.25f;
	}
}
