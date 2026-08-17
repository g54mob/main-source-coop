using System;
using Ami.BroAudio;
using Coffee.UIEffects;
using EvilCore.Audio;
using NomadDrive.Features.Objectives;
using NomadDrive.Features.Player.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace NomadDrive.Features.Player.UI
{
	public class PeakStatusBarDisplayer : MonoBehaviour
	{
		private struct UIEffectScaleRef
		{
			public UIEffect Effect;

			public RectTransform Rect;

			public Vector2 OrigDetailScale;

			public Vector2 OrigTransitionScale;
		}

		[Header("Zone RectTransforms")]
		[SerializeField]
		private RectTransform healthZoneRect;

		[SerializeField]
		private RectTransform energyZoneRect;

		[SerializeField]
		private RectTransform hydrationZoneRect;

		[SerializeField]
		private RectTransform nutritionZoneRect;

		[SerializeField]
		private RectTransform damageZoneRect;

		[SerializeField]
		private RectTransform poisonZoneRect;

		[Header("Zone Images")]
		[SerializeField]
		private Image healthZoneImage;

		[SerializeField]
		private Image energyZoneImage;

		[SerializeField]
		private Image hydrationZoneImage;

		[SerializeField]
		private Image nutritionZoneImage;

		[SerializeField]
		private Image damageZoneImage;

		[SerializeField]
		private Image poisonZoneImage;

		[Header("Zone UIEffects (pattern)")]
		[SerializeField]
		private UIEffect healthZoneEffect;

		[SerializeField]
		private UIEffect energyZoneEffect;

		[SerializeField]
		private UIEffect hydrationZoneEffect;

		[SerializeField]
		private UIEffect nutritionZoneEffect;

		[SerializeField]
		private UIEffect damageZoneEffect;

		[SerializeField]
		private UIEffect poisonZoneEffect;

		[Header("Zone Icons")]
		[SerializeField]
		private Image energyIconImage;

		[SerializeField]
		private Image hydrationIconImage;

		[SerializeField]
		private Image nutritionIconImage;

		[SerializeField]
		private Image damageIconImage;

		[SerializeField]
		private Image poisonIconImage;

		[Header("Text")]
		[SerializeField]
		private TMP_Text effectiveHealthText;

		[Header("Config")]
		[SerializeField]
		private PlayerStatusBarConfig config;

		[Header("Downed (Chicken Form)")]
		[Tooltip("Optional centered icon shown while the player is a chicken (sprite comes from the config). Leave null to skip.")]
		[SerializeField]
		private Image downedFormIconImage;

		[Header("Audio")]
		[SerializeField]
		private SoundID zoneAppearSound;

		[SerializeField]
		private SoundID zoneDisappearSound;

		[Header("Animation")]
		[SerializeField]
		[Min(0.01f)]
		private float iconShowThreshold = 0.05f;

		[Header("Zone Spacing")]
		[SerializeField]
		[Min(0f)]
		private float zoneGapPixels = 2f;

		[Inject]
		private IAudioManager _audioManager;

		private PlayerStatsManager _statsManager;

		private float _displayedNutrition;

		private float _displayedHydration;

		private float _displayedEnergy;

		private float _displayedDamage;

		private float _displayedPoison;

		private float _velNutrition;

		private float _velHydration;

		private float _velEnergy;

		private float _velDamage;

		private float _velPoison;

		private float _pulsePhase;

		private Color _healthZoneOrigColor;

		private bool _prevEnergyAppearing;

		private bool _prevHydrationAppearing;

		private bool _prevNutritionAppearing;

		private bool _prevDamageAppearing;

		private bool _prevPoisonAppearing;

		private float _barWidth;

		private UIEffectScaleRef[] _effectScaleRefs;

		private bool _downedForm;

		private float _downedBlend;

		public bool IsInitialized { get; private set; }

		public void SetDownedForm(bool active)
		{
			_downedForm = active;
			if (active && downedFormIconImage != null && config != null && config.downedFormIcon != null)
			{
				downedFormIconImage.sprite = config.downedFormIcon;
			}
		}

		public void Init(PlayerStatsManager statsManager)
		{
			_statsManager = statsManager;
			if (!(config == null))
			{
				_healthZoneOrigColor = ((healthZoneImage != null) ? healthZoneImage.color : Color.green);
				ZoneSizes zoneSizes = _statsManager.GetZoneSizes();
				_displayedNutrition = zoneSizes.Nutrition;
				_displayedHydration = zoneSizes.Hydration;
				_displayedEnergy = zoneSizes.Energy;
				_displayedDamage = zoneSizes.Damage;
				_displayedPoison = zoneSizes.Poison;
				float zoneAppearThreshold = config.zoneAppearThreshold;
				float num = Mathf.Max(config.damageZoneAppearThreshold, 0.001f);
				float num2 = Mathf.Max(config.poisonZoneAppearThreshold, 0.001f);
				_prevEnergyAppearing = zoneSizes.Energy >= zoneAppearThreshold;
				_prevHydrationAppearing = zoneSizes.Hydration >= zoneAppearThreshold;
				_prevNutritionAppearing = zoneSizes.Nutrition >= zoneAppearThreshold;
				_prevDamageAppearing = zoneSizes.Damage >= num;
				_prevPoisonAppearing = zoneSizes.Poison >= num2;
				_barWidth = GetBarWidth();
				CaptureEffectScaleRefs();
				ApplyZoneAnchors();
				UpdateEffectiveHealthText();
				IsInitialized = true;
			}
		}

		private void Update()
		{
			if (IsInitialized)
			{
				_downedBlend = Mathf.MoveTowards(_downedBlend, _downedForm ? 1f : 0f, Time.deltaTime / Mathf.Max(0.01f, config.downedFormBlendDuration));
				ZoneSizes zoneSizes = _statsManager.GetZoneSizes();
				if (_downedForm)
				{
					zoneSizes.Nutrition = 0f;
					zoneSizes.Hydration = 0f;
					zoneSizes.Energy = 0f;
					zoneSizes.Damage = 0f;
					zoneSizes.Poison = 0f;
					zoneSizes.EffectiveHealth = 1f;
				}
				float zoneSmoothTime = config.zoneSmoothTime;
				float zoneSnapEpsilon = config.zoneSnapEpsilon;
				float zoneAppearThreshold = config.zoneAppearThreshold;
				float damageZoneAppearThreshold = config.damageZoneAppearThreshold;
				float poisonZoneAppearThreshold = config.poisonZoneAppearThreshold;
				float zoneStepSize = config.zoneStepSize;
				SmoothZone(ref _displayedNutrition, zoneSizes.Nutrition, ref _velNutrition, zoneSmoothTime, zoneSnapEpsilon, zoneAppearThreshold, zoneStepSize);
				SmoothZone(ref _displayedHydration, zoneSizes.Hydration, ref _velHydration, zoneSmoothTime, zoneSnapEpsilon, zoneAppearThreshold, zoneStepSize);
				SmoothZone(ref _displayedEnergy, zoneSizes.Energy, ref _velEnergy, zoneSmoothTime, zoneSnapEpsilon, zoneAppearThreshold, zoneStepSize);
				SmoothZone(ref _displayedDamage, zoneSizes.Damage, ref _velDamage, zoneSmoothTime, zoneSnapEpsilon, damageZoneAppearThreshold, zoneStepSize);
				SmoothZone(ref _displayedPoison, zoneSizes.Poison, ref _velPoison, zoneSmoothTime, zoneSnapEpsilon, poisonZoneAppearThreshold, zoneStepSize);
				if (!_downedForm && _downedBlend <= 0f)
				{
					CheckZoneAppearSound(zoneSizes, zoneAppearThreshold, damageZoneAppearThreshold, poisonZoneAppearThreshold);
				}
				ApplyZoneAnchors();
				UpdatePatternScales();
				UpdateZoneIconVisibility(zoneSizes, zoneAppearThreshold, damageZoneAppearThreshold, poisonZoneAppearThreshold);
				ApplyCriticalPulse(zoneSizes.EffectiveHealth);
				ApplyDownedFormOverlay();
				UpdateEffectiveHealthText();
			}
		}

		private void ApplyDownedFormOverlay()
		{
			if (_downedBlend <= 0f)
			{
				if (downedFormIconImage != null && downedFormIconImage.enabled)
				{
					downedFormIconImage.enabled = false;
				}
				return;
			}
			if (healthZoneImage != null)
			{
				healthZoneImage.color = Color.Lerp(healthZoneImage.color, config.downedFormColor, _downedBlend);
			}
			if (downedFormIconImage != null)
			{
				downedFormIconImage.enabled = config.downedFormIcon != null;
				Color color = downedFormIconImage.color;
				color.a = _downedBlend;
				downedFormIconImage.color = color;
			}
		}

		private static void SmoothZone(ref float displayed, float target, ref float velocity, float smoothTime, float epsilon, float appearThreshold, float stepSize)
		{
			float num = ((target >= appearThreshold) ? target : 0f);
			num = Mathf.Round(num / stepSize) * stepSize;
			if (Mathf.Abs(displayed - num) < epsilon)
			{
				displayed = num;
				velocity = 0f;
			}
			else
			{
				displayed = Mathf.SmoothDamp(displayed, num, ref velocity, smoothTime);
			}
		}

		private void CheckZoneAppearSound(ZoneSizes target, float threshold, float damageThreshold, float poisonThreshold)
		{
			bool flag = target.Energy >= threshold;
			bool flag2 = target.Hydration >= threshold;
			bool flag3 = target.Nutrition >= threshold;
			bool flag4 = target.Damage >= Mathf.Max(damageThreshold, 0.001f);
			bool flag5 = target.Poison >= Mathf.Max(poisonThreshold, 0.001f);
			bool flag6 = (flag && !_prevEnergyAppearing) || (flag2 && !_prevHydrationAppearing) || (flag3 && !_prevNutritionAppearing) || (flag4 && !_prevDamageAppearing) || (flag5 && !_prevPoisonAppearing);
			bool num = (!flag && _prevEnergyAppearing) || (!flag2 && _prevHydrationAppearing) || (!flag3 && _prevNutritionAppearing) || (!flag4 && _prevDamageAppearing) || (!flag5 && _prevPoisonAppearing);
			if (flag6 && zoneAppearSound.IsValid())
			{
				_audioManager?.PlayOneShotUI(zoneAppearSound);
			}
			if (num && zoneDisappearSound.IsValid())
			{
				_audioManager?.PlayOneShotUI(zoneDisappearSound);
			}
			if (flag3 && !_prevNutritionAppearing)
			{
				ObjectivesEventBus.Raise(ObjectiveSignal.NutritionZoneShown, this);
			}
			if (flag2 && !_prevHydrationAppearing)
			{
				ObjectivesEventBus.Raise(ObjectiveSignal.HydrationZoneShown, this);
			}
			if (flag4 && !_prevDamageAppearing)
			{
				ObjectivesEventBus.Raise(ObjectiveSignal.HealthZoneShown, this);
			}
			if (flag5 && !_prevPoisonAppearing)
			{
				ObjectivesEventBus.Raise(ObjectiveSignal.PoisonZoneShown, this);
			}
			_prevEnergyAppearing = flag;
			_prevHydrationAppearing = flag2;
			_prevNutritionAppearing = flag3;
			_prevDamageAppearing = flag4;
			_prevPoisonAppearing = flag5;
		}

		private void ApplyZoneAnchors()
		{
			float num = ((_barWidth > 0f) ? (zoneGapPixels / _barWidth) : 0f) * 0.5f;
			float num2 = _displayedEnergy + _displayedHydration + _displayedNutrition + _displayedDamage + _displayedPoison;
			float num3 = Mathf.Clamp01(1f - num2);
			float num4 = num3 + _displayedEnergy;
			float num5 = num4 + _displayedHydration;
			float num6 = num5 + _displayedNutrition;
			float num7 = num6 + _displayedDamage;
			bool num8 = _displayedEnergy >= 0.001f;
			bool flag = _displayedHydration >= 0.001f;
			bool flag2 = _displayedNutrition >= 0.001f;
			bool flag3 = _displayedDamage >= 0.001f;
			bool flag4 = _displayedPoison >= 0.001f;
			float num9 = ((num8 || flag || flag2 || flag3 || flag4) ? num : 0f);
			float num10 = (num8 ? num : 0f);
			float num11 = ((num8 && (flag || flag2 || flag3 || flag4)) ? num : 0f);
			float num12 = (flag ? num : 0f);
			float num13 = ((flag && (flag2 || flag3 || flag4)) ? num : 0f);
			float num14 = (flag2 ? num : 0f);
			float num15 = ((flag2 && (flag3 || flag4)) ? num : 0f);
			float num16 = (flag3 ? num : 0f);
			float num17 = ((flag3 && flag4) ? num : 0f);
			float num18 = (flag4 ? num : 0f);
			SetZoneAnchors(healthZoneRect, 0f, num3 - num9);
			SetZoneAnchors(energyZoneRect, num3 + num10, num4 - num11, _displayedEnergy);
			SetZoneAnchors(hydrationZoneRect, num4 + num12, num5 - num13, _displayedHydration);
			SetZoneAnchors(nutritionZoneRect, num5 + num14, num6 - num15, _displayedNutrition);
			SetZoneAnchors(damageZoneRect, num6 + num16, num7 - num17, _displayedDamage);
			SetZoneAnchors(poisonZoneRect, num7 + num18, 1f, _displayedPoison);
		}

		private static void SetZoneAnchors(RectTransform rect, float minX, float maxX, float zoneSize = -1f)
		{
			if (!(rect == null))
			{
				if (zoneSize >= 0f && zoneSize < 0.001f)
				{
					rect.anchorMin = new Vector2(minX, 0f);
					rect.anchorMax = new Vector2(minX, 1f);
				}
				else
				{
					rect.anchorMin = new Vector2(Mathf.Max(0f, minX), 0f);
					rect.anchorMax = new Vector2(Mathf.Min(1f, maxX), 1f);
				}
				rect.offsetMin = Vector2.zero;
				rect.offsetMax = Vector2.zero;
			}
		}

		private void UpdateZoneIconVisibility(ZoneSizes target, float appearThreshold, float damageThreshold, float poisonThreshold)
		{
			SetIconVisible(energyIconImage, _displayedEnergy, target.Energy, appearThreshold);
			SetIconVisible(hydrationIconImage, _displayedHydration, target.Hydration, appearThreshold);
			SetIconVisible(nutritionIconImage, _displayedNutrition, target.Nutrition, appearThreshold);
			SetIconVisible(damageIconImage, _displayedDamage, target.Damage, damageThreshold);
			SetIconVisible(poisonIconImage, _displayedPoison, target.Poison, poisonThreshold);
		}

		private void SetIconVisible(Image icon, float displayed, float target, float appearThreshold)
		{
			if (!(icon == null))
			{
				bool flag = target < appearThreshold;
				icon.enabled = !flag && displayed >= iconShowThreshold;
			}
		}

		private void ApplyCriticalPulse(float effectiveHealth)
		{
			if (!(healthZoneImage == null))
			{
				float criticalEffectiveHealthThreshold = config.criticalEffectiveHealthThreshold;
				if (effectiveHealth > criticalEffectiveHealthThreshold)
				{
					_pulsePhase = 0f;
					healthZoneImage.color = _healthZoneOrigColor;
					return;
				}
				float t = ((criticalEffectiveHealthThreshold > 0f) ? (1f - Mathf.Clamp01(effectiveHealth / criticalEffectiveHealthThreshold)) : 0f);
				float num = Mathf.Lerp(config.criticalPulseDurationMax, config.criticalPulseDurationMin, t);
				_pulsePhase = Mathf.Repeat(_pulsePhase + Time.deltaTime / (2f * num), 1f);
				float t2 = 0.5f - 0.5f * Mathf.Cos(_pulsePhase * 2f * (float)Math.PI);
				healthZoneImage.color = Color.Lerp(_healthZoneOrigColor, config.criticalPulseColor, t2);
			}
		}

		private void UpdateEffectiveHealthText()
		{
			if (!(effectiveHealthText == null))
			{
				float num = _statsManager.EffectiveHealth * 100f;
				effectiveHealthText.text = ((num < 10f) ? num.ToString("F1") : Mathf.RoundToInt(num).ToString());
				effectiveHealthText.alpha = 1f - _downedBlend;
			}
		}

		private float GetBarWidth()
		{
			RectTransform component = GetComponent<RectTransform>();
			if (!(component != null))
			{
				return 1f;
			}
			return component.rect.width;
		}

		private void CaptureEffectScaleRefs()
		{
			UIEffect[] array = new UIEffect[6] { healthZoneEffect, energyZoneEffect, hydrationZoneEffect, nutritionZoneEffect, damageZoneEffect, poisonZoneEffect };
			RectTransform[] array2 = new RectTransform[6] { healthZoneRect, energyZoneRect, hydrationZoneRect, nutritionZoneRect, damageZoneRect, poisonZoneRect };
			_effectScaleRefs = new UIEffectScaleRef[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				_effectScaleRefs[i] = new UIEffectScaleRef
				{
					Effect = array[i],
					Rect = array2[i],
					OrigDetailScale = ((array[i] != null) ? array[i].detailTextureScale : Vector2.one),
					OrigTransitionScale = ((array[i] != null) ? array[i].transitionTextureScale : Vector2.one)
				};
			}
		}

		private void UpdatePatternScales()
		{
			if (_effectScaleRefs == null || _barWidth <= 0f)
			{
				return;
			}
			for (int i = 0; i < _effectScaleRefs.Length; i++)
			{
				ref UIEffectScaleRef reference = ref _effectScaleRefs[i];
				if (reference.Effect == null || reference.Rect == null)
				{
					continue;
				}
				float width = reference.Rect.rect.width;
				if (!(width <= 0f))
				{
					float num = width / _barWidth;
					Vector2 vector = new Vector2(reference.OrigDetailScale.x * num, reference.OrigDetailScale.y);
					Vector2 vector2 = new Vector2(reference.OrigTransitionScale.x * num, reference.OrigTransitionScale.y);
					if ((vector - reference.Effect.detailTextureScale).sqrMagnitude > 1E-06f)
					{
						reference.Effect.detailTextureScale = vector;
					}
					if ((vector2 - reference.Effect.transitionTextureScale).sqrMagnitude > 1E-06f)
					{
						reference.Effect.transitionTextureScale = vector2;
					}
				}
			}
		}
	}
}
