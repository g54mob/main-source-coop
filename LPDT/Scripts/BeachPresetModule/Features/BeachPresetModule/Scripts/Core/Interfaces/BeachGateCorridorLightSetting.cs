using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Core.Interfaces
{
	public class BeachGateCorridorLightSetting
	{
		public int GateIndex { get; set; }

		public float OriginalIntensity { get; set; }

		public Color Color { get; set; }

		public LightType LightType { get; set; }

		public float Range { get; set; }

		public float SpotAngle { get; set; }

		public float InnerSpotAngle { get; set; }

		public float DistanceForMaxIntensity { get; set; }

		public float AttenuatedIntensity { get; set; }

		public float ExitBias { get; set; }
	}
}
