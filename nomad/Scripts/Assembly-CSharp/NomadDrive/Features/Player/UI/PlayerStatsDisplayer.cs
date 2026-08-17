using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NomadDrive.Features.Player.UI
{
	public class PlayerStatsDisplayer : MonoBehaviour
	{
		[Header("Pulse Images")]
		[SerializeField]
		private List<Image> nutritionPulseImages = new List<Image>();

		[SerializeField]
		private List<Image> hydrationPulseImages = new List<Image>();

		[SerializeField]
		private List<Image> energyPulseImages = new List<Image>();

		[SerializeField]
		private List<Image> healthPulseImages = new List<Image>();

		[Header("Stat Icons")]
		[SerializeField]
		private Transform nutritionIcon;

		[SerializeField]
		private Transform hydrationIcon;

		[SerializeField]
		private Transform energyIcon;

		[SerializeField]
		private Transform healthIcon;

		[Header("Fill Images")]
		[SerializeField]
		private Image nutritionFillImage;

		[SerializeField]
		private Image hydrationFillImage;

		[SerializeField]
		private Image energyFillImage;

		[SerializeField]
		private Image healthFillImage;

		[Header("Value Texts")]
		[SerializeField]
		private TMP_Text nutritionValueText;

		[SerializeField]
		private TMP_Text hydrationValueText;

		[SerializeField]
		private TMP_Text energyValueText;

		[SerializeField]
		private TMP_Text healthValueText;

		[Header("Value Flow")]
		[SerializeField]
		[Min(0.01f)]
		private float valueSmoothTime = 0.5f;

		[SerializeField]
		[Min(0.001f)]
		private float valueSnapEpsilon = 0.05f;

		[Header("Critical Pulse")]
		[SerializeField]
		private Color pulseColorMild = new Color(1f, 0.5f, 0.5f, 1f);

		[SerializeField]
		private Color pulseColorSevere = new Color(0.8f, 0f, 0f, 1f);

		[SerializeField]
		[Min(0.05f)]
		private float pulseDurationMax = 0.9f;

		[SerializeField]
		[Min(0.05f)]
		private float pulseDurationMin = 0.18f;

		[Header("Icon Breathing")]
		[SerializeField]
		[Min(1f)]
		private float iconBreathPeakMild = 1.05f;

		[SerializeField]
		[Min(1f)]
		private float iconBreathPeakSevere = 1.25f;

		private PlayerStatsManager _playerStatsManager;

		private Color[] _origColorsNutrition;

		private Color[] _origColorsHydration;

		private Color[] _origColorsEnergy;

		private Color[] _origColorsHealth;

		private Vector3 _origScaleIconNutrition = Vector3.one;

		private Vector3 _origScaleIconHydration = Vector3.one;

		private Vector3 _origScaleIconEnergy = Vector3.one;

		private Vector3 _origScaleIconHealth = Vector3.one;

		private float _displayedNutrition;

		private float _displayedHydration;

		private float _displayedEnergy;

		private float _displayedHealth;

		private float _displayedNutritionVelocity;

		private float _displayedHydrationVelocity;

		private float _displayedEnergyVelocity;

		private float _displayedHealthVelocity;

		private float _pulsePhaseNutrition;

		private float _pulsePhaseHydration;

		private float _pulsePhaseEnergy;

		private float _pulsePhaseHealth;

		public bool IsInitialized { get; set; }

		public void Init(PlayerStatsManager playerStatsManager)
		{
			_playerStatsManager = playerStatsManager;
			_origColorsNutrition = CaptureOriginalColors(nutritionPulseImages);
			_origColorsHydration = CaptureOriginalColors(hydrationPulseImages);
			_origColorsEnergy = CaptureOriginalColors(energyPulseImages);
			_origColorsHealth = CaptureOriginalColors(healthPulseImages);
			if (nutritionIcon != null)
			{
				_origScaleIconNutrition = nutritionIcon.localScale;
			}
			if (hydrationIcon != null)
			{
				_origScaleIconHydration = hydrationIcon.localScale;
			}
			if (energyIcon != null)
			{
				_origScaleIconEnergy = energyIcon.localScale;
			}
			if (healthIcon != null)
			{
				_origScaleIconHealth = healthIcon.localScale;
			}
			_displayedNutrition = _playerStatsManager.Nutrition.CurrentValue;
			_displayedHydration = _playerStatsManager.Hydration.CurrentValue;
			_displayedEnergy = _playerStatsManager.Energy.CurrentValue;
			_displayedHealth = _playerStatsManager.Health.CurrentValue;
			ApplyInitialVisuals(_playerStatsManager.Nutrition, nutritionFillImage, nutritionValueText, _displayedNutrition);
			ApplyInitialVisuals(_playerStatsManager.Hydration, hydrationFillImage, hydrationValueText, _displayedHydration);
			ApplyInitialVisuals(_playerStatsManager.Energy, energyFillImage, energyValueText, _displayedEnergy);
			ApplyInitialVisuals(_playerStatsManager.Health, healthFillImage, healthValueText, _displayedHealth);
			IsInitialized = true;
		}

		private static Color[] CaptureOriginalColors(List<Image> images)
		{
			if (images == null)
			{
				return Array.Empty<Color>();
			}
			Color[] array = new Color[images.Count];
			for (int i = 0; i < images.Count; i++)
			{
				array[i] = ((images[i] != null) ? images[i].color : Color.white);
			}
			return array;
		}

		private static void ApplyInitialVisuals(PlayerStat stat, Image fill, TMP_Text text, float displayed)
		{
			if (fill != null && stat.MaxValue > 0f)
			{
				fill.fillAmount = displayed / stat.MaxValue;
			}
			if (text != null)
			{
				text.text = FormatStatValue(displayed);
			}
		}

		private static string FormatStatValue(float value)
		{
			if (!(value < 10f))
			{
				return Mathf.RoundToInt(value).ToString();
			}
			return value.ToString("F1");
		}

		private void Update()
		{
			if (IsInitialized)
			{
				TickStat(_playerStatsManager.Nutrition, nutritionFillImage, nutritionValueText, nutritionPulseImages, _origColorsNutrition, nutritionIcon, _origScaleIconNutrition, ref _displayedNutrition, ref _displayedNutritionVelocity, ref _pulsePhaseNutrition);
				TickStat(_playerStatsManager.Hydration, hydrationFillImage, hydrationValueText, hydrationPulseImages, _origColorsHydration, hydrationIcon, _origScaleIconHydration, ref _displayedHydration, ref _displayedHydrationVelocity, ref _pulsePhaseHydration);
				TickStat(_playerStatsManager.Energy, energyFillImage, energyValueText, energyPulseImages, _origColorsEnergy, energyIcon, _origScaleIconEnergy, ref _displayedEnergy, ref _displayedEnergyVelocity, ref _pulsePhaseEnergy);
				TickStat(_playerStatsManager.Health, healthFillImage, healthValueText, healthPulseImages, _origColorsHealth, healthIcon, _origScaleIconHealth, ref _displayedHealth, ref _displayedHealthVelocity, ref _pulsePhaseHealth);
			}
		}

		private void TickStat(PlayerStat stat, Image fillImage, TMP_Text valueText, List<Image> pulseImages, Color[] origColors, Transform icon, Vector3 origScale, ref float displayed, ref float velocity, ref float pulsePhase)
		{
			float currentValue = stat.CurrentValue;
			if (Mathf.Abs(displayed - currentValue) < valueSnapEpsilon)
			{
				displayed = currentValue;
				velocity = 0f;
			}
			else
			{
				displayed = Mathf.SmoothDamp(displayed, currentValue, ref velocity, valueSmoothTime);
			}
			if (fillImage != null && stat.MaxValue > 0f)
			{
				fillImage.fillAmount = displayed / stat.MaxValue;
			}
			if (valueText != null)
			{
				valueText.text = FormatStatValue(displayed);
			}
			ApplyPulseAndBreath(stat, displayed, pulseImages, origColors, icon, origScale, ref pulsePhase);
		}

		private void ApplyPulseAndBreath(PlayerStat stat, float displayed, List<Image> pulseImages, Color[] origColors, Transform icon, Vector3 origScale, ref float pulsePhase)
		{
			float criticalLowThreshold = stat.CriticalLowThreshold;
			if (!(displayed <= criticalLowThreshold))
			{
				pulsePhase = 0f;
				RestorePulseImages(pulseImages, origColors);
				if (icon != null)
				{
					icon.localScale = origScale;
				}
				return;
			}
			float t = ((criticalLowThreshold > 0f) ? (1f - Mathf.Clamp01(displayed / criticalLowThreshold)) : 0f);
			float num = Mathf.Lerp(pulseDurationMax, pulseDurationMin, t);
			Color b = Color.Lerp(pulseColorMild, pulseColorSevere, t);
			float num2 = Mathf.Lerp(iconBreathPeakMild, iconBreathPeakSevere, t);
			Vector3 b2 = origScale * num2;
			pulsePhase = Mathf.Repeat(pulsePhase + Time.deltaTime / (2f * num), 1f);
			float t2 = 0.5f - 0.5f * Mathf.Cos(pulsePhase * 2f * (float)Math.PI);
			if (pulseImages != null && origColors != null)
			{
				int num3 = Mathf.Min(pulseImages.Count, origColors.Length);
				for (int i = 0; i < num3; i++)
				{
					Image image = pulseImages[i];
					if (!(image == null))
					{
						image.color = Color.Lerp(origColors[i], b, t2);
					}
				}
			}
			if (icon != null)
			{
				icon.localScale = Vector3.Lerp(origScale, b2, t2);
			}
		}

		private static void RestorePulseImages(List<Image> images, Color[] origColors)
		{
			if (images == null || origColors == null)
			{
				return;
			}
			int num = Mathf.Min(images.Count, origColors.Length);
			for (int i = 0; i < num; i++)
			{
				Image image = images[i];
				if (!(image == null))
				{
					image.color = origColors[i];
				}
			}
		}
	}
}
