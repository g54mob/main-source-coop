using System;
using System.Collections.Generic;
using Rewired;
using Rewired.Config;
using Rewired.Data;
using Rewired.Interfaces;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Classes.Data;

internal class PRPvEnHMXBtNOrkiBYneErPlRGdP : PlatformInputManager, INativePlatformHelper
{
	private class UfcGukYTvzxBtvGJSfvcMkQErsqo
	{
		private class UMrTMrHNTeROVFmwAKaSmttEGAMB
		{
			public int intzOazUmOAoEfDNHbTtFSTaYFPbA;

			public int YkkTVvFxmZCLEgjfHIxJoZVBFHVV;

			public int NEauRSlStYeerBqlmDuMMKDmjhbe;

			public InputSource upgpTawNaXwDgrlbzhRUvCrQIdOc;

			public UMrTMrHNTeROVFmwAKaSmttEGAMB(int P_0, int P_1, int P_2, InputSource P_3)
			{
				intzOazUmOAoEfDNHbTtFSTaYFPbA = P_0;
				YkkTVvFxmZCLEgjfHIxJoZVBFHVV = P_1;
				NEauRSlStYeerBqlmDuMMKDmjhbe = P_2;
				upgpTawNaXwDgrlbzhRUvCrQIdOc = P_3;
			}

			public void mPVLsAWcaTKfbIPORRXexWhffTDm(int P_0)
			{
				YkkTVvFxmZCLEgjfHIxJoZVBFHVV = P_0;
			}

			public itLJIiIxgabLxDPAzaZWoHcIInuBA aoSvfphhZiFPEDahxShDmlxhlhrAb()
			{
				return new itLJIiIxgabLxDPAzaZWoHcIInuBA(intzOazUmOAoEfDNHbTtFSTaYFPbA, YkkTVvFxmZCLEgjfHIxJoZVBFHVV, upgpTawNaXwDgrlbzhRUvCrQIdOc);
			}

			public static int YqdFXOombKDixtZJTaBFSoqHSPMP(UMrTMrHNTeROVFmwAKaSmttEGAMB P_0, UMrTMrHNTeROVFmwAKaSmttEGAMB P_1)
			{
				if (P_0.intzOazUmOAoEfDNHbTtFSTaYFPbA < P_1.intzOazUmOAoEfDNHbTtFSTaYFPbA)
				{
					return -1;
				}
				if (P_0.intzOazUmOAoEfDNHbTtFSTaYFPbA > P_1.intzOazUmOAoEfDNHbTtFSTaYFPbA)
				{
					return 1;
				}
				return 0;
			}
		}

		public struct itLJIiIxgabLxDPAzaZWoHcIInuBA
		{
			public int intzOazUmOAoEfDNHbTtFSTaYFPbA;

			public int YkkTVvFxmZCLEgjfHIxJoZVBFHVV;

			public InputSource upgpTawNaXwDgrlbzhRUvCrQIdOc;

			public itLJIiIxgabLxDPAzaZWoHcIInuBA(int P_0, int P_1, InputSource P_2)
			{
				intzOazUmOAoEfDNHbTtFSTaYFPbA = P_0;
				YkkTVvFxmZCLEgjfHIxJoZVBFHVV = P_1;
				upgpTawNaXwDgrlbzhRUvCrQIdOc = P_2;
			}
		}

		public enum VsOZJFSbGqhHqIvTQToBiabccXTs
		{
			Connected = 0,
			Disconnected = 1
		}

		private List<UMrTMrHNTeROVFmwAKaSmttEGAMB> mhIYgMMoDpIfJGWyFPCUdMRuPzjqA;

		private List<UMrTMrHNTeROVFmwAKaSmttEGAMB> WewrUgCbyaCOwmERNjRvchcJTfAn;

		public int enDrVyzOZObHnPeyXXEwUKXIvMxs => WewrUgCbyaCOwmERNjRvchcJTfAn.Count;

		public UfcGukYTvzxBtvGJSfvcMkQErsqo()
		{
			WewrUgCbyaCOwmERNjRvchcJTfAn = new List<UMrTMrHNTeROVFmwAKaSmttEGAMB>();
			mhIYgMMoDpIfJGWyFPCUdMRuPzjqA = new List<UMrTMrHNTeROVFmwAKaSmttEGAMB>();
		}

