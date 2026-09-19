using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Factories;

namespace Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities
{
	public class EntityStatEntity : StatEntityBase<EntityStatType>
	{
		public EntityStatEntity(IStatFactory<EntityStatType> statFactory)
			: base(statFactory)
		{
		}
	}
}
