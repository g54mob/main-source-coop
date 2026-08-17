using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroFlatCloudsModule : EnviroModule
	{
		public EnviroFlatClouds settings;

		public EnviroFlatCloudsModule preset;

		[HideInInspector]
		public bool showCirrusCloudsControls;

		[HideInInspector]
		public bool show2DCloudsControls;

		[HideInInspector]
		public Vector2 cloudFlatBaseAnim;

		[HideInInspector]
		public Vector2 cloudFlatDetailAnim;

		[HideInInspector]
		public Vector2 cirrusAnim;

		public override void UpdateModule()
		{
			if (!active || EnviroManager.instance == null)
			{
				return;
			}
			UpdateWind();
			if (settings.useCirrusClouds)
			{
				Shader.SetGlobalFloat("_CirrusClouds", 1f);
				if (settings.cirrusCloudsTex != null)
				{
					Shader.SetGlobalTexture("_CirrusCloudMap", settings.cirrusCloudsTex);
				}
				Shader.SetGlobalFloat("_CirrusCloudAlpha", settings.cirrusCloudsAlpha);
				Shader.SetGlobalFloat("_CirrusCloudCoverage", settings.cirrusCloudsCoverage);
				Shader.SetGlobalFloat("_CirrusCloudColorPower", settings.cirrusCloudsColorPower);
				Shader.SetGlobalColor("_CirrusCloudColor", settings.cirrusCloudsColor.Evaluate(EnviroManager.instance.solarTime));
				Shader.SetGlobalVector("_CirrusCloudAnimation", new Vector4(cirrusAnim.x, cirrusAnim.y, 0f, 0f));
			}
			else
			{
				Shader.SetGlobalFloat("_CirrusClouds", 0f);
			}
			if (settings.useFlatClouds)
			{
				Shader.SetGlobalFloat("_FlatClouds", 1f);
				if (settings.flatCloudsBaseTex != null)
				{
					Shader.SetGlobalTexture("_FlatCloudsBaseTexture", settings.flatCloudsBaseTex);
				}
				if (settings.flatCloudsDetailTex != null)
				{
					Shader.SetGlobalTexture("_FlatCloudsDetailTexture", settings.flatCloudsDetailTex);
				}
				Shader.SetGlobalColor("_FlatCloudsLightColor", settings.flatCloudsLightColor.Evaluate(EnviroManager.instance.solarTime));
				Shader.SetGlobalColor("_FlatCloudsAmbientColor", settings.flatCloudsAmbientColor.Evaluate(EnviroManager.instance.solarTime));
				Vector3 forward = Vector3.forward;
				if (EnviroManager.instance.Objects.directionalLight != null)
				{
					forward = EnviroManager.instance.Objects.directionalLight.transform.forward;
				}
				Shader.SetGlobalVector("_FlatCloudsLightDirection", forward);
				Shader.SetGlobalVector("_FlatCloudsLightingParams", new Vector4(settings.flatCloudsLightIntensity * 10f, settings.flatCloudsAmbientIntensity, settings.flatCloudsShadowIntensity, settings.flatCloudsHGPhase));
				Shader.SetGlobalVector("_FlatCloudsParams", new Vector4(settings.flatCloudsCoverage, settings.flatCloudsDensity * 5f, settings.flatCloudsAltitude, settings.flatCloudsShadowSteps));
				Shader.SetGlobalVector("_FlatCloudsTiling", new Vector4(settings.flatCloudsBaseTiling, settings.flatCloudsDetailTiling, 0f, 0f));
				Shader.SetGlobalVector("_FlatCloudsAnimation", new Vector4(cloudFlatBaseAnim.x, cloudFlatBaseAnim.y, cloudFlatDetailAnim.x, cloudFlatDetailAnim.y));
			}
			else
			{
				Shader.SetGlobalFloat("_FlatClouds", 0f);
			}
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				settings = JsonUtility.FromJson<EnviroFlatClouds>(JsonUtility.ToJson(preset.settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		private void UpdateWind()
		{
			if (EnviroManager.instance.Environment != null)
			{
				cloudFlatBaseAnim += new Vector2(EnviroManager.instance.Environment.Settings.windSpeed * EnviroManager.instance.Environment.Settings.windDirectionX * settings.flatCloudsWindIntensity * Time.deltaTime * 0.01f, EnviroManager.instance.Environment.Settings.windSpeed * EnviroManager.instance.Environment.Settings.windDirectionY * settings.flatCloudsWindIntensity * Time.deltaTime * 0.01f);
				cloudFlatDetailAnim += new Vector2(EnviroManager.instance.Environment.Settings.windSpeed * EnviroManager.instance.Environment.Settings.windDirectionX * settings.flatCloudsDetailWindIntensity * Time.deltaTime * 0.1f, EnviroManager.instance.Environment.Settings.windSpeed * EnviroManager.instance.Environment.Settings.windDirectionY * settings.flatCloudsDetailWindIntensity * Time.deltaTime * 0.1f);
				cirrusAnim += new Vector2(EnviroManager.instance.Environment.Settings.windSpeed * EnviroManager.instance.Environment.Settings.windDirectionX * settings.cirrusCloudsWindIntensity * Time.deltaTime * 0.01f, EnviroManager.instance.Environment.Settings.windSpeed * EnviroManager.instance.Environment.Settings.windDirectionY * settings.cirrusCloudsWindIntensity * Time.deltaTime * 0.01f);
			}
			else
			{
				cloudFlatBaseAnim += new Vector2(settings.flatCloudsWindIntensity * Time.deltaTime * 0.01f, settings.flatCloudsWindIntensity * Time.deltaTime * 0.01f);
				cloudFlatDetailAnim += new Vector2(settings.flatCloudsDetailWindIntensity * Time.deltaTime * 0.1f, settings.flatCloudsDetailWindIntensity * Time.deltaTime * 0.1f);
				cirrusAnim += new Vector2(settings.cirrusCloudsWindIntensity * Time.deltaTime * 0.01f, settings.cirrusCloudsWindIntensity * Time.deltaTime * 0.01f);
			}
			cirrusAnim = EnviroHelper.PingPong(cirrusAnim);
			cloudFlatBaseAnim = EnviroHelper.PingPong(cloudFlatBaseAnim);
			cloudFlatDetailAnim = EnviroHelper.PingPong(cloudFlatDetailAnim);
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroFlatCloudsModule module)
		{
			module.settings = JsonUtility.FromJson<EnviroFlatClouds>(JsonUtility.ToJson(settings));
		}
	}
}
