using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Rewired;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

internal class pWRDtMxMZRbTxFlbqpruZytnvSMMA : PlatformInputManager
{
	private class yNiWVjPwkGeAdQDlCytbRfaFEdpd : IInputManagerJoystick, IInputManagerJoystickPublic
	{
		private int iBJLigwlrpREzcrOZRBYsQLACBxS;

		private int pMGcXGnpNlEuEoSoRzVdVwZtnLaY;

		private int didzuBeANhngrgHAykWqQoFewNRi;

		public Guid XNoWjSlkMoTudogcFNgcSpwdOElF;

		public string VBpfDcKvqfoAbLLVZevEXwbMOuTMA;

		public int rIqSfHxbNaLIcSMKroMldLDNRFdv;

		public string VBoNKBKRhflCmGnkNdKSCUnYrNKY;

		public string FnzIhrvNPciMldVLHwfNectrLChyA;

		private int pOrHBqJqyTterpPejSLZjASPWEbn = 29;

		private int kWrqOCMtzVPHhjrBXIHEtLGuBkoHA = 20;

		private float[] jhrbmZbplROuJQhBjIlWBHxTEWTV;

		private bool[] obOyrJIHptJvGVXEVwGyCqmnWcAw;

		private bool[] uQFjguVOLeWydxundbetOJbIdsrv;

		private float[] pyNmIkWjYwvzqSvUxBRfYGWfxvoM;

		private bool[] uABUTCPEHwWfkNdyubJevAWNfKlP;

		private HardwareJoystickMap_InputManager mpatVgWGKWbGcjJMCzykYfHofqIZ;

