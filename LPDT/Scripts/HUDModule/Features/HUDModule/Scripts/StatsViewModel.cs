using System;

namespace Features.HUDModule.Scripts
{
	public class StatsViewModel
	{
		public event Action<bool> OnStatsVisibilityChanged;

		public void ChangeStatsVisibility(bool visible)
		{
			this.OnStatsVisibilityChanged?.Invoke(visible);
		}
	}
}
