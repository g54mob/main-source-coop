using System;
using System.Collections.Generic;
using System.Linq;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Stats
{
	public sealed class AccumulativeStat : IStat
	{
		private const float PERCENTAGE_SCALE = 1f;

		private float _minValue;

		private float _defaultBaseValue;

		private float _modifiedMaxValue;

		private float _bonusValue;

		private float _value;

		private bool _hasPreviouslyReachedMinValue;

		private bool _hasPreviouslyReachedMaxValue;

		public List<StatModifier> StatModifiers { get; } = new List<StatModifier>();

		public float FullValue { get; private set; }

		public float NonModifiedMaxValue => _defaultBaseValue;

		public float MaxValue
		{
			get
			{
				return _modifiedMaxValue;
			}
			set
			{
				_defaultBaseValue = value;
				_modifiedMaxValue = CalculateValueWithModifiers(_defaultBaseValue, StatModifiers);
				this.OnMaxValueChanged?.Invoke(_modifiedMaxValue);
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
				FullValue = _value + _bonusValue;
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
				FullValue = _value + _bonusValue;
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
			if (BonusValue > 0f)
			{
				float num = subtractionValue - BonusValue;
				if (num > 0f)
				{
					SubtractValue(num);
				}
				else
				{
					BonusValue -= subtractionValue;
				}
			}
			else
			{
				SubtractValue(subtractionValue);
			}
		}

		public void OverrideValue(float value)
		{
			Value = Mathf.Min(value, MaxValue);
		}

		public void AddStatModifier(StatModifier statModifier)
		{
			float maxValue = MaxValue;
			StatModifiers.Add(statModifier);
			StatModifiers.Sort((StatModifier firstValue, StatModifier secondValue) => firstValue.Order.CompareTo(secondValue.Order));
			MaxValue = _defaultBaseValue;
			CalculateValueRelativeToMaxValueDelta(MaxValue - maxValue);
		}

		public void RemoveStatModifier(StatModifier statModifier)
		{
			if (StatModifiers != null && StatModifiers.Contains(statModifier))
			{
				float maxValue = MaxValue;
				StatModifiers.Remove(statModifier);
				MaxValue = _defaultBaseValue;
				CalculateValueRelativeToMaxValueDelta(MaxValue - maxValue);
			}
		}

		public void RemoveStatModifierThatEqual(StatModifier statModifier)
		{
			StatModifier statModifier2 = StatModifiers?.FirstOrDefault((StatModifier s) => s == statModifier);
			if (!(statModifier2 == null))
			{
				float maxValue = MaxValue;
				StatModifiers.Remove(statModifier2);
				MaxValue = _defaultBaseValue;
				CalculateValueRelativeToMaxValueDelta(MaxValue - maxValue);
			}
		}

		public void ClearModifiers()
		{
			float maxValue = MaxValue;
			StatModifiers.Clear();
			MaxValue = _defaultBaseValue;
			CalculateValueRelativeToMaxValueDelta(MaxValue - maxValue);
		}

		public float GetPreviewModifier(ICollection<StatModifier> newStatModifiers)
		{
			return CalculateValueWithModifiers(MaxValue, newStatModifiers);
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

		private void CalculateValueRelativeToMaxValueDelta(float maxValueDelta)
		{
			if (!(maxValueDelta < 0f))
			{
				if (maxValueDelta > 0f)
				{
					AddValue(maxValueDelta);
				}
			}
			else
			{
				Value = Mathf.Min(Value, MaxValue);
			}
		}

		private float CalculateValueWithModifiers(float initialValue, ICollection<StatModifier> modifiers)
		{
			return modifiers.Aggregate(initialValue, (float current, StatModifier statModifier) => ApplyModifier(statModifier, current));
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

		private void SubtractValue(float subtractionValue)
		{
			float value = ((!(Value <= subtractionValue)) ? (Value - subtractionValue) : MinValue);
			Value = value;
		}
	}
}
