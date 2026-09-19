using System;
using Fusion.Photon.Realtime;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public abstract class RegionInfoButtonViewBase : ViewBehaviour
	{
		public Action OnClickSettings;

		protected void InvokeOnClickSettings()
		{
			OnClickSettings?.Invoke();
		}

		public abstract void SetActualRegionPing(RegionInfo regionInfo);

		public abstract void SetRegionButtonInteractable(bool interactable);
	}
}
