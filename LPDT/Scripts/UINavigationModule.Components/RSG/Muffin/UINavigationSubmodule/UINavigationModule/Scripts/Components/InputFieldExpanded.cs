using RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts;
using RSG.Muffin.ScreenKeyboardSubmodule.ScreenKeyboardModule.Scripts;
using UnityEngine.EventSystems;
using Zenject;

namespace RSG.Muffin.UINavigationSubmodule.UINavigationModule.Scripts.Components
{
	public class InputFieldExpanded : InputFieldWithNavigationCallbacks
	{
		private IInputDeviceService _inputDeviceService;

		private IScreenKeyboardService _screenKeyboardService;

		private bool _isNeedsToShowKeyboard = true;

		public override bool shouldActivateOnSelect => false;

		[Inject]
		private void InjectDependencies(IInputDeviceService deviceService, IScreenKeyboardService screenKeyboardService)
		{
			_screenKeyboardService = screenKeyboardService;
			_inputDeviceService = deviceService;
		}

		public override void OnSubmit(BaseEventData eventData)
		{
			base.OnSubmit(eventData);
			if (IsActive() && IsInteractable())
			{
				if (_isNeedsToShowKeyboard)
				{
					SetConsoleInputFieldInternalActive(isActive: true);
				}
				_isNeedsToShowKeyboard = !_isNeedsToShowKeyboard;
			}
		}

		public override void OnCancel(BaseEventData eventData)
		{
			base.OnCancel(eventData);
			if (IsActive() && IsInteractable())
			{
				SetConsoleInputFieldInternalActive(isActive: false);
				_isNeedsToShowKeyboard = true;
			}
		}

		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			_isNeedsToShowKeyboard = true;
		}

		private void SetConsoleInputFieldInternalActive(bool isActive)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (ConsoleScreenKeyboardShouldBeUsed() && !base.readOnly)
			{
				if (isActive)
				{
					ShowConsoleKeyboard();
				}
				else
				{
					HideConsoleKeyboard();
				}
			}
			UpdateLabel();
		}

		private bool ConsoleScreenKeyboardShouldBeUsed()
		{
			return _inputDeviceService.IsCurrentActiveDeviceGamepad();
		}

		private void ShowConsoleKeyboard()
		{
			_screenKeyboardService.ShowScreenKeyboard();
		}

		private void HideConsoleKeyboard()
		{
			_screenKeyboardService.HideScreenKeyboard();
		}
	}
}
