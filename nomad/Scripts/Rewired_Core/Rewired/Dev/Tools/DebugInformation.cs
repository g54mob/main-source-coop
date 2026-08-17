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
		private class lGdydLvCDEUvIzWNGxvXEAXdbFfC : IDisposable
		{
			public readonly bool aRslJaNQkCQeYgQAsmZbxGNEdYVs;

			public lGdydLvCDEUvIzWNGxvXEAXdbFfC(string P_0, string P_1, IDictionary<string, bool> P_2)
			{
				aRslJaNQkCQeYgQAsmZbxGNEdYVs = fVYcjZCKQzrZdypUEOmThZXzKjEf(P_0, P_1, P_2);
				nXwJUZWdzUJFsBqPZripwkgyHYnQ.jsEeWFEOobNfIcVfsEFqjgmVgGaSA++;
			}

			private bool fVYcjZCKQzrZdypUEOmThZXzKjEf(string P_0, string P_1, IDictionary<string, bool> P_2)
			{
				return AQcHGJwLGSDPbHEaDtsgdgiVSqMEb(P_1, GUILayout.Toggle(XseFUxmQsQBBGILhYfGEBmnORvmrA(P_1, P_2), new GUIContent(P_0, P_0), GetToggleStyle()), P_2);
			}

			private bool XseFUxmQsQBBGILhYfGEBmnORvmrA(string P_0, IDictionary<string, bool> P_1)
			{
				if (!P_1.ContainsKey(P_0))
				{
					P_1.Add(P_0, value: false);
				}
				return P_1[P_0];
			}

			private bool AQcHGJwLGSDPbHEaDtsgdgiVSqMEb(string P_0, bool P_1, IDictionary<string, bool> P_2)
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
				nXwJUZWdzUJFsBqPZripwkgyHYnQ.jsEeWFEOobNfIcVfsEFqjgmVgGaSA--;
			}

			void IDisposable.Dispose()
			{
				//ILSpy generated this explicit interface implementation from .override directive in Dispose
				this.Dispose();
			}
		}

		private static class nXwJUZWdzUJFsBqPZripwkgyHYnQ
		{
			private static int OaTDySCeuCpwmXFzmZjcHIOKEwflA;

			public static int jsEeWFEOobNfIcVfsEFqjgmVgGaSA
			{
				get
				{
					return OaTDySCeuCpwmXFzmZjcHIOKEwflA;
				}
				set
				{
					OaTDySCeuCpwmXFzmZjcHIOKEwflA = Mathf.Max(0, b);
				}
			}
		}

		private static class iYwwWKyxDwuKkACdIyBqQtBIHTDn
		{
			public static void iYgcSAsDhZeHUJTtxZEeGfLTvaB()
			{
				GUILayout.BeginHorizontal();
			}

			public static void HxHQvsTuwWaTXmLqoydjtHcnBzRv()
			{
				GUILayout.EndHorizontal();
			}

			public static void XgnpEOWoIgMlkKGheiDzVHDhdssX()
			{
				GUILayout.BeginVertical();
			}

			public static void DViNoPugfMbxMgYcHFJTeXZHtgCgc()
			{
				GUILayout.EndVertical();
			}

			public static void vfqrJPXZhbuFxPLlkUkUrodZWVcu(string P_0, kfKibxrLZBGkiNupYEoxHQHHsJzOA P_1)
			{
				GUILayout.Label(P_0, eafeXFVdDZhyUnuIoelaCoagjACdA());
			}

			public static void DjfHFSQvhzhGlNkSfUBKwiKBFCvGA(string P_0, string P_1)
			{
				GUILayout.Label(P_0 + ": " + P_1, eafeXFVdDZhyUnuIoelaCoagjACdA());
			}

			public static void vhCAkIcTwurTILmYrZaLYuNvtRCI(string P_0, AnimationCurve P_1)
			{
				GUILayout.Label(P_0 + ": Curves are not visualized by this tool.");
			}

			public static bool nzwuXPWptatRFKxyPJIujCBDWGVx(string P_0, bool P_1)
			{
				return GUILayout.Toggle(P_1, P_0, eafeXFVdDZhyUnuIoelaCoagjACdA());
			}
		}

		private static class KVIvJDkRsTcoanxuoxFHxVaycnIr
		{
			[CompilerGenerated]
			private static float ZCZhLjbvjmWhdymMgFtqPBiYJgtT;

			[CompilerGenerated]
			private static float PiecTuksHXHFNqAwFMOvcljjAadsc;

			public static float PHsvnnsjrVpulkHVVlfvMPtLJtQi
			{
				[CompilerGenerated]
				get
				{
					return ZCZhLjbvjmWhdymMgFtqPBiYJgtT;
				}
				[CompilerGenerated]
				set
				{
					ZCZhLjbvjmWhdymMgFtqPBiYJgtT = zCZhLjbvjmWhdymMgFtqPBiYJgtT;
				}
			}

			public static float kWUEPCyoKGmSLCRavhmfEdnxYABJ
			{
				[CompilerGenerated]
				get
				{
					return PiecTuksHXHFNqAwFMOvcljjAadsc;
				}
				[CompilerGenerated]
				set
				{
					PiecTuksHXHFNqAwFMOvcljjAadsc = piecTuksHXHFNqAwFMOvcljjAadsc;
				}
			}
		}

		internal enum kfKibxrLZBGkiNupYEoxHQHHsJzOA
		{
			None = 0,
			Info = 1,
			Warning = 2,
			Error = 3
		}

		[Serializable]
		private sealed class yKrCMpucrQHDLOEAvDwkqwOHHdBCA
		{
			public static readonly yKrCMpucrQHDLOEAvDwkqwOHHdBCA _003C_003E9 = new yKrCMpucrQHDLOEAvDwkqwOHHdBCA();

			public static Comparison<InputAction> _003C_003E9__17_0;

			internal int incMDkCFhglvIlszCCVIJhIBjZxS(InputAction P_0, InputAction P_1)
			{
				return P_0.name.CompareTo(P_1.name);
			}
		}

		private sealed class hDAFffsscAgFrTuOmGKyjUqzOfmO
		{
			public InputCategory WqpdCYAnifmSuaCVAgHbyaRqxpeBb;

			internal bool fPWJqTSdtHnIXEMOtnCAIXGknBoX(InputAction P_0)
			{
				return P_0.categoryId == WqpdCYAnifmSuaCVAgHbyaRqxpeBb.id;
			}
		}

		[CustomObfuscation(rename = false)]
		[SerializeField]
		private int _fontSize = 13;

		private static DebugInformation zACEBhPBjvCYUHzLfCcoFBTjArawb;

		private IDictionary<string, bool> VMWqQFYVubqDRalFqOLYURyKPJHQ = new Dictionary<string, bool>();

		private static Vector2 uaIeBAynPWWMMXHqFVGOPFVdpHLp;

		private const string sUAeEfAAOvKLfsecMfKFHZwHMrzcc = "Rewired_DebugInformation";

		private const string RhHWhIGLHEiBaPsUnOVJgDkEKrsT = "Rewired Debug Information";

		private const int wJuKqXUyiwqolVkJKputFMnqqYkM = 20;

		[CustomObfuscation(rename = false)]
		private void OnEnable()
		{
			zACEBhPBjvCYUHzLfCcoFBTjArawb = this;
			if (VMWqQFYVubqDRalFqOLYURyKPJHQ.Count == 0)
			{
				VMWqQFYVubqDRalFqOLYURyKPJHQ.Add("Rewired_DebugInformation", value: true);
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnDisable()
		{
			if (zACEBhPBjvCYUHzLfCcoFBTjArawb == this)
			{
				zACEBhPBjvCYUHzLfCcoFBTjArawb = null;
			}
		}

		[CustomObfuscation(rename = false)]
		private void OnGUI()
		{
			nXwJUZWdzUJFsBqPZripwkgyHYnQ.jsEeWFEOobNfIcVfsEFqjgmVgGaSA = 0;
			GUILayout.BeginArea(new Rect(0f, 0f, Screen.width, Screen.height));
			uaIeBAynPWWMMXHqFVGOPFVdpHLp = GUILayout.BeginScrollView(uaIeBAynPWWMMXHqFVGOPFVdpHLp, GUILayout.ExpandWidth(expand: true), GUILayout.ExpandHeight(expand: true));
			DrawDebugInformation(enabled: true, VMWqQFYVubqDRalFqOLYURyKPJHQ);
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
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.iYgcSAsDhZeHUJTtxZEeGfLTvaB();
			GUILayout.FlexibleSpace();
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.HxHQvsTuwWaTXmLqoydjtHcnBzRv();
			Rect lastRect = GUILayoutUtility.GetLastRect();
			float num2 = lastRect.width / 3f;
			KVIvJDkRsTcoanxuoxFHxVaycnIr.PHsvnnsjrVpulkHVVlfvMPtLJtQi = lastRect.width - num2;
			KVIvJDkRsTcoanxuoxFHxVaycnIr.kWUEPCyoKGmSLCRavhmfEdnxYABJ = num2;
			vYtNezuBeuYlwfqFRIkNBlTCePDXA(enabled, foldouts);
			GUI.enabled = num;
			KVIvJDkRsTcoanxuoxFHxVaycnIr.PHsvnnsjrVpulkHVVlfvMPtLJtQi = 0f;
			KVIvJDkRsTcoanxuoxFHxVaycnIr.kWUEPCyoKGmSLCRavhmfEdnxYABJ = 0f;
		}

		private static void vYtNezuBeuYlwfqFRIkNBlTCePDXA(bool P_0, IDictionary<string, bool> P_1)
		{
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Rewired Debug Information", "Rewired_DebugInformation", P_1);
			if (!ReInput.isReady || !P_0)
			{
				GUILayout.Label("There is no active Rewired Input Manager in the scene.");
			}
			else
			{
				if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					return;
				}
				PXDSQVcactaKbXQmfPWoVbZrFYOBA(P_1, "Rewired_DebugInformation");
				bool flag = ReInput.configuration.disableNativeInput;
				if (!flag && (ReInput.currentPlatform == Platform.Windows || ReInput.currentPlatform == Platform.OSX) && ReInput.primaryInputManager.inputSourceType == InputSource.Fallback)
				{
					flag = true;
				}
				if (flag)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.vfqrJPXZhbuFxPLlkUkUrodZWVcu("Native input is disabled. Many special features are unavailable without native input.", kfKibxrLZBGkiNupYEoxHQHHsJzOA.Warning);
				}
				sXnhCOdkllBiymVOhpaaiPngylro(P_1, "Rewired_DebugInformation");
				string text = "Rewired_DebugInformation_controllers";
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Controllers", text, P_1);
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					LBnMnZtxFslasJfaidXScuKaiOZW(ReInput.controllers.Joysticks, P_1, text);
					ZArfWDpdcUAisjKzZmFMxzCVROAW(ReInput.controllers.CustomControllers, P_1, text);
					PGlpnZhsQHHuuUuYFDMTybaLHIOH(P_1, "Rewired_DebugInformation");
					aJgPNDHuBbqmJxtrinrlNNelTiSe(P_1, "Rewired_DebugInformation");
				}
				return;
			}
		}

		private static void PXDSQVcactaKbXQmfPWoVbZrFYOBA(IDictionary<string, bool> P_0, string P_1)
		{
			string text = P_1 + "_info";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Info", text, P_0);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Rewired Version", ReInput.programVersion);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Platform", ReInput.currentPlatform.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Primary Input Source", ReInput.primaryInputManager.inputSourceType.ToString());
				if (ReInput.currentPlatform == Platform.Windows)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Use Windows Gaming Input", ReInput.configuration.useWindowsGamingInput.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Use XInput", ReInput.configuration.useXInput.ToString());
				}
				else if (ReInput.currentPlatform == Platform.WindowsUWP)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Support HID Devices", ReInput.configuration.windowsUWPSupportHIDDevices.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Support Gamepads", ReInput.configuration.windowsUWPSupportGamepads.ToString());
				}
				else if (ReInput.currentPlatform == Platform.OSX)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Use Game Controller Framework", ReInput.configuration.useAppleGameControllerFramework.ToString());
				}
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enhanced Device Support", ReInput.configuration.enhancedDeviceSupport.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Native Keyboard Handling", ReInput.configuration.nativeKeyboardSupport.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Physical Key Mapping", ReInput.configVars.unityUsePhysicalKeys.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Native Mouse Handling", ReInput.configuration.nativeMouseSupport.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Ignore Input When App Not in Focus", ReInput.configuration.ignoreInputWhenAppNotInFocus.ToString());
			}
		}

		private static void sXnhCOdkllBiymVOhpaaiPngylro(IDictionary<string, bool> P_0, string P_1)
		{
			string text = P_1 + "_players";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Players (" + ReInput.players.allPlayerCount + ")", text, P_0);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				int playerCount = ReInput.players.playerCount;
				for (int i = 0; i < playerCount; i++)
				{
					IGPYGxNSraaQzWwqDTTmtIDJsGzD(ReInput.players.GetPlayer(i), i, P_0, text);
				}
				IGPYGxNSraaQzWwqDTTmtIDJsGzD(ReInput.players.SystemPlayer, -1, P_0, text);
			}
		}

		private static void LBnMnZtxFslasJfaidXScuKaiOZW(IList<Joystick> P_0, IDictionary<string, bool> P_1, string P_2)
		{
			int num = P_0?.Count ?? 0;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Joysticks (" + num + ")", P_2 + "_joysticks", P_1);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				Joystick joystick = P_0[i];
				int id = joystick.id;
				string text = P_2 + "_joystick" + id;
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(i + ": " + ((joystick.name == "Unknown Controller") ? joystick.hardwareName : joystick.name), text, P_1);
				if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					continue;
				}
				id = joystick.id;
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id (unique id)", id.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", joystick.name);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Hardware Name", joystick.hardwareName);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Is Recognized", (joystick.hardwareTypeGuid != Guid.Empty).ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enabled", joystick.enabled.ToString());
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
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Assigned to Players", (!string.IsNullOrEmpty(text2)) ? text2 : "None");
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("System Id", joystick.systemId.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Unity Id", ReInput.usingUnityInput ? joystick.unityId.ToString() : "--");
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Hardware Type Guid", joystick.hardwareTypeGuid.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Hardware Identifier", joystick.hardwareIdentifier);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Device Instance Guid", joystick.deviceInstanceGuid.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Tag", joystick.tag);
				XcLylFmjdEMxaCIIhGPkgKjMpORS(joystick.Axes, P_1, text);
				CpqewIVnHokdQlJwryHsRMnuTCrE(joystick.Buttons, ControllerType.Joystick, P_1, text);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis2D Count", joystick.axis2DCount.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Hat Count", joystick.hatCount.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("D-Pad Count", joystick.directionalPadCount.ToString());
				RqOjrlCkRILurhITpTLNAGzreUsf(joystick, P_1, text);
				CalibrationMap calibrationMap = joystick.calibrationMap;
				using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC4 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Calibration Map", text + "_calibrationMap", P_1))
				{
					if (lGdydLvCDEUvIzWNGxvXEAXdbFfC4.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
					{
						int axisCount = calibrationMap.axisCount;
						for (int k = 0; k < axisCount; k++)
						{
							AxisCalibration axisCalibration = calibrationMap.Axes[k];
							using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC5 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(k + ": Axis Calibration (" + (axisCalibration.enabled ? "Enabled" : "Disabled") + ")", text + "_AxisCalibration" + k, P_1);
							if (lGdydLvCDEUvIzWNGxvXEAXdbFfC5.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
							{
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enabled", axisCalibration.enabled.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Apply Range Calibration", axisCalibration.applyRangeCalibration.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Calibrated Max", axisCalibration.calibratedMax.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Calibrated Min", axisCalibration.calibratedMin.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Calibrated Zero", axisCalibration.calibratedZero.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Dead Zone", axisCalibration.deadZone.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Invert", axisCalibration.invert.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Sensitivity Type", axisCalibration.sensitivityType.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Sensitivity", axisCalibration.sensitivity.ToString());
								if (axisCalibration.sensitivityCurve != null)
								{
									bool num2 = GUI.enabled;
									GUI.enabled = false;
									iYwwWKyxDwuKkACdIyBqQtBIHTDn.vhCAkIcTwurTILmYrZaLYuNvtRCI("Sensitivity Curve", axisCalibration.sensitivityCurve);
									GUI.enabled = num2;
								}
								else
								{
									iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Sensitivity Curve", "--");
								}
							}
						}
					}
				}
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Supports Vibration", joystick.supportsVibration.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Vibration Motor Count", joystick.vibrationMotorCount.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Has Extension", (joystick.extension != null).ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Extension Type", (joystick.extension != null) ? joystick.extension.GetType().Name : "--");
				dLNGUZgNjnkZoPGtIFXVHCbHBKwVA(joystick, P_1, text);
			}
		}

		private static void PGlpnZhsQHHuuUuYFDMTybaLHIOH(IDictionary<string, bool> P_0, string P_1)
		{
			string text = P_1 + "_mouse";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Mouse", text, P_0);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			Mouse mouse = ReInput.controllers.Mouse;
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enabled", mouse.enabled.ToString());
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
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Assigned to Players", (!string.IsNullOrEmpty(text2)) ? text2 : "None");
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Screen Position", mouse.screenPosition.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Screen Position Prev", mouse.screenPositionPrev.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Screen Position Delta", mouse.screenPositionDelta.ToString());
			XcLylFmjdEMxaCIIhGPkgKjMpORS(mouse.Axes, P_0, text);
			CpqewIVnHokdQlJwryHsRMnuTCrE(mouse.Buttons, ControllerType.Mouse, P_0, text);
			RqOjrlCkRILurhITpTLNAGzreUsf(mouse, P_0, text);
			dLNGUZgNjnkZoPGtIFXVHCbHBKwVA(mouse, P_0, text);
		}

		private static void aJgPNDHuBbqmJxtrinrlNNelTiSe(IDictionary<string, bool> P_0, string P_1)
		{
			string text = P_1 + "_keyboard";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Keyboard", text, P_0);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			Keyboard keyboard = ReInput.controllers.Keyboard;
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enabled", keyboard.enabled.ToString());
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
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Assigned to Players", (!string.IsNullOrEmpty(text2)) ? text2 : "None");
			CpqewIVnHokdQlJwryHsRMnuTCrE(keyboard.Buttons, ControllerType.Keyboard, P_0, text);
			RqOjrlCkRILurhITpTLNAGzreUsf(keyboard, P_0, text);
			dLNGUZgNjnkZoPGtIFXVHCbHBKwVA(keyboard, P_0, text);
		}

		private static void ZArfWDpdcUAisjKzZmFMxzCVROAW(IList<CustomController> P_0, IDictionary<string, bool> P_1, string P_2)
		{
			int num = P_0?.Count ?? 0;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Custom Controllers (" + num + ")", P_2 + "_customControllers", P_1);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				CustomController customController = P_0[i];
				int id = customController.id;
				string text = P_2 + "_customController" + id;
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(i + ": " + customController.name, text, P_1);
				if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					continue;
				}
				id = customController.id;
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id", id.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", customController.name);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Hardware Name", customController.hardwareName);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Tag", customController.tag);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Hardware Identifier", customController.hardwareIdentifier);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enabled", customController.enabled.ToString());
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
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Assigned to Players", (!string.IsNullOrEmpty(text2)) ? text2 : "None");
				XcLylFmjdEMxaCIIhGPkgKjMpORS(customController.Axes, P_1, text);
				CpqewIVnHokdQlJwryHsRMnuTCrE(customController.Buttons, ControllerType.Custom, P_1, text);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis2D Count", customController.axis2DCount.ToString());
				using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC4 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Element Identifiers", text + "_elementIdentifiers", P_1))
				{
					if (lGdydLvCDEUvIzWNGxvXEAXdbFfC4.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
					{
						int num2 = ((customController.AxisElementIdentifiers != null) ? customController.AxisElementIdentifiers.Count : 0);
						using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC5 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Axis Element Identifiers (" + num2 + ")", text + "_axisEIs", P_1))
						{
							if (lGdydLvCDEUvIzWNGxvXEAXdbFfC5.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
							{
								for (int k = 0; k < num2; k++)
								{
									ControllerElementIdentifier controllerElementIdentifier = customController.AxisElementIdentifiers[k];
									using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC6 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(k + ": " + controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename + " (id: " + controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid + ")", text + "_AxisEI" + k + "_" + controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename, P_1);
									if (lGdydLvCDEUvIzWNGxvXEAXdbFfC6.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
									{
										iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id", controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid.ToString());
										iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename);
									}
								}
							}
						}
						num2 = ((customController.ButtonElementIdentifiers != null) ? customController.ButtonElementIdentifiers.Count : 0);
						using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC7 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Button Element Identifiers (" + num2 + ")", text + "_buttonEIs", P_1);
						if (lGdydLvCDEUvIzWNGxvXEAXdbFfC7.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
						{
							for (int l = 0; l < num2; l++)
							{
								ControllerElementIdentifier controllerElementIdentifier2 = customController.ButtonElementIdentifiers[l];
								using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC8 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(l + ": " + controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename + " (id: " + controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid + ")", text + "_ButtonEI" + l + "_" + controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename, P_1);
								if (lGdydLvCDEUvIzWNGxvXEAXdbFfC8.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
								{
									iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id", controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid.ToString());
									iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename);
								}
							}
						}
					}
				}
				CalibrationMap calibrationMap = customController.calibrationMap;
				using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC9 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Calibration Map", text + "_calibrationMap", P_1))
				{
					if (lGdydLvCDEUvIzWNGxvXEAXdbFfC9.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
					{
						int num2 = calibrationMap.axisCount;
						for (int m = 0; m < num2; m++)
						{
							AxisCalibration axisCalibration = calibrationMap.Axes[m];
							using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC10 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(m + ": Axis Calibration (" + (axisCalibration.enabled ? "Enabled" : "Disabled") + ")", text + "_AxisCalibration" + m, P_1);
							if (lGdydLvCDEUvIzWNGxvXEAXdbFfC10.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
							{
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enabled", axisCalibration.enabled.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Apply Range Calibration", axisCalibration.applyRangeCalibration.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Calibrated Max", axisCalibration.calibratedMax.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Calibrated Min", axisCalibration.calibratedMin.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Calibrated Zero", axisCalibration.calibratedZero.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Dead Zone", axisCalibration.deadZone.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Invert", axisCalibration.invert.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Sensitivity Type", axisCalibration.sensitivityType.ToString());
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Sensitivity", axisCalibration.sensitivity.ToString());
								if (axisCalibration.sensitivityCurve != null)
								{
									bool num3 = GUI.enabled;
									GUI.enabled = false;
									iYwwWKyxDwuKkACdIyBqQtBIHTDn.vhCAkIcTwurTILmYrZaLYuNvtRCI("Sensitivity Curve", axisCalibration.sensitivityCurve);
									GUI.enabled = num3;
								}
								else
								{
									iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Sensitivity Curve", "--");
								}
							}
						}
					}
				}
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Has Extension", (customController.extension != null).ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Extension Type", (customController.extension != null) ? customController.extension.GetType().Name : "--");
				dLNGUZgNjnkZoPGtIFXVHCbHBKwVA(customController, P_1, text);
			}
		}

		private static void IGPYGxNSraaQzWwqDTTmtIDJsGzD(Player P_0, int P_1, IDictionary<string, bool> P_2, string P_3)
		{
			string text = P_3 + "_player" + P_0.id;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC((P_0.id == 9999999) ? "System Player" : (P_1 + ": " + P_0.name), text, P_2);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Player Id", P_0.id.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", P_0.name);
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Descriptive Name", P_0.descriptiveName);
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Is Playing", P_0.isPlaying.ToString());
			using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Controllers", text + "_controllers", P_2))
			{
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					Player.ControllerHelper controllers = P_0.controllers;
					LBnMnZtxFslasJfaidXScuKaiOZW(controllers.Joysticks, P_2, text);
					ZArfWDpdcUAisjKzZmFMxzCVROAW(controllers.CustomControllers, P_2, text);
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Has Mouse", controllers.hasMouse.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Has Keyboard", controllers.hasKeyboard.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Exclude From Controller Auto Assignment", controllers.excludeFromControllerAutoAssignment.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Last Active Controller", (controllers.GetLastActiveController() != null) ? controllers.GetLastActiveController().name.ToString() : "NULL");
				}
			}
			string text2 = text + "_controllerMaps";
			using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC4 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Controller Maps", text2, P_2))
			{
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC4.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					xExEhZFKzqurDZthYCtGbEGHOYXtA(ControllerType.Keyboard, P_0.controllers.maps.GetMaps<KeyboardMap>(0), "Keyboard Maps", P_2, text2 + "_keyboard");
					xExEhZFKzqurDZthYCtGbEGHOYXtA(ControllerType.Mouse, P_0.controllers.maps.GetMaps<MouseMap>(0), "Mouse Maps", P_2, text2 + "_mouse");
					string text3 = text2 + "_joystickMaps";
					using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC5 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Joystick Maps (" + P_0.controllers.joystickCount + ")", text3, P_2))
					{
						if (lGdydLvCDEUvIzWNGxvXEAXdbFfC5.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
						{
							for (int i = 0; i < P_0.controllers.joystickCount; i++)
							{
								Joystick joystick = P_0.controllers.Joysticks[i];
								IList<JoystickMap> maps = P_0.controllers.maps.GetMaps<JoystickMap>(joystick.id);
								string text4 = text3;
								int id = joystick.id;
								text3 = text4 + "_joystickId" + id;
								xExEhZFKzqurDZthYCtGbEGHOYXtA(ControllerType.Joystick, maps, (joystick.name != "Unknown Controller") ? joystick.name : joystick.hardwareName, P_2, text3);
							}
						}
					}
					text3 = text2 + "_customControllerMaps";
					using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC6 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Custom Controller Maps (" + P_0.controllers.customControllerCount + ")", text3, P_2);
					if (lGdydLvCDEUvIzWNGxvXEAXdbFfC6.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
					{
						for (int j = 0; j < P_0.controllers.customControllerCount; j++)
						{
							CustomController customController = P_0.controllers.CustomControllers[j];
							IList<CustomControllerMap> maps2 = P_0.controllers.maps.GetMaps<CustomControllerMap>(customController.id);
							string text5 = text3;
							int id = customController.id;
							text3 = text5 + "_customControllerId" + id;
							xExEhZFKzqurDZthYCtGbEGHOYXtA(ControllerType.Custom, maps2, customController.name, P_2, text3);
						}
					}
				}
			}
			text2 = text + "_controllerMapLayoutManager";
			using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC7 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Layout Manager", text2, P_2))
			{
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC7.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					FsIqrpBgrNbDguBJHWNsMnZTHRXx(P_0.controllers.maps.layoutManager, P_2, text2);
				}
			}
			text2 = text + "_controllerMapEnabler";
			using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC8 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Map Enabler", text2, P_2))
			{
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC8.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					uPGifDEDMiykCmmzlCYZErxbjEGXA(P_0.controllers.maps.mapEnabler, P_2, text2);
				}
			}
			text2 = text + "_inputBehaviors";
			BWqzgTmcDxgZWEmpUeuWbAiYqbwGb(P_0.controllers.maps.InputBehaviors, P_2, text2);
			text2 = text + "_actions";
			List<InputAction> list = new List<InputAction>(ReInput.mapping.Actions);
			list.Sort(yKrCMpucrQHDLOEAvDwkqwOHHdBCA._003C_003E9.incMDkCFhglvIlszCCVIJhIBjZxS);
			IList<InputCategory> actionCategories = ReInput.mapping.ActionCategories;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC9 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Actions (" + list.Count + ")", text2, P_2);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC9.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			for (int k = 0; k < actionCategories.Count; k++)
			{
				hDAFffsscAgFrTuOmGKyjUqzOfmO hDAFffsscAgFrTuOmGKyjUqzOfmO2 = new hDAFffsscAgFrTuOmGKyjUqzOfmO();
				hDAFffsscAgFrTuOmGKyjUqzOfmO2.WqpdCYAnifmSuaCVAgHbyaRqxpeBb = actionCategories[k];
				string text6 = text2 + "_actionCat" + hDAFffsscAgFrTuOmGKyjUqzOfmO2.WqpdCYAnifmSuaCVAgHbyaRqxpeBb.id;
				int num = ListTools.Count(list, hDAFffsscAgFrTuOmGKyjUqzOfmO2.fPWJqTSdtHnIXEMOtnCAIXGknBoX);
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC10 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("id " + hDAFffsscAgFrTuOmGKyjUqzOfmO2.WqpdCYAnifmSuaCVAgHbyaRqxpeBb.id + ": " + hDAFffsscAgFrTuOmGKyjUqzOfmO2.WqpdCYAnifmSuaCVAgHbyaRqxpeBb.name + " (" + num + ")", text6, P_2);
				if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC10.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					continue;
				}
				for (int l = 0; l < list.Count; l++)
				{
					InputAction inputAction = list[l];
					if (inputAction.categoryId != hDAFffsscAgFrTuOmGKyjUqzOfmO2.WqpdCYAnifmSuaCVAgHbyaRqxpeBb.id)
					{
						continue;
					}
					string text7 = text6 + "_actionId" + inputAction.id;
					using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC11 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("id " + inputAction.id + ": " + inputAction.name + ": " + P_0.GetAxis(inputAction.id).ToString("f3"), text7, P_2);
					if (lGdydLvCDEUvIzWNGxvXEAXdbFfC11.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
					{
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Value", P_0.GetAxis(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Raw Value", P_0.GetAxisRaw(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Value", P_0.GetButton(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Double Press Value", P_0.GetButtonDoublePressHold(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Short Press Value", P_0.GetButtonShortPress(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Long Press Value", P_0.GetButtonLongPress(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Repeating Value", P_0.GetButtonRepeating(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Negative Button Value", P_0.GetNegativeButton(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Negative Button Double Press Value", P_0.GetNegativeButtonDoublePressHold(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Negative Button Short Press Value", P_0.GetNegativeButtonShortPress(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Negative Button Long Press Value", P_0.GetNegativeButtonLongPress(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Negative Button Repeating Value", P_0.GetNegativeButtonRepeating(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Time Active", P_0.GetAxisTimeActive(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Time Inactive", P_0.GetAxisTimeInactive(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Raw Time Active", P_0.GetAxisRawTimeActive(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Raw Time Inactive", P_0.GetAxisRawTimeInactive(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Time Pressed", P_0.GetButtonTimePressed(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Time Unpressed", P_0.GetButtonTimeUnpressed(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Negative Button Time Pressed", P_0.GetNegativeButtonTimePressed(inputAction.id).ToString());
						iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Negative Button Time Unpressed", P_0.GetNegativeButtonTimeUnpressed(inputAction.id).ToString());
					}
				}
			}
		}

		private static void BWqzgTmcDxgZWEmpUeuWbAiYqbwGb(IList<InputBehavior> P_0, IDictionary<string, bool> P_1, string P_2)
		{
			int num = P_0?.Count ?? 0;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Input Behaviors (" + num + ")", P_2 + "_inputBehaviors", P_1);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				for (int i = 0; i < num; i++)
				{
					qqxndlATpaxNYgvTOmZGAYNLHtAS(P_0[i], i, P_1, P_2);
				}
			}
		}

		private static void qqxndlATpaxNYgvTOmZGAYNLHtAS(InputBehavior P_0, int P_1, IDictionary<string, bool> P_2, string P_3)
		{
			string text = P_3 + "_inputBehavior" + P_0.id;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(P_1 + ": " + P_0.name, text, P_2);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id", P_0.id.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", P_0.name);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Digital Axis Gravity", P_0.digitalAxisGravity.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Digital Axis Instant Reverse", P_0.digitalAxisInstantReverse.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Digital Axis Sensitivity", P_0.digitalAxisSensitivity.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Digital Axis Snap", P_0.digitalAxisSnap.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Joystick Axis Sensitivity", P_0.joystickAxisSensitivity.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Custom Controller Axis Sensitivity", P_0.customControllerAxisSensitivity.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Mouse XY Axis Mode", P_0.mouseXYAxisMode.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Mouse XY Axis Sensitivity", P_0.mouseXYAxisSensitivity.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Mouse XY Axis Delta Calc", P_0.mouseXYAxisDeltaCalc.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Mouse Other Axis Mode", P_0.mouseOtherAxisMode.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Mouse Other Axis Sensitivity", P_0.mouseOtherAxisSensitivity.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Dead Zone", P_0.buttonDeadZone.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Double Press Speed", P_0.buttonDoublePressSpeed.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Short Press Time", P_0.buttonShortPressTime.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Short Press Expires In", P_0.buttonShortPressExpiresIn.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Long Press Time", P_0.buttonLongPressTime.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Long Press Expires In", P_0.buttonLongPressExpiresIn.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Button Down Buffer", P_0.buttonDownBuffer.ToString());
			}
		}

		private static void RqOjrlCkRILurhITpTLNAGzreUsf(Controller P_0, IDictionary<string, bool> P_1, string P_2)
		{
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Element Identifiers", P_2 + "_elementIdentifiers", P_1);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			int num;
			if (P_0 is ControllerWithAxes)
			{
				ControllerWithAxes controllerWithAxes = P_0 as ControllerWithAxes;
				num = ((controllerWithAxes.AxisElementIdentifiers != null) ? controllerWithAxes.AxisElementIdentifiers.Count : 0);
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Axis Element Identifiers (" + num + ")", P_2 + "_axisEIs", P_1);
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					for (int i = 0; i < num; i++)
					{
						ControllerElementIdentifier controllerElementIdentifier = controllerWithAxes.AxisElementIdentifiers[i];
						using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC4 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(i + ": " + controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename + " (id: " + controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid + ")", P_2 + "_AxisEI" + i + "_" + controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename, P_1);
						if (lGdydLvCDEUvIzWNGxvXEAXdbFfC4.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
						{
							iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id", controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid.ToString());
							iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", controllerElementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename);
						}
					}
				}
			}
			if (P_0 == null)
			{
				return;
			}
			num = ((P_0.ButtonElementIdentifiers != null) ? P_0.ButtonElementIdentifiers.Count : 0);
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC5 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Button Element Identifiers (" + num + ")", P_2 + "_buttonEIs", P_1);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC5.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			for (int j = 0; j < num; j++)
			{
				ControllerElementIdentifier controllerElementIdentifier2 = P_0.ButtonElementIdentifiers[j];
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC6 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(j + ": " + controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename + " (id: " + controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid + ")", P_2 + "_ButtonEI" + j + "_" + controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename, P_1);
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC6.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id", controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Eid.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", controllerElementIdentifier2.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename);
				}
			}
		}

		private static void CpqewIVnHokdQlJwryHsRMnuTCrE(IList<Controller.Button> P_0, ControllerType P_1, IDictionary<string, bool> P_2, string P_3)
		{
			string obj = ((P_1 == ControllerType.Keyboard) ? "Key" : "Button");
			int num = P_0?.Count ?? 0;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(obj + "s (" + num + ")", P_3 + "_Buttons", P_2);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				Controller.Button button = P_0[i];
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(i + ": " + ((P_1 == ControllerType.Keyboard) ? (Keyboard.GetKeyboardKeyCodeByButtonIndex(i).ToString() + " (" + Keyboard.GetKeyName((KeyCode)Keyboard.GetKeyboardKeyCodeByButtonIndex(i)) + ")") : button.elementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename) + ": " + (button.value ? "Pressed" : "") + " (" + button.pressure.ToString("f3") + ")", P_3 + "_" + button.name, P_2);
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Is Member Element", button.isMemberElement.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Is Pressure Sensitive", button.isPressureSensitive.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", button.value.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", button.valuePrev.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Pressure", button.pressure.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Pressure Prev", button.pressurePrev.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Just Pressed", button.justPressed.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Just Released", button.justReleased.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Just Double Pressed", button.justDoublePressed.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Double Pressed And Held", button.doublePressedAndHeld.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Time Pressed", button.timePressed.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Time Unpressed", button.timeUnpressed.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Last Time Pressed", button.lastTimePressed.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Last Time Unpressed", button.lastTimeUnpressed.ToString());
				}
			}
		}

		private static void XcLylFmjdEMxaCIIhGPkgKjMpORS(IList<Controller.Axis> P_0, IDictionary<string, bool> P_1, string P_2)
		{
			int num = P_0?.Count ?? 0;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Axes (" + num + ")", P_2 + "_Axes", P_1);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				Controller.Axis axis = P_0[i];
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(i + ": " + axis.elementIdentifier.Rewired_002EInterfaces_002EIControllerElementIdentifierCommon_Internal_002Ename + ": " + axis.value.ToString("f3") + " (" + axis.valueRaw.ToString("f3") + ")", P_2 + "_" + axis.name, P_1);
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Is Member Element", axis.isMemberElement.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", axis.value.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Raw", axis.valueRaw.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", axis.valuePrev.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Raw Prev", axis.valueRawPrev.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Delta", axis.valueDelta.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Delta Raw", axis.valueDeltaRaw.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Time Active", axis.timeActive.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Time Active Raw", axis.timeActiveRaw.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Time Inactive", axis.timeInactive.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Time Inactive Raw", axis.timeInactiveRaw.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Last Time Active", axis.lastTimeActive.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Last Time Active Raw", axis.lastTimeActiveRaw.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Last Time Inactive", axis.lastTimeInactive.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Last Time Inactive Raw", axis.lastTimeInactiveRaw.ToString());
				}
			}
		}

		private static void xExEhZFKzqurDZthYCtGbEGHOYXtA<_0001>(ControllerType P_0, IList<_0001> P_1, string P_2, IDictionary<string, bool> P_3, string P_4) where _0001 : ControllerMap
		{
			string text = P_4 + "_controllerMaps";
			int num = P_1?.Count ?? 0;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(P_2 + " (" + num + ")", text, P_3);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
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
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(i + ": " + text3 + ", " + text4 + ": " + text2, P_4 + "_index" + i, P_3);
				if (lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					if (P_1[i] is ControllerMapWithAxes)
					{
						DrmkHxBTbnOdgDGIyBeoaUrWoWttA(P_1[i] as ControllerMapWithAxes, P_3, text + i);
					}
					else
					{
						iawMdDAxETeTDOflZDndmPrbdxJY(P_1[i], P_3, text + i);
					}
				}
			}
		}

		private static void iawMdDAxETeTDOflZDndmPrbdxJY(ControllerMap P_0, IDictionary<string, bool> P_1, string P_2)
		{
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id (unique id)", P_0.id.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Source Map Id", P_0.sourceMapId.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enabled", P_0.enabled.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Controller Type", P_0.controllerType.ToString());
			if (P_0.controllerType == ControllerType.Joystick || P_0.controllerType == ControllerType.Custom)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Controller Id", P_0.controllerId.ToString());
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
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Category Id", text);
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
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Layout Id", text2);
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Is Modified", P_0.isModified.ToString());
			if (P_0.isModified)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Modified Time", P_0.modifiedTime.ToString());
			}
			int buttonMapCount = P_0.buttonMapCount;
			string text3 = P_2 + "_buttonMaps";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Button Maps (" + buttonMapCount + ")", text3, P_1);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				for (int i = 0; i < buttonMapCount; i++)
				{
					ObcNsHXaeFEgHPcrZGwPeAbeEzkYA(P_0.controllerType, P_0.ButtonMaps[i], i, P_1, text3 + i);
				}
			}
		}

		private static void DrmkHxBTbnOdgDGIyBeoaUrWoWttA(ControllerMapWithAxes P_0, IDictionary<string, bool> P_1, string P_2)
		{
			iawMdDAxETeTDOflZDndmPrbdxJY(P_0, P_1, P_2);
			string text = P_2 + "_axisMaps";
			int axisMapCount = P_0.axisMapCount;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Axis Maps (" + axisMapCount + ")", text, P_1);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				for (int i = 0; i < axisMapCount; i++)
				{
					ObcNsHXaeFEgHPcrZGwPeAbeEzkYA(P_0.controllerType, P_0.AxisMaps[i], i, P_1, text + i);
				}
			}
		}

		private static void ObcNsHXaeFEgHPcrZGwPeAbeEzkYA(ControllerType P_0, ActionElementMap P_1, int P_2, IDictionary<string, bool> P_3, string P_4)
		{
			string text = "Action Element Map";
			InputAction action = ReInput.mapping.GetAction(P_1.actionId);
			string text2 = ((action != null) ? action.name : string.Empty);
			string text3 = hsAXtBMMzxLDJRQxeVjfKHwFTWne(P_1);
			if (!string.IsNullOrEmpty(text3))
			{
				text = P_1.elementIdentifierName + " (" + text3 + ")";
			}
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(P_2 + ": " + text, P_4 + "_" + P_2, P_3);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id (unique id)", P_1.id.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Enabled", P_1.enabled.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Element Type", P_1.elementType.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Action Id", P_1.actionId + " " + ((action != null) ? ("(" + text2 + ")") : ""));
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Element Identifier Id", P_1.elementIdentifierId.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Element Identifier Name", P_1.elementIdentifierName);
			if (P_1.elementType == ControllerElementType.Axis)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Element Index", P_1.elementIndex.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Range", P_1.axisRange.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Type", P_1.axisType.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Invert", P_1.invert.ToString());
			}
			else if (P_1.elementType == ControllerElementType.Button)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Element Index", P_1.elementIndex.ToString());
				if (P_0 == ControllerType.Keyboard)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Key Code", P_1.keyCode.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Keyboard Key Code", P_1.keyboardKeyCode.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Has Modifiers", P_1.hasModifiers.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Modifier Key 1", P_1.modifierKey1.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Modifier Key 2", P_1.modifierKey2.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Modifier Key 3", P_1.modifierKey3.ToString());
				}
			}
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Contribution", P_1.axisContribution.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Modified Timestamp", P_1.modifiedTime.ToString());
		}

		private static string hsAXtBMMzxLDJRQxeVjfKHwFTWne(ActionElementMap P_0)
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

		private static void FsIqrpBgrNbDguBJHWNsMnZTHRXx(ControllerMapLayoutManager P_0, IDictionary<string, bool> P_1, string P_2)
		{
			if (lKKzEBdlZaypXVLQSZfaqiUsXPmp("Enabled", P_0.enabled))
			{
				P_0.enabled = !P_0.enabled;
			}
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Load from User Data Store", P_0.loadFromUserDataStore.ToString());
			string text = P_2 + "_ruleSets";
			int count = P_0.ruleSets.Count;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Rule Sets (" + count + ")", text, P_1);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				for (int i = 0; i < count; i++)
				{
					TgzikXhExLgwKyaCUqZkBhDVIJTI(P_0.ruleSets[i], i, P_1, text + i);
				}
			}
		}

		private static void TgzikXhExLgwKyaCUqZkBhDVIJTI(ControllerMapLayoutManager.RuleSet P_0, int P_1, IDictionary<string, bool> P_2, string P_3)
		{
			int num = P_0?.System_002ECollections_002EGeneric_002EICollection_00601_003CRewired_002EControllerMapLayoutManager_002ERule_003E_002ECount ?? 0;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(P_1 + ": " + ((!string.IsNullOrEmpty(P_0.tag)) ? (P_0.tag + ", ") : "") + (P_0.enabled ? "Enabled" : "Disabled"), P_3, P_2);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			if (lKKzEBdlZaypXVLQSZfaqiUsXPmp("Enabled", P_0.enabled))
			{
				P_0.enabled = !P_0.enabled;
			}
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Tag", P_0.tag);
			string text = P_3 + "_rules";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Rules (" + P_0.System_002ECollections_002EGeneric_002EICollection_00601_003CRewired_002EControllerMapLayoutManager_002ERule_003E_002ECount + ")", text, P_2);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				ControllerMapLayoutManager.Rule rule = P_0[i];
				string text2 = text + i;
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC4 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(i + ": " + ((!string.IsNullOrEmpty(rule.tag)) ? rule.tag : ""), text2, P_2);
				if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC4.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					continue;
				}
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Tag", rule.tag);
				BvCnenDLoCMJPYjEuSIemToZcJvo(rule.controllerSetSelector, P_2, text2);
				int[] categoryIds = rule.categoryIds;
				int num2 = ((categoryIds != null) ? categoryIds.Length : 0);
				using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC5 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Map Categories (" + num2 + ")", text2 + "_categoryIds", P_2))
				{
					if (lGdydLvCDEUvIzWNGxvXEAXdbFfC5.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
					{
						if (num2 == 0)
						{
							iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Category", "All Map Categories");
						}
						else
						{
							for (int j = 0; j < categoryIds.Length; j++)
							{
								InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(categoryIds[j]);
								string text3 = ((mapCategory != null) ? (mapCategory.name + " (" + mapCategory.id + ")") : "[INVALID]");
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Category " + j, text3);
							}
						}
					}
				}
				InputLayout layout = ReInput.mapping.GetLayout(rule.controllerSetSelector.controllerType, rule.layoutId);
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA(rule.controllerSetSelector.controllerType.ToString() + " Layout", (layout != null) ? (layout.name + " (" + layout.id + ")") : "[INVALID]");
			}
		}

		private static void uPGifDEDMiykCmmzlCYZErxbjEGXA(ControllerMapEnabler P_0, IDictionary<string, bool> P_1, string P_2)
		{
			if (lKKzEBdlZaypXVLQSZfaqiUsXPmp("Enabled", P_0.enabled))
			{
				P_0.enabled = !P_0.enabled;
			}
			string text = P_2 + "_ruleSets";
			int count = P_0.ruleSets.Count;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Rule Sets (" + count + ")", text, P_1);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				for (int i = 0; i < count; i++)
				{
					xsgZTffQFPwdgCOzNaIGfUwpCBAM(P_0.ruleSets[i], i, P_1, text + i);
				}
			}
		}

		private static void xsgZTffQFPwdgCOzNaIGfUwpCBAM(ControllerMapEnabler.RuleSet P_0, int P_1, IDictionary<string, bool> P_2, string P_3)
		{
			int num = P_0?.System_002ECollections_002EGeneric_002EICollection_00601_003CRewired_002EControllerMapEnabler_002ERule_003E_002ECount ?? 0;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(P_1 + ": " + ((!string.IsNullOrEmpty(P_0.tag)) ? (P_0.tag + ", ") : "") + (P_0.enabled ? "Enabled" : "Disabled"), P_3, P_2);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			if (lKKzEBdlZaypXVLQSZfaqiUsXPmp("Enabled", P_0.enabled))
			{
				P_0.enabled = !P_0.enabled;
			}
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Tag", P_0.tag);
			string text = P_3 + "_rules";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Rules (" + P_0.System_002ECollections_002EGeneric_002EICollection_00601_003CRewired_002EControllerMapEnabler_002ERule_003E_002ECount + ")", text, P_2);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				ControllerMapEnabler.Rule rule = P_0[i];
				string text2 = text + i;
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC4 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(i + ": " + ((!string.IsNullOrEmpty(rule.tag)) ? rule.tag : ""), text2, P_2);
				if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC4.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					continue;
				}
				if (lKKzEBdlZaypXVLQSZfaqiUsXPmp("Enable", rule.enable))
				{
					rule.enable = !rule.enable;
				}
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Tag", rule.tag);
				BvCnenDLoCMJPYjEuSIemToZcJvo(rule.controllerSetSelector, P_2, text2);
				int[] categoryIds = rule.categoryIds;
				int num2 = ((categoryIds != null) ? categoryIds.Length : 0);
				using (lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC5 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Map Categories (" + num2 + ")", text2 + "_categoryIds", P_2))
				{
					if (lGdydLvCDEUvIzWNGxvXEAXdbFfC5.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
					{
						if (num2 == 0)
						{
							iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Category", "All Map Categories");
						}
						else
						{
							for (int j = 0; j < categoryIds.Length; j++)
							{
								InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(categoryIds[j]);
								string text3 = ((mapCategory != null) ? (mapCategory.name + " (" + mapCategory.id + ")") : "[INVALID]");
								iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Category " + j, text3);
							}
						}
					}
				}
				int[] layoutIds = rule.layoutIds;
				int num3 = ((layoutIds != null) ? layoutIds.Length : 0);
				using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC6 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Layouts (" + num3 + ")", text2 + "_layoutIds", P_2);
				if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC6.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
				{
					continue;
				}
				if (num3 == 0)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Layout", (rule.controllerSetSelector.type == ControllerSetSelector.Type.All) ? "All Layouts" : ("All " + rule.controllerSetSelector.controllerType.ToString() + " Layouts"));
					continue;
				}
				for (int k = 0; k < layoutIds.Length; k++)
				{
					InputLayout layout = ReInput.mapping.GetLayout(rule.controllerSetSelector.controllerType, layoutIds[k]);
					string text4 = ((layout != null) ? (layout.name + " (" + layout.id + ")") : "[INVALID]");
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA(rule.controllerSetSelector.controllerType.ToString() + " Layout " + k, text4);
				}
			}
		}

		private static void BvCnenDLoCMJPYjEuSIemToZcJvo(ControllerSetSelector P_0, IDictionary<string, bool> P_1, string P_2)
		{
			string text = P_2 + "_controllerSetSelector";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Controller Set Selector", text, P_1);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Type", StringTools.AddSpacesToSentence(P_0.type.ToString(), preserveAcronyms: false));
				if (P_0.type != ControllerSetSelector.Type.All)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Controller Type", P_0.controllerType.ToString());
				}
				if (P_0.type == ControllerSetSelector.Type.HardwareType)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Hardware Type Guid", P_0.hardwareTypeGuid.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Hardware Identifier", P_0.hardwareIdentifier);
				}
				if (P_0.type == ControllerSetSelector.Type.ControllerTemplateType)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Controller Template Type Guid", P_0.controllerTemplateTypeGuid.ToString());
				}
				if (P_0.type == ControllerSetSelector.Type.PersistentControllerInstance)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Device Instance Guid", P_0.deviceInstanceGuid.ToString());
				}
				if (P_0.type == ControllerSetSelector.Type.SessionControllerInstance)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Controller Id", P_0.controllerId.ToString());
				}
			}
		}

		private static void dLNGUZgNjnkZoPGtIFXVHCbHBKwVA(Controller P_0, IDictionary<string, bool> P_1, string P_2)
		{
			P_2 += "_templates";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Templates (" + P_0.templateCount + ")", P_2, P_1);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				for (int i = 0; i < P_0.templateCount; i++)
				{
					SoTMTcWtYnMOsofzhozdAKQBxoNy(P_0.Templates[i], i, P_2, P_1);
				}
			}
		}

		private static void SoTMTcWtYnMOsofzhozdAKQBxoNy(IControllerTemplate P_0, int P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 += ((P_1 >= 0) ? ("_" + P_1) : "");
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(((P_1 >= 0) ? (P_1 + ": ") : "") + P_0.name, P_2, P_3);
			if (!lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				return;
			}
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Type GUID", P_0.typeGuid.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Class Type", P_0.GetType().ToString());
			P_2 += "_elements";
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC3 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Elements (" + P_0.elementCount + ")", P_2, P_3);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC3.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				for (int i = 0; i < P_0.elementCount; i++)
				{
					WrSJGzICorcHrUpsZbcBKlharICD(P_0.elements[i], i, P_2, P_3);
				}
			}
		}

		private static void WrSJGzICorcHrUpsZbcBKlharICD(IControllerTemplateElement P_0, int P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 += ((P_1 >= 0) ? ("_" + P_1) : "");
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(((P_1 >= 0) ? ": " : "") + P_0.descriptiveName + " (id: " + P_0.id + ")", P_2, P_3);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Id", P_0.id.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Name", P_0.descriptiveName.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Type", P_0.type.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Exists", P_0.exists.ToString());
				if (P_0.type == ControllerTemplateElementType.Button)
				{
					LmVoJSvFxThJRKDMFVTdgtTgBUwJ(P_0 as IControllerTemplateButton, P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Axis)
				{
					KOzgACorIEansHcjYGdxNvbbvQKHA(P_0 as IControllerTemplateAxis, P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.DPad)
				{
					IControllerTemplateDPad controllerTemplateDPad = P_0 as IControllerTemplateDPad;
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", controllerTemplateDPad.value.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", controllerTemplateDPad.valuePrev.ToString());
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateDPad.up, "Up", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateDPad.right, "Right", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateDPad.down, "Down", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateDPad.left, "Left", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Hat)
				{
					IControllerTemplateHat controllerTemplateHat = P_0 as IControllerTemplateHat;
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", controllerTemplateHat.value.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", controllerTemplateHat.valuePrev.ToString());
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateHat.up, "up", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateHat.upRight, "upRight", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateHat.right, "right", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateHat.downRight, "downRight", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateHat.down, "down", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateHat.downLeft, "downLeft", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateHat.left, "left", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateHat.upLeft, "upLeft", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Stick)
				{
					IControllerTemplateStick controllerTemplateStick = P_0 as IControllerTemplateStick;
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", controllerTemplateStick.value.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", controllerTemplateStick.valuePrev.ToString());
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick.horizontal, "horizontal", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick.vertical, "vertical", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick.rotation, "rotation", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Throttle)
				{
					IControllerTemplateThrottle controllerTemplateThrottle = P_0 as IControllerTemplateThrottle;
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", controllerTemplateThrottle.value.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", controllerTemplateThrottle.valuePrev.ToString());
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateThrottle.throttle, "throttle", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateThrottle.minDetent, "zeroDetent", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.ThumbStick)
				{
					IControllerTemplateThumbStick controllerTemplateThumbStick = P_0 as IControllerTemplateThumbStick;
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", controllerTemplateThumbStick.value.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", controllerTemplateThumbStick.valuePrev.ToString());
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateThumbStick.horizontal, "horizontal", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateThumbStick.vertical, "vertical", P_2, P_3);
					GhXUzLJxJGwjeWewCXxSggUvZRcJ(controllerTemplateThumbStick.press, "press", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Yoke)
				{
					IControllerTemplateYoke controllerTemplateYoke = P_0 as IControllerTemplateYoke;
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", controllerTemplateYoke.value.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", controllerTemplateYoke.valuePrev.ToString());
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateYoke.rotation, "rotation", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateYoke.pushPull, "pushPull", P_2, P_3);
				}
				else if (P_0.type == ControllerTemplateElementType.Stick6D)
				{
					IControllerTemplateStick6D controllerTemplateStick6D = P_0 as IControllerTemplateStick6D;
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Position", controllerTemplateStick6D.position.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Position Prev", controllerTemplateStick6D.positionPrev.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Rotation", controllerTemplateStick6D.rotation.ToString());
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Rotation Prev", controllerTemplateStick6D.rotationPrev.ToString());
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick6D.positionX, "PositionX", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick6D.positionY, "PositionY", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick6D.positionZ, "PositionZ", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick6D.rotationX, "RotationX", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick6D.rotationY, "RotationY", P_2, P_3);
					ZLCqDjnhwmNFvPJKjufKpoaMsWqi(controllerTemplateStick6D.rotationZ, "RotationZ", P_2, P_3);
				}
				else
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Unknown element type", P_0.type.ToString());
				}
			}
		}

		private static void ZLCqDjnhwmNFvPJKjufKpoaMsWqi(IControllerTemplateAxis P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(StringTools.VariableNameToDisplayName(P_1), P_2, P_3);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				KOzgACorIEansHcjYGdxNvbbvQKHA(P_0, P_2, P_3);
			}
		}

		private static void GhXUzLJxJGwjeWewCXxSggUvZRcJ(IControllerTemplateButton P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(StringTools.VariableNameToDisplayName(P_1), P_2, P_3);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				LmVoJSvFxThJRKDMFVTdgtTgBUwJ(P_0, P_2, P_3);
			}
		}

		private static void KOzgACorIEansHcjYGdxNvbbvQKHA(IControllerTemplateAxis P_0, string P_1, IDictionary<string, bool> P_2)
		{
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", P_0.value.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", P_0.valuePrev.ToString());
			eDuhQhXsJWINWbInIvIghAjNBLnbA(P_0.source, "target", P_1, P_2);
		}

		private static void LmVoJSvFxThJRKDMFVTdgtTgBUwJ(IControllerTemplateButton P_0, string P_1, IDictionary<string, bool> P_2)
		{
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value", P_0.value.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Value Prev", P_0.valuePrev.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Pressure", P_0.pressure.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Pressure Prev", P_0.pressurePrev.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Just Pressed", P_0.justPressed.ToString());
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Just Released", P_0.justReleased.ToString());
			nzACCfDVDWnlnSlvgvPikTtXzGexA(P_0.source, "target", P_1, P_2);
		}

		private static void eDuhQhXsJWINWbInIvIghAjNBLnbA(IControllerTemplateAxisSource P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC("Axis Target", P_2, P_3);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Split Axis", P_0.splitAxis.ToString());
				LHcwSzxfJmbmvkqPfTwZlZaObSncA(P_0.fullTarget, "target", P_2, P_3);
				LHcwSzxfJmbmvkqPfTwZlZaObSncA(P_0.positiveTarget, "positiveTarget", P_2, P_3);
				LHcwSzxfJmbmvkqPfTwZlZaObSncA(P_0.negativeTarget, "negativeTarget", P_2, P_3);
			}
		}

		private static void nzACCfDVDWnlnSlvgvPikTtXzGexA(IControllerTemplateButtonSource P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			LHcwSzxfJmbmvkqPfTwZlZaObSncA(P_0.target, "target", P_2, P_3);
		}

		private static void LHcwSzxfJmbmvkqPfTwZlZaObSncA(IControllerElementTarget P_0, string P_1, string P_2, IDictionary<string, bool> P_3)
		{
			P_2 = P_2 + "_" + P_1;
			using lGdydLvCDEUvIzWNGxvXEAXdbFfC lGdydLvCDEUvIzWNGxvXEAXdbFfC2 = new lGdydLvCDEUvIzWNGxvXEAXdbFfC(StringTools.VariableNameToDisplayName(P_1), P_2, P_3);
			if (lGdydLvCDEUvIzWNGxvXEAXdbFfC2.aRslJaNQkCQeYgQAsmZbxGNEdYVs)
			{
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Element Identifier Id", P_0.elementIdentifierId.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Axis Range", P_0.axisRange.ToString());
				iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Has Target", P_0.hasTarget.ToString());
				if (P_0.hasTarget)
				{
					iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA("Target Element", P_0.descriptiveName);
				}
			}
		}

		private static bool lKKzEBdlZaypXVLQSZfaqiUsXPmp(string P_0, bool P_1)
		{
			iYwwWKyxDwuKkACdIyBqQtBIHTDn.DjfHFSQvhzhGlNkSfUBKwiKBFCvGA(P_0, P_1.ToString());
			return false;
		}

		private static GUIStyle eafeXFVdDZhyUnuIoelaCoagjACdA()
		{
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
			gUIStyle.margin.top = 1;
			gUIStyle.margin.bottom = 1;
			gUIStyle.fontSize = zACEBhPBjvCYUHzLfCcoFBTjArawb._fontSize;
			return xERBQLiAjInVqCDydFmbcbOaaPdcB(gUIStyle);
		}

		public static GUIStyle GetToggleStyle()
		{
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.toggle);
			gUIStyle.margin.top = 0;
			gUIStyle.margin.bottom = 0;
			GUIStyle gUIStyle2 = xERBQLiAjInVqCDydFmbcbOaaPdcB(gUIStyle);
			gUIStyle2.fontSize = zACEBhPBjvCYUHzLfCcoFBTjArawb._fontSize;
			return gUIStyle2;
		}

		private static GUIStyle xERBQLiAjInVqCDydFmbcbOaaPdcB(GUIStyle P_0)
		{
			P_0 = new GUIStyle(P_0);
			P_0.margin.left = nXwJUZWdzUJFsBqPZripwkgyHYnQ.jsEeWFEOobNfIcVfsEFqjgmVgGaSA * 20;
			return P_0;
		}
	}
}
