using System;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Configurations;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Stats;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories
{
	public class StatFactoryBase<TStatEnum> : IStatFactory<TStatEnum> where TStatEnum : Enum
	{
		private readonly AccumulativeStatsConfigurationBase<TStatEnum> _accumulativeStatsConfigurationBase;

		public StatFactoryBase(AccumulativeStatsConfigurationBase<TStatEnum> accumulativeStatsConfigurationBase)
		{
			_accumulativeStatsConfigurationBase = accumulativeStatsConfigurationBase;
		}

		public IStat Create(TStatEnum entityStatType)
		{
			return CreateStat(entityStatType);
		}

		private IStat CreateStat(TStatEnum entityStatType)
		{
			if (_accumulativeStatsConfigurationBase.AccumulativeStats.Contains(entityStatType))
			{
				return new AccumulativeStat();
			}
			return new StaticValueStat();
		}
	}
}
