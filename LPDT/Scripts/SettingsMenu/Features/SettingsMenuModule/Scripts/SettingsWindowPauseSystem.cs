using System;
using Features.CameraModelModule;
using Features.GamePauseModule.Scripts;
using Features.InputModule.Scripts.Generated;
using Features.Movement.Scripts;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class SettingsWindowPauseSystem : IInitializable, IDisposable
	{
		private readonly SettingsWindow _settingsWindow;

		private readonly GameGlobalNetworkingPause _gameGlobalNetworkingPause;

		private readonly PlayerMovableModel _playerMovableModel;

		private readonly CameraModel _cameraModel;

		private readonly IInputService _inputService;

		public SettingsWindowPauseSystem(SettingsWindow settingsWindow, GameGlobalNetworkingPause gameGlobalNetworkingPause, PlayerMovableModel playerMovableModel, CameraModel cameraModel, IInputService inputService)
		{
			_settingsWindow = settingsWindow;
			_gameGlobalNetworkingPause = gameGlobalNetworkingPause;
			_playerMovableModel = playerMovableModel;
			_cameraModel = cameraModel;
			_inputService = inputService;
		}

		public void Initialize()
		{
			_settingsWindow.OnWindowOpened += ApplyLocalPause;
			_settingsWindow.OnWindowClosed += DisableLocalPause;
			_gameGlobalNetworkingPause.OnLocalPausedChanged += ProcessLocalPause;
		}

		public void Dispose()
		{
			_settingsWindow.OnWindowOpened -= ApplyLocalPause;
			_settingsWindow.OnWindowClosed -= DisableLocalPause;
			_gameGlobalNetworkingPause.OnLocalPausedChanged -= ProcessLocalPause;
		}

		private void ProcessLocalPause(bool isPaused)
		{
			if (isPaused)
			{
				_playerMovableModel.AddNoRotationReason(NoRotationReasonEnum.Settings);
				_playerMovableModel.LocalMovable.SetMovementInputEnabled(isEnabled: false);
				_cameraModel.AddCameraInputLockReason(LockCameraInputReasonEnum.Settings);
				_inputService.Disable();
				_inputService.EnableUI();
				_inputService.EnableUIMap();
			}
			else
			{
				_playerMovableModel.RemoveNoRotationReason(NoRotationReasonEnum.Settings);
				_playerMovableModel.LocalMovable.SetMovementInputEnabled(isEnabled: true);
				_cameraModel.RemoveCameraInputLockReason(LockCameraInputReasonEnum.Settings);
				_inputService.Enable();
			}
		}

		private void DisableLocalPause(Type type)
		{
			if (!(_playerMovableModel.LocalMovable == null) && _gameGlobalNetworkingPause.IsLocalPausedEnable)
			{
				_gameGlobalNetworkingPause.SetLocalPaused(isPaused: false);
			}
		}

		private void ApplyLocalPause(Type type)
		{
			if (!(_playerMovableModel.LocalMovable == null) && !_gameGlobalNetworkingPause.IsLocalPausedEnable)
			{
				_gameGlobalNetworkingPause.SetLocalPaused(isPaused: true);
			}
		}
	}
}
