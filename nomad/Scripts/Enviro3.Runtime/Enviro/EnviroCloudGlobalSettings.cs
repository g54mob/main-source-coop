using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroCloudGlobalSettings
	{
		public Vector3 floatingPointOriginMod;

		public Vector3 cloudScrollOffset;

		public Gradient sunLightColorGradient;

		public Gradient moonLightColorGradient;

		public Gradient ambientColorGradient;

		public Color sunLightColor;

		public Color moonLightColor;

		public Color ambientColor;

		public bool depthBlending;

		public bool depthTest = true;

		public Texture3D noise;

		public Texture3D detailNoise;

		public Texture2D curlTex;

		public Texture2D bottomsOffsetNoise;

		public Texture2D blueNoise;

		public Texture customWeatherMap;

		public float cloudsWorldScale = 5000000f;

		public float maxRenderDistance = 75000f;

		public float atmosphereColorSaturateDistance = 15000f;

		[Range(0f, 2f)]
		public float ambientLighIntensity = 1f;

		public bool cloudShadows = true;

		[Range(0f, 2f)]
		public float cloudShadowsIntensity = 1f;

		[Range(0f, 1f)]
		public float cloudsTravelSpeed = 0.5f;
	}
}
