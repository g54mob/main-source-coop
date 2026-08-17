using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Platforms;
using Rewired.Utils;
using UnityEngine;

internal sealed class rWicVimBtZDnhnNsYIsUlhoOElqkA : IElementIdentifierTool
{
	private GUIText jSjmKcccINbuVziYGRvwJIAjIuGw;

	private string sGGlhOAAzrMgSGdlahzKPbwcSPCP;

	private int zvHGHcjMfFlRwveGkBAMoTwWGJhfb;

	private AuSBfxYAktMaNvbYMEDVwcjrEcXEA LwjmourCZmetChnYCoEueXKdVsIQA;

	private JIDPHXmbYMSkzPnbLXOMHLCjPskd HrihIRfnogtAsBhpNbcOqYPftPWJ;

	private Guid TooJqKdqGbUrOJCVyhALuqNzkRps;

	private IList<JIDPHXmbYMSkzPnbLXOMHLCjPskd> QYEFOENpnjZyXGreanBNSTjZxatE;

	private bool KjoIFjuaaKJlzRZOElvRqLIFmfPu;

	private bool EKsQYwFJIACIlXQhZraXXodFSEbU;

	private bool OtniMcTXSnBeRcoesqNvcMoKgslO;

	private string[] QUcogrCLwMgIVWxNYrTjmSkfpzwJ;

	private int[] bdDMgaRmMwfffgcudIYAyueVEfZdA;

	public void Initialize(GUIText text)
	{
		jSjmKcccINbuVziYGRvwJIAjIuGw = text;
		QUcogrCLwMgIVWxNYrTjmSkfpzwJ = Enum.GetNames(typeof(RawInputAxis));
		bdDMgaRmMwfffgcudIYAyueVEfZdA = (int[])Enum.GetValues(typeof(RawInputAxis));
	}

	void IElementIdentifierTool.Initialize(GUIText text)
	{
		//ILSpy generated this explicit interface implementation from .override directive in Initialize
		this.Initialize(text);
	}

	public void Start()
	{
		if (ReInput.isEditor && ReInput.editorPlatform != EditorPlatform.Windows)
		{
			Rewired.Logger.LogError("Raw Input cannot be run on this platform. You must be running the editor in Windows.");
			return;
		}
		if (ReInput.currentPlatform != Platform.Windows)
		{
			Rewired.Logger.LogError("Raw Input cannot be run on this build target. Be sure Unity's build target is set to Windows Standalone.");
			return;
		}
		LwjmourCZmetChnYCoEueXKdVsIQA = ReInput.primaryInputManager.inputSource as AuSBfxYAktMaNvbYMEDVwcjrEcXEA;
		if (LwjmourCZmetChnYCoEueXKdVsIQA == null)
		{
			Rewired.Logger.LogError("Unable to initialize Raw Input! You must add a Rewired Input Manager to the scene and set the input mode to Raw Input.");
			return;
		}
		ReInput.primaryInputManager.SystemDeviceConnectedEvent += GGlRmATwJLUXNlubnqqJcMHHMNMQ;
		ReInput.primaryInputManager.SystemDeviceDisconnectedEvent += RypcwgcwnnRvcUqNdvJKoZJHlSneA;
		rHsLMjoyBzbEbKjWLtaPxIWywPhV();
		OtniMcTXSnBeRcoesqNvcMoKgslO = true;
	}

