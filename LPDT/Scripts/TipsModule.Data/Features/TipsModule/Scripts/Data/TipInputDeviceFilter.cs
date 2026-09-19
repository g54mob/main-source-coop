using UnityEngine.InputSystem;

namespace Features.TipsModule.Scripts.Data
{
	public static class TipInputDeviceFilter
	{
		public static TipInputDeviceCategory GetCategory(InputDevice device)
		{
			if (device is Keyboard || device is Mouse)
			{
				return TipInputDeviceCategory.KeyboardAndMouse;
			}
			if (device is Gamepad)
			{
				return TipInputDeviceCategory.Gamepad;
			}
			if (device is Joystick)
			{
				return TipInputDeviceCategory.Joystick;
			}
			return TipInputDeviceCategory.None;
		}

		public static bool IsVisibleForDevice(TipInputDeviceCategory showOnInputDevices, TipInputDeviceCategory hideOnInputDevices, InputDevice activeDevice)
		{
			if (activeDevice == null)
			{
				return false;
			}
			TipInputDeviceCategory category = GetCategory(activeDevice);
			if (hideOnInputDevices != TipInputDeviceCategory.None && (hideOnInputDevices & category) != TipInputDeviceCategory.None)
			{
				return false;
			}
			if (showOnInputDevices == TipInputDeviceCategory.None)
			{
				return true;
			}
			return (showOnInputDevices & category) != 0;
		}
	}
}
