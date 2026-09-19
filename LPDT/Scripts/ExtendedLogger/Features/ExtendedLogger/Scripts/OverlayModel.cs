using System;
using System.Collections.Generic;

namespace Features.ExtendedLogger.Scripts
{
	public class OverlayModel
	{
		public readonly Dictionary<DebugFilterType, string> CurrentOverlays = new Dictionary<DebugFilterType, string>();

		public event Action<DebugFilterType> OnOverlaySwitched;

		public void InvokeOnOverlaySwitched(DebugFilterType debugFilterType)
		{
			this.OnOverlaySwitched?.Invoke(debugFilterType);
		}
	}
}
