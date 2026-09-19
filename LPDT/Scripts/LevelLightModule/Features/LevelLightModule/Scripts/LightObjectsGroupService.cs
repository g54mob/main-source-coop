using UnityEngine;

namespace Features.LevelLightModule.Scripts
{
	public class LightObjectsGroupService : ILightObjectsGroupService
	{
		private readonly LightObjectsModel _lightObjectsModel;

		public LightObjectsGroupService(LightObjectsModel lightObjectsModel)
		{
			_lightObjectsModel = lightObjectsModel;
		}

		public void SetLightObjectGroupState(LightObjectGroup group, bool state)
		{
			_lightObjectsModel.SetGroupEnabledState(group, state);
			if (!_lightObjectsModel.LightObjectsGroup.TryGetValue(group, out var value))
			{
				return;
			}
			foreach (Light item in value)
			{
				item.enabled = state;
			}
		}

		public void SetLightObjectGroupState(LightObjectGroup group, LightSettingData lightSettingData)
		{
			if (!_lightObjectsModel.LightObjectsGroup.TryGetValue(group, out var value))
			{
				return;
			}
			foreach (Light item in value)
			{
				item.intensity = lightSettingData.Intensity;
				item.colorTemperature = lightSettingData.Temperature;
				item.range = lightSettingData.Range;
				item.color = lightSettingData.Color;
				item.shadows = lightSettingData.ShadowType;
				item.shadowStrength = lightSettingData.ShadowStrength;
				item.shadowNearPlane = lightSettingData.ShadowNearPlane;
			}
		}
	}
}
