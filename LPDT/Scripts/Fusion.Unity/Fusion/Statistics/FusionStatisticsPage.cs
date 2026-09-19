using UnityEngine;

namespace Fusion.Statistics
{
	public abstract class FusionStatisticsPage : MonoBehaviour
	{
		public FusionStatisticsManager StatisticsManager { get; private set; }

		public FusionStatistics Statistics { get; private set; }

		public NetworkRunner Runner { get; private set; }

		public abstract string PageName { get; }

		public abstract void Init();

		public abstract void Render();

		public abstract void AfterFusionUpdate();

		public virtual void Open()
		{
			base.gameObject.SetActive(value: true);
		}

		public virtual void Close()
		{
			base.gameObject.SetActive(value: false);
		}

		internal void SetupPage(NetworkRunner runner, FusionStatisticsManager statisticsManager, FusionStatistics statistics)
		{
			StatisticsManager = statisticsManager;
			Statistics = statistics;
			Runner = runner;
			Init();
			Close();
		}
	}
}
