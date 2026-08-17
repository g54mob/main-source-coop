using System;
using EvilCore;
using EvilCore.UI.Scripts;
using NomadDrive.Features.Consumables;
using NomadDrive.Features.Player.Core;
using NomadDrive.Features.Player.Survival;
using NomadDrive.Features.Player.UI;
using NomadDrive.Managers.GameTime;
using UnityEngine;
using VContainer;

namespace NomadDrive.Features.Player
{
	public class PlayerStatsManager : MonoBehaviour, IInitialize, IPlayerComponent
	{
		[Inject]
		private ITimeManager _timeManager;

		[SerializeField]
		private PlayerStatSetting _nutritionSetting;

		[SerializeField]
		private PlayerStatSetting _hydrationSetting;

		[SerializeField]
		private PlayerStatSetting _energySetting;

		[SerializeField]
		private PlayerStatSetting _healthSetting;

		[SerializeField]
		private PlayerStatSetting _poisonSetting;

		[SerializeField]
		private PlayerStatusBarConfig _peakStatusBarConfig;

		[Inject]
		private UIFeedbackManager _uiFeedbackManager;

		[Inject]
		private PeakStatusBarDisplayer _peakStatusBarDisplayer;

		[Inject]
		private IPlayerService _playerService;

		[Tooltip("Energy regained per in-game minute while the player is seated (resting). Gentle by default.")]
		[SerializeField]
		private float _energyRestRegenPerMinute = 0.5f;

		private float _poisonRatePerMinute;

		private float _poisonHealRatePerMinute;

		private bool _isCuring;

		private const float ReviveHealthRatio = 0.5f;

		public int SetupPriority => 15;

		[field: SerializeField]
		public PlayerStat Nutrition { get; private set; }

		[field: SerializeField]
		public PlayerStat Hydration { get; private set; }

		[field: SerializeField]
		public PlayerStat Energy { get; private set; }

		[field: SerializeField]
		public PlayerStat Health { get; private set; }

		[field: SerializeField]
		public PlayerStat Poison { get; private set; }

		public float EffectiveHealth { get; private set; } = 1f;

		public float CurrentDamage
		{
			get
			{
				if (Health == null)
				{
					return 0f;
				}
				return Mathf.Max(0f, Health.MaxValue - Health.CurrentValue);
			}
		}

		public bool IsPoisoned
		{
			get
			{
				if (Poison != null)
				{
					if (!(Poison.CurrentValue > 0f))
					{
						return _poisonRatePerMinute > 0f;
					}
					return true;
				}
				return false;
			}
		}

		public bool IsInitialized { get; set; }

		public bool IsDead { get; private set; }

		public event Action OnEffectiveHealthChanged;

		public event Action OnHealthDamaged;

		public event Action OnDeath;

		public event Action OnPoisoned;

		public void SetupForPlayer(bool isLocalPlayer)
		{
			if (isLocalPlayer)
			{
				Init();
			}
			else
			{
				base.enabled = false;
			}
		}

