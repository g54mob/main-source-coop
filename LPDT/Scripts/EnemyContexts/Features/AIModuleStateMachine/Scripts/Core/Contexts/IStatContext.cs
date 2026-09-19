using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;

namespace Features.AIModuleStateMachine.Scripts.Core.Contexts
{
	public interface IStatContext
	{
		float GetStatValue(EntityStatType statType, float fallbackValue = 0f);
	}
}
