using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Configurations;
using UnityEngine;

namespace Features.StatsUsageModule.Scripts.Configurations.EntityStatTypeEntities
{
	[CreateAssetMenu(fileName = "EntityAccumulativeStatsConfiguration_Default", menuName = "Configurations/StatsModule/EntityAccumulativeStatsConfiguration")]
	public class EntityAccumulativeStatsConfiguration : AccumulativeStatsConfigurationBase<EntityStatType>
	{
	}
}
