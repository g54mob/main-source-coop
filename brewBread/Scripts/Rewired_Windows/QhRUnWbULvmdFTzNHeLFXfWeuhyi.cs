using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Rewired;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Libraries.SharpDX.XInput;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

internal class QhRUnWbULvmdFTzNHeLFXfWeuhyi : PlatformInputManager
{
	private class OcxJGVolhfPasVdbifhMjzXBDVEd : IInputManagerJoystick, IInputManagerJoystickPublic, IDisposable
	{
		private bool GOnGCgYNNmgWeUvzYeLHfadqviaiA;

		private int dsZaaHQQniFEuaWwThJKBZCdgOQGA;

		private readonly int tnxtekCgMpSEJreJEAlSJbZpsmPR;

		public Guid zuApxEmgDBFmvkaOBnYuUCnBtIMn;

		public string tqIfLOspdMCCztTNShlwdTGJIKHY;

		public Guid EHtSVEDMGFNNxjAgYHlvciLhZqtv;

		public Rewired.Libraries.SharpDX.XInput.DeviceType mTNuatFiUwMvsnCHEKHfFIlibxJCA;

		public XInputDeviceSubType WmGUXMLdKObUkBomplWQnKJsvVVBA;

		public bool KzlHYrCaZVMIWVTPrmlMzWlIIkQW;

		public bool VbbnOuicgvHEuDhmWqNoZyktHBTf;

		public bool EtOciWRBDomBqcEQItHEcSEOtAHF;

		public bool eKcWHeiuDjBZLxCtvCsgstBOqXxe;

		private int bcigoYujygHhyEkcFPsAdJkvKzFb;

		private int gVNTADqePSyRWUNCfooSbyWJqbhA;

		private int PREACxwTPuKpBjsETieBvSfZzwpD;

		private int AyjdEuFcBKUXHJNkJZSPFHQLUvjnA;

		private readonly float[] tOZRhGxcfwMOaAFRxMekfiimnUQh;

		private readonly bool[] LpdmGlOWEtCNdwxEVvJweuCMbsSq;

		private HardwareJoystickMap_InputManager dwYNbWPkKijWbgSzahScVNSsqYHKA;

		public readonly hTqbXbSaCGbbKiuhgXToQLOuaIxwA MMTtbRDiEhfNsofvfEMKtHrGMMsL;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> XDfJqiwLYIwEbgMhAkMbksxTquAQ;

		private Action XjmpJeKyQDaGnFfhvdTdehdyhqBJA;

		private bool PzXedBlwGbkPNudHYEPuPBjzxgSm;

		private bool fyWigVzEzgjzBuyeeWRFUCzHcoOR;

		private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

		public string KXaFvuABkBbilSCsCmPbdwkGovqab
		{
			get
			{
				string text = qilHnOVTPZzmNJFjtGtmFgiOCyRx;
				if (text == string.Empty)
				{
					return string.Empty;
				}
				return text + " " + tnxtekCgMpSEJreJEAlSJbZpsmPR;
			}
		}

		public string qilHnOVTPZzmNJFjtGtmFgiOCyRx
		{
			get
			{
				if (!JuDoJMMSXVroARSjTyeCKpDYkYCj)
				{
					return string.Empty;
				}
				return WmGUXMLdKObUkBomplWQnKJsvVVBA.ToString();
			}
		}

		public bool JuDoJMMSXVroARSjTyeCKpDYkYCj
		{
			get
			{
				if (MMTtbRDiEhfNsofvfEMKtHrGMMsL == null || !eKcWHeiuDjBZLxCtvCsgstBOqXxe)
				{
					return false;
				}
				if (PzXedBlwGbkPNudHYEPuPBjzxgSm && !qjXIrGEEhXMtoyIjGmcNGBrgGSlU(mSpxQiVMUwFOidmosdvjPXPgCyyEb.Asynchronous))
				{
					ABCSbZgAbavfBIbQcNrSWKDMBXcFA();
				}
				return PzXedBlwGbkPNudHYEPuPBjzxgSm;
			}
		}

		[CustomObfuscation(rename = false)]
		public int rewiredId
		{
			get
			{
				return dsZaaHQQniFEuaWwThJKBZCdgOQGA;
			}
			set
			{
				dsZaaHQQniFEuaWwThJKBZCdgOQGA = value;
			}
		}

		[CustomObfuscation(rename = false)]
		public int inputManagerId => tnxtekCgMpSEJreJEAlSJbZpsmPR;

		[CustomObfuscation(rename = false)]
		public string name
		{
			get
			{
				if (GOnGCgYNNmgWeUvzYeLHfadqviaiA)
				{
					return WmGUXMLdKObUkBomplWQnKJsvVVBA.ToString() + " " + (tnxtekCgMpSEJreJEAlSJbZpsmPR + 1);
				}
				return "XInput " + WmGUXMLdKObUkBomplWQnKJsvVVBA.ToString() + " " + (tnxtekCgMpSEJreJEAlSJbZpsmPR + 1);
			}
		}

		[CustomObfuscation(rename = false)]
		public long? systemId => tnxtekCgMpSEJreJEAlSJbZpsmPR;

		[CustomObfuscation(rename = false)]
		public int unityId => 0;

		[CustomObfuscation(rename = false)]
		public Controller.Extension extension => null;

		[CustomObfuscation(rename = false)]
		public Guid instanceGuid => EHtSVEDMGFNNxjAgYHlvciLhZqtv;

		[CustomObfuscation(rename = false)]
		public Guid persistentGuid => instanceGuid;

		[CustomObfuscation(rename = false)]
		public void SetVibration(float amount, int motorIndex)
		{
			MMTtbRDiEhfNsofvfEMKtHrGMMsL.GhkbHidWDFgyXKWOQVKBFIymzDPjA(amount, motorIndex);
		}

		[CustomObfuscation(rename = false)]
		public void StopVibration()
		{
			MMTtbRDiEhfNsofvfEMKtHrGMMsL.QVtcEbpATNFXldOWOuhvjtoVFlb();
		}

		public OcxJGVolhfPasVdbifhMjzXBDVEd(int P_0, bool P_1, hTqbXbSaCGbbKiuhgXToQLOuaIxwA P_2, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_3, Action P_4)
		{
			MMTtbRDiEhfNsofvfEMKtHrGMMsL = P_2;
			GOnGCgYNNmgWeUvzYeLHfadqviaiA = P_1;
			tnxtekCgMpSEJreJEAlSJbZpsmPR = P_0;
			XDfJqiwLYIwEbgMhAkMbksxTquAQ = P_3;
			XjmpJeKyQDaGnFfhvdTdehdyhqBJA = P_4;
			dsZaaHQQniFEuaWwThJKBZCdgOQGA = -1;
			bcigoYujygHhyEkcFPsAdJkvKzFb = 6;
			gVNTADqePSyRWUNCfooSbyWJqbhA = 15;
			PREACxwTPuKpBjsETieBvSfZzwpD = bcigoYujygHhyEkcFPsAdJkvKzFb;
			AyjdEuFcBKUXHJNkJZSPFHQLUvjnA = gVNTADqePSyRWUNCfooSbyWJqbhA;
			tOZRhGxcfwMOaAFRxMekfiimnUQh = new float[bcigoYujygHhyEkcFPsAdJkvKzFb];
			LpdmGlOWEtCNdwxEVvJweuCMbsSq = new bool[gVNTADqePSyRWUNCfooSbyWJqbhA];
			tVvNlkncUOQnSBnilDhSFTlIVGq();
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			MMTtbRDiEhfNsofvfEMKtHrGMMsL.cuFmCfHdMKWDocOGeIIRHXWTOpkEA();
			bool[] array = MMTtbRDiEhfNsofvfEMKtHrGMMsL.tuufDWVlHDfpSHKzDcghqCXCzcVtA;
			ySBJVFLOzJJBLVFbFmjbAWZAqdrt(array, ref MMTtbRDiEhfNsofvfEMKtHrGMMsL.JFsBApJkkZmclnxpAyfybvKeBHFl);
			DwdAFSDeLDzJmbMthNtMATbAZroYB(array, ref MMTtbRDiEhfNsofvfEMKtHrGMMsL.JFsBApJkkZmclnxpAyfybvKeBHFl);
			MMTtbRDiEhfNsofvfEMKtHrGMMsL.UXsGlgdEPfSLHGRZMVqvnDmuNwHcA();
		}

		public void hzVXylspAStQLNzufXVziRmulsUf(bool P_0)
		{
			if (MMTtbRDiEhfNsofvfEMKtHrGMMsL != null)
			{
				EtOciWRBDomBqcEQItHEcSEOtAHF = P_0;
			}
		}

		public bool qjXIrGEEhXMtoyIjGmcNGBrgGSlU(mSpxQiVMUwFOidmosdvjPXPgCyyEb P_0)
		{
			iYtdkedLLoIExpmcNsjdFkVkGKMac(jaYHvLybRgnKQpLxyEoqcJpRhhKV(P_0));
			return PzXedBlwGbkPNudHYEPuPBjzxgSm;
		}

		public bool jaYHvLybRgnKQpLxyEoqcJpRhhKV(mSpxQiVMUwFOidmosdvjPXPgCyyEb P_0)
		{
			if (MMTtbRDiEhfNsofvfEMKtHrGMMsL == null)
			{
				return false;
			}
			return MMTtbRDiEhfNsofvfEMKtHrGMMsL.jaYHvLybRgnKQpLxyEoqcJpRhhKV(P_0);
		}

		public void iYtdkedLLoIExpmcNsjdFkVkGKMac(bool P_0)
		{
			PzXedBlwGbkPNudHYEPuPBjzxgSm = P_0;
		}

