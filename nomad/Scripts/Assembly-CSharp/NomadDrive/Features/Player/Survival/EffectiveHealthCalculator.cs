using UnityEngine;

namespace NomadDrive.Features.Player.Survival
{
	public static class EffectiveHealthCalculator
	{
		public static ZoneSizes Calculate(float nutritionRatio, float hydrationRatio, float energyRatio, float healthRatio, float poisonRatio, PlayerStatusBarConfig config)
		{
			float zoneAppearThreshold = config.zoneAppearThreshold;
			ZoneSizes result = new ZoneSizes
			{
				Nutrition = ApplyThreshold((1f - nutritionRatio) * config.nutritionMaxEncroachment, zoneAppearThreshold),
				Hydration = ApplyThreshold((1f - hydrationRatio) * config.hydrationMaxEncroachment, zoneAppearThreshold),
				Energy = ApplyThreshold((1f - energyRatio) * config.energyMaxEncroachment, zoneAppearThreshold),
				Damage = ApplyThreshold((1f - healthRatio) * config.damageMaxEncroachment, config.damageZoneAppearThreshold),
				Poison = ApplyThreshold(poisonRatio * config.poisonMaxEncroachment, config.poisonZoneAppearThreshold)
			};
			float num = result.Nutrition + result.Hydration + result.Energy + result.Damage + result.Poison;
			result.EffectiveHealth = Mathf.Clamp01(1f - num);
			return result;
		}

		private static float ApplyThreshold(float zoneSize, float threshold)
		{
			if (!(zoneSize >= threshold))
			{
				return 0f;
			}
			return zoneSize;
		}
	}
}
