using System;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity
{
	public interface IStatEntity<TStatEnum> where TStatEnum : Enum
	{
		IStat this[TStatEnum entityStatType] { get; }

		event Action<TStatEnum> OnReachedMinValue;

		event Action<TStatEnum> OnReachedMaxValue;

		event Action<TStatEnum> OnBaseValueChanged;

		event Action<TStatEnum> OnBonusValueChanged;

		event Action<TStatEnum> OnFullValueChanged;

		event Action<TStatEnum> OnMinValueChanged;

		event Action<TStatEnum> OnMaxValueChanged;

		IStat GetStat(TStatEnum statType);

		void ClearStats();
	}
}
