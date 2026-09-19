using System;
using System.Collections.Generic;
using Features.MultiplayerSessionServices.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.VignetteUIEffectModule.Scripts;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Features.PostProcessingModule.Scripts
{
	public class StaminaVignetteAdjustingSystem : IInitializable, IDisposable
	{
		private readonly VignetteAffectionConfiguration _vignetteAffectionConfiguration;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PostProcessingModel _postProcessingModel;

		private readonly VignetteUIEffectModel _vignetteUIEffectModel;

		private IStat _hiddenStaminaStat;

		private readonly Dictionary<PostProcessingType, VignetteEffect> _staminaEffects = new Dictionary<PostProcessingType, VignetteEffect>();

		private readonly Dictionary<VignetteUIEffectType, VignetteUIEffect> _staminaUiEffects = new Dictionary<VignetteUIEffectType, VignetteUIEffect>();

		private readonly Color _color = Color.red;

		public StaminaVignetteAdjustingSystem(VignetteAffectionConfiguration vignetteAffectionConfiguration, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, PostProcessingModel postProcessingModel, VignetteUIEffectModel vignetteUIEffectModel)
		{
			_vignetteAffectionConfiguration = vignetteAffectionConfiguration;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_postProcessingModel = postProcessingModel;
			_vignetteUIEffectModel = vignetteUIEffectModel;
		}

		public void Initialize()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= StartCheckForStaminaChange;
			_spawnedEntityStatsModel.OnPlayerStatRegistered += StartCheckForStaminaChange;
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				StartCheckForStaminaChange(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
			_postProcessingModel.OnVolumeRegistered += InitializeVignetteEffect;
			_vignetteUIEffectModel.OnLayerRegistered += InitializeVignetteUIEffect;
			foreach (PostProcessingType affectedPostProcessingType in _vignetteAffectionConfiguration.AffectedPostProcessingTypes)
			{
				if (_postProcessingModel.ActiveVolumes.TryGetValue(affectedPostProcessingType, out var value))
				{
					InitializeVignetteEffect(affectedPostProcessingType, value);
				}
			}
			foreach (VignetteUIEffectType affectedVignetteUIEffectType in _vignetteAffectionConfiguration.AffectedVignetteUIEffectTypes)
			{
				if (_vignetteUIEffectModel.ActiveLayers.ContainsKey(affectedVignetteUIEffectType))
				{
					InitializeVignetteUIEffect(affectedVignetteUIEffectType);
				}
			}
		}

		public void Dispose()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= StartCheckForStaminaChange;
			_vignetteUIEffectModel.OnLayerRegistered -= InitializeVignetteUIEffect;
			if (_hiddenStaminaStat != null)
			{
				_hiddenStaminaStat.OnFullValueChanged -= AdjustVignette;
				_hiddenStaminaStat.OnReachedMinValue -= OnMinValueChanged;
			}
			foreach (PostProcessingType affectedPostProcessingType in _vignetteAffectionConfiguration.AffectedPostProcessingTypes)
			{
				if (_postProcessingModel.ActiveVolumes.TryGetValue(affectedPostProcessingType, out var value) && _staminaEffects.TryGetValue(affectedPostProcessingType, out var value2))
				{
					_postProcessingModel.RemoveVignetteEffect(value, value2);
				}
			}
			foreach (VignetteUIEffectType affectedVignetteUIEffectType in _vignetteAffectionConfiguration.AffectedVignetteUIEffectTypes)
			{
				if (_staminaUiEffects.TryGetValue(affectedVignetteUIEffectType, out var value3))
				{
					_vignetteUIEffectModel.RemoveVignetteEffect(affectedVignetteUIEffectType, value3);
				}
			}
			_postProcessingModel.OnVolumeRegistered -= InitializeVignetteEffect;
		}

		private void InitializeVignetteEffect(PostProcessingType postProcessingType, Volume volume)
		{
			if (!_vignetteAffectionConfiguration.AffectedPostProcessingTypes.Contains(postProcessingType) || !volume.profile.TryGet<Vignette>(out var _))
			{
				return;
			}
			if (_staminaEffects.TryGetValue(postProcessingType, out var value))
			{
				if (!_postProcessingModel.ContainsEffect(volume, value))
				{
					_postProcessingModel.ApplyVignetteEffect(volume, value);
				}
			}
			else
			{
				VignetteEffect vignetteEffect = new VignetteEffect
				{
					EffectColor = _color,
					EffectIntensity = 0f
				};
				_staminaEffects[postProcessingType] = vignetteEffect;
				_postProcessingModel.ApplyVignetteEffect(volume, vignetteEffect);
			}
		}

		private void InitializeVignetteUIEffect(VignetteUIEffectType vignetteUIEffectType)
		{
			if (!_vignetteAffectionConfiguration.AffectedVignetteUIEffectTypes.Contains(vignetteUIEffectType) || !_vignetteUIEffectModel.ActiveLayers.ContainsKey(vignetteUIEffectType))
			{
				return;
			}
			if (_staminaUiEffects.TryGetValue(vignetteUIEffectType, out var value))
			{
				if (!_vignetteUIEffectModel.ContainsEffect(vignetteUIEffectType, value))
				{
					_vignetteUIEffectModel.ApplyVignetteEffect(vignetteUIEffectType, value);
				}
			}
			else
			{
				VignetteUIEffect vignetteUIEffect = new VignetteUIEffect
				{
					EffectIntensity = 0f
				};
				_staminaUiEffects[vignetteUIEffectType] = vignetteUIEffect;
				_vignetteUIEffectModel.ApplyVignetteEffect(vignetteUIEffectType, vignetteUIEffect);
			}
		}

		private void StartCheckForStaminaChange(int playerId)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId)
			{
				if (_hiddenStaminaStat != null)
				{
					_hiddenStaminaStat.OnFullValueChanged -= AdjustVignette;
					_hiddenStaminaStat.OnReachedMinValue -= OnMinValueChanged;
				}
				_hiddenStaminaStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.HiddenStamina);
				_hiddenStaminaStat.OnFullValueChanged += AdjustVignette;
				_hiddenStaminaStat.OnReachedMinValue += OnMinValueChanged;
			}
		}

		private void AdjustVignette(float stamina)
		{
			float effectIntensity = (1f - Mathf.Clamp01(_hiddenStaminaStat.FullValue / _hiddenStaminaStat.MaxValue)) * _vignetteAffectionConfiguration.StaminaPunishmentIntensityMultiplier;
			foreach (PostProcessingType affectedPostProcessingType in _vignetteAffectionConfiguration.AffectedPostProcessingTypes)
			{
				if (_postProcessingModel.ActiveVolumes.TryGetValue(affectedPostProcessingType, out var value) && value.profile.TryGet<Vignette>(out var _) && _staminaEffects.TryGetValue(affectedPostProcessingType, out var value2))
				{
					value2.EffectIntensity = effectIntensity;
				}
			}
			foreach (VignetteUIEffectType affectedVignetteUIEffectType in _vignetteAffectionConfiguration.AffectedVignetteUIEffectTypes)
			{
				if (_vignetteUIEffectModel.ActiveLayers.ContainsKey(affectedVignetteUIEffectType) && _staminaUiEffects.TryGetValue(affectedVignetteUIEffectType, out var value3))
				{
					value3.EffectIntensity = effectIntensity;
				}
			}
		}

		private void OnMinValueChanged()
		{
			foreach (VignetteUIEffectType affectedVignetteUIEffectType in _vignetteAffectionConfiguration.AffectedVignetteUIEffectTypes)
			{
				_vignetteUIEffectModel.PlayFocusAnimation(affectedVignetteUIEffectType);
			}
		}
	}
}
