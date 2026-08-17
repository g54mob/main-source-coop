using System;
using Rewired.Data;

namespace Rewired.Platforms.Custom
{
	[Serializable]
	public class CustomPlatformConfigVars : ConfigVars.PlatformVars
	{
		public bool useNativeKeyboard = true;

		public bool useNativeMouse = true;
	}
}
