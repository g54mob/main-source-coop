using System;
using System.Collections.Generic;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity
{
	public interface IStat
	{
		List<StatModifier> StatModifiers { get; }

		float NonModifiedMaxValue { get; }

		float MaxValue { get; set; }

		float MinValue { get; set; }

		float FullValue { get; }

		float BonusValue { get; }

		float Value { get; }

		event Action OnReachedMinValue;

		event Action OnReachedMaxValue;

		event Action<float> OnValueChanged;

		event Action<float> OnBonusValueChanged;

		event Action<float> OnFullValueChanged;

		event Action<float> OnMinValueChanged;

		event Action<float> OnMaxValueChanged;

		void AddValue(float value);

		void Subtract(float subtractionValue);

		void OverrideValue(float value);

		void AddStatModifier(StatModifier statModifier);

		void RemoveStatModifier(StatModifier statModifier);

		void RemoveStatModifierThatEqual(StatModifier statModifier);

		void ClearModifiers();

		float GetPreviewModifier(ICollection<StatModifier> newStatModifiers);

		void Clear();
	}
}
