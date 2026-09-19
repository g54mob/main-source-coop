using System;
using Features.CameraModelModule;
using Features.ViewSystemModule.Scripts.Windows;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.DebugModule.Scripts.CameraDebug
{
	public class PlayerCameraDebugSystem : IInitializable, IDisposable
	{
		private readonly IWindowsService _windowsService;

		private IWindow _debugWindow;

		private CameraModel _cameraModel;

		public PlayerCameraDebugSystem(IWindowsService windowsService, CameraModel cameraModel)
		{
			_windowsService = windowsService;
			_cameraModel = cameraModel;
		}

		public void Initialize()
		{
			_debugWindow = _windowsService.GetWindow(typeof(DebugWindow));
			_debugWindow.OnWindowOpened += OnDebugWindowOpened;
			_debugWindow.OnWindowClosed += OnDebugWindowClosed;
		}

		public void Dispose()
		{
			_debugWindow.OnWindowOpened -= OnDebugWindowOpened;
			_debugWindow.OnWindowClosed -= OnDebugWindowClosed;
		}

		private void OnDebugWindowClosed(Type windowType)
		{
			_cameraModel.RemoveCameraInputLockReason(LockCameraInputReasonEnum.Debug);
		}

		private void OnDebugWindowOpened(Type windowType)
		{
			_cameraModel.AddCameraInputLockReason(LockCameraInputReasonEnum.Debug);
		}
	}
}
