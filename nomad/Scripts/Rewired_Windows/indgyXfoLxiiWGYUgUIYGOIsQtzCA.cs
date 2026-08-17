using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Rewired;
using Rewired.Data;
using Rewired.Data.Mapping;
using Rewired.InputSources.SDL2;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Utils;

internal class indgyXfoLxiiWGYUgUIYGOIsQtzCA : PlatformInputManager
{
	private class QOXcnPhgxDavIagHJjejLlpTfEpi : IInputManagerJoystick, IInputManagerJoystickPublic
	{
		private int hFOpGKNHMyJYzSydpjpZLOmPgsZm;

		private int UXZADwbuhvRnBburRtDWEXEmwvioA;

		public Guid CbfhtYIsSuKnhOXENjPhnOeKmxbX;

		public string smCuRhayUwUCgDpwICuHKLVeHdff;

		public MnsFyTzecaNZBeNkLkANtMeQRWvs dTCEHMblxsNOkRIfLVMkDDtDknMVA;

		public kxCwhhkaljhYXADBmcNKvNKUIqSAA bOEJPiZjXfxVNxfANOszqNvCrzXE;

		public string qcKDCqJEobkIBWcpilNmUUUAOwas;

		public string JXJNmZVlEiWKSqGZoNwNxhaCvhDk;

		public int retFXMBWHUInogVxVrlawkBFWRjV;

		public int yWeERIHCzQbWUqsoxbsQVrthfgHhA;

		public Guid LkuDOwEuaISGXibmEFrFdOGKuIAzb;

		public PidVid ieNfdrhEqqLsBmqvwsqdIZzXsybtA;

		public Guid toDOvgxmHPmIkTPcXedYCKqbckqxA;

		public int GssBtobMjSjtjdMKetilBhyifLcUA;

		public int jNgKyPJcyZRARSPQpSCFeyEVDPMg;

		public int CqqxdcHSpwkOmLCMMiRjsMRkXyXr;

		public int XWnODFYhnVWstbJUVeXQfDgBbqQd;

		public int PdbPrAoJbZfzcJcSNnHkVyeKVJvX;

		public int VLbHMuVxuXpukdWWivlRBwCSBJhK;

		public bool gQascmYwConZhaeQNmFddkvcAZOW;

		public bool NeDLbjIyIlXJkVPOgtHecnjEJNRl;

		public int QOGCPTgrkSXICURZxmSrsRMgshIgA;

		private float[] GyqTzkAFYhPQjvJEeaNqgvjRnxoSA;

		private bool[] UsRVldmGnNbSSbQkRKwOcUsafnGTb;

		private HardwareJoystickMap_InputManager diXtAgKfBnuHFinxrFpdRcSvNxjs;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> MLufFYdOsxdktibnBxYwDOrKFYyIB;

		private bool xXGDToXGQBbcdYLEVDILCJGccTyN;

		private bool gqyAtyiEclLWevtpBQmknBEuPRdiA;

