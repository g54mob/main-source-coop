using System;
using System.Collections;
using Ami.BroAudio;
using EvilCore.Audio;
using EvilCore.CustomPass;
using EvilCore.EvilPack.EvilLogger;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Player.Core;
using PrimeTween;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using VContainer;

namespace NomadDrive.Features.Player.Survival
{
	public class PlayerScreenFeedbackController : MonoBehaviour, IPlayerComponent
	{
		[SerializeField]
		private ScreenFeedbackConfig config;

		[SerializeField]
		[Min(0.05f)]
		[Tooltip("Seconds to smoothly clear ALL screen feedback (grayscale desaturation + red damage vignette + fatigue) when the player becomes a chicken. Restored on revive.")]
		private float deathFeedbackClearDuration = 0.8f;

		[Inject]
		private ICustomPassManager _customPassManager;

		[Inject]
		private IGameUIManager _gameUI;

		[Inject]
		private IAudioManager _audioManager;

		private PlayerStatsManager _statsManager;

		private bool _isInitialized;

		private bool _suppressed;

		private float _suppressWeight;

		private const string IntensityProp = "_Intensity";

		private const string PulseProp = "_PulseIntensity";

		private const string ColorProp = "_VignetteColor";

		private const string RadiusInnerProp = "_RadiusInner";

		private const string RadiusOuterProp = "_RadiusOuter";

		private float _currentIntensity;

		private float _intensityVelocity;

		private float _currentPulse;

		private Sequence _pulseSequence;

		private CanvasGroup _fatigueCanvasGroup;

		private FatiguePhase _phase;

		private Coroutine _blinkCoroutine;

		private Tween _fatigueFadeTween;

		private AudioHandle _heartbeatHandle;

		private PlayerFeedbackVolumeTag _volumeTag;

		private LensDistortion _lensDistortion;

		private ChromaticAberration _chromaticAberration;

		private ColorAdjustments _colorAdjustments;

		private float _lensBaseline;

		private float _chromaBaseline;

		private float _saturationBaseline;

		private float _blinkLensOffset;

		private float _blinkChromaOffset;

		private Sequence _blinkSpikeSequence;

		private float _stumbleLensOffset;

		private float _stumbleChromaOffset;

		private float _stumbleCooldownTimer;

		private Sequence _stumbleSequence;

		private float _postZeroRamp;

		private bool _nutritionArmed = true;

		private float _nutritionCueTimer;

		private bool _energyArmed = true;

		private float _energyCueTimer;

		public int SetupPriority => 25;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (!isLocalPlayer)
			{
				base.enabled = false;
				return;
			}
			if (config == null)
			{
				EvilLogger.LogError("[PlayerScreenFeedbackController] ScreenFeedbackConfig not assigned. Feedback disabled.", "SetupForPlayer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Survival\\PlayerScreenFeedbackController.cs", 101);
				base.enabled = false;
				return;
			}
			_statsManager = GetComponent<PlayerStatsManager>();
			if (_statsManager == null || !_statsManager.IsInitialized)
			{
				EvilLogger.LogError("[PlayerScreenFeedbackController] PlayerStatsManager missing or not initialized. Feedback disabled.", "SetupForPlayer", "A:\\Nomad Drive Folder\\NomadDrive\\Assets\\_Project\\Features\\Player\\Scripts\\Survival\\PlayerScreenFeedbackController.cs", 109);
				base.enabled = false;
				return;
			}
			_statsManager.OnHealthDamaged += OnHealthDamaged;
			ResolveFatigueCanvasGroup();
			ApplyVignetteStaticProperties();
			ResolveFeedbackVolume();
			_nutritionArmed = _statsManager.Nutrition.CurrentValue > _statsManager.Nutrition.CriticalLowThreshold;
			_energyArmed = _statsManager.Energy.CurrentValue > _statsManager.Energy.CriticalLowThreshold;
			_nutritionCueTimer = 0f;
			_energyCueTimer = 0f;
			_isInitialized = true;
		}

