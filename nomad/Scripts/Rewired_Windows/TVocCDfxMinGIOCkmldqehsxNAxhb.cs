using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Rewired;
using Rewired.Config;
using Rewired.Data.Mapping;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Libraries.SharpDX.XInput;
using Rewired.Platforms;
using Rewired.Platforms.Windows.XInput;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;
using Rewired.Utils.Classes.Utility;
using UnityEngine;

internal class TVocCDfxMinGIOCkmldqehsxNAxhb : PlatformInputManager, DDWjNWYAVnuAprLJBexwaxtfkBnQA
{
	private class OaJsZnjraUCumAdijDhlcsUkRksfA : IInputManagerJoystick, IInputManagerJoystickPublic, ITryGetLocalizedName, IDisposable
	{
		private bool DFtgHmHdITsouofxhguoAZCYvBFvA;

		private int yEndDaadRqklibWfDZrdDtSjLBGA;

		private readonly int ZrDnFabqmZgCPDnZBAQFILJJTdJiA;

		public Guid GwlpqllBMXAZUErWdEfJpDJhWjQIA;

		public string BDFQFYUGoQtzMrUbwWemSUaOXdwe;

		public string NFagRJPhEOXjZwcklrhwGMEoGJcJA;

		public Guid PgqmvHXhZidFrCpweTkWHozIXFfqA;

		public Rewired.Libraries.SharpDX.XInput.DeviceType SvtdLZqPXXSdqPuXRNrwJphgDrtv;

		public XInputDeviceSubType jNUntUhpzorzgblufNVoYfcNIMOJ;

		public bool EPWHtrOuQZheiwNNugiHFFPcWBdS;

		public bool NeeiagHJpsWrFdDNFiyViWSqoIQvA;

		public bool CDztTwISAVOIpUfFAJqQRbIvKIzm;

		public bool OUzEDZhQskfBsIiagmripWzQfMRXb;

		private int pKLvVKcbYYFgMEIsoBKvnsYlBhkdb;

		private int cduGMXOmAyxVkxibvFForpnXsrwg;

		private int HfgfBIkeedPwchDmjHeXqQSElKov;

		private int AicKLnVGnEtDWLwkoNYXaImxUpUe;

		private readonly float[] GIitSxYmCskMdUpsNwsTEtRUNqQH;

		private readonly bool[] CNItCCiCwdAxpfiThmcsvsAxQDUd;

		private HardwareJoystickMap_InputManager wkFTWrWmhHZmQfDdODUPApCRbQWl;

		public readonly AlKsZxctHXLfYYkzfFYPZTTBpoGg AatjHeezYVaTfooOwzmbbrxvozlL;

		private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> rkHvTeFqcqaxgapqnshqNpCUFWYw;

		private Action IiydRKCRYVFmvXAVIMAFlptMNMsxA;

		private readonly LocalizedString XkFPlmFLDlkLEJdopNvdbBcWKaQe;

		private bool arRqZnuAeaWaVMFcYxpehUzOUDpD;

		private bool ekOgYSpwrSGBRMfalvDqdcYKbILu;

		private bool ShuFzXnUcVhRggCRChONECBBddlKA;

		public string UxpADUxFaMuqtygtRRObxYRjQnBb
		{
			get
			{
				string text = gAeqnrghDxYFbUpWJXdXxweLbPpK;
				if (text == string.Empty)
				{
					return string.Empty;
				}
				int zrDnFabqmZgCPDnZBAQFILJJTdJiA = ZrDnFabqmZgCPDnZBAQFILJJTdJiA;
				return text + " " + zrDnFabqmZgCPDnZBAQFILJJTdJiA;
			}
		}

		public string gAeqnrghDxYFbUpWJXdXxweLbPpK
		{
			get
			{
				if (!dkwdOIYrxMlAdLwfZsIEAYqwkWWn)
				{
					return string.Empty;
				}
				return jNUntUhpzorzgblufNVoYfcNIMOJ.ToString();
			}
		}

		public bool dkwdOIYrxMlAdLwfZsIEAYqwkWWn
		{
			get
			{
				if (AatjHeezYVaTfooOwzmbbrxvozlL == null || !OUzEDZhQskfBsIiagmripWzQfMRXb)
				{
					return false;
				}
				if (arRqZnuAeaWaVMFcYxpehUzOUDpD && !AcrMgDfigfEJLoOQnvIYrwQjrhai(VtLabcPRoBZcpVPBAzPxGpNfqXdn.Asynchronous))
				{
					dtTtCSwAmbAYpGUPJpTzpbEbhzNvA();
				}
				return arRqZnuAeaWaVMFcYxpehUzOUDpD;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.rewiredId
		{
			get
			{
				return yEndDaadRqklibWfDZrdDtSjLBGA;
			}
			set
			{
				yEndDaadRqklibWfDZrdDtSjLBGA = value;
			}
		}

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.inputManagerId => ZrDnFabqmZgCPDnZBAQFILJJTdJiA;

		[CustomObfuscation(rename = false)]
		string IInputManagerJoystickPublic.name => NFagRJPhEOXjZwcklrhwGMEoGJcJA;

		[CustomObfuscation(rename = false)]
		long? IInputManagerJoystickPublic.systemId => ZrDnFabqmZgCPDnZBAQFILJJTdJiA;

		[CustomObfuscation(rename = false)]
		int IInputManagerJoystickPublic.unityId => 0;

		[CustomObfuscation(rename = false)]
		Controller.Extension IInputManagerJoystickPublic.extension
		{
			get
			{
				if (AatjHeezYVaTfooOwzmbbrxvozlL == null)
				{
					return null;
				}
				return AatjHeezYVaTfooOwzmbbrxvozlL.FAluqucGyxUcbXoVzzTAHBGfWTRk;
			}
		}

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.instanceGuid => PgqmvHXhZidFrCpweTkWHozIXFfqA;

		[CustomObfuscation(rename = false)]
		Guid IInputManagerJoystickPublic.persistentGuid => Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;

		[CustomObfuscation(rename = false)]
		public void SetVibration(float amount, int motorIndex)
		{
			AatjHeezYVaTfooOwzmbbrxvozlL.zbGFxNQXqiwUDtmxrZUgVTbMEIpq(amount, motorIndex);
		}

		void IInputManagerJoystickPublic.SetVibration(float amount, int motorIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(amount, motorIndex);
		}

		[CustomObfuscation(rename = false)]
		public void StopVibration()
		{
			AatjHeezYVaTfooOwzmbbrxvozlL.nunYcxCqZRUKJXQdixrxNWguwYp();
		}

		void IInputManagerJoystickPublic.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		bool ITryGetLocalizedName.TryGetLocalizedName(out string value)
		{
			if ((LocalizationManager.GetAndUpdateLocalizedString(XkFPlmFLDlkLEJdopNvdbBcWKaQe, wkFTWrWmhHZmQfDdODUPApCRbQWl.deviceLocalizationInfo.parentKeys, "controller", BDFQFYUGoQtzMrUbwWemSUaOXdwe, out value) & LocalizationManager.GetAndUpdateLocalizedStringResultFlags.Changed) != LocalizationManager.GetAndUpdateLocalizedStringResultFlags.None)
			{
				value = $"{value} {(ZrDnFabqmZgCPDnZBAQFILJJTdJiA + 1).ToString()}";
				XkFPlmFLDlkLEJdopNvdbBcWKaQe.cachedValue = value;
			}
			return true;
		}

		public OaJsZnjraUCumAdijDhlcsUkRksfA(int P_0, bool P_1, AlKsZxctHXLfYYkzfFYPZTTBpoGg P_2, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_3, Action P_4)
		{
			AatjHeezYVaTfooOwzmbbrxvozlL = P_2;
			DFtgHmHdITsouofxhguoAZCYvBFvA = P_1;
			ZrDnFabqmZgCPDnZBAQFILJJTdJiA = P_0;
			rkHvTeFqcqaxgapqnshqNpCUFWYw = P_3;
			IiydRKCRYVFmvXAVIMAFlptMNMsxA = P_4;
			yEndDaadRqklibWfDZrdDtSjLBGA = -1;
			pKLvVKcbYYFgMEIsoBKvnsYlBhkdb = 6;
			cduGMXOmAyxVkxibvFForpnXsrwg = 15;
			HfgfBIkeedPwchDmjHeXqQSElKov = pKLvVKcbYYFgMEIsoBKvnsYlBhkdb;
			AicKLnVGnEtDWLwkoNYXaImxUpUe = cduGMXOmAyxVkxibvFForpnXsrwg;
			GIitSxYmCskMdUpsNwsTEtRUNqQH = new float[pKLvVKcbYYFgMEIsoBKvnsYlBhkdb];
			CNItCCiCwdAxpfiThmcsvsAxQDUd = new bool[cduGMXOmAyxVkxibvFForpnXsrwg];
			XkFPlmFLDlkLEJdopNvdbBcWKaQe = new LocalizedString();
			PnFVZbrCbPcduIhDvSzaMJouiwGWA();
		}

		[CustomObfuscation(rename = false)]
		public void Update()
		{
			AatjHeezYVaTfooOwzmbbrxvozlL.aFcCqTrdZZzMfheQsGnluLUnRTtR();
			bool[] array = AatjHeezYVaTfooOwzmbbrxvozlL.GkBrqmDIOtSopmNIJcseejXuDVUwA;
			BPUAqyTuMWvwNdzKtGtqFvjSHpiH(array, ref AatjHeezYVaTfooOwzmbbrxvozlL.CDrHJBtgGEETtenhNKdaHTUkxByZB);
			bWbcdZHPLpJbQtCpVkhKLguWWopQA(array, ref AatjHeezYVaTfooOwzmbbrxvozlL.CDrHJBtgGEETtenhNKdaHTUkxByZB);
			AatjHeezYVaTfooOwzmbbrxvozlL.LJxFAUWyMevDvIHjKEbHcqHQZFIGA();
		}

		void IInputManagerJoystick.Update()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Update
			this.Update();
		}

		public void MUFyBzyHeAgZbNFrdfExJIBTaEGCb(bool P_0)
		{
			if (AatjHeezYVaTfooOwzmbbrxvozlL != null)
			{
				CDztTwISAVOIpUfFAJqQRbIvKIzm = P_0;
			}
		}

		public bool AcrMgDfigfEJLoOQnvIYrwQjrhai(VtLabcPRoBZcpVPBAzPxGpNfqXdn P_0)
		{
			SLWYJjXDkczKqzAJNBHRlobnCnEj(QdTmCIHPCRbxRDqUNYmZHMeNQcCw(P_0));
			return arRqZnuAeaWaVMFcYxpehUzOUDpD;
		}

		public bool QdTmCIHPCRbxRDqUNYmZHMeNQcCw(VtLabcPRoBZcpVPBAzPxGpNfqXdn P_0)
		{
			if (AatjHeezYVaTfooOwzmbbrxvozlL == null)
			{
				return false;
			}
			return AatjHeezYVaTfooOwzmbbrxvozlL.RCPKGkOzGIPiahUUgUZIBGjglooP(P_0);
		}

		public void SLWYJjXDkczKqzAJNBHRlobnCnEj(bool P_0)
		{
			arRqZnuAeaWaVMFcYxpehUzOUDpD = P_0;
		}

		public void JiWviFcDkPiYGTgxqGbtjZcHdfnu()
		{
			if (!OUzEDZhQskfBsIiagmripWzQfMRXb || kVszEwPAcgDsnfgqFuvjDuVnEqGu())
			{
				PnFVZbrCbPcduIhDvSzaMJouiwGWA();
			}
			if (OUzEDZhQskfBsIiagmripWzQfMRXb && arRqZnuAeaWaVMFcYxpehUzOUDpD)
			{
				AatjHeezYVaTfooOwzmbbrxvozlL.uiUygUzvMkjJbsOaWCoiHvfgocNy();
			}
		}

		public void AyaRMUdEcIpVwYBpwuWAnxukPOpG()
		{
			yEndDaadRqklibWfDZrdDtSjLBGA = -1;
			OUzEDZhQskfBsIiagmripWzQfMRXb = false;
			AatjHeezYVaTfooOwzmbbrxvozlL.QpdjlkJkBQSWAqWmHaNSMVvEJUQi();
			Array.Clear(GIitSxYmCskMdUpsNwsTEtRUNqQH, 0, GIitSxYmCskMdUpsNwsTEtRUNqQH.Length);
			Array.Clear(CNItCCiCwdAxpfiThmcsvsAxQDUd, 0, CNItCCiCwdAxpfiThmcsvsAxQDUd.Length);
		}

		[CustomObfuscation(rename = false)]
		public void FillData(ControllerDataUpdater dataUpdater)
		{
			if (pKLvVKcbYYFgMEIsoBKvnsYlBhkdb != dataUpdater.axisCount || cduGMXOmAyxVkxibvFForpnXsrwg != dataUpdater.buttonCount)
			{
				throw new Exception("This controller signature does not match the data object!");
			}
			for (int i = 0; i < pKLvVKcbYYFgMEIsoBKvnsYlBhkdb; i++)
			{
				dataUpdater.axisValues[i] = GIitSxYmCskMdUpsNwsTEtRUNqQH[i];
			}
			for (int j = 0; j < cduGMXOmAyxVkxibvFForpnXsrwg; j++)
			{
				dataUpdater.buttonValues[j] = CNItCCiCwdAxpfiThmcsvsAxQDUd[j];
			}
			if (ekOgYSpwrSGBRMfalvDqdcYKbILu && !dataUpdater.hasReceivedInput)
			{
				dataUpdater.hasReceivedInput = true;
			}
		}

		void IInputManagerJoystick.FillData(ControllerDataUpdater dataUpdater)
		{
			//ILSpy generated this explicit interface implementation from .override directive in FillData
			this.FillData(dataUpdater);
		}

		public BridgedControllerHWInfo SPIcvJAqyPMgGfGoNMIJwLpnAGreb()
		{
			BridgedControllerHWInfo bridgedControllerHWInfo = new BridgedControllerHWInfo();
			KViIXgPTnImCvaeXqgahCLxGQoyV(bridgedControllerHWInfo);
			return bridgedControllerHWInfo;
		}

		[CustomObfuscation(rename = false)]
		public BridgedController ToBridgedController()
		{
			BridgedController bridgedController = new BridgedController();
			hkYcfUrNakIPorVmTpprhsrOPzzn(bridgedController);
			return bridgedController;
		}

		BridgedController IInputManagerJoystick.ToBridgedController()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToBridgedController
			return this.ToBridgedController();
		}

