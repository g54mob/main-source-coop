using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.ConfirmExitPopupService.Scripts
{
	public class ConfirmationPopupWindow : FocusableWindowBehaviour, IInitializable, IDisposable
	{
		public ConfirmationPopupWindow(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService, focusablesService)
		{
		}
	}
}
