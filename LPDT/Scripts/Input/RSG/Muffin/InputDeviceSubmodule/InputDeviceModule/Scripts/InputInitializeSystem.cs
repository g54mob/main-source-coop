using System;
using Features.DeviceModule.Scripts;
using Features.DeviceModule.Scripts.DeviceData;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using Zenject;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	public class InputInitializeSystem : IInitializable, IDisposable
	{
		private const string STEAM_DECK_DEVICE = "<SteamDeck>";

		private const string GAMEPAD_DEVICE = "<Gamepad>";

		private readonly IInputDeviceActions _inputDeviceActions;

		private readonly IDeviceService _deviceService;

		private readonly InputModel _inputModel;

		private readonly DeviceTypeModel _deviceTypeModel;

		public InputInitializeSystem(IDeviceService deviceService, InputModel inputModel, DeviceTypeModel deviceTypeModel, IInputDeviceActions inputDeviceActions)
		{
			_deviceService = deviceService;
			_inputDeviceActions = inputDeviceActions;
			_inputModel = inputModel;
			_deviceTypeModel = deviceTypeModel;
		}

		public void Initialize()
		{
			DeviceTypeModel deviceTypeModel = _deviceTypeModel;
			deviceTypeModel.OnDeviceChanged = (Action)Delegate.Combine(deviceTypeModel.OnDeviceChanged, new Action(ReplaceSteamDeckBindings));
			if (_deviceService.IsMobile() && !_deviceService.IsEditor())
			{
				RegisterMobileDevice();
			}
			ReplaceSteamDeckBindings();
		}

		public void Dispose()
		{
			DeviceTypeModel deviceTypeModel = _deviceTypeModel;
			deviceTypeModel.OnDeviceChanged = (Action)Delegate.Remove(deviceTypeModel.OnDeviceChanged, new Action(ReplaceSteamDeckBindings));
		}

		private void ReplaceSteamDeckBindings()
		{
			if (_deviceService.GetCurrentDevice() != DeviceType.SteamDeck)
			{
				return;
			}
			foreach (InputActionMap actionMap in _inputDeviceActions.asset.actionMaps)
			{
				foreach (InputAction action in actionMap.actions)
				{
					ChangeSteamDeckBindingsOfAction(action);
				}
			}
			_inputModel.InputSystemUIInputModule.actionsAsset = _inputDeviceActions.asset;
		}

		private void ChangeSteamDeckBindingsOfAction(InputAction action)
		{
			for (int i = 0; i < action.bindings.Count; i++)
			{
				InputBinding inputBinding = action.bindings[i];
				if (inputBinding.path.StartsWith("<SteamDeck>"))
				{
					string path = inputBinding.path.Replace("<SteamDeck>", "<Gamepad>");
					action.ApplyBindingOverride(i, path);
				}
			}
		}

		private static void RegisterMobileDevice()
		{
			InputSystem.RegisterLayout<MobileMock>(null, default(InputDeviceMatcher).WithInterface("MobileMock"));
			InputSystem.AddDevice<MobileMock>();
			InputSystem.GetDevice<MobileMock>();
		}
	}
}
