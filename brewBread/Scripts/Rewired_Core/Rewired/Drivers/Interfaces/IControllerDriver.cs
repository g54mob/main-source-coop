namespace Rewired.Drivers.Interfaces
{
	[CustomClassObfuscation(renamePrivateMembers = false, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal interface IControllerDriver
	{
		int AxisCount { get; }

		int ButtonCount { get; }

		int HatCount { get; }

		int AccelerometerCount { get; }

		int GyroscopeCount { get; }

		int TouchpadCount { get; }

		int LightCount { get; }

		int VibrationMotorCount { get; }
	}
}
