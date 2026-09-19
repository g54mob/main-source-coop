using System;
using System.Collections.Generic;
using System.Linq;
using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using UnityEngine;

namespace Features.BoosterModule.BoosterModule.Scripts.Entities
{
	[Serializable]
	public class TemporaryAddStatToPlayerBoosterSettings : BoosterSettingsBase
	{
		[SerializeField]
		private List<StatSerializationHolder> _statModifiers;

		private Dictionary<EntityStatType, StatModifier> _statModifiersDictionary;

		public Dictionary<EntityStatType, StatModifier> GetStatModifiers()
		{
			return _statModifiersDictionary ?? (_statModifiersDictionary = _statModifiers.ToDictionary((StatSerializationHolder s) => s.GetKeyValuePair().Key, (StatSerializationHolder s) => s.GetKeyValuePair().Value));
		}

		public override string GetIdentifier()
		{
			string text = string.Join("_", from s in _statModifiers
				select s.GetKeyValuePair().Key into s
				orderby s.ToString()
				select s);
			return GetType().Name + "_" + text;
		}

		public override string ToString()
		{
			return "TemporaryAddStatToPlayer";
		}
	}
}