		public void Init()
		{
			Nutrition = new PlayerStat(100f, _nutritionSetting.maxValue, _nutritionSetting.defaultConsumeSpeed, _nutritionSetting.criticalLowThreshold, _nutritionSetting.sleepingConsumeSpeedMultiplier);
			Hydration = new PlayerStat(100f, _hydrationSetting.maxValue, _hydrationSetting.defaultConsumeSpeed, _hydrationSetting.criticalLowThreshold, _hydrationSetting.sleepingConsumeSpeedMultiplier);
			Energy = new PlayerStat(100f, _energySetting.maxValue, _energySetting.defaultConsumeSpeed, _energySetting.criticalLowThreshold, _energySetting.sleepingConsumeSpeedMultiplier);
			Health = new PlayerStat(100f, _healthSetting.maxValue, 0f, _healthSetting.criticalLowThreshold, 0f);
			Poison = new PlayerStat(0f, _poisonSetting.maxValue, 0f, _poisonSetting.criticalLowThreshold, 0f);
			Nutrition.OnValueZero.AddListener(OnNutritionZero);
			Nutrition.OnValueMax.AddListener(OnNutritionFull);
			Nutrition.OnValueCriticalLow.AddListener(OnNutritionCriticalLow);
			Hydration.OnValueZero.AddListener(OnHydrationZero);
			Hydration.OnValueMax.AddListener(OnHydrationFull);
			Hydration.OnValueCriticalLow.AddListener(OnHydrationCriticalLow);
			Energy.OnValueZero.AddListener(OnEnergyZero);
			Energy.OnValueMax.AddListener(OnEnergyFull);
			Energy.OnValueCriticalLow.AddListener(OnEnergyCriticalLow);
			Health.OnValueZero.AddListener(OnHealthZero);
			Health.OnValueMax.AddListener(OnHealthFull);
			Health.OnValueCriticalLow.AddListener(OnHealthCriticalLow);
			Nutrition.OnValueChanged.AddListener(OnAnyStatValueChanged);
			Hydration.OnValueChanged.AddListener(OnAnyStatValueChanged);
			Energy.OnValueChanged.AddListener(OnAnyStatValueChanged);
			Health.OnValueChanged.AddListener(OnAnyStatValueChanged);
			Poison.OnValueChanged.AddListener(OnAnyStatValueChanged);
			Poison.OnValueZero.AddListener(OnPoisonCleared);
			_timeManager.OnMinutePassed.AddListener(OnMinutePassedHandler);
			IsInitialized = true;
			RecalculateEffectiveHealth();
			_peakStatusBarDisplayer.Init(this);
		}

		public void SetDownedFormDisplay(bool active)
		{
			_peakStatusBarDisplayer?.SetDownedForm(active);
		}

		public void SetDeathFrozen(bool frozen)
		{
			if (IsInitialized)
			{
				IsDead = frozen;
			}
		}

		public void SetStatsSleepingConsumingMultiplier()
		{
			Nutrition.SetSleepingConsumingMultiplier();
			Hydration.SetSleepingConsumingMultiplier();
			Energy.SetSleepingConsumingMultiplier();
		}

		public void ResetStatsConsumingMultiplier()
		{
			Nutrition.ResetConsumingMultiplier();
			Hydration.ResetConsumingMultiplier();
			Energy.ResetConsumingMultiplier();
		}

		private void OnDestroy()
		{
			_timeManager?.OnMinutePassed?.RemoveListener(OnMinutePassedHandler);
			Nutrition?.OnValueChanged?.RemoveListener(OnAnyStatValueChanged);
			Hydration?.OnValueChanged?.RemoveListener(OnAnyStatValueChanged);
			Energy?.OnValueChanged?.RemoveListener(OnAnyStatValueChanged);
			Health?.OnValueChanged?.RemoveListener(OnAnyStatValueChanged);
			Poison?.OnValueChanged?.RemoveListener(OnAnyStatValueChanged);
			Poison?.OnValueZero?.RemoveListener(OnPoisonCleared);
			Nutrition?.OnValueZero?.RemoveListener(OnNutritionZero);
			Nutrition?.OnValueMax?.RemoveListener(OnNutritionFull);
			Nutrition?.OnValueCriticalLow?.RemoveListener(OnNutritionCriticalLow);
			Hydration?.OnValueZero?.RemoveListener(OnHydrationZero);
			Hydration?.OnValueMax?.RemoveListener(OnHydrationFull);
			Hydration?.OnValueCriticalLow?.RemoveListener(OnHydrationCriticalLow);
			Energy?.OnValueZero?.RemoveListener(OnEnergyZero);
			Energy?.OnValueMax?.RemoveListener(OnEnergyFull);
			Energy?.OnValueCriticalLow?.RemoveListener(OnEnergyCriticalLow);
			Health?.OnValueZero?.RemoveListener(OnHealthZero);
			Health?.OnValueMax?.RemoveListener(OnHealthFull);
			Health?.OnValueCriticalLow?.RemoveListener(OnHealthCriticalLow);
		}

