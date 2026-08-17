using System;

namespace Rewired
{
	public sealed class JoystickMapSaveData : ControllerMapSaveData
	{
		public Joystick joystick
		{
			get
			{
				if (ReInput._id != PbuCWLhPESGXtsXepbdkNpjZHdmp)
				{
					ReInput.CheckInitialized(PbuCWLhPESGXtsXepbdkNpjZHdmp);
					return null;
				}
				return _controller as Joystick;
			}
		}

		public JoystickMap joystickMap
		{
			get
			{
				if (ReInput._id != PbuCWLhPESGXtsXepbdkNpjZHdmp)
				{
					ReInput.CheckInitialized(PbuCWLhPESGXtsXepbdkNpjZHdmp);
					return null;
				}
				return _map as JoystickMap;
			}
		}

		public Guid joystickHardwareTypeGuid
		{
			get
			{
				if (ReInput._id != PbuCWLhPESGXtsXepbdkNpjZHdmp)
				{
					ReInput.CheckInitialized(PbuCWLhPESGXtsXepbdkNpjZHdmp);
					return Guid.Empty;
				}
				return joystick.hardwareTypeGuid;
			}
		}

		internal JoystickMapSaveData(Joystick P_0, JoystickMap P_1)
			: base(P_0, P_1)
		{
		}
	}
}
