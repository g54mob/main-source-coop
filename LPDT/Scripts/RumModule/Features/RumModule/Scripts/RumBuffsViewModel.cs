using System;

namespace Features.RumModule.Scripts
{
	public class RumBuffsViewModel
	{
		public event Action<bool> OnBuffsVisibilityChanged;

		public void ChangeBuffsVisibility(bool visible)
		{
			this.OnBuffsVisibilityChanged?.Invoke(visible);
		}
	}
}