		private void OnMinutePassedHandler()
		{
			if (IsDead)
			{
				return;
			}
			Nutrition.ConsumeByMinute();
			Hydration.ConsumeByMinute();
			if (IsRestingForEnergy())
			{
				if (Energy.CurrentValue < Energy.MaxValue)
				{
					Energy.IncreaseValue(_energyRestRegenPerMinute);
				}
			}
			else
			{
				Energy.ConsumeByMinute();
			}
			TickPoison();
		}

		private void TickPoison()
		{
			if (_poisonRatePerMinute > 0f)
			{
				Poison.IncreaseValue(_poisonRatePerMinute);
			}
			else if (_isCuring && Poison.CurrentValue > 0f)
			{
				Poison.DecreaseValue(_poisonHealRatePerMinute);
			}
		}

		private bool IsRestingForEnergy()
		{
			IPlayerService playerService = _playerService;
			if (playerService != null && playerService.IsPlayerSpawned && _playerService.LocalPlayer != null)
			{
				return _playerService.LocalPlayer.IsPlayerSitting();
			}
			return false;
		}

		private void OnNutritionZero()
		{
		}

		private void OnNutritionFull()
		{
		}

		private void OnNutritionCriticalLow()
		{
			_uiFeedbackManager.CreateFloatingMessage("@player.very_hungry", FeedbackType.Warning);
		}

		private void OnHydrationZero()
		{
		}

		private void OnHydrationFull()
		{
		}

		private void OnHydrationCriticalLow()
		{
			_uiFeedbackManager.CreateFloatingMessage("@player.very_thirsty", FeedbackType.Warning);
		}

		private void OnEnergyZero()
		{
		}

		private void OnEnergyFull()
		{
		}

		private void OnEnergyCriticalLow()
		{
			_uiFeedbackManager.CreateFloatingMessage("@player.need_rest", FeedbackType.Warning);
		}

		private void OnHealthZero()
		{
		}

		private void OnHealthFull()
		{
		}

		private void OnHealthCriticalLow()
		{
			_uiFeedbackManager.CreateFloatingMessage("@player.need_heal", FeedbackType.Warning);
		}

