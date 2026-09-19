using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Configurations;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories;

namespace Features.StatsUsageModule.Scripts.Factories.EntityStatTypeFactories
{
	public class EntityStatFactory : StatFactoryBase<EntityStatType>
	{
		public EntityStatFactory(AccumulativeStatsConfigurationBase<EntityStatType> accumulativeStatsConfigurationBase)
			: base(accumulativeStatsConfigurationBase)
		{
		}
	}
}
