using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using Rewired;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

internal class JnpGFOTHilhwgNkQYIjkXacPlURx : PlatformInputManager
{
	private class zoqILdNBnNfxqCLemZToKclpOtet : IInputManagerJoystick, IInputManagerJoystickPublic
	{
		private int kniQQKPsrTyUTKqICYwwQKlUfoRf;

		private int nyPspetimSyUTqhIxJtYJkZCgjlM;

		private int xmEXVfyENtoyjKLfjizpdurnqqpi;

		public Guid cUhzBTzwBmFECgMqAsRWDOSsbdVY;

		public string PUgQxpYkUBIdUwRqBetvbNDaNRxF;

		public int mQPHeeJqpDVrGOXqKuQGLKqwdHwX;

		public string ypTGidgvvmNpwIGhvhmZEYWSvSMQ;

		public string DSkaXLxvBbkmkMXJKLKIeAKfAIeaA;

		private int yLLlShtixVkbYMaIfuUMijmLCboKA = 29;

		private int tpoonPbJmajQyAErVLnAOoXhqZqEb = 20;

		private float[] qugobParkJGeDYgxggFADuHJrSREA;

		private bool[] QKMemPJOCQdfGcajCUROfzfnXvDE;

		private bool[] oJhyqFKvnoQmuPVarvXocHVOuUmo;

		private float[] vZnqEotgQUkfWOprDNdZxrqurwoU;

		private bool[] UlMvPWNzOhnWpINgkvdfoOPBUjdm;

		private HardwareJoystickMap_InputManager yPbTGFEOQNqKOEHVnHaIUhnHjgYD;

		private bool ulpWNMyBfTTmuiYwnRErPHSaPABK;

		[CustomObfuscation(rename = false)]
		public int rewiredId
		{
			get
			{
				return kniQQKPsrTyUTKqICYwwQKlUfoRf;
			}
			set
			{
				kniQQKPsrTyUTKqICYwwQKlUfoRf = value;
			}
		}

		[CustomObfuscation(rename = false)]
		public int inputManagerId
		{
			get
			{
				return nyPspetimSyUTqhIxJtYJkZCgjlM;
			}
			set
			{
				nyPspetimSyUTqhIxJtYJkZCgjlM = value;
			}
		}

		[CustomObfuscation(rename = false)]
		public string name
		{
			get
			{
				if (!(PUgQxpYkUBIdUwRqBetvbNDaNRxF != "Unknown Controller"))
				{
					return ypTGidgvvmNpwIGhvhmZEYWSvSMQ;
				}
				return PUgQxpYkUBIdUwRqBetvbNDaNRxF;
			}
		}

		[CustomObfuscation(rename = false)]
		public long? systemId
		{
			get
			{
				if (xmEXVfyENtoyjKLfjizpdurnqqpi < 1)
				{
					return null;
				}
				return xmEXVfyENtoyjKLfjizpdurnqqpi;
			}
		}

		[CustomObfuscation(rename = false)]
		public int unityId
		{
			get
			{
				return xmEXVfyENtoyjKLfjizpdurnqqpi;
			}
			set
			{
				xmEXVfyENtoyjKLfjizpdurnqqpi = value;
			}
		}

		[CustomObfuscation(rename = false)]
		public Guid instanceGuid
		{
			get
			{
				if ((ReInput.isWindowsStandaloneWebplayerOrEditorPlatform && !UnityTools.windowsJoystickNamesReturnsEmptyStringsIfJoystickNull) || UnityTools.effectivePlatform == Platform.OSX)
				{
					return MiscTools.CreateGuidHashSHA1(name);
				}
				if (UnityTools.isIOSPlatform)
				{
					return MiscTools.CreateGuidHashSHA1(ypTGidgvvmNpwIGhvhmZEYWSvSMQ);
				}
				return MiscTools.CreateGuidHashSHA1(name + "_" + xmEXVfyENtoyjKLfjizpdurnqqpi);
			}
		}

		[CustomObfuscation(rename = false)]
		public Guid persistentGuid => instanceGuid;

		[CustomObfuscation(rename = false)]
		public Controller.Extension extension => null;

		[CustomObfuscation(rename = false)]
		public void SetVibration(float amount, int motorIndex)
		{
		}

		[CustomObfuscation(rename = false)]
		public void StopVibration()
		{
		}

		public zoqILdNBnNfxqCLemZToKclpOtet()
		{
			nyPspetimSyUTqhIxJtYJkZCgjlM = -1;
			kniQQKPsrTyUTKqICYwwQKlUfoRf = -1;
			xmEXVfyENtoyjKLfjizpdurnqqpi = 0;
		}

		public void XrAbLZWFEwcAyuirOieMPGMErUhj()
		{
			uxvVoENawKQUBgtDORfvcLmpPHIB();
			cUhzBTzwBmFECgMqAsRWDOSsbdVY = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.hardwareMapIdentifier.guid;
			PUgQxpYkUBIdUwRqBetvbNDaNRxF = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.controllerName;
			qugobParkJGeDYgxggFADuHJrSREA = new float[yLLlShtixVkbYMaIfuUMijmLCboKA];
			QKMemPJOCQdfGcajCUROfzfnXvDE = new bool[tpoonPbJmajQyAErVLnAOoXhqZqEb];
			oJhyqFKvnoQmuPVarvXocHVOuUmo = new bool[yLLlShtixVkbYMaIfuUMijmLCboKA];
			UlMvPWNzOhnWpINgkvdfoOPBUjdm = new bool[29];
			vZnqEotgQUkfWOprDNdZxrqurwoU = new float[29];
			Update();
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			if (xmEXVfyENtoyjKLfjizpdurnqqpi > 0)
			{
				NbTUPsrnRpwUelNZAIDXOUbwfvaD();
				dtkrhYOvGizZeJBgYiXXFyVpYjce();
				MGEvRBxTXmrzVAbFgmVqKAAmKjlj();
			}
		}

		public int dRTWaRkWIkEMLraxMRHrbdGkCqGCA(zoqILdNBnNfxqCLemZToKclpOtet P_0)
		{
			if ((!string.IsNullOrEmpty(DSkaXLxvBbkmkMXJKLKIeAKfAIeaA) || !string.IsNullOrEmpty(P_0.DSkaXLxvBbkmkMXJKLKIeAKfAIeaA)) && !string.Equals(DSkaXLxvBbkmkMXJKLKIeAKfAIeaA, P_0.DSkaXLxvBbkmkMXJKLKIeAKfAIeaA, StringComparison.Ordinal))
			{
				return 0;
			}
			if (P_0.ypTGidgvvmNpwIGhvhmZEYWSvSMQ == ypTGidgvvmNpwIGhvhmZEYWSvSMQ && P_0.mQPHeeJqpDVrGOXqKuQGLKqwdHwX == mQPHeeJqpDVrGOXqKuQGLKqwdHwX)
			{
				return 2;
			}
			if (P_0.ypTGidgvvmNpwIGhvhmZEYWSvSMQ == ypTGidgvvmNpwIGhvhmZEYWSvSMQ)
			{
				return 1;
			}
			return 0;
		}

