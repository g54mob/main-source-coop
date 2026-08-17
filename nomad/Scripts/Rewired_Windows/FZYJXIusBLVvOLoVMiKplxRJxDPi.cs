using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Interfaces;
using Rewired.Internal;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

internal sealed class FZYJXIusBLVvOLoVMiKplxRJxDPi : IElementIdentifierTool
{
	private GUIText YHnBjiqFpCEUIHDtCQIymgrdmZwdb;

	private string vgUSPeXWNJNRAqnTjUubkhwUJJsj;

	private int GERdrVCDQHvvZNgGiTcIKOdrVZFqA;

	private BKiqqPDcNrfyFbgcenapmsdywauK ouhhegqNpQUdwOyRMvRKAjJSSbIu;

	private YOgNgQpZZfYcTITAqIaepzvNafxe yENweyllzoleyXXfWoeNCooSLgRk;

	private Guid yKVVihTZawqctWqwWpKjOJoamNIE;

	private IList<AZmRVMBCLWjdSJdmldWMNaRORDDE> PNNmdtcIAosvoTxnUqoheBqGEpgz;

	private IList<AZmRVMBCLWjdSJdmldWMNaRORDDE> RemlznNxAbofxhKmGdVvFvStKKINA;

	private bool CKpGCZjjqwEcEGqHcqMFaLqIxTeKA;

	private bool MrjqoPDcbsBfSqeKXTLHPcieenuw;

	private bool GFsklpQNjMbsFmXvIFyLpsBjLKOP;

	private int wqAtRCzqaIyMZSTimLGKFDYTgoHbA;

	private TimerRealTime OjldbAzmNVOmwUtbbfBELjXfefLC;

