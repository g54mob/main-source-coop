using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.SceneTransitionsModule.Scripts.LoadingScreen
{
	public class LoadingScreenWindow : FocusableWindowBehaviour
	{
		public LoadingScreenWindow(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService, focusablesService)
		{
		}
	}
}
