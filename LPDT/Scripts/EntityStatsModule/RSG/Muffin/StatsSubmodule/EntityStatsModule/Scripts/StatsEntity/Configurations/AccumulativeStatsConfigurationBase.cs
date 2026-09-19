using System.Collections.Generic;
using UnityEngine;

namespace RSG.Muffin.StatsSubmodule.EntityStatsModule.Scripts.StatsEntity.Configurations
{
	public class AccumulativeStatsConfigurationBase<TStatEnum> : ScriptableObject
	{
		public List<TStatEnum> AccumulativeStats;
	}
}
