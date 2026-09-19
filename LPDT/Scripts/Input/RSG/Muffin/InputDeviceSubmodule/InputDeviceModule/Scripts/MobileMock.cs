using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;

namespace RSG.Muffin.InputDeviceSubmodule.InputDeviceModule.Scripts
{
	[InputControlLayout(displayName = "MobileMock")]
	public class MobileMock : InputDevice
	{
		[InputControl(layout = "stick", usage = "LeftStick")]
		public StickControl LeftStick { get; private set; }

		[InputControl(layout = "stick", usage = "RightStick")]
		public StickControl RightStick { get; private set; }

		protected override void FinishSetup()
		{
			LeftStick = GetChildControl<StickControl>("LeftStick");
			RightStick = GetChildControl<StickControl>("RightStick");
			base.FinishSetup();
		}

		static MobileMock()
		{
			InputSystem.RegisterLayout<MobileMock>(null, default(InputDeviceMatcher).WithInterface("MobileMock"));
		}
	}
}
