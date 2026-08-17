using System;

namespace Rewired.Platforms.Custom
{
	public sealed class CustomPlatformInitOptions
	{
		internal const int dBSYyAFNtBKHZEQnItfackEhZWjR = -1;

		public int platformId = -1;

		public string platformIdentifierString;

		public CustomInputSource inputSource;

		public IHardwareJoystickMapCustomPlatformMapProvider hardwareJoystickMapCustomPlatformMapProvider;

		public CustomPlatformConfigVars configVars = new CustomPlatformConfigVars();

		public CustomPlatformInitOptions()
		{
		}

		public CustomPlatformInitOptions(CustomPlatformInitOptions P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("other");
			}
			platformId = P_0.platformId;
			platformIdentifierString = P_0.platformIdentifierString;
			inputSource = P_0.inputSource;
			hardwareJoystickMapCustomPlatformMapProvider = P_0.hardwareJoystickMapCustomPlatformMapProvider;
			configVars = ((P_0.configVars != null) ? P_0.configVars : null);
		}
	}
}
