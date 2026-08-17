using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using Rewired;
using Rewired.Data;
using Rewired.Data.Mapping;
using Rewired.InputSources.SDL2;
using Rewired.Interfaces;
using Rewired.Utils;

internal class wMWDgGeOIAaKcfOESivauebILOjQA : PlatformInputManager
{
	private class EQhPridDEKKfFdiVYTzoWiifaWBo : IInputManagerJoystick, IInputManagerJoystickPublic
	{
		private int kniQQKPsrTyUTKqICYwwQKlUfoRf;

		private int nyPspetimSyUTqhIxJtYJkZCgjlM;

		public Guid cUhzBTzwBmFECgMqAsRWDOSsbdVY;

		public string qLrlyPlxBfqlGvrINimGkfRchbID;

		public YnTfoKfozPExlkgidDzpkPNmatxBb chcOLBMXAtWUArFXncgBETOGbAnFb;

		public mXvowndavAghHUVZMyskDbfqLAGD fGuUukUdWRwxJbfpPdINQlKZVCQt;

		public string csdDcoRxMblcETGIzCXAoNKwiIkDA;

		public string noDyXDUdlPKBMSkRvbqYasAXqAFaA;

		public int IghITlQqAHYzeQDzcXCztBBmmcAl;

		public int kUHHTIgTMDyMEXufsUvSHmaExSkL;

		public Guid LbKjkHaEYcflCVdSDiaXPdeSnZquA;

		public PidVid uHkXbPenQtCEXhGoWMoJMQqwsSNMA;

		public Guid zvTfrjbwbUYmNvIBVuFLrUIudLXeb;

		public int DQsyboqTuputZkKyhNEOttVOTBJG;

		public int PWYKkeCbcvmAFsJTtvneFnSKIVsc;

		public int ADTEbWAdWQhnOwPCMHsuFBKgFUKjc;

		public int STrqcXxJVXWmOzdWQDdtJmduqYaEb;

		public int RjAiGdvCPfvtkTaCKkDzQpjuKUmN;

		public int geYuFmevHVggNCfxsdAQIVPpwWDoA;

		public bool yDgHnhhlyvEtcevEXbGbcSGoBwsvA;

		public bool BMKmLgTjByiExXMvsepaerGfBiXH;

		public int IvQuNSqDypWdalUGYcleYiqEVPmd;

		private float[] qugobParkJGeDYgxggFADuHJrSREA;

		private bool[] QKMemPJOCQdfGcajCUROfzfnXvDE;

		private HardwareJoystickMap_InputManager yPbTGFEOQNqKOEHVnHaIUhnHjgYD;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> MzECUhxACfqrWuBRNBbBzuCqFsTG;

		private bool FtnNbulCxEuMeVHmSwBWccMmJzKp;

		private bool ulpWNMyBfTTmuiYwnRErPHSaPABK;

		[CompilerGenerated]
		private Controller.Extension blPXWklICRaUBJcoCVJPuUIGmfUt;

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
		public string name => qLrlyPlxBfqlGvrINimGkfRchbID;

		[CustomObfuscation(rename = false)]
		public long? systemId
		{
			get
			{
				if (nyPspetimSyUTqhIxJtYJkZCgjlM < 0)
				{
					return null;
				}
				return nyPspetimSyUTqhIxJtYJkZCgjlM;
			}
		}

		[CustomObfuscation(rename = false)]
		public int unityId => 0;

		[CustomObfuscation(rename = false)]
		public Guid instanceGuid => LbKjkHaEYcflCVdSDiaXPdeSnZquA;

		[CustomObfuscation(rename = false)]
		public Guid persistentGuid => instanceGuid;

		[CustomObfuscation(rename = false)]
		public Controller.Extension extension
		{
			[CompilerGenerated]
			get
			{
				return blPXWklICRaUBJcoCVJPuUIGmfUt;
			}
			[CompilerGenerated]
			set
			{
				blPXWklICRaUBJcoCVJPuUIGmfUt = value;
			}
		}

		[CustomObfuscation(rename = false)]
		public void SetVibration(float amount, int motorIndex)
		{
			chcOLBMXAtWUArFXncgBETOGbAnFb.PcPfHjNOLiCEiQyoFFcjZDZPhsEu(motorIndex, amount, false);
		}

		[CustomObfuscation(rename = false)]
		public void StopVibration()
		{
		}

		public EQhPridDEKKfFdiVYTzoWiifaWBo(Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_0)
		{
			MzECUhxACfqrWuBRNBbBzuCqFsTG = P_0;
			nyPspetimSyUTqhIxJtYJkZCgjlM = -1;
			kniQQKPsrTyUTKqICYwwQKlUfoRf = -1;
		}

		public void XrAbLZWFEwcAyuirOieMPGMErUhj()
		{
			zvTfrjbwbUYmNvIBVuFLrUIudLXeb = MiscTools.CreateGuidHashSHA1(csdDcoRxMblcETGIzCXAoNKwiIkDA + uHkXbPenQtCEXhGoWMoJMQqwsSNMA.ToProductGuid().ToString());
			PWYKkeCbcvmAFsJTtvneFnSKIVsc = STrqcXxJVXWmOzdWQDdtJmduqYaEb;
			ADTEbWAdWQhnOwPCMHsuFBKgFUKjc = RjAiGdvCPfvtkTaCKkDzQpjuKUmN + geYuFmevHVggNCfxsdAQIVPpwWDoA * 8;
			uxvVoENawKQUBgtDORfvcLmpPHIB();
			cUhzBTzwBmFECgMqAsRWDOSsbdVY = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.hardwareMapIdentifier.guid;
			qLrlyPlxBfqlGvrINimGkfRchbID = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.controllerName;
			FtnNbulCxEuMeVHmSwBWccMmJzKp = ((cUhzBTzwBmFECgMqAsRWDOSsbdVY == Guid.Empty) ? true : false);
			qugobParkJGeDYgxggFADuHJrSREA = new float[PWYKkeCbcvmAFsJTtvneFnSKIVsc];
			QKMemPJOCQdfGcajCUROfzfnXvDE = new bool[ADTEbWAdWQhnOwPCMHsuFBKgFUKjc];
			Update();
		}

		public void FOuxuJsHWhCpyXhouIptKEIcQGsIA(EQhPridDEKKfFdiVYTzoWiifaWBo P_0)
		{
			if (P_0 != null)
			{
				nyPspetimSyUTqhIxJtYJkZCgjlM = P_0.nyPspetimSyUTqhIxJtYJkZCgjlM;
				kniQQKPsrTyUTKqICYwwQKlUfoRf = P_0.kniQQKPsrTyUTKqICYwwQKlUfoRf;
				for (int i = 0; i < MathTools.Min(QKMemPJOCQdfGcajCUROfzfnXvDE.Length, P_0.QKMemPJOCQdfGcajCUROfzfnXvDE.Length); i++)
				{
					QKMemPJOCQdfGcajCUROfzfnXvDE[i] = P_0.QKMemPJOCQdfGcajCUROfzfnXvDE[i];
				}
				for (int j = 0; j < MathTools.Min(qugobParkJGeDYgxggFADuHJrSREA.Length, P_0.qugobParkJGeDYgxggFADuHJrSREA.Length); j++)
				{
					qugobParkJGeDYgxggFADuHJrSREA[j] = P_0.qugobParkJGeDYgxggFADuHJrSREA[j];
				}
				ulpWNMyBfTTmuiYwnRErPHSaPABK = P_0.ulpWNMyBfTTmuiYwnRErPHSaPABK;
			}
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			dtkrhYOvGizZeJBgYiXXFyVpYjce();
			MGEvRBxTXmrzVAbFgmVqKAAmKjlj();
			if (!ulpWNMyBfTTmuiYwnRErPHSaPABK && chcOLBMXAtWUArFXncgBETOGbAnFb.ODfJrIENbPyzdtRhKIDHwOKyrmzn)
			{
				ulpWNMyBfTTmuiYwnRErPHSaPABK = true;
			}
		}

