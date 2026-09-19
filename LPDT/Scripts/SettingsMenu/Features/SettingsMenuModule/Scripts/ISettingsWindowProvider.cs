using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SettingsMenuModule.Scripts
{
	public interface ISettingsWindowProvider
	{
		FocusableWindowBehaviour GetSettingsWindow(bool isMenu);

		bool IsAnySettingsWindowShown();
	}
}
