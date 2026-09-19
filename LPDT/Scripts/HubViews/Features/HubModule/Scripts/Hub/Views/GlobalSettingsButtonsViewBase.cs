using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.HubModule.Scripts.Hub.Views
{
	public abstract class GlobalSettingsButtonsViewBase : ViewBehaviour
	{
		public Action OnClickSettings;

		public abstract bool IsMenu { get; }

		protected void InvokeOnClickSettings()
		{
			OnClickSettings?.Invoke();
		}
	}
}
