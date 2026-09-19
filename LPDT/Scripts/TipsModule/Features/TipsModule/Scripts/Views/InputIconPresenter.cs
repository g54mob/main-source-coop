using Features.DeviceModule.Scripts;
using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using RSG.Muffin.MVPWindowsUnityUIArchitectureModule.Core;
using UnityEngine.InputSystem;

namespace Features.TipsModule.Scripts.Views
{
	public class InputIconPresenter : PresenterBehaviour<InputIconViewBase>
	{
		private readonly IInputDeviceService _inputDeviceService;

		private readonly IDeviceService _deviceService;

		private readonly InputModel _inputModel;

		public InputIconPresenter(IInputDeviceService inputDeviceService, IDeviceService deviceService, InputModel inputModel)
		{
			_inputDeviceService = inputDeviceService;
			_deviceService = deviceService;
			_inputModel = inputModel;
		}

		protected override void OnViewSet()
		{
			_inputDeviceService.OnCurrentActiveDeviceChange += SetInputKeyText;
			base.View.InputActionChanged += SetInputKeyText;
			SetInputKeyText();
		}

		protected override void OnDisposed()
		{
			_inputDeviceService.OnCurrentActiveDeviceChange -= SetInputKeyText;
			base.View.InputActionChanged -= SetInputKeyText;
		}

		protected override void OnViewEnabled()
		{
			SetInputKeyText();
		}

		private void SetInputKeyText(InputDevice _)
		{
			SetInputKeyText();
		}

		private void SetInputKeyText()
		{
			bool flag = _deviceService.IsMobile() && !base.View.IsEnableInputKeyTextOnMobile;
			if ((base.View.IsDisableOnKeyboard && _inputModel.CurrentActiveDevice is Keyboard) || base.View.InputAction?.actionMap == null || flag)
			{
				base.View.SetInputActive(isActive: false);
				return;
			}
			InputKeyVisualizationHolder bindingKey = _inputDeviceService.GetBindingKey(base.View.InputAction);
			if (bindingKey == null)
			{
				base.View.SetInputActive(isActive: false);
			}
			else if (bindingKey.IconCustomizationData?.Icon == null)
			{
				base.View.SetInputKeyText(bindingKey.TextVisualization);
			}
			else
			{
				base.View.SetInputKeySprite(bindingKey.IconCustomizationData.Icon, bindingKey.IconCustomizationData.Scale);
			}
		}
	}
}
