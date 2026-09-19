using Features.ConfirmExitPopupService.Scripts;
using Global.Modules.LocalizationModule.Scripts.Generated;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.QuiteGameModule.Scripts
{
	[PublicAPI]
	public class QuiteButtonPresenter : PresenterBehaviour<QuiteButtonViewBase>
	{
		private readonly IConfirmExitService _confirmExitService;

		public QuiteButtonPresenter(IConfirmExitService confirmExitService)
		{
			_confirmExitService = confirmExitService;
		}

		protected override void OnViewSet()
		{
			base.View.Button.onClick.AddListener(ShowConfirmWindow);
		}

		protected override void OnDisposed()
		{
			base.View.Button.onClick.RemoveListener(ShowConfirmWindow);
		}

		private void ShowConfirmWindow()
		{
			_confirmExitService.ShowPopup(QuiteGame, new ConfirmExitData
			{
				TitleLocalizationKey = LocalizationKey.UI_WantLeaveGamePopup_Title,
				DescriptionLocalizationKey = LocalizationKey.UI_WantLeaveGamePopup_Description
			});
		}

		private void QuiteGame()
		{
			base.View.Button.onClick.RemoveListener(QuiteGame);
			Application.Quit();
		}
	}
}
