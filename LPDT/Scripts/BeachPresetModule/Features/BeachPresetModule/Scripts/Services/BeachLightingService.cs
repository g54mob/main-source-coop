using System;
using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.BeachPresetModule.Scripts.Data;
using Features.LevelLightModule.Scripts;
using Features.LevelModule.Scripts.RoomVariations;
using UnityEngine;

namespace Features.BeachPresetModule.Scripts.Services
{
	public class BeachLightingService : IBeachLightingService
	{
		private readonly ILightObjectsGroupService _lightObjectsGroupService;

		private readonly LightObjectsModel _lightObjectsModel;

		private readonly BeachIndexedLightModel _indexedLightModel;

		private readonly LevelRoomsModel _levelRoomsModel;

		private readonly List<Light> _resolvedLights = new List<Light>();

		public BeachLightingService(ILightObjectsGroupService lightObjectsGroupService, LightObjectsModel lightObjectsModel, BeachIndexedLightModel indexedLightModel, LevelRoomsModel levelRoomsModel)
		{
			_lightObjectsGroupService = lightObjectsGroupService;
			_lightObjectsModel = lightObjectsModel;
			_indexedLightModel = indexedLightModel;
			_levelRoomsModel = levelRoomsModel;
		}

		public BeachLightingSnapshot Capture(BeachLightingSettings settings)
		{
			BeachLightingSnapshot beachLightingSnapshot = new BeachLightingSnapshot
			{
				RenderSettingsSnapshot = new BeachRenderSettingsSnapshot
				{
					AmbientMode = RenderSettings.ambientMode,
					AmbientLight = RenderSettings.ambientLight,
					AmbientSkyColor = RenderSettings.ambientSkyColor,
					AmbientEquatorColor = RenderSettings.ambientEquatorColor,
					AmbientGroundColor = RenderSettings.ambientGroundColor,
					AmbientIntensity = RenderSettings.ambientIntensity,
					Skybox = RenderSettings.skybox,
					ReflectionBounces = RenderSettings.reflectionBounces,
					ReflectionIntensity = RenderSettings.reflectionIntensity
				},
				PreviousSun = RenderSettings.sun
			};
			if (settings.ApplyDirectionalLight)
			{
				TrySnapshot(RenderSettings.sun, beachLightingSnapshot);
			}
			foreach (BeachLightGroupOverride lightGroupOverride in settings.LightGroupOverrides)
			{
				if (!_lightObjectsModel.LightObjectsGroup.TryGetValue(lightGroupOverride.Group, out var value))
				{
					continue;
				}
				foreach (Light item in value)
				{
					TrySnapshot(item, beachLightingSnapshot);
				}
			}
			foreach (BeachIndexedLightOverride item2 in settings.IndexedLightOverrides ?? Array.Empty<BeachIndexedLightOverride>())
			{
				if (item2 == null)
				{
					continue;
				}
				ResolveOverrideLights(item2);
				foreach (Light resolvedLight in _resolvedLights)
				{
					TrySnapshot(resolvedLight, beachLightingSnapshot);
				}
			}
			return beachLightingSnapshot;
		}

		public void Apply(BeachLightingSettings settings)
		{
			if (settings.ApplySkybox)
			{
				RenderSettings.skybox = settings.Skybox;
			}
			if (settings.ApplyAmbient)
			{
				RenderSettings.ambientMode = settings.AmbientMode;
				RenderSettings.ambientLight = settings.AmbientColor;
				RenderSettings.ambientSkyColor = settings.AmbientSkyColor;
				RenderSettings.ambientEquatorColor = settings.AmbientEquatorColor;
				RenderSettings.ambientGroundColor = settings.AmbientGroundColor;
				RenderSettings.ambientIntensity = settings.AmbientIntensity;
				RenderSettings.reflectionIntensity = settings.ReflectionIntensity;
				RenderSettings.reflectionBounces = settings.ReflectionBounces;
			}
			if (settings.ApplyDirectionalLight)
			{
				ApplyDirectionalLight(settings.DirectionalLightSettings);
			}
			foreach (BeachLightGroupOverride lightGroupOverride in settings.LightGroupOverrides)
			{
				_lightObjectsGroupService.SetLightObjectGroupState(lightGroupOverride.Group, lightGroupOverride.Settings);
			}
			foreach (BeachIndexedLightOverride item in settings.IndexedLightOverrides ?? Array.Empty<BeachIndexedLightOverride>())
			{
				if (item == null || item.Settings == null)
				{
					continue;
				}
				ResolveOverrideLights(item);
				foreach (Light resolvedLight in _resolvedLights)
				{
					ApplyLightSetting(resolvedLight, item.Settings);
				}
			}
			if (settings.ApplySkybox || settings.ApplyAmbient)
			{
				DynamicGI.UpdateEnvironment();
			}
		}

