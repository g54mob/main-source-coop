using UnityEngine;

namespace Features.LevelGatesModule.Data
{
	public interface IGateCorridorLightPresetTarget
	{
		int GateIndex { get; }

		Transform Transform { get; }

		void ApplyPreset(LightType type, float originalIntensity, Color color, float range, float spotAngle, float innerSpotAngle, float distanceForMaxIntensity, float attenuatedIntensity, float exitBias);
	}
}