		public void SEZecPHwRFkiwnwPqWOERGndlgZB()
		{
			if (!eKcWHeiuDjBZLxCtvCsgstBOqXxe || VKezwdbhvztSZSfplKYbawHfAlSr())
			{
				tVvNlkncUOQnSBnilDhSFTlIVGq();
			}
			if (eKcWHeiuDjBZLxCtvCsgstBOqXxe && PzXedBlwGbkPNudHYEPuPBjzxgSm)
			{
				MMTtbRDiEhfNsofvfEMKtHrGMMsL.mYzmWMRBpIzVtmFinXeJttVSUihm();
			}
		}

		public void zpkArhFROmzNjuKNleYXtlbiCFnSA()
		{
			dsZaaHQQniFEuaWwThJKBZCdgOQGA = -1;
			eKcWHeiuDjBZLxCtvCsgstBOqXxe = false;
			MMTtbRDiEhfNsofvfEMKtHrGMMsL.HXahjLaEqKAdOEjLEsQanbmcYlbLC();
			Array.Clear(tOZRhGxcfwMOaAFRxMekfiimnUQh, 0, tOZRhGxcfwMOaAFRxMekfiimnUQh.Length);
			Array.Clear(LpdmGlOWEtCNdwxEVvJweuCMbsSq, 0, LpdmGlOWEtCNdwxEVvJweuCMbsSq.Length);
		}

		[CustomObfuscation(rename = false)]
		public void FillData(ControllerDataUpdater dataUpdater)
		{
			if (bcigoYujygHhyEkcFPsAdJkvKzFb != dataUpdater.axisCount || gVNTADqePSyRWUNCfooSbyWJqbhA != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			for (int i = 0; i < bcigoYujygHhyEkcFPsAdJkvKzFb; i++)
			{
				dataUpdater.axisValues[i] = tOZRhGxcfwMOaAFRxMekfiimnUQh[i];
			}
			for (int j = 0; j < gVNTADqePSyRWUNCfooSbyWJqbhA; j++)
			{
				dataUpdater.buttonValues[j] = LpdmGlOWEtCNdwxEVvJweuCMbsSq[j];
			}
			if (fyWigVzEzgjzBuyeeWRFUCzHcoOR && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		public BridgedControllerHWInfo jRVbfabpmvrPSdwCpuRbtMWfovefA()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
			IprQKbzqZEVBFNAsPdNPLBiBfGtcA(bridgedControllerHWInfo);
			return bridgedControllerHWInfo;
		}

		[CustomObfuscation(rename = false)]
		public BridgedController ToBridgedController()
		{
			BridgedController bridgedController = new BridgedController();
			IprQKbzqZEVBFNAsPdNPLBiBfGtcA(bridgedController);
			return bridgedController;
		}

		[CustomObfuscation(rename = false)]
		public ControllerDisconnectedEventArgs ToControllerDisconnectedEventArgs()
		{
			return new ControllerDisconnectedEventArgs(dsZaaHQQniFEuaWwThJKBZCdgOQGA);
		}

		private void tVvNlkncUOQnSBnilDhSFTlIVGq()
		{
			if (MMTtbRDiEhfNsofvfEMKtHrGMMsL == null || !qjXIrGEEhXMtoyIjGmcNGBrgGSlU(mSpxQiVMUwFOidmosdvjPXPgCyyEb.Synchronous))
			{
				return;
			}
			try
			{
				nhiAwIACrbZMhcxkHxmYybOoRSMF();
				vkLQdmECkuLRduGdNYgGdjIRmYkU vkLQdmECkuLRduGdNYgGdjIRmYkU2 = MMTtbRDiEhfNsofvfEMKtHrGMMsL.GwYNpRkSmDCSiLMwAYWsVmzkHudR.scceFLceYcWIDewhxuegIiYbGtWh(CBcLxjxrGQlvGCLKnksXbuQbLsqS.Any);
				mTNuatFiUwMvsnCHEKHfFIlibxJCA = vkLQdmECkuLRduGdNYgGdjIRmYkU2.lhIrpCzAlkfgVHpeLkMrWybTDiDQA;
				WmGUXMLdKObUkBomplWQnKJsvVVBA = (XInputDeviceSubType)vkLQdmECkuLRduGdNYgGdjIRmYkU2.bAxnmRsHNztOpEZbmuTvjBkXFACF;
				if (MMTtbRDiEhfNsofvfEMKtHrGMMsL.GwYNpRkSmDCSiLMwAYWsVmzkHudR.GhkbHidWDFgyXKWOQVKBFIymzDPjA(default(nSQKzuFgkuUgePhnvOTkeuGuQDqo)).SbTUMPJTIURfYayZOoNGumrJrrIs)
				{
					KzlHYrCaZVMIWVTPrmlMzWlIIkQW = true;
				}
				VbbnOuicgvHEuDhmWqNoZyktHBTf = (vkLQdmECkuLRduGdNYgGdjIRmYkU2.ZIvyiOSzgMRCzZiwgApXsRrDjjHn & XWxpDJVDozpcqPyfFMlsajQuGlV.VoiceSupported) == XWxpDJVDozpcqPyfFMlsajQuGlV.VoiceSupported;
				tiGojvGhqDCejmeVHrELbGoOyoCn();
				zuApxEmgDBFmvkaOBnYuUCnBtIMn = dwYNbWPkKijWbgSzahScVNSsqYHKA.hardwareMapIdentifier.guid;
				tqIfLOspdMCCztTNShlwdTGJIKHY = dwYNbWPkKijWbgSzahScVNSsqYHKA.controllerName;
				MMTtbRDiEhfNsofvfEMKtHrGMMsL.mYzmWMRBpIzVtmFinXeJttVSUihm();
				EHtSVEDMGFNNxjAgYHlvciLhZqtv = MiscTools.CreateGuidHashSHA1(string.Concat(mTNuatFiUwMvsnCHEKHfFIlibxJCA, WmGUXMLdKObUkBomplWQnKJsvVVBA, tnxtekCgMpSEJreJEAlSJbZpsmPR));
				eKcWHeiuDjBZLxCtvCsgstBOqXxe = true;
			}
			catch (Exception)
			{
				eKcWHeiuDjBZLxCtvCsgstBOqXxe = false;
				PzXedBlwGbkPNudHYEPuPBjzxgSm = false;
				EHtSVEDMGFNNxjAgYHlvciLhZqtv = Guid.Empty;
			}
		}

		private bool VKezwdbhvztSZSfplKYbawHfAlSr()
		{
			try
			{
				if (WmGUXMLdKObUkBomplWQnKJsvVVBA != (XInputDeviceSubType)MMTtbRDiEhfNsofvfEMKtHrGMMsL.GwYNpRkSmDCSiLMwAYWsVmzkHudR.scceFLceYcWIDewhxuegIiYbGtWh(CBcLxjxrGQlvGCLKnksXbuQbLsqS.Any).bAxnmRsHNztOpEZbmuTvjBkXFACF)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		private void nhiAwIACrbZMhcxkHxmYybOoRSMF()
		{
			VbbnOuicgvHEuDhmWqNoZyktHBTf = false;
			KzlHYrCaZVMIWVTPrmlMzWlIIkQW = false;
			EtOciWRBDomBqcEQItHEcSEOtAHF = false;
			eKcWHeiuDjBZLxCtvCsgstBOqXxe = false;
		}

		private void ABCSbZgAbavfBIbQcNrSWKDMBXcFA()
		{
			if (XjmpJeKyQDaGnFfhvdTdehdyhqBJA != null)
			{
				XjmpJeKyQDaGnFfhvdTdehdyhqBJA();
			}
			MMTtbRDiEhfNsofvfEMKtHrGMMsL.HXahjLaEqKAdOEjLEsQanbmcYlbLC();
		}

		private void ySBJVFLOzJJBLVFbFmjbAWZAqdrt(bool[] P_0, ref OrBWtmRDFzukyjjKebjqLenVxLmw P_1)
		{
			HardwareJoystickMap.Platform_XInput_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_XInput_Base)dwYNbWPkKijWbgSzahScVNSsqYHKA.map).Axes_orig;
			if (axes_orig == null)
			{
				return;
			}
			for (int i = 0; i < axes_orig.Length; i++)
			{
				if (i >= bcigoYujygHhyEkcFPsAdJkvKzFb)
				{
					throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
				}
				tOZRhGxcfwMOaAFRxMekfiimnUQh[i] = mPKaIHfzhogaSXQJCwZlMPTETFwmA(axes_orig[i], P_0, ref P_1);
				if (!fyWigVzEzgjzBuyeeWRFUCzHcoOR && tOZRhGxcfwMOaAFRxMekfiimnUQh[i] != 0f)
				{
					fyWigVzEzgjzBuyeeWRFUCzHcoOR = true;
				}
			}
		}

		private void DwdAFSDeLDzJmbMthNtMATbAZroYB(bool[] P_0, ref OrBWtmRDFzukyjjKebjqLenVxLmw P_1)
		{
			HardwareJoystickMap.Platform_XInput_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_XInput_Base)dwYNbWPkKijWbgSzahScVNSsqYHKA.map).Buttons_orig;
			if (buttons_orig == null)
			{
				return;
			}
			for (int i = 0; i < buttons_orig.Length; i++)
			{
				if (i >= gVNTADqePSyRWUNCfooSbyWJqbhA)
				{
					throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
				}
				LpdmGlOWEtCNdwxEVvJweuCMbsSq[i] = CTRqAkNbPTeaCEiQihtiarTqVmzG(buttons_orig[i], P_0, ref P_1);
				if (!fyWigVzEzgjzBuyeeWRFUCzHcoOR && LpdmGlOWEtCNdwxEVvJweuCMbsSq[i])
				{
					fyWigVzEzgjzBuyeeWRFUCzHcoOR = true;
				}
			}
		}