		public void Restore(BeachLightingSnapshot snapshot)
		{
			BeachRenderSettingsSnapshot renderSettingsSnapshot = snapshot.RenderSettingsSnapshot;
			RenderSettings.ambientMode = renderSettingsSnapshot.AmbientMode;
			RenderSettings.ambientLight = renderSettingsSnapshot.AmbientLight;
			RenderSettings.ambientSkyColor = renderSettingsSnapshot.AmbientSkyColor;
			RenderSettings.ambientEquatorColor = renderSettingsSnapshot.AmbientEquatorColor;
			RenderSettings.ambientGroundColor = renderSettingsSnapshot.AmbientGroundColor;
			RenderSettings.ambientIntensity = renderSettingsSnapshot.AmbientIntensity;
			RenderSettings.skybox = renderSettingsSnapshot.Skybox;
			RenderSettings.reflectionBounces = renderSettingsSnapshot.ReflectionBounces;
			RenderSettings.reflectionIntensity = renderSettingsSnapshot.ReflectionIntensity;
			RenderSettings.sun = snapshot.PreviousSun;
			foreach (KeyValuePair<Light, BeachLightSnapshot> lightSnapshot in snapshot.LightSnapshots)
			{
				if (!(lightSnapshot.Key == null))
				{
					BeachLightSnapshot value = lightSnapshot.Value;
					lightSnapshot.Key.enabled = value.Enabled;
					lightSnapshot.Key.color = value.Color;
					lightSnapshot.Key.intensity = value.Intensity;
					lightSnapshot.Key.colorTemperature = value.Temperature;
					lightSnapshot.Key.range = value.Range;
					lightSnapshot.Key.shadows = value.ShadowType;
					lightSnapshot.Key.shadowStrength = value.ShadowStrength;
					lightSnapshot.Key.shadowNearPlane = value.ShadowNearPlane;
					lightSnapshot.Key.transform.rotation = value.Rotation;
				}
			}
			DynamicGI.UpdateEnvironment();
		}

		private void ResolveOverrideLights(BeachIndexedLightOverride indexedOverride)
		{
			_resolvedLights.Clear();
			if (indexedOverride.RoomOverride == RoomType.None)
			{
				if (_indexedLightModel.TryGetLight(indexedOverride.LightIndex, out var light))
				{
					_resolvedLights.Add(light);
				}
				return;
			}
			foreach (LevelRoomEntity room in _levelRoomsModel.GetRooms(indexedOverride.RoomOverride))
			{
				if (room.TryGetLight(indexedOverride.LightIndex, out var light2))
				{
					_resolvedLights.Add(light2);
				}
			}
		}

		private static void ApplyLightSetting(Light light, LightSettingData data)
		{
			if (!(light == null) && data != null)
			{
				light.intensity = data.Intensity;
				light.colorTemperature = data.Temperature;
				light.range = data.Range;
				light.color = data.Color;
				light.shadows = data.ShadowType;
				light.shadowStrength = data.ShadowStrength;
				light.shadowNearPlane = data.ShadowNearPlane;
			}
		}

		private static void ApplyDirectionalLight(BeachDirectionalLightSettings settings)
		{
			Light sun = RenderSettings.sun;
			if (!(sun == null))
			{
				sun.enabled = settings.Enabled;
				sun.color = settings.Color;
				sun.intensity = settings.Intensity;
				sun.transform.rotation = settings.Rotation;
			}
		}

		private static void TrySnapshot(Light light, BeachLightingSnapshot snapshot)
		{
			if (!(light == null) && !snapshot.LightSnapshots.ContainsKey(light))
			{
				snapshot.LightSnapshots.Add(light, new BeachLightSnapshot
				{
					Enabled = light.enabled,
					Color = light.color,
					Intensity = light.intensity,
					Temperature = light.colorTemperature,
					Range = light.range,
					ShadowType = light.shadows,
					ShadowStrength = light.shadowStrength,
					ShadowNearPlane = light.shadowNearPlane,
					Rotation = light.transform.rotation
				});
			}
		}
	}
}
