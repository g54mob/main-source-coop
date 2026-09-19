using System;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts.HubSettings
{
	public class HubSettingsWindow : FocusableWindowBehaviour, IBackButtonProcessor, IInitializable, IDisposable
	{
		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		public BackButtonProcessorType Type => BackButtonProcessorType.Settings;

		public HubSettingsWindow(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService, IUIBackButtonRegistrationService backButtonRegistrationService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService, focusablesService)
		{
			_backButtonRegistrationService = backButtonRegistrationService;
		}

		public bool CanHandleBack()
		{
			return true;
		}

		protected override void OnOpened()
		{
			base.OnOpened();
			_backButtonRegistrationService.Register(this);
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
