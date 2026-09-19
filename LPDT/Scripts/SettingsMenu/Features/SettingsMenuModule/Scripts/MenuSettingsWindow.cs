using Features.UINavigationModuleRealization.Scripts.BackButton;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts
{
	public class MenuSettingsWindow : SettingsWindow
	{
		public MenuSettingsWindow(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService, IUIBackButtonRegistrationService backButtonRegistrationService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService, focusablesService, backButtonRegistrationService)
		{
		}
	}
}
