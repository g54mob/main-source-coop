using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	public class EnviroAuroraModule : EnviroModule
	{
		public EnviroAurora Settings;

		public EnviroAuroraModule preset;

		public bool showAuroraControls;

		public override void UpdateModule()
		{
			if (active && !(EnviroManager.instance == null) && EnviroManager.instance.Sky != null)
			{
				UpdateAuroraShader();
			}
		}

		public void UpdateAuroraShader()
		{
			if (!Settings.useAurora)
			{
				Shader.SetGlobalFloat("_Aurora", 0f);
				return;
			}
			Shader.SetGlobalFloat("_Aurora", 1f);
			if (Settings.aurora_layer_1 != null)
			{
				Shader.SetGlobalTexture("_Aurora_Layer_1", Settings.aurora_layer_1);
			}
			if (Settings.aurora_layer_2 != null)
			{
				Shader.SetGlobalTexture("_Aurora_Layer_2", Settings.aurora_layer_2);
			}
			if (Settings.aurora_colorshift != null)
			{
				Shader.SetGlobalTexture("_Aurora_Colorshift", Settings.aurora_colorshift);
			}
			Shader.SetGlobalFloat("_AuroraIntensity", Mathf.Clamp01(Settings.auroraIntensityModifier * Settings.auroraIntensity.Evaluate(EnviroManager.instance.solarTime)));
			Shader.SetGlobalFloat("_AuroraBrightness", Settings.auroraBrightness);
			Shader.SetGlobalFloat("_AuroraContrast", Settings.auroraContrast);
			Shader.SetGlobalColor("_AuroraColor", Settings.auroraColor);
			Shader.SetGlobalFloat("_AuroraHeight", Settings.auroraHeight);
			Shader.SetGlobalFloat("_AuroraScale", Settings.auroraScale);
			Shader.SetGlobalFloat("_AuroraSpeed", Settings.auroraSpeed);
			Shader.SetGlobalFloat("_AuroraSteps", Settings.auroraSteps);
			Shader.SetGlobalFloat("_AuroraSteps", Settings.auroraSteps);
			Shader.SetGlobalVector("_Aurora_Tiling_Layer1", Settings.auroraLayer1Settings);
			Shader.SetGlobalVector("_Aurora_Tiling_Layer2", Settings.auroraLayer2Settings);
			Shader.SetGlobalVector("_Aurora_Tiling_ColorShift", Settings.auroraColorshiftSettings);
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroAurora>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroAuroraModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroAurora>(JsonUtility.ToJson(Settings));
		}
	}
}