		[CustomObfuscation(rename = false)]
		public ControllerDisconnectedEventArgs ToControllerDisconnectedEventArgs()
		{
			return new ControllerDisconnectedEventArgs(yEndDaadRqklibWfDZrdDtSjLBGA);
		}

		ControllerDisconnectedEventArgs IInputManagerJoystick.ToControllerDisconnectedEventArgs()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ToControllerDisconnectedEventArgs
			return this.ToControllerDisconnectedEventArgs();
		}

		private void PnFVZbrCbPcduIhDvSzaMJouiwGWA()
		{
			if (AatjHeezYVaTfooOwzmbbrxvozlL == null || !AcrMgDfigfEJLoOQnvIYrwQjrhai(VtLabcPRoBZcpVPBAzPxGpNfqXdn.Synchronous))
			{
				return;
			}
			try
			{
				PdiqkPfJhpkJGKJEbBaUFPwCRqjh();
				ahTDkqWAlkFHaatFgCIQfUIUlUbYA ahTDkqWAlkFHaatFgCIQfUIUlUbYA2 = AatjHeezYVaTfooOwzmbbrxvozlL.gUnAUFkHjFaDMbiGpFTHoDuxMnPpA.gEByLQALZRFcjYYxdrztcJDlGoWr(JymHAvrnPSxZRGvgWOgLWQQayzhF.Any);
				SvtdLZqPXXSdqPuXRNrwJphgDrtv = ahTDkqWAlkFHaatFgCIQfUIUlUbYA2.LDAirpgglklrdDKijDWXnMnRlyZUA;
				jNUntUhpzorzgblufNVoYfcNIMOJ = (XInputDeviceSubType)ahTDkqWAlkFHaatFgCIQfUIUlUbYA2.eHjvaVQoNDafjqdtwigHVDddFkSo;
				if (AatjHeezYVaTfooOwzmbbrxvozlL.gUnAUFkHjFaDMbiGpFTHoDuxMnPpA.omxvdSWlpWVuhBXZxIThfeZOQZus(default(wiMVRiZUpecBrLLXIdvoBrQxZgxk)).AIWJrlyFGcCuoMsGexRvcEoGivbCA)
				{
					EPWHtrOuQZheiwNNugiHFFPcWBdS = true;
				}
				NeeiagHJpsWrFdDNFiyViWSqoIQvA = (ahTDkqWAlkFHaatFgCIQfUIUlUbYA2.nEqSZFzvrvWAZpqjTBXluOzxSGAL & YSAwLPNbEetzlinWOixjLivDHGsp.VoiceSupported) == YSAwLPNbEetzlinWOixjLivDHGsp.VoiceSupported;
				jotnGixcgmgmMGEAPLeiopDGsDAgb();
				GwlpqllBMXAZUErWdEfJpDJhWjQIA = wkFTWrWmhHZmQfDdODUPApCRbQWl.hardwareMapIdentifier.guid;
				if (DFtgHmHdITsouofxhguoAZCYvBFvA)
				{
					BDFQFYUGoQtzMrUbwWemSUaOXdwe = StringTools.AddSpacesToCamelCase(jNUntUhpzorzgblufNVoYfcNIMOJ.ToString());
				}
				else
				{
					BDFQFYUGoQtzMrUbwWemSUaOXdwe = "XInput " + jNUntUhpzorzgblufNVoYfcNIMOJ;
				}
				NFagRJPhEOXjZwcklrhwGMEoGJcJA = $"{BDFQFYUGoQtzMrUbwWemSUaOXdwe} {(ZrDnFabqmZgCPDnZBAQFILJJTdJiA + 1).ToString()}";
				string additionalIdentifyingInformation = LocalizationManager.FormatKey(jNUntUhpzorzgblufNVoYfcNIMOJ.ToString());
				wkFTWrWmhHZmQfDdODUPApCRbQWl.deviceLocalizationInfo.additionalIdentifyingInformation = additionalIdentifyingInformation;
				XkFPlmFLDlkLEJdopNvdbBcWKaQe.Clear();
				AatjHeezYVaTfooOwzmbbrxvozlL.uiUygUzvMkjJbsOaWCoiHvfgocNy();
				PgqmvHXhZidFrCpweTkWHozIXFfqA = MiscTools.CreateGuidHashSHA1(string.Concat(SvtdLZqPXXSdqPuXRNrwJphgDrtv, jNUntUhpzorzgblufNVoYfcNIMOJ, ZrDnFabqmZgCPDnZBAQFILJJTdJiA));
				OUzEDZhQskfBsIiagmripWzQfMRXb = true;
			}
			catch (Exception)
			{
				OUzEDZhQskfBsIiagmripWzQfMRXb = false;
				arRqZnuAeaWaVMFcYxpehUzOUDpD = false;
				PgqmvHXhZidFrCpweTkWHozIXFfqA = Guid.Empty;
			}
		}