		private bool gZtNaGnJsjaXzXLBtydpWvcOnynR;

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.rewiredId
		{
			get
			{
				return iBJLigwlrpREzcrOZRBYsQLACBxS;
			}
			set
			{
				iBJLigwlrpREzcrOZRBYsQLACBxS = value;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.inputManagerId
		{
			get
			{
				return pMGcXGnpNlEuEoSoRzVdVwZtnLaY;
			}
			set
			{
				pMGcXGnpNlEuEoSoRzVdVwZtnLaY = value;
			}
		}

		[CustomObfuscation(rename = false)]
		string IInputManagerJoystickPublic.name
		{
			get
			{
				if (!(VBpfDcKvqfoAbLLVZevEXwbMOuTMA != "Unknown Controller"))
				{
					return VBoNKBKRhflCmGnkNdKSCUnYrNKY;
				}
				return VBpfDcKvqfoAbLLVZevEXwbMOuTMA;
			}
		}

		[CustomObfuscation(rename = false)]
		long? IInputManagerJoystickPublic.systemId
		{
			get
			{
				if (didzuBeANhngrgHAykWqQoFewNRi < 1)
				{
					return null;
				}
				return didzuBeANhngrgHAykWqQoFewNRi;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.unityId
		{
			get
			{
				return didzuBeANhngrgHAykWqQoFewNRi;
			}
			set
			{
				didzuBeANhngrgHAykWqQoFewNRi = value;
			}
		}

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.instanceGuid
		{
			get
			{
				if ((ReInput.isWindowsStandaloneWebplayerOrEditorPlatform && !UnityTools.windowsJoystickNamesReturnsEmptyStringsIfJoystickNull) || UnityTools.effectivePlatform == Platform.OSX)
				{
					return MiscTools.CreateGuidHashSHA1(Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Ename);
				}
				if (UnityTools.isIOSPlatform)
				{
					return MiscTools.CreateGuidHashSHA1(VBoNKBKRhflCmGnkNdKSCUnYrNKY);
				}
				return MiscTools.CreateGuidHashSHA1(Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Ename + "_" + didzuBeANhngrgHAykWqQoFewNRi);
			}
		}

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.persistentGuid => Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;

		[CustomObfuscation(rename = false)]
		Controller.Extension IInputManagerJoystickPublic.extension => null;

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

		public yNiWVjPwkGeAdQDlCytbRfaFEdpd()
		{
			pMGcXGnpNlEuEoSoRzVdVwZtnLaY = -1;
			iBJLigwlrpREzcrOZRBYsQLACBxS = -1;
			didzuBeANhngrgHAykWqQoFewNRi = 0;
		}

		public void CMpzAkyEOhiYDOadstACskPwdERkA()
		{
			wykWZNAVlAwgarJsuuGDffkOPAFt();
			XNoWjSlkMoTudogcFNgcSpwdOElF = mpatVgWGKWbGcjJMCzykYfHofqIZ.hardwareMapIdentifier.guid;
			VBpfDcKvqfoAbLLVZevEXwbMOuTMA = mpatVgWGKWbGcjJMCzykYfHofqIZ.controllerName;
			jhrbmZbplROuJQhBjIlWBHxTEWTV = new float[pOrHBqJqyTterpPejSLZjASPWEbn];
			obOyrJIHptJvGVXEVwGyCqmnWcAw = new bool[kWrqOCMtzVPHhjrBXIHEtLGuBkoHA];
			uQFjguVOLeWydxundbetOJbIdsrv = new bool[pOrHBqJqyTterpPejSLZjASPWEbn];
			uABUTCPEHwWfkNdyubJevAWNfKlP = new bool[29];
			pyNmIkWjYwvzqSvUxBRfYGWfxvoM = new float[29];
			Update();
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			if (didzuBeANhngrgHAykWqQoFewNRi > 0)
			{
				SifGpHfvTWsZbrOxWGeiuomrUFZhA();
				MUciRKlKQglpUZluuoXpcrdbkCSG();
				MUZeebJgKBukMkZqlOkaAHGfUBNE();
			}
		}

		void IInputManagerJoystick.Update()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Update
			this.Update();
		}

		public int ZWxiAIDOFPDoWuQvgckncugAqKjjb(yNiWVjPwkGeAdQDlCytbRfaFEdpd P_0)
		{
			if ((!string.IsNullOrEmpty(FnzIhrvNPciMldVLHwfNectrLChyA) || !string.IsNullOrEmpty(P_0.FnzIhrvNPciMldVLHwfNectrLChyA)) && !string.Equals(FnzIhrvNPciMldVLHwfNectrLChyA, P_0.FnzIhrvNPciMldVLHwfNectrLChyA, StringComparison.Ordinal))
			{
				return 0;
			}
			if (P_0.VBoNKBKRhflCmGnkNdKSCUnYrNKY == VBoNKBKRhflCmGnkNdKSCUnYrNKY && P_0.rIqSfHxbNaLIcSMKroMldLDNRFdv == rIqSfHxbNaLIcSMKroMldLDNRFdv)
			{
				return 2;
			}
			if (P_0.VBoNKBKRhflCmGnkNdKSCUnYrNKY == VBoNKBKRhflCmGnkNdKSCUnYrNKY)
			{
				return 1;
			}
			return 0;
		}

		private void gLsiAxUPalGsscqCqqDPdLZqhAfvA(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.Fallback;
			P_0.inputSource = JfTwARyBwCgQIccFyTufHOTnFwZeb();
			P_0.hardwareIdentifier = LahrDIgCEpbBItBMqFMujndKFEsqA();
			P_0.hardwareAxisCount = 0;
			P_0.hardwareButtonCount = 0;
			P_0.hardwareHatCount = 0;
			P_0.hw_productName = VBoNKBKRhflCmGnkNdKSCUnYrNKY;
		}

		private void IididArlqEXBPcvSDMHavDeaBmQW(BridgedController P_0)
		{
			gLsiAxUPalGsscqCqqDPdLZqhAfvA(P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = mpatVgWGKWbGcjJMCzykYfHofqIZ.ToGameHardwareControllerMap();
			P_0.instanceName = VBoNKBKRhflCmGnkNdKSCUnYrNKY;
			P_0.productName = VBoNKBKRhflCmGnkNdKSCUnYrNKY;
			P_0.isXInputDevice = false;
			P_0.axisCount = pOrHBqJqyTterpPejSLZjASPWEbn;
			P_0.buttonCount = kWrqOCMtzVPHhjrBXIHEtLGuBkoHA;
			P_0.controllerTypeGuid = XNoWjSlkMoTudogcFNgcSpwdOElF;
		}

		[CustomObfuscation(rename = false)]
		public void FillData(ControllerDataUpdater dataUpdater)
		{
			if (pOrHBqJqyTterpPejSLZjASPWEbn != dataUpdater.axisCount || kWrqOCMtzVPHhjrBXIHEtLGuBkoHA != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			float[] axisValues = dataUpdater.axisValues;
			bool[] axisHasBeenPressedOSXLinux = dataUpdater.axisHasBeenPressedOSXLinux;
			for (int i = 0; i < pOrHBqJqyTterpPejSLZjASPWEbn; i++)
			{
				if (axisValues[i] != jhrbmZbplROuJQhBjIlWBHxTEWTV[i])
				{
					axisValues[i] = jhrbmZbplROuJQhBjIlWBHxTEWTV[i];
					if (axisHasBeenPressedOSXLinux[i] != uQFjguVOLeWydxundbetOJbIdsrv[i])
					{
						axisHasBeenPressedOSXLinux[i] = uQFjguVOLeWydxundbetOJbIdsrv[i];
					}
				}
			}
			bool[] buttonValues = dataUpdater.buttonValues;
			for (int j = 0; j < kWrqOCMtzVPHhjrBXIHEtLGuBkoHA; j++)
			{
				if (buttonValues[j] != obOyrJIHptJvGVXEVwGyCqmnWcAw[j])
				{
					buttonValues[j] = obOyrJIHptJvGVXEVwGyCqmnWcAw[j];
				}
			}
			if (gZtNaGnJsjaXzXLBtydpWvcOnynR && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		void IInputManagerJoystick.FillData(ControllerDataUpdater dataUpdater)
		{
			//ILSpy generated this explicit interface implementation from .override directive in FillData
			this.FillData(dataUpdater);
		}

		public void PKkdUzpQMjltmWHbyfaCjckGfWjwA(int P_0)
		{
			if (P_0 >= 1 && P_0 <= 16)
			{
				Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId = P_0;
			}
		}

		public void HsvxAgMjLLpteYkgzexFGDwatgvoA()
		{
			didzuBeANhngrgHAykWqQoFewNRi = 0;
			pMSCubdyyKFWYraJrTTjRCevNNBtA();
		}

		public BridgedControllerHWInfo TOUfFWgxDyvDFcggSfsxgnZloWQnA()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
			gLsiAxUPalGsscqCqqDPdLZqhAfvA(bridgedControllerHWInfo);
			return bridgedControllerHWInfo;
		}

		[CustomObfuscation(rename = false)]
		public BridgedController ToBridgedController()
		{
			BridgedController bridgedController = new BridgedController();
			IididArlqEXBPcvSDMHavDeaBmQW(bridgedController);
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
			return new ControllerDisconnectedEventArgs(iBJLigwlrpREzcrOZRBYsQLACBxS);
		}

		ControllerDisconnectedEventArgs IInputManagerJoystick.ToControllerDisconnectedEventArgs()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToControllerDisconnectedEventArgs
			return this.ToControllerDisconnectedEventArgs();
		}

		private void SifGpHfvTWsZbrOxWGeiuomrUFZhA()
		{
			for (int i = 0; i < 29; i++)
			{
				float joystickAxisValueByJoystickId = UnityInputHelper.GetJoystickAxisValueByJoystickId(didzuBeANhngrgHAykWqQoFewNRi, i);
				if (pyNmIkWjYwvzqSvUxBRfYGWfxvoM[i] != joystickAxisValueByJoystickId)
				{
					pyNmIkWjYwvzqSvUxBRfYGWfxvoM[i] = joystickAxisValueByJoystickId;
					if (!uABUTCPEHwWfkNdyubJevAWNfKlP[i] && joystickAxisValueByJoystickId != 0f)
					{
						uABUTCPEHwWfkNdyubJevAWNfKlP[i] = true;
					}
				}
			}
		}

		private void MUciRKlKQglpUZluuoXpcrdbkCSG()
		{
			HardwareJoystickMap.Platform_Fallback_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_Fallback_Base)mpatVgWGKWbGcjJMCzykYfHofqIZ.map).Axes_orig;
			if (axes_orig == null)
			{
				return;
			}
			for (int i = 0; i < axes_orig.Length; i++)
			{
				if (axes_orig[i] == null)
				{
					continue;
				}
				if (i >= pOrHBqJqyTterpPejSLZjASPWEbn)
				{
					throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
				}
				float num = TWcVaCqRhPaZBVsDMKGxqRzNJKSl(axes_orig[i]);
				if (jhrbmZbplROuJQhBjIlWBHxTEWTV[i] == num)
				{
					continue;
				}
				jhrbmZbplROuJQhBjIlWBHxTEWTV[i] = num;
				if (!uQFjguVOLeWydxundbetOJbIdsrv[i])
				{
					if (axes_orig[i].sourceType == HardwareElementSourceTypeWithHat.Axis)
					{
						float num2 = rbnvDkJgHQDgSEQTMuMVgPGFodPI(axes_orig[i].sourceAxis);
						uQFjguVOLeWydxundbetOJbIdsrv[i] = num2 != 0f;
					}
					else
					{
						uQFjguVOLeWydxundbetOJbIdsrv[i] = true;
					}
				}
				if (!gZtNaGnJsjaXzXLBtydpWvcOnynR && jhrbmZbplROuJQhBjIlWBHxTEWTV[i] != 0f)
				{
					gZtNaGnJsjaXzXLBtydpWvcOnynR = true;
				}
			}
		}

		private void MUZeebJgKBukMkZqlOkaAHGfUBNE()
		{
			HardwareJoystickMap.Platform_Fallback_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_Fallback_Base)mpatVgWGKWbGcjJMCzykYfHofqIZ.map).Buttons_orig;
			if (buttons_orig == null)
			{
				return;
			}
			for (int i = 0; i < buttons_orig.Length; i++)
			{
				if (i >= kWrqOCMtzVPHhjrBXIHEtLGuBkoHA)
				{
					throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
				}
				bool flag = zNKSKuhZVBTEqNdHALUqkOPQihxw(buttons_orig[i]);
				if (obOyrJIHptJvGVXEVwGyCqmnWcAw[i] != flag)
				{
					obOyrJIHptJvGVXEVwGyCqmnWcAw[i] = flag;
					if (!gZtNaGnJsjaXzXLBtydpWvcOnynR && obOyrJIHptJvGVXEVwGyCqmnWcAw[i])
					{
						gZtNaGnJsjaXzXLBtydpWvcOnynR = true;
					}
				}
			}
		}

		private bool zNKSKuhZVBTEqNdHALUqkOPQihxw(HardwareJoystickMap.Platform_Fallback_Base.Button P_0)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				if (P_0.ignoreIfButtonsActive)
				{
					for (int i = 0; i < P_0.ignoreIfButtonsActiveButtons.Length; i++)
					{
						if (mccXHzWkfFBtXSOPjFqsryCBVemt(P_0.ignoreIfButtonsActiveButtons[i]))
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
						if (!mccXHzWkfFBtXSOPjFqsryCBVemt(P_0.requiredButtons[j]))
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
				if (P_0.sourceButton == UnityButton.None)
				{
					return false;
				}
				return mccXHzWkfFBtXSOPjFqsryCBVemt(P_0.sourceButton);
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				if (P_0.sourceAxis == UnityAxis.None)
				{
					return false;
				}
				float num = rbnvDkJgHQDgSEQTMuMVgPGFodPI(P_0.sourceAxis);
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
				return true;
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Hat)
			{
				if (P_0.unityHat_sourceAxis1 == UnityAxis.None || P_0.unityHat_sourceAxis2 == UnityAxis.None)
				{
					return false;
				}
				UnityAxis unityHat_sourceAxis = P_0.unityHat_sourceAxis1;
				UnityAxis unityHat_sourceAxis2 = P_0.unityHat_sourceAxis2;
				float num2 = rbnvDkJgHQDgSEQTMuMVgPGFodPI(unityHat_sourceAxis);
				float num3 = rbnvDkJgHQDgSEQTMuMVgPGFodPI(unityHat_sourceAxis2);
				float x;
				float y;
				if (P_0.unityHat_checkNeverPressed)
				{
					if (KDrYUxgsfOwoATkXizFDHIampPZn(unityHat_sourceAxis) || KDrYUxgsfOwoATkXizFDHIampPZn(unityHat_sourceAxis2))
					{
						x = P_0.unityHat_zeroValues.x;
						y = P_0.unityHat_zeroValues.y;
					}
					else
					{
						x = P_0.unityHat_neverPressedZeroValues.x;
						y = P_0.unityHat_neverPressedZeroValues.y;
					}
				}
				else
				{
					x = P_0.unityHat_zeroValues.x;
					y = P_0.unityHat_zeroValues.y;
				}
				if (MathTools.Approximately(num2, x) && MathTools.Approximately(num3, y))
				{
					return false;
				}
				if (dFjVZRVanznUdHOSCcErqnXKLRNy(P_0.unityHat_isActiveAxisValues1.x, num2) && dFjVZRVanznUdHOSCcErqnXKLRNy(P_0.unityHat_isActiveAxisValues1.y, num3))
				{
					return true;
				}
				if (dFjVZRVanznUdHOSCcErqnXKLRNy(P_0.unityHat_isActiveAxisValues2.x, num2) && dFjVZRVanznUdHOSCcErqnXKLRNy(P_0.unityHat_isActiveAxisValues2.y, num3))
				{
					return true;
				}
				if (dFjVZRVanznUdHOSCcErqnXKLRNy(P_0.unityHat_isActiveAxisValues3.x, num2) && dFjVZRVanznUdHOSCcErqnXKLRNy(P_0.unityHat_isActiveAxisValues3.y, num3))
				{
					return true;
				}
			}
			else
			{
				if (P_0.sourceType == HardwareElementSourceTypeWithHat.Key)
				{
					if (P_0.sourceKeyCode == KeyCode.None)
					{
						return false;
					}
					return Input.GetKey(P_0.sourceKeyCode);
				}
				if (P_0.sourceType == HardwareElementSourceTypeWithHat.Custom)
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
					HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData[] customCalculationSourceData = P_0.customCalculationSourceData;
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
							if (QfHeMwUPHIZSVjlReAcPaKiCkVkS(customCalculationSourceData[k], out var flag3))
							{
								customCalculation.AddData(flag3 ? 1f : 0f);
							}
							break;
						}
						case HardwareElementSourceTypeWithHat.Axis:
						{
							if (StLlJsceEcwuhfnWLnmXACUzAohr(customCalculationSourceData[k], out var num4))
							{
								customCalculation.AddData((num4 != 0f) ? 1f : 0f);
							}
							break;
						}
						case HardwareElementSourceTypeWithHat.Key:
						{
							if (BGPvLXkDNnSHoBnzcPkxxiEhTpsA(customCalculationSourceData[k], out var flag2))
							{
								customCalculation.AddData(flag2 ? 1f : 0f);
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
			}
			return false;
		}

		private bool dFjVZRVanznUdHOSCcErqnXKLRNy(float P_0, float P_1)
		{
			return MathTools.IsNear(P_1, P_0, 0.1f);
		}

		private float TWcVaCqRhPaZBVsDMKGxqRzNJKSl(HardwareJoystickMap.Platform_Fallback_Base.Axis P_0)
		{
			switch (P_0.sourceType)
			{
			case HardwareElementSourceTypeWithHat.Axis:
				if (P_0.sourceAxis == UnityAxis.None)
				{
					return 0f;
				}
				if (!KDrYUxgsfOwoATkXizFDHIampPZn(P_0.sourceAxis))
				{
					return 0f;
				}
				return rbnvDkJgHQDgSEQTMuMVgPGFodPI(P_0.sourceAxis);
			case HardwareElementSourceTypeWithHat.Button:
				if (P_0.sourceButton == UnityButton.None)
				{
					return 0f;
				}
				if (!mccXHzWkfFBtXSOPjFqsryCBVemt(P_0.sourceButton))
				{
					return 0f;
				}
				if (P_0.buttonAxisContribution == Pole.Positive)
				{
					return 1f;
				}
				return -1f;
			case HardwareElementSourceTypeWithHat.Key:
				if (P_0.sourceKeyCode == KeyCode.None)
				{
					return 0f;
				}
				if (!Input.GetKey(P_0.sourceKeyCode))
				{
					return 0f;
				}
				if (P_0.buttonAxisContribution == Pole.Positive)
				{
					return 1f;
				}
				return -1f;
			case HardwareElementSourceTypeWithHat.Custom:
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
				HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData[] customCalculationSourceData = P_0.customCalculationSourceData;
				if (customCalculationSourceData == null)
				{
					return 0f;
				}
				for (int i = 0; i < customCalculationSourceData.Length; i++)
				{
					if (customCalculationSourceData[i] != null && customCalculationSourceData[i].sourceType == 1 && StLlJsceEcwuhfnWLnmXACUzAohr(customCalculationSourceData[i], out var item))
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
			default:
				return 0f;
			}
		}

		private float rbnvDkJgHQDgSEQTMuMVgPGFodPI(UnityAxis P_0)
		{
			if (P_0 == UnityAxis.None)
			{
				return 0f;
			}
			int num = (int)(P_0 - 1);
			return pyNmIkWjYwvzqSvUxBRfYGWfxvoM[num];
		}

		private bool mccXHzWkfFBtXSOPjFqsryCBVemt(UnityButton P_0)
		{
			int buttonIndex = (int)(P_0 - 1);
			return UnityInputHelper.GetJoystickButtonValueByJoystickId(didzuBeANhngrgHAykWqQoFewNRi, buttonIndex);
		}

		private bool QfHeMwUPHIZSVjlReAcPaKiCkVkS(HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData P_0, out bool P_1)
		{
			P_1 = false;
			if (P_0.sourceType != 0)
			{
				return false;
			}
			UnityButton sourceElement = (UnityButton)P_0.sourceElement;
			if (sourceElement == UnityButton.None)
			{
				return false;
			}
			P_1 = mccXHzWkfFBtXSOPjFqsryCBVemt(sourceElement);
			return true;
		}

		private bool BGPvLXkDNnSHoBnzcPkxxiEhTpsA(HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData P_0, out bool P_1)
		{
			P_1 = false;
			if (P_0.sourceType != 3)
			{
				return false;
			}
			KeyCode sourceElement = (KeyCode)P_0.sourceElement;
			if (sourceElement == KeyCode.None)
			{
				return false;
			}
			P_1 = Input.GetKey(sourceElement);
			return true;
		}

		private bool StLlJsceEcwuhfnWLnmXACUzAohr(HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData P_0, out float P_1)
		{
			P_1 = 0f;
			if (P_0.sourceType != 1)
			{
				return false;
			}
			UnityAxis sourceElement = (UnityAxis)P_0.sourceElement;
			if (sourceElement == UnityAxis.None)
			{
				return false;
			}
			P_1 = rbnvDkJgHQDgSEQTMuMVgPGFodPI(sourceElement);
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
			if (P_0.deadzone > 0f && MathTools.Abs(P_1) <= P_0.deadzone)
			{
				P_1 = 0f;
			}
			if (P_0.invert)
			{
				P_1 *= -1f;
			}
			return true;
		}

		private bool KDrYUxgsfOwoATkXizFDHIampPZn(UnityAxis P_0)
		{
			int num = (int)(P_0 - 1);
			return uABUTCPEHwWfkNdyubJevAWNfKlP[num];
		}

		private void wykWZNAVlAwgarJsuuGDffkOPAFt()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = TOUfFWgxDyvDFcggSfsxgnZloWQnA();
			if (UnityTools.isAndroidPlatform)
			{
				if (Regex.IsMatch(VBoNKBKRhflCmGnkNdKSCUnYrNKY, "Xbox Wireless Controller.*"))
				{
					UnityTools.externalTools.GetDeviceVIDPIDs(out var vids, out var pids);
					for (int i = 0; i < vids.Count; i++)
					{
						if (vids[i] == 1118 && pids[i] == 736)
						{
							bridgedControllerHWInfo.definitionMatchTag = "[FW1]";
							break;
						}
					}
				}
				else if (UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk != null)
				{
					IAndroidFallbackDS4Helper ds4Helper = UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk.ds4Helper;
					if (ds4Helper != null && ds4Helper.IsDS4(VBoNKBKRhflCmGnkNdKSCUnYrNKY))
					{
						if (ds4Helper.IsDS4KeyMapped(rIqSfHxbNaLIcSMKroMldLDNRFdv))
						{
							bridgedControllerHWInfo.definitionMatchTag = "[KEYMAP]";
						}
						else
						{
							bridgedControllerHWInfo.definitionMatchTag = "[NOKEYMAP]";
						}
					}
				}
			}
			mpatVgWGKWbGcjJMCzykYfHofqIZ = ReInput.GetHardwareJoystickMap_InputManager(bridgedControllerHWInfo);
			if (mpatVgWGKWbGcjJMCzykYfHofqIZ == null)
			{
				Rewired.Logger.LogError("Default hardware map not found!");
				return;
			}
			if (UnityTools.isIOSPlatform && mpatVgWGKWbGcjJMCzykYfHofqIZ.hardwareMapIdentifier.guid == Consts.joystickGuid_appleMFiController)
			{
				string text = jSKwgIerbmTfpHnBqJFkskemXFzn(VBoNKBKRhflCmGnkNdKSCUnYrNKY);
				if (!string.IsNullOrEmpty(text))
				{
					mpatVgWGKWbGcjJMCzykYfHofqIZ.controllerName = text;
					if (mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.parentKeys.Count > 0 && !string.IsNullOrEmpty(mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.parentKeys[0]))
					{
						mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.InsertParentKey(0, LocalizationManager.AppendToKeyAsPath(mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.parentKeys[0], text));
					}
					mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.additionalIdentifyingInformation = text;
				}
			}
			else if (mpatVgWGKWbGcjJMCzykYfHofqIZ.useSystemName && !string.IsNullOrEmpty(VBoNKBKRhflCmGnkNdKSCUnYrNKY))
			{
				string text2 = Regex.Replace(VBoNKBKRhflCmGnkNdKSCUnYrNKY, "\\s+", " ");
				text2 = text2.Trim();
				if (!string.IsNullOrEmpty(text2))
				{
					mpatVgWGKWbGcjJMCzykYfHofqIZ.controllerName = text2;
					if (mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.parentKeys.Count > 0 && !string.IsNullOrEmpty(mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.parentKeys[0]))
					{
						mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.InsertParentKey(0, LocalizationManager.AppendToKeyAsPath(mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.parentKeys[0], text2));
					}
					mpatVgWGKWbGcjJMCzykYfHofqIZ.deviceLocalizationInfo.additionalIdentifyingInformation = text2;
				}
			}
			pOrHBqJqyTterpPejSLZjASPWEbn = mpatVgWGKWbGcjJMCzykYfHofqIZ.axisCount;
			kWrqOCMtzVPHhjrBXIHEtLGuBkoHA = mpatVgWGKWbGcjJMCzykYfHofqIZ.buttonCount;
		}

		private void pMSCubdyyKFWYraJrTTjRCevNNBtA()
		{
			Array.Clear(obOyrJIHptJvGVXEVwGyCqmnWcAw, 0, obOyrJIHptJvGVXEVwGyCqmnWcAw.Length);
			Array.Clear(jhrbmZbplROuJQhBjIlWBHxTEWTV, 0, jhrbmZbplROuJQhBjIlWBHxTEWTV.Length);
		}

		private string LahrDIgCEpbBItBMqFMujndKFEsqA()
		{
			if (ReInput.currentPlatform == Platform.Webplayer)
			{
				return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{ReInput.webplayerPlatform.ToString()}{JfTwARyBwCgQIccFyTufHOTnFwZeb().ToString()}{VBoNKBKRhflCmGnkNdKSCUnYrNKY}");
			}
			if (UnityTools.isIOSPlatform)
			{
				string arg = Regex.Replace(VBoNKBKRhflCmGnkNdKSCUnYrNKY, "joystick [0-9]+ by ", "");
				return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{JfTwARyBwCgQIccFyTufHOTnFwZeb().ToString()}{arg}");
			}
			return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{JfTwARyBwCgQIccFyTufHOTnFwZeb().ToString()}{VBoNKBKRhflCmGnkNdKSCUnYrNKY}");
		}

		private InputSource JfTwARyBwCgQIccFyTufHOTnFwZeb()
		{
			if (UnityTools.platform == Platform.Linux && UnityTools.externalTools.LinuxInput_IsJoystickPreconfigured(VBoNKBKRhflCmGnkNdKSCUnYrNKY))
			{
				return InputSource.Fallback_PreConfigured;
			}
			return InputSource.Fallback;
		}

		public static int IIqnAKOAcHaLegIqnafqqKPynUrfA(yNiWVjPwkGeAdQDlCytbRfaFEdpd P_0, yNiWVjPwkGeAdQDlCytbRfaFEdpd P_1)
		{
			if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId < P_1.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId)
			{
				return -1;
			}
			if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId > P_1.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId)
			{
				return 1;
			}
			return 0;
		}

