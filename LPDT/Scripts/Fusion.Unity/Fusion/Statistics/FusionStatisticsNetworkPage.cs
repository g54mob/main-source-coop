using System.Collections.Generic;
using UnityEngine;

namespace Fusion.Statistics
{
	public class FusionStatisticsNetworkPage : FusionStatisticsPage
	{
		[Header("References")]
		[SerializeField]
		private LineChart _rtt;

		[SerializeField]
		private LineChart _inBandwidth;

		[SerializeField]
		private LineChart _outBandwidth;

		[SerializeField]
		private LineChart _inPackets;

		[SerializeField]
		private LineChart _outPackets;

		[SerializeField]
		private LineChart _inputInBandwidth;

		[SerializeField]
		private LineChart _inputOutBandwidth;

		private float _lastRTT;

		public override string PageName => "Network";

		public override void Init()
		{
			string labelFormat = "{0} B";
			string[][] lOOKUP_TABLE_ = FusionStatsLookup.LOOKUP_TABLE_0;
			string[][] lOOKUP_TABLE_0_BYTES = FusionStatsLookup.LOOKUP_TABLE_0_BYTES;
			_rtt.Setup("RTT", FusionStatsLookup.LOOKUP_TABLE_0ms, "{0} ms", forcePerUpdate: true);
			_inBandwidth.Setup("In Bandwidth", lOOKUP_TABLE_0_BYTES, labelFormat);
			_outBandwidth.Setup("Out Bandwidth", lOOKUP_TABLE_0_BYTES, labelFormat);
			_inPackets.Setup("In Packets", lOOKUP_TABLE_);
			_outPackets.Setup("Out Packets", lOOKUP_TABLE_);
			_inputInBandwidth.Setup("Input In Bandwidth", lOOKUP_TABLE_0_BYTES, labelFormat);
			_inputOutBandwidth.Setup("Input Out Bandwidth", lOOKUP_TABLE_0_BYTES, labelFormat);
		}

		public override void Render()
		{
			_rtt.RefreshDisplay();
			_inBandwidth.RefreshDisplay();
			_outBandwidth.RefreshDisplay();
			_inPackets.RefreshDisplay();
			_outPackets.RefreshDisplay();
			_inputInBandwidth.RefreshDisplay();
			_inputOutBandwidth.RefreshDisplay();
		}

		public override void AfterFusionUpdate()
		{
			float num = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.RoundTripTime, 0f);
			float valueOrDefault = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.InBandwidth, 0f);
			float valueOrDefault2 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.OutBandwidth, 0f);
			float valueOrDefault3 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.InPackets, 0f);
			float valueOrDefault4 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.OutPackets, 0f);
			float valueOrDefault5 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.InputInBandwidth, 0f);
			float valueOrDefault6 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.InputOutBandwidth, 0f);
			if (num == 0f)
			{
				num = _lastRTT;
			}
			_lastRTT = num;
			num *= 1000f;
			_rtt.AddValue(num);
			_inBandwidth.AddValue(valueOrDefault);
			_outBandwidth.AddValue(valueOrDefault2);
			_inPackets.AddValue(valueOrDefault3);
			_outPackets.AddValue(valueOrDefault4);
			_inputInBandwidth.AddValue(valueOrDefault5);
			_inputOutBandwidth.AddValue(valueOrDefault6);
		}
	}
}
