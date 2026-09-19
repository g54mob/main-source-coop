using System;
using System.Collections.Generic;
using Features.DeviceModule.Scripts;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Features.TipsModule.Scripts.Views
{
	public class CustomInputIconMonoView : MonoBehaviour
	{
		[Serializable]
		public class DeviceSpriteData
		{
			public List<string> DeviceNames;

			public Sprite Sprite;

			public float Scale = 1f;
		}

		[SerializeField]
		private Image _imageKeyBinding;

		[SerializeField]
		private RectTransform _rebuildLayoutTarget;

		[SerializeField]
		private List<DeviceSpriteData> _deviceSprites;

		[SerializeField]
		private bool _isDisableOnMobile;

		private IInputDeviceService _inputDeviceService;

		private IDeviceService _deviceService;

		private InputModel _inputModel;

		[Inject]
		public void InjectDependencies(IInputDeviceService inputDeviceService, IDeviceService deviceService, InputModel inputModel)
		{
			_inputDeviceService = inputDeviceService;
			_deviceService = deviceService;
			_inputModel = inputModel;
		}

		private void OnEnable()
		{
			_inputDeviceService.OnCurrentActiveDeviceChange += OnActiveDeviceChanged;
			UpdateInputKeyVisualization();
		}

		private void OnDisable()
		{
			_inputDeviceService.OnCurrentActiveDeviceChange -= OnActiveDeviceChanged;
		}

		private void OnActiveDeviceChanged(InputDevice _)
		{
			UpdateInputKeyVisualization();
		}

		private void UpdateInputKeyVisualization()
		{
			if (_isDisableOnMobile && _deviceService.IsMobile())
			{
				SetInputActive(isActive: false);
				return;
			}
			DeviceSpriteData deviceSpriteData = GetDeviceSpriteData(_inputModel.CurrentActiveDevice);
			if (deviceSpriteData == null || deviceSpriteData.Sprite == null)
			{
				SetInputActive(isActive: false);
			}
			else
			{
				SetInputKeySprite(deviceSpriteData.Sprite, deviceSpriteData.Scale);
			}
		}

		private DeviceSpriteData GetDeviceSpriteData(InputDevice inputDevice)
		{
			if (inputDevice == null)
			{
				return null;
			}
			foreach (DeviceSpriteData deviceSprite in _deviceSprites)
			{
				foreach (string deviceName in deviceSprite.DeviceNames)
				{
					if (inputDevice.path.Contains("/" + deviceName + "/") || inputDevice.path.Contains("/" + deviceName))
					{
						return deviceSprite;
					}
				}
			}
			return null;
		}

		private void SetInputKeySprite(Sprite sprite, float scale)
		{
			if (_imageKeyBinding != null)
			{
				_imageKeyBinding.gameObject.SetActive(value: true);
				_imageKeyBinding.transform.localScale = Vector3.one * scale;
				_imageKeyBinding.sprite = sprite;
			}
			RebuildTargetLayout();
		}

		private void SetInputActive(bool isActive)
		{
			if (_imageKeyBinding != null)
			{
				_imageKeyBinding.gameObject.SetActive(isActive);
			}
			RebuildTargetLayout();
		}

		private void RebuildTargetLayout()
		{
			if (!(_rebuildLayoutTarget == null))
			{
				Canvas.ForceUpdateCanvases();
				LayoutRebuilder.ForceRebuildLayoutImmediate(_rebuildLayoutTarget);
			}
		}
	}
}