		private void ZsSGuyFeNhejoyHSIRMfnABaLlydA(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.Fallback;
			P_0.inputSource = ahmARFfBBEyFIxKNzxlGbdDXdOeBb();
			P_0.hardwareIdentifier = gGionhosfhAUReDWKmiZOyIKcoAI();
			P_0.hardwareAxisCount = 0;
			P_0.hardwareButtonCount = 0;
			P_0.hardwareHatCount = 0;
			P_0.hw_productName = ypTGidgvvmNpwIGhvhmZEYWSvSMQ;
		}

		private void ZsSGuyFeNhejoyHSIRMfnABaLlydA(BridgedController P_0)
		{
			ZsSGuyFeNhejoyHSIRMfnABaLlydA((BridgedControllerHWInfo)P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.ToGameHardwareControllerMap();
			P_0.instanceName = ypTGidgvvmNpwIGhvhmZEYWSvSMQ;
			P_0.productName = ypTGidgvvmNpwIGhvhmZEYWSvSMQ;
			P_0.isXInputDevice = false;
			P_0.axisCount = yLLlShtixVkbYMaIfuUMijmLCboKA;
			P_0.buttonCount = tpoonPbJmajQyAErVLnAOoXhqZqEb;
			P_0.controllerTypeGuid = cUhzBTzwBmFECgMqAsRWDOSsbdVY;
		}

		[CustomObfuscation(rename = false)]
		public void FillData(ControllerDataUpdater dataUpdater)
		{
			if (yLLlShtixVkbYMaIfuUMijmLCboKA != dataUpdater.axisCount || tpoonPbJmajQyAErVLnAOoXhqZqEb != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			float[] axisValues = dataUpdater.axisValues;
			bool[] axisHasBeenPressedOSXLinux = dataUpdater.axisHasBeenPressedOSXLinux;
			for (int i = 0; i < yLLlShtixVkbYMaIfuUMijmLCboKA; i++)
			{
				if (axisValues[i] != qugobParkJGeDYgxggFADuHJrSREA[i])
				{
					axisValues[i] = qugobParkJGeDYgxggFADuHJrSREA[i];
					if (axisHasBeenPressedOSXLinux[i] != oJhyqFKvnoQmuPVarvXocHVOuUmo[i])
					{
						axisHasBeenPressedOSXLinux[i] = oJhyqFKvnoQmuPVarvXocHVOuUmo[i];
					}
				}
			}
			bool[] buttonValues = dataUpdater.buttonValues;
			for (int j = 0; j < tpoonPbJmajQyAErVLnAOoXhqZqEb; j++)
			{
				if (buttonValues[j] != QKMemPJOCQdfGcajCUROfzfnXvDE[j])
				{
					buttonValues[j] = QKMemPJOCQdfGcajCUROfzfnXvDE[j];
				}
			}
			if (ulpWNMyBfTTmuiYwnRErPHSaPABK && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		public void MlPMLNgjWLmLZYnRFBEUGwEemXQI(int P_0)
		{
			if (P_0 >= 1 && P_0 <= 16)
			{
				unityId = P_0;
			}
		}

		public void fyIyAIgGFTkjOYexstkBHeMrXKfL()
		{
			xmEXVfyENtoyjKLfjizpdurnqqpi = 0;
			GicadQhCMRmSzBzyoPdNLHmFnguCA();
		}

		public BridgedControllerHWInfo wlgJJzqpwWlttiTkqCeBJnfMAizL()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
			ZsSGuyFeNhejoyHSIRMfnABaLlydA(bridgedControllerHWInfo);
			return bridgedControllerHWInfo;
		}

		[CustomObfuscation(rename = false)]
		public BridgedController ToBridgedController()
		{
			BridgedController bridgedController = new BridgedController();
			ZsSGuyFeNhejoyHSIRMfnABaLlydA(bridgedController);
			return bridgedController;
		}

		[CustomObfuscation(rename = false)]
		public ControllerDisconnectedEventArgs ToControllerDisconnectedEventArgs()
		{
			return new ControllerDisconnectedEventArgs(kniQQKPsrTyUTKqICYwwQKlUfoRf);
		}

		private void NbTUPsrnRpwUelNZAIDXOUbwfvaD()
		{
			for (int i = 0; i < 29; i++)
			{
				float joystickAxisValueByJoystickId = UnityInputHelper.GetJoystickAxisValueByJoystickId(xmEXVfyENtoyjKLfjizpdurnqqpi, i);
				if (vZnqEotgQUkfWOprDNdZxrqurwoU[i] != joystickAxisValueByJoystickId)
				{
					vZnqEotgQUkfWOprDNdZxrqurwoU[i] = joystickAxisValueByJoystickId;
					if (!UlMvPWNzOhnWpINgkvdfoOPBUjdm[i] && joystickAxisValueByJoystickId != 0f)
					{
						UlMvPWNzOhnWpINgkvdfoOPBUjdm[i] = true;
					}
				}
			}
		}

		private void dtkrhYOvGizZeJBgYiXXFyVpYjce()
		{
			HardwareJoystickMap.Platform_Fallback_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_Fallback_Base)yPbTGFEOQNqKOEHVnHaIUhnHjgYD.map).Axes_orig;
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
				if (i >= yLLlShtixVkbYMaIfuUMijmLCboKA)
				{
					throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
				}
				float num = vKvknQyltTIOpMBpDfAHAfgnfcpAA(axes_orig[i]);
				if (qugobParkJGeDYgxggFADuHJrSREA[i] == num)
				{
					continue;
				}
				qugobParkJGeDYgxggFADuHJrSREA[i] = num;
				if (!oJhyqFKvnoQmuPVarvXocHVOuUmo[i])
				{
					if (axes_orig[i].sourceType == HardwareElementSourceTypeWithHat.Axis)
					{
						float num2 = vKvknQyltTIOpMBpDfAHAfgnfcpAA(axes_orig[i].sourceAxis);
						oJhyqFKvnoQmuPVarvXocHVOuUmo[i] = num2 != 0f;
					}
					else
					{
						oJhyqFKvnoQmuPVarvXocHVOuUmo[i] = true;
					}
				}
				if (!ulpWNMyBfTTmuiYwnRErPHSaPABK && qugobParkJGeDYgxggFADuHJrSREA[i] != 0f)
				{
					ulpWNMyBfTTmuiYwnRErPHSaPABK = true;
				}
			}
		}

