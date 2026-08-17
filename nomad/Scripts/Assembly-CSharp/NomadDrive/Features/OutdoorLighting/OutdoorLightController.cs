using System;
using EvilCore.Extensions;
using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using VContainer;

namespace NomadDrive.Features.OutdoorLighting
{
	public class OutdoorLightController : MonoBehaviour
	{
		[Header("Schedule")]
		[SerializeField]
		[Range(0f, 23f)]
		private int turnOnHour = 19;

		[SerializeField]
		[Range(0f, 23f)]
		private int turnOffHour = 6;

		[Header("Lights")]
		[SerializeField]
		private Light[] lights;

		[Header("Volumetric Fog")]
		[SerializeField]
		private LocalVolumetricFog[] volumetricFogs;

		[Header("Emission")]
		[SerializeField]
		private Renderer[] emissionRenderers;

		[SerializeField]
		private int materialIndex;

		[SerializeField]
		private string emissionPropertyName = "_EmissiveExposureWeight";

		[SerializeField]
		private float emissionAnimDuration = 0.5f;

		[Header("Extra Objects")]
		[SerializeField]
		private GameObject[] extraObjects;

		[Header("Broken Light")]
		[SerializeField]
		private bool isBroken;

		[SerializeField]
		[Range(0.5f, 20f)]
		private float flickerFrequency = 5f;

		[SerializeField]
		[Range(0f, 1f)]
		private float flickerIrregularity = 0.5f;

		[SerializeField]
		[Range(0.1f, 3f)]
		private float flickerIntensityMultiplier = 1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float flickerMinIntensity;

		[SerializeField]
		[Range(0.01f, 1f)]
		private float flickerSmoothing = 0.1f;

		[SerializeField]
		[Range(0f, 1f)]
		private float burstChance = 0.15f;

		[SerializeField]
		private float burstMinDuration = 0.03f;

		[SerializeField]
		private float burstMaxDuration = 0.2f;

		[SerializeField]
		private float stablePhaseMinDuration = 0.5f;

		[SerializeField]
		private float stablePhaseMaxDuration = 4f;

		[SerializeField]
		private float flickerPhaseMinDuration = 0.3f;

		[SerializeField]
		private float flickerPhaseMaxDuration = 3f;

		[Inject]
		private IOutdoorLightingManager _manager;

		private Material[] _emissionMaterials;

		private int _emissionPropertyId;

		private float[] _baseLightIntensities;

		private HDAdditionalLightData[] _lightData;

		private OutdoorLightLodLevel _currentLevel;

		private bool _lodApplied;

		private float _flickerNoiseOffset;

		private float _currentIntensity = 1f;

		private bool _inStablePhase = true;

		private bool _inBurst;

		private float _phaseTimer;

		private float _phaseDuration;

		private float _burstTimer;

		private float _burstDuration;

		[NonSerialized]
		public int LodBandIndex = 3;

		public Vector3 Position => base.transform.position;

		public bool IsBroken => isBroken;

		public OutdoorLightLodLevel CurrentLevel => _currentLevel;

		private void Awake()
		{
			base.gameObject.InjectGameObject();
			CacheEmissionMaterials();
			CacheLightIntensities();
			CacheLightData();
			_flickerNoiseOffset = UnityEngine.Random.Range(0f, 1000f);
			ApplyImmediateOff();
		}

		private void OnEnable()
		{
			_manager?.Register(this);
		}

		private void OnDisable()
		{
			_manager?.Unregister(this);
		}

		public bool IsScheduledOn(int hour)
		{
			if (turnOnHour > turnOffHour)
			{
				if (hour < turnOnHour)
				{
					return hour < turnOffHour;
				}
				return true;
			}
			if (hour >= turnOnHour)
			{
				return hour < turnOffHour;
			}
			return false;
		}

		public void ApplyState(OutdoorLightLodLevel level)
		{
			if (level != _currentLevel)
			{
				bool flag = _currentLevel != OutdoorLightLodLevel.Off;
				bool lodApplied = _lodApplied;
				_lodApplied = true;
				_currentLevel = level;
				bool flag2 = level == OutdoorLightLodLevel.LightOnly || level == OutdoorLightLodLevel.LightWithFog;
				bool lightVolumetric = level == OutdoorLightLodLevel.LightWithFog;
				bool flag3 = level != OutdoorLightLodLevel.Off;
				SetLights(flag2);
				SetLightVolumetric(lightVolumetric);
				SetVolumetricFogs(lightVolumetric);
				SetExtraObjects(flag3);
				if (lodApplied && flag3 != flag)
				{
					AnimateEmission(flag3);
				}
				else
				{
					SetEmission(flag3 ? 0f : 1f);
				}
				if (flag2)
				{
					ResetFlickerState();
				}
				else
				{
					RestoreLightIntensities();
				}
			}
		}

