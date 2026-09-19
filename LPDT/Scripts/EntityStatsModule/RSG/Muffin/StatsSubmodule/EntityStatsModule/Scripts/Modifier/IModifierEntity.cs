using System;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier
{
	public interface IModifierEntity<in TStatEnum> where TStatEnum : Enum
	{
		void AddModifier(TStatEnum statType, StatModifier statModifier);

		void RemoveModifier(TStatEnum statType, StatModifier statModifier);

		void RemoveModifierThatEqual(TStatEnum statType, StatModifier statModifier);

		void RemoveAllModifiers();
	}
}
