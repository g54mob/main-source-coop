using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Enviro
{
	[Serializable]
	[ExecuteInEditMode]
	public class EnviroLightingModule : EnviroModule
	{
		public EnviroLighting Settings;

		public EnviroLightingModule preset;

		private int currentFrame;

		private float lastAmbientSkyboxUpdate;

		public bool showDirectLightingControls;

		public bool showAmbientLightingControls;

		public bool showReflectionControls;

		public HDAdditionalLightData directionalLightHDRP;

		public HDAdditionalLightData additionalLightHDRP;

		public Exposure exposureHDRP;

		public IndirectLightingController indirectLightingHDRP;

		public override void Enable()
		{
			if (!(EnviroManager.instance == null))
			{
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

		public void ApplyLightingChanges()
		{
			Cleanup();
			Setup();
		}

		private void Setup()
		{
			if (EnviroManager.instance.Objects.directionalLight == null)
			{
				GameObject gameObject = new GameObject();
				if (Settings.lightingMode == EnviroLighting.LightingMode.Single)
				{
					gameObject.name = "Sun and Moon Directional Light";
				}
				else
				{
					gameObject.name = "Sun Directional Light";
				}
				gameObject.transform.SetParent(EnviroManager.instance.transform);
				gameObject.transform.localPosition = Vector3.zero;
				EnviroManager.instance.Objects.directionalLight = gameObject.AddComponent<Light>();
				EnviroManager.instance.Objects.directionalLight.type = LightType.Directional;
				EnviroManager.instance.Objects.directionalLight.shadows = LightShadows.Soft;
			}
			if (EnviroManager.instance.Objects.additionalDirectionalLight == null && Settings.lightingMode == EnviroLighting.LightingMode.Dual)
			{
				GameObject gameObject2 = new GameObject();
				gameObject2.name = "Moon Directional Light";
				gameObject2.transform.SetParent(EnviroManager.instance.transform);
				gameObject2.transform.localPosition = Vector3.zero;
				EnviroManager.instance.Objects.additionalDirectionalLight = gameObject2.AddComponent<Light>();
				EnviroManager.instance.Objects.additionalDirectionalLight.type = LightType.Directional;
				EnviroManager.instance.Objects.additionalDirectionalLight.shadows = LightShadows.Soft;
			}
			else if (EnviroManager.instance.Objects.additionalDirectionalLight != null && Settings.lightingMode == EnviroLighting.LightingMode.Single)
			{
				UnityEngine.Object.DestroyImmediate(EnviroManager.instance.Objects.additionalDirectionalLight.gameObject);
			}
		}

		private void Cleanup()
		{
			if (!(EnviroManager.instance == null))
			{
				if (EnviroManager.instance.Objects.directionalLight != null)
				{
					UnityEngine.Object.DestroyImmediate(EnviroManager.instance.Objects.directionalLight.gameObject);
				}
				if (EnviroManager.instance.Objects.additionalDirectionalLight != null)
				{
					UnityEngine.Object.DestroyImmediate(EnviroManager.instance.Objects.additionalDirectionalLight.gameObject);
				}
			}
		}

		public override void UpdateModule()
		{
			if (active && !(EnviroManager.instance == null))
			{
				currentFrame++;
				if (currentFrame >= Settings.updateIntervallFrames)
				{
					EnviroManager.instance.updateSkyAndLighting = true;
					currentFrame = 0;
				}
				else
				{
					EnviroManager.instance.updateSkyAndLighting = false;
				}
				if (EnviroManager.instance.Objects.directionalLight != null && Settings.setDirectLighting && EnviroManager.instance.updateSkyAndLighting)
				{
					UpdateDirectLightingHDRP();
				}
				if (Settings.setAmbientLighting && EnviroManager.instance.updateSkyAndLighting)
				{
					UpdateAmbientLightingHDRP();
				}
				if (EnviroManager.instance.updateSkyAndLighting)
				{
					UpdateExposureHDRP();
				}
			}
		}

		public void UpdateDirectLighting()
		{
			if (Settings.lightingMode == EnviroLighting.LightingMode.Single)
			{
				if (!EnviroManager.instance.isNight)
				{
					EnviroManager.instance.Objects.directionalLight.transform.rotation = EnviroManager.instance.Objects.sun.transform.rotation;
					EnviroManager.instance.Objects.directionalLight.intensity = Settings.sunIntensityCurve.Evaluate(EnviroManager.instance.solarTime) * Settings.directLightIntensityModifier;
					EnviroManager.instance.Objects.directionalLight.color = Settings.sunColorGradient.Evaluate(EnviroManager.instance.solarTime);
				}
				else
				{
					EnviroManager.instance.Objects.directionalLight.transform.rotation = EnviroManager.instance.Objects.moon.transform.rotation;
					EnviroManager.instance.Objects.directionalLight.intensity = Settings.moonIntensityCurve.Evaluate(EnviroManager.instance.lunarTime) * Settings.directLightIntensityModifier;
					EnviroManager.instance.Objects.directionalLight.color = Settings.moonColorGradient.Evaluate(EnviroManager.instance.lunarTime);
				}
				EnviroManager.instance.Objects.directionalLight.shadowStrength = Settings.shadowIntensity;
			}
			else
			{
				EnviroManager.instance.Objects.directionalLight.transform.rotation = EnviroManager.instance.Objects.sun.transform.rotation;
				EnviroManager.instance.Objects.directionalLight.intensity = Settings.sunIntensityCurve.Evaluate(EnviroManager.instance.solarTime) * Settings.directLightIntensityModifier;
				EnviroManager.instance.Objects.directionalLight.color = Settings.sunColorGradient.Evaluate(EnviroManager.instance.solarTime);
				EnviroManager.instance.Objects.directionalLight.shadowStrength = Settings.shadowIntensity;
				EnviroManager.instance.Objects.additionalDirectionalLight.transform.rotation = EnviroManager.instance.Objects.moon.transform.rotation;
				EnviroManager.instance.Objects.additionalDirectionalLight.intensity = Settings.moonIntensityCurve.Evaluate(EnviroManager.instance.lunarTime) * Settings.directLightIntensityModifier;
				EnviroManager.instance.Objects.additionalDirectionalLight.color = Settings.moonColorGradient.Evaluate(EnviroManager.instance.lunarTime);
				EnviroManager.instance.Objects.additionalDirectionalLight.shadowStrength = Settings.shadowIntensity;
			}
		}

		public void UpdateDirectLightingHDRP()
		{
			if (directionalLightHDRP == null && EnviroManager.instance.Objects.directionalLight != null)
			{
				directionalLightHDRP = EnviroManager.instance.Objects.directionalLight.gameObject.GetComponent<HDAdditionalLightData>();
			}
			if (additionalLightHDRP == null && EnviroManager.instance.Objects.additionalDirectionalLight != null)
			{
				additionalLightHDRP = EnviroManager.instance.Objects.additionalDirectionalLight.gameObject.GetComponent<HDAdditionalLightData>();
			}
			if (Settings.lightingMode == EnviroLighting.LightingMode.Single)
			{
				if (!EnviroManager.instance.isNight)
				{
					EnviroManager.instance.Objects.directionalLight.transform.rotation = EnviroManager.instance.Objects.sun.transform.rotation;
					EnviroManager.instance.Objects.directionalLight.color = Settings.sunColorGradient.Evaluate(EnviroManager.instance.solarTime);
					EnviroManager.instance.Objects.directionalLight.useColorTemperature = true;
					EnviroManager.instance.Objects.directionalLight.colorTemperature = Settings.lightColorTemperatureHDRP.Evaluate(EnviroManager.instance.solarTime);
					EnviroManager.instance.Objects.directionalLight.intensity = Settings.sunIntensityCurve.Evaluate(EnviroManager.instance.solarTime) * Settings.lightIntensityHDRP * Settings.directLightIntensityModifier;
				}
				else
				{
					EnviroManager.instance.Objects.directionalLight.transform.rotation = EnviroManager.instance.Objects.moon.transform.rotation;
					EnviroManager.instance.Objects.directionalLight.color = Settings.moonColorGradient.Evaluate(EnviroManager.instance.lunarTime);
					EnviroManager.instance.Objects.directionalLight.useColorTemperature = true;
					EnviroManager.instance.Objects.directionalLight.colorTemperature = Settings.lightColorTemperatureHDRP.Evaluate(EnviroManager.instance.solarTime);
					EnviroManager.instance.Objects.directionalLight.intensity = Settings.moonIntensityCurve.Evaluate(EnviroManager.instance.lunarTime) * Settings.lightIntensityHDRP * Settings.directLightIntensityModifier;
				}
				if (directionalLightHDRP != null)
				{
					directionalLightHDRP.shadowDimmer = Settings.shadowIntensity;
				}
				return;
			}
			EnviroManager.instance.Objects.directionalLight.transform.rotation = EnviroManager.instance.Objects.sun.transform.rotation;
			EnviroManager.instance.Objects.directionalLight.color = Settings.sunColorGradient.Evaluate(EnviroManager.instance.solarTime);
			EnviroManager.instance.Objects.directionalLight.useColorTemperature = true;
			EnviroManager.instance.Objects.directionalLight.colorTemperature = Settings.lightColorTemperatureHDRP.Evaluate(EnviroManager.instance.solarTime);
			if (directionalLightHDRP != null)
			{
				EnviroManager.instance.Objects.directionalLight.intensity = Settings.sunIntensityCurve.Evaluate(EnviroManager.instance.solarTime) * Settings.lightIntensityHDRP * Settings.directLightIntensityModifier;
				directionalLightHDRP.shadowDimmer = Settings.shadowIntensity;
			}
			if (EnviroManager.instance.Objects.additionalDirectionalLight != null)
			{
				EnviroManager.instance.Objects.additionalDirectionalLight.transform.rotation = EnviroManager.instance.Objects.moon.transform.rotation;
				EnviroManager.instance.Objects.additionalDirectionalLight.color = Settings.moonColorGradient.Evaluate(EnviroManager.instance.lunarTime);
				EnviroManager.instance.Objects.additionalDirectionalLight.useColorTemperature = true;
				EnviroManager.instance.Objects.additionalDirectionalLight.colorTemperature = Settings.lightColorTemperatureHDRP.Evaluate(EnviroManager.instance.solarTime);
			}
			if (additionalLightHDRP != null)
			{
				EnviroManager.instance.Objects.directionalLight.intensity = Settings.moonIntensityCurve.Evaluate(EnviroManager.instance.lunarTime) * Settings.lightIntensityHDRP * Settings.directLightIntensityModifier;
				additionalLightHDRP.shadowDimmer = Settings.shadowIntensity;
			}
		}

		public void UpdateAmbientLightingHDRP()
		{
			if (!(EnviroManager.instance.volumeHDRP != null) || !(EnviroManager.instance.volumeProfileHDRP != null))
			{
				return;
			}
			if (indirectLightingHDRP == null)
			{
				if (EnviroManager.instance.volumeProfileHDRP.TryGet<IndirectLightingController>(out var component))
				{
					indirectLightingHDRP = component;
					return;
				}
				EnviroManager.instance.volumeProfileHDRP.Add<IndirectLightingController>();
				if (EnviroManager.instance.volumeProfileHDRP.TryGet<IndirectLightingController>(out component))
				{
					indirectLightingHDRP = component;
				}
			}
			else if (Settings.controlIndirectLighting)
			{
				indirectLightingHDRP.active = true;
				indirectLightingHDRP.indirectDiffuseLightingMultiplier.overrideState = true;
				indirectLightingHDRP.indirectDiffuseLightingMultiplier.value = Settings.diffuseIndirectIntensity.Evaluate(EnviroManager.instance.solarTime) * Settings.ambientIntensityModifier;
				indirectLightingHDRP.reflectionLightingMultiplier.overrideState = true;
				indirectLightingHDRP.reflectionLightingMultiplier.value = Settings.reflectionIndirectIntensity.Evaluate(EnviroManager.instance.solarTime);
			}
			else
			{
				indirectLightingHDRP.active = false;
			}
		}

		public void UpdateExposureHDRP()
		{
			if (!(EnviroManager.instance.volumeHDRP != null) || !(EnviroManager.instance.volumeProfileHDRP != null))
			{
				return;
			}
			if (exposureHDRP == null)
			{
				if (EnviroManager.instance.volumeProfileHDRP.TryGet<Exposure>(out var component))
				{
					exposureHDRP = component;
					return;
				}
				EnviroManager.instance.volumeProfileHDRP.Add<Exposure>();
				if (EnviroManager.instance.volumeProfileHDRP.TryGet<Exposure>(out component))
				{
					exposureHDRP = component;
				}
			}
			else if (Settings.controlExposure)
			{
				exposureHDRP.active = true;
				exposureHDRP.mode.overrideState = true;
				exposureHDRP.mode.value = ExposureMode.Fixed;
				exposureHDRP.fixedExposure.overrideState = true;
				exposureHDRP.fixedExposure.value = Settings.sceneExposure.Evaluate(EnviroManager.instance.solarTime);
			}
			else
			{
				exposureHDRP.active = false;
			}
		}

		public void UpdateAmbientLighting(bool forced = false)
		{
			RenderSettings.ambientMode = Settings.ambientMode;
			float intensity = (RenderSettings.ambientIntensity = Settings.ambientIntensityCurve.Evaluate(EnviroManager.instance.solarTime) * Settings.ambientIntensityModifier);
			if (forced)
			{
				UpdateAmbient(Settings.ambientMode, intensity);
				if (EnviroManager.instance.Time != null)
				{
					lastAmbientSkyboxUpdate = EnviroManager.instance.Time.Settings.timeOfDay + Settings.ambientUpdateIntervall;
				}
			}
			else if (EnviroManager.instance.Time != null)
			{
				if (lastAmbientSkyboxUpdate < EnviroManager.instance.Time.Settings.timeOfDay || lastAmbientSkyboxUpdate > EnviroManager.instance.Time.Settings.timeOfDay + (Settings.ambientUpdateIntervall + 0.01f))
				{
					UpdateAmbient(Settings.ambientMode, intensity);
					lastAmbientSkyboxUpdate = EnviroManager.instance.Time.Settings.timeOfDay + Settings.ambientUpdateIntervall;
				}
			}
			else if (lastAmbientSkyboxUpdate < Time.time)
			{
				UpdateAmbient(Settings.ambientMode, intensity);
				lastAmbientSkyboxUpdate = Time.time + Settings.ambientUpdateIntervall * 60f;
			}
		}

		private void UpdateAmbient(AmbientMode ambientMode, float intensity)
		{
			switch (ambientMode)
			{
			case AmbientMode.Flat:
				RenderSettings.ambientSkyColor = Settings.ambientSkyColorGradient.Evaluate(EnviroManager.instance.solarTime) * intensity;
				break;
			case AmbientMode.Trilight:
				RenderSettings.ambientSkyColor = Settings.ambientSkyColorGradient.Evaluate(EnviroManager.instance.solarTime) * intensity;
				RenderSettings.ambientEquatorColor = Settings.ambientEquatorColorGradient.Evaluate(EnviroManager.instance.solarTime) * intensity;
				RenderSettings.ambientGroundColor = Settings.ambientGroundColorGradient.Evaluate(EnviroManager.instance.solarTime) * intensity;
				break;
			case AmbientMode.Skybox:
				DynamicGI.UpdateEnvironment();
				break;
			case (AmbientMode)2:
				break;
			}
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroLighting>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroLightingModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroLighting>(JsonUtility.ToJson(Settings));
		}
	}
}
