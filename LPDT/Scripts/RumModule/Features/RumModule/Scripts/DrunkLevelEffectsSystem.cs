using System;
using Features.GameUpdaterModule;
using Features.MultiplayerSessionServices.Scripts;
using Features.PostProcessingModule.Scripts;
using Features.RagdollModule.Scripts;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using Features.StatsUsageModule.Scripts.StatsData;
using Features.VignetteUIEffectModule.Scripts;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Features.RumModule.Scripts
{
	public class DrunkLevelEffectsSystem : IInitializable, IDisposable
	{
		private enum FaintPhase
		{
			Idle = 0,
			FadingIn = 1,
			PassedOut = 2
		}

		private static readonly Color BlackoutColor = Color.black;

		private readonly IGameUpdater _gameUpdater;

		private readonly SpawnedEntityStatsModel _spawnedEntityStatsModel;

		private readonly MultiplayerModel _multiplayerModel;

		private readonly DrunkennessConfiguration _configuration;

		private readonly PlayersRagdollModel _playersRagdollModel;

		private readonly PostProcessingModel _postProcessingModel;

		private readonly VignetteUIEffectModel _vignetteUIEffectModel;

		private IStat _drunkennessStat;

		private float _faintCooldown;

		private float _phaseTimer;

		private FaintPhase _phase;

		private VignetteEffect _blackoutEffect;

		private VignetteUIEffect _staminaUiBlackoutEffect;

		public DrunkLevelEffectsSystem(IGameUpdater gameUpdater, SpawnedEntityStatsModel spawnedEntityStatsModel, MultiplayerModel multiplayerModel, DrunkennessConfiguration configuration, PlayersRagdollModel playersRagdollModel, PostProcessingModel postProcessingModel, VignetteUIEffectModel vignetteUIEffectModel)
		{
			_gameUpdater = gameUpdater;
			_spawnedEntityStatsModel = spawnedEntityStatsModel;
			_multiplayerModel = multiplayerModel;
			_configuration = configuration;
			_playersRagdollModel = playersRagdollModel;
			_postProcessingModel = postProcessingModel;
			_vignetteUIEffectModel = vignetteUIEffectModel;
		}

		public void Initialize()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered += OnPlayerStatRegistered;
			_postProcessingModel.OnVolumeRegistered += OnVolumeRegistered;
			_vignetteUIEffectModel.OnLayerRegistered += OnVignetteUiLayerRegistered;
			_gameUpdater.OnUpdate += Tick;
			if (_multiplayerModel.NetworkRunner != null && _spawnedEntityStatsModel.PlayerStats.ContainsKey(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId))
			{
				OnPlayerStatRegistered(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId);
			}
			EnsureBlackoutEffects();
		}

		public void Dispose()
		{
			_spawnedEntityStatsModel.OnPlayerStatRegistered -= OnPlayerStatRegistered;
			_postProcessingModel.OnVolumeRegistered -= OnVolumeRegistered;
			_vignetteUIEffectModel.OnLayerRegistered -= OnVignetteUiLayerRegistered;
			_gameUpdater.OnUpdate -= Tick;
			EndSequence(clearBlackout: true);
			RemoveBlackoutEffects();
		}

		private void OnPlayerStatRegistered(int playerId)
		{
			if (!(_multiplayerModel.NetworkRunner == null) && playerId == _multiplayerModel.NetworkRunner.LocalPlayer.PlayerId && _spawnedEntityStatsModel.PlayerStats.TryGetValue(playerId, out var value))
			{
				_drunkennessStat = value.GetStat(EntityStatType.Drunkenness);
			}
		}

		private void OnVolumeRegistered(PostProcessingType type, Volume _)
		{
			if (type == PostProcessingType.SessionSceneMain)
			{
				EnsureBlackoutEffects();
			}
		}

		private void OnVignetteUiLayerRegistered(VignetteUIEffectType type)
		{
			if (type == VignetteUIEffectType.Stamina)
			{
				EnsureBlackoutEffects();
			}
		}

		private void Tick()
		{
			float deltaTime = Time.deltaTime;
			if (_faintCooldown > 0f)
			{
				_faintCooldown -= deltaTime;
			}
			if (_drunkennessStat != null && !(_configuration == null))
			{
				switch (_phase)
				{
				case FaintPhase.FadingIn:
					TickFadeIn(deltaTime);
					break;
				case FaintPhase.PassedOut:
					TickPassedOut(deltaTime);
					break;
				default:
					TryStartFadeIn();
					break;
				}
			}
		}

		private void TryStartFadeIn()
		{
			if (!(_faintCooldown > 0f) && !(_drunkennessStat.FullValue < _configuration.FaintMinDrunkenness))
			{
				_phase = FaintPhase.FadingIn;
				_phaseTimer = 0f;
				_faintCooldown = _configuration.FaintCooldownSeconds;
				EnsureBlackoutEffects();
				SetBlackoutIntensity(0f);
				if (_configuration.DebugLog)
				{
					Debug.Log($"[Drunk] faint fade-in start drunk={_drunkennessStat.FullValue:0.###} " + $"fade={_configuration.FaintFadeInSeconds:0.#}s");
				}
			}
		}

		private void TickFadeIn(float dt)
		{
			float num = Mathf.Max(0.01f, _configuration.FaintFadeInSeconds);
			_phaseTimer += dt;
			float num2 = Mathf.Clamp01(_phaseTimer / num);
			float num3 = num2 * num2;
			SetBlackoutIntensity(num3 * _configuration.FaintBlackoutIntensity);
			if (!(num2 < 1f))
			{
				BeginPassOut();
			}
		}

		private void BeginPassOut()
		{
			if (!_playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				EndSequence(clearBlackout: true);
				return;
			}
			SetBlackoutIntensity(_configuration.FaintBlackoutIntensity);
			ragdoll.AddSimulationReason(RagdollSimulationReasonEnum.DrunkFaint);
			_phase = FaintPhase.PassedOut;
			_phaseTimer = 0f;
			if (_configuration.UseStaminaFocusPulse)
			{
				_vignetteUIEffectModel.PlayFocusAnimation(VignetteUIEffectType.Stamina);
			}
			if (_configuration.DebugLog)
			{
				Debug.Log($"[Drunk] faint fall drunk={_drunkennessStat.FullValue:0.###} " + $"duration={_configuration.FaintDurationSeconds:0.#}s");
			}
		}

		private void TickPassedOut(float dt)
		{
			_phaseTimer += dt;
			SetBlackoutIntensity(_configuration.FaintBlackoutIntensity);
			if (!(_phaseTimer < _configuration.FaintDurationSeconds))
			{
				EndSequence(clearBlackout: true);
			}
		}

		private void EndSequence(bool clearBlackout)
		{
			if (_phase == FaintPhase.PassedOut && _playersRagdollModel.TryGetPlayerRagdoll(_multiplayerModel.NetworkRunner.LocalPlayer.PlayerId, out var ragdoll))
			{
				ragdoll.RemoveSimulationReason(RagdollSimulationReasonEnum.DrunkFaint);
			}
			_phase = FaintPhase.Idle;
			_phaseTimer = 0f;
			if (clearBlackout)
			{
				SetBlackoutIntensity(0f);
			}
		}

		private void EnsureBlackoutEffects()
		{
			EnsurePpBlackoutEffect();
			if (_configuration != null && _configuration.UseStaminaUiVignette)
			{
				EnsureUiBlackoutEffect();
			}
		}

		private void EnsurePpBlackoutEffect()
		{
			if (_blackoutEffect == null && _postProcessingModel.ActiveVolumes.TryGetValue(PostProcessingType.SessionSceneMain, out var value) && value.profile.TryGet<Vignette>(out var _))
			{
				_blackoutEffect = new VignetteEffect
				{
					EffectColor = BlackoutColor,
					EffectIntensity = 0f
				};
				_postProcessingModel.ApplyVignetteEffect(value, _blackoutEffect);
			}
		}

		private void EnsureUiBlackoutEffect()
		{
			if (_staminaUiBlackoutEffect == null && _vignetteUIEffectModel.ActiveLayers.ContainsKey(VignetteUIEffectType.Stamina))
			{
				_staminaUiBlackoutEffect = new VignetteUIEffect
				{
					EffectIntensity = 0f
				};
				_vignetteUIEffectModel.ApplyVignetteEffect(VignetteUIEffectType.Stamina, _staminaUiBlackoutEffect);
			}
		}

		private void SetBlackoutIntensity(float intensity)
		{
			EnsureBlackoutEffects();
			float num = Mathf.Clamp01(intensity);
			if (_blackoutEffect != null)
			{
				_blackoutEffect.EffectIntensity = num;
			}
			if (_staminaUiBlackoutEffect != null)
			{
				_staminaUiBlackoutEffect.EffectIntensity = ((_configuration != null && _configuration.UseStaminaUiVignette) ? num : 0f);
			}
		}

		private void RemoveBlackoutEffects()
		{
			if (_blackoutEffect != null)
			{
				if (_postProcessingModel.ActiveVolumes.TryGetValue(PostProcessingType.SessionSceneMain, out var value))
				{
					_postProcessingModel.RemoveVignetteEffect(value, _blackoutEffect);
				}
				_blackoutEffect = null;
			}
			if (_staminaUiBlackoutEffect != null)
			{
				_vignetteUIEffectModel.RemoveVignetteEffect(VignetteUIEffectType.Stamina, _staminaUiBlackoutEffect);
				_staminaUiBlackoutEffect = null;
			}
		}
	}
}
