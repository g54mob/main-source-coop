using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	public interface IInputDeviceService
	{
		event Action<InputDevice> OnDeviceChange;

		event Action<InputDevice> OnDeviceAdded;

		event Action<InputDevice> OnDeviceRemoved;

		event Action<InputDevice> OnDeviceDisconnected;

		event Action<InputDevice> OnDeviceReconnected;

		event Action<InputDevice> OnCurrentActiveDeviceChange;

		InputKeyVisualizationHolder GetBindingKey(InputAction inputAction, int customizationIndex = -1);

		TMP_SpriteAsset GetCurrentInputSpriteAsset();

		bool IsAnyGamepadConnected();

		bool IsAnyJoystickConnected();

		bool IsCurrentActiveDeviceGamepad();

		bool IsCurrentActiveDeviceJoystick();

		bool IsDeviceController(InputDevice inputDevice);

		void SetGamepadLightbarColor(Color color);
	}
}
