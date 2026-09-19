using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	internal struct BeachLightSnapshot
	{
		public bool Enabled;

		public Color Color;

		public float Intensity;

		public float Temperature;

		public float Range;

		public LightShadows ShadowType;

		public float ShadowStrength;

		public float ShadowNearPlane;

		public Quaternion Rotation;
	}
}