		[CustomObfuscation(rename = false)]
		public void FillData(ControllerDataUpdater dataUpdater)
		{
			if (PWYKkeCbcvmAFsJTtvneFnSKIVsc != dataUpdater.axisCount || ADTEbWAdWQhnOwPCMHsuFBKgFUKjc != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			for (int i = 0; i < PWYKkeCbcvmAFsJTtvneFnSKIVsc; i++)
			{
				dataUpdater.axisValues[i] = qugobParkJGeDYgxggFADuHJrSREA[i];
			}
			for (int j = 0; j < ADTEbWAdWQhnOwPCMHsuFBKgFUKjc; j++)
			{
				dataUpdater.buttonValues[j] = QKMemPJOCQdfGcajCUROfzfnXvDE[j];
			}
			if (ulpWNMyBfTTmuiYwnRErPHSaPABK && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		public int dRTWaRkWIkEMLraxMRHrbdGkCqGCA(EQhPridDEKKfFdiVYTzoWiifaWBo P_0)
		{
			if (P_0.kniQQKPsrTyUTKqICYwwQKlUfoRf == kniQQKPsrTyUTKqICYwwQKlUfoRf)
			{
				return 2;
			}
			if (STrqcXxJVXWmOzdWQDdtJmduqYaEb != P_0.STrqcXxJVXWmOzdWQDdtJmduqYaEb)
			{
				return 0;
			}
			if (RjAiGdvCPfvtkTaCKkDzQpjuKUmN != P_0.RjAiGdvCPfvtkTaCKkDzQpjuKUmN)
			{
				return 0;
			}
			if (geYuFmevHVggNCfxsdAQIVPpwWDoA != P_0.geYuFmevHVggNCfxsdAQIVPpwWDoA)
			{
				return 0;
			}
			if (P_0.LbKjkHaEYcflCVdSDiaXPdeSnZquA == LbKjkHaEYcflCVdSDiaXPdeSnZquA)
			{
				return 2;
			}
			if (P_0.zvTfrjbwbUYmNvIBVuFLrUIudLXeb == zvTfrjbwbUYmNvIBVuFLrUIudLXeb)
			{
				return 1;
			}
			return 0;
		}

		private BridgedControllerHWInfo wlgJJzqpwWlttiTkqCeBJnfMAizL()
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

		private void dtkrhYOvGizZeJBgYiXXFyVpYjce()
		{
			if (PWYKkeCbcvmAFsJTtvneFnSKIVsc <= 0 || yPbTGFEOQNqKOEHVnHaIUhnHjgYD.map.platform != InputPlatform.SDL2)
			{
				return;
			}
			HardwareJoystickMap.Platform_SDL2_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_SDL2_Base)yPbTGFEOQNqKOEHVnHaIUhnHjgYD.map).Axes_orig;
			if (axes_orig != null)
			{
				for (int i = 0; i < axes_orig.Length; i++)
				{
					iOawuunvjZwOqDnpYWMPuCJrnrzn(axes_orig[i], i);
				}
			}
		}

		private void MGEvRBxTXmrzVAbFgmVqKAAmKjlj()
		{
			if (ADTEbWAdWQhnOwPCMHsuFBKgFUKjc <= 0)
			{
				return;
			}
			HardwareJoystickMap.Platform_SDL2_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_SDL2_Base)yPbTGFEOQNqKOEHVnHaIUhnHjgYD.map).Buttons_orig;
			if (buttons_orig != null)
			{
				for (int i = 0; i < buttons_orig.Length; i++)
				{
					sIfnqLaRioGzNgrXtisbaLOUNTdk(buttons_orig[i], i);
				}
			}
		}

		private void iOawuunvjZwOqDnpYWMPuCJrnrzn(HardwareJoystickMap.Platform_SDL2_Base.Axis P_0, int P_1)
		{
			if (P_1 >= PWYKkeCbcvmAFsJTtvneFnSKIVsc)
			{
				throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
			}
			qugobParkJGeDYgxggFADuHJrSREA[P_1] = vKvknQyltTIOpMBpDfAHAfgnfcpAA(P_0);
		}

