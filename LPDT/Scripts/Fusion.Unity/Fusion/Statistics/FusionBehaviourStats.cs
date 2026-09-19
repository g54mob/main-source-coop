using System;
using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	public class FusionBehaviourStats : MonoBehaviour
	{
		[SerializeField]
		private Text _name;

		[SerializeField]
		private Text _runCount;

		[SerializeField]
		private Text _time;

		private StatAccumulator _runCountAccum;

		private StatAccumulator _execTimeAccum;

		private Type _behaviour;

		private FusionBehaviourStatisticsPage _behaviourPage;

		public Type BehaviourType => _behaviour;

		public void Setup(Type BehaviourType, FusionBehaviourStatisticsPage behaviourPage)
		{
			_name.text = BehaviourType.Name;
			_behaviour = BehaviourType;
			_behaviourPage = behaviourPage;
			_runCountAccum.DisplayingPerSecond = true;
			_execTimeAccum.DisplayingPerSecond = true;
		}

		public void AccumulateRunAndTime(FusionBehaviourStatisticsPage statisticsPage)
		{
			if (statisticsPage.Runner.TryGetBehaviourStatistics(_behaviour, out var behaviourStatisticsSnapshot))
			{
				_runCountAccum.Accumulate(statisticsPage.DisplayingFun ? behaviourStatisticsSnapshot.FixedUpdateNetworkExecutionCount : behaviourStatisticsSnapshot.RenderExecutionCount);
				_execTimeAccum.Accumulate((float)(statisticsPage.DisplayingFun ? behaviourStatisticsSnapshot.FixedUpdateNetworkExecutionTime : behaviourStatisticsSnapshot.RenderExecutionTime));
			}
		}

		public void RefreshView()
		{
			float value = (_runCountAccum.DisplayingPerSecond ? _runCountAccum.ValuePerSecond : _runCountAccum.Value);
			float value2 = (_execTimeAccum.DisplayingPerSecond ? _execTimeAccum.ValuePerSecond : _execTimeAccum.Value);
			_runCount.text = FusionStatsLookup.GetValueText(value, FusionStatsLookup.LOOKUP_TABLE_0, "{0}");
			_time.text = FusionStatsLookup.GetValueText(value2, FusionStatsLookup.LOOKUP_TABLE_0_00ms, "{0} ms", 100f);
		}

		public void DeleteBehaviourStat()
		{
			_behaviourPage.DeleteStat(this);
		}
	}
}
