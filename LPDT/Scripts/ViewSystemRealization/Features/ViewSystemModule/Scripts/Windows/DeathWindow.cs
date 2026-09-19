using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.ViewSystemModule.Scripts.Windows
{
	public class DeathWindow : FocusableWindowBehaviour, IInitializable, IDisposable
	{
		public DeathWindow(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService, focusablesService)
		{
		}
	}
}
