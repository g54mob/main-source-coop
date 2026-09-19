using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity;

namespace Features.StatsUsageModule.Scripts
{
	public interface IPlayerStatsUpgradeService
	{
		void ApplyModifierStats(ModifierStatsData modifierStatsData);

		void DiscardModifierStats(ModifierStatsData modifierStatsData);

		void ApplyModifierStatsSynchronized(ModifierStatsData modifierStatsData);

		void DiscardModifierStatsSynchronized(ModifierStatsData modifierStatsData);

		IStat GetStat(EntityStatType statType);

		IStat GetStat(EntityStatType statType, int targetPlayer);

		bool IsPlayerStatsReady();
	}
}
