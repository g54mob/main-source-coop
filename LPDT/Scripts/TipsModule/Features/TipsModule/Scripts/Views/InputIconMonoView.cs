using Features.DeviceModule.Scripts;
using Features.InputDeviceModuleRealization.Scripts;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Features.TipsModule.Scripts.Views
{
	public class InputIconMonoView : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text _keyBinding;

		[SerializeField]
		private Image _imageKeyBinding;

		[SerializeField]
		private HorizontalLayoutGroup _background;

		[SerializeField]
		private RectTransform _rebuildLayoutTarget;

		[SerializeField]
		private InputActionReference _inputAction;

		[SerializeField]
		private InputDeviceIconCustomizationType _iconCustomizationType = InputDeviceIconCustomizationType.Tutorial;

		[SerializeField]
		private GameObject _keyboardMouseAnimatedRoot;

		[SerializeField]
		private Animator _keyboardMouseAnimator;

		[SerializeField]
		private bool _isDisableOnKeyboard;

		[SerializeField]
		private bool _isEnableInputKeyTextOnMobile;

		private IInputDeviceService _inputDeviceService;

		private IDeviceService _deviceService;

		private InputModel _inputModel;

		private InputAction _currentInputAction;

		[Inject]
		public void InjectDependencies(IInputDeviceService inputDeviceService, IDeviceService deviceService, InputModel inputModel)
		{
			_inputDeviceService = inputDeviceService;
			_deviceService = deviceService;
			_inputModel = inputModel;
		}

		private void OnEnable()
		{
			if (_inputAction != null)
			{
				_currentInputAction = _inputAction.action;
			}
			_inputDeviceService.OnCurrentActiveDeviceChange += OnActiveDeviceChanged;
			UpdateInputKeyVisualization();
		}

		private void OnDisable()
		{
			_inputDeviceService.OnCurrentActiveDeviceChange -= OnActiveDeviceChanged;
		}

		public void OverrideInputAction(InputAction inputAction)
		{
			_currentInputAction = inputAction;
			UpdateInputKeyVisualization();
		}

		private void OnActiveDeviceChanged(InputDevice _)
		{
			UpdateInputKeyVisualization();
		}

		private void UpdateInputKeyVisualization()
		{
			bool flag = _deviceService.IsMobile() && !_isEnableInputKeyTextOnMobile;
			if ((_isDisableOnKeyboard && _inputModel.CurrentActiveDevice is Keyboard) || _currentInputAction?.actionMap == null || flag)
			{
				if (_keyboardMouseAnimatedRoot != null)
				{
					SetAnimatedRootActive(isActive: false);
				}
				SetInputActive(isActive: false);
				return;
			}
			if (_keyboardMouseAnimatedRoot != null)
			{
				InputDevice currentActiveDevice = _inputModel.CurrentActiveDevice;
				if (currentActiveDevice is Keyboard || currentActiveDevice is Mouse)
				{
					SetInputActive(isActive: false);
					SetAnimatedRootActive(isActive: true);
					return;
				}
				SetAnimatedRootActive(isActive: false);
			}
			InputKeyVisualizationHolder bindingKey = _inputDeviceService.GetBindingKey(_currentInputAction, (int)_iconCustomizationType);
			if (bindingKey == null)
			{
				SetInputActive(isActive: false);
			}
			else if (bindingKey.IconCustomizationData?.Icon == null)
			{
				SetInputKeyText(bindingKey.TextVisualization);
			}
			else
			{
				SetInputKeySprite(bindingKey.IconCustomizationData.Icon, bindingKey.IconCustomizationData.Scale);
			}
		}

		private void SetAnimatedRootActive(bool isActive)
		{
			_keyboardMouseAnimatedRoot.SetActive(isActive);
			if (isActive)
			{
				_keyboardMouseAnimator.Play(0, 0, 0f);
			}
		}

		private void SetInputKeyText(string bindingKey)
		{
			if (_imageKeyBinding != null)
			{
				_imageKeyBinding.gameObject.SetActive(value: false);
			}
			if (_keyBinding != null && _background != null)
			{
				_background.gameObject.SetActive(value: true);
				_keyBinding.SetText(bindingKey);
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_background.transform);
			}
			RebuildTargetLayout();
		}

		private void SetInputKeySprite(Sprite sprite, float scale)
		{
			if (_background != null)
			{
				_background.gameObject.SetActive(value: false);
			}
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
			if (_background != null)
			{
				_background.gameObject.SetActive(isActive);
				LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_background.transform);
			}
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
