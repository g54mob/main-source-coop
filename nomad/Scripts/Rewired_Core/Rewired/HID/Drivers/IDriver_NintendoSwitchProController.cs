using Rewired.ControllerExtensions;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	internal interface IDriver_NintendoSwitchProController : IDriver_NintendoSwitchController, IControllerDriver, IHIDControllerExtension
	{
	}
}