	public void Initialize(GUIText text)
	{
		YHnBjiqFpCEUIHDtCQIymgrdmZwdb = text;
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
			Rewired.Logger.LogError("Direct Input cannot be run on this platform. You must be running the editor in Windows.");
		}
		else if (ReInput.currentPlatform != Platform.Windows)
		{
			Rewired.Logger.LogError("Direct Input cannot be run on this build target. Be sure Unity's build target is set to Windows Standalone.");
		}
		else if (ReInput.primaryInputManager.inputSource is InputSourceWrapper<BKiqqPDcNrfyFbgcenapmsdywauK> { source: not null } inputSourceWrapper)
		{
			ouhhegqNpQUdwOyRMvRKAjJSSbIu = inputSourceWrapper.source;
			ReInput.primaryInputManager.SystemDeviceConnectedEvent += qdxxhfxZtrBvVxVbvtOVAGPwBnNK;
			ReInput.primaryInputManager.SystemDeviceDisconnectedEvent += ekENsvofFTmtnUIitAREBHsMxXcjA;
			OjldbAzmNVOmwUtbbfBELjXfefLC = new TimerRealTime(1.0);
			OjldbAzmNVOmwUtbbfBELjXfefLC.Start();
			aBSgylEpYDSWttQqiqYKMehyEWUlA();
			GFsklpQNjMbsFmXvIFyLpsBjLKOP = true;
		}
		else
		{
			Rewired.Logger.LogError("Unable to initialize Direct Input! You must add a Rewired Input Manager to the scene and set the input mode to Direct Input.");
		}
	}

	void IElementIdentifierTool.Start()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Start
		this.Start();
	}

	public void Update()
	{
		if (!GFsklpQNjMbsFmXvIFyLpsBjLKOP)
		{
			return;
		}
		vgUSPeXWNJNRAqnTjUubkhwUJJsj = "Direct Input Joystick Element Identifier\n\n";
		YHnBjiqFpCEUIHDtCQIymgrdmZwdb.text = vgUSPeXWNJNRAqnTjUubkhwUJJsj;
		if (Input.GetKeyDown(KeyCode.A))
		{
			CKpGCZjjqwEcEGqHcqMFaLqIxTeKA = !CKpGCZjjqwEcEGqHcqMFaLqIxTeKA;
		}
		if (CKpGCZjjqwEcEGqHcqMFaLqIxTeKA)
		{
			YHnBjiqFpCEUIHDtCQIymgrdmZwdb.text += "All Devices:\n";
			foreach (AZmRVMBCLWjdSJdmldWMNaRORDDE item in RemlznNxAbofxhKmGdVvFvStKKINA)
			{
				GUIText yHnBjiqFpCEUIHDtCQIymgrdmZwdb = YHnBjiqFpCEUIHDtCQIymgrdmZwdb;
				yHnBjiqFpCEUIHDtCQIymgrdmZwdb.text = yHnBjiqFpCEUIHDtCQIymgrdmZwdb.text + item.VvQGQTZBUkfFSCosnyMActATHIaGb + ", " + item.xVuiCnTVhQXwQCNlaulBSNTHOOAL + ", " + new PidVid(item.EbuxgXKWWFMAMpYFAvRpwowfDfQCA).ToString() + ", " + item.btuyAUEvUkAMyZRtddLAqBfMcOGZ + ", " + item.svoXuyQtjqEufBoeaMucAjnxlHmg + ", " + item.AZDSATNUswZqkwVvzNYRJeLWxVJG + "\n";
			}
			YHnBjiqFpCEUIHDtCQIymgrdmZwdb.text += "\n";
		}
		int gERdrVCDQHvvZNgGiTcIKOdrVZFqA = GERdrVCDQHvvZNgGiTcIKOdrVZFqA;
		Guid guid = yKVVihTZawqctWqwWpKjOJoamNIE;
		if (ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Equals) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Plus) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.KeypadPlus))
		{
			GERdrVCDQHvvZNgGiTcIKOdrVZFqA++;
		}
		if (ReInput.controllers.Keyboard.GetKeyDown(KeyCode.KeypadMinus) || ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Minus))
		{
			GERdrVCDQHvvZNgGiTcIKOdrVZFqA--;
		}
		if (OjldbAzmNVOmwUtbbfBELjXfefLC.Update())
		{
			int num = ouhhegqNpQUdwOyRMvRKAjJSSbIu.oVadgeKpxfnGpxIUiencgqXIiMZEb(sJhqOxXkEhdfJrzCFpdosfXfzYPl.All, QeXRlJhevgcnqiRHTcdYHolJBWgCA.AttachedOnly);
			if (num != wqAtRCzqaIyMZSTimLGKFDYTgoHbA)
			{
				wqAtRCzqaIyMZSTimLGKFDYTgoHbA = num;
				MrjqoPDcbsBfSqeKXTLHPcieenuw = true;
			}
			OjldbAzmNVOmwUtbbfBELjXfefLC.Start();
		}
		if (MrjqoPDcbsBfSqeKXTLHPcieenuw)
		{
			aBSgylEpYDSWttQqiqYKMehyEWUlA();
			MrjqoPDcbsBfSqeKXTLHPcieenuw = false;
		}
		int num2 = ((PNNmdtcIAosvoTxnUqoheBqGEpgz != null) ? PNNmdtcIAosvoTxnUqoheBqGEpgz.Count : 0);
		if (num2 == 0)
		{
			return;
		}
		if (GERdrVCDQHvvZNgGiTcIKOdrVZFqA < 0)
		{
			GERdrVCDQHvvZNgGiTcIKOdrVZFqA = num2 - 1;
		}
		else if (GERdrVCDQHvvZNgGiTcIKOdrVZFqA >= num2)
		{
			GERdrVCDQHvvZNgGiTcIKOdrVZFqA = 0;
		}
		yKVVihTZawqctWqwWpKjOJoamNIE = PNNmdtcIAosvoTxnUqoheBqGEpgz[GERdrVCDQHvvZNgGiTcIKOdrVZFqA].sVtcKAcULxFXrzLoatpWBhluUdUs;
		bool flag = false;
		if (gERdrVCDQHvvZNgGiTcIKOdrVZFqA != GERdrVCDQHvvZNgGiTcIKOdrVZFqA || guid != yKVVihTZawqctWqwWpKjOJoamNIE)
		{
			flag = true;
		}
		if (yENweyllzoleyXXfWoeNCooSLgRk == null || flag)
		{
			if (yENweyllzoleyXXfWoeNCooSLgRk != null)
			{
				yENweyllzoleyXXfWoeNCooSLgRk.OBNDhaklAXLCuCkmkMPULPILkHiDb();
			}
			yENweyllzoleyXXfWoeNCooSLgRk = new YOgNgQpZZfYcTITAqIaepzvNafxe(ouhhegqNpQUdwOyRMvRKAjJSSbIu, PNNmdtcIAosvoTxnUqoheBqGEpgz[GERdrVCDQHvvZNgGiTcIKOdrVZFqA].sVtcKAcULxFXrzLoatpWBhluUdUs);
			if (yENweyllzoleyXXfWoeNCooSLgRk == null)
			{
				return;
			}
			IList<bqkAwqSafotyQAOrTnFIVdkzxoBR> list = yENweyllzoleyXXfWoeNCooSLgRk.EsnWDpdGQyPNDHpYkyTMzLERiXiFA();
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if ((list[i].LIGMofzcVgUaeUQojVFAmpVgBEUr.FISdKDwGlUgdCydNnzZTSNmsChCo & ZmXltWbhqdfqHdQeeybxZILIjOaj.Axis) != ZmXltWbhqdfqHdQeeybxZILIjOaj.All)
					{
						yENweyllzoleyXXfWoeNCooSLgRk.JANJnqDdtgdZeQdaCCMdLNmlQEuP.GOCnbILHIjzEOMWvDUYJafackBnq = new mBdPkttZXVmtfEOHgqkZsKGTmdlK(-65535, 65535);
					}
				}
			}
			yENweyllzoleyXXfWoeNCooSLgRk.bcyzOZiMxftRynJRwZLBQhRkvWot();
		}
		kwRPJuhGeGOOTxHiySsCucocjaNE kwRPJuhGeGOOTxHiySsCucocjaNE2;
		try
		{
			kwRPJuhGeGOOTxHiySsCucocjaNE2 = yENweyllzoleyXXfWoeNCooSLgRk.qRHfOwVKbSwGSZpKOGbwsnhTzrsU();
		}
		catch
		{
			kwRPJuhGeGOOTxHiySsCucocjaNE2 = null;
		}
		if (kwRPJuhGeGOOTxHiySsCucocjaNE2 == null)
		{
			return;
		}
		if (num2 > 0)
		{
			vgUSPeXWNJNRAqnTjUubkhwUJJsj = vgUSPeXWNJNRAqnTjUubkhwUJJsj + num2 + " connected devices:\n";
		}
		for (int j = 0; j < num2; j++)
		{
			vgUSPeXWNJNRAqnTjUubkhwUJJsj = vgUSPeXWNJNRAqnTjUubkhwUJJsj + PNNmdtcIAosvoTxnUqoheBqGEpgz[j].VvQGQTZBUkfFSCosnyMActATHIaGb + "\n";
		}
		vgUSPeXWNJNRAqnTjUubkhwUJJsj += "\n";
		vgUSPeXWNJNRAqnTjUubkhwUJJsj = vgUSPeXWNJNRAqnTjUubkhwUJJsj + "Current DI device " + GERdrVCDQHvvZNgGiTcIKOdrVZFqA + ": " + PNNmdtcIAosvoTxnUqoheBqGEpgz[GERdrVCDQHvvZNgGiTcIKOdrVZFqA].VvQGQTZBUkfFSCosnyMActATHIaGb + "\n";
		vgUSPeXWNJNRAqnTjUubkhwUJJsj += "(Press + or - to change monitored device id.)\n\n";
		ArdlVNTRuuHdFZYviKEEcegIzgHA("Identifier", new PidVid(yENweyllzoleyXXfWoeNCooSLgRk.VFOFQOIYEyNtdIgbNSVvmceWfRabb.EbuxgXKWWFMAMpYFAvRpwowfDfQCA));
		ArdlVNTRuuHdFZYviKEEcegIzgHA("Instance GUID", yENweyllzoleyXXfWoeNCooSLgRk.VFOFQOIYEyNtdIgbNSVvmceWfRabb.sVtcKAcULxFXrzLoatpWBhluUdUs);
		ArdlVNTRuuHdFZYviKEEcegIzgHA("Product Id", yENweyllzoleyXXfWoeNCooSLgRk.JANJnqDdtgdZeQdaCCMdLNmlQEuP.hUdFIigItAsRWpFvbtCrpHQlLcXo);
		ArdlVNTRuuHdFZYviKEEcegIzgHA("Device Type", yENweyllzoleyXXfWoeNCooSLgRk.hQXfGyIqaNQWNbVQpOFYUdyDQeXoA.nRZZrwgLRhNesHtuQKDSAdddeCyU.ToString());
		vgUSPeXWNJNRAqnTjUubkhwUJJsj += "\n";
		ArdlVNTRuuHdFZYviKEEcegIzgHA("Axis Count", yENweyllzoleyXXfWoeNCooSLgRk.hQXfGyIqaNQWNbVQpOFYUdyDQeXoA.wYdENNtFNGhijFxfgSqQxUxsLJxC);
		ArdlVNTRuuHdFZYviKEEcegIzgHA("Button Count", yENweyllzoleyXXfWoeNCooSLgRk.hQXfGyIqaNQWNbVQpOFYUdyDQeXoA.aNtvJKNgBcHUcHzbkEovCQIqXZVo);
		ArdlVNTRuuHdFZYviKEEcegIzgHA("Hat Count", yENweyllzoleyXXfWoeNCooSLgRk.hQXfGyIqaNQWNbVQpOFYUdyDQeXoA.DcmbHzmrWWcdqeKaQDnTFbZjtBNdB);
		vgUSPeXWNJNRAqnTjUubkhwUJJsj += "\n";
		if (flag)
		{
			Rewired.Logger.Log("Device Name: \"" + PNNmdtcIAosvoTxnUqoheBqGEpgz[GERdrVCDQHvvZNgGiTcIKOdrVZFqA].VvQGQTZBUkfFSCosnyMActATHIaGb + "\"");
			Rewired.Logger.Log("Identifier: " + new PidVid(yENweyllzoleyXXfWoeNCooSLgRk.VFOFQOIYEyNtdIgbNSVvmceWfRabb.EbuxgXKWWFMAMpYFAvRpwowfDfQCA).ToString());
		}
		for (int k = 0; k < 32; k++)
		{
			int num3 = rspxpmfVYxEuBvghIGjwmIbLDdix((DirectInputAxis)k, kwRPJuhGeGOOTxHiySsCucocjaNE2);
			DirectInputAxis directInputAxis = (DirectInputAxis)k;
			string text = directInputAxis.ToString();
			ArdlVNTRuuHdFZYviKEEcegIzgHA(text, num3 + " (" + miEIjOloYpAhuGSgcjPKyJugTURQ(num3) + ")");
		}
		int[] array = kwRPJuhGeGOOTxHiySsCucocjaNE2.sLReTPhcHDujnRlhoWjmCzOVLNOFA;
		for (int l = 0; l < 4; l++)
		{
			int num4 = array[l];
			string text2 = "Hat " + l;
			ArdlVNTRuuHdFZYviKEEcegIzgHA(text2, num4);
		}
		bool[] array2 = kwRPJuhGeGOOTxHiySsCucocjaNE2.CkhppmbBimKbGpQGgBPruJsQICQd;
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
		ArdlVNTRuuHdFZYviKEEcegIzgHA("Buttons ", text3);
		YHnBjiqFpCEUIHDtCQIymgrdmZwdb.text = vgUSPeXWNJNRAqnTjUubkhwUJJsj;
	}

	void IElementIdentifierTool.Update()
	{
		//ILSpy generated this explicit interface implementation from .override directive in Update
		this.Update();
	}

	private void aBSgylEpYDSWttQqiqYKMehyEWUlA()
	{
		PNNmdtcIAosvoTxnUqoheBqGEpgz = ouhhegqNpQUdwOyRMvRKAjJSSbIu.oOaSusGuwOHJyeCWSaIhJLfJFLBA(sJhqOxXkEhdfJrzCFpdosfXfzYPl.GameControl, QeXRlJhevgcnqiRHTcdYHolJBWgCA.AttachedOnly);
		RemlznNxAbofxhKmGdVvFvStKKINA = ouhhegqNpQUdwOyRMvRKAjJSSbIu.oOaSusGuwOHJyeCWSaIhJLfJFLBA(sJhqOxXkEhdfJrzCFpdosfXfzYPl.All, QeXRlJhevgcnqiRHTcdYHolJBWgCA.AttachedOnly);
		wqAtRCzqaIyMZSTimLGKFDYTgoHbA = ((RemlznNxAbofxhKmGdVvFvStKKINA != null) ? RemlznNxAbofxhKmGdVvFvStKKINA.Count : 0);
	}

	private void qdxxhfxZtrBvVxVbvtOVAGPwBnNK()
	{
		USkVbRGDVlEJGgkjxoJnCOIsCdoHb();
	}

	private void ekENsvofFTmtnUIitAREBHsMxXcjA()
	{
		USkVbRGDVlEJGgkjxoJnCOIsCdoHb();
	}

	private void USkVbRGDVlEJGgkjxoJnCOIsCdoHb()
	{
		aedaHVQLycHqKSilSaDskungzYkQA();
		MrjqoPDcbsBfSqeKXTLHPcieenuw = true;
	}

	private void aedaHVQLycHqKSilSaDskungzYkQA()
	{
		GERdrVCDQHvvZNgGiTcIKOdrVZFqA = 0;
		yENweyllzoleyXXfWoeNCooSLgRk = null;
		yKVVihTZawqctWqwWpKjOJoamNIE = Guid.Empty;
		PNNmdtcIAosvoTxnUqoheBqGEpgz = null;
		RemlznNxAbofxhKmGdVvFvStKKINA = null;
		CKpGCZjjqwEcEGqHcqMFaLqIxTeKA = false;
		MrjqoPDcbsBfSqeKXTLHPcieenuw = false;
		wqAtRCzqaIyMZSTimLGKFDYTgoHbA = 0;
	}

	private void ArdlVNTRuuHdFZYviKEEcegIzgHA(string P_0, object P_1)
	{
		vgUSPeXWNJNRAqnTjUubkhwUJJsj = vgUSPeXWNJNRAqnTjUubkhwUJJsj + P_0 + " = " + P_1.ToString() + "\n";
	}

	private int rspxpmfVYxEuBvghIGjwmIbLDdix(DirectInputAxis P_0, kwRPJuhGeGOOTxHiySsCucocjaNE P_1)
	{
		return P_0 switch
		{
			DirectInputAxis.X => P_1.mWFvViBDEHHBtuTrQQkzQhcMeOOfA, 
			DirectInputAxis.Y => P_1.JeQTyNhGBwbmcxlXcihSQjoiiWFY, 
			DirectInputAxis.Z => P_1.OIYSEzHkHxXvwrFZDVAQDecWqfUQ, 
			DirectInputAxis.RotationX => P_1.MHmeaCgmxlNobcbbPPITbdWBXKQi, 
			DirectInputAxis.RotationY => P_1.UpxRgEicuEQvDPxFXviqGQTcKlnJ, 
			DirectInputAxis.RotationZ => P_1.DUDmrSoEuvtnGjauGPYMCTGOIjqS, 
			DirectInputAxis.Slider0 => P_1.uRzpIYdZVItVnpMTmqAWgfYCEwco[0], 
			DirectInputAxis.Slider1 => P_1.uRzpIYdZVItVnpMTmqAWgfYCEwco[1], 
			DirectInputAxis.VelocityX => P_1.LVYKSfpyeNCFfQKMUaSMwgIZAtoz, 
			DirectInputAxis.VelocityY => P_1.jFgnZxRAbfleqtcyUIWCNFdtLJsh, 
			DirectInputAxis.VelocityZ => P_1.sNsitztzeACpUZMEJedTeTXkidWFb, 
			DirectInputAxis.AngularVelocityX => P_1.vGheOmKMqvcmIcYFAXWeiQMYxsPfb, 
			DirectInputAxis.AngularVelocityY => P_1.ymsdogeiGGYEBgsDxEPxeQVeHCOjB, 
			DirectInputAxis.AngularVelocityZ => P_1.djlGynKiFuEaeXACJaLReDyxcVMoA, 
			DirectInputAxis.VelocitySlider0 => P_1.rEJcHQjvgxJeFfTvoEeLyGYVHsWP[0], 
			DirectInputAxis.VelocitySlider1 => P_1.rEJcHQjvgxJeFfTvoEeLyGYVHsWP[1], 
			DirectInputAxis.AccelerationX => P_1.lYBcyRYSHQIEAVVRiIAEoILgGQjfA, 
			DirectInputAxis.AccelerationY => P_1.knOJBucJwpaiJguvaktiHssSwQce, 
			DirectInputAxis.AccelerationZ => P_1.gtBEqzqDRGqBjyvOCAfkZbputzNI, 
			DirectInputAxis.AngularAccelerationX => P_1.vavGCkeNxGiaFYRlfykNpeWqQefoA, 
			DirectInputAxis.AngularAccelerationY => P_1.qtMIcLlEGmnjeXqCQDqEeuokHhHh, 
			DirectInputAxis.AngularAccelerationZ => P_1.mBgfZrbiCscPuDzWDLswiBTrinVR, 
			DirectInputAxis.AccelerationSlider0 => P_1.VMigiindKyBQgLFlUizNSkZoVeGS[0], 
			DirectInputAxis.AccelerationSlider1 => P_1.VMigiindKyBQgLFlUizNSkZoVeGS[1], 
			DirectInputAxis.ForceX => P_1.PiHqOxPNhXdPWjlEmHGxAYiIrQVE, 
			DirectInputAxis.ForceY => P_1.hmOhybbljMMblKHLwhLjHHdIVVOC, 
			DirectInputAxis.ForceZ => P_1.ParClsmiGwiJOyXFbgqFfHhYljY, 
			DirectInputAxis.TorqueX => P_1.qbfNpNNHQaPYKFbtJOLChHcXckUc, 
			DirectInputAxis.TorqueY => P_1.FGVflzDYmZzDDBfmykVrZCbLAGDLA, 
			DirectInputAxis.TorqueZ => P_1.VBLGxvvyXDqaLvtDvHcAKrzXMXZd, 
			DirectInputAxis.ForceSlider0 => P_1.IWMXxMWjRBYYCvsGIoxsRUUBZaH[0], 
			DirectInputAxis.ForceSlider1 => P_1.IWMXxMWjRBYYCvsGIoxsRUUBZaH[1], 
			_ => 0, 
		};
	}

	private float miEIjOloYpAhuGSgcjPKyJugTURQ(int P_0)
	{
		if (P_0 == 0)
		{
			return 0f;
		}
		return MathTools.Clamp((float)MathTools.Abs(P_0) / 65535f * (float)MathTools.Sign(P_0), -1f, 1f);
	}

	public void OnDestroy()
	{
		if (yENweyllzoleyXXfWoeNCooSLgRk != null)
		{
			yENweyllzoleyXXfWoeNCooSLgRk.OBNDhaklAXLCuCkmkMPULPILkHiDb();
		}
	}

	void IElementIdentifierTool.OnDestroy()
	{
		//ILSpy generated this explicit interface implementation from .override directive in OnDestroy
		this.OnDestroy();
	}
}
