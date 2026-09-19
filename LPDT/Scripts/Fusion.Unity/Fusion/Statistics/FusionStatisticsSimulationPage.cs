using System.Collections.Generic;
using UnityEngine;

namespace Fusion.Statistics
{
	public class FusionStatisticsSimulationPage : FusionStatisticsPage
	{
		[Header("References")]
		[SerializeField]
		private LineChart _forwardTick;

		[SerializeField]
		private LineChart _resimTick;

		[SerializeField]
		private LineChart _objUpdateIn;

		[SerializeField]
		private LineChart _objUpdateOut;

		public override string PageName => "Simulation";

		public override void Init()
		{
			_forwardTick.Setup("Forward Ticks", FusionStatsLookup.LOOKUP_TABLE_0);
			_resimTick.Setup("Re-simulation Ticks", FusionStatsLookup.LOOKUP_TABLE_0);
			_objUpdateIn.Setup("Object Update In", FusionStatsLookup.LOOKUP_TABLE_0);
			_objUpdateOut.Setup("Object Update Out", FusionStatsLookup.LOOKUP_TABLE_0);
		}

		public override void Render()
		{
			_forwardTick.RefreshDisplay();
			_resimTick.RefreshDisplay();
			_objUpdateIn.RefreshDisplay();
			_objUpdateOut.RefreshDisplay();
		}

		public override void AfterFusionUpdate()
		{
			float valueOrDefault = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.ForwardTicks, 0f);
			float valueOrDefault2 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.Resimulations, 0f);
			float valueOrDefault3 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.InObjectUpdates, 0f);
			float valueOrDefault4 = base.StatisticsManager.SimulationSnapshot.Stats.GetValueOrDefault(FusionStatType.OutObjectUpdates, 0f);
			_forwardTick.AddValue(valueOrDefault);
			_resimTick.AddValue(valueOrDefault2);
			_objUpdateIn.AddValue(valueOrDefault3);
			_objUpdateOut.AddValue(valueOrDefault4);
		}
	}
}
