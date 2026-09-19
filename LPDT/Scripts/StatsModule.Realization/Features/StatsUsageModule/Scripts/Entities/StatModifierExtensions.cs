using System.Collections.Generic;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;

namespace Features.StatsUsageModule.Scripts.Entities
{
	public static class StatModifierExtensions
	{
		public static StatModifier ToStatModifierClass(this StatModifierStruct structModifier)
		{
			return new StatModifier(structModifier.Value, structModifier.ModifierType, structModifier.Order);
		}

		public static StatModifierStruct ToStatModifierStruct(this StatModifier modifier)
		{
			return new StatModifierStruct(modifier.Value, modifier.ModifierType, modifier.Order);
		}

		public static List<StatModifier> ToListOfStatModifierClass(this StatModifierStruct[] structModifier)
		{
			List<StatModifier> list = new List<StatModifier>();
			foreach (StatModifierStruct structModifier2 in structModifier)
			{
				list.Add(structModifier2.ToStatModifierClass());
			}
			return list;
		}

		public static StatModifierStruct[] ToArrayOfStatModifierStruct(this List<StatModifier> modifier)
		{
			StatModifierStruct[] array = new StatModifierStruct[modifier.Count];
			for (int i = 0; i < modifier.Count; i++)
			{
				array[i] = modifier[i].ToStatModifierStruct();
			}
			return array;
		}
	}
}
