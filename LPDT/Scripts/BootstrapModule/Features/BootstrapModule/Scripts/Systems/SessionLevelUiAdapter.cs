using Features.SessionManagementModule.Models;
using Features.ViewSystemModule.Scripts.Windows;
using Features.VignetteUIEffectModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine;

namespace Features.BootstrapModule.Scripts.Systems
{
	public sealed class SessionLevelUiAdapter : ISessionLevelUi
	{
		private readonly IWindowsService _windowsService;

		public SessionLevelUiAdapter(IWindowsService windowsService)
		{
			_windowsService = windowsService;
		}

		public void Open()
		{
			OpenIfClosed<SessionWindow>();
			OpenIfClosed<VignetteEffectWindow>();
			if (Debug.isDebugBuild || Application.isEditor)
			{
				OpenIfClosed<DebugEnableWindow>();
			}
			OpenIfClosed<WorldCanvasWindow>();
		}

		private void OpenIfClosed<TWindow>() where TWindow : IWindow
		{
			IWindow window = _windowsService.GetWindow(typeof(TWindow));
			if (window == null || window.WindowStatus == WindowStatus.Closed)
			{
				_windowsService.OpenWindow<TWindow>();
			}
		}
	}
}
