using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Stats
{
	public sealed class StaticValueStat : IStat
	{
		private const float PERCENTAGE_SCALE = 1f;

		private float _minValue;

		private float _maxValue;

		private float _bonusValue;

		private float _value;

		private bool _hasPreviouslyReachedMinValue;

		private bool _hasPreviouslyReachedMaxValue;

		public List<StatModifier> StatModifiers { get; } = new List<StatModifier>();

		public float FullValue => _value + _bonusValue;

		public float NonModifiedMaxValue => _maxValue;

		public float MaxValue
		{
			get
			{
				return _maxValue;
			}
			set
			{
				_maxValue = value;
				this.OnMaxValueChanged?.Invoke(_maxValue);
			}
		}

		public float MinValue
		{
			get
			{
				return _minValue;
			}
			set
			{
				_minValue = value;
				this.OnMinValueChanged?.Invoke(_minValue);
			}
		}

		public float BonusValue
		{
			get
			{
				return _bonusValue;
			}
			private set
			{
				_bonusValue = value;
				this.OnBonusValueChanged?.Invoke(_bonusValue);
				this.OnFullValueChanged?.Invoke(FullValue);
			}
		}

		public float Value
		{
			get
			{
				return _value;
			}
			private set
			{
				_value = value;
				this.OnValueChanged?.Invoke(_value);
				this.OnFullValueChanged?.Invoke(FullValue);
				if (Mathf.Approximately(Value, MaxValue))
				{
					if (!_hasPreviouslyReachedMaxValue)
					{
						this.OnReachedMaxValue?.Invoke();
						_hasPreviouslyReachedMaxValue = true;
					}
				}
				else
				{
					_hasPreviouslyReachedMaxValue = false;
				}
				if (Mathf.Approximately(Value, MinValue))
				{
					if (!_hasPreviouslyReachedMinValue)
					{
						this.OnReachedMinValue?.Invoke();
						_hasPreviouslyReachedMinValue = true;
					}
				}
				else
				{
					_hasPreviouslyReachedMinValue = false;
				}
			}
		}

		public event Action OnReachedMinValue;

		public event Action OnReachedMaxValue;

		public event Action<float> OnValueChanged;

		public event Action<float> OnBonusValueChanged;

		public event Action<float> OnFullValueChanged;

		public event Action<float> OnMinValueChanged;

		public event Action<float> OnMaxValueChanged;

		public void AddValue(float value)
		{
			float a = value + Value;
			Value = Mathf.Min(a, MaxValue);
		}

		public void Subtract(float subtractionValue)
		{
			float a = Value - subtractionValue;
			Value = Mathf.Max(a, MinValue);
		}

		public void OverrideValue(float value)
		{
			Value = value;
		}

		public void AddStatModifier(StatModifier statModifier)
		{
			StatModifiers.Add(statModifier);
			StatModifiers.Sort((StatModifier firstValue, StatModifier secondValue) => firstValue.Order.CompareTo(secondValue.Order));
			BonusValue = CalculateBonusValue(StatModifiers);
		}

		public void RemoveStatModifier(StatModifier statModifier)
		{
			if (StatModifiers != null && StatModifiers.Contains(statModifier))
			{
				StatModifiers.Remove(statModifier);
				BonusValue = CalculateBonusValue(StatModifiers);
			}
		}

		public void RemoveStatModifierThatEqual(StatModifier statModifier)
		{
			if (!(StatModifiers?.FirstOrDefault((StatModifier s) => s == statModifier) == null))
			{
				StatModifiers.Remove(statModifier);
				BonusValue = CalculateBonusValue(StatModifiers);
			}
		}

		public void ClearModifiers()
		{
			StatModifiers.Clear();
			BonusValue = CalculateBonusValue(StatModifiers);
		}

		public float GetPreviewModifier(ICollection<StatModifier> newStatModifiers)
		{
			List<StatModifier> list = new List<StatModifier>(StatModifiers);
			list.AddRange(newStatModifiers);
			list.Sort((StatModifier firstValue, StatModifier secondValue) => firstValue.Order.CompareTo(secondValue.Order));
			return CalculateBonusValue(list) + Value;
		}

		public void Clear()
		{
			this.OnReachedMinValue = null;
			this.OnReachedMaxValue = null;
			this.OnValueChanged = null;
			this.OnBonusValueChanged = null;
			this.OnFullValueChanged = null;
			this.OnMinValueChanged = null;
			this.OnMaxValueChanged = null;
		}

		private float CalculateBonusValue(ICollection<StatModifier> modifiers)
		{
			return modifiers.Aggregate(Value, (float current, StatModifier statModifier) => ApplyModifier(statModifier, current)) - Value;
		}

		private float ApplyModifier(StatModifier statModifier, float finalValue)
		{
			finalValue = statModifier.ModifierType switch
			{
				ModifierType.Flat => finalValue + statModifier.Value, 
				ModifierType.PercentAdd => finalValue * (1f + statModifier.Value), 
				ModifierType.PercentMulti => finalValue * statModifier.Value, 
				_ => throw new ArgumentOutOfRangeException("ModifierType", "Unknown modifier type"), 
			};
			return finalValue;
		}
	}
}
