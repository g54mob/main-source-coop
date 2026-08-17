using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Rewired;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Platforms;
using Rewired.Platforms.Windows.DirectInput;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

internal class wXurhSlljPxwGOSKaGPOzKtqnUtm : PlatformInputManager, LrgFZTysTrlKPEyvhAYFSHPAbEyV
{
	private class eySnxgpDnPUzyhkGpTWqagWLjids : IInputManagerJoystick, IInputManagerJoystickPublic
	{
		private int zpbFBdwRTcliRKRycAPTDUfZkuSf;

		private int GxlDpOCPlGMoOToZYdvHTPatfrxN;

		public Guid ijnyrDSNUvwQyIrbNBxzWiRILaeV;

		public string LiWsZPifmknkHLVXdGaeSrHgBrmKA;

		public readonly WtFyEJeQdSfjObjirEdLChzyqzFpA PKlODBQBiDgMBEfJhWvttvaAJIxmA;

		public AZmRVMBCLWjdSJdmldWMNaRORDDE QBVYxdtibGkLrtiQxFwokOeZtsWA;

		public DbcdvnelxeDtXsOptqKPFmRfXXCcA CwhHBDZhlkQjnJvqUGdoOdUMVsfK;

		public string XqViQakmppWVzWGyLZWWdtdVuuSC;

		public string iWVQeywgpMGYQjlLkSeMnIqgqKzH;

		public int NjPaIVrAwsYAZzJBujbZNwpNReEV;

		public Guid ndpCcpaFlxXPRiUPxtfMAQjJymNS;

		public Guid XhIEXYFzsDCrZdeZrJaluEOiBoVgA;

		public Guid bPKUQQOuSxJXkBAetBaJbHwHPStDA;

		public int DlHDYRKYxFEekvHErjQWmmMWdcUGA;

		public bool oPWNpkLdCWYmPIxHQPkyVBwoizMA;

		public string PaMhIShNdZqnKPZmVQHlOwiQwXVCA;

		public string BCrvVjeWDoOVwayuqpGWqOqYWIXs;

		public int DCpgAUBamDuaklIaPSAKqOqNyOKaA;

		public int sjirONFlnehJdaHWENVtfFUIWdiiA;

		public int hTjAsxCEdFIqVgGpcElosIWvJZtrB;

		public int biFqRTcWgotJCdYTKlrjBUiwvpLf;

		public int oHydTJYKdBErDKpaWkNlQajUmvjuA;

		public bool BGMSobwPwBMCEvEdfPSmcBahMxhp;

		public Controller.Extension xschkhGYczeWeFxnOREeVuyJGzotA;

		private float[] OSsGjYdbvEANjNYifAPoaovBhsBrB;

		private bool[] lZTDrGHIvVcoqKOkRWrKUXoQbXlyA;

		private HardwareJoystickMap_InputManager ZLFHMjcLIETlkzibBmaSeGKiFXNl;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> MPivaBLtjwqPLRIBqIEmCckTAdUk;

		private bool iqOVLoJQLHONrmuDxtcnHfDHlfXC;

		private bool WHSOWtldDKKEAgrPmhtWySKCpFkW;

		private bool OnwCbQWkXSNMMZPexBrgTXFggnTc;

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.rewiredId
		{
			get
			{
				return zpbFBdwRTcliRKRycAPTDUfZkuSf;
			}
			set
			{
				zpbFBdwRTcliRKRycAPTDUfZkuSf = value;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.inputManagerId
		{
			get
			{
				return GxlDpOCPlGMoOToZYdvHTPatfrxN;
			}
			set
			{
				GxlDpOCPlGMoOToZYdvHTPatfrxN = value;
			}
		}

		[CustomObfuscation(rename = false)]
		string IInputManagerJoystickPublic.name
		{
			get
			{
				if (LiWsZPifmknkHLVXdGaeSrHgBrmKA != "Unknown Controller")
				{
					return LiWsZPifmknkHLVXdGaeSrHgBrmKA;
				}
				if (oPWNpkLdCWYmPIxHQPkyVBwoizMA && !string.IsNullOrEmpty(PaMhIShNdZqnKPZmVQHlOwiQwXVCA))
				{
					return PaMhIShNdZqnKPZmVQHlOwiQwXVCA;
				}
				return iWVQeywgpMGYQjlLkSeMnIqgqKzH;
			}
		}

		[CustomObfuscation(rename = false)]
		long? IInputManagerJoystickPublic.systemId
		{
			get
			{
				if (GxlDpOCPlGMoOToZYdvHTPatfrxN < 0)
				{
					return null;
				}
				return GxlDpOCPlGMoOToZYdvHTPatfrxN;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.unityId => 0;

		[CustomObfuscation(rename = false)]
		Controller.Extension IInputManagerJoystickPublic.extension => xschkhGYczeWeFxnOREeVuyJGzotA;

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.instanceGuid => ndpCcpaFlxXPRiUPxtfMAQjJymNS;

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.persistentGuid => Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;

		[CustomObfuscation(rename = false)]
		public void SetVibration(float amount, int motorIndex)
		{
		}

		void IInputManagerJoystickPublic.SetVibration(float amount, int motorIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(amount, motorIndex);
		}

		[CustomObfuscation(rename = false)]
		public void StopVibration()
		{
		}

		void IInputManagerJoystickPublic.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		public eySnxgpDnPUzyhkGpTWqagWLjids(WtFyEJeQdSfjObjirEdLChzyqzFpA P_0, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_1)
		{
			PKlODBQBiDgMBEfJhWvttvaAJIxmA = P_0;
			MPivaBLtjwqPLRIBqIEmCckTAdUk = P_1;
			GxlDpOCPlGMoOToZYdvHTPatfrxN = -1;
			zpbFBdwRTcliRKRycAPTDUfZkuSf = -1;
		}

		public void RkDkIEodKqMuPrgDZBHWaFMAKOIR()
		{
			string text = iWVQeywgpMGYQjlLkSeMnIqgqKzH;
			Guid xhIEXYFzsDCrZdeZrJaluEOiBoVgA = XhIEXYFzsDCrZdeZrJaluEOiBoVgA;
			bPKUQQOuSxJXkBAetBaJbHwHPStDA = MiscTools.CreateGuidHashSHA1(text + xhIEXYFzsDCrZdeZrJaluEOiBoVgA.ToString());
			DCpgAUBamDuaklIaPSAKqOqNyOKaA = hTjAsxCEdFIqVgGpcElosIWvJZtrB;
			sjirONFlnehJdaHWENVtfFUIWdiiA = biFqRTcWgotJCdYTKlrjBUiwvpLf + oHydTJYKdBErDKpaWkNlQajUmvjuA * 8;
			sbQUtgelwDfKusShXLAIUAwUxsJI();
			ijnyrDSNUvwQyIrbNBxzWiRILaeV = ZLFHMjcLIETlkzibBmaSeGKiFXNl.hardwareMapIdentifier.guid;
			LiWsZPifmknkHLVXdGaeSrHgBrmKA = ZLFHMjcLIETlkzibBmaSeGKiFXNl.controllerName;
			iqOVLoJQLHONrmuDxtcnHfDHlfXC = ijnyrDSNUvwQyIrbNBxzWiRILaeV == Guid.Empty;
			OSsGjYdbvEANjNYifAPoaovBhsBrB = new float[DCpgAUBamDuaklIaPSAKqOqNyOKaA];
			lZTDrGHIvVcoqKOkRWrKUXoQbXlyA = new bool[sjirONFlnehJdaHWENVtfFUIWdiiA];
			PKlODBQBiDgMBEfJhWvttvaAJIxmA.cYCctDtCkrKlUSRcccFdNgORGRhjA();
			Update();
		}

		public void slgUmYnKazmjpjPEadlrwhmYwjHh(eySnxgpDnPUzyhkGpTWqagWLjids P_0)
		{
			if (P_0 != null)
			{
				GxlDpOCPlGMoOToZYdvHTPatfrxN = P_0.GxlDpOCPlGMoOToZYdvHTPatfrxN;
				zpbFBdwRTcliRKRycAPTDUfZkuSf = P_0.zpbFBdwRTcliRKRycAPTDUfZkuSf;
				for (int i = 0; i < MathTools.Min(lZTDrGHIvVcoqKOkRWrKUXoQbXlyA.Length, P_0.lZTDrGHIvVcoqKOkRWrKUXoQbXlyA.Length); i++)
				{
					lZTDrGHIvVcoqKOkRWrKUXoQbXlyA[i] = P_0.lZTDrGHIvVcoqKOkRWrKUXoQbXlyA[i];
				}
				for (int j = 0; j < MathTools.Min(OSsGjYdbvEANjNYifAPoaovBhsBrB.Length, P_0.OSsGjYdbvEANjNYifAPoaovBhsBrB.Length); j++)
				{
					OSsGjYdbvEANjNYifAPoaovBhsBrB[j] = P_0.OSsGjYdbvEANjNYifAPoaovBhsBrB[j];
				}
				WHSOWtldDKKEAgrPmhtWySKCpFkW = P_0.WHSOWtldDKKEAgrPmhtWySKCpFkW;
				PKlODBQBiDgMBEfJhWvttvaAJIxmA.wDYwVmcoElvdjsyRWuLigIoYRmAt(P_0.PKlODBQBiDgMBEfJhWvttvaAJIxmA);
			}
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			PKlODBQBiDgMBEfJhWvttvaAJIxmA.LALLSsvfLLrdgebywGNQunRNIbuD();
			bool[] array = PKlODBQBiDgMBEfJhWvttvaAJIxmA.jdNIHJlklkDayRPXMJnFTtoWFOLE;
			int[] idJdbVGwyzvheplqsVSgULIJSMFHA = PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.idJdbVGwyzvheplqsVSgULIJSMFHA;
			hkTFvmRxrshVZJZcHRAepGkIEtuR(array, idJdbVGwyzvheplqsVSgULIJSMFHA);
			cgSTQsmtkpEzieeEoIoyxEHQYPcm(array, idJdbVGwyzvheplqsVSgULIJSMFHA);
			PKlODBQBiDgMBEfJhWvttvaAJIxmA.dKlTIDkVOArtGJIBgeJKspeILsEx();
		}

		void IInputManagerJoystick.Update()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Update
			this.Update();
		}

		[CustomObfuscation(rename = false)]
		public void FillData(ControllerDataUpdater dataUpdater)
		{
			if (DCpgAUBamDuaklIaPSAKqOqNyOKaA != dataUpdater.axisCount || sjirONFlnehJdaHWENVtfFUIWdiiA != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			for (int i = 0; i < DCpgAUBamDuaklIaPSAKqOqNyOKaA; i++)
			{
				dataUpdater.axisValues[i] = OSsGjYdbvEANjNYifAPoaovBhsBrB[i];
			}
			for (int j = 0; j < sjirONFlnehJdaHWENVtfFUIWdiiA; j++)
			{
				dataUpdater.buttonValues[j] = lZTDrGHIvVcoqKOkRWrKUXoQbXlyA[j];
			}
			if (WHSOWtldDKKEAgrPmhtWySKCpFkW && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		void IInputManagerJoystick.FillData(ControllerDataUpdater dataUpdater)
		{
			//ILSpy generated this explicit interface implementation from .override directive in FillData
			this.FillData(dataUpdater);
		}

		public int MFHZGXqDVsHuHOBjbDbxgoqBLuzd(eySnxgpDnPUzyhkGpTWqagWLjids P_0)
		{
			if (P_0.zpbFBdwRTcliRKRycAPTDUfZkuSf == zpbFBdwRTcliRKRycAPTDUfZkuSf)
			{
				return 2;
			}
			if (hTjAsxCEdFIqVgGpcElosIWvJZtrB != P_0.hTjAsxCEdFIqVgGpcElosIWvJZtrB)
			{
				return 0;
			}
			if (biFqRTcWgotJCdYTKlrjBUiwvpLf != P_0.biFqRTcWgotJCdYTKlrjBUiwvpLf)
			{
				return 0;
			}
			if (oHydTJYKdBErDKpaWkNlQajUmvjuA != P_0.oHydTJYKdBErDKpaWkNlQajUmvjuA)
			{
				return 0;
			}
			if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid == Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid)
			{
				return 2;
			}
			if (P_0.bPKUQQOuSxJXkBAetBaJbHwHPStDA == bPKUQQOuSxJXkBAetBaJbHwHPStDA)
			{
				return 1;
			}
			return 0;
		}

		private BridgedControllerHWInfo lNsplGZXNVZEndpWLAJsRDrJQgJL()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
			yeOdSOIwuCuppwSrDKHpGEebiNRdb(bridgedControllerHWInfo);
			return bridgedControllerHWInfo;
		}

		[CustomObfuscation(rename = false)]
		public BridgedController ToBridgedController()
		{
			BridgedController bridgedController = new BridgedController();
			ljVoEoNCxJyHRHMQLgzwnbhMDOiO(bridgedController);
			return bridgedController;
		}

		BridgedController IInputManagerJoystick.ToBridgedController()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToBridgedController
			return this.ToBridgedController();
		}

		[CustomObfuscation(rename = false)]
		public ControllerDisconnectedEventArgs ToControllerDisconnectedEventArgs()
		{
			return new ControllerDisconnectedEventArgs(zpbFBdwRTcliRKRycAPTDUfZkuSf);
		}

		ControllerDisconnectedEventArgs IInputManagerJoystick.ToControllerDisconnectedEventArgs()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToControllerDisconnectedEventArgs
			return this.ToControllerDisconnectedEventArgs();
		}

		public bool BMfPcUGHAojfIdCnOpODVGQjFHicb()
		{
			try
			{
				PKlODBQBiDgMBEfJhWvttvaAJIxmA.mBNCpNkMAbDKlnnmLbDDTOnprebgA.NbkOwBMWQWPypJZUBoykfGjafWYC();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void FhOUygdauUQvwpMOoQeAaVgoqTMB()
		{
			try
			{
				if (PKlODBQBiDgMBEfJhWvttvaAJIxmA.mBNCpNkMAbDKlnnmLbDDTOnprebgA != null)
				{
					PKlODBQBiDgMBEfJhWvttvaAJIxmA.mBNCpNkMAbDKlnnmLbDDTOnprebgA.OBNDhaklAXLCuCkmkMPULPILkHiDb();
				}
			}
			catch
			{
			}
		}

		private void hkTFvmRxrshVZJZcHRAepGkIEtuR(bool[] P_0, int[] P_1)
		{
			if (DCpgAUBamDuaklIaPSAKqOqNyOKaA <= 0)
			{
				return;
			}
			switch (ZLFHMjcLIETlkzibBmaSeGKiFXNl.map.platform)
			{
			case InputPlatform.WindowsRawInput:
			{
				HardwareJoystickMap.Platform_RawInput_Base.Axis[] axes_orig2 = ((HardwareJoystickMap.Platform_RawInput_Base)ZLFHMjcLIETlkzibBmaSeGKiFXNl.map).Axes_orig;
				if (axes_orig2 != null)
				{
					for (int j = 0; j < axes_orig2.Length; j++)
					{
						FYlhhaVAawehAfNPZDasaIhHUpROA(axes_orig2[j], j, P_0, P_1);
					}
				}
				break;
			}
			case InputPlatform.WindowsDirectInput:
			{
				HardwareJoystickMap.Platform_DirectInput_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_DirectInput_Base)ZLFHMjcLIETlkzibBmaSeGKiFXNl.map).Axes_orig;
				if (axes_orig != null)
				{
					for (int i = 0; i < axes_orig.Length; i++)
					{
						FYlhhaVAawehAfNPZDasaIhHUpROA(axes_orig[i], i, P_0, P_1);
					}
				}
				break;
			}
			}
		}

		private void cgSTQsmtkpEzieeEoIoyxEHQYPcm(bool[] P_0, int[] P_1)
		{
			if (sjirONFlnehJdaHWENVtfFUIWdiiA <= 0)
			{
				return;
			}
			switch (ZLFHMjcLIETlkzibBmaSeGKiFXNl.map.platform)
			{
			case InputPlatform.WindowsRawInput:
			{
				HardwareJoystickMap.Platform_RawInput_Base.Button[] buttons_orig2 = ((HardwareJoystickMap.Platform_RawInput_Base)ZLFHMjcLIETlkzibBmaSeGKiFXNl.map).Buttons_orig;
				if (buttons_orig2 != null)
				{
					for (int j = 0; j < buttons_orig2.Length; j++)
					{
						czbvRiKzjpmHONtISDCTUXLNBeik(buttons_orig2[j], j, P_0, P_1);
					}
				}
				break;
			}
			case InputPlatform.WindowsDirectInput:
			{
				HardwareJoystickMap.Platform_DirectInput_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_DirectInput_Base)ZLFHMjcLIETlkzibBmaSeGKiFXNl.map).Buttons_orig;
				if (buttons_orig != null)
				{
					for (int i = 0; i < buttons_orig.Length; i++)
					{
						czbvRiKzjpmHONtISDCTUXLNBeik(buttons_orig[i], i, P_0, P_1);
					}
				}
				break;
			}
			}
		}

		private void FYlhhaVAawehAfNPZDasaIhHUpROA(HardwareJoystickMap.Platform_RawOrDirectInput.Axis_Base P_0, int P_1, bool[] P_2, int[] P_3)
		{
			if (P_1 >= DCpgAUBamDuaklIaPSAKqOqNyOKaA)
			{
				throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
			}
			OSsGjYdbvEANjNYifAPoaovBhsBrB[P_1] = inNcuMmBXtbZlmopSSMvAJSkMBOE(P_0, P_2, P_3);
			if (!WHSOWtldDKKEAgrPmhtWySKCpFkW && OSsGjYdbvEANjNYifAPoaovBhsBrB[P_1] != 0f)
			{
				WHSOWtldDKKEAgrPmhtWySKCpFkW = true;
			}
		}

		private void czbvRiKzjpmHONtISDCTUXLNBeik(HardwareJoystickMap.Platform_RawOrDirectInput.Button_Base P_0, int P_1, bool[] P_2, int[] P_3)
		{
			if (P_1 >= sjirONFlnehJdaHWENVtfFUIWdiiA)
			{
				throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
			}
			lZTDrGHIvVcoqKOkRWrKUXoQbXlyA[P_1] = nPETaIPRUHSleISygFIkfVWbYADrA(P_0, P_2, P_3);
			if (!WHSOWtldDKKEAgrPmhtWySKCpFkW && lZTDrGHIvVcoqKOkRWrKUXoQbXlyA[P_1])
			{
				WHSOWtldDKKEAgrPmhtWySKCpFkW = true;
			}
		}

