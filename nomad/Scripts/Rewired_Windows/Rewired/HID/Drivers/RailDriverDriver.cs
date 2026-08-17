using System;
using Rewired.ControllerExtensions;
using Rewired.Utils.Classes.Data;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class RailDriverDriver : HIDDeviceDriver, IDisposable, IDriver_RailDriver, IControllerDriver, IHIDControllerExtension
	{
		private enum dYjgyjeGZdhkrfUGAJoLnclqVxJfc
		{
			Speaker = 0,
			LED = 1
		}

		private readonly NativeBuffer wAOqLMsJwVgerEmOOIjPmmiUIYwj;

		private readonly NativeBuffer GdREIdMxNtwbfJdtaGGzijLkBLxcA;

		private bool gyvWwRsYvnFgbrgsDhIQjmfVGUEV;

		private byte[] kopDOVpIeUdfNkeAChiqRMoBfyXEA = new byte[3];

		private readonly IHIDDevice rlyynoIdhufeZfQUnfFbvoTWStKV;

		private readonly HIDProperties VBqbFijUizcKRGtGwFQStpsXVaMg;

		private readonly aMZqdyjJERTAUbjSZWzzHWVxTEnF WFpIPncSNGMqcZNLheXuZePaRXtS;

		bool IDriver_RailDriver.SpeakerEnabled
		{
			get
			{
				return gyvWwRsYvnFgbrgsDhIQjmfVGUEV;
			}
			set
			{
				gyvWwRsYvnFgbrgsDhIQjmfVGUEV = value;
				wLdUEcGkDoiFoGEhvAryYQDyemKPA(dYjgyjeGZdhkrfUGAJoLnclqVxJfc.Speaker, NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
			}
		}

		ushort IHIDControllerExtension.vendorId => VBqbFijUizcKRGtGwFQStpsXVaMg.vendorId;

		ushort IHIDControllerExtension.productId => VBqbFijUizcKRGtGwFQStpsXVaMg.productId;

		string IHIDControllerExtension.productName => VBqbFijUizcKRGtGwFQStpsXVaMg.productName;

		string IHIDControllerExtension.manufacturer => VBqbFijUizcKRGtGwFQStpsXVaMg.manufacturer;

		ushort IHIDControllerExtension.usagePage => VBqbFijUizcKRGtGwFQStpsXVaMg.usagePage;

		ushort IHIDControllerExtension.usage => VBqbFijUizcKRGtGwFQStpsXVaMg.usage;

		public void SetLEDDisplay(int digitIndex, byte digitBitValues)
		{
			if (digitIndex >= 0 && digitIndex < 3)
			{
				kopDOVpIeUdfNkeAChiqRMoBfyXEA[digitIndex] = digitBitValues;
				wLdUEcGkDoiFoGEhvAryYQDyemKPA(dYjgyjeGZdhkrfUGAJoLnclqVxJfc.LED, NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
			}
		}

		void IDriver_RailDriver.SetLEDDisplay(int digitIndex, byte digitBitValues)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetLEDDisplay
			this.SetLEDDisplay(digitIndex, digitBitValues);
		}

		public void SetLEDDisplay(byte digit1BitValues, byte digit2BitValues, byte digit3BitValues)
		{
			kopDOVpIeUdfNkeAChiqRMoBfyXEA[0] = digit1BitValues;
			kopDOVpIeUdfNkeAChiqRMoBfyXEA[1] = digit2BitValues;
			kopDOVpIeUdfNkeAChiqRMoBfyXEA[2] = digit3BitValues;
			wLdUEcGkDoiFoGEhvAryYQDyemKPA(dYjgyjeGZdhkrfUGAJoLnclqVxJfc.LED, NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
		}

		void IDriver_RailDriver.SetLEDDisplay(byte digit1BitValues, byte digit2BitValues, byte digit3BitValues)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetLEDDisplay
			this.SetLEDDisplay(digit1BitValues, digit2BitValues, digit3BitValues);
		}

		public RailDriverDriver(InitArgs P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initArgs");
			}
			rlyynoIdhufeZfQUnfFbvoTWStKV = P_0.hidDevice;
			VBqbFijUizcKRGtGwFQStpsXVaMg = rlyynoIdhufeZfQUnfFbvoTWStKV.properties;
			wAOqLMsJwVgerEmOOIjPmmiUIYwj = new NativeBuffer(15);
			GdREIdMxNtwbfJdtaGGzijLkBLxcA = new NativeBuffer(9);
			WFpIPncSNGMqcZNLheXuZePaRXtS = new aMZqdyjJERTAUbjSZWzzHWVxTEnF(GdREIdMxNtwbfJdtaGGzijLkBLxcA.Pointer, GdREIdMxNtwbfJdtaGGzijLkBLxcA.Length, 9);
			buttons = new RyDagBEfRFfkQlRDvQAHmQXROhrtA[50];
			for (int i = 0; i < 50; i++)
			{
				buttons[i] = new RyDagBEfRFfkQlRDvQAHmQXROhrtA(0, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 9,
					usage = (ushort)i
				});
			}
			axes = new eTBgDLAnVcEreaYiOpvDFMeVVuExA[4]
			{
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(0, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 48,
					dataIndex = 1,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(0, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 49,
					dataIndex = 2,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(0, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 49,
					dataIndex = 3,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(0, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 50,
					dataIndex = 4,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127)
			};
		}

		public override void Update(UpdateLoopType updateLoop)
		{
		}

		public override bool ParseInputReport(IntPtr inputReportPtr, int inputReportLength, double timestamp)
		{
			if (inputReportPtr == IntPtr.Zero)
			{
				return false;
			}
			if (inputReportLength < wAOqLMsJwVgerEmOOIjPmmiUIYwj.Length)
			{
				return false;
			}
			wAOqLMsJwVgerEmOOIjPmmiUIYwj.Write(inputReportPtr, inputReportLength, wAOqLMsJwVgerEmOOIjPmmiUIYwj.Length);
			zrVpAsktnoPCIXNveyvLHVBExeNK(wAOqLMsJwVgerEmOOIjPmmiUIYwj, timestamp);
			LDJGvqLnFydDhJMnXduxzIERUQI[] array = axes;
			keLuheEiXUjsfqeMeuEPEmoiqGwu(array, wAOqLMsJwVgerEmOOIjPmmiUIYwj, timestamp);
			return true;
		}

		public override Controller.Extension CreateControllerExtension()
		{
			return new RailDriverExtension(this);
		}

		private bool wLdUEcGkDoiFoGEhvAryYQDyemKPA(dYjgyjeGZdhkrfUGAJoLnclqVxJfc P_0, NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_1)
		{
			WjmiOJyKeeumRfmAcsoLiLvdOYUO(P_0);
			return LMaIuMbVBmOhRFZYWTKcdszakAbd(P_1);
		}

		private void WjmiOJyKeeumRfmAcsoLiLvdOYUO(dYjgyjeGZdhkrfUGAJoLnclqVxJfc P_0)
		{
			switch (P_0)
			{
			case dYjgyjeGZdhkrfUGAJoLnclqVxJfc.Speaker:
				GdREIdMxNtwbfJdtaGGzijLkBLxcA.Clear();
				GdREIdMxNtwbfJdtaGGzijLkBLxcA[1] = 133;
				GdREIdMxNtwbfJdtaGGzijLkBLxcA[7] = (gyvWwRsYvnFgbrgsDhIQjmfVGUEV ? ((byte)1) : ((byte)0));
				break;
			case dYjgyjeGZdhkrfUGAJoLnclqVxJfc.LED:
				GdREIdMxNtwbfJdtaGGzijLkBLxcA.Clear();
				GdREIdMxNtwbfJdtaGGzijLkBLxcA[1] = 134;
				GdREIdMxNtwbfJdtaGGzijLkBLxcA[2] = kopDOVpIeUdfNkeAChiqRMoBfyXEA[0];
				GdREIdMxNtwbfJdtaGGzijLkBLxcA[3] = kopDOVpIeUdfNkeAChiqRMoBfyXEA[1];
				GdREIdMxNtwbfJdtaGGzijLkBLxcA[4] = kopDOVpIeUdfNkeAChiqRMoBfyXEA[2];
				break;
			default:
				throw new NotImplementedException();
			}
		}

		private bool LMaIuMbVBmOhRFZYWTKcdszakAbd(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			switch (P_0)
			{
			case NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous:
				return rlyynoIdhufeZfQUnfFbvoTWStKV.WriteSync(WFpIPncSNGMqcZNLheXuZePaRXtS, 0);
			case NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous:
				rlyynoIdhufeZfQUnfFbvoTWStKV.WriteAsync(WFpIPncSNGMqcZNLheXuZePaRXtS, 1000);
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		private void zrVpAsktnoPCIXNveyvLHVBExeNK(NativeBuffer P_0, double P_1)
		{
			for (int i = 0; i < 6; i++)
			{
				byte b = P_0[8 + i];
				int num = i * 8;
				for (int j = 0; j < 8; j++)
				{
					int num2 = num + j;
					if (num2 >= 44)
					{
						break;
					}
					buttons[num2].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & (1 << j)) != 0, P_1);
				}
			}
			byte b2 = P_0[6];
			buttons[44].RWoHtlZxvbZGShUfgFhbuBHdgLlD(b2 < 95, P_1);
			buttons[45].RWoHtlZxvbZGShUfgFhbuBHdgLlD(b2 >= 95 && b2 < 161, P_1);
			buttons[46].RWoHtlZxvbZGShUfgFhbuBHdgLlD(b2 >= 161, P_1);
			b2 = P_0[7];
			buttons[47].RWoHtlZxvbZGShUfgFhbuBHdgLlD(b2 < 95, P_1);
			buttons[48].RWoHtlZxvbZGShUfgFhbuBHdgLlD(b2 >= 95 && b2 < 161, P_1);
			buttons[49].RWoHtlZxvbZGShUfgFhbuBHdgLlD(b2 >= 161, P_1);
		}

		private void keLuheEiXUjsfqeMeuEPEmoiqGwu(LDJGvqLnFydDhJMnXduxzIERUQI[] P_0, NativeBuffer P_1, double P_2)
		{
			for (int i = 0; i < P_0.Length; i++)
			{
				P_0[i].asArJiunXbfpvgEDUosbEuyCYgWWA(P_1, P_2);
			}
		}

		~RailDriverDriver()
		{
			Dispose(disposing: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (base.disposed)
			{
				return;
			}
			base.Dispose(disposing);
			if (disposing)
			{
				if (wAOqLMsJwVgerEmOOIjPmmiUIYwj != null)
				{
					wAOqLMsJwVgerEmOOIjPmmiUIYwj.Dispose();
				}
				if (GdREIdMxNtwbfJdtaGGzijLkBLxcA != null)
				{
					GdREIdMxNtwbfJdtaGGzijLkBLxcA.Dispose();
				}
			}
		}

		public static bool Matches(int vid, int pid)
		{
			if (1523 == vid)
			{
				return 210 == pid;
			}
			return false;
		}
	}
}
