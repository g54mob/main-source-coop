using System;
using Rewired.ControllerExtensions;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using UnityEngine;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class DualShock4Driver : HIDDeviceDriver, IDriver_DualShock4, IControllerDriver, IHIDControllerExtension, IDisposable
	{
		private enum vpSznjZXJaSyXmxrUjYlmJyyAlLw
		{
			None = 0,
			XZ = 1,
			Y = 2
		}

		private static class pcqybQuDduhHcAOKcQglFCHbFBAhB
		{
			public unsafe static uint vhlEPaCrKXYPMYciQjXVaaNONLhwA(byte* P_0, int P_1, uint P_2)
			{
				return ~HLPNFjYnnWPQvVGMbXUPyDOHhxzfA(HLPNFjYnnWPQvVGMbXUPyDOHhxzfA(4294967295u, (byte*)(&P_2), 1, 3988292384u), P_0, P_1, 3988292384u);
			}

			private unsafe static uint HLPNFjYnnWPQvVGMbXUPyDOHhxzfA(uint P_0, byte* P_1, int P_2, uint P_3)
			{
				for (int i = 0; i < P_2; i++)
				{
					P_0 ^= P_1[i];
					for (int j = 0; j < 8; j++)
					{
						P_0 = (P_0 >> 1) ^ (((P_0 & 1) != 0) ? P_3 : 0);
					}
				}
				return P_0;
			}
		}

		private enum RQNHrwuFSvzqqBVknotRibPNUUAq
		{
			Discharging = 0,
			Charging = 1,
			Full = 2,
			Unknown = 3
		}

		private readonly IHIDDevice gvodinVafsknMqadZCkzYQfILURO;

		private readonly HIDProperties WzfPXTzpCeLnTZBpgJbYyWuPisrq;

		private readonly bool fEGigHhstdROEnASIpCkcwZoSxnDA;

		private readonly YDvFqJokstcNyQQOYydcruGncmeb JwpkwHojAGPSnMZIVpCkWxcISfLt;

		private readonly int oCXWJdcQfhfNsJouQorPDQlvnYfg;

		private readonly int LBYTuqvFgwgdlEBoHkgheAxewIhHb;

		private readonly bool KyJChzuIjPtnZxOpGNoEEBRcpfTU;

		private readonly byte qnhwOCMoBiBUtGVivOrDEgVYqJsx;

		private readonly int wokLJMSWhfCVhAaltefICpPzKFojA;

		private readonly int gWmJGSqAOvFbIccEroFjNYFVjyZR;

		private readonly int ihabjTsEALCsBkVpaJlksGrfvYXKA;

		private readonly int eBovnLlaCqRjtiftqZCNTFREFHYQ;

		private readonly NativeBuffer fjSOMESmrqGKEQdotDVyULRIUEJF;

		private readonly NativeBuffer TpdrVjRvVLzzaIMSRStdwQcbdETG;

		private readonly aMZqdyjJERTAUbjSZWzzHWVxTEnF NwUMMziSKoMRbNAYpDPxqwtUdeFd;

		private readonly byte[] iLPPIrdoZQskUvWHGzSooCAjrAhE = new byte[1] { 162 };

		private bool OIrczxarJJHYlUFTwUlKAMiqHvsc;

		private bool IGYKiMXPlotzrqUKIiXdiHptPGYl;

		private double szNVaGrKkRdHjVhaXTMTuoXFttiM;

		private int haqhsRtndWAVOjlDBIrdAonKodimA;

		private RQNHrwuFSvzqqBVknotRibPNUUAq RbPbOXdPkErATTcWkYqVWRmOLGhQA = RQNHrwuFSvzqqBVknotRibPNUUAq.Unknown;

		private Quaternion LNnWXKmibVqzopQIjKkHZglWCtwF = Quaternion.identity;

		private ushort utdhnlEcaFhkZcSkjMvvPxmtrkyOB;

		private float fTRRHETsoFNlHFGxAKnHrKkVOibH;

		private double cwkuhuNIDownDHstQHgRrwogbRNk;

		private float zloAXNFvhlMmrFWNFhrTFhXBBflzB;

		private bool ArVDdgjgdnAMicpkknkQfFNuVOwC;

		private bool jnqApNimEdlaHCNMGLbmcsyJekzDc;

		private bool rbIFGRQgfabTGYtaykSwwagYUQCe;

		private bool ycsskyFLipOSQyPjLxbdKCfSUcBc;

		private byte qZPmEtzCikjqwBaWsLohpGDQPICFb;

		private byte SkrTIqBpbDcMuEtjbvVEWajshasO;

		private Quaternion kyoiAAxasuXAjPsRjKcxkhnBnPiy = Quaternion.identity;

		private Quaternion iUqgErqYibOuOetcANIyxQAOyAzl = Quaternion.identity;

		private bool TTBnJIJnDdpPYTBNDQqDNnKuUdaS;

		private int xwenFFYbPOjKeuDcStBrgRYdKmXh;

		private int[] UcPIHWjTLrXINIuEYLOPaszezousB = new int[2];

		private int[] DVlcyHQffOOHieKbmEWxBYrsZSaMA = new int[2];

		private bool isVibrating
		{
			get
			{
				for (int i = 0; i < base.Rewired_002EHID_002EDrivers_002EIControllerDriver_002EVibrationMotorCount; i++)
				{
					if (vibrationMotors[i].ZcjoZwbIDbbFlaWQFjFKWrESBVuu > 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		float IDriver_DualShock4.BatteryLevel => haqhsRtndWAVOjlDBIrdAonKodimA;

		bool IDriver_DualShock4.BatteryCharging => RbPbOXdPkErATTcWkYqVWRmOLGhQA == RQNHrwuFSvzqqBVknotRibPNUUAq.Charging;

		float IDriver_DualShock4.LeftMotor
		{
			get
			{
				return vibrationMotors[0].IzilEZFnKKPoEpcKyoPmGolsUlOt;
			}
			set
			{
				vibrationMotors[0].IzilEZFnKKPoEpcKyoPmGolsUlOt = value;
			}
		}

		float IDriver_DualShock4.RightMotor
		{
			get
			{
				return vibrationMotors[1].IzilEZFnKKPoEpcKyoPmGolsUlOt;
			}
			set
			{
				vibrationMotors[1].IzilEZFnKKPoEpcKyoPmGolsUlOt = value;
			}
		}

		float IDriver_DualShock4.LightColorR
		{
			get
			{
				return lights[0].mQPaVmdRqYozYExrtihYfahdYhPF;
			}
			set
			{
				lights[0].mQPaVmdRqYozYExrtihYfahdYhPF = value;
			}
		}

		float IDriver_DualShock4.LightColorG
		{
			get
			{
				return lights[0].ffwrCveDusqznMzxjnoAkStTGZyeA;
			}
			set
			{
				lights[0].ffwrCveDusqznMzxjnoAkStTGZyeA = value;
			}
		}

		float IDriver_DualShock4.LightColorB
		{
			get
			{
				return lights[0].DnwgdpganiBQCDljGtGXWAzaoHBmB;
			}
			set
			{
				lights[0].DnwgdpganiBQCDljGtGXWAzaoHBmB = value;
			}
		}

		float IDriver_DualShock4.LightFlashOnDuration
		{
			get
			{
				return (int)qZPmEtzCikjqwBaWsLohpGDQPICFb;
			}
			set
			{
				qZPmEtzCikjqwBaWsLohpGDQPICFb = (byte)MathTools.Clamp(MathTools.Clamp(value, 0f, 2.5f) * 100f, 0f, 255f);
				xRzlBatrDYNiFOBJwfZjbOvzzpKoA();
				if (qZPmEtzCikjqwBaWsLohpGDQPICFb == 0 && SkrTIqBpbDcMuEtjbvVEWajshasO == 0)
				{
					IGYKiMXPlotzrqUKIiXdiHptPGYl = true;
				}
			}
		}

		float IDriver_DualShock4.LightFlashOffDuration
		{
			get
			{
				return (int)SkrTIqBpbDcMuEtjbvVEWajshasO;
			}
			set
			{
				SkrTIqBpbDcMuEtjbvVEWajshasO = (byte)MathTools.Clamp(MathTools.Clamp(value, 0f, 2.5f) * 100f, 0f, 255f);
				xRzlBatrDYNiFOBJwfZjbOvzzpKoA();
				if (qZPmEtzCikjqwBaWsLohpGDQPICFb == 0 && SkrTIqBpbDcMuEtjbvVEWajshasO == 0)
				{
					IGYKiMXPlotzrqUKIiXdiHptPGYl = true;
				}
			}
		}

		Vector3 IDriver_DualShock4.AccelerometerValue => CMoPDShknnoxovqjXFKoTdPrFDSB(accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP);

		Vector3 IDriver_DualShock4.AccelerometerValueRaw => new Vector3(accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[0], accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[1], accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[2]);

		Vector3 IDriver_DualShock4.GyroscopeValue => ODAwPGABeAmegaErhPqISHIVIeRkA(gyroscopes[0].garhibHNwyDACbuxuiOfayIWtbZD);

		Vector3 IDriver_DualShock4.GyroscopeValueRaw => new Vector3(gyroscopes[0].OrthfcEpPRtmJfLlFdtCctIoezeQ[0], gyroscopes[0].OrthfcEpPRtmJfLlFdtCctIoezeQ[1], gyroscopes[0].OrthfcEpPRtmJfLlFdtCctIoezeQ[2]);

		Vector3 IDriver_DualShock4.LastGyroscopeValue
		{
			get
			{
				Vector3 vector = new Vector3(gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[0], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[1], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[2]);
				return jVWOxJkQEbLyVoEDJfdSBDuGKcYN(vector, fTRRHETsoFNlHFGxAKnHrKkVOibH);
			}
		}

		Vector3 IDriver_DualShock4.LastGyroscopeValueRaw => new Vector3(gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[0], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[1], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[2]);

		Quaternion IDriver_DualShock4.Orientation => LNnWXKmibVqzopQIjKkHZglWCtwF;

		int IDriver_DualShock4.MaxTouches => 2;

		ushort IHIDControllerExtension.vendorId => WzfPXTzpCeLnTZBpgJbYyWuPisrq.vendorId;

		ushort IHIDControllerExtension.productId => WzfPXTzpCeLnTZBpgJbYyWuPisrq.productId;

		string IHIDControllerExtension.productName => WzfPXTzpCeLnTZBpgJbYyWuPisrq.productName;

		string IHIDControllerExtension.manufacturer => WzfPXTzpCeLnTZBpgJbYyWuPisrq.manufacturer;

		ushort IHIDControllerExtension.usagePage => WzfPXTzpCeLnTZBpgJbYyWuPisrq.usagePage;

		ushort IHIDControllerExtension.usage => WzfPXTzpCeLnTZBpgJbYyWuPisrq.usage;

		public void ResetOrientation()
		{
			LNnWXKmibVqzopQIjKkHZglWCtwF = Quaternion.identity;
			TTBnJIJnDdpPYTBNDQqDNnKuUdaS = false;
		}

		void IDriver_DualShock4.ResetOrientation()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ResetOrientation
			this.ResetOrientation();
		}

		public int GetTouchCount()
		{
			int num = 0;
			for (int i = 0; i < 2; i++)
			{
				if (touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].isTouching)
				{
					num++;
				}
			}
			return num;
		}

		int IDriver_DualShock4.GetTouchCount()
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchCount
			return this.GetTouchCount();
		}

		public bool IsTouchingAtIndex(int index)
		{
			if (index < 0 || index >= 2)
			{
				return false;
			}
			return touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].isTouching;
		}

		bool IDriver_DualShock4.IsTouchingAtIndex(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IsTouchingAtIndex
			return this.IsTouchingAtIndex(index);
		}

		public bool IsTouchingAtTouchId(int touchId)
		{
			return touchpads[0].VRQDYrjowDqtUNGMQEXSGOOHLRDj(touchId);
		}

		bool IDriver_DualShock4.IsTouchingAtTouchId(int touchId)
		{
			//ILSpy generated this explicit interface implementation from .override directive in IsTouchingAtTouchId
			return this.IsTouchingAtTouchId(touchId);
		}

		public int GetTouchIdAtIndex(int index)
		{
			if (index < 0 || index >= 2)
			{
				return -1;
			}
			return touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].touchId;
		}

		int IDriver_DualShock4.GetTouchIdAtIndex(int index)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchIdAtIndex
			return this.GetTouchIdAtIndex(index);
		}

		public bool GetTouchPositionByIndex(int index, out Vector2 position)
		{
			position = default(Vector2);
			if (index < 0 || index >= 2)
			{
				return false;
			}
			JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] sBWbRIEBtbRxLkclWCpSvIwxSXTqA = touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA;
			if (!sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].isTouching)
			{
				return false;
			}
			position.x = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].positionX;
			position.y = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].positionY;
			return true;
		}

		bool IDriver_DualShock4.GetTouchPositionByIndex(int index, out Vector2 position)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionByIndex
			return this.GetTouchPositionByIndex(index, out position);
		}

		public bool GetTouchPositionByTouchId(int touchId, out Vector2 position)
		{
			position = default(Vector2);
			if (!touchpads[0].VRQDYrjowDqtUNGMQEXSGOOHLRDj(touchId))
			{
				return false;
			}
			JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] sBWbRIEBtbRxLkclWCpSvIwxSXTqA = touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA;
			for (int i = 0; i < sBWbRIEBtbRxLkclWCpSvIwxSXTqA.Length; i++)
			{
				if (sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].isTouching)
				{
					position.x = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].positionX;
					position.y = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].positionY;
				}
			}
			return true;
		}

		bool IDriver_DualShock4.GetTouchPositionByTouchId(int touchId, out Vector2 position)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionByTouchId
			return this.GetTouchPositionByTouchId(touchId, out position);
		}

		public bool GetTouchPositionAbsoluteByIndex(int index, out int positionX, out int positionY)
		{
			positionX = 0;
			positionY = 0;
			if (index < 0 || index >= 2)
			{
				return false;
			}
			JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] sBWbRIEBtbRxLkclWCpSvIwxSXTqA = touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA;
			if (!sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].isTouching)
			{
				return false;
			}
			positionX = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].positionAbsX;
			positionY = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[index].positionAbsY;
			return true;
		}

		bool IDriver_DualShock4.GetTouchPositionAbsoluteByIndex(int index, out int positionX, out int positionY)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionAbsoluteByIndex
			return this.GetTouchPositionAbsoluteByIndex(index, out positionX, out positionY);
		}

		public bool GetTouchPositionAbsoluteByTouchId(int touchId, out int positionX, out int positionY)
		{
			positionX = 0;
			positionY = 0;
			if (!touchpads[0].VRQDYrjowDqtUNGMQEXSGOOHLRDj(touchId))
			{
				return false;
			}
			JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] sBWbRIEBtbRxLkclWCpSvIwxSXTqA = touchpads[0].SBWbRIEBtbRxLkclWCpSvIwxSXTqA;
			for (int i = 0; i < sBWbRIEBtbRxLkclWCpSvIwxSXTqA.Length; i++)
			{
				if (sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].isTouching)
				{
					positionX = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].positionAbsX;
					positionY = sBWbRIEBtbRxLkclWCpSvIwxSXTqA[i].positionAbsY;
				}
			}
			return true;
		}

		bool IDriver_DualShock4.GetTouchPositionAbsoluteByTouchId(int touchId, out int positionX, out int positionY)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetTouchPositionAbsoluteByTouchId
			return this.GetTouchPositionAbsoluteByTouchId(touchId, out positionX, out positionY);
		}

		public void StopLightFlash()
		{
			qZPmEtzCikjqwBaWsLohpGDQPICFb = 0;
			SkrTIqBpbDcMuEtjbvVEWajshasO = 0;
			OIrczxarJJHYlUFTwUlKAMiqHvsc = true;
			IGYKiMXPlotzrqUKIiXdiHptPGYl = true;
			rbIFGRQgfabTGYtaykSwwagYUQCe = true;
		}

		void IDriver_DualShock4.StopLightFlash()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopLightFlash
			this.StopLightFlash();
		}

		public void StopVibration()
		{
			int num = base.Rewired_002EHID_002EDrivers_002EIControllerDriver_002EVibrationMotorCount;
			for (int i = 0; i < num; i++)
			{
				vibrationMotors[i].ZcjoZwbIDbbFlaWQFjFKWrESBVuu = 0;
			}
		}

		void IDriver_DualShock4.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		public DualShock4Driver(InitArgs P_0)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initArgs");
			}
			gvodinVafsknMqadZCkzYQfILURO = P_0.hidDevice;
			WzfPXTzpCeLnTZBpgJbYyWuPisrq = gvodinVafsknMqadZCkzYQfILURO.properties;
			oCXWJdcQfhfNsJouQorPDQlvnYfg = P_0.hatZeroValue;
			LBYTuqvFgwgdlEBoHkgheAxewIhHb = P_0.hatSpan;
			JwpkwHojAGPSnMZIVpCkWxcISfLt = P_0.connectionType;
			fEGigHhstdROEnASIpCkcwZoSxnDA = JwpkwHojAGPSnMZIVpCkWxcISfLt == YDvFqJokstcNyQQOYydcruGncmeb.Bluetooth;
			if (fEGigHhstdROEnASIpCkcwZoSxnDA)
			{
				WzfPXTzpCeLnTZBpgJbYyWuPisrq.maxOutputReportLength = 78;
			}
			if (WzfPXTzpCeLnTZBpgJbYyWuPisrq.maxOutputReportLength < 23)
			{
				WzfPXTzpCeLnTZBpgJbYyWuPisrq.maxOutputReportLength = 23;
			}
			fjSOMESmrqGKEQdotDVyULRIUEJF = new NativeBuffer(64);
			TpdrVjRvVLzzaIMSRStdwQcbdETG = new NativeBuffer(WzfPXTzpCeLnTZBpgJbYyWuPisrq.maxOutputReportLength);
			NwUMMziSKoMRbNAYpDPxqwtUdeFd = new aMZqdyjJERTAUbjSZWzzHWVxTEnF(TpdrVjRvVLzzaIMSRStdwQcbdETG.Pointer, TpdrVjRvVLzzaIMSRStdwQcbdETG.Length, WzfPXTzpCeLnTZBpgJbYyWuPisrq.maxOutputReportLength);
			lights = new dRxYZKovikdvFiOlZLmFiKpaWUdu[1]
			{
				new dRxYZKovikdvFiOlZLmFiKpaWUdu(11, 24, 28)
			};
			lights[0].KhdFqLHnkQpyjokVAndSadBMcFSRA += nJRwrNGaMIKpYOjqwMFrRuismJVk;
			jnqApNimEdlaHCNMGLbmcsyJekzDc = true;
			vibrationMotors = new iwnZquMFWHwhZjzckYkHRPdcqkIc[2]
			{
				new iwnZquMFWHwhZjzckYkHRPdcqkIc(0, 255),
				new iwnZquMFWHwhZjzckYkHRPdcqkIc(0, 255)
			};
			vibrationMotors[0].JbLUwmUKfnDCvYnjJuByJLLCsxze += oSbUccFBrWLrFVaGxulcNsRLNPeb;
			vibrationMotors[1].JbLUwmUKfnDCvYnjJuByJLLCsxze += oSbUccFBrWLrFVaGxulcNsRLNPeb;
			if (gvodinVafsknMqadZCkzYQfILURO.GetHidFeatureData(2, 37, 1000, 3) == null)
			{
				throw new Exception();
			}
			ycsskyFLipOSQyPjLxbdKCfSUcBc = true;
			if (fEGigHhstdROEnASIpCkcwZoSxnDA)
			{
				KyJChzuIjPtnZxOpGNoEEBRcpfTU = true;
				NwUMMziSKoMRbNAYpDPxqwtUdeFd.jWyxbwpyOBwigomFzcjATaXrYEzP |= cearvUhOhIGFiMrovHXSAoxpvgdP.WriteDirect;
				KyJChzuIjPtnZxOpGNoEEBRcpfTU = SfaJGYdrDITnrqQHRlQwYOYHIilib(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
				if (!KyJChzuIjPtnZxOpGNoEEBRcpfTU)
				{
					NwUMMziSKoMRbNAYpDPxqwtUdeFd.jWyxbwpyOBwigomFzcjATaXrYEzP &= ~cearvUhOhIGFiMrovHXSAoxpvgdP.WriteDirect;
				}
			}
			else
			{
				KyJChzuIjPtnZxOpGNoEEBRcpfTU = SfaJGYdrDITnrqQHRlQwYOYHIilib(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
			}
			if (!KyJChzuIjPtnZxOpGNoEEBRcpfTU)
			{
				throw new Exception();
			}
			qnhwOCMoBiBUtGVivOrDEgVYqJsx = 1;
			wokLJMSWhfCVhAaltefICpPzKFojA = 0;
			if (fEGigHhstdROEnASIpCkcwZoSxnDA && KyJChzuIjPtnZxOpGNoEEBRcpfTU)
			{
				qnhwOCMoBiBUtGVivOrDEgVYqJsx = 17;
				wokLJMSWhfCVhAaltefICpPzKFojA = 2;
			}
			gWmJGSqAOvFbIccEroFjNYFVjyZR = 5 + wokLJMSWhfCVhAaltefICpPzKFojA;
			ihabjTsEALCsBkVpaJlksGrfvYXKA = 6 + wokLJMSWhfCVhAaltefICpPzKFojA;
			eBovnLlaCqRjtiftqZCNTFREFHYQ = 7 + wokLJMSWhfCVhAaltefICpPzKFojA;
			buttons = new RyDagBEfRFfkQlRDvQAHmQXROhrtA[14];
			for (int i = 0; i < 14; i++)
			{
				buttons[i] = new RyDagBEfRFfkQlRDvQAHmQXROhrtA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 9,
					usage = (ushort)i
				});
			}
			axes = new eTBgDLAnVcEreaYiOpvDFMeVVuExA[6]
			{
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 48,
					dataIndex = 1 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 49,
					dataIndex = 2 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 50,
					dataIndex = 3 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 53,
					dataIndex = 4 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 0,
					units = 0u,
					unitsExp = 0u
				}, false, 127),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 51,
					dataIndex = 8 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 315,
					units = 0u,
					unitsExp = 0u
				}, false, 0),
				new eTBgDLAnVcEreaYiOpvDFMeVVuExA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 52,
					dataIndex = 9 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 8,
					logicalMin = 0,
					logicalMax = 255,
					physicalMin = 0,
					physicalMax = 315,
					units = 0u,
					unitsExp = 0u
				}, false, 0)
			};
			hats = new AlQQSkDXAKgzPiahlYVsHmMBdhGkA[1]
			{
				new AlQQSkDXAKgzPiahlYVsHmMBdhGkA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					usage = 57,
					dataIndex = 5 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 4,
					logicalMin = 0,
					logicalMax = 7,
					physicalMin = 0,
					physicalMax = 315,
					units = 20u,
					unitsExp = 0u
				}, HIIlGhHwRbyJZeUDDBcrEzFJWxKe)
			};
			accelerometers = new fcgInupHfYVLlnSfBDoHscyUgTsEA[1]
			{
				new fcgInupHfYVLlnSfBDoHscyUgTsEA(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					dataIndex = 19 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 48
				}, 3, YQctCtILLjXfUPvfKyBhLwwynJxi)
			};
			gyroscopes = new zeduVYzSnJpVQGxDoGRFMdphEaCi[1]
			{
				new zeduVYzSnJpVQGxDoGRFMdphEaCi(P_0.updateLoopSetting, qnhwOCMoBiBUtGVivOrDEgVYqJsx, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					dataIndex = 13 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 48
				}, 3, 60, DFQtdREmNPFyLKkmKcgilvDcinhX, yBXMFaadOIBckaPgGuLkMyvUlOrq)
			};
			touchpads = new JeEihaxNGDZUEopEZTyRorKoTSAm[1]
			{
				new JeEihaxNGDZUEopEZTyRorKoTSAm(qnhwOCMoBiBUtGVivOrDEgVYqJsx, new JeEihaxNGDZUEopEZTyRorKoTSAm.TouchpadInfo(2, 0, 1912, 0, 941, false, true), new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 1,
					dataIndex = 35 + wokLJMSWhfCVhAaltefICpPzKFojA,
					bitSize = 48
				}, 60, SGwkIddZTVUptueyjAnozfBSTLwk)
			};
			cwkuhuNIDownDHstQHgRrwogbRNk = ReInput.realTime;
		}

		public override void Update(UpdateLoopType updateLoop)
		{
			dbKzxWVsBZqXyPIjeVefbjLhJjQo();
			BUOyUJocuyrZFMeeMymgsYawwuRH(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous);
		}

		public override bool ParseInputReport(IntPtr inputReportPtr, int inputReportLength, double timestamp)
		{
			if (inputReportPtr == IntPtr.Zero)
			{
				return false;
			}
			if (inputReportLength < fjSOMESmrqGKEQdotDVyULRIUEJF.Length)
			{
				return false;
			}
			zloAXNFvhlMmrFWNFhrTFhXBBflzB = (float)(timestamp - cwkuhuNIDownDHstQHgRrwogbRNk);
			cwkuhuNIDownDHstQHgRrwogbRNk = timestamp;
			fjSOMESmrqGKEQdotDVyULRIUEJF.Write(inputReportPtr, inputReportLength, fjSOMESmrqGKEQdotDVyULRIUEJF.Length);
			CwWeMpJwXvWdCPqhpjSFsZWaIjOTA(fjSOMESmrqGKEQdotDVyULRIUEJF);
			EymTmBLFxSXOEnipfNipMeYAkBKT(fjSOMESmrqGKEQdotDVyULRIUEJF, timestamp);
			LDJGvqLnFydDhJMnXduxzIERUQI[] array = axes;
			yWAhTOJGDSPXSfLuBeNjAPerRAsWb(array, fjSOMESmrqGKEQdotDVyULRIUEJF, timestamp);
			array = hats;
			yWAhTOJGDSPXSfLuBeNjAPerRAsWb(array, fjSOMESmrqGKEQdotDVyULRIUEJF, timestamp);
			array = accelerometers;
			yWAhTOJGDSPXSfLuBeNjAPerRAsWb(array, fjSOMESmrqGKEQdotDVyULRIUEJF, timestamp);
			array = gyroscopes;
			yWAhTOJGDSPXSfLuBeNjAPerRAsWb(array, fjSOMESmrqGKEQdotDVyULRIUEJF, timestamp);
			array = touchpads;
			yWAhTOJGDSPXSfLuBeNjAPerRAsWb(array, fjSOMESmrqGKEQdotDVyULRIUEJF, timestamp);
			byte num = fjSOMESmrqGKEQdotDVyULRIUEJF[30 + wokLJMSWhfCVhAaltefICpPzKFojA];
			byte b = (byte)(num & 0xF);
			if ((num & 0x10) != 0)
			{
				if (b <= 10)
				{
					haqhsRtndWAVOjlDBIrdAonKodimA = MathTools.Clamp(b * 10 + 5, 0, 100);
					RbPbOXdPkErATTcWkYqVWRmOLGhQA = RQNHrwuFSvzqqBVknotRibPNUUAq.Charging;
				}
				else
				{
					switch (b)
					{
					case 11:
						haqhsRtndWAVOjlDBIrdAonKodimA = 100;
						RbPbOXdPkErATTcWkYqVWRmOLGhQA = RQNHrwuFSvzqqBVknotRibPNUUAq.Full;
						break;
					case 14:
						haqhsRtndWAVOjlDBIrdAonKodimA = 0;
						RbPbOXdPkErATTcWkYqVWRmOLGhQA = RQNHrwuFSvzqqBVknotRibPNUUAq.Charging;
						break;
					default:
						haqhsRtndWAVOjlDBIrdAonKodimA = 0;
						RbPbOXdPkErATTcWkYqVWRmOLGhQA = RQNHrwuFSvzqqBVknotRibPNUUAq.Unknown;
						break;
					}
				}
			}
			else
			{
				switch (MathTools.Clamp((int)b, 0, 8))
				{
				case 0:
					haqhsRtndWAVOjlDBIrdAonKodimA = 5;
					break;
				case 1:
					haqhsRtndWAVOjlDBIrdAonKodimA = 20;
					break;
				case 2:
					haqhsRtndWAVOjlDBIrdAonKodimA = 30;
					break;
				case 3:
					haqhsRtndWAVOjlDBIrdAonKodimA = 45;
					break;
				case 4:
					haqhsRtndWAVOjlDBIrdAonKodimA = 55;
					break;
				case 5:
					haqhsRtndWAVOjlDBIrdAonKodimA = 70;
					break;
				case 6:
					haqhsRtndWAVOjlDBIrdAonKodimA = 80;
					break;
				case 7:
					haqhsRtndWAVOjlDBIrdAonKodimA = 95;
					break;
				case 8:
					haqhsRtndWAVOjlDBIrdAonKodimA = 100;
					break;
				}
				RbPbOXdPkErATTcWkYqVWRmOLGhQA = RQNHrwuFSvzqqBVknotRibPNUUAq.Discharging;
			}
			szLLsBraCKQTbFXlJmCCjANyqirg();
			return true;
		}

		public override Controller.Extension CreateControllerExtension()
		{
			return new DualShock4Extension(this);
		}

		private void BUOyUJocuyrZFMeeMymgsYawwuRH(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			if (OIrczxarJJHYlUFTwUlKAMiqHvsc)
			{
				SfaJGYdrDITnrqQHRlQwYOYHIilib(P_0);
				OIrczxarJJHYlUFTwUlKAMiqHvsc = false;
			}
		}

		private bool SfaJGYdrDITnrqQHRlQwYOYHIilib(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			fpzGeuceKntVRWMBkWGKhOmCeFk();
			bool result = SjpiHWxJDanmLcIwebzogeWabqIgb(P_0);
			if (IGYKiMXPlotzrqUKIiXdiHptPGYl)
			{
				result = SjpiHWxJDanmLcIwebzogeWabqIgb(P_0);
				IGYKiMXPlotzrqUKIiXdiHptPGYl = false;
			}
			return result;
		}

		private unsafe void fpzGeuceKntVRWMBkWGKhOmCeFk()
		{
			byte b = 0;
			b |= 1;
			ArVDdgjgdnAMicpkknkQfFNuVOwC = false;
			b |= 2;
			jnqApNimEdlaHCNMGLbmcsyJekzDc = false;
			b |= 4;
			rbIFGRQgfabTGYtaykSwwagYUQCe = false;
			byte b2 = 128;
			if (fEGigHhstdROEnASIpCkcwZoSxnDA)
			{
				b2 |= 0x40;
			}
			if (ycsskyFLipOSQyPjLxbdKCfSUcBc)
			{
				b2 |= 4;
				ycsskyFLipOSQyPjLxbdKCfSUcBc = false;
			}
			if (fEGigHhstdROEnASIpCkcwZoSxnDA && KyJChzuIjPtnZxOpGNoEEBRcpfTU)
			{
				TpdrVjRvVLzzaIMSRStdwQcbdETG[0] = 17;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[1] = b2;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[2] = 0;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[3] = b;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[4] = 0;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[5] = 0;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[6] = (byte)vibrationMotors[1].ZcjoZwbIDbbFlaWQFjFKWrESBVuu;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[7] = (byte)vibrationMotors[0].ZcjoZwbIDbbFlaWQFjFKWrESBVuu;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[8] = lights[0].dzlPvBalHRSfegtkxkAECZRZUliD;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[9] = lights[0].pUNnpXbqlHMdMbFBrwAbNRJiZxKR;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[10] = lights[0].ZfhhhzCONloJjmcuIfFhItNGYTyBc;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[11] = qZPmEtzCikjqwBaWsLohpGDQPICFb;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[12] = SkrTIqBpbDcMuEtjbvVEWajshasO;
				int cxyjjQvHRbbDygUbIyJDqcXAgiJJA = NwUMMziSKoMRbNAYpDPxqwtUdeFd.CxyjjQvHRbbDygUbIyJDqcXAgiJJA;
				uint bytes = pcqybQuDduhHcAOKcQglFCHbFBAhB.vhlEPaCrKXYPMYciQjXVaaNONLhwA((byte*)(void*)TpdrVjRvVLzzaIMSRStdwQcbdETG.Pointer, cxyjjQvHRbbDygUbIyJDqcXAgiJJA - 4, 162u);
				TpdrVjRvVLzzaIMSRStdwQcbdETG.Write(bytes, cxyjjQvHRbbDygUbIyJDqcXAgiJJA - 4);
			}
			else
			{
				TpdrVjRvVLzzaIMSRStdwQcbdETG[0] = 5;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[1] = b;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[2] = 0;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[4] = (byte)vibrationMotors[1].ZcjoZwbIDbbFlaWQFjFKWrESBVuu;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[5] = (byte)vibrationMotors[0].ZcjoZwbIDbbFlaWQFjFKWrESBVuu;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[6] = lights[0].dzlPvBalHRSfegtkxkAECZRZUliD;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[7] = lights[0].pUNnpXbqlHMdMbFBrwAbNRJiZxKR;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[8] = lights[0].ZfhhhzCONloJjmcuIfFhItNGYTyBc;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[9] = qZPmEtzCikjqwBaWsLohpGDQPICFb;
				TpdrVjRvVLzzaIMSRStdwQcbdETG[10] = SkrTIqBpbDcMuEtjbvVEWajshasO;
			}
		}

		private bool SjpiHWxJDanmLcIwebzogeWabqIgb(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			szNVaGrKkRdHjVhaXTMTuoXFttiM = ReInput.realTime + 4.0;
			switch (P_0)
			{
			case NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous:
				return gvodinVafsknMqadZCkzYQfILURO.WriteSync(NwUMMziSKoMRbNAYpDPxqwtUdeFd, 0);
			case NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous:
				gvodinVafsknMqadZCkzYQfILURO.WriteAsync(NwUMMziSKoMRbNAYpDPxqwtUdeFd, 1000);
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		private void EymTmBLFxSXOEnipfNipMeYAkBKT(NativeBuffer P_0, double P_1)
		{
			byte b = P_0[gWmJGSqAOvFbIccEroFjNYFVjyZR];
			buttons[0].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x10) != 0, P_1);
			buttons[1].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x20) != 0, P_1);
			buttons[2].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x40) != 0, P_1);
			buttons[3].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x80) != 0, P_1);
			b = P_0[ihabjTsEALCsBkVpaJlksGrfvYXKA];
			buttons[4].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 1) != 0, P_1);
			buttons[5].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 2) != 0, P_1);
			buttons[6].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 4) != 0, P_1);
			buttons[7].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 8) != 0, P_1);
			buttons[8].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x10) != 0, P_1);
			buttons[9].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x20) != 0, P_1);
			buttons[10].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x40) != 0, P_1);
			buttons[11].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 0x80) != 0, P_1);
			b = P_0[eBovnLlaCqRjtiftqZCNTFREFHYQ];
			buttons[12].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 1) != 0, P_1);
			buttons[13].RWoHtlZxvbZGShUfgFhbuBHdgLlD((b & 2) != 0, P_1);
		}

		private void yWAhTOJGDSPXSfLuBeNjAPerRAsWb(LDJGvqLnFydDhJMnXduxzIERUQI[] P_0, NativeBuffer P_1, double P_2)
		{
			for (int i = 0; i < P_0.Length; i++)
			{
				P_0[i].asArJiunXbfpvgEDUosbEuyCYgWWA(P_1, P_2);
			}
		}

		private void dbKzxWVsBZqXyPIjeVefbjLhJjQo()
		{
			if (isVibrating && ReInput.realTime >= szNVaGrKkRdHjVhaXTMTuoXFttiM)
			{
				OIrczxarJJHYlUFTwUlKAMiqHvsc = true;
				ArVDdgjgdnAMicpkknkQfFNuVOwC = true;
			}
		}

		private void CwWeMpJwXvWdCPqhpjSFsZWaIjOTA(NativeBuffer P_0)
		{
			if (KyJChzuIjPtnZxOpGNoEEBRcpfTU)
			{
				ushort num = fjSOMESmrqGKEQdotDVyULRIUEJF.ReadUShort(10 + wokLJMSWhfCVhAaltefICpPzKFojA);
				float num3;
				if (num != utdhnlEcaFhkZcSkjMvvPxmtrkyOB)
				{
					int num2 = ((num >= utdhnlEcaFhkZcSkjMvvPxmtrkyOB) ? (num - utdhnlEcaFhkZcSkjMvvPxmtrkyOB) : (num + 65535 - utdhnlEcaFhkZcSkjMvvPxmtrkyOB));
					num3 = (float)num2 / 187500f;
				}
				else
				{
					int num2 = 0;
					num3 = 0f;
				}
				utdhnlEcaFhkZcSkjMvvPxmtrkyOB = num;
				fTRRHETsoFNlHFGxAKnHrKkVOibH = num3;
			}
		}

		private void szLLsBraCKQTbFXlJmCCjANyqirg()
		{
			if (KyJChzuIjPtnZxOpGNoEEBRcpfTU)
			{
				_ = fTRRHETsoFNlHFGxAKnHrKkVOibH;
				_ = 0f;
				Vector3 vector = jVWOxJkQEbLyVoEDJfdSBDuGKcYN(new Vector3(gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[0], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[1], gyroscopes[0].HViVbfIrDiBECuxWlqkhZscDKxn[2]), fTRRHETsoFNlHFGxAKnHrKkVOibH);
				vrkKjZILPpMoAyESURyEJofQCHwJ(ref vector);
				Vector3 vector2 = new Vector3(accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[0] * -1f, accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[1] * -1f, accelerometers[0].SytbxvDfrRdLDWckugtMRDSBscWP[2] * -1f);
				rhIcKcvyvCLSpNgSsEMRftSStoXX(vector2, vector);
			}
		}

		private static bool vrkKjZILPpMoAyESURyEJofQCHwJ(ref Vector3 P_0)
		{
			if (P_0.magnitude < 0.004f)
			{
				P_0.x = 0f;
				P_0.y = 0f;
				P_0.z = 0f;
				return false;
			}
			return true;
		}

		private void rhIcKcvyvCLSpNgSsEMRftSStoXX(Vector3 P_0, Vector3 P_1)
		{
			Quaternion quaternion = Quaternion.Euler(P_1);
			float sqrMagnitude = P_0.sqrMagnitude;
			if (sqrMagnitude > 16777216f && sqrMagnitude < 268435460f && AMMdwANfPodZuiKrCaGCIcQfzHdqb(P_0, out var vpSznjZXJaSyXmxrUjYlmJyyAlLw2))
			{
				Quaternion a = LNnWXKmibVqzopQIjKkHZglWCtwF * quaternion;
				if (!TTBnJIJnDdpPYTBNDQqDNnKuUdaS)
				{
					TTBnJIJnDdpPYTBNDQqDNnKuUdaS = true;
					kyoiAAxasuXAjPsRjKcxkhnBnPiy = Quaternion.identity * Quaternion.Euler(new Vector3(90f, 0f, 0f));
					iUqgErqYibOuOetcANIyxQAOyAzl = LNnWXKmibVqzopQIjKkHZglWCtwF;
				}
				kyoiAAxasuXAjPsRjKcxkhnBnPiy *= quaternion;
				iUqgErqYibOuOetcANIyxQAOyAzl *= quaternion;
				Quaternion b;
				if ((vpSznjZXJaSyXmxrUjYlmJyyAlLw2 & vpSznjZXJaSyXmxrUjYlmJyyAlLw.XZ) != vpSznjZXJaSyXmxrUjYlmJyyAlLw.None)
				{
					b = iAbyxrdnxVUeuPydElYIoqzLqpPH(P_0, a.eulerAngles.y);
				}
				else if ((vpSznjZXJaSyXmxrUjYlmJyyAlLw2 & vpSznjZXJaSyXmxrUjYlmJyyAlLw.Y) != vpSznjZXJaSyXmxrUjYlmJyyAlLw.None)
				{
					b = sztOwQrvIMRSdIXTFtkzNjHomRIv(P_0);
					Vector3 vector = iUqgErqYibOuOetcANIyxQAOyAzl * Vector3.right;
					float y = 0f - MathTools.SignedAngle(new Vector3(vector.x, 0f, vector.z), Vector3.right, Vector3.up);
					b = Quaternion.Euler(0f, y, 0f) * b;
				}
				else
				{
					b = Quaternion.identity;
				}
				LNnWXKmibVqzopQIjKkHZglWCtwF = Quaternion.Lerp(a, b, 0.01999998f);
			}
			else
			{
				LNnWXKmibVqzopQIjKkHZglWCtwF *= quaternion;
				if (TTBnJIJnDdpPYTBNDQqDNnKuUdaS)
				{
					TTBnJIJnDdpPYTBNDQqDNnKuUdaS = false;
				}
			}
		}

		private Quaternion iAbyxrdnxVUeuPydElYIoqzLqpPH(Vector3 P_0, float P_1 = 0f)
		{
			float num = MathTools.Atan2(P_0.z, P_0.y);
			float num2 = MathTools.Atan2(x: MathTools.Sqrt(MathTools.Pow(P_0.y, 2f) + MathTools.Pow(P_0.z, 2f)), y: P_0.x);
			float x = num * 57.29578f + 180f;
			float z = (0f - num2) * 57.29578f;
			return Quaternion.Euler(x, P_1, z);
		}

		private Quaternion sztOwQrvIMRSdIXTFtkzNjHomRIv(Vector3 P_0, float P_1 = 0f)
		{
			float num = MathTools.Atan2(P_0.z, P_0.y);
			float x = MathTools.Sqrt(MathTools.Pow(P_0.y, 2f) + MathTools.Pow(P_0.z, 2f));
			float num2 = MathTools.Atan2(P_0.x, x);
			float x2 = num * 57.29578f + 180f;
			float z = (0f - num2) * 57.29578f;
			Quaternion quaternion = Quaternion.Euler(0f, 0f, z) * Quaternion.Euler(x2, 0f, 0f);
			if (P_1 != 0f)
			{
				return quaternion * Quaternion.Euler(0f, P_1, 0f);
			}
			return quaternion;
		}

		private bool AMMdwANfPodZuiKrCaGCIcQfzHdqb(Vector3 P_0, out vpSznjZXJaSyXmxrUjYlmJyyAlLw P_1)
		{
			P_0.Normalize();
			P_1 = vpSznjZXJaSyXmxrUjYlmJyyAlLw.None;
			bool result = false;
			if (NsMZqPpUcibReCHAHUORmWzJEbrw(P_0))
			{
				result = true;
				P_1 |= vpSznjZXJaSyXmxrUjYlmJyyAlLw.XZ;
			}
			if (KrlDWYvuPbXMosQyKuTqkvIXvGqF(P_0))
			{
				result = true;
				P_1 |= vpSznjZXJaSyXmxrUjYlmJyyAlLw.Y;
			}
			return result;
		}

		private bool NsMZqPpUcibReCHAHUORmWzJEbrw(Vector3 P_0)
		{
			if (P_0.y > 0f)
			{
				return false;
			}
			if (Vector3.Angle(Vector3.down, P_0) > 45f)
			{
				return false;
			}
			return true;
		}

		private bool KrlDWYvuPbXMosQyKuTqkvIXvGqF(Vector3 P_0)
		{
			if (P_0.z < 0f)
			{
				return false;
			}
			if (Vector3.Angle(new Vector3(0f, 0f, 1f), P_0) > 20f)
			{
				return false;
			}
			return true;
		}

		private Vector3 CMoPDShknnoxovqjXFKoTdPrFDSB(float[] P_0)
		{
			return new Vector3(P_0[0] * 0.00012207031f * -1f, P_0[1] * 0.00012207031f * -1f, P_0[2] * 0.00012207031f);
		}

		private Vector3 ODAwPGABeAmegaErhPqISHIVIeRkA(RingBuffer<zeduVYzSnJpVQGxDoGRFMdphEaCi.hOJhFTpGFkIeuGuGckkEoiyPlXuc> P_0)
		{
			Vector3 result = default(Vector3);
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				zeduVYzSnJpVQGxDoGRFMdphEaCi.hOJhFTpGFkIeuGuGckkEoiyPlXuc hOJhFTpGFkIeuGuGckkEoiyPlXuc = P_0[i];
				result += jVWOxJkQEbLyVoEDJfdSBDuGKcYN(hOJhFTpGFkIeuGuGckkEoiyPlXuc.ZCNwuekgJmAkwEDhmlrFhlleBLIy, hOJhFTpGFkIeuGuGckkEoiyPlXuc.pEjIWtERgNgCAQHSarniWniWPwXdb);
			}
			return result;
		}

		private Vector3 jVWOxJkQEbLyVoEDJfdSBDuGKcYN(Vector3 P_0, float P_1)
		{
			P_0.x *= -1f;
			P_0.y *= -1f;
			return P_0 * 0.06103702f * P_1;
		}

		private int HIIlGhHwRbyJZeUDDBcrEzFJWxKe(int P_0)
		{
			P_0 &= 0xF;
			return P_0;
		}

		private void YQctCtILLjXfUPvfKyBhLwwynJxi(byte[] P_0, float[] P_1)
		{
			P_1[0] = BitConverter.ToInt16(P_0, 0);
			P_1[1] = BitConverter.ToInt16(P_0, 2);
			P_1[2] = BitConverter.ToInt16(P_0, 4);
		}

		private void DFQtdREmNPFyLKkmKcgilvDcinhX(byte[] P_0, float[] P_1)
		{
			P_1[0] = BitConverter.ToInt16(P_0, 0);
			P_1[1] = BitConverter.ToInt16(P_0, 2);
			P_1[2] = BitConverter.ToInt16(P_0, 4);
		}

		private float yBXMFaadOIBckaPgGuLkMyvUlOrq()
		{
			return fTRRHETsoFNlHFGxAKnHrKkVOibH;
		}

		private void SGwkIddZTVUptueyjAnozfBSTLwk(NativeBuffer P_0, JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData[] P_1)
		{
			int num = 35 + wokLJMSWhfCVhAaltefICpPzKFojA;
			int positionRawX = P_0[1 + num] + (P_0[2 + num] & 0xF) * 255;
			int positionRawY = ((P_0[2 + num] & 0xF0) >> 4) + P_0[3 + num] * 16;
			int positionRawX2 = P_0[5 + num] + (P_0[6 + num] & 0xF) * 255;
			int positionRawY2 = ((P_0[6 + num] & 0xF0) >> 4) + P_0[7 + num] * 16;
			byte b = P_0[num];
			bool flag = b < 128;
			byte num2 = P_0[num + 4];
			bool flag2 = num2 < 128;
			int num3 = b & 0x7F;
			int num4 = num2 & 0x7F;
			P_1[0].isTouching = flag;
			P_1[0].touchId = LoEbHUfeYoYIvEBpVsRGtcQICqYCA(0, flag, num3);
			P_1[0].positionRawX = positionRawX;
			P_1[0].positionRawY = positionRawY;
			P_1[1].isTouching = flag2;
			P_1[1].touchId = LoEbHUfeYoYIvEBpVsRGtcQICqYCA(1, flag2, num4);
			P_1[1].positionRawX = positionRawX2;
			P_1[1].positionRawY = positionRawY2;
		}

		private int LoEbHUfeYoYIvEBpVsRGtcQICqYCA(int P_0, bool P_1, int P_2)
		{
			if (!P_1)
			{
				UcPIHWjTLrXINIuEYLOPaszezousB[P_0] = -1;
				DVlcyHQffOOHieKbmEWxBYrsZSaMA[P_0] = P_2;
				return -1;
			}
			if (P_2 != DVlcyHQffOOHieKbmEWxBYrsZSaMA[P_0])
			{
				int num = xwenFFYbPOjKeuDcStBrgRYdKmXh;
				if (xwenFFYbPOjKeuDcStBrgRYdKmXh == 2147483647)
				{
					xwenFFYbPOjKeuDcStBrgRYdKmXh = 0;
				}
				else
				{
					xwenFFYbPOjKeuDcStBrgRYdKmXh++;
				}
				DVlcyHQffOOHieKbmEWxBYrsZSaMA[P_0] = P_2;
				UcPIHWjTLrXINIuEYLOPaszezousB[P_0] = num;
				return num;
			}
			return UcPIHWjTLrXINIuEYLOPaszezousB[P_0];
		}

		private void nJRwrNGaMIKpYOjqwMFrRuismJVk()
		{
			jnqApNimEdlaHCNMGLbmcsyJekzDc = true;
			UJgSKzmRyUsXOHMLjlnwKoNHRNVC();
		}

		private void xRzlBatrDYNiFOBJwfZjbOvzzpKoA()
		{
			rbIFGRQgfabTGYtaykSwwagYUQCe = true;
			UJgSKzmRyUsXOHMLjlnwKoNHRNVC();
		}

		private void oSbUccFBrWLrFVaGxulcNsRLNPeb()
		{
			ArVDdgjgdnAMicpkknkQfFNuVOwC = true;
			UJgSKzmRyUsXOHMLjlnwKoNHRNVC();
		}

		private void UJgSKzmRyUsXOHMLjlnwKoNHRNVC()
		{
			OIrczxarJJHYlUFTwUlKAMiqHvsc = true;
		}

		~DualShock4Driver()
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
				StopVibration();
				BUOyUJocuyrZFMeeMymgsYawwuRH(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
				if (fjSOMESmrqGKEQdotDVyULRIUEJF != null)
				{
					fjSOMESmrqGKEQdotDVyULRIUEJF.Dispose();
				}
				if (TpdrVjRvVLzzaIMSRStdwQcbdETG != null)
				{
					TpdrVjRvVLzzaIMSRStdwQcbdETG.Dispose();
				}
			}
		}

		public static bool Matches(int vid, int pid)
		{
			for (int i = 0; i < Consts.pidVids_sony_dualShock4.Count; i++)
			{
				if (Consts.pidVids_sony_dualShock4[i].vendorId == vid && Consts.pidVids_sony_dualShock4[i].productId == pid)
				{
					return true;
				}
			}
			return false;
		}
	}
}
