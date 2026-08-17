using System;
using System.Collections;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	[ExecuteInEditMode]
	public class EnviroWeatherModule : EnviroModule
	{
		public EnviroWeather Settings;

		public EnviroWeatherModule preset;

		public EnviroWeatherType targetWeatherType;

		public float weatherBlendProgress;

		public bool globalAutoWeatherChange = true;

		public BoxCollider triggerCollider;

		public Rigidbody triggerRB;

		public bool showWeatherPresetsControls;

		public bool showTransitionControls;

		public bool showZoneControls;

		private bool instantTransition;

		public override void Enable()
		{
			if (!(EnviroManager.instance == null))
			{
				if (targetWeatherType == null && Settings.weatherTypes.Count > 0)
				{
					targetWeatherType = Settings.weatherTypes[0];
				}
				if (EnviroManager.instance.defaultZone != null)
				{
					EnviroManager.instance.currentZone = EnviroManager.instance.defaultZone;
				}
				weatherBlendProgress = 1f;
				Setup();
			}
		}

		public override void Disable()
		{
			if (!(EnviroManager.instance == null))
			{
				Cleanup();
			}
		}

		private void Setup()
		{
			if (EnviroManager.instance.gameObject.GetComponent<BoxCollider>() == null)
			{
				triggerCollider = EnviroManager.instance.gameObject.AddComponent<BoxCollider>();
			}
			else
			{
				triggerCollider = EnviroManager.instance.gameObject.GetComponent<BoxCollider>();
			}
			triggerCollider.isTrigger = true;
			triggerCollider.size = new Vector3(0.1f, 0.1f, 0.1f);
			if (EnviroManager.instance.gameObject.GetComponent<Rigidbody>() == null)
			{
				triggerRB = EnviroManager.instance.gameObject.AddComponent<Rigidbody>();
			}
			else
			{
				triggerRB = EnviroManager.instance.gameObject.GetComponent<Rigidbody>();
			}
			triggerRB.isKinematic = true;
		}

		private void Cleanup()
		{
			if (triggerCollider != null)
			{
				UnityEngine.Object.DestroyImmediate(triggerCollider);
			}
			if (triggerRB != null)
			{
				UnityEngine.Object.DestroyImmediate(triggerRB);
			}
		}

		public void CreateNewWeatherType()
		{
			EnviroWeatherType item = EnviroWeatherTypeCreation.CreateMyAsset();
			Settings.weatherTypes.Add(item);
		}

		public void RemoveWeatherType(EnviroWeatherType type)
		{
			Settings.weatherTypes.Remove(type);
		}

		public void CleanupList()
		{
			for (int i = 0; i < Settings.weatherTypes.Count; i++)
			{
				if (Settings.weatherTypes[i] == null)
				{
					Settings.weatherTypes.RemoveAt(i);
				}
			}
		}

		private IEnumerator InstantTransition()
		{
			yield return null;
			instantTransition = false;
		}

		public override void UpdateModule()
		{
			if (!active || EnviroManager.instance == null)
			{
				return;
			}
			if (!Application.isPlaying || instantTransition)
			{
				if (targetWeatherType != null)
				{
					BlendVolumetricCloudsOverride(1f, 1f);
					BlendFlatCloudsOverride(1f);
					BlendLightingOverride(1f);
					BlendSkyOverride(1f);
					BlendEffectsOverride(1f);
					BlendAuroraOverride(1f);
					BlendFogOverride(1f);
					BlendAudioOverride(1f);
					BlendEnvironmentOverride(1f);
					BlendLightningOverride(1f);
				}
				if (instantTransition)
				{
					instantTransition = false;
				}
			}
			else if (targetWeatherType != null)
			{
				BlendVolumetricCloudsOverride(Settings.cloudsTransitionSpeed * Time.deltaTime, 1f);
				BlendFlatCloudsOverride(Settings.cloudsTransitionSpeed * Time.deltaTime);
				BlendLightingOverride(Settings.lightingTransitionSpeed * Time.deltaTime);
				BlendSkyOverride(Settings.skyTransitionSpeed * Time.deltaTime);
				BlendEffectsOverride(Settings.effectsTransitionSpeed * Time.deltaTime);
				BlendAuroraOverride(Settings.auroraTransitionSpeed * Time.deltaTime);
				BlendFogOverride(Settings.fogTransitionSpeed * Time.deltaTime);
				BlendAudioOverride(Settings.audioTransitionSpeed * Time.deltaTime);
				BlendEnvironmentOverride(Settings.environmentTransitionSpeed * Time.deltaTime);
				BlendLightningOverride(1f);
				UpdateWeatherBlendProgress(Settings.cloudsTransitionSpeed * Time.deltaTime);
			}
		}

		private void BlendLightingOverride(float blendTime)
		{
			EnviroLightingModule lighting = EnviroManager.instance.Lighting;
			if (lighting != null)
			{
				lighting.Settings.directLightIntensityModifier = Mathf.Lerp(lighting.Settings.directLightIntensityModifier, targetWeatherType.lightingOverride.directLightIntensityModifier, blendTime);
				lighting.Settings.ambientIntensityModifier = Mathf.Lerp(lighting.Settings.ambientIntensityModifier, targetWeatherType.lightingOverride.ambientIntensityModifier, blendTime);
				lighting.Settings.shadowIntensity = Mathf.Lerp(lighting.Settings.shadowIntensity, targetWeatherType.lightingOverride.shadowIntensity, blendTime);
			}
		}

		private void BlendSkyOverride(float blendTime)
		{
			EnviroSkyModule sky = EnviroManager.instance.Sky;
			if (sky != null)
			{
				sky.Settings.mieScatteringMultiplier = Mathf.Lerp(sky.Settings.mieScatteringMultiplier, targetWeatherType.skyOverride.mieScatteringMultiplier, blendTime);
				sky.Settings.skyColorExponent = Mathf.Lerp(sky.Settings.skyColorExponent, targetWeatherType.skyOverride.skyColorExponent, blendTime);
				sky.Settings.skyColorTint = Color.Lerp(sky.Settings.skyColorTint, targetWeatherType.skyOverride.skyColorTint, blendTime);
			}
		}

		private void BlendFogOverride(float blendTime)
		{
			EnviroFogModule fog = EnviroManager.instance.Fog;
			if (fog != null)
			{
				fog.Settings.fogDensity = Mathf.Lerp(fog.Settings.fogDensity, targetWeatherType.fogOverride.fogDensity, blendTime);
				fog.Settings.fogHeightFalloff = Mathf.Lerp(fog.Settings.fogHeightFalloff, targetWeatherType.fogOverride.fogHeightFalloff, blendTime);
				fog.Settings.fogHeight = Mathf.Lerp(fog.Settings.fogHeight, targetWeatherType.fogOverride.fogHeight, blendTime);
				fog.Settings.fogDensity2 = Mathf.Lerp(fog.Settings.fogDensity2, targetWeatherType.fogOverride.fogDensity2, blendTime);
				fog.Settings.fogHeightFalloff2 = Mathf.Lerp(fog.Settings.fogHeightFalloff2, targetWeatherType.fogOverride.fogHeightFalloff2, blendTime);
				fog.Settings.fogHeight2 = Mathf.Lerp(fog.Settings.fogHeight2, targetWeatherType.fogOverride.fogHeight2, blendTime);
				fog.Settings.fogColorBlend = Mathf.Lerp(fog.Settings.fogColorBlend, targetWeatherType.fogOverride.fogColorBlend, blendTime);
				fog.Settings.fogColorMod = Color.Lerp(fog.Settings.fogColorMod, targetWeatherType.fogOverride.fogColorMod, blendTime);
				fog.Settings.scattering = Mathf.Lerp(fog.Settings.scattering, targetWeatherType.fogOverride.scattering, blendTime);
				fog.Settings.extinction = Mathf.Lerp(fog.Settings.extinction, targetWeatherType.fogOverride.extinction, blendTime);
				fog.Settings.anistropy = Mathf.Lerp(fog.Settings.anistropy, targetWeatherType.fogOverride.anistropy, blendTime);
				fog.Settings.fogAttenuationDistance = Mathf.Lerp(fog.Settings.fogAttenuationDistance, targetWeatherType.fogOverride.fogAttenuationDistance, blendTime);
				fog.Settings.baseHeight = Mathf.Lerp(fog.Settings.baseHeight, targetWeatherType.fogOverride.baseHeight, blendTime);
				fog.Settings.maxHeight = Mathf.Lerp(fog.Settings.maxHeight, targetWeatherType.fogOverride.maxHeight, blendTime);
				fog.Settings.ambientDimmer = Mathf.Lerp(fog.Settings.ambientDimmer, targetWeatherType.fogOverride.ambientDimmer, blendTime);
				fog.Settings.directLightMultiplier = Mathf.Lerp(fog.Settings.directLightMultiplier, targetWeatherType.fogOverride.directLightMultiplier, blendTime);
				fog.Settings.directLightShadowdimmer = Mathf.Lerp(fog.Settings.ambientDimmer, targetWeatherType.fogOverride.directLightShadowdimmer, blendTime);
				fog.Settings.unityFogDensity = Mathf.Lerp(fog.Settings.unityFogDensity, targetWeatherType.fogOverride.unityFogDensity, blendTime);
				fog.Settings.unityFogStartDistance = Mathf.Lerp(fog.Settings.unityFogStartDistance, targetWeatherType.fogOverride.unityFogStartDistance, blendTime);
				fog.Settings.unityFogEndDistance = Mathf.Lerp(fog.Settings.unityFogEndDistance, targetWeatherType.fogOverride.unityFogEndDistance, blendTime);
			}
		}

		private void BlendEffectsOverride(float blendTime)
		{
			EnviroEffectsModule effects = EnviroManager.instance.Effects;
			if (!(effects != null))
			{
				return;
			}
			for (int i = 0; i < effects.Settings.effectTypes.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < targetWeatherType.effectsOverride.effectsOverride.Count; j++)
				{
					if (effects.Settings.effectTypes[i].name == targetWeatherType.effectsOverride.effectsOverride[j].name)
					{
						effects.Settings.effectTypes[i].emissionRate = Mathf.Lerp(effects.Settings.effectTypes[i].emissionRate, targetWeatherType.effectsOverride.effectsOverride[j].emission, blendTime);
						flag = true;
					}
				}
				if (!flag)
				{
					effects.Settings.effectTypes[i].emissionRate = Mathf.Lerp(effects.Settings.effectTypes[i].emissionRate, 0f, blendTime);
				}
			}
		}

		private void BlendVolumetricCloudsOverride(float blendTime, float blendTime2)
		{
			EnviroVolumetricCloudsModule volumetricClouds = EnviroManager.instance.VolumetricClouds;
			if (volumetricClouds != null)
			{
				volumetricClouds.settingsGlobal.ambientLighIntensity = Mathf.Lerp(volumetricClouds.settingsGlobal.ambientLighIntensity, targetWeatherType.cloudsOverride.ambientLightIntensity, blendTime);
				volumetricClouds.settingsVolume.baseNoiseUVMultiplier = Mathf.Lerp(volumetricClouds.settingsVolume.baseNoiseUVMultiplier, targetWeatherType.cloudsOverride.baseNoiseUVMultiplier, blendTime2);
				volumetricClouds.settingsVolume.detailNoiseUVMultiplier = Mathf.Lerp(volumetricClouds.settingsVolume.detailNoiseUVMultiplier, targetWeatherType.cloudsOverride.detailNoiseUVMultiplier, blendTime2);
				volumetricClouds.settingsVolume.coverage = Mathf.Lerp(volumetricClouds.settingsVolume.coverage, targetWeatherType.cloudsOverride.coverage, blendTime);
				volumetricClouds.settingsVolume.dilateCoverage = Mathf.Lerp(volumetricClouds.settingsVolume.dilateCoverage, targetWeatherType.cloudsOverride.dilateCoverage, blendTime);
				volumetricClouds.settingsVolume.dilateType = Mathf.Lerp(volumetricClouds.settingsVolume.dilateType, targetWeatherType.cloudsOverride.dilateType, blendTime);
				volumetricClouds.settingsVolume.cloudsTypeModifier = Mathf.Lerp(volumetricClouds.settingsVolume.cloudsTypeModifier, targetWeatherType.cloudsOverride.typeModifier, blendTime);
				volumetricClouds.settingsVolume.cloudTypeShaping = Mathf.Lerp(volumetricClouds.settingsVolume.cloudTypeShaping, targetWeatherType.cloudsOverride.cloudTypeShaping, blendTime);
				volumetricClouds.settingsVolume.silverLiningIntensity = Mathf.Lerp(volumetricClouds.settingsVolume.silverLiningIntensity, targetWeatherType.cloudsOverride.silverLiningIntensity, blendTime);
				volumetricClouds.settingsVolume.edgeHighlightStrength = Mathf.Lerp(volumetricClouds.settingsVolume.edgeHighlightStrength, targetWeatherType.cloudsOverride.edgeHighlightStrength, blendTime);
				volumetricClouds.settingsVolume.bottomShape = Mathf.Lerp(volumetricClouds.settingsVolume.bottomShape, targetWeatherType.cloudsOverride.bottomShape, blendTime);
				volumetricClouds.settingsVolume.midShape = Mathf.Lerp(volumetricClouds.settingsVolume.midShape, targetWeatherType.cloudsOverride.midShape, blendTime);
				volumetricClouds.settingsVolume.topShape = Mathf.Lerp(volumetricClouds.settingsVolume.topShape, targetWeatherType.cloudsOverride.topShape, blendTime);
				volumetricClouds.settingsVolume.topLayer = Mathf.Lerp(volumetricClouds.settingsVolume.topLayer, targetWeatherType.cloudsOverride.topLayer, blendTime);
				volumetricClouds.settingsVolume.rampShape = Mathf.Lerp(volumetricClouds.settingsVolume.rampShape, targetWeatherType.cloudsOverride.rampShape, blendTime);
				volumetricClouds.settingsVolume.scatteringIntensity = Mathf.Lerp(volumetricClouds.settingsVolume.scatteringIntensity, targetWeatherType.cloudsOverride.scatteringIntensity, blendTime);
				volumetricClouds.settingsVolume.multiScatterStrength = Mathf.Lerp(volumetricClouds.settingsVolume.multiScatterStrength, targetWeatherType.cloudsOverride.multiScatterStrength, blendTime);
				volumetricClouds.settingsVolume.multiScatterFalloff = Mathf.Lerp(volumetricClouds.settingsVolume.multiScatterFalloff, targetWeatherType.cloudsOverride.multiScatterFalloff, blendTime);
				volumetricClouds.settingsVolume.ambientFloor = Mathf.Lerp(volumetricClouds.settingsVolume.ambientFloor, targetWeatherType.cloudsOverride.ambientFloor, blendTime);
				volumetricClouds.settingsVolume.lightningIntensity = Mathf.Lerp(volumetricClouds.settingsVolume.lightningIntensity, targetWeatherType.cloudsOverride.lightningIntensity, blendTime);
				volumetricClouds.settingsVolume.exposure = Mathf.Lerp(volumetricClouds.settingsVolume.exposure, targetWeatherType.cloudsOverride.exposure, blendTime);
				volumetricClouds.settingsVolume.silverLiningSpread = Mathf.Lerp(volumetricClouds.settingsVolume.silverLiningSpread, targetWeatherType.cloudsOverride.silverLiningSpread, blendTime);
				volumetricClouds.settingsVolume.absorbtion = Mathf.Lerp(volumetricClouds.settingsVolume.absorbtion, targetWeatherType.cloudsOverride.ligthAbsorbtion, blendTime);
				volumetricClouds.settingsVolume.density = Mathf.Lerp(volumetricClouds.settingsVolume.density, targetWeatherType.cloudsOverride.density, blendTime);
				volumetricClouds.settingsVolume.densitySmoothness = Mathf.Lerp(volumetricClouds.settingsVolume.densitySmoothness, targetWeatherType.cloudsOverride.densitySmoothness, blendTime);
				volumetricClouds.settingsVolume.baseErosionIntensity = Mathf.Lerp(volumetricClouds.settingsVolume.baseErosionIntensity, targetWeatherType.cloudsOverride.baseErosionIntensity, blendTime);
				volumetricClouds.settingsVolume.detailErosionIntensity = Mathf.Lerp(volumetricClouds.settingsVolume.detailErosionIntensity, targetWeatherType.cloudsOverride.detailErosionIntensity, blendTime);
				volumetricClouds.settingsVolume.curlIntensity = Mathf.Lerp(volumetricClouds.settingsVolume.curlIntensity, targetWeatherType.cloudsOverride.curlIntensity, blendTime);
				volumetricClouds.settingsVolume.baseNoiseMultiplier = Mathf.Lerp(volumetricClouds.settingsVolume.baseNoiseMultiplier, targetWeatherType.cloudsOverride.baseNoiseMultiplier, blendTime);
				volumetricClouds.settingsVolume.detailNoiseMultiplier = Mathf.Lerp(volumetricClouds.settingsVolume.detailNoiseMultiplier, targetWeatherType.cloudsOverride.detailNoiseMultiplier, blendTime);
			}
		}

		private void BlendFlatCloudsOverride(float blendTime)
		{
			EnviroFlatCloudsModule flatClouds = EnviroManager.instance.FlatClouds;
			if (flatClouds != null)
			{
				flatClouds.settings.cirrusCloudsAlpha = Mathf.Lerp(flatClouds.settings.cirrusCloudsAlpha, targetWeatherType.flatCloudsOverride.cirrusCloudsAlpha, blendTime);
				flatClouds.settings.cirrusCloudsCoverage = Mathf.Lerp(flatClouds.settings.cirrusCloudsCoverage, targetWeatherType.flatCloudsOverride.cirrusCloudsCoverage, blendTime);
				flatClouds.settings.cirrusCloudsColorPower = Mathf.Lerp(flatClouds.settings.cirrusCloudsColorPower, targetWeatherType.flatCloudsOverride.cirrusCloudsColorPower, blendTime);
				flatClouds.settings.flatCloudsCoverage = Mathf.Lerp(flatClouds.settings.flatCloudsCoverage, targetWeatherType.flatCloudsOverride.flatCloudsCoverage, blendTime);
				flatClouds.settings.flatCloudsDensity = Mathf.Lerp(flatClouds.settings.flatCloudsDensity, targetWeatherType.flatCloudsOverride.flatCloudsDensity, blendTime);
				flatClouds.settings.flatCloudsLightIntensity = Mathf.Lerp(flatClouds.settings.flatCloudsLightIntensity, targetWeatherType.flatCloudsOverride.flatCloudsLightIntensity, blendTime);
				flatClouds.settings.flatCloudsAmbientIntensity = Mathf.Lerp(flatClouds.settings.flatCloudsAmbientIntensity, targetWeatherType.flatCloudsOverride.flatCloudsAmbientIntensity, blendTime);
				flatClouds.settings.flatCloudsShadowIntensity = Mathf.Lerp(flatClouds.settings.flatCloudsShadowIntensity, targetWeatherType.flatCloudsOverride.flatCloudsShadowIntensity, blendTime);
			}
		}

		private void BlendAuroraOverride(float blendTime)
		{
			EnviroAuroraModule aurora = EnviroManager.instance.Aurora;
			if (aurora != null)
			{
				aurora.Settings.auroraIntensityModifier = Mathf.Lerp(aurora.Settings.auroraIntensityModifier, targetWeatherType.auroraOverride.auroraIntensity, blendTime);
			}
		}

		private void BlendEnvironmentOverride(float blendTime)
		{
			EnviroEnvironmentModule environment = EnviroManager.instance.Environment;
			if (environment != null)
			{
				environment.Settings.temperatureWeatherMod = Mathf.Lerp(environment.Settings.temperatureWeatherMod, targetWeatherType.environmentOverride.temperatureWeatherMod, blendTime);
				environment.Settings.wetnessTarget = Mathf.Lerp(environment.Settings.wetnessTarget, targetWeatherType.environmentOverride.wetnessTarget, blendTime);
				environment.Settings.snowTarget = Mathf.Lerp(environment.Settings.snowTarget, targetWeatherType.environmentOverride.snowTarget, blendTime);
				environment.Settings.windDirectionX = Mathf.Lerp(environment.Settings.windDirectionX, targetWeatherType.environmentOverride.windDirectionX, blendTime);
				environment.Settings.windDirectionY = Mathf.Lerp(environment.Settings.windDirectionY, targetWeatherType.environmentOverride.windDirectionY, blendTime);
				environment.Settings.windSpeed = Mathf.Lerp(environment.Settings.windSpeed, targetWeatherType.environmentOverride.windSpeed, blendTime);
				environment.Settings.windTurbulence = Mathf.Lerp(environment.Settings.windTurbulence, targetWeatherType.environmentOverride.windTurbulence, blendTime);
			}
		}

		private void BlendAudioOverride(float blendTime)
		{
			EnviroAudioModule audio = EnviroManager.instance.Audio;
			if (!(audio != null))
			{
				return;
			}
			for (int i = 0; i < audio.Settings.ambientClips.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < targetWeatherType.audioOverride.ambientOverride.Count; j++)
				{
					if (targetWeatherType.audioOverride.ambientOverride[j].name == audio.Settings.ambientClips[i].name)
					{
						audio.Settings.ambientClips[i].volume = Mathf.Lerp(audio.Settings.ambientClips[i].volume, targetWeatherType.audioOverride.ambientOverride[j].volume, blendTime);
						flag = true;
					}
				}
				if (!flag)
				{
					audio.Settings.ambientClips[i].volume = Mathf.Lerp(audio.Settings.ambientClips[i].volume, 0f, blendTime);
				}
			}
			for (int k = 0; k < audio.Settings.weatherClips.Count; k++)
			{
				bool flag2 = false;
				for (int l = 0; l < targetWeatherType.audioOverride.weatherOverride.Count; l++)
				{
					if (targetWeatherType.audioOverride.weatherOverride[l].name == audio.Settings.weatherClips[k].name)
					{
						audio.Settings.weatherClips[k].volume = Mathf.Lerp(audio.Settings.weatherClips[k].volume, targetWeatherType.audioOverride.weatherOverride[l].volume, blendTime);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					audio.Settings.weatherClips[k].volume = Mathf.Lerp(audio.Settings.weatherClips[k].volume, 0f, blendTime);
				}
			}
		}

		private void BlendLightningOverride(float blendTime)
		{
			EnviroLightningModule lightning = EnviroManager.instance.Lightning;
			if (lightning != null)
			{
				lightning.Settings.lightningStorm = targetWeatherType.lightningOverride.lightningStorm;
				lightning.Settings.randomLightingDelay = Mathf.Lerp(lightning.Settings.randomLightingDelay, targetWeatherType.lightningOverride.randomLightningDelay, blendTime);
			}
		}

		private void UpdateWeatherBlendProgress(float blendTime)
		{
			weatherBlendProgress = Mathf.Lerp(weatherBlendProgress, 1f, blendTime);
			weatherBlendProgress = Mathf.Clamp01(weatherBlendProgress);
			if ((double)weatherBlendProgress >= 0.99)
			{
				weatherBlendProgress = 1f;
			}
		}

		public void ChangeWeather(EnviroWeatherType type)
		{
			if (targetWeatherType != type)
			{
				EnviroManager.instance.NotifyWeatherChanged(type);
				EnviroManager.instance.NotifyZoneWeatherChanged(type, null);
			}
			if (EnviroManager.instance.currentZone != null)
			{
				EnviroManager.instance.currentZone.currentWeatherType = type;
			}
			targetWeatherType = type;
			weatherBlendProgress = 0f;
		}

		public void ChangeWeather(string typeName)
		{
			for (int i = 0; i < Settings.weatherTypes.Count; i++)
			{
				if (Settings.weatherTypes[i].name == typeName)
				{
					if (targetWeatherType != Settings.weatherTypes[i])
					{
						EnviroManager.instance.NotifyWeatherChanged(Settings.weatherTypes[i]);
						EnviroManager.instance.NotifyZoneWeatherChanged(Settings.weatherTypes[i], null);
					}
					if (EnviroManager.instance.currentZone != null)
					{
						EnviroManager.instance.currentZone.currentWeatherType = Settings.weatherTypes[i];
					}
					targetWeatherType = Settings.weatherTypes[i];
					weatherBlendProgress = 0f;
				}
			}
		}

		public void ChangeWeather(int index)
		{
			for (int i = 0; i < Settings.weatherTypes.Count; i++)
			{
				if (i == index)
				{
					if (targetWeatherType != Settings.weatherTypes[i])
					{
						EnviroManager.instance.NotifyWeatherChanged(Settings.weatherTypes[i]);
						EnviroManager.instance.NotifyZoneWeatherChanged(Settings.weatherTypes[i], null);
					}
					if (EnviroManager.instance.currentZone != null)
					{
						EnviroManager.instance.currentZone.currentWeatherType = Settings.weatherTypes[i];
					}
					targetWeatherType = Settings.weatherTypes[i];
					weatherBlendProgress = 0f;
					break;
				}
			}
		}

		public void ChangeZoneWeather(int weather, int zone)
		{
			if (EnviroManager.instance.zones.Count >= zone && Settings.weatherTypes.Count >= weather)
			{
				EnviroManager.instance.zones[zone].currentWeatherType = Settings.weatherTypes[weather];
				EnviroManager.instance.NotifyZoneWeatherChanged(Settings.weatherTypes[weather], EnviroManager.instance.zones[zone]);
			}
		}

		public void ChangeWeatherInstant(EnviroWeatherType type)
		{
			if (targetWeatherType != type)
			{
				EnviroManager.instance.NotifyWeatherChanged(type);
				EnviroManager.instance.NotifyZoneWeatherChanged(type, null);
			}
			if (EnviroManager.instance.currentZone != null)
			{
				EnviroManager.instance.currentZone.currentWeatherType = type;
			}
			targetWeatherType = type;
			weatherBlendProgress = 1f;
			instantTransition = true;
		}

		public void ChangeWeatherInstant(string typeName)
		{
			for (int i = 0; i < Settings.weatherTypes.Count; i++)
			{
				if (Settings.weatherTypes[i].name == typeName)
				{
					if (targetWeatherType != Settings.weatherTypes[i])
					{
						EnviroManager.instance.NotifyWeatherChanged(Settings.weatherTypes[i]);
						EnviroManager.instance.NotifyZoneWeatherChanged(Settings.weatherTypes[i], null);
					}
					if (EnviroManager.instance.currentZone != null)
					{
						EnviroManager.instance.currentZone.currentWeatherType = Settings.weatherTypes[i];
					}
					targetWeatherType = Settings.weatherTypes[i];
					weatherBlendProgress = 1f;
					instantTransition = true;
				}
			}
		}

		public void ChangeWeatherInstant(int index)
		{
			for (int i = 0; i < Settings.weatherTypes.Count; i++)
			{
				if (i == index)
				{
					if (targetWeatherType != Settings.weatherTypes[i])
					{
						EnviroManager.instance.NotifyWeatherChanged(Settings.weatherTypes[i]);
						EnviroManager.instance.NotifyZoneWeatherChanged(Settings.weatherTypes[i], null);
					}
					if (EnviroManager.instance.currentZone != null)
					{
						EnviroManager.instance.currentZone.currentWeatherType = Settings.weatherTypes[i];
					}
					targetWeatherType = Settings.weatherTypes[i];
					weatherBlendProgress = 1f;
					instantTransition = true;
					break;
				}
			}
		}

		public void RegisterZone(EnviroZone zone)
		{
			EnviroManager.instance.zones.Add(zone);
		}

		public void RemoveZone(EnviroZone zone)
		{
			if (EnviroManager.instance.zones.Contains(zone))
			{
				EnviroManager.instance.zones.Remove(zone);
			}
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroWeather>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroWeatherModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroWeather>(JsonUtility.ToJson(Settings));
		}
	}
}
