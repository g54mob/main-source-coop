using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Internal.Localization;
using Rewired.Platforms;
using Rewired.Platforms.Custom;
using Rewired.Utils;

namespace Rewired.InputManagers
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal class CustomInputManager : PlatformInputManager
	{
		private class eMRBpxOrihViWrGtUAboblLLhaOq : IInputManagerJoystick, IInputManagerJoystickPublic, ITryGetLocalizedName
		{
			private readonly InputSource OLOjHLgIfUnBaBNZDiiPPMrRCYTb;

			private readonly CustomInputSource GxliDgfjJhVlUBiupPvyzOGWJTCL;

			private readonly Controller.Extension rVxEiWQQnfLLteUMFchTDYtrSKCW;

			private int PEFjaZNQhPjxZxlxWIjYbzmPcihs;

			private int tnQDrvLlNlcAYAtWOdRqgMGquElsA;

			private long? lgPPHFeVHDvJiQOPeHhmBANPtVhW;

			private int RyzdRlbIqTrfwnDVsEInSUdeORNn;

			public Guid pYvPHLTuYrbFqUeVsBTlxCBPAtcI;

			public string GyHIlTcgQcEbrTjwIgqyhPAsqyHi;

			public string ZhlGFebTIkcNpfiKbeMfkkvnrNNqA;

			private int myFKrWzSPNuBooFzNkJvrelNisVx;

			private int CTPHUcDNtgeoaxGwizOjFDKvPGkr;

			private float[] itjrwJTskMOCIBDMHZFJeeJsVVi;

			private bool[] xVBtRMtpzANkcHPJxZjaDocWwgLe;

			private float[] pEcCOvbGsUnLsfgcbxBXCcguTiKnc;

			private bool[] nwKnrVkNxXELctJfMnpjebSZpfqo;

			private HardwareJoystickMap_InputManager kXxhbxFCNNbeZuCaCeaPIVwoxfJSA;

			public CustomInputSource.Joystick KHpSPJfGZxvBentoOqzsGGiKAch;

			private bool XYZltPJBRetDSHKEqiizKSJHVlFL;

			private readonly bool FttlbvpYdardZKjZfHOzBcTihZPo;

			private readonly LocalizedString vMJiGUectNZffFWGlGhlFhiWGMWO;

			private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> xiIyIGzbtOTBCGbQEIlpkLTWSGlF;

			public int TWOyaBHQrchfKBdzbwamgOfAjzTQA
			{
				get
				{
					if (KHpSPJfGZxvBentoOqzsGGiKAch == null)
					{
						return 0;
					}
					return KHpSPJfGZxvBentoOqzsGGiKAch.buttonCount;
				}
			}

			public int LBekxSnMnDVzlBEcariaaWLiDnXB
			{
				get
				{
					if (KHpSPJfGZxvBentoOqzsGGiKAch == null)
					{
						return 0;
					}
					return KHpSPJfGZxvBentoOqzsGGiKAch.axisCount;
				}
			}

			[CustomObfuscation(rename = false)]
			int IInputManagerJoystickPublic.rewiredId
			{
				get
				{
					return PEFjaZNQhPjxZxlxWIjYbzmPcihs;
				}
				set
				{
					PEFjaZNQhPjxZxlxWIjYbzmPcihs = value;
				}
			}

			[CustomObfuscation(rename = false)]
			int IInputManagerJoystickPublic.inputManagerId
			{
				get
				{
					return tnQDrvLlNlcAYAtWOdRqgMGquElsA;
				}
				set
				{
					tnQDrvLlNlcAYAtWOdRqgMGquElsA = value;
				}
			}

			[CustomObfuscation(rename = false)]
			string IInputManagerJoystickPublic.name
			{
				get
				{
					string text = ((!string.IsNullOrEmpty(KHpSPJfGZxvBentoOqzsGGiKAch.customName)) ? KHpSPJfGZxvBentoOqzsGGiKAch.customName : GyHIlTcgQcEbrTjwIgqyhPAsqyHi);
					if (text == "Unknown Controller")
					{
						text = ZhlGFebTIkcNpfiKbeMfkkvnrNNqA;
					}
					return text;
				}
			}

			[CustomObfuscation(rename = false)]
			long? IInputManagerJoystickPublic.systemId => lgPPHFeVHDvJiQOPeHhmBANPtVhW;

			[CustomObfuscation(rename = false)]
			int IInputManagerJoystickPublic.unityId => RyzdRlbIqTrfwnDVsEInSUdeORNn;

			[CustomObfuscation(rename = false)]
			Guid IInputManagerJoystickPublic.instanceGuid
			{
				get
				{
					if (!lgPPHFeVHDvJiQOPeHhmBANPtVhW.HasValue)
					{
						return Guid.Empty;
					}
					return MiscTools.CreateGuidHashSHA1(Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Ename + "_" + lgPPHFeVHDvJiQOPeHhmBANPtVhW);
				}
			}

			[CustomObfuscation(rename = false)]
			Guid IInputManagerJoystickPublic.persistentGuid
			{
				get
				{
					if (!(KHpSPJfGZxvBentoOqzsGGiKAch.deviceInstanceGuid != Guid.Empty))
					{
						return Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;
					}
					return KHpSPJfGZxvBentoOqzsGGiKAch.deviceInstanceGuid;
				}
			}

			[CustomObfuscation(rename = false)]
			Controller.Extension IInputManagerJoystickPublic.extension => rVxEiWQQnfLLteUMFchTDYtrSKCW;

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

			public eMRBpxOrihViWrGtUAboblLLhaOq(CustomInputSource P_0, long? P_1, int P_2, CustomInputSource.Joystick P_3, InputSource P_4, Controller.Extension P_5, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_6)
			{
				FttlbvpYdardZKjZfHOzBcTihZPo = P_0.ynIdjZCpVlwTpZFWgFUmNBinSjFu == InputSource.PS4 || P_0.ynIdjZCpVlwTpZFWgFUmNBinSjFu == InputSource.PS5;
				vMJiGUectNZffFWGlGhlFhiWGMWO = new LocalizedString();
				GxliDgfjJhVlUBiupPvyzOGWJTCL = P_0;
				OLOjHLgIfUnBaBNZDiiPPMrRCYTb = P_4;
				lgPPHFeVHDvJiQOPeHhmBANPtVhW = P_1;
				KHpSPJfGZxvBentoOqzsGGiKAch = P_3;
				RyzdRlbIqTrfwnDVsEInSUdeORNn = P_2;
				rVxEiWQQnfLLteUMFchTDYtrSKCW = P_5;
				xiIyIGzbtOTBCGbQEIlpkLTWSGlF = P_6;
				tnQDrvLlNlcAYAtWOdRqgMGquElsA = -1;
				PEFjaZNQhPjxZxlxWIjYbzmPcihs = -1;
				qQkbTWVwkdGKUDcxWgxqFibYzqPk();
				RNWxDbtOOlnUkPCTQPyYWLDdTCmK();
				pYvPHLTuYrbFqUeVsBTlxCBPAtcI = kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.hardwareMapIdentifier.guid;
				GyHIlTcgQcEbrTjwIgqyhPAsqyHi = kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.controllerName;
				itjrwJTskMOCIBDMHZFJeeJsVVi = new float[myFKrWzSPNuBooFzNkJvrelNisVx];
				xVBtRMtpzANkcHPJxZjaDocWwgLe = new bool[CTPHUcDNtgeoaxGwizOjFDKvPGkr];
				pEcCOvbGsUnLsfgcbxBXCcguTiKnc = new float[CTPHUcDNtgeoaxGwizOjFDKvPGkr];
				nwKnrVkNxXELctJfMnpjebSZpfqo = new bool[CTPHUcDNtgeoaxGwizOjFDKvPGkr];
				HardwareJoystickMap.Platform_Custom.Button[] buttons = ((HardwareJoystickMap.Platform_Custom)kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.map).Buttons;
				if (buttons != null)
				{
					int num = MathTools.Min(buttons.Length, CTPHUcDNtgeoaxGwizOjFDKvPGkr);
					for (int i = 0; i < num; i++)
					{
						if (buttons[i] != null && buttons[i].buttonInfo != null)
						{
							nwKnrVkNxXELctJfMnpjebSZpfqo[i] = buttons[i].buttonInfo.isPressureSensitive;
						}
					}
				}
				Update();
			}

			public void qQkbTWVwkdGKUDcxWgxqFibYzqPk()
			{
				ZhlGFebTIkcNpfiKbeMfkkvnrNNqA = KHpSPJfGZxvBentoOqzsGGiKAch.deviceName;
			}

			[CustomObfuscation(rename = false)]
			public void Update()
			{
				if (KHpSPJfGZxvBentoOqzsGGiKAch.isConnected)
				{
					tHEAOAizHetqquKTJuXvNXVVKDrDA();
					qXWEoPKzLJSRrIshznMnlQNIPfGTA();
				}
			}

			void IInputManagerJoystick.Update()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Update
				this.Update();
			}

			public int foKVHXApBOczpRYNIVfazsxiLqip(eMRBpxOrihViWrGtUAboblLLhaOq P_0)
			{
				if (P_0.ZhlGFebTIkcNpfiKbeMfkkvnrNNqA == ZhlGFebTIkcNpfiKbeMfkkvnrNNqA && P_0.lgPPHFeVHDvJiQOPeHhmBANPtVhW == lgPPHFeVHDvJiQOPeHhmBANPtVhW)
				{
					return 2;
				}
				if (P_0.ZhlGFebTIkcNpfiKbeMfkkvnrNNqA == ZhlGFebTIkcNpfiKbeMfkkvnrNNqA)
				{
					return 1;
				}
				return 0;
			}

			private void ObHkKIVzgcemRnxfbJqnZzkpHmel(BridgedControllerHWInfo P_0)
			{
				P_0.inputManagerSource = OLOjHLgIfUnBaBNZDiiPPMrRCYTb;
				P_0.inputSource = OLOjHLgIfUnBaBNZDiiPPMrRCYTb;
				P_0.hardwareIdentifier = sLebBvQqwuzjKNXafgPPskjwXEWb();
				P_0.hardwareAxisCount = myFKrWzSPNuBooFzNkJvrelNisVx;
				P_0.hardwareButtonCount = CTPHUcDNtgeoaxGwizOjFDKvPGkr;
				P_0.hardwareHatCount = 0;
				P_0.hw_productName = ZhlGFebTIkcNpfiKbeMfkkvnrNNqA;
				P_0.hw_supportsVibration = KHpSPJfGZxvBentoOqzsGGiKAch.supportsVibration;
				P_0.userCustomIdentifier = KHpSPJfGZxvBentoOqzsGGiKAch.customIdentifier;
			}

			private void ZJKLhELGAuLNJMiyVkgNghrbJbtl(BridgedController P_0)
			{
				ObHkKIVzgcemRnxfbJqnZzkpHmel(P_0);
				P_0.sourceJoystick = this;
				P_0.gameHardwareMap = kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.ToGameHardwareControllerMap();
				P_0.instanceName = ZhlGFebTIkcNpfiKbeMfkkvnrNNqA;
				P_0.productName = ZhlGFebTIkcNpfiKbeMfkkvnrNNqA;
				P_0.isXInputDevice = false;
				P_0.axisCount = myFKrWzSPNuBooFzNkJvrelNisVx;
				P_0.buttonCount = CTPHUcDNtgeoaxGwizOjFDKvPGkr;
				P_0.controllerTypeGuid = pYvPHLTuYrbFqUeVsBTlxCBPAtcI;
				P_0.customInputSource = GxliDgfjJhVlUBiupPvyzOGWJTCL;
				P_0.controllerExtension = rVxEiWQQnfLLteUMFchTDYtrSKCW;
				P_0.isButtonPressureSensitive = new bool[nwKnrVkNxXELctJfMnpjebSZpfqo.Length];
				for (int i = 0; i < nwKnrVkNxXELctJfMnpjebSZpfqo.Length; i++)
				{
					P_0.isButtonPressureSensitive[i] = nwKnrVkNxXELctJfMnpjebSZpfqo[i];
				}
			}

			[CustomObfuscation(rename = false)]
			public void FillData(ControllerDataUpdater dataUpdater)
			{
				if (myFKrWzSPNuBooFzNkJvrelNisVx != dataUpdater.axisCount || CTPHUcDNtgeoaxGwizOjFDKvPGkr != dataUpdater.buttonCount)
				{
					throw new Exception("This controller signature does not match the data object!");
				}
				for (int i = 0; i < myFKrWzSPNuBooFzNkJvrelNisVx; i++)
				{
					dataUpdater.axisValues[i] = itjrwJTskMOCIBDMHZFJeeJsVVi[i];
				}
				for (int j = 0; j < CTPHUcDNtgeoaxGwizOjFDKvPGkr; j++)
				{
					if (nwKnrVkNxXELctJfMnpjebSZpfqo[j])
					{
						dataUpdater.buttonPressureValues[j] = pEcCOvbGsUnLsfgcbxBXCcguTiKnc[j];
					}
					dataUpdater.buttonValues[j] = xVBtRMtpzANkcHPJxZjaDocWwgLe[j];
				}
				if (XYZltPJBRetDSHKEqiizKSJHVlFL && !dataUpdater.hasReceivedInput)
				{
					dataUpdater.hasReceivedInput = true;
				}
			}

			void IInputManagerJoystick.FillData(ControllerDataUpdater dataUpdater)
			{
				//ILSpy generated this explicit interface implementation from .override directive in FillData
				this.FillData(dataUpdater);
			}

			public BridgedControllerHWInfo PVYtEhnQSBodDawQSLLvGnODPenv()
			{
				BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
				ObHkKIVzgcemRnxfbJqnZzkpHmel(bridgedControllerHWInfo);
				return bridgedControllerHWInfo;
			}

			[CustomObfuscation(rename = false)]
			public BridgedController ToBridgedController()
			{
				BridgedController bridgedController = new BridgedController();
				ZJKLhELGAuLNJMiyVkgNghrbJbtl(bridgedController);
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
				return new ControllerDisconnectedEventArgs(PEFjaZNQhPjxZxlxWIjYbzmPcihs);
			}

			ControllerDisconnectedEventArgs IInputManagerJoystick.ToControllerDisconnectedEventArgs()
			{
				//ILSpy generated this explicit interface implementation from .override directive in ToControllerDisconnectedEventArgs
				return this.ToControllerDisconnectedEventArgs();
			}

			private void tHEAOAizHetqquKTJuXvNXVVKDrDA()
			{
				HardwareJoystickMap.Platform_Custom.Axis[] axes = ((HardwareJoystickMap.Platform_Custom)kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.map).Axes;
				if (axes == null)
				{
					return;
				}
				for (int i = 0; i < axes.Length; i++)
				{
					if (axes[i] != null)
					{
						if (i >= myFKrWzSPNuBooFzNkJvrelNisVx)
						{
							throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
						}
						itjrwJTskMOCIBDMHZFJeeJsVVi[i] = nuWGyWiAVmantqvOlCtbMFuQkkrj(axes[i]);
						if (!XYZltPJBRetDSHKEqiizKSJHVlFL && itjrwJTskMOCIBDMHZFJeeJsVVi[i] != 0f)
						{
							XYZltPJBRetDSHKEqiizKSJHVlFL = true;
						}
					}
				}
			}

			private void qXWEoPKzLJSRrIshznMnlQNIPfGTA()
			{
				HardwareJoystickMap.Platform_Custom.Button[] buttons = ((HardwareJoystickMap.Platform_Custom)kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.map).Buttons;
				if (buttons == null)
				{
					return;
				}
				for (int i = 0; i < buttons.Length; i++)
				{
					if (i >= CTPHUcDNtgeoaxGwizOjFDKvPGkr)
					{
						throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
					}
					xVBtRMtpzANkcHPJxZjaDocWwgLe[i] = ONPCLnMbUVcLyooeZuIgAEOWIFrK(buttons[i], out pEcCOvbGsUnLsfgcbxBXCcguTiKnc[i]);
					if (!XYZltPJBRetDSHKEqiizKSJHVlFL && (xVBtRMtpzANkcHPJxZjaDocWwgLe[i] || (nwKnrVkNxXELctJfMnpjebSZpfqo[i] && pEcCOvbGsUnLsfgcbxBXCcguTiKnc[i] != 0f)))
					{
						XYZltPJBRetDSHKEqiizKSJHVlFL = true;
					}
				}
			}

			private bool ONPCLnMbUVcLyooeZuIgAEOWIFrK(HardwareJoystickMap.Platform_Custom.Button P_0, out float P_1)
			{
				if (P_0.sourceType == 0)
				{
					bool result = dDwIxAszaKQEVlacKNZTOwoxZQUA(P_0.sourceButton, out P_1);
					if (MathTools.Abs(P_1) <= P_0.axisDeadZone)
					{
						P_1 = 0f;
					}
					return result;
				}
				if (P_0.sourceType == 1)
				{
					P_1 = 0f;
					float num = ZoHePfakllmMBLLeAFHvDLLYuBfOA(P_0.sourceAxis);
					if (MathTools.Abs(num) <= P_0.axisDeadZone)
					{
						return false;
					}
					if (P_0.sourceAxisPole == Pole.Positive && num < 0f)
					{
						return false;
					}
					if (P_0.sourceAxisPole == Pole.Negative && num > 0f)
					{
						return false;
					}
					if (num < 0f)
					{
						num *= -1f;
					}
					if (num > 1f)
					{
						num = 1f;
					}
					P_1 = num;
					return true;
				}
				P_1 = 0f;
				return false;
			}

			private bool SOQbLhoAUZVwBcpoGhOcRhlFwWNj(float P_0, float P_1)
			{
				return MathTools.IsNear(P_1, P_0, 0.1f);
			}

			private float nuWGyWiAVmantqvOlCtbMFuQkkrj(HardwareJoystickMap.Platform_Custom.Axis P_0)
			{
				if (P_0.sourceType == 1)
				{
					return ZoHePfakllmMBLLeAFHvDLLYuBfOA(P_0.sourceAxis);
				}
				if (P_0.sourceType == 0)
				{
					if (!dDwIxAszaKQEVlacKNZTOwoxZQUA(P_0.sourceButton, out var _))
					{
						return 0f;
					}
					if (P_0.buttonAxisContribution == Pole.Positive)
					{
						return 1f;
					}
					return -1f;
				}
				throw new NotImplementedException();
			}

			private float ZoHePfakllmMBLLeAFHvDLLYuBfOA(int P_0)
			{
				return KHpSPJfGZxvBentoOqzsGGiKAch.GetAxisValue(P_0);
			}

			private bool dDwIxAszaKQEVlacKNZTOwoxZQUA(int P_0, out float P_1)
			{
				KHpSPJfGZxvBentoOqzsGGiKAch.ljiSaOCcXfofdbVaKCGSKyocBRMG(P_0, out var result, out P_1);
				return result;
			}

			private void RNWxDbtOOlnUkPCTQPyYWLDdTCmK()
			{
				kXxhbxFCNNbeZuCaCeaPIVwoxfJSA = xiIyIGzbtOTBCGbQEIlpkLTWSGlF(PVYtEhnQSBodDawQSLLvGnODPenv());
				if (kXxhbxFCNNbeZuCaCeaPIVwoxfJSA == null)
				{
					Logger.LogError("Default hardware map not found!");
					return;
				}
				if (KHpSPJfGZxvBentoOqzsGGiKAch is IInputManagerHardwareJoystickMapHandler)
				{
					try
					{
						((IInputManagerHardwareJoystickMapHandler)KHpSPJfGZxvBentoOqzsGGiKAch).InitializeHardwareJoystickMap(kXxhbxFCNNbeZuCaCeaPIVwoxfJSA);
					}
					catch
					{
					}
				}
				myFKrWzSPNuBooFzNkJvrelNisVx = kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.axisCount;
				CTPHUcDNtgeoaxGwizOjFDKvPGkr = kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.buttonCount;
			}

			private void hrCGvStroHOETeGznCixYNTZEfOE()
			{
				Array.Clear(xVBtRMtpzANkcHPJxZjaDocWwgLe, 0, xVBtRMtpzANkcHPJxZjaDocWwgLe.Length);
				Array.Clear(pEcCOvbGsUnLsfgcbxBXCcguTiKnc, 0, pEcCOvbGsUnLsfgcbxBXCcguTiKnc.Length);
				Array.Clear(itjrwJTskMOCIBDMHZFJeeJsVVi, 0, itjrwJTskMOCIBDMHZFJeeJsVVi.Length);
			}

			private string sLebBvQqwuzjKNXafgPPskjwXEWb()
			{
				if (ReInput.currentPlatform == Platform.Webplayer)
				{
					return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{ReInput.webplayerPlatform.ToString()}{OLOjHLgIfUnBaBNZDiiPPMrRCYTb.ToString()}{ZhlGFebTIkcNpfiKbeMfkkvnrNNqA}");
				}
				if (pvRPaQsIqigEcORkKjmYXGlVnyZO.GqHFnYqaTUNNdsujzsTuwmmbVupw)
				{
					return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{pvRPaQsIqigEcORkKjmYXGlVnyZO.NHVnMXpAyAALDdpkOKuGoAwnKIZjA()}{ZhlGFebTIkcNpfiKbeMfkkvnrNNqA}");
				}
				return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{OLOjHLgIfUnBaBNZDiiPPMrRCYTb.ToString()}{ZhlGFebTIkcNpfiKbeMfkkvnrNNqA}");
			}

			bool ITryGetLocalizedName.TryGetLocalizedName(out string value)
			{
				if (!(KHpSPJfGZxvBentoOqzsGGiKAch is ITryGetLocalizedName))
				{
					if (FttlbvpYdardZKjZfHOzBcTihZPo)
					{
						if ((LocalizationManager.GetAndUpdateLocalizedString(vMJiGUectNZffFWGlGhlFhiWGMWO, kXxhbxFCNNbeZuCaCeaPIVwoxfJSA.deviceLocalizationInfo.parentKeys, "controller", Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Ename, out value) & LocalizationManager.GetAndUpdateLocalizedStringResultFlags.Changed) != LocalizationManager.GetAndUpdateLocalizedStringResultFlags.None)
						{
							string text = Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Ename;
							string text2 = null;
							MatchCollection matchCollection = Regex.Matches(text, "^(.*) ([0-9]+)$");
							if (matchCollection.Count > 0 && matchCollection[0].Groups != null && matchCollection[0].Groups.Count > 2)
							{
								text = matchCollection[0].Groups[1].Value;
								text2 = matchCollection[0].Groups[2].Value;
							}
							if (!string.IsNullOrEmpty(text2))
							{
								value = $"{text} {text2}";
							}
							vMJiGUectNZffFWGlGhlFhiWGMWO.cachedValue = value;
						}
						return true;
					}
					value = null;
					return false;
				}
				return ((ITryGetLocalizedName)KHpSPJfGZxvBentoOqzsGGiKAch).TryGetLocalizedName(out value);
			}

			public static int XIcHvKDcehIJNzjMDjCmRuFRRugh(eMRBpxOrihViWrGtUAboblLLhaOq P_0, eMRBpxOrihViWrGtUAboblLLhaOq P_1)
			{
				if (P_0.tnQDrvLlNlcAYAtWOdRqgMGquElsA < P_1.tnQDrvLlNlcAYAtWOdRqgMGquElsA)
				{
					return -1;
				}
				if (P_0.tnQDrvLlNlcAYAtWOdRqgMGquElsA > P_1.tnQDrvLlNlcAYAtWOdRqgMGquElsA)
				{
					return 1;
				}
				return 0;
			}

			public static int lqCTGsXOypdfRIvjrXMMTkHTMPNJA(eMRBpxOrihViWrGtUAboblLLhaOq P_0, eMRBpxOrihViWrGtUAboblLLhaOq P_1)
			{
				if (P_0.lgPPHFeVHDvJiQOPeHhmBANPtVhW < P_1.lgPPHFeVHDvJiQOPeHhmBANPtVhW)
				{
					return -1;
				}
				if (P_0.lgPPHFeVHDvJiQOPeHhmBANPtVhW > P_1.lgPPHFeVHDvJiQOPeHhmBANPtVhW)
				{
					return 1;
				}
				return 0;
			}
		}

		private class AZEwCZSvLdcVzQKEtNUyzoCuwKSI
		{
			public enum difiVlFYGwibpKOPmqeXULMHVAAp
			{
				Exact = 0,
				Approximate = 1
			}

			public class kDKDuakmleOWRHxMStnOpEONLkDu
			{
				public int ENizFfQtGYFZWMYKHkCyVOHOBmkh;

				public long? UVluCNyeVvEGqqBwczamVFbUQczJ;

				public string GGnQaYyFuOMXVTsxvZUHXCPaAeRo;

				public int fUjrlntmGorLFOugqVTPLdhNQLYc;

				public int upRmtvTirCjplhzwhlMxZMIoaWzr;

				public int fVOedUXPUuFCgFURfvOcrRgoFbpJA;

				public kDKDuakmleOWRHxMStnOpEONLkDu(int P_0, long? P_1, string P_2, int P_3, int P_4, int P_5)
				{
					ENizFfQtGYFZWMYKHkCyVOHOBmkh = P_0;
					UVluCNyeVvEGqqBwczamVFbUQczJ = P_1;
					GGnQaYyFuOMXVTsxvZUHXCPaAeRo = P_2;
					fUjrlntmGorLFOugqVTPLdhNQLYc = P_3;
					upRmtvTirCjplhzwhlMxZMIoaWzr = P_4;
					fVOedUXPUuFCgFURfvOcrRgoFbpJA = P_5;
				}

				public bool MdDySpcyVCGhREXKIhZbSQPGvOPS(eMRBpxOrihViWrGtUAboblLLhaOq P_0, difiVlFYGwibpKOPmqeXULMHVAAp P_1)
				{
					if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == ENizFfQtGYFZWMYKHkCyVOHOBmkh)
					{
						return true;
					}
					if (P_0.TWOyaBHQrchfKBdzbwamgOfAjzTQA != upRmtvTirCjplhzwhlMxZMIoaWzr)
					{
						return false;
					}
					if (P_0.LBekxSnMnDVzlBEcariaaWLiDnXB != fVOedUXPUuFCgFURfvOcrRgoFbpJA)
					{
						return false;
					}
					switch (P_1)
					{
					case difiVlFYGwibpKOPmqeXULMHVAAp.Exact:
						if (UVluCNyeVvEGqqBwczamVFbUQczJ == P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EsystemId)
						{
							return GGnQaYyFuOMXVTsxvZUHXCPaAeRo == P_0.ZhlGFebTIkcNpfiKbeMfkkvnrNNqA;
						}
						return false;
					case difiVlFYGwibpKOPmqeXULMHVAAp.Approximate:
						return GGnQaYyFuOMXVTsxvZUHXCPaAeRo == P_0.ZhlGFebTIkcNpfiKbeMfkkvnrNNqA;
					default:
						throw new NotImplementedException();
					}
				}
			}

			private sealed class ptPiELtCwjswwuVLGfvDxGvaONxU : IEnumerable<kDKDuakmleOWRHxMStnOpEONLkDu>, IEnumerable, IEnumerator<kDKDuakmleOWRHxMStnOpEONLkDu>, IEnumerator, IDisposable
			{
				private int SXCtanZkTNWTCUrYfVxBUegKGnqgA;

				private kDKDuakmleOWRHxMStnOpEONLkDu CMHfPxKrMXNMhUcZOwiEYDyFAgTN;

				private int xOMpBYCWGJNLCKSMSsVyzdZVjiQA;

				public AZEwCZSvLdcVzQKEtNUyzoCuwKSI TlejczRZLmhyPmfGyAJTPjQEAALFA;

				private eMRBpxOrihViWrGtUAboblLLhaOq CkKFVGGSKYHZcqQUHoiiExVlgsam;

				public eMRBpxOrihViWrGtUAboblLLhaOq wgzPyUzcgPviMkBEYcOodwdlorll;

				private difiVlFYGwibpKOPmqeXULMHVAAp EIQlUlpnSvKfKkLCmquahWCGOIGS;

				public difiVlFYGwibpKOPmqeXULMHVAAp QvdikqzsquWQWUxaJQNuyEcZaSVV;

				private int NYdEXhhPSMLMDENNGcIcUZSlbeHcA;

				private int ortvSfYRflBZghxSYguYhRxPDVfQ;

				kDKDuakmleOWRHxMStnOpEONLkDu IEnumerator<kDKDuakmleOWRHxMStnOpEONLkDu>.Current
				{
					[DebuggerHidden]
					get
					{
						return CMHfPxKrMXNMhUcZOwiEYDyFAgTN;
					}
				}

				object IEnumerator.Current
				{
					[DebuggerHidden]
					get
					{
						return CMHfPxKrMXNMhUcZOwiEYDyFAgTN;
					}
				}

				[DebuggerHidden]
				public ptPiELtCwjswwuVLGfvDxGvaONxU(int P_0)
				{
					SXCtanZkTNWTCUrYfVxBUegKGnqgA = P_0;
					xOMpBYCWGJNLCKSMSsVyzdZVjiQA = Environment.CurrentManagedThreadId;
				}

				[DebuggerHidden]
				void IDisposable.Dispose()
				{
					SXCtanZkTNWTCUrYfVxBUegKGnqgA = -2;
				}

				private bool MoveNext()
				{
					int sXCtanZkTNWTCUrYfVxBUegKGnqgA = SXCtanZkTNWTCUrYfVxBUegKGnqgA;
					AZEwCZSvLdcVzQKEtNUyzoCuwKSI tlejczRZLmhyPmfGyAJTPjQEAALFA = TlejczRZLmhyPmfGyAJTPjQEAALFA;
					if (sXCtanZkTNWTCUrYfVxBUegKGnqgA != 0)
					{
						if (sXCtanZkTNWTCUrYfVxBUegKGnqgA != 1)
						{
							return false;
						}
						SXCtanZkTNWTCUrYfVxBUegKGnqgA = -1;
						goto IL_0083;
					}
					SXCtanZkTNWTCUrYfVxBUegKGnqgA = -1;
					NYdEXhhPSMLMDENNGcIcUZSlbeHcA = tlejczRZLmhyPmfGyAJTPjQEAALFA.OjgTKdOQpEiATsjIRBDuiFnNbgYRA.Count;
					ortvSfYRflBZghxSYguYhRxPDVfQ = 0;
					goto IL_0093;
					IL_0083:
					ortvSfYRflBZghxSYguYhRxPDVfQ++;
					goto IL_0093;
					IL_0093:
					if (ortvSfYRflBZghxSYguYhRxPDVfQ < NYdEXhhPSMLMDENNGcIcUZSlbeHcA)
					{
						if (tlejczRZLmhyPmfGyAJTPjQEAALFA.OjgTKdOQpEiATsjIRBDuiFnNbgYRA[ortvSfYRflBZghxSYguYhRxPDVfQ].MdDySpcyVCGhREXKIhZbSQPGvOPS(CkKFVGGSKYHZcqQUHoiiExVlgsam, EIQlUlpnSvKfKkLCmquahWCGOIGS))
						{
							CMHfPxKrMXNMhUcZOwiEYDyFAgTN = tlejczRZLmhyPmfGyAJTPjQEAALFA.OjgTKdOQpEiATsjIRBDuiFnNbgYRA[ortvSfYRflBZghxSYguYhRxPDVfQ];
							SXCtanZkTNWTCUrYfVxBUegKGnqgA = 1;
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
				IEnumerator<kDKDuakmleOWRHxMStnOpEONLkDu> IEnumerable<kDKDuakmleOWRHxMStnOpEONLkDu>.GetEnumerator()
				{
					ptPiELtCwjswwuVLGfvDxGvaONxU ptPiELtCwjswwuVLGfvDxGvaONxU2;
					if (SXCtanZkTNWTCUrYfVxBUegKGnqgA == -2 && xOMpBYCWGJNLCKSMSsVyzdZVjiQA == Environment.CurrentManagedThreadId)
					{
						SXCtanZkTNWTCUrYfVxBUegKGnqgA = 0;
						ptPiELtCwjswwuVLGfvDxGvaONxU2 = this;
					}
					else
					{
						ptPiELtCwjswwuVLGfvDxGvaONxU2 = new ptPiELtCwjswwuVLGfvDxGvaONxU(0);
						ptPiELtCwjswwuVLGfvDxGvaONxU2.TlejczRZLmhyPmfGyAJTPjQEAALFA = TlejczRZLmhyPmfGyAJTPjQEAALFA;
					}
					ptPiELtCwjswwuVLGfvDxGvaONxU2.CkKFVGGSKYHZcqQUHoiiExVlgsam = wgzPyUzcgPviMkBEYcOodwdlorll;
					ptPiELtCwjswwuVLGfvDxGvaONxU2.EIQlUlpnSvKfKkLCmquahWCGOIGS = QvdikqzsquWQWUxaJQNuyEcZaSVV;
					return ptPiELtCwjswwuVLGfvDxGvaONxU2;
				}

				[DebuggerHidden]
				IEnumerator IEnumerable.GetEnumerator()
				{
					return ((IEnumerable<kDKDuakmleOWRHxMStnOpEONLkDu>)this).GetEnumerator();
				}
			}

			private List<kDKDuakmleOWRHxMStnOpEONLkDu> OjgTKdOQpEiATsjIRBDuiFnNbgYRA;

			public int RCHdBGTtYpvxqAwKpRAMQeQZNVZQ => OjgTKdOQpEiATsjIRBDuiFnNbgYRA.Count;

			public AZEwCZSvLdcVzQKEtNUyzoCuwKSI()
			{
				OjgTKdOQpEiATsjIRBDuiFnNbgYRA = new List<kDKDuakmleOWRHxMStnOpEONLkDu>();
			}

			public void XOFwtsKCObjvttnTtyZxJUrUmPZF(eMRBpxOrihViWrGtUAboblLLhaOq P_0)
			{
				if (P_0 == null)
				{
					return;
				}
				int count = OjgTKdOQpEiATsjIRBDuiFnNbgYRA.Count;
				for (int i = 0; i < count; i++)
				{
					if (OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i].MdDySpcyVCGhREXKIhZbSQPGvOPS(P_0, difiVlFYGwibpKOPmqeXULMHVAAp.Exact))
					{
						OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i].ENizFfQtGYFZWMYKHkCyVOHOBmkh = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
						OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i].UVluCNyeVvEGqqBwczamVFbUQczJ = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EsystemId;
						OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i].GGnQaYyFuOMXVTsxvZUHXCPaAeRo = P_0.ZhlGFebTIkcNpfiKbeMfkkvnrNNqA;
						OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i].fUjrlntmGorLFOugqVTPLdhNQLYc = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId;
						OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i].upRmtvTirCjplhzwhlMxZMIoaWzr = P_0.TWOyaBHQrchfKBdzbwamgOfAjzTQA;
						OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i].fVOedUXPUuFCgFURfvOcrRgoFbpJA = P_0.LBekxSnMnDVzlBEcariaaWLiDnXB;
						DjReYKFiLlADBOuNQDoYHMoUITXE(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, i);
						return;
					}
				}
				OjgTKdOQpEiATsjIRBDuiFnNbgYRA.Add(new kDKDuakmleOWRHxMStnOpEONLkDu(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EsystemId, P_0.ZhlGFebTIkcNpfiKbeMfkkvnrNNqA, P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId, P_0.TWOyaBHQrchfKBdzbwamgOfAjzTQA, P_0.LBekxSnMnDVzlBEcariaaWLiDnXB));
				DjReYKFiLlADBOuNQDoYHMoUITXE(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, OjgTKdOQpEiATsjIRBDuiFnNbgYRA.Count - 1);
			}

			public bool iCoukpZqStvQCZKEYvFUGbRefRZc(eMRBpxOrihViWrGtUAboblLLhaOq P_0, difiVlFYGwibpKOPmqeXULMHVAAp P_1)
			{
				int count = OjgTKdOQpEiATsjIRBDuiFnNbgYRA.Count;
				for (int i = 0; i < count; i++)
				{
					if (OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i].MdDySpcyVCGhREXKIhZbSQPGvOPS(P_0, P_1))
					{
						return true;
					}
				}
				return false;
			}

			[IteratorStateMachine(typeof(ptPiELtCwjswwuVLGfvDxGvaONxU))]
			public IEnumerable<kDKDuakmleOWRHxMStnOpEONLkDu> pvNPglaGEiGUZSoXUlAmmAGbcFAQ(eMRBpxOrihViWrGtUAboblLLhaOq P_0, difiVlFYGwibpKOPmqeXULMHVAAp P_1)
			{
				return new ptPiELtCwjswwuVLGfvDxGvaONxU(-2)
				{
					TlejczRZLmhyPmfGyAJTPjQEAALFA = this,
					wgzPyUzcgPviMkBEYcOodwdlorll = P_0,
					QvdikqzsquWQWUxaJQNuyEcZaSVV = P_1
				};
			}

			public int UZvBEXDDzTTUGSBEAmUDeCtAPTdW(kDKDuakmleOWRHxMStnOpEONLkDu P_0)
			{
				int count = OjgTKdOQpEiATsjIRBDuiFnNbgYRA.Count;
				for (int i = 0; i < count; i++)
				{
					if (OjgTKdOQpEiATsjIRBDuiFnNbgYRA[i] == P_0)
					{
						return i;
					}
				}
				return -1;
			}

			private void DjReYKFiLlADBOuNQDoYHMoUITXE(int P_0, int P_1)
			{
				for (int num = OjgTKdOQpEiATsjIRBDuiFnNbgYRA.Count - 1; num >= 0; num--)
				{
					if (num != P_1 && OjgTKdOQpEiATsjIRBDuiFnNbgYRA[num].ENizFfQtGYFZWMYKHkCyVOHOBmkh == P_0)
					{
						OjgTKdOQpEiATsjIRBDuiFnNbgYRA.RemoveAt(num);
					}
				}
			}
		}

		private List<eMRBpxOrihViWrGtUAboblLLhaOq> iyXhXNTJdGgFoYmyXEHgXlIQryup;

		private int dcnefqGdLffOvTsYUThvgNKKMaXh;

		private AZEwCZSvLdcVzQKEtNUyzoCuwKSI KemOKeKfqnayYnjrbbJMcfQcreHgA;

		private UpdateLoopType cOjbFoipYObcTBxMdgESNUEtgqdsA;

		private Action<int, ControllerDataUpdater> OpkFTrDmYKVzJuHasYMyyqyIQuaf;

		private PlatformInputManager yEwheghgmQqcqjJNFndcwKwdIihIb;

		private CustomInputSource iCuhWTxoqjxdScvtOiacLiUjFMHC;

		private bool cjbezUMldGYdcAJddEoETXsHtpVx;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> IbtRuZVWKTzmhRbzGiJhGsRIgtEB;

		private Func<int> UOGVXCCfQmVfdbqWMXMEklzWbYkW;

		[CustomObfuscation(rename = false)]
		int PlatformInputManager.deviceCount => dcnefqGdLffOvTsYUThvgNKKMaXh;

		[CustomObfuscation(rename = false)]
		PlatformInputManager PlatformInputManager.primaryInputManager => yEwheghgmQqcqjJNFndcwKwdIihIb;

		[CustomObfuscation(rename = false)]
		IInputSource PlatformInputManager.inputSource => null;

		[CustomObfuscation(rename = false)]
		InputSource PlatformInputManager.inputSourceType => iCuhWTxoqjxdScvtOiacLiUjFMHC.ynIdjZCpVlwTpZFWgFUmNBinSjFu;

		public CustomInputManager(CustomInputSource P_0, UpdateLoopSetting P_1, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_2, Func<int> P_3)
		{
			iCuhWTxoqjxdScvtOiacLiUjFMHC = P_0;
			IbtRuZVWKTzmhRbzGiJhGsRIgtEB = P_2;
			UOGVXCCfQmVfdbqWMXMEklzWbYkW = P_3;
			yEwheghgmQqcqjJNFndcwKwdIihIb = this;
			try
			{
				OpkFTrDmYKVzJuHasYMyyqyIQuaf = UpdateControllerData;
				P_0.ELgzSCNUHMcXrkZIbwNubcEZCRLO += SystemDeviceConnected;
				P_0.gyzaOhVNWBapcBNiAHNFidYiUZvDA += SystemDeviceDisconnected;
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
			KemOKeKfqnayYnjrbbJMcfQcreHgA = new AZEwCZSvLdcVzQKEtNUyzoCuwKSI();
			iyXhXNTJdGgFoYmyXEHgXlIQryup = new List<eMRBpxOrihViWrGtUAboblLLhaOq>();
			cjbezUMldGYdcAJddEoETXsHtpVx = true;
			iCuhWTxoqjxdScvtOiacLiUjFMHC.BrcOHbBoQMdzRAEoYlowdclvaIsvA();
		}

		[CustomObfuscation(rename = false)]
		public override void Update(UpdateLoopType updateLoop)
		{
			cOjbFoipYObcTBxMdgESNUEtgqdsA = updateLoop;
			if (iCuhWTxoqjxdScvtOiacLiUjFMHC.isReady)
			{
				iCuhWTxoqjxdScvtOiacLiUjFMHC.Update();
				iCuhWTxoqjxdScvtOiacLiUjFMHC.nmiHBHNfCudAZvaUpaMnJnprHnMy();
				if (cjbezUMldGYdcAJddEoETXsHtpVx)
				{
					fCANAcmOprJLmtcfvTDaMbtztOhJ();
				}
				UUBekRCUjbvPMOhhkWQRpgAPJgpVA();
			}
		}

		[CustomObfuscation(rename = false)]
		public override void OnDestroy()
		{
			if (iCuhWTxoqjxdScvtOiacLiUjFMHC != null)
			{
				iCuhWTxoqjxdScvtOiacLiUjFMHC.Dispose();
			}
		}

		[CustomObfuscation(rename = false)]
		public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
		{
			return OpkFTrDmYKVzJuHasYMyyqyIQuaf;
		}

		[CustomObfuscation(rename = false)]
		public override void UpdateControllerData(int inputManagerId, ControllerDataUpdater data)
		{
			for (int i = 0; i < dcnefqGdLffOvTsYUThvgNKKMaXh; i++)
			{
				if (iyXhXNTJdGgFoYmyXEHgXlIQryup[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == inputManagerId)
				{
					iyXhXNTJdGgFoYmyXEHgXlIQryup[i].FillData(data);
					return;
				}
			}
			Logger.LogError("Invalid joystick Id " + inputManagerId + "!");
		}

		[CustomObfuscation(rename = false)]
		public override void SystemDeviceConnected()
		{
			cjbezUMldGYdcAJddEoETXsHtpVx = true;
			if (_SystemDeviceConnectedEvent != null)
			{
				_SystemDeviceConnectedEvent();
			}
		}

		[CustomObfuscation(rename = false)]
		public override void SystemDeviceDisconnected()
		{
			cjbezUMldGYdcAJddEoETXsHtpVx = true;
			if (_SystemDeviceDisconnectedEvent != null)
			{
				_SystemDeviceDisconnectedEvent();
			}
		}

		[CustomObfuscation(rename = false)]
		public override void SetUnityJoystickId(int joystickId, int unityJoystickIndex)
		{
		}

		[CustomObfuscation(rename = false)]
		public override IUnifiedMouseSource GetUnifiedMouseSource()
		{
			return iCuhWTxoqjxdScvtOiacLiUjFMHC.HcrPovgnNSheDcZTplDJNufqleGA();
		}

		[CustomObfuscation(rename = false)]
		public override IUnifiedKeyboardSource GetUnifiedKeyboardSource()
		{
			return iCuhWTxoqjxdScvtOiacLiUjFMHC.CgmdcXLZYwNvCEAMqCnUidmgmAhq();
		}

		private void HkmGYqTdOdkjlhJNJNWBqhvBFYR(CustomInputSource.Joystick[] P_0)
		{
			int num = 0;
			List<eMRBpxOrihViWrGtUAboblLLhaOq> list = iyXhXNTJdGgFoYmyXEHgXlIQryup;
			int num2 = dcnefqGdLffOvTsYUThvgNKKMaXh;
			iyXhXNTJdGgFoYmyXEHgXlIQryup = new List<eMRBpxOrihViWrGtUAboblLLhaOq>();
			for (int i = 0; i < P_0.Length; i++)
			{
				if (P_0[i] != null)
				{
					eMRBpxOrihViWrGtUAboblLLhaOq item = new eMRBpxOrihViWrGtUAboblLLhaOq(iCuhWTxoqjxdScvtOiacLiUjFMHC, P_0[i].systemId, P_0[i].unityId, P_0[i], iCuhWTxoqjxdScvtOiacLiUjFMHC.ynIdjZCpVlwTpZFWgFUmNBinSjFu, P_0[i].extension, IbtRuZVWKTzmhRbzGiJhGsRIgtEB);
					iyXhXNTJdGgFoYmyXEHgXlIQryup.Add(item);
					num++;
				}
			}
			dcnefqGdLffOvTsYUThvgNKKMaXh = num;
			jovBjZdJzWXlIPVismcdBlXGLYnCb(num2, num, list, iyXhXNTJdGgFoYmyXEHgXlIQryup);
			for (int j = 0; j < num; j++)
			{
				if (_UpdateControllerInfoEvent != null)
				{
					_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(iyXhXNTJdGgFoYmyXEHgXlIQryup[j]));
				}
			}
			IsDACpAVasmCXQnQwdaEHkmmuSsRA(list, iyXhXNTJdGgFoYmyXEHgXlIQryup, false);
			IsDACpAVasmCXQnQwdaEHkmmuSsRA(iyXhXNTJdGgFoYmyXEHgXlIQryup, list, true);
		}

		private void UUBekRCUjbvPMOhhkWQRpgAPJgpVA()
		{
			for (int i = 0; i < dcnefqGdLffOvTsYUThvgNKKMaXh; i++)
			{
				iyXhXNTJdGgFoYmyXEHgXlIQryup[i].Update();
			}
		}

		private void jovBjZdJzWXlIPVismcdBlXGLYnCb(int P_0, int P_1, List<eMRBpxOrihViWrGtUAboblLLhaOq> P_2, List<eMRBpxOrihViWrGtUAboblLLhaOq> P_3)
		{
			if (P_1 > 0)
			{
				P_3.Sort(eMRBpxOrihViWrGtUAboblLLhaOq.lqCTGsXOypdfRIvjrXMMTkHTMPNJA);
			}
			if (P_0 > 0 && P_1 > 0)
			{
				FBcAmWqXColSMfHHEGumrcuArLlG(P_1, P_3, P_0, P_2, AZEwCZSvLdcVzQKEtNUyzoCuwKSI.difiVlFYGwibpKOPmqeXULMHVAAp.Exact);
				if (iCuhWTxoqjxdScvtOiacLiUjFMHC.useApproximateMatching)
				{
					FBcAmWqXColSMfHHEGumrcuArLlG(P_1, P_3, P_0, P_2, AZEwCZSvLdcVzQKEtNUyzoCuwKSI.difiVlFYGwibpKOPmqeXULMHVAAp.Approximate);
				}
			}
			mAnZqJnLphzvgwClNPswDZvjiJqO(P_1, P_3, AZEwCZSvLdcVzQKEtNUyzoCuwKSI.difiVlFYGwibpKOPmqeXULMHVAAp.Exact);
			if (iCuhWTxoqjxdScvtOiacLiUjFMHC.useApproximateMatching)
			{
				mAnZqJnLphzvgwClNPswDZvjiJqO(P_1, P_3, AZEwCZSvLdcVzQKEtNUyzoCuwKSI.difiVlFYGwibpKOPmqeXULMHVAAp.Approximate);
			}
			for (int i = 0; i < P_1; i++)
			{
				eMRBpxOrihViWrGtUAboblLLhaOq eMRBpxOrihViWrGtUAboblLLhaOq2 = P_3[i];
				if (eMRBpxOrihViWrGtUAboblLLhaOq2 != null && eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId < 0)
				{
					eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = xPmLsUrKIypIxLfVohCfjrFBjusO(P_3);
					eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = ReInput.GetNewJoystickId();
					KemOKeKfqnayYnjrbbJMcfQcreHgA.XOFwtsKCObjvttnTtyZxJUrUmPZF(eMRBpxOrihViWrGtUAboblLLhaOq2);
				}
			}
			P_3.Sort(eMRBpxOrihViWrGtUAboblLLhaOq.XIcHvKDcehIJNzjMDjCmRuFRRugh);
		}

		private void wbiolUfwNunSPxlaTRhAtsiTUqPU(List<eMRBpxOrihViWrGtUAboblLLhaOq> P_0, int P_1, int P_2)
		{
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				if (i != P_1 && P_0[i] != null && P_0[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == P_2)
				{
					P_0[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = -1;
				}
			}
		}

		private bool kNsMClxZytrhlQPJKaxSaIgBRTGPA(List<eMRBpxOrihViWrGtUAboblLLhaOq> P_0, int P_1)
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

		private int xPmLsUrKIypIxLfVohCfjrFBjusO(List<eMRBpxOrihViWrGtUAboblLLhaOq> P_0)
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

		private bool iXMzDGRIrlcmmKgXTtAqtSJiCIrIb(List<eMRBpxOrihViWrGtUAboblLLhaOq> P_0, int P_1)
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

		private void FBcAmWqXColSMfHHEGumrcuArLlG(int P_0, List<eMRBpxOrihViWrGtUAboblLLhaOq> P_1, int P_2, List<eMRBpxOrihViWrGtUAboblLLhaOq> P_3, AZEwCZSvLdcVzQKEtNUyzoCuwKSI.difiVlFYGwibpKOPmqeXULMHVAAp P_4)
		{
			int num = ((P_4 != AZEwCZSvLdcVzQKEtNUyzoCuwKSI.difiVlFYGwibpKOPmqeXULMHVAAp.Exact) ? 1 : 2);
			for (int i = 0; i < P_0; i++)
			{
				eMRBpxOrihViWrGtUAboblLLhaOq eMRBpxOrihViWrGtUAboblLLhaOq2 = P_1[i];
				if (eMRBpxOrihViWrGtUAboblLLhaOq2 == null || eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
				{
					continue;
				}
				for (int j = 0; j < P_2; j++)
				{
					eMRBpxOrihViWrGtUAboblLLhaOq eMRBpxOrihViWrGtUAboblLLhaOq3 = P_3[j];
					if (eMRBpxOrihViWrGtUAboblLLhaOq3 != null && !iXMzDGRIrlcmmKgXTtAqtSJiCIrIb(P_1, eMRBpxOrihViWrGtUAboblLLhaOq3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId) && eMRBpxOrihViWrGtUAboblLLhaOq2.foKVHXApBOczpRYNIVfazsxiLqip(eMRBpxOrihViWrGtUAboblLLhaOq3) >= num)
					{
						eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = eMRBpxOrihViWrGtUAboblLLhaOq3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId;
						eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = eMRBpxOrihViWrGtUAboblLLhaOq3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
						KemOKeKfqnayYnjrbbJMcfQcreHgA.XOFwtsKCObjvttnTtyZxJUrUmPZF(eMRBpxOrihViWrGtUAboblLLhaOq2);
					}
				}
			}
		}

		private void mAnZqJnLphzvgwClNPswDZvjiJqO(int P_0, List<eMRBpxOrihViWrGtUAboblLLhaOq> P_1, AZEwCZSvLdcVzQKEtNUyzoCuwKSI.difiVlFYGwibpKOPmqeXULMHVAAp P_2)
		{
			for (int i = 0; i < P_0; i++)
			{
				eMRBpxOrihViWrGtUAboblLLhaOq eMRBpxOrihViWrGtUAboblLLhaOq2 = P_1[i];
				if (eMRBpxOrihViWrGtUAboblLLhaOq2 == null || eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
				{
					continue;
				}
				AZEwCZSvLdcVzQKEtNUyzoCuwKSI.kDKDuakmleOWRHxMStnOpEONLkDu kDKDuakmleOWRHxMStnOpEONLkDu = null;
				foreach (AZEwCZSvLdcVzQKEtNUyzoCuwKSI.kDKDuakmleOWRHxMStnOpEONLkDu item in KemOKeKfqnayYnjrbbJMcfQcreHgA.pvNPglaGEiGUZSoXUlAmmAGbcFAQ(eMRBpxOrihViWrGtUAboblLLhaOq2, P_2))
				{
					if (!iXMzDGRIrlcmmKgXTtAqtSJiCIrIb(P_1, item.ENizFfQtGYFZWMYKHkCyVOHOBmkh) && item.fUjrlntmGorLFOugqVTPLdhNQLYc >= 0)
					{
						kDKDuakmleOWRHxMStnOpEONLkDu = item;
						break;
					}
				}
				if (kDKDuakmleOWRHxMStnOpEONLkDu != null)
				{
					int num = kDKDuakmleOWRHxMStnOpEONLkDu.fUjrlntmGorLFOugqVTPLdhNQLYc;
					if (!kNsMClxZytrhlQPJKaxSaIgBRTGPA(P_1, num))
					{
						num = (kDKDuakmleOWRHxMStnOpEONLkDu.fUjrlntmGorLFOugqVTPLdhNQLYc = xPmLsUrKIypIxLfVohCfjrFBjusO(P_1));
					}
					eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = num;
					eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = kDKDuakmleOWRHxMStnOpEONLkDu.ENizFfQtGYFZWMYKHkCyVOHOBmkh;
					KemOKeKfqnayYnjrbbJMcfQcreHgA.XOFwtsKCObjvttnTtyZxJUrUmPZF(eMRBpxOrihViWrGtUAboblLLhaOq2);
				}
			}
		}

		private void fCANAcmOprJLmtcfvTDaMbtztOhJ()
		{
			CustomInputSource.Joystick[] array = iCuhWTxoqjxdScvtOiacLiUjFMHC.YXuIcbyhIzTJDUqKWkzLEETsGHeV();
			if (CZXtEvbAwRAsaBcXwrxmNWVcRrGwA(array))
			{
				HkmGYqTdOdkjlhJNJNWBqhvBFYR(array);
			}
			cjbezUMldGYdcAJddEoETXsHtpVx = false;
		}

		private bool CZXtEvbAwRAsaBcXwrxmNWVcRrGwA(CustomInputSource.Joystick[] P_0)
		{
			int num = P_0.Length;
			int count = iyXhXNTJdGgFoYmyXEHgXlIQryup.Count;
			if (num != count)
			{
				return true;
			}
			for (int i = 0; i < num; i++)
			{
				if (P_0[i] == null)
				{
					continue;
				}
				long? systemId = P_0[i].systemId;
				bool flag = false;
				for (int j = 0; j < count; j++)
				{
					if (iyXhXNTJdGgFoYmyXEHgXlIQryup[j] != null && systemId == iyXhXNTJdGgFoYmyXEHgXlIQryup[j].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EsystemId)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			for (int k = 0; k < count; k++)
			{
				if (iyXhXNTJdGgFoYmyXEHgXlIQryup[k] == null)
				{
					continue;
				}
				long? num2 = iyXhXNTJdGgFoYmyXEHgXlIQryup[k].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EsystemId;
				bool flag2 = false;
				for (int l = 0; l < num; l++)
				{
					if (P_0[l] != null && num2 == P_0[l].systemId)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return true;
				}
			}
			return false;
		}

		private void IsDACpAVasmCXQnQwdaEHkmmuSsRA(List<eMRBpxOrihViWrGtUAboblLLhaOq> P_0, List<eMRBpxOrihViWrGtUAboblLLhaOq> P_1, bool P_2)
		{
			if (P_0 == null)
			{
				return;
			}
			int num = P_0?.Count ?? 0;
			int num2 = P_1?.Count ?? 0;
			for (int i = 0; i < num; i++)
			{
				eMRBpxOrihViWrGtUAboblLLhaOq eMRBpxOrihViWrGtUAboblLLhaOq2 = P_0[i];
				if (eMRBpxOrihViWrGtUAboblLLhaOq2 == null)
				{
					continue;
				}
				bool flag = false;
				if (P_1 != null)
				{
					for (int j = 0; j < num2; j++)
					{
						eMRBpxOrihViWrGtUAboblLLhaOq eMRBpxOrihViWrGtUAboblLLhaOq3 = P_1[j];
						if (eMRBpxOrihViWrGtUAboblLLhaOq3 != null && eMRBpxOrihViWrGtUAboblLLhaOq2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == eMRBpxOrihViWrGtUAboblLLhaOq3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					honbSiISQUlmhQIbFrjzeeHUDMqF(P_0[i], P_2);
				}
			}
		}

		private void honbSiISQUlmhQIbFrjzeeHUDMqF(eMRBpxOrihViWrGtUAboblLLhaOq P_0, bool P_1)
		{
			if (P_1)
			{
				P_0.qQkbTWVwkdGKUDcxWgxqFibYzqPk();
			}
			RHvLywidIJSaPDKZpYMmiJOKagFn(P_0, P_1);
		}

		private void RHvLywidIJSaPDKZpYMmiJOKagFn(eMRBpxOrihViWrGtUAboblLLhaOq P_0, bool P_1)
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
	}
}
