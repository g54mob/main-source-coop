using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;

namespace Features.GameOverModule.Scripts.Views
{
	[PublicAPI]
	public class GameOverPresenter : PresenterBehaviour<GameOverViewBase>
	{
		private readonly INavigationService _navigationService;

		public GameOverPresenter(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			base.View.OnFocused += OnFocused;
			SelectFirstNavigation();
		}

		protected override void OnViewDisabled()
		{
			base.View.OnFocused -= OnFocused;
			base.OnViewDisabled();
		}

		private void OnFocused()
		{
			SelectFirstNavigation();
		}

		private void SelectFirstNavigation()
		{
			if (base.View.FirstButtonToSelect != null)
			{
				_navigationService.SetNavigationToObject(base.View.FirstButtonToSelect);
			}
		}
	}
}
