using System;
using Features.SettingsMenuModule.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.HubModule.Scripts.Hub.Views
{
	[PublicAPI]
	public class GlobalSettingsButtonsPresenter : PresenterBehaviour<GlobalSettingsButtonsViewBase>
	{
		private readonly ISettingsWindowProvider _settingsWindowProvider;

		public GlobalSettingsButtonsPresenter(ISettingsWindowProvider settingsWindowProvider)
		{
			_settingsWindowProvider = settingsWindowProvider;
		}

		protected override void OnViewSet()
		{
			GlobalSettingsButtonsViewBase view = base.View;
			view.OnClickSettings = (Action)Delegate.Combine(view.OnClickSettings, new Action(OpenSettingsWindow));
		}

		protected override void OnDisposed()
		{
			GlobalSettingsButtonsViewBase view = base.View;
			view.OnClickSettings = (Action)Delegate.Remove(view.OnClickSettings, new Action(OpenSettingsWindow));
		}

		private void OpenSettingsWindow()
		{
			ProcessWindow(_settingsWindowProvider.GetSettingsWindow(base.View.IsMenu));
		}

		private static void ProcessWindow(FocusableWindowBehaviour window)
		{
			switch (window.WindowStatus)
			{
			case WindowStatus.Closed:
				window.Open();
				break;
			case WindowStatus.Hidden:
				window.Show();
				break;
			}
		}
	}
}
