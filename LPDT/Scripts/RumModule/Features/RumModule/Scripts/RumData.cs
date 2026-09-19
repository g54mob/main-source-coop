using System;
using System.Collections.Generic;
using Features.StatsUsageModule.Scripts;

namespace Features.RumModule.Scripts
{
	[Serializable]
	public class RumData
	{
		public RumType RumType;

		public List<ModifierStatsData> RumModifiers;

		public bool WithDuration;

		public bool PerLevel;

		public float Duration;

		public bool HideFromBuffUi;
	}
}
