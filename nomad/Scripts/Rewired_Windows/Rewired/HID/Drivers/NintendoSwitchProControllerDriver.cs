using Rewired.ControllerExtensions;
using Rewired.Utils.Classes.Data;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal sealed class NintendoSwitchProControllerDriver : NintendoSwitchGamepadDriver, IDriver_NintendoSwitchProController, IDriver_NintendoSwitchController, IControllerDriver, IHIDControllerExtension
	{
		private readonly byte[] NVLMuZcWShiYTnotXROyLjaYYEoM = new byte[6];

		private readonly NativeBuffer BPOQrConnOIMfJzCORqBWOHhbklE;

		public NintendoSwitchProControllerDriver(InitArgs P_0)
			: base(P_0, EpsjEqBsgEWVUiCpmfVMyaeRIRPM.ProController, 18, 4, 2)
		{
			BPOQrConnOIMfJzCORqBWOHhbklE = new NativeBuffer(9);
			axes = new eTBgDLAnVcEreaYiOpvDFMeVVuExA[4]
			{
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(48, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 48,
					dataIndex = 1,
					bitSize = 16,
					logicalMin = 0,
					logicalMax = 65535,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 32767),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(48, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 49,
					dataIndex = 3,
					bitSize = 16,
					logicalMin = 0,
					logicalMax = 65535,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 32767),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(48, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 50,
					dataIndex = 5,
					bitSize = 16,
					logicalMin = 0,
					logicalMax = 65535,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 32767),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(48, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 53,
					dataIndex = 7,
					bitSize = 16,
					logicalMin = 0,
					logicalMax = 65535,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 32767)
			};
			Initialize();
		}

		public override void Update(UpdateLoopType updateLoop)
		{
			base.Update(updateLoop);
		}

		public override Controller.Extension CreateControllerExtension()
		{
			return new NintendoSwitchProControllerExtension(this);
		}

		protected override void UpdateButtons(NativeBuffer inputReport, double timestamp)
		{
			inputReport.Read(NVLMuZcWShiYTnotXROyLjaYYEoM, 3, 3);
			buttons[0].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[0] & 4) != 0, timestamp);
			buttons[1].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[0] & 8) != 0, timestamp);
			buttons[2].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[0] & 1) != 0, timestamp);
			buttons[3].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[0] & 2) != 0, timestamp);
			buttons[4].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[2] & 0x40) != 0, timestamp);
			buttons[5].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[0] & 0x40) != 0, timestamp);
			buttons[6].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[2] & 0x80) != 0, timestamp);
			buttons[7].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[0] & 0x80) != 0, timestamp);
			buttons[8].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[1] & 1) != 0, timestamp);
			buttons[9].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[1] & 2) != 0, timestamp);
			buttons[10].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[1] & 0x20) != 0, timestamp);
			buttons[11].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[1] & 0x10) != 0, timestamp);
			buttons[12].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[1] & 8) != 0, timestamp);
			buttons[13].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[1] & 4) != 0, timestamp);
			buttons[14].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[2] & 2) != 0, timestamp);
			buttons[15].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[2] & 4) != 0, timestamp);
			buttons[16].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[2] & 1) != 0, timestamp);
			buttons[17].RWoHtlZxvbZGShUfgFhbuBHdgLlD((NVLMuZcWShiYTnotXROyLjaYYEoM[2] & 8) != 0, timestamp);
		}

		protected override void UpdateElements(LDJGvqLnFydDhJMnXduxzIERUQI[] elements, NativeBuffer inputReport, double timestamp)
		{
			inputReport.Read(NVLMuZcWShiYTnotXROyLjaYYEoM, 6, 6);
			byte[] nVLMuZcWShiYTnotXROyLjaYYEoM = NVLMuZcWShiYTnotXROyLjaYYEoM;
			int num = 0;
			ushort valueX = (ushort)(nVLMuZcWShiYTnotXROyLjaYYEoM[num] | ((nVLMuZcWShiYTnotXROyLjaYYEoM[1 + num] & 0xF) << 8));
			ushort valueY = (ushort)((nVLMuZcWShiYTnotXROyLjaYYEoM[1 + num] >> 4) | (nVLMuZcWShiYTnotXROyLjaYYEoM[2 + num] << 4));
			num = 3;
			ushort valueX2 = (ushort)(nVLMuZcWShiYTnotXROyLjaYYEoM[num] | ((nVLMuZcWShiYTnotXROyLjaYYEoM[1 + num] & 0xF) << 8));
			ushort valueY2 = (ushort)((nVLMuZcWShiYTnotXROyLjaYYEoM[1 + num] >> 4) | (nVLMuZcWShiYTnotXROyLjaYYEoM[2 + num] << 4));
			GetCalibratedStickValue(valueX, valueY, GetAxisCalibration(0), GetAxisCalibration(1), out var calibratedX, out var calibratedY);
			GetCalibratedStickValue(valueX2, valueY2, GetAxisCalibration(2), GetAxisCalibration(3), out var calibratedX2, out var calibratedY2);
			BPOQrConnOIMfJzCORqBWOHhbklE.Write((byte)48, 0);
			BPOQrConnOIMfJzCORqBWOHhbklE.Write(calibratedX, 1);
			BPOQrConnOIMfJzCORqBWOHhbklE.Write(calibratedY, 3);
			BPOQrConnOIMfJzCORqBWOHhbklE.Write(calibratedX2, 5);
			BPOQrConnOIMfJzCORqBWOHhbklE.Write(calibratedY2, 7);
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i].asArJiunXbfpvgEDUosbEuyCYgWWA(BPOQrConnOIMfJzCORqBWOHhbklE, timestamp);
			}
		}

		~NintendoSwitchProControllerDriver()
		{
			Dispose(disposing: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (!base.disposed)
			{
				if (disposing && BPOQrConnOIMfJzCORqBWOHhbklE != null)
				{
					BPOQrConnOIMfJzCORqBWOHhbklE.Dispose();
				}
				base.Dispose(disposing);
			}
		}

		public static bool Matches(int vid, int pid)
		{
			if (vid == 1406)
			{
				return pid == 8201;
			}
			return false;
		}
	}
}