		public void SetFeedbackSuppressed(bool suppressed)
		{
			if (_suppressed == suppressed)
			{
				return;
			}
			_suppressed = suppressed;
			if (suppressed)
			{
				_pulseSequence.Stop();
				_stumbleSequence.Stop();
				_blinkSpikeSequence.Stop();
				_currentPulse = 0f;
				_stumbleLensOffset = 0f;
				_stumbleChromaOffset = 0f;
				_blinkLensOffset = 0f;
				_blinkChromaOffset = 0f;
				if (_phase != FatiguePhase.None)
				{
					TransitionToPhase(FatiguePhase.None);
				}
			}
		}

		private void ResolveFatigueCanvasGroup()
		{
			GameCanvasGroup[] array = UnityEngine.Object.FindObjectsByType<GameCanvasGroup>(FindObjectsInactive.Include, FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].GameCanvasGroupName == GameCanvasGroupName.FatigueOverlay)
				{
					_fatigueCanvasGroup = array[i].GetComponent<CanvasGroup>();
					if (_fatigueCanvasGroup == null)
					{
						_fatigueCanvasGroup = array[i].gameObject.AddComponent<CanvasGroup>();
					}
					_fatigueCanvasGroup.alpha = 0f;
					break;
				}
			}
		}

		private void ApplyVignetteStaticProperties()
		{
			if (_customPassManager != null && !string.IsNullOrEmpty(config.vignettePassId))
			{
				_customPassManager.GetProperties(config.vignettePassId).SetColor("_VignetteColor", config.vignetteColor).SetFloat("_RadiusInner", config.radiusInner)
					.SetFloat("_RadiusOuter", config.radiusOuter);
			}
		}

		private void ResolveFeedbackVolume()
		{
			_volumeTag = UnityEngine.Object.FindFirstObjectByType<PlayerFeedbackVolumeTag>(FindObjectsInactive.Include);
			if (_volumeTag == null)
			{
				return;
			}
			VolumeProfile profile = _volumeTag.Volume.profile;
			if (!(profile == null))
			{
				profile.TryGet<LensDistortion>(out _lensDistortion);
				profile.TryGet<ChromaticAberration>(out _chromaticAberration);
				profile.TryGet<ColorAdjustments>(out _colorAdjustments);
				if (_lensDistortion != null)
				{
					_lensBaseline = _lensDistortion.intensity.value;
				}
				if (_chromaticAberration != null)
				{
					_chromaBaseline = _chromaticAberration.intensity.value;
				}
				if (_colorAdjustments != null)
				{
					_saturationBaseline = _colorAdjustments.saturation.value;
				}
			}
		}

		private void OnDestroy()
		{
			if (_statsManager != null)
			{
				_statsManager.OnHealthDamaged -= OnHealthDamaged;
			}
			_pulseSequence.Stop();
			_fatigueFadeTween.Stop();
			_stumbleSequence.Stop();
			_blinkSpikeSequence.Stop();
			if (_blinkCoroutine != null)
			{
				StopCoroutine(_blinkCoroutine);
			}
			_blinkCoroutine = null;
			StopAudioLoop(ref _heartbeatHandle, 0.1f);
			if (_lensDistortion != null)
			{
				_lensDistortion.intensity.value = _lensBaseline;
			}
			if (_chromaticAberration != null)
			{
				_chromaticAberration.intensity.value = _chromaBaseline;
			}
			if (_colorAdjustments != null)
			{
				_colorAdjustments.saturation.value = _saturationBaseline;
			}
			if (_customPassManager != null && config != null && !string.IsNullOrEmpty(config.vignettePassId))
			{
				_customPassManager.GetProperties(config.vignettePassId).SetFloat("_Intensity", 0f).SetFloat("_PulseIntensity", 0f);
			}
		}

		private void Update()
		{
			if (!_isInitialized)
			{
				return;
			}
			if (_suppressed)
			{
				if (_phase != FatiguePhase.None)
				{
					TransitionToPhase(FatiguePhase.None);
				}
			}
			else
			{
				SyncFatiguePhaseFromEnergy();
				SyncStatCriticalCue(_statsManager.Nutrition, config.nutritionCriticalEnterSound, config.nutritionCriticalRepeatIntervalSeconds, ref _nutritionArmed, ref _nutritionCueTimer);
				SyncStatCriticalCue(_statsManager.Energy, config.energyCriticalEnterSound, config.energyCriticalRepeatIntervalSeconds, ref _energyArmed, ref _energyCueTimer);
				UpdatePostZeroRamp();
				TickStumble();
			}
		}

		private void SyncStatCriticalCue(PlayerStat stat, SoundID sound, float repeatIntervalSeconds, ref bool armed, ref float timer)
		{
			float currentValue = stat.CurrentValue;
			float criticalLowThreshold = stat.CriticalLowThreshold;
			bool num = currentValue > criticalLowThreshold;
			bool flag = _statsManager.EffectiveHealth > 0f;
			if (num)
			{
				if (!armed)
				{
					armed = true;
					timer = 0f;
				}
			}
			else if (armed)
			{
				if (flag)
				{
					PlayCue(sound);
				}
				armed = false;
				timer = repeatIntervalSeconds;
			}
			else
			{
				if (repeatIntervalSeconds <= 0f)
				{
					return;
				}
				timer -= Time.deltaTime;
				if (!(timer > 0f))
				{
					if (flag)
					{
						PlayCue(sound);
					}
					timer = repeatIntervalSeconds;
				}
			}
		}

		private void PlayCue(SoundID sound)
		{
			if (_audioManager != null && sound.IsValid())
			{
				_audioManager.PlayOneShotUI(sound);
			}
		}

		private void SyncFatiguePhaseFromEnergy()
		{
			float currentValue = _statsManager.Energy.CurrentValue;
			float criticalLowThreshold = _statsManager.Energy.CriticalLowThreshold;
			FatiguePhase fatiguePhase = ((currentValue <= 0f) ? FatiguePhase.Zero : ((currentValue <= criticalLowThreshold) ? FatiguePhase.Critical : FatiguePhase.None));
			if (fatiguePhase != _phase)
			{
				TransitionToPhase(fatiguePhase);
			}
		}

		private void UpdatePostZeroRamp()
		{
			float deltaTime = Time.deltaTime;
			if (_statsManager.Energy.CurrentValue <= 0f)
			{
				float num = Mathf.Max(0.01f, config.postZeroEscalationDuration);
				_postZeroRamp = Mathf.Clamp01(_postZeroRamp + deltaTime / num);
			}
			else if (_postZeroRamp > 0f)
			{
				float num2 = Mathf.Max(0.01f, config.postZeroRecoveryDuration);
				_postZeroRamp = Mathf.Clamp01(_postZeroRamp - deltaTime / num2);
			}
		}

		private float GetPostZeroEscalation()
		{
			if (_postZeroRamp <= 0.001f)
			{
				return 0f;
			}
			if (config.postZeroEscalationCurve == null)
			{
				return _postZeroRamp;
			}
			return Mathf.Clamp01(config.postZeroEscalationCurve.Evaluate(_postZeroRamp));
		}

		private void LateUpdate()
		{
			if (!_isInitialized)
			{
				return;
			}
			_suppressWeight = Mathf.MoveTowards(_suppressWeight, _suppressed ? 1f : 0f, Time.deltaTime / Mathf.Max(0.05f, deathFeedbackClearDuration));
			float num = 1f - _suppressWeight;
			float num2 = ((config.criticalHealthThreshold > 0f) ? Mathf.Clamp01((config.criticalHealthThreshold - _statsManager.EffectiveHealth) / config.criticalHealthThreshold) : 0f);
			float energyDepletion = 1f - _statsManager.Energy.GetValueRatio();
			float target = num2 * config.vignetteByDamageRatio.Evaluate(1f);
			_currentIntensity = Mathf.SmoothDamp(_currentIntensity, target, ref _intensityVelocity, config.vignetteSmoothTime);
			if (_customPassManager != null && !string.IsNullOrEmpty(config.vignettePassId))
			{
				_customPassManager.GetProperties(config.vignettePassId).SetFloat("_Intensity", _currentIntensity * num).SetFloat("_PulseIntensity", _currentPulse * num);
			}
			ApplyVolumeModulation(energyDepletion, num2);
			if (_suppressWeight > 0f)
			{
				if (_lensDistortion != null)
				{
					_lensDistortion.intensity.value = Mathf.Lerp(_lensDistortion.intensity.value, _lensBaseline, _suppressWeight);
				}
				if (_chromaticAberration != null)
				{
					_chromaticAberration.intensity.value = Mathf.Lerp(_chromaticAberration.intensity.value, _chromaBaseline, _suppressWeight);
				}
				if (_colorAdjustments != null)
				{
					_colorAdjustments.saturation.value = Mathf.Lerp(_colorAdjustments.saturation.value, _saturationBaseline, _suppressWeight);
				}
			}
			float targetVolume = num2 * config.heartbeatVolumeByDamageRatio.Evaluate(1f) * num;
			DriveAudioLoop(ref _heartbeatHandle, config.heartbeatLoopSound, targetVolume, config.heartbeatFadeDuration);
		}

		private void ApplyVolumeModulation(float energyDepletion, float criticalSeverity)
		{
			if (_volumeTag == null)
			{
				return;
			}
			float num = ((config.saturationByHealthDamageRatio != null) ? (criticalSeverity * config.saturationByHealthDamageRatio.Evaluate(1f)) : 0f);
			float num2 = ((config.chromaticAberrationByHealthDamageRatio != null) ? (criticalSeverity * config.chromaticAberrationByHealthDamageRatio.Evaluate(1f)) : 0f);
			if (energyDepletion <= 0.001f && _postZeroRamp <= 0.001f && !_stumbleSequence.isAlive && !_blinkSpikeSequence.isAlive)
			{
				_stumbleLensOffset = 0f;
				_stumbleChromaOffset = 0f;
				_blinkLensOffset = 0f;
				_blinkChromaOffset = 0f;
				_postZeroRamp = 0f;
				if (_lensDistortion != null)
				{
					_lensDistortion.intensity.overrideState = true;
					_lensDistortion.intensity.value = _lensBaseline;
				}
				if (_chromaticAberration != null)
				{
					_chromaticAberration.intensity.overrideState = true;
					_chromaticAberration.intensity.value = Mathf.Clamp01(_chromaBaseline + num2);
				}
				if (_colorAdjustments != null)
				{
					_colorAdjustments.saturation.overrideState = true;
					_colorAdjustments.saturation.value = Mathf.Clamp(_saturationBaseline + num, -100f, 100f);
				}
				return;
			}
			float num3 = Mathf.Sin(Time.time * 2f * (float)Math.PI * Mathf.Max(0.01f, config.breathSyncFrequency));
			float postZeroEscalation = GetPostZeroEscalation();
			float num4 = Mathf.Lerp(1f, config.postZeroBreathAmplitudeMultiplier, postZeroEscalation);
			if (_lensDistortion != null)
			{
				float num5 = config.lensDistortionByEnergyDepletion.Evaluate(energyDepletion);
				float num6 = config.postZeroLensDistortionPeak * postZeroEscalation;
				float num7 = num3 * config.lensDistortionBreathAmplitude * energyDepletion * num4;
				_lensDistortion.intensity.overrideState = true;
				_lensDistortion.intensity.value = Mathf.Clamp(_lensBaseline + num5 + num6 + num7 + _blinkLensOffset + _stumbleLensOffset, -1f, 1f);
			}
			if (_chromaticAberration != null)
			{
				float num8 = config.chromaticAberrationByEnergyDepletion.Evaluate(energyDepletion);
				float num9 = config.postZeroChromaticAberrationPeak * postZeroEscalation;
				float num10 = num3 * config.chromaticAberrationBreathAmplitude * energyDepletion * num4;
				_chromaticAberration.intensity.overrideState = true;
				_chromaticAberration.intensity.value = Mathf.Clamp01(_chromaBaseline + num8 + num9 + num10 + num2 + _blinkChromaOffset + _stumbleChromaOffset);
			}
			if (_colorAdjustments != null)
			{
				float num11 = config.saturationByEnergyDepletion.Evaluate(energyDepletion);
				float num12 = config.postZeroSaturationPeak * postZeroEscalation;
				_colorAdjustments.saturation.overrideState = true;
				_colorAdjustments.saturation.value = Mathf.Clamp(_saturationBaseline + num11 + num12 + num, -100f, 100f);
			}
		}

		private void TickStumble()
		{
			if (_stumbleCooldownTimer > 0f)
			{
				_stumbleCooldownTimer -= Time.deltaTime;
			}
			if (!_stumbleSequence.isAlive && !(_stumbleCooldownTimer > 0f))
			{
				float time = 1f - _statsManager.Energy.GetValueRatio();
				float num = config.stumbleChanceByEnergyDepletion.Evaluate(time);
				float postZeroEscalation = GetPostZeroEscalation();
				num *= Mathf.Lerp(1f, config.postZeroStumbleChanceMultiplier, postZeroEscalation);
				if (!(num <= 0f) && UnityEngine.Random.value < num * Time.deltaTime)
				{
					StartStumble();
				}
			}
		}

		private void StartStumble()
		{
			_stumbleSequence.Stop();
			_stumbleLensOffset = 0f;
			_stumbleChromaOffset = 0f;
			_stumbleCooldownTimer = config.stumbleMinCooldown;
			float duration = Mathf.Max(0.01f, config.stumbleDuration * 0.5f);
			_stumbleSequence = Sequence.Create().Chain(Tween.Custom(0f, config.stumbleLensDistortionSpike, duration, delegate(float v)
			{
				_stumbleLensOffset = v;
			}, config.stumbleEase)).Group(Tween.Custom(0f, config.stumbleChromaticAberrationSpike, duration, delegate(float v)
			{
				_stumbleChromaOffset = v;
			}, config.stumbleEase))
				.Chain(Tween.Custom(config.stumbleLensDistortionSpike, 0f, duration, delegate(float v)
				{
					_stumbleLensOffset = v;
				}, config.stumbleEase))
				.Group(Tween.Custom(config.stumbleChromaticAberrationSpike, 0f, duration, delegate(float v)
				{
					_stumbleChromaOffset = v;
				}, config.stumbleEase));
		}

		private void OnHealthDamaged()
		{
			_pulseSequence.Stop();
			_currentPulse = 0f;
			float duration = Mathf.Max(0.01f, config.pulseDuration * 0.5f);
			_pulseSequence = Sequence.Create().Chain(Tween.Custom(0f, config.pulseAmplitude, duration, delegate(float v)
			{
				_currentPulse = v;
			}, config.pulseEase)).Chain(Tween.Custom(config.pulseAmplitude, 0f, duration, delegate(float v)
			{
				_currentPulse = v;
			}, config.pulseEase));
		}

		private void TransitionToPhase(FatiguePhase next)
		{
			if (_phase == next)
			{
				return;
			}
			_phase = next;
			if (next == FatiguePhase.None)
			{
				if (_blinkCoroutine != null)
				{
					StopCoroutine(_blinkCoroutine);
					_blinkCoroutine = null;
				}
				if (_fatigueCanvasGroup != null)
				{
					_fatigueFadeTween.Stop();
					_fatigueFadeTween = Tween.Alpha(_fatigueCanvasGroup, 0f, config.fatigueFadeOutDuration);
				}
				_blinkSpikeSequence.Stop();
				_blinkLensOffset = 0f;
				_blinkChromaOffset = 0f;
				_gameUI?.HideCanvasGroup(GameCanvasGroupName.FatigueOverlay);
			}
			else if (!(_fatigueCanvasGroup == null))
			{
				_gameUI?.ShowCanvasGroup(GameCanvasGroupName.FatigueOverlay, interactable: false, blockRaycast: false);
				_fatigueCanvasGroup.alpha = 0f;
				if (_blinkCoroutine == null)
				{
					_blinkCoroutine = StartCoroutine(BlinkLoop());
				}
			}
		}

		private IEnumerator BlinkLoop()
		{
			while (_phase != FatiguePhase.None)
			{
				float seconds = ((_phase == FatiguePhase.Zero) ? UnityEngine.Random.Range(config.zeroIntervalRange.x, config.zeroIntervalRange.y) : UnityEngine.Random.Range(config.criticalIntervalRange.x, config.criticalIntervalRange.y));
				yield return new WaitForSeconds(seconds);
				if (_phase == FatiguePhase.None)
				{
					yield break;
				}
				float num = ((_phase == FatiguePhase.Zero) ? config.zeroBlinkDuration : config.criticalBlinkDuration);
				float endValue = ((_phase == FatiguePhase.Zero) ? config.zeroBlinkAlpha : config.criticalBlinkAlpha);
				float hold = ((_phase == FatiguePhase.Zero) ? config.zeroBlinkHoldAtClosed : 0f);
				Ease ease = ((_phase == FatiguePhase.Zero) ? config.zeroBlinkEase : config.criticalBlinkEase);
				float halfDur = Mathf.Max(0.01f, num * 0.5f);
				_fatigueFadeTween.Stop();
				_fatigueFadeTween = Tween.Alpha(_fatigueCanvasGroup, endValue, halfDur, ease);
				StartBlinkSpike(rampIn: true);
				yield return new WaitForSeconds(halfDur);
				if (_phase == FatiguePhase.None)
				{
					yield break;
				}
				if (hold > 0f)
				{
					yield return new WaitForSeconds(hold);
				}
				if (_phase == FatiguePhase.None)
				{
					yield break;
				}
				_fatigueFadeTween.Stop();
				_fatigueFadeTween = Tween.Alpha(_fatigueCanvasGroup, 0f, halfDur, ease);
				StartBlinkSpike(rampIn: false);
				yield return new WaitForSeconds(halfDur);
			}
			_blinkCoroutine = null;
		}

		private void StartBlinkSpike(bool rampIn)
		{
			_blinkSpikeSequence.Stop();
			float duration = Mathf.Max(0.01f, config.blinkSpikeDuration);
			float endValue = (rampIn ? config.blinkLensDistortionSpike : 0f);
			float endValue2 = (rampIn ? config.blinkChromaticAberrationSpike : 0f);
			float blinkLensOffset = _blinkLensOffset;
			float blinkChromaOffset = _blinkChromaOffset;
			_blinkSpikeSequence = Sequence.Create().Chain(Tween.Custom(blinkLensOffset, endValue, duration, delegate(float v)
			{
				_blinkLensOffset = v;
			}, config.blinkSpikeEase)).Group(Tween.Custom(blinkChromaOffset, endValue2, duration, delegate(float v)
			{
				_blinkChromaOffset = v;
			}, config.blinkSpikeEase));
		}

		private void DriveAudioLoop(ref AudioHandle handle, SoundID id, float targetVolume, float fadeDuration)
		{
			if (_audioManager == null || !id.IsValid())
			{
				return;
			}
			if (targetVolume > 0.001f)
			{
				if (!handle.IsValid)
				{
					handle = _audioManager.PlayEventAttached(id, base.gameObject);
					if (handle.IsValid)
					{
						_audioManager.SetParameter(handle, "volume", 0f);
					}
				}
				if (handle.IsValid)
				{
					_audioManager.FadeVolume(handle, targetVolume, fadeDuration);
				}
			}
			else if (handle.IsValid)
			{
				_audioManager.StopEvent(handle, AudioStopMode.AllowFadeout, fadeDuration);
				handle = AudioHandle.Invalid;
			}
		}

		private void StopAudioLoop(ref AudioHandle handle, float fade)
		{
			if (handle.IsValid && _audioManager != null)
			{
				_audioManager.StopEvent(handle, AudioStopMode.AllowFadeout, fade);
				handle = AudioHandle.Invalid;
			}
		}
	}
}
