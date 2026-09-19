using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Statistics
{
	public class FusionStatisticsNetworkObjectStats : MonoBehaviour
	{
		[SerializeField]
		private LineChart _inB;

		[SerializeField]
		private LineChart _outB;

		[SerializeField]
		private Text _title;

		[SerializeField]
		private Button _closeButton;

		public NetworkId ID;

		private float _timer;

		public void Setup(FusionStatisticsNetworkObjectPage objectPage, string title, NetworkId id)
		{
			_inB.Setup("In Bandwidth", FusionStatsLookup.LOOKUP_TABLE_0_BYTES, "{0} B");
			_outB.Setup("Out Bandwidth", FusionStatsLookup.LOOKUP_TABLE_0_BYTES, "{0} B");
			_title.text = title;
			ID = id;
			_closeButton.onClick.RemoveAllListeners();
			_closeButton.onClick.AddListener(delegate
			{
				objectPage.RemoveMonitoredNetworkObject(this);
			});
		}

		public void SetData(FusionStatisticsManager statisticsManager)
		{
			if (!statisticsManager.ObjectSnapshot.NetworkObjectStatistics.TryGetValue(ID, out var value))
			{
				_inB.AddValue(0f);
				_outB.AddValue(0f);
				return;
			}
			if (value.TryGetValue(FusionObjectStatType.InBandwidth, out var value2))
			{
				_inB.AddValue(value2);
			}
			if (value.TryGetValue(FusionObjectStatType.OutBandwidth, out var value3))
			{
				_outB.AddValue(value3);
			}
		}

		private void Update()
		{
			if (_timer > 0f)
			{
				_timer -= Time.deltaTime;
			}
		}

		public void RefreshView()
		{
			_inB.RefreshDisplay();
			_outB.RefreshDisplay();
		}
	}
}
