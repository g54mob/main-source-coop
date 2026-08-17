using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rewired.ControllerExtensions;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

namespace Rewired.HID.Drivers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal abstract class NintendoSwitchGamepadDriver : HIDDeviceDriver, IDriver_NintendoSwitchController, IControllerDriver, IHIDControllerExtension, IDisposable
	{
		private enum KgnAJVvECgDMOyRGJrStJEOjaPCh
		{
			GetUsbStatus = 1
		}

		private delegate bool jTgvXqDErhbYeYuUxwkmJsMMXLvg(uint responseTimeoutMs);

		protected enum EpsjEqBsgEWVUiCpmfVMyaeRIRPM
		{
			ProController = 0,
			JoyConLeft = 1,
			JoyConRight = 2
		}

		protected class stfckzRjtXZbwudcgoqwfYMkbPoF
		{
			private iwnZquMFWHwhZjzckYkHRPdcqkIc NBFuwOwAnvKQDqXKJmfmgEZwAEBP;

			private mIJZURMoWXbiTFibclZPkOZWkoOwA iCaAXETSiEHdtMZXbVPyqPHAXBBI;

			private float gdVzIiDslmVVHVbNKSEHhdplsweD;

			private double saQhqJpAXspRHdtNUNuAgSFZAanc;

			public mIJZURMoWXbiTFibclZPkOZWkoOwA NozevYZCCqkeWcqpXxyeVoGLQPJN => iCaAXETSiEHdtMZXbVPyqPHAXBBI;

			public bool ZLcESAzWTHKLOQqWVZJlOeBVkSYK => NBFuwOwAnvKQDqXKJmfmgEZwAEBP.ZcjoZwbIDbbFlaWQFjFKWrESBVuu > 0;

			public stfckzRjtXZbwudcgoqwfYMkbPoF(iwnZquMFWHwhZjzckYkHRPdcqkIc P_0)
			{
				NBFuwOwAnvKQDqXKJmfmgEZwAEBP = P_0;
				ipfFLsbkBonBmcWkRFtUDOjyzb();
			}

			public void rmcAShGojFMvKmyRoFqKJSCWaXVN(float P_0, float P_1, float P_2, float P_3, float P_4)
			{
				if (P_4 < 0f)
				{
					P_4 = 0f;
				}
				gdVzIiDslmVVHVbNKSEHhdplsweD = P_4;
				iCaAXETSiEHdtMZXbVPyqPHAXBBI.AiWrSadsDlTEuCDKSkcLKtqiQEVQ = MathTools.Clamp01(P_0);
				iCaAXETSiEHdtMZXbVPyqPHAXBBI.hutIcjbGXpDkDBuYBjvlQZNlHMAQA = MathTools.Clamp(P_1, 40.875885f, 626.28613f);
				iCaAXETSiEHdtMZXbVPyqPHAXBBI.xCtDiCIQLqThxFjYNgEMjeBESiLdA = MathTools.Clamp01(P_2);
				iCaAXETSiEHdtMZXbVPyqPHAXBBI.EEjLhwaGkFzDzTomkpgHrcsKOzYB = MathTools.Clamp(P_3, 81.75177f, 1252.5723f);
				NBFuwOwAnvKQDqXKJmfmgEZwAEBP.IzilEZFnKKPoEpcKyoPmGolsUlOt = Math.Max(iCaAXETSiEHdtMZXbVPyqPHAXBBI.AiWrSadsDlTEuCDKSkcLKtqiQEVQ, iCaAXETSiEHdtMZXbVPyqPHAXBBI.xCtDiCIQLqThxFjYNgEMjeBESiLdA);
				saQhqJpAXspRHdtNUNuAgSFZAanc = ReInput.realTime;
			}

			public bool bugUdxGmbAfafpHCqzpvuceHeKYGA(double P_0)
			{
				if ((iCaAXETSiEHdtMZXbVPyqPHAXBBI.AiWrSadsDlTEuCDKSkcLKtqiQEVQ > 0f || iCaAXETSiEHdtMZXbVPyqPHAXBBI.xCtDiCIQLqThxFjYNgEMjeBESiLdA > 0f) && gdVzIiDslmVVHVbNKSEHhdplsweD > 0f && P_0 >= saQhqJpAXspRHdtNUNuAgSFZAanc + (double)gdVzIiDslmVVHVbNKSEHhdplsweD)
				{
					kjkQRSxTuxTRGblzzgWysffAJJac();
					return true;
				}
				return false;
			}

			public void kjkQRSxTuxTRGblzzgWysffAJJac()
			{
				iCaAXETSiEHdtMZXbVPyqPHAXBBI.xCtDiCIQLqThxFjYNgEMjeBESiLdA = 0f;
				iCaAXETSiEHdtMZXbVPyqPHAXBBI.AiWrSadsDlTEuCDKSkcLKtqiQEVQ = 0f;
				NBFuwOwAnvKQDqXKJmfmgEZwAEBP.ZcjoZwbIDbbFlaWQFjFKWrESBVuu = 0;
				gdVzIiDslmVVHVbNKSEHhdplsweD = 0f;
				saQhqJpAXspRHdtNUNuAgSFZAanc = ReInput.realTime;
			}

			public void ipfFLsbkBonBmcWkRFtUDOjyzb()
			{
				iCaAXETSiEHdtMZXbVPyqPHAXBBI = mIJZURMoWXbiTFibclZPkOZWkoOwA.cjneBxgoZhifGzbcJTBcqXHcWGVAA();
				NBFuwOwAnvKQDqXKJmfmgEZwAEBP.ZcjoZwbIDbbFlaWQFjFKWrESBVuu = 0;
				gdVzIiDslmVVHVbNKSEHhdplsweD = 0f;
				saQhqJpAXspRHdtNUNuAgSFZAanc = 0.0;
			}
		}

		protected struct mIJZURMoWXbiTFibclZPkOZWkoOwA
		{
			public float AiWrSadsDlTEuCDKSkcLKtqiQEVQ;

			public float hutIcjbGXpDkDBuYBjvlQZNlHMAQA;

			public float xCtDiCIQLqThxFjYNgEMjeBESiLdA;

			public float EEjLhwaGkFzDzTomkpgHrcsKOzYB;

			internal mIJZURMoWXbiTFibclZPkOZWkoOwA(float P_0, float P_1, float P_2, float P_3)
			{
				if (P_0 < 0f)
				{
					P_0 = 0f;
				}
				if (P_0 > 1f)
				{
					P_0 = 1f;
				}
				if (P_1 < 0f)
				{
					P_1 = 0f;
				}
				if (P_2 < 0f)
				{
					P_2 = 0f;
				}
				if (P_2 > 1f)
				{
					P_2 = 1f;
				}
				if (P_3 < 0f)
				{
					P_3 = 0f;
				}
				AiWrSadsDlTEuCDKSkcLKtqiQEVQ = P_0;
				hutIcjbGXpDkDBuYBjvlQZNlHMAQA = P_1;
				xCtDiCIQLqThxFjYNgEMjeBESiLdA = P_2;
				EEjLhwaGkFzDzTomkpgHrcsKOzYB = P_3;
			}

			public static mIJZURMoWXbiTFibclZPkOZWkoOwA cjneBxgoZhifGzbcJTBcqXHcWGVAA()
			{
				return new mIJZURMoWXbiTFibclZPkOZWkoOwA(0f, 160f, 0f, 320f);
			}

			public string tDtxBTtfKUiWQgURUOgXiXbxvCgg()
			{
				return "amplitudeLow: " + AiWrSadsDlTEuCDKSkcLKtqiQEVQ + ", frequencyLow: " + hutIcjbGXpDkDBuYBjvlQZNlHMAQA + ", amplitudeHigh: " + xCtDiCIQLqThxFjYNgEMjeBESiLdA + ", frequencyHigh: " + EEjLhwaGkFzDzTomkpgHrcsKOzYB;
			}
		}

		private struct LGKUNJycLZjkHFXfEFJdSFgJGnrX
		{
			public byte IBvxuXMGCkeHHflveIdnlUsNWYQy;

			public byte[] VgaoTztJXcTQjSfoHqFenpBYaNSGA;

			public int TmRBkJeUjnjTmaPnvVcNaCYZmNigb;

			public LGKUNJycLZjkHFXfEFJdSFgJGnrX(byte P_0, byte[] P_1, int P_2)
			{
				IBvxuXMGCkeHHflveIdnlUsNWYQy = P_0;
				VgaoTztJXcTQjSfoHqFenpBYaNSGA = P_1;
				TmRBkJeUjnjTmaPnvVcNaCYZmNigb = P_2;
			}
		}

		protected class DXLALPvbodPzPPEzSpuUrZiTuyoJ
		{
			public ushort ymUhjLEjpnmfoKqsbALgicJEmoOH;

			public ushort qiXWLnwQutdnzLygoohtJSRdzHvf;

			public ushort LIZGHpbNSgzuWMGwcGFGGTCIQofqA;

			public ushort aRRpTkkAwqKNvWFvncROFIVgylNj;

			public virtual string nfBQkGgsLXlwzuzDqNUfgMZJoeLt()
			{
				return "min: " + ymUhjLEjpnmfoKqsbALgicJEmoOH + ", max: " + qiXWLnwQutdnzLygoohtJSRdzHvf + ", zero: " + LIZGHpbNSgzuWMGwcGFGGTCIQofqA + ", deadzone: " + aRRpTkkAwqKNvWFvncROFIVgylNj;
			}
		}

		protected readonly EpsjEqBsgEWVUiCpmfVMyaeRIRPM _controllerType;

		protected readonly int _buttonCount;

		protected readonly int _axisCount;

		protected readonly int _vibrationMotorCount;

		private readonly IHIDDevice zOxwXCGDboBwxHjJBcYJBruFgLlLB;

		private readonly HIDProperties fPcoESPVQuxEWDVthiUHSEaVaiwC;

		private readonly bool wWPmkEOmdCkRqlpKxPiTGZSzAtfF;

		private readonly NativeBuffer tkCxTkhRNqjEHlCMZTuyVYkhQHeM;

		private readonly NativeBuffer tprOYEPPPfPIzeXgJHvYkmZHFYWfA;

		private readonly NativeBuffer fmihdATydDWxChnwrBvtntntkJFV;

		private readonly byte[] YYDOpvgolunfIqOYZkXeCWJkDmcs;

		private readonly NativeBuffer CERMYyhpMgehCuEjQKjPvIjgyQOC;

		private readonly NativeBuffer RjHhMLJUfPdkwjhGrkTenmlvlhrF;

		private aMZqdyjJERTAUbjSZWzzHWVxTEnF BNzVZjIXePLNiOonmlazzaGkhJAJ;

		private byte clesXzPFKDlsHJibGSpUEGOtcgco;

		private bool RqTSZJIGiTvCPfBVbkJLflwefOyP;

		private stfckzRjtXZbwudcgoqwfYMkbPoF[] TmwzartcIkJcoVqInFqZdGkqyRHQ;

		private DXLALPvbodPzPPEzSpuUrZiTuyoJ[] UFCqekVHOQAmRULfLIPittTCewGG;

		private double vPelTmHGHtCmFQWWbEdsfLrELFQeb;

		private Dictionary<int, jTgvXqDErhbYeYuUxwkmJsMMXLvg> ygHkEBpEuokZXvPsRSRJkGYIyLOT;

		int IDriver_NintendoSwitchController.vibrationMotorCount => _vibrationMotorCount;

		ushort IHIDControllerExtension.vendorId => fPcoESPVQuxEWDVthiUHSEaVaiwC.vendorId;

		ushort IHIDControllerExtension.productId => fPcoESPVQuxEWDVthiUHSEaVaiwC.productId;

		string IHIDControllerExtension.productName => fPcoESPVQuxEWDVthiUHSEaVaiwC.productName;

		string IHIDControllerExtension.manufacturer => fPcoESPVQuxEWDVthiUHSEaVaiwC.manufacturer;

		ushort IHIDControllerExtension.usagePage => fPcoESPVQuxEWDVthiUHSEaVaiwC.usagePage;

		ushort IHIDControllerExtension.usage => fPcoESPVQuxEWDVthiUHSEaVaiwC.usage;

		private Dictionary<int, jTgvXqDErhbYeYuUxwkmJsMMXLvg> initializationCommands
		{
			get
			{
				if (ygHkEBpEuokZXvPsRSRJkGYIyLOT == null)
				{
					ygHkEBpEuokZXvPsRSRJkGYIyLOT = new Dictionary<int, jTgvXqDErhbYeYuUxwkmJsMMXLvg> { { 1, IHGxcdFTCIvSnABpLfTmGOqwemLl } };
				}
				return ygHkEBpEuokZXvPsRSRJkGYIyLOT;
			}
		}

		public void GetVibration(int motorIndex, out float amplitudeLow, out float frequencyLow, out float amplitudeHigh, out float frequencyHigh)
		{
			if (motorIndex < 0 || motorIndex >= _vibrationMotorCount)
			{
				amplitudeLow = 0f;
				frequencyLow = 0f;
				amplitudeHigh = 0f;
				frequencyHigh = 0f;
			}
			else
			{
				mIJZURMoWXbiTFibclZPkOZWkoOwA mIJZURMoWXbiTFibclZPkOZWkoOwA2 = TmwzartcIkJcoVqInFqZdGkqyRHQ[motorIndex].NozevYZCCqkeWcqpXxyeVoGLQPJN;
				amplitudeLow = mIJZURMoWXbiTFibclZPkOZWkoOwA2.AiWrSadsDlTEuCDKSkcLKtqiQEVQ;
				frequencyLow = mIJZURMoWXbiTFibclZPkOZWkoOwA2.hutIcjbGXpDkDBuYBjvlQZNlHMAQA;
				amplitudeHigh = mIJZURMoWXbiTFibclZPkOZWkoOwA2.xCtDiCIQLqThxFjYNgEMjeBESiLdA;
				frequencyHigh = mIJZURMoWXbiTFibclZPkOZWkoOwA2.EEjLhwaGkFzDzTomkpgHrcsKOzYB;
			}
		}

		void IDriver_NintendoSwitchController.GetVibration(int motorIndex, out float amplitudeLow, out float frequencyLow, out float amplitudeHigh, out float frequencyHigh)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetVibration
			this.GetVibration(motorIndex, out amplitudeLow, out frequencyLow, out amplitudeHigh, out frequencyHigh);
		}

		public void SetVibration(int motorIndex, float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh)
		{
			SetVibration(motorIndex, amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh, 0f, stopOtherMotors: false);
		}

		void IDriver_NintendoSwitchController.SetVibration(int motorIndex, float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(motorIndex, amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh);
		}

		public void SetVibration(int motorIndex, float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh, bool stopOtherMotors)
		{
			SetVibration(motorIndex, amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh, 0f, stopOtherMotors);
		}

		void IDriver_NintendoSwitchController.SetVibration(int motorIndex, float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh, bool stopOtherMotors)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(motorIndex, amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh, stopOtherMotors);
		}

		public void SetVibration(int motorIndex, float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh, float duration)
		{
			SetVibration(motorIndex, amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh, duration, stopOtherMotors: false);
		}

		void IDriver_NintendoSwitchController.SetVibration(int motorIndex, float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh, float duration)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(motorIndex, amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh, duration);
		}

		public void SetVibration(int motorIndex, float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh, float duration, bool stopOtherMotors)
		{
			if (motorIndex >= 0 && motorIndex < _vibrationMotorCount)
			{
				if (stopOtherMotors)
				{
					SOubREjFNwJqIIsFfSHElONmgrdWA(motorIndex);
				}
				TmwzartcIkJcoVqInFqZdGkqyRHQ[motorIndex].rmcAShGojFMvKmyRoFqKJSCWaXVN(amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh, duration);
				XrdPXbwkBvByhboUsLZjUPaTpQDc(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous);
			}
		}

		void IDriver_NintendoSwitchController.SetVibration(int motorIndex, float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh, float duration, bool stopOtherMotors)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(motorIndex, amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh, duration, stopOtherMotors);
		}

		public void StopVibration(int motorIndex)
		{
			if (motorIndex >= 0 && motorIndex < _vibrationMotorCount)
			{
				TmwzartcIkJcoVqInFqZdGkqyRHQ[motorIndex].kjkQRSxTuxTRGblzzgWysffAJJac();
				XrdPXbwkBvByhboUsLZjUPaTpQDc(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous);
			}
		}

		void IDriver_NintendoSwitchController.StopVibration(int motorIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration(motorIndex);
		}

		public void StopVibration()
		{
			StopVibration(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous);
		}

		void IDriver_NintendoSwitchController.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		private void StopVibration(NTgeZKbzmGIqlMGAIOSUBklVGTkNA asyncMode)
		{
			for (int i = 0; i < _vibrationMotorCount; i++)
			{
				TmwzartcIkJcoVqInFqZdGkqyRHQ[i].kjkQRSxTuxTRGblzzgWysffAJJac();
			}
			XrdPXbwkBvByhboUsLZjUPaTpQDc(asyncMode);
		}

		private void SOubREjFNwJqIIsFfSHElONmgrdWA(int P_0)
		{
			for (int i = 0; i < TmwzartcIkJcoVqInFqZdGkqyRHQ.Length; i++)
			{
				if (i != P_0)
				{
					TmwzartcIkJcoVqInFqZdGkqyRHQ[i].kjkQRSxTuxTRGblzzgWysffAJJac();
				}
			}
		}

		protected NintendoSwitchGamepadDriver(InitArgs P_0, EpsjEqBsgEWVUiCpmfVMyaeRIRPM P_1, int P_2, int P_3, int P_4)
		{
			if (P_0 == null)
			{
				throw new ArgumentNullException("initArgs");
			}
			_controllerType = P_1;
			_buttonCount = P_2;
			_axisCount = P_3;
			_vibrationMotorCount = P_4;
			zOxwXCGDboBwxHjJBcYJBruFgLlLB = P_0.hidDevice;
			fPcoESPVQuxEWDVthiUHSEaVaiwC = P_0.hidDevice.properties;
			wWPmkEOmdCkRqlpKxPiTGZSzAtfF = P_0.connectionType == YDvFqJokstcNyQQOYydcruGncmeb.Bluetooth;
			if (fPcoESPVQuxEWDVthiUHSEaVaiwC.maxOutputReportLength < 2)
			{
				throw new ArgumentException("Output report buffer is too small.");
			}
			tkCxTkhRNqjEHlCMZTuyVYkhQHeM = TcNARdASgOIcYMadwSbdRKHrnQmFb(fPcoESPVQuxEWDVthiUHSEaVaiwC.maxInputReportLength);
			tprOYEPPPfPIzeXgJHvYkmZHFYWfA = TcNARdASgOIcYMadwSbdRKHrnQmFb(fPcoESPVQuxEWDVthiUHSEaVaiwC.maxOutputReportLength);
			fmihdATydDWxChnwrBvtntntkJFV = TcNARdASgOIcYMadwSbdRKHrnQmFb(32);
			YYDOpvgolunfIqOYZkXeCWJkDmcs = new byte[fPcoESPVQuxEWDVthiUHSEaVaiwC.maxInputReportLength];
			CERMYyhpMgehCuEjQKjPvIjgyQOC = TcNARdASgOIcYMadwSbdRKHrnQmFb(fPcoESPVQuxEWDVthiUHSEaVaiwC.maxOutputReportLength);
			RjHhMLJUfPdkwjhGrkTenmlvlhrF = TcNARdASgOIcYMadwSbdRKHrnQmFb(49);
			BNzVZjIXePLNiOonmlazzaGkhJAJ = new aMZqdyjJERTAUbjSZWzzHWVxTEnF(tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Pointer, tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Length, tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Length);
			ReInput.ApplicationPauseChangedEvent += SrKffnKqQOccSfZjEydXFmGzKwgtb;
			buttons = new RyDagBEfRFfkQlRDvQAHmQXROhrtA[P_2];
			for (int i = 0; i < P_2; i++)
			{
				buttons[i] = new RyDagBEfRFfkQlRDvQAHmQXROhrtA(48, new LDJGvqLnFydDhJMnXduxzIERUQI.HIDInfo
				{
					usagePage = 9,
					usage = (ushort)i
				});
			}
			UFCqekVHOQAmRULfLIPittTCewGG = new DXLALPvbodPzPPEzSpuUrZiTuyoJ[_axisCount];
			vibrationMotors = new iwnZquMFWHwhZjzckYkHRPdcqkIc[P_4];
			for (int j = 0; j < vibrationMotors.Length; j++)
			{
				vibrationMotors[j] = new iwnZquMFWHwhZjzckYkHRPdcqkIc(0, 255);
			}
			TmwzartcIkJcoVqInFqZdGkqyRHQ = new stfckzRjtXZbwudcgoqwfYMkbPoF[P_4];
			for (int k = 0; k < TmwzartcIkJcoVqInFqZdGkqyRHQ.Length; k++)
			{
				TmwzartcIkJcoVqInFqZdGkqyRHQ[k] = new stfckzRjtXZbwudcgoqwfYMkbPoF(vibrationMotors[k]);
			}
		}

		protected void Initialize()
		{
			RqTSZJIGiTvCPfBVbkJLflwefOyP = false;
			try
			{
				tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Clear();
				if (!wWPmkEOmdCkRqlpKxPiTGZSzAtfF)
				{
					NativeBuffer nativeBuffer = tprOYEPPPfPIzeXgJHvYkmZHFYWfA;
					KtRluNMbuBpTXfviQLXzvuRHWVed(KgnAJVvECgDMOyRGJrStJEOjaPCh.GetUsbStatus, 5u, 500u);
					nativeBuffer[0] = 128;
					nativeBuffer[1] = 2;
					if (!GwPfwsoklgixbEVZSTjEOsdeFdAU(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous))
					{
						Logger.LogError("Failed to write output report to device: USB handshake 1.", requiredThreadSafety: true);
						throw new Exception();
					}
					nativeBuffer[0] = 128;
					nativeBuffer[1] = 3;
					if (!GwPfwsoklgixbEVZSTjEOsdeFdAU(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous))
					{
						Logger.LogError("Failed to write output report to device: USB set baudrate.", requiredThreadSafety: true);
						throw new Exception();
					}
					nativeBuffer[0] = 128;
					nativeBuffer[1] = 2;
					if (!GwPfwsoklgixbEVZSTjEOsdeFdAU(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous))
					{
						Logger.LogError("Failed to write output report to device: USB handshake 2.", requiredThreadSafety: true);
						throw new Exception();
					}
					nativeBuffer[0] = 128;
					nativeBuffer[1] = 4;
					if (!GwPfwsoklgixbEVZSTjEOsdeFdAU(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous))
					{
						Logger.LogError("Failed to write output report to device: USB prevent hid timeout.", requiredThreadSafety: true);
						throw new Exception();
					}
				}
				if (!blJGbhwNzdoBuJExbuVEPPLcGFfKA(new LGKUNJycLZjkHFXfEFJdSFgJGnrX(72, new byte[1] { 1 }, 1), YYDOpvgolunfIqOYZkXeCWJkDmcs))
				{
					throw new Exception();
				}
				if (!blJGbhwNzdoBuJExbuVEPPLcGFfKA(new LGKUNJycLZjkHFXfEFJdSFgJGnrX(3, new byte[1] { 48 }, 1), YYDOpvgolunfIqOYZkXeCWJkDmcs))
				{
					throw new Exception();
				}
				kbLacBkxjSieSxuLtENuewlisUYr();
				if (!fHoXfUgoCEswRwbCYrnCpwfjraZv())
				{
					throw new Exception();
				}
				RqTSZJIGiTvCPfBVbkJLflwefOyP = true;
			}
			catch
			{
				Dispose();
				throw;
			}
		}

		private bool KtRluNMbuBpTXfviQLXzvuRHWVed(KgnAJVvECgDMOyRGJrStJEOjaPCh P_0, uint P_1, uint P_2)
		{
			if (!initializationCommands.TryGetValue((int)P_0, out var value))
			{
				Logger.LogError("Unknown command.", requiredThreadSafety: true);
				throw new Exception();
			}
			uint num = 0u;
			do
			{
				if (value(P_2))
				{
					return true;
				}
				num++;
			}
			while (num <= P_1);
			throw new Exception();
		}

		private bool IHGxcdFTCIvSnABpLfTmGOqwemLl(uint P_0)
		{
			NativeBuffer nativeBuffer = tprOYEPPPfPIzeXgJHvYkmZHFYWfA;
			nativeBuffer.Clear();
			nativeBuffer[0] = 128;
			nativeBuffer[1] = 1;
			if (!GwPfwsoklgixbEVZSTjEOsdeFdAU(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous))
			{
				return false;
			}
			double num = ReInput.realTime + (double)((float)P_0 * 0.001f);
			do
			{
				IL_0069:
				if (zOxwXCGDboBwxHjJBcYJBruFgLlLB.ReadSync(RjHhMLJUfPdkwjhGrkTenmlvlhrF, RjHhMLJUfPdkwjhGrkTenmlvlhrF.Length, 100))
				{
					if (RjHhMLJUfPdkwjhGrkTenmlvlhrF[0] == 129 && RjHhMLJUfPdkwjhGrkTenmlvlhrF[1] == 1)
					{
						return true;
					}
					if (!(ReInput.realTime >= num))
					{
						goto IL_0069;
					}
				}
			}
			while (ReInput.realTime < num);
			return false;
		}

		public override void Update(UpdateLoopType updateLoop)
		{
			double realTime = ReInput.realTime;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < TmwzartcIkJcoVqInFqZdGkqyRHQ.Length; i++)
			{
				if (TmwzartcIkJcoVqInFqZdGkqyRHQ[i].bugUdxGmbAfafpHCqzpvuceHeKYGA(realTime))
				{
					flag = true;
				}
				if (TmwzartcIkJcoVqInFqZdGkqyRHQ[i].ZLcESAzWTHKLOQqWVZJlOeBVkSYK)
				{
					flag2 = true;
				}
			}
			if (flag || (flag2 && realTime >= vPelTmHGHtCmFQWWbEdsfLrELFQeb + 0.02500000037252903))
			{
				XrdPXbwkBvByhboUsLZjUPaTpQDc(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous);
			}
		}

		public override bool ParseInputReport(IntPtr inputReportPtr, int inputReportLength, double timestamp)
		{
			if (!RqTSZJIGiTvCPfBVbkJLflwefOyP)
			{
				return false;
			}
			if (inputReportPtr == IntPtr.Zero)
			{
				return false;
			}
			if (tkCxTkhRNqjEHlCMZTuyVYkhQHeM.Length < 49)
			{
				return false;
			}
			byte b = Marshal.ReadByte(inputReportPtr, 0);
			if (b != 33 && b != 48)
			{
				return false;
			}
			int numBytesToWrite = Math.Min(inputReportLength, tkCxTkhRNqjEHlCMZTuyVYkhQHeM.Length);
			tkCxTkhRNqjEHlCMZTuyVYkhQHeM.Write(inputReportPtr, inputReportLength, numBytesToWrite);
			tkCxTkhRNqjEHlCMZTuyVYkhQHeM.Write((byte)48, 0);
			UpdateButtons(tkCxTkhRNqjEHlCMZTuyVYkhQHeM, timestamp);
			LDJGvqLnFydDhJMnXduxzIERUQI[] elements = axes;
			UpdateElements(elements, tkCxTkhRNqjEHlCMZTuyVYkhQHeM, timestamp);
			return true;
		}

		protected abstract void UpdateButtons(NativeBuffer inputReport, double timestamp);

		protected abstract void UpdateElements(LDJGvqLnFydDhJMnXduxzIERUQI[] elements, NativeBuffer inputReport, double timestamp);

		private bool blJGbhwNzdoBuJExbuVEPPLcGFfKA(LGKUNJycLZjkHFXfEFJdSFgJGnrX P_0, byte[] P_1)
		{
			try
			{
				if (P_0.VgaoTztJXcTQjSfoHqFenpBYaNSGA.Length + 11 > CERMYyhpMgehCuEjQKjPvIjgyQOC.Length)
				{
					return false;
				}
				pGnQgrKelDpkGjCTUSqFOVtrgEgo(CERMYyhpMgehCuEjQKjPvIjgyQOC);
				CERMYyhpMgehCuEjQKjPvIjgyQOC[10] = P_0.IBvxuXMGCkeHHflveIdnlUsNWYQy;
				CERMYyhpMgehCuEjQKjPvIjgyQOC.TryWriteBytes(P_0.VgaoTztJXcTQjSfoHqFenpBYaNSGA, P_0.TmRBkJeUjnjTmaPnvVcNaCYZmNigb, 11);
				int num = 3;
				bool flag = false;
				int num2 = 0;
				double num3 = 0.0;
				while (zOxwXCGDboBwxHjJBcYJBruFgLlLB.ReadSync(RjHhMLJUfPdkwjhGrkTenmlvlhrF, RjHhMLJUfPdkwjhGrkTenmlvlhrF.Length, 1))
				{
				}
				for (int i = 0; i < num; i++)
				{
					Array.Clear(P_1, 0, P_1.Length);
					RjHhMLJUfPdkwjhGrkTenmlvlhrF.Clear();
					gduBBZUQfwpwFJYyuQPnHOASkEb(CERMYyhpMgehCuEjQKjPvIjgyQOC, P_0.IBvxuXMGCkeHHflveIdnlUsNWYQy);
					num3 = ReInput.realTime;
					if (i == 0)
					{
						_ = ReInput.realTime;
					}
					int num4 = 0;
					while (!(ReInput.realTime >= num3 + 1.0))
					{
						if (zOxwXCGDboBwxHjJBcYJBruFgLlLB.ReadSync(RjHhMLJUfPdkwjhGrkTenmlvlhrF, RjHhMLJUfPdkwjhGrkTenmlvlhrF.Length, 200) && RjHhMLJUfPdkwjhGrkTenmlvlhrF[0] == 33)
						{
							if (RjHhMLJUfPdkwjhGrkTenmlvlhrF[14] == P_0.IBvxuXMGCkeHHflveIdnlUsNWYQy)
							{
								flag = true;
								_ = ReInput.realTime;
								break;
							}
							num4++;
							num2++;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					RjHhMLJUfPdkwjhGrkTenmlvlhrF.Read(P_1, RjHhMLJUfPdkwjhGrkTenmlvlhrF.Length);
				}
				return flag;
			}
			catch (Exception)
			{
				return false;
			}
		}

		private bool gduBBZUQfwpwFJYyuQPnHOASkEb(NativeBuffer P_0, byte P_1)
		{
			if (!zOxwXCGDboBwxHjJBcYJBruFgLlLB.WriteSync(new aMZqdyjJERTAUbjSZWzzHWVxTEnF(P_0, P_0.Length, P_0.Length), 1000))
			{
				return false;
			}
			return true;
		}

		private void pGnQgrKelDpkGjCTUSqFOVtrgEgo(NativeBuffer P_0)
		{
			P_0.Clear();
			P_0[0] = 1;
			P_0[1] = WABluCimHCcvvDrflhcSQWFQZfLeA();
			kBTjrtQWRXrsacskdHeoPYaXFrbC(P_0, 2);
		}

		private void kBTjrtQWRXrsacskdHeoPYaXFrbC(NativeBuffer P_0, int P_1)
		{
			if (_controllerType == EpsjEqBsgEWVUiCpmfVMyaeRIRPM.JoyConRight)
			{
				P_1 += 4;
			}
			for (int i = 0; i < TmwzartcIkJcoVqInFqZdGkqyRHQ.Length; i++)
			{
				AcxXYdplDrKmePvhFFXIEYvwRnXb(P_0, P_1, TmwzartcIkJcoVqInFqZdGkqyRHQ[i].NozevYZCCqkeWcqpXxyeVoGLQPJN);
				P_1 += 4;
			}
		}

		private static void AcxXYdplDrKmePvhFFXIEYvwRnXb(NativeBuffer P_0, int P_1, mIJZURMoWXbiTFibclZPkOZWkoOwA P_2)
		{
			if (P_1 + 4 >= P_0.Length)
			{
				return;
			}
			if (P_2.AiWrSadsDlTEuCDKSkcLKtqiQEVQ == 0f && P_2.xCtDiCIQLqThxFjYNgEMjeBESiLdA == 0f)
			{
				P_0[P_1] = 0;
				P_0[1 + P_1] = 1;
				P_0[2 + P_1] = 64;
				P_0[3 + P_1] = 64;
				return;
			}
			ushort num = (ushort)((Math.Round(32.0 * Math.Log(P_2.EEjLhwaGkFzDzTomkpgHrcsKOzYB * 0.1f, 2.0)) - 96.0) * 4.0);
			byte b = (byte)(Math.Round(32.0 * Math.Log(P_2.hutIcjbGXpDkDBuYBjvlQZNlHMAQA * 0.1f, 2.0)) - 64.0);
			byte b2 = UZTGJthNknmPOykOEvwDsMjJnDoZ(P_2.xCtDiCIQLqThxFjYNgEMjeBESiLdA);
			ushort num2 = (ushort)(Math.Round((double)(int)UZTGJthNknmPOykOEvwDsMjJnDoZ(P_2.AiWrSadsDlTEuCDKSkcLKtqiQEVQ)) * 0.5);
			byte num3 = (byte)(num2 % 2);
			if (num3 > 0)
			{
				num2--;
			}
			num2 >>= 1;
			num2 += 64;
			if (num3 > 0)
			{
				num2 |= 0x8000;
			}
			b2 = (byte)(b2 - b2 % 2);
			P_0[P_1] = (byte)(num & 0xFF);
			P_0[1 + P_1] = (byte)(((num >> 8) & 0xFF) + b2);
			P_0[2 + P_1] = (byte)(((num2 >> 8) & 0xFF) + b);
			P_0[3 + P_1] = (byte)(num2 & 0xFF);
		}

		private static byte UZTGJthNknmPOykOEvwDsMjJnDoZ(float P_0)
		{
			if (P_0 < 0.01f)
			{
				return 0;
			}
			if ((double)P_0 < 0.117)
			{
				return (byte)((Math.Log(P_0 * 1000f, 2.0) * 32.0 - 96.0) / (5.0 - Math.Pow(P_0, 2.0)) - 1.0);
			}
			if ((double)P_0 < 0.23)
			{
				return (byte)(Math.Log(P_0 * 1000f, 2.0) * 32.0 - 96.0 - 92.0);
			}
			return (byte)((Math.Log(P_0 * 1000f, 2.0) * 32.0 - 96.0) * 2.0 - 246.0);
		}

		private void XrdPXbwkBvByhboUsLZjUPaTpQDc(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			if (tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Length >= 2 + TmwzartcIkJcoVqInFqZdGkqyRHQ.Length * 4)
			{
				tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Clear();
				tprOYEPPPfPIzeXgJHvYkmZHFYWfA[0] = 16;
				tprOYEPPPfPIzeXgJHvYkmZHFYWfA[1] = WABluCimHCcvvDrflhcSQWFQZfLeA();
				kBTjrtQWRXrsacskdHeoPYaXFrbC(tprOYEPPPfPIzeXgJHvYkmZHFYWfA, 2);
				if (GwPfwsoklgixbEVZSTjEOsdeFdAU(P_0))
				{
					vPelTmHGHtCmFQWWbEdsfLrELFQeb = ReInput.realTime;
				}
			}
		}

		private bool kbLacBkxjSieSxuLtENuewlisUYr()
		{
			byte[] array = new byte[25];
			ArrayTools.Fill(array, (byte)255);
			array[0] = 24;
			array[1] = 1;
			return blJGbhwNzdoBuJExbuVEPPLcGFfKA(new LGKUNJycLZjkHFXfEFJdSFgJGnrX(56, array, 25), YYDOpvgolunfIqOYZkXeCWJkDmcs);
		}

		private bool GVphTkAakQDZOIIEOMtIlVTQieRm(byte P_0, byte P_1, byte P_2, byte[] P_3)
		{
			byte[] array = new byte[5] { P_1, P_0, 0, 0, P_2 };
			bool flag = false;
			for (int i = 0; i < 10; i++)
			{
				if (blJGbhwNzdoBuJExbuVEPPLcGFfKA(new LGKUNJycLZjkHFXfEFJdSFgJGnrX(16, array, array.Length), P_3) && P_3[15] == P_1 && P_3[16] == P_0)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			Array.Copy(P_3, 20, P_3, 0, P_2);
			ArrayTools.Fill(P_3, (byte)0, (int)P_2, P_3.Length - P_2);
			return true;
		}

		private bool GwPfwsoklgixbEVZSTjEOsdeFdAU(NTgeZKbzmGIqlMGAIOSUBklVGTkNA P_0)
		{
			switch (P_0)
			{
			case NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous:
				return zOxwXCGDboBwxHjJBcYJBruFgLlLB.WriteSync(BNzVZjIXePLNiOonmlazzaGkhJAJ, 0);
			case NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Asynchronous:
				zOxwXCGDboBwxHjJBcYJBruFgLlLB.WriteAsync(BNzVZjIXePLNiOonmlazzaGkhJAJ, 1000);
				return true;
			default:
				throw new NotImplementedException();
			}
		}

		private byte WABluCimHCcvvDrflhcSQWFQZfLeA()
		{
			if (clesXzPFKDlsHJibGSpUEGOtcgco == 15)
			{
				clesXzPFKDlsHJibGSpUEGOtcgco = 0;
			}
			else
			{
				clesXzPFKDlsHJibGSpUEGOtcgco++;
			}
			return clesXzPFKDlsHJibGSpUEGOtcgco;
		}

		private bool fHoXfUgoCEswRwbCYrnCpwfjraZv()
		{
			bool flag = false;
			bool flag2 = _controllerType == EpsjEqBsgEWVUiCpmfVMyaeRIRPM.JoyConLeft || _controllerType == EpsjEqBsgEWVUiCpmfVMyaeRIRPM.ProController;
			Array.Clear(UFCqekVHOQAmRULfLIPittTCewGG, 0, UFCqekVHOQAmRULfLIPittTCewGG.Length);
			bool flag3 = false;
			if (GVphTkAakQDZOIIEOMtIlVTQieRm(128, (byte)(flag2 ? 18 : 29), 9, YYDOpvgolunfIqOYZkXeCWJkDmcs))
			{
				for (int i = 0; i < 9; i++)
				{
					if (YYDOpvgolunfIqOYZkXeCWJkDmcs[i] != 255)
					{
						flag3 = true;
					}
				}
			}
			if (!flag3 && GVphTkAakQDZOIIEOMtIlVTQieRm(96, (byte)(flag2 ? 61 : 70), 9, YYDOpvgolunfIqOYZkXeCWJkDmcs))
			{
				flag3 = true;
			}
			if (flag3)
			{
				DXLALPvbodPzPPEzSpuUrZiTuyoJ dXLALPvbodPzPPEzSpuUrZiTuyoJ = new DXLALPvbodPzPPEzSpuUrZiTuyoJ();
				DXLALPvbodPzPPEzSpuUrZiTuyoJ dXLALPvbodPzPPEzSpuUrZiTuyoJ2 = new DXLALPvbodPzPPEzSpuUrZiTuyoJ();
				aPygMUkDgOUBQVsIIxgVxqwVqGwU(YYDOpvgolunfIqOYZkXeCWJkDmcs, dXLALPvbodPzPPEzSpuUrZiTuyoJ, dXLALPvbodPzPPEzSpuUrZiTuyoJ2, flag2);
				UFCqekVHOQAmRULfLIPittTCewGG[0] = dXLALPvbodPzPPEzSpuUrZiTuyoJ;
				UFCqekVHOQAmRULfLIPittTCewGG[1] = dXLALPvbodPzPPEzSpuUrZiTuyoJ2;
				flag = true;
				if (GVphTkAakQDZOIIEOMtIlVTQieRm(96, (byte)(flag2 ? 134 : 152), 16, YYDOpvgolunfIqOYZkXeCWJkDmcs))
				{
					OetviBdstPjfEcXoBGdyeqzRFbfeA(YYDOpvgolunfIqOYZkXeCWJkDmcs, dXLALPvbodPzPPEzSpuUrZiTuyoJ, dXLALPvbodPzPPEzSpuUrZiTuyoJ2);
				}
			}
			else
			{
				flag = false;
			}
			if (_controllerType == EpsjEqBsgEWVUiCpmfVMyaeRIRPM.ProController)
			{
				bool flag4 = false;
				if (GVphTkAakQDZOIIEOMtIlVTQieRm(128, (byte)((!flag2) ? 18 : 29), 9, YYDOpvgolunfIqOYZkXeCWJkDmcs))
				{
					for (int j = 0; j < 9; j++)
					{
						if (YYDOpvgolunfIqOYZkXeCWJkDmcs[j] != 255)
						{
							flag4 = true;
						}
					}
				}
				if (!flag4 && GVphTkAakQDZOIIEOMtIlVTQieRm(96, (byte)((!flag2) ? 61 : 70), 9, YYDOpvgolunfIqOYZkXeCWJkDmcs))
				{
					flag4 = true;
				}
				if (flag4)
				{
					DXLALPvbodPzPPEzSpuUrZiTuyoJ dXLALPvbodPzPPEzSpuUrZiTuyoJ3 = new DXLALPvbodPzPPEzSpuUrZiTuyoJ();
					DXLALPvbodPzPPEzSpuUrZiTuyoJ dXLALPvbodPzPPEzSpuUrZiTuyoJ4 = new DXLALPvbodPzPPEzSpuUrZiTuyoJ();
					aPygMUkDgOUBQVsIIxgVxqwVqGwU(YYDOpvgolunfIqOYZkXeCWJkDmcs, dXLALPvbodPzPPEzSpuUrZiTuyoJ3, dXLALPvbodPzPPEzSpuUrZiTuyoJ4, !flag2);
					UFCqekVHOQAmRULfLIPittTCewGG[2] = dXLALPvbodPzPPEzSpuUrZiTuyoJ3;
					UFCqekVHOQAmRULfLIPittTCewGG[3] = dXLALPvbodPzPPEzSpuUrZiTuyoJ4;
					flag = true;
					if (GVphTkAakQDZOIIEOMtIlVTQieRm(96, (byte)((!flag2) ? 134 : 152), 16, YYDOpvgolunfIqOYZkXeCWJkDmcs))
					{
						OetviBdstPjfEcXoBGdyeqzRFbfeA(YYDOpvgolunfIqOYZkXeCWJkDmcs, dXLALPvbodPzPPEzSpuUrZiTuyoJ3, dXLALPvbodPzPPEzSpuUrZiTuyoJ4);
					}
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		private static void aPygMUkDgOUBQVsIIxgVxqwVqGwU(byte[] P_0, DXLALPvbodPzPPEzSpuUrZiTuyoJ P_1, DXLALPvbodPzPPEzSpuUrZiTuyoJ P_2, bool P_3)
		{
			ushort num = (ushort)(((P_0[1] << 8) & 0xF00) | P_0[0]);
			ushort num2 = (ushort)((P_0[2] << 4) | (P_0[1] >> 4));
			ushort num3 = (ushort)(((P_0[4] << 8) & 0xF00) | P_0[3]);
			ushort num4 = (ushort)((P_0[5] << 4) | (P_0[4] >> 4));
			ushort num5 = (ushort)(((P_0[7] << 8) & 0xF00) | P_0[6]);
			ushort num6 = (ushort)((P_0[8] << 4) | (P_0[7] >> 4));
			if (P_3)
			{
				P_1.qiXWLnwQutdnzLygoohtJSRdzHvf = num;
				P_2.qiXWLnwQutdnzLygoohtJSRdzHvf = num2;
				P_1.LIZGHpbNSgzuWMGwcGFGGTCIQofqA = num3;
				P_2.LIZGHpbNSgzuWMGwcGFGGTCIQofqA = num4;
				P_1.ymUhjLEjpnmfoKqsbALgicJEmoOH = num5;
				P_2.ymUhjLEjpnmfoKqsbALgicJEmoOH = num6;
			}
			else
			{
				P_1.LIZGHpbNSgzuWMGwcGFGGTCIQofqA = num;
				P_2.LIZGHpbNSgzuWMGwcGFGGTCIQofqA = num2;
				P_1.ymUhjLEjpnmfoKqsbALgicJEmoOH = num3;
				P_2.ymUhjLEjpnmfoKqsbALgicJEmoOH = num4;
				P_1.qiXWLnwQutdnzLygoohtJSRdzHvf = num5;
				P_2.qiXWLnwQutdnzLygoohtJSRdzHvf = num6;
			}
		}

		private static void OetviBdstPjfEcXoBGdyeqzRFbfeA(byte[] P_0, DXLALPvbodPzPPEzSpuUrZiTuyoJ P_1, DXLALPvbodPzPPEzSpuUrZiTuyoJ P_2)
		{
			P_1.aRRpTkkAwqKNvWFvncROFIVgylNj = (ushort)(((P_0[4] << 8) & 0xF00) | P_0[3]);
			P_2.aRRpTkkAwqKNvWFvncROFIVgylNj = P_1.aRRpTkkAwqKNvWFvncROFIVgylNj;
		}

		protected bool GetCalibratedStickValue(ushort valueX, ushort valueY, DXLALPvbodPzPPEzSpuUrZiTuyoJ calX, DXLALPvbodPzPPEzSpuUrZiTuyoJ calY, out ushort calibratedX, out ushort calibratedY)
		{
			calibratedX = 32767;
			calibratedY = 32767;
			if (calX == null || calY == null)
			{
				return false;
			}
			ushort aRRpTkkAwqKNvWFvncROFIVgylNj = calX.aRRpTkkAwqKNvWFvncROFIVgylNj;
			float num = valueX - calX.LIZGHpbNSgzuWMGwcGFGGTCIQofqA;
			float num2 = valueY - calY.LIZGHpbNSgzuWMGwcGFGGTCIQofqA;
			if (Math.Abs(num * num + num2 * num2) < (float)(aRRpTkkAwqKNvWFvncROFIVgylNj * aRRpTkkAwqKNvWFvncROFIVgylNj))
			{
				return false;
			}
			calibratedX = (ushort)MathTools.ValueInNewRange(MathTools.Clamp(num / (float)(int)((num > 0f) ? calX.qiXWLnwQutdnzLygoohtJSRdzHvf : calX.ymUhjLEjpnmfoKqsbALgicJEmoOH), -0.95f, 0.95f), -0.95f, 0.95f, 0f, 65535f);
			calibratedY = (ushort)MathTools.ValueInNewRange(MathTools.Clamp(num2 / (float)(int)((num2 > 0f) ? calY.qiXWLnwQutdnzLygoohtJSRdzHvf : calY.ymUhjLEjpnmfoKqsbALgicJEmoOH), -0.95f, 0.95f), -0.95f, 0.95f, 0f, 65535f);
			return true;
		}

		protected DXLALPvbodPzPPEzSpuUrZiTuyoJ GetAxisCalibration(int index)
		{
			return UFCqekVHOQAmRULfLIPittTCewGG[index];
		}

		private void SrKffnKqQOccSfZjEydXFmGzKwgtb(bool P_0)
		{
		}

		~NintendoSwitchGamepadDriver()
		{
			Dispose(disposing: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (base.disposed)
			{
				return;
			}
			if (disposing)
			{
				ReInput.ApplicationPauseChangedEvent -= SrKffnKqQOccSfZjEydXFmGzKwgtb;
				try
				{
					StopVibration(NTgeZKbzmGIqlMGAIOSUBklVGTkNA.Synchronous);
				}
				catch (Exception)
				{
				}
				if (!wWPmkEOmdCkRqlpKxPiTGZSzAtfF && tprOYEPPPfPIzeXgJHvYkmZHFYWfA != null)
				{
					tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Clear();
					tprOYEPPPfPIzeXgJHvYkmZHFYWfA[0] = 128;
					tprOYEPPPfPIzeXgJHvYkmZHFYWfA[1] = 5;
					try
					{
						zOxwXCGDboBwxHjJBcYJBruFgLlLB.WriteSync(BNzVZjIXePLNiOonmlazzaGkhJAJ, 0);
					}
					catch
					{
					}
					tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Clear();
					tprOYEPPPfPIzeXgJHvYkmZHFYWfA[0] = 128;
					tprOYEPPPfPIzeXgJHvYkmZHFYWfA[1] = 6;
					try
					{
						zOxwXCGDboBwxHjJBcYJBruFgLlLB.WriteSync(BNzVZjIXePLNiOonmlazzaGkhJAJ, 0);
					}
					catch
					{
					}
				}
				if (tkCxTkhRNqjEHlCMZTuyVYkhQHeM != null)
				{
					tkCxTkhRNqjEHlCMZTuyVYkhQHeM.Dispose();
				}
				if (tprOYEPPPfPIzeXgJHvYkmZHFYWfA != null)
				{
					tprOYEPPPfPIzeXgJHvYkmZHFYWfA.Dispose();
				}
				if (CERMYyhpMgehCuEjQKjPvIjgyQOC != null)
				{
					CERMYyhpMgehCuEjQKjPvIjgyQOC.Dispose();
				}
				if (RjHhMLJUfPdkwjhGrkTenmlvlhrF != null)
				{
					RjHhMLJUfPdkwjhGrkTenmlvlhrF.Dispose();
				}
				if (fmihdATydDWxChnwrBvtntntkJFV == null)
				{
					fmihdATydDWxChnwrBvtntntkJFV.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private static NativeBuffer TcNARdASgOIcYMadwSbdRKHrnQmFb(int P_0)
		{
			NativeBuffer nativeBuffer = new NativeBuffer(P_0);
			if (nativeBuffer.Length != P_0)
			{
				throw new Exception("Failed to allocate memory.");
			}
			return nativeBuffer;
		}
	}
}