		private bool kVszEwPAcgDsnfgqFuvjDuVnEqGu()
		{
			try
			{
				if (jNUntUhpzorzgblufNVoYfcNIMOJ != (XInputDeviceSubType)AatjHeezYVaTfooOwzmbbrxvozlL.gUnAUFkHjFaDMbiGpFTHoDuxMnPpA.gEByLQALZRFcjYYxdrztcJDlGoWr(JymHAvrnPSxZRGvgWOgLWQQayzhF.Any).eHjvaVQoNDafjqdtwigHVDddFkSo)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		private void PdiqkPfJhpkJGKJEbBaUFPwCRqjh()
		{
			NeeiagHJpsWrFdDNFiyViWSqoIQvA = false;
			EPWHtrOuQZheiwNNugiHFFPcWBdS = false;
			CDztTwISAVOIpUfFAJqQRbIvKIzm = false;
			OUzEDZhQskfBsIiagmripWzQfMRXb = false;
		}

		private void dtTtCSwAmbAYpGUPJpTzpbEbhzNvA()
		{
			if (IiydRKCRYVFmvXAVIMAFlptMNMsxA != null)
			{
				IiydRKCRYVFmvXAVIMAFlptMNMsxA();
			}
			AatjHeezYVaTfooOwzmbbrxvozlL.QpdjlkJkBQSWAqWmHaNSMVvEJUQi();
		}

		private void BPUAqyTuMWvwNdzKtGtqFvjSHpiH(bool[] P_0, ref ZKRDewROhvkbxhswDBtkelzSFbjE P_1)
		{
			HardwareJoystickMap.Platform_XInput_Base.Axis[] axes_orig = ((HardwareJoystickMap.Platform_XInput_Base)wkFTWrWmhHZmQfDdODUPApCRbQWl.map).Axes_orig;
			if (axes_orig == null)
			{
				return;
			}
			for (int i = 0; i < axes_orig.Length; i++)
			{
				if (i >= pKLvVKcbYYFgMEIsoBKvnsYlBhkdb)
				{
					throw new Exception("Number of axes in hardware map does not match number of axes found in controller!");
				}
				GIitSxYmCskMdUpsNwsTEtRUNqQH[i] = zsRbnyFxGbuRPkGpZNcuGkEDPakwb(axes_orig[i], P_0, ref P_1);
				if (!ekOgYSpwrSGBRMfalvDqdcYKbILu && GIitSxYmCskMdUpsNwsTEtRUNqQH[i] != 0f)
				{
					ekOgYSpwrSGBRMfalvDqdcYKbILu = true;
				}
			}
		}

		private void bWbcdZHPLpJbQtCpVkhKLguWWopQA(bool[] P_0, ref ZKRDewROhvkbxhswDBtkelzSFbjE P_1)
		{
			HardwareJoystickMap.Platform_XInput_Base.Button[] buttons_orig = ((HardwareJoystickMap.Platform_XInput_Base)wkFTWrWmhHZmQfDdODUPApCRbQWl.map).Buttons_orig;
			if (buttons_orig == null)
			{
				return;
			}
			for (int i = 0; i < buttons_orig.Length; i++)
			{
				if (i >= cduGMXOmAyxVkxibvFForpnXsrwg)
				{
					throw new Exception("Number of buttons in hardware map does not match number of buttons found in controller!");
				}
				CNItCCiCwdAxpfiThmcsvsAxQDUd[i] = BJRBUacTraYGznUVbRWLnGRnWudMA(buttons_orig[i], P_0, ref P_1);
				if (!ekOgYSpwrSGBRMfalvDqdcYKbILu && CNItCCiCwdAxpfiThmcsvsAxQDUd[i])
				{
					ekOgYSpwrSGBRMfalvDqdcYKbILu = true;
				}
			}
		}

		private float zsRbnyFxGbuRPkGpZNcuGkEDPakwb(HardwareJoystickMap.Platform_XInput_Base.Axis P_0, bool[] P_1, ref ZKRDewROhvkbxhswDBtkelzSFbjE P_2)
		{
			if (P_0.sourceType == HardwareElementSourceType.Axis)
			{
				if (P_0.sourceAxis == XInputAxis.None)
				{
					return 0f;
				}
				return FeNCCSJOjxFOVFDpXMgjnuGytAvj(P_0.sourceAxis, ref P_2);
			}
			if (P_0.sourceType == HardwareElementSourceType.Button)
			{
				if (P_0.sourceButton == XInputButton.None)
				{
					return 0f;
				}
				if (!jQtzcINMHvdEdFzLgGHmlzzMuXRq(P_0.sourceButton, P_1))
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

		private float FeNCCSJOjxFOVFDpXMgjnuGytAvj(XInputAxis P_0, ref ZKRDewROhvkbxhswDBtkelzSFbjE P_1)
		{
			return P_0 switch
			{
				XInputAxis.LeftThumbX => AlKsZxctHXLfYYkzfFYPZTTBpoGg.FpJeupWPbrVzYSDnTnnQoLCFxRHS(P_1.aeEKeyMrcUrsZTTmJvKlMuMSPFzd), 
				XInputAxis.LeftThumbY => AlKsZxctHXLfYYkzfFYPZTTBpoGg.FpJeupWPbrVzYSDnTnnQoLCFxRHS(P_1.JZFDZqWQpAaDOcHkpnpmvaGpVRYS), 
				XInputAxis.RightThumbX => AlKsZxctHXLfYYkzfFYPZTTBpoGg.FpJeupWPbrVzYSDnTnnQoLCFxRHS(P_1.urRJbCMCytMpuuFKPvPkBUqZeFfg), 
				XInputAxis.RightThumbY => AlKsZxctHXLfYYkzfFYPZTTBpoGg.FpJeupWPbrVzYSDnTnnQoLCFxRHS(P_1.YvMaXaGnspVVytophcLgvXHmBeazA), 
				XInputAxis.LeftTrigger => AlKsZxctHXLfYYkzfFYPZTTBpoGg.gtPCkmBWEqOiRxYKJAiGVHLZOFJL(P_1.cayjbsAbgDkJjRgfedBgdQOnZyEQ), 
				XInputAxis.RightTrigger => AlKsZxctHXLfYYkzfFYPZTTBpoGg.gtPCkmBWEqOiRxYKJAiGVHLZOFJL(P_1.roVhDzSFwKWtnbfHbmbUKgcEInLh), 
				_ => 0f, 
			};
		}

		private bool BJRBUacTraYGznUVbRWLnGRnWudMA(HardwareJoystickMap.Platform_XInput_Base.Button P_0, bool[] P_1, ref ZKRDewROhvkbxhswDBtkelzSFbjE P_2)
		{
			if (P_0.sourceType == HardwareElementSourceType.Button)
			{
				if (P_0.sourceButton == XInputButton.None)
				{
					return false;
				}
				return jQtzcINMHvdEdFzLgGHmlzzMuXRq(P_0.sourceButton, P_1);
			}
			if (P_0.sourceType == HardwareElementSourceType.Axis)
			{
				if (P_0.sourceAxis == XInputAxis.None)
				{
					return false;
				}
				float num = FeNCCSJOjxFOVFDpXMgjnuGytAvj(P_0.sourceAxis, ref P_2);
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

		private bool jQtzcINMHvdEdFzLgGHmlzzMuXRq(XInputButton P_0, bool[] P_1)
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

		private void jotnGixcgmgmMGEAPLeiopDGsDAgb()
		{
			wkFTWrWmhHZmQfDdODUPApCRbQWl = rkHvTeFqcqaxgapqnshqNpCUFWYw(SPIcvJAqyPMgGfGoNMIJwLpnAGreb());
			if (wkFTWrWmhHZmQfDdODUPApCRbQWl == null)
			{
				Rewired.Logger.LogError("Default hardware map not found!");
				return;
			}
			pKLvVKcbYYFgMEIsoBKvnsYlBhkdb = wkFTWrWmhHZmQfDdODUPApCRbQWl.axisCount;
			cduGMXOmAyxVkxibvFForpnXsrwg = wkFTWrWmhHZmQfDdODUPApCRbQWl.buttonCount;
		}

		private string eCmmVxhxQiFlTQWtPCVyTpcIdkHT()
		{
			return InputTools.FormatHardwareIdentifierString($"{ReInput.currentPlatform.ToString()}{InputSource.XInput.ToString()}{SvtdLZqPXXSdqPuXRNrwJphgDrtv.ToString()}{jNUntUhpzorzgblufNVoYfcNIMOJ.ToString()}");
		}

		private void KViIXgPTnImCvaeXqgahCLxGQoyV(BridgedControllerHWInfo P_0)
		{
			P_0.inputManagerSource = InputSource.XInput;
			P_0.inputSource = P_0.inputManagerSource;
			P_0.deviceType = ControlDeviceType.Unknown;
			P_0.hardwareIdentifier = eCmmVxhxQiFlTQWtPCVyTpcIdkHT();
			P_0.hardwareAxisCount = HfgfBIkeedPwchDmjHeXqQSElKov;
			P_0.hardwareButtonCount = AicKLnVGnEtDWLwkoNYXaImxUpUe;
			P_0.hardwareHatCount = 0;
			P_0.hw_productName = gAeqnrghDxYFbUpWJXdXxweLbPpK;
			P_0.hw_supportsVoice = NeeiagHJpsWrFdDNFiyViWSqoIQvA;
			P_0.hw_supportsVibration = EPWHtrOuQZheiwNNugiHFFPcWBdS;
			P_0.hw_localVibrationMotorCount = (EPWHtrOuQZheiwNNugiHFFPcWBdS ? 2 : 0);
			P_0.hw_xInputSubType = jNUntUhpzorzgblufNVoYfcNIMOJ;
		}

		private void hkYcfUrNakIPorVmTpprhsrOPzzn(BridgedController P_0)
		{
			KViIXgPTnImCvaeXqgahCLxGQoyV(P_0);
			P_0.sourceJoystick = this;
			P_0.gameHardwareMap = wkFTWrWmhHZmQfDdODUPApCRbQWl.ToGameHardwareControllerMap();
			P_0.instanceName = "XInput " + UxpADUxFaMuqtygtRRObxYRjQnBb;
			P_0.productName = "XInput " + gAeqnrghDxYFbUpWJXdXxweLbPpK;
			P_0.isXInputDevice = true;
			P_0.axisCount = pKLvVKcbYYFgMEIsoBKvnsYlBhkdb;
			P_0.buttonCount = cduGMXOmAyxVkxibvFForpnXsrwg;
			P_0.controllerTypeGuid = GwlpqllBMXAZUErWdEfJpDJhWjQIA;
			P_0.controllerExtension = Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002Eextension;
		}

		public void Dispose()
		{
			PMHjoPhHNYGHWilMsQvXTkIbRAHN(true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		protected virtual void GHoefuaTfVkWBvYdXgJMhMUPtNpqA()
		{
			try
			{
				PMHjoPhHNYGHWilMsQvXTkIbRAHN(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void PMHjoPhHNYGHWilMsQvXTkIbRAHN(bool P_0)
		{
			if (ShuFzXnUcVhRggCRChONECBBddlKA)
			{
				return;
			}
			if (P_0)
			{
				if (dkwdOIYrxMlAdLwfZsIEAYqwkWWn)
				{
					AatjHeezYVaTfooOwzmbbrxvozlL.cxnSzDEdfpSCXMliRdbpFRHuuScu();
				}
				if (AatjHeezYVaTfooOwzmbbrxvozlL != null)
				{
					AatjHeezYVaTfooOwzmbbrxvozlL.Dispose();
				}
			}
			ShuFzXnUcVhRggCRChONECBBddlKA = true;
		}
	}

	private class QaWmLSZcQbbEczEXihAkoMFznddU
	{
		private class ofbgtNgYLSRSadtNcEWPfJqlZJZP
		{
			public bool moAvOZnlPsJmNOooxFXNdmbSqlpfA;

			public int ZlRRfVmIKgkzGrULrOOxKDvIBaXGA;

			public XInputDeviceSubType OvYTIMLFfISLurCnwPrqOfbVveSK;

			public void hGxhbeeeoRjtqzfvdEIMPrBgmGWy(OaJsZnjraUCumAdijDhlcsUkRksfA P_0, bool P_1)
			{
				moAvOZnlPsJmNOooxFXNdmbSqlpfA = P_1;
				ZlRRfVmIKgkzGrULrOOxKDvIBaXGA = P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId;
				OvYTIMLFfISLurCnwPrqOfbVveSK = P_0.jNUntUhpzorzgblufNVoYfcNIMOJ;
			}

			public ofbgtNgYLSRSadtNcEWPfJqlZJZP(int P_0, XInputDeviceSubType P_1)
			{
				ZlRRfVmIKgkzGrULrOOxKDvIBaXGA = P_0;
				OvYTIMLFfISLurCnwPrqOfbVveSK = P_1;
			}
		}

		private List<ofbgtNgYLSRSadtNcEWPfJqlZJZP> KdsOSLwvdtPgBjUbwUOiOyGXfTYh;

		public QaWmLSZcQbbEczEXihAkoMFznddU()
		{
			KdsOSLwvdtPgBjUbwUOiOyGXfTYh = new List<ofbgtNgYLSRSadtNcEWPfJqlZJZP>();
		}

		public void tkHCxyCOlDalBIYDirRpCygajnnQc(OaJsZnjraUCumAdijDhlcsUkRksfA P_0, bool P_1)
		{
			if (LfCtYLmeJWcdpCmRKIgSXMXffCvmA(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.jNUntUhpzorzgblufNVoYfcNIMOJ, true) < 0)
			{
				ofbgtNgYLSRSadtNcEWPfJqlZJZP ofbgtNgYLSRSadtNcEWPfJqlZJZP2 = new ofbgtNgYLSRSadtNcEWPfJqlZJZP(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.jNUntUhpzorzgblufNVoYfcNIMOJ);
				ofbgtNgYLSRSadtNcEWPfJqlZJZP2.moAvOZnlPsJmNOooxFXNdmbSqlpfA = P_1;
				KdsOSLwvdtPgBjUbwUOiOyGXfTYh.Add(ofbgtNgYLSRSadtNcEWPfJqlZJZP2);
			}
		}

		public void mDgJoSUeGqSGbDlcWfhFSuHhRoGx(int P_0, OaJsZnjraUCumAdijDhlcsUkRksfA P_1, bool P_2)
		{
			if (P_0 >= 0 && P_0 < KdsOSLwvdtPgBjUbwUOiOyGXfTYh.Count)
			{
				KdsOSLwvdtPgBjUbwUOiOyGXfTYh[P_0].hGxhbeeeoRjtqzfvdEIMPrBgmGWy(P_1, P_2);
			}
		}

		public int CathrAkrpknfHJvdRAbTyRGyvWgtA(XInputDeviceSubType P_0, bool P_1)
		{
			int count = KdsOSLwvdtPgBjUbwUOiOyGXfTYh.Count;
			for (int i = 0; i < count; i++)
			{
				if ((P_1 || !KdsOSLwvdtPgBjUbwUOiOyGXfTYh[i].moAvOZnlPsJmNOooxFXNdmbSqlpfA) && KdsOSLwvdtPgBjUbwUOiOyGXfTYh[i].OvYTIMLFfISLurCnwPrqOfbVveSK == P_0)
				{
					return i;
				}
			}
			return -1;
		}

		public int LfCtYLmeJWcdpCmRKIgSXMXffCvmA(int P_0, XInputDeviceSubType P_1, bool P_2)
		{
			int count = KdsOSLwvdtPgBjUbwUOiOyGXfTYh.Count;
			for (int i = 0; i < count; i++)
			{
				if ((P_2 || !KdsOSLwvdtPgBjUbwUOiOyGXfTYh[i].moAvOZnlPsJmNOooxFXNdmbSqlpfA) && KdsOSLwvdtPgBjUbwUOiOyGXfTYh[i].ZlRRfVmIKgkzGrULrOOxKDvIBaXGA == P_0 && KdsOSLwvdtPgBjUbwUOiOyGXfTYh[i].OvYTIMLFfISLurCnwPrqOfbVveSK == P_1)
				{
					return i;
				}
			}
			return -1;
		}

		public int EmQqhrgSHkRVTXJDAYUvAIUCyHAi(int P_0)
		{
			if (P_0 < 0 || P_0 >= KdsOSLwvdtPgBjUbwUOiOyGXfTYh.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return KdsOSLwvdtPgBjUbwUOiOyGXfTYh[P_0].ZlRRfVmIKgkzGrULrOOxKDvIBaXGA;
		}

		public void RkpKQdZKkvoGYNhAuCXgycBDeqXe(int P_0, bool P_1)
		{
			if (P_0 >= 0 && P_0 < KdsOSLwvdtPgBjUbwUOiOyGXfTYh.Count)
			{
				KdsOSLwvdtPgBjUbwUOiOyGXfTYh[P_0].moAvOZnlPsJmNOooxFXNdmbSqlpfA = P_1;
			}
		}
	}

	private class gwWTapOvyzSmWrnNjOFVZEldLLhp
	{
		public bool YowVxUdTXhshTbkxzBglDShsgTPCb;

		private double MZmTMafUORCQOdHYRKCjcqCJddtu;

		public float fOdjSyZvzafOHtnMADYjeVCcvTLf;

		public gwWTapOvyzSmWrnNjOFVZEldLLhp(float P_0)
		{
			fOdjSyZvzafOHtnMADYjeVCcvTLf = P_0;
		}

		public void rVccHoirmbbKECgHNRTWAFLCHFaMb()
		{
			YowVxUdTXhshTbkxzBglDShsgTPCb = true;
			MZmTMafUORCQOdHYRKCjcqCJddtu = (double)fOdjSyZvzafOHtnMADYjeVCcvTLf + ReInput.unscaledTime;
		}

		public bool EIQRGHRhDOGDkiCQZTlFSLejqIScb()
		{
			if (!YowVxUdTXhshTbkxzBglDShsgTPCb)
			{
				return false;
			}
			if (ReInput.unscaledTime >= MZmTMafUORCQOdHYRKCjcqCJddtu)
			{
				YowVxUdTXhshTbkxzBglDShsgTPCb = false;
				return true;
			}
			return false;
		}
	}

	public class AlKsZxctHXLfYYkzfFYPZTTBpoGg : IDisposable
	{
		public readonly NdFZhsSzsZDhsLnhJcJIaVPkCbZU gUnAUFkHjFaDMbiGpFTHoDuxMnPpA;

		private readonly Controller.Extension acLsicCqJrCeAgVIpBxCLZLjoSlKA;

		public ZKRDewROhvkbxhswDBtkelzSFbjE CDrHJBtgGEETtenhNKdaHTUkxByZB;

		private bool cnmISicVMEisrNNopBcZcHBVlRXLA;

		private readonly ButtonLoopSet ZNUiCLZKCCteNZvPHUTtFaLFoaEL;

		private ZKRDewROhvkbxhswDBtkelzSFbjE DegioCkjHQmzDLYRyuFhSQBsoiCDA;

		private bool PDtiGInbPRMSzuKAJiUWdqqbgQIGA;

		private DualThreadLowLevelInputEventQueue mpHtikFjhSiGhfCbPQuoCEfjqKNuB;

		private readonly object CVIJtaKNbgKEMJfuEBGraIzJdlgZ;

		private RingBuffer<wiMVRiZUpecBrLLXIdvoBrQxZgxk> RtBPceywAvTwoBbrFFVDXPNcqfdH = new RingBuffer<wiMVRiZUpecBrLLXIdvoBrQxZgxk>(5);

		private RingBuffer<wiMVRiZUpecBrLLXIdvoBrQxZgxk> zpIBoxHQeXhIABszjzDfbQLCTuJtb = new RingBuffer<wiMVRiZUpecBrLLXIdvoBrQxZgxk>(5);

		private readonly object lGFBFydrGggfZrvrKzPWOefrYcwNA = new object();

		private readonly object CeidnDXsFDOgFeKZwrdktKZnFsObA = new object();

		private wiMVRiZUpecBrLLXIdvoBrQxZgxk yEKpkEaSUeqEMBWIjnpJoyMckuym;

		private double qdGaBqjJPAYhZvIQaMDJjOCiVLTaA;

		private bool PMfAvGYHJicUdpdlVBvGeEQuvTsSA;

		public Controller.Extension FAluqucGyxUcbXoVzzTAHBGfWTRk => acLsicCqJrCeAgVIpBxCLZLjoSlKA;

		public bool[] GkBrqmDIOtSopmNIJcseejXuDVUwA => ZNUiCLZKCCteNZvPHUTtFaLFoaEL.Current.effectiveValue;

		public AlKsZxctHXLfYYkzfFYPZTTBpoGg(int P_0, UpdateLoopSetting P_1)
		{
			gUnAUFkHjFaDMbiGpFTHoDuxMnPpA = new NdFZhsSzsZDhsLnhJcJIaVPkCbZU((lNPIgqtXddQkIZGnyyeMdgoipcBi)P_0);
			ZNUiCLZKCCteNZvPHUTtFaLFoaEL = new ButtonLoopSet(P_1, 15);
			CVIJtaKNbgKEMJfuEBGraIzJdlgZ = new object();
			mpHtikFjhSiGhfCbPQuoCEfjqKNuB = new DualThreadLowLevelInputEventQueue((int)((float)FwvuhjisMNfwRNPCnXxbQzkrWKy.VjeFpcjaFerIirGHKspqAQDAgjGxA * 0.25f), 15, 6, 0);
			acLsicCqJrCeAgVIpBxCLZLjoSlKA = new XInputControllerExtension(this);
		}

		public void aFcCqTrdZZzMfheQsGnluLUnRTtR()
		{
			ZNUiCLZKCCteNZvPHUTtFaLFoaEL.SetUpdateLoop(ReInput.currentUpdateLoop);
			BQRxjDrJLsRstOnNqmhjEdSPJogF(ref CDrHJBtgGEETtenhNKdaHTUkxByZB);
		}

		public void LJxFAUWyMevDvIHjKEbHcqHQZFIGA()
		{
			ykyKzcGOCSuHaeRdpimzurDPHbye();
			ZNUiCLZKCCteNZvPHUTtFaLFoaEL.Current.ClearWasTrueThisFrame();
		}

		public void uiUygUzvMkjJbsOaWCoiHvfgocNy()
		{
			yDvkSAfogsZiTRbvPAiHaIkCmPeI();
			cnmISicVMEisrNNopBcZcHBVlRXLA = true;
			PDtiGInbPRMSzuKAJiUWdqqbgQIGA = gUnAUFkHjFaDMbiGpFTHoDuxMnPpA.vzdAOjvBqovFbmLHSfIMgBZObqxH;
		}

		public void QpdjlkJkBQSWAqWmHaNSMVvEJUQi()
		{
			cnmISicVMEisrNNopBcZcHBVlRXLA = false;
			PDtiGInbPRMSzuKAJiUWdqqbgQIGA = false;
			yDvkSAfogsZiTRbvPAiHaIkCmPeI();
		}

		public bool RCPKGkOzGIPiahUUgUZIBGjglooP(VtLabcPRoBZcpVPBAzPxGpNfqXdn P_0)
		{
			return P_0 switch
			{
				VtLabcPRoBZcpVPBAzPxGpNfqXdn.Synchronous => PDtiGInbPRMSzuKAJiUWdqqbgQIGA = gUnAUFkHjFaDMbiGpFTHoDuxMnPpA.vzdAOjvBqovFbmLHSfIMgBZObqxH, 
				VtLabcPRoBZcpVPBAzPxGpNfqXdn.Asynchronous => PDtiGInbPRMSzuKAJiUWdqqbgQIGA, 
				_ => throw new NotImplementedException(), 
			};
		}

		public void zbGFxNQXqiwUDtmxrZUgVTbMEIpq(float P_0, int P_1)
		{
			switch (P_1)
			{
			case 0:
				yEKpkEaSUeqEMBWIjnpJoyMckuym.uMUITUOyZUNzcSMmbpywptbGlawH = (ushort)(MathTools.Clamp01(P_0) * 65535f);
				break;
			case 1:
				yEKpkEaSUeqEMBWIjnpJoyMckuym.vroBaCSHMYENemrvZxqfCRtwJdJc = (ushort)(MathTools.Clamp01(P_0) * 65535f);
				break;
			}
			YnxAOsmNuyXqgmqTtUloORmfcDRW();
		}

		public void nunYcxCqZRUKJXQdixrxNWguwYp()
		{
			yEKpkEaSUeqEMBWIjnpJoyMckuym.uMUITUOyZUNzcSMmbpywptbGlawH = 0;
			yEKpkEaSUeqEMBWIjnpJoyMckuym.vroBaCSHMYENemrvZxqfCRtwJdJc = 0;
			YnxAOsmNuyXqgmqTtUloORmfcDRW();
		}

		public void cxnSzDEdfpSCXMliRdbpFRHuuScu()
		{
			yEKpkEaSUeqEMBWIjnpJoyMckuym.uMUITUOyZUNzcSMmbpywptbGlawH = 0;
			yEKpkEaSUeqEMBWIjnpJoyMckuym.vroBaCSHMYENemrvZxqfCRtwJdJc = 0;
			lock (CeidnDXsFDOgFeKZwrdktKZnFsObA)
			{
				lock (lGFBFydrGggfZrvrKzPWOefrYcwNA)
				{
					RtBPceywAvTwoBbrFFVDXPNcqfdH.Clear();
					zpIBoxHQeXhIABszjzDfbQLCTuJtb.Clear();
					PEMtzETXwPcWGHtcpjTPtAEqYRPX(gUnAUFkHjFaDMbiGpFTHoDuxMnPpA, yEKpkEaSUeqEMBWIjnpJoyMckuym, ref qdGaBqjJPAYhZvIQaMDJjOCiVLTaA);
				}
			}
		}

		public void ptgWWAVBuDrYNOKFciHFpUfXXEBf()
		{
			if (!cnmISicVMEisrNNopBcZcHBVlRXLA || !PDtiGInbPRMSzuKAJiUWdqqbgQIGA)
			{
				return;
			}
			pKkCzsEckBaUoBcvMDSUkuZdVCzgA pKkCzsEckBaUoBcvMDSUkuZdVCzgA2;
			double realTime;
			try
			{
				if (!gUnAUFkHjFaDMbiGpFTHoDuxMnPpA.taOguhdGZneCqqKKAsHTPNnZBkRX(out pKkCzsEckBaUoBcvMDSUkuZdVCzgA2))
				{
					PDtiGInbPRMSzuKAJiUWdqqbgQIGA = false;
					return;
				}
				realTime = ReInput.realTime;
			}
			catch
			{
				PDtiGInbPRMSzuKAJiUWdqqbgQIGA = false;
				return;
			}
			lock (CVIJtaKNbgKEMJfuEBGraIzJdlgZ)
			{
				if (!XgPaZveYmDYuKsWyMRguAeLVAAI(pKkCzsEckBaUoBcvMDSUkuZdVCzgA2.TAKQpZqWFULeWWLZwWSzYizeaFtgA, DegioCkjHQmzDLYRyuFhSQBsoiCDA))
				{
					using (DualThreadLowLevelInputEventQueue.INewEventWrapper newEventWrapper = mpHtikFjhSiGhfCbPQuoCEfjqKNuB.T_CreateEvent())
					{
						fgjWMHyYiDEzlfyypIysesXdeSgm(ref pKkCzsEckBaUoBcvMDSUkuZdVCzgA2.TAKQpZqWFULeWWLZwWSzYizeaFtgA, realTime, newEventWrapper.Event);
					}
					DegioCkjHQmzDLYRyuFhSQBsoiCDA = pKkCzsEckBaUoBcvMDSUkuZdVCzgA2.TAKQpZqWFULeWWLZwWSzYizeaFtgA;
				}
			}
		}

		public void nqegbNSauoLMkFhwVpVssPKDfWPV()
		{
			if (!cnmISicVMEisrNNopBcZcHBVlRXLA || !PDtiGInbPRMSzuKAJiUWdqqbgQIGA || ReInput.realTime < qdGaBqjJPAYhZvIQaMDJjOCiVLTaA + 0.009999999776482582)
			{
				return;
			}
			lock (CeidnDXsFDOgFeKZwrdktKZnFsObA)
			{
				lock (lGFBFydrGggfZrvrKzPWOefrYcwNA)
				{
					MiscTools.Swap(ref RtBPceywAvTwoBbrFFVDXPNcqfdH, ref zpIBoxHQeXhIABszjzDfbQLCTuJtb);
				}
				jmsMltokEthCEoPAzgKeivMraRdc(zpIBoxHQeXhIABszjzDfbQLCTuJtb, gUnAUFkHjFaDMbiGpFTHoDuxMnPpA, ref qdGaBqjJPAYhZvIQaMDJjOCiVLTaA);
			}
		}

		private void ykyKzcGOCSuHaeRdpimzurDPHbye()
		{
			bflhBykHlOKHVssQHpUogvREbBZPB();
		}

		private void bflhBykHlOKHVssQHpUogvREbBZPB()
		{
			if (!(ReInput.realTime < qdGaBqjJPAYhZvIQaMDJjOCiVLTaA + 1.5) && (!Mathf.Approximately((int)yEKpkEaSUeqEMBWIjnpJoyMckuym.uMUITUOyZUNzcSMmbpywptbGlawH, 0f) || !Mathf.Approximately((int)yEKpkEaSUeqEMBWIjnpJoyMckuym.vroBaCSHMYENemrvZxqfCRtwJdJc, 0f)))
			{
				YnxAOsmNuyXqgmqTtUloORmfcDRW();
			}
		}

		private void YnxAOsmNuyXqgmqTtUloORmfcDRW()
		{
			lock (lGFBFydrGggfZrvrKzPWOefrYcwNA)
			{
				RtBPceywAvTwoBbrFFVDXPNcqfdH.Enqueue(yEKpkEaSUeqEMBWIjnpJoyMckuym);
			}
		}

		private static void jmsMltokEthCEoPAzgKeivMraRdc(RingBuffer<wiMVRiZUpecBrLLXIdvoBrQxZgxk> P_0, NdFZhsSzsZDhsLnhJcJIaVPkCbZU P_1, ref double P_2)
		{
			if (P_0.Count > 0)
			{
				PEMtzETXwPcWGHtcpjTPtAEqYRPX(P_1, P_0[P_0.Count - 1], ref P_2);
				P_0.Clear();
			}
		}

		private static void PEMtzETXwPcWGHtcpjTPtAEqYRPX(NdFZhsSzsZDhsLnhJcJIaVPkCbZU P_0, wiMVRiZUpecBrLLXIdvoBrQxZgxk P_1, ref double P_2)
		{
			try
			{
				P_0.omxvdSWlpWVuhBXZxIThfeZOQZus(P_1);
			}
			catch
			{
			}
			P_2 = ReInput.realTime;
		}

		private void BQRxjDrJLsRstOnNqmhjEdSPJogF(ref ZKRDewROhvkbxhswDBtkelzSFbjE P_0)
		{
			while (mpHtikFjhSiGhfCbPQuoCEfjqKNuB.ProcessNewEvents())
			{
				nNWrxHRGbqxuNPsQPshMXmcDXEsi(ref P_0, ref mpHtikFjhSiGhfCbPQuoCEfjqKNuB.currentEvent);
				for (int i = 0; i < 15; i++)
				{
					ZNUiCLZKCCteNZvPHUTtFaLFoaEL.SetValue(i, lYgIBWSOFOVcFMDzLBUfDRfWHNUu((int)P_0.MqEmANuduWtSVLzdXRbfaChCIDkIA, i), mpHtikFjhSiGhfCbPQuoCEfjqKNuB.currentEvent.GetTimestamp());
				}
			}
		}

		private void fgjWMHyYiDEzlfyypIysesXdeSgm(ref ZKRDewROhvkbxhswDBtkelzSFbjE P_0, double P_1, LowLevelInputEvent P_2)
		{
			P_2.SetTimestamp(P_1);
			int mqEmANuduWtSVLzdXRbfaChCIDkIA = (int)P_0.MqEmANuduWtSVLzdXRbfaChCIDkIA;
			P_2.SetButtonsBitMask((mqEmANuduWtSVLzdXRbfaChCIDkIA & 0x7FF) | ((mqEmANuduWtSVLzdXRbfaChCIDkIA & (mqEmANuduWtSVLzdXRbfaChCIDkIA & -4096)) >> 1), 0);
			P_2.SetAxisValue(0, FpJeupWPbrVzYSDnTnnQoLCFxRHS(P_0.aeEKeyMrcUrsZTTmJvKlMuMSPFzd));
			P_2.SetAxisValue(1, FpJeupWPbrVzYSDnTnnQoLCFxRHS(P_0.JZFDZqWQpAaDOcHkpnpmvaGpVRYS));
			P_2.SetAxisValue(2, FpJeupWPbrVzYSDnTnnQoLCFxRHS(P_0.urRJbCMCytMpuuFKPvPkBUqZeFfg));
			P_2.SetAxisValue(3, FpJeupWPbrVzYSDnTnnQoLCFxRHS(P_0.YvMaXaGnspVVytophcLgvXHmBeazA));
			P_2.SetAxisValue(4, gtPCkmBWEqOiRxYKJAiGVHLZOFJL(P_0.cayjbsAbgDkJjRgfedBgdQOnZyEQ));
			P_2.SetAxisValue(5, gtPCkmBWEqOiRxYKJAiGVHLZOFJL(P_0.roVhDzSFwKWtnbfHbmbUKgcEInLh));
		}

		private void nNWrxHRGbqxuNPsQPshMXmcDXEsi(ref ZKRDewROhvkbxhswDBtkelzSFbjE P_0, ref LowLevelInputEvent P_1)
		{
			int buttonsBitMask = P_1.GetButtonsBitMask(0);
			P_0.MqEmANuduWtSVLzdXRbfaChCIDkIA = (IxDRBNzpMbZOVBrStyJSGFUAAqar)((buttonsBitMask & 0x7FF) | ((buttonsBitMask & (buttonsBitMask & -2048)) << 1));
			P_0.aeEKeyMrcUrsZTTmJvKlMuMSPFzd = (short)(P_1.GetAxisValue(0) * 32768f);
			P_0.JZFDZqWQpAaDOcHkpnpmvaGpVRYS = (short)(P_1.GetAxisValue(1) * 32768f);
			P_0.urRJbCMCytMpuuFKPvPkBUqZeFfg = (short)(P_1.GetAxisValue(2) * 32768f);
			P_0.YvMaXaGnspVVytophcLgvXHmBeazA = (short)(P_1.GetAxisValue(3) * 32768f);
			P_0.cayjbsAbgDkJjRgfedBgdQOnZyEQ = (byte)(P_1.GetAxisValue(4) * 255f);
			P_0.roVhDzSFwKWtnbfHbmbUKgcEInLh = (byte)(P_1.GetAxisValue(5) * 255f);
		}

		private static bool lYgIBWSOFOVcFMDzLBUfDRfWHNUu(int P_0, int P_1)
		{
			if (P_1 > 10)
			{
				P_1++;
			}
			return (P_0 & (1 << P_1)) != 0;
		}

		private void yDvkSAfogsZiTRbvPAiHaIkCmPeI()
		{
			lock (CVIJtaKNbgKEMJfuEBGraIzJdlgZ)
			{
				CDrHJBtgGEETtenhNKdaHTUkxByZB = default(ZKRDewROhvkbxhswDBtkelzSFbjE);
				DegioCkjHQmzDLYRyuFhSQBsoiCDA = default(ZKRDewROhvkbxhswDBtkelzSFbjE);
				ZNUiCLZKCCteNZvPHUTtFaLFoaEL.Clear();
				mpHtikFjhSiGhfCbPQuoCEfjqKNuB.Clear();
			}
		}

		public void Dispose()
		{
			JubpuxPpSwmRuauKKlZBjDrevAQy(true);
			GC.SuppressFinalize(this);
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in Dispose
			this.Dispose();
		}

		protected virtual void aXpicnURJniegldxdaSurbvSjiVM()
		{
			try
			{
				JubpuxPpSwmRuauKKlZBjDrevAQy(false);
			}
			finally
			{
				base.Finalize();
			}
		}

		protected virtual void JubpuxPpSwmRuauKKlZBjDrevAQy(bool P_0)
		{
			if (!PMfAvGYHJicUdpdlVBvGeEQuvTsSA)
			{
				if (P_0)
				{
					mpHtikFjhSiGhfCbPQuoCEfjqKNuB.Dispose();
				}
				PMfAvGYHJicUdpdlVBvGeEQuvTsSA = true;
			}
		}

		public static float FpJeupWPbrVzYSDnTnnQoLCFxRHS(int P_0)
		{
			if (P_0 == 0)
			{
				return 0f;
			}
			return MathTools.Clamp((float)MathTools.Abs(P_0) / 32768f * (float)MathTools.Sign(P_0), -1f, 1f);
		}

		public static float gtPCkmBWEqOiRxYKJAiGVHLZOFJL(int P_0)
		{
			if (P_0 == 0)
			{
				return 0f;
			}
			return MathTools.Clamp((float)MathTools.Abs(P_0) / 255f * (float)MathTools.Sign(P_0), -1f, 1f);
		}

		private static bool XgPaZveYmDYuKsWyMRguAeLVAAI(ZKRDewROhvkbxhswDBtkelzSFbjE P_0, ZKRDewROhvkbxhswDBtkelzSFbjE P_1)
		{
			if (P_0.MqEmANuduWtSVLzdXRbfaChCIDkIA == P_1.MqEmANuduWtSVLzdXRbfaChCIDkIA && P_0.cayjbsAbgDkJjRgfedBgdQOnZyEQ == P_1.cayjbsAbgDkJjRgfedBgdQOnZyEQ && P_0.roVhDzSFwKWtnbfHbmbUKgcEInLh == P_1.roVhDzSFwKWtnbfHbmbUKgcEInLh && P_0.aeEKeyMrcUrsZTTmJvKlMuMSPFzd == P_1.aeEKeyMrcUrsZTTmJvKlMuMSPFzd && P_0.JZFDZqWQpAaDOcHkpnpmvaGpVRYS == P_1.JZFDZqWQpAaDOcHkpnpmvaGpVRYS && P_0.urRJbCMCytMpuuFKPvPkBUqZeFfg == P_1.urRJbCMCytMpuuFKPvPkBUqZeFfg)
			{
				return P_0.YvMaXaGnspVVytophcLgvXHmBeazA == P_1.YvMaXaGnspVVytophcLgvXHmBeazA;
			}
			return false;
		}
	}

	public enum VtLabcPRoBZcpVPBAzPxGpNfqXdn
	{
		Synchronous = 0,
		Asynchronous = 1
	}

	private OaJsZnjraUCumAdijDhlcsUkRksfA[] vEIwJUJiXsrOYbAmsRFqehQkvUsp;

	private bool smzLuJytmocWFLwNvAqYZNzHIEIM;

	private gwWTapOvyzSmWrnNjOFVZEldLLhp QFWEprjgVJxmCFQfOsDZQrklsvYdA;

	private QaWmLSZcQbbEczEXihAkoMFznddU kLsuZhvpZmHEBxxbzMofegtOxcLb;

	private global::gxiqikdujHobFnpuuAGPldgYhOvdA<bool> cNqhwpBTLbsfsJPggRjDxfkKAfGe;

	private bool[] AtMTXqlMkXUuzONTclJXohvTUjYf;

	private bool[] SIWNpGQFEplwPOWSOjpqZbxKMHKy;

	private bool eUwfhIfbAqsxLParDLQpvIHyEfvIb;

	private readonly bool dmfzHdrSuNdXgGAvMOlfYIsLQRkZ;

	private readonly UpdateLoopSetting SadTTmYlRmbPhhuqGeNEypCeHcAlA;

	private UpdateLoopType qFZWbNBrQWlyDiGLxetPzslvikRG;

	private UpdateLoopType dTPiWoRrCBZGCSipMYoUomOPzNBm;

	private Action<int, ControllerDataUpdater> qaWIJYibditHsGVPheztYXZkBKXEb;

	private bool JMXeBkboiHtDzWMxfpHQxhVKiabv;

	private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> nqXWFzjfOeTmzuQTWenLyitzMPFB;

	private Func<int> YddWjEvdMspshIoxJQAFCbnUgpmB;

	private Func<PidVid, bool> LcFCbJfyheZrvZybZNzmRwIgCDY;

	private static Guid[] NaMRNggqjoEYsDYVtgLYrhMihOKB;

	private static string[] JCIhdcCKxHrjoonEikTLgNCfpCFDB;

	private static string[] GKjmkBolIilbturrBmQNReQpwGMi;

	[CustomObfuscation(rename = false)]
	int PlatformInputManager.deviceCount
	{
		get
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if (vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].dkwdOIYrxMlAdLwfZsIEAYqwkWWn)
				{
					num++;
				}
			}
			return num;
		}
	}

	[CustomObfuscation(rename = false)]
	PlatformInputManager PlatformInputManager.primaryInputManager => this;

	[CustomObfuscation(rename = false)]
	IInputSource PlatformInputManager.inputSource => null;

	[CustomObfuscation(rename = false)]
	InputSource PlatformInputManager.inputSourceType => InputSource.XInput;

	hffAeeWHwYDDOcXNFMampSOQKLMy DDWjNWYAVnuAprLJBexwaxtfkBnQA.zUsuOOCKPxLJNQcKWCIOgzcNdLNGb => hffAeeWHwYDDOcXNFMampSOQKLMy.XInput;

	public TVocCDfxMinGIOCkmldqehsxNAxhb(bool P_0, UpdateLoopSetting P_1, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_2, Func<int> P_3, Func<PidVid, bool> P_4)
	{
		dmfzHdrSuNdXgGAvMOlfYIsLQRkZ = P_0;
		SadTTmYlRmbPhhuqGeNEypCeHcAlA = P_1;
		LcFCbJfyheZrvZybZNzmRwIgCDY = P_4;
		JMXeBkboiHtDzWMxfpHQxhVKiabv = true;
		try
		{
			if (!wHjfVJsrMiRUzqkXLzlmJabeEUxs.tmSPMeeaNrAIkoZQOoVxEotHEtqiA(out var akrbRtVxdChjDUDDQVgNeGhDwOgV2, out var text, out var _))
			{
				throw new Exception("XInput is not available.");
			}
			if (akrbRtVxdChjDUDDQVgNeGhDwOgV2 < akrbRtVxdChjDUDDQVgNeGhDwOgV.XINPUT_1_3)
			{
				Rewired.Logger.LogWarning("The version of XInput (" + text + ") detected on your system is out of date. Please update to the latest version of XInput. Input will still function, but all features may not be available. See the documentation for required dependencies.");
			}
			else
			{
				_ = 4;
			}
			nqXWFzjfOeTmzuQTWenLyitzMPFB = P_2;
			YddWjEvdMspshIoxJQAFCbnUgpmB = P_3;
			eUwfhIfbAqsxLParDLQpvIHyEfvIb = UnityTools.platform == Platform.WindowsAppStore;
			using (TempListPool.TList<UpdateLoopType> tList = TempListPool.GetTList<UpdateLoopType>(3))
			{
				List<UpdateLoopType> list = tList.list;
				EnumConverter.ToUpdateLoopTypes(SadTTmYlRmbPhhuqGeNEypCeHcAlA, list);
				int num2 = 0;
				if (num2 < list.Count)
				{
					dTPiWoRrCBZGCSipMYoUomOPzNBm = list[num2];
				}
			}
			cNqhwpBTLbsfsJPggRjDxfkKAfGe = new global::gxiqikdujHobFnpuuAGPldgYhOvdA<bool>(true, hTqTkBCYEZIQlcEoorCPaTfXvTeY);
			AtMTXqlMkXUuzONTclJXohvTUjYf = new bool[4];
			SIWNpGQFEplwPOWSOjpqZbxKMHKy = new bool[4];
			qaWIJYibditHsGVPheztYXZkBKXEb = UpdateControllerData;
			if (eUwfhIfbAqsxLParDLQpvIHyEfvIb)
			{
				ViDWZxQiJZiJaiCeTjHlSyAIeSen();
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
		if (JMXeBkboiHtDzWMxfpHQxhVKiabv)
		{
			QFWEprjgVJxmCFQfOsDZQrklsvYdA = new gwWTapOvyzSmWrnNjOFVZEldLLhp(1f);
		}
		kLsuZhvpZmHEBxxbzMofegtOxcLb = new QaWmLSZcQbbEczEXihAkoMFznddU();
		if (vEIwJUJiXsrOYbAmsRFqehQkvUsp == null)
		{
			vEIwJUJiXsrOYbAmsRFqehQkvUsp = new OaJsZnjraUCumAdijDhlcsUkRksfA[4];
			for (int i = 0; i < 4; i++)
			{
				AlKsZxctHXLfYYkzfFYPZTTBpoGg alKsZxctHXLfYYkzfFYPZTTBpoGg = new AlKsZxctHXLfYYkzfFYPZTTBpoGg(i, SadTTmYlRmbPhhuqGeNEypCeHcAlA);
				FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf.ThreadUpdateEvent += alKsZxctHXLfYYkzfFYPZTTBpoGg.ptgWWAVBuDrYNOKFciHFpUfXXEBf;
				FwvuhjisMNfwRNPCnXxbQzkrWKy.apYJZWeiHEkMNcOtWVjXVlAhaguy.ThreadUpdateEvent += alKsZxctHXLfYYkzfFYPZTTBpoGg.nqegbNSauoLMkFhwVpVssPKDfWPV;
				vEIwJUJiXsrOYbAmsRFqehQkvUsp[i] = new OaJsZnjraUCumAdijDhlcsUkRksfA(i, eUwfhIfbAqsxLParDLQpvIHyEfvIb, alKsZxctHXLfYYkzfFYPZTTBpoGg, nqXWFzjfOeTmzuQTWenLyitzMPFB, SystemDeviceDisconnected);
			}
		}
		TmBYgBeUCQlTxnCztQRJTcxWBngl(true);
		Update(UpdateLoopType.Update);
	}

	[CustomObfuscation(rename = false)]
	public override void Update(UpdateLoopType currentUpdateLoop)
	{
		qFZWbNBrQWlyDiGLxetPzslvikRG = currentUpdateLoop;
		URbQqZouBgOPlqMcjmnvKskXNHUO();
		for (int i = 0; i < 4; i++)
		{
			if (vEIwJUJiXsrOYbAmsRFqehQkvUsp[i] != null && vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].dkwdOIYrxMlAdLwfZsIEAYqwkWWn)
			{
				vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].Update();
			}
		}
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		if (cNqhwpBTLbsfsJPggRjDxfkKAfGe != null)
		{
			cNqhwpBTLbsfsJPggRjDxfkKAfGe.WvLYkoCkCYjVtJGzHiicpMYRreXw();
		}
		if (vEIwJUJiXsrOYbAmsRFqehQkvUsp != null)
		{
			for (int i = 0; i < 4; i++)
			{
				if (vEIwJUJiXsrOYbAmsRFqehQkvUsp[i] != null)
				{
					if (FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf != null)
					{
						FwvuhjisMNfwRNPCnXxbQzkrWKy.fEAUVDpMSAiwvnNRVUnogqubQGhf.ThreadUpdateEvent -= vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].AatjHeezYVaTfooOwzmbbrxvozlL.ptgWWAVBuDrYNOKFciHFpUfXXEBf;
					}
					if (FwvuhjisMNfwRNPCnXxbQzkrWKy.apYJZWeiHEkMNcOtWVjXVlAhaguy != null)
					{
						FwvuhjisMNfwRNPCnXxbQzkrWKy.apYJZWeiHEkMNcOtWVjXVlAhaguy.ThreadUpdateEvent -= vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].AatjHeezYVaTfooOwzmbbrxvozlL.nqegbNSauoLMkFhwVpVssPKDfWPV;
					}
					vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].Dispose();
				}
			}
		}
		wHjfVJsrMiRUzqkXLzlmJabeEUxs.acMqKyPUxPmXOkMIsTwChGHaXiWn();
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return qaWIJYibditHsGVPheztYXZkBKXEb;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int assignedControllerId, ControllerDataUpdater data)
	{
		vEIwJUJiXsrOYbAmsRFqehQkvUsp[assignedControllerId].FillData(data);
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
		TmBYgBeUCQlTxnCztQRJTcxWBngl(true);
		ZazGTEglyvWJohfxtCLIOvNEzIGW();
		if (_SystemDeviceConnectedEvent != null)
		{
			_SystemDeviceConnectedEvent();
		}
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
		TmBYgBeUCQlTxnCztQRJTcxWBngl(true);
		ZazGTEglyvWJohfxtCLIOvNEzIGW();
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

	bool DDWjNWYAVnuAprLJBexwaxtfkBnQA.MT_HandlesController(string devicePath, string productName, string bluetoothName, PidVid pidVid)
	{
		if (LcFCbJfyheZrvZybZNzmRwIgCDY(pidVid))
		{
			return false;
		}
		return HflsaIeTlfxEnYiIQPtgEHToGzZq(devicePath, productName, bluetoothName, MiscTools.CreateHIDProductGuid(pidVid.vendorId, pidVid.productId));
	}

	private bool dlJDIdFnNORrLjyScECntuhHYEeiA()
	{
		if (qFZWbNBrQWlyDiGLxetPzslvikRG != dTPiWoRrCBZGCSipMYoUomOPzNBm)
		{
			return false;
		}
		bool num = QFWEprjgVJxmCFQfOsDZQrklsvYdA.EIQRGHRhDOGDkiCQZTlFSLejqIScb();
		if (num)
		{
			TmBYgBeUCQlTxnCztQRJTcxWBngl(true);
		}
		return num;
	}

	private void TmBYgBeUCQlTxnCztQRJTcxWBngl(bool P_0)
	{
		smzLuJytmocWFLwNvAqYZNzHIEIM = P_0;
		if (JMXeBkboiHtDzWMxfpHQxhVKiabv)
		{
			QFWEprjgVJxmCFQfOsDZQrklsvYdA.rVccHoirmbbKECgHNRTWAFLCHFaMb();
		}
	}

	private void ZazGTEglyvWJohfxtCLIOvNEzIGW()
	{
		if (cNqhwpBTLbsfsJPggRjDxfkKAfGe != null)
		{
			cNqhwpBTLbsfsJPggRjDxfkKAfGe.LmbHGGTdiCfvkXltfXtklzlyoGD();
		}
	}

	private void ViDWZxQiJZiJaiCeTjHlSyAIeSen()
	{
		_ = new NdFZhsSzsZDhsLnhJcJIaVPkCbZU().vzdAOjvBqovFbmLHSfIMgBZObqxH;
	}

	private void URbQqZouBgOPlqMcjmnvKskXNHUO()
	{
		bool flag = false;
		if (JMXeBkboiHtDzWMxfpHQxhVKiabv)
		{
			flag = dlJDIdFnNORrLjyScECntuhHYEeiA();
		}
		if (!flag && smzLuJytmocWFLwNvAqYZNzHIEIM)
		{
			MczzEDvQxGYJXYWCeVfLRcjqMUdG(CBUOUUvNnalPcNHTiLUJONHCvgBI());
			TmBYgBeUCQlTxnCztQRJTcxWBngl(false);
			ZazGTEglyvWJohfxtCLIOvNEzIGW();
			return;
		}
		if (smzLuJytmocWFLwNvAqYZNzHIEIM)
		{
			zyOPHZeoVZeCxNFRUjPFLNZqqaon();
		}
		if (cNqhwpBTLbsfsJPggRjDxfkKAfGe.LYkqHsINOrRmqDzoBVFvzhFfDHwO && cNqhwpBTLbsfsJPggRjDxfkKAfGe.XrInaWDtMKIvqgrHsTdTFhcYalgT())
		{
			nNyuptFFxhhnEgPoNmIbEoPZEdsKA();
		}
	}

	private void zyOPHZeoVZeCxNFRUjPFLNZqqaon()
	{
		smzLuJytmocWFLwNvAqYZNzHIEIM = false;
		if (!cNqhwpBTLbsfsJPggRjDxfkKAfGe.LYkqHsINOrRmqDzoBVFvzhFfDHwO)
		{
			cNqhwpBTLbsfsJPggRjDxfkKAfGe.dLArMTCEuzcNhWTwyvoEESZrqiGi();
		}
	}

	private void nNyuptFFxhhnEgPoNmIbEoPZEdsKA()
	{
		lock (AtMTXqlMkXUuzONTclJXohvTUjYf)
		{
			Array.Copy(AtMTXqlMkXUuzONTclJXohvTUjYf, SIWNpGQFEplwPOWSOjpqZbxKMHKy, 4);
		}
		MczzEDvQxGYJXYWCeVfLRcjqMUdG(SIWNpGQFEplwPOWSOjpqZbxKMHKy);
	}

	private bool hTqTkBCYEZIQlcEoorCPaTfXvTeY()
	{
		lock (AtMTXqlMkXUuzONTclJXohvTUjYf)
		{
			for (int i = 0; i < 4; i++)
			{
				if (vEIwJUJiXsrOYbAmsRFqehQkvUsp[i] != null)
				{
					AtMTXqlMkXUuzONTclJXohvTUjYf[i] = vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].QdTmCIHPCRbxRDqUNYmZHMeNQcCw(VtLabcPRoBZcpVPBAzPxGpNfqXdn.Synchronous);
				}
			}
		}
		return true;
	}

	private bool[] CBUOUUvNnalPcNHTiLUJONHCvgBI()
	{
		for (int i = 0; i < 4; i++)
		{
			SIWNpGQFEplwPOWSOjpqZbxKMHKy[i] = vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].QdTmCIHPCRbxRDqUNYmZHMeNQcCw(VtLabcPRoBZcpVPBAzPxGpNfqXdn.Synchronous);
		}
		return SIWNpGQFEplwPOWSOjpqZbxKMHKy;
	}

	private void MczzEDvQxGYJXYWCeVfLRcjqMUdG(bool[] P_0)
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (vEIwJUJiXsrOYbAmsRFqehQkvUsp[i] != null && vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].CDztTwISAVOIpUfFAJqQRbIvKIzm)
			{
				bool flag = P_0[i];
				vEIwJUJiXsrOYbAmsRFqehQkvUsp[i].SLWYJjXDkczKqzAJNBHRlobnCnEj(flag);
				if (!flag)
				{
					uMLztJshhLIGqPSBMVDXPubBrKLI(vEIwJUJiXsrOYbAmsRFqehQkvUsp[i], false);
				}
			}
		}
		for (int j = 0; j < 4; j++)
		{
			if (vEIwJUJiXsrOYbAmsRFqehQkvUsp[j] != null && !vEIwJUJiXsrOYbAmsRFqehQkvUsp[j].CDztTwISAVOIpUfFAJqQRbIvKIzm)
			{
				bool flag2 = P_0[j];
				vEIwJUJiXsrOYbAmsRFqehQkvUsp[j].SLWYJjXDkczKqzAJNBHRlobnCnEj(flag2);
				if (flag2 && !uMLztJshhLIGqPSBMVDXPubBrKLI(vEIwJUJiXsrOYbAmsRFqehQkvUsp[j], true))
				{
					num |= ((j == 0) ? 1 : (1 << j));
				}
			}
		}
		for (int k = 0; k < 4; k++)
		{
			if (vEIwJUJiXsrOYbAmsRFqehQkvUsp[k] != null)
			{
				int num2 = ((k == 0) ? 1 : (1 << k));
				if ((num & num2) != 1 << k)
				{
					vEIwJUJiXsrOYbAmsRFqehQkvUsp[k].MUFyBzyHeAgZbNFrdfExJIBTaEGCb(P_0[k]);
				}
			}
		}
	}

	private bool uMLztJshhLIGqPSBMVDXPubBrKLI(OaJsZnjraUCumAdijDhlcsUkRksfA P_0, bool P_1)
	{
		if (P_1)
		{
			P_0.JiWviFcDkPiYGTgxqGbtjZcHdfnu();
			if (!P_0.OUzEDZhQskfBsIiagmripWzQfMRXb)
			{
				return false;
			}
			int num = kLsuZhvpZmHEBxxbzMofegtOxcLb.CathrAkrpknfHJvdRAbTyRGyvWgtA(P_0.jNUntUhpzorzgblufNVoYfcNIMOJ, false);
			if (num >= 0)
			{
				P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = kLsuZhvpZmHEBxxbzMofegtOxcLb.EmQqhrgSHkRVTXJDAYUvAIUCyHAi(num);
				kLsuZhvpZmHEBxxbzMofegtOxcLb.mDgJoSUeGqSGbDlcWfhFSuHhRoGx(num, P_0, true);
			}
			else
			{
				P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId = YddWjEvdMspshIoxJQAFCbnUgpmB();
				kLsuZhvpZmHEBxxbzMofegtOxcLb.tkHCxyCOlDalBIYDirRpCygajnnQc(P_0, true);
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
			int num2 = kLsuZhvpZmHEBxxbzMofegtOxcLb.LfCtYLmeJWcdpCmRKIgSXMXffCvmA(P_0.Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002ErewiredId, P_0.jNUntUhpzorzgblufNVoYfcNIMOJ, true);
			if (num2 >= 0)
			{
				kLsuZhvpZmHEBxxbzMofegtOxcLb.RkpKQdZKkvoGYNhAuCXgycBDeqXe(num2, false);
			}
			ControllerDisconnectedEventArgs obj2 = P_0.ToControllerDisconnectedEventArgs();
			P_0.AyaRMUdEcIpVwYBpwuWAnxukPOpG();
			if (_DeviceDisconnectedEvent != null)
			{
				_DeviceDisconnectedEvent(obj2);
			}
		}
		return true;
	}

	static TVocCDfxMinGIOCkmldqehsxNAxhb()
	{
		NaMRNggqjoEYsDYVtgLYrhMihOKB = new Guid[2]
		{
			new Guid("72100955-0000-0000-0000-504944564944"),
			new Guid("02e0045e-0000-0000-0000-504944564944")
		};
		JCIhdcCKxHrjoonEikTLgNCfpCFDB = new string[1] { "Xbox Bluetooth Gamepad" };
		GKjmkBolIilbturrBmQNReQpwGMi = new string[1] { "Xbox Wireless Controller.*" };
	}

	public static bool HflsaIeTlfxEnYiIQPtgEHToGzZq(string P_0, string P_1, string P_2, Guid P_3)
	{
		if (ArrayTools.Contains(NaMRNggqjoEYsDYVtgLYrhMihOKB, P_3))
		{
			return true;
		}
		if (!string.IsNullOrEmpty(P_1))
		{
			for (int i = 0; i < JCIhdcCKxHrjoonEikTLgNCfpCFDB.Length; i++)
			{
				if (P_1.Equals(JCIhdcCKxHrjoonEikTLgNCfpCFDB[i], StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		if (!string.IsNullOrEmpty(P_2))
		{
			for (int j = 0; j < GKjmkBolIilbturrBmQNReQpwGMi.Length; j++)
			{
				if (Regex.IsMatch(P_2, GKjmkBolIilbturrBmQNReQpwGMi[j], RegexOptions.IgnoreCase))
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
