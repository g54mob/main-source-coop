using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

internal sealed class IYEfrYckYTRMFLDxdEQtEATSabUv : IElementIdentifierTool
{
	private Rewired.Internal.GUIText DWMQwVRyhyHhDDgjbtCayJLAZKPw;

	private string jwECMxSvaMhNDKjcsJCsxsTTpOoD;

	private int oxEZsmOAsmRKMdpdNdBMzxOCCEBIA;

	private QQoDCFHRGbHrSVrOFoRnxRpjfifrA JeGjtgLjukMNmhoPpgJxouBJciwP;

	private RKkwdQvNYvJKdMhjDwqoOEdKUccR lTvcYXgAOKmjzpfkXhzOEzdVloT;

	private Guid HegwfvpxQkEttDMRtMMCrTxaIPOl;

	private IList<NWgGzQDOZYBvNGVNEGBMOyyPyPYIA> btjMNRMAgftRTvEiwoMKEHJBmHdA;

	private IList<NWgGzQDOZYBvNGVNEGBMOyyPyPYIA> xHqQBHUNfvGVgQBdDwobjQzroCYW;

	private bool nVCcoSVrAapJrzWtHaAdarUkCrEx;

	private bool pwvUlkFWdUiHpyiUuPirIdfbIsJHA;

	private bool CcnIDmKtmDmKqfjMiODuXQouCNfYB;

	private int fCgaJqarIfInCGUVEKTjhZiaHrxmB;

	private TimerRealTime dLOqPTlBUuMOjMgtmlbohJhdAEGZ;

	public void Initialize(Rewired.Internal.GUIText text)
	{
		DWMQwVRyhyHhDDgjbtCayJLAZKPw = text;
	}

	public void Start()
	{
		if (ReInput.isEditor && ReInput.editorPlatform != EditorPlatform.Windows)
		{
			Rewired.Logger.LogError("Direct Input cannot be run on this platform. You must be running the editor in Windows.");
		}
		else if (ReInput.currentPlatform != Platform.Windows)
		{
			Rewired.Logger.LogError("Direct Input cannot be run on this build target. Be sure Unity's build target is set to Windows Standalone.");
		}
		else if (ReInput.primaryInputManager.inputSource is InputSourceWrapper<QQoDCFHRGbHrSVrOFoRnxRpjfifrA> { source: not null } inputSourceWrapper)
		{
			JeGjtgLjukMNmhoPpgJxouBJciwP = inputSourceWrapper.source;
			ReInput.primaryInputManager.SystemDeviceConnectedEvent += pqSAuCVZSOhHtVpLhThQGyDWlkFF;
			ReInput.primaryInputManager.SystemDeviceDisconnectedEvent += rsjVtmZDOZqmLjDrBUCiDDiakaEu;
			dLOqPTlBUuMOjMgtmlbohJhdAEGZ = new TimerRealTime(1.0);
			dLOqPTlBUuMOjMgtmlbohJhdAEGZ.Start();
			ZtRaIOgkPuoXDUPGciGGmKDJFjxz();
			CcnIDmKtmDmKqfjMiODuXQouCNfYB = true;
		}
		else
		{
			Rewired.Logger.LogError("Unable to initialize Direct Input! You must add a Rewired Input Manager to the scene and set the input mode to Direct Input.");
		}
	}

