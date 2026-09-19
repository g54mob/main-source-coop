using System;
using System.Collections.Generic;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.Modifier;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories;
using UnityEngine;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity
{
	public class StatEntityBase<TStatEnum> : IModifierEntity<TStatEnum>, IStatEntity<TStatEnum> where TStatEnum : Enum
	{
		private class EntityStatComparator : IEqualityComparer<TStatEnum>
		{
			public bool Equals(TStatEnum x, TStatEnum y)
			{
				return x.Equals(y);
			}

			public int GetHashCode(TStatEnum x)
			{
				return Convert.ToInt32(x);
			}
		}

		private readonly IStatFactory<TStatEnum> _statFactory;

		private readonly Dictionary<TStatEnum, IStat> _statsHolder = new Dictionary<TStatEnum, IStat>(new EntityStatComparator());

		public IStat this[TStatEnum TStatEnum]
		{
			get
			{
				if (!_statsHolder.ContainsKey(TStatEnum))
				{
					AddStat(TStatEnum);
				}
				return _statsHolder[TStatEnum];
			}
		}

		public event Action<TStatEnum> OnReachedMinValue;

		public event Action<TStatEnum> OnReachedMaxValue;

		public event Action<TStatEnum> OnBaseValueChanged;

		public event Action<TStatEnum> OnBonusValueChanged;

		public event Action<TStatEnum> OnFullValueChanged;

		public event Action<TStatEnum> OnMinValueChanged;

		public event Action<TStatEnum> OnMaxValueChanged;

		public StatEntityBase(IStatFactory<TStatEnum> statFactory)
		{
			_statFactory = statFactory;
		}

		public IStat GetStat(TStatEnum statType)
		{
			if (!_statsHolder.ContainsKey(statType))
			{
				AddStat(statType);
			}
			return _statsHolder[statType];
		}

		public void AddModifier(TStatEnum statType, StatModifier statModifier)
		{
			if (_statsHolder.TryGetValue(statType, out var value))
			{
				value.AddStatModifier(statModifier);
			}
		}

		public void RemoveModifier(TStatEnum statType, StatModifier statModifier)
		{
			if (_statsHolder.TryGetValue(statType, out var value))
			{
				value.RemoveStatModifier(statModifier);
			}
		}

		public void RemoveModifierThatEqual(TStatEnum statType, StatModifier statModifier)
		{
			if (_statsHolder.TryGetValue(statType, out var value))
			{
				value.RemoveStatModifierThatEqual(statModifier);
			}
		}

		public void RemoveAllModifiers()
		{
			foreach (IStat value in _statsHolder.Values)
			{
				value.ClearModifiers();
			}
		}

		private void AddStat(TStatEnum statType)
		{
			if (_statsHolder.ContainsKey(statType))
			{
				Debug.LogError($"Stats Holder already contains {statType}!");
				return;
			}
			IStat stat = _statFactory.Create(statType);
			_statsHolder.Add(statType, stat);
			stat.OnReachedMinValue += delegate
			{
				this.OnReachedMinValue?.Invoke(statType);
			};
			stat.OnReachedMaxValue += delegate
			{
				this.OnReachedMaxValue?.Invoke(statType);
			};
			stat.OnValueChanged += delegate
			{
				this.OnBaseValueChanged?.Invoke(statType);
			};
			stat.OnBonusValueChanged += delegate
			{
				this.OnBonusValueChanged?.Invoke(statType);
			};
			stat.OnFullValueChanged += delegate
			{
				this.OnFullValueChanged?.Invoke(statType);
			};
			stat.OnMinValueChanged += delegate
			{
				this.OnMinValueChanged?.Invoke(statType);
			};
			stat.OnMaxValueChanged += delegate
			{
				this.OnMaxValueChanged?.Invoke(statType);
			};
		}

		public void ClearStats()
		{
			foreach (IStat value in _statsHolder.Values)
			{
				value.Clear();
			}
		}
	}
}