		private float mPKaIHfzhogaSXQJCwZlMPTETFwmA(HardwareJoystickMap.Platform_XInput_Base.Axis P_0, bool[] P_1, ref OrBWtmRDFzukyjjKebjqLenVxLmw P_2)
		{
			if (P_0.sourceType == HardwareElementSourceType.Axis)
			{
				if (P_0.sourceAxis == XInputAxis.None)
				{
					return 0f;
				}
				return mPKaIHfzhogaSXQJCwZlMPTETFwmA(P_0.sourceAxis, ref P_2);
			}
			if (P_0.sourceType == HardwareElementSourceType.Button)
			{
				if (P_0.sourceButton == XInputButton.None)
				{
					return 0f;
				}
				if (!CTRqAkNbPTeaCEiQihtiarTqVmzG(P_0.sourceButton, P_1))
				{
					return 0f;
				}
				if (P_0.buttonAxisContribution == Pole.Positive)
				{
					return 1f;
				}
				return -1f;
			}
			return 0f;
		}

		private float mPKaIHfzhogaSXQJCwZlMPTETFwmA(XInputAxis P_0, ref OrBWtmRDFzukyjjKebjqLenVxLmw P_1)
		{
			return P_0 switch
			{
				XInputAxis.LeftThumbX => hTqbXbSaCGbbKiuhgXToQLOuaIxwA.FdAIjbgcfatFrghHnIipFWDBvhqyA(P_1.IXadPWOaEOjhaibAMcagSWUENZHKA), 
				XInputAxis.LeftThumbY => hTqbXbSaCGbbKiuhgXToQLOuaIxwA.FdAIjbgcfatFrghHnIipFWDBvhqyA(P_1.yHAVaJMLRhAFVRsztnGDCDgsbKZH), 
				XInputAxis.RightThumbX => hTqbXbSaCGbbKiuhgXToQLOuaIxwA.FdAIjbgcfatFrghHnIipFWDBvhqyA(P_1.uYEFdqkHSnctFdAqxbiKLQEyuTShA), 
				XInputAxis.RightThumbY => hTqbXbSaCGbbKiuhgXToQLOuaIxwA.FdAIjbgcfatFrghHnIipFWDBvhqyA(P_1.CnnQoQkCuYWBpRyForWdVgZwIfnk), 
				XInputAxis.LeftTrigger => hTqbXbSaCGbbKiuhgXToQLOuaIxwA.MsmttBzqGneydBeJePmdHRMgtZUDB(P_1.cQwIuxmdMVOscpetaAvxRxURGJShA), 
				XInputAxis.RightTrigger => hTqbXbSaCGbbKiuhgXToQLOuaIxwA.MsmttBzqGneydBeJePmdHRMgtZUDB(P_1.AnqVjrzlDVpSmyuIFnJNfBHUfebM), 
				_ => 0f, 
			};
		}

