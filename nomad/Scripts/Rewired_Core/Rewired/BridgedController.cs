using System;
using Rewired.Interfaces;
using Rewired.Platforms.Custom;

namespace Rewired
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	internal class BridgedController : BridgedControllerHWInfo
	{
		public IInputManagerJoystickPublic sourceJoystick;

		public HardwareControllerMap_Game gameHardwareMap;

		public Guid controllerTypeGuid;

		public Controller.Extension controllerExtension;

		public string instanceName;

		public string productName;

		public bool isXInputDevice;

		public int axisCount;

		public int buttonCount;

		public bool[] isButtonPressureSensitive;

		public UnknownControllerHat[] unknownControllerHats;

		public CustomInputSource customInputSource;

		public bool isUnknownController => controllerTypeGuid == Guid.Empty;
	}
}
