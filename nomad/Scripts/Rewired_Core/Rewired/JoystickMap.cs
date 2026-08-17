using System;

namespace Rewired
{
	public sealed class JoystickMap : ControllerMapWithAxes
	{
		public JoystickMap()
		{
			_controllerType = ControllerType.Joystick;
		}

		public JoystickMap(JoystickMap P_0)
			: base(P_0)
		{
		}

		internal void LpfidFzDLEdNHIRSSPKMLjUqaoaLA(Guid P_0, int P_1, int P_2)
		{
			_hardwareGuid = P_0;
			_categoryId = P_1;
			_layoutId = P_2;
		}

		internal static JoystickMap PfrqCKEIgMPkwkEXazIGyUcXeKcP(Guid P_0, int P_1, int P_2)
		{
			return new JoystickMap
			{
				_hardwareGuid = P_0,
				_categoryId = P_1,
				_layoutId = P_2,
				_sourceMapId = -1
			};
		}
	}
}