		public void ApplyConsumableEffects(Consumable consumable)
		{
			if (!IsInitialized || IsDead)
			{
				return;
			}
			foreach (PlayerStatType key in consumable.currentPlayerStatCollection.Keys)
			{
				float amount = consumable.currentPlayerStatCollection[key];
				switch (key)
				{
				case PlayerStatType.Nutrition:
					IncreaseAndAutoFill(Nutrition, amount, _peakStatusBarConfig.nutritionMaxEncroachment);
					break;
				case PlayerStatType.Hydration:
					IncreaseAndAutoFill(Hydration, amount, _peakStatusBarConfig.hydrationMaxEncroachment);
					break;
				case PlayerStatType.Energy:
					IncreaseAndAutoFill(Energy, amount, _peakStatusBarConfig.energyMaxEncroachment);
					break;
				case PlayerStatType.Health:
					IncreaseAndAutoFill(Health, amount, _peakStatusBarConfig.damageMaxEncroachment);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void IncreaseAndAutoFill(PlayerStat stat, float amount, float maxEncroachment)
		{
			float num = (1f - stat.GetValueRatio()) * maxEncroachment;
			stat.IncreaseValue(amount);
			float num2 = (1f - stat.GetValueRatio()) * maxEncroachment;
			float zoneAppearThreshold = _peakStatusBarConfig.zoneAppearThreshold;
			if (num >= zoneAppearThreshold && num2 < zoneAppearThreshold)
			{
				stat.SetMax();
			}
		}

		public void ApplyPoison(float ratePerMinute, float instantAmount)
		{
			if (IsInitialized && !IsDead)
			{
				_isCuring = false;
				_poisonHealRatePerMinute = 0f;
				_poisonRatePerMinute += Mathf.Max(0f, ratePerMinute);
				if (instantAmount > 0f)
				{
					Poison.IncreaseValue(instantAmount);
				}
				_uiFeedbackManager.CreateFloatingMessage("@player.poisoned");
				this.OnPoisoned?.Invoke();
			}
		}

		public bool ApplyMedicine(float healRatePerMinute, float instantReduction)
		{
			if (!IsInitialized)
			{
				return false;
			}
			if (IsDead)
			{
				return false;
			}
			if (!IsPoisoned)
			{
				return false;
			}
			_poisonRatePerMinute = 0f;
			_isCuring = true;
			_poisonHealRatePerMinute = Mathf.Max(0f, healRatePerMinute);
			if (instantReduction > 0f)
			{
				Poison.DecreaseValue(instantReduction);
			}
			return true;
		}

		private void OnPoisonCleared()
		{
			_isCuring = false;
			_poisonHealRatePerMinute = 0f;
		}

		public void ApplyDirectDamage(float amount)
		{
			if (IsInitialized && !IsDead)
			{
				Health.DecreaseValue(amount);
				this.OnHealthDamaged?.Invoke();
			}
		}

		public void HealDamage(float amount)
		{
			if (IsInitialized && !IsDead)
			{
				IncreaseAndAutoFill(Health, amount, _peakStatusBarConfig.damageMaxEncroachment);
			}
		}

		public void ReviveReset(float healthRatio = 0.5f)
		{
			if (IsInitialized)
			{
				IsDead = false;
				Nutrition.ResetValue(Nutrition.MaxValue);
				Hydration.ResetValue(Hydration.MaxValue);
				Energy.ResetValue(Energy.MaxValue);
				Health.ResetValue(Health.MaxValue * healthRatio);
				_poisonRatePerMinute = 0f;
				_poisonHealRatePerMinute = 0f;
				_isCuring = false;
				Poison.ResetValue(0f);
				RecalculateEffectiveHealth();
			}
		}

		public void RestoreFromSave(float nutrition, float hydration, float energy, float health, float poison)
		{
			if (IsInitialized)
			{
				IsDead = false;
				Nutrition.ResetValue(nutrition);
				Hydration.ResetValue(hydration);
				Energy.ResetValue(energy);
				Health.ResetValue(health);
				Poison.ResetValue(poison);
				RecalculateEffectiveHealth();
			}
		}

		public ZoneSizes GetZoneSizes()
		{
			if (_peakStatusBarConfig == null)
			{
				return default(ZoneSizes);
			}
			return EffectiveHealthCalculator.Calculate(Nutrition.GetValueRatio(), Hydration.GetValueRatio(), Energy.GetValueRatio(), Health.GetValueRatio(), Poison.GetValueRatio(), _peakStatusBarConfig);
		}

		private void OnAnyStatValueChanged()
		{
			RecalculateEffectiveHealth();
		}

		private void RecalculateEffectiveHealth()
		{
			if (!(_peakStatusBarConfig == null))
			{
				ZoneSizes zoneSizes = GetZoneSizes();
				float effectiveHealth = EffectiveHealth;
				EffectiveHealth = zoneSizes.EffectiveHealth;
				if (!Mathf.Approximately(effectiveHealth, EffectiveHealth))
				{
					this.OnEffectiveHealthChanged?.Invoke();
				}
				if (EffectiveHealth <= _peakStatusBarConfig.deathThreshold && effectiveHealth > _peakStatusBarConfig.deathThreshold)
				{
					IsDead = true;
					this.OnDeath?.Invoke();
				}
			}
		}
	}
}
