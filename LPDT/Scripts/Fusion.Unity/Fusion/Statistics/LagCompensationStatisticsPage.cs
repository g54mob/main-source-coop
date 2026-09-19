using UnityEngine;

namespace Fusion.Statistics
{
	public class LagCompensationStatisticsPage : FusionStatisticsPage
	{
		[SerializeField]
		private RadialChart _hitboxesUsage;

		[Space]
		[SerializeField]
		private LineChart _totalElapsedTime;

		[SerializeField]
		private LineChart _advanceBufferTime;

		[SerializeField]
		private LineChart _updateBufferTime;

		[SerializeField]
		private LineChart _addOnBufferTime;

		[SerializeField]
		private LineChart _refitBVHTime;

		[SerializeField]
		private LineChart _updateBVHTime;

		[SerializeField]
		private LineChart _addOnBVHTime;

		public override string PageName => "Lag Compensation";

		public override void Init()
		{
			_hitboxesUsage.Setup("Hitboxes Usage");
			_totalElapsedTime.Setup("Total Elapsed Time", FusionStatsLookup.LOOKUP_TABLE_0_00ms, "{0} ms", forcePerUpdate: false, 100f);
			_advanceBufferTime.Setup("Advance Buffer Time", FusionStatsLookup.LOOKUP_TABLE_0_00ms, "{0} ms", forcePerUpdate: false, 100f);
			_updateBufferTime.Setup("Update Buffer Time", FusionStatsLookup.LOOKUP_TABLE_0_00ms, "{0} ms", forcePerUpdate: false, 100f);
			_addOnBufferTime.Setup("Add on Buffer Time", FusionStatsLookup.LOOKUP_TABLE_0_00ms, "{0} ms", forcePerUpdate: false, 100f);
			_refitBVHTime.Setup("Refit BVH Time", FusionStatsLookup.LOOKUP_TABLE_0_00ms, "{0} ms", forcePerUpdate: false, 100f);
			_updateBVHTime.Setup("Update BVH Time", FusionStatsLookup.LOOKUP_TABLE_0_00ms, "{0} ms", forcePerUpdate: false, 100f);
			_addOnBVHTime.Setup("Add on BVH Time", FusionStatsLookup.LOOKUP_TABLE_0_00ms, "{0} ms", forcePerUpdate: false, 100f);
		}

		public override void Render()
		{
			_totalElapsedTime.RefreshDisplay();
			_advanceBufferTime.RefreshDisplay();
			_updateBufferTime.RefreshDisplay();
			_addOnBufferTime.RefreshDisplay();
			_refitBVHTime.RefreshDisplay();
			_updateBVHTime.RefreshDisplay();
			_addOnBVHTime.RefreshDisplay();
			_hitboxesUsage.RefreshDisplay();
		}

		public override void AfterFusionUpdate()
		{
			LagCompensationStatisticsSnapshot lagCompensationSnapshot = base.StatisticsManager.LagCompensationSnapshot;
			if (lagCompensationSnapshot != null)
			{
				_totalElapsedTime.AddValue((float)lagCompensationSnapshot.TotalElapsedTime);
				_advanceBufferTime.AddValue((float)lagCompensationSnapshot.AdvanceBufferTime);
				_updateBufferTime.AddValue((float)lagCompensationSnapshot.UpdateBufferTime);
				_addOnBufferTime.AddValue((float)lagCompensationSnapshot.AddOnBufferTime);
				_refitBVHTime.AddValue((float)lagCompensationSnapshot.RefitBVHTime);
				_updateBVHTime.AddValue((float)lagCompensationSnapshot.UpdateBVHTime);
				_addOnBVHTime.AddValue((float)lagCompensationSnapshot.AddOnBVHTime);
				_hitboxesUsage.SetValue(lagCompensationSnapshot.HitboxesCount, base.Runner.Config.LagCompensation.HitboxDefaultCapacity);
			}
		}
	}
}
