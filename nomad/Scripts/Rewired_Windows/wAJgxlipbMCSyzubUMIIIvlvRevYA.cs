using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rewired;
using Rewired.Config;
using Rewired.Data;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Platforms;
using Rewired.Platforms.Windows.RawInput;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;

internal class wAJgxlipbMCSyzubUMIIIvlvRevYA : PlatformInputManager, LrgFZTysTrlKPEyvhAYFSHPAbEyV
{
	private class mgmlbBRQFaeoqiMIcNTspgMYlddBb : IInputManagerJoystick, IInputManagerJoystickPublic
	{
		private int sSzrszycPOlKlaWMmGVQNbduVkTP;

		private int QlWFrLHgojFQBiRHAaBxVmnTKkvW;

		public Guid DLimkmRsoQHzrNFaPQMFiFhLChxeA;

		public string XiKfntdfYKVrRkvHbkwnBJMjHNXab;

		private readonly JIDPHXmbYMSkzPnbLXOMHLCjPskd DZHRXyxLRGXOsVmtvgYLtukKYKJe;

		private readonly DeviceType bOXRSHunCqisPQNkJYZoOpeUBoI;

		public string FzydYXcZAaAycEHYIJIohPgTxBeJ;

		public string jdKiFeajMWZALcqxhnovacouhDpab;

		public string QmsrlWmtWGTlUGRfcLtxLyjqEXCk;

		public int AXLReMpRMhWwqczNZdqsTGQVZncq;

		public int ZDewcBMTjUbsYXeQDkjNshaSOduJ;

		public Guid ocnlExBGzgvrbbiLumGDMMksucjT;

		public Guid OjIFPJdqkPIULfMkjMZGNcpHsJTVA;

		public Guid YdQfdsSzVZHuCBcAgLOAUtTXGuGx;

		public int sFJRxDuicBykGWtzbbImKRHjuLBi;

		public int HQGWwLrkWDUWhgpyXVdpjipBBNBv;

		public int RYxNJtDPWRbEOjknBCwrBwBcNONyB;

		public int eyKEXEvzpXuagpJhbMJksTcowmYX;

		public int JJAEtoXSWDTTQIwxmmcrINdFEjsC;

		public int PaAdzljaxFJFWEaUsOrabIGDajIdA;

		public bool TjYPbLYxwDISjQqbvtvGMQkgVTVo;

		public bool XIrQMhERePtFjYZAwnZzDkPPlChx;

		public bool eQLIlFhSBmzVfhEJUpSBzPmEmndv;

		public int ETMQmyjBbOoUCMrtSbvLFxZuEFDbA;

		private float[] tGgBIoQaPDUpWHgeUomZUlPfJQYS;

		private float[] lHpDiTTCrmkXPhWrjQUsFkQSALbp;

		private bool[] wBuAMWywHHZTpzMcCqwOWIvvHsvk;

		private HardwareJoystickMap_InputManager FObUmlaHPpIaGAIwrbCGcZTaTAHh;

		private NOJZCogaHTLiuIrHVflAYfKhuPdJ vBHFnkHGTRBUValjumsbtLKWPPWIA;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> ZKVLrjzxNmLDpClnIlvbtaXBwuZN;

		private bool lqZPNguEZjLlsYDnyWcTqMiHaIgU;

		private bool EalJrOdaPtFXkMkCNcXdFcsnBXMx;

		[CompilerGenerated]
		private Controller.Extension ZuuLKFdfpsNMUQQpoOcQDRsbdae;

		private bool UhJyouJlvhwxVMBqhmZOwvqkTeEs;