		public void rjuvkDZjDlwGtHMWPlbpHnyytTBA(BridgedController P_0)
		{
			if (P_0 == null || P_0.sourceJoystick == null)
			{
				return;
			}
			IInputManagerJoystickPublic sourceJoystick = P_0.sourceJoystick;
			int num = uWrVYCSALeSsGsvUfnkTZtSRlidv(sourceJoystick.rewiredId, VsOZJFSbGqhHqIvTQToBiabccXTs.Connected);
			UMrTMrHNTeROVFmwAKaSmttEGAMB uMrTMrHNTeROVFmwAKaSmttEGAMB;
			if (num >= 0)
			{
				uMrTMrHNTeROVFmwAKaSmttEGAMB = WewrUgCbyaCOwmERNjRvchcJTfAn[num];
				uMrTMrHNTeROVFmwAKaSmttEGAMB.mPVLsAWcaTKfbIPORRXexWhffTDm(sourceJoystick.inputManagerId);
				P_0.sourceJoystick = new SNoXgxUWvrpIAwUilSiZapIMVjjl(sourceJoystick, uMrTMrHNTeROVFmwAKaSmttEGAMB.intzOazUmOAoEfDNHbTtFSTaYFPbA);
				return;
			}
			num = uWrVYCSALeSsGsvUfnkTZtSRlidv(sourceJoystick.rewiredId, VsOZJFSbGqhHqIvTQToBiabccXTs.Disconnected);
			if (num >= 0)
			{
				uMrTMrHNTeROVFmwAKaSmttEGAMB = mhIYgMMoDpIfJGWyFPCUdMRuPzjqA[num];
				mhIYgMMoDpIfJGWyFPCUdMRuPzjqA.RemoveAt(num);
				int intzOazUmOAoEfDNHbTtFSTaYFPbA = nmqJXoraksenbfLDKoyfeNgVuGIp(uMrTMrHNTeROVFmwAKaSmttEGAMB.intzOazUmOAoEfDNHbTtFSTaYFPbA);
				uMrTMrHNTeROVFmwAKaSmttEGAMB.intzOazUmOAoEfDNHbTtFSTaYFPbA = intzOazUmOAoEfDNHbTtFSTaYFPbA;
			}
			else
			{
				uMrTMrHNTeROVFmwAKaSmttEGAMB = new UMrTMrHNTeROVFmwAKaSmttEGAMB(nmqJXoraksenbfLDKoyfeNgVuGIp(), sourceJoystick.inputManagerId, sourceJoystick.rewiredId, P_0.inputManagerSource);
			}
			P_0.sourceJoystick = new SNoXgxUWvrpIAwUilSiZapIMVjjl(sourceJoystick, uMrTMrHNTeROVFmwAKaSmttEGAMB.intzOazUmOAoEfDNHbTtFSTaYFPbA);
			WewrUgCbyaCOwmERNjRvchcJTfAn.Add(uMrTMrHNTeROVFmwAKaSmttEGAMB);
			WewrUgCbyaCOwmERNjRvchcJTfAn.Sort(UMrTMrHNTeROVFmwAKaSmttEGAMB.YqdFXOombKDixtZJTaBFSoqHSPMP);
		}

		public void SlsKNXnJEuvgRXQdMjsJWyVdKWsc(ControllerDisconnectedEventArgs P_0)
		{
			if (P_0 != null)
			{
				int num = uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0.rewiredId, VsOZJFSbGqhHqIvTQToBiabccXTs.Connected);
				if (num < 0)
				{
					Logger.LogError("Device was not in connected list! Cannot remove!");
					return;
				}
				UMrTMrHNTeROVFmwAKaSmttEGAMB item = WewrUgCbyaCOwmERNjRvchcJTfAn[num];
				WewrUgCbyaCOwmERNjRvchcJTfAn.RemoveAt(num);
				mhIYgMMoDpIfJGWyFPCUdMRuPzjqA.Add(item);
			}
		}

