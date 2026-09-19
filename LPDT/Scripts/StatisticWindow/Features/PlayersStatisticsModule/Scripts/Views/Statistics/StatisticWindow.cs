using System;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.PlayersStatisticsModule.Scripts.Views.Statistics
{
	public class StatisticWindow : FocusableWindowBehaviour, IInitializable, IDisposable
	{
		public StatisticWindow(IWindowsFactory windowsFactory, IWindowsComponentsFinderService windowsComponentsFinderService, IWindowsService windowsService, IFocusablesService focusablesService)
			: base(windowsFactory, windowsComponentsFinderService, windowsService, focusablesService)
		{
		}
	}
}