		public bool lIOtHasIxEmGWFdDtIUzoOipIQNg
		{
			get
			{
				if (DZHRXyxLRGXOsVmtvgYLtukKYKJe == null)
				{
					return false;
				}
				return DZHRXyxLRGXOsVmtvgYLtukKYKJe.FPEaWPeThpkGJZVEKOwWmJJBhkYwA != null;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.rewiredId
		{
			get
			{
				return sSzrszycPOlKlaWMmGVQNbduVkTP;
			}
			set
			{
				sSzrszycPOlKlaWMmGVQNbduVkTP = value;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.inputManagerId
		{
			get
			{
				return QlWFrLHgojFQBiRHAaBxVmnTKkvW;
			}
			set
			{
				QlWFrLHgojFQBiRHAaBxVmnTKkvW = value;
			}
		}

		[CustomObfuscation(rename = false)]
		string IInputManagerJoystickPublic.name
		{
			get
			{
				if (XiKfntdfYKVrRkvHbkwnBJMjHNXab != "Unknown Controller")
				{
					return XiKfntdfYKVrRkvHbkwnBJMjHNXab;
				}
				if (XIrQMhERePtFjYZAwnZzDkPPlChx && !string.IsNullOrEmpty(QmsrlWmtWGTlUGRfcLtxLyjqEXCk))
				{
					return QmsrlWmtWGTlUGRfcLtxLyjqEXCk;
				}
				return jdKiFeajMWZALcqxhnovacouhDpab;
			}
		}

		[CustomObfuscation(rename = false)]
		long? IInputManagerJoystickPublic.systemId
		{
			get
			{
				if (QlWFrLHgojFQBiRHAaBxVmnTKkvW < 0)
				{
					return null;
				}
				return QlWFrLHgojFQBiRHAaBxVmnTKkvW;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.unityId => 0;

		[CustomObfuscation(rename = false)]
		Controller.Extension IInputManagerJoystickPublic.extension
		{
			[CompilerGenerated]
			get
			{
				return ZuuLKFdfpsNMUQQpoOcQDRsbdae;
			}
			[CompilerGenerated]
			set
			{
				ZuuLKFdfpsNMUQQpoOcQDRsbdae = value;
			}
		}

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.instanceGuid => ocnlExBGzgvrbbiLumGDMMksucjT;

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.persistentGuid => Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;

		public bool RGcnHgTfJdsyfegDhhPIIGoZZZRx
		{
			get
			{
				if (!UhJyouJlvhwxVMBqhmZOwvqkTeEs && DZHRXyxLRGXOsVmtvgYLtukKYKJe != null)
				{
					return DZHRXyxLRGXOsVmtvgYLtukKYKJe.NncKhwCZyCAgAQkitjrZChljBRTfb;
				}
				return false;
			}
		}

		[CustomObfuscation(rename = false)]
		public void SetVibration(float amount, int motorIndex)
		{
			_ = RGcnHgTfJdsyfegDhhPIIGoZZZRx;
		}

		void IInputManagerJoystickPublic.SetVibration(float amount, int motorIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(amount, motorIndex);
		}

		[CustomObfuscation(rename = false)]
		public void StopVibration()
		{
			_ = RGcnHgTfJdsyfegDhhPIIGoZZZRx;
		}

		void IInputManagerJoystickPublic.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		public mgmlbBRQFaeoqiMIcNTspgMYlddBb(JIDPHXmbYMSkzPnbLXOMHLCjPskd P_0, DeviceType P_1, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_2)
		{
			DZHRXyxLRGXOsVmtvgYLtukKYKJe = P_0;
			bOXRSHunCqisPQNkJYZoOpeUBoI = P_1;
			ZKVLrjzxNmLDpClnIlvbtaXBwuZN = P_2;
			QlWFrLHgojFQBiRHAaBxVmnTKkvW = -1;
			sSzrszycPOlKlaWMmGVQNbduVkTP = -1;
		}

		public void tKHXpKHQkGDnuYJkWjOWdqDHNOwz()
		{
			if (!RGcnHgTfJdsyfegDhhPIIGoZZZRx)
			{
				return;
			}
			string obj = ((!string.IsNullOrEmpty(QmsrlWmtWGTlUGRfcLtxLyjqEXCk)) ? QmsrlWmtWGTlUGRfcLtxLyjqEXCk : jdKiFeajMWZALcqxhnovacouhDpab);
			Guid ojIFPJdqkPIULfMkjMZGNcpHsJTVA = OjIFPJdqkPIULfMkjMZGNcpHsJTVA;
			YdQfdsSzVZHuCBcAgLOAUtTXGuGx = MiscTools.CreateGuidHashSHA1(obj + ojIFPJdqkPIULfMkjMZGNcpHsJTVA.ToString());
			HQGWwLrkWDUWhgpyXVdpjipBBNBv = eyKEXEvzpXuagpJhbMJksTcowmYX;
			RYxNJtDPWRbEOjknBCwrBwBcNONyB = JJAEtoXSWDTTQIwxmmcrINdFEjsC + PaAdzljaxFJFWEaUsOrabIGDajIdA * 8;
			XvTxAHgidKWgMhDSXgjZwjPgcRtp();
			DLimkmRsoQHzrNFaPQMFiFhLChxeA = FObUmlaHPpIaGAIwrbCGcZTaTAHh.hardwareMapIdentifier.guid;
			XiKfntdfYKVrRkvHbkwnBJMjHNXab = FObUmlaHPpIaGAIwrbCGcZTaTAHh.controllerName;
			lqZPNguEZjLlsYDnyWcTqMiHaIgU = DLimkmRsoQHzrNFaPQMFiFhLChxeA == Guid.Empty;
			tGgBIoQaPDUpWHgeUomZUlPfJQYS = new float[HQGWwLrkWDUWhgpyXVdpjipBBNBv];
			lHpDiTTCrmkXPhWrjQUsFkQSALbp = new float[RYxNJtDPWRbEOjknBCwrBwBcNONyB];
			wBuAMWywHHZTpzMcCqwOWIvvHsvk = new bool[RYxNJtDPWRbEOjknBCwrBwBcNONyB];
			if (FObUmlaHPpIaGAIwrbCGcZTaTAHh != null && RYxNJtDPWRbEOjknBCwrBwBcNONyB > 0)
			{
				switch (FObUmlaHPpIaGAIwrbCGcZTaTAHh.map.platform)
				{
				case InputPlatform.WindowsRawInput:
				{
					HardwareJoystickMap.Platform_RawInput_Base.Button[] buttons_orig2 = ((HardwareJoystickMap.Platform_RawInput_Base)FObUmlaHPpIaGAIwrbCGcZTaTAHh.map).Buttons_orig;
					if (buttons_orig2 != null)
					{
						for (int j = 0; j < buttons_orig2.Length; j++)
						{
							wBuAMWywHHZTpzMcCqwOWIvvHsvk[j] = buttons_orig2[j].buttonInfo.isPressureSensitive;
						}
					}
					break;
				}
				case InputPlatform.WindowsDirectInput:
				{
					HardwareJoystickMap.Platform_DirectInput_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_DirectInput_Base)FObUmlaHPpIaGAIwrbCGcZTaTAHh.map).Buttons_orig;
					if (buttons_orig != null)
					{
						for (int i = 0; i < buttons_orig.Length; i++)
						{
							wBuAMWywHHZTpzMcCqwOWIvvHsvk[i] = buttons_orig[i].buttonInfo.isPressureSensitive;
						}
					}
					break;
				}
				}
			}
			vBHFnkHGTRBUValjumsbtLKWPPWIA = DZHRXyxLRGXOsVmtvgYLtukKYKJe.sbmUGmkfgCgHIFaOCErQOcpRkUkp;
			Update();
		}

		public void iGnvPmQMIqnprLwYBiPpICpMgyGFA(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0)
		{
			if (RGcnHgTfJdsyfegDhhPIIGoZZZRx && P_0 != null)
			{
				QlWFrLHgojFQBiRHAaBxVmnTKkvW = P_0.QlWFrLHgojFQBiRHAaBxVmnTKkvW;
				sSzrszycPOlKlaWMmGVQNbduVkTP = P_0.sSzrszycPOlKlaWMmGVQNbduVkTP;
				for (int i = 0; i < MathTools.Min(lHpDiTTCrmkXPhWrjQUsFkQSALbp.Length, P_0.lHpDiTTCrmkXPhWrjQUsFkQSALbp.Length); i++)
				{
					lHpDiTTCrmkXPhWrjQUsFkQSALbp[i] = P_0.lHpDiTTCrmkXPhWrjQUsFkQSALbp[i];
				}
				for (int j = 0; j < MathTools.Min(wBuAMWywHHZTpzMcCqwOWIvvHsvk.Length, P_0.wBuAMWywHHZTpzMcCqwOWIvvHsvk.Length); j++)
				{
					wBuAMWywHHZTpzMcCqwOWIvvHsvk[j] = P_0.wBuAMWywHHZTpzMcCqwOWIvvHsvk[j];
				}
				for (int k = 0; k < MathTools.Min(tGgBIoQaPDUpWHgeUomZUlPfJQYS.Length, P_0.tGgBIoQaPDUpWHgeUomZUlPfJQYS.Length); k++)
				{
					tGgBIoQaPDUpWHgeUomZUlPfJQYS[k] = P_0.tGgBIoQaPDUpWHgeUomZUlPfJQYS[k];
				}
				EalJrOdaPtFXkMkCNcXdFcsnBXMx = P_0.EalJrOdaPtFXkMkCNcXdFcsnBXMx;
			}
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			if (RGcnHgTfJdsyfegDhhPIIGoZZZRx)
			{
				bool[] array = DZHRXyxLRGXOsVmtvgYLtukKYKJe.yqNATulmpNqcrGqZkcEmAOMHzLyvA;
				int[] array2 = DZHRXyxLRGXOsVmtvgYLtukKYKJe.bqdIFfNDwwaOWKnvaZbevVyJnpGy;
				TUrlebxDSRTKZBjDFRTWLPVYEEbO(array, array2);
				GQxeilZGwaAVNcHJjnGazPDRMGpyA(array, array2);
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
			if (!RGcnHgTfJdsyfegDhhPIIGoZZZRx)
			{
				return;
			}
			if (HQGWwLrkWDUWhgpyXVdpjipBBNBv != dataUpdater.axisCount || RYxNJtDPWRbEOjknBCwrBwBcNONyB != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			for (int i = 0; i < HQGWwLrkWDUWhgpyXVdpjipBBNBv; i++)
			{
				dataUpdater.axisValues[i] = tGgBIoQaPDUpWHgeUomZUlPfJQYS[i];
			}
			for (int j = 0; j < RYxNJtDPWRbEOjknBCwrBwBcNONyB; j++)
			{
				if (wBuAMWywHHZTpzMcCqwOWIvvHsvk[j])
				{
					dataUpdater.buttonPressureValues[j] = lHpDiTTCrmkXPhWrjQUsFkQSALbp[j];
				}
				else
				{
					dataUpdater.buttonValues[j] = lHpDiTTCrmkXPhWrjQUsFkQSALbp[j] > 0f;
				}
			}
			if (EalJrOdaPtFXkMkCNcXdFcsnBXMx && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		void IInputManagerJoystick.FillData(ControllerDataUpdater dataUpdater)
		{
			//ILSpy generated this explicit interface implementation from .override directive in FillData
			this.FillData(dataUpdater);
		}

		public int hcTsMltMZFRQPhZCrWAbvtCIFqhb(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0)
		{
			if (!RGcnHgTfJdsyfegDhhPIIGoZZZRx)
			{
				return 0;
			}
			if (P_0.sSzrszycPOlKlaWMmGVQNbduVkTP == sSzrszycPOlKlaWMmGVQNbduVkTP)
			{
				return 2;
			}
			if (eyKEXEvzpXuagpJhbMJksTcowmYX != P_0.eyKEXEvzpXuagpJhbMJksTcowmYX)
			{
				return 0;
			}
			if (JJAEtoXSWDTTQIwxmmcrINdFEjsC != P_0.JJAEtoXSWDTTQIwxmmcrINdFEjsC)
			{
				return 0;
			}
			if (PaAdzljaxFJFWEaUsOrabIGDajIdA != P_0.PaAdzljaxFJFWEaUsOrabIGDajIdA)
			{
				return 0;
			}
			if (lIOtHasIxEmGWFdDtIUzoOipIQNg != P_0.lIOtHasIxEmGWFdDtIUzoOipIQNg)
			{
				return 0;
			}
			if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid == Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid)
			{
				return 2;
			}
			if (P_0.YdQfdsSzVZHuCBcAgLOAUtTXGuGx == YdQfdsSzVZHuCBcAgLOAUtTXGuGx)
			{
				return 1;
			}
			return 0;
		}

		private BridgedControllerHWInfo WlSClVwZoGXcnFBsqITUXryvTgdd()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
			udCNRVtPUVOFiSNwcDFMdTrdtBRL(bridgedControllerHWInfo);
			return bridgedControllerHWInfo;
		}

		[CustomObfuscation(rename = false)]
		public BridgedController ToBridgedController()
		{
			if (!RGcnHgTfJdsyfegDhhPIIGoZZZRx)
			{
				return null;
			}
			BridgedController bridgedController = new BridgedController();
			lIujdrNmMXSUnpANZanvHTmQHfwe(bridgedController);
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
			return new ControllerDisconnectedEventArgs(sSzrszycPOlKlaWMmGVQNbduVkTP);
		}

		ControllerDisconnectedEventArgs IInputManagerJoystick.ToControllerDisconnectedEventArgs()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToControllerDisconnectedEventArgs
			return this.ToControllerDisconnectedEventArgs();
		}

		private void TUrlebxDSRTKZBjDFRTWLPVYEEbO(bool[] P_0, int[] P_1)
		{
			if (HQGWwLrkWDUWhgpyXVdpjipBBNBv <= 0)
			{
				return;
			}
			switch (FObUmlaHPpIaGAIwrbCGcZTaTAHh.map.platform)
			{
			case InputPlatform.WindowsRawInput:
			{
				HardwareJoystickMap.Platform_RawInput_Base.Axis[] axes_orig3 = ((HardwareJoystickMap.Platform_RawInput_Base)FObUmlaHPpIaGAIwrbCGcZTaTAHh.map).Axes_orig;
				if (axes_orig3 != null)
				{
					for (int k = 0; k < axes_orig3.Length; k++)
					{
						ByJTgfZqNSAowizLdxdepHUbDtHvA(axes_orig3[k], k, P_0, P_1);
					}
				}
				break;
			}
			case InputPlatform.WindowsDirectInput:
			{
				HardwareJoystickMap.Platform_DirectInput_Base.Axis[] axes_orig2 = ((HardwareJoystickMap.Platform_DirectInput_Base)FObUmlaHPpIaGAIwrbCGcZTaTAHh.map).Axes_orig;
				if (axes_orig2 != null)
				{
					for (int j = 0; j < axes_orig2.Length; j++)
					{
						ByJTgfZqNSAowizLdxdepHUbDtHvA(axes_orig2[j], j, P_0, P_1);
					}
				}
				break;
			}
			case InputPlatform.InternalDriver:
			{
				HardwareJoystickMap.Platform_InternalDriver_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_InternalDriver_Base)FObUmlaHPpIaGAIwrbCGcZTaTAHh.map).Axes_orig;
				if (axes_orig != null)
				{
					for (int i = 0; i < axes_orig.Length; i++)
					{
						xUZAgRblzaKMXcuANoDxyiZdjdLt(axes_orig[i], i, P_0, P_1);
					}
				}
				break;
			}
			}
		}

		private void GQxeilZGwaAVNcHJjnGazPDRMGpyA(bool[] P_0, int[] P_1)
		{
			if (RYxNJtDPWRbEOjknBCwrBwBcNONyB <= 0)
			{
				return;
			}
			switch (FObUmlaHPpIaGAIwrbCGcZTaTAHh.map.platform)
			{
			case InputPlatform.WindowsRawInput:
			{
				HardwareJoystickMap.Platform_RawInput_Base.Button[] buttons_orig3 = ((HardwareJoystickMap.Platform_RawInput_Base)FObUmlaHPpIaGAIwrbCGcZTaTAHh.map).Buttons_orig;
				if (buttons_orig3 != null)
				{
					for (int k = 0; k < buttons_orig3.Length; k++)
					{
						qjTAMgQoLYmYjSjAZULVSJpwrsTK(buttons_orig3[k], k, P_0, P_1);
					}
				}
				break;
			}
			case InputPlatform.WindowsDirectInput:
			{
				HardwareJoystickMap.Platform_DirectInput_Base.Button[] buttons_orig2 = ((HardwareJoystickMap.Platform_DirectInput_Base)FObUmlaHPpIaGAIwrbCGcZTaTAHh.map).Buttons_orig;
				if (buttons_orig2 != null)
				{
					for (int j = 0; j < buttons_orig2.Length; j++)
					{
						qjTAMgQoLYmYjSjAZULVSJpwrsTK(buttons_orig2[j], j, P_0, P_1);
					}
				}
				break;
			}
			case InputPlatform.InternalDriver:
			{
				HardwareJoystickMap.Platform_InternalDriver_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_InternalDriver_Base)FObUmlaHPpIaGAIwrbCGcZTaTAHh.map).Buttons_orig;
				if (buttons_orig != null)
				{
					for (int i = 0; i < buttons_orig.Length; i++)
					{
						AcNHFDPmNuAQJuioFGFOUnHwgCZQ(buttons_orig[i], i, P_0, P_1);
					}
				}
				break;
			}
			}
		}

		private void ByJTgfZqNSAowizLdxdepHUbDtHvA(HardwareJoystickMap.Platform_RawOrDirectInput.Axis_Base P_0, int P_1, bool[] P_2, int[] P_3)
		{
			if (P_1 >= HQGWwLrkWDUWhgpyXVdpjipBBNBv)
			{
				throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
			}
			tGgBIoQaPDUpWHgeUomZUlPfJQYS[P_1] = IpzyAseSAsQVPIKBnjuRcJEDGnZm(P_0, P_2, P_3);
			if (!EalJrOdaPtFXkMkCNcXdFcsnBXMx && tGgBIoQaPDUpWHgeUomZUlPfJQYS[P_1] != 0f)
			{
				EalJrOdaPtFXkMkCNcXdFcsnBXMx = true;
			}
		}

		private void qjTAMgQoLYmYjSjAZULVSJpwrsTK(HardwareJoystickMap.Platform_RawOrDirectInput.Button_Base P_0, int P_1, bool[] P_2, int[] P_3)
		{
			if (P_1 >= RYxNJtDPWRbEOjknBCwrBwBcNONyB)
			{
				throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
			}
			lHpDiTTCrmkXPhWrjQUsFkQSALbp[P_1] = YXiUipTSskLFJULYVPcLTNzyXJW(P_0, P_2, P_3);
			if (!EalJrOdaPtFXkMkCNcXdFcsnBXMx && lHpDiTTCrmkXPhWrjQUsFkQSALbp[P_1] != 0f)
			{
				EalJrOdaPtFXkMkCNcXdFcsnBXMx = true;
			}
		}

		private float IpzyAseSAsQVPIKBnjuRcJEDGnZm(HardwareJoystickMap.Platform_RawOrDirectInput.Axis_Base P_0, bool[] P_1, int[] P_2)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				int sourceAxis = P_0.sourceAxis;
				int num;
				switch (sourceAxis)
				{
				case 0:
					return 0f;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					num = 0;
					break;
				default:
					if (sourceAxis == 1000)
					{
						if (!(P_0 is HardwareJoystickMap.Platform_RawInput_Base.Axis axis))
						{
							return 0f;
						}
						num = axis.sourceOtherAxis;
						break;
					}
					return 0f;
				}
				return spmhOImXlCvCQGEpMRMAPZNHxfut((RawInputAxis)sourceAxis, num);
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= JJAEtoXSWDTTQIwxmmcrINdFEjsC || sourceButton >= 256)
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
				if (sourceHat < 0 || sourceHat >= PaAdzljaxFJFWEaUsOrabIGDajIdA || sourceHat >= 4)
				{
					return 0f;
				}
				int num2 = P_2[sourceHat];
				if (num2 < 0)
				{
					return 0f;
				}
				float num3;
				if (P_0.sourceHatDirection == AxisDirection.Horizontal)
				{
					num3 = xjNItWnhgsQVpOvVPRyzGRbiViSl(num2, AxisDirection.Horizontal);
					if (P_0.sourceHatRange != AxisRange.Full)
					{
						if (P_0.sourceHatRange == AxisRange.Positive)
						{
							if (num3 < 0f)
							{
								return 0f;
							}
						}
						else if (num3 > 0f)
						{
							return 0f;
						}
					}
				}
				else
				{
					num3 = xjNItWnhgsQVpOvVPRyzGRbiViSl(num2, AxisDirection.Vertical);
					if (P_0.sourceHatRange != AxisRange.Full)
					{
						if (P_0.sourceHatRange == AxisRange.Positive)
						{
							if (num3 < 0f)
							{
								return 0f;
							}
						}
						else if (num3 > 0f)
						{
							return 0f;
						}
					}
				}
				if (P_0.invert)
				{
					num3 *= -1f;
				}
				return num3;
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
					if (customCalculationSourceData[i] != null && customCalculationSourceData[i].sourceType == 1 && UsBbIYSENBIaUDdjhebbIbUsqWEA(customCalculationSourceData[i], out var item))
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

