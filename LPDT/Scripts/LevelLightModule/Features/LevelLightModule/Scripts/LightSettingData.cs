using System;
using UnityEngine;

namespace Features.LevelLightModule.Scripts
{
	[Serializable]
	public class LightSettingData
	{
		public Color Color;

		public float Temperature;

		public float Intensity;

		public float Range;

		public LightShadows ShadowType;

		public float ShadowStrength;

		public float ShadowNearPlane;
	}
}
