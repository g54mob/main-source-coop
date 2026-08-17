using System;
using Rewired.Interfaces;

namespace Rewired
{
	[CustomClassObfuscation(renamePubIntMembers = false, renamePrivateMembers = true)]
	[CustomObfuscation(rename = false)]
	internal class UpdateControllerInfoEventArgs : EventArgs
	{
		public readonly IInputManagerJoystickPublic sourceJoystick;

		public UpdateControllerInfoEventArgs(IInputManagerJoystickPublic P_0)
		{
			sourceJoystick = P_0;
		}
	}
}