		private bool CTRqAkNbPTeaCEiQihtiarTqVmzG(HardwareJoystickMap.Platform_XInput_Base.Button P_0, bool[] P_1, ref OrBWtmRDFzukyjjKebjqLenVxLmw P_2)
		{
			if (P_0.sourceType == HardwareElementSourceType.Button)
			{
				if (P_0.sourceButton == XInputButton.None)
				{
					return false;
				}
				return CTRqAkNbPTeaCEiQihtiarTqVmzG(P_0.sourceButton, P_1);
			}
			if (P_0.sourceType == HardwareElementSourceType.Axis)
			{
				if (P_0.sourceAxis == XInputAxis.None)
				{
					return false;
				}
				float num = mPKaIHfzhogaSXQJCwZlMPTETFwmA(P_0.sourceAxis, ref P_2);
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
			return false;
		}

		private bool CTRqAkNbPTeaCEiQihtiarTqVmzG(XInputButton P_0, bool[] P_1)
		{
			return P_0 switch
			{
				XInputButton.DPadUp => P_1[0], 
				XInputButton.DPadDown => P_1[1], 
				XInputButton.DPadLeft => P_1[2], 
				XInputButton.DPadRight => P_1[3], 
				XInputButton.Start => P_1[4], 
				XInputButton.Back => P_1[5], 
				XInputButton.LeftThumb => P_1[6], 
				XInputButton.RightThumb => P_1[7], 
				XInputButton.LeftShoulder => P_1[8], 
				XInputButton.RightShoulder => P_1[9], 
				XInputButton.Guide => P_1[10], 
				XInputButton.A => P_1[11], 
				XInputButton.B => P_1[12], 
				XInputButton.X => P_1[13], 
				XInputButton.Y => P_1[14], 
				_ => false, 
			};
		}

		private void tiGojvGhqDCejmeVHrELbGoOyoCn()
		{
			dwYNbWPkKijWbgSzahScVNSsqYHKA = XDfJqiwLYIwEbgMhAkMbksxTquAQ(jRVbfabpmvrPSdwCpuRbtMWfovefA());
			if (dwYNbWPkKijWbgSzahScVNSsqYHKA == null)
			{
				Rewired.Logger.LogError("Default hardware map not found!");
				return;
			}
			bcigoYujygHhyEkcFPsAdJkvKzFb = dwYNbWPkKijWbgSzahScVNSsqYHKA.axisCount;
			gVNTADqePSyRWUNCfooSbyWJqbhA = dwYNbWPkKijWbgSzahScVNSsqYHKA.buttonCount;
		}

		private bool fdxJiAXrZxmkReisQyqxIeFEdHbp(ref nSQKzuFgkuUgePhnvOTkeuGuQDqo P_0)
		{
			if (P_0.nZIhNrWMHpvRIUFXTYGPVEGNGUpO > 0 || P_0.sPQMTnJyYYFuCgqeuiLRnxXvWcYQ > 0)
			{
				return true;
			}
			return false;
		}

		private void ISZIZWZJApmHMFmVeAiktNIxuqPg(ref nSQKzuFgkuUgePhnvOTkeuGuQDqo P_0)
		{
			P_0.nZIhNrWMHpvRIUFXTYGPVEGNGUpO = 0;
			P_0.sPQMTnJyYYFuCgqeuiLRnxXvWcYQ = 0;
		}

		private void MekdpJrGnBPbTvGISCpKJdKXYFZc(ref nSQKzuFgkuUgePhnvOTkeuGuQDqo P_0, ref nSQKzuFgkuUgePhnvOTkeuGuQDqo P_1)
		{
			P_1.nZIhNrWMHpvRIUFXTYGPVEGNGUpO = P_0.nZIhNrWMHpvRIUFXTYGPVEGNGUpO;
			P_1.sPQMTnJyYYFuCgqeuiLRnxXvWcYQ = P_0.sPQMTnJyYYFuCgqeuiLRnxXvWcYQ;
		}

		private string dFDPslyjEmlkmhoLdsxXJbtXkXJ()
		{
			return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{InputSource.XInput.ToString()}{mTNuatFiUwMvsnCHEKHfFIlibxJCA.ToString()}{WmGUXMLdKObUkBomplWQnKJsvVVBA.ToString()}");
		}

		private void IprQKbzqZEVBFNAsPdNPLBiBfGtcA(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.XInput;
			P_0.inputSource = P_0.inputManagerSource;
			P_0.deviceType = ControlDeviceType.Unknown;
			P_0.hardwareIdentifier = dFDPslyjEmlkmhoLdsxXJbtXkXJ();
			P_0.hardwareAxisCount = PREACxwTPuKpBjsETieBvSfZzwpD;
			P_0.hardwareButtonCount = AyjdEuFcBKUXHJNkJZSPFHQLUvjnA;
			P_0.hardwareHatCount = 0;
			P_0.hw_productName = qilHnOVTPZzmNJFjtGtmFgiOCyRx;
			P_0.hw_supportsVoice = VbbnOuicgvHEuDhmWqNoZyktHBTf;
			P_0.hw_supportsVibration = KzlHYrCaZVMIWVTPrmlMzWlIIkQW;
			P_0.hw_localVibrationMotorCount = (KzlHYrCaZVMIWVTPrmlMzWlIIkQW ? 2 : 0);
			P_0.hw_xInputSubType = WmGUXMLdKObUkBomplWQnKJsvVVBA;
		}

		private void IprQKbzqZEVBFNAsPdNPLBiBfGtcA(BridgedController P_0)
		{
			IprQKbzqZEVBFNAsPdNPLBiBfGtcA((BridgedControllerHWInfo)P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = dwYNbWPkKijWbgSzahScVNSsqYHKA.ToGameHardwareControllerMap();
			P_0.instanceName = "XInput " + KXaFvuABkBbilSCsCmPbdwkGovqab;
			P_0.productName = "XInput " + qilHnOVTPZzmNJFjtGtmFgiOCyRx;
			P_0.isXInputDevice = true;
			P_0.axisCount = bcigoYujygHhyEkcFPsAdJkvKzFb;
			P_0.buttonCount = gVNTADqePSyRWUNCfooSbyWJqbhA;
			P_0.controllerTypeGuid = zuApxEmgDBFmvkaOBnYuUCnBtIMn;
			P_0.controllerExtension = extension;
		}

		public void Dispose()
		{
			lDxnsjCDTQrmresvWgbliNUVruIc(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void zNJVymYugIbeeZuNgMrKxyYWbziV()
		{
			try
			{
				lDxnsjCDTQrmresvWgbliNUVruIc(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
		{
			if (NchdYNbKzqsssgcQJdenZuGqXgLo)
			{
				return;
			}
			if (P_0)
			{
				if (JuDoJMMSXVroARSjTyeCKpDYkYCj)
				{
					MMTtbRDiEhfNsofvfEMKtHrGMMsL.wnYdYZpaeCwbldWKcWxEGKxjpfsR();
				}
				if (MMTtbRDiEhfNsofvfEMKtHrGMMsL != null)
				{
					MMTtbRDiEhfNsofvfEMKtHrGMMsL.Dispose();
				}
			}
			NchdYNbKzqsssgcQJdenZuGqXgLo = true;
		}
	}

	private class VXItTSauYxYTlYaABLtKshlmfhFh
	{
		private class aOnVvwGlzTUFEmzcTdfcyPOaeFwA
		{
			public bool HlITrbeQjYGRHGnEvesxxqhggxBF;

			public int iTZlVgtCqSxzFnKheClbpmeFvLmG;

			public XInputDeviceSubType WmGUXMLdKObUkBomplWQnKJsvVVBA;

			public void mPVLsAWcaTKfbIPORRXexWhffTDm(OcxJGVolhfPasVdbifhMjzXBDVEd P_0, bool P_1)
			{
				HlITrbeQjYGRHGnEvesxxqhggxBF = P_1;
				iTZlVgtCqSxzFnKheClbpmeFvLmG = P_0.rewiredId;
				WmGUXMLdKObUkBomplWQnKJsvVVBA = P_0.WmGUXMLdKObUkBomplWQnKJsvVVBA;
			}

			public aOnVvwGlzTUFEmzcTdfcyPOaeFwA(int P_0, XInputDeviceSubType P_1)
			{
				iTZlVgtCqSxzFnKheClbpmeFvLmG = P_0;
				WmGUXMLdKObUkBomplWQnKJsvVVBA = P_1;
			}
		}

		private List<aOnVvwGlzTUFEmzcTdfcyPOaeFwA> JijZVkKtCuneUBOsMGIhmCybOFwL;

		public VXItTSauYxYTlYaABLtKshlmfhFh()
		{
			JijZVkKtCuneUBOsMGIhmCybOFwL = new List<aOnVvwGlzTUFEmzcTdfcyPOaeFwA>();
		}

		public void RBQmkHWZJfxhkxpcpdAYTuhueORX(OcxJGVolhfPasVdbifhMjzXBDVEd P_0, bool P_1)
		{
			if (uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0.rewiredId, P_0.WmGUXMLdKObUkBomplWQnKJsvVVBA, true) < 0)
			{
				aOnVvwGlzTUFEmzcTdfcyPOaeFwA aOnVvwGlzTUFEmzcTdfcyPOaeFwA2 = new aOnVvwGlzTUFEmzcTdfcyPOaeFwA(P_0.rewiredId, P_0.WmGUXMLdKObUkBomplWQnKJsvVVBA);
				aOnVvwGlzTUFEmzcTdfcyPOaeFwA2.HlITrbeQjYGRHGnEvesxxqhggxBF = P_1;
				JijZVkKtCuneUBOsMGIhmCybOFwL.Add(aOnVvwGlzTUFEmzcTdfcyPOaeFwA2);
			}
		}

		public void mPVLsAWcaTKfbIPORRXexWhffTDm(int P_0, OcxJGVolhfPasVdbifhMjzXBDVEd P_1, bool P_2)
		{
			if (P_0 >= 0 && P_0 < JijZVkKtCuneUBOsMGIhmCybOFwL.Count)
			{
				JijZVkKtCuneUBOsMGIhmCybOFwL[P_0].mPVLsAWcaTKfbIPORRXexWhffTDm(P_1, P_2);
			}
		}

		public int VAYqhqAqTZNUiYZXCjbwXeXSnnMB(XInputDeviceSubType P_0, bool P_1)
		{
			int count = JijZVkKtCuneUBOsMGIhmCybOFwL.Count;
			for (int i = 0; i < count; i++)
			{
				if ((P_1 || !JijZVkKtCuneUBOsMGIhmCybOFwL[i].HlITrbeQjYGRHGnEvesxxqhggxBF) && JijZVkKtCuneUBOsMGIhmCybOFwL[i].WmGUXMLdKObUkBomplWQnKJsvVVBA == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		public int uWrVYCSALeSsGsvUfnkTZtSRlidv(int P_0, XInputDeviceSubType P_1, bool P_2)
		{
			int count = JijZVkKtCuneUBOsMGIhmCybOFwL.Count;
			for (int i = 0; i < count; i++)
			{
				if ((P_2 || !JijZVkKtCuneUBOsMGIhmCybOFwL[i].HlITrbeQjYGRHGnEvesxxqhggxBF) && JijZVkKtCuneUBOsMGIhmCybOFwL[i].iTZlVgtCqSxzFnKheClbpmeFvLmG == P_0 && JijZVkKtCuneUBOsMGIhmCybOFwL[i].WmGUXMLdKObUkBomplWQnKJsvVVBA == P_1)
				{
					return i;
				}
			}
			return -1;
		}

		public int qIPaWKBCnNRJFDlWXDQqgiDSOmxsA(int P_0)
		{
			if (P_0 < 0 || P_0 >= JijZVkKtCuneUBOsMGIhmCybOFwL.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return JijZVkKtCuneUBOsMGIhmCybOFwL[P_0].iTZlVgtCqSxzFnKheClbpmeFvLmG;
		}

		public void JiBVnMvfyLQQTbghyZhvFHAObzLn(int P_0, bool P_1)
		{
			if (P_0 >= 0 && P_0 < JijZVkKtCuneUBOsMGIhmCybOFwL.Count)
			{
				JijZVkKtCuneUBOsMGIhmCybOFwL[P_0].HlITrbeQjYGRHGnEvesxxqhggxBF = P_1;
			}
		}
	}

	private class AUFgaVIXFecJKZbGrAAKkdQUMTXYA
	{
		public bool eKkqqyaRzvhtOKxNzJdEtZgrdQEo;

		private double kbfAYWPPbpywAeEakaBsKMJvBdBn;

		public float SHtQBbZniKBdCJZwlwvZKEBlYPmZ;

		public AUFgaVIXFecJKZbGrAAKkdQUMTXYA()
		{
		}

		public AUFgaVIXFecJKZbGrAAKkdQUMTXYA(float P_0)
		{
			SHtQBbZniKBdCJZwlwvZKEBlYPmZ = P_0;
		}

		public void hXtqHxopDjDyKrvrUxISDuLjcnbe()
		{
			eKkqqyaRzvhtOKxNzJdEtZgrdQEo = true;
			kbfAYWPPbpywAeEakaBsKMJvBdBn = (double)SHtQBbZniKBdCJZwlwvZKEBlYPmZ + ReInput.unscaledTime;
		}

		public void hXtqHxopDjDyKrvrUxISDuLjcnbe(float P_0)
		{
			eKkqqyaRzvhtOKxNzJdEtZgrdQEo = true;
			SHtQBbZniKBdCJZwlwvZKEBlYPmZ = P_0;
			kbfAYWPPbpywAeEakaBsKMJvBdBn = (double)SHtQBbZniKBdCJZwlwvZKEBlYPmZ + ReInput.unscaledTime;
		}

		public bool mPVLsAWcaTKfbIPORRXexWhffTDm()
		{
			if (!eKkqqyaRzvhtOKxNzJdEtZgrdQEo)
			{
				return false;
			}
			if (ReInput.unscaledTime >= kbfAYWPPbpywAeEakaBsKMJvBdBn)
			{
				eKkqqyaRzvhtOKxNzJdEtZgrdQEo = false;
				return true;
			}
			return false;
		}

		public void ZrbFhGEbWRbTzVxxQimUntnkwisKA()
		{
			eKkqqyaRzvhtOKxNzJdEtZgrdQEo = false;
			kbfAYWPPbpywAeEakaBsKMJvBdBn = 0.0;
		}

		public void ofFizofbRNphqjzDDdeRWooQzgqnb(float P_0)
		{
			SHtQBbZniKBdCJZwlwvZKEBlYPmZ = P_0;
		}

		public AUFgaVIXFecJKZbGrAAKkdQUMTXYA ixwHJGSvjAxYSHUjZMwwwHRtecyh()
		{
			return (AUFgaVIXFecJKZbGrAAKkdQUMTXYA)MemberwiseClone();
		}
	}

	public class hTqbXbSaCGbbKiuhgXToQLOuaIxwA : IDisposable
	{
		public readonly GBiViMpvNabbNsDeKnGXkPrzjOP GwYNpRkSmDCSiLMwAYWsVmzkHudR;

		public OrBWtmRDFzukyjjKebjqLenVxLmw JFsBApJkkZmclnxpAyfybvKeBHFl;

		private bool PzXedBlwGbkPNudHYEPuPBjzxgSm;

		private readonly ButtonLoopSet BLwbKZYwmvAMJrbfsaqeyusASoXy;

		private OrBWtmRDFzukyjjKebjqLenVxLmw dvyQIceIeDXGPzMlaAxwcvrKJfBYA;

		private bool nvzjPDsnBgZLdvQjnCfBBWGYvlwJ;

		private DualThreadLowLevelInputEventQueue XkzOZLmPxhEiRBLcedjSDqYlavfj;

		private readonly object ifhEwCkIVuTGOnfpubaTrgsqjFiQ;

		private RingBuffer<nSQKzuFgkuUgePhnvOTkeuGuQDqo> kZRTpVBbVIOGCZsrnZLoAZnsfgYU = new RingBuffer<nSQKzuFgkuUgePhnvOTkeuGuQDqo>(5);

		private RingBuffer<nSQKzuFgkuUgePhnvOTkeuGuQDqo> szEeHtaucEubVHqxOiskSHcplZqe = new RingBuffer<nSQKzuFgkuUgePhnvOTkeuGuQDqo>(5);

		private readonly object WwKWERrTQPZyErbLDTBcVRVyodJc = new object();

		private readonly object FvdqJkaumOSCLesxOncODynpRqSs = new object();

		private nSQKzuFgkuUgePhnvOTkeuGuQDqo zdwVqCEeYUVmDnVUFGbhfcTrkIBp;

		private double DfWxTdwgEobokckvyaIIfywdqTUCB;

		private bool NchdYNbKzqsssgcQJdenZuGqXgLo;

		public bool[] tuufDWVlHDfpSHKzDcghqCXCzcVtA => BLwbKZYwmvAMJrbfsaqeyusASoXy.Current.effectiveValue;

		public hTqbXbSaCGbbKiuhgXToQLOuaIxwA(int P_0, UpdateLoopSetting P_1)
		{
			GwYNpRkSmDCSiLMwAYWsVmzkHudR = new GBiViMpvNabbNsDeKnGXkPrzjOP((utTumibwKpLALZhsRUCMGlmpAfUS)P_0);
			BLwbKZYwmvAMJrbfsaqeyusASoXy = new ButtonLoopSet(P_1, 15);
			ifhEwCkIVuTGOnfpubaTrgsqjFiQ = new object();
			XkzOZLmPxhEiRBLcedjSDqYlavfj = new DualThreadLowLevelInputEventQueue((int)((float)BDwAsVxXXjygYaSUuZfLCpBQFMEJ.NombDecpdaRJnwimtgaeUHHBCkQO * 0.25f), 15, 6, 0);
		}

		public void cuFmCfHdMKWDocOGeIIRHXWTOpkEA()
		{
			BLwbKZYwmvAMJrbfsaqeyusASoXy.SetUpdateLoop(ReInput.currentUpdateLoop);
			uJLiZvQFhyrIgWCrXSIRDXLcJNwm(ref JFsBApJkkZmclnxpAyfybvKeBHFl);
		}

		public void UXsGlgdEPfSLHGRZMVqvnDmuNwHcA()
		{
			BqwFeZstKYsyawMiwqEsSpQwazrCA();
			BLwbKZYwmvAMJrbfsaqeyusASoXy.Current.ClearWasTrueThisFrame();
		}

		public void mYzmWMRBpIzVtmFinXeJttVSUihm()
		{
			woKyyTBPSBeTJzbsBHrQfbqfolrf();
			PzXedBlwGbkPNudHYEPuPBjzxgSm = true;
			nvzjPDsnBgZLdvQjnCfBBWGYvlwJ = GwYNpRkSmDCSiLMwAYWsVmzkHudR.LnsGeYXuVXqxrAqgebBxnSEmFRMR;
		}

		public void HXahjLaEqKAdOEjLEsQanbmcYlbLC()
		{
			PzXedBlwGbkPNudHYEPuPBjzxgSm = false;
			nvzjPDsnBgZLdvQjnCfBBWGYvlwJ = false;
			woKyyTBPSBeTJzbsBHrQfbqfolrf();
		}

		public bool jaYHvLybRgnKQpLxyEoqcJpRhhKV(mSpxQiVMUwFOidmosdvjPXPgCyyEb P_0)
		{
			return P_0 switch
			{
				mSpxQiVMUwFOidmosdvjPXPgCyyEb.Synchronous => nvzjPDsnBgZLdvQjnCfBBWGYvlwJ = GwYNpRkSmDCSiLMwAYWsVmzkHudR.LnsGeYXuVXqxrAqgebBxnSEmFRMR, 
				mSpxQiVMUwFOidmosdvjPXPgCyyEb.Asynchronous => nvzjPDsnBgZLdvQjnCfBBWGYvlwJ, 
				_ => throw new NotImplementedException(), 
			};
		}

		public void GhkbHidWDFgyXKWOQVKBFIymzDPjA(float P_0, int P_1)
		{
			switch (P_1)
			{
			case 0:
				zdwVqCEeYUVmDnVUFGbhfcTrkIBp.nZIhNrWMHpvRIUFXTYGPVEGNGUpO = (ushort)(MathTools.Clamp01(P_0) * 65535f);
				break;
			case 1:
				zdwVqCEeYUVmDnVUFGbhfcTrkIBp.sPQMTnJyYYFuCgqeuiLRnxXvWcYQ = (ushort)(MathTools.Clamp01(P_0) * 65535f);
				break;
			}
			UDNejtbAWvtDifTORtxWABSREDQT();
		}

		public void QVtcEbpATNFXldOWOuhvjtoVFlb()
		{
			zdwVqCEeYUVmDnVUFGbhfcTrkIBp.nZIhNrWMHpvRIUFXTYGPVEGNGUpO = 0;
			zdwVqCEeYUVmDnVUFGbhfcTrkIBp.sPQMTnJyYYFuCgqeuiLRnxXvWcYQ = 0;
			UDNejtbAWvtDifTORtxWABSREDQT();
		}

		public void wnYdYZpaeCwbldWKcWxEGKxjpfsR()
		{
			zdwVqCEeYUVmDnVUFGbhfcTrkIBp.nZIhNrWMHpvRIUFXTYGPVEGNGUpO = 0;
			zdwVqCEeYUVmDnVUFGbhfcTrkIBp.sPQMTnJyYYFuCgqeuiLRnxXvWcYQ = 0;
			lock (FvdqJkaumOSCLesxOncODynpRqSs)
			{
				lock (WwKWERrTQPZyErbLDTBcVRVyodJc)
				{
					kZRTpVBbVIOGCZsrnZLoAZnsfgYU.Clear();
					szEeHtaucEubVHqxOiskSHcplZqe.Clear();
					tkpMxMSpSpMrvmeRhbkfCvqphHYb(GwYNpRkSmDCSiLMwAYWsVmzkHudR, zdwVqCEeYUVmDnVUFGbhfcTrkIBp, ref DfWxTdwgEobokckvyaIIfywdqTUCB);
				}
			}
		}

		public void rPvWSsckcXMSIqpmLyltulScTOyK()
		{
			if (!PzXedBlwGbkPNudHYEPuPBjzxgSm || !nvzjPDsnBgZLdvQjnCfBBWGYvlwJ)
			{
				return;
			}
			aomOZkICjBSElBGTrxaITNTycEuQ aomOZkICjBSElBGTrxaITNTycEuQ2;
			double realTime;
			try
			{
				if (!GwYNpRkSmDCSiLMwAYWsVmzkHudR.UPojRbNwXIXHbCXBShjEWdfKtRZL(out aomOZkICjBSElBGTrxaITNTycEuQ2))
				{
					nvzjPDsnBgZLdvQjnCfBBWGYvlwJ = false;
					return;
				}
				realTime = ReInput.realTime;
			}
			catch
			{
				nvzjPDsnBgZLdvQjnCfBBWGYvlwJ = false;
				return;
			}
			lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
			{
				if (!hRsUxUkaUuteOvATKtsTnNmnygjd(aomOZkICjBSElBGTrxaITNTycEuQ2.FuMUcuZFgBHHmjCceRqXVtolPhOxA, dvyQIceIeDXGPzMlaAxwcvrKJfBYA))
				{
					using (DualThreadLowLevelInputEventQueue.INewEventWrapper newEventWrapper = XkzOZLmPxhEiRBLcedjSDqYlavfj.T_CreateEvent())
					{
						rXORuJZISahXMHvpeoPKjJuRuRTX(ref aomOZkICjBSElBGTrxaITNTycEuQ2.FuMUcuZFgBHHmjCceRqXVtolPhOxA, realTime, newEventWrapper.Event);
					}
					dvyQIceIeDXGPzMlaAxwcvrKJfBYA = aomOZkICjBSElBGTrxaITNTycEuQ2.FuMUcuZFgBHHmjCceRqXVtolPhOxA;
				}
			}
		}

		public void QeUGjCSckePKHjabnPIrWrhRgeME()
		{
			if (!PzXedBlwGbkPNudHYEPuPBjzxgSm || !nvzjPDsnBgZLdvQjnCfBBWGYvlwJ || ReInput.realTime < DfWxTdwgEobokckvyaIIfywdqTUCB + 0.009999999776482582)
			{
				return;
			}
			lock (FvdqJkaumOSCLesxOncODynpRqSs)
			{
				lock (WwKWERrTQPZyErbLDTBcVRVyodJc)
				{
					MiscTools.Swap(ref kZRTpVBbVIOGCZsrnZLoAZnsfgYU, ref szEeHtaucEubVHqxOiskSHcplZqe);
				}
				DNUgYnUTXqIaLQHNbxjjvOtcCmwU(szEeHtaucEubVHqxOiskSHcplZqe, GwYNpRkSmDCSiLMwAYWsVmzkHudR, ref DfWxTdwgEobokckvyaIIfywdqTUCB);
			}
		}

		private void BqwFeZstKYsyawMiwqEsSpQwazrCA()
		{
			kLUUiBzhjniLAjoLNQaFTEmbXSqSA();
		}

		private void kLUUiBzhjniLAjoLNQaFTEmbXSqSA()
		{
			if (!(ReInput.realTime < DfWxTdwgEobokckvyaIIfywdqTUCB + 1.5) && (!Mathf.Approximately((int)zdwVqCEeYUVmDnVUFGbhfcTrkIBp.nZIhNrWMHpvRIUFXTYGPVEGNGUpO, 0f) || !Mathf.Approximately((int)zdwVqCEeYUVmDnVUFGbhfcTrkIBp.sPQMTnJyYYFuCgqeuiLRnxXvWcYQ, 0f)))
			{
				UDNejtbAWvtDifTORtxWABSREDQT();
			}
		}

		private void UDNejtbAWvtDifTORtxWABSREDQT()
		{
			lock (WwKWERrTQPZyErbLDTBcVRVyodJc)
			{
				kZRTpVBbVIOGCZsrnZLoAZnsfgYU.Enqueue(zdwVqCEeYUVmDnVUFGbhfcTrkIBp);
			}
		}

		private static void DNUgYnUTXqIaLQHNbxjjvOtcCmwU(RingBuffer<nSQKzuFgkuUgePhnvOTkeuGuQDqo> P_0, GBiViMpvNabbNsDeKnGXkPrzjOP P_1, ref double P_2)
		{
			if (P_0.Count > 0)
			{
				tkpMxMSpSpMrvmeRhbkfCvqphHYb(P_1, P_0[P_0.Count - 1], ref P_2);
				P_0.Clear();
			}
		}

		private static void tkpMxMSpSpMrvmeRhbkfCvqphHYb(GBiViMpvNabbNsDeKnGXkPrzjOP P_0, nSQKzuFgkuUgePhnvOTkeuGuQDqo P_1, ref double P_2)
		{
			try
			{
				P_0.GhkbHidWDFgyXKWOQVKBFIymzDPjA(P_1);
			}
			catch
			{
			}
			P_2 = ReInput.realTime;
		}

		private void uJLiZvQFhyrIgWCrXSIRDXLcJNwm(ref OrBWtmRDFzukyjjKebjqLenVxLmw P_0)
		{
			while (XkzOZLmPxhEiRBLcedjSDqYlavfj.ProcessNewEvents())
			{
				CzgggviTdiIGPxIFwOxWFptwBlzDA(ref P_0, ref XkzOZLmPxhEiRBLcedjSDqYlavfj.currentEvent);
				for (int i = 0; i < 15; i++)
				{
					BLwbKZYwmvAMJrbfsaqeyusASoXy.SetValue(i, CTRqAkNbPTeaCEiQihtiarTqVmzG((int)P_0.kUBzXDgUpYEZoaPrTtuPPIDjpHZM, i), XkzOZLmPxhEiRBLcedjSDqYlavfj.currentEvent.GetTimestamp());
				}
			}
		}

		private void rXORuJZISahXMHvpeoPKjJuRuRTX(ref OrBWtmRDFzukyjjKebjqLenVxLmw P_0, double P_1, LowLevelInputEvent P_2)
		{
			P_2.SetTimestamp(P_1);
			int kUBzXDgUpYEZoaPrTtuPPIDjpHZM = (int)P_0.kUBzXDgUpYEZoaPrTtuPPIDjpHZM;
			P_2.SetButtonsBitMask((kUBzXDgUpYEZoaPrTtuPPIDjpHZM & 0x7FF) | ((kUBzXDgUpYEZoaPrTtuPPIDjpHZM & (kUBzXDgUpYEZoaPrTtuPPIDjpHZM & -4096)) >> 1), 0);
			P_2.SetAxisValue(0, FdAIjbgcfatFrghHnIipFWDBvhqyA(P_0.IXadPWOaEOjhaibAMcagSWUENZHKA));
			P_2.SetAxisValue(1, FdAIjbgcfatFrghHnIipFWDBvhqyA(P_0.yHAVaJMLRhAFVRsztnGDCDgsbKZH));
			P_2.SetAxisValue(2, FdAIjbgcfatFrghHnIipFWDBvhqyA(P_0.uYEFdqkHSnctFdAqxbiKLQEyuTShA));
			P_2.SetAxisValue(3, FdAIjbgcfatFrghHnIipFWDBvhqyA(P_0.CnnQoQkCuYWBpRyForWdVgZwIfnk));
			P_2.SetAxisValue(4, MsmttBzqGneydBeJePmdHRMgtZUDB(P_0.cQwIuxmdMVOscpetaAvxRxURGJShA));
			P_2.SetAxisValue(5, MsmttBzqGneydBeJePmdHRMgtZUDB(P_0.AnqVjrzlDVpSmyuIFnJNfBHUfebM));
		}

		private void CzgggviTdiIGPxIFwOxWFptwBlzDA(ref OrBWtmRDFzukyjjKebjqLenVxLmw P_0, ref LowLevelInputEvent P_1)
		{
			int buttonsBitMask = P_1.GetButtonsBitMask(0);
			P_0.kUBzXDgUpYEZoaPrTtuPPIDjpHZM = (JbZmbVjQNhHSYFYaMOrArpGBwExh)((buttonsBitMask & 0x7FF) | ((buttonsBitMask & (buttonsBitMask & -2048)) << 1));
			P_0.IXadPWOaEOjhaibAMcagSWUENZHKA = (short)(P_1.GetAxisValue(0) * 32768f);
			P_0.yHAVaJMLRhAFVRsztnGDCDgsbKZH = (short)(P_1.GetAxisValue(1) * 32768f);
			P_0.uYEFdqkHSnctFdAqxbiKLQEyuTShA = (short)(P_1.GetAxisValue(2) * 32768f);
			P_0.CnnQoQkCuYWBpRyForWdVgZwIfnk = (short)(P_1.GetAxisValue(3) * 32768f);
			P_0.cQwIuxmdMVOscpetaAvxRxURGJShA = (byte)(P_1.GetAxisValue(4) * 255f);
			P_0.AnqVjrzlDVpSmyuIFnJNfBHUfebM = (byte)(P_1.GetAxisValue(5) * 255f);
		}

		private static bool CTRqAkNbPTeaCEiQihtiarTqVmzG(int P_0, int P_1)
		{
			if (P_1 > 10)
			{
				P_1++;
			}
			return (P_0 & (1 << P_1)) != 0;
		}

		private void woKyyTBPSBeTJzbsBHrQfbqfolrf()
		{
			lock (ifhEwCkIVuTGOnfpubaTrgsqjFiQ)
			{
				JFsBApJkkZmclnxpAyfybvKeBHFl = default(OrBWtmRDFzukyjjKebjqLenVxLmw);
				dvyQIceIeDXGPzMlaAxwcvrKJfBYA = default(OrBWtmRDFzukyjjKebjqLenVxLmw);
				BLwbKZYwmvAMJrbfsaqeyusASoXy.Clear();
				XkzOZLmPxhEiRBLcedjSDqYlavfj.Clear();
			}
		}

		public void Dispose()
		{
			lDxnsjCDTQrmresvWgbliNUVruIc(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void zNJVymYugIbeeZuNgMrKxyYWbziV()
		{
			try
			{
				lDxnsjCDTQrmresvWgbliNUVruIc(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void lDxnsjCDTQrmresvWgbliNUVruIc(bool P_0)
		{
			if (!NchdYNbKzqsssgcQJdenZuGqXgLo)
			{
				if (P_0)
				{
					XkzOZLmPxhEiRBLcedjSDqYlavfj.Dispose();
				}
				NchdYNbKzqsssgcQJdenZuGqXgLo = true;
			}
		}

		public static float FdAIjbgcfatFrghHnIipFWDBvhqyA(int P_0)
		{
			if (P_0 == 0)
			{
				return 0f;
			}
			return MathTools.Clamp((float)MathTools.Abs(P_0) / 32768f * (float)MathTools.Sign(P_0), -1f, 1f);
		}

		public static float MsmttBzqGneydBeJePmdHRMgtZUDB(int P_0)
		{
			if (P_0 == 0)
			{
				return 0f;
			}
			return MathTools.Clamp((float)MathTools.Abs(P_0) / 255f * (float)MathTools.Sign(P_0), -1f, 1f);
		}

		private static bool hRsUxUkaUuteOvATKtsTnNmnygjd(OrBWtmRDFzukyjjKebjqLenVxLmw P_0, OrBWtmRDFzukyjjKebjqLenVxLmw P_1)
		{
			if (P_0.kUBzXDgUpYEZoaPrTtuPPIDjpHZM == P_1.kUBzXDgUpYEZoaPrTtuPPIDjpHZM && P_0.cQwIuxmdMVOscpetaAvxRxURGJShA == P_1.cQwIuxmdMVOscpetaAvxRxURGJShA && P_0.AnqVjrzlDVpSmyuIFnJNfBHUfebM == P_1.AnqVjrzlDVpSmyuIFnJNfBHUfebM && P_0.IXadPWOaEOjhaibAMcagSWUENZHKA == P_1.IXadPWOaEOjhaibAMcagSWUENZHKA && P_0.yHAVaJMLRhAFVRsztnGDCDgsbKZH == P_1.yHAVaJMLRhAFVRsztnGDCDgsbKZH && P_0.uYEFdqkHSnctFdAqxbiKLQEyuTShA == P_1.uYEFdqkHSnctFdAqxbiKLQEyuTShA)
			{
				return P_0.CnnQoQkCuYWBpRyForWdVgZwIfnk == P_1.CnnQoQkCuYWBpRyForWdVgZwIfnk;
			}
			return false;
		}
	}

	public enum mSpxQiVMUwFOidmosdvjPXPgCyyEb
	{
		Synchronous = 0,
		Asynchronous = 1
	}

	public const int slGnPvPfojUVbvtFMjCTFNpcWqjA = 4;

	public const int AJmmyhTTXdMExghCMXlCxEmKbCCG = 32768;

	public const int RuVANolGwRFTeyZyGKmxeYCmHjjj = -32768;

	public const int PzJAEBREbweSEKxkeltjAMPAintH = 255;

	public const int kdZVOXzncirkGanLYtOlyvVaSeqG = 0;

	public const int xRTDHSmIuVhAYEQwtDOSkOLqLllcb = 18;

	public const int KpULbHHmWhbvkYjibzggxiHhIVEX = 14;

	public const int zhGqKzPzqNaXylIwvmZnihQhepps = 6;

	public const int GUugNzpCfSSPGJyeGTQtftRKnexl = 15;

	private OcxJGVolhfPasVdbifhMjzXBDVEd[] XkOuZuiKvHqxAYCCOPUFRgSBBiTm;

	private bool uNuLbUTjWaLXWEBqXujaqPGfucUS;

	private AUFgaVIXFecJKZbGrAAKkdQUMTXYA SMIPjcKRCMfxEpLbCwcHfgZplLsc;

	private VXItTSauYxYTlYaABLtKshlmfhFh GesWpteoztiemxguwblegBYdBOkL;

	private zCiKSorjoVkhIfNAHGoZOwuVeIyT<bool> YQkPyDFsizvDdjeUcbennkOoSSHC;

	private bool[] MeWTWfDBpvwGnEQCJSMbPCKhkzoC;

	private bool[] xPwtrnQysKhisggAlpHiMmMEtQbt;

	private bool GOnGCgYNNmgWeUvzYeLHfadqviaiA;

	private readonly bool oVzuCxDWvOSuDlsbCekJCvomNgVVA;

	private readonly UpdateLoopSetting uwGvIUXeAEcvaOQoLrEevKuRiPIO;

	private UpdateLoopType ZjNnWYnoQbZaWOvhpLmwjekoCjdj;

	private UpdateLoopType phxTsbdtCMdCfuCMgbKgeVOYbQqb;

	private Action<int, ControllerDataUpdater> kFtEhCDMsBpCscbniaVUseitMyFJ;

	private bool jIdjAGfFZTnPrgrFHGDuKrPCTfDh;

	private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> XDfJqiwLYIwEbgMhAkMbksxTquAQ;

	private Func<int> WHEWdNESHgukupIXVEuPORSWLLeJ;

	private static Guid[] ADYeWAqtOHEjvhCKBKlSVkUqOUXb;

	private static string[] edCaGLCOAVwIctAXypnGChGDiGyJb;

	private static string[] yRCkGkZXEEFirCJGGENuDUfKjYII;

	[CustomObfuscation(rename = false)]
	public override int deviceCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].JuDoJMMSXVroARSjTyeCKpDYkYCj)
				{
					num++;
				}
			}
			return num;
		}
	}

	[CustomObfuscation(rename = false)]
	public override PlatformInputManager primaryInputManager => this;

	[CustomObfuscation(rename = false)]
	public override IInputSource inputSource => null;

	[CustomObfuscation(rename = false)]
	public override InputSource inputSourceType => InputSource.XInput;

	public QhRUnWbULvmdFTzNHeLFXfWeuhyi(bool P_0, UpdateLoopSetting P_1, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_2, Func<int> P_3)
	{
		oVzuCxDWvOSuDlsbCekJCvomNgVVA = P_0;
		uwGvIUXeAEcvaOQoLrEevKuRiPIO = P_1;
		jIdjAGfFZTnPrgrFHGDuKrPCTfDh = true;
		try
		{
			if (!nidrBTyyTsEkugfveLPskTbfMKqg.tVvNlkncUOQnSBnilDhSFTlIVGq(out var nnzNxjXscSAtMAablRKHBPvKsOhp2, out var text, out var _))
			{
				throw new Exception("XInput is not available.");
			}
			if (nnzNxjXscSAtMAablRKHBPvKsOhp2 < nnzNxjXscSAtMAablRKHBPvKsOhp.XINPUT_1_3)
			{
				Rewired.Logger.LogWarning("The version of XInput (" + text + ") detected on your system is out of date. Please update to the latest version of XInput. Input will still function, but all features may not be available. See the documentation for required dependencies.");
			}
			else
			{
				_ = 4;
			}
			XDfJqiwLYIwEbgMhAkMbksxTquAQ = P_2;
			WHEWdNESHgukupIXVEuPORSWLLeJ = P_3;
			GOnGCgYNNmgWeUvzYeLHfadqviaiA = UnityTools.platform == Platform.WindowsAppStore;
			using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
			{
				List<UpdateLoopType> list = tList.list;
				EnumConverter.ToUpdateLoopTypes(uwGvIUXeAEcvaOQoLrEevKuRiPIO, list);
				int num2 = 0;
				if (num2 < list.Count)
				{
					phxTsbdtCMdCfuCMgbKgeVOYbQqb = list[num2];
				}
			}
			YQkPyDFsizvDdjeUcbennkOoSSHC = new zCiKSorjoVkhIfNAHGoZOwuVeIyT<bool>(true, CUkyANOuFqmdjVxOuUXHFiMmRdqm);
			MeWTWfDBpvwGnEQCJSMbPCKhkzoC = new bool[4];
			xPwtrnQysKhisggAlpHiMmMEtQbt = new bool[4];
			kFtEhCDMsBpCscbniaVUseitMyFJ = UpdateControllerData;
			if (GOnGCgYNNmgWeUvzYeLHfadqviaiA)
			{
				RRVCliBJcwdIdQkyCgYesflxyhJNA();
			}
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
		if (jIdjAGfFZTnPrgrFHGDuKrPCTfDh)
		{
			SMIPjcKRCMfxEpLbCwcHfgZplLsc = new AUFgaVIXFecJKZbGrAAKkdQUMTXYA(1f);
		}
		GesWpteoztiemxguwblegBYdBOkL = new VXItTSauYxYTlYaABLtKshlmfhFh();
		if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm == null)
		{
			XkOuZuiKvHqxAYCCOPUFRgSBBiTm = new OcxJGVolhfPasVdbifhMjzXBDVEd[4];
			for (int i = 0; i < 4; i++)
			{
				hTqbXbSaCGbbKiuhgXToQLOuaIxwA hTqbXbSaCGbbKiuhgXToQLOuaIxwA2 = new hTqbXbSaCGbbKiuhgXToQLOuaIxwA(i, uwGvIUXeAEcvaOQoLrEevKuRiPIO);
				BDwAsVxXXjygYaSUuZfLCpBQFMEJ.LrMvxKgLebxAruEpHRhBkzmuwWOF.ThreadUpdateEvent += hTqbXbSaCGbbKiuhgXToQLOuaIxwA2.rPvWSsckcXMSIqpmLyltulScTOyK;
				BDwAsVxXXjygYaSUuZfLCpBQFMEJ.yTVbPyUIUIUOrvAjtqUooaqsSzuO.ThreadUpdateEvent += hTqbXbSaCGbbKiuhgXToQLOuaIxwA2.QeUGjCSckePKHjabnPIrWrhRgeME;
				XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i] = new OcxJGVolhfPasVdbifhMjzXBDVEd(i, GOnGCgYNNmgWeUvzYeLHfadqviaiA, hTqbXbSaCGbbKiuhgXToQLOuaIxwA2, XDfJqiwLYIwEbgMhAkMbksxTquAQ, SystemDeviceDisconnected);
			}
		}
		KnESqFASdNHqdfAlstilncmlgsJDA(true);
		Update(UpdateLoopType.Update);
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType currentUpdateLoop)
	{
		ZjNnWYnoQbZaWOvhpLmwjekoCjdj = currentUpdateLoop;
		RsZcaytHGpGWwGpYNIazTcVxItHT();
		for (int i = 0; i < 4; i++)
		{
			if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i] != null && XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].JuDoJMMSXVroARSjTyeCKpDYkYCj)
			{
				XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].Update();
			}
		}
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (YQkPyDFsizvDdjeUcbennkOoSSHC != null)
		{
			YQkPyDFsizvDdjeUcbennkOoSSHC.lDxnsjCDTQrmresvWgbliNUVruIc();
		}
		if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm != null)
		{
			for (int i = 0; i < 4; i++)
			{
				if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i] != null)
				{
					if (BDwAsVxXXjygYaSUuZfLCpBQFMEJ.LrMvxKgLebxAruEpHRhBkzmuwWOF != null)
					{
						BDwAsVxXXjygYaSUuZfLCpBQFMEJ.LrMvxKgLebxAruEpHRhBkzmuwWOF.ThreadUpdateEvent -= XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].MMTtbRDiEhfNsofvfEMKtHrGMMsL.rPvWSsckcXMSIqpmLyltulScTOyK;
					}
					if (BDwAsVxXXjygYaSUuZfLCpBQFMEJ.yTVbPyUIUIUOrvAjtqUooaqsSzuO != null)
					{
						BDwAsVxXXjygYaSUuZfLCpBQFMEJ.yTVbPyUIUIUOrvAjtqUooaqsSzuO.ThreadUpdateEvent -= XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].MMTtbRDiEhfNsofvfEMKtHrGMMsL.QeUGjCSckePKHjabnPIrWrhRgeME;
					}
					XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].Dispose();
				}
			}
		}
		nidrBTyyTsEkugfveLPskTbfMKqg.PXrxyMprYZekwShJCRsXWAoDYdQr();
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return kFtEhCDMsBpCscbniaVUseitMyFJ;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int assignedControllerId, ControllerDataUpdater data)
	{
		XkOuZuiKvHqxAYCCOPUFRgSBBiTm[assignedControllerId].FillData(data);
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		KnESqFASdNHqdfAlstilncmlgsJDA(true);
		OywUpueSCGaIqBGwoWxnvklAcsqy();
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		KnESqFASdNHqdfAlstilncmlgsJDA(true);
		OywUpueSCGaIqBGwoWxnvklAcsqy();
		if (_SystemDeviceDisconnectedEvent != null)
		{
			_SystemDeviceDisconnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
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

	private bool ycJcObkTXvDrPthRYwFSqcrCkYxab()
	{
		if (ZjNnWYnoQbZaWOvhpLmwjekoCjdj != phxTsbdtCMdCfuCMgbKgeVOYbQqb)
		{
			return false;
		}
		bool num = SMIPjcKRCMfxEpLbCwcHfgZplLsc.mPVLsAWcaTKfbIPORRXexWhffTDm();
		if (num)
		{
			KnESqFASdNHqdfAlstilncmlgsJDA(true);
		}
		return num;
	}

	private void KnESqFASdNHqdfAlstilncmlgsJDA(bool P_0)
	{
		uNuLbUTjWaLXWEBqXujaqPGfucUS = P_0;
		if (jIdjAGfFZTnPrgrFHGDuKrPCTfDh)
		{
			SMIPjcKRCMfxEpLbCwcHfgZplLsc.hXtqHxopDjDyKrvrUxISDuLjcnbe();
		}
	}

	private void OywUpueSCGaIqBGwoWxnvklAcsqy()
	{
		if (YQkPyDFsizvDdjeUcbennkOoSSHC != null)
		{
			YQkPyDFsizvDdjeUcbennkOoSSHC.ZrbFhGEbWRbTzVxxQimUntnkwisKA();
		}
	}

	private void RRVCliBJcwdIdQkyCgYesflxyhJNA()
	{
		_ = new GBiViMpvNabbNsDeKnGXkPrzjOP().LnsGeYXuVXqxrAqgebBxnSEmFRMR;
	}

	private void RsZcaytHGpGWwGpYNIazTcVxItHT()
	{
		bool flag = false;
		if (jIdjAGfFZTnPrgrFHGDuKrPCTfDh)
		{
			flag = ycJcObkTXvDrPthRYwFSqcrCkYxab();
		}
		if (!flag && uNuLbUTjWaLXWEBqXujaqPGfucUS)
		{
			mOwoiTszqUSvvufezLHkDmyvQZyn(OThhbPDYAeefCPgboVjnGYlZfAHF());
			KnESqFASdNHqdfAlstilncmlgsJDA(false);
			OywUpueSCGaIqBGwoWxnvklAcsqy();
			return;
		}
		if (uNuLbUTjWaLXWEBqXujaqPGfucUS)
		{
			pijIxDpHQrsOUzmTMAFkSLhIBLtC();
		}
		if (YQkPyDFsizvDdjeUcbennkOoSSHC.ZkLDDCubwAduGHcHaTbJRgqUXFzZ && YQkPyDFsizvDdjeUcbennkOoSSHC.JXQYCqxIFkDGZeLuFZlXEfMklrZtA())
		{
			mgNyFCYOmaDhuDlmPtVuuEkxCLGY();
		}
	}

	private void pijIxDpHQrsOUzmTMAFkSLhIBLtC()
	{
		uNuLbUTjWaLXWEBqXujaqPGfucUS = false;
		if (!YQkPyDFsizvDdjeUcbennkOoSSHC.ZkLDDCubwAduGHcHaTbJRgqUXFzZ)
		{
			YQkPyDFsizvDdjeUcbennkOoSSHC.uRxHPttoThKrBNCurCuvwPhMcGUfA();
		}
	}

	private void mgNyFCYOmaDhuDlmPtVuuEkxCLGY()
	{
		lock (MeWTWfDBpvwGnEQCJSMbPCKhkzoC)
		{
			Array.Copy(MeWTWfDBpvwGnEQCJSMbPCKhkzoC, xPwtrnQysKhisggAlpHiMmMEtQbt, 4);
		}
		mOwoiTszqUSvvufezLHkDmyvQZyn(xPwtrnQysKhisggAlpHiMmMEtQbt);
	}

	private bool CUkyANOuFqmdjVxOuUXHFiMmRdqm()
	{
		lock (MeWTWfDBpvwGnEQCJSMbPCKhkzoC)
		{
			for (int i = 0; i < 4; i++)
			{
				if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i] != null)
				{
					MeWTWfDBpvwGnEQCJSMbPCKhkzoC[i] = XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].jaYHvLybRgnKQpLxyEoqcJpRhhKV(mSpxQiVMUwFOidmosdvjPXPgCyyEb.Synchronous);
				}
			}
		}
		return true;
	}

	private bool[] OThhbPDYAeefCPgboVjnGYlZfAHF()
	{
		for (int i = 0; i < 4; i++)
		{
			xPwtrnQysKhisggAlpHiMmMEtQbt[i] = XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].jaYHvLybRgnKQpLxyEoqcJpRhhKV(mSpxQiVMUwFOidmosdvjPXPgCyyEb.Synchronous);
		}
		return xPwtrnQysKhisggAlpHiMmMEtQbt;
	}

	private void mOwoiTszqUSvvufezLHkDmyvQZyn(bool[] P_0)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i] != null && XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].EtOciWRBDomBqcEQItHEcSEOtAHF)
			{
				bool flag = P_0[i];
				XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i].iYtdkedLLoIExpmcNsjdFkVkGKMac(flag);
				if (!flag)
				{
					ZnrdBivVmpBqYzBrBialzGrnSBjC(XkOuZuiKvHqxAYCCOPUFRgSBBiTm[i], false);
				}
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm[j] != null && !XkOuZuiKvHqxAYCCOPUFRgSBBiTm[j].EtOciWRBDomBqcEQItHEcSEOtAHF)
			{
				bool flag2 = P_0[j];
				XkOuZuiKvHqxAYCCOPUFRgSBBiTm[j].iYtdkedLLoIExpmcNsjdFkVkGKMac(flag2);
				if (flag2 && !ZnrdBivVmpBqYzBrBialzGrnSBjC(XkOuZuiKvHqxAYCCOPUFRgSBBiTm[j], true))
				{
					num |= ((j == 0) ? 1 : (1 << j));
				}
			}
		}
		for (int k = 0; k < 4; k++)
		{
			if (XkOuZuiKvHqxAYCCOPUFRgSBBiTm[k] != null)
			{
				int num2 = ((k == 0) ? 1 : (1 << k));
				if ((num & num2) != 1 << k)
				{
					XkOuZuiKvHqxAYCCOPUFRgSBBiTm[k].hzVXylspAStQLNzufXVziRmulsUf(P_0[k]);
				}
			}
		}
	}

	private bool ZnrdBivVmpBqYzBrBialzGrnSBjC(OcxJGVolhfPasVdbifhMjzXBDVEd P_0, bool P_1)
	{
		if (P_1)
		{
			P_0.SEZecPHwRFkiwnwPqWOERGndlgZB();
			if (!P_0.eKcWHeiuDjBZLxCtvCsgstBOqXxe)
			{
				return false;
			}
			int num = GesWpteoztiemxguwblegBYdBOkL.VAYqhqAqTZNUiYZXCjbwXeXSnnMB(P_0.WmGUXMLdKObUkBomplWQnKJsvVVBA, false);
			if (num >= 0)
			{
				P_0.rewiredId = GesWpteoztiemxguwblegBYdBOkL.qIPaWKBCnNRJFDlWXDQqgiDSOmxsA(num);
				GesWpteoztiemxguwblegBYdBOkL.mPVLsAWcaTKfbIPORRXexWhffTDm(num, P_0, true);
			}
			else
			{
				P_0.rewiredId = WHEWdNESHgukupIXVEuPORSWLLeJ();
				GesWpteoztiemxguwblegBYdBOkL.RBQmkHWZJfxhkxpcpdAYTuhueORX(P_0, true);
			}
			if (_UpdateControllerInfoEvent != null)
			{
				_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(P_0));
			}
			BridgedController obj = P_0.ToBridgedController();
			if (_DeviceConnectedEvent != null)
			{
				_DeviceConnectedEvent(obj);
			}
		}
		else
		{
			int num2 = GesWpteoztiemxguwblegBYdBOkL.uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0.rewiredId, P_0.WmGUXMLdKObUkBomplWQnKJsvVVBA, true);
			if (num2 >= 0)
			{
				GesWpteoztiemxguwblegBYdBOkL.JiBVnMvfyLQQTbghyZhvFHAObzLn(num2, false);
			}
			ControllerDisconnectedEventArgs obj2 = P_0.ToControllerDisconnectedEventArgs();
			P_0.zpkArhFROmzNjuKNleYXtlbiCFnSA();
			if (_DeviceDisconnectedEvent != null)
			{
				_DeviceDisconnectedEvent(obj2);
			}
		}
		return true;
	}

	static QhRUnWbULvmdFTzNHeLFXfWeuhyi()
	{
		ADYeWAqtOHEjvhCKBKlSVkUqOUXb = new Guid[2]
		{
			new Guid("72100955-0000-0000-0000-504944564944"),
			new Guid("02e0045e-0000-0000-0000-504944564944")
		};
		edCaGLCOAVwIctAXypnGChGDiGyJb = new string[1] { "Xbox Bluetooth Gamepad" };
		yRCkGkZXEEFirCJGGENuDUfKjYII = new string[1] { "Xbox Wireless Controller.*" };
	}

	public static bool noJvcChzupkIRqHKqAWEkKaBEUbV(string P_0, string P_1, string P_2, Guid P_3)
	{
		if (ArrayTools.Contains(ADYeWAqtOHEjvhCKBKlSVkUqOUXb, P_3))
		{
			return true;
		}
		if (!string.IsNullOrEmpty(P_1))
		{
			for (int i = 0; i < edCaGLCOAVwIctAXypnGChGDiGyJb.Length; i++)
			{
				if (P_1.Equals(edCaGLCOAVwIctAXypnGChGDiGyJb[i], StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		if (!string.IsNullOrEmpty(P_2))
		{
			for (int j = 0; j < yRCkGkZXEEFirCJGGENuDUfKjYII.Length; j++)
			{
				if (Regex.IsMatch(P_2, yRCkGkZXEEFirCJGGENuDUfKjYII[j], RegexOptions.IgnoreCase))
				{
					return true;
				}
			}
		}
		P_0 = P_0.ToLower();
		int num = P_0.IndexOf("vid_");
		if (num < 0)
		{
			return false;
		}
		if (P_0.IndexOf("ig_") < num)
		{
			return false;
		}
		return true;
	}
}
