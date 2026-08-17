using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Config;
using Rewired.Data;
using Rewired.Interfaces;
using Rewired.Internal.Localization;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal class wDUrumWuOqINTrddWncdMKuNHzcq : PlatformInputManager, INativePlatformHelper
{
	private class lfOpqzNfzduAWAkmMJpRyDYjunfl
	{
		private class GgoqbogGqmeqpbFlYzRGMIUtbwIQA
		{
			public int JoCVkTXVFTVYEPfgcTbaOGjMiBOk;

			public int pYfqgxqpBJRwtMylIxTZNdopoMUr;

			public int OKOzrFAkGxNXarxcTjQflZgKFxqk;

			public InputSource bxcOPjRMsFmkKsDYLIxMKiTlBMVv;

			public GgoqbogGqmeqpbFlYzRGMIUtbwIQA(int P_0, int P_1, int P_2, InputSource P_3)
			{
				JoCVkTXVFTVYEPfgcTbaOGjMiBOk = P_0;
				pYfqgxqpBJRwtMylIxTZNdopoMUr = P_1;
				OKOzrFAkGxNXarxcTjQflZgKFxqk = P_2;
				bxcOPjRMsFmkKsDYLIxMKiTlBMVv = P_3;
			}

			public void YSzVZeahALuzteqJAMQpkXjXfMcX(int P_0)
			{
				pYfqgxqpBJRwtMylIxTZNdopoMUr = P_0;
			}

			public hmfEBvhAnYkbQiokyzPkGEbknMrP JsFfDYxSSoCKKnsvOGcSyiCfuFwy()
			{
				return new hmfEBvhAnYkbQiokyzPkGEbknMrP(JoCVkTXVFTVYEPfgcTbaOGjMiBOk, pYfqgxqpBJRwtMylIxTZNdopoMUr, bxcOPjRMsFmkKsDYLIxMKiTlBMVv);
			}

			public static int ktfiHVeCMscmthnHovOmVnVgamfrA(GgoqbogGqmeqpbFlYzRGMIUtbwIQA P_0, GgoqbogGqmeqpbFlYzRGMIUtbwIQA P_1)
			{
				if (P_0.JoCVkTXVFTVYEPfgcTbaOGjMiBOk < P_1.JoCVkTXVFTVYEPfgcTbaOGjMiBOk)
				{
					return -1;
				}
				if (P_0.JoCVkTXVFTVYEPfgcTbaOGjMiBOk > P_1.JoCVkTXVFTVYEPfgcTbaOGjMiBOk)
				{
					return 1;
				}
				return 0;
			}
		}

		public struct hmfEBvhAnYkbQiokyzPkGEbknMrP
		{
			public int SuPXMNYjtglivxEPihSbzOxedcjDA;

			public int UNwAiOFjQfZDLtaXwvpAnCVLNMLB;

			public InputSource BHJKAXOHkVHadtxwKGbsRiLprGuh;

			public hmfEBvhAnYkbQiokyzPkGEbknMrP(int P_0, int P_1, InputSource P_2)
			{
				SuPXMNYjtglivxEPihSbzOxedcjDA = P_0;
				UNwAiOFjQfZDLtaXwvpAnCVLNMLB = P_1;
				BHJKAXOHkVHadtxwKGbsRiLprGuh = P_2;
			}
		}

		public enum wzqhaKiwanPPMNIyTokJUcRBndgj
		{
			Connected = 0,
			Disconnected = 1
		}

		private List<GgoqbogGqmeqpbFlYzRGMIUtbwIQA> cMUgMNmJEEyFedbWgGQJYQCFNDpv;

		private List<GgoqbogGqmeqpbFlYzRGMIUtbwIQA> CaskJRuWQrmhCekqspLNtVPvVAbr;

		public int EsUAgwGtbOrRciYoGAbhfbBKzHuxB => CaskJRuWQrmhCekqspLNtVPvVAbr.Count;

		public lfOpqzNfzduAWAkmMJpRyDYjunfl()
		{
			CaskJRuWQrmhCekqspLNtVPvVAbr = new List<GgoqbogGqmeqpbFlYzRGMIUtbwIQA>();
			cMUgMNmJEEyFedbWgGQJYQCFNDpv = new List<GgoqbogGqmeqpbFlYzRGMIUtbwIQA>();
		}

		public void kZjuHeuMEoPOKQAwxCuhBYUZpBojA(BridgedController P_0)
		{
			if (P_0 == null || P_0.sourceJoystick == null)
			{
				return;
			}
			IInputManagerJoystickPublic sourceJoystick = P_0.sourceJoystick;
			int num = InlhSdAVgMaPTdNwecdUjNTlNFEA(sourceJoystick.rewiredId, wzqhaKiwanPPMNIyTokJUcRBndgj.Connected);
			GgoqbogGqmeqpbFlYzRGMIUtbwIQA ggoqbogGqmeqpbFlYzRGMIUtbwIQA;
			if (num >= 0)
			{
				ggoqbogGqmeqpbFlYzRGMIUtbwIQA = CaskJRuWQrmhCekqspLNtVPvVAbr[num];
				ggoqbogGqmeqpbFlYzRGMIUtbwIQA.YSzVZeahALuzteqJAMQpkXjXfMcX(sourceJoystick.inputManagerId);
				P_0.sourceJoystick = new kFdQYdyGFhGUCIytnlodCQarkVpy(sourceJoystick, ggoqbogGqmeqpbFlYzRGMIUtbwIQA.JoCVkTXVFTVYEPfgcTbaOGjMiBOk);
				return;
			}
			num = InlhSdAVgMaPTdNwecdUjNTlNFEA(sourceJoystick.rewiredId, wzqhaKiwanPPMNIyTokJUcRBndgj.Disconnected);
			if (num >= 0)
			{
				ggoqbogGqmeqpbFlYzRGMIUtbwIQA = cMUgMNmJEEyFedbWgGQJYQCFNDpv[num];
				cMUgMNmJEEyFedbWgGQJYQCFNDpv.RemoveAt(num);
				int joCVkTXVFTVYEPfgcTbaOGjMiBOk = QVUedOTXXNMWBwHLVOJEeIwaXAPN(ggoqbogGqmeqpbFlYzRGMIUtbwIQA.JoCVkTXVFTVYEPfgcTbaOGjMiBOk);
				ggoqbogGqmeqpbFlYzRGMIUtbwIQA.JoCVkTXVFTVYEPfgcTbaOGjMiBOk = joCVkTXVFTVYEPfgcTbaOGjMiBOk;
			}
			else
			{
				ggoqbogGqmeqpbFlYzRGMIUtbwIQA = new GgoqbogGqmeqpbFlYzRGMIUtbwIQA(wMKcqCUBqpUdhaGMPwpuviBQSCEG(), sourceJoystick.inputManagerId, sourceJoystick.rewiredId, P_0.inputManagerSource);
			}
			P_0.sourceJoystick = new kFdQYdyGFhGUCIytnlodCQarkVpy(sourceJoystick, ggoqbogGqmeqpbFlYzRGMIUtbwIQA.JoCVkTXVFTVYEPfgcTbaOGjMiBOk);
			CaskJRuWQrmhCekqspLNtVPvVAbr.Add(ggoqbogGqmeqpbFlYzRGMIUtbwIQA);
			CaskJRuWQrmhCekqspLNtVPvVAbr.Sort(GgoqbogGqmeqpbFlYzRGMIUtbwIQA.ktfiHVeCMscmthnHovOmVnVgamfrA);
		}

		public void BkbJpjjyElMvtzqLVsrlCTcytmXL(ControllerDisconnectedEventArgs P_0)
		{
			if (P_0 != null)
			{
				int num = InlhSdAVgMaPTdNwecdUjNTlNFEA(P_0.rewiredId, wzqhaKiwanPPMNIyTokJUcRBndgj.Connected);
				if (num < 0)
				{
					Logger.LogError("Device was not in connected list! Cannot remove!");
					return;
				}
				GgoqbogGqmeqpbFlYzRGMIUtbwIQA item = CaskJRuWQrmhCekqspLNtVPvVAbr[num];
				CaskJRuWQrmhCekqspLNtVPvVAbr.RemoveAt(num);
				cMUgMNmJEEyFedbWgGQJYQCFNDpv.Add(item);
			}
		}

		public void xNrmCdqchNSdOqBzCjYFnODarpxW(int P_0, int P_1)
		{
			int num = InlhSdAVgMaPTdNwecdUjNTlNFEA(P_0, wzqhaKiwanPPMNIyTokJUcRBndgj.Connected);
			if (num >= 0)
			{
				CaskJRuWQrmhCekqspLNtVPvVAbr[num].YSzVZeahALuzteqJAMQpkXjXfMcX(P_1);
				return;
			}
			num = InlhSdAVgMaPTdNwecdUjNTlNFEA(P_0, wzqhaKiwanPPMNIyTokJUcRBndgj.Disconnected);
			if (num >= 0)
			{
				cMUgMNmJEEyFedbWgGQJYQCFNDpv[num].YSzVZeahALuzteqJAMQpkXjXfMcX(P_1);
			}
		}

		public int InlhSdAVgMaPTdNwecdUjNTlNFEA(int P_0, wzqhaKiwanPPMNIyTokJUcRBndgj P_1)
		{
			switch (P_1)
			{
			case wzqhaKiwanPPMNIyTokJUcRBndgj.Connected:
			{
				int count2 = CaskJRuWQrmhCekqspLNtVPvVAbr.Count;
				for (int j = 0; j < count2; j++)
				{
					if (CaskJRuWQrmhCekqspLNtVPvVAbr[j].OKOzrFAkGxNXarxcTjQflZgKFxqk == P_0)
					{
						return j;
					}
				}
				break;
			}
			case wzqhaKiwanPPMNIyTokJUcRBndgj.Disconnected:
			{
				int count = cMUgMNmJEEyFedbWgGQJYQCFNDpv.Count;
				for (int i = 0; i < count; i++)
				{
					if (cMUgMNmJEEyFedbWgGQJYQCFNDpv[i].OKOzrFAkGxNXarxcTjQflZgKFxqk == P_0)
					{
						return i;
					}
				}
				break;
			}
			}
			return -1;
		}

		public int iDXImmueCOvpOZAajRzLREUycAFw(int P_0, InputSource P_1, wzqhaKiwanPPMNIyTokJUcRBndgj P_2)
		{
			switch (P_2)
			{
			case wzqhaKiwanPPMNIyTokJUcRBndgj.Connected:
			{
				int count2 = CaskJRuWQrmhCekqspLNtVPvVAbr.Count;
				for (int j = 0; j < count2; j++)
				{
					if (CaskJRuWQrmhCekqspLNtVPvVAbr[j].JoCVkTXVFTVYEPfgcTbaOGjMiBOk == P_0 && CaskJRuWQrmhCekqspLNtVPvVAbr[j].bxcOPjRMsFmkKsDYLIxMKiTlBMVv == P_1)
					{
						return j;
					}
				}
				break;
			}
			case wzqhaKiwanPPMNIyTokJUcRBndgj.Disconnected:
			{
				int count = cMUgMNmJEEyFedbWgGQJYQCFNDpv.Count;
				for (int i = 0; i < count; i++)
				{
					if (cMUgMNmJEEyFedbWgGQJYQCFNDpv[i].JoCVkTXVFTVYEPfgcTbaOGjMiBOk == P_0 && cMUgMNmJEEyFedbWgGQJYQCFNDpv[i].bxcOPjRMsFmkKsDYLIxMKiTlBMVv == P_1)
					{
						return i;
					}
				}
				break;
			}
			}
			return -1;
		}

		public hmfEBvhAnYkbQiokyzPkGEbknMrP TnmoOXEyXWconYJXtKJfMpzOQmmQ(int P_0, wzqhaKiwanPPMNIyTokJUcRBndgj P_1)
		{
			if (P_1 == wzqhaKiwanPPMNIyTokJUcRBndgj.Connected)
			{
				if (P_0 < 0 || P_0 >= CaskJRuWQrmhCekqspLNtVPvVAbr.Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				return CaskJRuWQrmhCekqspLNtVPvVAbr[P_0].JsFfDYxSSoCKKnsvOGcSyiCfuFwy();
			}
			if (P_0 < 0 || P_0 >= cMUgMNmJEEyFedbWgGQJYQCFNDpv.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return cMUgMNmJEEyFedbWgGQJYQCFNDpv[P_0].JsFfDYxSSoCKKnsvOGcSyiCfuFwy();
		}

		public int EkbeTMGnvSzRkMMdlKFbHqeGNdcjB(int P_0, InputSource P_1, wzqhaKiwanPPMNIyTokJUcRBndgj P_2)
		{
			int num = iDXImmueCOvpOZAajRzLREUycAFw(P_0, P_1, P_2);
			if (num < 0)
			{
				return -1;
			}
			return P_2 switch
			{
				wzqhaKiwanPPMNIyTokJUcRBndgj.Connected => CaskJRuWQrmhCekqspLNtVPvVAbr[num].pYfqgxqpBJRwtMylIxTZNdopoMUr, 
				wzqhaKiwanPPMNIyTokJUcRBndgj.Disconnected => cMUgMNmJEEyFedbWgGQJYQCFNDpv[num].pYfqgxqpBJRwtMylIxTZNdopoMUr, 
				_ => -1, 
			};
		}

		private int QVUedOTXXNMWBwHLVOJEeIwaXAPN(int P_0)
		{
			int count = CaskJRuWQrmhCekqspLNtVPvVAbr.Count;
			for (int i = 0; i < count; i++)
			{
				if (CaskJRuWQrmhCekqspLNtVPvVAbr[i].JoCVkTXVFTVYEPfgcTbaOGjMiBOk == P_0)
				{
					return wMKcqCUBqpUdhaGMPwpuviBQSCEG();
				}
			}
			return P_0;
		}

		private int wMKcqCUBqpUdhaGMPwpuviBQSCEG()
		{
			int count = CaskJRuWQrmhCekqspLNtVPvVAbr.Count;
			int num = 0;
			while (true)
			{
				bool flag = false;
				for (int i = 0; i < count; i++)
				{
					if (CaskJRuWQrmhCekqspLNtVPvVAbr[i].JoCVkTXVFTVYEPfgcTbaOGjMiBOk == num)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				num++;
			}
			return num;
		}
	}

	private class kFdQYdyGFhGUCIytnlodCQarkVpy : IInputManagerJoystickPublic, ITryGetLocalizedName
	{
		private IInputManagerJoystickPublic fqJnCjcHpkFErUUbXdwFcztpqvWfA;

		private int dLZpSySBukSaPygQTAjRloGzehsP;

		int IInputManagerJoystickPublic.rewiredId => fqJnCjcHpkFErUUbXdwFcztpqvWfA.rewiredId;

		int IInputManagerJoystickPublic.inputManagerId => dLZpSySBukSaPygQTAjRloGzehsP;

		string IInputManagerJoystickPublic.name => fqJnCjcHpkFErUUbXdwFcztpqvWfA.name;

		long? IInputManagerJoystickPublic.systemId => fqJnCjcHpkFErUUbXdwFcztpqvWfA.systemId;

		int IInputManagerJoystickPublic.unityId => fqJnCjcHpkFErUUbXdwFcztpqvWfA.unityId;

		Guid IInputManagerJoystickPublic.instanceGuid => fqJnCjcHpkFErUUbXdwFcztpqvWfA.instanceGuid;

		Guid IInputManagerJoystickPublic.persistentGuid => Rewired_002EInterfaces_002EIInputManagerJoystickPublic_002EinstanceGuid;

		Controller.Extension IInputManagerJoystickPublic.extension => fqJnCjcHpkFErUUbXdwFcztpqvWfA.extension;

		public kFdQYdyGFhGUCIytnlodCQarkVpy(IInputManagerJoystickPublic P_0, int P_1)
		{
			fqJnCjcHpkFErUUbXdwFcztpqvWfA = P_0;
			dLZpSySBukSaPygQTAjRloGzehsP = P_1;
		}

		public void SetVibration(float amount, int motorIndex)
		{
			fqJnCjcHpkFErUUbXdwFcztpqvWfA.SetVibration(amount, motorIndex);
		}

		void IInputManagerJoystickPublic.SetVibration(float amount, int motorIndex)
		{
			//ILSpy generated this explicit interface implementation from .override directive in SetVibration
			this.SetVibration(amount, motorIndex);
		}

		public void StopVibration()
		{
			fqJnCjcHpkFErUUbXdwFcztpqvWfA.StopVibration();
		}

		void IInputManagerJoystickPublic.StopVibration()
		{
			//ILSpy generated this explicit interface implementation from .override directive in StopVibration
			this.StopVibration();
		}

		bool ITryGetLocalizedName.TryGetLocalizedName(out string value)
		{
			if (fqJnCjcHpkFErUUbXdwFcztpqvWfA is ITryGetLocalizedName tryGetLocalizedName)
			{
				return tryGetLocalizedName.TryGetLocalizedName(out value);
			}
			value = null;
			return false;
		}
	}

	[Serializable]
	private sealed class FyVTEHgwKgFKCjiJOyyDMnCiwADN
	{
		public static readonly FyVTEHgwKgFKCjiJOyyDMnCiwADN _003C_003E9 = new FyVTEHgwKgFKCjiJOyyDMnCiwADN();

		public static Func<PidVid, bool> _003C_003E9__17_0;

		internal bool vmknWFukjRKWrkHVGbbkbChdfSOn(PidVid P_0)
		{
			return false;
		}
	}

	private sealed class mfFVHjDdwbDlkWqAhEyACGFwHeUx
	{
		public int NQShyERBQcpPgjcESHsYXYJSGCwj;

		internal int cvjDJzCJebjCqoDVwhkRpoOenEaR()
		{
			return NQShyERBQcpPgjcESHsYXYJSGCwj++;
		}
	}

	private bool mwqJrCfOVnZgZwyfEIrsdSjpCduT;

	private sfTKOSJHTxLKQElXFevvOTjQPpQt SbpWtydGHkgqciwTkwZbKlTtnMPhA;

	private IndexedDictionary<int, PlatformInputManager> ruXBvXpfgaloYVYIugqDNlnOoWyV;

	private lfOpqzNfzduAWAkmMJpRyDYjunfl azvXqxwJhseOeQJkyZLzjMrYehvCA;

	private Action<int, ControllerDataUpdater> gyBvZUSDTScwWcxkcyCTmHNkGCZSA;

	private WindowsStandalonePrimaryInputSource ipdHcawJUlmaLUeqoNwaaNHVxcOj;

	private PlatformInputManager LZXKNfHZifZSzpWbQegJSeGFAArG;

	private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> CXNVnwZOBPdGnqgfRYXiTcXhKiXx;

	private Func<int> bpItCZQfQzZOxeIfyeSPJEbsxlyc;

	private Func<PidVid, bool> bKkxJFSwdwIKeyDcZeWsIjhQNBgF;

	bool INativePlatformHelper.isApplicationFocused
	{
		get
		{
			IntPtr intPtr = KQKvYsAXvDlLWOZXkMKdMDaTTekW.AibyfVejWlavgkstvckwcMQlHaQgb();
			IntPtr intPtr2 = KQKvYsAXvDlLWOZXkMKdMDaTTekW.dYTsadZkMhizYZtWgRTZblGzQsAK();
			if (intPtr2 != IntPtr.Zero)
			{
				return intPtr == intPtr2;
			}
			return false;
		}
	}

	[CustomObfuscation(rename = false)]
	int PlatformInputManager.deviceCount => azvXqxwJhseOeQJkyZLzjMrYehvCA.EsUAgwGtbOrRciYoGAbhfbBKzHuxB;

	[CustomObfuscation(rename = false)]
	PlatformInputManager PlatformInputManager.primaryInputManager => LZXKNfHZifZSzpWbQegJSeGFAArG;

	[CustomObfuscation(rename = false)]
	IInputSource PlatformInputManager.inputSource => LZXKNfHZifZSzpWbQegJSeGFAArG.inputSource;

	[CustomObfuscation(rename = false)]
	InputSource PlatformInputManager.inputSourceType
	{
		get
		{
			if (LZXKNfHZifZSzpWbQegJSeGFAArG == null)
			{
				return InputSource.None;
			}
			return LZXKNfHZifZSzpWbQegJSeGFAArG.inputSourceType;
		}
	}

	public wDUrumWuOqINTrddWncdMKuNHzcq(ConfigVars P_0, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_1, Func<int> P_2)
	{
		try
		{
			ipdHcawJUlmaLUeqoNwaaNHVxcOj = P_0.windowsStandalonePrimaryInputSource;
			bKkxJFSwdwIKeyDcZeWsIjhQNBgF = FyVTEHgwKgFKCjiJOyyDMnCiwADN._003C_003E9.vmknWFukjRKWrkHVGbbkbChdfSOn;
			bool flag = UnityTools.platform == Platform.WindowsAppStore || UnityTools.platform == Platform.Windows81Store || UnityTools.platform == Platform.WindowsPhone8;
			bool flag2 = UnityTools.platform == Platform.Windows && (ipdHcawJUlmaLUeqoNwaaNHVxcOj == WindowsStandalonePrimaryInputSource.DirectInput || ipdHcawJUlmaLUeqoNwaaNHVxcOj == WindowsStandalonePrimaryInputSource.RawInput);
			hffAeeWHwYDDOcXNFMampSOQKLMy hffAeeWHwYDDOcXNFMampSOQKLMy2 = hffAeeWHwYDDOcXNFMampSOQKLMy.None;
			if (flag2)
			{
				hffAeeWHwYDDOcXNFMampSOQKLMy2 = (P_0.GetPlatformVar_useWindowsGamingInput() ? hffAeeWHwYDDOcXNFMampSOQKLMy.WindowsGamingInput : (P_0.useXInput ? hffAeeWHwYDDOcXNFMampSOQKLMy.XInput : hffAeeWHwYDDOcXNFMampSOQKLMy.None));
			}
			bool flag3 = hffAeeWHwYDDOcXNFMampSOQKLMy2 == hffAeeWHwYDDOcXNFMampSOQKLMy.WindowsGamingInput || hffAeeWHwYDDOcXNFMampSOQKLMy2 == hffAeeWHwYDDOcXNFMampSOQKLMy.XInput || ipdHcawJUlmaLUeqoNwaaNHVxcOj == WindowsStandalonePrimaryInputSource.XInput || ipdHcawJUlmaLUeqoNwaaNHVxcOj == WindowsStandalonePrimaryInputSource.WindowsGamingInput;
			CXNVnwZOBPdGnqgfRYXiTcXhKiXx = P_1;
			bpItCZQfQzZOxeIfyeSPJEbsxlyc = P_2;
			bool flag4 = false;
			ruXBvXpfgaloYVYIugqDNlnOoWyV = new IndexedDictionary<int, PlatformInputManager>();
			PlatformInputManager platformInputManager = null;
			if (UnityTools.platform != Platform.WindowsAppStore)
			{
				try
				{
					FwvuhjisMNfwRNPCnXxbQzkrWKy.KzanHXbflnAqADtLFigURXhKxzaZ(flag3);
				}
				catch (Exception ex)
				{
					OnDestroy();
					Logger.LogWarning("Unable to initialize input source!\n" + ex.Message);
					throw;
				}
			}
			if (flag2)
			{
				switch (hffAeeWHwYDDOcXNFMampSOQKLMy2)
				{
				case hffAeeWHwYDDOcXNFMampSOQKLMy.XInput:
					if (wkxGetwmxXpMnMScFgdfdwsfKthc(P_0, false, out platformInputManager))
					{
						flag4 = true;
					}
					else
					{
						P_0.useXInput = false;
					}
					break;
				case hffAeeWHwYDDOcXNFMampSOQKLMy.WindowsGamingInput:
					if (harKIObXeuymYhpRCYKfYESrBpZy(P_0, false, out platformInputManager))
					{
						break;
					}
					P_0.SetPlatformVar_useWindowsGamingInput(value: false);
					if (P_0.useXInput && !flag4)
					{
						Logger.Log("Attempting to fallback to XInput...");
						if (wkxGetwmxXpMnMScFgdfdwsfKthc(P_0, false, out platformInputManager))
						{
							flag4 = true;
							Logger.Log("XInput initialized.");
						}
						else
						{
							P_0.useXInput = false;
						}
					}
					break;
				}
			}
			if (flag)
			{
				if (!flag4 && !wkxGetwmxXpMnMScFgdfdwsfKthc(P_0, true, out LZXKNfHZifZSzpWbQegJSeGFAArG))
				{
					throw new Exception();
				}
			}
			else if (UnityTools.platform != Platform.WindowsAppStore)
			{
				SbpWtydGHkgqciwTkwZbKlTtnMPhA = new sfTKOSJHTxLKQElXFevvOTjQPpQt();
				bool flag5 = false;
				if (ipdHcawJUlmaLUeqoNwaaNHVxcOj == WindowsStandalonePrimaryInputSource.DirectInput)
				{
					flag5 = vEiGFLkppeSfWBLZYWugEeEiBUPCA(P_0, SbpWtydGHkgqciwTkwZbKlTtnMPhA, platformInputManager as DDWjNWYAVnuAprLJBexwaxtfkBnQA);
					if (!flag5)
					{
						Logger.Log("Attempting to fallback to Raw Input...");
						flag5 = QNpUKKPGTKSnZeOSFCISIfIiWEZn(P_0, SbpWtydGHkgqciwTkwZbKlTtnMPhA, platformInputManager as DDWjNWYAVnuAprLJBexwaxtfkBnQA);
						if (flag5)
						{
							P_0.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.RawInput;
							ipdHcawJUlmaLUeqoNwaaNHVxcOj = P_0.windowsStandalonePrimaryInputSource;
							Logger.Log("Raw Input initialized.");
						}
					}
				}
				else if (ipdHcawJUlmaLUeqoNwaaNHVxcOj == WindowsStandalonePrimaryInputSource.RawInput)
				{
					flag5 = QNpUKKPGTKSnZeOSFCISIfIiWEZn(P_0, SbpWtydGHkgqciwTkwZbKlTtnMPhA, platformInputManager as DDWjNWYAVnuAprLJBexwaxtfkBnQA);
					if (!flag5)
					{
						Logger.Log("Attempting to fallback to Direct Input...");
						flag5 = vEiGFLkppeSfWBLZYWugEeEiBUPCA(P_0, SbpWtydGHkgqciwTkwZbKlTtnMPhA, platformInputManager as DDWjNWYAVnuAprLJBexwaxtfkBnQA);
						if (flag5)
						{
							P_0.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.DirectInput;
							ipdHcawJUlmaLUeqoNwaaNHVxcOj = P_0.windowsStandalonePrimaryInputSource;
							Logger.Log("Direct Input initialized.");
						}
					}
				}
				else if (ipdHcawJUlmaLUeqoNwaaNHVxcOj == WindowsStandalonePrimaryInputSource.XInput)
				{
					P_0.SetPlatformVar_useWindowsGamingInput(value: false);
					flag5 = wkxGetwmxXpMnMScFgdfdwsfKthc(P_0, true, out LZXKNfHZifZSzpWbQegJSeGFAArG);
					flag4 = flag5;
					if (flag5)
					{
						dHmRXDJoSeNFGVVcglhPbcpodbeZ(P_0, SbpWtydGHkgqciwTkwZbKlTtnMPhA);
					}
					else
					{
						P_0.useXInput = false;
						Logger.Log("Attempting to fallback to Raw Input...");
						flag5 = QNpUKKPGTKSnZeOSFCISIfIiWEZn(P_0, SbpWtydGHkgqciwTkwZbKlTtnMPhA, null);
						if (flag5)
						{
							P_0.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.RawInput;
							ipdHcawJUlmaLUeqoNwaaNHVxcOj = P_0.windowsStandalonePrimaryInputSource;
							Logger.Log("Raw Input initialized.");
						}
					}
				}
				else if (ipdHcawJUlmaLUeqoNwaaNHVxcOj == WindowsStandalonePrimaryInputSource.WindowsGamingInput)
				{
					bool flag6 = true;
					flag5 = harKIObXeuymYhpRCYKfYESrBpZy(P_0, true, out LZXKNfHZifZSzpWbQegJSeGFAArG);
					if (!flag5)
					{
						P_0.SetPlatformVar_useWindowsGamingInput(value: false);
						if (P_0.useXInput && !flag4)
						{
							Logger.Log("Attempting to fallback to XInput...");
							flag5 = wkxGetwmxXpMnMScFgdfdwsfKthc(P_0, true, out LZXKNfHZifZSzpWbQegJSeGFAArG);
							flag4 = flag5;
							if (flag5)
							{
								P_0.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.XInput;
								Logger.Log("XInput initialized.");
							}
							else
							{
								P_0.useXInput = false;
							}
						}
						if (!flag5)
						{
							Logger.Log("Attempting to fallback to Raw Input...");
							flag5 = QNpUKKPGTKSnZeOSFCISIfIiWEZn(P_0, SbpWtydGHkgqciwTkwZbKlTtnMPhA, null);
							if (flag5)
							{
								flag6 = false;
								P_0.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.RawInput;
								ipdHcawJUlmaLUeqoNwaaNHVxcOj = P_0.windowsStandalonePrimaryInputSource;
								Logger.Log("Raw Input initialized.");
							}
						}
					}
					if (flag5 && flag6)
					{
						dHmRXDJoSeNFGVVcglhPbcpodbeZ(P_0, SbpWtydGHkgqciwTkwZbKlTtnMPhA);
					}
				}
				if (!flag5)
				{
					throw new Exception();
				}
				SbpWtydGHkgqciwTkwZbKlTtnMPhA.xznaairANVFFyaYloAxTdaphRlvzB += WsrgOAiAAkzFWuZboPJSOVJqeQqDA;
				SbpWtydGHkgqciwTkwZbKlTtnMPhA.hbgWbHlpwcDWGLiViBalBPzBdlNjb += kTNBQtPjDGIDvFcfWliGprcGsJeSA;
			}
			if (LZXKNfHZifZSzpWbQegJSeGFAArG == null)
			{
				throw new Exception("No primary input manager could be initialized.");
			}
			gyBvZUSDTScwWcxkcyCTmHNkGCZSA = UpdateControllerData;
		}
		catch (Exception ex2)
		{
			OnDestroy();
			Logger.LogWarning("Unable to initialize input source!\n" + ex2.Message);
			throw;
		}
	}

	private bool vEiGFLkppeSfWBLZYWugEeEiBUPCA(ConfigVars P_0, sfTKOSJHTxLKQElXFevvOTjQPpQt P_1, DDWjNWYAVnuAprLJBexwaxtfkBnQA P_2)
	{
		wAJgxlipbMCSyzubUMIIIvlvRevYA wAJgxlipbMCSyzubUMIIIvlvRevYA2 = null;
		wXurhSlljPxwGOSKaGPOzKtqnUtm wXurhSlljPxwGOSKaGPOzKtqnUtm2 = null;
		try
		{
			wAJgxlipbMCSyzubUMIIIvlvRevYA2 = new wAJgxlipbMCSyzubUMIIIvlvRevYA(P_0, null, null, null, false, P_0.GetPlatformVar_useNativeMouse(), P_0.GetPlatformVar_useNativeKeyboard(), P_0.GetPlatformVar_useEnhancedDeviceSupport());
			wXurhSlljPxwGOSKaGPOzKtqnUtm2 = (wXurhSlljPxwGOSKaGPOzKtqnUtm)(LZXKNfHZifZSzpWbQegJSeGFAArG = new wXurhSlljPxwGOSKaGPOzKtqnUtm(P_0.updateLoop, P_2, P_1.pZKmIspXNDWgFcvoFKDrZtQYBPAH, CXNVnwZOBPdGnqgfRYXiTcXhKiXx, bpItCZQfQzZOxeIfyeSPJEbsxlyc));
			ruXBvXpfgaloYVYIugqDNlnOoWyV.Add(5, wAJgxlipbMCSyzubUMIIIvlvRevYA2);
			ruXBvXpfgaloYVYIugqDNlnOoWyV.Add(1, LZXKNfHZifZSzpWbQegJSeGFAArG);
			P_1.ApnQUOZcBTcxrbfTIjJXFTyaNxFBB += wAJgxlipbMCSyzubUMIIIvlvRevYA2.RqSdaohHjoPxNvQkkpntdFkVCrbzA;
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.DeviceConnectedEvent += FphSwmvjULdLPLFSjMbtxfkqIgVr;
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.DeviceDisconnectedEvent += HdjqiUJBgFhkuDmYylnyhVUdzbyC;
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.UpdateControllerInfoEvent += kzzkRWvKyAGoQmurnApTefPNWPPQA;
			wXurhSlljPxwGOSKaGPOzKtqnUtm2.DeviceConnectedEvent += FphSwmvjULdLPLFSjMbtxfkqIgVr;
			wXurhSlljPxwGOSKaGPOzKtqnUtm2.DeviceDisconnectedEvent += HdjqiUJBgFhkuDmYylnyhVUdzbyC;
			wXurhSlljPxwGOSKaGPOzKtqnUtm2.UpdateControllerInfoEvent += kzzkRWvKyAGoQmurnApTefPNWPPQA;
			return true;
		}
		catch (Exception)
		{
			wXurhSlljPxwGOSKaGPOzKtqnUtm2?.OnDestroy();
			wAJgxlipbMCSyzubUMIIIvlvRevYA2?.OnDestroy();
			Logger.LogWarning("Unable to initialize Direct Input! ");
		}
		return false;
	}

	private bool QNpUKKPGTKSnZeOSFCISIfIiWEZn(ConfigVars P_0, sfTKOSJHTxLKQElXFevvOTjQPpQt P_1, DDWjNWYAVnuAprLJBexwaxtfkBnQA P_2)
	{
		wAJgxlipbMCSyzubUMIIIvlvRevYA wAJgxlipbMCSyzubUMIIIvlvRevYA2 = null;
		try
		{
			wAJgxlipbMCSyzubUMIIIvlvRevYA2 = new wAJgxlipbMCSyzubUMIIIvlvRevYA(P_0, P_2, CXNVnwZOBPdGnqgfRYXiTcXhKiXx, bpItCZQfQzZOxeIfyeSPJEbsxlyc, true, P_0.GetPlatformVar_useNativeMouse(), P_0.GetPlatformVar_useNativeKeyboard(), P_0.GetPlatformVar_useEnhancedDeviceSupport());
			ruXBvXpfgaloYVYIugqDNlnOoWyV.Add(5, wAJgxlipbMCSyzubUMIIIvlvRevYA2);
			P_1.ApnQUOZcBTcxrbfTIjJXFTyaNxFBB += wAJgxlipbMCSyzubUMIIIvlvRevYA2.RqSdaohHjoPxNvQkkpntdFkVCrbzA;
			LZXKNfHZifZSzpWbQegJSeGFAArG = wAJgxlipbMCSyzubUMIIIvlvRevYA2;
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.DeviceConnectedEvent += FphSwmvjULdLPLFSjMbtxfkqIgVr;
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.DeviceDisconnectedEvent += HdjqiUJBgFhkuDmYylnyhVUdzbyC;
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.UpdateControllerInfoEvent += kzzkRWvKyAGoQmurnApTefPNWPPQA;
			return true;
		}
		catch (Exception)
		{
			Logger.LogWarning("Unable to initialize Raw Input! This error can be caused by running Unity sandboxed.");
			wAJgxlipbMCSyzubUMIIIvlvRevYA2?.OnDestroy();
		}
		return false;
	}

	private bool dHmRXDJoSeNFGVVcglhPbcpodbeZ(ConfigVars P_0, sfTKOSJHTxLKQElXFevvOTjQPpQt P_1)
	{
		bool platformVar_useNativeMouse = P_0.GetPlatformVar_useNativeMouse();
		bool platformVar_useNativeKeyboard = P_0.GetPlatformVar_useNativeKeyboard();
		if (!platformVar_useNativeMouse && !platformVar_useNativeKeyboard)
		{
			return false;
		}
		wAJgxlipbMCSyzubUMIIIvlvRevYA wAJgxlipbMCSyzubUMIIIvlvRevYA2 = null;
		try
		{
			wAJgxlipbMCSyzubUMIIIvlvRevYA2 = new wAJgxlipbMCSyzubUMIIIvlvRevYA(P_0, null, null, null, false, platformVar_useNativeMouse, platformVar_useNativeKeyboard, P_0.GetPlatformVar_useEnhancedDeviceSupport());
			P_1.ApnQUOZcBTcxrbfTIjJXFTyaNxFBB += wAJgxlipbMCSyzubUMIIIvlvRevYA2.RqSdaohHjoPxNvQkkpntdFkVCrbzA;
			ruXBvXpfgaloYVYIugqDNlnOoWyV.Add(5, wAJgxlipbMCSyzubUMIIIvlvRevYA2);
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.DeviceConnectedEvent += FphSwmvjULdLPLFSjMbtxfkqIgVr;
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.DeviceDisconnectedEvent += HdjqiUJBgFhkuDmYylnyhVUdzbyC;
			wAJgxlipbMCSyzubUMIIIvlvRevYA2.UpdateControllerInfoEvent += kzzkRWvKyAGoQmurnApTefPNWPPQA;
			return true;
		}
		catch
		{
			Logger.LogWarning("Unable to initialize Raw Input for native mouse handling! Unity mouse input will be used instead.");
			wAJgxlipbMCSyzubUMIIIvlvRevYA2?.OnDestroy();
			wAJgxlipbMCSyzubUMIIIvlvRevYA2 = null;
			return false;
		}
	}

	private bool wkxGetwmxXpMnMScFgdfdwsfKthc(ConfigVars P_0, bool P_1, out PlatformInputManager P_2)
	{
		UpdateLoopSetting updateLoop = P_0.updateLoop;
		bool flag = false;
		try
		{
			if (flag)
			{
				mfFVHjDdwbDlkWqAhEyACGFwHeUx mfFVHjDdwbDlkWqAhEyACGFwHeUx2 = new mfFVHjDdwbDlkWqAhEyACGFwHeUx();
				mfFVHjDdwbDlkWqAhEyACGFwHeUx2.NQShyERBQcpPgjcESHsYXYJSGCwj = 0;
				P_2 = new TVocCDfxMinGIOCkmldqehsxNAxhb(flag, updateLoop, CXNVnwZOBPdGnqgfRYXiTcXhKiXx, mfFVHjDdwbDlkWqAhEyACGFwHeUx2.cvjDJzCJebjCqoDVwhkRpoOenEaR, bKkxJFSwdwIKeyDcZeWsIjhQNBgF);
				ruXBvXpfgaloYVYIugqDNlnOoWyV.Add(2, P_2);
			}
			else
			{
				P_2 = new TVocCDfxMinGIOCkmldqehsxNAxhb(flag, updateLoop, CXNVnwZOBPdGnqgfRYXiTcXhKiXx, bpItCZQfQzZOxeIfyeSPJEbsxlyc, bKkxJFSwdwIKeyDcZeWsIjhQNBgF);
				ruXBvXpfgaloYVYIugqDNlnOoWyV.Add(2, P_2);
				P_2.DeviceConnectedEvent += FphSwmvjULdLPLFSjMbtxfkqIgVr;
				P_2.DeviceDisconnectedEvent += HdjqiUJBgFhkuDmYylnyhVUdzbyC;
				P_2.UpdateControllerInfoEvent += kzzkRWvKyAGoQmurnApTefPNWPPQA;
			}
			return true;
		}
		catch
		{
			P_2 = null;
			if (P_1)
			{
				Logger.LogWarning("Unable to initialize XInput!");
			}
			else if (!flag)
			{
				P_0.useXInput = false;
				for (int i = 0; i < ruXBvXpfgaloYVYIugqDNlnOoWyV.Count; i++)
				{
					if (ruXBvXpfgaloYVYIugqDNlnOoWyV[i] != null && ruXBvXpfgaloYVYIugqDNlnOoWyV[i] is LrgFZTysTrlKPEyvhAYFSHPAbEyV { CGyAAtvckTjlzgONMmncsFHBDVgHA: not null } lrgFZTysTrlKPEyvhAYFSHPAbEyV && lrgFZTysTrlKPEyvhAYFSHPAbEyV.CGyAAtvckTjlzgONMmncsFHBDVgHA.xbeYUspxexgrgzXbZDMWROAofIVw == hffAeeWHwYDDOcXNFMampSOQKLMy.XInput)
					{
						lrgFZTysTrlKPEyvhAYFSHPAbEyV.CGyAAtvckTjlzgONMmncsFHBDVgHA = null;
					}
				}
				Logger.LogWarning("Unable to initialize XInput! XInput controllers will be handled by " + ipdHcawJUlmaLUeqoNwaaNHVxcOj.ToString() + " instead. Vibration is not supported and the L/R triggers are treated as a single axis and input cannot be detected when both are pressed simultaneously. ");
			}
			return false;
		}
	}

	private bool harKIObXeuymYhpRCYKfYESrBpZy(ConfigVars P_0, bool P_1, out PlatformInputManager P_2)
	{
		_ = P_0.updateLoop;
		if (!(P_0.GetPlatformVar_useWindowsGamingInput() || P_1))
		{
			P_2 = null;
			return false;
		}
		try
		{
			P_2 = new PYsiGJTNRGZVcSgOLVziTWcLQwKC(P_0, CXNVnwZOBPdGnqgfRYXiTcXhKiXx, bpItCZQfQzZOxeIfyeSPJEbsxlyc, bKkxJFSwdwIKeyDcZeWsIjhQNBgF);
			if (P_1)
			{
				LZXKNfHZifZSzpWbQegJSeGFAArG = P_2;
			}
			ruXBvXpfgaloYVYIugqDNlnOoWyV.Add(30, P_2);
			P_2.DeviceConnectedEvent += FphSwmvjULdLPLFSjMbtxfkqIgVr;
			P_2.DeviceDisconnectedEvent += HdjqiUJBgFhkuDmYylnyhVUdzbyC;
			P_2.UpdateControllerInfoEvent += kzzkRWvKyAGoQmurnApTefPNWPPQA;
			return true;
		}
		catch (Exception)
		{
			P_2 = null;
			if (!P_1)
			{
				P_0.SetPlatformVar_useWindowsGamingInput(value: false);
				for (int i = 0; i < ruXBvXpfgaloYVYIugqDNlnOoWyV.Count; i++)
				{
					if (ruXBvXpfgaloYVYIugqDNlnOoWyV[i] != null && ruXBvXpfgaloYVYIugqDNlnOoWyV[i] is LrgFZTysTrlKPEyvhAYFSHPAbEyV { CGyAAtvckTjlzgONMmncsFHBDVgHA: not null } lrgFZTysTrlKPEyvhAYFSHPAbEyV && lrgFZTysTrlKPEyvhAYFSHPAbEyV.CGyAAtvckTjlzgONMmncsFHBDVgHA.xbeYUspxexgrgzXbZDMWROAofIVw == hffAeeWHwYDDOcXNFMampSOQKLMy.WindowsGamingInput)
					{
						lrgFZTysTrlKPEyvhAYFSHPAbEyV.CGyAAtvckTjlzgONMmncsFHBDVgHA = null;
					}
				}
			}
			Logger.LogWarning("Unable to initialize Windows Gaming Input! ");
			return false;
		}
	}

	[CustomObfuscation(rename = false)]
	public override void Initialize()
	{
		mwqJrCfOVnZgZwyfEIrsdSjpCduT = true;
		azvXqxwJhseOeQJkyZLzjMrYehvCA = new lfOpqzNfzduAWAkmMJpRyDYjunfl();
		for (int i = 0; i < ruXBvXpfgaloYVYIugqDNlnOoWyV.Count; i++)
		{
			ruXBvXpfgaloYVYIugqDNlnOoWyV[i].Initialize();
		}
	}

	public virtual void RUwDvNFhKQirvFJnTHwKThuctCXOA(UpdateLoopType P_0)
	{
		for (int i = 0; i < ruXBvXpfgaloYVYIugqDNlnOoWyV.Count; i++)
		{
			ruXBvXpfgaloYVYIugqDNlnOoWyV[i].Update(P_0);
		}
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		for (int num = ruXBvXpfgaloYVYIugqDNlnOoWyV.Count - 1; num >= 0; num--)
		{
			ruXBvXpfgaloYVYIugqDNlnOoWyV[num].OnDestroy();
		}
		ruXBvXpfgaloYVYIugqDNlnOoWyV.Clear();
		if (SbpWtydGHkgqciwTkwZbKlTtnMPhA != null)
		{
			SbpWtydGHkgqciwTkwZbKlTtnMPhA.fgfETyhHaFUoCIBmVzZjipzQqILDb();
			SbpWtydGHkgqciwTkwZbKlTtnMPhA = null;
		}
		FwvuhjisMNfwRNPCnXxbQzkrWKy.wFjbdaEaLsAZFmfjExaiwfzfniukA();
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return gyBvZUSDTScwWcxkcyCTmHNkGCZSA;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int controllerId, ControllerDataUpdater data)
	{
		ruXBvXpfgaloYVYIugqDNlnOoWyV.GetValue((int)data.source).UpdateControllerData(azvXqxwJhseOeQJkyZLzjMrYehvCA.EkbeTMGnvSzRkMMdlKFbHqeGNdcjB(controllerId, data.source, lfOpqzNfzduAWAkmMJpRyDYjunfl.wzqhaKiwanPPMNIyTokJUcRBndgj.Connected), data);
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceConnected()
	{
	}

	[CustomObfuscation(rename = false)]
	public override void SystemDeviceDisconnected()
	{
	}

	[CustomObfuscation(rename = false)]
	public override void SetUnityJoystickId(int joystickId, int unityJoystickId)
	{
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedMouseSource GetUnifiedMouseSource()
	{
		for (int i = 0; i < ruXBvXpfgaloYVYIugqDNlnOoWyV.Count; i++)
		{
			IUnifiedMouseSource unifiedMouseSource = ruXBvXpfgaloYVYIugqDNlnOoWyV[i].GetUnifiedMouseSource();
			if (unifiedMouseSource != null)
			{
				return unifiedMouseSource;
			}
		}
		return null;
	}

	[CustomObfuscation(rename = false)]
	public override IUnifiedKeyboardSource GetUnifiedKeyboardSource()
	{
		for (int i = 0; i < ruXBvXpfgaloYVYIugqDNlnOoWyV.Count; i++)
		{
			IUnifiedKeyboardSource unifiedKeyboardSource = ruXBvXpfgaloYVYIugqDNlnOoWyV[i].GetUnifiedKeyboardSource();
			if (unifiedKeyboardSource != null)
			{
				return unifiedKeyboardSource;
			}
		}
		return null;
	}

	private void FphSwmvjULdLPLFSjMbtxfkqIgVr(BridgedController P_0)
	{
		if (P_0 != null)
		{
			azvXqxwJhseOeQJkyZLzjMrYehvCA.kZjuHeuMEoPOKQAwxCuhBYUZpBojA(P_0);
			if (_DeviceConnectedEvent != null)
			{
				_DeviceConnectedEvent(P_0);
			}
		}
	}

	private void HdjqiUJBgFhkuDmYylnyhVUdzbyC(ControllerDisconnectedEventArgs P_0)
	{
		if (P_0 != null)
		{
			azvXqxwJhseOeQJkyZLzjMrYehvCA.BkbJpjjyElMvtzqLVsrlCTcytmXL(P_0);
			if (_DeviceDisconnectedEvent != null)
			{
				_DeviceDisconnectedEvent(P_0);
			}
		}
	}

	private void WsrgOAiAAkzFWuZboPJSOVJqeQqDA(EventArgs P_0)
	{
		if (mwqJrCfOVnZgZwyfEIrsdSjpCduT)
		{
			for (int i = 0; i < ruXBvXpfgaloYVYIugqDNlnOoWyV.Count; i++)
			{
				ruXBvXpfgaloYVYIugqDNlnOoWyV[i].SystemDeviceConnected();
			}
		}
	}

	private void kTNBQtPjDGIDvFcfWliGprcGsJeSA(EventArgs P_0)
	{
		if (mwqJrCfOVnZgZwyfEIrsdSjpCduT)
		{
			for (int i = 0; i < ruXBvXpfgaloYVYIugqDNlnOoWyV.Count; i++)
			{
				ruXBvXpfgaloYVYIugqDNlnOoWyV[i].SystemDeviceDisconnected();
			}
		}
	}

	private void kzzkRWvKyAGoQmurnApTefPNWPPQA(UpdateControllerInfoEventArgs P_0)
	{
		if (P_0 == null || P_0.sourceJoystick == null)
		{
			return;
		}
		azvXqxwJhseOeQJkyZLzjMrYehvCA.xNrmCdqchNSdOqBzCjYFnODarpxW(P_0.sourceJoystick.rewiredId, P_0.sourceJoystick.inputManagerId);
		lfOpqzNfzduAWAkmMJpRyDYjunfl.wzqhaKiwanPPMNIyTokJUcRBndgj wzqhaKiwanPPMNIyTokJUcRBndgj = lfOpqzNfzduAWAkmMJpRyDYjunfl.wzqhaKiwanPPMNIyTokJUcRBndgj.Connected;
		int num = azvXqxwJhseOeQJkyZLzjMrYehvCA.InlhSdAVgMaPTdNwecdUjNTlNFEA(P_0.sourceJoystick.rewiredId, wzqhaKiwanPPMNIyTokJUcRBndgj);
		if (num < 0)
		{
			wzqhaKiwanPPMNIyTokJUcRBndgj = lfOpqzNfzduAWAkmMJpRyDYjunfl.wzqhaKiwanPPMNIyTokJUcRBndgj.Disconnected;
			num = azvXqxwJhseOeQJkyZLzjMrYehvCA.InlhSdAVgMaPTdNwecdUjNTlNFEA(P_0.sourceJoystick.rewiredId, wzqhaKiwanPPMNIyTokJUcRBndgj);
		}
		if (num >= 0)
		{
			lfOpqzNfzduAWAkmMJpRyDYjunfl.hmfEBvhAnYkbQiokyzPkGEbknMrP hmfEBvhAnYkbQiokyzPkGEbknMrP = azvXqxwJhseOeQJkyZLzjMrYehvCA.TnmoOXEyXWconYJXtKJfMpzOQmmQ(num, wzqhaKiwanPPMNIyTokJUcRBndgj);
			if (_UpdateControllerInfoEvent != null)
			{
				_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(new kFdQYdyGFhGUCIytnlodCQarkVpy(P_0.sourceJoystick, hmfEBvhAnYkbQiokyzPkGEbknMrP.SuPXMNYjtglivxEPihSbzOxedcjDA)));
			}
		}
	}
}
