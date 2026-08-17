using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Platforms;
using Rewired.Platforms.Custom;
using Rewired.Utils;

namespace Rewired.InputManagers
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
	internal class CustomInputManager : PlatformInputManager
	{
		private class KmjMmfyELXzeRNHGiatyYKKdIqBx : IInputManagerJoystick, IInputManagerJoystickPublic
		{
			private readonly InputSource twtzIKfuPcwLuTdbfRDPmfUOUduI;

			private readonly CustomInputSource uvqirFvSaQaSLJywHrfkGSZIjZIBA;

			private readonly Controller.Extension ieNssgrFeJiIpjsZwbsJdGwhvyJU;

			private int kniQQKPsrTyUTKqICYwwQKlUfoRf;

			private int nyPspetimSyUTqhIxJtYJkZCgjlM;

			private long? orGeZnHyIQImwrQbTiysYDyAvZUR;

			private int xmEXVfyENtoyjKLfjizpdurnqqpi;

			public Guid cUhzBTzwBmFECgMqAsRWDOSsbdVY;

			public string gLHfwCyqreiUZEYrUxDpSgvRFFht;

			public string QKGJzZtcEVRujqXgWpGOAarEKtHh;

			private int yLLlShtixVkbYMaIfuUMijmLCboKA;

			private int tpoonPbJmajQyAErVLnAOoXhqZqEb;

			private float[] qugobParkJGeDYgxggFADuHJrSREA;

			private bool[] QKMemPJOCQdfGcajCUROfzfnXvDE;

			private HardwareJoystickMap_InputManager yPbTGFEOQNqKOEHVnHaIUhnHjgYD;

			public CustomInputSource.Joystick LsybBclOJVPTDUJtdKjIwxCfcMse;

			private bool ulpWNMyBfTTmuiYwnRErPHSaPABK;

			private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> MzECUhxACfqrWuBRNBbBzuCqFsTG;

			public int RjAiGdvCPfvtkTaCKkDzQpjuKUmN
			{
				get
				{
					if (LsybBclOJVPTDUJtdKjIwxCfcMse == null)
					{
						return 0;
					}
					return LsybBclOJVPTDUJtdKjIwxCfcMse.buttonCount;
				}
			}

			public int STrqcXxJVXWmOzdWQDdtJmduqYaEb
			{
				get
				{
					if (LsybBclOJVPTDUJtdKjIwxCfcMse == null)
					{
						return 0;
					}
					return LsybBclOJVPTDUJtdKjIwxCfcMse.axisCount;
				}
			}

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
					string text = ((!string.IsNullOrEmpty(LsybBclOJVPTDUJtdKjIwxCfcMse.customName)) ? LsybBclOJVPTDUJtdKjIwxCfcMse.customName : gLHfwCyqreiUZEYrUxDpSgvRFFht);
					if (text == "Unknown Controller")
					{
						text = QKGJzZtcEVRujqXgWpGOAarEKtHh;
					}
					return text;
				}
			}

			[CustomObfuscation(rename = false)]
			public long? systemId => orGeZnHyIQImwrQbTiysYDyAvZUR;

			[CustomObfuscation(rename = false)]
			public int unityId => xmEXVfyENtoyjKLfjizpdurnqqpi;

			[CustomObfuscation(rename = false)]
			public Guid instanceGuid
			{
				get
				{
					if (!orGeZnHyIQImwrQbTiysYDyAvZUR.HasValue)
					{
						return Guid.Empty;
					}
					return MiscTools.CreateGuidHashSHA1(name + "_" + orGeZnHyIQImwrQbTiysYDyAvZUR);
				}
			}

			[CustomObfuscation(rename = false)]
			public Guid persistentGuid => instanceGuid;

			[CustomObfuscation(rename = false)]
			public Controller.Extension extension => ieNssgrFeJiIpjsZwbsJdGwhvyJU;

			[CustomObfuscation(rename = false)]
			public void SetVibration(float amount, int motorIndex)
			{
			}

			[CustomObfuscation(rename = false)]
			public void StopVibration()
			{
			}

			public KmjMmfyELXzeRNHGiatyYKKdIqBx(CustomInputSource P_0, long? P_1, int P_2, CustomInputSource.Joystick P_3, InputSource P_4, Controller.Extension P_5, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_6)
			{
				uvqirFvSaQaSLJywHrfkGSZIjZIBA = P_0;
				twtzIKfuPcwLuTdbfRDPmfUOUduI = P_4;
				orGeZnHyIQImwrQbTiysYDyAvZUR = P_1;
				LsybBclOJVPTDUJtdKjIwxCfcMse = P_3;
				xmEXVfyENtoyjKLfjizpdurnqqpi = P_2;
				ieNssgrFeJiIpjsZwbsJdGwhvyJU = P_5;
				MzECUhxACfqrWuBRNBbBzuCqFsTG = P_6;
				nyPspetimSyUTqhIxJtYJkZCgjlM = -1;
				kniQQKPsrTyUTKqICYwwQKlUfoRf = -1;
				kzmsoJcHfxzYjwlZofNXqivrtrbh();
				uxvVoENawKQUBgtDORfvcLmpPHIB();
				cUhzBTzwBmFECgMqAsRWDOSsbdVY = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.hardwareMapIdentifier.guid;
				gLHfwCyqreiUZEYrUxDpSgvRFFht = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.controllerName;
				qugobParkJGeDYgxggFADuHJrSREA = new float[yLLlShtixVkbYMaIfuUMijmLCboKA];
				QKMemPJOCQdfGcajCUROfzfnXvDE = new bool[tpoonPbJmajQyAErVLnAOoXhqZqEb];
				Update();
			}

			public void kzmsoJcHfxzYjwlZofNXqivrtrbh()
			{
				QKGJzZtcEVRujqXgWpGOAarEKtHh = LsybBclOJVPTDUJtdKjIwxCfcMse.deviceName;
			}

			[CustomObfuscation(rename = false)]
			public void Update()
			{
				if (LsybBclOJVPTDUJtdKjIwxCfcMse.isConnected)
				{
					dtkrhYOvGizZeJBgYiXXFyVpYjce();
					MGEvRBxTXmrzVAbFgmVqKAAmKjlj();
				}
			}

			public int dRTWaRkWIkEMLraxMRHrbdGkCqGCA(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0)
			{
				if (P_0.QKGJzZtcEVRujqXgWpGOAarEKtHh == QKGJzZtcEVRujqXgWpGOAarEKtHh && P_0.orGeZnHyIQImwrQbTiysYDyAvZUR == orGeZnHyIQImwrQbTiysYDyAvZUR)
				{
					return 2;
				}
				if (P_0.QKGJzZtcEVRujqXgWpGOAarEKtHh == QKGJzZtcEVRujqXgWpGOAarEKtHh)
				{
					return 1;
				}
				return 0;
			}

			private void ZsSGuyFeNhejoyHSIRMfnABaLlydA(BridgedControllerHWInfo P_0)
			{
				P_0.inputManagerSource = twtzIKfuPcwLuTdbfRDPmfUOUduI;
				P_0.inputSource = twtzIKfuPcwLuTdbfRDPmfUOUduI;
				P_0.hardwareIdentifier = gGionhosfhAUReDWKmiZOyIKcoAI();
				P_0.hardwareAxisCount = yLLlShtixVkbYMaIfuUMijmLCboKA;
				P_0.hardwareButtonCount = tpoonPbJmajQyAErVLnAOoXhqZqEb;
				P_0.hardwareHatCount = 0;
				P_0.hw_productName = QKGJzZtcEVRujqXgWpGOAarEKtHh;
				P_0.hw_supportsVibration = LsybBclOJVPTDUJtdKjIwxCfcMse.supportsVibration;
			}

			private void ZsSGuyFeNhejoyHSIRMfnABaLlydA(BridgedController P_0)
			{
				ZsSGuyFeNhejoyHSIRMfnABaLlydA((BridgedControllerHWInfo)P_0);
				P_0.sourceJoystick = this;
				P_0.gameHardwareMap = yPbTGFEOQNqKOEHVnHaIUhnHjgYD.ToGameHardwareControllerMap();
				P_0.instanceName = QKGJzZtcEVRujqXgWpGOAarEKtHh;
				P_0.productName = QKGJzZtcEVRujqXgWpGOAarEKtHh;
				P_0.isXInputDevice = false;
				P_0.axisCount = yLLlShtixVkbYMaIfuUMijmLCboKA;
				P_0.buttonCount = tpoonPbJmajQyAErVLnAOoXhqZqEb;
				P_0.controllerTypeGuid = cUhzBTzwBmFECgMqAsRWDOSsbdVY;
				P_0.customInputSource = uvqirFvSaQaSLJywHrfkGSZIjZIBA;
				P_0.controllerExtension = ieNssgrFeJiIpjsZwbsJdGwhvyJU;
			}

			[CustomObfuscation(rename = false)]
			public void FillData(ControllerDataUpdater dataUpdater)
			{
				if (yLLlShtixVkbYMaIfuUMijmLCboKA != dataUpdater.axisCount || tpoonPbJmajQyAErVLnAOoXhqZqEb != dataUpdater.buttonCount)
				{
					throw new Exception("This controller signature does not match the data object!");
				}
				for (int i = 0; i < yLLlShtixVkbYMaIfuUMijmLCboKA; i++)
				{
					dataUpdater.axisValues[i] = qugobParkJGeDYgxggFADuHJrSREA[i];
				}
				for (int j = 0; j < tpoonPbJmajQyAErVLnAOoXhqZqEb; j++)
				{
					dataUpdater.buttonValues[j] = QKMemPJOCQdfGcajCUROfzfnXvDE[j];
				}
				if (ulpWNMyBfTTmuiYwnRErPHSaPABK && !dataUpdater.hasReceivedInput)
				{
					dataUpdater.hasReceivedInput = true;
				}
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

			private void dtkrhYOvGizZeJBgYiXXFyVpYjce()
			{
				HardwareJoystickMap.Platform_Custom.Axis[] axes = ((HardwareJoystickMap.Platform_Custom)yPbTGFEOQNqKOEHVnHaIUhnHjgYD.map).Axes;
				if (axes == null)
				{
					return;
				}
				for (int i = 0; i < axes.Length; i++)
				{
					if (axes[i] != null)
					{
						if (i >= yLLlShtixVkbYMaIfuUMijmLCboKA)
						{
							throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
						}
						qugobParkJGeDYgxggFADuHJrSREA[i] = vKvknQyltTIOpMBpDfAHAfgnfcpAA(axes[i]);
						if (!ulpWNMyBfTTmuiYwnRErPHSaPABK && qugobParkJGeDYgxggFADuHJrSREA[i] != 0f)
						{
							ulpWNMyBfTTmuiYwnRErPHSaPABK = true;
						}
					}
				}
			}

			private void MGEvRBxTXmrzVAbFgmVqKAAmKjlj()
			{
				HardwareJoystickMap.Platform_Custom.Button[] buttons = ((HardwareJoystickMap.Platform_Custom)yPbTGFEOQNqKOEHVnHaIUhnHjgYD.map).Buttons;
				if (buttons == null)
				{
					return;
				}
				for (int i = 0; i < buttons.Length; i++)
				{
					if (i >= tpoonPbJmajQyAErVLnAOoXhqZqEb)
					{
						throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
					}
					QKMemPJOCQdfGcajCUROfzfnXvDE[i] = HJcRYlOYnieEhWqubAkAbPeBDumaA(buttons[i]);
					if (!ulpWNMyBfTTmuiYwnRErPHSaPABK && QKMemPJOCQdfGcajCUROfzfnXvDE[i])
					{
						ulpWNMyBfTTmuiYwnRErPHSaPABK = true;
					}
				}
			}

			private bool HJcRYlOYnieEhWqubAkAbPeBDumaA(HardwareJoystickMap.Platform_Custom.Button P_0)
			{
				if (P_0.sourceType == 0)
				{
					return HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0.sourceButton);
				}
				if (P_0.sourceType == 1)
				{
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
				return false;
			}

			private bool iVimKXCDtVYxZskYyfyfyiTuBJcc(float P_0, float P_1)
			{
				return MathTools.IsNear(P_1, P_0, 0.1f);
			}

			private float vKvknQyltTIOpMBpDfAHAfgnfcpAA(HardwareJoystickMap.Platform_Custom.Axis P_0)
			{
				if (P_0.sourceType == 1)
				{
					return vKvknQyltTIOpMBpDfAHAfgnfcpAA(P_0.sourceAxis);
				}
				if (P_0.sourceType == 0)
				{
					if (!HJcRYlOYnieEhWqubAkAbPeBDumaA(P_0.sourceButton))
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

			private float vKvknQyltTIOpMBpDfAHAfgnfcpAA(int P_0)
			{
				return LsybBclOJVPTDUJtdKjIwxCfcMse.GetAxisValue(P_0);
			}

			private bool HJcRYlOYnieEhWqubAkAbPeBDumaA(int P_0)
			{
				return LsybBclOJVPTDUJtdKjIwxCfcMse.GetButtonValue(P_0);
			}

			private void uxvVoENawKQUBgtDORfvcLmpPHIB()
			{
				yPbTGFEOQNqKOEHVnHaIUhnHjgYD = MzECUhxACfqrWuBRNBbBzuCqFsTG(wlgJJzqpwWlttiTkqCeBJnfMAizL());
				if (yPbTGFEOQNqKOEHVnHaIUhnHjgYD == null)
				{
					Logger.LogError("Default hardware map not found!");
					return;
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
					return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{ReInput.webplayerPlatform.ToString()}{twtzIKfuPcwLuTdbfRDPmfUOUduI.ToString()}{QKGJzZtcEVRujqXgWpGOAarEKtHh}");
				}
				return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{twtzIKfuPcwLuTdbfRDPmfUOUduI.ToString()}{QKGJzZtcEVRujqXgWpGOAarEKtHh}");
			}

			public static int XLDWYYvsgeAfEFpjEpYvIWmVwcbE(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0, KmjMmfyELXzeRNHGiatyYKKdIqBx P_1)
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

			public static int uSJfSSvTECbbVqebAzVeyXUpCeHd(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0, KmjMmfyELXzeRNHGiatyYKKdIqBx P_1)
			{
				if (P_0.orGeZnHyIQImwrQbTiysYDyAvZUR < P_1.orGeZnHyIQImwrQbTiysYDyAvZUR)
				{
					return -1;
				}
				if (P_0.orGeZnHyIQImwrQbTiysYDyAvZUR > P_1.orGeZnHyIQImwrQbTiysYDyAvZUR)
				{
					return 1;
				}
				return 0;
			}
		}

		private class uXwpZJyBcTFBiyOnNcViQRLSxmDx
		{
			public enum NEFqovlzdGEkcwPeIoXZpJBhQWBJ
			{
				Exact = 0,
				Approximate = 1
			}

			public class MggRSwUVUSfoCdutybFWQDHtDgUt
			{
				public int vLaDHvfmijzLwCtDfpDRfeHfsWbqc;

				public long? TGwSKVtHvYDggEqcgfOlEPytpNqMA;

				public string dguorTmaquKzUTHTlsJHWLmDYLpe;

				public int lICdlIDUnBRyGFLyltrkLDJRDJVO;

				public int tpoonPbJmajQyAErVLnAOoXhqZqEb;

				public int yLLlShtixVkbYMaIfuUMijmLCboKA;

				public MggRSwUVUSfoCdutybFWQDHtDgUt(int P_0, long? P_1, string P_2, int P_3, int P_4, int P_5)
				{
					vLaDHvfmijzLwCtDfpDRfeHfsWbqc = P_0;
					TGwSKVtHvYDggEqcgfOlEPytpNqMA = P_1;
					dguorTmaquKzUTHTlsJHWLmDYLpe = P_2;
					lICdlIDUnBRyGFLyltrkLDJRDJVO = P_3;
					tpoonPbJmajQyAErVLnAOoXhqZqEb = P_4;
					yLLlShtixVkbYMaIfuUMijmLCboKA = P_5;
				}

				public bool dRTWaRkWIkEMLraxMRHrbdGkCqGCA(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0, NEFqovlzdGEkcwPeIoXZpJBhQWBJ P_1)
				{
					if (P_0.rewiredId == vLaDHvfmijzLwCtDfpDRfeHfsWbqc)
					{
						return true;
					}
					if (P_0.RjAiGdvCPfvtkTaCKkDzQpjuKUmN != tpoonPbJmajQyAErVLnAOoXhqZqEb)
					{
						return false;
					}
					if (P_0.STrqcXxJVXWmOzdWQDdtJmduqYaEb != yLLlShtixVkbYMaIfuUMijmLCboKA)
					{
						return false;
					}
					switch (P_1)
					{
					case NEFqovlzdGEkcwPeIoXZpJBhQWBJ.Exact:
						if (TGwSKVtHvYDggEqcgfOlEPytpNqMA == P_0.systemId)
						{
							return dguorTmaquKzUTHTlsJHWLmDYLpe == P_0.QKGJzZtcEVRujqXgWpGOAarEKtHh;
						}
						return false;
					case NEFqovlzdGEkcwPeIoXZpJBhQWBJ.Approximate:
						return dguorTmaquKzUTHTlsJHWLmDYLpe == P_0.QKGJzZtcEVRujqXgWpGOAarEKtHh;
					default:
						throw new NotImplementedException();
					}
				}
			}

			private sealed class VAiwrfHLDPdVFnytijkRVTvsZtsi : IDisposable, IEnumerable, IEnumerator, IEnumerable<MggRSwUVUSfoCdutybFWQDHtDgUt>, IEnumerator<MggRSwUVUSfoCdutybFWQDHtDgUt>
			{
				private int RxAoyfYzYDsYonLGXsvUgwChukLk;

				private MggRSwUVUSfoCdutybFWQDHtDgUt VqEePGSMyrGKIqkWibsjeHcWPSIx;

				private int wLBvvzlDcbAEPyDfgnprolEZBsgN;

				public uXwpZJyBcTFBiyOnNcViQRLSxmDx TtytLoUfsgUyhsklaKccrnoMiiek;

				private KmjMmfyELXzeRNHGiatyYKKdIqBx bkgcrrWCRFOtLkjgQGzzaVDQJynbA;

				public KmjMmfyELXzeRNHGiatyYKKdIqBx VukUHiuaNKfVWgJZtDZBhZgYFbeXA;

				private NEFqovlzdGEkcwPeIoXZpJBhQWBJ WqfMvsqFdNcyCXobEzTCxnUdUxrF;

				public NEFqovlzdGEkcwPeIoXZpJBhQWBJ aGoqjFKjTqxYmTFXGFrTzlYRkhXy;

				private int YhcsJywchOwatkdZFPbKBklpfunV;

				private int fIMVaffCgsuIJcnrkMmGGKfPwwel;

				MggRSwUVUSfoCdutybFWQDHtDgUt IEnumerator<MggRSwUVUSfoCdutybFWQDHtDgUt>.Current
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
				public VAiwrfHLDPdVFnytijkRVTvsZtsi(int P_0)
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
					uXwpZJyBcTFBiyOnNcViQRLSxmDx ttytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
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
				IEnumerator<MggRSwUVUSfoCdutybFWQDHtDgUt> IEnumerable<MggRSwUVUSfoCdutybFWQDHtDgUt>.GetEnumerator()
				{
					VAiwrfHLDPdVFnytijkRVTvsZtsi vAiwrfHLDPdVFnytijkRVTvsZtsi;
					if (RxAoyfYzYDsYonLGXsvUgwChukLk == -2 && wLBvvzlDcbAEPyDfgnprolEZBsgN == Thread.CurrentThread.ManagedThreadId)
					{
						RxAoyfYzYDsYonLGXsvUgwChukLk = 0;
						vAiwrfHLDPdVFnytijkRVTvsZtsi = this;
					}
					else
					{
						vAiwrfHLDPdVFnytijkRVTvsZtsi = new VAiwrfHLDPdVFnytijkRVTvsZtsi(0);
						vAiwrfHLDPdVFnytijkRVTvsZtsi.TtytLoUfsgUyhsklaKccrnoMiiek = TtytLoUfsgUyhsklaKccrnoMiiek;
					}
					vAiwrfHLDPdVFnytijkRVTvsZtsi.bkgcrrWCRFOtLkjgQGzzaVDQJynbA = VukUHiuaNKfVWgJZtDZBhZgYFbeXA;
					vAiwrfHLDPdVFnytijkRVTvsZtsi.WqfMvsqFdNcyCXobEzTCxnUdUxrF = aGoqjFKjTqxYmTFXGFrTzlYRkhXy;
					return vAiwrfHLDPdVFnytijkRVTvsZtsi;
				}

				[DebuggerHidden]
				IEnumerator IEnumerable.GetEnumerator()
				{
					return ((IEnumerable<MggRSwUVUSfoCdutybFWQDHtDgUt>)this).GetEnumerator();
				}
			}

			private List<MggRSwUVUSfoCdutybFWQDHtDgUt> UwGmbGTujPCBtLRcPMDVnuFMDlrS;

			public int dtBwysZFYjgnHwWwaJGMOdwhhiAO => UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count;

			public uXwpZJyBcTFBiyOnNcViQRLSxmDx()
			{
				UwGmbGTujPCBtLRcPMDVnuFMDlrS = new List<MggRSwUVUSfoCdutybFWQDHtDgUt>();
			}

			public void CQYqTMmvttNbVynJkterllzoWgKe(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0)
			{
				if (P_0 == null)
				{
					return;
				}
				int count = UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count;
				for (int i = 0; i < count; i++)
				{
					if (UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].dRTWaRkWIkEMLraxMRHrbdGkCqGCA(P_0, NEFqovlzdGEkcwPeIoXZpJBhQWBJ.Exact))
					{
						UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].vLaDHvfmijzLwCtDfpDRfeHfsWbqc = P_0.rewiredId;
						UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].TGwSKVtHvYDggEqcgfOlEPytpNqMA = P_0.systemId;
						UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].dguorTmaquKzUTHTlsJHWLmDYLpe = P_0.QKGJzZtcEVRujqXgWpGOAarEKtHh;
						UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].lICdlIDUnBRyGFLyltrkLDJRDJVO = P_0.inputManagerId;
						UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].tpoonPbJmajQyAErVLnAOoXhqZqEb = P_0.RjAiGdvCPfvtkTaCKkDzQpjuKUmN;
						UwGmbGTujPCBtLRcPMDVnuFMDlrS[i].yLLlShtixVkbYMaIfuUMijmLCboKA = P_0.STrqcXxJVXWmOzdWQDdtJmduqYaEb;
						mIMjpeKiQlKAsiYxehIGdvJWmupB(P_0.rewiredId, i);
						return;
					}
				}
				UwGmbGTujPCBtLRcPMDVnuFMDlrS.Add(new MggRSwUVUSfoCdutybFWQDHtDgUt(P_0.rewiredId, P_0.systemId, P_0.QKGJzZtcEVRujqXgWpGOAarEKtHh, P_0.inputManagerId, P_0.RjAiGdvCPfvtkTaCKkDzQpjuKUmN, P_0.STrqcXxJVXWmOzdWQDdtJmduqYaEb));
				mIMjpeKiQlKAsiYxehIGdvJWmupB(P_0.rewiredId, UwGmbGTujPCBtLRcPMDVnuFMDlrS.Count - 1);
			}

			public bool fSBMVaLfQvgqXypKYGeLWMIbARbB(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0, NEFqovlzdGEkcwPeIoXZpJBhQWBJ P_1)
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

			public IEnumerable<MggRSwUVUSfoCdutybFWQDHtDgUt> JhAfrVJVWWlugTxWtItbSJXlMNCd(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0, NEFqovlzdGEkcwPeIoXZpJBhQWBJ P_1)
			{
				return new VAiwrfHLDPdVFnytijkRVTvsZtsi(-2)
				{
					TtytLoUfsgUyhsklaKccrnoMiiek = this,
					VukUHiuaNKfVWgJZtDZBhZgYFbeXA = P_0,
					aGoqjFKjTqxYmTFXGFrTzlYRkhXy = P_1
				};
			}

			public int lsWdiPZgHHfEfIiesCpnLAlcpBgUA(MggRSwUVUSfoCdutybFWQDHtDgUt P_0)
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

		private List<KmjMmfyELXzeRNHGiatyYKKdIqBx> pfvHZfkVJHuFpTIaVPSjIEuhIisl;

		private int OeeHBecrguVtJOJkdEjZKpQgYwls;

		private uXwpZJyBcTFBiyOnNcViQRLSxmDx yOgOXMdWSZKgHOWSXqOzTssrAWni;

		private UpdateLoopType IykDTZdiEUdjfaMPasESCgLSFroIc;

		private Action<int, ControllerDataUpdater> zxGQuXWGAufVLbkLjNzszbXATpQrA;

		private PlatformInputManager dLeGbmnDSSIBftSnJkmMPNabIURs;

		private CustomInputSource uvqirFvSaQaSLJywHrfkGSZIjZIBA;

		private bool mosSJnjLnefWplKjFDNNyyTdOAaG;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> MzECUhxACfqrWuBRNBbBzuCqFsTG;

		private Func<int> VbbCuMeRTNjQXExlYVBnBBhBxmfmB;

		[CustomObfuscation(rename = false)]
		public override int deviceCount => OeeHBecrguVtJOJkdEjZKpQgYwls;

		[CustomObfuscation(rename = false)]
		public override PlatformInputManager primaryInputManager => dLeGbmnDSSIBftSnJkmMPNabIURs;

		[CustomObfuscation(rename = false)]
		public override IInputSource inputSource => null;

		[CustomObfuscation(rename = false)]
		public override InputSource inputSourceType => uvqirFvSaQaSLJywHrfkGSZIjZIBA.twtzIKfuPcwLuTdbfRDPmfUOUduI;

		public CustomInputManager(CustomInputSource P_0, UpdateLoopSetting P_1, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_2, Func<int> P_3)
		{
			uvqirFvSaQaSLJywHrfkGSZIjZIBA = P_0;
			MzECUhxACfqrWuBRNBbBzuCqFsTG = P_2;
			VbbCuMeRTNjQXExlYVBnBBhBxmfmB = P_3;
			dLeGbmnDSSIBftSnJkmMPNabIURs = this;
			try
			{
				zxGQuXWGAufVLbkLjNzszbXATpQrA = UpdateControllerData;
				P_0.IjBZhVnpMDVJOoHiocMFmfzWUjXD += SystemDeviceConnected;
				P_0.bEQVXqVyYwXaKGcXFdnuHwwjmrcf += SystemDeviceDisconnected;
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
			yOgOXMdWSZKgHOWSXqOzTssrAWni = new uXwpZJyBcTFBiyOnNcViQRLSxmDx();
			pfvHZfkVJHuFpTIaVPSjIEuhIisl = new List<KmjMmfyELXzeRNHGiatyYKKdIqBx>();
			mosSJnjLnefWplKjFDNNyyTdOAaG = true;
		}

		[CustomObfuscation(rename = false)]
		public override void Update(UpdateLoopType updateLoop)
		{
			IykDTZdiEUdjfaMPasESCgLSFroIc = updateLoop;
			if (uvqirFvSaQaSLJywHrfkGSZIjZIBA.isReady)
			{
				uvqirFvSaQaSLJywHrfkGSZIjZIBA.Update();
				if (mosSJnjLnefWplKjFDNNyyTdOAaG)
				{
					vKPnlrydRfdvHsSeMDWMWLWGxPmt();
				}
				QbVAjVWFtjNLxYhAcdqihqDFeguS();
			}
		}

		[CustomObfuscation(rename = false)]
		public override void OnDestroy()
		{
			if (uvqirFvSaQaSLJywHrfkGSZIjZIBA != null)
			{
				uvqirFvSaQaSLJywHrfkGSZIjZIBA.Dispose();
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
			for (int i = 0; i < OeeHBecrguVtJOJkdEjZKpQgYwls; i++)
			{
				if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].inputManagerId == inputManagerId)
				{
					pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].FillData(data);
					return;
				}
			}
			Logger.LogError("Invalid joystick Id " + inputManagerId + "!");
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

		[CustomObfuscation(rename = false)]
		public override void SetUnityJoystickId(int joystickId, int unityJoystickIndex)
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

		private void vOupObHoGGqcZUAbdBHmnAnfgYrI(CustomInputSource.Joystick[] P_0)
		{
			int num = 0;
			List<KmjMmfyELXzeRNHGiatyYKKdIqBx> list = pfvHZfkVJHuFpTIaVPSjIEuhIisl;
			int oeeHBecrguVtJOJkdEjZKpQgYwls = OeeHBecrguVtJOJkdEjZKpQgYwls;
			pfvHZfkVJHuFpTIaVPSjIEuhIisl = new List<KmjMmfyELXzeRNHGiatyYKKdIqBx>();
			for (int i = 0; i < P_0.Length; i++)
			{
				if (P_0[i] != null)
				{
					KmjMmfyELXzeRNHGiatyYKKdIqBx item = new KmjMmfyELXzeRNHGiatyYKKdIqBx(uvqirFvSaQaSLJywHrfkGSZIjZIBA, P_0[i].systemId, P_0[i].unityId, P_0[i], uvqirFvSaQaSLJywHrfkGSZIjZIBA.twtzIKfuPcwLuTdbfRDPmfUOUduI, P_0[i].extension, MzECUhxACfqrWuBRNBbBzuCqFsTG);
					pfvHZfkVJHuFpTIaVPSjIEuhIisl.Add(item);
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
				pfvHZfkVJHuFpTIaVPSjIEuhIisl[i].Update();
			}
		}

		private void ftbgfAiDvmSdIeusVzSkTpuhpzBV(int P_0, int P_1, List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_2, List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_3)
		{
			if (P_1 > 0)
			{
				P_3.Sort(KmjMmfyELXzeRNHGiatyYKKdIqBx.uSJfSSvTECbbVqebAzVeyXUpCeHd);
			}
			if (P_0 > 0 && P_1 > 0)
			{
				xRxIiLagAyRqoXRhMeqWoRVnsVfg(P_1, P_3, P_0, P_2, uXwpZJyBcTFBiyOnNcViQRLSxmDx.NEFqovlzdGEkcwPeIoXZpJBhQWBJ.Exact);
				if (uvqirFvSaQaSLJywHrfkGSZIjZIBA.useApproximateMatching)
				{
					xRxIiLagAyRqoXRhMeqWoRVnsVfg(P_1, P_3, P_0, P_2, uXwpZJyBcTFBiyOnNcViQRLSxmDx.NEFqovlzdGEkcwPeIoXZpJBhQWBJ.Approximate);
				}
			}
			pEdRcuYEfPeDfCbDzlhCjQTcTroNA(P_1, P_3, uXwpZJyBcTFBiyOnNcViQRLSxmDx.NEFqovlzdGEkcwPeIoXZpJBhQWBJ.Exact);
			if (uvqirFvSaQaSLJywHrfkGSZIjZIBA.useApproximateMatching)
			{
				pEdRcuYEfPeDfCbDzlhCjQTcTroNA(P_1, P_3, uXwpZJyBcTFBiyOnNcViQRLSxmDx.NEFqovlzdGEkcwPeIoXZpJBhQWBJ.Approximate);
			}
			for (int i = 0; i < P_1; i++)
			{
				KmjMmfyELXzeRNHGiatyYKKdIqBx kmjMmfyELXzeRNHGiatyYKKdIqBx = P_3[i];
				if (kmjMmfyELXzeRNHGiatyYKKdIqBx != null && kmjMmfyELXzeRNHGiatyYKKdIqBx.inputManagerId < 0)
				{
					kmjMmfyELXzeRNHGiatyYKKdIqBx.inputManagerId = UZDtQCJtoBAzOcKCFQAhAcWCSlRe(P_3);
					kmjMmfyELXzeRNHGiatyYKKdIqBx.rewiredId = ReInput.GetNewJoystickId();
					yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(kmjMmfyELXzeRNHGiatyYKKdIqBx);
				}
			}
			P_3.Sort(KmjMmfyELXzeRNHGiatyYKKdIqBx.XLDWYYvsgeAfEFpjEpYvIWmVwcbE);
		}

		private void GyKaBpqsNjicYdUjpqfYSgHENAVTA(List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_0, int P_1, int P_2)
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

		private bool YOpkZHCZwdYZaaxJAJIvhEZXQJFg(List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_0, int P_1)
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

		private int UZDtQCJtoBAzOcKCFQAhAcWCSlRe(List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_0)
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

		private bool SOyWHlXMPORgpMASVXPztNAVPsaL(List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_0, int P_1)
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

		private void xRxIiLagAyRqoXRhMeqWoRVnsVfg(int P_0, List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_1, int P_2, List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_3, uXwpZJyBcTFBiyOnNcViQRLSxmDx.NEFqovlzdGEkcwPeIoXZpJBhQWBJ P_4)
		{
			int num = ((P_4 != uXwpZJyBcTFBiyOnNcViQRLSxmDx.NEFqovlzdGEkcwPeIoXZpJBhQWBJ.Exact) ? 1 : 2);
			for (int i = 0; i < P_0; i++)
			{
				KmjMmfyELXzeRNHGiatyYKKdIqBx kmjMmfyELXzeRNHGiatyYKKdIqBx = P_1[i];
				if (kmjMmfyELXzeRNHGiatyYKKdIqBx == null || kmjMmfyELXzeRNHGiatyYKKdIqBx.inputManagerId >= 0)
				{
					continue;
				}
				for (int j = 0; j < P_2; j++)
				{
					KmjMmfyELXzeRNHGiatyYKKdIqBx kmjMmfyELXzeRNHGiatyYKKdIqBx2 = P_3[j];
					if (kmjMmfyELXzeRNHGiatyYKKdIqBx2 != null && !SOyWHlXMPORgpMASVXPztNAVPsaL(P_1, kmjMmfyELXzeRNHGiatyYKKdIqBx2.rewiredId) && kmjMmfyELXzeRNHGiatyYKKdIqBx.dRTWaRkWIkEMLraxMRHrbdGkCqGCA(kmjMmfyELXzeRNHGiatyYKKdIqBx2) >= num)
					{
						kmjMmfyELXzeRNHGiatyYKKdIqBx.inputManagerId = kmjMmfyELXzeRNHGiatyYKKdIqBx2.inputManagerId;
						kmjMmfyELXzeRNHGiatyYKKdIqBx.rewiredId = kmjMmfyELXzeRNHGiatyYKKdIqBx2.rewiredId;
						yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(kmjMmfyELXzeRNHGiatyYKKdIqBx);
					}
				}
			}
		}

		private void pEdRcuYEfPeDfCbDzlhCjQTcTroNA(int P_0, List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_1, uXwpZJyBcTFBiyOnNcViQRLSxmDx.NEFqovlzdGEkcwPeIoXZpJBhQWBJ P_2)
		{
			for (int i = 0; i < P_0; i++)
			{
				KmjMmfyELXzeRNHGiatyYKKdIqBx kmjMmfyELXzeRNHGiatyYKKdIqBx = P_1[i];
				if (kmjMmfyELXzeRNHGiatyYKKdIqBx == null || kmjMmfyELXzeRNHGiatyYKKdIqBx.inputManagerId >= 0)
				{
					continue;
				}
				uXwpZJyBcTFBiyOnNcViQRLSxmDx.MggRSwUVUSfoCdutybFWQDHtDgUt mggRSwUVUSfoCdutybFWQDHtDgUt = null;
				foreach (uXwpZJyBcTFBiyOnNcViQRLSxmDx.MggRSwUVUSfoCdutybFWQDHtDgUt item in yOgOXMdWSZKgHOWSXqOzTssrAWni.JhAfrVJVWWlugTxWtItbSJXlMNCd(kmjMmfyELXzeRNHGiatyYKKdIqBx, P_2))
				{
					if (!SOyWHlXMPORgpMASVXPztNAVPsaL(P_1, item.vLaDHvfmijzLwCtDfpDRfeHfsWbqc) && item.lICdlIDUnBRyGFLyltrkLDJRDJVO >= 0)
					{
						mggRSwUVUSfoCdutybFWQDHtDgUt = item;
						break;
					}
				}
				if (mggRSwUVUSfoCdutybFWQDHtDgUt != null)
				{
					int num = mggRSwUVUSfoCdutybFWQDHtDgUt.lICdlIDUnBRyGFLyltrkLDJRDJVO;
					if (!YOpkZHCZwdYZaaxJAJIvhEZXQJFg(P_1, num))
					{
						num = (mggRSwUVUSfoCdutybFWQDHtDgUt.lICdlIDUnBRyGFLyltrkLDJRDJVO = UZDtQCJtoBAzOcKCFQAhAcWCSlRe(P_1));
					}
					kmjMmfyELXzeRNHGiatyYKKdIqBx.inputManagerId = num;
					kmjMmfyELXzeRNHGiatyYKKdIqBx.rewiredId = mggRSwUVUSfoCdutybFWQDHtDgUt.vLaDHvfmijzLwCtDfpDRfeHfsWbqc;
					yOgOXMdWSZKgHOWSXqOzTssrAWni.CQYqTMmvttNbVynJkterllzoWgKe(kmjMmfyELXzeRNHGiatyYKKdIqBx);
				}
			}
		}

		private void vKPnlrydRfdvHsSeMDWMWLWGxPmt()
		{
			CustomInputSource.Joystick[] array = uvqirFvSaQaSLJywHrfkGSZIjZIBA.VLkpLxcdERXBKIOsaWtDTHIzOMAS();
			if (YkzrGmlCpVDGEtVFCmSTRQlVMnyR(array))
			{
				vOupObHoGGqcZUAbdBHmnAnfgYrI(array);
			}
			mosSJnjLnefWplKjFDNNyyTdOAaG = false;
		}

		private bool YkzrGmlCpVDGEtVFCmSTRQlVMnyR(CustomInputSource.Joystick[] P_0)
		{
			int num = P_0.Length;
			int count = pfvHZfkVJHuFpTIaVPSjIEuhIisl.Count;
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
					if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[j] != null && systemId == pfvHZfkVJHuFpTIaVPSjIEuhIisl[j].systemId)
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
				if (pfvHZfkVJHuFpTIaVPSjIEuhIisl[k] == null)
				{
					continue;
				}
				long? systemId2 = pfvHZfkVJHuFpTIaVPSjIEuhIisl[k].systemId;
				bool flag2 = false;
				for (int l = 0; l < num; l++)
				{
					if (P_0[l] != null && systemId2 == P_0[l].systemId)
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

		private void mZaUgDoxXIdSkCMllUmYWlPzEksd(List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_0, List<KmjMmfyELXzeRNHGiatyYKKdIqBx> P_1, bool P_2)
		{
			if (P_0 == null)
			{
				return;
			}
			int num = P_0?.Count ?? 0;
			int num2 = P_1?.Count ?? 0;
			for (int i = 0; i < num; i++)
			{
				KmjMmfyELXzeRNHGiatyYKKdIqBx kmjMmfyELXzeRNHGiatyYKKdIqBx = P_0[i];
				if (kmjMmfyELXzeRNHGiatyYKKdIqBx == null)
				{
					continue;
				}
				bool flag = false;
				if (P_1 != null)
				{
					for (int j = 0; j < num2; j++)
					{
						KmjMmfyELXzeRNHGiatyYKKdIqBx kmjMmfyELXzeRNHGiatyYKKdIqBx2 = P_1[j];
						if (kmjMmfyELXzeRNHGiatyYKKdIqBx2 != null && kmjMmfyELXzeRNHGiatyYKKdIqBx.rewiredId == kmjMmfyELXzeRNHGiatyYKKdIqBx2.rewiredId)
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

		private void CTOpPdkFJUgxrzSfEfbZBihGShecA(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0, bool P_1)
		{
			if (P_1)
			{
				P_0.kzmsoJcHfxzYjwlZofNXqivrtrbh();
			}
			xvpcZQAidNYRofKPLPjklwQcsfJI(P_0, P_1);
		}

		private void xvpcZQAidNYRofKPLPjklwQcsfJI(KmjMmfyELXzeRNHGiatyYKKdIqBx P_0, bool P_1)
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