		public static int nzIpyXHwkjUtQHuHNdFCPnCJZxkQ(yNiWVjPwkGeAdQDlCytbRfaFEdpd P_0, yNiWVjPwkGeAdQDlCytbRfaFEdpd P_1)
		{
			if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId < P_1.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId)
			{
				return -1;
			}
			if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId > P_1.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId)
			{
				return 1;
			}
			return 0;
		}

		private static string jSKwgIerbmTfpHnBqJFkskemXFzn(string P_0)
		{
			string input = Regex.Replace(P_0, "\\[.*\\] joystick [0-9]+ by ", "");
			input = Regex.Replace(input, "\\s+", " ");
			if (!string.IsNullOrEmpty(input))
			{
				input = input.Trim();
			}
			return input;
		}
	}

	private class pnMPsWPwditlRiwnpirmreUOfsvA
	{
		public enum CNANhRQQGajsiBwOuZQEoSFPLoOf
		{
			Exact = 0,
			Approximate = 1
		}

		public class KBwsBPFEcWCeoIVctOfQJQaBBfgGb
		{
			public int BbZjRofMgqDfyTFmUkoaBvpLZGTjA;

			public int hSTLqBIYYHFZQuIOgFDKhbxdEjUj;

			public string tYBdDTkqNBqZeiFpbhfRnMtHROFqA;

			public int aRlWEWsNLgDgFfVudftoyeAVaykn;

			public string NKCCyXCJAiefsaMBVYGYlqFCuHrEA;

			public bool xxcXqFeJWCVUrvXUGgwFBfXmTHaR(yNiWVjPwkGeAdQDlCytbRfaFEdpd P_0, CNANhRQQGajsiBwOuZQEoSFPLoOf P_1)
			{
				if (P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == BbZjRofMgqDfyTFmUkoaBvpLZGTjA)
				{
					return true;
				}
				if ((!string.IsNullOrEmpty(NKCCyXCJAiefsaMBVYGYlqFCuHrEA) || !string.IsNullOrEmpty(P_0.FnzIhrvNPciMldVLHwfNectrLChyA)) && !string.Equals(NKCCyXCJAiefsaMBVYGYlqFCuHrEA, P_0.FnzIhrvNPciMldVLHwfNectrLChyA, StringComparison.Ordinal))
				{
					return false;
				}
				switch (P_1)
				{
				case CNANhRQQGajsiBwOuZQEoSFPLoOf.Exact:
					if (hSTLqBIYYHFZQuIOgFDKhbxdEjUj == P_0.rIqSfHxbNaLIcSMKroMldLDNRFdv)
					{
						return tYBdDTkqNBqZeiFpbhfRnMtHROFqA == P_0.VBoNKBKRhflCmGnkNdKSCUnYrNKY;
					}
					return false;
				case CNANhRQQGajsiBwOuZQEoSFPLoOf.Approximate:
					return tYBdDTkqNBqZeiFpbhfRnMtHROFqA == P_0.VBoNKBKRhflCmGnkNdKSCUnYrNKY;
				default:
					throw new NotImplementedException();
				}
			}
		}

		private sealed class FjguHmhLjcHsSDnGPvQAkpxMzmzO : IEnumerable<KBwsBPFEcWCeoIVctOfQJQaBBfgGb>, IEnumerable, IEnumerator<KBwsBPFEcWCeoIVctOfQJQaBBfgGb>, IEnumerator, IDisposable
		{
			private int zdHrFWvhjLzoyFiuugGKOQQhNybR;

			private KBwsBPFEcWCeoIVctOfQJQaBBfgGb IUdBLtguczRLoxnoKPUgvLUmVryj;

			private int jUgwvHBjXGEatLoyRTUVrBbFtUTq;

			public pnMPsWPwditlRiwnpirmreUOfsvA sMEAWKQfPADGgHOwTqdiHEfafPftA;

			private yNiWVjPwkGeAdQDlCytbRfaFEdpd kTiJNqfcjCWdpldMXWXKpHcjOBbd;

			public yNiWVjPwkGeAdQDlCytbRfaFEdpd NSPcJguudsKSgjERWUWvpGefqpqi;

			private CNANhRQQGajsiBwOuZQEoSFPLoOf FJEaejXeXhXFoyLPTZSxziBaRABH;

			public CNANhRQQGajsiBwOuZQEoSFPLoOf SHfxdSxQyzbuXYQbNyIhFejqqgQc;

			private int lelEIvPphJnmqDhvZjYEQXYNoFVD;

			private int jjUdidgFxgDfgQlGcJkApypmpNCNA;

			KBwsBPFEcWCeoIVctOfQJQaBBfgGb IEnumerator<KBwsBPFEcWCeoIVctOfQJQaBBfgGb>.Current
			{
				[DebuggerHidden]
				get
				{
					return IUdBLtguczRLoxnoKPUgvLUmVryj;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return IUdBLtguczRLoxnoKPUgvLUmVryj;
				}
			}

			[DebuggerHidden]
			public FjguHmhLjcHsSDnGPvQAkpxMzmzO(int P_0)
			{
				zdHrFWvhjLzoyFiuugGKOQQhNybR = P_0;
				jUgwvHBjXGEatLoyRTUVrBbFtUTq = Environment.CurrentManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				zdHrFWvhjLzoyFiuugGKOQQhNybR = -2;
			}

			private bool MoveNext()
			{
				int num = zdHrFWvhjLzoyFiuugGKOQQhNybR;
				pnMPsWPwditlRiwnpirmreUOfsvA pnMPsWPwditlRiwnpirmreUOfsvA2 = sMEAWKQfPADGgHOwTqdiHEfafPftA;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					zdHrFWvhjLzoyFiuugGKOQQhNybR = -1;
					goto IL_0083;
				}
				zdHrFWvhjLzoyFiuugGKOQQhNybR = -1;
				lelEIvPphJnmqDhvZjYEQXYNoFVD = pnMPsWPwditlRiwnpirmreUOfsvA2.nwWmKyyKdkjxWCHzRAijfLzCbbzCB.Count;
				jjUdidgFxgDfgQlGcJkApypmpNCNA = 0;
				goto IL_0093;
				IL_0083:
				jjUdidgFxgDfgQlGcJkApypmpNCNA++;
				goto IL_0093;
				IL_0093:
				if (jjUdidgFxgDfgQlGcJkApypmpNCNA < lelEIvPphJnmqDhvZjYEQXYNoFVD)
				{
					if (pnMPsWPwditlRiwnpirmreUOfsvA2.nwWmKyyKdkjxWCHzRAijfLzCbbzCB[jjUdidgFxgDfgQlGcJkApypmpNCNA].xxcXqFeJWCVUrvXUGgwFBfXmTHaR(kTiJNqfcjCWdpldMXWXKpHcjOBbd, FJEaejXeXhXFoyLPTZSxziBaRABH))
					{
						IUdBLtguczRLoxnoKPUgvLUmVryj = pnMPsWPwditlRiwnpirmreUOfsvA2.nwWmKyyKdkjxWCHzRAijfLzCbbzCB[jjUdidgFxgDfgQlGcJkApypmpNCNA];
						zdHrFWvhjLzoyFiuugGKOQQhNybR = 1;
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
			IEnumerator<KBwsBPFEcWCeoIVctOfQJQaBBfgGb> IEnumerable<KBwsBPFEcWCeoIVctOfQJQaBBfgGb>.GetEnumerator()
			{
				FjguHmhLjcHsSDnGPvQAkpxMzmzO fjguHmhLjcHsSDnGPvQAkpxMzmzO;
				if (zdHrFWvhjLzoyFiuugGKOQQhNybR == -2 && jUgwvHBjXGEatLoyRTUVrBbFtUTq == Environment.CurrentManagedThreadId)
				{
					zdHrFWvhjLzoyFiuugGKOQQhNybR = 0;
					fjguHmhLjcHsSDnGPvQAkpxMzmzO = this;
				}
				else
				{
					fjguHmhLjcHsSDnGPvQAkpxMzmzO = new FjguHmhLjcHsSDnGPvQAkpxMzmzO(0);
					fjguHmhLjcHsSDnGPvQAkpxMzmzO.sMEAWKQfPADGgHOwTqdiHEfafPftA = sMEAWKQfPADGgHOwTqdiHEfafPftA;
				}
				fjguHmhLjcHsSDnGPvQAkpxMzmzO.kTiJNqfcjCWdpldMXWXKpHcjOBbd = NSPcJguudsKSgjERWUWvpGefqpqi;
				fjguHmhLjcHsSDnGPvQAkpxMzmzO.FJEaejXeXhXFoyLPTZSxziBaRABH = SHfxdSxQyzbuXYQbNyIhFejqqgQc;
				return fjguHmhLjcHsSDnGPvQAkpxMzmzO;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<KBwsBPFEcWCeoIVctOfQJQaBBfgGb>)this).GetEnumerator();
			}
		}

		private List<KBwsBPFEcWCeoIVctOfQJQaBBfgGb> nwWmKyyKdkjxWCHzRAijfLzCbbzCB;

		public int EHhUHgqRgCAbCNbdMrNgDHaCnXPS => nwWmKyyKdkjxWCHzRAijfLzCbbzCB.Count;

		public pnMPsWPwditlRiwnpirmreUOfsvA()
		{
			nwWmKyyKdkjxWCHzRAijfLzCbbzCB = new List<KBwsBPFEcWCeoIVctOfQJQaBBfgGb>();
		}

		public void ZdqwNbiSBdBHcBCeFuBgglHHIoeS(yNiWVjPwkGeAdQDlCytbRfaFEdpd P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			int count = nwWmKyyKdkjxWCHzRAijfLzCbbzCB.Count;
			for (int i = 0; i < count; i++)
			{
				if (nwWmKyyKdkjxWCHzRAijfLzCbbzCB[i].xxcXqFeJWCVUrvXUGgwFBfXmTHaR(P_0, CNANhRQQGajsiBwOuZQEoSFPLoOf.Exact))
				{
					nwWmKyyKdkjxWCHzRAijfLzCbbzCB[i].BbZjRofMgqDfyTFmUkoaBvpLZGTjA = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
					nwWmKyyKdkjxWCHzRAijfLzCbbzCB[i].tYBdDTkqNBqZeiFpbhfRnMtHROFqA = P_0.VBoNKBKRhflCmGnkNdKSCUnYrNKY;
					nwWmKyyKdkjxWCHzRAijfLzCbbzCB[i].hSTLqBIYYHFZQuIOgFDKhbxdEjUj = P_0.rIqSfHxbNaLIcSMKroMldLDNRFdv;
					nwWmKyyKdkjxWCHzRAijfLzCbbzCB[i].aRlWEWsNLgDgFfVudftoyeAVaykn = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId;
					nwWmKyyKdkjxWCHzRAijfLzCbbzCB[i].NKCCyXCJAiefsaMBVYGYlqFCuHrEA = P_0.FnzIhrvNPciMldVLHwfNectrLChyA;
					DLTyQstgccYdwLhZMutFsfePvePU(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, i);
					return;
				}
			}
			nwWmKyyKdkjxWCHzRAijfLzCbbzCB.Add(new KBwsBPFEcWCeoIVctOfQJQaBBfgGb
			{
				BbZjRofMgqDfyTFmUkoaBvpLZGTjA = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId,
				tYBdDTkqNBqZeiFpbhfRnMtHROFqA = P_0.VBoNKBKRhflCmGnkNdKSCUnYrNKY,
				hSTLqBIYYHFZQuIOgFDKhbxdEjUj = P_0.rIqSfHxbNaLIcSMKroMldLDNRFdv,
				aRlWEWsNLgDgFfVudftoyeAVaykn = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId,
				NKCCyXCJAiefsaMBVYGYlqFCuHrEA = P_0.FnzIhrvNPciMldVLHwfNectrLChyA
			});
			DLTyQstgccYdwLhZMutFsfePvePU(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, nwWmKyyKdkjxWCHzRAijfLzCbbzCB.Count - 1);
		}

		public bool TKfaENHitStmoQYysHsBEvcMgckL(yNiWVjPwkGeAdQDlCytbRfaFEdpd P_0, CNANhRQQGajsiBwOuZQEoSFPLoOf P_1)
		{
			int count = nwWmKyyKdkjxWCHzRAijfLzCbbzCB.Count;
			for (int i = 0; i < count; i++)
			{
				if (nwWmKyyKdkjxWCHzRAijfLzCbbzCB[i].xxcXqFeJWCVUrvXUGgwFBfXmTHaR(P_0, P_1))
				{
					return true;
				}
			}
			return false;
		}

		[IteratorStateMachine(typeof(FjguHmhLjcHsSDnGPvQAkpxMzmzO))]
		public IEnumerable<KBwsBPFEcWCeoIVctOfQJQaBBfgGb> fsKAUJXfyrLPTJdHsZRXhossdMIK(yNiWVjPwkGeAdQDlCytbRfaFEdpd P_0, CNANhRQQGajsiBwOuZQEoSFPLoOf P_1)
		{
			return new FjguHmhLjcHsSDnGPvQAkpxMzmzO(-2)
			{
				sMEAWKQfPADGgHOwTqdiHEfafPftA = this,
				NSPcJguudsKSgjERWUWvpGefqpqi = P_0,
				SHfxdSxQyzbuXYQbNyIhFejqqgQc = P_1
			};
		}

		public int XiEYjyMVjymILhnzbazwgZSgJUCSA(KBwsBPFEcWCeoIVctOfQJQaBBfgGb P_0)
		{
			int count = nwWmKyyKdkjxWCHzRAijfLzCbbzCB.Count;
			for (int i = 0; i < count; i++)
			{
				if (nwWmKyyKdkjxWCHzRAijfLzCbbzCB[i] == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		private void DLTyQstgccYdwLhZMutFsfePvePU(int P_0, int P_1)
		{
			for (int num = nwWmKyyKdkjxWCHzRAijfLzCbbzCB.Count - 1; num >= 0; num--)
			{
				if (num != P_1 && nwWmKyyKdkjxWCHzRAijfLzCbbzCB[num].BbZjRofMgqDfyTFmUkoaBvpLZGTjA == P_0)
				{
					nwWmKyyKdkjxWCHzRAijfLzCbbzCB.RemoveAt(num);
				}
			}
		}
	}

	private List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> ewTGRbivisjmWdiuJQzPRpRjmQywb;

	private int SbvEasByedJOZGPHYsdEOtxXDoCdA;

	private pnMPsWPwditlRiwnpirmreUOfsvA JyLWftlCRaOliPVHKRdLyFzvNUyI;

	private bool cUCzZDqJtsqRRrjsTtHZbYzGIJsQ;

	private bool vSabOyToibJEOvxdxLcngSnYhzFk;

	private UpdateLoopType ICZaytBbnInshiOfoBaKieDKPBCG;

	private UpdateLoopType QUfyTvrdOozFcSNQVkRbETMZHCVO;

	private TimerAbs YCBCABZsJIDXfVLrEsCBkMjYbMeAA;

	private Action<int, ControllerDataUpdater> EClxnhNTXSDeiwINrwvkdHEsomIm;

	private PlatformInputManager HRYHMkJEVEFEghnIjnatDFlWQaRlA;

	private readonly IUnifiedKeyboardSource ZkUyoyMDiPpZNCBmChdWICndeKNR;

	private readonly IUnifiedMouseSource tGXStJPFteMkPTVsKGQRGVrHHfkB;

	private bool SaYrHWHxzVCmfRpQIbTJaDgwkzXFA;

	private string[] BrbRAsRHuVVaWGEHldGhMFsJGQffA;

	[CustomObfuscation(rename = false)]
	int PlatformInputManager.deviceCount => SbvEasByedJOZGPHYsdEOtxXDoCdA;

	[CustomObfuscation(rename = false)]
	PlatformInputManager PlatformInputManager.primaryInputManager => HRYHMkJEVEFEghnIjnatDFlWQaRlA;

	[CustomObfuscation(rename = false)]
	IInputSource PlatformInputManager.inputSource => null;

	[CustomObfuscation(rename = false)]
	InputSource PlatformInputManager.inputSourceType => InputSource.Fallback;

	public pWRDtMxMZRbTxFlbqpruZytnvSMMA(UpdateLoopSetting P_0)
	{
		HRYHMkJEVEFEghnIjnatDFlWQaRlA = this;
		ZkUyoyMDiPpZNCBmChdWICndeKNR = new UnityUnifiedKeyboardSource();
		tGXStJPFteMkPTVsKGQRGVrHHfkB = new UnityUnifiedMouseSource();
		using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
		{
			List<UpdateLoopType> list = tList.list;
			EnumConverter.ToUpdateLoopTypes(P_0, list);
			int num = 0;
			if (num < list.Count)
			{
				QUfyTvrdOozFcSNQVkRbETMZHCVO = list[num];
			}
		}
		BrbRAsRHuVVaWGEHldGhMFsJGQffA = new string[0];
		EClxnhNTXSDeiwINrwvkdHEsomIm = UpdateControllerData;
	}

	[CustomObfuscation(rename = false)]
	public override void Initialize()
	{
		if (UnityTools.isAndroidPlatform && UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk != null)
		{
			UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk.DeviceChangedEvent += iYhdzApIbiKcYhPfSAivrmUmWQlQ;
		}
		YCBCABZsJIDXfVLrEsCBkMjYbMeAA = new TimerAbs(1.0);
		JyLWftlCRaOliPVHKRdLyFzvNUyI = new pnMPsWPwditlRiwnpirmreUOfsvA();
		tEahvCetpBkOyIzQUpMzHVkWtFxuA();
		cUCzZDqJtsqRRrjsTtHZbYzGIJsQ = true;
		YCBCABZsJIDXfVLrEsCBkMjYbMeAA.Start();
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType updateLoop)
	{
		ICZaytBbnInshiOfoBaKieDKPBCG = updateLoop;
		wvflWaHDKQYbOPNHWqQBVCMcIxNs();
		if (cUCzZDqJtsqRRrjsTtHZbYzGIJsQ)
		{
			rsZQBCMmUENjMFAoXTRotDUDJCWy();
		}
		JchuCPvOKDhCEHGYwzPLvdatTreN(updateLoop);
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (UnityTools.isAndroidPlatform && UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk != null)
		{
			UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk.DeviceChangedEvent -= iYhdzApIbiKcYhPfSAivrmUmWQlQ;
		}
		(ZkUyoyMDiPpZNCBmChdWICndeKNR as IDisposable).Dispose();
		(tGXStJPFteMkPTVsKGQRGVrHHfkB as IDisposable).Dispose();
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return EClxnhNTXSDeiwINrwvkdHEsomIm;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int assignedControllerId, ControllerDataUpdater data)
	{
		for (int i = 0; i < SbvEasByedJOZGPHYsdEOtxXDoCdA; i++)
		{
			if (ewTGRbivisjmWdiuJQzPRpRjmQywb[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId == assignedControllerId)
			{
				ewTGRbivisjmWdiuJQzPRpRjmQywb[i].FillData(data);
				return;
			}
		}
		Rewired.Logger.LogError("Invalid joystick Id " + assignedControllerId + "!");
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		cUCzZDqJtsqRRrjsTtHZbYzGIJsQ = true;
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		cUCzZDqJtsqRRrjsTtHZbYzGIJsQ = true;
		if (_SystemDeviceDisconnectedEvent != null)
		{
			_SystemDeviceDisconnectedEvent();
		}
	}

	private void iYhdzApIbiKcYhPfSAivrmUmWQlQ()
	{
		cUCzZDqJtsqRRrjsTtHZbYzGIJsQ = true;
		vSabOyToibJEOvxdxLcngSnYhzFk = true;
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
	{
		for (int i = 0; i < ewTGRbivisjmWdiuJQzPRpRjmQywb.Count; i++)
		{
			if (ewTGRbivisjmWdiuJQzPRpRjmQywb[i].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId == unityJoystickId)
			{
				ewTGRbivisjmWdiuJQzPRpRjmQywb[i].HsvxAgMjLLpteYkgzexFGDwatgvoA();
			}
		}
		for (int j = 0; j < ewTGRbivisjmWdiuJQzPRpRjmQywb.Count; j++)
		{
			if (ewTGRbivisjmWdiuJQzPRpRjmQywb[j].Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == joystickId)
			{
				ewTGRbivisjmWdiuJQzPRpRjmQywb[j].PKkdUzpQMjltmWHbyfaCjckGfWjwA(unityJoystickId);
				break;
			}
		}
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedMouseSource GetUnifiedMouseSource()
	{
		return tGXStJPFteMkPTVsKGQRGVrHHfkB;
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedKeyboardSource GetUnifiedKeyboardSource()
	{
		return ZkUyoyMDiPpZNCBmChdWICndeKNR;
	}

	private void tEahvCetpBkOyIzQUpMzHVkWtFxuA()
	{
		gwDhqXluKzIkDGmSUXzARYFzRyddA(Input.GetJoystickNames());
	}

	private void gwDhqXluKzIkDGmSUXzARYFzRyddA(string[] P_0)
	{
		int num = 0;
		List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> list = ewTGRbivisjmWdiuJQzPRpRjmQywb;
		int sbvEasByedJOZGPHYsdEOtxXDoCdA = SbvEasByedJOZGPHYsdEOtxXDoCdA;
		ewTGRbivisjmWdiuJQzPRpRjmQywb = new List<yNiWVjPwkGeAdQDlCytbRfaFEdpd>();
		for (int i = 0; i < P_0.Length; i++)
		{
			string text = StringTools.SanitizeDeviceString(P_0[i]);
			if (UnityTools.IsValidUnityJoystickName(text))
			{
				yNiWVjPwkGeAdQDlCytbRfaFEdpd yNiWVjPwkGeAdQDlCytbRfaFEdpd2 = new yNiWVjPwkGeAdQDlCytbRfaFEdpd();
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.VBoNKBKRhflCmGnkNdKSCUnYrNKY = text;
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.VBpfDcKvqfoAbLLVZevEXwbMOuTMA = text;
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.rIqSfHxbNaLIcSMKroMldLDNRFdv = i;
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId = i + 1;
				if (UnityTools.isAndroidPlatform && UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk != null)
				{
					yNiWVjPwkGeAdQDlCytbRfaFEdpd2.FnzIhrvNPciMldVLHwfNectrLChyA = UnityTools.xtkrcfYiRlMAbbhtyriicjfJbkdk.GetUniqueDeviceIdentifier(text, i);
				}
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.CMpzAkyEOhiYDOadstACskPwdERkA();
				ewTGRbivisjmWdiuJQzPRpRjmQywb.Add(yNiWVjPwkGeAdQDlCytbRfaFEdpd2);
				num++;
			}
		}
		SbvEasByedJOZGPHYsdEOtxXDoCdA = num;
		EqxtuMteZxgFonbjjrxPcqfDEsSA(sbvEasByedJOZGPHYsdEOtxXDoCdA, num, list, ewTGRbivisjmWdiuJQzPRpRjmQywb);
		for (int j = 0; j < num; j++)
		{
			if (_UpdateControllerInfoEvent != null)
			{
				_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(ewTGRbivisjmWdiuJQzPRpRjmQywb[j]));
			}
		}
		TwlfbuCmvkjxvQptRWxGNdJiiCWSA(list, ewTGRbivisjmWdiuJQzPRpRjmQywb, false);
		TwlfbuCmvkjxvQptRWxGNdJiiCWSA(ewTGRbivisjmWdiuJQzPRpRjmQywb, list, true);
		BrbRAsRHuVVaWGEHldGhMFsJGQffA = P_0;
	}

	private void JchuCPvOKDhCEHGYwzPLvdatTreN(UpdateLoopType P_0)
	{
		int count = ewTGRbivisjmWdiuJQzPRpRjmQywb.Count;
		for (int i = 0; i < count; i++)
		{
			if (ewTGRbivisjmWdiuJQzPRpRjmQywb[i] != null)
			{
				ewTGRbivisjmWdiuJQzPRpRjmQywb[i].Update();
			}
		}
	}

	private void EqxtuMteZxgFonbjjrxPcqfDEsSA(int P_0, int P_1, List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_2, List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_3)
	{
		if (P_1 > 0)
		{
			P_3.Sort(yNiWVjPwkGeAdQDlCytbRfaFEdpd.nzIpyXHwkjUtQHuHNdFCPnCJZxkQ);
		}
		if (P_0 > 0 && P_1 > 0)
		{
			ZqTTwvDaElBivPdcNHVyfQllznHcA(P_1, P_3, P_0, P_2, pnMPsWPwditlRiwnpirmreUOfsvA.CNANhRQQGajsiBwOuZQEoSFPLoOf.Exact);
			ZqTTwvDaElBivPdcNHVyfQllznHcA(P_1, P_3, P_0, P_2, pnMPsWPwditlRiwnpirmreUOfsvA.CNANhRQQGajsiBwOuZQEoSFPLoOf.Approximate);
		}
		jmxlpsfiIJdcSPxpUOjQhPrmVLNC(P_1, P_3, pnMPsWPwditlRiwnpirmreUOfsvA.CNANhRQQGajsiBwOuZQEoSFPLoOf.Exact);
		jmxlpsfiIJdcSPxpUOjQhPrmVLNC(P_1, P_3, pnMPsWPwditlRiwnpirmreUOfsvA.CNANhRQQGajsiBwOuZQEoSFPLoOf.Approximate);
		for (int i = 0; i < P_1; i++)
		{
			yNiWVjPwkGeAdQDlCytbRfaFEdpd yNiWVjPwkGeAdQDlCytbRfaFEdpd2 = P_3[i];
			if (yNiWVjPwkGeAdQDlCytbRfaFEdpd2 != null && yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId < 0)
			{
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = nqRQerKRYjMPiLCxtCvhgDDYaJPAb(P_3);
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = ReInput.GetNewJoystickId();
				JyLWftlCRaOliPVHKRdLyFzvNUyI.ZdqwNbiSBdBHcBCeFuBgglHHIoeS(yNiWVjPwkGeAdQDlCytbRfaFEdpd2);
			}
		}
		P_3.Sort(yNiWVjPwkGeAdQDlCytbRfaFEdpd.IIqnAKOAcHaLegIqnafqqKPynUrfA);
	}

	private void dZtXCjKnPIxQtcubRuAIKTkZKKZ(List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_0, int P_1, int P_2)
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

	private bool bSyQAlhNfWeAaAYtdRJoGxJYaSZtA(List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_0, int P_1)
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

	private int nqRQerKRYjMPiLCxtCvhgDDYaJPAb(List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_0)
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

	private bool WIAIqGzJXuBtbHVRRbVRwBFSjQNT(List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_0, int P_1)
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

	private void ZqTTwvDaElBivPdcNHVyfQllznHcA(int P_0, List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_1, int P_2, List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_3, pnMPsWPwditlRiwnpirmreUOfsvA.CNANhRQQGajsiBwOuZQEoSFPLoOf P_4)
	{
		int num = ((P_4 != pnMPsWPwditlRiwnpirmreUOfsvA.CNANhRQQGajsiBwOuZQEoSFPLoOf.Exact) ? 1 : 2);
		for (int i = 0; i < P_0; i++)
		{
			yNiWVjPwkGeAdQDlCytbRfaFEdpd yNiWVjPwkGeAdQDlCytbRfaFEdpd2 = P_1[i];
			if (yNiWVjPwkGeAdQDlCytbRfaFEdpd2 == null || yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			for (int j = 0; j < P_2; j++)
			{
				yNiWVjPwkGeAdQDlCytbRfaFEdpd yNiWVjPwkGeAdQDlCytbRfaFEdpd3 = P_3[j];
				if (yNiWVjPwkGeAdQDlCytbRfaFEdpd3 != null && !WIAIqGzJXuBtbHVRRbVRwBFSjQNT(P_1, yNiWVjPwkGeAdQDlCytbRfaFEdpd3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId) && yNiWVjPwkGeAdQDlCytbRfaFEdpd2.ZWxiAIDOFPDoWuQvgckncugAqKjjb(yNiWVjPwkGeAdQDlCytbRfaFEdpd3) >= num)
				{
					yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = yNiWVjPwkGeAdQDlCytbRfaFEdpd3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId;
					yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = yNiWVjPwkGeAdQDlCytbRfaFEdpd3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
					if (ReInput.isWindowsStandaloneWebplayerOrEditorPlatform && !UnityTools.windowsJoystickNamesReturnsEmptyStringsIfJoystickNull)
					{
						yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId = yNiWVjPwkGeAdQDlCytbRfaFEdpd3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EunityId;
					}
					JyLWftlCRaOliPVHKRdLyFzvNUyI.ZdqwNbiSBdBHcBCeFuBgglHHIoeS(yNiWVjPwkGeAdQDlCytbRfaFEdpd2);
				}
			}
		}
	}

	private void jmxlpsfiIJdcSPxpUOjQhPrmVLNC(int P_0, List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_1, pnMPsWPwditlRiwnpirmreUOfsvA.CNANhRQQGajsiBwOuZQEoSFPLoOf P_2)
	{
		for (int i = 0; i < P_0; i++)
		{
			yNiWVjPwkGeAdQDlCytbRfaFEdpd yNiWVjPwkGeAdQDlCytbRfaFEdpd2 = P_1[i];
			if (yNiWVjPwkGeAdQDlCytbRfaFEdpd2 == null || yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId >= 0)
			{
				continue;
			}
			pnMPsWPwditlRiwnpirmreUOfsvA.KBwsBPFEcWCeoIVctOfQJQaBBfgGb kBwsBPFEcWCeoIVctOfQJQaBBfgGb = null;
			foreach (pnMPsWPwditlRiwnpirmreUOfsvA.KBwsBPFEcWCeoIVctOfQJQaBBfgGb item in JyLWftlCRaOliPVHKRdLyFzvNUyI.fsKAUJXfyrLPTJdHsZRXhossdMIK(yNiWVjPwkGeAdQDlCytbRfaFEdpd2, P_2))
			{
				if (!WIAIqGzJXuBtbHVRRbVRwBFSjQNT(P_1, item.BbZjRofMgqDfyTFmUkoaBvpLZGTjA) && item.aRlWEWsNLgDgFfVudftoyeAVaykn >= 0)
				{
					kBwsBPFEcWCeoIVctOfQJQaBBfgGb = item;
					break;
				}
			}
			if (kBwsBPFEcWCeoIVctOfQJQaBBfgGb != null)
			{
				int num = kBwsBPFEcWCeoIVctOfQJQaBBfgGb.aRlWEWsNLgDgFfVudftoyeAVaykn;
				if (!bSyQAlhNfWeAaAYtdRJoGxJYaSZtA(P_1, num))
				{
					num = (kBwsBPFEcWCeoIVctOfQJQaBBfgGb.aRlWEWsNLgDgFfVudftoyeAVaykn = nqRQerKRYjMPiLCxtCvhgDDYaJPAb(P_1));
				}
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinputManagerId = num;
				yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = kBwsBPFEcWCeoIVctOfQJQaBBfgGb.BbZjRofMgqDfyTFmUkoaBvpLZGTjA;
				JyLWftlCRaOliPVHKRdLyFzvNUyI.ZdqwNbiSBdBHcBCeFuBgglHHIoeS(yNiWVjPwkGeAdQDlCytbRfaFEdpd2);
			}
		}
	}

	private void rsZQBCMmUENjMFAoXTRotDUDJCWy()
	{
		string[] joystickNames = Input.GetJoystickNames();
		if (vSabOyToibJEOvxdxLcngSnYhzFk || LQJPtEbDKBtWwACcPiIgkuiIiLYC(joystickNames))
		{
			gwDhqXluKzIkDGmSUXzARYFzRyddA(joystickNames);
		}
		cUCzZDqJtsqRRrjsTtHZbYzGIJsQ = false;
		if (vSabOyToibJEOvxdxLcngSnYhzFk)
		{
			vSabOyToibJEOvxdxLcngSnYhzFk = false;
		}
	}

	private bool LQJPtEbDKBtWwACcPiIgkuiIiLYC(string[] P_0)
	{
		if (P_0.Length != BrbRAsRHuVVaWGEHldGhMFsJGQffA.Length)
		{
			return true;
		}
		for (int i = 0; i < P_0.Length; i++)
		{
			if (!string.Equals(P_0[i], BrbRAsRHuVVaWGEHldGhMFsJGQffA[i], StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	private void TwlfbuCmvkjxvQptRWxGNdJiiCWSA(List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_0, List<yNiWVjPwkGeAdQDlCytbRfaFEdpd> P_1, bool P_2)
	{
		if (P_0 == null)
		{
			return;
		}
		int num = P_0?.Count ?? 0;
		int num2 = P_1?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			yNiWVjPwkGeAdQDlCytbRfaFEdpd yNiWVjPwkGeAdQDlCytbRfaFEdpd2 = P_0[i];
			if (yNiWVjPwkGeAdQDlCytbRfaFEdpd2 == null)
			{
				continue;
			}
			bool flag = false;
			if (P_1 != null)
			{
				for (int j = 0; j < num2; j++)
				{
					yNiWVjPwkGeAdQDlCytbRfaFEdpd yNiWVjPwkGeAdQDlCytbRfaFEdpd3 = P_1[j];
					if (yNiWVjPwkGeAdQDlCytbRfaFEdpd3 != null && yNiWVjPwkGeAdQDlCytbRfaFEdpd2.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId == yNiWVjPwkGeAdQDlCytbRfaFEdpd3.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				spfBTurCiHYZiIVyUtwlerKryAwE(P_0[i], P_2);
			}
		}
	}

	private void spfBTurCiHYZiIVyUtwlerKryAwE(yNiWVjPwkGeAdQDlCytbRfaFEdpd P_0, bool P_1)
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

	private void wvflWaHDKQYbOPNHWqQBVCMcIxNs()
	{
		if (ICZaytBbnInshiOfoBaKieDKPBCG == QUfyTvrdOozFcSNQVkRbETMZHCVO && YCBCABZsJIDXfVLrEsCBkMjYbMeAA.Update())
		{
			cUCzZDqJtsqRRrjsTtHZbYzGIJsQ = true;
			YCBCABZsJIDXfVLrEsCBkMjYbMeAA.Start();
		}
	}
}
