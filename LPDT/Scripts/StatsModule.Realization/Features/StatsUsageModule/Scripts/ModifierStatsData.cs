using System;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine;

namespace Features.StatsUsageModule.Scripts
{
	[Serializable]
	public struct ModifierStatsData
	{
		[field: SerializeField]
		public EntityStatType EntityStatType { get; private set; }

		[field: SerializeField]
		public ModifierType ModifierType { get; private set; }

		[field: SerializeField]
		public float Value { get; private set; }

		public ModifierStatsData(EntityStatType entityStatType, ModifierType modifierType, float value)
		{
			EntityStatType = entityStatType;
			ModifierType = modifierType;
			Value = value;
		}
	}
}
