using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts.SettingViews
{
	public class CloseGammaWindowPresenter : PresenterBehaviour<CloseGammaWindowViewBase>
	{
		private readonly IWindowsService _windowsService;

		public CloseGammaWindowPresenter(IWindowsService windowsService)
		{
			_windowsService = windowsService;
		}

		protected override void OnViewSet()
		{
			base.View.OnButtonClicked += HandleButtonClicked;
		}

		protected override void OnDisposed()
		{
			base.View.OnButtonClicked -= HandleButtonClicked;
		}

		private void HandleButtonClicked()
		{
			_windowsService.CloseWindow<GammaSetupWindow>();
		}
	}
}
