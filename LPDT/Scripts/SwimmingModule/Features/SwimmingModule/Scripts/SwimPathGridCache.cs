using System.Collections.Generic;
using Features.GameCycle.Scripts.SessionCleanup;
using UnityEngine;

namespace Features.SwimmingModule.Scripts
{
	public sealed class SwimPathGridCache : ISessionCleanup
	{
		private readonly Dictionary<int, SwimWaterGrid> _grids = new Dictionary<int, SwimWaterGrid>();

		public bool TryGet(GameObject waterSource, out SwimWaterGrid grid)
		{
			grid = null;
			if (waterSource == null)
			{
				return false;
			}
			return _grids.TryGetValue(waterSource.GetInstanceID(), out grid);
		}

		public void Set(SwimWaterGrid grid)
		{
			if (grid != null && !(grid.WaterSource == null))
			{
				_grids[grid.WaterSource.GetInstanceID()] = grid;
			}
		}

		public void Cleanup()
		{
			_grids.Clear();
		}
	}
}
