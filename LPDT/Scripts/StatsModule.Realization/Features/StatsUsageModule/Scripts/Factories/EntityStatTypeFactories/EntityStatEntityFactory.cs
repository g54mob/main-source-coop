using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories;

namespace Features.StatsUsageModule.Scripts.Factories.EntityStatTypeFactories
{
	public class EntityStatEntityFactory : StatEntityFactoryBase<EntityStatType>
	{
		public EntityStatEntityFactory(IStatFactory<EntityStatType> statFactory)
			: base(statFactory)
		{
		}
	}
}
