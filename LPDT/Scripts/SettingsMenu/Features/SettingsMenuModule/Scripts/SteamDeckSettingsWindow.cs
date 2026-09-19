using Features.UINavigationModuleRealization.Scripts.BackButton;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts
{
	public class SteamDeckSettingsWindow : SettingsWindow
	{
		public SteamDeckSettingsWindow(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService, IUIBackButtonRegistrationService backButtonRegistrationService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService, focusablesService, backButtonRegistrationService)
		{
		}
	}
}
