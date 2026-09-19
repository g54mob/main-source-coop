using System;
using System.Collections.Generic;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine;

namespace Features.BoosterModule.BoosterModule.Scripts.Entities
{
	[Serializable]
	public class StatSerializationHolder
	{
		[SerializeField]
		private EntityStatType _entityStatType;

		[SerializeField]
		private ModifierType _modifierType;

		[SerializeField]
		private float _value;

		public KeyValuePair<EntityStatType, StatModifier> GetKeyValuePair()
		{
			return new KeyValuePair<EntityStatType, StatModifier>(_entityStatType, new StatModifier(_value, _modifierType, (int)_modifierType));
		}
	}
}
