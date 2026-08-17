using System;
using System.Collections.Generic;
using Rewired;
using Rewired.HID;
using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Platforms;
using Rewired.Utils;
using UnityEngine;

internal sealed class ePgLYuadsDMdgfpWdCYYYFmXGbnM : IElementIdentifierTool
{
	private Rewired.Internal.GUIText DWMQwVRyhyHhDDgjbtCayJLAZKPw;

	private string jwECMxSvaMhNDKjcsJCsxsTTpOoD;

	private int oxEZsmOAsmRKMdpdNdBMzxOCCEBIA;

	private pYDDNhBibHfSZzyMuFjJSOUJKDWk MZpweTLKzUXrzNGXSlQtLnMgOvOs;

	private gNzuXmbvyelIPcqSZuxQNOcbBnvA lTvcYXgAOKmjzpfkXhzOEzdVloT;

	private Guid HegwfvpxQkEttDMRtMMCrTxaIPOl;

	private IList<gNzuXmbvyelIPcqSZuxQNOcbBnvA> btjMNRMAgftRTvEiwoMKEHJBmHdA;

	private bool nVCcoSVrAapJrzWtHaAdarUkCrEx;

	private bool pwvUlkFWdUiHpyiUuPirIdfbIsJHA;

	private bool CcnIDmKtmDmKqfjMiODuXQouCNfYB;

	private string[] qOgPFPLICHCExcrGvmEfLDYtFupB;

	private int[] VLSStXfkaqRgZNacKcIleXbHsHqo;

	public void Initialize(Rewired.Internal.GUIText text)
	{
		DWMQwVRyhyHhDDgjbtCayJLAZKPw = text;
		qOgPFPLICHCExcrGvmEfLDYtFupB = Enum.GetNames(typeof(RawInputAxis));
		VLSStXfkaqRgZNacKcIleXbHsHqo = (int[])Enum.GetValues(typeof(RawInputAxis));
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
		MZpweTLKzUXrzNGXSlQtLnMgOvOs = ReInput.primaryInputManager.inputSource as pYDDNhBibHfSZzyMuFjJSOUJKDWk;
		if (MZpweTLKzUXrzNGXSlQtLnMgOvOs == null)
		{
			Rewired.Logger.LogError("Unable to initialize Raw Input! You must add a Rewired Input Manager to the scene and set the input mode to Raw Input.");
			return;
		}
		ReInput.primaryInputManager.SystemDeviceConnectedEvent += pqSAuCVZSOhHtVpLhThQGyDWlkFF;
		ReInput.primaryInputManager.SystemDeviceDisconnectedEvent += rsjVtmZDOZqmLjDrBUCiDDiakaEu;
		ZtRaIOgkPuoXDUPGciGGmKDJFjxz();
		CcnIDmKtmDmKqfjMiODuXQouCNfYB = true;
	}

