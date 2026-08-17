using System;
using Rewired.ControllerExtensions;
using Rewired.Interfaces;
using Rewired.Utils.Classes.Data;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal abstract class NintendoSwitchJoyConDriver : NintendoSwitchGamepadDriver, IDriver_NintendoSwitchJoyCon, IDriver_NintendoSwitchController, IControllerDriver, IHIDControllerExtension, IAxisCalibrationIndexMap
	{
		private readonly NativeBuffer DzACdnrXXPxQrVhzpffTFxKytKnM;

		private readonly NintendoSwitchJoyConType uzONFFJEJoCpwAIeAMmLnjvzxGeU;

		private NintendoSwitchJoyConGripStyle tKZkXqTOFlBuRDQlHBXGfCmyPzTLA;

		private readonly byte[] LgxaeNmVnzodWtHtYcKuGuITBKhwA = new byte[3];

		protected byte[] buttonAxisReadBuffer => LgxaeNmVnzodWtHtYcKuGuITBKhwA;

		protected abstract int byteIndexStartSticks { get; }

		NintendoSwitchJoyConType IDriver_NintendoSwitchJoyCon.joyConType => uzONFFJEJoCpwAIeAMmLnjvzxGeU;

		NintendoSwitchJoyConGripStyle IDriver_NintendoSwitchJoyCon.joyConGripStyle
		{
			get
			{
				return tKZkXqTOFlBuRDQlHBXGfCmyPzTLA;
			}
			set
			{
				tKZkXqTOFlBuRDQlHBXGfCmyPzTLA = value;
			}
		}

		int IAxisCalibrationIndexMap.GetMappedAxisIndex(int elementIndex)
		{
			if (elementIndex < 0 || elementIndex > 1)
			{
				return elementIndex;
			}
			if (tKZkXqTOFlBuRDQlHBXGfCmyPzTLA == NintendoSwitchJoyConGripStyle.Vertical)
			{
				if (elementIndex == 0)
				{
					return 1;
				}
				return 0;
			}
			return elementIndex;
		}

		protected NintendoSwitchJoyConDriver(InitArgs P_0, EpsjEqBsgEWVUiCpmfVMyaeRIRPM P_1)
			: base(P_0, P_1, 11, 2, 1)
		{
			if (P_1 != EpsjEqBsgEWVUiCpmfVMyaeRIRPM.JoyConLeft && P_1 != EpsjEqBsgEWVUiCpmfVMyaeRIRPM.JoyConRight)
			{
				throw new ArgumentException("controllerType");
			}
			uzONFFJEJoCpwAIeAMmLnjvzxGeU = ((P_1 != EpsjEqBsgEWVUiCpmfVMyaeRIRPM.JoyConLeft) ? NintendoSwitchJoyConType.Right : NintendoSwitchJoyConType.Left);
			tKZkXqTOFlBuRDQlHBXGfCmyPzTLA = NintendoSwitchJoyConGripStyle.Horizontal;
			DzACdnrXXPxQrVhzpffTFxKytKnM = new NativeBuffer(5);
			axes = new eTBgDLAnVcEreaYiOpvDFMeVVuExA[2]
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
				}, false, 32767)
			};
			Initialize();
		}

		public override Controller.Extension CreateControllerExtension()
		{
			return new NintendoSwitchJoyConExtension(this);
		}

		protected override void UpdateElements(LDJGvqLnFydDhJMnXduxzIERUQI[] elements, NativeBuffer inputReport, double timestamp)
		{
			byte[] lgxaeNmVnzodWtHtYcKuGuITBKhwA = LgxaeNmVnzodWtHtYcKuGuITBKhwA;
			inputReport.Read(lgxaeNmVnzodWtHtYcKuGuITBKhwA, 3, byteIndexStartSticks);
			ushort valueX = (ushort)(lgxaeNmVnzodWtHtYcKuGuITBKhwA[0] | ((lgxaeNmVnzodWtHtYcKuGuITBKhwA[1] & 0xF) << 8));
			ushort valueY = (ushort)((lgxaeNmVnzodWtHtYcKuGuITBKhwA[1] >> 4) | (lgxaeNmVnzodWtHtYcKuGuITBKhwA[2] << 4));
			GetCalibratedStickValue(valueX, valueY, GetAxisCalibration(0), GetAxisCalibration(1), out var calibratedX, out var calibratedY);
			HandleGripStyleStickAxisSwap(ref calibratedX, ref calibratedY);
			DzACdnrXXPxQrVhzpffTFxKytKnM.Write((byte)48, 0);
			DzACdnrXXPxQrVhzpffTFxKytKnM.Write(calibratedX, 1);
			DzACdnrXXPxQrVhzpffTFxKytKnM.Write(calibratedY, 3);
			for (int i = 0; i < elements.Length; i++)
			{
				elements[i].asArJiunXbfpvgEDUosbEuyCYgWWA(DzACdnrXXPxQrVhzpffTFxKytKnM, timestamp);
			}
		}

		protected abstract void HandleGripStyleStickAxisSwap(ref ushort stickX, ref ushort stickY);

		~NintendoSwitchJoyConDriver()
		{
			Dispose(disposing: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (!base.disposed)
			{
				if (disposing && DzACdnrXXPxQrVhzpffTFxKytKnM != null)
				{
					DzACdnrXXPxQrVhzpffTFxKytKnM.Dispose();
				}
				base.Dispose(disposing);
			}
		}
	}
}