		[CompilerGenerated]
		private Controller.Extension vJTrUFyutjjBIiGrVgJoRBUrRXkM;

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.rewiredId
		{
			get
			{
				return hFOpGKNHMyJYzSydpjpZLOmPgsZm;
			}
			set
			{
				hFOpGKNHMyJYzSydpjpZLOmPgsZm = value;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.inputManagerId
		{
			get
			{
				return UXZADwbuhvRnBburRtDWEXEmwvioA;
			}
			set
			{
				UXZADwbuhvRnBburRtDWEXEmwvioA = value;
			}
		}

		[CustomObfuscation(rename = false)]
		string IInputManagerJoystickPublic.name => smCuRhayUwUCgDpwICuHKLVeHdff;

		[CustomObfuscation(rename = false)]
		long? IInputManagerJoystickPublic.systemId
		{
			get
			{
				if (UXZADwbuhvRnBburRtDWEXEmwvioA < 0)
				{
					return null;
				}
				return UXZADwbuhvRnBburRtDWEXEmwvioA;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.unityId => 0;

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.instanceGuid => LkuDOwEuaISGXibmEFrFdOGKuIAzb;

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.persistentGuid => Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;

		[CustomObfuscation(rename = false)]
		Controller.Extension IInputManagerJoystickPublic.extension
		{
			[CompilerGenerated]
			get
			{
				return vJTrUFyutjjBIiGrVgJoRBUrRXkM;
			}
			[CompilerGenerated]
			set
			{
				vJTrUFyutjjBIiGrVgJoRBUrRXkM = value;
			}
		}

		[CustomObfuscation(rename = false)]
		public void SetVibration(float amount, int motorIndex)
		{
			dTCEHMblxsNOkRIfLVMkDDtDknMVA.TlSkvEAPvqbNWiNXzVZkUeABEnReb(motorIndex, amount, false);
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

		public QOXcnPhgxDavIagHJjejLlpTfEpi(Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_0)
		{
			MLufFYdOsxdktibnBxYwDOrKFYyIB = P_0;
			UXZADwbuhvRnBburRtDWEXEmwvioA = -1;
			hFOpGKNHMyJYzSydpjpZLOmPgsZm = -1;
		}

		public void szZhMTUTkommVoAguvKtdGWNmMvs()
		{
			toDOvgxmHPmIkTPcXedYCKqbckqxA = MiscTools.CreateGuidHashSHA1(qcKDCqJEobkIBWcpilNmUUUAOwas + ieNfdrhEqqLsBmqvwsqdIZzXsybtA.ToProductGuid().ToString());
			jNgKyPJcyZRARSPQpSCFeyEVDPMg = XWnODFYhnVWstbJUVeXQfDgBbqQd;
			CqqxdcHSpwkOmLCMMiRjsMRkXyXr = PdbPrAoJbZfzcJcSNnHkVyeKVJvX + VLbHMuVxuXpukdWWivlRBwCSBJhK * 8;
			SMMDvqufiqCKhBuknpneJWFYqHiv();
			CbfhtYIsSuKnhOXENjPhnOeKmxbX = diXtAgKfBnuHFinxrFpdRcSvNxjs.hardwareMapIdentifier.guid;
			smCuRhayUwUCgDpwICuHKLVeHdff = diXtAgKfBnuHFinxrFpdRcSvNxjs.controllerName;
			xXGDToXGQBbcdYLEVDILCJGccTyN = CbfhtYIsSuKnhOXENjPhnOeKmxbX == Guid.Empty;
			GyqTzkAFYhPQjvJEeaNqgvjRnxoSA = new float[jNgKyPJcyZRARSPQpSCFeyEVDPMg];
			UsRVldmGnNbSSbQkRKwOcUsafnGTb = new bool[CqqxdcHSpwkOmLCMMiRjsMRkXyXr];
			Update();
		}

		public void CvJjfaLpgkXQOlcWgIrpxDBKwVpH(QOXcnPhgxDavIagHJjejLlpTfEpi P_0)
		{
			if (P_0 != null)
			{
				UXZADwbuhvRnBburRtDWEXEmwvioA = P_0.UXZADwbuhvRnBburRtDWEXEmwvioA;
				hFOpGKNHMyJYzSydpjpZLOmPgsZm = P_0.hFOpGKNHMyJYzSydpjpZLOmPgsZm;
				for (int i = 0; i < MathTools.Min(UsRVldmGnNbSSbQkRKwOcUsafnGTb.Length, P_0.UsRVldmGnNbSSbQkRKwOcUsafnGTb.Length); i++)
				{
					UsRVldmGnNbSSbQkRKwOcUsafnGTb[i] = P_0.UsRVldmGnNbSSbQkRKwOcUsafnGTb[i];
				}
				for (int j = 0; j < MathTools.Min(GyqTzkAFYhPQjvJEeaNqgvjRnxoSA.Length, P_0.GyqTzkAFYhPQjvJEeaNqgvjRnxoSA.Length); j++)
				{
					GyqTzkAFYhPQjvJEeaNqgvjRnxoSA[j] = P_0.GyqTzkAFYhPQjvJEeaNqgvjRnxoSA[j];
				}
				gqyAtyiEclLWevtpBQmknBEuPRdiA = P_0.gqyAtyiEclLWevtpBQmknBEuPRdiA;
			}
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			MmfAOpyMdjPAsdyJFhTVEiGDhcMcA();
			HwIFRQivlsjcrdVnUNREsPozPiRG();
			if (!gqyAtyiEclLWevtpBQmknBEuPRdiA && dTCEHMblxsNOkRIfLVMkDDtDknMVA.nKeUqEcMZvFbbNysLlzgzgQizezN)
			{
				gqyAtyiEclLWevtpBQmknBEuPRdiA = true;
			}
		}

		void IInputManagerJoystick.Update()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Update
			this.Update();
		}

		[CustomObfuscation(rename = false)]
		public void FillData(ControllerDataUpdater dataUpdater)
		{
			if (jNgKyPJcyZRARSPQpSCFeyEVDPMg != dataUpdater.axisCount || CqqxdcHSpwkOmLCMMiRjsMRkXyXr != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			for (int i = 0; i < jNgKyPJcyZRARSPQpSCFeyEVDPMg; i++)
			{
				dataUpdater.axisValues[i] = GyqTzkAFYhPQjvJEeaNqgvjRnxoSA[i];
			}
			for (int j = 0; j < CqqxdcHSpwkOmLCMMiRjsMRkXyXr; j++)
			{
				dataUpdater.buttonValues[j] = UsRVldmGnNbSSbQkRKwOcUsafnGTb[j];
			}
			if (gqyAtyiEclLWevtpBQmknBEuPRdiA && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		void IInputManagerJoystick.FillData(ControllerDataUpdater dataUpdater)
		{
			//ILSpy generated this explicit interface implementation from .override directive in FillData
			this.FillData(dataUpdater);
		}

		public int cXQgvWYfrPASWAYxMGIzExFGwpNF(QOXcnPhgxDavIagHJjejLlpTfEpi P_0)
		{
			if (P_0.hFOpGKNHMyJYzSydpjpZLOmPgsZm == hFOpGKNHMyJYzSydpjpZLOmPgsZm)
			{
				return 2;
			}
			if (XWnODFYhnVWstbJUVeXQfDgBbqQd != P_0.XWnODFYhnVWstbJUVeXQfDgBbqQd)
			{
				return 0;
			}
			if (PdbPrAoJbZfzcJcSNnHkVyeKVJvX != P_0.PdbPrAoJbZfzcJcSNnHkVyeKVJvX)
			{
				return 0;
			}
			if (VLbHMuVxuXpukdWWivlRBwCSBJhK != P_0.VLbHMuVxuXpukdWWivlRBwCSBJhK)
			{
				return 0;
			}
			if (P_0.LkuDOwEuaISGXibmEFrFdOGKuIAzb == LkuDOwEuaISGXibmEFrFdOGKuIAzb)
			{
				return 2;
			}
			if (P_0.toDOvgxmHPmIkTPcXedYCKqbckqxA == toDOvgxmHPmIkTPcXedYCKqbckqxA)
			{
				return 1;
			}
			return 0;
		}

		private BridgedControllerHWInfo AyVWdqhlPfVlmcDmIeXkkDPFZFHH()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
			tTZhGBpDWaONfOyRiPatGgvtcihKA(bridgedControllerHWInfo);
			return bridgedControllerHWInfo;
		}

		[CustomObfuscation(rename = false)]
		public BridgedController ToBridgedController()
		{
			BridgedController bridgedController = new BridgedController();
			XYhvPpeSOZDbpwsBcmejzqylJeDc(bridgedController);
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
			return new ControllerDisconnectedEventArgs(hFOpGKNHMyJYzSydpjpZLOmPgsZm);
		}

		ControllerDisconnectedEventArgs IInputManagerJoystick.ToControllerDisconnectedEventArgs()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToControllerDisconnectedEventArgs
			return this.ToControllerDisconnectedEventArgs();
		}

		private void MmfAOpyMdjPAsdyJFhTVEiGDhcMcA()
		{
			if (jNgKyPJcyZRARSPQpSCFeyEVDPMg <= 0 || diXtAgKfBnuHFinxrFpdRcSvNxjs.map.platform != InputPlatform.SDL2)
			{
				return;
			}
			HardwareJoystickMap.Platform_SDL2_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_SDL2_Base)diXtAgKfBnuHFinxrFpdRcSvNxjs.map).Axes_orig;
			if (axes_orig != null)
			{
				for (int i = 0; i < axes_orig.Length; i++)
				{
					UteydSARQpbGOYEUYucacKnlWVLM(axes_orig[i], i);
				}
			}
		}

		private void HwIFRQivlsjcrdVnUNREsPozPiRG()
		{
			if (CqqxdcHSpwkOmLCMMiRjsMRkXyXr <= 0)
			{
				return;
			}
			HardwareJoystickMap.Platform_SDL2_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_SDL2_Base)diXtAgKfBnuHFinxrFpdRcSvNxjs.map).Buttons_orig;
			if (buttons_orig != null)
			{
				for (int i = 0; i < buttons_orig.Length; i++)
				{
					xbMVYJhkSoOTjNXHxUXGOwMuiasr(buttons_orig[i], i);
				}
			}
		}

		private void UteydSARQpbGOYEUYucacKnlWVLM(HardwareJoystickMap.Platform_SDL2_Base.Axis P_0, int P_1)
		{
			if (P_1 >= jNgKyPJcyZRARSPQpSCFeyEVDPMg)
			{
				throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
			}
			GyqTzkAFYhPQjvJEeaNqgvjRnxoSA[P_1] = APcdXSznIokRVYwDelExcTvaquAw(P_0);
		}

		private void xbMVYJhkSoOTjNXHxUXGOwMuiasr(HardwareJoystickMap.Platform_SDL2_Base.Button P_0, int P_1)
		{
			if (P_1 >= CqqxdcHSpwkOmLCMMiRjsMRkXyXr)
			{
				throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
			}
			UsRVldmGnNbSSbQkRKwOcUsafnGTb[P_1] = TxVbfrULVugakHJgcGWZqCYnCmwf(P_0);
		}

		private float APcdXSznIokRVYwDelExcTvaquAw(HardwareJoystickMap.Platform_SDL2_Base.Axis P_0)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				int sourceAxis = P_0.sourceAxis;
				if (sourceAxis < 0 || sourceAxis >= XWnODFYhnVWstbJUVeXQfDgBbqQd || sourceAxis >= 56)
				{
					return 0f;
				}
				return dTCEHMblxsNOkRIfLVMkDDtDknMVA.lgwptabYgqwjBJLWwSJQTGtzgorFA(sourceAxis);
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= PdbPrAoJbZfzcJcSNnHkVyeKVJvX || sourceButton >= 256)
				{
					return 0f;
				}
				if (!dTCEHMblxsNOkRIfLVMkDDtDknMVA.rVzNVNpkHCuqmpqvtoGwGXczIcQg(sourceButton))
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
				if (sourceHat < 0 || sourceHat >= VLbHMuVxuXpukdWWivlRBwCSBJhK || sourceHat >= 4)
				{
					return 0f;
				}
				int num = dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat);
				if (num < 0)
				{
					return 0f;
				}
				float num2;
				if (P_0.sourceHatDirection == AxisDirection.Horizontal)
				{
					num2 = yokhmqffqrfyISZaKXjftEsiRSrg(num, AxisDirection.Horizontal);
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
					num2 = yokhmqffqrfyISZaKXjftEsiRSrg(num, AxisDirection.Vertical);
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
			return 0f;
		}

