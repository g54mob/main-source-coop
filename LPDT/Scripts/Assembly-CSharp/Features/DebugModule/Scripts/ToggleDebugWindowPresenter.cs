using System;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;

namespace Features.DebugModule.Scripts
{
	public class ToggleDebugWindowPresenter : PresenterBehaviour<ToggleDebugWindowViewBase>
	{
		private readonly IWindowsService _windowsService;

		private IWindow _debugWindow;

		public ToggleDebugWindowPresenter(IWindowsService windowsService)
		{
			_windowsService = windowsService;
		}

		protected override void OnViewSet()
		{
			ToggleDebugWindowViewBase view = base.View;
			view.OnEnableDebugButtonClick = (Action)Delegate.Combine(view.OnEnableDebugButtonClick, new Action(ToggleDebugWindow));
			InitializeDebugWindow();
		}

		protected override void OnDisposed()
		{
			if (IsDebugWindowOpened())
			{
				_debugWindow.Close();
			}
			ToggleDebugWindowViewBase view = base.View;
			view.OnEnableDebugButtonClick = (Action)Delegate.Remove(view.OnEnableDebugButtonClick, new Action(ToggleDebugWindow));
		}

		private bool IsDebugWindowOpened()
		{
			return _debugWindow.WindowStatus != WindowStatus.Closed;
		}

		private void ToggleDebugWindow()
		{
			if (base.View.gameObject.activeSelf && base.View.gameObject.activeInHierarchy)
			{
				if (DebugWindowIsClosed())
				{
					_debugWindow.Open();
				}
				else
				{
					_debugWindow.Close();
				}
			}
		}

		private void CloseDebugWindow()
		{
			_debugWindow.Close();
		}

		private bool DebugWindowIsClosed()
		{
			return _debugWindow.WindowStatus == WindowStatus.Closed;
		}

		private void InitializeDebugWindow()
		{
			_debugWindow = _windowsService.GetWindow(typeof(DebugWindow));
		}
	}
}
