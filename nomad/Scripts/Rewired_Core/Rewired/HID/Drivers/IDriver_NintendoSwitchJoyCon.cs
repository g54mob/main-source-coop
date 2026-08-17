using Rewired.ControllerExtensions;
using Rewired.Interfaces;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	internal interface IDriver_NintendoSwitchJoyCon : IDriver_NintendoSwitchController, IControllerDriver, IHIDControllerExtension, IAxisCalibrationIndexMap
	{
		NintendoSwitchJoyConType joyConType { get; }

		NintendoSwitchJoyConGripStyle joyConGripStyle { get; set; }
	}
}