		private void MGEvRBxTXmrzVAbFgmVqKAAmKjlj()
		{
			HardwareJoystickMap.Platform_Fallback_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_Fallback_Base)yPbTGFEOQNqKOEHVnHaIUhnHjgYD.map).Buttons_orig;
			if (buttons_orig == null)
			{
				return;
			}
			for (int i = 0; i < buttons_orig.Length; i++)
			{
				if (i >= tpoonPbJmajQyAErVLnAOoXhqZqEb)
				{
					throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
				}
				bool flag = HJcRYlOYnieEhWqubAkAbPeBDumaA(buttons_orig[i]);
				if (QKMemPJOCQdfGcajCUROfzfnXvDE[i] != flag)
				{
					QKMemPJOCQdfGcajCUROfzfnXvDE[i] = flag;
					if (!ulpWNMyBfTTmuiYwnRErPHSaPABK && QKMemPJOCQdfGcajCUROfzfnXvDE[i])
					{
						ulpWNMyBfTTmuiYwnRErPHSaPABK = true;
					}
				}
			}
		}

		private bool HJcRYlOYnieEhWqubAkAbPeBDumaA(HardwareJoystickMap.Platform_Fallback_Base.Button P_0)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				if (P_0.ignoreIfButtonsActive)
				{
					for (int i = 0; i < P_0.ignoreIfButtonsActiveButtons.Length; i++)
					{
						if (HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0.ignoreIfButtonsActiveButtons[i]))
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
						if (!HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0.requiredButtons[j]))
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
				return HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0.sourceButton);
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				if (P_0.sourceAxis == UnityAxis.None)
				{
					return false;
				}
				float num = vKvknQyltTIOpMBpDfAHAfgnfcpAA(P_0.sourceAxis);
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
				float num2 = vKvknQyltTIOpMBpDfAHAfgnfcpAA(unityHat_sourceAxis);
				float num3 = vKvknQyltTIOpMBpDfAHAfgnfcpAA(unityHat_sourceAxis2);
				float x;
				float y;
				if (P_0.unityHat_checkNeverPressed)
				{
					if (CcAvFMRRhkwFbNNdTrjtVgrxqUyQ(unityHat_sourceAxis) || CcAvFMRRhkwFbNNdTrjtVgrxqUyQ(unityHat_sourceAxis2))
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
				if (iVimKXCDtVYxZskYyfyfyiTuBJcc(P_0.unityHat_isActiveAxisValues1.x, num2) && iVimKXCDtVYxZskYyfyfyiTuBJcc(P_0.unityHat_isActiveAxisValues1.y, num3))
				{
					return true;
				}
				if (iVimKXCDtVYxZskYyfyfyiTuBJcc(P_0.unityHat_isActiveAxisValues2.x, num2) && iVimKXCDtVYxZskYyfyfyiTuBJcc(P_0.unityHat_isActiveAxisValues2.y, num3))
				{
					return true;
				}
				if (iVimKXCDtVYxZskYyfyfyiTuBJcc(P_0.unityHat_isActiveAxisValues3.x, num2) && iVimKXCDtVYxZskYyfyfyiTuBJcc(P_0.unityHat_isActiveAxisValues3.y, num3))
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
							if (AAvzDaYnBdMWzfxfnvbphjzugTRg(customCalculationSourceData[k], out var flag3))
							{
								customCalculation.AddData(flag3 ? 1f : 0f);
							}
							break;
						}
						case HardwareElementSourceTypeWithHat.Axis:
						{
							if (hWiHvhhnCFuGrFtPSstgZhNuSbsR(customCalculationSourceData[k], out var num4))
							{
								customCalculation.AddData((num4 != 0f) ? 1f : 0f);
							}
							break;
						}
						case HardwareElementSourceTypeWithHat.Key:
						{
							if (KZvAPFAuDDwAmWlbLkIhAYVOwaQab(customCalculationSourceData[k], out var flag2))
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

		private bool iVimKXCDtVYxZskYyfyfyiTuBJcc(float P_0, float P_1)
		{
			return MathTools.IsNear(P_1, P_0, 0.1f);
		}

		private float vKvknQyltTIOpMBpDfAHAfgnfcpAA(HardwareJoystickMap.Platform_Fallback_Base.Axis P_0)
		{
			switch (P_0.sourceType)
			{
			case HardwareElementSourceTypeWithHat.Axis:
				if (P_0.sourceAxis == UnityAxis.None)
				{
					return 0f;
				}
				if (!CcAvFMRRhkwFbNNdTrjtVgrxqUyQ(P_0.sourceAxis))
				{
					return 0f;
				}
				return vKvknQyltTIOpMBpDfAHAfgnfcpAA(P_0.sourceAxis);
			case HardwareElementSourceTypeWithHat.Button:
				if (P_0.sourceButton == UnityButton.None)
				{
					return 0f;
				}
				if (!HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0.sourceButton))
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
					if (customCalculationSourceData[i] != null && customCalculationSourceData[i].sourceType == 1 && hWiHvhhnCFuGrFtPSstgZhNuSbsR(customCalculationSourceData[i], out var item))
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

		private float vKvknQyltTIOpMBpDfAHAfgnfcpAA(UnityAxis P_0)
		{
			if (P_0 == UnityAxis.None)
			{
				return 0f;
			}
			int num = (int)(P_0 - 1);
			return vZnqEotgQUkfWOprDNdZxrqurwoU[num];
		}

		private bool HJcRYlOYnieEhWqubAkAbPeBDumaA(UnityButton P_0)
		{
			int buttonIndex = (int)(P_0 - 1);
			return UnityInputHelper.GetJoystickButtonValueByJoystickId(xmEXVfyENtoyjKLfjizpdurnqqpi, buttonIndex);
		}

		private bool AAvzDaYnBdMWzfxfnvbphjzugTRg(HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData P_0, out bool P_1)
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
			P_1 = HJcRYlOYnieEhWqubAkAbPeBDumaA(sourceElement);
			return true;
		}

		private bool KZvAPFAuDDwAmWlbLkIhAYVOwaQab(HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData P_0, out bool P_1)
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

		private bool hWiHvhhnCFuGrFtPSstgZhNuSbsR(HardwareJoystickMap.Platform_Fallback_Base.CustomCalculationSourceData P_0, out float P_1)
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
			P_1 = vKvknQyltTIOpMBpDfAHAfgnfcpAA(sourceElement);
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

		private bool CcAvFMRRhkwFbNNdTrjtVgrxqUyQ(UnityAxis P_0)
		{
			int num = (int)(P_0 - 1);
			return UlMvPWNzOhnWpINgkvdfoOPBUjdm[num];
		}

		private void uxvVoENawKQUBgtDORfvcLmpPHIB()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = wlgJJzqpwWlttiTkqCeBJnfMAizL();
			if (UnityTools.isAndroidPlatform)
			{
				if (Regex.IsMatch(ypTGidgvvmNpwIGhvhmZEYWSvSMQ, "Xbox Wireless Controller.*"))
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
				else if (UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ != null)
				{
					IAndroidFallbackDS4Helper ds4Helper = UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ.ds4Helper;
					if (ds4Helper != null && ds4Helper.IsDS4(ypTGidgvvmNpwIGhvhmZEYWSvSMQ))
					{
						if (ds4Helper.IsDS4KeyMapped(mQPHeeJqpDVrGOXqKuQGLKqwdHwX))
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
			yPbTGFEOQNqKOEHVnHaIUhnHjgYD = ReInput.GetHardwareJoystickMap_InputManager(bridgedControllerHWInfo);
			if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD == null)
			{
				Rewired.Logger.LogError("Default hardware map not found!");
				return;
			}
			if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD.useSystemName && !string.IsNullOrEmpty(ypTGidgvvmNpwIGhvhmZEYWSvSMQ))
			{
				string text = Regex.Replace(ypTGidgvvmNpwIGhvhmZEYWSvSMQ, "\\s+", " ");
				text = text.Trim();
				if (!string.IsNullOrEmpty(text))
				{
					yPbTGFEOQNqKOEHVnHaIUhnHjgYD.controllerName = text;
				}
			}
			if (UnityTools.isIOSPlatform && yPbTGFEOQNqKOEHVnHaIUhnHjgYD.hardwareMapIdentifier.guid == Consts.joystickGuid_appleMFiController)
			{
				string text2 = fFqAVpIsbMJPMspfUpmDOWBKWshT(ypTGidgvvmNpwIGhvhmZEYWSvSMQ);
				if (!string.IsNullOrEmpty(text2))
				{
					yPbTGFEOQNqKOEHVnHaIUhnHjgYD.controllerName = text2;
				}
			}
			yLLlShtixVkbYMaIfuUMijmLCboKA = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.axisCount;
			tpoonPbJmajQyAErVLnAOoXhqZqEb = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonCount;
		}

		private void GicadQhCMRmSzBzyoPdNLHmFnguCA()
		{
			Array.Clear(QKMemPJOCQdfGcajCUROfzfnXvDE, 0, QKMemPJOCQdfGcajCUROfzfnXvDE.Length);
			Array.Clear(qugobParkJGeDYgxggFADuHJrSREA, 0, qugobParkJGeDYgxggFADuHJrSREA.Length);
		}

		private string gGionhosfhAUReDWKmiZOyIKcoAI()
		{
			if (ReInput.currentPlatform == Platform.Webplayer)
			{
				return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{ReInput.webplayerPlatform.ToString()}{ahmARFfBBEyFIxKNzxlGbdDXdOeBb().ToString()}{ypTGidgvvmNpwIGhvhmZEYWSvSMQ}");
			}
			if (UnityTools.isIOSPlatform)
			{
				string arg = Regex.Replace(ypTGidgvvmNpwIGhvhmZEYWSvSMQ, "joystick [0-9]+ by ", "");
				return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{ahmARFfBBEyFIxKNzxlGbdDXdOeBb().ToString()}{arg}");
			}
			return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{ahmARFfBBEyFIxKNzxlGbdDXdOeBb().ToString()}{ypTGidgvvmNpwIGhvhmZEYWSvSMQ}");
		}

		private InputSource ahmARFfBBEyFIxKNzxlGbdDXdOeBb()
		{
			if (UnityTools.platform == Platform.Linux && UnityTools.externalTools.LinuxInput_IsJoystickPreconfigured(ypTGidgvvmNpwIGhvhmZEYWSvSMQ))
			{
				return InputSource.Fallback_PreConfigured;
			}
			return InputSource.Fallback;
		}

		public static int XLDWYYvsgeAfEFpjEpYvIWmVwcbE(zoqILdNBnNfxqCLemZToKclpOtet P_0, zoqILdNBnNfxqCLemZToKclpOtet P_1)
		{
			if (P_0.inputManagerId < P_1.inputManagerId)
			{
				return -1;
			}
			if (P_0.inputManagerId > P_1.inputManagerId)
			{
				return 1;
			}
			return 0;
		}

		public static int TRDIAJUFCxRhmDTIJSePsDqSYGqX(zoqILdNBnNfxqCLemZToKclpOtet P_0, zoqILdNBnNfxqCLemZToKclpOtet P_1)
		{
			if (P_0.unityId < P_1.unityId)
			{
				return -1;
			}
			if (P_0.unityId > P_1.unityId)
			{
				return 1;
			}
			return 0;
		}

		private static string fFqAVpIsbMJPMspfUpmDOWBKWshT(string P_0)
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

	private class EYWqqnrkIRdtTDNjCHDYBPcNpdcnA
	{
		public enum VpKpvoOQcgISKeKbQejeGjnFalWlA
		{
			Exact = 0,
			Approximate = 1
		}

		public class GYlXTljJOVudFPaBLmYPxoIQOdfW
		{
			public int vLaDHvfmijzLwCtDfpDRfeHfsWbqc;

			public int mQPHeeJqpDVrGOXqKuQGLKqwdHwX;

			public string dguorTmaquKzUTHTlsJHWLmDYLpe;

			public int lICdlIDUnBRyGFLyltrkLDJRDJVO;

			public string DSkaXLxvBbkmkMXJKLKIeAKfAIeaA;

			public bool dRTWaRkWIkEMLraxMRHrbdGkCqGCA(zoqILdNBnNfxqCLemZToKclpOtet P_0, VpKpvoOQcgISKeKbQejeGjnFalWlA P_1)
			{
				if (P_0.rewiredId == vLaDHvfmijzLwCtDfpDRfeHfsWbqc)
				{
					return true;
				}
				if ((!string.IsNullOrEmpty(DSkaXLxvBbkmkMXJKLKIeAKfAIeaA) || !string.IsNullOrEmpty(P_0.DSkaXLxvBbkmkMXJKLKIeAKfAIeaA)) && !string.Equals(DSkaXLxvBbkmkMXJKLKIeAKfAIeaA, P_0.DSkaXLxvBbkmkMXJKLKIeAKfAIeaA, StringComparison.Ordinal))
				{
					return false;
				}
				switch (P_1)
				{
				case VpKpvoOQcgISKeKbQejeGjnFalWlA.Exact:
					if (mQPHeeJqpDVrGOXqKuQGLKqwdHwX == P_0.mQPHeeJqpDVrGOXqKuQGLKqwdHwX)
					{
						return dguorTmaquKzUTHTlsJHWLmDYLpe == P_0.ypTGidgvvmNpwIGhvhmZEYWSvSMQ;
					}
					return false;
				case VpKpvoOQcgISKeKbQejeGjnFalWlA.Approximate:
					return dguorTmaquKzUTHTlsJHWLmDYLpe == P_0.ypTGidgvvmNpwIGhvhmZEYWSvSMQ;
				default:
					throw new NotImplementedException();
				}
			}
		}

		private sealed class faudHEJibvDKvGeMAdKrBteqfxAw : IDisposable, IEnumerable<GYlXTljJOVudFPaBLmYPxoIQOdfW>, IEnumerator<GYlXTljJOVudFPaBLmYPxoIQOdfW>, IEnumerable, IEnumerator
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private GYlXTljJOVudFPaBLmYPxoIQOdfW VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public EYWqqnrkIRdtTDNjCHDYBPcNpdcnA TtytLoUfsgUyhsklaKccrnoMiiek;

			private zoqILdNBnNfxqCLemZToKclpOtet bkgcrrWCRFOtLkjgQGzzaVDQJynbA;

			public zoqILdNBnNfxqCLemZToKclpOtet VukUHiuaNKfVWgJZtDZBhZgYFbeXA;

			private VpKpvoOQcgISKeKbQejeGjnFalWlA WqfMvsqFdNcyCXobEzTCxnUdUxrF;

			public VpKpvoOQcgISKeKbQejeGjnFalWlA aGoqjFKjTqxYmTFXGFrTzlYRkhXy;

			private int YhcsJywchOwatkdZFPbKBklpfunV;

			private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

			GYlXTljJOVudFPaBLmYPxoIQOdfW IEnumerator<GYlXTljJOVudFPaBLmYPxoIQOdfW>.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return VqEePGSMyrGKIqkWibsjeHcWPSIx;
				}
			}

			[DebuggerHidden]
			public faudHEJibvDKvGeMAdKrBteqfxAw(int P_0)
			{
				RxAoyfYzYDsYonLGXsvUgwChukLk = P_0;
				wLBvvzlDcbAEPyDfgnprolEZBsgN = Thread.CurrentThread.ManagedThreadId;
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				int rxAoyfYzYDsYonLGXsvUgwChukLk = RxAoyfYzYDsYonLGXsvUgwChukLk;
				EYWqqnrkIRdtTDNjCHDYBPcNpdcnA ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				if (rxAoyfYzYDsYonLGXsvUgwChukLk != 0)
				{
					if (rxAoyfYzYDsYonLGXsvUgwChukLk != 1)
					{
						return false;
					}
					RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
					goto IL_0083;
				}
				RxAoyfYzYDsYonLGXsvUgwChukLk = -1;
				YhcsJywchOwatkdZFPbKBklpfunV = ttytLoUfsgUyhsklaKccrnoMiiek.UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count;
				fIMVaffCgsuIJcnrkMmGGKfPwwel = 0;
				goto IL_0093;
				IL_0083:
				fIMVaffCgsuIJcnrkMmGGKfPwwel++;
				goto IL_0093;
				IL_0093:
				if (fIMVaffCgsuIJcnrkMmGGKfPwwel < YhcsJywchOwatkdZFPbKBklpfunV)
				{
					if (ttytLoUfsgUyhsklaKccrnoMiiek.UwGmbGTujPCBtLRcPMDVnuFMDlrS[fIMVaffCgsuIJcnrkMmGGKfPwwel].dRTWaRkWIkEMLraxMRHrbdGkCqGCA(bkgcrrWCRFOtLkjgQGzzaVDQJynbA, WqfMvsqFdNcyCXobEzTCxnUdUxrF))
					{
						VqEePGSMyrGKIqkWibsjeHcWPSIx = ttytLoUfsgUyhsklaKccrnoMiiek.UwGmbGTujPCBtLRcPMDVnuFMDlrS[fIMVaffCgsuIJcnrkMmGGKfPwwel];
						RxAoyfYzYDsYonLGXsvUgwChukLk = 1;
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
			IEnumerator<GYlXTljJOVudFPaBLmYPxoIQOdfW> IEnumerable<GYlXTljJOVudFPaBLmYPxoIQOdfW>.GetEnumerator()
			{
				faudHEJibvDKvGeMAdKrBteqfxAw faudHEJibvDKvGeMAdKrBteqfxAw2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					faudHEJibvDKvGeMAdKrBteqfxAw2 = this;
				}
				else
				{
					faudHEJibvDKvGeMAdKrBteqfxAw2 = new faudHEJibvDKvGeMAdKrBteqfxAw(0);
					faudHEJibvDKvGeMAdKrBteqfxAw2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				faudHEJibvDKvGeMAdKrBteqfxAw2.bkgcrrWCRFOtLkjgQGzzaVDQJynbA = VukUHiuaNKfVWgJZtDZBhZgYFbeXA;
				faudHEJibvDKvGeMAdKrBteqfxAw2.WqfMvsqFdNcyCXobEzTCxnUdUxrF = aGoqjFKjTqxYmTFXGFrTzlYRkhXy;
				return faudHEJibvDKvGeMAdKrBteqfxAw2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<GYlXTljJOVudFPaBLmYPxoIQOdfW>)this).GetEnumerator();
			}
		}

		private List<GYlXTljJOVudFPaBLmYPxoIQOdfW> UwGmbGTujPCBtLRcPMDVnuFMDlrS;

		public int dtBwysZFYjgnHwWwaJGMOdwhhiAO => UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count;

		public EYWqqnrkIRdtTDNjCHDYBPcNpdcnA()
		{
			UwGmbGTujPCBtLRcPMDVnuFMDlrS = new List<GYlXTljJOVudFPaBLmYPxoIQOdfW>();
		}

		public void CQYqTMmvttNbVynJkterllzoWgKe(zoqILdNBnNfxqCLemZToKclpOtet P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			int count = UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count;
			for (int i = 0; i < count; i++)
			{
				if (UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].dRTWaRkWIkEMLraxMRHrbdGkCqGCA(P_0, VpKpvoOQcgISKeKbQejeGjnFalWlA.Exact))
				{
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].vLaDHvfmijzLwCtDfpDRfeHfsWbqc = P_0.rewiredId;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].dguorTmaquKzUTHTlsJHWLmDYLpe = P_0.ypTGidgvvmNpwIGhvhmZEYWSvSMQ;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].mQPHeeJqpDVrGOXqKuQGLKqwdHwX = P_0.mQPHeeJqpDVrGOXqKuQGLKqwdHwX;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].lICdlIDUnBRyGFLyltrkLDJRDJVO = P_0.inputManagerId;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].DSkaXLxvBbkmkMXJKLKIeAKfAIeaA = P_0.DSkaXLxvBbkmkMXJKLKIeAKfAIeaA;
					mIMjpeKiQlKAsiYxehIGdvJWmupB(P_0.rewiredId, i);
					return;
				}
			}
			UwGmbGTujPCBtLRcPMDVnuFMDlrS.Add(new GYlXTljJOVudFPaBLmYPxoIQOdfW
			{
				vLaDHvfmijzLwCtDfpDRfeHfsWbqc = P_0.rewiredId,
				dguorTmaquKzUTHTlsJHWLmDYLpe = P_0.ypTGidgvvmNpwIGhvhmZEYWSvSMQ,
				mQPHeeJqpDVrGOXqKuQGLKqwdHwX = P_0.mQPHeeJqpDVrGOXqKuQGLKqwdHwX,
				lICdlIDUnBRyGFLyltrkLDJRDJVO = P_0.inputManagerId,
				DSkaXLxvBbkmkMXJKLKIeAKfAIeaA = P_0.DSkaXLxvBbkmkMXJKLKIeAKfAIeaA
			});
			mIMjpeKiQlKAsiYxehIGdvJWmupB(P_0.rewiredId, UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count - 1);
		}

		public bool fSBMVaLfQvgqXypKYGeLWMIbARbB(zoqILdNBnNfxqCLemZToKclpOtet P_0, VpKpvoOQcgISKeKbQejeGjnFalWlA P_1)
		{
			int count = UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count;
			for (int i = 0; i < count; i++)
			{
				if (UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].dRTWaRkWIkEMLraxMRHrbdGkCqGCA(P_0, P_1))
				{
					return true;
				}
			}
			return false;
		}

		public IEnumerable<GYlXTljJOVudFPaBLmYPxoIQOdfW> JhAfrVJVWWlugTxWtItbSJXlMNCd(zoqILdNBnNfxqCLemZToKclpOtet P_0, VpKpvoOQcgISKeKbQejeGjnFalWlA P_1)
		{
			return new faudHEJibvDKvGeMAdKrBteqfxAw(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				VukUHiuaNKfVWgJZtDZBhZgYFbeXA = P_0,
				aGoqjFKjTqxYmTFXGFrTzlYRkhXy = P_1
			};
		}

		public int lsWdiPZgHHfEfIiesCpnLAlcpBgUA(GYlXTljJOVudFPaBLmYPxoIQOdfW P_0)
		{
			int count = UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count;
			for (int i = 0; i < count; i++)
			{
				if (UwGmbGTujPCBtLRcPMDVnuFMDlrS[i] == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		private void mIMjpeKiQlKAsiYxehIGdvJWmupB(int P_0, int P_1)
		{
			for (int num = UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count - 1; num >= 0; num--)
			{
				if (num != P_1 && UwGmbGTujPCBtLRcPMDVnuFMDlrS[num].vLaDHvfmijzLwCtDfpDRfeHfsWbqc == P_0)
				{
					UwGmbGTujPCBtLRcPMDVnuFMDlrS.RemoveAt(num);
				}
			}
		}
	}

	private List<zoqILdNBnNfxqCLemZToKclpOtet> pfvHZfkVJHuFpTIaVPSjIEuhIisl;

	private int OeeHBecrguVtJOJkdEjZKpQgYwls;

	private EYWqqnrkIRdtTDNjCHDYBPcNpdcnA yOgOXMdWSZKgHOWSXqOzTssrAWni;

	private bool mosSJnjLnefWplKjFDNNyyTdOAaG;

	private bool gyZmdDWBLLAULePnmwjuqaBhKJXF;

	private UpdateLoopType IykDTZdiEUdjfaMPasESCgLSFroIc;

	private UpdateLoopType aeALjbuijpFXdfoanKcgEpkzwSLpA;

	private TimerAbs QxgZNcDAKEOaKWcCjHpNWQgQDzuw;

	private Action<int, ControllerDataUpdater> zxGQuXWGAufVLbkLjNzszbXATpQrA;

	private PlatformInputManager dLeGbmnDSSIBftSnJkmMPNabIURs;

	private readonly IUnifiedKeyboardSource vhzLPVBhEKkbhqfdYYpfVnccxsE;

	private readonly IUnifiedMouseSource RLbEVGkEQQZvYRbhXfvKGeJrToou;

	private bool RZvaPAAXDgkohJRmfHnVBiaojHvgA;

	private string[] tSFfhliALhgEcIZXpkcXbpLANdlRA;

	[CustomObfuscation(rename = false)]
	public override int deviceCount => OeeHBecrguVtJOJkdEjZKpQgYwls;

	[CustomObfuscation(rename = false)]
	public override PlatformInputManager primaryInputManager => dLeGbmnDSSIBftSnJkmMPNabIURs;

	[CustomObfuscation(rename = false)]
	public override IInputSource inputSource => null;

	[CustomObfuscation(rename = false)]
	public override InputSource inputSourceType => InputSource.Fallback;

	public JnpGFOTHilhwgNkQYIjkXacPlURx(UpdateLoopSetting P_0)
	{
		dLeGbmnDSSIBftSnJkmMPNabIURs = this;
		vhzLPVBhEKkbhqfdYYpfVnccxsE = new UnityUnifiedKeyboardSource();
		RLbEVGkEQQZvYRbhXfvKGeJrToou = new UnityUnifiedMouseSource();
		using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
		{
			List<UpdateLoopType> list = tList.list;
			EnumConverter.ToUpdateLoopTypes(P_0, list);
			int num = 0;
			if (num < list.Count)
			{
				aeALjbuijpFXdfoanKcgEpkzwSLpA = list[num];
			}
		}
		tSFfhliALhgEcIZXpkcXbpLANdlRA = new string[0];
		zxGQuXWGAufVLbkLjNzszbXATpQrA = UpdateControllerData;
	}

	[CustomObfuscation(rename = false)]
	public override void Initialize()
	{
		if (UnityTools.isAndroidPlatform && UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ != null)
		{
			UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ.DeviceChangedEvent += rXBkAsgnUTJmrHAQYeXECWMjZrBR;
		}
		QxgZNcDAKEOaKWcCjHpNWQgQDzuw = new TimerAbs(1.0);
		yOgOXMdWSZKgHOWSXqOzTssrAWni = new EYWqqnrkIRdtTDNjCHDYBPcNpdcnA();
		vOupObHoGGqcZUAbdBHmnAnfgYrI();
		mosSJnjLnefWplKjFDNNyyTdOAaG = true;
		QxgZNcDAKEOaKWcCjHpNWQgQDzuw.Start();
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType updateLoop)
	{
		IykDTZdiEUdjfaMPasESCgLSFroIc = updateLoop;
		bjmwBoIMJKBLazbtLKGmjJSrgzcBA();
		if (mosSJnjLnefWplKjFDNNyyTdOAaG)
		{
			vKPnlrydRfdvHsSeMDWMWLWGxPmt();
		}
		QbVAjVWFtjNLxYhAcdqihqDFeguS(updateLoop);
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (UnityTools.isAndroidPlatform && UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ != null)
		{
			UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ.DeviceChangedEvent -= rXBkAsgnUTJmrHAQYeXECWMjZrBR;
		}
		(vhzLPVBhEKkbhqfdYYpfVnccxsE as IDisposable).Dispose();
		(RLbEVGkEQQZvYRbhXfvKGeJrToou as IDisposable).Dispose();
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return zxGQuXWGAufVLbkLjNzszbXATpQrA;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int assignedControllerId, ControllerDataUpdater data)
	{
		for (int i = 0; i < OeeHBecrguVtJOJkdEjZKpQgYwls; i++)
		{
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].inputManagerId == assignedControllerId)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].FillData(data);
				return;
			}
		}
		Rewired.Logger.LogError("Invalid joystick Id " + assignedControllerId + "!");
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		mosSJnjLnefWplKjFDNNyyTdOAaG = true;
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		mosSJnjLnefWplKjFDNNyyTdOAaG = true;
		if (_SystemDeviceDisconnectedEvent != null)
		{
			_SystemDeviceDisconnectedEvent();
		}
	}

	private void rXBkAsgnUTJmrHAQYeXECWMjZrBR()
	{
		mosSJnjLnefWplKjFDNNyyTdOAaG = true;
		gyZmdDWBLLAULePnmwjuqaBhKJXF = true;
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
	{
		for (int i = 0; i < pfvHZfkVJHuFpTIaVPSjIEuhIisl.Count; i++)
		{
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].unityId == unityJoystickId)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].fyIyAIgGFTkjOYexstkBHeMrXKfL();
			}
		}
		for (int j = 0; j < pfvHZfkVJHuFpTIaVPSjIEuhIisl.Count; j++)
		{
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[j].rewiredId == joystickId)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[j].MlPMLNgjWLmLZYnRFBEUGwEemXQI(unityJoystickId);
				break;
			}
		}
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedMouseSource GetUnifiedMouseSource()
	{
		return RLbEVGkEQQZvYRbhXfvKGeJrToou;
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedKeyboardSource GetUnifiedKeyboardSource()
	{
		return vhzLPVBhEKkbhqfdYYpfVnccxsE;
	}

	private void vOupObHoGGqcZUAbdBHmnAnfgYrI()
	{
		vOupObHoGGqcZUAbdBHmnAnfgYrI(Input.GetJoystickNames());
	}

	private void vOupObHoGGqcZUAbdBHmnAnfgYrI(string[] P_0)
	{
		int num = 0;
		List<zoqILdNBnNfxqCLemZToKclpOtet> list = pfvHZfkVJHuFpTIaVPSjIEuhIisl;
		int oeeHBecrguVtJOJkdEjZKpQgYwls = OeeHBecrguVtJOJkdEjZKpQgYwls;
		pfvHZfkVJHuFpTIaVPSjIEuhIisl = new List<zoqILdNBnNfxqCLemZToKclpOtet>();
		for (int i = 0; i < P_0.Length; i++)
		{
			string text = StringTools.SanitizeDeviceString(P_0[i]);
			if (UnityTools.IsValidUnityJoystickName(text))
			{
				zoqILdNBnNfxqCLemZToKclpOtet zoqILdNBnNfxqCLemZToKclpOtet2 = new zoqILdNBnNfxqCLemZToKclpOtet();
				zoqILdNBnNfxqCLemZToKclpOtet2.ypTGidgvvmNpwIGhvhmZEYWSvSMQ = text;
				zoqILdNBnNfxqCLemZToKclpOtet2.PUgQxpYkUBIdUwRqBetvbNDaNRxF = text;
				zoqILdNBnNfxqCLemZToKclpOtet2.mQPHeeJqpDVrGOXqKuQGLKqwdHwX = i;
				zoqILdNBnNfxqCLemZToKclpOtet2.unityId = i + 1;
				if (UnityTools.isAndroidPlatform && UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ != null)
				{
					zoqILdNBnNfxqCLemZToKclpOtet2.DSkaXLxvBbkmkMXJKLKIeAKfAIeaA = UnityTools.YamuuACgfaSgRUmyfSScpMCIdkMJ.GetUniqueDeviceIdentifier(text, i);
				}
				zoqILdNBnNfxqCLemZToKclpOtet2.XrAbLZWFEwcAyuirOieMPGMErUhj();
				pfvHZfkVJHuFpTIaVPSjIEuhIisl.Add(zoqILdNBnNfxqCLemZToKclpOtet2);
				num++;
			}
		}
		OeeHBecrguVtJOJkdEjZKpQgYwls = num;
		ftbgfAiDvmSdIeusVzSkTpuhpzBV(oeeHBecrguVtJOJkdEjZKpQgYwls, num, list, pfvHZfkVJHuFpTIaVPSjIEuhIisl);
		for (int j = 0; j < num; j++)
		{
			if (_UpdateControllerInfoEvent != null)
			{
				_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(pfvHZfkVJHuFpTIaVPSjIEuhIisl[j]));
			}
		}
		mZaUgDoxXIdSkCMllUmYWlPzEksd(list, pfvHZfkVJHuFpTIaVPSjIEuhIisl, false);
		mZaUgDoxXIdSkCMllUmYWlPzEksd(pfvHZfkVJHuFpTIaVPSjIEuhIisl, list, true);
		tSFfhliALhgEcIZXpkcXbpLANdlRA = P_0;
	}

	private void QbVAjVWFtjNLxYhAcdqihqDFeguS(UpdateLoopType P_0)
	{
		int count = pfvHZfkVJHuFpTIaVPSjIEuhIisl.Count;
		for (int i = 0; i < count; i++)
		{
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[i] != null)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].Update();
			}
		}
	}

	private void ftbgfAiDvmSdIeusVzSkTpuhpzBV(int P_0, int P_1, List<zoqILdNBnNfxqCLemZToKclpOtet> P_2, List<zoqILdNBnNfxqCLemZToKclpOtet> P_3)
	{
		if (P_1 > 0)
		{
			P_3.Sort(zoqILdNBnNfxqCLemZToKclpOtet.TRDIAJUFCxRhmDTIJSePsDqSYGqX);
		}
		if (P_0 > 0 && P_1 > 0)
		{
			xRxIiLagAyRqoXRhMeqWoRVnsVfg(P_1, P_3, P_0, P_2, EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.VpKpvoOQcgISKeKbQejeGjnFalWlA.Exact);
			xRxIiLagAyRqoXRhMeqWoRVnsVfg(P_1, P_3, P_0, P_2, EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.VpKpvoOQcgISKeKbQejeGjnFalWlA.Approximate);
		}
		pEdRcuYEfPeDfCbDzlhCjQTcTroNA(P_1, P_3, EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.VpKpvoOQcgISKeKbQejeGjnFalWlA.Exact);
		pEdRcuYEfPeDfCbDzlhCjQTcTroNA(P_1, P_3, EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.VpKpvoOQcgISKeKbQejeGjnFalWlA.Approximate);
		for (int i = 0; i < P_1; i++)
		{
			zoqILdNBnNfxqCLemZToKclpOtet zoqILdNBnNfxqCLemZToKclpOtet2 = P_3[i];
			if (zoqILdNBnNfxqCLemZToKclpOtet2 != null && zoqILdNBnNfxqCLemZToKclpOtet2.inputManagerId < 0)
			{
				zoqILdNBnNfxqCLemZToKclpOtet2.inputManagerId = UZDtQCJtoBAzOcKCFQAhAcWCSlRe(P_3);
				zoqILdNBnNfxqCLemZToKclpOtet2.rewiredId = ReInput.GetNewJoystickId();
				yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(zoqILdNBnNfxqCLemZToKclpOtet2);
			}
		}
		P_3.Sort(zoqILdNBnNfxqCLemZToKclpOtet.XLDWYYvsgeAfEFpjEpYvIWmVwcbE);
	}

	private void GyKaBpqsNjicYdUjpqfYSgHENAVTA(List<zoqILdNBnNfxqCLemZToKclpOtet> P_0, int P_1, int P_2)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (i != P_1 && P_0[i] != null && P_0[i].inputManagerId == P_2)
			{
				P_0[i].inputManagerId = -1;
			}
		}
	}

	private bool YOpkZHCZwdYZaaxJAJIvhEZXQJFg(List<zoqILdNBnNfxqCLemZToKclpOtet> P_0, int P_1)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && P_0[i].inputManagerId == P_1)
			{
				return false;
			}
		}
		return true;
	}

	private int UZDtQCJtoBAzOcKCFQAhAcWCSlRe(List<zoqILdNBnNfxqCLemZToKclpOtet> P_0)
	{
		int num = 0;
		while (true)
		{
			bool flag = false;
			int count = P_0.Count;
			for (int i = 0; i < count; i++)
			{
				if (P_0[i] != null && P_0[i].inputManagerId == num)
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

	private bool SOyWHlXMPORgpMASVXPztNAVPsaL(List<zoqILdNBnNfxqCLemZToKclpOtet> P_0, int P_1)
	{
		if (P_0 == null)
		{
			return false;
		}
		for (int i = 0; i < P_0.Count; i++)
		{
			if (P_0[i].rewiredId == P_1)
			{
				return true;
			}
		}
		return false;
	}

	private void xRxIiLagAyRqoXRhMeqWoRVnsVfg(int P_0, List<zoqILdNBnNfxqCLemZToKclpOtet> P_1, int P_2, List<zoqILdNBnNfxqCLemZToKclpOtet> P_3, EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.VpKpvoOQcgISKeKbQejeGjnFalWlA P_4)
	{
		int num = ((P_4 != EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.VpKpvoOQcgISKeKbQejeGjnFalWlA.Exact) ? 1 : 2);
		for (int i = 0; i < P_0; i++)
		{
			zoqILdNBnNfxqCLemZToKclpOtet zoqILdNBnNfxqCLemZToKclpOtet2 = P_1[i];
			if (zoqILdNBnNfxqCLemZToKclpOtet2 == null || zoqILdNBnNfxqCLemZToKclpOtet2.inputManagerId >= 0)
			{
				continue;
			}
			for (int j = 0; j < P_2; j++)
			{
				zoqILdNBnNfxqCLemZToKclpOtet zoqILdNBnNfxqCLemZToKclpOtet3 = P_3[j];
				if (zoqILdNBnNfxqCLemZToKclpOtet3 != null && !SOyWHlXMPORgpMASVXPztNAVPsaL(P_1, zoqILdNBnNfxqCLemZToKclpOtet3.rewiredId) && zoqILdNBnNfxqCLemZToKclpOtet2.dRTWaRkWIkEMLraxMRHrbdGkCqGCA(zoqILdNBnNfxqCLemZToKclpOtet3) >= num)
				{
					zoqILdNBnNfxqCLemZToKclpOtet2.inputManagerId = zoqILdNBnNfxqCLemZToKclpOtet3.inputManagerId;
					zoqILdNBnNfxqCLemZToKclpOtet2.rewiredId = zoqILdNBnNfxqCLemZToKclpOtet3.rewiredId;
					if (ReInput.isWindowsStandaloneWebplayerOrEditorPlatform && !UnityTools.windowsJoystickNamesReturnsEmptyStringsIfJoystickNull)
					{
						zoqILdNBnNfxqCLemZToKclpOtet2.unityId = zoqILdNBnNfxqCLemZToKclpOtet3.unityId;
					}
					yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(zoqILdNBnNfxqCLemZToKclpOtet2);
				}
			}
		}
	}

	private void pEdRcuYEfPeDfCbDzlhCjQTcTroNA(int P_0, List<zoqILdNBnNfxqCLemZToKclpOtet> P_1, EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.VpKpvoOQcgISKeKbQejeGjnFalWlA P_2)
	{
		for (int i = 0; i < P_0; i++)
		{
			zoqILdNBnNfxqCLemZToKclpOtet zoqILdNBnNfxqCLemZToKclpOtet2 = P_1[i];
			if (zoqILdNBnNfxqCLemZToKclpOtet2 == null || zoqILdNBnNfxqCLemZToKclpOtet2.inputManagerId >= 0)
			{
				continue;
			}
			EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.GYlXTljJOVudFPaBLmYPxoIQOdfW gYlXTljJOVudFPaBLmYPxoIQOdfW = null;
			foreach (EYWqqnrkIRdtTDNjCHDYBPcNpdcnA.GYlXTljJOVudFPaBLmYPxoIQOdfW item in yOgOXMdWSZKgHOWSXqOzTssrAWni.JhAfrVJVWWlugTxWtItbSJXlMNCd(zoqILdNBnNfxqCLemZToKclpOtet2, P_2))
			{
				if (!SOyWHlXMPORgpMASVXPztNAVPsaL(P_1, item.vLaDHvfmijzLwCtDfpDRfeHfsWbqc) && item.lICdlIDUnBRyGFLyltrkLDJRDJVO >= 0)
				{
					gYlXTljJOVudFPaBLmYPxoIQOdfW = item;
					break;
				}
			}
			if (gYlXTljJOVudFPaBLmYPxoIQOdfW != null)
			{
				int num = gYlXTljJOVudFPaBLmYPxoIQOdfW.lICdlIDUnBRyGFLyltrkLDJRDJVO;
				if (!YOpkZHCZwdYZaaxJAJIvhEZXQJFg(P_1, num))
				{
					num = (gYlXTljJOVudFPaBLmYPxoIQOdfW.lICdlIDUnBRyGFLyltrkLDJRDJVO = UZDtQCJtoBAzOcKCFQAhAcWCSlRe(P_1));
				}
				zoqILdNBnNfxqCLemZToKclpOtet2.inputManagerId = num;
				zoqILdNBnNfxqCLemZToKclpOtet2.rewiredId = gYlXTljJOVudFPaBLmYPxoIQOdfW.vLaDHvfmijzLwCtDfpDRfeHfsWbqc;
				yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(zoqILdNBnNfxqCLemZToKclpOtet2);
			}
		}
	}

	private void vKPnlrydRfdvHsSeMDWMWLWGxPmt()
	{
		string[] joystickNames = Input.GetJoystickNames();
		if (gyZmdDWBLLAULePnmwjuqaBhKJXF || YkzrGmlCpVDGEtVFCmSTRQlVMnyR(joystickNames))
		{
			vOupObHoGGqcZUAbdBHmnAnfgYrI(joystickNames);
		}
		mosSJnjLnefWplKjFDNNyyTdOAaG = false;
		if (gyZmdDWBLLAULePnmwjuqaBhKJXF)
		{
			gyZmdDWBLLAULePnmwjuqaBhKJXF = false;
		}
	}

	private bool YkzrGmlCpVDGEtVFCmSTRQlVMnyR(string[] P_0)
	{
		if (P_0.Length != tSFfhliALhgEcIZXpkcXbpLANdlRA.Length)
		{
			return true;
		}
		for (int i = 0; i < P_0.Length; i++)
		{
			if (!string.Equals(P_0[i], tSFfhliALhgEcIZXpkcXbpLANdlRA[i], StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	private void mZaUgDoxXIdSkCMllUmYWlPzEksd(List<zoqILdNBnNfxqCLemZToKclpOtet> P_0, List<zoqILdNBnNfxqCLemZToKclpOtet> P_1, bool P_2)
	{
		if (P_0 == null)
		{
			return;
		}
		int num = P_0?.Count ?? 0;
		int num2 = P_1?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			zoqILdNBnNfxqCLemZToKclpOtet zoqILdNBnNfxqCLemZToKclpOtet2 = P_0[i];
			if (zoqILdNBnNfxqCLemZToKclpOtet2 == null)
			{
				continue;
			}
			bool flag = false;
			if (P_1 != null)
			{
				for (int j = 0; j < num2; j++)
				{
					zoqILdNBnNfxqCLemZToKclpOtet zoqILdNBnNfxqCLemZToKclpOtet3 = P_1[j];
					if (zoqILdNBnNfxqCLemZToKclpOtet3 != null && zoqILdNBnNfxqCLemZToKclpOtet2.rewiredId == zoqILdNBnNfxqCLemZToKclpOtet3.rewiredId)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				CTOpPdkFJUgxrzSfEfbZBihGShecA(P_0[i], P_2);
			}
		}
	}

	private void CTOpPdkFJUgxrzSfEfbZBihGShecA(zoqILdNBnNfxqCLemZToKclpOtet P_0, bool P_1)
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

	private void bjmwBoIMJKBLazbtLKGmjJSrgzcBA()
	{
		if (IykDTZdiEUdjfaMPasESCgLSFroIc == aeALjbuijpFXdfoanKcgEpkzwSLpA && QxgZNcDAKEOaKWcCjHpNWQgQDzuw.Update())
		{
			mosSJnjLnefWplKjFDNNyyTdOAaG = true;
			QxgZNcDAKEOaKWcCjHpNWQgQDzuw.Start();
		}
	}
}