		private void sIfnqLaRioGzNgrXtisbaLOUNTdk(HardwareJoystickMap.Platform_SDL2_Base.Button P_0, int P_1)
		{
			if (P_1 >= ADTEbWAdWQhnOwPCMHsuFBKgFUKjc)
			{
				throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
			}
			QKMemPJOCQdfGcajCUROfzfnXvDE[P_1] = HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0);
		}

		private float vKvknQyltTIOpMBpDfAHAfgnfcpAA(HardwareJoystickMap.Platform_SDL2_Base.Axis P_0)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				int sourceAxis = P_0.sourceAxis;
				if (sourceAxis < 0 || sourceAxis >= STrqcXxJVXWmOzdWQDdtJmduqYaEb || sourceAxis >= 56)
				{
					return 0f;
				}
				return chcOLBMXAtWUArFXncgBETOGbAnFb.vKvknQyltTIOpMBpDfAHAfgnfcpAA(sourceAxis);
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				int sourceButton = P_0.sourceButton;
				if (sourceButton < 0 || sourceButton >= RjAiGdvCPfvtkTaCKkDzQpjuKUmN || sourceButton >= 256)
				{
					return 0f;
				}
				if (!chcOLBMXAtWUArFXncgBETOGbAnFb.HJcRYlOYnieEhWqubAkAbPeBDumaA(sourceButton))
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
				if (sourceHat < 0 || sourceHat >= geYuFmevHVggNCfxsdAQIVPpwWDoA || sourceHat >= 4)
				{
					return 0f;
				}
				int num = chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat);
				if (num < 0)
				{
					return 0f;
				}
				float num2;
				if (P_0.sourceHatDirection == AxisDirection.Horizontal)
				{
					num2 = IvdcddfvwwRILnGlVFSeHcKjGePRA(num, AxisDirection.Horizontal);
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
					num2 = IvdcddfvwwRILnGlVFSeHcKjGePRA(num, AxisDirection.Vertical);
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

		private bool HJcRYlOYnieEhWqubAkAbPeBDumaA(HardwareJoystickMap.Platform_SDL2_Base.Button P_0)
		{
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Button)
			{
				if (P_0.ignoreIfButtonsActive)
				{
					for (int i = 0; i < P_0.ignoreIfButtonsActiveButtons.Length; i++)
					{
						if (chcOLBMXAtWUArFXncgBETOGbAnFb.HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0.ignoreIfButtonsActiveButtons[i]))
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
						if (!chcOLBMXAtWUArFXncgBETOGbAnFb.HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0.requiredButtons[j]))
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
				if (sourceButton < 0 || sourceButton >= RjAiGdvCPfvtkTaCKkDzQpjuKUmN || sourceButton >= 256)
				{
					return false;
				}
				return chcOLBMXAtWUArFXncgBETOGbAnFb.HJcRYlOYnieEhWqubAkAbPeBDumaA(sourceButton);
			}
			if (P_0.sourceType == HardwareElementSourceTypeWithHat.Axis)
			{
				int sourceAxis = P_0.sourceAxis;
				if (sourceAxis <= 0 || sourceAxis >= STrqcXxJVXWmOzdWQDdtJmduqYaEb || sourceAxis >= 56)
				{
					return false;
				}
				float num = chcOLBMXAtWUArFXncgBETOGbAnFb.vKvknQyltTIOpMBpDfAHAfgnfcpAA(sourceAxis);
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
				if (sourceHat < 0 || sourceHat >= geYuFmevHVggNCfxsdAQIVPpwWDoA || sourceHat >= 4)
				{
					return false;
				}
				switch (P_0.sourceHatDirection)
				{
				case HatDirection.Up:
					return HGluafWxgyVhKeNbIhhZFzHqDurAb(chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat), 0, P_0.sourceHatType);
				case HatDirection.UpRight:
					return HGluafWxgyVhKeNbIhhZFzHqDurAb(chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat), 1, P_0.sourceHatType);
				case HatDirection.Right:
					return HGluafWxgyVhKeNbIhhZFzHqDurAb(chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat), 2, P_0.sourceHatType);
				case HatDirection.DownRight:
					return HGluafWxgyVhKeNbIhhZFzHqDurAb(chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat), 3, P_0.sourceHatType);
				case HatDirection.Down:
					return HGluafWxgyVhKeNbIhhZFzHqDurAb(chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat), 4, P_0.sourceHatType);
				case HatDirection.DownLeft:
					return HGluafWxgyVhKeNbIhhZFzHqDurAb(chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat), 5, P_0.sourceHatType);
				case HatDirection.Left:
					return HGluafWxgyVhKeNbIhhZFzHqDurAb(chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat), 6, P_0.sourceHatType);
				case HatDirection.UpLeft:
					return HGluafWxgyVhKeNbIhhZFzHqDurAb(chcOLBMXAtWUArFXncgBETOGbAnFb.wdZBeFkvcmdlqMODBpHbBGqBelryb(sourceHat), 7, P_0.sourceHatType);
				}
			}
			return false;
		}

		private bool HGluafWxgyVhKeNbIhhZFzHqDurAb(int P_0, int P_1, HatType P_2)
		{
			if (P_0 < 0)
			{
				return false;
			}
			if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD.isUnknownController && !InputTools.HandleForced4WayHatsOnUnknownControllers(P_1, ref P_2))
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

		private float IvdcddfvwwRILnGlVFSeHcKjGePRA(int P_0, AxisDirection P_1)
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

		private ControlDeviceType pOySFecLitEWsSXvjoCUDiVkDlVI(mXvowndavAghHUVZMyskDbfqLAGD P_0)
		{
			return P_0 switch
			{
				mXvowndavAghHUVZMyskDbfqLAGD.Joystick => ControlDeviceType.Joystick, 
				mXvowndavAghHUVZMyskDbfqLAGD.Gamepad => ControlDeviceType.Gamepad, 
				mXvowndavAghHUVZMyskDbfqLAGD.Keyboard => ControlDeviceType.Keyboard, 
				mXvowndavAghHUVZMyskDbfqLAGD.Mouse => ControlDeviceType.Mouse, 
				_ => ControlDeviceType.Unknown, 
			};
		}

		private void uxvVoENawKQUBgtDORfvcLmpPHIB()
		{
			yPbTGFEOQNqKOEHVnHaIUhnHjgYD = MzECUhxACfqrWuBRNBbBzuCqFsTG(wlgJJzqpwWlttiTkqCeBJnfMAizL());
			if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD == null)
			{
				Logger.LogError("Default hardware map not found!");
				return;
			}
			if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD.useSystemName && !string.IsNullOrEmpty(noDyXDUdlPKBMSkRvbqYasAXqAFaA))
			{
				string text = Regex.Replace(noDyXDUdlPKBMSkRvbqYasAXqAFaA, "\\s+", " ");
				text = text.Trim();
				if (!string.IsNullOrEmpty(text))
				{
					yPbTGFEOQNqKOEHVnHaIUhnHjgYD.controllerName = text;
				}
			}
			PWYKkeCbcvmAFsJTtvneFnSKIVsc = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.axisCount;
			ADTEbWAdWQhnOwPCMHsuFBKgFUKjc = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.buttonCount;
		}

		private string gtoVVZNIDmgtTIvhvxTNFXqPhDvn()
		{
			return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{chcOLBMXAtWUArFXncgBETOGbAnFb.TTLdWdiVtetpvRZZFSbUvyMAQDkcA}{csdDcoRxMblcETGIzCXAoNKwiIkDA}{IghITlQqAHYzeQDzcXCztBBmmcAl}{uHkXbPenQtCEXhGoWMoJMQqwsSNMA.ToProductGuid()}");
		}

		private void ZsSGuyFeNhejoyHSIRMfnABaLlydA(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.SDL2;
			P_0.inputSource = chcOLBMXAtWUArFXncgBETOGbAnFb.TTLdWdiVtetpvRZZFSbUvyMAQDkcA;
			P_0.deviceType = pOySFecLitEWsSXvjoCUDiVkDlVI(fGuUukUdWRwxJbfpPdINQlKZVCQt);
			P_0.hardwareIdentifier = gtoVVZNIDmgtTIvhvxTNFXqPhDvn();
			P_0.hardwareAxisCount = STrqcXxJVXWmOzdWQDdtJmduqYaEb;
			P_0.hardwareButtonCount = RjAiGdvCPfvtkTaCKkDzQpjuKUmN;
			P_0.hardwareHatCount = geYuFmevHVggNCfxsdAQIVPpwWDoA;
			P_0.hw_productName = csdDcoRxMblcETGIzCXAoNKwiIkDA;
			P_0.hw_deviceGuid = LbKjkHaEYcflCVdSDiaXPdeSnZquA;
			P_0.hw_productId = IghITlQqAHYzeQDzcXCztBBmmcAl;
			P_0.hw_pidVid = uHkXbPenQtCEXhGoWMoJMQqwsSNMA;
			P_0.hw_isBluetoothDevice = yDgHnhhlyvEtcevEXbGbcSGoBwsvA;
			P_0.hw_bluetoothDeviceName = csdDcoRxMblcETGIzCXAoNKwiIkDA;
			P_0.hw_systemDeviceName = csdDcoRxMblcETGIzCXAoNKwiIkDA;
			P_0.hw_supportsVibration = BMKmLgTjByiExXMvsepaerGfBiXH;
			P_0.hw_isSDL2Gamepad = chcOLBMXAtWUArFXncgBETOGbAnFb.TKSlXXrapHpSocUbWYuNoGeAvlLl == mXvowndavAghHUVZMyskDbfqLAGD.Gamepad;
			P_0.hw_localVibrationMotorCount = IvQuNSqDypWdalUGYcleYiqEVPmd;
		}

		private void ZsSGuyFeNhejoyHSIRMfnABaLlydA(BridgedController P_0)
		{
			ZsSGuyFeNhejoyHSIRMfnABaLlydA((BridgedControllerHWInfo)P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.ToGameHardwareControllerMap();
			P_0.instanceName = csdDcoRxMblcETGIzCXAoNKwiIkDA;
			P_0.productName = csdDcoRxMblcETGIzCXAoNKwiIkDA;
			P_0.axisCount = PWYKkeCbcvmAFsJTtvneFnSKIVsc;
			P_0.buttonCount = ADTEbWAdWQhnOwPCMHsuFBKgFUKjc;
			P_0.unknownControllerHats = eRozBhKEInCxvuoaZhGEKdOcJaCF();
			P_0.controllerTypeGuid = cUhzBTzwBmFECgMqAsRWDOSsbdVY;
			P_0.controllerExtension = extension;
		}

		private void heCLPJLQigDdgYqIaqVoLnCSSEtt()
		{
			for (int i = 0; i < ADTEbWAdWQhnOwPCMHsuFBKgFUKjc; i++)
			{
				QKMemPJOCQdfGcajCUROfzfnXvDE[i] = false;
			}
			for (int j = 0; j < PWYKkeCbcvmAFsJTtvneFnSKIVsc; j++)
			{
				qugobParkJGeDYgxggFADuHJrSREA[j] = 0f;
			}
		}

		private UnknownControllerHat[] eRozBhKEInCxvuoaZhGEKdOcJaCF()
		{
			if (!FtnNbulCxEuMeVHmSwBWccMmJzKp)
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

		public static int tcaSqFHHskanwDdVoIpQoXasFnou(EQhPridDEKKfFdiVYTzoWiifaWBo P_0, EQhPridDEKKfFdiVYTzoWiifaWBo P_1)
		{
			if (P_0.nyPspetimSyUTqhIxJtYJkZCgjlM < P_1.nyPspetimSyUTqhIxJtYJkZCgjlM)
			{
				return -1;
			}
			if (P_0.nyPspetimSyUTqhIxJtYJkZCgjlM > P_1.nyPspetimSyUTqhIxJtYJkZCgjlM)
			{
				return 1;
			}
			return 0;
		}

		public static int bfSsVtsssSHIAIjxnhSiETrvlrDy(EQhPridDEKKfFdiVYTzoWiifaWBo P_0, EQhPridDEKKfFdiVYTzoWiifaWBo P_1)
		{
			if (P_0.DQsyboqTuputZkKyhNEOttVOTBJG < P_1.DQsyboqTuputZkKyhNEOttVOTBJG)
			{
				return -1;
			}
			if (P_0.DQsyboqTuputZkKyhNEOttVOTBJG > P_1.DQsyboqTuputZkKyhNEOttVOTBJG)
			{
				return 1;
			}
			return 0;
		}
	}

	private class TJbdTBCpXhbfktlyjegnXzRVsHrWA
	{
		public enum exEStnnLxQgAFfOceYTIvndgAdCM
		{
			Exact = 0,
			Approximate = 1
		}

		public class ZzLhWCBmdUYOmSHtQZkdQIXhgjli
		{
			public int vLaDHvfmijzLwCtDfpDRfeHfsWbqc;

			public Guid rWMIAFcgxIbsJaFNJhzhhxuHtcTX;

			public Guid zvTfrjbwbUYmNvIBVuFLrUIudLXeb;

			public int lICdlIDUnBRyGFLyltrkLDJRDJVO;

			public int STrqcXxJVXWmOzdWQDdtJmduqYaEb;

			public int RjAiGdvCPfvtkTaCKkDzQpjuKUmN;

			public int geYuFmevHVggNCfxsdAQIVPpwWDoA;

			public bool dRTWaRkWIkEMLraxMRHrbdGkCqGCA(EQhPridDEKKfFdiVYTzoWiifaWBo P_0, exEStnnLxQgAFfOceYTIvndgAdCM P_1)
			{
				if (P_0.rewiredId == vLaDHvfmijzLwCtDfpDRfeHfsWbqc)
				{
					return true;
				}
				if (STrqcXxJVXWmOzdWQDdtJmduqYaEb != P_0.STrqcXxJVXWmOzdWQDdtJmduqYaEb)
				{
					return false;
				}
				if (RjAiGdvCPfvtkTaCKkDzQpjuKUmN != P_0.RjAiGdvCPfvtkTaCKkDzQpjuKUmN)
				{
					return false;
				}
				if (geYuFmevHVggNCfxsdAQIVPpwWDoA != P_0.geYuFmevHVggNCfxsdAQIVPpwWDoA)
				{
					return false;
				}
				return P_1 switch
				{
					exEStnnLxQgAFfOceYTIvndgAdCM.Exact => rWMIAFcgxIbsJaFNJhzhhxuHtcTX == P_0.LbKjkHaEYcflCVdSDiaXPdeSnZquA, 
					exEStnnLxQgAFfOceYTIvndgAdCM.Approximate => zvTfrjbwbUYmNvIBVuFLrUIudLXeb == P_0.zvTfrjbwbUYmNvIBVuFLrUIudLXeb, 
					_ => throw new NotImplementedException(), 
				};
			}
		}

		private sealed class oQfrtfexxzmkZhwQnONxlTERpLFy : IDisposable, IEnumerable, IEnumerator, IEnumerable<ZzLhWCBmdUYOmSHtQZkdQIXhgjli>, IEnumerator<ZzLhWCBmdUYOmSHtQZkdQIXhgjli>
		{
			private int RxAoyfYzYDsYonLGXsvUgwChukLk;

			private ZzLhWCBmdUYOmSHtQZkdQIXhgjli VqEePGSMyrGKIqkWibsjeHcWPSIx;

			private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

			public TJbdTBCpXhbfktlyjegnXzRVsHrWA TtytLoUfsgUyhsklaKccrnoMiiek;

			private EQhPridDEKKfFdiVYTzoWiifaWBo bkgcrrWCRFOtLkjgQGzzaVDQJynbA;

			public EQhPridDEKKfFdiVYTzoWiifaWBo VukUHiuaNKfVWgJZtDZBhZgYFbeXA;

			private exEStnnLxQgAFfOceYTIvndgAdCM WqfMvsqFdNcyCXobEzTCxnUdUxrF;

			public exEStnnLxQgAFfOceYTIvndgAdCM aGoqjFKjTqxYmTFXGFrTzlYRkhXy;

			private int YhcsJywchOwatkdZFPbKBklpfunV;

			private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

			ZzLhWCBmdUYOmSHtQZkdQIXhgjli IEnumerator<ZzLhWCBmdUYOmSHtQZkdQIXhgjli>.Current
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
			public oQfrtfexxzmkZhwQnONxlTERpLFy(int P_0)
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
				TJbdTBCpXhbfktlyjegnXzRVsHrWA ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
			IEnumerator<ZzLhWCBmdUYOmSHtQZkdQIXhgjli> IEnumerable<ZzLhWCBmdUYOmSHtQZkdQIXhgjli>.GetEnumerator()
			{
				oQfrtfexxzmkZhwQnONxlTERpLFy oQfrtfexxzmkZhwQnONxlTERpLFy2;
				if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
				{
					RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
					oQfrtfexxzmkZhwQnONxlTERpLFy2 = this;
				}
				else
				{
					oQfrtfexxzmkZhwQnONxlTERpLFy2 = new oQfrtfexxzmkZhwQnONxlTERpLFy(0);
					oQfrtfexxzmkZhwQnONxlTERpLFy2.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
				}
				oQfrtfexxzmkZhwQnONxlTERpLFy2.bkgcrrWCRFOtLkjgQGzzaVDQJynbA = VukUHiuaNKfVWgJZtDZBhZgYFbeXA;
				oQfrtfexxzmkZhwQnONxlTERpLFy2.WqfMvsqFdNcyCXobEzTCxnUdUxrF = aGoqjFKjTqxYmTFXGFrTzlYRkhXy;
				return oQfrtfexxzmkZhwQnONxlTERpLFy2;
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ZzLhWCBmdUYOmSHtQZkdQIXhgjli>)this).GetEnumerator();
			}
		}

		private List<ZzLhWCBmdUYOmSHtQZkdQIXhgjli> UwGmbGTujPCBtLRcPMDVnuFMDlrS;

		public TJbdTBCpXhbfktlyjegnXzRVsHrWA()
		{
			UwGmbGTujPCBtLRcPMDVnuFMDlrS = new List<ZzLhWCBmdUYOmSHtQZkdQIXhgjli>();
		}

		public void CQYqTMmvttNbVynJkterllzoWgKe(EQhPridDEKKfFdiVYTzoWiifaWBo P_0)
		{
			if (P_0 == null)
			{
				return;
			}
			int count = UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count;
			for (int i = 0; i < count; i++)
			{
				if (UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].dRTWaRkWIkEMLraxMRHrbdGkCqGCA(P_0, exEStnnLxQgAFfOceYTIvndgAdCM.Exact))
				{
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].vLaDHvfmijzLwCtDfpDRfeHfsWbqc = P_0.rewiredId;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].rWMIAFcgxIbsJaFNJhzhhxuHtcTX = P_0.LbKjkHaEYcflCVdSDiaXPdeSnZquA;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].zvTfrjbwbUYmNvIBVuFLrUIudLXeb = P_0.zvTfrjbwbUYmNvIBVuFLrUIudLXeb;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].lICdlIDUnBRyGFLyltrkLDJRDJVO = P_0.inputManagerId;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].STrqcXxJVXWmOzdWQDdtJmduqYaEb = P_0.STrqcXxJVXWmOzdWQDdtJmduqYaEb;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].RjAiGdvCPfvtkTaCKkDzQpjuKUmN = P_0.RjAiGdvCPfvtkTaCKkDzQpjuKUmN;
					UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].geYuFmevHVggNCfxsdAQIVPpwWDoA = P_0.geYuFmevHVggNCfxsdAQIVPpwWDoA;
					mIMjpeKiQlKAsiYxehIGdvJWmupB(P_0.rewiredId, P_0.LbKjkHaEYcflCVdSDiaXPdeSnZquA, i);
					return;
				}
			}
			UwGmbGTujPCBtLRcPMDVnuFMDlrS.Add(new ZzLhWCBmdUYOmSHtQZkdQIXhgjli
			{
				vLaDHvfmijzLwCtDfpDRfeHfsWbqc = P_0.rewiredId,
				rWMIAFcgxIbsJaFNJhzhhxuHtcTX = P_0.LbKjkHaEYcflCVdSDiaXPdeSnZquA,
				zvTfrjbwbUYmNvIBVuFLrUIudLXeb = P_0.zvTfrjbwbUYmNvIBVuFLrUIudLXeb,
				lICdlIDUnBRyGFLyltrkLDJRDJVO = P_0.inputManagerId,
				STrqcXxJVXWmOzdWQDdtJmduqYaEb = P_0.STrqcXxJVXWmOzdWQDdtJmduqYaEb,
				RjAiGdvCPfvtkTaCKkDzQpjuKUmN = P_0.RjAiGdvCPfvtkTaCKkDzQpjuKUmN,
				geYuFmevHVggNCfxsdAQIVPpwWDoA = P_0.geYuFmevHVggNCfxsdAQIVPpwWDoA
			});
			mIMjpeKiQlKAsiYxehIGdvJWmupB(P_0.rewiredId, P_0.LbKjkHaEYcflCVdSDiaXPdeSnZquA, UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count - 1);
		}

		public bool fSBMVaLfQvgqXypKYGeLWMIbARbB(EQhPridDEKKfFdiVYTzoWiifaWBo P_0, exEStnnLxQgAFfOceYTIvndgAdCM P_1)
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

		public IEnumerable<ZzLhWCBmdUYOmSHtQZkdQIXhgjli> JhAfrVJVWWlugTxWtItbSJXlMNCd(EQhPridDEKKfFdiVYTzoWiifaWBo P_0, exEStnnLxQgAFfOceYTIvndgAdCM P_1)
		{
			return new oQfrtfexxzmkZhwQnONxlTERpLFy(-2)
			{
				TtytLoUfsgUyhsklaKccrnoMiiek = this,
				VukUHiuaNKfVWgJZtDZBhZgYFbeXA = P_0,
				aGoqjFKjTqxYmTFXGFrTzlYRkhXy = P_1
			};
		}

		private void mIMjpeKiQlKAsiYxehIGdvJWmupB(int P_0, Guid P_1, int P_2)
		{
			for (int num = UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count - 1; num >= 0; num--)
			{
				if (num != P_2 && (UwGmbGTujPCBtLRcPMDVnuFMDlrS[num].vLaDHvfmijzLwCtDfpDRfeHfsWbqc == P_0 || UwGmbGTujPCBtLRcPMDVnuFMDlrS[num].rWMIAFcgxIbsJaFNJhzhhxuHtcTX == P_1))
				{
					UwGmbGTujPCBtLRcPMDVnuFMDlrS.RemoveAt(num);
				}
			}
		}
	}

	internal const bool BUmHgvFAJSaxNqKAkNFxTGAZejPIA = true;

	private IInputSource mKGFJudopEcewqKpuxRtaCtIVgIn;

	private List<EQhPridDEKKfFdiVYTzoWiifaWBo> pfvHZfkVJHuFpTIaVPSjIEuhIisl;

	private int OeeHBecrguVtJOJkdEjZKpQgYwls;

	private TJbdTBCpXhbfktlyjegnXzRVsHrWA yOgOXMdWSZKgHOWSXqOzTssrAWni;

	private bool mosSJnjLnefWplKjFDNNyyTdOAaG;

	private Action<int, ControllerDataUpdater> zxGQuXWGAufVLbkLjNzszbXATpQrA;

	private PlatformInputManager dLeGbmnDSSIBftSnJkmMPNabIURs;

	private readonly bool APoNWtXoeYHyrbDkDswtiTxkfkUL;

	private readonly bool eEerJXpIkLJjWGhWhVTVcNOaIiBAA;

	private readonly bool ZZJerRuoZikIBFxmWuqdynzQHrgs;

	private readonly Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> MzECUhxACfqrWuBRNBbBzuCqFsTG;

	private readonly Func<int> VbbCuMeRTNjQXExlYVBnBBhBxmfmB;

	[CustomObfuscation(rename = false)]
	public override int deviceCount => OeeHBecrguVtJOJkdEjZKpQgYwls;

	[CustomObfuscation(rename = false)]
	public override PlatformInputManager primaryInputManager => dLeGbmnDSSIBftSnJkmMPNabIURs;

	[CustomObfuscation(rename = false)]
	public override IInputSource inputSource => mKGFJudopEcewqKpuxRtaCtIVgIn;

	[CustomObfuscation(rename = false)]
	public override InputSource inputSourceType => InputSource.SDL2;

	public wMWDgGeOIAaKcfOESivauebILOjQA(ConfigVars P_0, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_1, Func<int> P_2, bool P_3, bool P_4, bool P_5)
	{
		try
		{
			MzECUhxACfqrWuBRNBbBzuCqFsTG = P_1;
			VbbCuMeRTNjQXExlYVBnBBhBxmfmB = P_2;
			APoNWtXoeYHyrbDkDswtiTxkfkUL = P_3;
			eEerJXpIkLJjWGhWhVTVcNOaIiBAA = P_4;
			ZZJerRuoZikIBFxmWuqdynzQHrgs = P_5;
			dLeGbmnDSSIBftSnJkmMPNabIURs = this;
			mKGFJudopEcewqKpuxRtaCtIVgIn = new SDL2InputSource(P_0.updateLoop, P_3, P_3, P_4, P_5);
			zxGQuXWGAufVLbkLjNzszbXATpQrA = UpdateControllerData;
			mKGFJudopEcewqKpuxRtaCtIVgIn.DeviceChangedEvent += uuLlAFZlZpgBkHAqKoLlGqkTLpdC;
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
		if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
		{
			yOgOXMdWSZKgHOWSXqOzTssrAWni = new TJbdTBCpXhbfktlyjegnXzRVsHrWA();
			vOupObHoGGqcZUAbdBHmnAnfgYrI();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType updateLoop)
	{
		if (mKGFJudopEcewqKpuxRtaCtIVgIn != null)
		{
			mKGFJudopEcewqKpuxRtaCtIVgIn.Update();
		}
		if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
		{
			if (mosSJnjLnefWplKjFDNNyyTdOAaG)
			{
				vKPnlrydRfdvHsSeMDWMWLWGxPmt();
			}
			if (mKGFJudopEcewqKpuxRtaCtIVgIn != null)
			{
				for (int i = 0; i < OeeHBecrguVtJOJkdEjZKpQgYwls; i++)
				{
					pfvHZfkVJHuFpTIaVPSjIEuhIisl[i]?.chcOLBMXAtWUArFXncgBETOGbAnFb.Update(updateLoop);
				}
				mKGFJudopEcewqKpuxRtaCtIVgIn.UpdateDevices(updateLoop);
			}
			QbVAjVWFtjNLxYhAcdqihqDFeguS();
			if (mKGFJudopEcewqKpuxRtaCtIVgIn != null)
			{
				mKGFJudopEcewqKpuxRtaCtIVgIn.UpdateFinished();
				for (int j = 0; j < OeeHBecrguVtJOJkdEjZKpQgYwls; j++)
				{
					pfvHZfkVJHuFpTIaVPSjIEuhIisl[j]?.chcOLBMXAtWUArFXncgBETOGbAnFb.UpdateFinished();
				}
			}
		}
		_ = eEerJXpIkLJjWGhWhVTVcNOaIiBAA;
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (pfvHZfkVJHuFpTIaVPSjIEuhIisl != null)
		{
			int count = pfvHZfkVJHuFpTIaVPSjIEuhIisl.Count;
			for (int i = 0; i < count; i++)
			{
				if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[i] != null)
				{
					pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].chcOLBMXAtWUArFXncgBETOGbAnFb?.uBudcUDlbaqFsEZPnsQAdFKIQwlpA();
				}
			}
		}
		if (mKGFJudopEcewqKpuxRtaCtIVgIn != null)
		{
			mKGFJudopEcewqKpuxRtaCtIVgIn.Dispose();
		}
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return zxGQuXWGAufVLbkLjNzszbXATpQrA;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int inputManagerId, ControllerDataUpdater data)
	{
		if (!APoNWtXoeYHyrbDkDswtiTxkfkUL)
		{
			return;
		}
		for (int i = 0; i < OeeHBecrguVtJOJkdEjZKpQgYwls; i++)
		{
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].inputManagerId == inputManagerId)
			{
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].FillData(data);
				break;
			}
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
		{
			mosSJnjLnefWplKjFDNNyyTdOAaG = true;
		}
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
		{
			mosSJnjLnefWplKjFDNNyyTdOAaG = true;
		}
		if (_SystemDeviceDisconnectedEvent != null)
		{
			_SystemDeviceDisconnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
	{
		_ = APoNWtXoeYHyrbDkDswtiTxkfkUL;
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

	private void vOupObHoGGqcZUAbdBHmnAnfgYrI()
	{
		vOupObHoGGqcZUAbdBHmnAnfgYrI(gwuqAFYNpjJGCopUdNidgJZgiWVg());
	}

	private void vOupObHoGGqcZUAbdBHmnAnfgYrI(IList<YnTfoKfozPExlkgidDzpkPNmatxBb> P_0)
	{
		int num = 0;
		List<EQhPridDEKKfFdiVYTzoWiifaWBo> list = pfvHZfkVJHuFpTIaVPSjIEuhIisl;
		int oeeHBecrguVtJOJkdEjZKpQgYwls = OeeHBecrguVtJOJkdEjZKpQgYwls;
		pfvHZfkVJHuFpTIaVPSjIEuhIisl = new List<EQhPridDEKKfFdiVYTzoWiifaWBo>();
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null)
			{
				YnTfoKfozPExlkgidDzpkPNmatxBb ynTfoKfozPExlkgidDzpkPNmatxBb = P_0[i];
				EQhPridDEKKfFdiVYTzoWiifaWBo eQhPridDEKKfFdiVYTzoWiifaWBo = new EQhPridDEKKfFdiVYTzoWiifaWBo(MzECUhxACfqrWuBRNBbBzuCqFsTG);
				eQhPridDEKKfFdiVYTzoWiifaWBo.chcOLBMXAtWUArFXncgBETOGbAnFb = ynTfoKfozPExlkgidDzpkPNmatxBb;
				eQhPridDEKKfFdiVYTzoWiifaWBo.LbKjkHaEYcflCVdSDiaXPdeSnZquA = ynTfoKfozPExlkgidDzpkPNmatxBb.JPBQxjoRxbCjKCYpPonqxyLKCUHO;
				eQhPridDEKKfFdiVYTzoWiifaWBo.csdDcoRxMblcETGIzCXAoNKwiIkDA = ynTfoKfozPExlkgidDzpkPNmatxBb.tuqGmntIrErQAhrlXaMInEYNPABk;
				eQhPridDEKKfFdiVYTzoWiifaWBo.noDyXDUdlPKBMSkRvbqYasAXqAFaA = ynTfoKfozPExlkgidDzpkPNmatxBb.wNgdJWkbzGlbKPdkimftWNoTOBOH;
				eQhPridDEKKfFdiVYTzoWiifaWBo.uHkXbPenQtCEXhGoWMoJMQqwsSNMA = ynTfoKfozPExlkgidDzpkPNmatxBb.UYhHPpNnVWxSYEspxKOOraAFqiWr;
				eQhPridDEKKfFdiVYTzoWiifaWBo.IghITlQqAHYzeQDzcXCztBBmmcAl = ynTfoKfozPExlkgidDzpkPNmatxBb.sAjdsHjhDlydNhJmvBSlEPTeQQRtb;
				eQhPridDEKKfFdiVYTzoWiifaWBo.kUHHTIgTMDyMEXufsUvSHmaExSkL = ynTfoKfozPExlkgidDzpkPNmatxBb.sJJYTrxqqCBnONHpVTrbUvDhSrsq;
				eQhPridDEKKfFdiVYTzoWiifaWBo.fGuUukUdWRwxJbfpPdINQlKZVCQt = ynTfoKfozPExlkgidDzpkPNmatxBb.TKSlXXrapHpSocUbWYuNoGeAvlLl;
				eQhPridDEKKfFdiVYTzoWiifaWBo.DQsyboqTuputZkKyhNEOttVOTBJG = ynTfoKfozPExlkgidDzpkPNmatxBb.SLfCsMJeRjsDOoBdIvygtdngNpevA;
				eQhPridDEKKfFdiVYTzoWiifaWBo.STrqcXxJVXWmOzdWQDdtJmduqYaEb = ynTfoKfozPExlkgidDzpkPNmatxBb.ZJfUpCttcCvXukzpPLgxkmqGsDX;
				eQhPridDEKKfFdiVYTzoWiifaWBo.RjAiGdvCPfvtkTaCKkDzQpjuKUmN = ynTfoKfozPExlkgidDzpkPNmatxBb.QbWWWqDfCwsrFYIwDENWHYhXUTsBA;
				eQhPridDEKKfFdiVYTzoWiifaWBo.geYuFmevHVggNCfxsdAQIVPpwWDoA = ynTfoKfozPExlkgidDzpkPNmatxBb.wEWJeNudDoxMIBHsPNpLBrUiFwdi;
				eQhPridDEKKfFdiVYTzoWiifaWBo.yDgHnhhlyvEtcevEXbGbcSGoBwsvA = ynTfoKfozPExlkgidDzpkPNmatxBb.FZpEAgXPllhRbEoizirYCYJKuhJNA;
				eQhPridDEKKfFdiVYTzoWiifaWBo.BMKmLgTjByiExXMvsepaerGfBiXH = ynTfoKfozPExlkgidDzpkPNmatxBb.ZbOzjBqFYZVPgWGenibPJvnLimwv;
				eQhPridDEKKfFdiVYTzoWiifaWBo.IvQuNSqDypWdalUGYcleYiqEVPmd = ynTfoKfozPExlkgidDzpkPNmatxBb.fkBIwCtkqMAUoCWyZgkaGsGcprnP;
				eQhPridDEKKfFdiVYTzoWiifaWBo.extension = ynTfoKfozPExlkgidDzpkPNmatxBb.DtUUTohNtONEZGOrzSWbnhaxwVyF;
				ynTfoKfozPExlkgidDzpkPNmatxBb.hxQQtSUYqHmHfSmSevdfsROTCozBA();
				eQhPridDEKKfFdiVYTzoWiifaWBo.XrAbLZWFEwcAyuirOieMPGMErUhj();
				pfvHZfkVJHuFpTIaVPSjIEuhIisl.Add(eQhPridDEKKfFdiVYTzoWiifaWBo);
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
	}

	private void QbVAjVWFtjNLxYhAcdqihqDFeguS()
	{
		for (int i = 0; i < OeeHBecrguVtJOJkdEjZKpQgYwls; i++)
		{
			pfvHZfkVJHuFpTIaVPSjIEuhIisl[i]?.Update();
		}
	}

	private bool dYIuZYdgNaLwXFvIpZfHbBTeIqOe(iJNhZMfwcNtZpDlvPFwjcjTFYpKk P_0)
	{
		try
		{
			return P_0.YfufEGSapqNHkdiETLbVWQWZBEIG();
		}
		catch
		{
			return false;
		}
	}

	private IList<YnTfoKfozPExlkgidDzpkPNmatxBb> gwuqAFYNpjJGCopUdNidgJZgiWVg()
	{
		return mKGFJudopEcewqKpuxRtaCtIVgIn.GetJoysticks<YnTfoKfozPExlkgidDzpkPNmatxBb>();
	}

	private void ftbgfAiDvmSdIeusVzSkTpuhpzBV(int P_0, int P_1, List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_2, List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_3)
	{
		if (P_1 > 0)
		{
			P_3.Sort(EQhPridDEKKfFdiVYTzoWiifaWBo.bfSsVtsssSHIAIjxnhSiETrvlrDy);
		}
		if (P_0 > 0 && P_1 > 0)
		{
			xRxIiLagAyRqoXRhMeqWoRVnsVfg(P_1, P_3, P_0, P_2, TJbdTBCpXhbfktlyjegnXzRVsHrWA.exEStnnLxQgAFfOceYTIvndgAdCM.Exact);
			xRxIiLagAyRqoXRhMeqWoRVnsVfg(P_1, P_3, P_0, P_2, TJbdTBCpXhbfktlyjegnXzRVsHrWA.exEStnnLxQgAFfOceYTIvndgAdCM.Approximate);
		}
		pEdRcuYEfPeDfCbDzlhCjQTcTroNA(P_1, P_3, TJbdTBCpXhbfktlyjegnXzRVsHrWA.exEStnnLxQgAFfOceYTIvndgAdCM.Exact);
		pEdRcuYEfPeDfCbDzlhCjQTcTroNA(P_1, P_3, TJbdTBCpXhbfktlyjegnXzRVsHrWA.exEStnnLxQgAFfOceYTIvndgAdCM.Approximate);
		for (int i = 0; i < P_1; i++)
		{
			EQhPridDEKKfFdiVYTzoWiifaWBo eQhPridDEKKfFdiVYTzoWiifaWBo = P_3[i];
			if (eQhPridDEKKfFdiVYTzoWiifaWBo != null && eQhPridDEKKfFdiVYTzoWiifaWBo.inputManagerId < 0)
			{
				eQhPridDEKKfFdiVYTzoWiifaWBo.inputManagerId = UZDtQCJtoBAzOcKCFQAhAcWCSlRe(P_3);
				eQhPridDEKKfFdiVYTzoWiifaWBo.rewiredId = VbbCuMeRTNjQXExlYVBnBBhBxmfmB();
				yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(eQhPridDEKKfFdiVYTzoWiifaWBo);
			}
		}
		P_3.Sort(EQhPridDEKKfFdiVYTzoWiifaWBo.tcaSqFHHskanwDdVoIpQoXasFnou);
	}

	private void GyKaBpqsNjicYdUjpqfYSgHENAVTA(List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_0, int P_1, int P_2)
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

	private bool YOpkZHCZwdYZaaxJAJIvhEZXQJFg(List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_0, int P_1)
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

	private int UZDtQCJtoBAzOcKCFQAhAcWCSlRe(List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_0)
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

	private bool SOyWHlXMPORgpMASVXPztNAVPsaL(List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_0, int P_1)
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

	private void xRxIiLagAyRqoXRhMeqWoRVnsVfg(int P_0, List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_1, int P_2, List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_3, TJbdTBCpXhbfktlyjegnXzRVsHrWA.exEStnnLxQgAFfOceYTIvndgAdCM P_4)
	{
		int num = ((P_4 != TJbdTBCpXhbfktlyjegnXzRVsHrWA.exEStnnLxQgAFfOceYTIvndgAdCM.Exact) ? 1 : 2);
		for (int i = 0; i < P_0; i++)
		{
			EQhPridDEKKfFdiVYTzoWiifaWBo eQhPridDEKKfFdiVYTzoWiifaWBo = P_1[i];
			if (eQhPridDEKKfFdiVYTzoWiifaWBo == null || eQhPridDEKKfFdiVYTzoWiifaWBo.inputManagerId >= 0)
			{
				continue;
			}
			for (int j = 0; j < P_2; j++)
			{
				EQhPridDEKKfFdiVYTzoWiifaWBo eQhPridDEKKfFdiVYTzoWiifaWBo2 = P_3[j];
				if (eQhPridDEKKfFdiVYTzoWiifaWBo2 != null && !SOyWHlXMPORgpMASVXPztNAVPsaL(P_1, eQhPridDEKKfFdiVYTzoWiifaWBo2.rewiredId) && eQhPridDEKKfFdiVYTzoWiifaWBo.dRTWaRkWIkEMLraxMRHrbdGkCqGCA(eQhPridDEKKfFdiVYTzoWiifaWBo2) >= num)
				{
					eQhPridDEKKfFdiVYTzoWiifaWBo.FOuxuJsHWhCpyXhouIptKEIcQGsIA(eQhPridDEKKfFdiVYTzoWiifaWBo2);
					yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(eQhPridDEKKfFdiVYTzoWiifaWBo);
				}
			}
		}
	}

	private void pEdRcuYEfPeDfCbDzlhCjQTcTroNA(int P_0, List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_1, TJbdTBCpXhbfktlyjegnXzRVsHrWA.exEStnnLxQgAFfOceYTIvndgAdCM P_2)
	{
		for (int i = 0; i < P_0; i++)
		{
			EQhPridDEKKfFdiVYTzoWiifaWBo eQhPridDEKKfFdiVYTzoWiifaWBo = P_1[i];
			if (eQhPridDEKKfFdiVYTzoWiifaWBo == null || eQhPridDEKKfFdiVYTzoWiifaWBo.inputManagerId >= 0)
			{
				continue;
			}
			TJbdTBCpXhbfktlyjegnXzRVsHrWA.ZzLhWCBmdUYOmSHtQZkdQIXhgjli zzLhWCBmdUYOmSHtQZkdQIXhgjli = null;
			foreach (TJbdTBCpXhbfktlyjegnXzRVsHrWA.ZzLhWCBmdUYOmSHtQZkdQIXhgjli item in yOgOXMdWSZKgHOWSXqOzTssrAWni.JhAfrVJVWWlugTxWtItbSJXlMNCd(eQhPridDEKKfFdiVYTzoWiifaWBo, P_2))
			{
				if (!SOyWHlXMPORgpMASVXPztNAVPsaL(P_1, item.vLaDHvfmijzLwCtDfpDRfeHfsWbqc) && item.lICdlIDUnBRyGFLyltrkLDJRDJVO >= 0)
				{
					zzLhWCBmdUYOmSHtQZkdQIXhgjli = item;
					break;
				}
			}
			if (zzLhWCBmdUYOmSHtQZkdQIXhgjli != null)
			{
				int num = zzLhWCBmdUYOmSHtQZkdQIXhgjli.lICdlIDUnBRyGFLyltrkLDJRDJVO;
				if (!YOpkZHCZwdYZaaxJAJIvhEZXQJFg(P_1, num))
				{
					num = (zzLhWCBmdUYOmSHtQZkdQIXhgjli.lICdlIDUnBRyGFLyltrkLDJRDJVO = UZDtQCJtoBAzOcKCFQAhAcWCSlRe(P_1));
				}
				eQhPridDEKKfFdiVYTzoWiifaWBo.inputManagerId = num;
				eQhPridDEKKfFdiVYTzoWiifaWBo.rewiredId = zzLhWCBmdUYOmSHtQZkdQIXhgjli.vLaDHvfmijzLwCtDfpDRfeHfsWbqc;
				yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(eQhPridDEKKfFdiVYTzoWiifaWBo);
			}
		}
	}

	private void vKPnlrydRfdvHsSeMDWMWLWGxPmt()
	{
		IList<YnTfoKfozPExlkgidDzpkPNmatxBb> list = gwuqAFYNpjJGCopUdNidgJZgiWVg();
		vOupObHoGGqcZUAbdBHmnAnfgYrI(list);
		mosSJnjLnefWplKjFDNNyyTdOAaG = false;
	}

	private bool YkzrGmlCpVDGEtVFCmSTRQlVMnyR(IList<YnTfoKfozPExlkgidDzpkPNmatxBb> P_0)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && !XZGNKzJiapzbotXCZHcHFnignvZX(P_0[i].JPBQxjoRxbCjKCYpPonqxyLKCUHO))
			{
				return true;
			}
		}
		int count2 = pfvHZfkVJHuFpTIaVPSjIEuhIisl.Count;
		for (int j = 0; j < count2; j++)
		{
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[j] != null && !oFOctAJOKPlmQFgjyUKfDYCKvtwv(P_0, pfvHZfkVJHuFpTIaVPSjIEuhIisl[j].LbKjkHaEYcflCVdSDiaXPdeSnZquA))
			{
				return true;
			}
		}
		return false;
	}

	private bool XZGNKzJiapzbotXCZHcHFnignvZX(Guid P_0)
	{
		int count = pfvHZfkVJHuFpTIaVPSjIEuhIisl.Count;
		for (int i = 0; i < count; i++)
		{
			if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[i] != null && pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].LbKjkHaEYcflCVdSDiaXPdeSnZquA == P_0)
			{
				return true;
			}
		}
		return false;
	}

	private bool oFOctAJOKPlmQFgjyUKfDYCKvtwv(IList<YnTfoKfozPExlkgidDzpkPNmatxBb> P_0, Guid P_1)
	{
		int count = P_0.Count;
		for (int i = 0; i < count; i++)
		{
			if (P_0[i] != null && P_0[i].JPBQxjoRxbCjKCYpPonqxyLKCUHO == P_1)
			{
				return true;
			}
		}
		return false;
	}

	private void mZaUgDoxXIdSkCMllUmYWlPzEksd(List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_0, List<EQhPridDEKKfFdiVYTzoWiifaWBo> P_1, bool P_2)
	{
		if (P_0 == null)
		{
			return;
		}
		int num = P_0?.Count ?? 0;
		int num2 = P_1?.Count ?? 0;
		for (int i = 0; i < num; i++)
		{
			EQhPridDEKKfFdiVYTzoWiifaWBo eQhPridDEKKfFdiVYTzoWiifaWBo = P_0[i];
			if (eQhPridDEKKfFdiVYTzoWiifaWBo == null)
			{
				continue;
			}
			bool flag = false;
			if (P_1 != null)
			{
				for (int j = 0; j < num2; j++)
				{
					EQhPridDEKKfFdiVYTzoWiifaWBo eQhPridDEKKfFdiVYTzoWiifaWBo2 = P_1[j];
					if (eQhPridDEKKfFdiVYTzoWiifaWBo2 != null && eQhPridDEKKfFdiVYTzoWiifaWBo.LbKjkHaEYcflCVdSDiaXPdeSnZquA == eQhPridDEKKfFdiVYTzoWiifaWBo2.LbKjkHaEYcflCVdSDiaXPdeSnZquA)
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

	private void CTOpPdkFJUgxrzSfEfbZBihGShecA(EQhPridDEKKfFdiVYTzoWiifaWBo P_0, bool P_1)
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

	private void uuLlAFZlZpgBkHAqKoLlGqkTLpdC()
	{
		if (APoNWtXoeYHyrbDkDswtiTxkfkUL)
		{
			mosSJnjLnefWplKjFDNNyyTdOAaG = true;
		}
		SystemDeviceConnected();
	}
}
