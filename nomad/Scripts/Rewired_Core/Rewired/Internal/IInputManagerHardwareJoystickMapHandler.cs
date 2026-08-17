namespace Rewired.Internal
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal interface IInputManagerHardwareJoystickMapHandler
	{
		void InitializeHardwareJoystickMap(HardwareJoystickMap_InputManager hardwareMap);
	}
}