		private bool TxVbfrULVugakHJgcGWZqCYnCmwf(HardwareJoystickMap.Platform_SDL2_Base.Button P_0)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				if (P_0.ignoreIfButtonsActive)
				{
					for (int i = 0; i < P_0.ignoreIfButtonsActiveButtons.Length; i++)
					{
						if (dTCEHMblxsNOkRIfLVMkDDtDknMVA.rVzNVNpkHCuqmpqvtoGwGXczIcQg(P_0.ignoreIfButtonsActiveButtons[i]))
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
						if (!dTCEHMblxsNOkRIfLVMkDDtDknMVA.rVzNVNpkHCuqmpqvtoGwGXczIcQg(P_0.requiredButtons[j]))
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
				if (sourceButton < 0 || sourceButton >= PdbPrAoJbZfzcJcSNnHkVyeKVJvX || sourceButton >= 256)
				{
					return false;
				}
				return dTCEHMblxsNOkRIfLVMkDDtDknMVA.rVzNVNpkHCuqmpqvtoGwGXczIcQg(sourceButton);
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				int sourceAxis = P_0.sourceAxis;
				if (sourceAxis <= 0 || sourceAxis >= XWnODFYhnVWstbJUVeXQfDgBbqQd || sourceAxis >= 56)
				{
					return false;
				}
				float num = dTCEHMblxsNOkRIfLVMkDDtDknMVA.lgwptabYgqwjBJLWwSJQTGtzgorFA(sourceAxis);
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
				if (sourceHat < 0 || sourceHat >= VLbHMuVxuXpukdWWivlRBwCSBJhK || sourceHat >= 4)
				{
					return false;
				}
				switch (P_0.sourceHatDirection)
				{
				case HatDirection.Up:
					return dyztUxPHOVlmjSCMoyhTucYSpAcj(dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat), 0, P_0.sourceHatType);
				case HatDirection.UpRight:
					return dyztUxPHOVlmjSCMoyhTucYSpAcj(dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat), 1, P_0.sourceHatType);
				case HatDirection.Right:
					return dyztUxPHOVlmjSCMoyhTucYSpAcj(dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat), 2, P_0.sourceHatType);
				case HatDirection.DownRight:
					return dyztUxPHOVlmjSCMoyhTucYSpAcj(dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat), 3, P_0.sourceHatType);
				case HatDirection.Down:
					return dyztUxPHOVlmjSCMoyhTucYSpAcj(dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat), 4, P_0.sourceHatType);
				case HatDirection.DownLeft:
					return dyztUxPHOVlmjSCMoyhTucYSpAcj(dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat), 5, P_0.sourceHatType);
				case HatDirection.Left:
					return dyztUxPHOVlmjSCMoyhTucYSpAcj(dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat), 6, P_0.sourceHatType);
				case HatDirection.UpLeft:
					return dyztUxPHOVlmjSCMoyhTucYSpAcj(dTCEHMblxsNOkRIfLVMkDDtDknMVA.PpskowCPcwjnjIzOFWAKXQJAlDYb(sourceHat), 7, P_0.sourceHatType);
				}
			}
			return false;
		}

		private bool dyztUxPHOVlmjSCMoyhTucYSpAcj(int P_0, int P_1, HatType P_2)
		{
			if (P_0 < 0)
			{
				return false;
			}
			if (diXtAgKfBnuHFinxrFpdRcSvNxjs.isUnknownController && !InputTools.HandleForced4WayHatsOnUnknownControllers(P_1, ref P_2))
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

		private float yokhmqffqrfyISZaKXjftEsiRSrg(int P_0, AxisDirection P_1)
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

		private ControlDeviceType AvlfofJKHHGoXGVNMMqCBPpmemTi(kxCwhhkaljhYXADBmcNKvNKUIqSAA P_0)
		{
			return P_0 switch
			{
				kxCwhhkaljhYXADBmcNKvNKUIqSAA.Joystick => ControlDeviceType.Joystick, 
				kxCwhhkaljhYXADBmcNKvNKUIqSAA.Gamepad => ControlDeviceType.Gamepad, 
				kxCwhhkaljhYXADBmcNKvNKUIqSAA.Keyboard => ControlDeviceType.Keyboard, 
				kxCwhhkaljhYXADBmcNKvNKUIqSAA.Mouse => ControlDeviceType.Mouse, 
				_ => ControlDeviceType.Unknown, 
			};
		}

		private void SMMDvqufiqCKhBuknpneJWFYqHiv()
		{
			diXtAgKfBnuHFinxrFpdRcSvNxjs = MLufFYdOsxdktibnBxYwDOrKFYyIB(AyVWdqhlPfVlmcDmIeXkkDPFZFHH());
			if (diXtAgKfBnuHFinxrFpdRcSvNxjs == null)
			{
				Logger.LogError("Default hardware map not found!");
				return;
			}
			if (diXtAgKfBnuHFinxrFpdRcSvNxjs.useSystemName)
			{
				if (!string.IsNullOrEmpty(JXJNmZVlEiWKSqGZoNwNxhaCvhDk))
				{
					string text = Regex.Replace(JXJNmZVlEiWKSqGZoNwNxhaCvhDk, "\\s+", " ");
					text = text.Trim();
					if (!string.IsNullOrEmpty(text))
					{
						diXtAgKfBnuHFinxrFpdRcSvNxjs.controllerName = text;
					}
				}
				if (diXtAgKfBnuHFinxrFpdRcSvNxjs.deviceLocalizationInfo.parentKeys.Count > 0 && !string.IsNullOrEmpty(diXtAgKfBnuHFinxrFpdRcSvNxjs.deviceLocalizationInfo.parentKeys[0]))
				{
					string a = diXtAgKfBnuHFinxrFpdRcSvNxjs.deviceLocalizationInfo.parentKeys[0];
					string text2 = string.Format("{0}:{1}", dTCEHMblxsNOkRIfLVMkDDtDknMVA.DRSqdpKFiYhQwiUJMiIGrSgPhsfq.vendorId.ToString("x4"), dTCEHMblxsNOkRIfLVMkDDtDknMVA.DRSqdpKFiYhQwiUJMiIGrSgPhsfq.productId.ToString("x4"));
					diXtAgKfBnuHFinxrFpdRcSvNxjs.deviceLocalizationInfo.InsertParentKey(0, LocalizationManager.AppendToKeyAsPath(a, text2));
					if (!string.IsNullOrEmpty(dTCEHMblxsNOkRIfLVMkDDtDknMVA.QqHoOyddFVEouDZAVUrVfEhIkmKpA))
					{
						diXtAgKfBnuHFinxrFpdRcSvNxjs.deviceLocalizationInfo.InsertParentKey(1, LocalizationManager.AppendToKeyAsPath(a, dTCEHMblxsNOkRIfLVMkDDtDknMVA.QqHoOyddFVEouDZAVUrVfEhIkmKpA));
					}
					if (!string.IsNullOrEmpty(dTCEHMblxsNOkRIfLVMkDDtDknMVA.QqHoOyddFVEouDZAVUrVfEhIkmKpA))
					{
						diXtAgKfBnuHFinxrFpdRcSvNxjs.deviceLocalizationInfo.additionalIdentifyingInformation = $"{dTCEHMblxsNOkRIfLVMkDDtDknMVA.QqHoOyddFVEouDZAVUrVfEhIkmKpA} [{text2}]";
					}
					else
					{
						diXtAgKfBnuHFinxrFpdRcSvNxjs.deviceLocalizationInfo.additionalIdentifyingInformation = $"[{text2}]";
					}
				}
			}
			jNgKyPJcyZRARSPQpSCFeyEVDPMg = diXtAgKfBnuHFinxrFpdRcSvNxjs.axisCount;
			CqqxdcHSpwkOmLCMMiRjsMRkXyXr = diXtAgKfBnuHFinxrFpdRcSvNxjs.buttonCount;
		}

		private string WjMjgkSkvREtPkmPRjmXifmVQMtp()
		{
			return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{dTCEHMblxsNOkRIfLVMkDDtDknMVA.WdBQKlEddYOWiqSsIGESbfcFjmtU}{qcKDCqJEobkIBWcpilNmUUUAOwas}{retFXMBWHUInogVxVrlawkBFWRjV}{ieNfdrhEqqLsBmqvwsqdIZzXsybtA.ToProductGuid()}");
		}

		private void tTZhGBpDWaONfOyRiPatGgvtcihKA(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.SDL2;
			P_0.inputSource = dTCEHMblxsNOkRIfLVMkDDtDknMVA.WdBQKlEddYOWiqSsIGESbfcFjmtU;
			P_0.deviceType = AvlfofJKHHGoXGVNMMqCBPpmemTi(bOEJPiZjXfxVNxfANOszqNvCrzXE);
			P_0.hardwareIdentifier = WjMjgkSkvREtPkmPRjmXifmVQMtp();
			P_0.hardwareAxisCount = XWnODFYhnVWstbJUVeXQfDgBbqQd;
			P_0.hardwareButtonCount = PdbPrAoJbZfzcJcSNnHkVyeKVJvX;
			P_0.hardwareHatCount = VLbHMuVxuXpukdWWivlRBwCSBJhK;
			P_0.hw_productName = qcKDCqJEobkIBWcpilNmUUUAOwas;
			P_0.hw_deviceGuid = LkuDOwEuaISGXibmEFrFdOGKuIAzb;
			P_0.hw_productId = retFXMBWHUInogVxVrlawkBFWRjV;
			P_0.hw_pidVid = ieNfdrhEqqLsBmqvwsqdIZzXsybtA;
			P_0.hw_isBluetoothDevice = gQascmYwConZhaeQNmFddkvcAZOW;
			P_0.hw_bluetoothDeviceName = qcKDCqJEobkIBWcpilNmUUUAOwas;
			P_0.hw_systemDeviceName = qcKDCqJEobkIBWcpilNmUUUAOwas;
			P_0.hw_supportsVibration = NeDLbjIyIlXJkVPOgtHecnjEJNRl;
			P_0.hw_isSDL2Gamepad = dTCEHMblxsNOkRIfLVMkDDtDknMVA.KDFAiGgZDBaAPqRTEdpPsmzOdrrDb == kxCwhhkaljhYXADBmcNKvNKUIqSAA.Gamepad;
			P_0.hw_localVibrationMotorCount = QOGCPTgrkSXICURZxmSrsRMgshIgA;
		}

		private void XYhvPpeSOZDbpwsBcmejzqylJeDc(BridgedController P_0)
		{
			tTZhGBpDWaONfOyRiPatGgvtcihKA(P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = diXtAgKfBnuHFinxrFpdRcSvNxjs.ToGameHardwareControllerMap();
			P_0.instanceName = qcKDCqJEobkIBWcpilNmUUUAOwas;
			P_0.productName = qcKDCqJEobkIBWcpilNmUUUAOwas;
			P_0.axisCount = jNgKyPJcyZRARSPQpSCFeyEVDPMg;
			P_0.buttonCount = CqqxdcHSpwkOmLCMMiRjsMRkXyXr;
			P_0.unknownControllerHats = IfAQRdcXGDuacyHeiifApcQagYPd();
			P_0.controllerTypeGuid = CbfhtYIsSuKnhOXENjPhnOeKmxbX;
			P_0.controllerExtension = Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Eextension;
		}

		private UnknownControllerHat[] IfAQRdcXGDuacyHeiifApcQagYPd()
		{
			if (!xXGDToXGQBbcdYLEVDILCJGccTyN)
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

		public static int hPWqbVTnBaybulSbFEEOGoSLDNpP(QOXcnPhgxDavIagHJjejLlpTfEpi P_0, QOXcnPhgxDavIagHJjejLlpTfEpi P_1)
		{
			if (P_0.UXZADwbuhvRnBburRtDWEXEmwvioA < P_1.UXZADwbuhvRnBburRtDWEXEmwvioA)
			{
				return -1;
			}
			if (P_0.UXZADwbuhvRnBburRtDWEXEmwvioA > P_1.UXZADwbuhvRnBburRtDWEXEmwvioA)
			{
				return 1;
			}
			return 0;
		}

		public static int GwmfFPrfLKHqPGiVrTqCqalQisPAb(QOXcnPhgxDavIagHJjejLlpTfEpi P_0, QOXcnPhgxDavIagHJjejLlpTfEpi P_1)
		{
			if (P_0.GssBtobMjSjtjdMKetilBhyifLcUA < P_1.GssBtobMjSjtjdMKetilBhyifLcUA)
			{
				return -1;
			}
			if (P_0.GssBtobMjSjtjdMKetilBhyifLcUA > P_1.GssBtobMjSjtjdMKetilBhyifLcUA)
			{
				return 1;
			}
			return 0;
		}
	}

	private class YRfWNSLfRefVykhAFsenxqrJSUBq
	{
		public enum muvrxcuxynOijnIwYxkgJCUYSSYx
		{
			Exact = 0,
			Approximate = 1
		}

		public class lVYmJbwKHfUAJBpeCEIDKDrABZWzA
		{
			public int XHpbfXkGEmIshhyvzNJWfaCZlMuM;

			public Guid AjwbOVHVGomeSjmIcKnbcSukbSmvA;

			public Guid purcjAIEseHkFMBRWSQpkMTEJIpU;

			public int eUSDttDGIHrEarGjVLiAhNIxwnth;

			public int oqiKhiFqJAIZmyrLqbQJSMgzMfvV;

			public int teawjVIwPluPMlknQirZVluZIFgC;

			public int uKPpwfEyGxdPfbryYtKvdzJKEGPx;

			public bool bqxyeKCEZbfMNAYLxEewrIoIrmFc(QOXcnPhgxDavIagHJjejLlpTfEpi P_0, muvrxcuxynOijnIwYxkgJCUYSSYx P_1)
			{
				if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == XHpbfXkGEmIshhyvzNJWfaCZlMuM)
				{
					return true;
				}
				if (oqiKhiFqJAIZmyrLqbQJSMgzMfvV != P_0.XWnODFYhnVWstbJUVeXQfDgBbqQd)
				{
					return false;
				}
				if (teawjVIwPluPMlknQirZVluZIFgC != P_0.PdbPrAoJbZfzcJcSNnHkVyeKVJvX)
				{
					return false;
				}
				if (uKPpwfEyGxdPfbryYtKvdzJKEGPx != P_0.VLbHMuVxuXpukdWWivlRBwCSBJhK)
				{
					return false;
				}
				return P_1 switch
				{
					muvrxcuxynOijnIwYxkgJCUYSSYx.Exact => AjwbOVHVGomeSjmIcKnbcSukbSmvA == P_0.LkuDOwEuaISGXibmEFrFdOGKuIAzb, 
					muvrxcuxynOijnIwYxkgJCUYSSYx.Approximate => purcjAIEseHkFMBRWSQpkMTEJIpU == P_0.toDOvgxmHPmIkTPcXedYCKqbckqxA, 
					_ => throw new NotImplementedException(), 
				};
			}
		}

		private sealed class OHBdaxJDIRkFezjMRDGeNvMKKLzS : IEnumerable<lVYmJbwKHfUAJBpeCEIDKDrABZWzA>, IEnumerable, IEnumerator<lVYmJbwKHfUAJBpeCEIDKDrABZWzA>, IEnumerator, IDisposable
		{
			private int VNiEhxkpyvZlSwUbenAKHtpFkubsB;

			private lVYmJbwKHfUAJBpeCEIDKDrABZWzA hMNaRBIpgpAUddeJFFzZRWDHUMwYb;

			private int tGMxIngIyADhRXHNZAMNCiAugLwEb;

			public YRfWNSLfRefVykhAFsenxqrJSUBq GoUBnMXKTTZPhvuPTRYBTgHWlXHr;

			private QOXcnPhgxDavIagHJjejLlpTfEpi hkNyYMtNsaghROUnIPzglCUOilR;

			public QOXcnPhgxDavIagHJjejLlpTfEpi tzhdJdhmsZnIosdwDeTfZjdCfLmcb;

			private muvrxcuxynOijnIwYxkgJCUYSSYx ixPzJKAfzAndguwzXOHGApoygqIFA;

			public muvrxcuxynOijnIwYxkgJCUYSSYx ZvgFEZBVcSSDBfMvHmDimUmPcWQqc;

			private int qlXthovEGcfSTRAHRbNtApXGGgQm;

			private int hKdAPbDdvoThvqvLJmXypjYDuagx;

			lVYmJbwKHfUAJBpeCEIDKDrABZWzA IEnumerator<lVYmJbwKHfUAJBpeCEIDKDrABZWzA>.Current
			{
				[DebuggerHidden]
				get
				{
					return hMNaRBIpgpAUddeJFFzZRWDHUMwYb;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return hMNaRBIpgpAUddeJFFzZRWDHUMwYb;
				}
			}

			[DebuggerHidden]
			public OHBdaxJDIRkFezjMRDGeNvMKKLzS(int P_0)
			{
				VNiEhxkpyvZlSwUbenAKHtpFkubsB = P_0;
				tGMxIngIyADhRXHNZAMNCiAugLwEb = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				VNiEhxkpyvZlSwUbenAKHtpFkubsB = -2;
			}

			private bool MoveNext()
			{
				int vNiEhxkpyvZlSwUbenAKHtpFkubsB = VNiEhxkpyvZlSwUbenAKHtpFkubsB;
				YRfWNSLfRefVykhAFsenxqrJSUBq goUBnMXKTTZPhvuPTRYBTgHWlXHr = GoUBnMXKTTZPhvuPTRYBTgHWlXHr;
				if (vNiEhxkpyvZlSwUbenAKHtpFkubsB != 0)
				{
					if (vNiEhxkpyvZlSwUbenAKHtpFkubsB != 1)
					{
						return false;
					}
					VNiEhxkpyvZlSwUbenAKHtpFkubsB = -1;
					goto IL_0083;
				}
				VNiEhxkpyvZlSwUbenAKHtpFkubsB = -1;
				qlXthovEGcfSTRAHRbNtApXGGgQm = goUBnMXKTTZPhvuPTRYBTgHWlXHr.olVWzxGtawIXCGxaDlvejhjnGpVP.Count;
				hKdAPbDdvoThvqvLJmXypjYDuagx = 0;
				goto IL_0093;
				IL_0083:
				hKdAPbDdvoThvqvLJmXypjYDuagx++;
				goto IL_0093;
				IL_0093:
				if (hKdAPbDdvoThvqvLJmXypjYDuagx < qlXthovEGcfSTRAHRbNtApXGGgQm)
				{
					if (goUBnMXKTTZPhvuPTRYBTgHWlXHr.olVWzxGtawIXCGxaDlvejhjnGpVP[hKdAPbDdvoThvqvLJmXypjYDuagx].bqxyeKCEZbfMNAYLxEewrIoIrmFc(hkNyYMtNsaghROUnIPzglCUOilR, ixPzJKAfzAndguwzXOHGApoygqIFA))
					{
						hMNaRBIpgpAUddeJFFzZRWDHUMwYb = goUBnMXKTTZPhvuPTRYBTgHWlXHr.olVWzxGtawIXCGxaDlvejhjnGpVP[hKdAPbDdvoThvqvLJmXypjYDuagx];
						VNiEhxkpyvZlSwUbenAKHtpFkubsB = 1;
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
			IEnumerator<lVYmJbwKHfUAJBpeCEIDKDrABZWzA> IEnumerable<lVYmJbwKHfUAJBpeCEIDKDrABZWzA>.GetEnumerator()
			{
				OHBdaxJDIRkFezjMRDGeNvMKKLzS oHBdaxJDIRkFezjMRDGeNvMKKLzS;
				if (VNiEhxkpyvZlSwUbenAKHtpFkubsB == -2 && tGMxIngIyADhRXHNZAMNCiAugLwEb == Environment.CurrentManagedThreadId)
				{
					VNiEhxkpyvZlSwUbenAKHtpFkubsB = 0;
					oHBdaxJDIRkFezjMRDGeNvMKKLzS = this;
				}
				else
				{
					oHBdaxJDIRkFezjMRDGeNvMKKLzS = new OHBdaxJDIRkFezjMRDGeNvMKKLzS(0);
					oHBdaxJDIRkFezjMRDGeNvMKKLzS.GoUBnMXKTTZPhvuPTRYBTgHWlXHr = GoUBnMXKTTZPhvuPTRYBTgHWlXHr;
				}
				oHBdaxJDIRkFezjMRDGeNvMKKLzS.hkNyYMtNsaghROUnIPzglCUOilR = tzhdJdhmsZnIosdwDeTfZjdCfLmcb;
				oHBdaxJDIRkFezjMRDGeNvMKKLzS.ixPzJKAfzAndguwzXOHGApoygqIFA = ZvgFEZBVcSSDBfMvHmDimUmPcWQqc;
				return oHBdaxJDIRkFezjMRDGeNvMKKLzS;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<lVYmJbwKHfUAJBpeCEIDKDrABZWzA>)this).GetEnumerator();
			}
		}

		private List<lVYmJbwKHfUAJBpeCEIDKDrABZWzA> olVWzxGtawIXCGxaDlvejhjnGpVP;

		public YRfWNSLfRefVykhAFsenxqrJSUBq()
		{
			olVWzxGtawIXCGxaDlvejhjnGpVP = new List<lVYmJbwKHfUAJBpeCEIDKDrABZWzA>();
		}

		public void MXypvwchLLQxSpbzlBfHgSoMKZTmA(QOXcnPhgxDavIagHJjejLlpTfEpi P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			int count = olVWzxGtawIXCGxaDlvejhjnGpVP.Count;
			for (int i = 0; i < count; i++)
			{
				if (olVWzxGtawIXCGxaDlvejhjnGpVP[i].bqxyeKCEZbfMNAYLxEewrIoIrmFc(P_0, muvrxcuxynOijnIwYxkgJCUYSSYx.Exact))
				{
					olVWzxGtawIXCGxaDlvejhjnGpVP[i].XHpbfXkGEmIshhyvzNJWfaCZlMuM = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
					olVWzxGtawIXCGxaDlvejhjnGpVP[i].AjwbOVHVGomeSjmIcKnbcSukbSmvA = P_0.LkuDOwEuaISGXibmEFrFdOGKuIAzb;
					olVWzxGtawIXCGxaDlvejhjnGpVP[i].purcjAIEseHkFMBRWSQpkMTEJIpU = P_0.toDOvgxmHPmIkTPcXedYCKqbckqxA;
					olVWzxGtawIXCGxaDlvejhjnGpVP[i].eUSDttDGIHrEarGjVLiAhNIxwnth = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId;
					olVWzxGtawIXCGxaDlvejhjnGpVP[i].oqiKhiFqJAIZmyrLqbQJSMgzMfvV = P_0.XWnODFYhnVWstbJUVeXQfDgBbqQd;
					olVWzxGtawIXCGxaDlvejhjnGpVP[i].teawjVIwPluPMlknQirZVluZIFgC = P_0.PdbPrAoJbZfzcJcSNnHkVyeKVJvX;
					olVWzxGtawIXCGxaDlvejhjnGpVP[i].uKPpwfEyGxdPfbryYtKvdzJKEGPx = P_0.VLbHMuVxuXpukdWWivlRBwCSBJhK;
					clrWjnVRaBIIjaLQaDcfGqiDogtz(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.LkuDOwEuaISGXibmEFrFdOGKuIAzb, i);
					return;
				}
			}
			olVWzxGtawIXCGxaDlvejhjnGpVP.Add(new lVYmJbwKHfUAJBpeCEIDKDrABZWzA
			{
				XHpbfXkGEmIshhyvzNJWfaCZlMuM = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId,
				AjwbOVHVGomeSjmIcKnbcSukbSmvA = P_0.LkuDOwEuaISGXibmEFrFdOGKuIAzb,
				purcjAIEseHkFMBRWSQpkMTEJIpU = P_0.toDOvgxmHPmIkTPcXedYCKqbckqxA,
				eUSDttDGIHrEarGjVLiAhNIxwnth = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId,
				oqiKhiFqJAIZmyrLqbQJSMgzMfvV = P_0.XWnODFYhnVWstbJUVeXQfDgBbqQd,
				teawjVIwPluPMlknQirZVluZIFgC = P_0.PdbPrAoJbZfzcJcSNnHkVyeKVJvX,
				uKPpwfEyGxdPfbryYtKvdzJKEGPx = P_0.VLbHMuVxuXpukdWWivlRBwCSBJhK
			});
			clrWjnVRaBIIjaLQaDcfGqiDogtz(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.LkuDOwEuaISGXibmEFrFdOGKuIAzb, olVWzxGtawIXCGxaDlvejhjnGpVP.Count - 1);
		}

		[IteratorStateMachine(typeof(OHBdaxJDIRkFezjMRDGeNvMKKLzS))]
		public IEnumerable<lVYmJbwKHfUAJBpeCEIDKDrABZWzA> mdIaYxzqhpeZKHqzPFsaWvukmYdKA(QOXcnPhgxDavIagHJjejLlpTfEpi P_0, muvrxcuxynOijnIwYxkgJCUYSSYx P_1)
		{
			return new OHBdaxJDIRkFezjMRDGeNvMKKLzS(-2)
			{
				GoUBnMXKTTZPhvuPTRYBTgHWlXHr = this,
				tzhdJdhmsZnIosdwDeTfZjdCfLmcb = P_0,
				ZvgFEZBVcSSDBfMvHmDimUmPcWQqc = P_1
			};
		}

		private void clrWjnVRaBIIjaLQaDcfGqiDogtz(int P_0, Guid P_1, int P_2)
		{
			for (int num = olVWzxGtawIXCGxaDlvejhjnGpVP.Count - 1; num >= 0; num--)
			{
				if (num != P_2 && (olVWzxGtawIXCGxaDlvejhjnGpVP[num].XHpbfXkGEmIshhyvzNJWfaCZlMuM == P_0 || olVWzxGtawIXCGxaDlvejhjnGpVP[num].AjwbOVHVGomeSjmIcKnbcSukbSmvA == P_1))
				{
					olVWzxGtawIXCGxaDlvejhjnGpVP.RemoveAt(num);
				}
			}
		}
	}

	private IInputSource CCeESedYRJmPZRqPEYBMRwYSaOJxA;

	private List<QOXcnPhgxDavIagHJjejLlpTfEpi> XhLuYDkTHLJgCpRYXVFsFCPWOJZm;

	private int NvrpEuDtVcDXzZkqYNeXOugGcwSe;

	private YRfWNSLfRefVykhAFsenxqrJSUBq lrmkuJEFDTYIiHOtWvRtrUigYqWL;

	private bool EGVGwyEpOCVfMoNCuVfTNmTVrrkb;

	private Action<int, ControllerDataUpdater> AYqjgKZaddshadSZwYwkJCcBmYZK;

	private PlatformInputManager CNqZgZlxQSOfkDIJgvgKOqFZxquV;

	private readonly bool pIhmFgAVFAnfgXlyknLUqeegByEv;

	private readonly bool xFaoBIxHNWahSDhCaTPLPSRMEfvBA;

	private readonly bool livwzxcLwroOYhDBRgKrOJsFAzhhA;

	private readonly Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> YTPDGiIauRymFBahOlmGEyuFxCnCA;

	private readonly Func<int> WBfhtqKphrjjcvlnEgjPEbhZKhOyA;

	[CustomObfuscation(rename = false)]
	int PlatformInputManager.deviceCount => NvrpEuDtVcDXzZkqYNeXOugGcwSe;

	[CustomObfuscation(rename = false)]
	PlatformInputManager PlatformInputManager.primaryInputManager => CNqZgZlxQSOfkDIJgvgKOqFZxquV;

	[CustomObfuscation(rename = false)]
	IInputSource PlatformInputManager.inputSource => CCeESedYRJmPZRqPEYBMRwYSaOJxA;

	[CustomObfuscation(rename = false)]
	InputSource PlatformInputManager.inputSourceType => InputSource.SDL2;

	public indgyXfoLxiiWGYUgUIYGOIsQtzCA(ConfigVars P_0, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_1, Func<int> P_2, bool P_3, bool P_4, bool P_5)
	{
		try
		{
			YTPDGiIauRymFBahOlmGEyuFxCnCA = P_1;
			WBfhtqKphrjjcvlnEgjPEbhZKhOyA = P_2;
			pIhmFgAVFAnfgXlyknLUqeegByEv = P_3;
			xFaoBIxHNWahSDhCaTPLPSRMEfvBA = P_4;
			livwzxcLwroOYhDBRgKrOJsFAzhhA = P_5;
			CNqZgZlxQSOfkDIJgvgKOqFZxquV = this;
			CCeESedYRJmPZRqPEYBMRwYSaOJxA = new SDL2InputSource(P_0.updateLoop, P_3, P_3, P_4, P_5);
			AYqjgKZaddshadSZwYwkJCcBmYZK = UpdateControllerData;
			CCeESedYRJmPZRqPEYBMRwYSaOJxA.DeviceChangedEvent += uIdzuRvaxsFXzEHjfRxpecFYeXKoA;
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
		if (pIhmFgAVFAnfgXlyknLUqeegByEv)
		{
			lrmkuJEFDTYIiHOtWvRtrUigYqWL = new YRfWNSLfRefVykhAFsenxqrJSUBq();
			iUtAHSVValdODfxRbhFRtdpBdEIq();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType updateLoop)
	{
		if (CCeESedYRJmPZRqPEYBMRwYSaOJxA != null)
		{
			CCeESedYRJmPZRqPEYBMRwYSaOJxA.Update();
		}
		if (pIhmFgAVFAnfgXlyknLUqeegByEv)
		{
			if (EGVGwyEpOCVfMoNCuVfTNmTVrrkb)
			{
				JEjvaNTkjkxSYhOeuemebaVXHHNpA();
			}
			if (CCeESedYRJmPZRqPEYBMRwYSaOJxA != null)
			{
				for (int i = 0; i < NvrpEuDtVcDXzZkqYNeXOugGcwSe; i++)
				{
					XhLuYDkTHLJgCpRYXVFsFCPWOJZm[i]?.dTCEHMblxsNOkRIfLVMkDDtDknMVA.XJnbimdAhduykEEPsKYaCQwEKAahb(updateLoop);
				}
				CCeESedYRJmPZRqPEYBMRwYSaOJxA.UpdateDevices(updateLoop);
			}
			vACBEqshdQBHxqqzoJbkcyPYFpSh();
			if (CCeESedYRJmPZRqPEYBMRwYSaOJxA != null)
			{
				CCeESedYRJmPZRqPEYBMRwYSaOJxA.UpdateFinished();
				for (int j = 0; j < NvrpEuDtVcDXzZkqYNeXOugGcwSe; j++)
				{
					XhLuYDkTHLJgCpRYXVFsFCPWOJZm[j]?.dTCEHMblxsNOkRIfLVMkDDtDknMVA.zHGKpsfvCVFaGNhLgaGtFCpMeOjVA();
				}
			}
		}
		_ = xFaoBIxHNWahSDhCaTPLPSRMEfvBA;
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (XhLuYDkTHLJgCpRYXVFsFCPWOJZm != null)
		{
			int count = XhLuYDkTHLJgCpRYXVFsFCPWOJZm.Count;
			for (int i = 0; i < count; i++)
			{
				if (XhLuYDkTHLJgCpRYXVFsFCPWOJZm[i] != null)
				{
					XhLuYDkTHLJgCpRYXVFsFCPWOJZm[i].dTCEHMblxsNOkRIfLVMkDDtDknMVA?.IJkUyalJtpWDpgMZhZEJwsNlPBHi();
				}
			}
		}
		if (CCeESedYRJmPZRqPEYBMRwYSaOJxA != null)
		{
			CCeESedYRJmPZRqPEYBMRwYSaOJxA.Dispose();
		}
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return AYqjgKZaddshadSZwYwkJCcBmYZK;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int inputManagerId, ControllerDataUpdater data)
	{
		if (!pIhmFgAVFAnfgXlyknLUqeegByEv)
		{
			return;
		}
		for (int i = 0; i < NvrpEuDtVcDXzZkqYNeXOugGcwSe; i++)
		{
			if (XhLuYDkTHLJgCpRYXVFsFCPWOJZm[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == inputManagerId)
			{
				XhLuYDkTHLJgCpRYXVFsFCPWOJZm[i].FillData(data);
				break;
			}
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		if (pIhmFgAVFAnfgXlyknLUqeegByEv)
		{
			EGVGwyEpOCVfMoNCuVfTNmTVrrkb = true;
		}
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		if (pIhmFgAVFAnfgXlyknLUqeegByEv)
		{
			EGVGwyEpOCVfMoNCuVfTNmTVrrkb = true;
		}
		if (_SystemDeviceDisconnectedEvent != null)
		{
			_SystemDeviceDisconnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
	{
		_ = pIhmFgAVFAnfgXlyknLUqeegByEv;
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

	private void iUtAHSVValdODfxRbhFRtdpBdEIq()
	{
		ERmcewnURlGvYUGbCRGKxzoNhVBm(fokzTnLbvkXkDDnOXgTVGLnyEbin());
	}

	private void ERmcewnURlGvYUGbCRGKxzoNhVBm(IList<MnsFyTzecaNZBeNkLkANtMeQRWvs> P_0)
	{
		int num = 0;
		List<QOXcnPhgxDavIagHJjejLlpTfEpi> xhLuYDkTHLJgCpRYXVFsFCPWOJZm = XhLuYDkTHLJgCpRYXVFsFCPWOJZm;
		int nvrpEuDtVcDXzZkqYNeXOugGcwSe = NvrpEuDtVcDXzZkqYNeXOugGcwSe;
		XhLuYDkTHLJgCpRYXVFsFCPWOJZm = new List<QOXcnPhgxDavIagHJjejLlpTfEpi>();
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null)
			{
				MnsFyTzecaNZBeNkLkANtMeQRWvs mnsFyTzecaNZBeNkLkANtMeQRWvs = P_0[i];
				QOXcnPhgxDavIagHJjejLlpTfEpi qOXcnPhgxDavIagHJjejLlpTfEpi = new QOXcnPhgxDavIagHJjejLlpTfEpi(YTPDGiIauRymFBahOlmGEyuFxCnCA);
				qOXcnPhgxDavIagHJjejLlpTfEpi.dTCEHMblxsNOkRIfLVMkDDtDknMVA = mnsFyTzecaNZBeNkLkANtMeQRWvs;
				qOXcnPhgxDavIagHJjejLlpTfEpi.LkuDOwEuaISGXibmEFrFdOGKuIAzb = mnsFyTzecaNZBeNkLkANtMeQRWvs.zEkIVajCasqqJODvtVoFRFKksmBE;
				qOXcnPhgxDavIagHJjejLlpTfEpi.qcKDCqJEobkIBWcpilNmUUUAOwas = mnsFyTzecaNZBeNkLkANtMeQRWvs.QqHoOyddFVEouDZAVUrVfEhIkmKpA;
				qOXcnPhgxDavIagHJjejLlpTfEpi.JXJNmZVlEiWKSqGZoNwNxhaCvhDk = mnsFyTzecaNZBeNkLkANtMeQRWvs.VEOhRMySMmrQdBjBwarGnjFerZqQ;
				qOXcnPhgxDavIagHJjejLlpTfEpi.ieNfdrhEqqLsBmqvwsqdIZzXsybtA = mnsFyTzecaNZBeNkLkANtMeQRWvs.DRSqdpKFiYhQwiUJMiIGrSgPhsfq;
				qOXcnPhgxDavIagHJjejLlpTfEpi.retFXMBWHUInogVxVrlawkBFWRjV = mnsFyTzecaNZBeNkLkANtMeQRWvs.duQGLlWkEcUxGzaVwFyIsqnoyQlQ;
				qOXcnPhgxDavIagHJjejLlpTfEpi.yWeERIHCzQbWUqsoxbsQVrthfgHhA = mnsFyTzecaNZBeNkLkANtMeQRWvs.EvJKCcTGXJPVykhKUrFjTzyShpZu;
				qOXcnPhgxDavIagHJjejLlpTfEpi.bOEJPiZjXfxVNxfANOszqNvCrzXE = mnsFyTzecaNZBeNkLkANtMeQRWvs.KDFAiGgZDBaAPqRTEdpPsmzOdrrDb;
				qOXcnPhgxDavIagHJjejLlpTfEpi.GssBtobMjSjtjdMKetilBhyifLcUA = mnsFyTzecaNZBeNkLkANtMeQRWvs.YjNcuscoAVusSfOTOVbvKOQOOXDKA;
				qOXcnPhgxDavIagHJjejLlpTfEpi.XWnODFYhnVWstbJUVeXQfDgBbqQd = mnsFyTzecaNZBeNkLkANtMeQRWvs.eymxebVmptUKyMqnkTUMLZAtIPtV;
				qOXcnPhgxDavIagHJjejLlpTfEpi.PdbPrAoJbZfzcJcSNnHkVyeKVJvX = mnsFyTzecaNZBeNkLkANtMeQRWvs.WqPKjVPPOTgPceAajhEZiaJOOUZD;
				qOXcnPhgxDavIagHJjejLlpTfEpi.VLbHMuVxuXpukdWWivlRBwCSBJhK = mnsFyTzecaNZBeNkLkANtMeQRWvs.lazczcOAAnApOBVJoqdKAeedhnzpB;
				qOXcnPhgxDavIagHJjejLlpTfEpi.gQascmYwConZhaeQNmFddkvcAZOW = mnsFyTzecaNZBeNkLkANtMeQRWvs.GUebJnwmzGGnzaTGjyyBQTUQoLNSA;
				qOXcnPhgxDavIagHJjejLlpTfEpi.NeDLbjIyIlXJkVPOgtHecnjEJNRl = mnsFyTzecaNZBeNkLkANtMeQRWvs.gYiKMZaInrjJDivgSZyTceSiqjM;
				qOXcnPhgxDavIagHJjejLlpTfEpi.QOGCPTgrkSXICURZxmSrsRMgshIgA = mnsFyTzecaNZBeNkLkANtMeQRWvs.tEPeneguQTAhnyvzZPYncDrmWXcBA;
				qOXcnPhgxDavIagHJjejLlpTfEpi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Eextension = mnsFyTzecaNZBeNkLkANtMeQRWvs.URbZpPMlZDWkNhsEtjKDiUBVSbYC;
				mnsFyTzecaNZBeNkLkANtMeQRWvs.MAseMmLlokxPngiYeTOdEaKjMDRv();
				qOXcnPhgxDavIagHJjejLlpTfEpi.szZhMTUTkommVoAguvKtdGWNmMvs();
				XhLuYDkTHLJgCpRYXVFsFCPWOJZm.Add(qOXcnPhgxDavIagHJjejLlpTfEpi);
				num++;
			}
		}
		NvrpEuDtVcDXzZkqYNeXOugGcwSe = num;
		FjvCwxlHFRheUacWGeuhwagVkezy(nvrpEuDtVcDXzZkqYNeXOugGcwSe, num, xhLuYDkTHLJgCpRYXVFsFCPWOJZm, XhLuYDkTHLJgCpRYXVFsFCPWOJZm);
		for (int j = 0; j < num; j++)
		{
			if (_UpdateControllerInfoEvent != null)
			{
				_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(XhLuYDkTHLJgCpRYXVFsFCPWOJZm[j]));
			}
		}
		lxrnjBREwltHecyngPzMsPekblLp(xhLuYDkTHLJgCpRYXVFsFCPWOJZm, XhLuYDkTHLJgCpRYXVFsFCPWOJZm, false);
		lxrnjBREwltHecyngPzMsPekblLp(XhLuYDkTHLJgCpRYXVFsFCPWOJZm, xhLuYDkTHLJgCpRYXVFsFCPWOJZm, true);
	}

	private void vACBEqshdQBHxqqzoJbkcyPYFpSh()
	{
		for (int i = 0; i < NvrpEuDtVcDXzZkqYNeXOugGcwSe; i++)
		{
			XhLuYDkTHLJgCpRYXVFsFCPWOJZm[i]?.Update();
		}
	}

	private IList<MnsFyTzecaNZBeNkLkANtMeQRWvs> fokzTnLbvkXkDDnOXgTVGLnyEbin()
	{
		return CCeESedYRJmPZRqPEYBMRwYSaOJxA.GetJoysticks<MnsFyTzecaNZBeNkLkANtMeQRWvs>();
	}

	private void FjvCwxlHFRheUacWGeuhwagVkezy(int P_0, int P_1, List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_2, List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_3)
	{
		if (P_1 > 0)
		{
			P_3.Sort(QOXcnPhgxDavIagHJjejLlpTfEpi.GwmfFPrfLKHqPGiVrTqCqalQisPAb);
		}
		if (P_0 > 0 && P_1 > 0)
		{
			VCljLNcrxOGmBbLChbeLjocSOANMb(P_1, P_3, P_0, P_2, YRfWNSLfRefVykhAFsenxqrJSUBq.muvrxcuxynOijnIwYxkgJCUYSSYx.Exact);
			VCljLNcrxOGmBbLChbeLjocSOANMb(P_1, P_3, P_0, P_2, YRfWNSLfRefVykhAFsenxqrJSUBq.muvrxcuxynOijnIwYxkgJCUYSSYx.Approximate);
		}
		agQPTroyKMggaWmZZzqKADSIQQVb(P_1, P_3, YRfWNSLfRefVykhAFsenxqrJSUBq.muvrxcuxynOijnIwYxkgJCUYSSYx.Exact);
		agQPTroyKMggaWmZZzqKADSIQQVb(P_1, P_3, YRfWNSLfRefVykhAFsenxqrJSUBq.muvrxcuxynOijnIwYxkgJCUYSSYx.Approximate);
		for (int i = 0; i < P_1; i++)
		{
			QOXcnPhgxDavIagHJjejLlpTfEpi qOXcnPhgxDavIagHJjejLlpTfEpi = P_3[i];
			if (qOXcnPhgxDavIagHJjejLlpTfEpi != null && qOXcnPhgxDavIagHJjejLlpTfEpi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId < 0)
			{
				qOXcnPhgxDavIagHJjejLlpTfEpi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = yzQLsHPhBlSqGJQXxLHlCZbJcsDw(P_3);
				qOXcnPhgxDavIagHJjejLlpTfEpi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = WBfhtqKphrjjcvlnEgjPEbhZKhOyA();
				lrmkuJEFDTYIiHOtWvRtrUigYqWL.MXypvwchLLQxSpbzlBfHgSoMKZTmA(qOXcnPhgxDavIagHJjejLlpTfEpi);
			}
		}
		P_3.Sort(QOXcnPhgxDavIagHJjejLlpTfEpi.hPWqbVTnBaybulSbFEEOGoSLDNpP);
	}

	private bool IGotEDiJTrnjcvNEcLkbPQtbAAKY(List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_0, int P_1)
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

	private int yzQLsHPhBlSqGJQXxLHlCZbJcsDw(List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_0)
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

	private bool fICEiuYjySiAnEixnqxEdJNbfvOg(List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_0, int P_1)
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

	private void VCljLNcrxOGmBbLChbeLjocSOANMb(int P_0, List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_1, int P_2, List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_3, YRfWNSLfRefVykhAFsenxqrJSUBq.muvrxcuxynOijnIwYxkgJCUYSSYx P_4)
	{
		int num = ((P_4 != YRfWNSLfRefVykhAFsenxqrJSUBq.muvrxcuxynOijnIwYxkgJCUYSSYx.Exact) ? 1 : 2);
		for (int i = 0; i < P_0; i++)
		{
			QOXcnPhgxDavIagHJjejLlpTfEpi qOXcnPhgxDavIagHJjejLlpTfEpi = P_1[i];
			if (qOXcnPhgxDavIagHJjejLlpTfEpi == null || qOXcnPhgxDavIagHJjejLlpTfEpi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			for (int j = 0; j < P_2; j++)
			{
				QOXcnPhgxDavIagHJjejLlpTfEpi qOXcnPhgxDavIagHJjejLlpTfEpi2 = P_3[j];
				if (qOXcnPhgxDavIagHJjejLlpTfEpi2 != null && !fICEiuYjySiAnEixnqxEdJNbfvOg(P_1, qOXcnPhgxDavIagHJjejLlpTfEpi2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId) && qOXcnPhgxDavIagHJjejLlpTfEpi.cXQgvWYfrPASWAYxMGIzExFGwpNF(qOXcnPhgxDavIagHJjejLlpTfEpi2) >= num)
				{
					qOXcnPhgxDavIagHJjejLlpTfEpi.CvJjfaLpgkXQOlcWgIrpxDBKwVpH(qOXcnPhgxDavIagHJjejLlpTfEpi2);
					lrmkuJEFDTYIiHOtWvRtrUigYqWL.MXypvwchLLQxSpbzlBfHgSoMKZTmA(qOXcnPhgxDavIagHJjejLlpTfEpi);
				}
			}
		}
	}

	private void agQPTroyKMggaWmZZzqKADSIQQVb(int P_0, List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_1, YRfWNSLfRefVykhAFsenxqrJSUBq.muvrxcuxynOijnIwYxkgJCUYSSYx P_2)
	{
		for (int i = 0; i < P_0; i++)
		{
			QOXcnPhgxDavIagHJjejLlpTfEpi qOXcnPhgxDavIagHJjejLlpTfEpi = P_1[i];
			if (qOXcnPhgxDavIagHJjejLlpTfEpi == null || qOXcnPhgxDavIagHJjejLlpTfEpi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			YRfWNSLfRefVykhAFsenxqrJSUBq.lVYmJbwKHfUAJBpeCEIDKDrABZWzA lVYmJbwKHfUAJBpeCEIDKDrABZWzA = null;
			foreach (YRfWNSLfRefVykhAFsenxqrJSUBq.lVYmJbwKHfUAJBpeCEIDKDrABZWzA item in lrmkuJEFDTYIiHOtWvRtrUigYqWL.mdIaYxzqhpeZKHqzPFsaWvukmYdKA(qOXcnPhgxDavIagHJjejLlpTfEpi, P_2))
			{
				if (!fICEiuYjySiAnEixnqxEdJNbfvOg(P_1, item.XHpbfXkGEmIshhyvzNJWfaCZlMuM) && item.eUSDttDGIHrEarGjVLiAhNIxwnth >= 0)
				{
					lVYmJbwKHfUAJBpeCEIDKDrABZWzA = item;
					break;
				}
			}
			if (lVYmJbwKHfUAJBpeCEIDKDrABZWzA != null)
			{
				int num = lVYmJbwKHfUAJBpeCEIDKDrABZWzA.eUSDttDGIHrEarGjVLiAhNIxwnth;
				if (!IGotEDiJTrnjcvNEcLkbPQtbAAKY(P_1, num))
				{
					num = (lVYmJbwKHfUAJBpeCEIDKDrABZWzA.eUSDttDGIHrEarGjVLiAhNIxwnth = yzQLsHPhBlSqGJQXxLHlCZbJcsDw(P_1));
				}
				qOXcnPhgxDavIagHJjejLlpTfEpi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = num;
				qOXcnPhgxDavIagHJjejLlpTfEpi.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = lVYmJbwKHfUAJBpeCEIDKDrABZWzA.XHpbfXkGEmIshhyvzNJWfaCZlMuM;
				lrmkuJEFDTYIiHOtWvRtrUigYqWL.MXypvwchLLQxSpbzlBfHgSoMKZTmA(qOXcnPhgxDavIagHJjejLlpTfEpi);
			}
		}
	}

	private void JEjvaNTkjkxSYhOeuemebaVXHHNpA()
	{
		IList<MnsFyTzecaNZBeNkLkANtMeQRWvs> list = fokzTnLbvkXkDDnOXgTVGLnyEbin();
		ERmcewnURlGvYUGbCRGKxzoNhVBm(list);
		EGVGwyEpOCVfMoNCuVfTNmTVrrkb = false;
	}

	private void lxrnjBREwltHecyngPzMsPekblLp(List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_0, List<QOXcnPhgxDavIagHJjejLlpTfEpi> P_1, bool P_2)
	{
		if (P_0 == null)
		{
			return;
		}
		int num = P_0?.Count ?? 0;
		int num2 = P_1?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			QOXcnPhgxDavIagHJjejLlpTfEpi qOXcnPhgxDavIagHJjejLlpTfEpi = P_0[i];
			if (qOXcnPhgxDavIagHJjejLlpTfEpi == null)
			{
				continue;
			}
			bool flag = false;
			if (P_1 != null)
			{
				for (int j = 0; j < num2; j++)
				{
					QOXcnPhgxDavIagHJjejLlpTfEpi qOXcnPhgxDavIagHJjejLlpTfEpi2 = P_1[j];
					if (qOXcnPhgxDavIagHJjejLlpTfEpi2 != null && qOXcnPhgxDavIagHJjejLlpTfEpi.LkuDOwEuaISGXibmEFrFdOGKuIAzb == qOXcnPhgxDavIagHJjejLlpTfEpi2.LkuDOwEuaISGXibmEFrFdOGKuIAzb)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				SIuCnExbzWFJZcTLRXGuTBHymYUl(P_0[i], P_2);
			}
		}
	}

	private void SIuCnExbzWFJZcTLRXGuTBHymYUl(QOXcnPhgxDavIagHJjejLlpTfEpi P_0, bool P_1)
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

	private void uIdzuRvaxsFXzEHjfRxpecFYeXKoA()
	{
		if (pIhmFgAVFAnfgXlyknLUqeegByEv)
		{
			EGVGwyEpOCVfMoNCuVfTNmTVrrkb = true;
		}
		SystemDeviceConnected();
	}
}
