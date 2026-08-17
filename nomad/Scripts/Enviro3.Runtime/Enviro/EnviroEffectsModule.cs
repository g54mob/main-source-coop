using System;
using UnityEngine;

namespace Enviro
{
	[Serializable]
	[ExecuteInEditMode]
	public class EnviroEffectsModule : EnviroModule
	{
		public EnviroEffects Settings;

		public EnviroEffectsModule preset;

		public bool showSetupControls;

		public bool showEmissionControls;

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

		private void Setup()
		{
			if (active)
			{
				CreateEffects();
			}
		}

		private void Cleanup()
		{
			if (EnviroManager.instance.Objects.effects != null)
			{
				UnityEngine.Object.DestroyImmediate(EnviroManager.instance.Objects.effects);
			}
		}

		public override void UpdateModule()
		{
			UpdateEffects();
		}

		public void CreateEffects()
		{
			if (EnviroManager.instance.Objects.effects != null)
			{
				EnviroHelper.DestroyExtended(EnviroManager.instance.Objects.effects);
				EnviroManager.instance.Objects.effects = null;
			}
			if (EnviroManager.instance.Objects.effects == null)
			{
				EnviroManager.instance.Objects.effects = new GameObject();
				EnviroManager.instance.Objects.effects.hideFlags = HideFlags.DontSave;
				EnviroManager.instance.Objects.effects.name = "Effects";
				EnviroManager.instance.Objects.effects.transform.SetParent(EnviroManager.instance.transform);
				EnviroManager.instance.Objects.effects.transform.localPosition = Vector3.zero;
			}
			for (int i = 0; i < Settings.effectTypes.Count; i++)
			{
				if (Settings.effectTypes[i].mySystem != null)
				{
					EnviroHelper.DestroyExtended(Settings.effectTypes[i].mySystem.gameObject);
					Settings.effectTypes[i].mySystem = null;
				}
				if (Settings.effectTypes[i].prefab != null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(Settings.effectTypes[i].prefab, Settings.effectTypes[i].localPositionOffset, Quaternion.identity);
					gameObject.transform.SetParent(EnviroManager.instance.Objects.effects.transform);
					gameObject.name = Settings.effectTypes[i].name + " Particle System";
					gameObject.transform.localPosition = Settings.effectTypes[i].localPositionOffset;
					gameObject.transform.localEulerAngles = Settings.effectTypes[i].localRotationOffset;
					Settings.effectTypes[i].mySystem = gameObject.GetComponent<ParticleSystem>();
					if (Settings.effectTypes[i].emissionRate <= 0f)
					{
						Settings.effectTypes[i].mySystem.Clear();
						Settings.effectTypes[i].mySystem.Stop();
					}
				}
			}
		}

		public float GetEmissionRate(ParticleSystem system)
		{
			return system.emission.rateOverTime.constantMax;
		}

		public void SetEmissionRate(ParticleSystem sys, float emissionRate)
		{
			ParticleSystem.EmissionModule emission = sys.emission;
			ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
			rateOverTime.constantMax = emissionRate;
			emission.rateOverTime = rateOverTime;
		}

		private void UpdateEffects()
		{
			Shader.SetGlobalFloat("_EnviroLightIntensity", EnviroManager.instance.solarTime);
			for (int i = 0; i < Settings.effectTypes.Count; i++)
			{
				if (Settings.effectTypes[i].mySystem != null)
				{
					float num = Settings.effectTypes[i].maxEmission * Settings.effectTypes[i].emissionRate * Settings.particeEmissionRateModifier;
					SetEmissionRate(Settings.effectTypes[i].mySystem, num);
					if (num > 0f && !Settings.effectTypes[i].mySystem.isPlaying)
					{
						Settings.effectTypes[i].mySystem.Play();
					}
				}
			}
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroEffects>(JsonUtility.ToJson(preset.Settings));
			}
			else
			{
				Debug.Log("Please assign a saved module to load from!");
			}
		}

		public void SaveModuleValues()
		{
		}

		public void SaveModuleValues(EnviroEffectsModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroEffects>(JsonUtility.ToJson(Settings));
		}
	}
}
