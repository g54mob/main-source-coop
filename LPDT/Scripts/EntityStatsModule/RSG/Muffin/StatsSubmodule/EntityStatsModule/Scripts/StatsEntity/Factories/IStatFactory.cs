using System;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories
{
	public interface IStatFactory<in TStatEnum> where TStatEnum : Enum
	{
		IStat Create(TStatEnum entityStatType);
	}
}
