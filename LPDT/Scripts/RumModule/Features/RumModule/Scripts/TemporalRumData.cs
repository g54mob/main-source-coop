using System;
using System.Collections.Generic;
using Features.StatsUsageModule.Scripts;

namespace Features.RumModule.Scripts
{
	[Serializable]
	public class TemporalRumData
	{
		public RumData RumData;

		public List<ModifierStatsData> ActivatedModifiers;

		public float Duration;

		public TemporalRumData(RumData rumData, List<ModifierStatsData> activatedModifiers)
		{
			RumData = rumData;
			ActivatedModifiers = activatedModifiers;
		}
	}
}
