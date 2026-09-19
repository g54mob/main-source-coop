using System;
using System.Linq;
using RSG.Muffin.InputSubmodule.InputModule.Core.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Zenject;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	public class InputDeviceService : IInputDeviceService, IInitializable, IDisposable
	{
		private readonly InputDevicesPriorityConfiguration _inputDevicesPriorityConfiguration;

		private readonly IDeviceInputService _inputService;

		private readonly IInputDeviceActions _inputActions;

		private readonly InputModel _inputModel;

		private IDisposable _disposable;

		public event Action<InputDevice> OnDeviceChange;

		public event Action<InputDevice> OnDeviceAdded;

		public event Action<InputDevice> OnDeviceRemoved;

		public event Action<InputDevice> OnDeviceDisconnected;

		public event Action<InputDevice> OnDeviceReconnected;

		public event Action<InputDevice> OnCurrentActiveDeviceChange;

		public InputDeviceService(IDeviceInputService inputService, InputModel inputModel, IInputDeviceActions inputActions, InputDevicesPriorityConfiguration inputDevicesPriorityConfiguration)
		{
			_inputService = inputService;
			_inputModel = inputModel;
			_inputActions = inputActions;
			_inputDevicesPriorityConfiguration = inputDevicesPriorityConfiguration;
		}

		public void Initialize()
		{
			InputSystem.onDeviceChange += DeviceChanged;
			_disposable = InputSystem.onAnyButtonPress.Call(OnAnyButtonPressed);
			InputVector2Actions anyVectorChange = _inputService.AnyVectorChange;
			anyVectorChange.VectorChangedWithDeviceCallbackStarted = (Action<InputDevice, Vector2>)Delegate.Combine(anyVectorChange.VectorChangedWithDeviceCallbackStarted, new Action<InputDevice, Vector2>(OnAnyVector2Changed));
		}

		public void Dispose()
		{
			InputSystem.onDeviceChange -= DeviceChanged;
			_disposable.Dispose();
			InputVector2Actions anyVectorChange = _inputService.AnyVectorChange;
			anyVectorChange.VectorChangedWithDeviceCallbackStarted = (Action<InputDevice, Vector2>)Delegate.Remove(anyVectorChange.VectorChangedWithDeviceCallbackStarted, new Action<InputDevice, Vector2>(OnAnyVector2Changed));
		}

		public InputKeyVisualizationHolder GetBindingKey(InputAction inputAction, int customizationIndex = -1)
		{
			InputAction inputAction2 = _inputActions.FindAction(inputAction.name);
			if (inputAction2 == null)
			{
				throw new Exception("Action " + inputAction.name + " not found");
			}
			if (!inputAction2.controls.Any())
			{
				return null;
			}
			InputKeyVisualizationHolder inputKeyVisualizationHolder = new InputKeyVisualizationHolder();
			SetInputToAnyThatExist();
			if (_inputModel.CurrentActiveDevice == null)
			{
				return null;
			}
			foreach (InputDeviceTypeWithIcons inputDeviceTypesPriorityWithIcon in _inputDevicesPriorityConfiguration.InputDeviceTypesPriorityWithIcons)
			{
				Type type = Type.GetType(inputDeviceTypesPriorityWithIcon.Type);
				if (_inputModel.CurrentActiveDevice.GetType() != type && (_inputModel.SubDevice == null || _inputModel.SubDevice.GetType() != type))
				{
					continue;
				}
				InputControl inputControl = inputAction2.controls.FirstOrDefault((InputControl c) => c.device.GetType() == type);
				if (inputControl == null)
				{
					continue;
				}
				int bindingIndexForControl = inputAction2.GetBindingIndexForControl(inputControl);
				InputControl inputControl2 = InputSystem.FindControl(inputAction2.bindings[bindingIndexForControl].effectivePath);
				InputDeviceShortNameMapItem inputDeviceShortNameMapItemByPath = GetInputDeviceShortNameMapItemByPath(inputControl2.path);
				if (inputDeviceShortNameMapItemByPath != null)
				{
					InputDeviceIconCustomizationData value;
					if (customizationIndex == -1)
					{
						inputKeyVisualizationHolder.IconCustomizationData = inputDeviceShortNameMapItemByPath.DefaultCustomizationData;
					}
					else if (inputDeviceShortNameMapItemByPath.CustomizationsData.TryGetValue(customizationIndex, out value))
					{
						inputKeyVisualizationHolder.IconCustomizationData = value;
						inputKeyVisualizationHolder.IsCustomizationDataDefault = false;
					}
					else
					{
						inputKeyVisualizationHolder.IconCustomizationData = inputDeviceShortNameMapItemByPath.DefaultCustomizationData;
						inputKeyVisualizationHolder.IsCustomizationDataDefault = true;
					}
				}
				else
				{
					inputKeyVisualizationHolder.TextVisualization = inputControl2.name.ToUpper();
				}
				return inputKeyVisualizationHolder;
			}
			return null;
		}

		public TMP_SpriteAsset GetCurrentInputSpriteAsset()
		{
			SetInputToAnyThatExist();
			if (_inputModel.CurrentActiveDevice == null)
			{
				return _inputDevicesPriorityConfiguration.InputDeviceTypesPriorityWithIcons.First().SpriteAsset;
			}
			InputDeviceTypeWithIcons inputDeviceTypeWithIcons = _inputDevicesPriorityConfiguration.InputDeviceTypesPriorityWithIcons.FirstOrDefault((InputDeviceTypeWithIcons i) => Type.GetType(i.Type) == _inputModel.CurrentActiveDevice.GetType());
			if (inputDeviceTypeWithIcons != null)
			{
				return inputDeviceTypeWithIcons.SpriteAsset;
			}
			return _inputDevicesPriorityConfiguration.InputDeviceTypesPriorityWithIcons.First().SpriteAsset;
		}

		public bool IsAnyGamepadConnected()
		{
			return InputSystem.devices.Any((InputDevice d) => d is Gamepad);
		}

		public bool IsAnyJoystickConnected()
		{
			return InputSystem.devices.Any((InputDevice d) => d is Joystick);
		}

		public bool IsCurrentActiveDeviceGamepad()
		{
			return _inputModel.CurrentActiveDevice is Gamepad;
		}

		public bool IsCurrentActiveDeviceJoystick()
		{
			return _inputModel.CurrentActiveDevice is Joystick;
		}

		public bool IsDeviceController(InputDevice inputDevice)
		{
			if (!(inputDevice is Gamepad))
			{
				return inputDevice is Joystick;
			}
			return true;
		}

		public void SetGamepadLightbarColor(Color color)
		{
			_inputModel.SetCurrentGamePadLightbarColor(color);
		}

		private void DeviceChanged(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
		{
			if (inputDeviceChange == InputDeviceChange.Added || inputDeviceChange == InputDeviceChange.Removed || inputDeviceChange == InputDeviceChange.Disconnected || inputDeviceChange == InputDeviceChange.Reconnected)
			{
				if (inputDevice is Mouse && _inputModel.SubDevice != inputDevice)
				{
					_inputModel.SubDevice = inputDevice;
				}
				int currentActiveDeviceId = SetMouseDeviceOnKeyboard(inputDevice.deviceId);
				_inputModel.CurrentActiveDeviceId = currentActiveDeviceId;
				_inputModel.CurrentActiveDevice = GetDeviceById(_inputModel.CurrentActiveDeviceId);
				this.OnCurrentActiveDeviceChange?.Invoke(_inputModel.CurrentActiveDevice);
				this.OnDeviceChange?.Invoke(inputDevice);
				switch (inputDeviceChange)
				{
				case InputDeviceChange.Added:
					this.OnDeviceAdded?.Invoke(inputDevice);
					break;
				case InputDeviceChange.Removed:
					this.OnDeviceRemoved?.Invoke(inputDevice);
					break;
				case InputDeviceChange.Disconnected:
					this.OnDeviceDisconnected?.Invoke(inputDevice);
					break;
				case InputDeviceChange.Reconnected:
					this.OnDeviceReconnected?.Invoke(inputDevice);
					break;
				}
			}
		}

		private InputDevice GetDeviceById(int deviceId)
		{
			foreach (InputDevice device in InputSystem.devices)
			{
				if (device.deviceId == deviceId)
				{
					return device;
				}
			}
			return null;
		}

		private int SetMouseDeviceOnKeyboard(int deviceId)
		{
			if (GetDeviceById(deviceId) is Mouse && Keyboard.current != null)
			{
				deviceId = Keyboard.current.deviceId;
			}
			return deviceId;
		}

		private void OnAnyButtonPressed(InputControl inputEventPtr)
		{
			OnAnyButtonPressedByDeviceId(inputEventPtr.device.deviceId);
		}

		private void OnAnyVector2Changed(InputDevice inputDevice, Vector2 vector2)
		{
			OnAnyButtonPressedByDeviceId(inputDevice.deviceId);
		}

		private void OnAnyButtonPressedByDeviceId(int deviceId)
		{
			InputDevice deviceById = GetDeviceById(deviceId);
			if (deviceById is Mouse && _inputModel.SubDevice != deviceById)
			{
				_inputModel.SubDevice = deviceById;
			}
			int num = SetMouseDeviceOnKeyboard(deviceId);
			if (_inputModel.CurrentActiveDeviceId != num)
			{
				_inputModel.CurrentActiveDeviceId = num;
				_inputModel.CurrentActiveDevice = GetDeviceById(_inputModel.CurrentActiveDeviceId);
				this.OnCurrentActiveDeviceChange?.Invoke(_inputModel.CurrentActiveDevice);
			}
		}

		private void SetInputToAnyThatExist()
		{
			if (_inputModel.SubDevice == null)
			{
				InputDevice inputDevice = InputSystem.devices.FirstOrDefault((InputDevice d) => d.GetType() == typeof(Mouse));
				if (inputDevice != null)
				{
					_inputModel.SubDevice = inputDevice;
				}
			}
			if (_inputModel.CurrentActiveDevice != null)
			{
				return;
			}
			foreach (InputDeviceTypeWithIcons inputDeviceTypesPriorityWithIcon in _inputDevicesPriorityConfiguration.InputDeviceTypesPriorityWithIcons)
			{
				InputDevice inputDevice2 = InputSystem.devices.FirstOrDefault((InputDevice d) => d.GetType() == Type.GetType(inputDeviceTypesPriorityWithIcon.Type));
				if (inputDevice2 != null)
				{
					_inputModel.CurrentActiveDevice = inputDevice2;
					break;
				}
			}
		}

		private InputDeviceShortNameMapItem GetInputDeviceShortNameMapItemByPath(string inputControlPath)
		{
			foreach (InputDeviceVisualizationConfiguration inputDeviceVisualizationConfiguration in _inputDevicesPriorityConfiguration.InputDeviceVisualizationConfigurations)
			{
				foreach (string deviceName in inputDeviceVisualizationConfiguration.DeviceNames)
				{
					foreach (InputDeviceShortNameMapItem inputDeviceShortNameMapItem in inputDeviceVisualizationConfiguration.InputDeviceShortNameMapItems)
					{
						if ((inputControlPath.Contains("/" + deviceName + "/") || inputControlPath.Contains("/" + deviceName)) && string.Equals(inputControlPath.Split('/')[^1], inputDeviceShortNameMapItem.InputControlPath, StringComparison.OrdinalIgnoreCase))
						{
							return inputDeviceShortNameMapItem;
						}
					}
				}
			}
			return null;
		}
	}
}
