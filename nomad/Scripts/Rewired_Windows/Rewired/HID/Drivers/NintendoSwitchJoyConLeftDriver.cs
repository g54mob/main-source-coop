using Rewired.ControllerExtensions;
using Rewired.Utils.Classes.Data;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal sealed class NintendoSwitchJoyConLeftDriver : NintendoSwitchJoyConDriver
	{
		int NintendoSwitchJoyConDriver.byteIndexStartSticks => 6;

		public NintendoSwitchJoyConLeftDriver(InitArgs P_0)
			: base(P_0, EpsjEqBsgEWVUiCpmfVMyaeRIRPM.JoyConLeft)
		{
		}

		protected override void UpdateButtons(NativeBuffer inputReport, double timestamp)
		{
			byte[] array = base.buttonAxisReadBuffer;
			inputReport.Read(array, 2, 4);
			byte b = array[1];
			if (base.Rewired_002EHID_002EDrivers_002EIDriver_NintendoSwitchJoyCon_002EjoyConGripStyle == NintendoSwitchJoyConGripStyle.Horizontal)
			{
				buttons[0].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 8) != 0, timestamp);
				buttons[1].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 1) != 0, timestamp);
				buttons[2].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 2) != 0, timestamp);
				buttons[3].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 4) != 0, timestamp);
			}
			else
			{
				buttons[0].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 1) != 0, timestamp);
				buttons[1].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 4) != 0, timestamp);
				buttons[2].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 8) != 0, timestamp);
				buttons[3].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 2) != 0, timestamp);
			}
			buttons[4].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x20) != 0, timestamp);
			buttons[5].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x10) != 0, timestamp);
			buttons[6].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x40) != 0, timestamp);
			buttons[7].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x80) != 0, timestamp);
			b = array[0];
			buttons[8].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 1) != 0, timestamp);
			buttons[9].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x20) != 0, timestamp);
			buttons[10].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 8) != 0, timestamp);
		}

		protected override void HandleGripStyleStickAxisSwap(ref ushort stickX, ref ushort stickY)
		{
			if (base.Rewired_002EHID_002EDrivers_002EIDriver_NintendoSwitchJoyCon_002EjoyConGripStyle == NintendoSwitchJoyConGripStyle.Horizontal)
			{
				ushort num = stickY;
				stickY = stickX;
				stickX = (ushort)(65535 - num);
			}
		}

		~NintendoSwitchJoyConLeftDriver()
		{
			Dispose(disposing: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (!base.disposed)
			{
				base.Dispose(disposing);
			}
		}

		public static bool Matches(int vid, int pid)
		{
			if (vid == 1406)
			{
				return pid == 8198;
			}
			return false;
		}
	}
}
