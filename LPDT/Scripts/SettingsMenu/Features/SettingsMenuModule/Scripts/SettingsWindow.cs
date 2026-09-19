using System;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsWindow : FocusableWindowBehaviour, IBackButtonProcessor, IInitializable, IDisposable
	{
		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		public BackButtonProcessorType Type => BackButtonProcessorType.Settings;

		public SettingsWindow(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService, IUIBackButtonRegistrationService backButtonRegistrationService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService, focusablesService)
		{
			_backButtonRegistrationService = backButtonRegistrationService;
		}

		public bool CanHandleBack()
		{
			return base.WindowStatus == WindowStatus.Showed;
		}

		protected override void OnOpened()
		{
			base.OnOpened();
			_backButtonRegistrationService.Register(this);
			GetPresenter<SettingsPresenter>().PlayOpenAnimation();
		}

		protected override void OnClosed()
		{
			_backButtonRegistrationService.Unregister(this);
			base.OnClosed();
		}

		public override void OnBack()
		{
			GetPresenter<SettingsPresenter>().CloseSettingsWindowWithCheck();
		}
	}
}
