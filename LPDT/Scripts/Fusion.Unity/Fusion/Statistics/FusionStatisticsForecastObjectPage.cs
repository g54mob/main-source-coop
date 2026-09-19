using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fusion.Statistics
{
	public class FusionStatisticsForecastObjectPage : FusionStatisticsPage
	{
		[Header("References")]
		[SerializeField]
		private FusionStatisticsForecastObjectStats _prefabNOStats;

		[SerializeField]
		private MultipleOptionsPanel _multipleOptionsPrefab;

		[SerializeField]
		private Transform _content;

		private MultipleOptionsPanel _NoOptionsInstance;

		private List<FusionStatisticsForecastObjectStats> _forecastedObjectStats = new List<FusionStatisticsForecastObjectStats>();

		public override string PageName => "Forecast Object";

		public void MonitorObject(NetworkTransform nt)
		{
			FusionStatisticsForecastObjectStats fusionStatisticsForecastObjectStats = Object.Instantiate(_prefabNOStats, _content);
			fusionStatisticsForecastObjectStats.Setup(this, nt.name, nt.Object.Id);
			_forecastedObjectStats.Add(fusionStatisticsForecastObjectStats);
		}

		public void RemoveMonitoredNetworkObject(FusionStatisticsForecastObjectStats stats)
		{
			_forecastedObjectStats.Remove(stats);
			base.Runner.TryGetNetworkedBehaviourFromNetworkedObjectRef<NetworkTransform>(stats.ID);
			Object.Destroy(stats.gameObject);
		}

		public void SearchAllNetworkObjects()
		{
			if (!_NoOptionsInstance)
			{
				NetworkTransform[] options = (from obj in base.Runner.GetAllBehaviours<NetworkTransform>()
					where obj.HasForecastEnabled
					select obj).ToArray();
				_NoOptionsInstance = Object.Instantiate(_multipleOptionsPrefab, FusionStatistics.GlobalStatisticsCanvas.transform);
				_NoOptionsInstance.Setup("Select Object", options, (NetworkTransform nt) => nt.gameObject.name, delegate(NetworkTransform nt)
				{
					MonitorObject(nt);
				});
			}
		}

		public override void Init()
		{
		}

		public override void Render()
		{
			foreach (FusionStatisticsForecastObjectStats forecastedObjectStat in _forecastedObjectStats)
			{
				forecastedObjectStat.RefreshView();
			}
		}

		public override void AfterFusionUpdate()
		{
			List<FusionStatisticsForecastObjectStats> list = new List<FusionStatisticsForecastObjectStats>();
			foreach (FusionStatisticsForecastObjectStats forecastedObjectStat in _forecastedObjectStats)
			{
				if (!base.Runner.Exists(forecastedObjectStat.ID))
				{
					list.Add(forecastedObjectStat);
				}
				else
				{
					forecastedObjectStat.SetData(base.StatisticsManager);
				}
			}
			foreach (FusionStatisticsForecastObjectStats item in list)
			{
				_forecastedObjectStats.Remove(item);
				Object.Destroy(item.gameObject);
			}
		}
	}
}
