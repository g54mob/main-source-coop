using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroQualityModule : EnviroModule
	{
		public EnviroQualities Settings = new EnviroQualities();

		public EnviroQualityModule preset;

		public bool showQualityControls;

		public override void Enable()
		{
			base.Enable();
			if (Settings.defaultQuality == null)
			{
				if (Settings.Qualities.Count > 0)
				{
					Settings.defaultQuality = Settings.Qualities[0];
					return;
				}
				CreateNewQuality();
				Settings.defaultQuality = Settings.Qualities[0];
			}
		}

		public override void UpdateModule()
		{
			if (!(EnviroManager.instance == null) && Settings.defaultQuality != null)
			{
				if (EnviroManager.instance.Sky != null)
				{
					EnviroManager.instance.Sky.Settings.skyMode = Settings.defaultQuality.skyOverride.skyMode;
				}
				if (EnviroManager.instance.VolumetricClouds != null)
				{
					EnviroManager.instance.VolumetricClouds.settingsQuality.volumetricClouds = Settings.defaultQuality.volumetricCloudsOverride.volumetricClouds;
					EnviroManager.instance.VolumetricClouds.settingsQuality.lightningSupport = Settings.defaultQuality.volumetricCloudsOverride.lightningSupport;
					EnviroManager.instance.VolumetricClouds.settingsQuality.variableBottomNoise = Settings.defaultQuality.volumetricCloudsOverride.variableBottomNoise;
					EnviroManager.instance.VolumetricClouds.settingsQuality.downsampling = Settings.defaultQuality.volumetricCloudsOverride.downsampling;
					EnviroManager.instance.VolumetricClouds.settingsQuality.stepsLayer1 = Settings.defaultQuality.volumetricCloudsOverride.stepsLayer1;
					EnviroManager.instance.VolumetricClouds.settingsQuality.blueNoiseIntensity = Settings.defaultQuality.volumetricCloudsOverride.blueNoiseIntensity;
					EnviroManager.instance.VolumetricClouds.settingsQuality.reprojectionBlendTime = Settings.defaultQuality.volumetricCloudsOverride.reprojectionBlendTime;
					EnviroManager.instance.VolumetricClouds.settingsQuality.lodDistance = Settings.defaultQuality.volumetricCloudsOverride.lodDistance;
				}
				if (EnviroManager.instance.Fog != null)
				{
					EnviroManager.instance.Fog.Settings.fog = Settings.defaultQuality.fogOverride.fog;
					EnviroManager.instance.Fog.Settings.fogQualityMode = Settings.defaultQuality.fogOverride.fogQualityMode;
					EnviroManager.instance.Fog.Settings.volumetrics = Settings.defaultQuality.fogOverride.volumetrics;
					EnviroManager.instance.Fog.Settings.unityFog = Settings.defaultQuality.fogOverride.unityFog;
					EnviroManager.instance.Fog.Settings.quality = Settings.defaultQuality.fogOverride.quality;
					EnviroManager.instance.Fog.Settings.steps = Settings.defaultQuality.fogOverride.steps;
				}
				if (EnviroManager.instance.FlatClouds != null)
				{
					EnviroManager.instance.FlatClouds.settings.useFlatClouds = Settings.defaultQuality.flatCloudsOverride.flatClouds;
					EnviroManager.instance.FlatClouds.settings.useCirrusClouds = Settings.defaultQuality.flatCloudsOverride.cirrusClouds;
					EnviroManager.instance.FlatClouds.settings.flatCloudsShadowSteps = Settings.defaultQuality.flatCloudsOverride.flatCloudsShadowSteps;
				}
				if (EnviroManager.instance.Aurora != null)
				{
					EnviroManager.instance.Aurora.Settings.useAurora = Settings.defaultQuality.auroraOverride.aurora;
					EnviroManager.instance.Aurora.Settings.auroraSteps = Settings.defaultQuality.auroraOverride.steps;
				}
				if (EnviroManager.instance.Effects != null)
				{
					EnviroManager.instance.Effects.Settings.particeEmissionRateModifier = Settings.defaultQuality.effectsOverride.particeEmissionRateModifier;
				}
			}
		}

		public void CleanupQualityList()
		{
			for (int i = 0; i < Settings.Qualities.Count; i++)
			{
				if (Settings.Qualities[i] == null)
				{
					Settings.Qualities.RemoveAt(i);
				}
			}
		}

		public void CreateNewQuality()
		{
			EnviroQuality item = EnviroQualityCreation.CreateMyAsset();
			Settings.Qualities.Add(item);
		}

		public void RemoveQuality(EnviroQuality quality)
		{
			Settings.Qualities.Remove(quality);
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroQualities>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroQualityModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroQualities>(JsonUtility.ToJson(Settings));
		}
	}
}
