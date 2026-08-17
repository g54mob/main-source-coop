using System;
using Rewired.Data;
using Rewired.Interfaces;
using Rewired.Platforms;
using Rewired.Utils;

namespace Rewired.InputManagers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class Initializer : PlatformInitializer
	{
		internal const string initErrorMsg = "Please see the Installation section of the documentation for information on required libraries. Documentation can be found in the menu: Window -> Rewired -> Help -> Documentation.";

		private static PlatformInitializer HVRiWsLCvmvdlCzibTjWzIjfSPzK;

		public static PlatformInitializer GetPlatformInitializer()
		{
			if (HVRiWsLCvmvdlCzibTjWzIjfSPzK == null)
			{
				HVRiWsLCvmvdlCzibTjWzIjfSPzK = new Initializer();
			}
			return HVRiWsLCvmvdlCzibTjWzIjfSPzK;
		}

		public override object Initialize(IConfigVars_Internal configVars)
		{
			if (UnityTools.platform == Platform.Windows || UnityTools.platform == Platform.WindowsAppStore)
			{
				try
				{
					return new PRPvEnHMXBtNOrkiBYneErPlRGdP((ConfigVars)configVars, ReInput.GetHardwareJoystickMap_InputManager, ReInput.GetNewJoystickId);
				}
				catch (Exception)
				{
					Logger.LogWarning("Rewired will fall back to Unity input. Certain features may not be available.\nPlease see the Installation section of the documentation for information on required libraries. Documentation can be found in the menu: Window -> Rewired -> Help -> Documentation.");
					return null;
				}
			}
			return null;
		}

		public override IElementIdentifierTool CreateTool(string inputSourceString)
		{
			if (inputSourceString == "DirectInput")
			{
				return new IYEfrYckYTRMFLDxdEQtEATSabUv();
			}
			if (inputSourceString == "RawInput")
			{
				return new ePgLYuadsDMdgfpWdCYYYFmXGbnM();
			}
			return null;
		}
	}
}
