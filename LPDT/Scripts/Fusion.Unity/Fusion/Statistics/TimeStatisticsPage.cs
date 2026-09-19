using System.Collections.Generic;
using UnityEngine;

namespace Fusion.Statistics
{
	public class TimeStatisticsPage : FusionStatisticsPage
	{
		[SerializeField]
		private LineChart _rtt;

		[SerializeField]
		private LineChart _inputReceiveDelta;

		[SerializeField]
		private LineChart _timeResets;

		[SerializeField]
		private LineChart _stateReceiveDelta;

		[SerializeField]
		private LineChart _simulationTimeOffset;

		[SerializeField]
		private LineChart _simulationSpeed;

		[SerializeField]
		private LineChart _interpolationOffset;

		[SerializeField]
		private LineChart _interpolationSpeed;

		[SerializeField]
		private LineChart _inputDelay;

		private float _lastRTT;

		public override string PageName => "Time Statistics";

		public override void Init()
		{
			_rtt.Setup("RTT", FusionStatsLookup.LOOKUP_TABLE_0ms, "{0}ms", forcePerUpdate: true);
			_inputReceiveDelta.Setup("Input Receive Delta", FusionStatsLookup.LOOKUP_TABLE_0ms, "{0:0}ms");
			_timeResets.Setup("Time Resets", FusionStatsLookup.LOOKUP_TABLE_0ms, "{0:0}ms");
			_stateReceiveDelta.Setup("State Receive Delta", FusionStatsLookup.LOOKUP_TABLE_0ms, "{0:0}ms");
			_simulationTimeOffset.Setup("Simulation Time Offset", FusionStatsLookup.LOOKUP_TABLE_0ms, "{0:0}ms");
			_simulationSpeed.Setup("Simulation Speed", null, "{0:f2}x", forcePerUpdate: true);
			_interpolationOffset.Setup("Interpolation Offset", FusionStatsLookup.LOOKUP_TABLE_0ms, "{0:0}ms");
			_interpolationSpeed.Setup("Interpolation Speed", null, "{0:f2}x", forcePerUpdate: true);
			_inputDelay.Setup("Input Delay", FusionStatsLookup.LOOKUP_TABLE_0ms, "{0:0}ms");
		}

		public override void Render()
		{
			_rtt.RefreshDisplay();
			_inputReceiveDelta.RefreshDisplay();
			_timeResets.RefreshDisplay();
			_stateReceiveDelta.RefreshDisplay();
			_simulationTimeOffset.RefreshDisplay();
			_simulationSpeed.RefreshDisplay();
			_interpolationOffset.RefreshDisplay();
			_interpolationSpeed.RefreshDisplay();
			_inputDelay.RefreshDisplay();
		}

		public override void AfterFusionUpdate()
		{
			float value = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.InputReceiveDelta, 0f) * 1000f;
			float value2 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.TimeResets, 0f) * 1000f;
			float value3 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.StateReceiveDelta, 0f) * 1000f;
			float value4 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.SimulationTimeOffset, 0f) * 1000f;
			float valueOrDefault = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.SimulationSpeed, 0f);
			float value5 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.InterpolationOffset, 0f) * 1000f;
			float valueOrDefault2 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.InterpolationSpeed, 0f);
			float value6 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.SimulationInputDelay, 0f) * 1000f;
			float num = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.RoundTripTime, 0f) * 1000f;
			if (num == 0f)
			{
				num = _lastRTT;
			}
			_lastRTT = num;
			_rtt.AddValue(num);
			_inputReceiveDelta.AddValue(value);
			_timeResets.AddValue(value2);
			_stateReceiveDelta.AddValue(value3);
			_simulationTimeOffset.AddValue(value4);
			_simulationSpeed.AddValue(valueOrDefault);
			_interpolationOffset.AddValue(value5);
			_interpolationSpeed.AddValue(valueOrDefault2);
			_inputDelay.AddValue(value6);
		}
	}
}
