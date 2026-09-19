using UnityEngine;

namespace Features.VSyncServiceModule.Scripts
{
	public class VsyncService : IVsyncService
	{
		public void SetVSync(bool enabled)
		{
			QualitySettings.vSyncCount = (enabled ? 1 : 0);
		}
	}
}
