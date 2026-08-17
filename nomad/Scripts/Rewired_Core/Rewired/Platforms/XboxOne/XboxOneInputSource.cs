using System;
using System.Collections.Generic;
using Rewired.Internal;
using Rewired.Internal.Localization;
using Rewired.Platforms.Custom;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Platforms.XboxOne
{
	[CustomObfuscation(rename = false)]
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	internal sealed class XboxOneInputSource : CustomInputSource, IXboxOneInputSource
	{
		[CustomObfuscation(rename = false)]
		private enum BadConnectionReason
		{
			[CustomObfuscation(rename = false)]
			None = 0,
			[CustomObfuscation(rename = false)]
			GamepadNotActive = 1,
			[CustomObfuscation(rename = false)]
			InvalidName = 2
		}

		private struct oowdQnBzADoGkVjyeyEZhQdRNfer
		{
			public uint InAeDgxibEotdDNUrNCxiXRFIFTe;

			public uint GNqccZaAJiPAxsNQtcjaFRTjeHxrB;

			public oowdQnBzADoGkVjyeyEZhQdRNfer(uint P_0, uint P_1)
			{
				InAeDgxibEotdDNUrNCxiXRFIFTe = P_0;
				GNqccZaAJiPAxsNQtcjaFRTjeHxrB = P_1;
			}
		}

		private class QolkQVxmqKDDptMUYIBZzWlOSUmp : Joystick, ITryGetLocalizedName, IInputManagerHardwareJoystickMapHandler
		{
			private const int bVMPIxiMXJtDOsdsgaugVTYPVXCH = 6;

			private const int fmjNMwVexiSBIZXsSXiaZSJRazQb = 14;

			private const string ZdWbRwmWOIhjKEHOyCOEcieOjJeIb = "Xbox One Controller";

			private const string kvdHefoeCoWsDkKJOiqFTQPPyUMn = "Controller";

			private const int lmbgYZAzXbLbChVbslEtriSYHGNEA = 0;

			private const int OpzFdjGVLGXktcTWqSsELLgJRJdT = 1;

			private const int pKUtHmYnQPszFWHeQLszspcvLohg = 2;

			private const int zytAQFBnxKKIySAoOJPuNPLnbHhRA = 3;

			private const int ztiurxiUxnNuaJsutweajzxXOxXL = 4;

			private const int hmeCmNBrasSrxBabMYvHohnQivgiA = 5;

			private const int cTTtAawRPPpeULtpIckovhrdyfUc = 6;

			private const int uUPLjMvQGAHlJeAVOuJUokzvFwue = 7;

			private const int ealerobXsHqoWfvOQfHDLzZYRMyT = 8;

			private const int CNDaXsWVAVkmUqVMWxxnWSSsycGj = 9;

			private const int TDYZUlXIyPiddoxgguFTpWpDrDNq = 12;

			private const int eLBohqWiscyNmxrTCksTFZqapALl = 13;

			private const int RnKEILelCIUVaZLaeEJHbKPnyKlCA = 14;

			private const int CpjOHbTofeiqbQQYNPholWfCUTPd = 15;

			private const int MmOuekeyIgSNhdPANBjuEshEMNcKA = 0;

			private const int FVnATLdWSlIXtwiTLCHhwaWIggqR = 1;

			private const int LdxGCkSiCvicNjaqlPEhwvtdVRZQA = 3;

			private const int nfnDIhTAdcIhlDARYSppImTRnROCb = 4;

			private const int lHGWPVdoRXqreysGTaAPMQYwbvkc = 8;

			private const int lZgfDPlBXPIOWcUUosxoWLIDtATi = 9;

			private readonly IXboxOneInputSource JXbdVDGwkYTHAEuLldydCHldciMmc;

			private int gWCBxcaliwlqNfAMjkWsWmMfeyJDc;

			private ulong TRHpYHnEZOzPiCFRhvGgraqFaIHk;

			private string[] wClCayHJAddhymSFoewTchdaCmmJb;

			private HardwareJoystickMap_InputManager feGcnxcKcQjLjbgXVAFGpotvhlOCA;

			private readonly LocalizedString KplsRMxlIAOZNhAnUkemqAztGYcq;

			public ulong NFjclaWOeFGDyvbCUOxaZSSVMSmd => TRHpYHnEZOzPiCFRhvGgraqFaIHk;

			public QolkQVxmqKDDptMUYIBZzWlOSUmp(IXboxOneInputSource P_0, ulong P_1, int P_2, bool P_3)
				: base(P_3 ? UnityTools.externalTools.XboxOneInput_GetControllerType(P_1) : "Xbox One Controller", (long)P_1, P_2, 6, 14)
			{
				JXbdVDGwkYTHAEuLldydCHldciMmc = P_0;
				gWCBxcaliwlqNfAMjkWsWmMfeyJDc = P_2 - 1;
				wClCayHJAddhymSFoewTchdaCmmJb = new string[6];
				qNzKrkxnjBzCMyVuTmEmWjjNHXrE();
				KplsRMxlIAOZNhAnUkemqAztGYcq = new LocalizedString();
				base.extension = new XboxOneGamepadExtension(true, P_0);
				_isConnected = P_3;
				if (_isConnected)
				{
					pAcFFECHKkfNPKJcSTVISMfOmwuWA(P_1);
				}
				else
				{
					TRHpYHnEZOzPiCFRhvGgraqFaIHk = P_1;
				}
			}

			public virtual void FzplAPrFIwgERkRttABATNzuGnhQ()
			{
				if (_isConnected)
				{
					IList<Button> buttons = base.Buttons;
					buttons[0].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(0);
					buttons[1].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(1);
					buttons[2].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(2);
					buttons[3].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(3);
					buttons[4].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(4);
					buttons[5].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(5);
					buttons[6].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(6);
					buttons[7].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(7);
					buttons[8].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(8);
					buttons[9].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(9);
					buttons[10].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(12);
					buttons[11].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(15);
					buttons[12].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(13);
					buttons[13].boolValue = jmGXyOqucrAeUmeqOkAjAZNNplSb(14);
					IList<Axis> axes = base.Axes;
					axes[0].value = Input.GetAxisRaw(wClCayHJAddhymSFoewTchdaCmmJb[0]);
					axes[1].value = Input.GetAxisRaw(wClCayHJAddhymSFoewTchdaCmmJb[1]);
					axes[2].value = Input.GetAxisRaw(wClCayHJAddhymSFoewTchdaCmmJb[2]);
					axes[3].value = Input.GetAxisRaw(wClCayHJAddhymSFoewTchdaCmmJb[3]);
					axes[4].value = Input.GetAxisRaw(wClCayHJAddhymSFoewTchdaCmmJb[4]);
					axes[5].value = Input.GetAxisRaw(wClCayHJAddhymSFoewTchdaCmmJb[5]);
				}
			}

			public void pAcFFECHKkfNPKJcSTVISMfOmwuWA(ulong P_0)
			{
				if (!_isConnected)
				{
					_isConnected = true;
					TRHpYHnEZOzPiCFRhvGgraqFaIHk = P_0;
					base.systemId = (long)P_0;
					if (UnityTools.externalTools.XboxOneInput_GetJoystickId(P_0) != (uint)base.unityId)
					{
						Logger.LogError("Unity joystick id does not match expected id!");
						_isConnected = false;
					}
					else
					{
						epPArKKwVVmuSaUdjYRDlZJddAzk();
					}
				}
			}

			private void epPArKKwVVmuSaUdjYRDlZJddAzk()
			{
				if (_isConnected)
				{
					_deviceName = UnityTools.externalTools.XboxOneInput_GetControllerType(TRHpYHnEZOzPiCFRhvGgraqFaIHk);
				}
				_customName = string.Format("{0} {1}", "Controller", base.unityId);
				KplsRMxlIAOZNhAnUkemqAztGYcq.Clear();
			}

			private bool jmGXyOqucrAeUmeqOkAjAZNNplSb(int P_0)
			{
				return Input.GetKey((KeyCode)(350 + P_0 + gWCBxcaliwlqNfAMjkWsWmMfeyJDc * 20));
			}

			private void qNzKrkxnjBzCMyVuTmEmWjjNHXrE()
			{
				wClCayHJAddhymSFoewTchdaCmmJb[0] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 0);
				wClCayHJAddhymSFoewTchdaCmmJb[1] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 1);
				wClCayHJAddhymSFoewTchdaCmmJb[2] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 3);
				wClCayHJAddhymSFoewTchdaCmmJb[3] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 4);
				wClCayHJAddhymSFoewTchdaCmmJb[4] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 8);
				wClCayHJAddhymSFoewTchdaCmmJb[5] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 9);
			}

			void IInputManagerHardwareJoystickMapHandler.InitializeHardwareJoystickMap(HardwareJoystickMap_InputManager hardwareMap)
			{
				feGcnxcKcQjLjbgXVAFGpotvhlOCA = hardwareMap;
			}

			bool ITryGetLocalizedName.TryGetLocalizedName(out string value)
			{
				if (feGcnxcKcQjLjbgXVAFGpotvhlOCA == null)
				{
					value = null;
					return false;
				}
				if ((LocalizationManager.GetAndUpdateLocalizedString(KplsRMxlIAOZNhAnUkemqAztGYcq, feGcnxcKcQjLjbgXVAFGpotvhlOCA.deviceLocalizationInfo.parentKeys, "controller", "Controller", out value) & LocalizationManager.GetAndUpdateLocalizedStringResultFlags.Changed) != LocalizationManager.GetAndUpdateLocalizedStringResultFlags.None)
				{
					value = $"{value} {base.unityId}";
					KplsRMxlIAOZNhAnUkemqAztGYcq.cachedValue = value;
				}
				return true;
			}
		}

		private const int pnaqpnaOZeLPzBteTkVrmRRTbSYN = 8;

		private readonly bool QADHmkTZBIYdlBCcTDovHQZILNsW;

		private bool fRbAtpMtwPjuosRusDQOcmUHOsCn;

		private Queue<oowdQnBzADoGkVjyeyEZhQdRNfer> eZWzKYbCfaVslavAjFBwSpvJpUJf;

		private bool sbpMGWdeXINfpBfpGYWfZlEHMCEd;

		bool CustomInputSource.isReady => QADHmkTZBIYdlBCcTDovHQZILNsW;

		public XboxOneInputSource()
			: base(21)
		{
			try
			{
				eZWzKYbCfaVslavAjFBwSpvJpUJf = new Queue<oowdQnBzADoGkVjyeyEZhQdRNfer>();
				base.useApproximateMatching = false;
				for (int i = 0; i < 8; i++)
				{
					int num = i + 1;
					BadConnectionReason badConnectionReason;
					bool flag = kwpvpwYnkiIDGFrIfJLMrpIrPyhgb((uint)num, true, out badConnectionReason);
					ulong num2 = (flag ? UnityTools.externalTools.XboxOneInput_GetControllerId((uint)num) : 0);
					AddJoystick(new QolkQVxmqKDDptMUYIBZzWlOSUmp(this, num2, num, flag)
					{
						supportsVibration = true
					});
				}
				UnityTools.externalTools.XboxOneInput_OnGamepadStateChange += wgPtsBxCubiAyLvPSeydfBYSieaD;
				QADHmkTZBIYdlBCcTDovHQZILNsW = true;
			}
			catch
			{
			}
		}

		public override void Update()
		{
			if (QADHmkTZBIYdlBCcTDovHQZILNsW)
			{
				bOZfnzRDdEyLuvRElXIPhrhNpzxD();
				UnityTools.externalTools.XboxOne_Gamepad_UpdatePlugin();
				IList<Joystick> joysticks = GetJoysticks();
				int count = joysticks.Count;
				for (int i = 0; i < count; i++)
				{
					joysticks[i].Update();
				}
			}
		}

		private void wgPtsBxCubiAyLvPSeydfBYSieaD(uint P_0, bool P_1)
		{
			if (!QADHmkTZBIYdlBCcTDovHQZILNsW)
			{
				return;
			}
			if (P_0 == 0)
			{
				Logger.LogError("Invalid unity joystick id");
			}
			else if (P_1)
			{
				if (kwpvpwYnkiIDGFrIfJLMrpIrPyhgb(P_0, true, out var _))
				{
					eHpTNbjwCwaWrrIxfQyEdAWbcaay(P_0, true);
				}
			}
			else
			{
				int index = (int)(P_0 - 1);
				(GetJoysticks()[index] as QolkQVxmqKDDptMUYIBZzWlOSUmp).Disconnect();
				OnJoystickDisconnected();
			}
		}

		private void eHpTNbjwCwaWrrIxfQyEdAWbcaay(uint P_0, bool P_1)
		{
			int index = (int)(P_0 - 1);
			QolkQVxmqKDDptMUYIBZzWlOSUmp obj = GetJoysticks()[index] as QolkQVxmqKDDptMUYIBZzWlOSUmp;
			ulong num = UnityTools.externalTools.XboxOneInput_GetControllerId(P_0);
			obj.pAcFFECHKkfNPKJcSTVISMfOmwuWA(num);
			if (P_1)
			{
				OnJoystickConnected();
			}
		}

		private void bOZfnzRDdEyLuvRElXIPhrhNpzxD()
		{
			int num = eZWzKYbCfaVslavAjFBwSpvJpUJf.Count;
			if (num == 0)
			{
				return;
			}
			bool flag = false;
			uint currentFrame = ReInput.time.currentFrame;
			while (num > 0)
			{
				oowdQnBzADoGkVjyeyEZhQdRNfer item = eZWzKYbCfaVslavAjFBwSpvJpUJf.Dequeue();
				if (currentFrame >= item.GNqccZaAJiPAxsNQtcjaFRTjeHxrB + 1)
				{
					if (kwpvpwYnkiIDGFrIfJLMrpIrPyhgb(item.InAeDgxibEotdDNUrNCxiXRFIFTe, true, out var _))
					{
						eHpTNbjwCwaWrrIxfQyEdAWbcaay(item.InAeDgxibEotdDNUrNCxiXRFIFTe, false);
						flag = true;
					}
				}
				else
				{
					eZWzKYbCfaVslavAjFBwSpvJpUJf.Enqueue(item);
				}
				num--;
			}
			if (flag)
			{
				OnJoystickConnected();
			}
		}

		private bool kwpvpwYnkiIDGFrIfJLMrpIrPyhgb(uint P_0, bool P_1, out BadConnectionReason P_2)
		{
			if (!UnityTools.externalTools.XboxOneInput_IsGamepadActive(P_0))
			{
				P_2 = BadConnectionReason.GamepadNotActive;
				return false;
			}
			string text = UnityTools.externalTools.XboxOneInput_GetControllerType(UnityTools.externalTools.XboxOneInput_GetControllerId(P_0));
			if (string.IsNullOrEmpty(text) || text == " ")
			{
				if (P_1)
				{
					eZWzKYbCfaVslavAjFBwSpvJpUJf.Enqueue(new oowdQnBzADoGkVjyeyEZhQdRNfer(P_0, ReInput.time.currentFrame));
				}
				P_2 = BadConnectionReason.InvalidName;
				return false;
			}
			P_2 = BadConnectionReason.None;
			return true;
		}

		private void VxmQwTKGvygxmfxjchkVvmAJJgnm()
		{
			if (!fRbAtpMtwPjuosRusDQOcmUHOsCn)
			{
				fRbAtpMtwPjuosRusDQOcmUHOsCn = true;
				Logger.LogError("A required native library is missing! See documentation for Xbox One installation instructions.");
			}
		}

		public int GetXboxOneUserIdFromUnityJoystick(int unityJoystickId)
		{
			if (!QADHmkTZBIYdlBCcTDovHQZILNsW)
			{
				return -1;
			}
			return UnityTools.externalTools.XboxOneInput_GetUserIdForGamepad((uint)unityJoystickId);
		}

		int IXboxOneInputSource.GetXboxOneUserIdFromUnityJoystick(int unityJoystickId)
		{
			//ILSpy generated this explicit interface implementation from .override directive in GetXboxOneUserIdFromUnityJoystick
			return this.GetXboxOneUserIdFromUnityJoystick(unityJoystickId);
		}

		public void PulseVibrateMotor(ulong xboxOneJoystickId, XboxOneGamepadMotorType motor, float startLevel, float endLevel, float duration)
		{
			if (QADHmkTZBIYdlBCcTDovHQZILNsW)
			{
				ulong durationMS = (ulong)(duration * 1000f);
				UnityTools.externalTools.XboxOne_Gamepad_PulseVibrateMotor(xboxOneJoystickId, (int)motor, startLevel, endLevel, durationMS);
			}
		}

		void IXboxOneInputSource.PulseVibrateMotor(ulong xboxOneJoystickId, XboxOneGamepadMotorType motor, float startLevel, float endLevel, float duration)
		{
			//ILSpy generated this explicit interface implementation from .override directive in PulseVibrateMotor
			this.PulseVibrateMotor(xboxOneJoystickId, motor, startLevel, endLevel, duration);
		}

		public bool SetXboxOneVibration(ulong xboxOneJoystickId, gPBXUmUByuNZDOTYEcbeSlXNYIzu vibration)
		{
			if (!QADHmkTZBIYdlBCcTDovHQZILNsW)
			{
				return false;
			}
			return UnityTools.externalTools.XboxOne_Gamepad_SetGamepadVibration(xboxOneJoystickId, vibration.oAhdgdFZXLtTlGjVOObkzwuLPbLJA, vibration.rKnIuNfuTPgoanctbQTxEjgfLyVO, vibration.fQBycGTqeYBbidpSCIjrrxuOvEUsA, vibration.JoWYsigeMoXdMyZBJMcXhTJOMoWp);
		}

		bool IXboxOneInputSource.SetXboxOneVibration(ulong xboxOneJoystickId, gPBXUmUByuNZDOTYEcbeSlXNYIzu vibration)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetXboxOneVibration
			return this.SetXboxOneVibration(xboxOneJoystickId, vibration);
		}

		public override void Dispose()
		{
			base.Dispose();
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		~XboxOneInputSource()
		{
			Dispose(disposing: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (!sbpMGWdeXINfpBfpGYWfZlEHMCEd)
			{
				if (disposing)
				{
					UnityTools.externalTools.XboxOneInput_OnGamepadStateChange -= wgPtsBxCubiAyLvPSeydfBYSieaD;
				}
				sbpMGWdeXINfpBfpGYWfZlEHMCEd = true;
			}
		}
	}
}