	void IElementIdentifierTool.Start()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Start
		this.Start();
	}

	public void Update()
	{
		if (!OtniMcTXSnBeRcoesqNvcMoKgslO)
		{
			return;
		}
		sGGlhOAAzrMgSGdlahzKPbwcSPCP = "Raw Input Joystick Element Identifier\n\n";
		jSjmKcccINbuVziYGRvwJIAjIuGw.text = sGGlhOAAzrMgSGdlahzKPbwcSPCP;
		int num = zvHGHcjMfFlRwveGkBAMoTwWGJhfb;
		Guid tooJqKdqGbUrOJCVyhALuqNzkRps = TooJqKdqGbUrOJCVyhALuqNzkRps;
		if (ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Equals) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Plus) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.KeypadPlus))
		{
			zvHGHcjMfFlRwveGkBAMoTwWGJhfb++;
		}
		if (ReInput.controllers.Keyboard.GetKeyDown(KeyCode.KeypadMinus) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Minus))
		{
			zvHGHcjMfFlRwveGkBAMoTwWGJhfb--;
		}
		if (EKsQYwFJIACIlXQhZraXXodFSEbU)
		{
			rHsLMjoyBzbEbKjWLtaPxIWywPhV();
			EKsQYwFJIACIlXQhZraXXodFSEbU = false;
		}
		int num2 = ((QYEFOENpnjZyXGreanBNSTjZxatE != null) ? QYEFOENpnjZyXGreanBNSTjZxatE.Count : 0);
		if (num2 == 0)
		{
			return;
		}
		if (zvHGHcjMfFlRwveGkBAMoTwWGJhfb < 0)
		{
			zvHGHcjMfFlRwveGkBAMoTwWGJhfb = num2 - 1;
		}
		else if (zvHGHcjMfFlRwveGkBAMoTwWGJhfb >= num2)
		{
			zvHGHcjMfFlRwveGkBAMoTwWGJhfb = 0;
		}
		TooJqKdqGbUrOJCVyhALuqNzkRps = QYEFOENpnjZyXGreanBNSTjZxatE[zvHGHcjMfFlRwveGkBAMoTwWGJhfb].eeLYrwPQKctoTapvzRLHAtZIfwnV;
		bool flag = false;
		if (num != zvHGHcjMfFlRwveGkBAMoTwWGJhfb || tooJqKdqGbUrOJCVyhALuqNzkRps != TooJqKdqGbUrOJCVyhALuqNzkRps)
		{
			flag = true;
		}
		if (HrihIRfnogtAsBhpNbcOqYPftPWJ == null || flag)
		{
			if (HrihIRfnogtAsBhpNbcOqYPftPWJ != null)
			{
				HrihIRfnogtAsBhpNbcOqYPftPWJ.YBWYuhQDEVtIJrpKPUKYfjuVBycQ();
			}
			HrihIRfnogtAsBhpNbcOqYPftPWJ = QYEFOENpnjZyXGreanBNSTjZxatE[zvHGHcjMfFlRwveGkBAMoTwWGJhfb];
			if (HrihIRfnogtAsBhpNbcOqYPftPWJ == null)
			{
				return;
			}
			HrihIRfnogtAsBhpNbcOqYPftPWJ.kvqcLvPweCnbRsOECVFMUPhBdLCN();
		}
		bool flag2 = false;
		if (HrihIRfnogtAsBhpNbcOqYPftPWJ.sbmUGmkfgCgHIFaOCErQOcpRkUkp is tSoAuFePUqEfqeHjHNvjyrcIaCSLA)
		{
			flag2 = true;
		}
		else if (!(HrihIRfnogtAsBhpNbcOqYPftPWJ.sbmUGmkfgCgHIFaOCErQOcpRkUkp is ooMJwxMVKWpFygFkttyIRbnIpFzV))
		{
			return;
		}
		if (num2 > 0)
		{
			sGGlhOAAzrMgSGdlahzKPbwcSPCP = sGGlhOAAzrMgSGdlahzKPbwcSPCP + num2 + " connected devices:\n";
		}
		for (int i = 0; i < num2; i++)
		{
			sGGlhOAAzrMgSGdlahzKPbwcSPCP = sGGlhOAAzrMgSGdlahzKPbwcSPCP + QYEFOENpnjZyXGreanBNSTjZxatE[i].OeGYfNLnWbjgMjPkTgdrilHpguDCb + "\n";
		}
		sGGlhOAAzrMgSGdlahzKPbwcSPCP += "\n";
		sGGlhOAAzrMgSGdlahzKPbwcSPCP = sGGlhOAAzrMgSGdlahzKPbwcSPCP + "Current RI device " + zvHGHcjMfFlRwveGkBAMoTwWGJhfb + ": \"" + HrihIRfnogtAsBhpNbcOqYPftPWJ.OeGYfNLnWbjgMjPkTgdrilHpguDCb + "\"\n";
		sGGlhOAAzrMgSGdlahzKPbwcSPCP += "(Press + or - to change monitored device id.)\n\n";
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Product Name", "\"" + HrihIRfnogtAsBhpNbcOqYPftPWJ.OeGYfNLnWbjgMjPkTgdrilHpguDCb + "\"");
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Is Bluetooth Device", HrihIRfnogtAsBhpNbcOqYPftPWJ.OVAmXCmTjKlJhFrwXPxhSgJZucaM);
		if (HrihIRfnogtAsBhpNbcOqYPftPWJ.OVAmXCmTjKlJhFrwXPxhSgJZucaM)
		{
			MzGQfkTIENESCLneDXRAbdZgeDKJA("Bluetooth Device Name", "\"" + HrihIRfnogtAsBhpNbcOqYPftPWJ.FZebZwAYjUaXGKBUheDcqiDVcAzcA + "\"");
		}
		if (flag2)
		{
			MzGQfkTIENESCLneDXRAbdZgeDKJA("Using Custom Driver", "TRUE");
		}
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Device Type", HrihIRfnogtAsBhpNbcOqYPftPWJ.tEwPjlwcvVfzfflaOHoSKOTEjEmz.ToString());
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Identifier", new PidVid(HrihIRfnogtAsBhpNbcOqYPftPWJ.ibSeINshSGScITkFjKtcOMvvNXjb));
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Product Id", HrihIRfnogtAsBhpNbcOqYPftPWJ.MlUWuQbZqNWzsYRXZioNJpBxWurv);
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Vendor Id", HrihIRfnogtAsBhpNbcOqYPftPWJ.MXYCKUZFcJBUCZiuMtejjGoCRoFK);
		sGGlhOAAzrMgSGdlahzKPbwcSPCP += "\n";
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Axis Count", HrihIRfnogtAsBhpNbcOqYPftPWJ.dZoMCESFJjGeFSCXDeONdZuyeOCDA);
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Button Count", HrihIRfnogtAsBhpNbcOqYPftPWJ.PmrsxLoxUwGFsxXQTOWpQFpRcinN);
		MzGQfkTIENESCLneDXRAbdZgeDKJA("Hat Count", HrihIRfnogtAsBhpNbcOqYPftPWJ.fWUbcorZqVMccGMcDGDlaEupdJAQ);
		sGGlhOAAzrMgSGdlahzKPbwcSPCP += "\n";
		if (flag)
		{
			string text = "";
			text = text + "Device Name: \"" + QYEFOENpnjZyXGreanBNSTjZxatE[zvHGHcjMfFlRwveGkBAMoTwWGJhfb].OeGYfNLnWbjgMjPkTgdrilHpguDCb + "\"\n";
			if (HrihIRfnogtAsBhpNbcOqYPftPWJ.OVAmXCmTjKlJhFrwXPxhSgJZucaM)
			{
				text = text + "Bluetooth Device Name: \"" + HrihIRfnogtAsBhpNbcOqYPftPWJ.FZebZwAYjUaXGKBUheDcqiDVcAzcA + "\"\n";
			}
			text = text + "Identifier: " + new PidVid(HrihIRfnogtAsBhpNbcOqYPftPWJ.ibSeINshSGScITkFjKtcOMvvNXjb).ToString() + "\n";
			Rewired.Logger.Log(text);
		}
		if (!flag2)
		{
			ooMJwxMVKWpFygFkttyIRbnIpFzV ooMJwxMVKWpFygFkttyIRbnIpFzV2 = HrihIRfnogtAsBhpNbcOqYPftPWJ.sbmUGmkfgCgHIFaOCErQOcpRkUkp as ooMJwxMVKWpFygFkttyIRbnIpFzV;
			for (int j = 1; j < QUcogrCLwMgIVWxNYrTjmSkfpzwJ.Length - 1; j++)
			{
				int num3 = QhDqBrIMWlWjFfrfREpYeAEaVFkx((RawInputAxis)bdDMgaRmMwfffgcudIYAyueVEfZdA[j], 0, ooMJwxMVKWpFygFkttyIRbnIpFzV2);
				string text2 = QUcogrCLwMgIVWxNYrTjmSkfpzwJ[j];
				try
				{
					MzGQfkTIENESCLneDXRAbdZgeDKJA(text2, num3 + " (" + qQkKWOPKbhBIdFvVsIrDfmpvZUZN(num3) + ")");
				}
				catch
				{
					MzGQfkTIENESCLneDXRAbdZgeDKJA(text2, "FAILED! Axis value = " + num3);
				}
			}
			if (ooMJwxMVKWpFygFkttyIRbnIpFzV2.SgcSQyIsIwMZIYRdrVSoZnGnbZZR > 0)
			{
				for (int k = 0; k < ooMJwxMVKWpFygFkttyIRbnIpFzV2.SgcSQyIsIwMZIYRdrVSoZnGnbZZR; k++)
				{
					int num4 = QhDqBrIMWlWjFfrfREpYeAEaVFkx(RawInputAxis.Other, k, ooMJwxMVKWpFygFkttyIRbnIpFzV2);
					string text3 = "Other Axis " + k;
					try
					{
						MzGQfkTIENESCLneDXRAbdZgeDKJA(text3, num4 + " (" + qQkKWOPKbhBIdFvVsIrDfmpvZUZN(num4) + ")");
					}
					catch
					{
						MzGQfkTIENESCLneDXRAbdZgeDKJA(text3, "FAILED! Axis value = " + num4);
					}
				}
			}
			int[] array = HrihIRfnogtAsBhpNbcOqYPftPWJ.bqdIFfNDwwaOWKnvaZbevVyJnpGy;
			for (int l = 0; l < array.Length; l++)
			{
				int num5 = array[l];
				string text4 = "Hat " + l;
				MzGQfkTIENESCLneDXRAbdZgeDKJA(text4, num5);
			}
			bool[] array2 = HrihIRfnogtAsBhpNbcOqYPftPWJ.yqNATulmpNqcrGqZkcEmAOMHzLyvA;
			string text5 = "";
			for (int m = 0; m < array2.Length; m++)
			{
				if (array2[m])
				{
					if (text5 != "")
					{
						text5 += ", ";
					}
					text5 += m;
				}
			}
			MzGQfkTIENESCLneDXRAbdZgeDKJA("Buttons ", text5);
		}
		else
		{
			tSoAuFePUqEfqeHjHNvjyrcIaCSLA tSoAuFePUqEfqeHjHNvjyrcIaCSLA2 = HrihIRfnogtAsBhpNbcOqYPftPWJ.sbmUGmkfgCgHIFaOCErQOcpRkUkp as tSoAuFePUqEfqeHjHNvjyrcIaCSLA;
			for (int n = 0; n < HrihIRfnogtAsBhpNbcOqYPftPWJ.dZoMCESFJjGeFSCXDeONdZuyeOCDA; n++)
			{
				float num6 = tSoAuFePUqEfqeHjHNvjyrcIaCSLA2.IcMCQfMtfMCEJnpccEDXKUPAkKMM(n);
				string text6 = n.ToString();
				try
				{
					MzGQfkTIENESCLneDXRAbdZgeDKJA(text6, num6 + " (" + tSoAuFePUqEfqeHjHNvjyrcIaCSLA2.zeakccAttMgLEGhFtxFxhozCFmsNA(n) + ")");
				}
				catch
				{
					MzGQfkTIENESCLneDXRAbdZgeDKJA(text6, "FAILED! Axis value = " + num6);
				}
			}
			int[] array3 = HrihIRfnogtAsBhpNbcOqYPftPWJ.bqdIFfNDwwaOWKnvaZbevVyJnpGy;
			for (int num7 = 0; num7 < HrihIRfnogtAsBhpNbcOqYPftPWJ.fWUbcorZqVMccGMcDGDlaEupdJAQ; num7++)
			{
				int num8 = array3[num7];
				string text7 = "Hat " + num7;
				MzGQfkTIENESCLneDXRAbdZgeDKJA(text7, num8);
			}
			for (int num9 = 0; num9 < HrihIRfnogtAsBhpNbcOqYPftPWJ.FPEaWPeThpkGJZVEKOwWmJJBhkYwA.Rewired_002EHID_002EDrivers_002EIControllerDriver_002EGyroscopeCount; num9++)
			{
				int iTpGZghAGXrXxXzLlPVWVbuNgZkFA = HrihIRfnogtAsBhpNbcOqYPftPWJ.FPEaWPeThpkGJZVEKOwWmJJBhkYwA.gyroscopes[num9].iTpGZghAGXrXxXzLlPVWVbuNgZkFA;
				string text8 = "";
				for (int num10 = 0; num10 < iTpGZghAGXrXxXzLlPVWVbuNgZkFA; num10++)
				{
					float num11 = HrihIRfnogtAsBhpNbcOqYPftPWJ.FPEaWPeThpkGJZVEKOwWmJJBhkYwA.gyroscopes[num9].OrthfcEpPRtmJfLlFdtCctIoezeQ[num10];
					text8 = text8 + "[" + num10 + "]: " + num11.ToString("f3");
					if (num10 < iTpGZghAGXrXxXzLlPVWVbuNgZkFA - 1)
					{
						text8 += " ";
					}
				}
				MzGQfkTIENESCLneDXRAbdZgeDKJA("Gyro " + num9, text8);
			}
			for (int num12 = 0; num12 < HrihIRfnogtAsBhpNbcOqYPftPWJ.FPEaWPeThpkGJZVEKOwWmJJBhkYwA.Rewired_002EHID_002EDrivers_002EIControllerDriver_002EAccelerometerCount; num12++)
			{
				int xEUekQvCarjTbCYxMKFCWVhhQrjsA = HrihIRfnogtAsBhpNbcOqYPftPWJ.FPEaWPeThpkGJZVEKOwWmJJBhkYwA.accelerometers[num12].XEUekQvCarjTbCYxMKFCWVhhQrjsA;
				string text9 = "";
				for (int num13 = 0; num13 < xEUekQvCarjTbCYxMKFCWVhhQrjsA; num13++)
				{
					float num14 = HrihIRfnogtAsBhpNbcOqYPftPWJ.FPEaWPeThpkGJZVEKOwWmJJBhkYwA.accelerometers[num12].SytbxvDfrRdLDWckugtMRDSBscWP[num13];
					text9 = text9 + "[" + num13 + "]: " + num14.ToString("f3");
					if (num13 < xEUekQvCarjTbCYxMKFCWVhhQrjsA - 1)
					{
						text9 += " ";
					}
				}
				MzGQfkTIENESCLneDXRAbdZgeDKJA("Accelerometer " + num12, text9);
			}
			for (int num15 = 0; num15 < HrihIRfnogtAsBhpNbcOqYPftPWJ.FPEaWPeThpkGJZVEKOwWmJJBhkYwA.Rewired_002EHID_002EDrivers_002EIControllerDriver_002ETouchpadCount; num15++)
			{
				JeEihaxNGDZUEopEZTyRorKoTSAm jeEihaxNGDZUEopEZTyRorKoTSAm = HrihIRfnogtAsBhpNbcOqYPftPWJ.FPEaWPeThpkGJZVEKOwWmJJBhkYwA.touchpads[num15];
				int num16 = jeEihaxNGDZUEopEZTyRorKoTSAm.SBWbRIEBtbRxLkclWCpSvIwxSXTqA.Length;
				string text10 = "";
				for (int num17 = 0; num17 < num16; num17++)
				{
					JeEihaxNGDZUEopEZTyRorKoTSAm.TouchData touchData = jeEihaxNGDZUEopEZTyRorKoTSAm.SBWbRIEBtbRxLkclWCpSvIwxSXTqA[num17];
					text10 = text10 + "Touch " + num17 + ": Is Touching = " + touchData.isTouching + "\n";
					text10 = text10 + "Touch " + num17 + ": Touch Id = " + touchData.touchId + "\n";
					text10 = text10 + "Touch " + num17 + ": Position = " + touchData.positionX + ", " + touchData.positionY + "\n";
					text10 = text10 + "Touch " + num17 + ": Abs Position = " + touchData.positionAbsX + ", " + touchData.positionAbsY + " (" + touchData.positionRawX + ", " + touchData.positionRawY + ")\n";
				}
				aAkeCKLclCvQdYLJCQOCKRNcgDEAA("Touchpad " + num15, text10);
			}
			bool[] array4 = HrihIRfnogtAsBhpNbcOqYPftPWJ.yqNATulmpNqcrGqZkcEmAOMHzLyvA;
			string text11 = "";
			for (int num18 = 0; num18 < array4.Length; num18++)
			{
				if (array4[num18])
				{
					if (text11 != "")
					{
						text11 += ", ";
					}
					text11 += num18;
				}
			}
			MzGQfkTIENESCLneDXRAbdZgeDKJA("Buttons ", text11);
		}
		jSjmKcccINbuVziYGRvwJIAjIuGw.text = sGGlhOAAzrMgSGdlahzKPbwcSPCP;
	}

	void IElementIdentifierTool.Update()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Update
		this.Update();
	}

	public void OnDestroy()
	{
		if (HrihIRfnogtAsBhpNbcOqYPftPWJ != null)
		{
			HrihIRfnogtAsBhpNbcOqYPftPWJ.YBWYuhQDEVtIJrpKPUKYfjuVBycQ();
		}
	}

	void IElementIdentifierTool.OnDestroy()
	{
		//ILSpy generated this explicit interface implementation from .override directive in OnDestroy
		this.OnDestroy();
	}

	private void rHsLMjoyBzbEbKjWLtaPxIWywPhV()
	{
		QYEFOENpnjZyXGreanBNSTjZxatE = LwjmourCZmetChnYCoEueXKdVsIQA.GetJoysticks<JIDPHXmbYMSkzPnbLXOMHLCjPskd>();
	}

	private void GGlRmATwJLUXNlubnqqJcMHHMNMQ()
	{
		arYOoldHBpqaBDSqGDFUfmMkCkvDA();
	}

	private void RypcwgcwnnRvcUqNdvJKoZJHlSneA()
	{
		arYOoldHBpqaBDSqGDFUfmMkCkvDA();
	}

	private void arYOoldHBpqaBDSqGDFUfmMkCkvDA()
	{
		mZvWdQGsJtKmwmTBoSrweasrpoQg();
		EKsQYwFJIACIlXQhZraXXodFSEbU = true;
	}

	private void mZvWdQGsJtKmwmTBoSrweasrpoQg()
	{
		zvHGHcjMfFlRwveGkBAMoTwWGJhfb = 0;
		HrihIRfnogtAsBhpNbcOqYPftPWJ = null;
		TooJqKdqGbUrOJCVyhALuqNzkRps = Guid.Empty;
		QYEFOENpnjZyXGreanBNSTjZxatE = null;
		KjoIFjuaaKJlzRZOElvRqLIFmfPu = false;
		EKsQYwFJIACIlXQhZraXXodFSEbU = false;
	}

	private void MzGQfkTIENESCLneDXRAbdZgeDKJA(string P_0, object P_1)
	{
		sGGlhOAAzrMgSGdlahzKPbwcSPCP = sGGlhOAAzrMgSGdlahzKPbwcSPCP + P_0 + " = " + P_1.ToString() + "\n";
	}

	private void aAkeCKLclCvQdYLJCQOCKRNcgDEAA(string P_0, object P_1)
	{
		sGGlhOAAzrMgSGdlahzKPbwcSPCP = sGGlhOAAzrMgSGdlahzKPbwcSPCP + P_0 + ":\n" + P_1.ToString() + "\n";
	}

	private int QhDqBrIMWlWjFfrfREpYeAEaVFkx(RawInputAxis P_0, int P_1, ooMJwxMVKWpFygFkttyIRbnIpFzV P_2)
	{
		return P_2.uiYQrpyDzwfXgpSBreDzMReEEzCDA(P_0, P_1);
	}

	private float qQkKWOPKbhBIdFvVsIrDfmpvZUZN(int P_0)
	{
		if (P_0 == 0)
		{
			return 0f;
		}
		return MathTools.Clamp((float)MathTools.Abs(P_0) / 65535f * (float)MathTools.Sign(P_0), -1f, 1f);
	}
}