		private float inNcuMmBXtbZlmopSSMvAJSkMBOE(HardwareJoystickMap.Platform_RawOrDirectInput.Axis_Base P_0, bool[] P_1, int[] P_2)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				if (P_0.sourceAxis <= 0 || P_0.sourceAxis >= 32)
				{
					return 0f;
				}
				return KVSRmTHGABUvuxUSWkEbPDPlZjZM((DirectInputAxis)P_0.sourceAxis);
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= biFqRTcWgotJCdYTKlrjBUiwvpLf || sourceButton >= 128)
				{
					return 0f;
				}
				if (!P_1[sourceButton])
				{
					return 0f;
				}
				if (P_0.buttonAxisContribution == Pole.Positive)
				{
					return 1f;
				}
				return -1f;
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Hat)
			{
				int sourceHat = P_0.sourceHat;
				if (sourceHat < 0 || sourceHat >= oHydTJYKdBErDKpaWkNlQajUmvjuA || sourceHat >= 4)
				{
					return 0f;
				}
				int num = P_2[sourceHat];
				if (num < 0)
				{
					return 0f;
				}
				float num2;
				if (P_0.sourceHatDirection == AxisDirection.Horizontal)
				{
					num2 = vjlipKeQocDvICXaASHbWEMHfustA(num, AxisDirection.Horizontal);
					if (P_0.sourceHatRange != AxisRange.Full)
					{
						if (P_0.sourceHatRange == AxisRange.Positive)
						{
							if (num2 < 0f)
							{
								return 0f;
							}
						}
						else if (num2 > 0f)
						{
							return 0f;
						}
					}
				}
				else
				{
					num2 = vjlipKeQocDvICXaASHbWEMHfustA(num, AxisDirection.Vertical);
					if (P_0.sourceHatRange != AxisRange.Full)
					{
						if (P_0.sourceHatRange == AxisRange.Positive)
						{
							if (num2 < 0f)
							{
								return 0f;
							}
						}
						else if (num2 > 0f)
						{
							return 0f;
						}
					}
				}
				if (P_0.invert)
				{
					num2 *= -1f;
				}
				return num2;
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Custom)
			{
				CustomCalculation customCalculation = P_0.customCalculation;
				if (customCalculation == null)
				{
					return 0f;
				}
				if (customCalculation.ResultType != TypeWrapper.DataType.Single)
				{
					return 0f;
				}
				HardwareJoystickMap.Platform_RawOrDirectInput.CustomCalculationSourceData[] customCalculationSourceData = P_0.customCalculationSourceData;
				if (customCalculationSourceData == null)
				{
					return 0f;
				}
				for (int i = 0; i < customCalculationSourceData.Length; i++)
				{
					if (customCalculationSourceData[i] != null && customCalculationSourceData[i].sourceType == 1 && aoZOSHewYQwkjiKvJGFKeSjvssmg(customCalculationSourceData[i], out var item))
					{
						customCalculation.AddData(item);
					}
				}
				if (!customCalculation.Process())
				{
					return 0f;
				}
				if (customCalculation.Result.type != TypeWrapper.DataType.Single)
				{
					return 0f;
				}
				return customCalculation.Result;
			}
			return 0f;
		}

		private float KVSRmTHGABUvuxUSWkEbPDPlZjZM(DirectInputAxis P_0)
		{
			return P_0 switch
			{
				DirectInputAxis.X => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.GdLDodAwqJfctiGlKCUMdpBEsmFs, 
				DirectInputAxis.Y => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.zNQXPBsMoDJOGhqwPFXzhpoKekE, 
				DirectInputAxis.Z => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.iCYsgtraoqfFFhgdyHTCqAywoBKk, 
				DirectInputAxis.RotationX => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.AgJwWpyrguHIoUjYNDqLcwpHodgV, 
				DirectInputAxis.RotationY => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.lUOrbBkFnffvQwuqcghYboKogzigb, 
				DirectInputAxis.RotationZ => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.KyZkWSxabsGszXxBybUZaFwqjZmiA, 
				DirectInputAxis.Slider0 => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.zuQtVqBImuPSVGGmKGomathZitrJ[0], 
				DirectInputAxis.Slider1 => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.zuQtVqBImuPSVGGmKGomathZitrJ[1], 
				DirectInputAxis.VelocityX => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.tcrkijfItOlFqDMJQOEcmVndJaxt, 
				DirectInputAxis.VelocityY => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.IgNcDqBoIcfaQBDPvodaeppQuRVtA, 
				DirectInputAxis.VelocityZ => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.UaDDerAEFKhrmDHbzFoCfgBBVsMrb, 
				DirectInputAxis.AngularVelocityX => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.gDgSSFPqqPJVbUAmIzAACeSBFQlz, 
				DirectInputAxis.AngularVelocityY => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.IhmuEvMIkePzkzUrPwYQWmzXpvMn, 
				DirectInputAxis.AngularVelocityZ => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.holsvoqGVigUCHszGmGxQqrMlJDy, 
				DirectInputAxis.VelocitySlider0 => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.cTTRLRyfatffqjzjJLnFebrwqdedA[0], 
				DirectInputAxis.VelocitySlider1 => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.cTTRLRyfatffqjzjJLnFebrwqdedA[1], 
				DirectInputAxis.AccelerationX => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.TtFEQTklrsFlDBrJXpIfKIQIIECeA, 
				DirectInputAxis.AccelerationY => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.LcNYqDGmHDsgTnXfovvgUnAKPVFP, 
				DirectInputAxis.AccelerationZ => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.sAjEbUcZkrljjjPCOXcYJpTyanAP, 
				DirectInputAxis.AngularAccelerationX => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.CwoBWgXUgGAuSmKirHDWVZgPVMsK, 
				DirectInputAxis.AngularAccelerationY => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.ehoCdaMHvHwnSxyBgSMkZcnZPtOE, 
				DirectInputAxis.AngularAccelerationZ => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.CjqxpBaqAkZYVBIWXCPlJoSxwjjo, 
				DirectInputAxis.AccelerationSlider0 => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.bSLAnReblVFbFNUoWcVbNjmODsFT[0], 
				DirectInputAxis.AccelerationSlider1 => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.bSLAnReblVFbFNUoWcVbNjmODsFT[1], 
				DirectInputAxis.ForceX => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.vECNnGSKAHaUzTVIHdEXhgkqAoMIA, 
				DirectInputAxis.ForceY => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.dqlYxOpxldDzaQLDNuydVQKzjfKF, 
				DirectInputAxis.ForceZ => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.UlDFHnXDBxaQBETAawCAIffDMjCu, 
				DirectInputAxis.TorqueX => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.qcnhDaOQflwjZMhseDPZOsdyCKkhA, 
				DirectInputAxis.TorqueY => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.ihqzPlWTimmKJLVScXuukQHniHdD, 
				DirectInputAxis.TorqueZ => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.GghpcuUbLlYycQNfGyTgGECdJvMr, 
				DirectInputAxis.ForceSlider0 => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.XGgGAZfXScTdXttciBqistygdFhl[0], 
				DirectInputAxis.ForceSlider1 => PKlODBQBiDgMBEfJhWvttvaAJIxmA.WgMNPYeANHtPwGfMOltdVTSjdtfY.XGgGAZfXScTdXttciBqistygdFhl[1], 
				_ => 0f, 
			};
		}

		private bool nPETaIPRUHSleISygFIkfVWbYADrA(HardwareJoystickMap.Platform_RawOrDirectInput.Button_Base P_0, bool[] P_1, int[] P_2)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				if (P_0.ignoreIfButtonsActive)
				{
					for (int i = 0; i < P_0.ignoreIfButtonsActiveButtons.Length; i++)
					{
						if (P_1[P_0.ignoreIfButtonsActiveButtons[i]])
						{
							return false;
						}
					}
				}
				if (P_0.requireMultipleButtons)
				{
					bool flag = false;
					for (int j = 0; j < P_0.requiredButtons.Length; j++)
					{
						if (!P_1[P_0.requiredButtons[j]])
						{
							return false;
						}
						flag = true;
					}
					if (flag)
					{
						return true;
					}
					return false;
				}
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= biFqRTcWgotJCdYTKlrjBUiwvpLf || sourceButton >= 128)
				{
					return false;
				}
				return P_1[sourceButton];
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				if (P_0.sourceAxis <= 0 || P_0.sourceAxis > 32)
				{
					return false;
				}
				float num = KVSRmTHGABUvuxUSWkEbPDPlZjZM((DirectInputAxis)P_0.sourceAxis);
				if (MathTools.Abs(num) <= P_0.axisDeadZone)
				{
					return false;
				}
				if (P_0.sourceAxisPole == Pole.Positive)
				{
					if (num < 0f)
					{
						return false;
					}
				}
				else if (num > 0f)
				{
					return false;
				}
				return true;
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Hat)
			{
				int sourceHat = P_0.sourceHat;
				if (sourceHat < 0 || sourceHat >= oHydTJYKdBErDKpaWkNlQajUmvjuA || sourceHat >= 4)
				{
					return false;
				}
				switch (P_0.sourceHatDirection)
				{
				case HatDirection.Up:
					return rcNotcjiZKMktUfiWckfHfVGjCKp(P_2[sourceHat], 0, P_0.sourceHatType);
				case HatDirection.UpRight:
					return rcNotcjiZKMktUfiWckfHfVGjCKp(P_2[sourceHat], 1, P_0.sourceHatType);
				case HatDirection.Right:
					return rcNotcjiZKMktUfiWckfHfVGjCKp(P_2[sourceHat], 2, P_0.sourceHatType);
				case HatDirection.DownRight:
					return rcNotcjiZKMktUfiWckfHfVGjCKp(P_2[sourceHat], 3, P_0.sourceHatType);
				case HatDirection.Down:
					return rcNotcjiZKMktUfiWckfHfVGjCKp(P_2[sourceHat], 4, P_0.sourceHatType);
				case HatDirection.DownLeft:
					return rcNotcjiZKMktUfiWckfHfVGjCKp(P_2[sourceHat], 5, P_0.sourceHatType);
				case HatDirection.Left:
					return rcNotcjiZKMktUfiWckfHfVGjCKp(P_2[sourceHat], 6, P_0.sourceHatType);
				case HatDirection.UpLeft:
					return rcNotcjiZKMktUfiWckfHfVGjCKp(P_2[sourceHat], 7, P_0.sourceHatType);
				}
			}
			else if (P_0.sourceType == HardwareElementSourceTypeWithHat.Custom)
			{
				CustomCalculation customCalculation = P_0.customCalculation;
				if (customCalculation == null)
				{
					return false;
				}
				if (customCalculation.ResultType != TypeWrapper.DataType.Single)
				{
					return false;
				}
				HardwareJoystickMap.Platform_RawOrDirectInput.CustomCalculationSourceData[] customCalculationSourceData = P_0.customCalculationSourceData;
				if (customCalculationSourceData == null)
				{
					return false;
				}
				for (int k = 0; k < customCalculationSourceData.Length; k++)
				{
					if (customCalculationSourceData[k] == null)
					{
						continue;
					}
					switch ((HardwareElementSourceTypeWithHat)customCalculationSourceData[k].sourceType)
					{
					case HardwareElementSourceTypeWithHat.Button:
					{
						if (QyjPxoIdlOKqbRSmsbJyXasLBbeHA(customCalculationSourceData[k], P_1, out var flag2))
						{
							customCalculation.AddData(flag2 ? 1f : 0f);
						}
						break;
					}
					case HardwareElementSourceTypeWithHat.Axis:
					{
						if (aoZOSHewYQwkjiKvJGFKeSjvssmg(customCalculationSourceData[k], out var num2))
						{
							customCalculation.AddData((num2 != 0f) ? 1f : 0f);
						}
						break;
					}
					}
				}
				if (!customCalculation.Process())
				{
					return false;
				}
				if (customCalculation.Result.type != TypeWrapper.DataType.Single)
				{
					return false;
				}
				return (float)customCalculation.Result != 0f;
			}
			return false;
		}

		private bool rcNotcjiZKMktUfiWckfHfVGjCKp(int P_0, int P_1, HatType P_2)
		{
			if (P_0 < 0)
			{
				return false;
			}
			if (ZLFHMjcLIETlkzibBmaSeGKiFXNl.isUnknownController && !InputTools.HandleForced4WayHatsOnUnknownControllers(P_1, ref P_2))
			{
				return false;
			}
			int num = 4500 * P_1;
			if (P_2 == HatType.EightWay && P_0 != num)
			{
				return false;
			}
			int num2;
			int num3;
			if (P_2 == HatType.EightWay)
			{
				num2 = 31500;
				num3 = 4500;
			}
			else
			{
				num2 = 27000;
				num3 = 9000;
			}
			if (P_1 == 0 && P_0 > num2)
			{
				P_0 -= 36000;
			}
			if (P_0 < num + num3 && P_0 > num - num3)
			{
				return true;
			}
			return false;
		}

		private float vjlipKeQocDvICXaASHbWEMHfustA(int P_0, AxisDirection P_1)
		{
			if (P_0 < 0)
			{
				return 0f;
			}
			if (P_1 == AxisDirection.Vertical)
			{
				if (P_0 > 27000 || P_0 < 9000)
				{
					return 1f;
				}
				if (P_0 < 27000 && P_0 > 9000)
				{
					return -1f;
				}
				return 0f;
			}
			if (P_0 > 0 && P_0 < 18000)
			{
				return 1f;
			}
			if (P_0 > 18000)
			{
				return -1f;
			}
			return 0f;
		}

		private bool QyjPxoIdlOKqbRSmsbJyXasLBbeHA(HardwareJoystickMap.Platform_RawOrDirectInput.CustomCalculationSourceData P_0, bool[] P_1, out bool P_2)
		{
			P_2 = false;
			if (P_0.sourceType != 0)
			{
				return false;
			}
			int sourceButton = P_0.sourceButton;
			if (sourceButton < 0 || sourceButton >= biFqRTcWgotJCdYTKlrjBUiwvpLf || sourceButton >= 128)
			{
				return false;
			}
			P_2 = P_1[sourceButton];
			return true;
		}

		private bool aoZOSHewYQwkjiKvJGFKeSjvssmg(HardwareJoystickMap.Platform_RawOrDirectInput.CustomCalculationSourceData P_0, out float P_1)
		{
			P_1 = 0f;
			if (P_0.sourceType != 1)
			{
				return false;
			}
			if (P_0.sourceAxis <= 0 || P_0.sourceAxis >= 32)
			{
				return false;
			}
			P_1 = KVSRmTHGABUvuxUSWkEbPDPlZjZM((DirectInputAxis)P_0.sourceAxis);
			switch (P_0.sourceAxisRange)
			{
			case AxisRange.Negative:
				if (P_1 > 0f)
				{
					P_1 = 0f;
				}
				break;
			case AxisRange.Positive:
				if (P_1 < 0f)
				{
					P_1 = 0f;
				}
				break;
			}
			if (P_0.axisCalibrationType == AxisCalibrationType.Default)
			{
				P_1 = InputTools.GetCalibratedAxisValueClamped(P_1, P_0.axisZero, -1f, 1f, P_0.axisDeadZone, P_0.invert, applySensitivity: false, AxisSensitivityType.Multiplier, 1f, null);
			}
			else if (P_0.axisCalibrationType == AxisCalibrationType.Custom)
			{
				P_1 = InputTools.GetCalibratedAxisValueClamped(P_1, P_0.axisZero, P_0.axisMin, P_0.axisMax, P_0.axisDeadZone, P_0.invert, applySensitivity: false, AxisSensitivityType.Multiplier, 1f, null);
			}
			else if (P_0.axisCalibrationType == AxisCalibrationType.Uncalibrated && P_0.axisDeadZone > 0f && MathTools.Abs(P_1) <= P_0.axisDeadZone)
			{
				P_1 = 0f;
			}
			return true;
		}

		private ControlDeviceType TxwMhyTmwlsqwTDmnHuFIwqFIImf(DbcdvnelxeDtXsOptqKPFmRfXXCcA P_0)
		{
			return P_0 switch
			{
				DbcdvnelxeDtXsOptqKPFmRfXXCcA.Keyboard => ControlDeviceType.Keyboard, 
				DbcdvnelxeDtXsOptqKPFmRfXXCcA.Joystick => ControlDeviceType.Joystick, 
				DbcdvnelxeDtXsOptqKPFmRfXXCcA.Gamepad => ControlDeviceType.Gamepad, 
				DbcdvnelxeDtXsOptqKPFmRfXXCcA.Mouse => ControlDeviceType.Mouse, 
				DbcdvnelxeDtXsOptqKPFmRfXXCcA.Flight => ControlDeviceType.Flight, 
				DbcdvnelxeDtXsOptqKPFmRfXXCcA.Driving => ControlDeviceType.Wheel, 
				_ => ControlDeviceType.Unknown, 
			};
		}

		private void sbQUtgelwDfKusShXLAIUAwUxsJI()
		{
			ZLFHMjcLIETlkzibBmaSeGKiFXNl = MPivaBLtjwqPLRIBqIEmCckTAdUk(lNsplGZXNVZEndpWLAJsRDrJQgJL());
			if (ZLFHMjcLIETlkzibBmaSeGKiFXNl == null)
			{
				Logger.LogError("Default hardware map not found!");
				return;
			}
			DCpgAUBamDuaklIaPSAKqOqNyOKaA = ZLFHMjcLIETlkzibBmaSeGKiFXNl.axisCount;
			sjirONFlnehJdaHWENVtfFUIWdiiA = ZLFHMjcLIETlkzibBmaSeGKiFXNl.buttonCount;
		}

		private string NuIeqsCQwnLJSMJqwApwAmGuCbSbb()
		{
			return InputTools.FormatHardwareIdentifierString(string.Format("{0}{1}{2}{3}{4}", ReInput.currentPlatform.ToString(), InputSource.DirectInput, (oPWNpkLdCWYmPIxHQPkyVBwoizMA && !string.IsNullOrEmpty(PaMhIShNdZqnKPZmVQHlOwiQwXVCA)) ? PaMhIShNdZqnKPZmVQHlOwiQwXVCA : iWVQeywgpMGYQjlLkSeMnIqgqKzH, NjPaIVrAwsYAZzJBujbZNwpNReEV.ToString("X4"), new PidVid(XhIEXYFzsDCrZdeZrJaluEOiBoVgA).vendorId.ToString("X4")));
		}

		private void yeOdSOIwuCuppwSrDKHpGEebiNRdb(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.DirectInput;
			P_0.inputSource = P_0.inputManagerSource;
			P_0.deviceType = TxwMhyTmwlsqwTDmnHuFIwqFIImf(CwhHBDZhlkQjnJvqUGdoOdUMVsfK);
			P_0.hardwareIdentifier = NuIeqsCQwnLJSMJqwApwAmGuCbSbb();
			P_0.hardwareAxisCount = hTjAsxCEdFIqVgGpcElosIWvJZtrB;
			P_0.hardwareButtonCount = biFqRTcWgotJCdYTKlrjBUiwvpLf;
			P_0.hardwareHatCount = oHydTJYKdBErDKpaWkNlQajUmvjuA;
			P_0.hw_productName = iWVQeywgpMGYQjlLkSeMnIqgqKzH;
			P_0.hw_deviceGuid = Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;
			P_0.hw_productId = NjPaIVrAwsYAZzJBujbZNwpNReEV;
			P_0.hw_pidVid = new PidVid(XhIEXYFzsDCrZdeZrJaluEOiBoVgA);
			P_0.hw_isBluetoothDevice = oPWNpkLdCWYmPIxHQPkyVBwoizMA;
			P_0.hw_bluetoothDeviceName = ((!string.IsNullOrEmpty(PaMhIShNdZqnKPZmVQHlOwiQwXVCA)) ? PaMhIShNdZqnKPZmVQHlOwiQwXVCA : string.Empty);
			P_0.definitionMatchTag = BCrvVjeWDoOVwayuqpGWqOqYWIXs;
		}

		private void ljVoEoNCxJyHRHMQLgzwnbhMDOiO(BridgedController P_0)
		{
			yeOdSOIwuCuppwSrDKHpGEebiNRdb(P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = ZLFHMjcLIETlkzibBmaSeGKiFXNl.ToGameHardwareControllerMap();
			P_0.instanceName = XqViQakmppWVzWGyLZWWdtdVuuSC;
			P_0.productName = iWVQeywgpMGYQjlLkSeMnIqgqKzH;
			P_0.isXInputDevice = BGMSobwPwBMCEvEdfPSmcBahMxhp;
			P_0.axisCount = DCpgAUBamDuaklIaPSAKqOqNyOKaA;
			P_0.buttonCount = sjirONFlnehJdaHWENVtfFUIWdiiA;
			P_0.unknownControllerHats = YnNgzDnItbomqknhQHBGsRAsVMsJ();
			P_0.controllerTypeGuid = ijnyrDSNUvwQyIrbNBxzWiRILaeV;
			P_0.controllerExtension = Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Eextension;
		}

		private UnknownControllerHat[] YnNgzDnItbomqknhQHBGsRAsVMsJ()
		{
			if (!iqOVLoJQLHONrmuDxtcnHfDHlfXC)
			{
				return null;
			}
			UnknownControllerHat[] array = new UnknownControllerHat[2];
			for (int i = 0; i < 2; i++)
			{
				int num = 128 + i * 8;
				UnknownControllerHat.HatButtons hatButtons = new UnknownControllerHat.HatButtons(new int[8]
				{
					num,
					num + 1,
					num + 2,
					num + 3,
					num + 4,
					num + 5,
					num + 6,
					num + 7
				});
				array[i] = new UnknownControllerHat(hatButtons);
			}
			return array;
		}

		public void HfWZXJtxroFOgThibiGxfMhjXyGMA()
		{
			vScFGAWzrCKeZmICRgejRxkYlrlp(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void cfhIkRKXdLXVhAhTRXkgwGfNXIvTA()
		{
			try
			{
				vScFGAWzrCKeZmICRgejRxkYlrlp(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void vScFGAWzrCKeZmICRgejRxkYlrlp(bool P_0)
		{
			if (!OnwCbQWkXSNMMZPexBrgTXFggnTc)
			{
				if (P_0 && PKlODBQBiDgMBEfJhWvttvaAJIxmA != null)
				{
					PKlODBQBiDgMBEfJhWvttvaAJIxmA.Dispose();
				}
				OnwCbQWkXSNMMZPexBrgTXFggnTc = true;
			}
		}

		public static int BdTIuEfOcsaZQZPLJkmkWjmfNMnuA(eySnxgpDnPUzyhkGpTWqagWLjids P_0, eySnxgpDnPUzyhkGpTWqagWLjids P_1)
		{
			if (P_0.GxlDpOCPlGMoOToZYdvHTPatfrxN < P_1.GxlDpOCPlGMoOToZYdvHTPatfrxN)
			{
				return -1;
			}
			if (P_0.GxlDpOCPlGMoOToZYdvHTPatfrxN > P_1.GxlDpOCPlGMoOToZYdvHTPatfrxN)
			{
				return 1;
			}
			return 0;
		}

		public static int EtPAYJsCFRFePNcqZdjeDiRjJyVT(eySnxgpDnPUzyhkGpTWqagWLjids P_0, eySnxgpDnPUzyhkGpTWqagWLjids P_1)
		{
			if (P_0.DlHDYRKYxFEekvHErjQWmmMWdcUGA < P_1.DlHDYRKYxFEekvHErjQWmmMWdcUGA)
			{
				return -1;
			}
			if (P_0.DlHDYRKYxFEekvHErjQWmmMWdcUGA > P_1.DlHDYRKYxFEekvHErjQWmmMWdcUGA)
			{
				return 1;
			}
			return 0;
		}
	}

	private class WtFyEJeQdSfjObjirEdLChzyqzFpA : IDisposable
	{
		public class WMFXrjibkepVHxKQaTfxIGFUNKMb
		{
			public float GdLDodAwqJfctiGlKCUMdpBEsmFs;

			public float zNQXPBsMoDJOGhqwPFXzhpoKekE;

			public float iCYsgtraoqfFFhgdyHTCqAywoBKk;

			public float AgJwWpyrguHIoUjYNDqLcwpHodgV;

			public float lUOrbBkFnffvQwuqcghYboKogzigb;

			public float KyZkWSxabsGszXxBybUZaFwqjZmiA;

			public float[] zuQtVqBImuPSVGGmKGomathZitrJ;

			public readonly int[] idJdbVGwyzvheplqsVSgULIJSMFHA;

			public readonly bool[] QXvEhzIeFDPLSSuwuoZWEtoAqifTb;

			public float tcrkijfItOlFqDMJQOEcmVndJaxt;

			public float IgNcDqBoIcfaQBDPvodaeppQuRVtA;

			public float UaDDerAEFKhrmDHbzFoCfgBBVsMrb;

			public float gDgSSFPqqPJVbUAmIzAACeSBFQlz;

			public float IhmuEvMIkePzkzUrPwYQWmzXpvMn;

			public float holsvoqGVigUCHszGmGxQqrMlJDy;

			public readonly float[] cTTRLRyfatffqjzjJLnFebrwqdedA;

			public float TtFEQTklrsFlDBrJXpIfKIQIIECeA;

			public float LcNYqDGmHDsgTnXfovvgUnAKPVFP;

			public float sAjEbUcZkrljjjPCOXcYJpTyanAP;

			public float CwoBWgXUgGAuSmKirHDWVZgPVMsK;

			public float ehoCdaMHvHwnSxyBgSMkZcnZPtOE;

			public float CjqxpBaqAkZYVBIWXCPlJoSxwjjo;

			public readonly float[] bSLAnReblVFbFNUoWcVbNjmODsFT;

			public float vECNnGSKAHaUzTVIHdEXhgkqAoMIA;

			public float dqlYxOpxldDzaQLDNuydVQKzjfKF;

			public float UlDFHnXDBxaQBETAawCAIffDMjCu;

			public float qcnhDaOQflwjZMhseDPZOsdyCKkhA;

			public float ihqzPlWTimmKJLVScXuukQHniHdD;

			public float GghpcuUbLlYycQNfGyTgGECdJvMr;

			public readonly float[] XGgGAZfXScTdXttciBqistygdFhl;

			public WMFXrjibkepVHxKQaTfxIGFUNKMb()
			{
				zuQtVqBImuPSVGGmKGomathZitrJ = new float[2];
				idJdbVGwyzvheplqsVSgULIJSMFHA = new int[4];
				QXvEhzIeFDPLSSuwuoZWEtoAqifTb = new bool[128];
				cTTRLRyfatffqjzjJLnFebrwqdedA = new float[2];
				bSLAnReblVFbFNUoWcVbNjmODsFT = new float[2];
				XGgGAZfXScTdXttciBqistygdFhl = new float[2];
			}

			public void GnCARrOsnVmBWvePPOlIJfLilVio()
			{
				GdLDodAwqJfctiGlKCUMdpBEsmFs = 0f;
				zNQXPBsMoDJOGhqwPFXzhpoKekE = 0f;
				iCYsgtraoqfFFhgdyHTCqAywoBKk = 0f;
				AgJwWpyrguHIoUjYNDqLcwpHodgV = 0f;
				lUOrbBkFnffvQwuqcghYboKogzigb = 0f;
				KyZkWSxabsGszXxBybUZaFwqjZmiA = 0f;
				for (int i = 0; i < zuQtVqBImuPSVGGmKGomathZitrJ.Length; i++)
				{
					zuQtVqBImuPSVGGmKGomathZitrJ[i] = 0f;
				}
				for (int j = 0; j < idJdbVGwyzvheplqsVSgULIJSMFHA.Length; j++)
				{
					idJdbVGwyzvheplqsVSgULIJSMFHA[j] = 0;
				}
				for (int k = 0; k < QXvEhzIeFDPLSSuwuoZWEtoAqifTb.Length; k++)
				{
					QXvEhzIeFDPLSSuwuoZWEtoAqifTb[k] = false;
				}
				tcrkijfItOlFqDMJQOEcmVndJaxt = 0f;
				IgNcDqBoIcfaQBDPvodaeppQuRVtA = 0f;
				UaDDerAEFKhrmDHbzFoCfgBBVsMrb = 0f;
				gDgSSFPqqPJVbUAmIzAACeSBFQlz = 0f;
				IhmuEvMIkePzkzUrPwYQWmzXpvMn = 0f;
				holsvoqGVigUCHszGmGxQqrMlJDy = 0f;
				for (int l = 0; l < cTTRLRyfatffqjzjJLnFebrwqdedA.Length; l++)
				{
					cTTRLRyfatffqjzjJLnFebrwqdedA[l] = 0f;
				}
				TtFEQTklrsFlDBrJXpIfKIQIIECeA = 0f;
				LcNYqDGmHDsgTnXfovvgUnAKPVFP = 0f;
				sAjEbUcZkrljjjPCOXcYJpTyanAP = 0f;
				CwoBWgXUgGAuSmKirHDWVZgPVMsK = 0f;
				ehoCdaMHvHwnSxyBgSMkZcnZPtOE = 0f;
				CjqxpBaqAkZYVBIWXCPlJoSxwjjo = 0f;
				for (int m = 0; m < bSLAnReblVFbFNUoWcVbNjmODsFT.Length; m++)
				{
					bSLAnReblVFbFNUoWcVbNjmODsFT[m] = 0f;
				}
				vECNnGSKAHaUzTVIHdEXhgkqAoMIA = 0f;
				dqlYxOpxldDzaQLDNuydVQKzjfKF = 0f;
				UlDFHnXDBxaQBETAawCAIffDMjCu = 0f;
				qcnhDaOQflwjZMhseDPZOsdyCKkhA = 0f;
				ihqzPlWTimmKJLVScXuukQHniHdD = 0f;
				GghpcuUbLlYycQNfGyTgGECdJvMr = 0f;
				for (int n = 0; n < XGgGAZfXScTdXttciBqistygdFhl.Length; n++)
				{
					XGgGAZfXScTdXttciBqistygdFhl[n] = 0f;
				}
			}

			public void ycvbQuRBohIKMnipANEpQhdkTULB(WMFXrjibkepVHxKQaTfxIGFUNKMb P_0)
			{
				GdLDodAwqJfctiGlKCUMdpBEsmFs = P_0.GdLDodAwqJfctiGlKCUMdpBEsmFs;
				zNQXPBsMoDJOGhqwPFXzhpoKekE = P_0.zNQXPBsMoDJOGhqwPFXzhpoKekE;
				iCYsgtraoqfFFhgdyHTCqAywoBKk = P_0.iCYsgtraoqfFFhgdyHTCqAywoBKk;
				AgJwWpyrguHIoUjYNDqLcwpHodgV = P_0.AgJwWpyrguHIoUjYNDqLcwpHodgV;
				lUOrbBkFnffvQwuqcghYboKogzigb = P_0.lUOrbBkFnffvQwuqcghYboKogzigb;
				KyZkWSxabsGszXxBybUZaFwqjZmiA = P_0.KyZkWSxabsGszXxBybUZaFwqjZmiA;
				for (int i = 0; i < zuQtVqBImuPSVGGmKGomathZitrJ.Length; i++)
				{
					zuQtVqBImuPSVGGmKGomathZitrJ[i] = P_0.zuQtVqBImuPSVGGmKGomathZitrJ[i];
				}
				for (int j = 0; j < idJdbVGwyzvheplqsVSgULIJSMFHA.Length; j++)
				{
					idJdbVGwyzvheplqsVSgULIJSMFHA[j] = P_0.idJdbVGwyzvheplqsVSgULIJSMFHA[j];
				}
				for (int k = 0; k < QXvEhzIeFDPLSSuwuoZWEtoAqifTb.Length; k++)
				{
					QXvEhzIeFDPLSSuwuoZWEtoAqifTb[k] = P_0.QXvEhzIeFDPLSSuwuoZWEtoAqifTb[k];
				}
				tcrkijfItOlFqDMJQOEcmVndJaxt = P_0.tcrkijfItOlFqDMJQOEcmVndJaxt;
				IgNcDqBoIcfaQBDPvodaeppQuRVtA = P_0.IgNcDqBoIcfaQBDPvodaeppQuRVtA;
				UaDDerAEFKhrmDHbzFoCfgBBVsMrb = P_0.UaDDerAEFKhrmDHbzFoCfgBBVsMrb;
				gDgSSFPqqPJVbUAmIzAACeSBFQlz = P_0.gDgSSFPqqPJVbUAmIzAACeSBFQlz;
				IhmuEvMIkePzkzUrPwYQWmzXpvMn = P_0.IhmuEvMIkePzkzUrPwYQWmzXpvMn;
				holsvoqGVigUCHszGmGxQqrMlJDy = P_0.holsvoqGVigUCHszGmGxQqrMlJDy;
				for (int l = 0; l < cTTRLRyfatffqjzjJLnFebrwqdedA.Length; l++)
				{
					cTTRLRyfatffqjzjJLnFebrwqdedA[l] = P_0.cTTRLRyfatffqjzjJLnFebrwqdedA[l];
				}
				TtFEQTklrsFlDBrJXpIfKIQIIECeA = P_0.TtFEQTklrsFlDBrJXpIfKIQIIECeA;
				LcNYqDGmHDsgTnXfovvgUnAKPVFP = P_0.LcNYqDGmHDsgTnXfovvgUnAKPVFP;
				sAjEbUcZkrljjjPCOXcYJpTyanAP = P_0.sAjEbUcZkrljjjPCOXcYJpTyanAP;
				CwoBWgXUgGAuSmKirHDWVZgPVMsK = P_0.CwoBWgXUgGAuSmKirHDWVZgPVMsK;
				ehoCdaMHvHwnSxyBgSMkZcnZPtOE = P_0.ehoCdaMHvHwnSxyBgSMkZcnZPtOE;
				CjqxpBaqAkZYVBIWXCPlJoSxwjjo = P_0.CjqxpBaqAkZYVBIWXCPlJoSxwjjo;
				for (int m = 0; m < bSLAnReblVFbFNUoWcVbNjmODsFT.Length; m++)
				{
					bSLAnReblVFbFNUoWcVbNjmODsFT[m] = P_0.bSLAnReblVFbFNUoWcVbNjmODsFT[m];
				}
				vECNnGSKAHaUzTVIHdEXhgkqAoMIA = P_0.vECNnGSKAHaUzTVIHdEXhgkqAoMIA;
				dqlYxOpxldDzaQLDNuydVQKzjfKF = P_0.dqlYxOpxldDzaQLDNuydVQKzjfKF;
				UlDFHnXDBxaQBETAawCAIffDMjCu = P_0.UlDFHnXDBxaQBETAawCAIffDMjCu;
				qcnhDaOQflwjZMhseDPZOsdyCKkhA = P_0.qcnhDaOQflwjZMhseDPZOsdyCKkhA;
				ihqzPlWTimmKJLVScXuukQHniHdD = P_0.ihqzPlWTimmKJLVScXuukQHniHdD;
				GghpcuUbLlYycQNfGyTgGECdJvMr = P_0.GghpcuUbLlYycQNfGyTgGECdJvMr;
				for (int n = 0; n < XGgGAZfXScTdXttciBqistygdFhl.Length; n++)
				{
					XGgGAZfXScTdXttciBqistygdFhl[n] = P_0.XGgGAZfXScTdXttciBqistygdFhl[n];
				}
			}

			public unsafe void uZsxeGoNsVKpswdSuBOJPCQmNIPi(ref LowLevelInputEvent P_0)
			{
				for (int i = 0; i < 4; i++)
				{
					int num = *(int*)((byte*)(void*)P_0._buffer + P_0.byteIndex_buttonsStart + i * 4);
					for (int j = 0; j < 32; j++)
					{
						QXvEhzIeFDPLSSuwuoZWEtoAqifTb[i * 32 + j] = (num & (1 << j)) != 0;
					}
				}
				float* ptr = (float*)((byte*)(void*)P_0._buffer + P_0.byteIndex_axesStart);
				for (int k = 0; k < 2; k++)
				{
					bSLAnReblVFbFNUoWcVbNjmODsFT[k] = *ptr;
					ptr++;
				}
				TtFEQTklrsFlDBrJXpIfKIQIIECeA = *ptr;
				ptr++;
				LcNYqDGmHDsgTnXfovvgUnAKPVFP = *ptr;
				ptr++;
				sAjEbUcZkrljjjPCOXcYJpTyanAP = *ptr;
				ptr++;
				CwoBWgXUgGAuSmKirHDWVZgPVMsK = *ptr;
				ptr++;
				ehoCdaMHvHwnSxyBgSMkZcnZPtOE = *ptr;
				ptr++;
				CjqxpBaqAkZYVBIWXCPlJoSxwjjo = *ptr;
				ptr++;
				gDgSSFPqqPJVbUAmIzAACeSBFQlz = *ptr;
				ptr++;
				IhmuEvMIkePzkzUrPwYQWmzXpvMn = *ptr;
				ptr++;
				holsvoqGVigUCHszGmGxQqrMlJDy = *ptr;
				ptr++;
				for (int l = 0; l < 2; l++)
				{
					XGgGAZfXScTdXttciBqistygdFhl[l] = *ptr;
					ptr++;
				}
				vECNnGSKAHaUzTVIHdEXhgkqAoMIA = *ptr;
				ptr++;
				dqlYxOpxldDzaQLDNuydVQKzjfKF = *ptr;
				ptr++;
				UlDFHnXDBxaQBETAawCAIffDMjCu = *ptr;
				ptr++;
				AgJwWpyrguHIoUjYNDqLcwpHodgV = *ptr;
				ptr++;
				lUOrbBkFnffvQwuqcghYboKogzigb = *ptr;
				ptr++;
				KyZkWSxabsGszXxBybUZaFwqjZmiA = *ptr;
				ptr++;
				for (int m = 0; m < 2; m++)
				{
					zuQtVqBImuPSVGGmKGomathZitrJ[m] = *ptr;
					ptr++;
				}
				qcnhDaOQflwjZMhseDPZOsdyCKkhA = *ptr;
				ptr++;
				ihqzPlWTimmKJLVScXuukQHniHdD = *ptr;
				ptr++;
				GghpcuUbLlYycQNfGyTgGECdJvMr = *ptr;
				ptr++;
				for (int n = 0; n < 2; n++)
				{
					cTTRLRyfatffqjzjJLnFebrwqdedA[n] = *ptr;
					ptr++;
				}
				tcrkijfItOlFqDMJQOEcmVndJaxt = *ptr;
				ptr++;
				IgNcDqBoIcfaQBDPvodaeppQuRVtA = *ptr;
				ptr++;
				UaDDerAEFKhrmDHbzFoCfgBBVsMrb = *ptr;
				ptr++;
				GdLDodAwqJfctiGlKCUMdpBEsmFs = *ptr;
				ptr++;
				zNQXPBsMoDJOGhqwPFXzhpoKekE = *ptr;
				ptr++;
				iCYsgtraoqfFFhgdyHTCqAywoBKk = *ptr;
				ptr++;
				int* ptr2 = (int*)((byte*)(void*)P_0._buffer + P_0.byteIndex_hatsStart);
				for (int num2 = 0; num2 < 2; num2++)
				{
					idJdbVGwyzvheplqsVSgULIJSMFHA[num2] = *ptr2;
					ptr2++;
				}
			}

			public unsafe static void atKsabNhsOffEfiiGBdFWghCqhxr(kwRPJuhGeGOOTxHiySsCucocjaNE P_0, double P_1, LowLevelInputEvent P_2)
			{
				int[] array = P_0.sLReTPhcHDujnRlhoWjmCzOVLNOFA;
				int[] array2 = P_0.VMigiindKyBQgLFlUizNSkZoVeGS;
				int[] array3 = P_0.IWMXxMWjRBYYCvsGIoxsRUUBZaH;
				int[] array4 = P_0.uRzpIYdZVItVnpMTmqAWgfYCEwco;
				int[] array5 = P_0.rEJcHQjvgxJeFfTvoEeLyGYVHsWP;
				*(double*)((byte*)(void*)P_2._buffer + 4) = P_1;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				for (int i = 0; i < 128; i++)
				{
					if (P_0.CkhppmbBimKbGpQGgBPruJsQICQd[i])
					{
						num |= 1 << num3;
					}
					num3++;
					if (num3 == 32)
					{
						*(int*)((byte*)(void*)P_2._buffer + P_2.byteIndex_buttonsStart + num2 * 4) = num;
						num3 = 0;
						num = 0;
						num2++;
					}
				}
				float* ptr = (float*)((byte*)(void*)P_2._buffer + P_2.byteIndex_axesStart);
				for (int j = 0; j < 2; j++)
				{
					*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(array2[j]);
					ptr++;
				}
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.lYBcyRYSHQIEAVVRiIAEoILgGQjfA);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.knOJBucJwpaiJguvaktiHssSwQce);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.gtBEqzqDRGqBjyvOCAfkZbputzNI);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.vavGCkeNxGiaFYRlfykNpeWqQefoA);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.qtMIcLlEGmnjeXqCQDqEeuokHhHh);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.mBgfZrbiCscPuDzWDLswiBTrinVR);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.vGheOmKMqvcmIcYFAXWeiQMYxsPfb);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.ymsdogeiGGYEBgsDxEPxeQVeHCOjB);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.djlGynKiFuEaeXACJaLReDyxcVMoA);
				ptr++;
				for (int k = 0; k < 2; k++)
				{
					*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(array3[k]);
					ptr++;
				}
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.PiHqOxPNhXdPWjlEmHGxAYiIrQVE);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.hmOhybbljMMblKHLwhLjHHdIVVOC);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.ParClsmiGwiJOyXFbgqFfHhYljY);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.MHmeaCgmxlNobcbbPPITbdWBXKQi);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.UpxRgEicuEQvDPxFXviqGQTcKlnJ);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.DUDmrSoEuvtnGjauGPYMCTGOIjqS);
				ptr++;
				for (int l = 0; l < 2; l++)
				{
					*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(array4[l]);
					ptr++;
				}
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.qbfNpNNHQaPYKFbtJOLChHcXckUc);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.FGVflzDYmZzDDBfmykVrZCbLAGDLA);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.VBLGxvvyXDqaLvtDvHcAKrzXMXZd);
				ptr++;
				for (int m = 0; m < 2; m++)
				{
					*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(array5[m]);
					ptr++;
				}
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.LVYKSfpyeNCFfQKMUaSMwgIZAtoz);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.jFgnZxRAbfleqtcyUIWCNFdtLJsh);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.sNsitztzeACpUZMEJedTeTXkidWFb);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.mWFvViBDEHHBtuTrQQkzQhcMeOOfA);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.JeQTyNhGBwbmcxlXcihSQjoiiWFY);
				ptr++;
				*ptr = NDtQnUBddQnbLoxniWvjRgsOeAhN(P_0.OIYSEzHkHxXvwrFZDVAQDecWqfUQ);
				ptr++;
				int* ptr2 = (int*)((byte*)(void*)P_2._buffer + P_2.byteIndex_hatsStart);
				for (int n = 0; n < 2; n++)
				{
					*ptr2 = array[n];
					ptr2++;
				}
			}
		}

		private readonly int lnxvmJYGFowhBjEnAndQhObBWDkc;

		private readonly ButtonLoopSet jFXzBIPTasaRpvojGokTzjMbkGIp;

		private readonly DualThreadLowLevelInputEventQueue TatcBdJdSHZhGNTfvSATzpEDTEYGA;

		private fVAezyXQMYSIrtqcKaZCSvRvCGQfA kVDCQvsobNiJyOFIgLinGXJqiLio;

		private readonly kwRPJuhGeGOOTxHiySsCucocjaNE bIkWWkXHrNpOmgjCshcgcyIHdqqq;

		private readonly kwRPJuhGeGOOTxHiySsCucocjaNE DTsYyXzydmFVOWyqIHLjzOpmEqTX;

		private readonly object emOPFsMqLWsyKdDbZJerxWqxCmuO;

		private bool OysnaOACwmOxMzVhxUsDrMxzEHWj;

		public readonly YOgNgQpZZfYcTITAqIaepzvNafxe mBNCpNkMAbDKlnnmLbDDTOnprebgA;

		private readonly WMFXrjibkepVHxKQaTfxIGFUNKMb MiCQsbzmWgAzKjCyXVIRCyKguZKe;

		private bool pedQXdpjNJnZrGKaEtZGltFFBfeC;

		public bool[] jdNIHJlklkDayRPXMJnFTtoWFOLE => jFXzBIPTasaRpvojGokTzjMbkGIp.Current.effectiveValue;

		public WMFXrjibkepVHxKQaTfxIGFUNKMb WgMNPYeANHtPwGfMOltdVTSjdtfY => MiCQsbzmWgAzKjCyXVIRCyKguZKe;

		public WtFyEJeQdSfjObjirEdLChzyqzFpA(YOgNgQpZZfYcTITAqIaepzvNafxe P_0, UpdateLoopSetting P_1)
		{
			mBNCpNkMAbDKlnnmLbDDTOnprebgA = P_0;
			lnxvmJYGFowhBjEnAndQhObBWDkc = P_0.hQXfGyIqaNQWNbVQpOFYUdyDQeXoA.aNtvJKNgBcHUcHzbkEovCQIqXZVo;
			jFXzBIPTasaRpvojGokTzjMbkGIp = new ButtonLoopSet(P_1, lnxvmJYGFowhBjEnAndQhObBWDkc);
			TatcBdJdSHZhGNTfvSATzpEDTEYGA = new DualThreadLowLevelInputEventQueue((int)((float)FwvuhjisMNfwRNPCnXxbQzkrWKy.VjeFpcjaFerIirGHKspqAQDAgjGxA * 0.25f), 128, 32, 2);
			MiCQsbzmWgAzKjCyXVIRCyKguZKe = new WMFXrjibkepVHxKQaTfxIGFUNKMb();
			bIkWWkXHrNpOmgjCshcgcyIHdqqq = new kwRPJuhGeGOOTxHiySsCucocjaNE();
			DTsYyXzydmFVOWyqIHLjzOpmEqTX = new kwRPJuhGeGOOTxHiySsCucocjaNE();
			emOPFsMqLWsyKdDbZJerxWqxCmuO = new object();
			if (FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf != null)
			{
				FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf.ThreadUpdateEvent += kXdkYNPzlFCyyfREBygvqEXJpGJm;
			}
		}

		public void LALLSsvfLLrdgebywGNQunRNIbuD()
		{
			jFXzBIPTasaRpvojGokTzjMbkGIp.SetUpdateLoop(ReInput.currentUpdateLoop);
			DpqqvxwLBJSFepcJZRDZirYMuHHR();
		}

		public void dKlTIDkVOArtGJIBgeJKspeILsEx()
		{
			jFXzBIPTasaRpvojGokTzjMbkGIp.Current.ClearWasTrueThisFrame();
		}

		public void cYCctDtCkrKlUSRcccFdNgORGRhjA()
		{
			wHhaaudzRJUvIGAaMmHLDOLFFiTAc();
			OysnaOACwmOxMzVhxUsDrMxzEHWj = true;
		}

		public void NOCpfTEtlATtnIPWxVaaMvfFsrRx()
		{
			OysnaOACwmOxMzVhxUsDrMxzEHWj = false;
			wHhaaudzRJUvIGAaMmHLDOLFFiTAc();
		}

		public void wDYwVmcoElvdjsyRWuLigIoYRmAt(WtFyEJeQdSfjObjirEdLChzyqzFpA P_0)
		{
			if (P_0 == null || P_0 == this || P_0.lnxvmJYGFowhBjEnAndQhObBWDkc != lnxvmJYGFowhBjEnAndQhObBWDkc)
			{
				return;
			}
			_ = ReInput.realTime;
			lock (emOPFsMqLWsyKdDbZJerxWqxCmuO)
			{
				lock (P_0.emOPFsMqLWsyKdDbZJerxWqxCmuO)
				{
					jFXzBIPTasaRpvojGokTzjMbkGIp.Import(P_0.jFXzBIPTasaRpvojGokTzjMbkGIp);
					MiCQsbzmWgAzKjCyXVIRCyKguZKe.ycvbQuRBohIKMnipANEpQhdkTULB(P_0.MiCQsbzmWgAzKjCyXVIRCyKguZKe);
					bIkWWkXHrNpOmgjCshcgcyIHdqqq.WWXFExClbcyqcaMXcTIdrcsagQiq(P_0.bIkWWkXHrNpOmgjCshcgcyIHdqqq);
					DTsYyXzydmFVOWyqIHLjzOpmEqTX.WWXFExClbcyqcaMXcTIdrcsagQiq(P_0.DTsYyXzydmFVOWyqIHLjzOpmEqTX);
					TatcBdJdSHZhGNTfvSATzpEDTEYGA.ImportAll(P_0.TatcBdJdSHZhGNTfvSATzpEDTEYGA);
					kVDCQvsobNiJyOFIgLinGXJqiLio = fVAezyXQMYSIrtqcKaZCSvRvCGQfA.boDlVYXLAajbHhvdZqAxzkjmVvRuA(P_0.kVDCQvsobNiJyOFIgLinGXJqiLio, bIkWWkXHrNpOmgjCshcgcyIHdqqq);
					OysnaOACwmOxMzVhxUsDrMxzEHWj = P_0.OysnaOACwmOxMzVhxUsDrMxzEHWj;
				}
			}
		}

		public void BMiEzUbSUwtFNyQAPrjCihHYJBk(int P_0, int P_1, int P_2, float P_3)
		{
			lock (emOPFsMqLWsyKdDbZJerxWqxCmuO)
			{
				kVDCQvsobNiJyOFIgLinGXJqiLio = new fVAezyXQMYSIrtqcKaZCSvRvCGQfA(bIkWWkXHrNpOmgjCshcgcyIHdqqq, P_0, P_1, P_2, P_3);
			}
		}

		private void kXdkYNPzlFCyyfREBygvqEXJpGJm()
		{
			if (!OysnaOACwmOxMzVhxUsDrMxzEHWj)
			{
				return;
			}
			double realTime;
			try
			{
				mBNCpNkMAbDKlnnmLbDDTOnprebgA.CPzpQOSfMBjaUAymHmyAJivJMMCnA(bIkWWkXHrNpOmgjCshcgcyIHdqqq);
				realTime = ReInput.realTime;
			}
			catch
			{
				return;
			}
			lock (emOPFsMqLWsyKdDbZJerxWqxCmuO)
			{
				if (kVDCQvsobNiJyOFIgLinGXJqiLio != null)
				{
					kVDCQvsobNiJyOFIgLinGXJqiLio.sOnJqOLfLUodsclhoMydUFaGaJcE(realTime);
				}
				if (!bIkWWkXHrNpOmgjCshcgcyIHdqqq.gybgcpiDKgfjgEyFuVIEcfvRnyMcb(DTsYyXzydmFVOWyqIHLjzOpmEqTX))
				{
					using (DualThreadLowLevelInputEventQueue.INewEventWrapper newEventWrapper = TatcBdJdSHZhGNTfvSATzpEDTEYGA.T_CreateEvent())
					{
						WMFXrjibkepVHxKQaTfxIGFUNKMb.atKsabNhsOffEfiiGBdFWghCqhxr(bIkWWkXHrNpOmgjCshcgcyIHdqqq, realTime, newEventWrapper.Event);
					}
					DTsYyXzydmFVOWyqIHLjzOpmEqTX.WWXFExClbcyqcaMXcTIdrcsagQiq(bIkWWkXHrNpOmgjCshcgcyIHdqqq);
				}
			}
		}

		private void DpqqvxwLBJSFepcJZRDZirYMuHHR()
		{
			while (TatcBdJdSHZhGNTfvSATzpEDTEYGA.ProcessNewEvents())
			{
				MiCQsbzmWgAzKjCyXVIRCyKguZKe.uZsxeGoNsVKpswdSuBOJPCQmNIPi(ref TatcBdJdSHZhGNTfvSATzpEDTEYGA.currentEvent);
				for (int i = 0; i < lnxvmJYGFowhBjEnAndQhObBWDkc; i++)
				{
					jFXzBIPTasaRpvojGokTzjMbkGIp.SetValue(i, MiCQsbzmWgAzKjCyXVIRCyKguZKe.QXvEhzIeFDPLSSuwuoZWEtoAqifTb[i], TatcBdJdSHZhGNTfvSATzpEDTEYGA.currentEvent.GetTimestamp());
				}
			}
		}

		private void wHhaaudzRJUvIGAaMmHLDOLFFiTAc()
		{
			MiCQsbzmWgAzKjCyXVIRCyKguZKe.GnCARrOsnVmBWvePPOlIJfLilVio();
			lock (emOPFsMqLWsyKdDbZJerxWqxCmuO)
			{
				bIkWWkXHrNpOmgjCshcgcyIHdqqq.JKhOIWdZjuHyJheVCblHSCpsylJs();
				DTsYyXzydmFVOWyqIHLjzOpmEqTX.JKhOIWdZjuHyJheVCblHSCpsylJs();
				TatcBdJdSHZhGNTfvSATzpEDTEYGA.Clear();
			}
			jFXzBIPTasaRpvojGokTzjMbkGIp.Clear();
		}

		public void Dispose()
		{
			tcyujIwfbxYojzFKTAPiXRYbUtdR(true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		protected virtual void kfnVcWeRYLSBFHAlNvDfHUlZCfqx()
		{
			try
			{
				tcyujIwfbxYojzFKTAPiXRYbUtdR(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void tcyujIwfbxYojzFKTAPiXRYbUtdR(bool P_0)
		{
			if (!pedQXdpjNJnZrGKaEtZGltFFBfeC)
			{
				if (P_0)
				{
					NOCpfTEtlATtnIPWxVaaMvfFsrRx();
					TatcBdJdSHZhGNTfvSATzpEDTEYGA.Dispose();
				}
				if (FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf != null)
				{
					FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf.ThreadUpdateEvent -= kXdkYNPzlFCyyfREBygvqEXJpGJm;
				}
				pedQXdpjNJnZrGKaEtZGltFFBfeC = true;
			}
		}

		private static float NDtQnUBddQnbLoxniWvjRgsOeAhN(int P_0)
		{
			if (P_0 == 0)
			{
				return 0f;
			}
			return MathTools.Clamp((float)MathTools.Abs(P_0) / 65535f * (float)MathTools.Sign(P_0), -1f, 1f);
		}
	}

	private class fVAezyXQMYSIrtqcKaZCSvRvCGQfA
	{
		private kwRPJuhGeGOOTxHiySsCucocjaNE lGkuHywmOBfvfBELRGVSPdYGROhE;

		private AExlrnskzCOIjXArOidBzGBJtTNN nsAIpCaHhDaXnaWDHvBEruFSCcyh;

		private int qFPrzGulRrabusoHNfqkICbohkrE;

		private int kZChlkdnyYCqZsthTVlpfEUJGqiw;

		private int fjSkHhmpkHzfIRDOCKYUdCQVuker;

		private float DytEFljVfaENnnwYpbWorOwThrEvA;

		public static fVAezyXQMYSIrtqcKaZCSvRvCGQfA boDlVYXLAajbHhvdZqAxzkjmVvRuA(fVAezyXQMYSIrtqcKaZCSvRvCGQfA P_0, kwRPJuhGeGOOTxHiySsCucocjaNE P_1)
		{
			if (P_0 == null || P_1 == null)
			{
				return null;
			}
			return new fVAezyXQMYSIrtqcKaZCSvRvCGQfA(P_0, P_1);
		}

		public fVAezyXQMYSIrtqcKaZCSvRvCGQfA(kwRPJuhGeGOOTxHiySsCucocjaNE P_0, int P_1, int P_2, int P_3, float P_4)
			: this(P_1, P_2, P_3, P_4)
		{
			nsAIpCaHhDaXnaWDHvBEruFSCcyh = new AExlrnskzCOIjXArOidBzGBJtTNN(P_0);
			lGkuHywmOBfvfBELRGVSPdYGROhE = new kwRPJuhGeGOOTxHiySsCucocjaNE();
		}

		private fVAezyXQMYSIrtqcKaZCSvRvCGQfA(fVAezyXQMYSIrtqcKaZCSvRvCGQfA P_0, kwRPJuhGeGOOTxHiySsCucocjaNE P_1)
			: this(P_1, P_0.qFPrzGulRrabusoHNfqkICbohkrE, P_0.kZChlkdnyYCqZsthTVlpfEUJGqiw, P_0.fjSkHhmpkHzfIRDOCKYUdCQVuker, P_0.DytEFljVfaENnnwYpbWorOwThrEvA)
		{
			ymvPQgVCkhdyITpYQHOuFRvRtLEc(P_0);
		}

		private fVAezyXQMYSIrtqcKaZCSvRvCGQfA(int P_0, int P_1, int P_2, float P_3)
		{
			qFPrzGulRrabusoHNfqkICbohkrE = P_0;
			kZChlkdnyYCqZsthTVlpfEUJGqiw = P_1;
			fjSkHhmpkHzfIRDOCKYUdCQVuker = P_2;
			DytEFljVfaENnnwYpbWorOwThrEvA = P_3;
		}

		public void sOnJqOLfLUodsclhoMydUFaGaJcE(double P_0)
		{
			nsAIpCaHhDaXnaWDHvBEruFSCcyh.cssHFvtCTbbKAmItlAWOcufXjCcS(P_0);
			if (!nsAIpCaHhDaXnaWDHvBEruFSCcyh.aDIiUUfBHETLFQbOZpKdbFefAIFYb)
			{
				if (P_0 >= nsAIpCaHhDaXnaWDHvBEruFSCcyh.lRPQsvHJIAWcSMRxrgQgFOOHRXBr + (double)DytEFljVfaENnnwYpbWorOwThrEvA)
				{
					lGkuHywmOBfvfBELRGVSPdYGROhE.JKhOIWdZjuHyJheVCblHSCpsylJs();
				}
				return;
			}
			kwRPJuhGeGOOTxHiySsCucocjaNE kwRPJuhGeGOOTxHiySsCucocjaNE2 = nsAIpCaHhDaXnaWDHvBEruFSCcyh.cyocRHefHQOnHhgRWMKniGlrecUr;
			kwRPJuhGeGOOTxHiySsCucocjaNE kwRPJuhGeGOOTxHiySsCucocjaNE3 = nsAIpCaHhDaXnaWDHvBEruFSCcyh.HwkvntgdQOzsuvCiBUOLRthqvcpl;
			lGkuHywmOBfvfBELRGVSPdYGROhE.mWFvViBDEHHBtuTrQQkzQhcMeOOfA = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.mWFvViBDEHHBtuTrQQkzQhcMeOOfA);
			lGkuHywmOBfvfBELRGVSPdYGROhE.JeQTyNhGBwbmcxlXcihSQjoiiWFY = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.JeQTyNhGBwbmcxlXcihSQjoiiWFY);
			lGkuHywmOBfvfBELRGVSPdYGROhE.OIYSEzHkHxXvwrFZDVAQDecWqfUQ = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.OIYSEzHkHxXvwrFZDVAQDecWqfUQ);
			lGkuHywmOBfvfBELRGVSPdYGROhE.MHmeaCgmxlNobcbbPPITbdWBXKQi = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.MHmeaCgmxlNobcbbPPITbdWBXKQi);
			lGkuHywmOBfvfBELRGVSPdYGROhE.UpxRgEicuEQvDPxFXviqGQTcKlnJ = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.UpxRgEicuEQvDPxFXviqGQTcKlnJ);
			lGkuHywmOBfvfBELRGVSPdYGROhE.DUDmrSoEuvtnGjauGPYMCTGOIjqS = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.DUDmrSoEuvtnGjauGPYMCTGOIjqS);
			for (int i = 0; i < lGkuHywmOBfvfBELRGVSPdYGROhE.uRzpIYdZVItVnpMTmqAWgfYCEwco.Length; i++)
			{
				lGkuHywmOBfvfBELRGVSPdYGROhE.uRzpIYdZVItVnpMTmqAWgfYCEwco[i] = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.uRzpIYdZVItVnpMTmqAWgfYCEwco[i]);
			}
			for (int j = 0; j < lGkuHywmOBfvfBELRGVSPdYGROhE.sLReTPhcHDujnRlhoWjmCzOVLNOFA.Length; j++)
			{
				lGkuHywmOBfvfBELRGVSPdYGROhE.sLReTPhcHDujnRlhoWjmCzOVLNOFA[j] = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.sLReTPhcHDujnRlhoWjmCzOVLNOFA[j]);
			}
			for (int k = 0; k < lGkuHywmOBfvfBELRGVSPdYGROhE.CkhppmbBimKbGpQGgBPruJsQICQd.Length; k++)
			{
				lGkuHywmOBfvfBELRGVSPdYGROhE.CkhppmbBimKbGpQGgBPruJsQICQd[k] = kwRPJuhGeGOOTxHiySsCucocjaNE3.CkhppmbBimKbGpQGgBPruJsQICQd[k];
			}
			lGkuHywmOBfvfBELRGVSPdYGROhE.LVYKSfpyeNCFfQKMUaSMwgIZAtoz = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.LVYKSfpyeNCFfQKMUaSMwgIZAtoz);
			lGkuHywmOBfvfBELRGVSPdYGROhE.jFgnZxRAbfleqtcyUIWCNFdtLJsh = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.jFgnZxRAbfleqtcyUIWCNFdtLJsh);
			lGkuHywmOBfvfBELRGVSPdYGROhE.sNsitztzeACpUZMEJedTeTXkidWFb = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.sNsitztzeACpUZMEJedTeTXkidWFb);
			lGkuHywmOBfvfBELRGVSPdYGROhE.vGheOmKMqvcmIcYFAXWeiQMYxsPfb = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.vGheOmKMqvcmIcYFAXWeiQMYxsPfb);
			lGkuHywmOBfvfBELRGVSPdYGROhE.ymsdogeiGGYEBgsDxEPxeQVeHCOjB = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.ymsdogeiGGYEBgsDxEPxeQVeHCOjB);
			lGkuHywmOBfvfBELRGVSPdYGROhE.djlGynKiFuEaeXACJaLReDyxcVMoA = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.djlGynKiFuEaeXACJaLReDyxcVMoA);
			for (int l = 0; l < lGkuHywmOBfvfBELRGVSPdYGROhE.rEJcHQjvgxJeFfTvoEeLyGYVHsWP.Length; l++)
			{
				lGkuHywmOBfvfBELRGVSPdYGROhE.rEJcHQjvgxJeFfTvoEeLyGYVHsWP[l] = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.rEJcHQjvgxJeFfTvoEeLyGYVHsWP[l]);
			}
			lGkuHywmOBfvfBELRGVSPdYGROhE.lYBcyRYSHQIEAVVRiIAEoILgGQjfA = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.lYBcyRYSHQIEAVVRiIAEoILgGQjfA);
			lGkuHywmOBfvfBELRGVSPdYGROhE.knOJBucJwpaiJguvaktiHssSwQce = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.knOJBucJwpaiJguvaktiHssSwQce);
			lGkuHywmOBfvfBELRGVSPdYGROhE.gtBEqzqDRGqBjyvOCAfkZbputzNI = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.gtBEqzqDRGqBjyvOCAfkZbputzNI);
			lGkuHywmOBfvfBELRGVSPdYGROhE.vavGCkeNxGiaFYRlfykNpeWqQefoA = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.vavGCkeNxGiaFYRlfykNpeWqQefoA);
			lGkuHywmOBfvfBELRGVSPdYGROhE.qtMIcLlEGmnjeXqCQDqEeuokHhHh = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.qtMIcLlEGmnjeXqCQDqEeuokHhHh);
			lGkuHywmOBfvfBELRGVSPdYGROhE.mBgfZrbiCscPuDzWDLswiBTrinVR = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.mBgfZrbiCscPuDzWDLswiBTrinVR);
			for (int m = 0; m < lGkuHywmOBfvfBELRGVSPdYGROhE.VMigiindKyBQgLFlUizNSkZoVeGS.Length; m++)
			{
				lGkuHywmOBfvfBELRGVSPdYGROhE.VMigiindKyBQgLFlUizNSkZoVeGS[m] = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.VMigiindKyBQgLFlUizNSkZoVeGS[m]);
			}
			lGkuHywmOBfvfBELRGVSPdYGROhE.PiHqOxPNhXdPWjlEmHGxAYiIrQVE = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.PiHqOxPNhXdPWjlEmHGxAYiIrQVE);
			lGkuHywmOBfvfBELRGVSPdYGROhE.hmOhybbljMMblKHLwhLjHHdIVVOC = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.hmOhybbljMMblKHLwhLjHHdIVVOC);
			lGkuHywmOBfvfBELRGVSPdYGROhE.ParClsmiGwiJOyXFbgqFfHhYljY = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.ParClsmiGwiJOyXFbgqFfHhYljY);
			lGkuHywmOBfvfBELRGVSPdYGROhE.qbfNpNNHQaPYKFbtJOLChHcXckUc = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.qbfNpNNHQaPYKFbtJOLChHcXckUc);
			lGkuHywmOBfvfBELRGVSPdYGROhE.FGVflzDYmZzDDBfmykVrZCbLAGDLA = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.FGVflzDYmZzDDBfmykVrZCbLAGDLA);
			lGkuHywmOBfvfBELRGVSPdYGROhE.VBLGxvvyXDqaLvtDvHcAKrzXMXZd = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.VBLGxvvyXDqaLvtDvHcAKrzXMXZd);
			for (int n = 0; n < lGkuHywmOBfvfBELRGVSPdYGROhE.IWMXxMWjRBYYCvsGIoxsRUUBZaH.Length; n++)
			{
				lGkuHywmOBfvfBELRGVSPdYGROhE.IWMXxMWjRBYYCvsGIoxsRUUBZaH[n] = bavydbttvjvLIgECbCfqOlxeAEwJ(kwRPJuhGeGOOTxHiySsCucocjaNE2.IWMXxMWjRBYYCvsGIoxsRUUBZaH[n]);
			}
		}

		public void ymvPQgVCkhdyITpYQHOuFRvRtLEc(fVAezyXQMYSIrtqcKaZCSvRvCGQfA P_0)
		{
			lGkuHywmOBfvfBELRGVSPdYGROhE.WWXFExClbcyqcaMXcTIdrcsagQiq(P_0.lGkuHywmOBfvfBELRGVSPdYGROhE);
			nsAIpCaHhDaXnaWDHvBEruFSCcyh.NZVCrAkHzrCfqJbzQcchGVqfVbjGb(P_0.nsAIpCaHhDaXnaWDHvBEruFSCcyh);
			qFPrzGulRrabusoHNfqkICbohkrE = P_0.qFPrzGulRrabusoHNfqkICbohkrE;
			kZChlkdnyYCqZsthTVlpfEUJGqiw = P_0.kZChlkdnyYCqZsthTVlpfEUJGqiw;
			fjSkHhmpkHzfIRDOCKYUdCQVuker = P_0.fjSkHhmpkHzfIRDOCKYUdCQVuker;
			DytEFljVfaENnnwYpbWorOwThrEvA = P_0.DytEFljVfaENnnwYpbWorOwThrEvA;
		}

		private int bavydbttvjvLIgECbCfqOlxeAEwJ(int P_0)
		{
			return MathTools.ValueInNewRange(P_0, qFPrzGulRrabusoHNfqkICbohkrE, kZChlkdnyYCqZsthTVlpfEUJGqiw, -65535, 65535);
		}
	}

	private class AExlrnskzCOIjXArOidBzGBJtTNN
	{
		private double zNNuNtqBzPTMmfCOLrWsrGncgIID;

		private kwRPJuhGeGOOTxHiySsCucocjaNE xCDSpMsPjvrNGLfxZtMDHocghvRC;

		private kwRPJuhGeGOOTxHiySsCucocjaNE sgwBfufAYQEoIPpsQPyQZIqMwNiD;

		private kwRPJuhGeGOOTxHiySsCucocjaNE AMLUzJctoHjfaYsUQCrCKqJWLNDY;

		private bool VjDaFUIxHcMmkPFMWheEZbobkjJeA;

		private double XPKevfjaAphCMYsTunLTlPbrArZP;

		public kwRPJuhGeGOOTxHiySsCucocjaNE HwkvntgdQOzsuvCiBUOLRthqvcpl => xCDSpMsPjvrNGLfxZtMDHocghvRC;

		public kwRPJuhGeGOOTxHiySsCucocjaNE cyocRHefHQOnHhgRWMKniGlrecUr => AMLUzJctoHjfaYsUQCrCKqJWLNDY;

		public bool aDIiUUfBHETLFQbOZpKdbFefAIFYb => VjDaFUIxHcMmkPFMWheEZbobkjJeA;

		public double lRPQsvHJIAWcSMRxrgQgFOOHRXBr => XPKevfjaAphCMYsTunLTlPbrArZP;

		public AExlrnskzCOIjXArOidBzGBJtTNN(kwRPJuhGeGOOTxHiySsCucocjaNE P_0)
		{
			xCDSpMsPjvrNGLfxZtMDHocghvRC = P_0;
			sgwBfufAYQEoIPpsQPyQZIqMwNiD = new kwRPJuhGeGOOTxHiySsCucocjaNE();
			AMLUzJctoHjfaYsUQCrCKqJWLNDY = new kwRPJuhGeGOOTxHiySsCucocjaNE();
		}

		public void cssHFvtCTbbKAmItlAWOcufXjCcS(double P_0)
		{
			zNNuNtqBzPTMmfCOLrWsrGncgIID = P_0;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.mWFvViBDEHHBtuTrQQkzQhcMeOOfA = xCDSpMsPjvrNGLfxZtMDHocghvRC.mWFvViBDEHHBtuTrQQkzQhcMeOOfA - sgwBfufAYQEoIPpsQPyQZIqMwNiD.mWFvViBDEHHBtuTrQQkzQhcMeOOfA;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.JeQTyNhGBwbmcxlXcihSQjoiiWFY = xCDSpMsPjvrNGLfxZtMDHocghvRC.JeQTyNhGBwbmcxlXcihSQjoiiWFY - sgwBfufAYQEoIPpsQPyQZIqMwNiD.JeQTyNhGBwbmcxlXcihSQjoiiWFY;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.OIYSEzHkHxXvwrFZDVAQDecWqfUQ = xCDSpMsPjvrNGLfxZtMDHocghvRC.OIYSEzHkHxXvwrFZDVAQDecWqfUQ - sgwBfufAYQEoIPpsQPyQZIqMwNiD.OIYSEzHkHxXvwrFZDVAQDecWqfUQ;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.MHmeaCgmxlNobcbbPPITbdWBXKQi = xCDSpMsPjvrNGLfxZtMDHocghvRC.MHmeaCgmxlNobcbbPPITbdWBXKQi - sgwBfufAYQEoIPpsQPyQZIqMwNiD.MHmeaCgmxlNobcbbPPITbdWBXKQi;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.UpxRgEicuEQvDPxFXviqGQTcKlnJ = xCDSpMsPjvrNGLfxZtMDHocghvRC.UpxRgEicuEQvDPxFXviqGQTcKlnJ - sgwBfufAYQEoIPpsQPyQZIqMwNiD.UpxRgEicuEQvDPxFXviqGQTcKlnJ;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.DUDmrSoEuvtnGjauGPYMCTGOIjqS = xCDSpMsPjvrNGLfxZtMDHocghvRC.DUDmrSoEuvtnGjauGPYMCTGOIjqS - sgwBfufAYQEoIPpsQPyQZIqMwNiD.DUDmrSoEuvtnGjauGPYMCTGOIjqS;
			for (int i = 0; i < xCDSpMsPjvrNGLfxZtMDHocghvRC.uRzpIYdZVItVnpMTmqAWgfYCEwco.Length; i++)
			{
				AMLUzJctoHjfaYsUQCrCKqJWLNDY.uRzpIYdZVItVnpMTmqAWgfYCEwco[i] = xCDSpMsPjvrNGLfxZtMDHocghvRC.uRzpIYdZVItVnpMTmqAWgfYCEwco[i] - sgwBfufAYQEoIPpsQPyQZIqMwNiD.uRzpIYdZVItVnpMTmqAWgfYCEwco[i];
			}
			for (int j = 0; j < xCDSpMsPjvrNGLfxZtMDHocghvRC.sLReTPhcHDujnRlhoWjmCzOVLNOFA.Length; j++)
			{
				AMLUzJctoHjfaYsUQCrCKqJWLNDY.sLReTPhcHDujnRlhoWjmCzOVLNOFA[j] = xCDSpMsPjvrNGLfxZtMDHocghvRC.sLReTPhcHDujnRlhoWjmCzOVLNOFA[j] - sgwBfufAYQEoIPpsQPyQZIqMwNiD.sLReTPhcHDujnRlhoWjmCzOVLNOFA[j];
			}
			for (int k = 0; k < xCDSpMsPjvrNGLfxZtMDHocghvRC.CkhppmbBimKbGpQGgBPruJsQICQd.Length; k++)
			{
				AMLUzJctoHjfaYsUQCrCKqJWLNDY.CkhppmbBimKbGpQGgBPruJsQICQd[k] = xCDSpMsPjvrNGLfxZtMDHocghvRC.CkhppmbBimKbGpQGgBPruJsQICQd[k] != sgwBfufAYQEoIPpsQPyQZIqMwNiD.CkhppmbBimKbGpQGgBPruJsQICQd[k];
			}
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.LVYKSfpyeNCFfQKMUaSMwgIZAtoz = xCDSpMsPjvrNGLfxZtMDHocghvRC.LVYKSfpyeNCFfQKMUaSMwgIZAtoz - sgwBfufAYQEoIPpsQPyQZIqMwNiD.LVYKSfpyeNCFfQKMUaSMwgIZAtoz;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.jFgnZxRAbfleqtcyUIWCNFdtLJsh = xCDSpMsPjvrNGLfxZtMDHocghvRC.jFgnZxRAbfleqtcyUIWCNFdtLJsh - sgwBfufAYQEoIPpsQPyQZIqMwNiD.jFgnZxRAbfleqtcyUIWCNFdtLJsh;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.sNsitztzeACpUZMEJedTeTXkidWFb = xCDSpMsPjvrNGLfxZtMDHocghvRC.sNsitztzeACpUZMEJedTeTXkidWFb - sgwBfufAYQEoIPpsQPyQZIqMwNiD.sNsitztzeACpUZMEJedTeTXkidWFb;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.vGheOmKMqvcmIcYFAXWeiQMYxsPfb = xCDSpMsPjvrNGLfxZtMDHocghvRC.vGheOmKMqvcmIcYFAXWeiQMYxsPfb - sgwBfufAYQEoIPpsQPyQZIqMwNiD.vGheOmKMqvcmIcYFAXWeiQMYxsPfb;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.ymsdogeiGGYEBgsDxEPxeQVeHCOjB = xCDSpMsPjvrNGLfxZtMDHocghvRC.ymsdogeiGGYEBgsDxEPxeQVeHCOjB - sgwBfufAYQEoIPpsQPyQZIqMwNiD.ymsdogeiGGYEBgsDxEPxeQVeHCOjB;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.djlGynKiFuEaeXACJaLReDyxcVMoA = xCDSpMsPjvrNGLfxZtMDHocghvRC.djlGynKiFuEaeXACJaLReDyxcVMoA - sgwBfufAYQEoIPpsQPyQZIqMwNiD.djlGynKiFuEaeXACJaLReDyxcVMoA;
			for (int l = 0; l < xCDSpMsPjvrNGLfxZtMDHocghvRC.rEJcHQjvgxJeFfTvoEeLyGYVHsWP.Length; l++)
			{
				AMLUzJctoHjfaYsUQCrCKqJWLNDY.rEJcHQjvgxJeFfTvoEeLyGYVHsWP[l] = xCDSpMsPjvrNGLfxZtMDHocghvRC.rEJcHQjvgxJeFfTvoEeLyGYVHsWP[l] - sgwBfufAYQEoIPpsQPyQZIqMwNiD.rEJcHQjvgxJeFfTvoEeLyGYVHsWP[l];
			}
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.lYBcyRYSHQIEAVVRiIAEoILgGQjfA = xCDSpMsPjvrNGLfxZtMDHocghvRC.lYBcyRYSHQIEAVVRiIAEoILgGQjfA - sgwBfufAYQEoIPpsQPyQZIqMwNiD.lYBcyRYSHQIEAVVRiIAEoILgGQjfA;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.knOJBucJwpaiJguvaktiHssSwQce = xCDSpMsPjvrNGLfxZtMDHocghvRC.knOJBucJwpaiJguvaktiHssSwQce - sgwBfufAYQEoIPpsQPyQZIqMwNiD.knOJBucJwpaiJguvaktiHssSwQce;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.gtBEqzqDRGqBjyvOCAfkZbputzNI = xCDSpMsPjvrNGLfxZtMDHocghvRC.gtBEqzqDRGqBjyvOCAfkZbputzNI - sgwBfufAYQEoIPpsQPyQZIqMwNiD.gtBEqzqDRGqBjyvOCAfkZbputzNI;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.vavGCkeNxGiaFYRlfykNpeWqQefoA = xCDSpMsPjvrNGLfxZtMDHocghvRC.vavGCkeNxGiaFYRlfykNpeWqQefoA - sgwBfufAYQEoIPpsQPyQZIqMwNiD.vavGCkeNxGiaFYRlfykNpeWqQefoA;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.qtMIcLlEGmnjeXqCQDqEeuokHhHh = xCDSpMsPjvrNGLfxZtMDHocghvRC.qtMIcLlEGmnjeXqCQDqEeuokHhHh - sgwBfufAYQEoIPpsQPyQZIqMwNiD.qtMIcLlEGmnjeXqCQDqEeuokHhHh;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.mBgfZrbiCscPuDzWDLswiBTrinVR = xCDSpMsPjvrNGLfxZtMDHocghvRC.mBgfZrbiCscPuDzWDLswiBTrinVR - sgwBfufAYQEoIPpsQPyQZIqMwNiD.mBgfZrbiCscPuDzWDLswiBTrinVR;
			for (int m = 0; m < xCDSpMsPjvrNGLfxZtMDHocghvRC.VMigiindKyBQgLFlUizNSkZoVeGS.Length; m++)
			{
				AMLUzJctoHjfaYsUQCrCKqJWLNDY.VMigiindKyBQgLFlUizNSkZoVeGS[m] = xCDSpMsPjvrNGLfxZtMDHocghvRC.VMigiindKyBQgLFlUizNSkZoVeGS[m] - sgwBfufAYQEoIPpsQPyQZIqMwNiD.VMigiindKyBQgLFlUizNSkZoVeGS[m];
			}
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.PiHqOxPNhXdPWjlEmHGxAYiIrQVE = xCDSpMsPjvrNGLfxZtMDHocghvRC.PiHqOxPNhXdPWjlEmHGxAYiIrQVE - sgwBfufAYQEoIPpsQPyQZIqMwNiD.PiHqOxPNhXdPWjlEmHGxAYiIrQVE;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.hmOhybbljMMblKHLwhLjHHdIVVOC = xCDSpMsPjvrNGLfxZtMDHocghvRC.hmOhybbljMMblKHLwhLjHHdIVVOC - sgwBfufAYQEoIPpsQPyQZIqMwNiD.hmOhybbljMMblKHLwhLjHHdIVVOC;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.ParClsmiGwiJOyXFbgqFfHhYljY = xCDSpMsPjvrNGLfxZtMDHocghvRC.ParClsmiGwiJOyXFbgqFfHhYljY - sgwBfufAYQEoIPpsQPyQZIqMwNiD.ParClsmiGwiJOyXFbgqFfHhYljY;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.qbfNpNNHQaPYKFbtJOLChHcXckUc = xCDSpMsPjvrNGLfxZtMDHocghvRC.qbfNpNNHQaPYKFbtJOLChHcXckUc - sgwBfufAYQEoIPpsQPyQZIqMwNiD.qbfNpNNHQaPYKFbtJOLChHcXckUc;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.FGVflzDYmZzDDBfmykVrZCbLAGDLA = xCDSpMsPjvrNGLfxZtMDHocghvRC.FGVflzDYmZzDDBfmykVrZCbLAGDLA - sgwBfufAYQEoIPpsQPyQZIqMwNiD.FGVflzDYmZzDDBfmykVrZCbLAGDLA;
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.VBLGxvvyXDqaLvtDvHcAKrzXMXZd = xCDSpMsPjvrNGLfxZtMDHocghvRC.VBLGxvvyXDqaLvtDvHcAKrzXMXZd - sgwBfufAYQEoIPpsQPyQZIqMwNiD.VBLGxvvyXDqaLvtDvHcAKrzXMXZd;
			for (int n = 0; n < xCDSpMsPjvrNGLfxZtMDHocghvRC.IWMXxMWjRBYYCvsGIoxsRUUBZaH.Length; n++)
			{
				AMLUzJctoHjfaYsUQCrCKqJWLNDY.IWMXxMWjRBYYCvsGIoxsRUUBZaH[n] = xCDSpMsPjvrNGLfxZtMDHocghvRC.IWMXxMWjRBYYCvsGIoxsRUUBZaH[n] - sgwBfufAYQEoIPpsQPyQZIqMwNiD.IWMXxMWjRBYYCvsGIoxsRUUBZaH[n];
			}
			VjDaFUIxHcMmkPFMWheEZbobkjJeA = DpKdSKJIZrtlvnBkBZLYetrLlKfIb();
			if (VjDaFUIxHcMmkPFMWheEZbobkjJeA)
			{
				XPKevfjaAphCMYsTunLTlPbrArZP = P_0;
				sgwBfufAYQEoIPpsQPyQZIqMwNiD.WWXFExClbcyqcaMXcTIdrcsagQiq(xCDSpMsPjvrNGLfxZtMDHocghvRC);
			}
		}

		public void NZVCrAkHzrCfqJbzQcchGVqfVbjGb(AExlrnskzCOIjXArOidBzGBJtTNN P_0)
		{
			zNNuNtqBzPTMmfCOLrWsrGncgIID = P_0.zNNuNtqBzPTMmfCOLrWsrGncgIID;
			sgwBfufAYQEoIPpsQPyQZIqMwNiD.WWXFExClbcyqcaMXcTIdrcsagQiq(P_0.sgwBfufAYQEoIPpsQPyQZIqMwNiD);
			AMLUzJctoHjfaYsUQCrCKqJWLNDY.WWXFExClbcyqcaMXcTIdrcsagQiq(P_0.AMLUzJctoHjfaYsUQCrCKqJWLNDY);
		}

		private bool DpKdSKJIZrtlvnBkBZLYetrLlKfIb()
		{
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.JeQTyNhGBwbmcxlXcihSQjoiiWFY != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.OIYSEzHkHxXvwrFZDVAQDecWqfUQ != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.MHmeaCgmxlNobcbbPPITbdWBXKQi != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.UpxRgEicuEQvDPxFXviqGQTcKlnJ != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.DUDmrSoEuvtnGjauGPYMCTGOIjqS != 0)
			{
				return true;
			}
			for (int i = 0; i < xCDSpMsPjvrNGLfxZtMDHocghvRC.uRzpIYdZVItVnpMTmqAWgfYCEwco.Length; i++)
			{
				if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.uRzpIYdZVItVnpMTmqAWgfYCEwco[i] != 0)
				{
					return true;
				}
			}
			for (int j = 0; j < xCDSpMsPjvrNGLfxZtMDHocghvRC.sLReTPhcHDujnRlhoWjmCzOVLNOFA.Length; j++)
			{
				if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.sLReTPhcHDujnRlhoWjmCzOVLNOFA[j] != 0)
				{
					return true;
				}
			}
			for (int k = 0; k < xCDSpMsPjvrNGLfxZtMDHocghvRC.CkhppmbBimKbGpQGgBPruJsQICQd.Length; k++)
			{
				if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.CkhppmbBimKbGpQGgBPruJsQICQd[k])
				{
					return true;
				}
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.LVYKSfpyeNCFfQKMUaSMwgIZAtoz != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.jFgnZxRAbfleqtcyUIWCNFdtLJsh != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.sNsitztzeACpUZMEJedTeTXkidWFb != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.vGheOmKMqvcmIcYFAXWeiQMYxsPfb != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.ymsdogeiGGYEBgsDxEPxeQVeHCOjB != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.djlGynKiFuEaeXACJaLReDyxcVMoA != 0)
			{
				return true;
			}
			for (int l = 0; l < xCDSpMsPjvrNGLfxZtMDHocghvRC.rEJcHQjvgxJeFfTvoEeLyGYVHsWP.Length; l++)
			{
				if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.rEJcHQjvgxJeFfTvoEeLyGYVHsWP[l] != 0)
				{
					return true;
				}
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.lYBcyRYSHQIEAVVRiIAEoILgGQjfA != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.knOJBucJwpaiJguvaktiHssSwQce != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.gtBEqzqDRGqBjyvOCAfkZbputzNI != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.vavGCkeNxGiaFYRlfykNpeWqQefoA != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.qtMIcLlEGmnjeXqCQDqEeuokHhHh != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.mBgfZrbiCscPuDzWDLswiBTrinVR != 0)
			{
				return true;
			}
			for (int m = 0; m < xCDSpMsPjvrNGLfxZtMDHocghvRC.VMigiindKyBQgLFlUizNSkZoVeGS.Length; m++)
			{
				AMLUzJctoHjfaYsUQCrCKqJWLNDY.VMigiindKyBQgLFlUizNSkZoVeGS[m] = xCDSpMsPjvrNGLfxZtMDHocghvRC.VMigiindKyBQgLFlUizNSkZoVeGS[m] - sgwBfufAYQEoIPpsQPyQZIqMwNiD.VMigiindKyBQgLFlUizNSkZoVeGS[m];
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.PiHqOxPNhXdPWjlEmHGxAYiIrQVE != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.hmOhybbljMMblKHLwhLjHHdIVVOC != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.ParClsmiGwiJOyXFbgqFfHhYljY != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.qbfNpNNHQaPYKFbtJOLChHcXckUc != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.FGVflzDYmZzDDBfmykVrZCbLAGDLA != 0)
			{
				return true;
			}
			if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.VBLGxvvyXDqaLvtDvHcAKrzXMXZd != 0)
			{
				return true;
			}
			for (int n = 0; n < xCDSpMsPjvrNGLfxZtMDHocghvRC.IWMXxMWjRBYYCvsGIoxsRUUBZaH.Length; n++)
			{
				if (AMLUzJctoHjfaYsUQCrCKqJWLNDY.IWMXxMWjRBYYCvsGIoxsRUUBZaH[n] != 0)
				{
					return true;
				}
			}
			return false;
		}
	}

	private class YqUaXflujHDbIipqDvDDYEWsdNuY
	{
		public enum aYHwOJJWObHJKEFfuAcKIpVJRLspA
		{
			Exact = 0,
			Approximate = 1
		}

		public class UeqtsQhdjnnfXWzWuGffLTLUBrEI
		{
			public int abGnoSlhVvlcPHaiayRjrdePRqID;

			public Guid sxCJNrMJeyHDwffjTqvXONizbpqAb;

			public Guid eKeopiYjqPiUsFCIMikNFnogfubm;

			public int rSFEtgvowLBxjOwhlGaaEPQDucuJA;

			public int gbNXDdwNoeeOQNsCOZNXtEDbCqXt;

			public int DIfWRszsewoYkBfwpVgUKvXbWOhd;

			public int avmexpcDFqLjzIspeZwvklpvacWVA;

			public bool dOFCuCSSRZTtmMKCtdNzFnAYGCuIA(eySnxgpDnPUzyhkGpTWqagWLjids P_0, aYHwOJJWObHJKEFfuAcKIpVJRLspA P_1)
			{
				if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == abGnoSlhVvlcPHaiayRjrdePRqID)
				{
					return true;
				}
				if (gbNXDdwNoeeOQNsCOZNXtEDbCqXt != P_0.hTjAsxCEdFIqVgGpcElosIWvJZtrB)
				{
					return false;
				}
				if (DIfWRszsewoYkBfwpVgUKvXbWOhd != P_0.biFqRTcWgotJCdYTKlrjBUiwvpLf)
				{
					return false;
				}
				if (avmexpcDFqLjzIspeZwvklpvacWVA != P_0.oHydTJYKdBErDKpaWkNlQajUmvjuA)
				{
					return false;
				}
				return P_1 switch
				{
					aYHwOJJWObHJKEFfuAcKIpVJRLspA.Exact => sxCJNrMJeyHDwffjTqvXONizbpqAb == P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid, 
					aYHwOJJWObHJKEFfuAcKIpVJRLspA.Approximate => eKeopiYjqPiUsFCIMikNFnogfubm == P_0.bPKUQQOuSxJXkBAetBaJbHwHPStDA, 
					_ => throw new NotImplementedException(), 
				};
			}

			public virtual string gaBGhrjuOjbIzMWCUrvoBunwGPfTA()
			{
				string text = "" + "rewiredId = " + abGnoSlhVvlcPHaiayRjrdePRqID + "\n";
				Guid guid = sxCJNrMJeyHDwffjTqvXONizbpqAb;
				string text2 = text + "instanceGuid = " + guid.ToString() + "\n";
				guid = eKeopiYjqPiUsFCIMikNFnogfubm;
				return string.Concat(string.Concat(string.Concat(string.Concat(text2 + "typeIdentifierGuid = " + guid.ToString() + "\n", "lastInputManagerId = ", rSFEtgvowLBxjOwhlGaaEPQDucuJA.ToString(), "\n"), "hardwareAxisCount = ", gbNXDdwNoeeOQNsCOZNXtEDbCqXt.ToString(), "\n"), "hardwareButtonCount = ", DIfWRszsewoYkBfwpVgUKvXbWOhd.ToString(), "\n"), "hardwareHatCount = ", avmexpcDFqLjzIspeZwvklpvacWVA.ToString(), "\n");
			}
		}

		private sealed class ydvnUwsLCHyFVhriJtflylTSukuD : IEnumerable<UeqtsQhdjnnfXWzWuGffLTLUBrEI>, IEnumerable, IEnumerator<UeqtsQhdjnnfXWzWuGffLTLUBrEI>, IEnumerator, IDisposable
		{
			private int qtbdZuOfJsqeivwvxqiWfDRZFxlc;

			private UeqtsQhdjnnfXWzWuGffLTLUBrEI iCcfrgAmnCumoTYUMdLhFbufuJNQB;

			private int xEjBWJnvfTPjsgGPaFsWHFxiCmDRA;

			public YqUaXflujHDbIipqDvDDYEWsdNuY NQRzWbixTnFrGVDrxTpRfWogeUcG;

			private eySnxgpDnPUzyhkGpTWqagWLjids rWFyPePNJHZLXCiiTzWpBLrYfvoM;

			public eySnxgpDnPUzyhkGpTWqagWLjids UoMSAPqVnNuGhuFuLwzTIQBOqEAs;

			private aYHwOJJWObHJKEFfuAcKIpVJRLspA gulHDHZvrphlxOYEnERJYQfkbsaY;

			public aYHwOJJWObHJKEFfuAcKIpVJRLspA bxUOOkagZsExhCTYnjzXdLkFOuczb;

			private int ptliEgWOePxQTYTIZaopOAtNwilv;

			private int IBnDksrbVKuHnTutFiPhNFpXoPWL;

			UeqtsQhdjnnfXWzWuGffLTLUBrEI IEnumerator<UeqtsQhdjnnfXWzWuGffLTLUBrEI>.Current
			{
				[DebuggerHidden]
				get
				{
					return iCcfrgAmnCumoTYUMdLhFbufuJNQB;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return iCcfrgAmnCumoTYUMdLhFbufuJNQB;
				}
			}

			[DebuggerHidden]
			public ydvnUwsLCHyFVhriJtflylTSukuD(int P_0)
			{
				qtbdZuOfJsqeivwvxqiWfDRZFxlc = P_0;
				xEjBWJnvfTPjsgGPaFsWHFxiCmDRA = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				qtbdZuOfJsqeivwvxqiWfDRZFxlc = -2;
			}

			private bool MoveNext()
			{
				int num = qtbdZuOfJsqeivwvxqiWfDRZFxlc;
				YqUaXflujHDbIipqDvDDYEWsdNuY nQRzWbixTnFrGVDrxTpRfWogeUcG = NQRzWbixTnFrGVDrxTpRfWogeUcG;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					qtbdZuOfJsqeivwvxqiWfDRZFxlc = -1;
					goto IL_0083;
				}
				qtbdZuOfJsqeivwvxqiWfDRZFxlc = -1;
				ptliEgWOePxQTYTIZaopOAtNwilv = nQRzWbixTnFrGVDrxTpRfWogeUcG.hMnFiFDFJOhEDyzIWEzfCgOnLjKD.Count;
				IBnDksrbVKuHnTutFiPhNFpXoPWL = 0;
				goto IL_0093;
				IL_0083:
				IBnDksrbVKuHnTutFiPhNFpXoPWL++;
				goto IL_0093;
				IL_0093:
				if (IBnDksrbVKuHnTutFiPhNFpXoPWL < ptliEgWOePxQTYTIZaopOAtNwilv)
				{
					if (nQRzWbixTnFrGVDrxTpRfWogeUcG.hMnFiFDFJOhEDyzIWEzfCgOnLjKD[IBnDksrbVKuHnTutFiPhNFpXoPWL].dOFCuCSSRZTtmMKCtdNzFnAYGCuIA(rWFyPePNJHZLXCiiTzWpBLrYfvoM, gulHDHZvrphlxOYEnERJYQfkbsaY))
					{
						iCcfrgAmnCumoTYUMdLhFbufuJNQB = nQRzWbixTnFrGVDrxTpRfWogeUcG.hMnFiFDFJOhEDyzIWEzfCgOnLjKD[IBnDksrbVKuHnTutFiPhNFpXoPWL];
						qtbdZuOfJsqeivwvxqiWfDRZFxlc = 1;
						return true;
					}
					goto IL_0083;
				}
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator<UeqtsQhdjnnfXWzWuGffLTLUBrEI> IEnumerable<UeqtsQhdjnnfXWzWuGffLTLUBrEI>.GetEnumerator()
			{
				ydvnUwsLCHyFVhriJtflylTSukuD ydvnUwsLCHyFVhriJtflylTSukuD2;
				if (qtbdZuOfJsqeivwvxqiWfDRZFxlc == -2 && xEjBWJnvfTPjsgGPaFsWHFxiCmDRA == Environment.CurrentManagedThreadId)
				{
					qtbdZuOfJsqeivwvxqiWfDRZFxlc = 0;
					ydvnUwsLCHyFVhriJtflylTSukuD2 = this;
				}
				else
				{
					ydvnUwsLCHyFVhriJtflylTSukuD2 = new ydvnUwsLCHyFVhriJtflylTSukuD(0);
					ydvnUwsLCHyFVhriJtflylTSukuD2.NQRzWbixTnFrGVDrxTpRfWogeUcG = NQRzWbixTnFrGVDrxTpRfWogeUcG;
				}
				ydvnUwsLCHyFVhriJtflylTSukuD2.rWFyPePNJHZLXCiiTzWpBLrYfvoM = UoMSAPqVnNuGhuFuLwzTIQBOqEAs;
				ydvnUwsLCHyFVhriJtflylTSukuD2.gulHDHZvrphlxOYEnERJYQfkbsaY = bxUOOkagZsExhCTYnjzXdLkFOuczb;
				return ydvnUwsLCHyFVhriJtflylTSukuD2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<UeqtsQhdjnnfXWzWuGffLTLUBrEI>)this).GetEnumerator();
			}
		}

		private List<UeqtsQhdjnnfXWzWuGffLTLUBrEI> hMnFiFDFJOhEDyzIWEzfCgOnLjKD;

		public YqUaXflujHDbIipqDvDDYEWsdNuY()
		{
			hMnFiFDFJOhEDyzIWEzfCgOnLjKD = new List<UeqtsQhdjnnfXWzWuGffLTLUBrEI>();
		}

		public void gebCiBgRWcBQrsfaOlznDxUtoGhC(eySnxgpDnPUzyhkGpTWqagWLjids P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			int count = hMnFiFDFJOhEDyzIWEzfCgOnLjKD.Count;
			for (int i = 0; i < count; i++)
			{
				if (hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].dOFCuCSSRZTtmMKCtdNzFnAYGCuIA(P_0, aYHwOJJWObHJKEFfuAcKIpVJRLspA.Exact))
				{
					hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].abGnoSlhVvlcPHaiayRjrdePRqID = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
					hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].sxCJNrMJeyHDwffjTqvXONizbpqAb = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;
					hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].eKeopiYjqPiUsFCIMikNFnogfubm = P_0.bPKUQQOuSxJXkBAetBaJbHwHPStDA;
					hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].rSFEtgvowLBxjOwhlGaaEPQDucuJA = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId;
					hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].gbNXDdwNoeeOQNsCOZNXtEDbCqXt = P_0.hTjAsxCEdFIqVgGpcElosIWvJZtrB;
					hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].DIfWRszsewoYkBfwpVgUKvXbWOhd = P_0.biFqRTcWgotJCdYTKlrjBUiwvpLf;
					hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].avmexpcDFqLjzIspeZwvklpvacWVA = P_0.oHydTJYKdBErDKpaWkNlQajUmvjuA;
					gpDZvKhdFPujyvFjISUUwWiwjWmC(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid, i);
					return;
				}
			}
			hMnFiFDFJOhEDyzIWEzfCgOnLjKD.Add(new UeqtsQhdjnnfXWzWuGffLTLUBrEI
			{
				abGnoSlhVvlcPHaiayRjrdePRqID = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId,
				sxCJNrMJeyHDwffjTqvXONizbpqAb = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid,
				eKeopiYjqPiUsFCIMikNFnogfubm = P_0.bPKUQQOuSxJXkBAetBaJbHwHPStDA,
				rSFEtgvowLBxjOwhlGaaEPQDucuJA = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId,
				gbNXDdwNoeeOQNsCOZNXtEDbCqXt = P_0.hTjAsxCEdFIqVgGpcElosIWvJZtrB,
				DIfWRszsewoYkBfwpVgUKvXbWOhd = P_0.biFqRTcWgotJCdYTKlrjBUiwvpLf,
				avmexpcDFqLjzIspeZwvklpvacWVA = P_0.oHydTJYKdBErDKpaWkNlQajUmvjuA
			});
			gpDZvKhdFPujyvFjISUUwWiwjWmC(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid, hMnFiFDFJOhEDyzIWEzfCgOnLjKD.Count - 1);
		}

		[IteratorStateMachine(typeof(ydvnUwsLCHyFVhriJtflylTSukuD))]
		public IEnumerable<UeqtsQhdjnnfXWzWuGffLTLUBrEI> QeLBlArDEpCNRukXvvuFGRJPACHbA(eySnxgpDnPUzyhkGpTWqagWLjids P_0, aYHwOJJWObHJKEFfuAcKIpVJRLspA P_1)
		{
			return new ydvnUwsLCHyFVhriJtflylTSukuD(-2)
			{
				NQRzWbixTnFrGVDrxTpRfWogeUcG = this,
				UoMSAPqVnNuGhuFuLwzTIQBOqEAs = P_0,
				bxUOOkagZsExhCTYnjzXdLkFOuczb = P_1
			};
		}

		private void gpDZvKhdFPujyvFjISUUwWiwjWmC(int P_0, Guid P_1, int P_2)
		{
			for (int num = hMnFiFDFJOhEDyzIWEzfCgOnLjKD.Count - 1; num >= 0; num--)
			{
				if (num != P_2 && (hMnFiFDFJOhEDyzIWEzfCgOnLjKD[num].abGnoSlhVvlcPHaiayRjrdePRqID == P_0 || hMnFiFDFJOhEDyzIWEzfCgOnLjKD[num].sxCJNrMJeyHDwffjTqvXONizbpqAb == P_1))
				{
					hMnFiFDFJOhEDyzIWEzfCgOnLjKD.RemoveAt(num);
				}
			}
		}

		public virtual string UJvARBFxPfCzGhXkIihvuDxhHHBMA()
		{
			string text = "";
			text = text + "Joystick records: " + hMnFiFDFJOhEDyzIWEzfCgOnLjKD.Count + "\n";
			for (int i = 0; i < hMnFiFDFJOhEDyzIWEzfCgOnLjKD.Count; i++)
			{
				text = text + "Record " + i + ":\n";
				text = text + hMnFiFDFJOhEDyzIWEzfCgOnLjKD[i].ToString() + "\n\n";
			}
			return text;
		}
	}

	private class QEYyaxOxRAjFDafQtRoAUIgPOsSR
	{
		public eySnxgpDnPUzyhkGpTWqagWLjids AwkWbzGDjIbUxrcPpzKKovdXcULc;

		public AZmRVMBCLWjdSJdmldWMNaRORDDE EdcdrQYOjvURWqyQhvrcMDjHAoKJA;

		public bool DLxaSORuFhwITLSeMSTpEsWFaHTI
		{
			get
			{
				if (AwkWbzGDjIbUxrcPpzKKovdXcULc != null)
				{
					return EdcdrQYOjvURWqyQhvrcMDjHAoKJA != null;
				}
				return false;
			}
		}

		public QEYyaxOxRAjFDafQtRoAUIgPOsSR(eySnxgpDnPUzyhkGpTWqagWLjids P_0, AZmRVMBCLWjdSJdmldWMNaRORDDE P_1)
		{
			AwkWbzGDjIbUxrcPpzKKovdXcULc = P_0;
			EdcdrQYOjvURWqyQhvrcMDjHAoKJA = P_1;
		}

		public static List<AZmRVMBCLWjdSJdmldWMNaRORDDE> MxXMxbihbfPPGklXidvjKCnLCrLbA(List<QEYyaxOxRAjFDafQtRoAUIgPOsSR> P_0)
		{
			if (P_0 == null)
			{
				return new List<AZmRVMBCLWjdSJdmldWMNaRORDDE>();
			}
			List<AZmRVMBCLWjdSJdmldWMNaRORDDE> list = new List<AZmRVMBCLWjdSJdmldWMNaRORDDE>();
			for (int i = 0; i < P_0.Count; i++)
			{
				if (P_0[i].DLxaSORuFhwITLSeMSTpEsWFaHTI)
				{
					list.Add(P_0[i].EdcdrQYOjvURWqyQhvrcMDjHAoKJA);
				}
			}
			return list;
		}
	}

	private class hawIISJBjASuFZmhsCagivquVYCD
	{
		private IFbwjzxeoJceOAopzPRTHJqyoTRuA.gHOYjGBOsDTiaCiNrCrCpWBttqAq ZQcTrVgSascGglljrWAvycmLnagG;

		private IFbwjzxeoJceOAopzPRTHJqyoTRuA.XadLASLHNACjnejAakESELiacdisB LrCZRoNqAUYDfOSYyEbNgdZpHZYS;

		private NativeBuffer jAQCMqKlEQbCMtKXEpMidLtJrGvsA;

		private int MfOGoXIqBEGuSSeFqSTivdOINecIb;

		public hawIISJBjASuFZmhsCagivquVYCD()
		{
			ZQcTrVgSascGglljrWAvycmLnagG = new IFbwjzxeoJceOAopzPRTHJqyoTRuA.gHOYjGBOsDTiaCiNrCrCpWBttqAq
			{
				eLTcdiWGUhQkifpbPuYFZhmKJmpJ = (uint)Marshal.SizeOf(typeof(IFbwjzxeoJceOAopzPRTHJqyoTRuA.gHOYjGBOsDTiaCiNrCrCpWBttqAq)),
				wQZaWFFlBXPOLzqiUgDuJuUuXUSk = true,
				ivKdLFXQJEhuuBkMWxbpwXIHaiqn = true,
				qyAsCNCqhQnbawjPbjiwfueDeQYT = false,
				eqrcbdFsetkYUEEmEhsYFmNvkfKgA = true,
				fXsfoRGJrBCTJaRxqEMhKqiFdefBb = IntPtr.Zero
			};
			LrCZRoNqAUYDfOSYyEbNgdZpHZYS = IFbwjzxeoJceOAopzPRTHJqyoTRuA.XadLASLHNACjnejAakESELiacdisB.YYjBrUScxyVVQdtQjeGHpYOOUxVK();
			jAQCMqKlEQbCMtKXEpMidLtJrGvsA = new NativeBuffer((int)LrCZRoNqAUYDfOSYyEbNgdZpHZYS.jqXyQZlUDXGiXogBgzqDdiwSRPXr);
			jAQCMqKlEQbCMtKXEpMidLtJrGvsA.Write(LrCZRoNqAUYDfOSYyEbNgdZpHZYS.jqXyQZlUDXGiXogBgzqDdiwSRPXr, 0);
		}

		public bool pObeUeyzdLkifYPHThxdIaIEbkWx()
		{
			int num = QUXEpiQVqgkuIqmCQNvdkSkkHwcW();
			if (num == MfOGoXIqBEGuSSeFqSTivdOINecIb)
			{
				return false;
			}
			MfOGoXIqBEGuSSeFqSTivdOINecIb = num;
			return true;
		}

		public void byyLYVluTuvqvnFChvrdZdZTBnOj(int P_0)
		{
			MfOGoXIqBEGuSSeFqSTivdOINecIb = P_0;
		}

		private int QUXEpiQVqgkuIqmCQNvdkSkkHwcW()
		{
			try
			{
				return VgeWXOWKtHAtHYvQawbKkivsGohCA.KFTstTehzUzISAovGGdebUsVWDuV(ref ZQcTrVgSascGglljrWAvycmLnagG, jAQCMqKlEQbCMtKXEpMidLtJrGvsA);
			}
			catch
			{
				return 0;
			}
		}
	}

	private enum DbcdvnelxeDtXsOptqKPFmRfXXCcA
	{
		Device = 17,
		Mouse = 18,
		Keyboard = 19,
		Joystick = 20,
		Gamepad = 21,
		Driving = 22,
		Flight = 23,
		FirstPerson = 24,
		ControlDevice = 25,
		ScreenPointer = 26,
		Remote = 27,
		Supplemental = 28
	}

	private IntPtr KarUmMKKdDJZnvEjstnBGvuwdLie;

	private BKiqqPDcNrfyFbgcenapmsdywauK oqsgCXcQhYcYZealJAlprdVwdjWub;

	private List<eySnxgpDnPUzyhkGpTWqagWLjids> XnvAUkdyNdlgvEIwuwyMdQRkGekX;

	private int veAwFJuhzSHQsGoKjdjSjrjEHQwsA;

	private YqUaXflujHDbIipqDvDDYEWsdNuY DbhkPXsTDCAPRLPQVOqzrPsqFxNY;

	private bool dppyAtOMYKWobchRxdNWGaBkGoag;

	private DDWjNWYAVnuAprLJBexwaxtfkBnQA EKReQqmMmAjbykSUAnqAlvyYrzeO;

	private UpdateLoopSetting pmRJXkDRSxEFHkFDlIEbKwXoQdiAA;

	private Action<int, ControllerDataUpdater> jftBKGawcbxHhLhzECKRfKNxoNKSA;

	private PlatformInputManager fLDxHRAMuVOmZAVvyrgiiorcsvNF;

	private TimerRealTime PtqIaWBdcadmUgHypYcznszrzPib;

	private global::gxiqikdujHobFnpuuAGPldgYhOvdA<bool> ByswoqywIaEzVUcEHPRxQAzWHukl;

	private hawIISJBjASuFZmhsCagivquVYCD VXWzfoVpMrJIhclKMiXWQdUoNmAR;

	private int jlhOiQLjNtOoJiouKhjsxUPPxiM;

	private int outyTplJGZxsIkLeZSZlzTAcPdhp;

	private global::gxiqikdujHobFnpuuAGPldgYhOvdA<List<QEYyaxOxRAjFDafQtRoAUIgPOsSR>> CpPcRvgDzIBQRxqqaYQJjqmXgitDA;

	private readonly object SWQmXYSsAyfZheXpHBYZeFTdlYRG = new object();

	private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> enLJuAssMDXfGkofUdLfHlJnbRdGb;

	private Func<int> PDyXCMvqozYnWAeYewkVNCqXFWYM;

	DDWjNWYAVnuAprLJBexwaxtfkBnQA LrgFZTysTrlKPEyvhAYFSHPAbEyV.CGyAAtvckTjlzgONMmncsFHBDVgHA
	{
		get
		{
			return EKReQqmMmAjbykSUAnqAlvyYrzeO;
		}
		set
		{
			EKReQqmMmAjbykSUAnqAlvyYrzeO = eKReQqmMmAjbykSUAnqAlvyYrzeO;
		}
	}

	[CustomObfuscation(rename = false)]
	int PlatformInputManager.deviceCount => veAwFJuhzSHQsGoKjdjSjrjEHQwsA;

	[CustomObfuscation(rename = false)]
	PlatformInputManager PlatformInputManager.primaryInputManager => fLDxHRAMuVOmZAVvyrgiiorcsvNF;

	[CustomObfuscation(rename = false)]
	IInputSource PlatformInputManager.inputSource => new InputSourceWrapper<BKiqqPDcNrfyFbgcenapmsdywauK>(oqsgCXcQhYcYZealJAlprdVwdjWub);

	[CustomObfuscation(rename = false)]
	InputSource PlatformInputManager.inputSourceType => InputSource.DirectInput;

	public wXurhSlljPxwGOSKaGPOzKtqnUtm(UpdateLoopSetting P_0, DDWjNWYAVnuAprLJBexwaxtfkBnQA P_1, IntPtr P_2, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_3, Func<int> P_4)
	{
		try
		{
			pmRJXkDRSxEFHkFDlIEbKwXoQdiAA = P_0;
			EKReQqmMmAjbykSUAnqAlvyYrzeO = P_1;
			KarUmMKKdDJZnvEjstnBGvuwdLie = P_2;
			enLJuAssMDXfGkofUdLfHlJnbRdGb = P_3;
			PDyXCMvqozYnWAeYewkVNCqXFWYM = P_4;
			fLDxHRAMuVOmZAVvyrgiiorcsvNF = this;
			oqsgCXcQhYcYZealJAlprdVwdjWub = new BKiqqPDcNrfyFbgcenapmsdywauK();
			jftBKGawcbxHhLhzECKRfKNxoNKSA = UpdateControllerData;
			VXWzfoVpMrJIhclKMiXWQdUoNmAR = new hawIISJBjASuFZmhsCagivquVYCD();
			ByswoqywIaEzVUcEHPRxQAzWHukl = new global::gxiqikdujHobFnpuuAGPldgYhOvdA<bool>(true, UntjeySEvjxqvoyGqMRKxdmahEKS);
			CpPcRvgDzIBQRxqqaYQJjqmXgitDA = new global::gxiqikdujHobFnpuuAGPldgYhOvdA<List<QEYyaxOxRAjFDafQtRoAUIgPOsSR>>(true, () => HmBAWLdNMQucqtCPxBdOtstEGxei());
			TKbwneLiauAakZdhcGXwBvuHeADZA();
		}
		catch (Exception)
		{
			OnDestroy();
			throw;
		}
	}

	[CustomObfuscation(rename = false)]
	public override void Initialize()
	{
		DbhkPXsTDCAPRLPQVOqzrPsqFxNY = new YqUaXflujHDbIipqDvDDYEWsdNuY();
		PtqIaWBdcadmUgHypYcznszrzPib = new TimerRealTime(1.0);
		PtqIaWBdcadmUgHypYcznszrzPib.Start();
		dUKLAEKhHHNzgASylsMjSwzsKdNu();
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType updateLoop)
	{
		oRebKNysvVQBIXizuAiJDUojJBdqA();
		RnEEbfJtyHlzGgeYNKNjCUfeHANCA();
		QjxEHGFEoiRnPNoKMsRBnxSRXuHEA();
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (CpPcRvgDzIBQRxqqaYQJjqmXgitDA != null)
		{
			CpPcRvgDzIBQRxqqaYQJjqmXgitDA.WvLYkoCkCYjVtJGzHiicpMYRreXw();
		}
		if (ByswoqywIaEzVUcEHPRxQAzWHukl != null)
		{
			ByswoqywIaEzVUcEHPRxQAzWHukl.WvLYkoCkCYjVtJGzHiicpMYRreXw();
		}
		if (XnvAUkdyNdlgvEIwuwyMdQRkGekX == null)
		{
			return;
		}
		lock (SWQmXYSsAyfZheXpHBYZeFTdlYRG)
		{
			for (int i = 0; i < XnvAUkdyNdlgvEIwuwyMdQRkGekX.Count; i++)
			{
				if (XnvAUkdyNdlgvEIwuwyMdQRkGekX[i] != null)
				{
					XnvAUkdyNdlgvEIwuwyMdQRkGekX[i].FhOUygdauUQvwpMOoQeAaVgoqTMB();
					XnvAUkdyNdlgvEIwuwyMdQRkGekX[i].HfWZXJtxroFOgThibiGxfMhjXyGMA();
				}
			}
		}
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return jftBKGawcbxHhLhzECKRfKNxoNKSA;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int inputManagerId, ControllerDataUpdater data)
	{
		lock (SWQmXYSsAyfZheXpHBYZeFTdlYRG)
		{
			for (int i = 0; i < veAwFJuhzSHQsGoKjdjSjrjEHQwsA; i++)
			{
				if (XnvAUkdyNdlgvEIwuwyMdQRkGekX[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == inputManagerId)
				{
					XnvAUkdyNdlgvEIwuwyMdQRkGekX[i].FillData(data);
					return;
				}
			}
		}
		Logger.LogError("Invalid joystick Id " + inputManagerId + "!");
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		dppyAtOMYKWobchRxdNWGaBkGoag = true;
		PtqIaWBdcadmUgHypYcznszrzPib.Start();
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		dppyAtOMYKWobchRxdNWGaBkGoag = true;
		PtqIaWBdcadmUgHypYcznszrzPib.Start();
		if (_SystemDeviceDisconnectedEvent != null)
		{
			_SystemDeviceDisconnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
	{
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedMouseSource GetUnifiedMouseSource()
	{
		return null;
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedKeyboardSource GetUnifiedKeyboardSource()
	{
		return null;
	}

	private void oRebKNysvVQBIXizuAiJDUojJBdqA()
	{
		if (ByswoqywIaEzVUcEHPRxQAzWHukl.LYkqHsINOrRmqDzoBVFvzhFfDHwO)
		{
			if (ByswoqywIaEzVUcEHPRxQAzWHukl.XrInaWDtMKIvqgrHsTdTFhcYalgT() && !PtqIaWBdcadmUgHypYcznszrzPib.running && !CpPcRvgDzIBQRxqqaYQJjqmXgitDA.LYkqHsINOrRmqDzoBVFvzhFfDHwO)
			{
				if (ByswoqywIaEzVUcEHPRxQAzWHukl.JNANzwwQdbCSWIOPljErVgGJvfwO)
				{
					dppyAtOMYKWobchRxdNWGaBkGoag = true;
				}
				PtqIaWBdcadmUgHypYcznszrzPib.Start();
			}
		}
		else if (!PtqIaWBdcadmUgHypYcznszrzPib.running)
		{
			PtqIaWBdcadmUgHypYcznszrzPib.Start();
		}
		else if (PtqIaWBdcadmUgHypYcznszrzPib.Update())
		{
			ByswoqywIaEzVUcEHPRxQAzWHukl.dLArMTCEuzcNhWTwyvoEESZrqiGi();
		}
	}

	private List<QEYyaxOxRAjFDafQtRoAUIgPOsSR> HmBAWLdNMQucqtCPxBdOtstEGxei()
	{
		List<QEYyaxOxRAjFDafQtRoAUIgPOsSR> list = new List<QEYyaxOxRAjFDafQtRoAUIgPOsSR>();
		IList<AZmRVMBCLWjdSJdmldWMNaRORDDE> list2 = ckANaHszmQDhnJjrHUJsqeOTOrXO();
		int count = list2.Count;
		for (int i = 0; i < count; i++)
		{
			if (list2[i] == null)
			{
				continue;
			}
			try
			{
				AZmRVMBCLWjdSJdmldWMNaRORDDE aZmRVMBCLWjdSJdmldWMNaRORDDE = list2[i];
				Guid sVtcKAcULxFXrzLoatpWBhluUdUs = aZmRVMBCLWjdSJdmldWMNaRORDDE.sVtcKAcULxFXrzLoatpWBhluUdUs;
				YOgNgQpZZfYcTITAqIaepzvNafxe yOgNgQpZZfYcTITAqIaepzvNafxe = new YOgNgQpZZfYcTITAqIaepzvNafxe(oqsgCXcQhYcYZealJAlprdVwdjWub, sVtcKAcULxFXrzLoatpWBhluUdUs);
				vXBPVMcLbNwwxhRXmeLqaNqEVEIX vXBPVMcLbNwwxhRXmeLqaNqEVEIX2 = yOgNgQpZZfYcTITAqIaepzvNafxe.JANJnqDdtgdZeQdaCCMdLNmlQEuP;
				if (EKReQqmMmAjbykSUAnqAlvyYrzeO == null)
				{
					goto IL_00bd;
				}
				string text = aZmRVMBCLWjdSJdmldWMNaRORDDE.EbuxgXKWWFMAMpYFAvRpwowfDfQCA.ToString();
				if (!EKReQqmMmAjbykSUAnqAlvyYrzeO.lCLevvbxoZxcHvVWdmGodlHNIhfIA(vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.HeRpsBDulZaNQpVQvddLIpbrNOiD, StringTools.SanitizeDeviceString(aZmRVMBCLWjdSJdmldWMNaRORDDE.VvQGQTZBUkfFSCosnyMActATHIaGb), string.Empty, new PidVid(Convert.ToUInt16(text.Substring(0, 4), 16), Convert.ToUInt16(text.Substring(4, 4), 16))))
				{
					goto IL_00bd;
				}
				goto end_IL_0028;
				IL_00bd:
				if (ZhesWimaaBnRGUppGgZxDkpfWGlk.rpCDpbayXsXDNWeSlctYbytBjjdPb(InputSource.DirectInput, (ushort)vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.cfOdAfTfPQHiScPSUPDbGfIAyhqlB, (ushort)vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.hUdFIigItAsRWpFvbtCrpHQlLcXo, (ZhesWimaaBnRGUppGgZxDkpfWGlk.tiQIyXHnZPhOHCSaxgMFwzWWbpSy)3))
				{
					continue;
				}
				Guid guid = ((!string.IsNullOrEmpty(vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.HeRpsBDulZaNQpVQvddLIpbrNOiD)) ? MiscTools.CreateGuidHashSHA256(vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.HeRpsBDulZaNQpVQvddLIpbrNOiD) : aZmRVMBCLWjdSJdmldWMNaRORDDE.sVtcKAcULxFXrzLoatpWBhluUdUs);
				bool flag = false;
				lock (SWQmXYSsAyfZheXpHBYZeFTdlYRG)
				{
					if (XnvAUkdyNdlgvEIwuwyMdQRkGekX != null)
					{
						for (int j = 0; j < XnvAUkdyNdlgvEIwuwyMdQRkGekX.Count; j++)
						{
							if (XnvAUkdyNdlgvEIwuwyMdQRkGekX[j] != null && XnvAUkdyNdlgvEIwuwyMdQRkGekX[j].ndpCcpaFlxXPRiUPxtfMAQjJymNS == guid)
							{
								yOgNgQpZZfYcTITAqIaepzvNafxe = XnvAUkdyNdlgvEIwuwyMdQRkGekX[j].PKlODBQBiDgMBEfJhWvttvaAJIxmA.mBNCpNkMAbDKlnnmLbDDTOnprebgA;
								flag = true;
								break;
							}
						}
					}
				}
				eySnxgpDnPUzyhkGpTWqagWLjids eySnxgpDnPUzyhkGpTWqagWLjids2 = new eySnxgpDnPUzyhkGpTWqagWLjids(new WtFyEJeQdSfjObjirEdLChzyqzFpA(yOgNgQpZZfYcTITAqIaepzvNafxe, pmRJXkDRSxEFHkFDlIEbKwXoQdiAA), enLJuAssMDXfGkofUdLfHlJnbRdGb);
				eySnxgpDnPUzyhkGpTWqagWLjids2.QBVYxdtibGkLrtiQxFwokOeZtsWA = aZmRVMBCLWjdSJdmldWMNaRORDDE;
				eySnxgpDnPUzyhkGpTWqagWLjids2.XqViQakmppWVzWGyLZWWdtdVuuSC = aZmRVMBCLWjdSJdmldWMNaRORDDE.BDtjCrHXltZYOgcBxKqNsuofrpRvA;
				eySnxgpDnPUzyhkGpTWqagWLjids2.ndpCcpaFlxXPRiUPxtfMAQjJymNS = guid;
				eySnxgpDnPUzyhkGpTWqagWLjids2.iWVQeywgpMGYQjlLkSeMnIqgqKzH = StringTools.SanitizeDeviceString(aZmRVMBCLWjdSJdmldWMNaRORDDE.VvQGQTZBUkfFSCosnyMActATHIaGb);
				eySnxgpDnPUzyhkGpTWqagWLjids2.XhIEXYFzsDCrZdeZrJaluEOiBoVgA = aZmRVMBCLWjdSJdmldWMNaRORDDE.EbuxgXKWWFMAMpYFAvRpwowfDfQCA;
				eySnxgpDnPUzyhkGpTWqagWLjids2.CwhHBDZhlkQjnJvqUGdoOdUMVsfK = (DbcdvnelxeDtXsOptqKPFmRfXXCcA)aZmRVMBCLWjdSJdmldWMNaRORDDE.gOzLOVUgXaIyOqBoDfRdJlrcdmUb;
				GrDbmamhpkiUXKPfGIsGtfTScelg grDbmamhpkiUXKPfGIsGtfTScelg = yOgNgQpZZfYcTITAqIaepzvNafxe.hQXfGyIqaNQWNbVQpOFYUdyDQeXoA;
				eySnxgpDnPUzyhkGpTWqagWLjids2.NjPaIVrAwsYAZzJBujbZNwpNReEV = vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.hUdFIigItAsRWpFvbtCrpHQlLcXo;
				eySnxgpDnPUzyhkGpTWqagWLjids2.BGMSobwPwBMCEvEdfPSmcBahMxhp = false;
				try
				{
					eySnxgpDnPUzyhkGpTWqagWLjids2.DlHDYRKYxFEekvHErjQWmmMWdcUGA = vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.EFIYTukNYVILZhyuoxRMmEdXppDF;
				}
				catch (Exception)
				{
					eySnxgpDnPUzyhkGpTWqagWLjids2.DlHDYRKYxFEekvHErjQWmmMWdcUGA = 0;
				}
				eySnxgpDnPUzyhkGpTWqagWLjids2.hTjAsxCEdFIqVgGpcElosIWvJZtrB = grDbmamhpkiUXKPfGIsGtfTScelg.wYdENNtFNGhijFxfgSqQxUxsLJxC;
				eySnxgpDnPUzyhkGpTWqagWLjids2.biFqRTcWgotJCdYTKlrjBUiwvpLf = grDbmamhpkiUXKPfGIsGtfTScelg.aNtvJKNgBcHUcHzbkEovCQIqXZVo;
				eySnxgpDnPUzyhkGpTWqagWLjids2.oHydTJYKdBErDKpaWkNlQajUmvjuA = grDbmamhpkiUXKPfGIsGtfTScelg.DcmbHzmrWWcdqeKaQDnTFbZjtBNdB;
				eySnxgpDnPUzyhkGpTWqagWLjids2.xschkhGYczeWeFxnOREeVuyJGzotA = new DirectInputControllerExtension(aZmRVMBCLWjdSJdmldWMNaRORDDE, yOgNgQpZZfYcTITAqIaepzvNafxe);
				QbjVAAoiYfjVcdBFVYhKzIWzwKYx(eySnxgpDnPUzyhkGpTWqagWLjids2, vXBPVMcLbNwwxhRXmeLqaNqEVEIX2, out eySnxgpDnPUzyhkGpTWqagWLjids2.BCrvVjeWDoOVwayuqpGWqOqYWIXs);
				try
				{
					string text2;
					try
					{
						text2 = vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.jylQTjZiUCZzLSJBZIvTnKIRkKcv;
					}
					catch
					{
						text2 = eySnxgpDnPUzyhkGpTWqagWLjids2.iWVQeywgpMGYQjlLkSeMnIqgqKzH;
					}
					if (nTeWMHoeeeDrcnFVrUHTkDjFkqW.pXfiUJNVcdTJNwGoGbKgEgZhjpLFb((ushort)vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.cfOdAfTfPQHiScPSUPDbGfIAyhqlB, (ushort)vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.hUdFIigItAsRWpFvbtCrpHQlLcXo, text2) && nTeWMHoeeeDrcnFVrUHTkDjFkqW.dQpmAwaxayWnzzoseTFugUUgcJQU((ushort)vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.cfOdAfTfPQHiScPSUPDbGfIAyhqlB, (ushort)vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.hUdFIigItAsRWpFvbtCrpHQlLcXo, text2, out var num, out var num2, out var num3))
					{
						eySnxgpDnPUzyhkGpTWqagWLjids2.PKlODBQBiDgMBEfJhWvttvaAJIxmA.BMiEzUbSUwtFNyQAPrjCihHYJBk(num, num2, num3, nTeWMHoeeeDrcnFVrUHTkDjFkqW.WUktugqnNKNKpiwRZLjKXkFHFhod((ushort)vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.cfOdAfTfPQHiScPSUPDbGfIAyhqlB, (ushort)vXBPVMcLbNwwxhRXmeLqaNqEVEIX2.hUdFIigItAsRWpFvbtCrpHQlLcXo, text2));
					}
				}
				catch (Exception)
				{
				}
				if (!flag)
				{
					IList<bqkAwqSafotyQAOrTnFIVdkzxoBR> list3 = yOgNgQpZZfYcTITAqIaepzvNafxe.EsnWDpdGQyPNDHpYkyTMzLERiXiFA();
					if (list3 != null)
					{
						for (int k = 0; k < list3.Count; k++)
						{
							if ((list3[k].LIGMofzcVgUaeUQojVFAmpVgBEUr.FISdKDwGlUgdCydNnzZTSNmsChCo & ZmXltWbhqdfqHdQeeybxZILIjOaj.Axis) != ZmXltWbhqdfqHdQeeybxZILIjOaj.All)
							{
								yOgNgQpZZfYcTITAqIaepzvNafxe.JANJnqDdtgdZeQdaCCMdLNmlQEuP.GOCnbILHIjzEOMWvDUYJafackBnq = new mBdPkttZXVmtfEOHgqkZsKGTmdlK(-65535, 65535);
							}
						}
					}
					yOgNgQpZZfYcTITAqIaepzvNafxe.JANJnqDdtgdZeQdaCCMdLNmlQEuP.iBqHGliOheNFwWHmnsWBkPqkElLz = smssWtRVvEJqvivGJYHiIpPiUpIJ.Absolute;
					yOgNgQpZZfYcTITAqIaepzvNafxe.fCuYtMHdQmcvJhElPIqbOhjFUfBc(KarUmMKKdDJZnvEjstnBGvuwdLie, ZaiHPyLAlLloAZnbsMnTzfHNtgsr.NonExclusive | ZaiHPyLAlLloAZnbsMnTzfHNtgsr.Background);
					yOgNgQpZZfYcTITAqIaepzvNafxe.bcyzOZiMxftRynJRwZLBQhRkvWot();
				}
				list.Add(new QEYyaxOxRAjFDafQtRoAUIgPOsSR(eySnxgpDnPUzyhkGpTWqagWLjids2, aZmRVMBCLWjdSJdmldWMNaRORDDE));
				end_IL_0028:;
			}
			catch (Exception)
			{
			}
		}
		return list;
	}

	private void dUKLAEKhHHNzgASylsMjSwzsKdNu()
	{
		TzztxYfHTsmBZZKOHfafjmUIDfHeA(HmBAWLdNMQucqtCPxBdOtstEGxei());
	}

	private void TzztxYfHTsmBZZKOHfafjmUIDfHeA(List<QEYyaxOxRAjFDafQtRoAUIgPOsSR> P_0)
	{
		List<eySnxgpDnPUzyhkGpTWqagWLjids> list = new List<eySnxgpDnPUzyhkGpTWqagWLjids>();
		jlhOiQLjNtOoJiouKhjsxUPPxiM = 0;
		int num = P_0?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			if (P_0[i] == null || !P_0[i].DLxaSORuFhwITLSeMSTpEsWFaHTI)
			{
				continue;
			}
			try
			{
				eySnxgpDnPUzyhkGpTWqagWLjids awkWbzGDjIbUxrcPpzKKovdXcULc = P_0[i].AwkWbzGDjIbUxrcPpzKKovdXcULc;
				awkWbzGDjIbUxrcPpzKKovdXcULc.RkDkIEodKqMuPrgDZBHWaFMAKOIR();
				if (awkWbzGDjIbUxrcPpzKKovdXcULc.oPWNpkLdCWYmPIxHQPkyVBwoizMA)
				{
					jlhOiQLjNtOoJiouKhjsxUPPxiM++;
				}
				list.Add(awkWbzGDjIbUxrcPpzKKovdXcULc);
			}
			catch (Exception)
			{
			}
		}
		VXWzfoVpMrJIhclKMiXWQdUoNmAR.byyLYVluTuvqvnFChvrdZdZTBnOj(jlhOiQLjNtOoJiouKhjsxUPPxiM);
		lock (SWQmXYSsAyfZheXpHBYZeFTdlYRG)
		{
			List<eySnxgpDnPUzyhkGpTWqagWLjids> xnvAUkdyNdlgvEIwuwyMdQRkGekX = XnvAUkdyNdlgvEIwuwyMdQRkGekX;
			int num2 = veAwFJuhzSHQsGoKjdjSjrjEHQwsA;
			int count = list.Count;
			jcsfqaupVkQYHYPhUdkjPNfYFPrZ(num2, count, xnvAUkdyNdlgvEIwuwyMdQRkGekX, list);
			for (int j = 0; j < count; j++)
			{
				if (_UpdateControllerInfoEvent != null)
				{
					_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(list[j]));
				}
			}
			fQBDNseFUbOxLhcWcbKYIUwvlzVD(xnvAUkdyNdlgvEIwuwyMdQRkGekX, list, false);
			fQBDNseFUbOxLhcWcbKYIUwvlzVD(list, xnvAUkdyNdlgvEIwuwyMdQRkGekX, true);
			cDpCZyBpUfkqhsGturwXOWEaQsNIA(list, xnvAUkdyNdlgvEIwuwyMdQRkGekX);
			XnvAUkdyNdlgvEIwuwyMdQRkGekX = list;
			veAwFJuhzSHQsGoKjdjSjrjEHQwsA = list.Count;
		}
	}

	private void QbjVAAoiYfjVcdBFVYhKzIWzwKYx(eySnxgpDnPUzyhkGpTWqagWLjids P_0, vXBPVMcLbNwwxhRXmeLqaNqEVEIX P_1, out string P_2)
	{
		P_2 = string.Empty;
		if (P_0 == null || P_1 == null)
		{
			return;
		}
		string text = ukOapkBndpfhzrsQGpDKSpCquzAqA.REdCZcEnNdBEDFORweJqIprradFS(P_1.HeRpsBDulZaNQpVQvddLIpbrNOiD);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		try
		{
			EpWcxfdzTRDwZlCGYDLEIMydTvDw epWcxfdzTRDwZlCGYDLEIMydTvDw = VgeWXOWKtHAtHYvQawbKkivsGohCA.XkxcYVoknejPlBFVgwkmeQloiATT(text.ToLower(CultureInfo.InvariantCulture));
			if (epWcxfdzTRDwZlCGYDLEIMydTvDw != null)
			{
				P_0.oPWNpkLdCWYmPIxHQPkyVBwoizMA = epWcxfdzTRDwZlCGYDLEIMydTvDw.CgqfJkJKEtGookWDWyrIgfbWAzjN;
				P_0.PaMhIShNdZqnKPZmVQHlOwiQwXVCA = epWcxfdzTRDwZlCGYDLEIMydTvDw.WoucJzpgBPDcOXLfZoSbeRexqsLd;
				P_2 = ZhesWimaaBnRGUppGgZxDkpfWGlk.gLMtIoCxfxIRTjGFnCCoLgYSqluG(epWcxfdzTRDwZlCGYDLEIMydTvDw, P_0.XhIEXYFzsDCrZdeZrJaluEOiBoVgA, P_0.iWVQeywgpMGYQjlLkSeMnIqgqKzH, P_0.PaMhIShNdZqnKPZmVQHlOwiQwXVCA);
				epWcxfdzTRDwZlCGYDLEIMydTvDw.Dispose();
			}
		}
		catch (Exception)
		{
		}
	}

	private void QjxEHGFEoiRnPNoKMsRBnxSRXuHEA()
	{
		lock (SWQmXYSsAyfZheXpHBYZeFTdlYRG)
		{
			for (int i = 0; i < veAwFJuhzSHQsGoKjdjSjrjEHQwsA; i++)
			{
				try
				{
					eySnxgpDnPUzyhkGpTWqagWLjids eySnxgpDnPUzyhkGpTWqagWLjids2 = XnvAUkdyNdlgvEIwuwyMdQRkGekX[i];
					if (eySnxgpDnPUzyhkGpTWqagWLjids2 != null && eySnxgpDnPUzyhkGpTWqagWLjids2.BMfPcUGHAojfIdCnOpODVGQjFHicb() && (CGyAAtvckTjlzgONMmncsFHBDVgHA == null || !eySnxgpDnPUzyhkGpTWqagWLjids2.BGMSobwPwBMCEvEdfPSmcBahMxhp))
					{
						eySnxgpDnPUzyhkGpTWqagWLjids2.Update();
					}
				}
				catch
				{
				}
			}
		}
	}

	private IList<AZmRVMBCLWjdSJdmldWMNaRORDDE> ckANaHszmQDhnJjrHUJsqeOTOrXO()
	{
		try
		{
			IList<AZmRVMBCLWjdSJdmldWMNaRORDDE> list = oqsgCXcQhYcYZealJAlprdVwdjWub.oOaSusGuwOHJyeCWSaIhJLfJFLBA(sJhqOxXkEhdfJrzCFpdosfXfzYPl.GameControl, QeXRlJhevgcnqiRHTcdYHolJBWgCA.AttachedOnly);
			outyTplJGZxsIkLeZSZlzTAcPdhp = list?.Count ?? 0;
			return list;
		}
		catch
		{
			Logger.LogError("Error getting devices from Direct Input!");
			outyTplJGZxsIkLeZSZlzTAcPdhp = 0;
			return EmptyObjects<AZmRVMBCLWjdSJdmldWMNaRORDDE>.EmptyReadOnlyIListT;
		}
	}

	private void TKbwneLiauAakZdhcGXwBvuHeADZA()
	{
		oqsgCXcQhYcYZealJAlprdVwdjWub.GsFDYgPLTUDHgLqdDXgpTPjKedLH();
	}

	private void jcsfqaupVkQYHYPhUdkjPNfYFPrZ(int P_0, int P_1, List<eySnxgpDnPUzyhkGpTWqagWLjids> P_2, List<eySnxgpDnPUzyhkGpTWqagWLjids> P_3)
	{
		if (P_1 > 0)
		{
			P_3.Sort(eySnxgpDnPUzyhkGpTWqagWLjids.EtPAYJsCFRFePNcqZdjeDiRjJyVT);
		}
		if (P_0 > 0 && P_1 > 0)
		{
			IrdDPuAwFlfrhEdCrlsvHRHdziIe(P_1, P_3, P_0, P_2, YqUaXflujHDbIipqDvDDYEWsdNuY.aYHwOJJWObHJKEFfuAcKIpVJRLspA.Exact);
		}
		usxLrMnnrBgpxeLVAtxarUVVbikMA(P_1, P_3, YqUaXflujHDbIipqDvDDYEWsdNuY.aYHwOJJWObHJKEFfuAcKIpVJRLspA.Exact);
		for (int i = 0; i < P_1; i++)
		{
			eySnxgpDnPUzyhkGpTWqagWLjids eySnxgpDnPUzyhkGpTWqagWLjids2 = P_3[i];
			if (eySnxgpDnPUzyhkGpTWqagWLjids2 != null && eySnxgpDnPUzyhkGpTWqagWLjids2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId < 0)
			{
				eySnxgpDnPUzyhkGpTWqagWLjids2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = RQmiMLqRbPTJmFhuacWNciWKoyXpA(P_3);
				eySnxgpDnPUzyhkGpTWqagWLjids2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = PDyXCMvqozYnWAeYewkVNCqXFWYM();
				DbhkPXsTDCAPRLPQVOqzrPsqFxNY.gebCiBgRWcBQrsfaOlznDxUtoGhC(eySnxgpDnPUzyhkGpTWqagWLjids2);
			}
		}
		P_3.Sort(eySnxgpDnPUzyhkGpTWqagWLjids.BdTIuEfOcsaZQZPLJkmkWjmfNMnuA);
	}

	private bool OPnPHqEDmWEcTLuAFmWzUsYwKHLS(List<eySnxgpDnPUzyhkGpTWqagWLjids> P_0, int P_1)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && P_0[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == P_1)
			{
				return false;
			}
		}
		return true;
	}

	private int RQmiMLqRbPTJmFhuacWNciWKoyXpA(List<eySnxgpDnPUzyhkGpTWqagWLjids> P_0)
	{
		int num = 0;
		while (true)
		{
			bool flag = false;
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				if (P_0[i] != null && P_0[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == num)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				break;
			}
			num++;
		}
		return num;
	}

	private bool qIsFaZDBBmDFdUlGUpCYjrVoNLNf(List<eySnxgpDnPUzyhkGpTWqagWLjids> P_0, int P_1)
	{
		if (P_0 == null)
		{
			return false;
		}
		for (int i = 0; i < P_0.Count; i++)
		{
			if (P_0[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == P_1)
			{
				return true;
			}
		}
		return false;
	}

	private void IrdDPuAwFlfrhEdCrlsvHRHdziIe(int P_0, List<eySnxgpDnPUzyhkGpTWqagWLjids> P_1, int P_2, List<eySnxgpDnPUzyhkGpTWqagWLjids> P_3, YqUaXflujHDbIipqDvDDYEWsdNuY.aYHwOJJWObHJKEFfuAcKIpVJRLspA P_4)
	{
		int num = ((P_4 != YqUaXflujHDbIipqDvDDYEWsdNuY.aYHwOJJWObHJKEFfuAcKIpVJRLspA.Exact) ? 1 : 2);
		for (int i = 0; i < P_0; i++)
		{
			eySnxgpDnPUzyhkGpTWqagWLjids eySnxgpDnPUzyhkGpTWqagWLjids2 = P_1[i];
			if (eySnxgpDnPUzyhkGpTWqagWLjids2 == null || eySnxgpDnPUzyhkGpTWqagWLjids2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			for (int j = 0; j < P_2; j++)
			{
				eySnxgpDnPUzyhkGpTWqagWLjids eySnxgpDnPUzyhkGpTWqagWLjids3 = P_3[j];
				if (eySnxgpDnPUzyhkGpTWqagWLjids3 != null && !qIsFaZDBBmDFdUlGUpCYjrVoNLNf(P_1, eySnxgpDnPUzyhkGpTWqagWLjids3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId) && eySnxgpDnPUzyhkGpTWqagWLjids2.MFHZGXqDVsHuHOBjbDbxgoqBLuzd(eySnxgpDnPUzyhkGpTWqagWLjids3) >= num)
				{
					eySnxgpDnPUzyhkGpTWqagWLjids2.slgUmYnKazmjpjPEadlrwhmYwjHh(eySnxgpDnPUzyhkGpTWqagWLjids3);
					DbhkPXsTDCAPRLPQVOqzrPsqFxNY.gebCiBgRWcBQrsfaOlznDxUtoGhC(eySnxgpDnPUzyhkGpTWqagWLjids2);
				}
			}
		}
	}

	private void usxLrMnnrBgpxeLVAtxarUVVbikMA(int P_0, List<eySnxgpDnPUzyhkGpTWqagWLjids> P_1, YqUaXflujHDbIipqDvDDYEWsdNuY.aYHwOJJWObHJKEFfuAcKIpVJRLspA P_2)
	{
		for (int i = 0; i < P_0; i++)
		{
			eySnxgpDnPUzyhkGpTWqagWLjids eySnxgpDnPUzyhkGpTWqagWLjids2 = P_1[i];
			if (eySnxgpDnPUzyhkGpTWqagWLjids2 == null || eySnxgpDnPUzyhkGpTWqagWLjids2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			YqUaXflujHDbIipqDvDDYEWsdNuY.UeqtsQhdjnnfXWzWuGffLTLUBrEI ueqtsQhdjnnfXWzWuGffLTLUBrEI = null;
			foreach (YqUaXflujHDbIipqDvDDYEWsdNuY.UeqtsQhdjnnfXWzWuGffLTLUBrEI item in DbhkPXsTDCAPRLPQVOqzrPsqFxNY.QeLBlArDEpCNRukXvvuFGRJPACHbA(eySnxgpDnPUzyhkGpTWqagWLjids2, P_2))
			{
				if (!qIsFaZDBBmDFdUlGUpCYjrVoNLNf(P_1, item.abGnoSlhVvlcPHaiayRjrdePRqID) && item.rSFEtgvowLBxjOwhlGaaEPQDucuJA >= 0)
				{
					ueqtsQhdjnnfXWzWuGffLTLUBrEI = item;
					break;
				}
			}
			if (ueqtsQhdjnnfXWzWuGffLTLUBrEI != null)
			{
				int num = ueqtsQhdjnnfXWzWuGffLTLUBrEI.rSFEtgvowLBxjOwhlGaaEPQDucuJA;
				if (!OPnPHqEDmWEcTLuAFmWzUsYwKHLS(P_1, num))
				{
					num = (ueqtsQhdjnnfXWzWuGffLTLUBrEI.rSFEtgvowLBxjOwhlGaaEPQDucuJA = RQmiMLqRbPTJmFhuacWNciWKoyXpA(P_1));
				}
				eySnxgpDnPUzyhkGpTWqagWLjids2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = num;
				eySnxgpDnPUzyhkGpTWqagWLjids2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = ueqtsQhdjnnfXWzWuGffLTLUBrEI.abGnoSlhVvlcPHaiayRjrdePRqID;
				DbhkPXsTDCAPRLPQVOqzrPsqFxNY.gebCiBgRWcBQrsfaOlznDxUtoGhC(eySnxgpDnPUzyhkGpTWqagWLjids2);
			}
		}
	}

	private void RnEEbfJtyHlzGgeYNKNjCUfeHANCA()
	{
		if (dppyAtOMYKWobchRxdNWGaBkGoag)
		{
			VxBapjITEDPEnZbvCQKhAfmVLFKO();
		}
		if (CpPcRvgDzIBQRxqqaYQJjqmXgitDA.LYkqHsINOrRmqDzoBVFvzhFfDHwO && CpPcRvgDzIBQRxqqaYQJjqmXgitDA.XrInaWDtMKIvqgrHsTdTFhcYalgT())
		{
			qdunvmpcWJOjkZQDtCfMxeXpOCLm(CpPcRvgDzIBQRxqqaYQJjqmXgitDA.JNANzwwQdbCSWIOPljErVgGJvfwO);
		}
	}

	private void VxBapjITEDPEnZbvCQKhAfmVLFKO()
	{
		dppyAtOMYKWobchRxdNWGaBkGoag = false;
		if (!CpPcRvgDzIBQRxqqaYQJjqmXgitDA.LYkqHsINOrRmqDzoBVFvzhFfDHwO)
		{
			CpPcRvgDzIBQRxqqaYQJjqmXgitDA.dLArMTCEuzcNhWTwyvoEESZrqiGi();
		}
	}

	private void qdunvmpcWJOjkZQDtCfMxeXpOCLm(List<QEYyaxOxRAjFDafQtRoAUIgPOsSR> P_0)
	{
		if (hIesHasXMTITHfhiBGnMRqgMRTdb(QEYyaxOxRAjFDafQtRoAUIgPOsSR.MxXMxbihbfPPGklXidvjKCnLCrLbA(P_0)))
		{
			TzztxYfHTsmBZZKOHfafjmUIDfHeA(P_0);
		}
	}

	private bool hIesHasXMTITHfhiBGnMRqgMRTdb(IList<AZmRVMBCLWjdSJdmldWMNaRORDDE> P_0)
	{
		lock (SWQmXYSsAyfZheXpHBYZeFTdlYRG)
		{
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				if (P_0[i] != null && !jFBBZVZEIQtHUXLIeIiDjrxCFHzFb(P_0[i].sVtcKAcULxFXrzLoatpWBhluUdUs))
				{
					return true;
				}
			}
			int count2 = XnvAUkdyNdlgvEIwuwyMdQRkGekX.Count;
			for (int j = 0; j < count2; j++)
			{
				if (XnvAUkdyNdlgvEIwuwyMdQRkGekX[j] != null && !mvxNAHNFSAiGbPCfkinkBQpgHJMsA(P_0, XnvAUkdyNdlgvEIwuwyMdQRkGekX[j].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool jFBBZVZEIQtHUXLIeIiDjrxCFHzFb(Guid P_0)
	{
		lock (SWQmXYSsAyfZheXpHBYZeFTdlYRG)
		{
			int count = XnvAUkdyNdlgvEIwuwyMdQRkGekX.Count;
			for (int i = 0; i < count; i++)
			{
				if (XnvAUkdyNdlgvEIwuwyMdQRkGekX[i] != null && XnvAUkdyNdlgvEIwuwyMdQRkGekX[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid == P_0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool mvxNAHNFSAiGbPCfkinkBQpgHJMsA(IList<AZmRVMBCLWjdSJdmldWMNaRORDDE> P_0, Guid P_1)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && P_0[i].sVtcKAcULxFXrzLoatpWBhluUdUs == P_1)
			{
				return true;
			}
		}
		return false;
	}

	private void fQBDNseFUbOxLhcWcbKYIUwvlzVD(List<eySnxgpDnPUzyhkGpTWqagWLjids> P_0, List<eySnxgpDnPUzyhkGpTWqagWLjids> P_1, bool P_2)
	{
		if (P_0 == null)
		{
			return;
		}
		int num = P_0?.Count ?? 0;
		int num2 = P_1?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			eySnxgpDnPUzyhkGpTWqagWLjids eySnxgpDnPUzyhkGpTWqagWLjids2 = P_0[i];
			if (eySnxgpDnPUzyhkGpTWqagWLjids2 == null)
			{
				continue;
			}
			bool flag = false;
			if (P_1 != null)
			{
				for (int j = 0; j < num2; j++)
				{
					eySnxgpDnPUzyhkGpTWqagWLjids eySnxgpDnPUzyhkGpTWqagWLjids3 = P_1[j];
					if (eySnxgpDnPUzyhkGpTWqagWLjids3 != null && eySnxgpDnPUzyhkGpTWqagWLjids2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid == eySnxgpDnPUzyhkGpTWqagWLjids3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				njWTCIPdRkGYJVAKkTnESpTFkRaf(P_0[i], P_2);
			}
		}
	}

	private void njWTCIPdRkGYJVAKkTnESpTFkRaf(eySnxgpDnPUzyhkGpTWqagWLjids P_0, bool P_1)
	{
		if (P_1)
		{
			if (_DeviceConnectedEvent != null)
			{
				_DeviceConnectedEvent(P_0.ToBridgedController());
			}
		}
		else if (_DeviceDisconnectedEvent != null)
		{
			_DeviceDisconnectedEvent(P_0.ToControllerDisconnectedEventArgs());
		}
	}

	private bool UntjeySEvjxqvoyGqMRKxdmahEKS()
	{
		int num = oqsgCXcQhYcYZealJAlprdVwdjWub.oVadgeKpxfnGpxIUiencgqXIiMZEb(sJhqOxXkEhdfJrzCFpdosfXfzYPl.GameControl, QeXRlJhevgcnqiRHTcdYHolJBWgCA.AttachedOnly);
		if (outyTplJGZxsIkLeZSZlzTAcPdhp != num)
		{
			outyTplJGZxsIkLeZSZlzTAcPdhp = num;
			return true;
		}
		if (jlhOiQLjNtOoJiouKhjsxUPPxiM > 0 && VXWzfoVpMrJIhclKMiXWQdUoNmAR.pObeUeyzdLkifYPHThxdIaIEbkWx())
		{
			return true;
		}
		return false;
	}

	private void cDpCZyBpUfkqhsGturwXOWEaQsNIA(List<eySnxgpDnPUzyhkGpTWqagWLjids> P_0, List<eySnxgpDnPUzyhkGpTWqagWLjids> P_1)
	{
		if (P_1 == null)
		{
			return;
		}
		for (int i = 0; i < P_1.Count; i++)
		{
			if (P_1[i] != null && (P_0 == null || !P_0.Contains(P_1[i])))
			{
				P_1[i].HfWZXJtxroFOgThibiGxfMhjXyGMA();
			}
		}
	}

	[CompilerGenerated]
	private List<QEYyaxOxRAjFDafQtRoAUIgPOsSR> gXEFECbqANPCSTEkIhfjHpdMZzbK()
	{
		return HmBAWLdNMQucqtCPxBdOtstEGxei();
	}
}
