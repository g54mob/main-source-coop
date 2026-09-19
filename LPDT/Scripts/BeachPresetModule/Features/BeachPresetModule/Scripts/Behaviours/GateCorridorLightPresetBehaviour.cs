using System;
using System.Collections.Generic;
using Features.BeachPresetModule.Scripts.Core;
using Features.BeachPresetModule.Scripts.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace Features.BeachPresetModule.Scripts.Behaviours
{
	[Serializable]
	public class GateCorridorLightPresetBehaviour : BeachBehaviour
	{
		[Serializable]
		private class GateCorridorLightSetting
		{
			[Tooltip("Gate index of the registered GateCorridorLightBehaviour that receives this setting.")]
			[SerializeField]
			private int _gateIndex;

			[Tooltip("Light intensity used as the maximum corridor light intensity.")]
			[SerializeField]
			private float _originalIntensity = 6.0000014f;

			[Tooltip("Corridor light color.")]
			[SerializeField]
			private Color _color = new Color(0f, 0.82745105f, 1f, 1f);

			[Tooltip("Unity light type used by this corridor light.")]
			[SerializeField]
			private LightType _lightType = LightType.Point;

			[Tooltip("Light range.")]
			[SerializeField]
			private float _range = 10f;

			[Tooltip("Outer spot angle.")]
			[SerializeField]
			private float _spotAngle = 30f;

			[Tooltip("Inner spot angle.")]
			[SerializeField]
			private float _innerSpotAngle = 21.80208f;

			[Tooltip("Distance from gate where light reaches maximum intensity.")]
			[SerializeField]
			private float _distanceForMaxIntensity = 7.5f;

			[Tooltip("Intensity used while the local player is outside a gate corridor.")]
			[SerializeField]
			private float _attenuatedIntensity;

			[Tooltip("Distance bias subtracted from the player-to-gate distance before attenuation is calculated.")]
			[SerializeField]
			private float _exitBias = 0.5f;

			public BeachGateCorridorLightSetting ToServiceSetting()
			{
				return new BeachGateCorridorLightSetting
				{
					GateIndex = _gateIndex,
					OriginalIntensity = _originalIntensity,
					Color = _color,
					LightType = _lightType,
					Range = _range,
					SpotAngle = _spotAngle,
					InnerSpotAngle = _innerSpotAngle,
					DistanceForMaxIntensity = _distanceForMaxIntensity,
					AttenuatedIntensity = _attenuatedIntensity,
					ExitBias = _exitBias
				};
			}

			public void Validate(BeachValidationResult result)
			{
				if (_gateIndex < 0)
				{
					result.AddError("Gate corridor light gate index cannot be negative.");
				}
				if (_originalIntensity < 0f)
				{
					result.AddError("Gate corridor original intensity cannot be negative.");
				}
				if (_range <= 0f)
				{
					result.AddError("Gate corridor light range must be greater than zero.");
				}
				if (_spotAngle <= 0f)
				{
					result.AddError("Gate corridor spot angle must be greater than zero.");
				}
				if (_innerSpotAngle < 0f || _innerSpotAngle > _spotAngle)
				{
					result.AddError("Gate corridor inner spot angle must be between zero and the outer spot angle.");
				}
				if (_distanceForMaxIntensity <= 0f)
				{
					result.AddError("Gate corridor distance for max intensity must be greater than zero.");
				}
				if (_attenuatedIntensity < 0f)
				{
					result.AddError("Gate corridor attenuated intensity cannot be negative.");
				}
				if (_exitBias < 0f)
				{
					result.AddError("Gate corridor exit bias cannot be negative.");
				}
			}
		}

		[Tooltip("Per-light settings copied from the Level_1_Rain and Level_1_Sundown GateCorridorLight overrides.")]
		[SerializeField]
		private List<GateCorridorLightSetting> _settings = new List<GateCorridorLightSetting>();

		private IBeachGateCorridorLightService _gateCorridorLightService;

		public override int Order => 60;

		[Inject]
		public void InjectDependencies(IBeachGateCorridorLightService gateCorridorLightService)
		{
			_gateCorridorLightService = gateCorridorLightService;
		}

		public override void Apply(BeachPresetRuntimeContext context)
		{
			_gateCorridorLightService.Apply(BuildServiceSettings());
		}

		public override void Clear(BeachPresetRuntimeContext context)
		{
		}

		public override BeachValidationResult Validate(BeachPreset preset)
		{
			BeachValidationResult beachValidationResult = new BeachValidationResult();
			if (_settings.Count == 0)
			{
				beachValidationResult.AddWarning("GateCorridorLightPresetBehaviour has no light settings.");
			}
			foreach (GateCorridorLightSetting setting in _settings)
			{
				setting?.Validate(beachValidationResult);
			}
			return beachValidationResult;
		}

		private IReadOnlyList<BeachGateCorridorLightSetting> BuildServiceSettings()
		{
			List<BeachGateCorridorLightSetting> list = new List<BeachGateCorridorLightSetting>();
			foreach (GateCorridorLightSetting setting in _settings)
			{
				if (setting != null)
				{
					list.Add(setting.ToServiceSetting());
				}
			}
			return list;
		}
	}
}
