using System;
using Features.InputDeviceModuleRealization.Scripts;
using Features.TipsModule.Scripts.Data;
using Features.TipsModule.Scripts.Views;
using UnityEngine.InputSystem;

namespace Features.TipsModule.Scripts.Display
{
	[Serializable]
	public abstract class TipDisplayDataBase
	{
		public TipInputDeviceCategory ShowOnInputDevices;

		public TipInputDeviceCategory HideOnInputDevices;

		public InputDeviceIconCustomizationType IconCustomizationType = InputDeviceIconCustomizationType.Tips;

		public bool IsVisibleForDevice(InputDevice activeDevice)
		{
			return TipInputDeviceFilter.IsVisibleForDevice(ShowOnInputDevices, HideOnInputDevices, activeDevice);
		}

		public abstract void Activate(ITipViewHost host, TipBindContext ctx);

		public abstract TipDisplayDataBase Clone();
	}
}