	public void Update()
	{
		if (!CcnIDmKtmDmKqfjMiODuXQouCNfYB)
		{
			return;
		}
		jwECMxSvaMhNDKjcsJCsxsTTpOoD = "Direct Input Joystick Element Identifier\n\n";
		DWMQwVRyhyHhDDgjbtCayJLAZKPw.text = jwECMxSvaMhNDKjcsJCsxsTTpOoD;
		if (Input.GetKeyDown(KeyCode.A))
		{
			nVCcoSVrAapJrzWtHaAdarUkCrEx = !nVCcoSVrAapJrzWtHaAdarUkCrEx;
		}
		if (nVCcoSVrAapJrzWtHaAdarUkCrEx)
		{
			DWMQwVRyhyHhDDgjbtCayJLAZKPw.text += "All Devices:\n";
			foreach (NWgGzQDOZYBvNGVNEGBMOyyPyPYIA item in xHqQBHUNfvGVgQBdDwobjQzroCYW)
			{
				Rewired.Internal.GUIText dWMQwVRyhyHhDDgjbtCayJLAZKPw = DWMQwVRyhyHhDDgjbtCayJLAZKPw;
				dWMQwVRyhyHhDDgjbtCayJLAZKPw.text = dWMQwVRyhyHhDDgjbtCayJLAZKPw.text + item.seJMgajoYjVKLhFsfXCNFjqYeLnd + ", " + item.wuNNupugiaqBLIYiuwngZSitBSyx + ", " + new PidVid(item.TbChgSIDKmcyjakuXntycNtcTsvz).ToString() + ", " + item.ggbsaxLgUhnOLhfPpVJzmelXqaFl + ", " + item.wdLEfLZxrjtcLdIntcuABrEoICsnA + ", " + item.uuNGjxmdPvmxDegJbENqZbvxonIw + "\n";
			}
			DWMQwVRyhyHhDDgjbtCayJLAZKPw.text += "\n";
		}
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
		if (dLOqPTlBUuMOjMgtmlbohJhdAEGZ.Update())
		{
			int num2 = JeGjtgLjukMNmhoPpgJxouBJciwP.IymjpiVVoPvIVgCRZbPpLtAuOIjt(zglyhjDFFnjpYbZueSPqXqHoCKQx.All, RiVrjRnzmcUdxsIdsIPMwktWaKzR.AttachedOnly);
			if (num2 != fCgaJqarIfInCGUVEKTjhZiaHrxmB)
			{
				fCgaJqarIfInCGUVEKTjhZiaHrxmB = num2;
				pwvUlkFWdUiHpyiUuPirIdfbIsJHA = true;
			}
			dLOqPTlBUuMOjMgtmlbohJhdAEGZ.Start();
		}
		if (pwvUlkFWdUiHpyiUuPirIdfbIsJHA)
		{
			ZtRaIOgkPuoXDUPGciGGmKDJFjxz();
			pwvUlkFWdUiHpyiUuPirIdfbIsJHA = false;
		}
		int num3 = ((btjMNRMAgftRTvEiwoMKEHJBmHdA != null) ? btjMNRMAgftRTvEiwoMKEHJBmHdA.Count : 0);
		if (num3 == 0)
		{
			return;
		}
		if (oxEZsmOAsmRKMdpdNdBMzxOCCEBIA < 0)
		{
			oxEZsmOAsmRKMdpdNdBMzxOCCEBIA = num3 - 1;
		}
		else if (oxEZsmOAsmRKMdpdNdBMzxOCCEBIA >= num3)
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
				lTvcYXgAOKmjzpfkXhzOEzdVloT.jJBMyFqihDzNBHSdwXOmErxjVBmj();
			}
			lTvcYXgAOKmjzpfkXhzOEzdVloT = new RKkwdQvNYvJKdMhjDwqoOEdKUccR(JeGjtgLjukMNmhoPpgJxouBJciwP, btjMNRMAgftRTvEiwoMKEHJBmHdA[oxEZsmOAsmRKMdpdNdBMzxOCCEBIA].CweZumdjFMXhjEVfSspSkoVjfBQD);
			if (lTvcYXgAOKmjzpfkXhzOEzdVloT == null)
			{
				return;
			}
			IList<gSsnEkKQgoAoVCLJetnIqykecuEr> list = lTvcYXgAOKmjzpfkXhzOEzdVloT.xcIMfHpCcwPSCwLZadmVAAIAQCxHA();
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if ((list[i].lcoQpyZhuEmDknMGwOuRZhZMkOMK.ZIvyiOSzgMRCzZiwgApXsRrDjjHn & IsDELYDpjvwtAtxUZEGxhuFLuhdoA.Axis) != IsDELYDpjvwtAtxUZEGxhuFLuhdoA.All)
					{
						lTvcYXgAOKmjzpfkXhzOEzdVloT.OHWZmNZIpWvZZgcTrRiXqifLzMCl.OBxzCTYfRoFayuUqlqzrBTQWHzyf = new xbjhbnvOWZcuiUojDwKPRpASJgyS(-65535, 65535);
					}
				}
			}
			lTvcYXgAOKmjzpfkXhzOEzdVloT.iDtJjPBihifBIOGoxiSJxtdwVbkH();
		}
		tyXuhihoNAjKOehjTkoCIXcHjxIiB tyXuhihoNAjKOehjTkoCIXcHjxIiB2;
		try
		{
			tyXuhihoNAjKOehjTkoCIXcHjxIiB2 = lTvcYXgAOKmjzpfkXhzOEzdVloT.PlgegTqPxnavhQiTtSJSfIUpemvAA();
		}
		catch
		{
			tyXuhihoNAjKOehjTkoCIXcHjxIiB2 = null;
		}
		if (tyXuhihoNAjKOehjTkoCIXcHjxIiB2 == null)
		{
			return;
		}
		if (num3 > 0)
		{
			jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + num3 + " connected devices:\n";
		}
		for (int j = 0; j < num3; j++)
		{
			jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + btjMNRMAgftRTvEiwoMKEHJBmHdA[j].seJMgajoYjVKLhFsfXCNFjqYeLnd + "\n";
		}
		jwECMxSvaMhNDKjcsJCsxsTTpOoD += "\n";
		jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + "Current DI device " + oxEZsmOAsmRKMdpdNdBMzxOCCEBIA + ": " + btjMNRMAgftRTvEiwoMKEHJBmHdA[oxEZsmOAsmRKMdpdNdBMzxOCCEBIA].seJMgajoYjVKLhFsfXCNFjqYeLnd + "\n";
		jwECMxSvaMhNDKjcsJCsxsTTpOoD += "(Press + or - to change monitored device id.)\n\n";
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Identifier", new PidVid(lTvcYXgAOKmjzpfkXhzOEzdVloT.kPkDmvCAztZUbaXHgvIUarzhrliW.TbChgSIDKmcyjakuXntycNtcTsvz));
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Instance GUID", lTvcYXgAOKmjzpfkXhzOEzdVloT.kPkDmvCAztZUbaXHgvIUarzhrliW.CweZumdjFMXhjEVfSspSkoVjfBQD);
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Product Id", lTvcYXgAOKmjzpfkXhzOEzdVloT.OHWZmNZIpWvZZgcTrRiXqifLzMCl.ziEOGSyTLWPsyHfYsFWJMcubfqGl);
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Device Type", lTvcYXgAOKmjzpfkXhzOEzdVloT.aFqVxNoAVNrjGmeFNuMTZaRDkWrU.lhIrpCzAlkfgVHpeLkMrWybTDiDQA.ToString());
		jwECMxSvaMhNDKjcsJCsxsTTpOoD += "\n";
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Axis Count", lTvcYXgAOKmjzpfkXhzOEzdVloT.aFqVxNoAVNrjGmeFNuMTZaRDkWrU.VXrfXxdQVIXmftJrFDcdGEfCJtDlc);
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Button Count", lTvcYXgAOKmjzpfkXhzOEzdVloT.aFqVxNoAVNrjGmeFNuMTZaRDkWrU.ZkrylnAACTgZqQuGOMOiRqOmBgjR);
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Hat Count", lTvcYXgAOKmjzpfkXhzOEzdVloT.aFqVxNoAVNrjGmeFNuMTZaRDkWrU.sOlPAhTLnGPgnZbhWruDWcuhOflb);
		jwECMxSvaMhNDKjcsJCsxsTTpOoD += "\n";
		if (flag)
		{
			Rewired.Logger.Log("Device Name: \"" + btjMNRMAgftRTvEiwoMKEHJBmHdA[oxEZsmOAsmRKMdpdNdBMzxOCCEBIA].seJMgajoYjVKLhFsfXCNFjqYeLnd + "\"");
			Rewired.Logger.Log("Identifier: " + new PidVid(lTvcYXgAOKmjzpfkXhzOEzdVloT.kPkDmvCAztZUbaXHgvIUarzhrliW.TbChgSIDKmcyjakuXntycNtcTsvz).ToString());
		}
		for (int k = 0; k < 32; k++)
		{
			int num4 = mPKaIHfzhogaSXQJCwZlMPTETFwmA((DirectInputAxis)k, tyXuhihoNAjKOehjTkoCIXcHjxIiB2);
			DirectInputAxis directInputAxis = (DirectInputAxis)k;
			string text = directInputAxis.ToString();
			cZNehjAzbMtvVLEmkpcxuFmjtmd(text, num4 + " (" + FdAIjbgcfatFrghHnIipFWDBvhqyA(num4) + ")");
		}
		int[] array = tyXuhihoNAjKOehjTkoCIXcHjxIiB2.WceXTchDfkHYuRNIcUzEQAzAuvXi;
		for (int l = 0; l < 4; l++)
		{
			int num5 = array[l];
			string text2 = "Hat " + l;
			cZNehjAzbMtvVLEmkpcxuFmjtmd(text2, num5);
		}
		bool[] array2 = tyXuhihoNAjKOehjTkoCIXcHjxIiB2.kUBzXDgUpYEZoaPrTtuPPIDjpHZM;
		string text3 = "";
		for (int m = 0; m < 128; m++)
		{
			if (array2[m])
			{
				if (text3 != "")
				{
					text3 += ", ";
				}
				text3 += m;
			}
		}
		cZNehjAzbMtvVLEmkpcxuFmjtmd("Buttons ", text3);
		DWMQwVRyhyHhDDgjbtCayJLAZKPw.text = jwECMxSvaMhNDKjcsJCsxsTTpOoD;
	}

	private void ZtRaIOgkPuoXDUPGciGGmKDJFjxz()
	{
		btjMNRMAgftRTvEiwoMKEHJBmHdA = JeGjtgLjukMNmhoPpgJxouBJciwP.PcxRaKuzIVnuHUPdgfTRrZhcecFg(zglyhjDFFnjpYbZueSPqXqHoCKQx.GameControl, RiVrjRnzmcUdxsIdsIPMwktWaKzR.AttachedOnly);
		xHqQBHUNfvGVgQBdDwobjQzroCYW = JeGjtgLjukMNmhoPpgJxouBJciwP.PcxRaKuzIVnuHUPdgfTRrZhcecFg(zglyhjDFFnjpYbZueSPqXqHoCKQx.All, RiVrjRnzmcUdxsIdsIPMwktWaKzR.AttachedOnly);
		fCgaJqarIfInCGUVEKTjhZiaHrxmB = ((xHqQBHUNfvGVgQBdDwobjQzroCYW != null) ? xHqQBHUNfvGVgQBdDwobjQzroCYW.Count : 0);
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
		xHqQBHUNfvGVgQBdDwobjQzroCYW = null;
		nVCcoSVrAapJrzWtHaAdarUkCrEx = false;
		pwvUlkFWdUiHpyiUuPirIdfbIsJHA = false;
		fCgaJqarIfInCGUVEKTjhZiaHrxmB = 0;
	}

	private void cZNehjAzbMtvVLEmkpcxuFmjtmd(string P_0, object P_1)
	{
		jwECMxSvaMhNDKjcsJCsxsTTpOoD = jwECMxSvaMhNDKjcsJCsxsTTpOoD + P_0 + " = " + P_1.ToString() + "\n";
	}

	private int mPKaIHfzhogaSXQJCwZlMPTETFwmA(DirectInputAxis P_0, tyXuhihoNAjKOehjTkoCIXcHjxIiB P_1)
	{
		return P_0 switch
		{
			DirectInputAxis.X => P_1.LeaAlLlLUVpUZEtsNQKSfcVaXRbL, 
			DirectInputAxis.Y => P_1.nYgMBIIMubbuyaBAFyuFsqYaYOAtA, 
			DirectInputAxis.Z => P_1.dOuRBCKDBhffYSgvmkclUElpgLap, 
			DirectInputAxis.RotationX => P_1.sRpsoQIHmDvGVZvkrdIAVbMwHDoHA, 
			DirectInputAxis.RotationY => P_1.facOlRrFAOoFgYgIgXaWwybZrwFT, 
			DirectInputAxis.RotationZ => P_1.ekpDPUkdvHynWAMlQdLpekUyfQmJ, 
			DirectInputAxis.Slider0 => P_1.CincbAchMUMUPGPnFgMSFpZucovmA[0], 
			DirectInputAxis.Slider1 => P_1.CincbAchMUMUPGPnFgMSFpZucovmA[1], 
			DirectInputAxis.VelocityX => P_1.MdwdewGAdufaxBEuuZTqEAewIFnuA, 
			DirectInputAxis.VelocityY => P_1.NKQDxcCfebdLMnFQMVtiZiVGCFXu, 
			DirectInputAxis.VelocityZ => P_1.kiTDNBZHzlnZvfDYBYMqnXsfdFeT, 
			DirectInputAxis.AngularVelocityX => P_1.AqZJxqrfgIAoEffOyeqiJwaHtrVx, 
			DirectInputAxis.AngularVelocityY => P_1.aSnhQQFBIsVqyWNaCQpypzlSTVoP, 
			DirectInputAxis.AngularVelocityZ => P_1.HWFDgNisFxyNgEWSwfqciWQDSMvi, 
			DirectInputAxis.VelocitySlider0 => P_1.lffXabdrblEdAxqPdNndWLCkieVF[0], 
			DirectInputAxis.VelocitySlider1 => P_1.lffXabdrblEdAxqPdNndWLCkieVF[1], 
			DirectInputAxis.AccelerationX => P_1.cAIwAgSqlwZslfRBaFaBFtEerweg, 
			DirectInputAxis.AccelerationY => P_1.ORPEwZGSndhiewYFXbssAxRAHEJEc, 
			DirectInputAxis.AccelerationZ => P_1.ruHesJnFVcqvohZzKxYSeWWxOoiB, 
			DirectInputAxis.AngularAccelerationX => P_1.qHHcMVDkyXLxwEbMZHiEliOBvHArA, 
			DirectInputAxis.AngularAccelerationY => P_1.QKqZWUBiKGPznIkbaGGBEfjcYkvp, 
			DirectInputAxis.AngularAccelerationZ => P_1.fazKqrjHLGJZACqKfGUHmBKubMQH, 
			DirectInputAxis.AccelerationSlider0 => P_1.jbELMRTjMYJNoOlgKvnfcGOwlUob[0], 
			DirectInputAxis.AccelerationSlider1 => P_1.jbELMRTjMYJNoOlgKvnfcGOwlUob[1], 
			DirectInputAxis.ForceX => P_1.xRMcFYHuMLBdLMNVOtLvinnmpQHGb, 
			DirectInputAxis.ForceY => P_1.pPbHXGlwdQlPYWqIKEICIFDlBLOg, 
			DirectInputAxis.ForceZ => P_1.scUfVIhUFHKdMykwqwlSiJElTSoGA, 
			DirectInputAxis.TorqueX => P_1.fjpyfuEoFtFyQYDTTJaYhEGNsQmT, 
			DirectInputAxis.TorqueY => P_1.lFWgOQuPlujNgiNfSoWOFklULpynA, 
			DirectInputAxis.TorqueZ => P_1.AeEdVbGsueZLfgvzAsNDyjLVBpsB, 
			DirectInputAxis.ForceSlider0 => P_1.hzNemwABzSjCKRGcUnJldwdDoIiPb[0], 
			DirectInputAxis.ForceSlider1 => P_1.hzNemwABzSjCKRGcUnJldwdDoIiPb[1], 
			_ => 0, 
		};
	}

	private float FdAIjbgcfatFrghHnIipFWDBvhqyA(int P_0)
	{
		if (P_0 == 0)
		{
			return 0f;
		}
		return MathTools.Clamp((float)MathTools.Abs(P_0) / 65535f * (float)MathTools.Sign(P_0), -1f, 1f);
	}

	public void OnDestroy()
	{
		if (lTvcYXgAOKmjzpfkXhzOEzdVloT != null)
		{
			lTvcYXgAOKmjzpfkXhzOEzdVloT.jJBMyFqihDzNBHSdwXOmErxjVBmj();
		}
	}
}
