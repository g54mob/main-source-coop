using System;
using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using Features.LevelLightModule.Scripts;
using Features.LevelModule.Scripts.RoomVariations;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Behaviours
{
	[Serializable]
	public class LightingBehaviour : BeachBehaviour
	{
		[Serializable]
		private class DirectionalLightSettings
		{
			[Tooltip("Enable or disable RenderSettings.sun directional light.")]
			[SerializeField]
			private bool _enabled = true;

			[Tooltip("Light color applied by this preset.")]
			[SerializeField]
			private Color _color = Color.white;

			[Tooltip("Use randomized intensity instead of a single static intensity.")]
			[SerializeField]
			private bool _randomizeIntensity;

			[Tooltip("Static light intensity used when randomization is disabled.")]
			[SerializeField]
			private float _intensity = 1f;

			[Tooltip("Random intensity range used when randomization is enabled.")]
			[SerializeField]
			private BeachFloatRange _intensityRange = new BeachFloatRange(0.75f, 1.25f);

			[SerializeField]
			private bool _enableRotation;

			[Tooltip("Use randomized rotation instead of a single static rotation.")]
			[SerializeField]
			private bool _randomizeRotation;

			[Tooltip("Static light rotation in local Euler angles.")]
			[SerializeField]
			private Vector3 _rotationEuler = new Vector3(50f, -30f, 0f);

			[Tooltip("Minimum random rotation in Euler angles.")]
			[SerializeField]
			private Vector3 _minRotationEuler = new Vector3(45f, -45f, 0f);

			[Tooltip("Maximum random rotation in Euler angles.")]
			[SerializeField]
			private Vector3 _maxRotationEuler = new Vector3(60f, 45f, 0f);

			public BeachDirectionalLightSettings ToServiceSettings()
			{
				return new BeachDirectionalLightSettings
				{
					Enabled = _enabled,
					Color = _color,
					Intensity = (_randomizeIntensity ? _intensityRange.Evaluate() : _intensity),
					Rotation = Quaternion.Euler(EvaluateRotation())
				};
			}

			public void Validate(BeachValidationResult result)
			{
				if (_randomizeIntensity)
				{
					_intensityRange.Validate(result, "Directional light intensity range");
				}
				if (!_randomizeIntensity && _intensity < 0f)
				{
					result.AddError("Directional light intensity cannot be negative.");
				}
				if (_randomizeRotation && (_minRotationEuler.x > _maxRotationEuler.x || _minRotationEuler.y > _maxRotationEuler.y || _minRotationEuler.z > _maxRotationEuler.z))
				{
					result.AddError("Directional light random rotation minimum values must not exceed maximum values.");
				}
			}

			private Vector3 EvaluateRotation()
			{
				if (!_randomizeRotation)
				{
					return _rotationEuler;
				}
				return new Vector3(UnityEngine.Random.Range(_minRotationEuler.x, _maxRotationEuler.x), UnityEngine.Random.Range(_minRotationEuler.y, _maxRotationEuler.y), UnityEngine.Random.Range(_minRotationEuler.z, _maxRotationEuler.z));
			}
		}

		[Serializable]
		private class LightGroupOverride
		{
			[Tooltip("Registered scene light group that will receive this setting.")]
			[SerializeField]
			private LightObjectGroup _group = LightObjectGroup.Location;

			[Tooltip("Lighting values applied to every registered light in this group.")]
			[SerializeField]
			private LightSettingData _settings = new LightSettingData();

			[Tooltip("Use randomized intensity for this group override.")]
			[SerializeField]
			private bool _randomizeIntensity;

			[Tooltip("Random intensity range used when group intensity randomization is enabled.")]
			[SerializeField]
			private BeachFloatRange _intensityRange = new BeachFloatRange(0.5f, 2f);

			public BeachLightGroupOverride ToServiceOverride()
			{
				return new BeachLightGroupOverride
				{
					Group = _group,
					Settings = new LightSettingData
					{
						Color = _settings.Color,
						Temperature = _settings.Temperature,
						Intensity = (_randomizeIntensity ? _intensityRange.Evaluate() : _settings.Intensity),
						Range = _settings.Range,
						ShadowType = _settings.ShadowType,
						ShadowStrength = _settings.ShadowStrength,
						ShadowNearPlane = _settings.ShadowNearPlane
					}
				};
			}

			public void Validate(BeachValidationResult result)
			{
				if (_group == LightObjectGroup.None)
				{
					result.AddError("Light group override cannot target LightObjectGroup.None.");
				}
				if (_settings == null)
				{
					result.AddError("Light group override settings are missing.");
					return;
				}
				if (_settings.Intensity < 0f)
				{
					result.AddError($"Light group {_group} intensity cannot be negative.");
				}
				if (_settings.Range < 0f)
				{
					result.AddError($"Light group {_group} range cannot be negative.");
				}
				if (_randomizeIntensity)
				{
					_intensityRange.Validate(result, $"Light group {_group} intensity range");
				}
			}
		}

		[Serializable]
		private class IndexedLightOverride
		{
			[Tooltip("Must match BeachIndexedLightAutoRegister on the scene light, or an indexed light on the overridden room's LevelRoomEntity.")]
			[SerializeField]
			private int _lightIndex;

			[Tooltip("Optional: when set, the index is resolved on every registered room of this type instead of the global indexed light registry.")]
			[SerializeField]
			private RoomType _roomOverride;

			[Tooltip("Lighting values applied to the registered light with this index.")]
			[SerializeField]
			private LightSettingData _settings = new LightSettingData();

			[Tooltip("Use randomized intensity for this indexed light override.")]
			[SerializeField]
			private bool _randomizeIntensity;

			[Tooltip("Random intensity range used when indexed light intensity randomization is enabled.")]
			[SerializeField]
			private BeachFloatRange _intensityRange = new BeachFloatRange(0.5f, 2f);

			public BeachIndexedLightOverride ToServiceOverride()
			{
				return new BeachIndexedLightOverride
				{
					LightIndex = _lightIndex,
					RoomOverride = _roomOverride,
					Settings = new LightSettingData
					{
						Color = _settings.Color,
						Temperature = _settings.Temperature,
						Intensity = (_randomizeIntensity ? _intensityRange.Evaluate() : _settings.Intensity),
						Range = _settings.Range,
						ShadowType = _settings.ShadowType,
						ShadowStrength = _settings.ShadowStrength,
						ShadowNearPlane = _settings.ShadowNearPlane
					}
				};
			}

			public void Validate(BeachValidationResult result, HashSet<(RoomType, int)> usedIndices)
			{
				if (_lightIndex < 0)
				{
					result.AddError("Indexed light override index cannot be negative.");
				}
				else if (!usedIndices.Add((_roomOverride, _lightIndex)))
				{
					result.AddError($"Duplicate indexed light override for index {_lightIndex} (room override {_roomOverride}).");
				}
				if (_settings == null)
				{
					result.AddError($"Indexed light #{_lightIndex} override settings are missing.");
					return;
				}
				if (_settings.Intensity < 0f)
				{
					result.AddError($"Indexed light {_lightIndex} intensity cannot be negative.");
				}
				if (_settings.Range < 0f)
				{
					result.AddError($"Indexed light {_lightIndex} range cannot be negative.");
				}
				if (_randomizeIntensity)
				{
					_intensityRange.Validate(result, $"Indexed light {_lightIndex} intensity range");
				}
			}
		}

		[Tooltip("Apply a skybox material when this preset is activated.")]
		[SerializeField]
		private bool _applySkybox = true;

		[Tooltip("Skybox material used by this beach preset.")]
		[SerializeField]
		private Material _skybox;

		[Tooltip("Apply ambient lighting values when this preset is activated.")]
		[SerializeField]
		private bool _applyAmbient = true;

		[Tooltip("Unity ambient lighting mode for this beach preset.")]
		[SerializeField]
		private AmbientMode _ambientMode = AmbientMode.Flat;

		[Tooltip("Flat ambient color used when ambient mode is Flat.")]
		[SerializeField]
		private Color _ambientColor = Color.gray;

		[Tooltip("Sky ambient color used when ambient mode is Trilight.")]
		[SerializeField]
		private Color _ambientSkyColor = Color.gray;

		[Tooltip("Equator ambient color used when ambient mode is Trilight.")]
		[SerializeField]
		private Color _ambientEquatorColor = Color.gray;

		[Tooltip("Ground ambient color used when ambient mode is Trilight.")]
		[SerializeField]
		private Color _ambientGroundColor = Color.gray;

		[Tooltip("Ambient intensity multiplier.")]
		[SerializeField]
		private float _ambientIntensity = 1f;

		[Tooltip("Reflection intensity multiplier.")]
		[SerializeField]
		private float _reflectionIntensity = 1f;

		[Tooltip("Number of reflection bounces.")]
		[SerializeField]
		private int _reflectionBounces = 1;

		[Tooltip("Apply directional light settings when this preset is activated.")]
		[SerializeField]
		private bool _applyDirectionalLight = true;

		[Tooltip("Directional light settings for sun/moon style lighting.")]
		[SerializeField]
		private DirectionalLightSettings _directionalLightSettings = new DirectionalLightSettings();

		[Tooltip("Registered light groups to configure through LightObjectsGroupService.")]
		[SerializeField]
		private List<LightGroupOverride> _lightGroupOverrides = new List<LightGroupOverride>();

		[Tooltip("Lights registered by index via BeachIndexedLightAutoRegister.")]
		[SerializeField]
		private List<IndexedLightOverride> _indexedLightOverrides = new List<IndexedLightOverride>();

		private IBeachLightingService _lightingService;

		private BeachLightingSnapshot _snapshot;

		public override int Order => 0;

		[Inject]
		public void InjectDependencies(IBeachLightingService lightingService)
		{
			_lightingService = lightingService;
		}

		public override void Apply(BeachPresetRuntimeContext context)
		{
			BeachLightingSettings settings = BuildSettings();
			_snapshot = _lightingService.Capture(settings);
			_lightingService.Apply(settings);
		}

		public override void Clear(BeachPresetRuntimeContext context)
		{
			if (_snapshot != null)
			{
				_lightingService.Restore(_snapshot);
				_snapshot = null;
			}
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult beachValidationResult = new BeachValidationResult();
			if (_applySkybox && _skybox == null)
			{
				beachValidationResult.AddWarning("LightingBehaviour is set to apply a skybox, but no skybox material is assigned.");
			}
			if (_applyAmbient && _ambientIntensity < 0f)
			{
				beachValidationResult.AddError("Ambient intensity cannot be negative.");
			}
			if (_reflectionIntensity < 0f)
			{
				beachValidationResult.AddError("Reflection intensity cannot be negative.");
			}
			if (_reflectionBounces < 0)
			{
				beachValidationResult.AddError("Reflection bounces cannot be negative.");
			}
			if (_applyDirectionalLight)
			{
				_directionalLightSettings.Validate(beachValidationResult);
			}
			foreach (LightGroupOverride lightGroupOverride in _lightGroupOverrides)
			{
				lightGroupOverride?.Validate(beachValidationResult);
			}
			HashSet<(RoomType, int)> usedIndices = new HashSet<(RoomType, int)>();
			foreach (IndexedLightOverride indexedLightOverride in _indexedLightOverrides)
			{
				indexedLightOverride?.Validate(beachValidationResult, usedIndices);
			}
			return beachValidationResult;
		}

		private BeachLightingSettings BuildSettings()
		{
			List<BeachLightGroupOverride> list = new List<BeachLightGroupOverride>();
			foreach (LightGroupOverride lightGroupOverride in _lightGroupOverrides)
			{
				if (lightGroupOverride != null)
				{
					list.Add(lightGroupOverride.ToServiceOverride());
				}
			}
			List<BeachIndexedLightOverride> list2 = new List<BeachIndexedLightOverride>();
			foreach (IndexedLightOverride indexedLightOverride in _indexedLightOverrides)
			{
				if (indexedLightOverride != null)
				{
					list2.Add(indexedLightOverride.ToServiceOverride());
				}
			}
			return new BeachLightingSettings
			{
				ApplySkybox = _applySkybox,
				Skybox = _skybox,
				ApplyAmbient = _applyAmbient,
				AmbientMode = _ambientMode,
				AmbientColor = _ambientColor,
				AmbientSkyColor = _ambientSkyColor,
				AmbientEquatorColor = _ambientEquatorColor,
				AmbientGroundColor = _ambientGroundColor,
				AmbientIntensity = _ambientIntensity,
				ReflectionIntensity = _reflectionIntensity,
				ReflectionBounces = _reflectionBounces,
				ApplyDirectionalLight = _applyDirectionalLight,
				DirectionalLightSettings = _directionalLightSettings.ToServiceSettings(),
				LightGroupOverrides = list,
				IndexedLightOverrides = list2
			};
		}
	}
}
