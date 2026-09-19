using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.StoreModule.Scripts
{
	public abstract class StoreStatsViewBase : ViewBehaviour
	{
		public abstract EntityStatType TrackedStatType { get; }

		public abstract void SetMaxValue(float health);

		public abstract void SetCurrentValue(float health);
	}
}
