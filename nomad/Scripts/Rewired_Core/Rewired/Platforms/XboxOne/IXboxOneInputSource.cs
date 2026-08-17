namespace Rewired.Platforms.XboxOne
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	internal interface IXboxOneInputSource
	{
		int GetXboxOneUserIdFromUnityJoystick(int unityJoystickId);

		bool SetXboxOneVibration(ulong xboxOneJoystickId, gPBXUmUByuNZDOTYEcbeSlXNYIzu vibration);

		void PulseVibrateMotor(ulong xboxOneJoystickId, XboxOneGamepadMotorType motor, float startLevel, float endLevel, float duration);
	}
}
