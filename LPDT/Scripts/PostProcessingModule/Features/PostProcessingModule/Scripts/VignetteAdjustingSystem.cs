using System;
using System.Collections;
using System.Collections.Generic;
using Features.CoroutineUtils.Scripts;
using Features.MultiplayerSessionServices.Scripts;
using Features.PlayerStatesModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Features.PostProcessingModule.Scripts
{
	public class VignetteAdjustingSystem : IInitializable, IDisposable
	{
		private static readonly Color EmptyColor = Color.black;

		private static readonly float EmptyIntensity = 0f;

		private readonly VignetteAffectionConfiguration _vignetteAffectionConfiguration;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly PostProcessingModel _postProcessingModel;

		private readonly ICoroutineRunner _coroutineRunner;

		private readonly PlayersStatesSynchronizer _playersStatesSynchronizer;

		private readonly IPlayerStateService _playerStateService;

		private IStat _healthStat;

		private float _previousHealth;

		private readonly Dictionary<PostProcessingType, Coroutine> _damageAnimationCoroutines = new Dictionary<PostProcessingType, Coroutine>();

		private readonly Dictionary<PostProcessingType, Color> _baseColors = new Dictionary<PostProcessingType, Color>();

		private readonly Dictionary<PostProcessingType, VignetteEffect> _damageEffects = new Dictionary<PostProcessingType, VignetteEffect>();

		private readonly Dictionary<PostProcessingType, VignetteEffect> _stunEffects = new Dictionary<PostProcessingType, VignetteEffect>();

		private readonly Dictionary<PostProcessingType, VignetteEffect> _deathEffects = new Dictionary<PostProcessingType, VignetteEffect>();

		public VignetteAdjustingSystem(VignetteAffectionConfiguration vignetteAffectionConfiguration, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, PostProcessingModel postProcessingModel, ICoroutineRunner coroutineRunner, PlayersStatesSynchronizer playersStatesSynchronizer, IPlayerStateService playerStateService)
		{
			_vignetteAffectionConfiguration = vignetteAffectionConfiguration;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_postProcessingModel = postProcessingModel;
			_coroutineRunner = coroutineRunner;
			_playersStatesSynchronizer = playersStatesSynchronizer;
			_playerStateService = playerStateService;
		}

		public void Initialize()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= StartCheckForDamage;
			_spawnedEntityStatsModel.OnPlayerStatRegistered += StartCheckForDamage;
			if (_spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				StartCheckForDamage(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
			_postProcessingModel.OnVolumeRegistered += OnVolumeRegistered;
			_playersStatesSynchronizer.OnSomePlayerStateChanged += AdjustVignetteBySomePlayerState;
			UpdateBaseVignetteValues();
		}

		public void Dispose()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= StartCheckForDamage;
			_postProcessingModel.OnVolumeRegistered -= OnVolumeRegistered;
			_playersStatesSynchronizer.OnSomePlayerStateChanged -= AdjustVignetteBySomePlayerState;
			if (_healthStat != null)
			{
				_healthStat.OnFullValueChanged -= AdjustVignette;
			}
			foreach (Coroutine value2 in _damageAnimationCoroutines.Values)
			{
				_coroutineRunner.StopCoroutine(value2);
			}
			foreach (PostProcessingType affectedPostProcessingType in _vignetteAffectionConfiguration.AffectedPostProcessingTypes)
			{
				if (_postProcessingModel.ActiveVolumes.TryGetValue(affectedPostProcessingType, out var value))
				{
					RemoveEffectIfExists(value, _damageEffects, affectedPostProcessingType);
					RemoveEffectIfExists(value, _stunEffects, affectedPostProcessingType);
					RemoveEffectIfExists(value, _deathEffects, affectedPostProcessingType);
				}
			}
		}

		private void OnVolumeRegistered(PostProcessingType ppType, Volume volume)
		{
			if (_vignetteAffectionConfiguration.AffectedPostProcessingTypes.Contains(ppType))
			{
				UpdateBaseVignetteValues();
			}
		}

		private void UpdateBaseVignetteValues()
		{
			foreach (PostProcessingType affectedPostProcessingType in _vignetteAffectionConfiguration.AffectedPostProcessingTypes)
			{
				if (_postProcessingModel.ActiveVolumes.TryGetValue(affectedPostProcessingType, out var value) && value.profile.TryGet<Vignette>(out var component))
				{
					_baseColors[affectedPostProcessingType] = component.color.value;
				}
			}
		}

		private void StartCheckForDamage(int playerId)
		{
			if (_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId == playerId)
			{
				if (_healthStat != null)
				{
					_healthStat.OnFullValueChanged -= AdjustVignette;
				}
				_healthStat = _spawnedEntityStatsModel.PlayerStats[playerId].GetStat(EntityStatType.Health);
				_previousHealth = _healthStat.FullValue;
				UpdateBaseVignetteValues();
				_healthStat.OnFullValueChanged += AdjustVignette;
			}
		}

		private void AdjustVignette(float health)
		{
			float num = _previousHealth - _healthStat.FullValue;
			if (num <= 0f)
			{
				_previousHealth = _healthStat.FullValue;
				return;
			}
			float damagePercent = Mathf.Clamp01(num / _healthStat.MaxValue);
			foreach (PostProcessingType affectedPostProcessingType in _vignetteAffectionConfiguration.AffectedPostProcessingTypes)
			{
				if (!_postProcessingModel.ActiveVolumes.TryGetValue(affectedPostProcessingType, out var value) || !value.profile.TryGet<Vignette>(out var _))
				{
					continue;
				}
				if (_damageAnimationCoroutines.TryGetValue(affectedPostProcessingType, out var value2))
				{
					if (_damageEffects.TryGetValue(affectedPostProcessingType, out var value3))
					{
						_postProcessingModel.RemoveVignetteEffect(value, value3);
					}
					_coroutineRunner.StopCoroutine(value2);
					_damageAnimationCoroutines.Remove(affectedPostProcessingType);
					_damageEffects.Remove(affectedPostProcessingType);
				}
				VignetteEffect vignetteEffect = new VignetteEffect
				{
					EffectColor = EmptyColor,
					EffectIntensity = EmptyIntensity
				};
				_damageEffects[affectedPostProcessingType] = vignetteEffect;
				_postProcessingModel.ApplyVignetteEffect(value, vignetteEffect);
				_damageAnimationCoroutines[affectedPostProcessingType] = _coroutineRunner.StartCoroutine(PlayDamageAnimation(affectedPostProcessingType, value, vignetteEffect, damagePercent));
			}
			_previousHealth = _healthStat.FullValue;
		}

		private IEnumerator PlayDamageAnimation(PostProcessingType postProcessingType, Volume volume, VignetteEffect vignetteEffect, float damagePercent)
		{
			float value = Mathf.Clamp01(damagePercent / _vignetteAffectionConfiguration.MaxDamagePercentForFullIntensity) * _vignetteAffectionConfiguration.MaxDamageAnimationIntensity;
			value = Mathf.Clamp01(value);
			Color value2;
			Color a = (_baseColors.TryGetValue(postProcessingType, out value2) ? value2 : EmptyColor);
			Color targetRedColor = Color.Lerp(a, _vignetteAffectionConfiguration.MaxAffectionColor, value);
			Color startColor = vignetteEffect.EffectColor;
			float startIntensity = vignetteEffect.EffectIntensity;
			float elapsed = 0f;
			float redDuration = _vignetteAffectionConfiguration.DamageAnimationRedDuration;
			while (elapsed < redDuration)
			{
				elapsed += Time.deltaTime;
				float t = elapsed / redDuration;
				vignetteEffect.EffectColor = Color.Lerp(startColor, targetRedColor, t);
				vignetteEffect.EffectIntensity = Mathf.Lerp(startIntensity, _vignetteAffectionConfiguration.MaxAffectionIntensity, t);
				yield return null;
			}
			elapsed = 0f;
			float returnDuration = _vignetteAffectionConfiguration.DamageAnimationReturnDuration;
			Color currentColor = vignetteEffect.EffectColor;
			float currentIntensity = vignetteEffect.EffectIntensity;
			while (elapsed < returnDuration)
			{
				elapsed += Time.deltaTime;
				float t2 = elapsed / returnDuration;
				vignetteEffect.EffectColor = Color.Lerp(currentColor, EmptyColor, t2);
				vignetteEffect.EffectIntensity = Mathf.Lerp(currentIntensity, EmptyIntensity, t2);
				yield return null;
			}
			vignetteEffect.EffectColor = EmptyColor;
			vignetteEffect.EffectIntensity = EmptyIntensity;
			_postProcessingModel.RemoveVignetteEffect(volume, vignetteEffect);
			if (_damageEffects.TryGetValue(postProcessingType, out var value3) && value3 == vignetteEffect)
			{
				_damageEffects.Remove(postProcessingType);
				_damageAnimationCoroutines.Remove(postProcessingType);
			}
		}

		private void AdjustVignetteBySomePlayerState(PlayerStateData playerStateData)
		{
			if (playerStateData.PlayerId != _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId)
			{
				return;
			}
			foreach (PostProcessingType affectedPostProcessingType in _vignetteAffectionConfiguration.AffectedPostProcessingTypes)
			{
				if (_postProcessingModel.ActiveVolumes.TryGetValue(affectedPostProcessingType, out var value))
				{
					StunHandling(playerStateData, affectedPostProcessingType, value);
					DeathHandling(playerStateData, affectedPostProcessingType, value);
				}
			}
		}

		private void RemoveEffectIfExists(Volume volume, Dictionary<PostProcessingType, VignetteEffect> effects, PostProcessingType postProcessingType)
		{
			if (effects.TryGetValue(postProcessingType, out var value))
			{
				_postProcessingModel.RemoveVignetteEffect(volume, value);
			}
		}

		private void StunHandling(PlayerStateData playerStateData, PostProcessingType postProcessingType, Volume volume)
		{
			VignetteEffect value2;
			if (_playerStateService.IsPlayerStunned(playerStateData.PlayerId))
			{
				if (_stunEffects.TryGetValue(postProcessingType, out var value))
				{
					if (!_postProcessingModel.ContainsEffect(volume, value))
					{
						_postProcessingModel.ApplyVignetteEffect(volume, value);
					}
					return;
				}
				value = new VignetteEffect
				{
					EffectColor = _vignetteAffectionConfiguration.StunStateColor,
					EffectIntensity = _vignetteAffectionConfiguration.StunStateIntensity,
					EffectSmoothness = _vignetteAffectionConfiguration.StunStateSmoothness
				};
				_stunEffects[postProcessingType] = value;
				_postProcessingModel.ApplyVignetteEffect(volume, value);
			}
			else if (_stunEffects.TryGetValue(postProcessingType, out value2))
			{
				_postProcessingModel.RemoveVignetteEffect(volume, value2);
				_stunEffects.Remove(postProcessingType);
			}
		}

		private void DeathHandling(PlayerStateData playerStateData, PostProcessingType postProcessingType, Volume volume)
		{
			VignetteEffect value2;
			if (_playerStateService.IsPlayerDead(playerStateData.PlayerId))
			{
				if (_deathEffects.TryGetValue(postProcessingType, out var value))
				{
					if (!_postProcessingModel.ContainsEffect(volume, value))
					{
						_postProcessingModel.ApplyVignetteEffect(volume, value);
					}
					return;
				}
				value = new VignetteEffect
				{
					EffectColor = _vignetteAffectionConfiguration.DeadStateColor,
					EffectIntensity = _vignetteAffectionConfiguration.DeadStateIntensity,
					EffectSmoothness = _vignetteAffectionConfiguration.DeadStateSmoothness
				};
				_deathEffects[postProcessingType] = value;
				_postProcessingModel.ApplyVignetteEffect(volume, value);
			}
			else if (_deathEffects.TryGetValue(postProcessingType, out value2))
			{
				_postProcessingModel.RemoveVignetteEffect(volume, value2);
				_deathEffects.Remove(postProcessingType);
			}
		}
	}
}
