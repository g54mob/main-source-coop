using System;

namespace Rewired
{
	public sealed class JoystickCalibrationMapSaveData : CalibrationMapSaveData
	{
		private Guid meFpAiPNzZoWYBkeuadNIeOoHykC;

		public Guid joystickHardwareTypeGuid => meFpAiPNzZoWYBkeuadNIeOoHykC;

		public JoystickCalibrationMapSaveData(CalibrationMap P_0, ControllerType P_1, string P_2, Guid P_3)
			: base(P_0, P_1, P_2)
		{
			meFpAiPNzZoWYBkeuadNIeOoHykC = P_3;
		}
	}
}