	public void Update()
	{
		if (!CcnIDmKtmDmKqfjMiODuXQouCNfYB)
		{
			return;
		}
		jwECMxSvaMhNDKjcsJCsxsTTpOoD = "Raw Input Joystick Element Identifier\n\n";
		DWMQwVRyhyHhDDgjbtCayJLAZKPw.text = jwECMxSvaMhNDKjcsJCsxsTTpOoD;
		int num = oxEZsmOAsmRKMdpdNdBMzxOCCEBIA;
		Guid hegwfvpxQkEttDMRtMMCrTxaIPOl = HegwfvpxQkEttDMRtMMCrTxaIPOl;
		if (ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Equals) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Plus) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.KeypadPlus))
		{
			oxEZsmOAsmRKMdpdNdBMzxOCCEBIA++;
		}
		if (ReInput.controllers.Keyboard.GetKeyDown(KeyCode.KeypadMinus) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Minus))
		{
			oxEZsmOAsmRKMdpdNdBMzxOCCEBIA--;
		}
		if (pwvUlkFWdUiHpyiUuPirIdfbIsJHA)
		{
			ZtRaIOgkPuoXDUPGciGGmKDJFjxz();
			pwvUlkFWdUiHpyiUuPirIdfbIsJHA = false;
		}
		int num2 = ((btjMNRMAgftRTvEiwoMKEHJBmHdA != null) ? btjMNRMAgftRTvEiwoMKEHJBmHdA.Count : 0);
		if (num2 == 0)
		{
			return;
		}
		if (oxEZsmOAsmRKMdpdNdBMzxOCCEBIA < 0)
		{
			oxEZsmOAsmRKMdpdNdBMzxOCCEBIA = num2 - 1;
		}
		else if (oxEZsmOAsmRKMdpdNdBMzxOCCEBIA >= num2)
		{
			oxEZsmOAsmRKMdpdNdBMzxOCCEBIA = 0;
		}
		HegwfvpxQkEttDMRtMMCrTxaIPOl = btjMNRMAgftRTvEiwoMKEHJBmHdA[oxEZsmOAsmRKMdpdNdBMzxOCCEBIA].CweZumdjFMXhjEVfSspSkoVjfBQD;
		bool flag = false;
		if (num != oxEZsmOAsmRKMdpdNdBMzxOCCEBIA || hegwfvpxQkEttDMRtMMCrTxaIPOl != HegwfvpxQkEttDMRtMMCrTxaIPOl)
		{
			flag = true;
		}
		if (lTvcYXgAOKmjzpfkXhzOEzdVloT == null || flag)
		{
			if (lTvcYXgAOKmjzpfkXhzOEzdVloT != null)
			{
				lTvcYXgAOKmjzpfkXhzOEzdVloT.Unacquire();
			}
			lTvcYXgAOKmjzpfkXhzOEzdVloT = btjMNRMAgftRTvEiwoMKEHJBmHdA[oxEZsmOAsmRKMdpdNdBMzxOCCEBIA];
			if (lTvcYXgAOKmjzpfkXhzOEzdVloT == null)
			{
				return;
			}
			lTvcYXgAOKmjzpfkXhzOEzdVloT.Acquire();
		}
		bool flag2 = false;
		if (lTvcYXgAOKmjzpfkXhzOEzdVloT.WxHAkmZTzXIuwdexnxzaPOPBefRT is YntIGkkeWxxBPFZEWuJjonhwByyAA)
		{
			flag2 = true;
		}
		else if (!(lTvcYXgAOKmjzpfkXhzOEzdVloT.WxHAkmZTzXIuwdexnxzaPOPBefRT is UcxWPXaAluhsBvojKjOcBoauCGaeA))
		{
			return;
		}
		if (num2 > 0)
		{
			jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + num2 + " connected devices:\n";
		}
		for (int i = 0; i < num2; i++)
		{
			jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + btjMNRMAgftRTvEiwoMKEHJBmHdA[i].seJMgajoYjVKLhFsfXCNFjqYeLnd + "\n";
		}
		jwECMxSvaMhNDKjcsJCsxsTTpOoD += "\n";
		jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + "Current RI device " + oxEZsmOAsmRKMdpdNdBMzxOCCEBIA + ": \"" + lTvcYXgAOKmjzpfkXhzOEzdVloT.seJMgajoYjVKLhFsfXCNFjqYeLnd + "\"\n";
		jwECMxSvaMhNDKjcsJCsxsTTpOoD += "(Press + or - to change monitored device id.)\n\n";
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Product Name", "\"" + lTvcYXgAOKmjzpfkXhzOEzdVloT.seJMgajoYjVKLhFsfXCNFjqYeLnd + "\"");
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Is Bluetooth Device", lTvcYXgAOKmjzpfkXhzOEzdVloT.UIGmmzWbBMfdGYECyaeiDEopKuSF);
		if (lTvcYXgAOKmjzpfkXhzOEzdVloT.UIGmmzWbBMfdGYECyaeiDEopKuSF)
		{
			cZNehjAzbMtvVLEmkpcxuFmjtmd("Bluetooth Device Name", "\"" + lTvcYXgAOKmjzpfkXhzOEzdVloT.mmXjvTYlEqPFbkDIvDABGnubFpUAA + "\"");
		}
		if (flag2)
		{
			cZNehjAzbMtvVLEmkpcxuFmjtmd("Using Custom Driver", "TRUE");
		}
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Device Type", lTvcYXgAOKmjzpfkXhzOEzdVloT.EdnDJUCqhoCBDsuXHJHzjbHhpEGGc.ToString());
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Identifier", new PidVid(lTvcYXgAOKmjzpfkXhzOEzdVloT.TbChgSIDKmcyjakuXntycNtcTsvz));
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Product Id", lTvcYXgAOKmjzpfkXhzOEzdVloT.ziEOGSyTLWPsyHfYsFWJMcubfqGl);
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Vendor Id", lTvcYXgAOKmjzpfkXhzOEzdVloT.fwkbdussmhNBdRwDSrsNNZyCOOxL);
		jwECMxSvaMhNDKjcsJCsxsTTpOoD += "\n";
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Axis Count", lTvcYXgAOKmjzpfkXhzOEzdVloT.YzulasZUtFpJkszTiuMKmmFHMHWy);
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Button Count", lTvcYXgAOKmjzpfkXhzOEzdVloT.ZkrylnAACTgZqQuGOMOiRqOmBgjR);
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Hat Count", lTvcYXgAOKmjzpfkXhzOEzdVloT.nshetUzdBJLHlFcQUmIlUQrXpimu);
		jwECMxSvaMhNDKjcsJCsxsTTpOoD += "\n";
		if (flag)
		{
			string text = "";
			text = text + "Device Name: \"" + btjMNRMAgftRTvEiwoMKEHJBmHdA[oxEZsmOAsmRKMdpdNdBMzxOCCEBIA].seJMgajoYjVKLhFsfXCNFjqYeLnd + "\"\n";
			if (lTvcYXgAOKmjzpfkXhzOEzdVloT.UIGmmzWbBMfdGYECyaeiDEopKuSF)
			{
				text = text + "Bluetooth Device Name: \"" + lTvcYXgAOKmjzpfkXhzOEzdVloT.mmXjvTYlEqPFbkDIvDABGnubFpUAA + "\"\n";
			}
			text = text + "Identifier: " + new PidVid(lTvcYXgAOKmjzpfkXhzOEzdVloT.TbChgSIDKmcyjakuXntycNtcTsvz).ToString() + "\n";
			Rewired.Logger.Log(text);
		}
		if (!flag2)
		{
			UcxWPXaAluhsBvojKjOcBoauCGaeA ucxWPXaAluhsBvojKjOcBoauCGaeA = lTvcYXgAOKmjzpfkXhzOEzdVloT.WxHAkmZTzXIuwdexnxzaPOPBefRT as UcxWPXaAluhsBvojKjOcBoauCGaeA;
			for (int j = 1; j < qOgPFPLICHCExcrGvmEfLDYtFupB.Length - 1; j++)
			{
				int num3 = mPKaIHfzhogaSXQJCwZlMPTETFwmA((RawInputAxis)VLSStXfkaqRgZNacKcIleXbHsHqo[j], 0, ucxWPXaAluhsBvojKjOcBoauCGaeA);
				string text2 = qOgPFPLICHCExcrGvmEfLDYtFupB[j];
				try
				{
					cZNehjAzbMtvVLEmkpcxuFmjtmd(text2, num3 + " (" + FdAIjbgcfatFrghHnIipFWDBvhqyA(num3) + ")");
				}
				catch
				{
					cZNehjAzbMtvVLEmkpcxuFmjtmd(text2, "FAILED! Axis value = " + num3);
				}
			}
			if (ucxWPXaAluhsBvojKjOcBoauCGaeA.LOGhnHvjOTgIHbJhRkNTadIEyfUzb > 0)
			{
				for (int k = 0; k < ucxWPXaAluhsBvojKjOcBoauCGaeA.LOGhnHvjOTgIHbJhRkNTadIEyfUzb; k++)
				{
					int num4 = mPKaIHfzhogaSXQJCwZlMPTETFwmA(RawInputAxis.Other, k, ucxWPXaAluhsBvojKjOcBoauCGaeA);
					string text3 = "Other Axis " + k;
					try
					{
						cZNehjAzbMtvVLEmkpcxuFmjtmd(text3, num4 + " (" + FdAIjbgcfatFrghHnIipFWDBvhqyA(num4) + ")");
					}
					catch
					{
						cZNehjAzbMtvVLEmkpcxuFmjtmd(text3, "FAILED! Axis value = " + num4);
					}
				}
			}
			int[] array = lTvcYXgAOKmjzpfkXhzOEzdVloT.YISVcAntcDKQZIzLWKZzfYhRmeuP;
			for (int l = 0; l < array.Length; l++)
			{
				int num5 = array[l];
				string text4 = "Hat " + l;
				cZNehjAzbMtvVLEmkpcxuFmjtmd(text4, num5);
			}
			bool[] array2 = lTvcYXgAOKmjzpfkXhzOEzdVloT.kUBzXDgUpYEZoaPrTtuPPIDjpHZM;
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
			cZNehjAzbMtvVLEmkpcxuFmjtmd("Buttons ", text5);
		}
		else
		{
			YntIGkkeWxxBPFZEWuJjonhwByyAA yntIGkkeWxxBPFZEWuJjonhwByyAA = lTvcYXgAOKmjzpfkXhzOEzdVloT.WxHAkmZTzXIuwdexnxzaPOPBefRT as YntIGkkeWxxBPFZEWuJjonhwByyAA;
			for (int n = 0; n < lTvcYXgAOKmjzpfkXhzOEzdVloT.YzulasZUtFpJkszTiuMKmmFHMHWy; n++)
			{
				float num6 = yntIGkkeWxxBPFZEWuJjonhwByyAA.mPKaIHfzhogaSXQJCwZlMPTETFwmA(n);
				string text6 = n.ToString();
				try
				{
					cZNehjAzbMtvVLEmkpcxuFmjtmd(text6, num6 + " (" + yntIGkkeWxxBPFZEWuJjonhwByyAA.EYfGGXiEAfJbAhSFicTjkSxKfbvgA(n) + ")");
				}
				catch
				{
					cZNehjAzbMtvVLEmkpcxuFmjtmd(text6, "FAILED! Axis value = " + num6);
				}
			}
			int[] array3 = lTvcYXgAOKmjzpfkXhzOEzdVloT.YISVcAntcDKQZIzLWKZzfYhRmeuP;
			for (int num7 = 0; num7 < lTvcYXgAOKmjzpfkXhzOEzdVloT.nshetUzdBJLHlFcQUmIlUQrXpimu; num7++)
			{
				int num8 = array3[num7];
				string text7 = "Hat " + num7;
				cZNehjAzbMtvVLEmkpcxuFmjtmd(text7, num8);
			}
			for (int num9 = 0; num9 < lTvcYXgAOKmjzpfkXhzOEzdVloT.vxIWkUhFtVnjmWAToKBMGCAEorIK.GyroscopeCount; num9++)
			{
				int valueLength = lTvcYXgAOKmjzpfkXhzOEzdVloT.vxIWkUhFtVnjmWAToKBMGCAEorIK.gyroscopes[num9].valueLength;
				string text8 = "";
				for (int num10 = 0; num10 < valueLength; num10++)
				{
					float num11 = lTvcYXgAOKmjzpfkXhzOEzdVloT.vxIWkUhFtVnjmWAToKBMGCAEorIK.gyroscopes[num9].rawValue[num10];
					text8 = text8 + "[" + num10 + "]: " + num11.ToString("f3");
					if (num10 < valueLength - 1)
					{
						text8 += " ";
					}
				}
				cZNehjAzbMtvVLEmkpcxuFmjtmd("Gyro " + num9, text8);
			}
			for (int num12 = 0; num12 < lTvcYXgAOKmjzpfkXhzOEzdVloT.vxIWkUhFtVnjmWAToKBMGCAEorIK.AccelerometerCount; num12++)
			{
				int valueLength2 = lTvcYXgAOKmjzpfkXhzOEzdVloT.vxIWkUhFtVnjmWAToKBMGCAEorIK.accelerometers[num12].valueLength;
				string text9 = "";
				for (int num13 = 0; num13 < valueLength2; num13++)
				{
					float num14 = lTvcYXgAOKmjzpfkXhzOEzdVloT.vxIWkUhFtVnjmWAToKBMGCAEorIK.accelerometers[num12].rawValue[num13];
					text9 = text9 + "[" + num13 + "]: " + num14.ToString("f3");
					if (num13 < valueLength2 - 1)
					{
						text9 += " ";
					}
				}
				cZNehjAzbMtvVLEmkpcxuFmjtmd("Accelerometer " + num12, text9);
			}
			for (int num15 = 0; num15 < lTvcYXgAOKmjzpfkXhzOEzdVloT.vxIWkUhFtVnjmWAToKBMGCAEorIK.TouchpadCount; num15++)
			{
				HIDTouchpad hIDTouchpad = lTvcYXgAOKmjzpfkXhzOEzdVloT.vxIWkUhFtVnjmWAToKBMGCAEorIK.touchpads[num15];
				int num16 = hIDTouchpad.values.Length;
				string text10 = "";
				for (int num17 = 0; num17 < num16; num17++)
				{
					HIDTouchpad.TouchData touchData = hIDTouchpad.values[num17];
					text10 = text10 + "Touch " + num17 + ": Is Touching = " + touchData.isTouching + "\n";
					text10 = text10 + "Touch " + num17 + ": Touch Id = " + touchData.touchId + "\n";
					text10 = text10 + "Touch " + num17 + ": Position = " + touchData.positionX + ", " + touchData.positionY + "\n";
					text10 = text10 + "Touch " + num17 + ": Abs Position = " + touchData.positionAbsX + ", " + touchData.positionAbsY + " (" + touchData.positionRawX + ", " + touchData.positionRawY + ")\n";
				}
				sQaMPWjnoCBqcTdAbBHEkIbUKkZKA("Touchpad " + num15, text10);
			}
			bool[] array4 = lTvcYXgAOKmjzpfkXhzOEzdVloT.kUBzXDgUpYEZoaPrTtuPPIDjpHZM;
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
			cZNehjAzbMtvVLEmkpcxuFmjtmd("Buttons ", text11);
		}
		DWMQwVRyhyHhDDgjbtCayJLAZKPw.text = jwECMxSvaMhNDKjcsJCsxsTTpOoD;
	}

	public void OnDestroy()
	{
		if (lTvcYXgAOKmjzpfkXhzOEzdVloT != null)
		{
			lTvcYXgAOKmjzpfkXhzOEzdVloT.Unacquire();
		}
	}

	private void ZtRaIOgkPuoXDUPGciGGmKDJFjxz()
	{
		btjMNRMAgftRTvEiwoMKEHJBmHdA = MZpweTLKzUXrzNGXSlQtLnMgOvOs.GetJoysticks<gNzuXmbvyelIPcqSZuxQNOcbBnvA>();
	}

	private void pqSAuCVZSOhHtVpLhThQGyDWlkFF()
	{
		hzVXylspAStQLNzufXVziRmulsUf();
	}

	private void rsjVtmZDOZqmLjDrBUCiDDiakaEu()
	{
		hzVXylspAStQLNzufXVziRmulsUf();
	}

	private void hzVXylspAStQLNzufXVziRmulsUf()
	{
		ZrbFhGEbWRbTzVxxQimUntnkwisKA();
		pwvUlkFWdUiHpyiUuPirIdfbIsJHA = true;
	}

	private void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
	{
		oxEZsmOAsmRKMdpdNdBMzxOCCEBIA = 0;
		lTvcYXgAOKmjzpfkXhzOEzdVloT = null;
		HegwfvpxQkEttDMRtMMCrTxaIPOl = Guid.Empty;
		btjMNRMAgftRTvEiwoMKEHJBmHdA = null;
		nVCcoSVrAapJrzWtHaAdarUkCrEx = false;
		pwvUlkFWdUiHpyiUuPirIdfbIsJHA = false;
	}

	private void cZNehjAzbMtvVLEmkpcxuFmjtmd(string P_0, object P_1)
	{
		jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + P_0 + " = " + P_1.ToString() + "\n";
	}

	private void sQaMPWjnoCBqcTdAbBHEkIbUKkZKA(string P_0, object P_1)
	{
		jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + P_0 + ":\n" + P_1.ToString() + "\n";
	}

	private int mPKaIHfzhogaSXQJCwZlMPTETFwmA(RawInputAxis P_0, int P_1, UcxWPXaAluhsBvojKjOcBoauCGaeA P_2)
	{
		return P_2.mPKaIHfzhogaSXQJCwZlMPTETFwmA(P_0, P_1);
	}

	private float FdAIjbgcfatFrghHnIipFWDBvhqyA(int P_0)
	{
		if (P_0 == 0)
		{
			return 0f;
		}
		return MathTools.Clamp((float)MathTools.Abs(P_0) / 65535f * (float)MathTools.Sign(P_0), -1f, 1f);
	}
}
