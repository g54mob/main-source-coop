using System;
using Rewired.ControllerExtensions;
using Rewired.Drivers.Interfaces;
using Rewired.Utils.Classes.Data;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class RailDriverDriver : HIDDeviceDriver, IDisposable, IControllerDriver, IDriver_RailDriver
	{
		private enum hEQDgoTwOIkOVKNMrjRfiUKvuKVkA
		{
			Speaker = 0,
			LED = 1
		}

		private const int rJlYmyHWAhBaDkivVCgQFAleSILlA = 1523;

		private const int buyYzIhAqVoCQEkjWxwmDJvxzKBk = 210;

		private const int VnQiGrFElxZQckFfuctAXoBRfYio = 50;

		private const int VeycoCZLIIDyfhtnOqYLNgrdmbfX = 44;

		private const int TzUFoNfBelAgCbOmskLbghtRMhEW = 6;

		private const int rAlVDoZczYLcmzjjiqkhZcPZpXfq = 44;

		private const int yvAgJnuISddVYdlSovnHCYBAjJSOA = 45;

		private const int zjcgIsxUIridpWKoDDOuEtTyKRDQA = 46;

		private const int kgIfXCcYwXfzhKXpSCzWFZXCrWuMb = 47;

		private const int XlDmWUStfhpCDNEgpRVlWYAGfurG = 48;

		private const int ecJZlUmiFYxXpLDUwWzznYlRbnOw = 49;

		private const int jEEWMMHafPrjdivhXJSEwDJfRTOT = 0;

		private const int gZPHMolSuPfiHAQtRyheomdhgTbkA = 15;

		private const int xCwSbUwJdYatLZgivzUmrMznxecq = 9;

		private const int kZqDlBpMkBdLlGfXBtyTFwpoDKuD = 1;

		private const int jHdzdSIGnRTeTwaFTxKiVhpTRKEQ = 2;

		private const int BwDCOaLqWQeXlzGRHNpGKCSqdXZc = 3;

		private const int mJrXTQrIqSNfAzGCXhbMLjpZivCw = 4;

		private const int HDiFKjHDizmimEYsxEDJlDGbhTCt = 5;

		private const int drTQULOhqVyBYxoHrTZjdOspsaGk = 6;

		private const int QrBwKSjDxLkYPGguhdUrzECIuXnS = 7;

		private const int uTUcFxblaqtfVenrbaDdwAjDaSftA = 8;

		private const int eTYbWnsBFHxjSoiaFYGqtrDPVIVB = 14;

		private const int ionaOrxWsrfSHVJibBgxMYKNIbce = 3;

		private const int lTzWdCVxiglAdUudWhIxXCdYGMv = 7;

		private readonly NativeBuffer HLsWXNxbuFUDCLhxfUYoppWgHBGi;

		private readonly NativeBuffer CAPBXhcnJamPmJsEbuPqwfyjKPHdb;

		private bool qNYpQMjBmERfozJDnVqxPmtStZVB;

		private byte[] TjTkowNNzdhTHPEznvFEhKtYoTXe = new byte[3];

		private readonly OutputReport BuQEXisXIqFXvdwXReGSUsjHrfng;

		private readonly Func<OutputReport, bool> nUfHkEItsYftJzYkqNavhDPHdlCe;

		private readonly Action<OutputReport> KMtNyOhjxErEtpBItWsLNqmherMHA;

		public bool SpeakerEnabled
		{
			get
			{
				return qNYpQMjBmERfozJDnVqxPmtStZVB;
			}
			set
			{
				qNYpQMjBmERfozJDnVqxPmtStZVB = value;
				bROjQYFTsjmSyLGlwglxdUhGIYpT(hEQDgoTwOIkOVKNMrjRfiUKvuKVkA.Speaker, patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous);
			}
		}

		public void SetLEDDisplay(int digitIndex, byte digitBitValues)
		{
			if (digitIndex >= 0 && digitIndex < 3)
			{
				TjTkowNNzdhTHPEznvFEhKtYoTXe[digitIndex] = digitBitValues;
				bROjQYFTsjmSyLGlwglxdUhGIYpT(hEQDgoTwOIkOVKNMrjRfiUKvuKVkA.LED, patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous);
			}
		}

		public void SetLEDDisplay(byte digit1BitValues, byte digit2BitValues, byte digit3BitValues)
		{
			TjTkowNNzdhTHPEznvFEhKtYoTXe[0] = digit1BitValues;
			TjTkowNNzdhTHPEznvFEhKtYoTXe[1] = digit2BitValues;
			TjTkowNNzdhTHPEznvFEhKtYoTXe[2] = digit3BitValues;
			bROjQYFTsjmSyLGlwglxdUhGIYpT(hEQDgoTwOIkOVKNMrjRfiUKvuKVkA.LED, patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous);
		}

		public RailDriverDriver(InitArgs P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initArgs");
			}
			HLsWXNxbuFUDCLhxfUYoppWgHBGi = new NativeBuffer(15);
			CAPBXhcnJamPmJsEbuPqwfyjKPHdb = new NativeBuffer(9);
			BuQEXisXIqFXvdwXReGSUsjHrfng = new OutputReport(CAPBXhcnJamPmJsEbuPqwfyjKPHdb.Pointer, CAPBXhcnJamPmJsEbuPqwfyjKPHdb.Length, 9);
			nUfHkEItsYftJzYkqNavhDPHdlCe = P_0.synchronousWriteOutputReportDelegate;
			KMtNyOhjxErEtpBItWsLNqmherMHA = P_0.asynchronousWriteOutputReportDelegate;
			buttons = new HIDButton[50];
			for (int i = 0; i < 50; i++)
			{
				buttons[i] = new HIDButton(0, new HIDControllerElement.HIDInfo
				{
					usagePage = 9,
					usage = (ushort)i
				});
			}
			axes = new HIDAxis[4]
			{
				new HIDAxis(0, new HIDControllerElement.HIDInfo
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
				new HIDAxis(0, new HIDControllerElement.HIDInfo
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
				new HIDAxis(0, new HIDControllerElement.HIDInfo
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
				new HIDAxis(0, new HIDControllerElement.HIDInfo
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
			if (inputReportLength < HLsWXNxbuFUDCLhxfUYoppWgHBGi.Length)
			{
				return false;
			}
			HLsWXNxbuFUDCLhxfUYoppWgHBGi.Write(inputReportPtr, inputReportLength, HLsWXNxbuFUDCLhxfUYoppWgHBGi.Length);
			MGEvRBxTXmrzVAbFgmVqKAAmKjlj(HLsWXNxbuFUDCLhxfUYoppWgHBGi, timestamp);
			HIDControllerElement[] array = axes;
			qWGyimoreyEdVdLhdiNleELnOMKsA(array, HLsWXNxbuFUDCLhxfUYoppWgHBGi, timestamp);
			return true;
		}

		public override Controller.Extension CreateControllerExtension()
		{
			return new RailDriverExtension(this);
		}

		private bool bROjQYFTsjmSyLGlwglxdUhGIYpT(hEQDgoTwOIkOVKNMrjRfiUKvuKVkA P_0, patCHGFpzQFWiFajobvxWhTzPZfhA P_1)
		{
			WQGtVGagMjJCQPcACQAcaZKqfuhJ(P_0);
			return zSmfRRtQOmVGUcQwVOBYunHIHatM(P_1);
		}

		private void WQGtVGagMjJCQPcACQAcaZKqfuhJ(hEQDgoTwOIkOVKNMrjRfiUKvuKVkA P_0)
		{
			switch (P_0)
			{
			case hEQDgoTwOIkOVKNMrjRfiUKvuKVkA.Speaker:
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb.Clear();
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[1] = 133;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[7] = (byte)(qNYpQMjBmERfozJDnVqxPmtStZVB ? 1 : 0);
				break;
			case hEQDgoTwOIkOVKNMrjRfiUKvuKVkA.LED:
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb.Clear();
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[1] = 134;
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[2] = TjTkowNNzdhTHPEznvFEhKtYoTXe[0];
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[3] = TjTkowNNzdhTHPEznvFEhKtYoTXe[1];
				CAPBXhcnJamPmJsEbuPqwfyjKPHdb[4] = TjTkowNNzdhTHPEznvFEhKtYoTXe[2];
				break;
			default:
				throw new NotImplementedException();
			}
		}

		private bool zSmfRRtQOmVGUcQwVOBYunHIHatM(patCHGFpzQFWiFajobvxWhTzPZfhA P_0)
		{
			switch (P_0)
			{
			case patCHGFpzQFWiFajobvxWhTzPZfhA.Synchronous:
				if (nUfHkEItsYftJzYkqNavhDPHdlCe == null)
				{
					return false;
				}
				return nUfHkEItsYftJzYkqNavhDPHdlCe(BuQEXisXIqFXvdwXReGSUsjHrfng);
			case patCHGFpzQFWiFajobvxWhTzPZfhA.Asynchronous:
				if (KMtNyOhjxErEtpBItWsLNqmherMHA == null)
				{
					return false;
				}
				KMtNyOhjxErEtpBItWsLNqmherMHA(BuQEXisXIqFXvdwXReGSUsjHrfng);
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		private void MGEvRBxTXmrzVAbFgmVqKAAmKjlj(NativeBuffer P_0, double P_1)
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
					buttons[num2].SetValue((b & (1 << j)) != 0, P_1);
				}
			}
			byte b2 = P_0[6];
			buttons[44].SetValue(b2 < 95, P_1);
			buttons[45].SetValue(b2 >= 95 && b2 < 161, P_1);
			buttons[46].SetValue(b2 >= 161, P_1);
			b2 = P_0[7];
			buttons[47].SetValue(b2 < 95, P_1);
			buttons[48].SetValue(b2 >= 95 && b2 < 161, P_1);
			buttons[49].SetValue(b2 >= 161, P_1);
		}

		private void qWGyimoreyEdVdLhdiNleELnOMKsA(HIDControllerElement[] P_0, NativeBuffer P_1, double P_2)
		{
			for (int i = 0; i < P_0.Length; i++)
			{
				P_0[i].UpdateValue(P_1, P_2);
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
				if (HLsWXNxbuFUDCLhxfUYoppWgHBGi != null)
				{
					HLsWXNxbuFUDCLhxfUYoppWgHBGi.Dispose();
				}
				if (CAPBXhcnJamPmJsEbuPqwfyjKPHdb != null)
				{
					CAPBXhcnJamPmJsEbuPqwfyjKPHdb.Dispose();
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
