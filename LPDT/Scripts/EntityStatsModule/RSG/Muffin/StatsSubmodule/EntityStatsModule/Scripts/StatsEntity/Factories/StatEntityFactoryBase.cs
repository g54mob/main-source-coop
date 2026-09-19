using System;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories
{
	public class StatEntityFactoryBase<TStatEnum> : IStatEntityFactory<TStatEnum> where TStatEnum : Enum
	{
		private readonly IStatFactory<TStatEnum> _statFactory;

		public StatEntityFactoryBase(IStatFactory<TStatEnum> statFactory)
		{
			_statFactory = statFactory;
		}

		public StatEntityBase<TStatEnum> Create()
		{
			return new StatEntityBase<TStatEnum>(_statFactory);
		}
	}
}
