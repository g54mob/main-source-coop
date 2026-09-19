using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.LevelGatesModule.Data;

namespace Features.BeachPresetModule.Scripts.Services
{
	public class BeachGateCorridorLightService : IBeachGateCorridorLightService
	{
		private readonly GateCorridorLightModel _gateCorridorLightModel;

		public BeachGateCorridorLightService(GateCorridorLightModel gateCorridorLightModel)
		{
			_gateCorridorLightModel = gateCorridorLightModel;
		}

		public void Apply(IReadOnlyList<BeachGateCorridorLightSetting> settings)
		{
			foreach (IGateCorridorLightPresetTarget registeredLight in _gateCorridorLightModel.RegisteredLights)
			{
				if (registeredLight != null)
				{
					BeachGateCorridorLightSetting beachGateCorridorLightSetting = FindSetting(settings, registeredLight);
					if (beachGateCorridorLightSetting != null)
					{
						registeredLight.ApplyPreset(beachGateCorridorLightSetting.LightType, beachGateCorridorLightSetting.OriginalIntensity, beachGateCorridorLightSetting.Color, beachGateCorridorLightSetting.Range, beachGateCorridorLightSetting.SpotAngle, beachGateCorridorLightSetting.InnerSpotAngle, beachGateCorridorLightSetting.DistanceForMaxIntensity, beachGateCorridorLightSetting.AttenuatedIntensity, beachGateCorridorLightSetting.ExitBias);
					}
				}
			}
		}

		private static BeachGateCorridorLightSetting FindSetting(IReadOnlyList<BeachGateCorridorLightSetting> settings, IGateCorridorLightPresetTarget corridorLight)
		{
			foreach (BeachGateCorridorLightSetting setting in settings)
			{
				if (setting != null && corridorLight.GateIndex == setting.GateIndex)
				{
					return setting;
				}
			}
			return null;
		}
	}
}
