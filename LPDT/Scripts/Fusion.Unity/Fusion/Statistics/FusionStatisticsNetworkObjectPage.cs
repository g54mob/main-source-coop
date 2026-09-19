using System.Collections.Generic;
using UnityEngine;

namespace Fusion.Statistics
{
	public class FusionStatisticsNetworkObjectPage : FusionStatisticsPage
	{
		[Header("References")]
		[SerializeField]
		private FusionStatisticsNetworkObjectStats _prefabNOStats;

		[SerializeField]
		private MultipleOptionsPanel _multipleOptionsPrefab;

		[SerializeField]
		private Transform _content;

		private MultipleOptionsPanel _NoOptionsInstance;

		private List<FusionStatisticsNetworkObjectStats> _networkObjectStats = new List<FusionStatisticsNetworkObjectStats>();

		public override string PageName => "Network Object";

		public void MonitorObject(NetworkId networkId)
		{
			if (!base.StatisticsManager.IsObjectMonitored(networkId))
			{
				FusionStatisticsNetworkObjectStats fusionStatisticsNetworkObjectStats = Object.Instantiate(_prefabNOStats, _content);
				fusionStatisticsNetworkObjectStats.Setup(this, base.Runner.FindObject(networkId).Name, networkId);
				_networkObjectStats.Add(fusionStatisticsNetworkObjectStats);
			}
		}

		public void RemoveMonitoredNetworkObject(FusionStatisticsNetworkObjectStats stats)
		{
			_networkObjectStats.Remove(stats);
			Object.Destroy(stats.gameObject);
		}

		public void SearchAllNetworkObjects()
		{
			if (!_NoOptionsInstance)
			{
				NetworkObject[] options = base.Runner.GetAllNetworkObjects().ToArray();
				_NoOptionsInstance = Object.Instantiate(_multipleOptionsPrefab, FusionStatistics.GlobalStatisticsCanvas.transform);
				_NoOptionsInstance.Setup("Select Object", options, (NetworkObject no) => no.Name, delegate(NetworkObject no)
				{
					MonitorObject(no.Id);
				});
			}
		}

		public override void Init()
		{
		}

		public override void Render()
		{
			foreach (FusionStatisticsNetworkObjectStats networkObjectStat in _networkObjectStats)
			{
				networkObjectStat.RefreshView();
			}
		}

		public override void AfterFusionUpdate()
		{
			List<FusionStatisticsNetworkObjectStats> list = new List<FusionStatisticsNetworkObjectStats>();
			foreach (FusionStatisticsNetworkObjectStats networkObjectStat in _networkObjectStats)
			{
				if (!base.Runner.Exists(networkObjectStat.ID))
				{
					list.Add(networkObjectStat);
				}
				else
				{
					networkObjectStat.SetData(base.StatisticsManager);
				}
			}
			foreach (FusionStatisticsNetworkObjectStats item in list)
			{
				_networkObjectStats.Remove(item);
				Object.Destroy(item.gameObject);
			}
		}
	}
}