		private float spmhOImXlCvCQGEpMRMAPZNHxfut(RawInputAxis P_0, int P_1)
		{
			return MubHsQEjrvDQvKIggYOjbfRsLPIK((vBHFnkHGTRBUValjumsbtLKWPPWIA as ooMJwxMVKWpFygFkttyIRbnIpFzV).uiYQrpyDzwfXgpSBreDzMReEEzCDA(P_0, P_1));
		}

		private float YXiUipTSskLFJULYVPcLTNzyXJW(HardwareJoystickMap.Platform_RawOrDirectInput.Button_Base P_0, bool[] P_1, int[] P_2)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				if (P_0.ignoreIfButtonsActive)
				{
					for (int i = 0; i < P_0.ignoreIfButtonsActiveButtons.Length; i++)
					{
						if (P_1[P_0.ignoreIfButtonsActiveButtons[i]])
						{
							return 0f;
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
							return 0f;
						}
						flag = true;
					}
					if (flag)
					{
						return 1f;
					}
					return 0f;
				}
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= JJAEtoXSWDTTQIwxmmcrINdFEjsC || sourceButton >= 256)
				{
					return 0f;
				}
				if (!P_1[sourceButton])
				{
					return 0f;
				}
				return 1f;
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				int sourceAxis = P_0.sourceAxis;
				int num;
				switch (sourceAxis)
				{
				case 0:
					return 0f;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					num = 0;
					break;
				default:
					if (sourceAxis == 1000)
					{
						if (!(P_0 is HardwareJoystickMap.Platform_RawInput_Base.Button button))
						{
							return 0f;
						}
						num = button.sourceOtherAxis;
						break;
					}
					return 0f;
				}
				float num2 = spmhOImXlCvCQGEpMRMAPZNHxfut((RawInputAxis)sourceAxis, num);
				float num3 = MathTools.Abs(num2);
				if (num3 <= P_0.axisDeadZone)
				{
					return 0f;
				}
				if (P_0.sourceAxisPole == Pole.Positive)
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
				return num3;
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Hat)
			{
				int sourceHat = P_0.sourceHat;
				if (sourceHat < 0 || sourceHat >= PaAdzljaxFJFWEaUsOrabIGDajIdA || sourceHat >= 4)
				{
					return 0f;
				}
				switch (P_0.sourceHatDirection)
				{
				case HatDirection.Up:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 0, P_0.sourceHatType);
				case HatDirection.UpRight:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 1, P_0.sourceHatType);
				case HatDirection.Right:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 2, P_0.sourceHatType);
				case HatDirection.DownRight:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 3, P_0.sourceHatType);
				case HatDirection.Down:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 4, P_0.sourceHatType);
				case HatDirection.DownLeft:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 5, P_0.sourceHatType);
				case HatDirection.Left:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 6, P_0.sourceHatType);
				case HatDirection.UpLeft:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 7, P_0.sourceHatType);
				}
			}
			else if (P_0.sourceType == HardwareElementSourceTypeWithHat.Custom)
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
						if (WJkzWKhVSMyoXJKKTLYyFGjGAcJj(customCalculationSourceData[k], P_1, out var flag2))
						{
							customCalculation.AddData(flag2 ? 1f : 0f);
						}
						break;
					}
					case HardwareElementSourceTypeWithHat.Axis:
					{
						if (UsBbIYSENBIaUDdjhebbIbUsqWEA(customCalculationSourceData[k], out var num4))
						{
							customCalculation.AddData((num4 != 0f) ? 1f : 0f);
						}
						break;
					}
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
				if ((float)customCalculation.Result == 0f)
				{
					return 0f;
				}
				return 1f;
			}
			return 0f;
		}

		private float MubHsQEjrvDQvKIggYOjbfRsLPIK(int P_0)
		{
			if (P_0 == 0)
			{
				return 0f;
			}
			return MathTools.Clamp((float)MathTools.Abs(P_0) / 65535f * (float)MathTools.Sign(P_0), -1f, 1f);
		}

		private float CaInUvqHwxHGePOdXcJEVMoQmSgh(int P_0, int P_1, HatType P_2)
		{
			if (P_0 < 0)
			{
				return 0f;
			}
			if (FObUmlaHPpIaGAIwrbCGcZTaTAHh.isUnknownController && !InputTools.HandleForced4WayHatsOnUnknownControllers(P_1, ref P_2))
			{
				return 0f;
			}
			int num = 4500 * P_1;
			if (P_2 == HatType.EightWay && P_0 != num)
			{
				return 0f;
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
				return 1f;
			}
			return 0f;
		}

		private float xjNItWnhgsQVpOvVPRyzGRbiViSl(int P_0, AxisDirection P_1)
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

		private bool WJkzWKhVSMyoXJKKTLYyFGjGAcJj(HardwareJoystickMap.Platform_RawOrDirectInput.CustomCalculationSourceData P_0, bool[] P_1, out bool P_2)
		{
			P_2 = false;
			if (P_0.sourceType != 0)
			{
				return false;
			}
			int sourceButton = P_0.sourceButton;
			if (sourceButton < 0 || sourceButton >= JJAEtoXSWDTTQIwxmmcrINdFEjsC || sourceButton >= 256)
			{
				return false;
			}
			P_2 = P_1[sourceButton];
			return true;
		}

		private bool UsBbIYSENBIaUDdjhebbIbUsqWEA(HardwareJoystickMap.Platform_RawOrDirectInput.CustomCalculationSourceData P_0, out float P_1)
		{
			P_1 = 0f;
			if (P_0.sourceType != 1)
			{
				return false;
			}
			if (P_0.sourceAxis == 0)
			{
				return false;
			}
			P_1 = spmhOImXlCvCQGEpMRMAPZNHxfut((RawInputAxis)P_0.sourceAxis, P_0.sourceOtherAxis);
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

		private ControlDeviceType HUziQVImsCaHEjrcAEgIKqbegRjIA(DeviceType P_0)
		{
			return P_0 switch
			{
				DeviceType.Keyboard => ControlDeviceType.Keyboard, 
				DeviceType.Joystick => ControlDeviceType.Joystick, 
				DeviceType.Gamepad => ControlDeviceType.Gamepad, 
				DeviceType.Mouse => ControlDeviceType.Mouse, 
				DeviceType.MultiAxisController => ControlDeviceType.Joystick, 
				_ => ControlDeviceType.Unknown, 
			};
		}

		private void xUZAgRblzaKMXcuANoDxyiZdjdLt(HardwareJoystickMap.Platform_InternalDriver_Base.Axis P_0, int P_1, bool[] P_2, int[] P_3)
		{
			if (P_1 >= HQGWwLrkWDUWhgpyXVdpjipBBNBv)
			{
				throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
			}
			tGgBIoQaPDUpWHgeUomZUlPfJQYS[P_1] = qovnsJvFykxpIUlSbzMXHgmrIKTJA(P_0, P_2, P_3);
			if (!EalJrOdaPtFXkMkCNcXdFcsnBXMx && tGgBIoQaPDUpWHgeUomZUlPfJQYS[P_1] != 0f)
			{
				EalJrOdaPtFXkMkCNcXdFcsnBXMx = true;
			}
		}

		private void AcNHFDPmNuAQJuioFGFOUnHwgCZQ(HardwareJoystickMap.Platform_InternalDriver_Base.Button P_0, int P_1, bool[] P_2, int[] P_3)
		{
			if (P_1 >= RYxNJtDPWRbEOjknBCwrBwBcNONyB)
			{
				throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
			}
			lHpDiTTCrmkXPhWrjQUsFkQSALbp[P_1] = BeXwTeFxMvGeFDVupZXaFqbCQlHJ(P_0, P_2, P_3);
			if (!EalJrOdaPtFXkMkCNcXdFcsnBXMx && lHpDiTTCrmkXPhWrjQUsFkQSALbp[P_1] != 0f)
			{
				EalJrOdaPtFXkMkCNcXdFcsnBXMx = true;
			}
		}

		private float qovnsJvFykxpIUlSbzMXHgmrIKTJA(HardwareJoystickMap.Platform_InternalDriver_Base.Axis P_0, bool[] P_1, int[] P_2)
		{
			if (P_0.sourceType == 1)
			{
				int sourceAxis = P_0.sourceAxis;
				if (sourceAxis < 0 || sourceAxis >= eyKEXEvzpXuagpJhbMJksTcowmYX || sourceAxis >= 56)
				{
					return 0f;
				}
				return ZvVEeVkbPSLzKdfVZGBxakhdzvLbb(sourceAxis);
			}
			if (P_0.sourceType == 0)
			{
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= JJAEtoXSWDTTQIwxmmcrINdFEjsC || sourceButton >= 256)
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
			if (P_0.sourceType == 2)
			{
				int sourceHat = P_0.sourceHat;
				if (sourceHat < 0 || sourceHat >= PaAdzljaxFJFWEaUsOrabIGDajIdA || sourceHat >= 4)
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
					num2 = xjNItWnhgsQVpOvVPRyzGRbiViSl(num, AxisDirection.Horizontal);
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
					num2 = xjNItWnhgsQVpOvVPRyzGRbiViSl(num, AxisDirection.Vertical);
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

		private float ZvVEeVkbPSLzKdfVZGBxakhdzvLbb(int P_0)
		{
			return (vBHFnkHGTRBUValjumsbtLKWPPWIA as tSoAuFePUqEfqeHjHNvjyrcIaCSLA).IcMCQfMtfMCEJnpccEDXKUPAkKMM(P_0);
		}

		private float BeXwTeFxMvGeFDVupZXaFqbCQlHJ(HardwareJoystickMap.Platform_InternalDriver_Base.Button P_0, bool[] P_1, int[] P_2)
		{
			if (P_0.sourceType == 0)
			{
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= JJAEtoXSWDTTQIwxmmcrINdFEjsC || sourceButton >= 256)
				{
					return 0f;
				}
				if (!P_1[sourceButton])
				{
					return 0f;
				}
				return 1f;
			}
			if (P_0.sourceType == 1)
			{
				int sourceAxis = P_0.sourceAxis;
				if (sourceAxis < 0 || sourceAxis >= eyKEXEvzpXuagpJhbMJksTcowmYX || sourceAxis >= 56)
				{
					return 0f;
				}
				float num = ZvVEeVkbPSLzKdfVZGBxakhdzvLbb(sourceAxis);
				if (MathTools.Abs(num) <= P_0.axisDeadZone)
				{
					return 0f;
				}
				if (P_0.sourceAxisPole == Pole.Positive)
				{
					if (num < 0f)
					{
						return 0f;
					}
				}
				else if (num > 0f)
				{
					return 0f;
				}
				return 1f;
			}
			if (P_0.sourceType == 2)
			{
				int sourceHat = P_0.sourceHat;
				if (sourceHat < 0 || sourceHat >= PaAdzljaxFJFWEaUsOrabIGDajIdA || sourceHat >= 4)
				{
					return 0f;
				}
				switch (P_0.sourceHatDirection)
				{
				case HatDirection.Up:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 0, P_0.sourceHatType);
				case HatDirection.UpRight:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 1, P_0.sourceHatType);
				case HatDirection.Right:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 2, P_0.sourceHatType);
				case HatDirection.DownRight:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 3, P_0.sourceHatType);
				case HatDirection.Down:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 4, P_0.sourceHatType);
				case HatDirection.DownLeft:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 5, P_0.sourceHatType);
				case HatDirection.Left:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 6, P_0.sourceHatType);
				case HatDirection.UpLeft:
					return CaInUvqHwxHGePOdXcJEVMoQmSgh(P_2[sourceHat], 7, P_0.sourceHatType);
				}
			}
			return 0f;
		}

		private void XvTxAHgidKWgMhDSXgjZwjPgcRtp()
		{
			FObUmlaHPpIaGAIwrbCGcZTaTAHh = ZKVLrjzxNmLDpClnIlvbtaXBwuZN(WlSClVwZoGXcnFBsqITUXryvTgdd());
			if (FObUmlaHPpIaGAIwrbCGcZTaTAHh == null)
			{
				Logger.LogError("Default hardware map not found!");
				return;
			}
			HQGWwLrkWDUWhgpyXVdpjipBBNBv = FObUmlaHPpIaGAIwrbCGcZTaTAHh.axisCount;
			RYxNJtDPWRbEOjknBCwrBwBcNONyB = FObUmlaHPpIaGAIwrbCGcZTaTAHh.buttonCount;
		}

		private string DOiKETLPPXFhpyCKmEmauiAzVlom()
		{
			return InputTools.FormatHardwareIdentifierString(string.Format("{0}{1}{2}{3}{4}", ReInput.currentPlatform.ToString(), DZHRXyxLRGXOsVmtvgYLtukKYKJe.qewWBJAUwEmEEDlsUDbXGhtGLNZJA, (XIrQMhERePtFjYZAwnZzDkPPlChx && !string.IsNullOrEmpty(QmsrlWmtWGTlUGRfcLtxLyjqEXCk)) ? QmsrlWmtWGTlUGRfcLtxLyjqEXCk : jdKiFeajMWZALcqxhnovacouhDpab, AXLReMpRMhWwqczNZdqsTGQVZncq.ToString("X4"), ZDewcBMTjUbsYXeQDkjNshaSOduJ.ToString("X4")));
		}

		private void udCNRVtPUVOFiSNwcDFMdTrdtBRL(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.RawInput;
			P_0.inputSource = DZHRXyxLRGXOsVmtvgYLtukKYKJe.qewWBJAUwEmEEDlsUDbXGhtGLNZJA;
			P_0.deviceType = HUziQVImsCaHEjrcAEgIKqbegRjIA(bOXRSHunCqisPQNkJYZoOpeUBoI);
			P_0.hardwareIdentifier = DOiKETLPPXFhpyCKmEmauiAzVlom();
			P_0.hardwareAxisCount = eyKEXEvzpXuagpJhbMJksTcowmYX;
			P_0.hardwareButtonCount = JJAEtoXSWDTTQIwxmmcrINdFEjsC;
			P_0.hardwareHatCount = PaAdzljaxFJFWEaUsOrabIGDajIdA;
			P_0.hw_productName = jdKiFeajMWZALcqxhnovacouhDpab;
			P_0.hw_deviceGuid = Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;
			P_0.hw_vendorId = ZDewcBMTjUbsYXeQDkjNshaSOduJ;
			P_0.hw_productId = AXLReMpRMhWwqczNZdqsTGQVZncq;
			P_0.hw_pidVid = new PidVid(OjIFPJdqkPIULfMkjMZGNcpHsJTVA);
			P_0.hw_isBluetoothDevice = XIrQMhERePtFjYZAwnZzDkPPlChx;
			P_0.hw_bluetoothDeviceName = QmsrlWmtWGTlUGRfcLtxLyjqEXCk;
			P_0.hw_supportsVibration = eQLIlFhSBmzVfhEJUpSBzPmEmndv;
			P_0.hw_localVibrationMotorCount = ETMQmyjBbOoUCMrtSbvLFxZuEFDbA;
			P_0.definitionMatchTag = DZHRXyxLRGXOsVmtvgYLtukKYKJe.LdxZRdaLiSAEHoTjwjPxxufWHKOHA;
		}

		private void lIujdrNmMXSUnpANZanvHTmQHfwe(BridgedController P_0)
		{
			udCNRVtPUVOFiSNwcDFMdTrdtBRL(P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = FObUmlaHPpIaGAIwrbCGcZTaTAHh.ToGameHardwareControllerMap();
			P_0.instanceName = FzydYXcZAaAycEHYIJIohPgTxBeJ;
			P_0.productName = jdKiFeajMWZALcqxhnovacouhDpab;
			P_0.isXInputDevice = TjYPbLYxwDISjQqbvtvGMQkgVTVo;
			P_0.axisCount = HQGWwLrkWDUWhgpyXVdpjipBBNBv;
			P_0.buttonCount = RYxNJtDPWRbEOjknBCwrBwBcNONyB;
			P_0.isButtonPressureSensitive = new bool[RYxNJtDPWRbEOjknBCwrBwBcNONyB];
			Array.Copy(wBuAMWywHHZTpzMcCqwOWIvvHsvk, P_0.isButtonPressureSensitive, RYxNJtDPWRbEOjknBCwrBwBcNONyB);
			P_0.unknownControllerHats = MFibjLCKTnfpnByhFgFlHBPtAcdPc();
			P_0.controllerTypeGuid = DLimkmRsoQHzrNFaPQMFiFhLChxeA;
			P_0.controllerExtension = Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Eextension;
		}

		private UnknownControllerHat[] MFibjLCKTnfpnByhFgFlHBPtAcdPc()
		{
			if (!lqZPNguEZjLlsYDnyWcTqMiHaIgU)
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

		public void zVCzgtFYiOaXdzRCQpIGMmhxgMWY()
		{
			wbKboHENShrvHdfuLPocLNTXdDmWA(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void XypxRKmhqOJDBMqRSDDntvJNgpfv()
		{
			try
			{
				wbKboHENShrvHdfuLPocLNTXdDmWA(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void wbKboHENShrvHdfuLPocLNTXdDmWA(bool P_0)
		{
			if (!UhJyouJlvhwxVMBqhmZOwvqkTeEs)
			{
				UhJyouJlvhwxVMBqhmZOwvqkTeEs = true;
			}
		}

		public static int UHxnclEPdAfilgRfhPhnhzsbJoQLb(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0, mgmlbBRQFaeoqiMIcNTspgMYlddBb P_1)
		{
			if (P_0.QlWFrLHgojFQBiRHAaBxVmnTKkvW < P_1.QlWFrLHgojFQBiRHAaBxVmnTKkvW)
			{
				return -1;
			}
			if (P_0.QlWFrLHgojFQBiRHAaBxVmnTKkvW > P_1.QlWFrLHgojFQBiRHAaBxVmnTKkvW)
			{
				return 1;
			}
			return 0;
		}

		public static int TgwBckwVPTQrFSamDOBOXmOUSIaP(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0, mgmlbBRQFaeoqiMIcNTspgMYlddBb P_1)
		{
			if (P_0.sFJRxDuicBykGWtzbbImKRHjuLBi < P_1.sFJRxDuicBykGWtzbbImKRHjuLBi)
			{
				return -1;
			}
			if (P_0.sFJRxDuicBykGWtzbbImKRHjuLBi > P_1.sFJRxDuicBykGWtzbbImKRHjuLBi)
			{
				return 1;
			}
			return 0;
		}
	}

	private class GSyjoFWrTdCkzcNRLgLTeMdPSDLQ
	{
		public enum iAvddRwBtmTSVsgWPUiRjTuLUrgk
		{
			Exact = 0,
			Approximate = 1
		}

		public class sbenqLpovPBasXaYIRDjMsZGAKKC
		{
			public int BnkyUIWVDlhHjhQGmuAQGRDLELTo;

			public Guid HFTRRaKPhpwJfCfGkcyBhJHEzWny;

			public Guid gmHjVzKaEZVxhvjjhRcdPgjCdEhj;

			public int dKFoUuqovuSRRvdrWeHXfzUMGHeBA;

			public int KuFEEofnVaOkuESeEhHBmhtIUyzjb;

			public int yUCdgQVimjDClBkWLFtHCgpPNKoP;

			public int pJRojZBGwAjLByogiSvwVlXkucUA;

			public int DcfwtVuQpUSrtmTQrGqYUBbpBpqK;

			public int mdCsDMhfmJZJQWMKUiOIiUrrbmte;

			public bool KIUqkrLPLsDmqdzDZbuCDUoRblBbb;

			public bool GfUBIMZQkhzTDmoVgZtKrwgVpCmU(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0, iAvddRwBtmTSVsgWPUiRjTuLUrgk P_1)
			{
				if (KuFEEofnVaOkuESeEhHBmhtIUyzjb != P_0.eyKEXEvzpXuagpJhbMJksTcowmYX)
				{
					return false;
				}
				if (yUCdgQVimjDClBkWLFtHCgpPNKoP != P_0.JJAEtoXSWDTTQIwxmmcrINdFEjsC)
				{
					return false;
				}
				if (pJRojZBGwAjLByogiSvwVlXkucUA != P_0.PaAdzljaxFJFWEaUsOrabIGDajIdA)
				{
					return false;
				}
				if (DcfwtVuQpUSrtmTQrGqYUBbpBpqK != P_0.RYxNJtDPWRbEOjknBCwrBwBcNONyB)
				{
					return false;
				}
				if (mdCsDMhfmJZJQWMKUiOIiUrrbmte != P_0.HQGWwLrkWDUWhgpyXVdpjipBBNBv)
				{
					return false;
				}
				if (KIUqkrLPLsDmqdzDZbuCDUoRblBbb != P_0.lIOtHasIxEmGWFdDtIUzoOipIQNg)
				{
					return false;
				}
				if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == BnkyUIWVDlhHjhQGmuAQGRDLELTo)
				{
					return true;
				}
				return P_1 switch
				{
					iAvddRwBtmTSVsgWPUiRjTuLUrgk.Exact => HFTRRaKPhpwJfCfGkcyBhJHEzWny == P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid, 
					iAvddRwBtmTSVsgWPUiRjTuLUrgk.Approximate => gmHjVzKaEZVxhvjjhRcdPgjCdEhj == P_0.YdQfdsSzVZHuCBcAgLOAUtTXGuGx, 
					_ => throw new NotImplementedException(), 
				};
			}

			public virtual string nUFmksNCjPlAPFdVrLoKgHfJnERm()
			{
				string text = "" + "rewiredId = " + BnkyUIWVDlhHjhQGmuAQGRDLELTo + "\n";
				Guid hFTRRaKPhpwJfCfGkcyBhJHEzWny = HFTRRaKPhpwJfCfGkcyBhJHEzWny;
				string text2 = text + "instanceGuid = " + hFTRRaKPhpwJfCfGkcyBhJHEzWny.ToString() + "\n";
				hFTRRaKPhpwJfCfGkcyBhJHEzWny = gmHjVzKaEZVxhvjjhRcdPgjCdEhj;
				return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(text2 + "typeIdentifierGuid = " + hFTRRaKPhpwJfCfGkcyBhJHEzWny.ToString() + "\n", "lastInputManagerId = ", dKFoUuqovuSRRvdrWeHXfzUMGHeBA.ToString(), "\n"), "hardwareAxisCount = ", KuFEEofnVaOkuESeEhHBmhtIUyzjb.ToString(), "\n"), "hardwareButtonCount = ", yUCdgQVimjDClBkWLFtHCgpPNKoP.ToString(), "\n"), "hardwareHatCount = ", pJRojZBGwAjLByogiSvwVlXkucUA.ToString(), "\n"), "gameButtonCount = ", DcfwtVuQpUSrtmTQrGqYUBbpBpqK.ToString(), "\n"), "gameAxisCount = ", mdCsDMhfmJZJQWMKUiOIiUrrbmte.ToString(), "\n"), "hasDriver = ", KIUqkrLPLsDmqdzDZbuCDUoRblBbb.ToString(), "\n");
			}
		}

		private sealed class amoAMMgoVCtQdsaSKDtrAjKFvouSb : IEnumerable<sbenqLpovPBasXaYIRDjMsZGAKKC>, IEnumerable, IEnumerator<sbenqLpovPBasXaYIRDjMsZGAKKC>, IEnumerator, IDisposable
		{
			private int WZMGFIZknHCiqWAfeTuwTTQElPsX;

			private sbenqLpovPBasXaYIRDjMsZGAKKC xRFAIWyFDROtryhZekovGddPSGpp;

			private int HKFjrXCCnZIkDsJqCvthAbWCIwSR;

			public GSyjoFWrTdCkzcNRLgLTeMdPSDLQ oiFmDuzCrjBoGvhWimeWBOKBukUh;

			private mgmlbBRQFaeoqiMIcNTspgMYlddBb CTxIPzWRbqnRhMHytIBRckWGopDy;

			public mgmlbBRQFaeoqiMIcNTspgMYlddBb cnsvGouExZJQkTOkpttFjVjwgtBD;

			private iAvddRwBtmTSVsgWPUiRjTuLUrgk NKNwSEXQGztWbAXKXKhDgSqArpRX;

			public iAvddRwBtmTSVsgWPUiRjTuLUrgk kEsCRTxrCszaBxjnGiYJDxtPuNim;

			private int dlipdnsDfcNscxIpYHFTPJYJqDEV;

			private int JQIwuNhEQFGvIengtXzlDeyxZklFA;

			sbenqLpovPBasXaYIRDjMsZGAKKC IEnumerator<sbenqLpovPBasXaYIRDjMsZGAKKC>.Current
			{
				[DebuggerHidden]
				get
				{
					return xRFAIWyFDROtryhZekovGddPSGpp;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return xRFAIWyFDROtryhZekovGddPSGpp;
				}
			}

			[DebuggerHidden]
			public amoAMMgoVCtQdsaSKDtrAjKFvouSb(int P_0)
			{
				WZMGFIZknHCiqWAfeTuwTTQElPsX = P_0;
				HKFjrXCCnZIkDsJqCvthAbWCIwSR = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				WZMGFIZknHCiqWAfeTuwTTQElPsX = -2;
			}

			private bool MoveNext()
			{
				int wZMGFIZknHCiqWAfeTuwTTQElPsX = WZMGFIZknHCiqWAfeTuwTTQElPsX;
				GSyjoFWrTdCkzcNRLgLTeMdPSDLQ gSyjoFWrTdCkzcNRLgLTeMdPSDLQ = oiFmDuzCrjBoGvhWimeWBOKBukUh;
				if (wZMGFIZknHCiqWAfeTuwTTQElPsX != 0)
				{
					if (wZMGFIZknHCiqWAfeTuwTTQElPsX != 1)
					{
						return false;
					}
					WZMGFIZknHCiqWAfeTuwTTQElPsX = -1;
					goto IL_0083;
				}
				WZMGFIZknHCiqWAfeTuwTTQElPsX = -1;
				dlipdnsDfcNscxIpYHFTPJYJqDEV = gSyjoFWrTdCkzcNRLgLTeMdPSDLQ.VYFoHoVKjBKemxmUfoRmDGBNXzQt.Count;
				JQIwuNhEQFGvIengtXzlDeyxZklFA = 0;
				goto IL_0093;
				IL_0083:
				JQIwuNhEQFGvIengtXzlDeyxZklFA++;
				goto IL_0093;
				IL_0093:
				if (JQIwuNhEQFGvIengtXzlDeyxZklFA < dlipdnsDfcNscxIpYHFTPJYJqDEV)
				{
					if (gSyjoFWrTdCkzcNRLgLTeMdPSDLQ.VYFoHoVKjBKemxmUfoRmDGBNXzQt[JQIwuNhEQFGvIengtXzlDeyxZklFA].GfUBIMZQkhzTDmoVgZtKrwgVpCmU(CTxIPzWRbqnRhMHytIBRckWGopDy, NKNwSEXQGztWbAXKXKhDgSqArpRX))
					{
						xRFAIWyFDROtryhZekovGddPSGpp = gSyjoFWrTdCkzcNRLgLTeMdPSDLQ.VYFoHoVKjBKemxmUfoRmDGBNXzQt[JQIwuNhEQFGvIengtXzlDeyxZklFA];
						WZMGFIZknHCiqWAfeTuwTTQElPsX = 1;
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
			IEnumerator<sbenqLpovPBasXaYIRDjMsZGAKKC> IEnumerable<sbenqLpovPBasXaYIRDjMsZGAKKC>.GetEnumerator()
			{
				amoAMMgoVCtQdsaSKDtrAjKFvouSb amoAMMgoVCtQdsaSKDtrAjKFvouSb2;
				if (WZMGFIZknHCiqWAfeTuwTTQElPsX == -2 && HKFjrXCCnZIkDsJqCvthAbWCIwSR == Environment.CurrentManagedThreadId)
				{
					WZMGFIZknHCiqWAfeTuwTTQElPsX = 0;
					amoAMMgoVCtQdsaSKDtrAjKFvouSb2 = this;
				}
				else
				{
					amoAMMgoVCtQdsaSKDtrAjKFvouSb2 = new amoAMMgoVCtQdsaSKDtrAjKFvouSb(0);
					amoAMMgoVCtQdsaSKDtrAjKFvouSb2.oiFmDuzCrjBoGvhWimeWBOKBukUh = oiFmDuzCrjBoGvhWimeWBOKBukUh;
				}
				amoAMMgoVCtQdsaSKDtrAjKFvouSb2.CTxIPzWRbqnRhMHytIBRckWGopDy = cnsvGouExZJQkTOkpttFjVjwgtBD;
				amoAMMgoVCtQdsaSKDtrAjKFvouSb2.NKNwSEXQGztWbAXKXKhDgSqArpRX = kEsCRTxrCszaBxjnGiYJDxtPuNim;
				return amoAMMgoVCtQdsaSKDtrAjKFvouSb2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<sbenqLpovPBasXaYIRDjMsZGAKKC>)this).GetEnumerator();
			}
		}

		private List<sbenqLpovPBasXaYIRDjMsZGAKKC> VYFoHoVKjBKemxmUfoRmDGBNXzQt;

		public GSyjoFWrTdCkzcNRLgLTeMdPSDLQ()
		{
			VYFoHoVKjBKemxmUfoRmDGBNXzQt = new List<sbenqLpovPBasXaYIRDjMsZGAKKC>();
		}

		public void gXveIzYJnCpdkqhmRmkWklaIIMVW(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			int count = VYFoHoVKjBKemxmUfoRmDGBNXzQt.Count;
			for (int i = 0; i < count; i++)
			{
				if (VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].GfUBIMZQkhzTDmoVgZtKrwgVpCmU(P_0, iAvddRwBtmTSVsgWPUiRjTuLUrgk.Exact))
				{
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].BnkyUIWVDlhHjhQGmuAQGRDLELTo = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].HFTRRaKPhpwJfCfGkcyBhJHEzWny = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].gmHjVzKaEZVxhvjjhRcdPgjCdEhj = P_0.YdQfdsSzVZHuCBcAgLOAUtTXGuGx;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].dKFoUuqovuSRRvdrWeHXfzUMGHeBA = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].KuFEEofnVaOkuESeEhHBmhtIUyzjb = P_0.eyKEXEvzpXuagpJhbMJksTcowmYX;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].yUCdgQVimjDClBkWLFtHCgpPNKoP = P_0.JJAEtoXSWDTTQIwxmmcrINdFEjsC;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].pJRojZBGwAjLByogiSvwVlXkucUA = P_0.PaAdzljaxFJFWEaUsOrabIGDajIdA;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].DcfwtVuQpUSrtmTQrGqYUBbpBpqK = P_0.RYxNJtDPWRbEOjknBCwrBwBcNONyB;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].mdCsDMhfmJZJQWMKUiOIiUrrbmte = P_0.HQGWwLrkWDUWhgpyXVdpjipBBNBv;
					VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].KIUqkrLPLsDmqdzDZbuCDUoRblBbb = P_0.lIOtHasIxEmGWFdDtIUzoOipIQNg;
					NhogjZeLDaBloVAnFBuQcidadSKu(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid, i);
					return;
				}
			}
			VYFoHoVKjBKemxmUfoRmDGBNXzQt.Add(new sbenqLpovPBasXaYIRDjMsZGAKKC
			{
				BnkyUIWVDlhHjhQGmuAQGRDLELTo = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId,
				HFTRRaKPhpwJfCfGkcyBhJHEzWny = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid,
				gmHjVzKaEZVxhvjjhRcdPgjCdEhj = P_0.YdQfdsSzVZHuCBcAgLOAUtTXGuGx,
				dKFoUuqovuSRRvdrWeHXfzUMGHeBA = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId,
				KuFEEofnVaOkuESeEhHBmhtIUyzjb = P_0.eyKEXEvzpXuagpJhbMJksTcowmYX,
				yUCdgQVimjDClBkWLFtHCgpPNKoP = P_0.JJAEtoXSWDTTQIwxmmcrINdFEjsC,
				pJRojZBGwAjLByogiSvwVlXkucUA = P_0.PaAdzljaxFJFWEaUsOrabIGDajIdA,
				DcfwtVuQpUSrtmTQrGqYUBbpBpqK = P_0.RYxNJtDPWRbEOjknBCwrBwBcNONyB,
				mdCsDMhfmJZJQWMKUiOIiUrrbmte = P_0.HQGWwLrkWDUWhgpyXVdpjipBBNBv,
				KIUqkrLPLsDmqdzDZbuCDUoRblBbb = P_0.lIOtHasIxEmGWFdDtIUzoOipIQNg
			});
			NhogjZeLDaBloVAnFBuQcidadSKu(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid, VYFoHoVKjBKemxmUfoRmDGBNXzQt.Count - 1);
		}

		[IteratorStateMachine(typeof(amoAMMgoVCtQdsaSKDtrAjKFvouSb))]
		public IEnumerable<sbenqLpovPBasXaYIRDjMsZGAKKC> jgOgQAadSGQzXcCMOqcQPgpOMiWnA(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0, iAvddRwBtmTSVsgWPUiRjTuLUrgk P_1)
		{
			return new amoAMMgoVCtQdsaSKDtrAjKFvouSb(-2)
			{
				oiFmDuzCrjBoGvhWimeWBOKBukUh = this,
				cnsvGouExZJQkTOkpttFjVjwgtBD = P_0,
				kEsCRTxrCszaBxjnGiYJDxtPuNim = P_1
			};
		}

		private void NhogjZeLDaBloVAnFBuQcidadSKu(int P_0, Guid P_1, int P_2)
		{
			for (int num = VYFoHoVKjBKemxmUfoRmDGBNXzQt.Count - 1; num >= 0; num--)
			{
				if (num != P_2 && (VYFoHoVKjBKemxmUfoRmDGBNXzQt[num].BnkyUIWVDlhHjhQGmuAQGRDLELTo == P_0 || VYFoHoVKjBKemxmUfoRmDGBNXzQt[num].HFTRRaKPhpwJfCfGkcyBhJHEzWny == P_1))
				{
					VYFoHoVKjBKemxmUfoRmDGBNXzQt.RemoveAt(num);
				}
			}
		}

		public virtual string OcxogIRIXMHJfxHMmiIicDBerrRz()
		{
			string text = "";
			text = text + "Joystick records: " + VYFoHoVKjBKemxmUfoRmDGBNXzQt.Count + "\n";
			for (int i = 0; i < VYFoHoVKjBKemxmUfoRmDGBNXzQt.Count; i++)
			{
				text = text + "Record " + i + ":\n";
				text = text + VYFoHoVKjBKemxmUfoRmDGBNXzQt[i].ToString() + "\n\n";
			}
			return text;
		}
	}

	private AuSBfxYAktMaNvbYMEDVwcjrEcXEA JaCMyZasfyDRcNCkePbOFEJMpRIs;

	private List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> NmYAcdHiwzXdFtBVvSAMtHGDQfFr;

	private int quCOYogNGCoKTRyJjYArxyflxCqB;

	private GSyjoFWrTdCkzcNRLgLTeMdPSDLQ jTJexdyZMVjIKEKwatcPAoRlKcqS;

	private bool WgoAOOCjEBpExflXlogOovhuanjZ;

	private TimerRealTime vQuqAkVexDBTCgzitMjBIRwVQQzVA;

	private global::gxiqikdujHobFnpuuAGPldgYhOvdA<bool> zMfZVZJudYruBOoCmSgZbUMxwhFB;

	private global::gxiqikdujHobFnpuuAGPldgYhOvdA<bool> jyfcyjxQLvCxGERUmiZGFsVAKDRIb;

	private int fJaodxcaFSZTSCjdOeDtIhGxKdXY;

	private int ftGWwIBfmWOyygYRTPUErslsyuUl;

	private ConfigVars KxYHLNbDJrTFWxMZnHrLpPiqrnbD;

	private DDWjNWYAVnuAprLJBexwaxtfkBnQA ytAMSILErDcsJgMLUVwVBTboKSutA;

	private Action<int, ControllerDataUpdater> BmFqKKZtUkaOlSoiIvDQXoXtkihk;

	private PlatformInputManager RNEsAapNgFgKizyAZboyUkTlxiRU;

	private readonly dXKLvQGSIvoiwbPpSGgTOgwmLqzq IodKrouyOIdWOfUxyLLjmlVtEcuw;

	private readonly pRoClBdLZeniBBIsKaciGjfyKYZnA ChWfNDYRBbACxAEHwiQOCVVqVJiib;

	private readonly bool MEfijyABNZTGjrStVmBcHmBcKVhl;

	private readonly bool nZYUKfrsvUmAJudLXApxFQQbITqh;

	private readonly bool iPxywJqPZRPqkblVQAAuFMFvuCtbA;

	private readonly Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> JFMGJaZkXVlGvJoXDkGmamjpaORD;

	private readonly Func<int> fttBJlBzXCHuZhPweDBzbpxTarSv;

	DDWjNWYAVnuAprLJBexwaxtfkBnQA LrgFZTysTrlKPEyvhAYFSHPAbEyV.CGyAAtvckTjlzgONMmncsFHBDVgHA
	{
		get
		{
			return ytAMSILErDcsJgMLUVwVBTboKSutA;
		}
		set
		{
			CGyAAtvckTjlzgONMmncsFHBDVgHA = dDWjNWYAVnuAprLJBexwaxtfkBnQA;
			JaCMyZasfyDRcNCkePbOFEJMpRIs.GUdRAEbGhpUDpnEuBhSMWqHUnyvC = dDWjNWYAVnuAprLJBexwaxtfkBnQA;
		}
	}

	[CustomObfuscation(rename = false)]
	int PlatformInputManager.deviceCount => quCOYogNGCoKTRyJjYArxyflxCqB;

	[CustomObfuscation(rename = false)]
	PlatformInputManager PlatformInputManager.primaryInputManager => RNEsAapNgFgKizyAZboyUkTlxiRU;

	[CustomObfuscation(rename = false)]
	IInputSource PlatformInputManager.inputSource => JaCMyZasfyDRcNCkePbOFEJMpRIs;

	[CustomObfuscation(rename = false)]
	InputSource PlatformInputManager.inputSourceType => InputSource.RawInput;

	public wAJgxlipbMCSyzubUMIIIvlvRevYA(ConfigVars P_0, DDWjNWYAVnuAprLJBexwaxtfkBnQA P_1, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_2, Func<int> P_3, bool P_4, bool P_5, bool P_6, bool P_7)
	{
		try
		{
			KxYHLNbDJrTFWxMZnHrLpPiqrnbD = P_0;
			ytAMSILErDcsJgMLUVwVBTboKSutA = P_1;
			JFMGJaZkXVlGvJoXDkGmamjpaORD = P_2;
			fttBJlBzXCHuZhPweDBzbpxTarSv = P_3;
			MEfijyABNZTGjrStVmBcHmBcKVhl = P_4;
			nZYUKfrsvUmAJudLXApxFQQbITqh = P_5;
			iPxywJqPZRPqkblVQAAuFMFvuCtbA = P_6;
			RNEsAapNgFgKizyAZboyUkTlxiRU = this;
			UpdateLoopSetting updateLoop = P_0.updateLoop;
			if (P_6)
			{
				ChWfNDYRBbACxAEHwiQOCVVqVJiib = new pRoClBdLZeniBBIsKaciGjfyKYZnA(updateLoop);
			}
			if (P_5)
			{
				IodKrouyOIdWOfUxyLLjmlVtEcuw = new dXKLvQGSIvoiwbPpSGgTOgwmLqzq(updateLoop);
			}
			JaCMyZasfyDRcNCkePbOFEJMpRIs = new AuSBfxYAktMaNvbYMEDVwcjrEcXEA(P_0, P_1, P_4, P_7, IodKrouyOIdWOfUxyLLjmlVtEcuw, ChWfNDYRBbACxAEHwiQOCVVqVJiib);
			BmFqKKZtUkaOlSoiIvDQXoXtkihk = UpdateControllerData;
			zMfZVZJudYruBOoCmSgZbUMxwhFB = new global::gxiqikdujHobFnpuuAGPldgYhOvdA<bool>(true, xItqiygUZZBKCWepLqUlexKmhWCO);
			jyfcyjxQLvCxGERUmiZGFsVAKDRIb = new global::gxiqikdujHobFnpuuAGPldgYhOvdA<bool>(true, JaCMyZasfyDRcNCkePbOFEJMpRIs.RfximAbhraSSaEfgCElBKNqEubXIA);
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
		if (MEfijyABNZTGjrStVmBcHmBcKVhl || JaCMyZasfyDRcNCkePbOFEJMpRIs.dTOAtkDuDLkCQVobbKggqBFVceyT)
		{
			vQuqAkVexDBTCgzitMjBIRwVQQzVA = new TimerRealTime(1.0);
			vQuqAkVexDBTCgzitMjBIRwVQQzVA.Start();
		}
		if (MEfijyABNZTGjrStVmBcHmBcKVhl)
		{
			jTJexdyZMVjIKEKwatcPAoRlKcqS = new GSyjoFWrTdCkzcNRLgLTeMdPSDLQ();
			zsDkudiDYCAxcijjLkFketaiQjCnA();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType updateLoop)
	{
		if (MEfijyABNZTGjrStVmBcHmBcKVhl || JaCMyZasfyDRcNCkePbOFEJMpRIs.dTOAtkDuDLkCQVobbKggqBFVceyT)
		{
			YyVMogqwFjtyJUyeMxYqMRSNESjC();
		}
		if (JaCMyZasfyDRcNCkePbOFEJMpRIs != null)
		{
			JaCMyZasfyDRcNCkePbOFEJMpRIs.Update();
		}
		zKJbECJwcUuKneNptPMZBfpflMQIA();
		if (MEfijyABNZTGjrStVmBcHmBcKVhl)
		{
			if (JaCMyZasfyDRcNCkePbOFEJMpRIs != null)
			{
				JaCMyZasfyDRcNCkePbOFEJMpRIs.UpdateDevices(updateLoop);
			}
			VpZlGFLgAfvefsZJMVbDEmxWEyBCA();
			if (JaCMyZasfyDRcNCkePbOFEJMpRIs != null)
			{
				JaCMyZasfyDRcNCkePbOFEJMpRIs.UpdateFinished();
			}
		}
		if (nZYUKfrsvUmAJudLXApxFQQbITqh)
		{
			IodKrouyOIdWOfUxyLLjmlVtEcuw.tMAcjHTSzHASJIFuUESIMlFqxFZG(updateLoop);
		}
		if (iPxywJqPZRPqkblVQAAuFMFvuCtbA)
		{
			ChWfNDYRBbACxAEHwiQOCVVqVJiib.ZcQIdKCQGwoiBrDvDrcilZkhStzW(updateLoop);
		}
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (jyfcyjxQLvCxGERUmiZGFsVAKDRIb != null)
		{
			jyfcyjxQLvCxGERUmiZGFsVAKDRIb.WvLYkoCkCYjVtJGzHiicpMYRreXw();
		}
		if (zMfZVZJudYruBOoCmSgZbUMxwhFB != null)
		{
			zMfZVZJudYruBOoCmSgZbUMxwhFB.WvLYkoCkCYjVtJGzHiicpMYRreXw();
		}
		if (NmYAcdHiwzXdFtBVvSAMtHGDQfFr != null)
		{
			int count = NmYAcdHiwzXdFtBVvSAMtHGDQfFr.Count;
			for (int i = 0; i < count; i++)
			{
				if (NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i] != null)
				{
					NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i].zVCzgtFYiOaXdzRCQpIGMmhxgMWY();
				}
			}
		}
		if (ChWfNDYRBbACxAEHwiQOCVVqVJiib != null)
		{
			ChWfNDYRBbACxAEHwiQOCVVqVJiib.Dispose();
		}
		if (IodKrouyOIdWOfUxyLLjmlVtEcuw != null)
		{
			IodKrouyOIdWOfUxyLLjmlVtEcuw.Dispose();
		}
		if (JaCMyZasfyDRcNCkePbOFEJMpRIs != null)
		{
			JaCMyZasfyDRcNCkePbOFEJMpRIs.Dispose();
		}
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return BmFqKKZtUkaOlSoiIvDQXoXtkihk;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int inputManagerId, ControllerDataUpdater data)
	{
		if (!MEfijyABNZTGjrStVmBcHmBcKVhl)
		{
			return;
		}
		for (int i = 0; i < quCOYogNGCoKTRyJjYArxyflxCqB; i++)
		{
			if (NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == inputManagerId)
			{
				NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i].FillData(data);
				return;
			}
		}
		Logger.LogError("Invalid joystick Id " + inputManagerId + "!");
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		JaCMyZasfyDRcNCkePbOFEJMpRIs.SystemDeviceConnected();
		WgoAOOCjEBpExflXlogOovhuanjZ = true;
		if (MEfijyABNZTGjrStVmBcHmBcKVhl || JaCMyZasfyDRcNCkePbOFEJMpRIs.dTOAtkDuDLkCQVobbKggqBFVceyT)
		{
			vQuqAkVexDBTCgzitMjBIRwVQQzVA.Start();
		}
		if (iPxywJqPZRPqkblVQAAuFMFvuCtbA)
		{
			ChWfNDYRBbACxAEHwiQOCVVqVJiib.sLYmdWYPwINZsuNKBIOusfaNLLXC(true);
		}
		if (nZYUKfrsvUmAJudLXApxFQQbITqh)
		{
			IodKrouyOIdWOfUxyLLjmlVtEcuw.pAfbVNMeJdAnESsUiBQJRIykXZdP(true);
		}
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		JaCMyZasfyDRcNCkePbOFEJMpRIs.SystemDeviceDisconnected();
		WgoAOOCjEBpExflXlogOovhuanjZ = true;
		if (MEfijyABNZTGjrStVmBcHmBcKVhl || JaCMyZasfyDRcNCkePbOFEJMpRIs.dTOAtkDuDLkCQVobbKggqBFVceyT)
		{
			vQuqAkVexDBTCgzitMjBIRwVQQzVA.Start();
		}
		if (iPxywJqPZRPqkblVQAAuFMFvuCtbA)
		{
			ChWfNDYRBbACxAEHwiQOCVVqVJiib.sLYmdWYPwINZsuNKBIOusfaNLLXC(false);
		}
		if (nZYUKfrsvUmAJudLXApxFQQbITqh)
		{
			IodKrouyOIdWOfUxyLLjmlVtEcuw.pAfbVNMeJdAnESsUiBQJRIykXZdP(false);
		}
		if (_SystemDeviceDisconnectedEvent != null)
		{
			_SystemDeviceDisconnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
	{
		_ = MEfijyABNZTGjrStVmBcHmBcKVhl;
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedMouseSource GetUnifiedMouseSource()
	{
		return IodKrouyOIdWOfUxyLLjmlVtEcuw;
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedKeyboardSource GetUnifiedKeyboardSource()
	{
		return ChWfNDYRBbACxAEHwiQOCVVqVJiib;
	}

	public void RqSdaohHjoPxNvQkkpntdFkVCrbzA(PfnQbhAAztkGebiJJBwStuolfJCF P_0, fBMrrsvbWMcOcdDbjcFUuOyQnpTbb P_1)
	{
	}

	private void YyVMogqwFjtyJUyeMxYqMRSNESjC()
	{
		if (zMfZVZJudYruBOoCmSgZbUMxwhFB.LYkqHsINOrRmqDzoBVFvzhFfDHwO)
		{
			if (zMfZVZJudYruBOoCmSgZbUMxwhFB.XrInaWDtMKIvqgrHsTdTFhcYalgT() && !vQuqAkVexDBTCgzitMjBIRwVQQzVA.running && !jyfcyjxQLvCxGERUmiZGFsVAKDRIb.LYkqHsINOrRmqDzoBVFvzhFfDHwO)
			{
				if (zMfZVZJudYruBOoCmSgZbUMxwhFB.JNANzwwQdbCSWIOPljErVgGJvfwO)
				{
					WgoAOOCjEBpExflXlogOovhuanjZ = true;
				}
				vQuqAkVexDBTCgzitMjBIRwVQQzVA.Start();
			}
		}
		else if (!vQuqAkVexDBTCgzitMjBIRwVQQzVA.running)
		{
			vQuqAkVexDBTCgzitMjBIRwVQQzVA.Start();
		}
		else if (vQuqAkVexDBTCgzitMjBIRwVQQzVA.Update())
		{
			zMfZVZJudYruBOoCmSgZbUMxwhFB.dLArMTCEuzcNhWTwyvoEESZrqiGi();
		}
	}

	private void zsDkudiDYCAxcijjLkFketaiQjCnA()
	{
		vfLkauHkgHdzxYKVHjPXqfTzKiwJ(SnZGLQkmLfXXOOWZEZQlknvIfjBe());
	}

	private void vfLkauHkgHdzxYKVHjPXqfTzKiwJ(IList<JIDPHXmbYMSkzPnbLXOMHLCjPskd> P_0)
	{
		int num = 0;
		List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> nmYAcdHiwzXdFtBVvSAMtHGDQfFr = NmYAcdHiwzXdFtBVvSAMtHGDQfFr;
		int num2 = quCOYogNGCoKTRyJjYArxyflxCqB;
		NmYAcdHiwzXdFtBVvSAMtHGDQfFr = new List<mgmlbBRQFaeoqiMIcNTspgMYlddBb>();
		fJaodxcaFSZTSCjdOeDtIhGxKdXY = 0;
		List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> list = new List<mgmlbBRQFaeoqiMIcNTspgMYlddBb>();
		for (int num3 = num2 - 1; num3 >= 0; num3--)
		{
			if (nmYAcdHiwzXdFtBVvSAMtHGDQfFr[num3] != null && !nmYAcdHiwzXdFtBVvSAMtHGDQfFr[num3].RGcnHgTfJdsyfegDhhPIIGoZZZRx)
			{
				list.Add(nmYAcdHiwzXdFtBVvSAMtHGDQfFr[num3]);
				nmYAcdHiwzXdFtBVvSAMtHGDQfFr.RemoveAt(num3);
			}
		}
		num2 = nmYAcdHiwzXdFtBVvSAMtHGDQfFr?.Count ?? 0;
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] == null)
			{
				continue;
			}
			JIDPHXmbYMSkzPnbLXOMHLCjPskd jIDPHXmbYMSkzPnbLXOMHLCjPskd = P_0[i];
			if (jIDPHXmbYMSkzPnbLXOMHLCjPskd != null)
			{
				mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb2 = new mgmlbBRQFaeoqiMIcNTspgMYlddBb(jIDPHXmbYMSkzPnbLXOMHLCjPskd, jIDPHXmbYMSkzPnbLXOMHLCjPskd.tEwPjlwcvVfzfflaOHoSKOTEjEmz, JFMGJaZkXVlGvJoXDkGmamjpaORD);
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.ocnlExBGzgvrbbiLumGDMMksucjT = jIDPHXmbYMSkzPnbLXOMHLCjPskd.eeLYrwPQKctoTapvzRLHAtZIfwnV;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.FzydYXcZAaAycEHYIJIohPgTxBeJ = jIDPHXmbYMSkzPnbLXOMHLCjPskd.OeGYfNLnWbjgMjPkTgdrilHpguDCb;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.jdKiFeajMWZALcqxhnovacouhDpab = jIDPHXmbYMSkzPnbLXOMHLCjPskd.OeGYfNLnWbjgMjPkTgdrilHpguDCb;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.OjIFPJdqkPIULfMkjMZGNcpHsJTVA = jIDPHXmbYMSkzPnbLXOMHLCjPskd.ibSeINshSGScITkFjKtcOMvvNXjb;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.AXLReMpRMhWwqczNZdqsTGQVZncq = jIDPHXmbYMSkzPnbLXOMHLCjPskd.MlUWuQbZqNWzsYRXZioNJpBxWurv;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.ZDewcBMTjUbsYXeQDkjNshaSOduJ = jIDPHXmbYMSkzPnbLXOMHLCjPskd.MXYCKUZFcJBUCZiuMtejjGoCRoFK;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.sFJRxDuicBykGWtzbbImKRHjuLBi = jIDPHXmbYMSkzPnbLXOMHLCjPskd.WdYzYRKcGrIImaPjqcwCrdMhPgLHA;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.eyKEXEvzpXuagpJhbMJksTcowmYX = jIDPHXmbYMSkzPnbLXOMHLCjPskd.dZoMCESFJjGeFSCXDeONdZuyeOCDA;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.JJAEtoXSWDTTQIwxmmcrINdFEjsC = jIDPHXmbYMSkzPnbLXOMHLCjPskd.PmrsxLoxUwGFsxXQTOWpQFpRcinN;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.PaAdzljaxFJFWEaUsOrabIGDajIdA = jIDPHXmbYMSkzPnbLXOMHLCjPskd.fWUbcorZqVMccGMcDGDlaEupdJAQ;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.TjYPbLYxwDISjQqbvtvGMQkgVTVo = false;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.XIrQMhERePtFjYZAwnZzDkPPlChx = jIDPHXmbYMSkzPnbLXOMHLCjPskd.OVAmXCmTjKlJhFrwXPxhSgJZucaM;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.QmsrlWmtWGTlUGRfcLtxLyjqEXCk = jIDPHXmbYMSkzPnbLXOMHLCjPskd.FZebZwAYjUaXGKBUheDcqiDVcAzcA;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.eQLIlFhSBmzVfhEJUpSBzPmEmndv = jIDPHXmbYMSkzPnbLXOMHLCjPskd.cbTOAlBPkdplJiaCpmwbqplqVJsK;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.ETMQmyjBbOoUCMrtSbvLFxZuEFDbA = jIDPHXmbYMSkzPnbLXOMHLCjPskd.DAVtKVRxfLtmUXMyhyILqEabEXqp;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Eextension = jIDPHXmbYMSkzPnbLXOMHLCjPskd.oSWQoAhLeZXWkoZqMASCMqaIiEnm;
				jIDPHXmbYMSkzPnbLXOMHLCjPskd.kvqcLvPweCnbRsOECVFMUPhBdLCN();
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.tKHXpKHQkGDnuYJkWjOWdqDHNOwz();
				NmYAcdHiwzXdFtBVvSAMtHGDQfFr.Add(mgmlbBRQFaeoqiMIcNTspgMYlddBb2);
				num++;
				if (mgmlbBRQFaeoqiMIcNTspgMYlddBb2.XIrQMhERePtFjYZAwnZzDkPPlChx)
				{
					fJaodxcaFSZTSCjdOeDtIhGxKdXY++;
				}
			}
		}
		quCOYogNGCoKTRyJjYArxyflxCqB = num;
		DMctMnZcxSVqDjEhICgbBPSralwM(num2, num, nmYAcdHiwzXdFtBVvSAMtHGDQfFr, NmYAcdHiwzXdFtBVvSAMtHGDQfFr);
		for (int j = 0; j < num; j++)
		{
			if (_UpdateControllerInfoEvent != null)
			{
				_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(NmYAcdHiwzXdFtBVvSAMtHGDQfFr[j]));
			}
		}
		list.ForEach(delegate(mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb3)
		{
			sOKMUutrGZbXpFTKBulBkMNggGcl(mgmlbBRQFaeoqiMIcNTspgMYlddBb3, false);
		});
		YPzVdZYpOkSbcGMtCXJfXPLcRtwh(nmYAcdHiwzXdFtBVvSAMtHGDQfFr, NmYAcdHiwzXdFtBVvSAMtHGDQfFr, false);
		YPzVdZYpOkSbcGMtCXJfXPLcRtwh(NmYAcdHiwzXdFtBVvSAMtHGDQfFr, nmYAcdHiwzXdFtBVvSAMtHGDQfFr, true);
	}

	private void VpZlGFLgAfvefsZJMVbDEmxWEyBCA()
	{
		for (int i = 0; i < quCOYogNGCoKTRyJjYArxyflxCqB; i++)
		{
			mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb2 = NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i];
			if (mgmlbBRQFaeoqiMIcNTspgMYlddBb2 != null && (ytAMSILErDcsJgMLUVwVBTboKSutA == null || !mgmlbBRQFaeoqiMIcNTspgMYlddBb2.TjYPbLYxwDISjQqbvtvGMQkgVTVo))
			{
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Update();
			}
		}
	}

	private IList<JIDPHXmbYMSkzPnbLXOMHLCjPskd> SnZGLQkmLfXXOOWZEZQlknvIfjBe()
	{
		return JaCMyZasfyDRcNCkePbOFEJMpRIs.GetJoysticks<JIDPHXmbYMSkzPnbLXOMHLCjPskd>();
	}

	private void DMctMnZcxSVqDjEhICgbBPSralwM(int P_0, int P_1, List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_2, List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_3)
	{
		if (P_1 > 0)
		{
			P_3.Sort(mgmlbBRQFaeoqiMIcNTspgMYlddBb.TgwBckwVPTQrFSamDOBOXmOUSIaP);
		}
		if (P_0 > 0 && P_1 > 0)
		{
			LUaJDYGEqmVPldbvzHerlbhRHkVo(P_1, P_3, P_0, P_2, GSyjoFWrTdCkzcNRLgLTeMdPSDLQ.iAvddRwBtmTSVsgWPUiRjTuLUrgk.Exact);
		}
		ZLmSOcFDPsjxSErnAfPgEpWHHGpjA(P_1, P_3, GSyjoFWrTdCkzcNRLgLTeMdPSDLQ.iAvddRwBtmTSVsgWPUiRjTuLUrgk.Exact);
		for (int i = 0; i < P_1; i++)
		{
			mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb2 = P_3[i];
			if (mgmlbBRQFaeoqiMIcNTspgMYlddBb2 != null && mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId < 0)
			{
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = JfQeaSPHDnyVXNefxkDtJZSUiDpS(P_3);
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = fttBJlBzXCHuZhPweDBzbpxTarSv();
				jTJexdyZMVjIKEKwatcPAoRlKcqS.gXveIzYJnCpdkqhmRmkWklaIIMVW(mgmlbBRQFaeoqiMIcNTspgMYlddBb2);
			}
		}
		P_3.Sort(mgmlbBRQFaeoqiMIcNTspgMYlddBb.UHxnclEPdAfilgRfhPhnhzsbJoQLb);
	}

	private bool GTzbXHizjPVTuyoumKhXtMKqrctm(List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_0, int P_1)
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

	private int JfQeaSPHDnyVXNefxkDtJZSUiDpS(List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_0)
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

	private bool pIUyUbCaLmSLsXobQHEXNgMmHVgbA(List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_0, int P_1)
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

	private void LUaJDYGEqmVPldbvzHerlbhRHkVo(int P_0, List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_1, int P_2, List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_3, GSyjoFWrTdCkzcNRLgLTeMdPSDLQ.iAvddRwBtmTSVsgWPUiRjTuLUrgk P_4)
	{
		int num = ((P_4 != GSyjoFWrTdCkzcNRLgLTeMdPSDLQ.iAvddRwBtmTSVsgWPUiRjTuLUrgk.Exact) ? 1 : 2);
		for (int i = 0; i < P_0; i++)
		{
			mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb2 = P_1[i];
			if (mgmlbBRQFaeoqiMIcNTspgMYlddBb2 == null || mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			for (int j = 0; j < P_2; j++)
			{
				mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb3 = P_3[j];
				if (mgmlbBRQFaeoqiMIcNTspgMYlddBb3 != null && !pIUyUbCaLmSLsXobQHEXNgMmHVgbA(P_1, mgmlbBRQFaeoqiMIcNTspgMYlddBb3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId) && mgmlbBRQFaeoqiMIcNTspgMYlddBb2.hcTsMltMZFRQPhZCrWAbvtCIFqhb(mgmlbBRQFaeoqiMIcNTspgMYlddBb3) >= num)
				{
					mgmlbBRQFaeoqiMIcNTspgMYlddBb2.iGnvPmQMIqnprLwYBiPpICpMgyGFA(mgmlbBRQFaeoqiMIcNTspgMYlddBb3);
					jTJexdyZMVjIKEKwatcPAoRlKcqS.gXveIzYJnCpdkqhmRmkWklaIIMVW(mgmlbBRQFaeoqiMIcNTspgMYlddBb2);
				}
			}
		}
	}

	private void ZLmSOcFDPsjxSErnAfPgEpWHHGpjA(int P_0, List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_1, GSyjoFWrTdCkzcNRLgLTeMdPSDLQ.iAvddRwBtmTSVsgWPUiRjTuLUrgk P_2)
	{
		for (int i = 0; i < P_0; i++)
		{
			mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb2 = P_1[i];
			if (mgmlbBRQFaeoqiMIcNTspgMYlddBb2 == null || mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			GSyjoFWrTdCkzcNRLgLTeMdPSDLQ.sbenqLpovPBasXaYIRDjMsZGAKKC sbenqLpovPBasXaYIRDjMsZGAKKC = null;
			foreach (GSyjoFWrTdCkzcNRLgLTeMdPSDLQ.sbenqLpovPBasXaYIRDjMsZGAKKC item in jTJexdyZMVjIKEKwatcPAoRlKcqS.jgOgQAadSGQzXcCMOqcQPgpOMiWnA(mgmlbBRQFaeoqiMIcNTspgMYlddBb2, P_2))
			{
				if (!pIUyUbCaLmSLsXobQHEXNgMmHVgbA(P_1, item.BnkyUIWVDlhHjhQGmuAQGRDLELTo) && item.dKFoUuqovuSRRvdrWeHXfzUMGHeBA >= 0)
				{
					sbenqLpovPBasXaYIRDjMsZGAKKC = item;
					break;
				}
			}
			if (sbenqLpovPBasXaYIRDjMsZGAKKC != null)
			{
				int num = sbenqLpovPBasXaYIRDjMsZGAKKC.dKFoUuqovuSRRvdrWeHXfzUMGHeBA;
				if (!GTzbXHizjPVTuyoumKhXtMKqrctm(P_1, num))
				{
					num = (sbenqLpovPBasXaYIRDjMsZGAKKC.dKFoUuqovuSRRvdrWeHXfzUMGHeBA = JfQeaSPHDnyVXNefxkDtJZSUiDpS(P_1));
				}
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = num;
				mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = sbenqLpovPBasXaYIRDjMsZGAKKC.BnkyUIWVDlhHjhQGmuAQGRDLELTo;
				jTJexdyZMVjIKEKwatcPAoRlKcqS.gXveIzYJnCpdkqhmRmkWklaIIMVW(mgmlbBRQFaeoqiMIcNTspgMYlddBb2);
			}
		}
	}

	private void zKJbECJwcUuKneNptPMZBfpflMQIA()
	{
		if (JaCMyZasfyDRcNCkePbOFEJMpRIs.IzixZZufVIIxbLYKYNyIdOaLwPmb(true))
		{
			WgoAOOCjEBpExflXlogOovhuanjZ = true;
		}
		if (WgoAOOCjEBpExflXlogOovhuanjZ)
		{
			EsZJlXJnwijMOkmJmjYIoSAlvmto();
		}
		if ((MEfijyABNZTGjrStVmBcHmBcKVhl || JaCMyZasfyDRcNCkePbOFEJMpRIs.dTOAtkDuDLkCQVobbKggqBFVceyT) && jyfcyjxQLvCxGERUmiZGFsVAKDRIb.LYkqHsINOrRmqDzoBVFvzhFfDHwO && jyfcyjxQLvCxGERUmiZGFsVAKDRIb.XrInaWDtMKIvqgrHsTdTFhcYalgT())
		{
			zmLFRQgzQXEgRuSIgqMaGeybOVriA();
		}
	}

	private void EsZJlXJnwijMOkmJmjYIoSAlvmto()
	{
		WgoAOOCjEBpExflXlogOovhuanjZ = false;
		if (!jyfcyjxQLvCxGERUmiZGFsVAKDRIb.LYkqHsINOrRmqDzoBVFvzhFfDHwO)
		{
			JaCMyZasfyDRcNCkePbOFEJMpRIs.SikYnMDEvyMtwnFOMwVljDdjXnpw();
			jyfcyjxQLvCxGERUmiZGFsVAKDRIb.dLArMTCEuzcNhWTwyvoEESZrqiGi();
		}
	}

	private void zmLFRQgzQXEgRuSIgqMaGeybOVriA()
	{
		JaCMyZasfyDRcNCkePbOFEJMpRIs.xcfRdrXIpbKvUSThJWzNYWANwVXg();
		if (MEfijyABNZTGjrStVmBcHmBcKVhl)
		{
			IList<JIDPHXmbYMSkzPnbLXOMHLCjPskd> list = SnZGLQkmLfXXOOWZEZQlknvIfjBe();
			if (ZOfiGTSpvcsQSgxCjAuhrwvSfytJ(list))
			{
				vfLkauHkgHdzxYKVHjPXqfTzKiwJ(list);
			}
		}
	}

	private bool ZOfiGTSpvcsQSgxCjAuhrwvSfytJ(IList<JIDPHXmbYMSkzPnbLXOMHLCjPskd> P_0)
	{
		for (int i = 0; i < NmYAcdHiwzXdFtBVvSAMtHGDQfFr.Count; i++)
		{
			if (NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i] != null && !NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i].RGcnHgTfJdsyfegDhhPIIGoZZZRx)
			{
				return true;
			}
		}
		int count = P_0.Count;
		for (int j = 0; j < count; j++)
		{
			if (P_0[j] != null && !BJgEqmYqegmojlHCQARBataFxZcw(P_0[j].eeLYrwPQKctoTapvzRLHAtZIfwnV))
			{
				return true;
			}
		}
		int count2 = NmYAcdHiwzXdFtBVvSAMtHGDQfFr.Count;
		for (int k = 0; k < count2; k++)
		{
			if (NmYAcdHiwzXdFtBVvSAMtHGDQfFr[k] != null && !VlAItxjewgBZIeirPVCjAiKWFKHH(P_0, NmYAcdHiwzXdFtBVvSAMtHGDQfFr[k].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid))
			{
				return true;
			}
		}
		return false;
	}

	private bool BJgEqmYqegmojlHCQARBataFxZcw(Guid P_0)
	{
		int count = NmYAcdHiwzXdFtBVvSAMtHGDQfFr.Count;
		for (int i = 0; i < count; i++)
		{
			if (NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i] != null && NmYAcdHiwzXdFtBVvSAMtHGDQfFr[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid == P_0)
			{
				return true;
			}
		}
		return false;
	}

	private bool VlAItxjewgBZIeirPVCjAiKWFKHH(IList<JIDPHXmbYMSkzPnbLXOMHLCjPskd> P_0, Guid P_1)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && P_0[i].eeLYrwPQKctoTapvzRLHAtZIfwnV == P_1)
			{
				return true;
			}
		}
		return false;
	}

	private void YPzVdZYpOkSbcGMtCXJfXPLcRtwh(List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_0, List<mgmlbBRQFaeoqiMIcNTspgMYlddBb> P_1, bool P_2)
	{
		if (P_0 == null)
		{
			return;
		}
		int num = P_0?.Count ?? 0;
		int num2 = P_1?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb2 = P_0[i];
			if (mgmlbBRQFaeoqiMIcNTspgMYlddBb2 == null)
			{
				continue;
			}
			bool flag = false;
			if (P_1 != null)
			{
				for (int j = 0; j < num2; j++)
				{
					mgmlbBRQFaeoqiMIcNTspgMYlddBb mgmlbBRQFaeoqiMIcNTspgMYlddBb3 = P_1[j];
					if (mgmlbBRQFaeoqiMIcNTspgMYlddBb3 != null && mgmlbBRQFaeoqiMIcNTspgMYlddBb2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid == mgmlbBRQFaeoqiMIcNTspgMYlddBb3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				sOKMUutrGZbXpFTKBulBkMNggGcl(P_0[i], P_2);
			}
		}
	}

	private void sOKMUutrGZbXpFTKBulBkMNggGcl(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0, bool P_1)
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

	private bool xItqiygUZZBKCWepLqUlexKmhWCO()
	{
		try
		{
			int num = 0;
			NrQfQnkqCzbdhahnYmfVCnDUJLLm.TtXCLBIgIxbCbqldyxqiMNzepooM(null, ref num, luYaFPaftNInTWGPWfCvgYuDUqDyA.glwGxVPzunhdOUxGIRKtVKPYTQvO<oavWbmfgJapTKculwWsDMRdHdvld>());
			if (ftGWwIBfmWOyygYRTPUErslsyuUl != num)
			{
				ftGWwIBfmWOyygYRTPUErslsyuUl = num;
				return true;
			}
		}
		catch (Exception ex)
		{
			Logger.Log("Exception getting Raw Input Device List.\n" + ex);
		}
		if (fJaodxcaFSZTSCjdOeDtIhGxKdXY > 0 && JaCMyZasfyDRcNCkePbOFEJMpRIs.fSElofsdFLmeDAHgdVuOzrIYvtiT())
		{
			return true;
		}
		return false;
	}

	[CompilerGenerated]
	private void WjXOkuFYEYMTBbeTxZTkiYSepeNu(mgmlbBRQFaeoqiMIcNTspgMYlddBb P_0)
	{
		sOKMUutrGZbXpFTKBulBkMNggGcl(P_0, false);
	}
}
