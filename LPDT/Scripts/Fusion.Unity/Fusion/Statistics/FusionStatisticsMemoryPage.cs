using UnityEngine;

namespace Fusion.Statistics
{
	public class FusionStatisticsMemoryPage : FusionStatisticsPage
	{
		[Header("Object Memory")]
		[SerializeField]
		private RadialChart _objectMemoryChart;

		[SerializeField]
		private RadialChart _objectFreeBlocksChart;

		[Space]
		[Header("General Memory")]
		[SerializeField]
		private RadialChart _generalMemoryChart;

		[SerializeField]
		private RadialChart _generalFreeBlocksChart;

		public override string PageName => "Memory";

		public override void Init()
		{
			_objectMemoryChart.Setup("Memory Usage");
			_objectFreeBlocksChart.Setup("Blocks Usage");
			_generalMemoryChart.Setup("Memory Usage");
			_generalFreeBlocksChart.Setup("Blocks Usage");
		}

		public override void Render()
		{
			_objectMemoryChart.RefreshDisplay();
			_objectFreeBlocksChart.RefreshDisplay();
			_generalMemoryChart.RefreshDisplay();
			_generalFreeBlocksChart.RefreshDisplay();
		}

		public override void AfterFusionUpdate()
		{
			FusionMemoryStatisticsSnapshot memorySnapshot = base.StatisticsManager.MemorySnapshot;
			int totalBytesUsed = memorySnapshot.ObjectAllocatorMemorySnapshot.TotalBytesUsed;
			int num = totalBytesUsed + memorySnapshot.ObjectAllocatorMemorySnapshot.TotalBytesFree;
			_objectMemoryChart.SetValue(totalBytesUsed, num);
			int totalBlocks = memorySnapshot.ObjectAllocatorMemorySnapshot.TotalBlocks;
			int num2 = totalBlocks - memorySnapshot.ObjectAllocatorMemorySnapshot.TotalFreeBlocks;
			_objectFreeBlocksChart.SetValue(num2, totalBlocks);
			int totalBytesUsed2 = memorySnapshot.GeneralAllocatorMemorySnapshot.TotalBytesUsed;
			int num3 = totalBytesUsed2 + memorySnapshot.GeneralAllocatorMemorySnapshot.TotalBytesFree;
			_generalMemoryChart.SetValue(totalBytesUsed2, num3);
			int totalBlocks2 = memorySnapshot.GeneralAllocatorMemorySnapshot.TotalBlocks;
			int num4 = totalBlocks2 - memorySnapshot.GeneralAllocatorMemorySnapshot.TotalFreeBlocks;
			_generalFreeBlocksChart.SetValue(num4, totalBlocks2);
		}
	}
}
