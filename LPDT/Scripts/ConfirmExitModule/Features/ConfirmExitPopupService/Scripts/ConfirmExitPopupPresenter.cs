using System;
using Cysharp.Threading.Tasks;
using Features.UIAnimationsModule.Scripts;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using Global.Modules.Localization_Module.Scripts;
using JetBrains.Annotations;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts;

namespace Features.ConfirmExitPopupService.Scripts
{
	[PublicAPI]
	public class ConfirmExitPopupPresenter : PresenterBehaviour<ConfirmExitPopupViewBase>, IBackButtonProcessor
	{
		private readonly ConfirmPopupModel _model;

		private readonly ILocalizationService _localizationService;

		private readonly INavigationService _navigationService;

		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		private readonly IUIAnimationService _uiAnimationService;

		public BackButtonProcessorType Type => BackButtonProcessorType.Popup;

		public ConfirmExitPopupPresenter(ConfirmPopupModel model, ILocalizationService localizationService, INavigationService navigationService, IUIBackButtonRegistrationService backButtonRegistrationService, IUIAnimationService uiAnimationService)
		{
			_model = model;
			_localizationService = localizationService;
			_navigationService = navigationService;
			_backButtonRegistrationService = backButtonRegistrationService;
			_uiAnimationService = uiAnimationService;
		}

		public bool CanHandleBack()
		{
			return _model.IsOpen;
		}

		public void OnBack()
		{
			if (_model.IsOpen)
			{
				CancelLeave();
			}
		}

		protected override void OnViewEnabled()
		{
			base.OnViewEnabled();
			SelectFirstNavigationButton();
		}

		protected override void OnViewSet()
		{
			base.OnViewSet();
			base.View.SetVisible(_model.IsOpen);
			ApplyLocalization();
			base.View.OnNoClicked += CancelLeave;
			base.View.OnYesClicked += ConfirmLeave;
			_model.OnOpenChanged += OnModelOpenChanged;
			if (_model.IsOpen)
			{
				_backButtonRegistrationService.Register(this);
			}
		}

		protected override void OnDisposed()
		{
			base.OnDisposed();
			base.View.OnNoClicked -= CancelLeave;
			base.View.OnYesClicked -= ConfirmLeave;
			_model.OnOpenChanged -= OnModelOpenChanged;
			_backButtonRegistrationService.Unregister(this);
		}

		private void SelectFirstNavigationButton()
		{
			if (base.View.FirstButtonToSelect != null)
			{
				_navigationService.SetNavigationToObject(base.View.FirstButtonToSelect);
			}
		}

		private void ApplyLocalization()
		{
			base.View.SetDescriptionText(GetLocalizedDescription());
			base.View.SetTitleText(GetLocalizedTitle());
		}

		private void OnModelOpenChanged()
		{
			base.View.SetVisible(_model.IsOpen);
			if (_model.IsOpen)
			{
				ApplyLocalization();
				PlayOpenAnimation();
				_backButtonRegistrationService.Register(this);
				SelectFirstNavigationButton();
			}
			else
			{
				_backButtonRegistrationService.Unregister(this);
			}
		}

		private void ConfirmLeave()
		{
			CloseAndRunAsync(_model.ConfirmYes).Forget();
		}

		private void CancelLeave()
		{
			CloseAndRunAsync(_model.Cancel).Forget();
		}

		private async UniTask CloseAndRunAsync(Action closeAction)
		{
			if (_model.IsOpen)
			{
				await PlayCloseAnimationAsync();
				closeAction();
			}
		}

		private void PlayOpenAnimation()
		{
			_uiAnimationService.Play(base.View.AnimationTarget, UIAnimationKey.PopupShow);
		}

		private UniTask PlayCloseAnimationAsync()
		{
			return _uiAnimationService.PlayAsync(base.View.AnimationTarget, UIAnimationKey.PopupHide);
		}

		private string GetLocalizedDescription()
		{
			return _localizationService.GetLocalizedString(_model.Data.DescriptionLocalizationKey);
		}

		private string GetLocalizedTitle()
		{
			return _localizationService.GetLocalizedString(_model.Data.TitleLocalizationKey);
		}
	}
}
