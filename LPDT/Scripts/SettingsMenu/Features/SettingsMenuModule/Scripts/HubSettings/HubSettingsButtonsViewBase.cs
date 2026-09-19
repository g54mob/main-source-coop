using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public abstract class HubSettingsButtonsViewBase : ViewBehaviour
	{
		public Action OnClickSettings;

		protected void InvokeOnClickSettings()
		{
			OnClickSettings?.Invoke();
		}

		public abstract void SetOpenSettingsButtonInteractable(bool interactable);
	}
}
