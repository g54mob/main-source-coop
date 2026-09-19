using Features.StatsUsageModule.Scripts.Entities.EntityStatTypeEntities;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.HUDModule.Scripts
{
	public abstract class PlayerStatViewBase : ViewBehaviour
	{
		public abstract EntityStatType TrackedStatType { get; }

		public abstract void SetMaxValue(float health);

		public abstract void SetCurrentValue(float health);

		public abstract void SetVisible(bool visible);

		public abstract void TriggerIncreaseAnimation();
	}
}
