using System;
using Features.InputModule.Scripts.Generated;
using Features.UINavigationModuleRealization.Scripts.BackButton;
using Global.StateMachinesModule.Scripts;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using Zenject;

namespace Features.SettingsMenuModule.Scripts
{
	public class CallSettingsViewByInputSystem : IInitializable, IDisposable, IBackButtonProcessor
	{
		private readonly IInputService _inputService;

		private readonly ISettingsWindowProvider _settingsWindowProvider;

		private readonly GameFlowStateMachine _gameFlowStateMachine;

		private readonly IInputDeviceService _inputDeviceService;

		private readonly IUIBackButtonRegistrationService _backButtonRegistrationService;

		public BackButtonProcessorType Type => BackButtonProcessorType.Settings;

		public CallSettingsViewByInputSystem(IInputService inputService, ISettingsWindowProvider settingsWindowProvider, GameFlowStateMachine gameFlowStateMachine, IUIBackButtonRegistrationService backButtonRegistrationService, IInputDeviceService inputDeviceService)
		{
			_inputService = inputService;
			_settingsWindowProvider = settingsWindowProvider;
			_gameFlowStateMachine = gameFlowStateMachine;
			_backButtonRegistrationService = backButtonRegistrationService;
			_inputDeviceService = inputDeviceService;
		}

		public bool CanHandleBack()
		{
			return !_settingsWindowProvider.IsAnySettingsWindowShown();
		}

		public void OnBack()
		{
			if (!IsGamepadMode())
			{
				OpenSettingsWindow();
			}
		}

		public void Initialize()
		{
			InputDefaultActions openSettings = _inputService.OpenSettings;
			openSettings.Performed = (Action)Delegate.Combine(openSettings.Performed, new Action(OpenSettingsWindow));
			_backButtonRegistrationService.Register(this);
		}

		public void Dispose()
		{
			InputDefaultActions openSettings = _inputService.OpenSettings;
			openSettings.Performed = (Action)Delegate.Remove(openSettings.Performed, new Action(OpenSettingsWindow));
			_backButtonRegistrationService.Unregister(this);
		}

		private void OpenSettingsWindow()
		{
			bool isMenu = _gameFlowStateMachine.CurrentState == GameFlowState.MenuGameState;
			OpenWindow(_settingsWindowProvider.GetSettingsWindow(isMenu));
		}

		private static void OpenWindow(FocusableWindowBehaviour window)
		{
			switch (window.WindowStatus)
			{
			case WindowStatus.Closed:
				window.Open();
				break;
			case WindowStatus.Hidden:
				window.Show();
				break;
			}
		}

		private bool IsGamepadMode()
		{
			if (_inputDeviceService != null)
			{
				if (!_inputDeviceService.IsCurrentActiveDeviceGamepad())
				{
					return _inputDeviceService.IsCurrentActiveDeviceJoystick();
				}
				return true;
			}
			return false;
		}
	}
}