		public void TickFlicker(float dt)
		{
			if (_inBurst)
			{
				_burstTimer -= dt;
				if (!(_burstTimer <= 0f))
				{
					ApplyFlickerIntensity(0f);
					return;
				}
				_inBurst = false;
			}
			if (UnityEngine.Random.value < burstChance * dt)
			{
				_inBurst = true;
				_burstDuration = UnityEngine.Random.Range(burstMinDuration, burstMaxDuration);
				_burstTimer = _burstDuration;
				ApplyFlickerIntensity(0f);
				return;
			}
			_phaseTimer -= dt;
			if (_phaseTimer <= 0f)
			{
				_inStablePhase = !_inStablePhase;
				_phaseDuration = (_inStablePhase ? UnityEngine.Random.Range(stablePhaseMinDuration, stablePhaseMaxDuration) : UnityEngine.Random.Range(flickerPhaseMinDuration, flickerPhaseMaxDuration));
				_phaseTimer = _phaseDuration;
			}
			float b;
			if (_inStablePhase)
			{
				b = flickerIntensityMultiplier;
			}
			else
			{
				float num = Time.time + _flickerNoiseOffset;
				float a = Mathf.PerlinNoise(num * flickerFrequency, 0f);
				float b2 = Mathf.PerlinNoise(num * flickerFrequency * 3.7f, 100f);
				float t = Mathf.Lerp(a, b2, flickerIrregularity);
				b = Mathf.Lerp(flickerMinIntensity, flickerIntensityMultiplier, t);
			}
			_currentIntensity = Mathf.Lerp(_currentIntensity, b, flickerSmoothing);
			ApplyFlickerIntensity(_currentIntensity);
		}

		private void ApplyImmediateOff()
		{
			_currentLevel = OutdoorLightLodLevel.Off;
			SetLights(on: false);
			SetLightVolumetric(on: false);
			SetVolumetricFogs(on: false);
			SetExtraObjects(on: false);
			SetEmission(1f);
		}

		private void ApplyFlickerIntensity(float multiplier)
		{
			if (lights != null && _baseLightIntensities != null)
			{
				for (int i = 0; i < lights.Length; i++)
				{
					if (lights[i] != null)
					{
						lights[i].intensity = _baseLightIntensities[i] * multiplier;
					}
				}
			}
			if (_emissionMaterials != null)
			{
				float emission = Mathf.Lerp(0f, 1f, 1f - multiplier);
				SetEmission(emission);
			}
		}

		private void ResetFlickerState()
		{
			_currentIntensity = 1f;
			_inStablePhase = true;
			_inBurst = false;
			_phaseTimer = UnityEngine.Random.Range(stablePhaseMinDuration, stablePhaseMaxDuration);
			_burstTimer = 0f;
		}

		private void CacheEmissionMaterials()
		{
			_emissionPropertyId = Shader.PropertyToID(emissionPropertyName);
			if (emissionRenderers == null || emissionRenderers.Length == 0)
			{
				return;
			}
			_emissionMaterials = new Material[emissionRenderers.Length];
			for (int i = 0; i < emissionRenderers.Length; i++)
			{
				if (emissionRenderers[i] != null)
				{
					_emissionMaterials[i] = emissionRenderers[i].materials[materialIndex];
				}
			}
		}

		private void CacheLightIntensities()
		{
			if (lights == null || lights.Length == 0)
			{
				return;
			}
			_baseLightIntensities = new float[lights.Length];
			for (int i = 0; i < lights.Length; i++)
			{
				if (lights[i] != null)
				{
					_baseLightIntensities[i] = lights[i].intensity;
				}
			}
		}

		private void CacheLightData()
		{
			if (lights == null || lights.Length == 0)
			{
				return;
			}
			_lightData = new HDAdditionalLightData[lights.Length];
			for (int i = 0; i < lights.Length; i++)
			{
				if (lights[i] != null)
				{
					lights[i].TryGetComponent<HDAdditionalLightData>(out _lightData[i]);
				}
			}
		}

		private void RestoreLightIntensities()
		{
			if (lights == null || _baseLightIntensities == null)
			{
				return;
			}
			for (int i = 0; i < lights.Length; i++)
			{
				if (lights[i] != null)
				{
					lights[i].intensity = _baseLightIntensities[i];
				}
			}
		}

		private void SetEmission(float value)
		{
			if (_emissionMaterials != null)
			{
				Material[] emissionMaterials = _emissionMaterials;
				for (int i = 0; i < emissionMaterials.Length; i++)
				{
					emissionMaterials[i]?.SetFloat(_emissionPropertyId, value);
				}
			}
		}

		private void AnimateEmission(bool on)
		{
			if (_emissionMaterials != null)
			{
				float startValue = (on ? 1f : 0f);
				float endValue = (on ? 0f : 1f);
				Tween.Custom(this, startValue, endValue, emissionAnimDuration, delegate(OutdoorLightController target, float val)
				{
					target.SetEmission(val);
				});
			}
		}

		private void SetLights(bool on)
		{
			if (lights == null)
			{
				return;
			}
			Light[] array = lights;
			foreach (Light light in array)
			{
				if (light != null)
				{
					light.enabled = on;
				}
			}
		}

		private void SetLightVolumetric(bool on)
		{
			if (_lightData == null)
			{
				return;
			}
			for (int i = 0; i < _lightData.Length; i++)
			{
				if (_lightData[i] != null)
				{
					_lightData[i].affectsVolumetric = on;
				}
			}
		}

		private void SetVolumetricFogs(bool on)
		{
			if (volumetricFogs == null)
			{
				return;
			}
			LocalVolumetricFog[] array = volumetricFogs;
			foreach (LocalVolumetricFog localVolumetricFog in array)
			{
				if (localVolumetricFog != null)
				{
					localVolumetricFog.enabled = on;
				}
			}
		}

		private void SetExtraObjects(bool on)
		{
			if (extraObjects == null)
			{
				return;
			}
			GameObject[] array = extraObjects;
			foreach (GameObject gameObject in array)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(on);
				}
			}
		}
	}
}
