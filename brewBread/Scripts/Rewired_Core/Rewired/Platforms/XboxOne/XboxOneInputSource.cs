using System;
using System.Collections.Generic;
using Rewired.Platforms.Custom;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Platforms.XboxOne
{
	[CustomClassObfuscation(renamePrivateMembers = true, renamePubIntMembers = false)]
	[CustomObfuscation(rename = false)]
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

		private struct KeIKjldUhnrhbxEDCqQPYBcpLrnR
		{
			public uint XiLNYbgSQRueYGNGMfEwcQjqkIZiA;

			public uint wqiepMikKKhrqfHDUTjbEWFFYBPIb;

			public KeIKjldUhnrhbxEDCqQPYBcpLrnR(uint P_0, uint P_1)
			{
				XiLNYbgSQRueYGNGMfEwcQjqkIZiA = P_0;
				wqiepMikKKhrqfHDUTjbEWFFYBPIb = P_1;
			}
		}

		private class eeJCpJPyLyCauVmvuyRDEsgwvGdw : Joystick
		{
			private const int gsGmLwrXoBgMTRxYcaAlQzdPHlrW = 6;

			private const int QJpgCvdDxIIaukCWckDkuElRUIGE = 14;

			private const string JwaphnPaUoJIIOIgqmhyFidKGhSY = "Xbox One Controller";

			private const int qUiugTeloUXriUCZWitGxvwqBTpG = 0;

			private const int PMTcWxgBGqrrsmdDsKEpuLgvLCMHb = 1;

			private const int sBDononqHdTICbehgCWBCxLJWhLJA = 2;

			private const int RobLIXFHJaZTIuGNpPggoTNFWYJg = 3;

			private const int OLtYHFMCebhSPKLnhAEJsFWFpRvzA = 4;

			private const int nTpxpfwKRnnahuFcAKEMAGiYsUYb = 5;

			private const int XSqRwGmVrTmrtqtPaqeaBbYcTquv = 6;

			private const int eDrjJSACLzfyeSGdQDHQDqeSVetP = 7;

			private const int UVRhJgFJEzpHkKAPSwyNTzbQJNnK = 8;

			private const int qTgMhBeuQuXijFRAuRVJaZGsdkTC = 9;

			private const int JJHaBLvqqhMwhmhycJYiLtTcxsNm = 12;

			private const int EGTnswDnUTyGscxAMWUHwOCgtXwf = 13;

			private const int mzYcsWHPMatIUKclOpHMCFmBThahA = 14;

			private const int yybuWiidghDaiFCnHAOFidzWRxbe = 15;

			private const int wqDQNcMHQsBMqioDfSoxMBYKEfHgA = 0;

			private const int TgwhLCuZcQlCaYrSxhGVLugWuCJk = 1;

			private const int MXZZBPuGSCVAqfuKUIVIQcpWzxGm = 3;

			private const int FMSFanUpiPerAMsCPSUSAvMUvbIv = 4;

			private const int cgsKosdYeirRscAvaGGUlFZXQXFQ = 8;

			private const int LHlLyhjzzVuvUrKKmeyzJQxYTnTX = 9;

			private readonly IXboxOneInputSource twtzIKfuPcwLuTdbfRDPmfUOUduI;

			private int ulCqkHwLZwjESRELyVoUMXJhfSsw;

			private ulong bFoBjtBxgrnmejxgvrbzdRweMJUJA;

			private string[] KIqgBODrHnRXqFsGFkLbIspBllAAB;

			public ulong cESFkGAYYyYTkrfbYrWVDzoaFMliA => bFoBjtBxgrnmejxgvrbzdRweMJUJA;

			public eeJCpJPyLyCauVmvuyRDEsgwvGdw(IXboxOneInputSource P_0, ulong P_1, int P_2, bool P_3)
				: base(P_3 ? UnityTools.externalTools.XboxOneInput_GetControllerType(P_1) : "Xbox One Controller", (long)P_1, P_2, 6, 14)
			{
				twtzIKfuPcwLuTdbfRDPmfUOUduI = P_0;
				ulCqkHwLZwjESRELyVoUMXJhfSsw = P_2 - 1;
				KIqgBODrHnRXqFsGFkLbIspBllAAB = new string[6];
				rPtSeZBOGGRFLDbCvUnzwwfdVeJb();
				base.extension = new XboxOneGamepadExtension(true, P_0);
				_isConnected = P_3;
				if (_isConnected)
				{
					zQQfvDZMmpVqPPLYlLuSJXXpwJcI(P_1);
				}
				else
				{
					bFoBjtBxgrnmejxgvrbzdRweMJUJA = P_1;
				}
			}

			public virtual void jRaYtHNVcykNMAbqOnSGaKIIGSEaA()
			{
				if (_isConnected)
				{
					IList<Button> buttons = base.Buttons;
					buttons[0].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(0);
					buttons[1].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(1);
					buttons[2].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(2);
					buttons[3].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(3);
					buttons[4].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(4);
					buttons[5].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(5);
					buttons[6].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(6);
					buttons[7].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(7);
					buttons[8].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(8);
					buttons[9].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(9);
					buttons[10].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(12);
					buttons[11].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(15);
					buttons[12].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(13);
					buttons[13].value = IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(14);
					IList<Axis> axes = base.Axes;
					axes[0].value = Input.GetAxisRaw(KIqgBODrHnRXqFsGFkLbIspBllAAB[0]);
					axes[1].value = Input.GetAxisRaw(KIqgBODrHnRXqFsGFkLbIspBllAAB[1]);
					axes[2].value = Input.GetAxisRaw(KIqgBODrHnRXqFsGFkLbIspBllAAB[2]);
					axes[3].value = Input.GetAxisRaw(KIqgBODrHnRXqFsGFkLbIspBllAAB[3]);
					axes[4].value = Input.GetAxisRaw(KIqgBODrHnRXqFsGFkLbIspBllAAB[4]);
					axes[5].value = Input.GetAxisRaw(KIqgBODrHnRXqFsGFkLbIspBllAAB[5]);
				}
			}

			public void zQQfvDZMmpVqPPLYlLuSJXXpwJcI(ulong P_0)
			{
				if (!_isConnected)
				{
					_isConnected = true;
					bFoBjtBxgrnmejxgvrbzdRweMJUJA = P_0;
					base.systemId = (long)P_0;
					if (UnityTools.externalTools.XboxOneInput_GetJoystickId(P_0) != (uint)base.unityId)
					{
						Logger.LogError("Unity joystick id does not match expected id!");
						_isConnected = false;
					}
					else
					{
						EkXCrrMAdYVgltAlInmpCPmGNlBs();
					}
				}
			}

			private void EkXCrrMAdYVgltAlInmpCPmGNlBs()
			{
				if (_isConnected)
				{
					_deviceName = UnityTools.externalTools.XboxOneInput_GetControllerType(bFoBjtBxgrnmejxgvrbzdRweMJUJA);
				}
				_customName = "Controller " + base.unityId;
			}

			private bool IKCLDPTiIKTdmSlZPgUWaOkbUbhVA(int P_0)
			{
				return Input.GetKey((KeyCode)(350 + P_0 + ulCqkHwLZwjESRELyVoUMXJhfSsw * 20));
			}

			private void rPtSeZBOGGRFLDbCvUnzwwfdVeJb()
			{
				KIqgBODrHnRXqFsGFkLbIspBllAAB[0] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 0);
				KIqgBODrHnRXqFsGFkLbIspBllAAB[1] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 1);
				KIqgBODrHnRXqFsGFkLbIspBllAAB[2] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 3);
				KIqgBODrHnRXqFsGFkLbIspBllAAB[3] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 4);
				KIqgBODrHnRXqFsGFkLbIspBllAAB[4] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 8);
				KIqgBODrHnRXqFsGFkLbIspBllAAB[5] = UnityTools.GetUnityInputAxisNameByJoystickId(base.unityId, 9);
			}
		}

		private const int fzWMVEkzlXkeohUTSiqkISuOrVlk = 8;

		private readonly bool jYTNgflwwEgvgbZuuTHYnPAnuRao;

		private bool UjrhGzAcMWgJurSxdyOsINQzfSuz;

		private Queue<KeIKjldUhnrhbxEDCqQPYBcpLrnR> fUtTCVwuOUQtKQMgsuQVPwKFAbYI;

		private bool AeWaeWamxRrERkciQkpFDWbRfZMkA;

		public override bool isReady => jYTNgflwwEgvgbZuuTHYnPAnuRao;

		public XboxOneInputSource()
			: base(21)
		{
			try
			{
				fUtTCVwuOUQtKQMgsuQVPwKFAbYI = new Queue<KeIKjldUhnrhbxEDCqQPYBcpLrnR>();
				base.useApproximateMatching = false;
				for (int i = 0; i < 8; i++)
				{
					int num = i + 1;
					BadConnectionReason badConnectionReason;
					bool flag = HIJBhUruJGvuQLxKyWqTuTalHYru((uint)num, true, out badConnectionReason);
					ulong num2 = (flag ? UnityTools.externalTools.XboxOneInput_GetControllerId((uint)num) : 0);
					AddJoystick(new eeJCpJPyLyCauVmvuyRDEsgwvGdw(this, num2, num, flag)
					{
						supportsVibration = true
					});
				}
				UnityTools.externalTools.XboxOneInput_OnGamepadStateChange += dcIWkUgDjpHhJaJAhasLaITZpIezA;
				jYTNgflwwEgvgbZuuTHYnPAnuRao = true;
			}
			catch
			{
			}
		}

		public override void Update()
		{
			if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				cqzbpIfBfoyibbCQFIZeDCjQRzcgb();
				UnityTools.externalTools.XboxOne_Gamepad_UpdatePlugin();
				IList<Joystick> joysticks = GetJoysticks();
				int count = joysticks.Count;
				for (int i = 0; i < count; i++)
				{
					joysticks[i].Update();
				}
			}
		}

		private void dcIWkUgDjpHhJaJAhasLaITZpIezA(uint P_0, bool P_1)
		{
			if (!jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return;
			}
			if (P_0 == 0)
			{
				Logger.LogError("Invalid unity joystick id");
			}
			else if (P_1)
			{
				if (HIJBhUruJGvuQLxKyWqTuTalHYru(P_0, true, out var _))
				{
					TCOVEDFPeYeNnjpNbQHuiRleVdNuB(P_0, true);
				}
			}
			else
			{
				int index = (int)(P_0 - 1);
				(GetJoysticks()[index] as eeJCpJPyLyCauVmvuyRDEsgwvGdw).Disconnect();
				OnJoystickDisconnected();
			}
		}

		private void TCOVEDFPeYeNnjpNbQHuiRleVdNuB(uint P_0, bool P_1)
		{
			int index = (int)(P_0 - 1);
			eeJCpJPyLyCauVmvuyRDEsgwvGdw obj = GetJoysticks()[index] as eeJCpJPyLyCauVmvuyRDEsgwvGdw;
			ulong num = UnityTools.externalTools.XboxOneInput_GetControllerId(P_0);
			obj.zQQfvDZMmpVqPPLYlLuSJXXpwJcI(num);
			if (P_1)
			{
				OnJoystickConnected();
			}
		}

		private void cqzbpIfBfoyibbCQFIZeDCjQRzcgb()
		{
			int num = fUtTCVwuOUQtKQMgsuQVPwKFAbYI.Count;
			if (num == 0)
			{
				return;
			}
			bool flag = false;
			uint currentFrame = ReInput.time.currentFrame;
			while (num > 0)
			{
				KeIKjldUhnrhbxEDCqQPYBcpLrnR item = fUtTCVwuOUQtKQMgsuQVPwKFAbYI.Dequeue();
				if (currentFrame >= item.wqiepMikKKhrqfHDUTjbEWFFYBPIb + 1)
				{
					if (HIJBhUruJGvuQLxKyWqTuTalHYru(item.XiLNYbgSQRueYGNGMfEwcQjqkIZiA, true, out var _))
					{
						TCOVEDFPeYeNnjpNbQHuiRleVdNuB(item.XiLNYbgSQRueYGNGMfEwcQjqkIZiA, false);
						flag = true;
					}
				}
				else
				{
					fUtTCVwuOUQtKQMgsuQVPwKFAbYI.Enqueue(item);
				}
				num--;
			}
			if (flag)
			{
				OnJoystickConnected();
			}
		}

		private bool HIJBhUruJGvuQLxKyWqTuTalHYru(uint P_0, bool P_1, out BadConnectionReason P_2)
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
					fUtTCVwuOUQtKQMgsuQVPwKFAbYI.Enqueue(new KeIKjldUhnrhbxEDCqQPYBcpLrnR(P_0, ReInput.time.currentFrame));
				}
				P_2 = BadConnectionReason.InvalidName;
				return false;
			}
			P_2 = BadConnectionReason.None;
			return true;
		}

		private void JMUWNbgLOsDmWtxiHRyYWlXCKWUl()
		{
			if (!UjrhGzAcMWgJurSxdyOsINQzfSuz)
			{
				UjrhGzAcMWgJurSxdyOsINQzfSuz = true;
				Logger.LogError("A required native library is missing! See documentation for Xbox One installation instructions.");
			}
		}

		public int GetXboxOneUserIdFromUnityJoystick(int unityJoystickId)
		{
			if (!jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return -1;
			}
			return UnityTools.externalTools.XboxOneInput_GetUserIdForGamepad((uint)unityJoystickId);
		}

		public void PulseVibrateMotor(ulong xboxOneJoystickId, XboxOneGamepadMotorType motor, float startLevel, float endLevel, float duration)
		{
			if (jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				ulong durationMS = (ulong)(duration * 1000f);
				UnityTools.externalTools.XboxOne_Gamepad_PulseVibrateMotor(xboxOneJoystickId, (int)motor, startLevel, endLevel, durationMS);
			}
		}

		public bool SetXboxOneVibration(ulong xboxOneJoystickId, OCxnhoeHJSmdQcYpgnyghAYbUGqI vibration)
		{
			if (!jYTNgflwwEgvgbZuuTHYnPAnuRao)
			{
				return false;
			}
			return UnityTools.externalTools.XboxOne_Gamepad_SetGamepadVibration(xboxOneJoystickId, vibration.iYnwtFXqkYqnGUSQFZDESWIzVLVj, vibration.ustXDwUALhHRVQXlEyYvEvvIQJsc, vibration.wTCGMUooHghhIgaiqISLfqkiHMvBc, vibration.dOBWtOmULbMIFyuANzzkOjTUWQTe);
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
			if (!AeWaeWamxRrERkciQkpFDWbRfZMkA)
			{
				if (disposing)
				{
					UnityTools.externalTools.XboxOneInput_OnGamepadStateChange -= dcIWkUgDjpHhJaJAhasLaITZpIezA;
				}
				AeWaeWamxRrERkciQkpFDWbRfZMkA = true;
			}
		}
	}
}
