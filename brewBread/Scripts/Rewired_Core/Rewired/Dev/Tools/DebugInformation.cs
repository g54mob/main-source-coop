using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Rewired.Platforms;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Dev.Tools
{
	public class DebugInformation : MonoBehaviour
	{
		private class DVVfNNDFzurhDkJneNxNbvLVtxey : IDisposable
		{
			public readonly bool SIbEMbCaPCQDoDhEDkwIKkLbsGAVE;

			public DVVfNNDFzurhDkJneNxNbvLVtxey(string P_0, string P_1, IDictionary<string, bool> P_2)
			{
				SIbEMbCaPCQDoDhEDkwIKkLbsGAVE = RoNcxaCIpsQDEiDKXuGKBzHixZbO(P_0, P_1, P_2);
				XuWlVJaGSgGynvtabUaxHWhMkCoT.bumJVxsmBzWqXCOVWtpuKFQKVVEX++;
			}

			private bool RoNcxaCIpsQDEiDKXuGKBzHixZbO(string P_0, string P_1, IDictionary<string, bool> P_2)
			{
				return PvSjxKtvtfAoDEuSfaqGIsYWbpyg(P_1, GUILayout.Toggle(tMofnUIacKSXXIkEFhicVncwVOuaA(P_1, P_2), new GUIContent(P_0, P_0), GetToggleStyle()), P_2);
			}

			private bool tMofnUIacKSXXIkEFhicVncwVOuaA(string P_0, IDictionary<string, bool> P_1)
			{
				if (!P_1.ContainsKey(P_0))
				{
					P_1.Add(P_0, value: false);
				}
				return P_1[P_0];
			}

			private bool PvSjxKtvtfAoDEuSfaqGIsYWbpyg(string P_0, bool P_1, IDictionary<string, bool> P_2)
			{
				if (!P_2.ContainsKey(P_0))
				{
					P_2.Add(P_0, P_1);
				}
				else
				{
					P_2[P_0] = P_1;
				}
				return P_1;
			}

			public void Dispose()
			{
				XuWlVJaGSgGynvtabUaxHWhMkCoT.bumJVxsmBzWqXCOVWtpuKFQKVVEX--;
			}
		}

		private static class XuWlVJaGSgGynvtabUaxHWhMkCoT
		{
			private static int wSuCXVMcyXbpFkHfHtuzalSRcWOHA;

			public static int bumJVxsmBzWqXCOVWtpuKFQKVVEX
			{
				get
				{
					return wSuCXVMcyXbpFkHfHtuzalSRcWOHA;
				}
				set
				{
					wSuCXVMcyXbpFkHfHtuzalSRcWOHA = Mathf.Max(0, b);
				}
			}
		}

		private static class MYGnSUSAaQtFziKUmXJatiOmTqIl
		{
			public static void WCXPgntJDTiddjAxONqIMioipqRF()
			{
				GUILayout.BeginHorizontal();
			}

			public static void FsgWioMqaJNnQxXfwvVTiHRLZvUO()
			{
				GUILayout.EndHorizontal();
			}

			public static void FjDqeyjGMFvZZFmBmuxxiedeFKEl()
			{
				GUILayout.BeginVertical();
			}

			public static void vmGtuTaNRTSPAkSSPpzttRiLylFH()
			{
				GUILayout.EndVertical();
			}

			public static void ceKtVCXucZkNlcoyCPTnTkiGfFxIA(string P_0, ETaKsbLjkpVsbjjMyjmzxMApFXyeA P_1)
			{
				GUILayout.Label(P_0, vdukSOoRCqDtWSjoOBiljZrzciRL());
			}

			public static void ePFaWFgPHYdtWhKnZOSDjWYuRGAiA(string P_0, string P_1)
			{
				GUILayout.Label(P_0 + ": " + P_1, vdukSOoRCqDtWSjoOBiljZrzciRL());
			}

			public static void WSveUXeypEWZJDNSahRtRTjNMCyxA(string P_0, AnimationCurve P_1)
			{
				GUILayout.Label(P_0 + ": Curves are not visualized by this tool.");
			}

			public static bool vCCfTBcxjUAGZBAhdNVGZezCRPEzb(string P_0, bool P_1)
			{
				return GUILayout.Toggle(P_1, P_0, vdukSOoRCqDtWSjoOBiljZrzciRL());
			}
		}

		private static class uwsyKZEfThZejBqZCNALGbzYnEZH
		{
			[CompilerGenerated]
			private static float frmPbnEClupjlsbikeLBBgqnbOXD;

			[CompilerGenerated]
			private static float dECefTKdnIbVwCrTBKDEIAnMNoLAb;

			public static float oJoTJoiFgjEQGAemCMXljviSoNsy
			{
				[CompilerGenerated]
				get
				{
					return frmPbnEClupjlsbikeLBBgqnbOXD;
				}
				[CompilerGenerated]
				set
				{
					frmPbnEClupjlsbikeLBBgqnbOXD = num;
				}
			}

			public static float pEzhssernoCbrvuiUPmcZeOTePUeb
			{
				[CompilerGenerated]
				get
				{
					return dECefTKdnIbVwCrTBKDEIAnMNoLAb;
				}
				[CompilerGenerated]
				set
				{
					dECefTKdnIbVwCrTBKDEIAnMNoLAb = num;
				}
			}
		}

		internal enum ETaKsbLjkpVsbjjMyjmzxMApFXyeA
		{
			None = 0,
			Info = 1,
			Warning = 2,
			Error = 3
		}

		[Serializable]
		private sealed class OqRHtpSSYiwkYagdZdksZHDnGrAs
		{
			public static readonly OqRHtpSSYiwkYagdZdksZHDnGrAs _003C_003E9 = new OqRHtpSSYiwkYagdZdksZHDnGrAs();

			public static Comparison<InputAction> _003C_003E9__16_0;

			internal int rVAVCLJjYMLGKqJvrnawIQGyvGae(InputAction P_0, InputAction P_1)
			{
				return P_0.name.CompareTo(P_1.name);
			}
		}

		private sealed class qtKSRsHcSiEQSicelcJLDCiDHQQHB
		{
			public InputCategory NVmskMpjRYGzbeUsEGkucwhVxiFeb;

			internal bool lphumxDPrbNKkMNnrrRHjRAGaRiU(InputAction P_0)
			{
				return P_0.categoryId == NVmskMpjRYGzbeUsEGkucwhVxiFeb.id;
			}
		}

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int _fontSize = 13;

		private static DebugInformation LDnMtBpByMhNcmNpPzdzLRPijZRi;

		private IDictionary<string, bool> oDmBmPQLoxXQzbaerNrgfDDlZXeO = new Dictionary<string, bool>();

		private static Vector2 UQZJCuJGxHJrfdnDaUsrsVUWxrXF;

		private const string zejTXclKnIceNkDsvTFRRmfwCIbw = "Rewired_DebugInformation";

		private const string BGcttrWoyUbzkTKIoVUZXKuHCfdi = "Rewired Debug Information";

		private const int xTAQlNWzWhTHAEwdAiesHKzkhuvK = 20;

		[CustomObfuscation(rename = false)]
		private void OnEnable()
		{
			LDnMtBpByMhNcmNpPzdzLRPijZRi = this;
			if (oDmBmPQLoxXQzbaerNrgfDDlZXeO.Count == 0)
			{
				oDmBmPQLoxXQzbaerNrgfDDlZXeO.Add("Rewired_DebugInformation", value: true);
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnDisable()
		{
			if (LDnMtBpByMhNcmNpPzdzLRPijZRi == this)
			{
				LDnMtBpByMhNcmNpPzdzLRPijZRi = null;
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnGUI()
		{
			XuWlVJaGSgGynvtabUaxHWhMkCoT.bumJVxsmBzWqXCOVWtpuKFQKVVEX = 0;
			GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height));
			UQZJCuJGxHJrfdnDaUsrsVUWxrXF = GUILayout.BeginScrollView(UQZJCuJGxHJrfdnDaUsrsVUWxrXF, GUILayout.ExpandWidth(expand: true), GUILayout.ExpandHeight(expand: true));
			DrawDebugInformation(enabled: true, oDmBmPQLoxXQzbaerNrgfDDlZXeO);
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		public static void DrawDebugInformation(bool enabled, IDictionary<string, bool> foldouts)
		{
			bool num = GUI.enabled;
			if (!ReInput.isReady || !enabled)
			{
				GUI.enabled = false;
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.WCXPgntJDTiddjAxONqIMioipqRF();
			GUILayout.FlexibleSpace();
			MYGnSUSAaQtFziKUmXJatiOmTqIl.FsgWioMqaJNnQxXfwvVTiHRLZvUO();
			Rect lastRect = GUILayoutUtility.GetLastRect();
			float num2 = lastRect.width / 3f;
			uwsyKZEfThZejBqZCNALGbzYnEZH.oJoTJoiFgjEQGAemCMXljviSoNsy = lastRect.width - num2;
			uwsyKZEfThZejBqZCNALGbzYnEZH.pEzhssernoCbrvuiUPmcZeOTePUeb = num2;
			HBLIxXmQsEqCGbwHsbZPIgHBLNJN(enabled, foldouts);
			GUI.enabled = num;
			uwsyKZEfThZejBqZCNALGbzYnEZH.oJoTJoiFgjEQGAemCMXljviSoNsy = 0f;
			uwsyKZEfThZejBqZCNALGbzYnEZH.pEzhssernoCbrvuiUPmcZeOTePUeb = 0f;
		}

		private static void HBLIxXmQsEqCGbwHsbZPIgHBLNJN(bool P_0, IDictionary<string, bool> P_1)
		{
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Rewired Debug Information", "Rewired_DebugInformation", P_1);
			if (!ReInput.isReady || !P_0)
			{
				GUILayout.Label("There is no active Rewired Input Manager in the scene.");
			}
			else
			{
				if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					return;
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Rewired Version", ReInput.programVersion);
				bool flag = ReInput.configuration.disableNativeInput;
				if (!flag && (ReInput.currentPlatform == Platform.Windows || ReInput.currentPlatform == Platform.OSX) && ReInput.primaryInputManager.inputSourceType == InputSource.Fallback)
				{
					flag = true;
				}
				if (flag)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ceKtVCXucZkNlcoyCPTnTkiGfFxIA("Native input is disabled. Many special features are unavailable without native input.", ETaKsbLjkpVsbjjMyjmzxMApFXyeA.Warning);
				}
				tMJFOulKhbFlWbYExdRJblvCMNYJc(P_1, "Rewired_DebugInformation");
				string text = "Rewired_DebugInformation_controllers";
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Controllers", text, P_1);
				if (dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					ZRFWzqTHPcCUIRJfyTkyqObHhlTN(ReInput.controllers.Joysticks, P_1, text);
					XOYcttfLLJTdUmGKuBWrZxzvVJyL(ReInput.controllers.CustomControllers, P_1, text);
					EnRXZfjXYEpACIYFFttRwtVbpvXK(P_1, "Rewired_DebugInformation");
					uqZODDkhZbbRlgxgtHRDkGTZThBWA(P_1, "Rewired_DebugInformation");
				}
				return;
			}
		}

		private static void tMJFOulKhbFlWbYExdRJblvCMNYJc(IDictionary<string, bool> P_0, string P_1)
		{
			string text = P_1 + "_players";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Players (" + ReInput.players.allPlayerCount + ")", text, P_0);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				int playerCount = ReInput.players.playerCount;
				for (int i = 0; i < playerCount; i++)
				{
					prSMLJESBSuNxXtrAxMZAVQaRUPR(ReInput.players.GetPlayer(i), i, P_0, text);
				}
				prSMLJESBSuNxXtrAxMZAVQaRUPR(ReInput.players.SystemPlayer, -1, P_0, text);
			}
		}

		private static void ZRFWzqTHPcCUIRJfyTkyqObHhlTN(IList<Joystick> P_0, IDictionary<string, bool> P_1, string P_2)
		{
			int num = P_0?.Count ?? 0;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Joysticks (" + num + ")", P_2 + "_joysticks", P_1);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				Joystick joystick = P_0[i];
				string text = P_2 + "_joystick" + joystick.id;
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey(i + ": " + ((joystick.name == "Unknown Controller") ? joystick.hardwareName : joystick.name), text, P_1);
				if (!dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					continue;
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id (unique id)", joystick.id.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", joystick.name);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Hardware Name", joystick.hardwareName);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Is Recognized", (joystick.hardwareTypeGuid != Guid.Empty).ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Enabled", joystick.enabled.ToString());
				string text2 = string.Empty;
				for (int j = 0; j < ReInput.players.allPlayerCount; j++)
				{
					Player player = ReInput.players.AllPlayers[j];
					if (ReInput.controllers.IsJoystickAssignedToPlayer(joystick.id, player.id))
					{
						if (text2 != string.Empty)
						{
							text2 += ", ";
						}
						text2 += ((player.id == 9999999) ? "System" : player.id.ToString());
					}
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Assigned to Players", (!string.IsNullOrEmpty(text2)) ? text2 : "None");
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("System Id", joystick.systemId.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Unity Id", ReInput.usingUnityInput ? joystick.unityId.ToString() : "--");
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Hardware Type Guid", joystick.hardwareTypeGuid.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Hardware Identifier", joystick.hardwareIdentifier);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Device Instance Guid", joystick.deviceInstanceGuid.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Tag", joystick.tag);
				RMHSqwwxQzTPiUaDHehaaASrIjcf(joystick.Axes, P_1, text);
				HHvWEFoXatouJjdkdKLdZQsDyyaC(joystick.Buttons, ControllerType.Joystick, P_1, text);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis2D Count", joystick.axis2DCount.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Hat Count", joystick.hatCount.ToString());
				hfhhpbCxyQyZNNBgDlvpbLdKWfGt(joystick, P_1, text);
				CalibrationMap calibrationMap = joystick.calibrationMap;
				using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey3 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Calibration Map", text + "_calibrationMap", P_1))
				{
					if (dVVfNNDFzurhDkJneNxNbvLVtxey3.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
					{
						int axisCount = calibrationMap.axisCount;
						for (int k = 0; k < axisCount; k++)
						{
							AxisCalibration axisCalibration = calibrationMap.Axes[k];
							using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey4 = new DVVfNNDFzurhDkJneNxNbvLVtxey(k + ": Axis Calibration (" + (axisCalibration.enabled ? "Enabled" : "Disabled") + ")", text + "_AxisCalibration" + k, P_1);
							if (dVVfNNDFzurhDkJneNxNbvLVtxey4.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
							{
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Enabled", axisCalibration.enabled.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Apply Range Calibration", axisCalibration.applyRangeCalibration.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Calibrated Max", axisCalibration.calibratedMax.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Calibrated Min", axisCalibration.calibratedMin.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Calibrated Zero", axisCalibration.calibratedZero.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Dead Zone", axisCalibration.deadZone.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Invert", axisCalibration.invert.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Sensitivity Type", axisCalibration.sensitivityType.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Sensitivity", axisCalibration.sensitivity.ToString());
								if (axisCalibration.sensitivityCurve != null)
								{
									bool num2 = GUI.enabled;
									GUI.enabled = false;
									MYGnSUSAaQtFziKUmXJatiOmTqIl.WSveUXeypEWZJDNSahRtRTjNMCyxA("Sensitivity Curve", axisCalibration.sensitivityCurve);
									GUI.enabled = num2;
								}
								else
								{
									MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Sensitivity Curve", "--");
								}
							}
						}
					}
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Supports Vibration", joystick.supportsVibration.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Vibration Motor Count", joystick.vibrationMotorCount.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Has Extension", (joystick.extension != null).ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Extension Type", (joystick.extension != null) ? joystick.extension.GetType().Name : "--");
				QgMEERdOSmbdxPImGZrMoVitwDZQ(joystick, P_1, text);
			}
		}

		private static void EnRXZfjXYEpACIYFFttRwtVbpvXK(IDictionary<string, bool> P_0, string P_1)
		{
			string text = P_1 + "_mouse";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Mouse", text, P_0);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			Mouse mouse = ReInput.controllers.Mouse;
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Enabled", mouse.enabled.ToString());
			string text2 = string.Empty;
			for (int i = 0; i < ReInput.players.allPlayerCount; i++)
			{
				Player player = ReInput.players.AllPlayers[i];
				if (player.controllers.hasMouse)
				{
					if (text2 != string.Empty)
					{
						text2 += ", ";
					}
					text2 += ((player.id == 9999999) ? "System" : player.id.ToString());
				}
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Assigned to Players", (!string.IsNullOrEmpty(text2)) ? text2 : "None");
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Screen Position", mouse.screenPosition.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Screen Position Prev", mouse.screenPositionPrev.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Screen Position Delta", mouse.screenPositionDelta.ToString());
			RMHSqwwxQzTPiUaDHehaaASrIjcf(mouse.Axes, P_0, text);
			HHvWEFoXatouJjdkdKLdZQsDyyaC(mouse.Buttons, ControllerType.Mouse, P_0, text);
			hfhhpbCxyQyZNNBgDlvpbLdKWfGt(mouse, P_0, text);
			QgMEERdOSmbdxPImGZrMoVitwDZQ(mouse, P_0, text);
		}

		private static void uqZODDkhZbbRlgxgtHRDkGTZThBWA(IDictionary<string, bool> P_0, string P_1)
		{
			string text = P_1 + "_keyboard";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Keyboard", text, P_0);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			Keyboard keyboard = ReInput.controllers.Keyboard;
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Enabled", keyboard.enabled.ToString());
			string text2 = string.Empty;
			for (int i = 0; i < ReInput.players.allPlayerCount; i++)
			{
				Player player = ReInput.players.AllPlayers[i];
				if (player.controllers.hasKeyboard)
				{
					if (text2 != string.Empty)
					{
						text2 += ", ";
					}
					text2 += ((player.id == 9999999) ? "System" : player.id.ToString());
				}
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Assigned to Players", (!string.IsNullOrEmpty(text2)) ? text2 : "None");
			HHvWEFoXatouJjdkdKLdZQsDyyaC(keyboard.Buttons, ControllerType.Keyboard, P_0, text);
			hfhhpbCxyQyZNNBgDlvpbLdKWfGt(keyboard, P_0, text);
			QgMEERdOSmbdxPImGZrMoVitwDZQ(keyboard, P_0, text);
		}

		private static void XOYcttfLLJTdUmGKuBWrZxzvVJyL(IList<CustomController> P_0, IDictionary<string, bool> P_1, string P_2)
		{
			int num = P_0?.Count ?? 0;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Custom Controllers (" + num + ")", P_2 + "_customControllers", P_1);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				CustomController customController = P_0[i];
				string text = P_2 + "_customController" + customController.id;
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey(i + ": " + customController.name, text, P_1);
				if (!dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					continue;
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id", customController.id.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", customController.name);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Hardware Name", customController.hardwareName);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Tag", customController.tag);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Hardware Identifier", customController.hardwareIdentifier);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Enabled", customController.enabled.ToString());
				string text2 = string.Empty;
				for (int j = 0; j < ReInput.players.allPlayerCount; j++)
				{
					Player player = ReInput.players.AllPlayers[j];
					if (ReInput.controllers.IsCustomControllerAssignedToPlayer(customController.id, player.id))
					{
						if (text2 != string.Empty)
						{
							text2 += ", ";
						}
						text2 += ((player.id == 9999999) ? "System" : player.id.ToString());
					}
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Assigned to Players", (!string.IsNullOrEmpty(text2)) ? text2 : "None");
				RMHSqwwxQzTPiUaDHehaaASrIjcf(customController.Axes, P_1, text);
				HHvWEFoXatouJjdkdKLdZQsDyyaC(customController.Buttons, ControllerType.Custom, P_1, text);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis2D Count", customController.axis2DCount.ToString());
				using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey3 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Element Identifiers", text + "_elementIdentifiers", P_1))
				{
					if (dVVfNNDFzurhDkJneNxNbvLVtxey3.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
					{
						int num2 = ((customController.AxisElementIdentifiers != null) ? customController.AxisElementIdentifiers.Count : 0);
						using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey4 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Axis Element Identifiers (" + num2 + ")", text + "_axisEIs", P_1))
						{
							if (dVVfNNDFzurhDkJneNxNbvLVtxey4.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
							{
								for (int k = 0; k < num2; k++)
								{
									ControllerElementIdentifier controllerElementIdentifier = customController.AxisElementIdentifiers[k];
									using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey5 = new DVVfNNDFzurhDkJneNxNbvLVtxey(k + ": " + controllerElementIdentifier.name + " (id: " + controllerElementIdentifier.id + ")", text + "_AxisEI" + k + "_" + controllerElementIdentifier.name, P_1);
									if (dVVfNNDFzurhDkJneNxNbvLVtxey5.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
									{
										MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id", controllerElementIdentifier.id.ToString());
										MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", controllerElementIdentifier.name);
									}
								}
							}
						}
						num2 = ((customController.ButtonElementIdentifiers != null) ? customController.ButtonElementIdentifiers.Count : 0);
						using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey6 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Button Element Identifiers (" + num2 + ")", text + "_buttonEIs", P_1);
						if (dVVfNNDFzurhDkJneNxNbvLVtxey6.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
						{
							for (int l = 0; l < num2; l++)
							{
								ControllerElementIdentifier controllerElementIdentifier2 = customController.ButtonElementIdentifiers[l];
								using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey7 = new DVVfNNDFzurhDkJneNxNbvLVtxey(l + ": " + controllerElementIdentifier2.name + " (id: " + controllerElementIdentifier2.id + ")", text + "_ButtonEI" + l + "_" + controllerElementIdentifier2.name, P_1);
								if (dVVfNNDFzurhDkJneNxNbvLVtxey7.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
								{
									MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id", controllerElementIdentifier2.id.ToString());
									MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", controllerElementIdentifier2.name);
								}
							}
						}
					}
				}
				CalibrationMap calibrationMap = customController.calibrationMap;
				using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey8 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Calibration Map", text + "_calibrationMap", P_1))
				{
					if (dVVfNNDFzurhDkJneNxNbvLVtxey8.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
					{
						int num2 = calibrationMap.axisCount;
						for (int m = 0; m < num2; m++)
						{
							AxisCalibration axisCalibration = calibrationMap.Axes[m];
							using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey9 = new DVVfNNDFzurhDkJneNxNbvLVtxey(m + ": Axis Calibration (" + (axisCalibration.enabled ? "Enabled" : "Disabled") + ")", text + "_AxisCalibration" + m, P_1);
							if (dVVfNNDFzurhDkJneNxNbvLVtxey9.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
							{
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Enabled", axisCalibration.enabled.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Apply Range Calibration", axisCalibration.applyRangeCalibration.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Calibrated Max", axisCalibration.calibratedMax.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Calibrated Min", axisCalibration.calibratedMin.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Calibrated Zero", axisCalibration.calibratedZero.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Dead Zone", axisCalibration.deadZone.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Invert", axisCalibration.invert.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Sensitivity Type", axisCalibration.sensitivityType.ToString());
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Sensitivity", axisCalibration.sensitivity.ToString());
								if (axisCalibration.sensitivityCurve != null)
								{
									bool num3 = GUI.enabled;
									GUI.enabled = false;
									MYGnSUSAaQtFziKUmXJatiOmTqIl.WSveUXeypEWZJDNSahRtRTjNMCyxA("Sensitivity Curve", axisCalibration.sensitivityCurve);
									GUI.enabled = num3;
								}
								else
								{
									MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Sensitivity Curve", "--");
								}
							}
						}
					}
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Has Extension", (customController.extension != null).ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Extension Type", (customController.extension != null) ? customController.extension.GetType().Name : "--");
				QgMEERdOSmbdxPImGZrMoVitwDZQ(customController, P_1, text);
			}
		}

		private static void prSMLJESBSuNxXtrAxMZAVQaRUPR(Player P_0, int P_1, IDictionary<string, bool> P_2, string P_3)
		{
			string text = P_3 + "_player" + P_0.id;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey((P_0.id == 9999999) ? "System Player" : (P_1 + ": " + P_0.name), text, P_2);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Player Id", P_0.id.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", P_0.name);
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Descriptive Name", P_0.descriptiveName);
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Is Playing", P_0.isPlaying.ToString());
			using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Controllers", text + "_controllers", P_2))
			{
				if (dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					Player.ControllerHelper controllers = P_0.controllers;
					ZRFWzqTHPcCUIRJfyTkyqObHhlTN(controllers.Joysticks, P_2, text);
					XOYcttfLLJTdUmGKuBWrZxzvVJyL(controllers.CustomControllers, P_2, text);
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Has Mouse", controllers.hasMouse.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Has Keyboard", controllers.hasKeyboard.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Exclude From Controller Auto Assignment", controllers.excludeFromControllerAutoAssignment.ToString());
				}
			}
			string text2 = text + "_controllerMaps";
			using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey3 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Controller Maps", text2, P_2))
			{
				if (dVVfNNDFzurhDkJneNxNbvLVtxey3.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					zeHWSRwarfRHvTnPJYxopYXwcDjB(ControllerType.Keyboard, P_0.controllers.maps.GetMaps<KeyboardMap>(0), "Keyboard Maps", P_2, text2 + "_keyboard");
					zeHWSRwarfRHvTnPJYxopYXwcDjB(ControllerType.Mouse, P_0.controllers.maps.GetMaps<MouseMap>(0), "Mouse Maps", P_2, text2 + "_mouse");
					string text3 = text2 + "_joystickMaps";
					using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey4 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Joysticks (" + P_0.controllers.joystickCount + ")", text3, P_2))
					{
						if (dVVfNNDFzurhDkJneNxNbvLVtxey4.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
						{
							for (int i = 0; i < P_0.controllers.joystickCount; i++)
							{
								Joystick joystick = P_0.controllers.Joysticks[i];
								IList<JoystickMap> maps = P_0.controllers.maps.GetMaps<JoystickMap>(joystick.id);
								text3 = text3 + "_joystickId" + joystick.id;
								zeHWSRwarfRHvTnPJYxopYXwcDjB(ControllerType.Joystick, maps, (joystick.name != "Unknown Controller") ? joystick.name : joystick.hardwareName, P_2, text3);
							}
						}
					}
					text3 = text2 + "_customControllerMaps";
					using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey5 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Custom Controllers (" + P_0.controllers.customControllerCount + ")", text3, P_2);
					if (dVVfNNDFzurhDkJneNxNbvLVtxey5.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
					{
						for (int j = 0; j < P_0.controllers.customControllerCount; j++)
						{
							CustomController customController = P_0.controllers.CustomControllers[j];
							IList<CustomControllerMap> maps2 = P_0.controllers.maps.GetMaps<CustomControllerMap>(customController.id);
							text3 = text3 + "_customControllerId" + customController.id;
							zeHWSRwarfRHvTnPJYxopYXwcDjB(ControllerType.Custom, maps2, customController.name, P_2, text3);
						}
					}
				}
			}
			text2 = text + "_controllerMapLayoutManager";
			using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey6 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Layout Manager", text2, P_2))
			{
				if (dVVfNNDFzurhDkJneNxNbvLVtxey6.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					AWoZPdVOglUJfkSAHldBCDbzDoeDA(P_0.controllers.maps.layoutManager, P_2, text2);
				}
			}
			text2 = text + "_controllerMapEnabler";
			using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey7 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Map Enabler", text2, P_2))
			{
				if (dVVfNNDFzurhDkJneNxNbvLVtxey7.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					NCrvFKjUIjqvpHirCacODaPSdWxEb(P_0.controllers.maps.mapEnabler, P_2, text2);
				}
			}
			text2 = text + "_inputBehaviors";
			mHEHEvclwRmidohTInsyJecVCuaM(P_0.controllers.maps.InputBehaviors, P_2, text2);
			text2 = text + "_actions";
			List<InputAction> list = new List<InputAction>(ReInput.mapping.Actions);
			list.Sort(OqRHtpSSYiwkYagdZdksZHDnGrAs._003C_003E9.rVAVCLJjYMLGKqJvrnawIQGyvGae);
			IList<InputCategory> actionCategories = ReInput.mapping.ActionCategories;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey8 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Actions (" + list.Count + ")", text2, P_2);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey8.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int k = 0; k < actionCategories.Count; k++)
			{
				qtKSRsHcSiEQSicelcJLDCiDHQQHB qtKSRsHcSiEQSicelcJLDCiDHQQHB2 = new qtKSRsHcSiEQSicelcJLDCiDHQQHB();
				qtKSRsHcSiEQSicelcJLDCiDHQQHB2.NVmskMpjRYGzbeUsEGkucwhVxiFeb = actionCategories[k];
				string text4 = text2 + "_actionCat" + qtKSRsHcSiEQSicelcJLDCiDHQQHB2.NVmskMpjRYGzbeUsEGkucwhVxiFeb.id;
				int num = ListTools.Count(list, qtKSRsHcSiEQSicelcJLDCiDHQQHB2.lphumxDPrbNKkMNnrrRHjRAGaRiU);
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey9 = new DVVfNNDFzurhDkJneNxNbvLVtxey("id " + qtKSRsHcSiEQSicelcJLDCiDHQQHB2.NVmskMpjRYGzbeUsEGkucwhVxiFeb.id + ": " + qtKSRsHcSiEQSicelcJLDCiDHQQHB2.NVmskMpjRYGzbeUsEGkucwhVxiFeb.name + " (" + num + ")", text4, P_2);
				if (!dVVfNNDFzurhDkJneNxNbvLVtxey9.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					continue;
				}
				for (int l = 0; l < list.Count; l++)
				{
					InputAction inputAction = list[l];
					if (inputAction.categoryId != qtKSRsHcSiEQSicelcJLDCiDHQQHB2.NVmskMpjRYGzbeUsEGkucwhVxiFeb.id)
					{
						continue;
					}
					string text5 = text4 + "_actionId" + inputAction.id;
					using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey10 = new DVVfNNDFzurhDkJneNxNbvLVtxey("id " + inputAction.id + ": " + inputAction.name + ": " + P_0.GetAxis(inputAction.id).ToString("f3"), text5, P_2);
					if (dVVfNNDFzurhDkJneNxNbvLVtxey10.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
					{
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Value", P_0.GetAxis(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Raw Value", P_0.GetAxisRaw(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Value", P_0.GetButton(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Double Press Value", P_0.GetButtonDoublePressHold(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Short Press Value", P_0.GetButtonShortPress(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Long Press Value", P_0.GetButtonLongPress(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Repeating Value", P_0.GetButtonRepeating(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Negative Button Value", P_0.GetNegativeButton(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Negative Button Double Press Value", P_0.GetNegativeButtonDoublePressHold(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Negative Button Short Press Value", P_0.GetNegativeButtonShortPress(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Negative Button Long Press Value", P_0.GetNegativeButtonLongPress(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Negative Button Repeating Value", P_0.GetNegativeButtonRepeating(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Time Active", P_0.GetAxisTimeActive(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Time Inactive", P_0.GetAxisTimeInactive(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Raw Time Active", P_0.GetAxisRawTimeActive(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Raw Time Inactive", P_0.GetAxisRawTimeInactive(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Time Pressed", P_0.GetButtonTimePressed(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Time Unpressed", P_0.GetButtonTimeUnpressed(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Negative Button Time Pressed", P_0.GetNegativeButtonTimePressed(inputAction.id).ToString());
						MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Negative Button Time Unpressed", P_0.GetNegativeButtonTimeUnpressed(inputAction.id).ToString());
					}
				}
			}
		}

		private static void mHEHEvclwRmidohTInsyJecVCuaM(IList<InputBehavior> P_0, IDictionary<string, bool> P_1, string P_2)
		{
			int num = P_0?.Count ?? 0;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Input Behaviors (" + num + ")", P_2 + "_inputBehaviors", P_1);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				for (int i = 0; i < num; i++)
				{
					eUBcmzoFWqcuyQzEJkmnNnMsIIbM(P_0[i], i, P_1, P_2);
				}
			}
		}

		private static void eUBcmzoFWqcuyQzEJkmnNnMsIIbM(InputBehavior P_0, int P_1, IDictionary<string, bool> P_2, string P_3)
		{
			string text = P_3 + "_inputBehavior" + P_0.id;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(P_1 + ": " + P_0.name, text, P_2);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id", P_0.id.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", P_0.name);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Digital Axis Gravity", P_0.digitalAxisGravity.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Digital Axis Instant Reverse", P_0.digitalAxisInstantReverse.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Digital Axis Sensitivity", P_0.digitalAxisSensitivity.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Digital Axis Snap", P_0.digitalAxisSnap.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Joystick Axis Sensitivity", P_0.joystickAxisSensitivity.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Custom Controller Axis Sensitivity", P_0.customControllerAxisSensitivity.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Mouse XY Axis Mode", P_0.mouseXYAxisMode.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Mouse XY Axis Sensitivity", P_0.mouseXYAxisSensitivity.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Mouse XY Axis Delta Calc", P_0.mouseXYAxisDeltaCalc.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Mouse Other Axis Mode", P_0.mouseOtherAxisMode.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Mouse Other Axis Sensitivity", P_0.mouseOtherAxisSensitivity.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Dead Zone", P_0.buttonDeadZone.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Double Press Speed", P_0.buttonDoublePressSpeed.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Short Press Time", P_0.buttonShortPressTime.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Short Press Expires In", P_0.buttonShortPressExpiresIn.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Long Press Time", P_0.buttonLongPressTime.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Long Press Expires In", P_0.buttonLongPressExpiresIn.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Button Down Buffer", P_0.buttonDownBuffer.ToString());
			}
		}

		private static void hfhhpbCxyQyZNNBgDlvpbLdKWfGt(Controller P_0, IDictionary<string, bool> P_1, string P_2)
		{
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Element Identifiers", P_2 + "_elementIdentifiers", P_1);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			int num;
			if (P_0 is ControllerWithAxes)
			{
				ControllerWithAxes controllerWithAxes = P_0 as ControllerWithAxes;
				num = ((controllerWithAxes.AxisElementIdentifiers != null) ? controllerWithAxes.AxisElementIdentifiers.Count : 0);
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Axis Element Identifiers (" + num + ")", P_2 + "_axisEIs", P_1);
				if (dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					for (int i = 0; i < num; i++)
					{
						ControllerElementIdentifier controllerElementIdentifier = controllerWithAxes.AxisElementIdentifiers[i];
						using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey3 = new DVVfNNDFzurhDkJneNxNbvLVtxey(i + ": " + controllerElementIdentifier.name + " (id: " + controllerElementIdentifier.id + ")", P_2 + "_AxisEI" + i + "_" + controllerElementIdentifier.name, P_1);
						if (dVVfNNDFzurhDkJneNxNbvLVtxey3.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
						{
							MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id", controllerElementIdentifier.id.ToString());
							MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", controllerElementIdentifier.name);
						}
					}
				}
			}
			if (P_0 == null)
			{
				return;
			}
			num = ((P_0.ButtonElementIdentifiers != null) ? P_0.ButtonElementIdentifiers.Count : 0);
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey4 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Button Element Identifiers (" + num + ")", P_2 + "_buttonEIs", P_1);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey4.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int j = 0; j < num; j++)
			{
				ControllerElementIdentifier controllerElementIdentifier2 = P_0.ButtonElementIdentifiers[j];
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey5 = new DVVfNNDFzurhDkJneNxNbvLVtxey(j + ": " + controllerElementIdentifier2.name + " (id: " + controllerElementIdentifier2.id + ")", P_2 + "_ButtonEI" + j + "_" + controllerElementIdentifier2.name, P_1);
				if (dVVfNNDFzurhDkJneNxNbvLVtxey5.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id", controllerElementIdentifier2.id.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", controllerElementIdentifier2.name);
				}
			}
		}

		private static void HHvWEFoXatouJjdkdKLdZQsDyyaC(IList<Controller.Button> P_0, ControllerType P_1, IDictionary<string, bool> P_2, string P_3)
		{
			string obj = ((P_1 == ControllerType.Keyboard) ? "Key" : "Button");
			int num = P_0?.Count ?? 0;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(obj + "s (" + num + ")", P_3 + "_Buttons", P_2);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				Controller.Button button = P_0[i];
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey(i + ": " + ((P_1 == ControllerType.Keyboard) ? (Keyboard.GetKeyboardKeyCodeByButtonIndex(i).ToString() + " (" + Keyboard.GetKeyName((KeyCode)Keyboard.GetKeyboardKeyCodeByButtonIndex(i)) + ")") : button.elementIdentifier.name) + ": " + (button.value ? "Pressed" : "") + " (" + button.pressure.ToString("f3") + ")", P_3 + "_" + button.name, P_2);
				if (dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Is Member Element", button.isMemberElement.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Is Pressure Sensitive", button.isPressureSensitive.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", button.value.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", button.valuePrev.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Pressure", button.pressure.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Pressure Prev", button.pressurePrev.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Just Pressed", button.justPressed.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Just Released", button.justReleased.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Just Double Pressed", button.justDoublePressed.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Double Pressed And Held", button.doublePressedAndHeld.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Time Pressed", button.timePressed.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Time Unpressed", button.timeUnpressed.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Last Time Pressed", button.lastTimePressed.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Last Time Unpressed", button.lastTimeUnpressed.ToString());
				}
			}
		}

		private static void RMHSqwwxQzTPiUaDHehaaASrIjcf(IList<Controller.Axis> P_0, IDictionary<string, bool> P_1, string P_2)
		{
			int num = P_0?.Count ?? 0;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Axes (" + num + ")", P_2 + "_Axes", P_1);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				Controller.Axis axis = P_0[i];
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey(i + ": " + axis.elementIdentifier.name + ": " + axis.value.ToString("f3") + " (" + axis.valueRaw.ToString("f3") + ")", P_2 + "_" + axis.name, P_1);
				if (dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Is Member Element", axis.isMemberElement.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", axis.value.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Raw", axis.valueRaw.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", axis.valuePrev.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Raw Prev", axis.valueRawPrev.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Delta", axis.valueDelta.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Delta Raw", axis.valueDeltaRaw.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Time Active", axis.timeActive.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Time Active Raw", axis.timeActiveRaw.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Time Inactive", axis.timeInactive.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Time Inactive Raw", axis.timeInactiveRaw.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Last Time Active", axis.lastTimeActive.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Last Time Active Raw", axis.lastTimeActiveRaw.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Last Time Inactive", axis.lastTimeInactive.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Last Time Inactive Raw", axis.lastTimeInactiveRaw.ToString());
				}
			}
		}

		private static void zeHWSRwarfRHvTnPJYxopYXwcDjB<_0001>(ControllerType P_0, IList<_0001> P_1, string P_2, IDictionary<string, bool> P_3, string P_4) where _0001 : ControllerMap
		{
			string text = P_4 + "_controllerMaps";
			int num = P_1?.Count ?? 0;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(P_2 + " (" + num + ")", text, P_3);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				string text2 = (P_1[i].enabled ? "Enabled" : "Disabled");
				InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(P_1[i].categoryId);
				InputLayout layout = ReInput.mapping.GetLayout(P_0, P_1[i].layoutId);
				string text3 = ((mapCategory != null) ? mapCategory.name : "n/a");
				string text4 = ((layout != null) ? layout.name : "n/a");
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey(i + ": " + text3 + ", " + text4 + ": " + text2, P_4 + "_index" + i, P_3);
				if (dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					if (P_1[i] is ControllerMapWithAxes)
					{
						ewbhyKfaLoWCCqOrUKuOgdKhfRaLA(P_1[i] as ControllerMapWithAxes, P_3, text + i);
					}
					else
					{
						ewbhyKfaLoWCCqOrUKuOgdKhfRaLA(P_1[i], P_3, text + i);
					}
				}
			}
		}

		private static void ewbhyKfaLoWCCqOrUKuOgdKhfRaLA(ControllerMap P_0, IDictionary<string, bool> P_1, string P_2)
		{
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id (unique id)", P_0.id.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Source Map Id", P_0.sourceMapId.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Enabled", P_0.enabled.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Controller Type", P_0.controllerType.ToString());
			if (P_0.controllerType == ControllerType.Joystick || P_0.controllerType == ControllerType.Custom)
			{
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Controller Id", P_0.controllerId.ToString());
			}
			string text = P_0.categoryId.ToString();
			if (P_0.categoryId >= 0)
			{
				try
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(P_0.categoryId);
					if (mapCategory != null)
					{
						text = text + " (" + mapCategory.name + ")";
					}
				}
				catch
				{
				}
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Category Id", text);
			string text2 = P_0.layoutId.ToString();
			if (P_0.layoutId >= 0)
			{
				try
				{
					InputLayout layout = ReInput.mapping.GetLayout(P_0.controllerType, P_0.layoutId);
					if (layout != null)
					{
						text2 = text2 + " (" + layout.name + ")";
					}
				}
				catch
				{
				}
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Layout Id", text2);
			int buttonMapCount = P_0.buttonMapCount;
			string text3 = P_2 + "_buttonMaps";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Button Maps (" + buttonMapCount + ")", text3, P_1);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				for (int i = 0; i < buttonMapCount; i++)
				{
					fsFXWkkkyztJfENiRLBAZmryQGOM(P_0.controllerType, P_0.ButtonMaps[i], i, P_1, text3 + i);
				}
			}
		}

		private static void ewbhyKfaLoWCCqOrUKuOgdKhfRaLA(ControllerMapWithAxes P_0, IDictionary<string, bool> P_1, string P_2)
		{
			ewbhyKfaLoWCCqOrUKuOgdKhfRaLA((ControllerMap)P_0, P_1, P_2);
			string text = P_2 + "_axisMaps";
			int axisMapCount = P_0.axisMapCount;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Axis Maps (" + axisMapCount + ")", text, P_1);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				for (int i = 0; i < axisMapCount; i++)
				{
					fsFXWkkkyztJfENiRLBAZmryQGOM(P_0.controllerType, P_0.AxisMaps[i], i, P_1, text + i);
				}
			}
		}

		private static void fsFXWkkkyztJfENiRLBAZmryQGOM(ControllerType P_0, ActionElementMap P_1, int P_2, IDictionary<string, bool> P_3, string P_4)
		{
			string text = "Action Element Map";
			InputAction action = ReInput.mapping.GetAction(P_1.actionId);
			string text2 = ((action != null) ? action.name : string.Empty);
			string text3 = rznkdVdnHdaZsCYNWakZrrzyGwTgA(P_1);
			if (!string.IsNullOrEmpty(text3))
			{
				text = P_1.elementIdentifierName + " (" + text3 + ")";
			}
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(P_2 + ": " + text, P_4 + "_" + P_2, P_3);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id (unique id)", P_1.id.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Enabled", P_1.enabled.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Element Type", P_1.elementType.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Action Id", P_1.actionId + " " + ((action != null) ? ("(" + text2 + ")") : ""));
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Element Identifier Id", P_1.elementIdentifierId.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Element Identifier Name", P_1.elementIdentifierName);
			if (P_1.elementType == ControllerElementType.Axis)
			{
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Element Index", P_1.elementIndex.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Range", P_1.axisRange.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Type", P_1.axisType.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Invert", P_1.invert.ToString());
			}
			else if (P_1.elementType == ControllerElementType.Button)
			{
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Element Index", P_1.elementIndex.ToString());
				if (P_0 == ControllerType.Keyboard)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Key Code", P_1.keyCode.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Keyboard Key Code", P_1.keyboardKeyCode.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Has Modifiers", P_1.hasModifiers.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Modifier Key 1", P_1.modifierKey1.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Modifier Key 2", P_1.modifierKey2.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Modifier Key 3", P_1.modifierKey3.ToString());
				}
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Contribution", P_1.axisContribution.ToString());
		}

		private static string rznkdVdnHdaZsCYNWakZrrzyGwTgA(ActionElementMap P_0)
		{
			InputAction action = ReInput.mapping.GetAction(P_0.actionId);
			if (action == null)
			{
				return string.Empty;
			}
			string text = string.Empty;
			if (P_0.elementType == ControllerElementType.Button || (P_0.elementType == ControllerElementType.Axis && P_0.axisType == AxisType.Split))
			{
				if (P_0.axisContribution == Pole.Positive)
				{
					text = action.positiveDescriptiveName;
					if (string.IsNullOrEmpty(text))
					{
						text = ((!string.IsNullOrEmpty(action.descriptiveName)) ? (action.descriptiveName + " +") : (action.name + " +"));
					}
				}
				else
				{
					text = action.negativeDescriptiveName;
					if (string.IsNullOrEmpty(text))
					{
						text = ((!string.IsNullOrEmpty(action.descriptiveName)) ? (action.descriptiveName + " -") : (action.name + " -"));
					}
				}
			}
			else if (P_0.elementType == ControllerElementType.Axis && P_0.axisType == AxisType.Normal)
			{
				text = ((!string.IsNullOrEmpty(action.descriptiveName)) ? action.descriptiveName : action.name);
			}
			return text;
		}

		private static void AWoZPdVOglUJfkSAHldBCDbzDoeDA(ControllerMapLayoutManager P_0, IDictionary<string, bool> P_1, string P_2)
		{
			if (vCCfTBcxjUAGZBAhdNVGZezCRPEzb("Enabled", P_0.enabled))
			{
				P_0.enabled = !P_0.enabled;
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Load from User Data Store", P_0.loadFromUserDataStore.ToString());
			string text = P_2 + "_ruleSets";
			int count = P_0.ruleSets.Count;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Rule Sets (" + count + ")", text, P_1);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				for (int i = 0; i < count; i++)
				{
					CjqAVSYyDyUniSogKeIqBhQaEjmVA(P_0.ruleSets[i], i, P_1, text + i);
				}
			}
		}

		private static void CjqAVSYyDyUniSogKeIqBhQaEjmVA(ControllerMapLayoutManager.RuleSet P_0, int P_1, IDictionary<string, bool> P_2, string P_3)
		{
			int num = P_0?.Count ?? 0;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(P_1 + ": " + ((!string.IsNullOrEmpty(P_0.tag)) ? (P_0.tag + ", ") : "") + (P_0.enabled ? "Enabled" : "Disabled"), P_3, P_2);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			if (vCCfTBcxjUAGZBAhdNVGZezCRPEzb("Enabled", P_0.enabled))
			{
				P_0.enabled = !P_0.enabled;
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Tag", P_0.tag);
			string text = P_3 + "_rules";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Rules (" + P_0.Count + ")", text, P_2);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				ControllerMapLayoutManager.Rule rule = P_0[i];
				string text2 = text + i;
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey3 = new DVVfNNDFzurhDkJneNxNbvLVtxey(i + ": " + ((!string.IsNullOrEmpty(rule.tag)) ? rule.tag : ""), text2, P_2);
				if (!dVVfNNDFzurhDkJneNxNbvLVtxey3.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					continue;
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Tag", rule.tag);
				GMBZNteYRltTVPoABtcGrykumuOj(rule.controllerSetSelector, P_2, text2);
				int[] categoryIds = rule.categoryIds;
				int num2 = ((categoryIds != null) ? categoryIds.Length : 0);
				using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey4 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Map Categories (" + num2 + ")", text2 + "_categoryIds", P_2))
				{
					if (dVVfNNDFzurhDkJneNxNbvLVtxey4.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
					{
						if (num2 == 0)
						{
							MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Category", "All Map Categories");
						}
						else
						{
							for (int j = 0; j < categoryIds.Length; j++)
							{
								InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(categoryIds[j]);
								string text3 = ((mapCategory != null) ? (mapCategory.name + " (" + mapCategory.id + ")") : "[INVALID]");
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Category " + j, text3);
							}
						}
					}
				}
				InputLayout layout = ReInput.mapping.GetLayout(rule.controllerSetSelector.controllerType, rule.layoutId);
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA(rule.controllerSetSelector.controllerType.ToString() + " Layout", (layout != null) ? (layout.name + " (" + layout.id + ")") : "[INVALID]");
			}
		}

		private static void NCrvFKjUIjqvpHirCacODaPSdWxEb(ControllerMapEnabler P_0, IDictionary<string, bool> P_1, string P_2)
		{
			if (vCCfTBcxjUAGZBAhdNVGZezCRPEzb("Enabled", P_0.enabled))
			{
				P_0.enabled = !P_0.enabled;
			}
			string text = P_2 + "_ruleSets";
			int count = P_0.ruleSets.Count;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Rule Sets (" + count + ")", text, P_1);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				for (int i = 0; i < count; i++)
				{
					fPZagpnuoOtmsPycvNmqyCsFMIpk(P_0.ruleSets[i], i, P_1, text + i);
				}
			}
		}

		private static void fPZagpnuoOtmsPycvNmqyCsFMIpk(ControllerMapEnabler.RuleSet P_0, int P_1, IDictionary<string, bool> P_2, string P_3)
		{
			int num = P_0?.Count ?? 0;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(P_1 + ": " + ((!string.IsNullOrEmpty(P_0.tag)) ? (P_0.tag + ", ") : "") + (P_0.enabled ? "Enabled" : "Disabled"), P_3, P_2);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			if (vCCfTBcxjUAGZBAhdNVGZezCRPEzb("Enabled", P_0.enabled))
			{
				P_0.enabled = !P_0.enabled;
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Tag", P_0.tag);
			string text = P_3 + "_rules";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Rules (" + P_0.Count + ")", text, P_2);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				ControllerMapEnabler.Rule rule = P_0[i];
				string text2 = text + i;
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey3 = new DVVfNNDFzurhDkJneNxNbvLVtxey(i + ": " + ((!string.IsNullOrEmpty(rule.tag)) ? rule.tag : ""), text2, P_2);
				if (!dVVfNNDFzurhDkJneNxNbvLVtxey3.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					continue;
				}
				if (vCCfTBcxjUAGZBAhdNVGZezCRPEzb("Enable", rule.enable))
				{
					rule.enable = !rule.enable;
				}
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Tag", rule.tag);
				GMBZNteYRltTVPoABtcGrykumuOj(rule.controllerSetSelector, P_2, text2);
				int[] categoryIds = rule.categoryIds;
				int num2 = ((categoryIds != null) ? categoryIds.Length : 0);
				using (DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey4 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Map Categories (" + num2 + ")", text2 + "_categoryIds", P_2))
				{
					if (dVVfNNDFzurhDkJneNxNbvLVtxey4.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
					{
						if (num2 == 0)
						{
							MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Category", "All Map Categories");
						}
						else
						{
							for (int j = 0; j < categoryIds.Length; j++)
							{
								InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(categoryIds[j]);
								string text3 = ((mapCategory != null) ? (mapCategory.name + " (" + mapCategory.id + ")") : "[INVALID]");
								MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Category " + j, text3);
							}
						}
					}
				}
				int[] layoutIds = rule.layoutIds;
				int num3 = ((layoutIds != null) ? layoutIds.Length : 0);
				using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey5 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Layouts (" + num3 + ")", text2 + "_layoutIds", P_2);
				if (!dVVfNNDFzurhDkJneNxNbvLVtxey5.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
				{
					continue;
				}
				if (num3 == 0)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Layout", (rule.controllerSetSelector.type == ControllerSetSelector.Type.All) ? "All Layouts" : ("All " + rule.controllerSetSelector.controllerType.ToString() + " Layouts"));
					continue;
				}
				for (int k = 0; k < layoutIds.Length; k++)
				{
					InputLayout layout = ReInput.mapping.GetLayout(rule.controllerSetSelector.controllerType, layoutIds[k]);
					string text4 = ((layout != null) ? (layout.name + " (" + layout.id + ")") : "[INVALID]");
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA(rule.controllerSetSelector.controllerType.ToString() + " Layout " + k, text4);
				}
			}
		}

		private static void GMBZNteYRltTVPoABtcGrykumuOj(ControllerSetSelector P_0, IDictionary<string, bool> P_1, string P_2)
		{
			string text = P_2 + "_controllerSetSelector";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Controller Set Selector", text, P_1);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Type", StringTools.AddSpacesToSentence(P_0.type.ToString(), preserveAcronyms: false));
				if (P_0.type != ControllerSetSelector.Type.All)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Controller Type", P_0.controllerType.ToString());
				}
				if (P_0.type == ControllerSetSelector.Type.HardwareType)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Hardware Type Guid", P_0.hardwareTypeGuid.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Hardware Identifier", P_0.hardwareIdentifier);
				}
				if (P_0.type == ControllerSetSelector.Type.ControllerTemplateType)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Controller Template Type Guid", P_0.controllerTemplateTypeGuid.ToString());
				}
				if (P_0.type == ControllerSetSelector.Type.PersistentControllerInstance)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Device Instance Guid", P_0.deviceInstanceGuid.ToString());
				}
				if (P_0.type == ControllerSetSelector.Type.SessionControllerInstance)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Controller Id", P_0.controllerId.ToString());
				}
			}
		}

		private static void QgMEERdOSmbdxPImGZrMoVitwDZQ(Controller P_0, IDictionary<string, bool> P_1, string P_2)
		{
			P_2 += "_templates";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Templates (" + P_0.templateCount + ")", P_2, P_1);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				for (int i = 0; i < P_0.templateCount; i++)
				{
					AyIexaCAKMKDTdvRcdnQrPZCeUShB(P_0.Templates[i], i, P_2, P_1);
				}
			}
		}

		private static void AyIexaCAKMKDTdvRcdnQrPZCeUShB(IControllerTemplate P_0, int P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 += ((P_1 >= 0) ? ("_" + P_1) : "");
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(((P_1 >= 0) ? (P_1 + ": ") : "") + P_0.name, P_2, P_3);
			if (!dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				return;
			}
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Type GUID", P_0.typeGuid.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Class Type", P_0.GetType().ToString());
			P_2 += "_elements";
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey2 = new DVVfNNDFzurhDkJneNxNbvLVtxey("Elements (" + P_0.elementCount + ")", P_2, P_3);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey2.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				for (int i = 0; i < P_0.elementCount; i++)
				{
					BGSnCiQlpKQpqkfMJGWygSQVRerDA(P_0.elements[i], i, P_2, P_3);
				}
			}
		}

		private static void BGSnCiQlpKQpqkfMJGWygSQVRerDA(IControllerTemplateElement P_0, int P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 += ((P_1 >= 0) ? ("_" + P_1) : "");
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(((P_1 >= 0) ? ": " : "") + P_0.descriptiveName + " (id: " + P_0.id + ")", P_2, P_3);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Id", P_0.id.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Name", P_0.descriptiveName.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Type", P_0.type.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Exists", P_0.exists.ToString());
				if (P_0.type == ControllerTemplateElementType.Button)
				{
					QcrbbOtPichKcAeqaoyFQIztPCHDb(P_0 as IControllerTemplateButton, P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Axis)
				{
					XSObTFEaqYiCbkKJcqzAPEYJTtGj(P_0 as IControllerTemplateAxis, P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.DPad)
				{
					IControllerTemplateDPad controllerTemplateDPad = P_0 as IControllerTemplateDPad;
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", controllerTemplateDPad.value.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", controllerTemplateDPad.valuePrev.ToString());
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateDPad.up, "Up", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateDPad.right, "Right", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateDPad.down, "Down", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateDPad.left, "Left", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Hat)
				{
					IControllerTemplateHat controllerTemplateHat = P_0 as IControllerTemplateHat;
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", controllerTemplateHat.value.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", controllerTemplateHat.valuePrev.ToString());
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateHat.up, "up", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateHat.upRight, "upRight", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateHat.right, "right", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateHat.downRight, "downRight", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateHat.down, "down", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateHat.downLeft, "downLeft", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateHat.left, "left", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateHat.upLeft, "upLeft", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Stick)
				{
					IControllerTemplateStick controllerTemplateStick = P_0 as IControllerTemplateStick;
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", controllerTemplateStick.value.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", controllerTemplateStick.valuePrev.ToString());
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick.horizontal, "horizontal", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick.vertical, "vertical", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick.rotation, "rotation", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Throttle)
				{
					IControllerTemplateThrottle controllerTemplateThrottle = P_0 as IControllerTemplateThrottle;
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", controllerTemplateThrottle.value.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", controllerTemplateThrottle.valuePrev.ToString());
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateThrottle.throttle, "throttle", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateThrottle.minDetent, "zeroDetent", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.ThumbStick)
				{
					IControllerTemplateThumbStick controllerTemplateThumbStick = P_0 as IControllerTemplateThumbStick;
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", controllerTemplateThumbStick.value.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", controllerTemplateThumbStick.valuePrev.ToString());
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateThumbStick.horizontal, "horizontal", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateThumbStick.vertical, "vertical", P_2, P_3);
					guPtTCGNTkqwdciKQyRzgeHkJidw(controllerTemplateThumbStick.press, "press", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Yoke)
				{
					IControllerTemplateYoke controllerTemplateYoke = P_0 as IControllerTemplateYoke;
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", controllerTemplateYoke.value.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", controllerTemplateYoke.valuePrev.ToString());
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateYoke.rotation, "rotation", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateYoke.pushPull, "pushPull", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Stick6D)
				{
					IControllerTemplateStick6D controllerTemplateStick6D = P_0 as IControllerTemplateStick6D;
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Position", controllerTemplateStick6D.position.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Position Prev", controllerTemplateStick6D.positionPrev.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Rotation", controllerTemplateStick6D.rotation.ToString());
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Rotation Prev", controllerTemplateStick6D.rotationPrev.ToString());
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick6D.positionX, "PositionX", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick6D.positionY, "PositionY", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick6D.positionZ, "PositionZ", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick6D.rotationX, "RotationX", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick6D.rotationY, "RotationY", P_2, P_3);
					MivEjaCWHSivTmSrGJcaLltQnRFE(controllerTemplateStick6D.rotationZ, "RotationZ", P_2, P_3);
				}
				else
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Unknown element type", P_0.type.ToString());
				}
			}
		}

		private static void MivEjaCWHSivTmSrGJcaLltQnRFE(IControllerTemplateAxis P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(StringTools.VariableNameToDisplayName(P_1), P_2, P_3);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				XSObTFEaqYiCbkKJcqzAPEYJTtGj(P_0, P_2, P_3);
			}
		}

		private static void guPtTCGNTkqwdciKQyRzgeHkJidw(IControllerTemplateButton P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(StringTools.VariableNameToDisplayName(P_1), P_2, P_3);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				QcrbbOtPichKcAeqaoyFQIztPCHDb(P_0, P_2, P_3);
			}
		}

		private static void XSObTFEaqYiCbkKJcqzAPEYJTtGj(IControllerTemplateAxis P_0, string P_1, IDictionary<string, bool> P_2)
		{
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", P_0.value.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", P_0.valuePrev.ToString());
			BfyHqSPIfDsfRHOdSeaWltlvYBbr(P_0.source, "target", P_1, P_2);
		}

		private static void QcrbbOtPichKcAeqaoyFQIztPCHDb(IControllerTemplateButton P_0, string P_1, IDictionary<string, bool> P_2)
		{
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value", P_0.value.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Value Prev", P_0.valuePrev.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Pressure", P_0.pressure.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Pressure Prev", P_0.pressurePrev.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Just Pressed", P_0.justPressed.ToString());
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Just Released", P_0.justReleased.ToString());
			fFOOEbrtFMFvcyfObHpbKiTSwpzSA(P_0.source, "target", P_1, P_2);
		}

		private static void BfyHqSPIfDsfRHOdSeaWltlvYBbr(IControllerTemplateAxisSource P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey("Axis Target", P_2, P_3);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Split Axis", P_0.splitAxis.ToString());
				ragBoOxERSfrhEWLPsTRYSBwHxZhA(P_0.fullTarget, "target", P_2, P_3);
				ragBoOxERSfrhEWLPsTRYSBwHxZhA(P_0.positiveTarget, "positiveTarget", P_2, P_3);
				ragBoOxERSfrhEWLPsTRYSBwHxZhA(P_0.negativeTarget, "negativeTarget", P_2, P_3);
			}
		}

		private static void fFOOEbrtFMFvcyfObHpbKiTSwpzSA(IControllerTemplateButtonSource P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			ragBoOxERSfrhEWLPsTRYSBwHxZhA(P_0.target, "target", P_2, P_3);
		}

		private static void ragBoOxERSfrhEWLPsTRYSBwHxZhA(IControllerElementTarget P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			using DVVfNNDFzurhDkJneNxNbvLVtxey dVVfNNDFzurhDkJneNxNbvLVtxey = new DVVfNNDFzurhDkJneNxNbvLVtxey(StringTools.VariableNameToDisplayName(P_1), P_2, P_3);
			if (dVVfNNDFzurhDkJneNxNbvLVtxey.SIbEMbCaPCQDoDhEDkwIKkLbsGAVE)
			{
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Element Identifier Id", P_0.elementIdentifierId.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Axis Range", P_0.axisRange.ToString());
				MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Has Target", P_0.hasTarget.ToString());
				if (P_0.hasTarget)
				{
					MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA("Target Element", P_0.descriptiveName);
				}
			}
		}

		private static bool vCCfTBcxjUAGZBAhdNVGZezCRPEzb(string P_0, bool P_1)
		{
			MYGnSUSAaQtFziKUmXJatiOmTqIl.ePFaWFgPHYdtWhKnZOSDjWYuRGAiA(P_0, P_1.ToString());
			return false;
		}

		private static GUIStyle vdukSOoRCqDtWSjoOBiljZrzciRL()
		{
			return snOCVPAmoWecmTkWRGKqdPcjXDPhA(new GUIStyle(GUI.skin.label)
			{
				margin = 
				{
					top = 1,
					bottom = 1
				},
				fontSize = LDnMtBpByMhNcmNpPzdzLRPijZRi._fontSize
			});
		}

		public static GUIStyle GetToggleStyle()
		{
			GUIStyle gUIStyle = snOCVPAmoWecmTkWRGKqdPcjXDPhA(new GUIStyle(GUI.skin.toggle)
			{
				margin = 
				{
					top = 0,
					bottom = 0
				}
			});
			gUIStyle.fontSize = LDnMtBpByMhNcmNpPzdzLRPijZRi._fontSize;
			return gUIStyle;
		}

		private static GUIStyle snOCVPAmoWecmTkWRGKqdPcjXDPhA(GUIStyle P_0)
		{
			P_0 = new GUIStyle(P_0);
			P_0.margin.left = XuWlVJaGSgGynvtabUaxHWhMkCoT.bumJVxsmBzWqXCOVWtpuKFQKVVEX * 20;
			return P_0;
		}
	}
}
