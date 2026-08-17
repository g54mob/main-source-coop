using System;
using Enviro;
using UnityEngine;
using UnityEngine.Rendering;

namespace NomadDrive.Features.CustomEnviro
{
	[Serializable]
	[CreateAssetMenu(fileName = "EnviroLensFlareModule", menuName = "Enviro3/Lens Flare Module")]
	public class EnviroLensFlareModule : EnviroModule
	{
		public EnviroLensFlareSettings Settings = new EnviroLensFlareSettings();

		public EnviroLensFlareModule preset;

		private LensFlareComponentSRP _sunFlare;

		private LensFlareComponentSRP _moonFlare;

		private bool _isSingleMode;

		private float _currentSunIntensity;

		private float _currentMoonIntensity;

		public override void Enable()
		{
			if (!(EnviroManager.instance == null))
			{
				FindFlares();
				ApplyImmediate();
			}
		}

		public override void Disable()
		{
			_sunFlare = null;
			_moonFlare = null;
		}

		public override void UpdateModule()
		{
			if (active && !(EnviroManager.instance == null))
			{
				float solarTime = EnviroManager.instance.solarTime;
				float dt = Time.deltaTime * Settings.transitionSpeed;
				if (_isSingleMode)
				{
					UpdateSingleMode(solarTime, dt);
				}
				else
				{
					UpdateDualMode(solarTime, dt);
				}
			}
		}

		private void UpdateSingleMode(float solarTime, float dt)
		{
			if (!(_sunFlare == null))
			{
				float num = Settings.sunFlareCurve.Evaluate(solarTime) * Settings.sunFlareIntensity;
				float num2 = Settings.moonFlareCurve.Evaluate(solarTime) * Settings.moonFlareIntensity;
				float b = num + num2;
				_currentSunIntensity = Mathf.Lerp(_currentSunIntensity, b, dt);
				_sunFlare.intensity = _currentSunIntensity;
			}
		}

		private void UpdateDualMode(float solarTime, float dt)
		{
			float b = Settings.sunFlareCurve.Evaluate(solarTime) * Settings.sunFlareIntensity;
			float b2 = Settings.moonFlareCurve.Evaluate(solarTime) * Settings.moonFlareIntensity;
			_currentSunIntensity = Mathf.Lerp(_currentSunIntensity, b, dt);
			_currentMoonIntensity = Mathf.Lerp(_currentMoonIntensity, b2, dt);
			if (_sunFlare != null)
			{
				_sunFlare.intensity = _currentSunIntensity;
			}
			if (_moonFlare != null)
			{
				_moonFlare.intensity = _currentMoonIntensity;
			}
		}

		public void SaveModuleValues(EnviroLensFlareModule module)
		{
			module.Settings = JsonUtility.FromJson<EnviroLensFlareSettings>(JsonUtility.ToJson(Settings));
		}

		public void LoadModuleValues()
		{
			if (preset != null)
			{
				Settings = JsonUtility.FromJson<EnviroLensFlareSettings>(JsonUtility.ToJson(preset.Settings));
			}
		}

		private void FindFlares()
		{
			EnviroManager instance = EnviroManager.instance;
			Light directionalLight = instance.Objects.directionalLight;
			if (directionalLight != null)
			{
				_sunFlare = directionalLight.GetComponent<LensFlareComponentSRP>();
			}
			Light additionalDirectionalLight = instance.Objects.additionalDirectionalLight;
			if (additionalDirectionalLight != null)
			{
				_moonFlare = additionalDirectionalLight.GetComponent<LensFlareComponentSRP>();
			}
			_isSingleMode = _moonFlare == null;
			if (_sunFlare == null)
			{
				_ = _moonFlare == null;
			}
		}

		private void ApplyImmediate()
		{
			float solarTime = EnviroManager.instance.solarTime;
			if (_isSingleMode)
			{
				if (!(_sunFlare == null))
				{
					float num = Settings.sunFlareCurve.Evaluate(solarTime) * Settings.sunFlareIntensity;
					float num2 = Settings.moonFlareCurve.Evaluate(solarTime) * Settings.moonFlareIntensity;
					_currentSunIntensity = num + num2;
					_sunFlare.intensity = _currentSunIntensity;
				}
				return;
			}
			_currentSunIntensity = Settings.sunFlareCurve.Evaluate(solarTime) * Settings.sunFlareIntensity;
			_currentMoonIntensity = Settings.moonFlareCurve.Evaluate(solarTime) * Settings.moonFlareIntensity;
			if (_sunFlare != null)
			{
				_sunFlare.intensity = _currentSunIntensity;
			}
			if (_moonFlare != null)
			{
				_moonFlare.intensity = _currentMoonIntensity;
			}
		}
	}
}