		public void itPrcTStUQeohuNHGbLIOuyYBDCc(int P_0, int P_1)
		{
			int num = uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0, VsOZJFSbGqhHqIvTQToBiabccXTs.Connected);
			if (num >= 0)
			{
				WewrUgCbyaCOwmERNjRvchcJTfAn[num].mPVLsAWcaTKfbIPORRXexWhffTDm(P_1);
				return;
			}
			num = uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0, VsOZJFSbGqhHqIvTQToBiabccXTs.Disconnected);
			if (num >= 0)
			{
				mhIYgMMoDpIfJGWyFPCUdMRuPzjqA[num].mPVLsAWcaTKfbIPORRXexWhffTDm(P_1);
			}
		}

		public bool cmurkWKHdSBEPcmDFNPQgPvQNrQCb(int P_0, VsOZJFSbGqhHqIvTQToBiabccXTs P_1)
		{
			if (uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0, P_1) < 0)
			{
				return false;
			}
			return true;
		}

		public int uWrVYCSALeSsGsvUfnkTZtSRlidv(int P_0, VsOZJFSbGqhHqIvTQToBiabccXTs P_1)
		{
			switch (P_1)
			{
			case VsOZJFSbGqhHqIvTQToBiabccXTs.Connected:
			{
				int count2 = WewrUgCbyaCOwmERNjRvchcJTfAn.Count;
				for (int j = 0; j < count2; j++)
				{
					if (WewrUgCbyaCOwmERNjRvchcJTfAn[j].NEauRSlStYeerBqlmDuMMKDmjhbe == P_0)
					{
						return j;
					}
				}
				break;
			}
			case VsOZJFSbGqhHqIvTQToBiabccXTs.Disconnected:
			{
				int count = mhIYgMMoDpIfJGWyFPCUdMRuPzjqA.Count;
				for (int i = 0; i < count; i++)
				{
					if (mhIYgMMoDpIfJGWyFPCUdMRuPzjqA[i].NEauRSlStYeerBqlmDuMMKDmjhbe == P_0)
					{
						return i;
					}
				}
				break;
			}
			}
			return -1;
		}

		public int uWrVYCSALeSsGsvUfnkTZtSRlidv(int P_0, InputSource P_1, VsOZJFSbGqhHqIvTQToBiabccXTs P_2)
		{
			switch (P_2)
			{
			case VsOZJFSbGqhHqIvTQToBiabccXTs.Connected:
			{
				int count2 = WewrUgCbyaCOwmERNjRvchcJTfAn.Count;
				for (int j = 0; j < count2; j++)
				{
					if (WewrUgCbyaCOwmERNjRvchcJTfAn[j].intzOazUmOAoEfDNHbTtFSTaYFPbA == P_0 && WewrUgCbyaCOwmERNjRvchcJTfAn[j].upgpTawNaXwDgrlbzhRUvCrQIdOc == P_1)
					{
						return j;
					}
				}
				break;
			}
			case VsOZJFSbGqhHqIvTQToBiabccXTs.Disconnected:
			{
				int count = mhIYgMMoDpIfJGWyFPCUdMRuPzjqA.Count;
				for (int i = 0; i < count; i++)
				{
					if (mhIYgMMoDpIfJGWyFPCUdMRuPzjqA[i].intzOazUmOAoEfDNHbTtFSTaYFPbA == P_0 && mhIYgMMoDpIfJGWyFPCUdMRuPzjqA[i].upgpTawNaXwDgrlbzhRUvCrQIdOc == P_1)
					{
						return i;
					}
				}
				break;
			}
			}
			return -1;
		}

		public itLJIiIxgabLxDPAzaZWoHcIInuBA aoSvfphhZiFPEDahxShDmlxhlhrAb(int P_0, VsOZJFSbGqhHqIvTQToBiabccXTs P_1)
		{
			if (P_1 == VsOZJFSbGqhHqIvTQToBiabccXTs.Connected)
			{
				if (P_0 < 0 || P_0 >= WewrUgCbyaCOwmERNjRvchcJTfAn.Count)
				{
					throw new ArgumentOutOfRangeException();
				}
				return WewrUgCbyaCOwmERNjRvchcJTfAn[P_0].aoSvfphhZiFPEDahxShDmlxhlhrAb();
			}
			if (P_0 < 0 || P_0 >= mhIYgMMoDpIfJGWyFPCUdMRuPzjqA.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			return mhIYgMMoDpIfJGWyFPCUdMRuPzjqA[P_0].aoSvfphhZiFPEDahxShDmlxhlhrAb();
		}

		public int JKuaoczDNyjiUhPFYIKIsjLkxpur(int P_0, InputSource P_1, VsOZJFSbGqhHqIvTQToBiabccXTs P_2)
		{
			int num = uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0, P_1, P_2);
			if (num < 0)
			{
				return -1;
			}
			return P_2 switch
			{
				VsOZJFSbGqhHqIvTQToBiabccXTs.Connected => WewrUgCbyaCOwmERNjRvchcJTfAn[num].YkkTVvFxmZCLEgjfHIxJoZVBFHVV, 
				VsOZJFSbGqhHqIvTQToBiabccXTs.Disconnected => mhIYgMMoDpIfJGWyFPCUdMRuPzjqA[num].YkkTVvFxmZCLEgjfHIxJoZVBFHVV, 
				_ => -1, 
			};
		}

		private int nmqJXoraksenbfLDKoyfeNgVuGIp(int P_0)
		{
			int count = WewrUgCbyaCOwmERNjRvchcJTfAn.Count;
			for (int i = 0; i < count; i++)
			{
				if (WewrUgCbyaCOwmERNjRvchcJTfAn[i].intzOazUmOAoEfDNHbTtFSTaYFPbA == P_0)
				{
					return nmqJXoraksenbfLDKoyfeNgVuGIp();
				}
			}
			return P_0;
		}

		private int nmqJXoraksenbfLDKoyfeNgVuGIp()
		{
			int count = WewrUgCbyaCOwmERNjRvchcJTfAn.Count;
			int num = 0;
			while (true)
			{
				bool flag = false;
				for (int i = 0; i < count; i++)
				{
					if (WewrUgCbyaCOwmERNjRvchcJTfAn[i].intzOazUmOAoEfDNHbTtFSTaYFPbA == num)
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

	private class SNoXgxUWvrpIAwUilSiZapIMVjjl : IInputManagerJoystickPublic
	{
		private IInputManagerJoystickPublic MMTtbRDiEhfNsofvfEMKtHrGMMsL;

		private int ULFkUvofFMojIIdtrArDQFerGhlIA;

		public int rewiredId => MMTtbRDiEhfNsofvfEMKtHrGMMsL.rewiredId;

		public int inputManagerId => ULFkUvofFMojIIdtrArDQFerGhlIA;

		public string name => MMTtbRDiEhfNsofvfEMKtHrGMMsL.name;

		public long? systemId => MMTtbRDiEhfNsofvfEMKtHrGMMsL.systemId;

		public int unityId => MMTtbRDiEhfNsofvfEMKtHrGMMsL.unityId;

		public Guid instanceGuid => MMTtbRDiEhfNsofvfEMKtHrGMMsL.instanceGuid;

		public Guid persistentGuid => instanceGuid;

		public Controller.Extension extension => MMTtbRDiEhfNsofvfEMKtHrGMMsL.extension;

		public SNoXgxUWvrpIAwUilSiZapIMVjjl(IInputManagerJoystickPublic P_0, int P_1)
		{
			MMTtbRDiEhfNsofvfEMKtHrGMMsL = P_0;
			ULFkUvofFMojIIdtrArDQFerGhlIA = P_1;
		}

		public void SetVibration(float amount, int motorIndex)
		{
			MMTtbRDiEhfNsofvfEMKtHrGMMsL.SetVibration(amount, motorIndex);
		}

		public void StopVibration()
		{
			MMTtbRDiEhfNsofvfEMKtHrGMMsL.StopVibration();
		}
	}

	private sealed class qtWwqyCRnIKJoqKYBZXitRcIpTRb
	{
		public int oPCGMBLFhOCIhehhynhMsDGZwaTwA;

		internal int TbfRKCJPLZZVpOhlsmRQAJFnHIdD()
		{
			return oPCGMBLFhOCIhehhynhMsDGZwaTwA++;
		}
	}

	private const bool XoHQpDhKELdXzpnyreIYaVgHaXfS = false;

	private const bool RhWBYBIvdJoDlVcImQLjfDpVsUJb = false;

	private const bool GYnhgTwYzkCpUpCIifBOVOzXGAJd = false;

	private const bool MIoKlOgBBQNnjygoiBARcJUMgRGK = false;

	private const bool AtwrXnJlDmoqidwUcRzhezhuDsJc = false;

	private bool eKcWHeiuDjBZLxCtvCsgstBOqXxe;

	private object mjcAnfSZCMXFItZOfBJKoKzyYMtj;

	private IndexedDictionary<int, PlatformInputManager> GzEuxNQffpuifnZvWaVFtiPdKeuD;

	private UfcGukYTvzxBtvGJSfvcMkQErsqo eWlfdEBFAfZbDnmNZkfHdmxfWDShA;

	private Action<int, ControllerDataUpdater> kFtEhCDMsBpCscbniaVUseitMyFJ;

	private WindowsStandalonePrimaryInputSource hGdcNaNFaFYbuzhgNvQHwwCpVkSN;

	private bool PXiJmHVeQttaMnqXULEWMzYhdnfc;

	private PlatformInputManager abLEytsrMfhnEafVEifaCIJcGbQLc;

	private bool uCLEomkHolIpIvxUbGUjgswANiGy;

	private Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> XDfJqiwLYIwEbgMhAkMbksxTquAQ;

	private Func<int> WHEWdNESHgukupIXVEuPORSWLLeJ;

	[CustomObfuscation(rename = false)]
	private int counter;

	bool INativePlatformHelper.isApplicationFocused
	{
		get
		{
			IntPtr intPtr = hUfdZejvJYlOtHWdillPtEZZfcOAA.yxZfeNLhHggOFLfUuATbJUJeBghvA();
			IntPtr intPtr2 = hUfdZejvJYlOtHWdillPtEZZfcOAA.rAUvmTfVyKUAaAHERFekLGhGYfNm();
			if (intPtr2 != IntPtr.Zero)
			{
				return intPtr == intPtr2;
			}
			return false;
		}
	}

	[CustomObfuscation(rename = false)]
	public override int deviceCount => eWlfdEBFAfZbDnmNZkfHdmxfWDShA.enDrVyzOZObHnPeyXXEwUKXIvMxs;

	[CustomObfuscation(rename = false)]
	public override PlatformInputManager primaryInputManager => abLEytsrMfhnEafVEifaCIJcGbQLc;

	[CustomObfuscation(rename = false)]
	public override IInputSource inputSource => abLEytsrMfhnEafVEifaCIJcGbQLc.inputSource;

	[CustomObfuscation(rename = false)]
	public override InputSource inputSourceType
	{
		get
		{
			if (abLEytsrMfhnEafVEifaCIJcGbQLc == null)
			{
				return InputSource.None;
			}
			return abLEytsrMfhnEafVEifaCIJcGbQLc.inputSourceType;
		}
	}

	public PRPvEnHMXBtNOrkiBYneErPlRGdP(ConfigVars P_0, Func<BridgedControllerHWInfo, HardwareJoystickMap_InputManager> P_1, Func<int> P_2)
	{
		hGdcNaNFaFYbuzhgNvQHwwCpVkSN = P_0.windowsStandalonePrimaryInputSource;
		PXiJmHVeQttaMnqXULEWMzYhdnfc = P_0.useXInput;
		XDfJqiwLYIwEbgMhAkMbksxTquAQ = P_1;
		WHEWdNESHgukupIXVEuPORSWLLeJ = P_2;
		bool flag = false;
		GzEuxNQffpuifnZvWaVFtiPdKeuD = new IndexedDictionary<int, PlatformInputManager>();
		if (UnityTools.platform != Platform.WindowsAppStore)
		{
			try
			{
				BDwAsVxXXjygYaSUuZfLCpBQFMEJ.iNjZxEWgGYMxgNiUyUmpWefCBkbC();
				IMpasrWeCbdVlFXOeTtKckSykDrR mpasrWeCbdVlFXOeTtKckSykDrR = (IMpasrWeCbdVlFXOeTtKckSykDrR)(mjcAnfSZCMXFItZOfBJKoKzyYMtj = new IMpasrWeCbdVlFXOeTtKckSykDrR());
				bool flag2 = false;
				if (hGdcNaNFaFYbuzhgNvQHwwCpVkSN == WindowsStandalonePrimaryInputSource.DirectInput)
				{
					flag2 = TJZEgZkvqTuUkEcCAqBiUjeoadUd(P_0, mpasrWeCbdVlFXOeTtKckSykDrR);
					if (!flag2)
					{
						Logger.Log("Attempting to fallback to Raw Input...");
						flag2 = nNwjvkzmDQEVBFPbAONZCaCAQaeNb(P_0, mpasrWeCbdVlFXOeTtKckSykDrR);
						if (flag2)
						{
							P_0.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.RawInput;
							hGdcNaNFaFYbuzhgNvQHwwCpVkSN = P_0.windowsStandalonePrimaryInputSource;
							Logger.Log("Raw Input initialized!");
						}
					}
				}
				else if (hGdcNaNFaFYbuzhgNvQHwwCpVkSN == WindowsStandalonePrimaryInputSource.RawInput)
				{
					flag2 = nNwjvkzmDQEVBFPbAONZCaCAQaeNb(P_0, mpasrWeCbdVlFXOeTtKckSykDrR);
					if (!flag2)
					{
						Logger.Log("Attempting to fallback to Direct Input...");
						flag2 = TJZEgZkvqTuUkEcCAqBiUjeoadUd(P_0, mpasrWeCbdVlFXOeTtKckSykDrR);
						if (flag2)
						{
							P_0.windowsStandalonePrimaryInputSource = WindowsStandalonePrimaryInputSource.DirectInput;
							hGdcNaNFaFYbuzhgNvQHwwCpVkSN = P_0.windowsStandalonePrimaryInputSource;
							Logger.Log("Direct Input initialized!");
						}
					}
				}
				else if (hGdcNaNFaFYbuzhgNvQHwwCpVkSN == WindowsStandalonePrimaryInputSource.XInput)
				{
					flag2 = jjjtZCKTiJWAWlvAlsjdAvKfEMJl(P_0, false);
					if (flag2)
					{
						wvPqOkGprotCSvQGGRMBzPrBQeAA(P_0, mpasrWeCbdVlFXOeTtKckSykDrR);
					}
					flag = flag2;
				}
				if (!flag2)
				{
					throw new Exception();
				}
				mpasrWeCbdVlFXOeTtKckSykDrR.GiHRsoeRcJMqWIxwkngZkFkTcMZo += pqSAuCVZSOhHtVpLhThQGyDWlkFF;
				mpasrWeCbdVlFXOeTtKckSykDrR.JfHzQfUlnPjlIHhEBRUpfBHGlVeQ += rsjVtmZDOZqmLjDrBUCiDDiakaEu;
				for (int i = 0; i < GzEuxNQffpuifnZvWaVFtiPdKeuD.Count; i++)
				{
					PlatformInputManager platformInputManager = GzEuxNQffpuifnZvWaVFtiPdKeuD[i];
					platformInputManager.DeviceConnectedEvent += zLNKddgcwwRAduOsXseihEuykldS;
					platformInputManager.DeviceDisconnectedEvent += UEYEgChtDymqRQHvBWqvZPDBCJcY;
					platformInputManager.UpdateControllerInfoEvent += KPBhKolrWjrjwSJhosEHkXWwFWDn;
				}
			}
			catch (Exception ex)
			{
				OnDestroy();
				Logger.LogWarning("Unable to initialize input source!\n" + ex.Message);
				throw;
			}
		}
		if (!flag)
		{
			jjjtZCKTiJWAWlvAlsjdAvKfEMJl(P_0, true);
		}
		kFtEhCDMsBpCscbniaVUseitMyFJ = UpdateControllerData;
	}

	private bool TJZEgZkvqTuUkEcCAqBiUjeoadUd(ConfigVars P_0, IMpasrWeCbdVlFXOeTtKckSykDrR P_1)
	{
		rzVaxhDvygHNuxtNbJgXqYRfTSVi rzVaxhDvygHNuxtNbJgXqYRfTSVi2 = null;
		VblIOZIthKvmaclqLRxuwwkYvPom vblIOZIthKvmaclqLRxuwwkYvPom = null;
		try
		{
			rzVaxhDvygHNuxtNbJgXqYRfTSVi2 = new rzVaxhDvygHNuxtNbJgXqYRfTSVi(P_0, false, null, null, false, P_0.GetPlatformVar_useNativeMouse(), P_0.GetPlatformVar_useNativeKeyboard(), P_0.GetPlatformVar_useEnhancedDeviceSupport());
			vblIOZIthKvmaclqLRxuwwkYvPom = (VblIOZIthKvmaclqLRxuwwkYvPom)(abLEytsrMfhnEafVEifaCIJcGbQLc = new VblIOZIthKvmaclqLRxuwwkYvPom(P_0.updateLoop, PXiJmHVeQttaMnqXULEWMzYhdnfc, ((IMpasrWeCbdVlFXOeTtKckSykDrR)mjcAnfSZCMXFItZOfBJKoKzyYMtj).KwjDKNDVyFfeKbepQfteBUEEsegIA, XDfJqiwLYIwEbgMhAkMbksxTquAQ, WHEWdNESHgukupIXVEuPORSWLLeJ));
			GzEuxNQffpuifnZvWaVFtiPdKeuD.Add(5, rzVaxhDvygHNuxtNbJgXqYRfTSVi2);
			GzEuxNQffpuifnZvWaVFtiPdKeuD.Add(1, abLEytsrMfhnEafVEifaCIJcGbQLc);
			P_1.cVnDnWwFtLpDMrHuUpJUkHBVnKVf += rzVaxhDvygHNuxtNbJgXqYRfTSVi2.rsibSRRtOsuuKGDzaeOwWaQmfJdV;
			return true;
		}
		catch (Exception)
		{
			vblIOZIthKvmaclqLRxuwwkYvPom?.OnDestroy();
			rzVaxhDvygHNuxtNbJgXqYRfTSVi2?.OnDestroy();
			Logger.LogWarning("Unable to initialize Direct Input! Please see the Installation section of the documentation for information on required libraries. Documentation can be found in the menu: Window -> Rewired -> Help -> Documentation.");
		}
		return false;
	}

	private bool nNwjvkzmDQEVBFPbAONZCaCAQaeNb(ConfigVars P_0, IMpasrWeCbdVlFXOeTtKckSykDrR P_1)
	{
		rzVaxhDvygHNuxtNbJgXqYRfTSVi rzVaxhDvygHNuxtNbJgXqYRfTSVi2 = null;
		try
		{
			rzVaxhDvygHNuxtNbJgXqYRfTSVi2 = new rzVaxhDvygHNuxtNbJgXqYRfTSVi(P_0, P_0.useXInput, XDfJqiwLYIwEbgMhAkMbksxTquAQ, WHEWdNESHgukupIXVEuPORSWLLeJ, true, P_0.GetPlatformVar_useNativeMouse(), P_0.GetPlatformVar_useNativeKeyboard(), P_0.GetPlatformVar_useEnhancedDeviceSupport());
			GzEuxNQffpuifnZvWaVFtiPdKeuD.Add(5, rzVaxhDvygHNuxtNbJgXqYRfTSVi2);
			P_1.cVnDnWwFtLpDMrHuUpJUkHBVnKVf += rzVaxhDvygHNuxtNbJgXqYRfTSVi2.rsibSRRtOsuuKGDzaeOwWaQmfJdV;
			abLEytsrMfhnEafVEifaCIJcGbQLc = rzVaxhDvygHNuxtNbJgXqYRfTSVi2;
			return true;
		}
		catch (Exception)
		{
			Logger.LogWarning("Unable to initialize Raw Input! This error can be caused by running Unity sandboxed.");
			rzVaxhDvygHNuxtNbJgXqYRfTSVi2?.OnDestroy();
		}
		return false;
	}

	private bool wvPqOkGprotCSvQGGRMBzPrBQeAA(ConfigVars P_0, IMpasrWeCbdVlFXOeTtKckSykDrR P_1)
	{
		bool platformVar_useNativeMouse = P_0.GetPlatformVar_useNativeMouse();
		bool platformVar_useNativeKeyboard = P_0.GetPlatformVar_useNativeKeyboard();
		if (!platformVar_useNativeMouse && !platformVar_useNativeKeyboard)
		{
			return false;
		}
		rzVaxhDvygHNuxtNbJgXqYRfTSVi rzVaxhDvygHNuxtNbJgXqYRfTSVi2 = null;
		try
		{
			rzVaxhDvygHNuxtNbJgXqYRfTSVi2 = new rzVaxhDvygHNuxtNbJgXqYRfTSVi(P_0, false, null, null, false, platformVar_useNativeMouse, platformVar_useNativeKeyboard, P_0.GetPlatformVar_useEnhancedDeviceSupport());
			P_1.cVnDnWwFtLpDMrHuUpJUkHBVnKVf += rzVaxhDvygHNuxtNbJgXqYRfTSVi2.rsibSRRtOsuuKGDzaeOwWaQmfJdV;
			GzEuxNQffpuifnZvWaVFtiPdKeuD.Add(5, rzVaxhDvygHNuxtNbJgXqYRfTSVi2);
			return true;
		}
		catch
		{
			Logger.LogWarning("Unable to initialize Raw Input for native mouse handling! Unity mouse input will be used instead.");
			rzVaxhDvygHNuxtNbJgXqYRfTSVi2?.OnDestroy();
			rzVaxhDvygHNuxtNbJgXqYRfTSVi2 = null;
			return false;
		}
	}

	private bool jjjtZCKTiJWAWlvAlsjdAvKfEMJl(ConfigVars P_0, bool P_1)
	{
		UpdateLoopSetting updateLoop = P_0.updateLoop;
		bool useXInput = P_0.useXInput;
		bool flag = abLEytsrMfhnEafVEifaCIJcGbQLc == null;
		bool num = useXInput || flag || ReInput.currentPlatform == Platform.WindowsAppStore;
		bool flag2 = false;
		if (!num)
		{
			return false;
		}
		try
		{
			if (flag2)
			{
				qtWwqyCRnIKJoqKYBZXitRcIpTRb qtWwqyCRnIKJoqKYBZXitRcIpTRb2 = new qtWwqyCRnIKJoqKYBZXitRcIpTRb();
				qtWwqyCRnIKJoqKYBZXitRcIpTRb2.oPCGMBLFhOCIhehhynhMsDGZwaTwA = 0;
				QhRUnWbULvmdFTzNHeLFXfWeuhyi value = new QhRUnWbULvmdFTzNHeLFXfWeuhyi(flag2, updateLoop, XDfJqiwLYIwEbgMhAkMbksxTquAQ, qtWwqyCRnIKJoqKYBZXitRcIpTRb2.TbfRKCJPLZZVpOhlsmRQAJFnHIdD);
				GzEuxNQffpuifnZvWaVFtiPdKeuD.Add(2, value);
			}
			else
			{
				QhRUnWbULvmdFTzNHeLFXfWeuhyi qhRUnWbULvmdFTzNHeLFXfWeuhyi = new QhRUnWbULvmdFTzNHeLFXfWeuhyi(flag2, updateLoop, XDfJqiwLYIwEbgMhAkMbksxTquAQ, WHEWdNESHgukupIXVEuPORSWLLeJ);
				if (flag)
				{
					abLEytsrMfhnEafVEifaCIJcGbQLc = qhRUnWbULvmdFTzNHeLFXfWeuhyi;
				}
				GzEuxNQffpuifnZvWaVFtiPdKeuD.Add(2, qhRUnWbULvmdFTzNHeLFXfWeuhyi);
				if (P_1)
				{
					qhRUnWbULvmdFTzNHeLFXfWeuhyi.DeviceConnectedEvent += zLNKddgcwwRAduOsXseihEuykldS;
					qhRUnWbULvmdFTzNHeLFXfWeuhyi.DeviceDisconnectedEvent += UEYEgChtDymqRQHvBWqvZPDBCJcY;
					qhRUnWbULvmdFTzNHeLFXfWeuhyi.UpdateControllerInfoEvent += KPBhKolrWjrjwSJhosEHkXWwFWDn;
				}
			}
			return true;
		}
		catch (Exception)
		{
			if (flag)
			{
				OnDestroy();
				Logger.LogWarning("Unable to initialize XInput!");
				throw;
			}
			if (!flag2)
			{
				Logger.LogWarning("Unable to initialize XInput! XInput controllers will be handled by " + hGdcNaNFaFYbuzhgNvQHwwCpVkSN.ToString() + " instead. The L/R triggers are treated as a single axis and input cannot be detected when both are pressed simultaneously. Please see the Installation section of the documentation for information on required libraries. Documentation can be found in the menu: Window -> Rewired -> Help -> Documentation.");
				P_0.useXInput = false;
				for (int i = 0; i < GzEuxNQffpuifnZvWaVFtiPdKeuD.Count; i++)
				{
					if (GzEuxNQffpuifnZvWaVFtiPdKeuD[i] != null && GzEuxNQffpuifnZvWaVFtiPdKeuD[i] is SQlCUqkDhZyCKBGwHBBMetuYQQNaA sQlCUqkDhZyCKBGwHBBMetuYQQNaA)
					{
						sQlCUqkDhZyCKBGwHBBMetuYQQNaA.PXiJmHVeQttaMnqXULEWMzYhdnfc = false;
					}
				}
				Logger.LogWarning("Unable to initialize XInput! Please see the Installation section of the documentation for information on required libraries. Documentation can be found in the menu: Window -> Rewired -> Help -> Documentation.");
			}
			return false;
		}
	}

	[CustomObfuscation(rename = false)]
	public override void Initialize()
	{
		eKcWHeiuDjBZLxCtvCsgstBOqXxe = true;
		eWlfdEBFAfZbDnmNZkfHdmxfWDShA = new UfcGukYTvzxBtvGJSfvcMkQErsqo();
		for (int i = 0; i < GzEuxNQffpuifnZvWaVFtiPdKeuD.Count; i++)
		{
			GzEuxNQffpuifnZvWaVFtiPdKeuD[i].Initialize();
		}
	}

	public virtual void mPVLsAWcaTKfbIPORRXexWhffTDm(UpdateLoopType P_0)
	{
		for (int i = 0; i < GzEuxNQffpuifnZvWaVFtiPdKeuD.Count; i++)
		{
			GzEuxNQffpuifnZvWaVFtiPdKeuD[i].Update(P_0);
		}
	}

	[CustomObfuscation(rename = false)]
	public override void OnDestroy()
	{
		for (int num = GzEuxNQffpuifnZvWaVFtiPdKeuD.Count - 1; num >= 0; num--)
		{
			GzEuxNQffpuifnZvWaVFtiPdKeuD[num].OnDestroy();
		}
		if (mjcAnfSZCMXFItZOfBJKoKzyYMtj != null)
		{
			((IMpasrWeCbdVlFXOeTtKckSykDrR)mjcAnfSZCMXFItZOfBJKoKzyYMtj).KDbJcTzplmYIYaYTKqHYSVsrpBkC();
			mjcAnfSZCMXFItZOfBJKoKzyYMtj = null;
		}
		BDwAsVxXXjygYaSUuZfLCpBQFMEJ.lDxnsjCDTQrmresvWgbliNUVruIc();
	}

	[CustomObfuscation(rename = false)]
	public override Action<int, ControllerDataUpdater> GetInputDataUpdateDelegate()
	{
		return kFtEhCDMsBpCscbniaVUseitMyFJ;
	}

	[CustomObfuscation(rename = false)]
	public override void UpdateControllerData(int controllerId, ControllerDataUpdater data)
	{
		GzEuxNQffpuifnZvWaVFtiPdKeuD.GetValue((int)data.source).UpdateControllerData(eWlfdEBFAfZbDnmNZkfHdmxfWDShA.JKuaoczDNyjiUhPFYIKIsjLkxpur(controllerId, data.source, UfcGukYTvzxBtvGJSfvcMkQErsqo.VsOZJFSbGqhHqIvTQToBiabccXTs.Connected), data);
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
		for (int i = 0; i < GzEuxNQffpuifnZvWaVFtiPdKeuD.Count; i++)
		{
			IUnifiedMouseSource unifiedMouseSource = GzEuxNQffpuifnZvWaVFtiPdKeuD[i].GetUnifiedMouseSource();
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
		for (int i = 0; i < GzEuxNQffpuifnZvWaVFtiPdKeuD.Count; i++)
		{
			IUnifiedKeyboardSource unifiedKeyboardSource = GzEuxNQffpuifnZvWaVFtiPdKeuD[i].GetUnifiedKeyboardSource();
			if (unifiedKeyboardSource != null)
			{
				return unifiedKeyboardSource;
			}
		}
		return null;
	}

	private void zLNKddgcwwRAduOsXseihEuykldS(BridgedController P_0)
	{
		if (P_0 != null)
		{
			eWlfdEBFAfZbDnmNZkfHdmxfWDShA.rjuvkDZjDlwGtHMWPlbpHnyytTBA(P_0);
			if (_DeviceConnectedEvent != null)
			{
				_DeviceConnectedEvent(P_0);
			}
		}
	}

	private void UEYEgChtDymqRQHvBWqvZPDBCJcY(ControllerDisconnectedEventArgs P_0)
	{
		if (P_0 != null)
		{
			eWlfdEBFAfZbDnmNZkfHdmxfWDShA.SlsKNXnJEuvgRXQdMjsJWyVdKWsc(P_0);
			if (_DeviceDisconnectedEvent != null)
			{
				_DeviceDisconnectedEvent(P_0);
			}
		}
	}

	private void pqSAuCVZSOhHtVpLhThQGyDWlkFF(EventArgs P_0)
	{
		if (eKcWHeiuDjBZLxCtvCsgstBOqXxe)
		{
			for (int i = 0; i < GzEuxNQffpuifnZvWaVFtiPdKeuD.Count; i++)
			{
				GzEuxNQffpuifnZvWaVFtiPdKeuD[i].SystemDeviceConnected();
			}
		}
	}

	private void rsjVtmZDOZqmLjDrBUCiDDiakaEu(EventArgs P_0)
	{
		if (eKcWHeiuDjBZLxCtvCsgstBOqXxe)
		{
			for (int i = 0; i < GzEuxNQffpuifnZvWaVFtiPdKeuD.Count; i++)
			{
				GzEuxNQffpuifnZvWaVFtiPdKeuD[i].SystemDeviceDisconnected();
			}
		}
	}

	private void KPBhKolrWjrjwSJhosEHkXWwFWDn(UpdateControllerInfoEventArgs P_0)
	{
		if (P_0 == null || P_0.sourceJoystick == null)
		{
			return;
		}
		eWlfdEBFAfZbDnmNZkfHdmxfWDShA.itPrcTStUQeohuNHGbLIOuyYBDCc(P_0.sourceJoystick.rewiredId, P_0.sourceJoystick.inputManagerId);
		UfcGukYTvzxBtvGJSfvcMkQErsqo.VsOZJFSbGqhHqIvTQToBiabccXTs vsOZJFSbGqhHqIvTQToBiabccXTs = UfcGukYTvzxBtvGJSfvcMkQErsqo.VsOZJFSbGqhHqIvTQToBiabccXTs.Connected;
		int num = eWlfdEBFAfZbDnmNZkfHdmxfWDShA.uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0.sourceJoystick.rewiredId, vsOZJFSbGqhHqIvTQToBiabccXTs);
		if (num < 0)
		{
			vsOZJFSbGqhHqIvTQToBiabccXTs = UfcGukYTvzxBtvGJSfvcMkQErsqo.VsOZJFSbGqhHqIvTQToBiabccXTs.Disconnected;
			num = eWlfdEBFAfZbDnmNZkfHdmxfWDShA.uWrVYCSALeSsGsvUfnkTZtSRlidv(P_0.sourceJoystick.rewiredId, vsOZJFSbGqhHqIvTQToBiabccXTs);
		}
		if (num >= 0)
		{
			UfcGukYTvzxBtvGJSfvcMkQErsqo.itLJIiIxgabLxDPAzaZWoHcIInuBA itLJIiIxgabLxDPAzaZWoHcIInuBA = eWlfdEBFAfZbDnmNZkfHdmxfWDShA.aoSvfphhZiFPEDahxShDmlxhlhrAb(num, vsOZJFSbGqhHqIvTQToBiabccXTs);
			if (_UpdateControllerInfoEvent != null)
			{
				_UpdateControllerInfoEvent(new UpdateControllerInfoEventArgs(new SNoXgxUWvrpIAwUilSiZapIMVjjl(P_0.sourceJoystick, itLJIiIxgabLxDPAzaZWoHcIInuBA.intzOazUmOAoEfDNHbTtFSTaYFPbA)));
			}
		}
	}
}
