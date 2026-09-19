using System;

namespace Features.LineArmModule.Scripts.Data
{
	public class LineArmGrabSettingsModel
	{
		public bool IsLatchGrabEnabled { get; private set; }

		public event Action<bool> OnLatchGrabEnabledChanged;

		public void SetLatchGrabEnabled(bool isEnabled)
		{
			if (IsLatchGrabEnabled != isEnabled)
			{
				IsLatchGrabEnabled = isEnabled;
				this.OnLatchGrabEnabledChanged?.Invoke(IsLatchGrabEnabled);
			}
		}
	}
}
