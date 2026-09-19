using System;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories
{
	public interface IStatEntityFactory<TStatEnum> where TStatEnum : Enum
	{
		StatEntityBase<TStatEnum> Create();
	}
}
